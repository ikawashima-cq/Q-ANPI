using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Security.Cryptography;

using ZXing;
using ZXing.QrCode.Internal;

namespace EncryptedQRCodeReader
{
    public partial class QR読み取り : Form
    {
        private WebCamera m_webCamera;
        private Timer m_webCameraTimer;
        private DB m_DB;
        private IniFile m_iniFile;

        // 32bit暗号の複合化用定数
        const String KEY = "zRcRMrWYdU2i3J4z";
        const String IV = "test_iva";

        // デコード時の文字コード
        static private int m_DecodeType_Unicode = 0;
        static private int m_DecodeType_ASCII = 1;
        static private int m_DecodeType_ShiftJIS = 2;

        // 前回認識結果
        private string m_OldString; // 今回認識結果
        private string m_OldStringForClear; // 前回認識結果
        private int m_currentCount;

        // メッセージボックス出力用文字列
        private string m_ErrorTitle = "エラー";
        private string m_ExceptionError = "異常が発生しました  ";

        // iniファイルを読み込む際の情報
        private string m_Sec_ErrorFileInfo = "ErrorFileInfo";
        private string m_Key_IgnoreFiles = "IgnoreFiles";
        private string m_Key_IgnoreExtensions = "IgnoreExtensions";

        private string m_Sec_WebCameraInfo = "WebCameraInfo";
        private string m_Key_CaptureInterval = "CaptureInterval";
        private string m_Key_ClearTime = "ClearTime";

        private string m_Sec_DBInfo = "DBInfo";
        private string m_Key_DBPass = "DBPass";

        private string m_Sec_DebugInfo = "DebugInfo";
        private string m_Key_DebugMode = "DebugMode";

        // カメラ未接続
        private string m_NotFound_Camera = "Webカメラが接続されているか確認してください。";

        // 読み取りダイアログ名
        private string m_Folder_DlgName = "フォルダ選択";

        // 読み取り結果メッセージボックス関連
        private string m_ReadFolder_MsgBoxName = "読み込み結果";
        //private string m_ReadFolder_MsgBoxLine1 = "登録済みのファイル、及び登録に失敗したファイルは、エラーファイルをご確認下さい。\n";
        private string m_ReadFolder_MsgBoxLine1 = "登録に失敗したファイルは、エラーファイルをご確認下さい。\n";
        private string m_ReadFolder_MsgBoxLine2 = "エラーファイル：";

        
        // 読み取り文言(読み取りボタン)
        private string m_Read_Button = "読み取り";
        private string m_Read_Start = "開始";
        private string m_Read_End = "終了";


        // 読み取り文言
        private string m_Read_OK = "[成功]";
        private string m_Read_NG = "[失敗]";
        private string m_Read_ALREADY = "[重複]";

        // 読み取りステータス文言
        private string m_Status_OK = "OK";
        private string m_Status_NG = "NG";
        
        // 以下iniファイルから読み込んだ情報
        private string[] m_IgnoreFiles;
        private string[] m_IgnoreExtensions;
        private int m_CaptureInterval;
        private int m_ClearTime;
        private int m_ClearCount;
        private int m_DebugMode;

        public QR読み取り()
        {
            InitializeComponent();
            m_OldString = "";
            m_OldStringForClear = "";
            m_DebugMode = 0;
        }

        private void EndProcess()
        {
            m_iniFile = null;
            Close();
        }

        private void ExceptionMsg(Exception exc)
        {
            ExceptionMsg(exc.Message);
        }

        private void ExceptionMsg(string errMsg)
        {
            if (m_DebugMode == 1)
            {
                // エラーメッセージボックスを表示中は、タイマーを一時停止する
                this.TimerStop();
                MessageBox.Show(m_ExceptionError + errMsg, m_ErrorTitle);
                this.TimerStart();
            }
        }

        private void TimerStart()
        {
            if (m_webCameraTimer != null)
            {
                m_webCameraTimer.Start();
            }
        }

        private void TimerStop()
        {
            if (m_webCameraTimer != null)
            {
                m_webCameraTimer.Stop();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_iniFile = new IniFile();
            bool bRet = m_iniFile.Initilize();
            if (bRet == false)
            {
                Close();
                return;
            }

            // 認識対象としないファイル情報の取得
            string sValue = m_iniFile.GetString(m_Sec_ErrorFileInfo, m_Key_IgnoreFiles);
            if (sValue.Length == 0)
            {
                m_iniFile.MsgBox(m_Sec_ErrorFileInfo, m_Key_IgnoreFiles);
                Close();
                return;
            }
            m_IgnoreFiles = sValue.Split(',');

            // 認識対象としないファイルの拡張子取得(取得できなくてもよい)
            sValue = m_iniFile.GetString(m_Sec_ErrorFileInfo, m_Key_IgnoreExtensions);
            if (sValue.Length != 0)
            {
                m_IgnoreExtensions = sValue.Split(',');
            }

            // キャプチャ間隔(msec)
            m_CaptureInterval = m_iniFile.GetInteger(m_Sec_WebCameraInfo, m_Key_CaptureInterval);
            if (m_CaptureInterval == 0)
            {
                m_iniFile.MsgBox(m_Sec_WebCameraInfo, m_Key_CaptureInterval);
                Close();
                return;
            }

            // 認識内容クリアまでの時間(sec)
            m_ClearTime = m_iniFile.GetInteger(m_Sec_WebCameraInfo, m_Key_ClearTime);
            if (m_ClearTime == 0)
            {
                m_iniFile.MsgBox(m_Sec_WebCameraInfo, m_Key_ClearTime);
                Close();
                return;
            }
            m_ClearCount = (m_ClearTime * 1000) / m_CaptureInterval;

            // デバッグ情報(取得できなくても問題ない)
            m_DebugMode = m_iniFile.GetInteger(m_Sec_DebugInfo, m_Key_DebugMode);

            // DBアクセス用パスワード
            string sPassWord = m_iniFile.GetString(m_Sec_DBInfo, m_Key_DBPass);
            if (sPassWord.Length == 0)
            {
                m_iniFile.MsgBox(m_Sec_DBInfo, m_Key_DBPass);
                Close();
                return;
            }

            // パスワードは暗号化されているので復号する
            string sDecode = this.CheckContents(sPassWord, m_DecodeType_ASCII);
            if (sDecode.Length == 0)
            {
                m_iniFile.MsgBox(m_Sec_DBInfo, m_Key_DBPass);
                Close();
                return;
            }

            m_DB = new DB();
            bRet = m_DB.Init(m_iniFile, sDecode);
            if (bRet == false)
            {
                Close();
                return;
            }

            // Webカメラクラスの初期化
            m_webCamera = new WebCamera { Container = pictureBox1 };
            bRet = m_webCamera.Init(m_iniFile);
            if (bRet == false)
            {
                Close();
                return;
            }
            m_webCamera.Dispose();
            m_webCamera = null;

            // WebCameraキャプチャ開始
            WebCamera.WebCameraStatus ret = WebCamera_OnOff();
            if (ret == WebCamera.WebCameraStatus.WC_INIERR)
            {
                m_iniFile.MsgBox(m_Sec_WebCameraInfo, "");
                Close();
                return;
            }
        }

        private WebCamera.WebCameraStatus WebCamera_OnOff()
        {
            if (m_webCamera == null)
            {
                m_webCamera = new WebCamera { Container = pictureBox1 };
                m_webCamera.Init(m_iniFile);
                WebCamera.WebCameraStatus ret = m_webCamera.OpenConnection();
                if (ret != WebCamera.WebCameraStatus.WC_OK)
                {
                    if (ret == WebCamera.WebCameraStatus.WC_NOTFOUND)
                    {
                        MessageBox.Show(m_NotFound_Camera, m_ErrorTitle);
                    }
                    m_webCamera.Dispose();
                    m_webCamera = null;
                    CaptureStartbutton.Text = m_Read_Button + m_Read_Start;
                    return ret;
                }

                m_webCameraTimer = new Timer();
                m_webCameraTimer.Tick += m_webCameraTimer_Tick;
                m_webCameraTimer.Interval = m_CaptureInterval;
                this.TimerStart();
                CaptureStartbutton.Text = m_Read_Button + m_Read_End;
            }
            else
            {
                this.TimerStop();
                m_webCameraTimer = null;
                m_webCamera.Dispose();
                m_webCamera = null;
                CaptureStartbutton.Text = m_Read_Button + m_Read_Start;
            }
            return WebCamera.WebCameraStatus.WC_OK;
        }

        private void qR読み取りアプリについてIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm hf = new HelpForm();

            // モーダルダイアログボックスとして表示
            hf.ShowDialog(this);

            // 後始末
            hf.Dispose();
            hf = null;

        }

        private void PlaySound(bool bOK_NG)
        {
            string sWavFile;
            if (bOK_NG == true)
            {
                sWavFile = "./OK.wav";
            }
            else
            {
                sWavFile = "./NG.wav";
            }

            System.Media.SoundPlayer player = null;
            player = new System.Media.SoundPlayer(sWavFile);
            try
            {
                player.Play();
            }
            catch (Exception exc)
            {
                ExceptionMsg(exc);
            }
            player = null;
        }

        static private string Decrypt(string text, int DecodeType)
        {
            const string AesKey = "zRcRMrWYdU2i3J4z";
            byte[] AesIV = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            // AES暗号化サービスプロバイダ
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 128;
            aes.IV = AesIV;
            aes.Key = Encoding.UTF8.GetBytes(AesKey);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = aes.CreateDecryptor();

            // Base64形式の文字列からバイト型配列に変換
            byte[] src = System.Convert.FromBase64String(text);
            byte[] planeText = new byte[src.Length];

            MemoryStream memoryStream = new MemoryStream(src);
            CryptoStream cryptStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            cryptStream.Read(planeText, 0, planeText.Length);

            string converted;
            if (DecodeType == EncryptedQRCodeReader.QR読み取り.m_DecodeType_Unicode)
            {
                converted = System.Text.Encoding.UTF8.GetString(planeText);
            }
            else
            {
                converted = System.Text.Encoding.ASCII.GetString(planeText);
            }
            int iRet = converted.IndexOf('\0');
            string trimed = converted;
            if (0 <= iRet)
            {
                trimed = converted.Substring(0, iRet);
            }

            planeText = null;
            memoryStream = null;
            cryptStream = null;
            aes = null;

            return (trimed);
        }

        /// <summary>
        /// 32bit暗号複合化処理
        /// </summary>
        /// <param name="inputStr">暗号化された文字列</param>
        /// <returns>複合化した文字列</returns>
        static private string decrypt_32bit(String inputStr, int DecodeType)
        {
            String outputStr = "";
            try
            {
                Encoding encording = Encoding.GetEncoding("Shift_JIS");
                if (DecodeType == EncryptedQRCodeReader.QR読み取り.m_DecodeType_Unicode)
                {
                    encording = Encoding.UTF8;
                }
                else if (DecodeType == EncryptedQRCodeReader.QR読み取り.m_DecodeType_ASCII)
                {
                    encording = Encoding.ASCII;
                }

                // 暗号化キー
                byte[] cipherKey = encording.GetBytes(KEY);
                // BlowFishインスタンス生成
                BlowFish bf = new BlowFish(cipherKey);
                bf.IV = encording.GetBytes(IV);
                byte[] bEnc = Convert.FromBase64String(inputStr);
                String strEnc = ByteToHex(bEnc);
                outputStr = bf.Decrypt_ECB_SJIS(strEnc);
            }
            catch (System.FormatException)
            {
                Console.WriteLine("文字列の変換に失敗しました。");
                return "";
            }
            catch (Exception)
            {
                Console.WriteLine("復号に失敗しました。");
                return "";
            }
            return outputStr;
        }
        static private String ByteToHex(byte[] bytes)
        {
            StringBuilder s = new StringBuilder();

            foreach (byte b in bytes)
            {
                s.Append(b.ToString("x2"));
            }

            return s.ToString();
        }

        void m_webCameraTimer_Tick(object sender, EventArgs e)
        {
            // 認識成功/失敗によらず常にクリア関数をよぶ
            this.Clear();

            // 何らかの文字列が認識できたか確認
            string sResult = this.GetBarcodeString();
            if (sResult.Length == 0)
            {
                // 何も認識しなかった
                return;
            }

            // ここから下の処理は何らかのバーコードを認識して
            // 読み取った際に動作する処理
            DB.DBAccessRet DBRet = DB.DBAccessRet.DB_NG;

            string encoded = sResult.Substring(0, 1);
            sResult = sResult.Substring(1, sResult.Length - 1);

            //string sEncode = this.CheckContents(sResult, m_DecodeType_Unicode);
            string sEncode = "";
            if (encoded == "1")
            {
                sEncode = this.CheckContents(sResult, m_DecodeType_ShiftJIS);
                if (sEncode.LastIndexOf(',') + 1 != sEncode.Length)
                {
                    // 複合化時に末端にゴミが入ることがあるので、最後の「,」から1文字のみ取得する
                    sEncode = sEncode.Substring(0, sEncode.LastIndexOf(',') + 2);
                }
            }
            else
            {
                sEncode = sResult;
            }

            if (sEncode.Length != 0)
            {
                DBRet = this.RegisterDB(sEncode);
            }

            if (DBRet == DB.DBAccessRet.DB_OK)
            {
                textBox1.Text = sEncode;

                // 成功音声を再生する
                this.PlaySound(true);

                m_OldString = textBox1.Text;
                label1.ForeColor = System.Drawing.Color.Green;
                label1.Text = m_Status_OK;
            }
            else
            {
                // 認識成功直後(m_CaptureInterval msec後)のDB重複は表示しない
                bool bRet = this.CheckErrorReason(DBRet, textBox1.Text, sEncode);
                if (bRet == true)
                {
                    return;
                }

                // 失敗時の再生音を鳴らすか確認
                if (m_OldString != sResult)
                {
                    // 失敗音声を再生し、認識結果を保存
                    this.PlaySound(false);
                    m_OldString = sResult;
                }
                label1.ForeColor = System.Drawing.Color.Red;
                label1.Text = m_Status_NG;
                if (DBRet == DB.DBAccessRet.DB_ALREADY)
                {
                    label1.Text = label1.Text + m_Read_ALREADY;
                }
                else 
                {
                    label1.Text = label1.Text + m_Read_NG;
                }
                Console.WriteLine("NGルート");
            }
        }

        // 現在表示時中の内容をクリアする
        private void Clear()
        {
            if (m_OldString.Length == 0)
            {
                return;
            }

            if (m_OldStringForClear != m_OldString)
            {
                // 別なバーコードを認識したのでカウンタークリア
                m_currentCount = 0;

                // 認識した文字列を設定しておく
                m_OldStringForClear = m_OldString;
                return;
            }
            m_currentCount++;

            if (m_ClearCount <= m_currentCount)
            {
                // 前回認識結果を一定時間画面に表示したのですべてクリアする
                m_OldString = "";
                m_OldStringForClear = "";
                m_currentCount = 0;
                textBox1.Text = ""; //読み取り結果領域
                label1.Text = ""; // 読み取り結果ステータス
            }
        }

        // 画像内のバーコード認識関数を呼び出し、認識結果を返却する
        private string GetBarcodeString()
        {
            string sResult = "";
            var bitmap = m_webCamera.GetCurrentImage();
            if (bitmap == null)
            {
                // 画像がキャプチャされていない
                return sResult;
            }

            var reader = new BarcodeReader();
            var result = reader.Decode(bitmap);
            reader = null;
            if (result == null)
            {
                return sResult;
            }
            sResult = result.Text;
            return sResult;
        }

        // エラー発生時の状況チェック。状況によりエラーが発生しても
        // 画面にエラー発生表示しないために使用する
        private bool CheckErrorReason(DB.DBAccessRet dbRet, string sTextBox, string sEncode)
        {
            if ((dbRet == DB.DBAccessRet.DB_ALREADY) &&
                (sTextBox == sEncode))
            {
                return true;
            }

            return false;
        }

        // アプリ終了ボタン押下時の処理
        private void button2_Click(object sender, EventArgs e)
        {
            this.EndProcess();
        }

        // 読み取り開始/終了ボタン押下時の処理
        private void CaptureStartbutton_Click(object sender, EventArgs e)
        {
            this.WebCamera_OnOff();
        }

        // ファイル読み込みメニュー選択時の処理
        private void ファイル読み込みRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string folderPath = this.GetFolderPath();
            if (folderPath.Length == 0)
            {
                // キャンセルされたので復帰
                return;
            }

            // 処理開始時の時刻を取得
            DateTime dtFileName = DateTime.Now;


            // 指定されたフォルダ内のファイルをピックアップ
            int iTotal = 0; // 対象総ファイル数
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(folderPath);
            System.IO.FileInfo[] files = di.GetFiles("*", System.IO.SearchOption.TopDirectoryOnly);
            List<KeyValuePair<string, DateTime>> DecodeErrFile = new List<KeyValuePair<string, DateTime>>();
            List<KeyValuePair<string, DateTime>> DBErrFile = new List<KeyValuePair<string, DateTime>>();
            List<KeyValuePair<string, DateTime>> SuccessFile = new List<KeyValuePair<string, DateTime>>();

            // 取得したファイルをファイル名でソート
            Array.Sort<FileInfo>(files, delegate(FileInfo a, FileInfo b)
            {
                // ファイルサイズで昇順ソート
                // return (int)(a.Length - b.Length);

                // ファイル名でソート
                return a.Name.CompareTo(b.Name);
            });

            foreach (System.IO.FileInfo f in files)
            {
                bool bRet = this.CheckFileName(f.Name);
                if (bRet == true)
                {
                    // 処理対象外のファイル
                    continue;
                }
                iTotal++;

                // ファイル内容を読み込む
                StreamReader sr = new StreamReader(folderPath + f.Name,System.Text.Encoding.GetEncoding("shift_jis"));
                string fileContents = sr.ReadToEnd();
                sr.Close();
                sr = null;

                // 現在時刻を取得
                DateTime dt = DateTime.Now;
                KeyValuePair<string, DateTime> tmpPair = new KeyValuePair<string, DateTime>(f.Name, dt);

                // 暗号化フラグを取得
                string encoded = fileContents.Substring(0, 1);
                fileContents = fileContents.Substring(1, fileContents.Length - 1);
                string sEncode = "";
                if (encoded == "1")
                {
                    // 暗号化フラグがON(=1)の場合は複合化
                    sEncode = this.CheckContents(fileContents, m_DecodeType_ShiftJIS);
                    if (sEncode.LastIndexOf(',') + 1 != sEncode.Length)
                    {
                        // 複合化時に末端にゴミが入ることがあるので、最後の「,」から1文字のみ取得する
                        sEncode = sEncode.Substring(0, sEncode.LastIndexOf(',') + 2);
                    }
                }
                else
                {
                    // 暗号化フラグがOFF(=0)の場合はそのまま読み取った文字列を渡す
                    sEncode = fileContents;
                }
                
                if (sEncode.Length == 0)
                {
                    // 復号失敗
                    DecodeErrFile.Add(tmpPair);
                    continue;
                }

                // DB登録
                DB.DBAccessRet DBret = this.RegisterDB(sEncode);
                if (DBret != DB.DBAccessRet.DB_OK)
                {
                    // 登録失敗
                    DecodeErrFile.Add(tmpPair);
                    continue;
                }

                // 登録成功したファイルをリストへ
                SuccessFile.Add(tmpPair);
            }

            // エラーファイル名作成
            string sFileName = folderPath + dtFileName.ToString("error_yyyyMMddHHmmss") + ".txt";

            // エラーファイル出力
            this.createErrorFileMain(sFileName, DecodeErrFile, DBErrFile);

            // 読み込み結果ダイアログ表示
            this.myMsgBox(sFileName, iTotal, DecodeErrFile.Count, DBErrFile.Count);

            // 成功したファイルは削除する
            this.deleteFiles(folderPath, SuccessFile);

            // 重複したファイルも削除する
            this.deleteFiles(folderPath, DBErrFile);

            return;
        }

        // フォルダーパス取得(キャンセル時は空文字を返す)
        private string GetFolderPath()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = false;
            fbd.Description = m_Folder_DlgName;

            string folderPath = "";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                // 選択されたフォルダパスを取得
                folderPath = fbd.SelectedPath + "\\";
            }
            fbd.Dispose();
            fbd = null;
            return folderPath;
        }

        // ファイル名チェック。処理対象外のファイル名ならtrue復帰
        private bool CheckFileName(string FileName)
        {
            // ファイル名の先頭が一致するか
            if (m_IgnoreFiles != null)
            {
                foreach (string IgnoreFile in m_IgnoreFiles)
                {
                    int nIdx = FileName.IndexOf(IgnoreFile);
                    if (nIdx == 0)
                    {
                        // 処理対象外のファイル
                        return true;
                    }
                }
            }

            // ファイルの拡張子が一致するか
            if (m_IgnoreExtensions != null)
            {
                string sExt = Path.GetExtension(FileName);
                foreach (string IgnoreExtension in m_IgnoreExtensions)
                {
                    if (IgnoreExtension == sExt)
                    {
                        // 処理対象外のファイル
                        return true;
                    }
                }
            }

            return false;
        }

        // ファイル内容の確認(復号可能かの確認のみ)
        // カンマ数のチェックは別クラスにて行う
        private string CheckContents(string Contents, int DecodeType)
        {
            string sEncode = "";
            try
            {
                //sEncode = QR読み取り.Decrypt(Contents, DecodeType);
                sEncode = QR読み取り.decrypt_32bit(Contents, DecodeType);
            }
            catch (Exception exc)
            {
                this.ExceptionMsg(exc);
            }
            return sEncode;
        }

        private DB.DBAccessRet RegisterDB(string sData)
        {
            // この関数から復帰するパターンは以下3種類
            // DBアクセス失敗、登録失敗
            // DB登録成功
            // DB重複
            DB.DBAccessRet aa = m_DB.DBRegister(sData);
            return aa;
        }

        // エラーファイル書き込みメイン
        private bool createErrorFileMain(string sFileName, List<KeyValuePair<string, DateTime>> DecodeErrFile, 
            List<KeyValuePair<string, DateTime>> DBErrFile)
        {
            // 読み込みエラーが存在した場合のみエラーファイルに書き込み
            // 読み込みエラーが存在しない場合はエラーファイルを作成しない
            // 復号失敗
            if (DecodeErrFile.Count > 0)
            {
                writeErrorFile(sFileName, m_Read_NG, DecodeErrFile);
            }
            // 重複
            if (DBErrFile.Count > 0)
            {
                writeErrorFile(sFileName, m_Read_ALREADY, DBErrFile);
            }
            return true;
        }

        // エラーファイル書き込み
        private bool writeErrorFile(string sFileName, string sSection, List<KeyValuePair<string, DateTime>> contents)
        {
            StreamWriter sw = new StreamWriter(sFileName, true, Encoding.UTF8);

            // セクションの書き込み
            sw.WriteLine(sSection);

            // 各ファイルの個別情報書き込み
            foreach (KeyValuePair<string, DateTime> oneLine in contents)
            {
                string sOneLine = oneLine.Key + "," + oneLine.Value.ToString("yyyy/MM/dd HH:mm:ss");
                sw.WriteLine(sOneLine);
            }
            sw.Close();

            return true;
        }

        // ファイル読み込み実行後のダイアログ表示
        private void myMsgBox(string sFileName, int iTotal, int iDecodeErr, int iDBErr)
        {
            int iSuccess = iTotal - iDecodeErr - iDBErr;
            string sTotal = "/" + iTotal.ToString() + " 件\n";

            string sText = "";
            sText += m_Read_OK + " : " + iSuccess.ToString() + sTotal;
            sText += m_Read_NG + " : " + iDecodeErr.ToString() + sTotal;
            //sText += m_Read_ALREADY + " : " + iDBErr.ToString()     + sTotal;
            // 読み込みエラーがあった場合のみ、エラーファイル名をメッセージに追加
            if ((iDecodeErr > 0) || (iDBErr > 0))
            {
                sText += "\n";
                sText += m_ReadFolder_MsgBoxLine1;
                sText += m_ReadFolder_MsgBoxLine2 + sFileName;
            }
            MessageBox.Show(sText, m_ReadFolder_MsgBoxName);
            return;
        }

        // 読み込みが終了したファイルの削除
        private void deleteFiles(string folderPath, List<KeyValuePair<string, DateTime>> contents)
        {
            foreach (KeyValuePair<string, DateTime> fileInfo in contents)
            {
                System.IO.File.Delete(folderPath + fileInfo.Key);
            }
        }

        // アプリ終了メニュー選択時の処理
        private void アプリ終了XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.EndProcess();
        }
    }
}
