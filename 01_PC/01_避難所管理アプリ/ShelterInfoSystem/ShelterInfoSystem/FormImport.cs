using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace ShelterInfoSystem
{
    public partial class FormImport : Form
    {
        private FormWait m_frmWait = new FormWait();
        private List<DbAccess.PersonInfo> m_personList = new List<DbAccess.PersonInfo>();
        private int m_importCnt = 0;
        private List<string> m_importErrStr = new List<string>();

        private const int textTel_MaxLength = 12;
        private const int textAge_MaxLength = 3;

        private FormShelterInfo m_mainDialog;
        private bool m_NotInstalledAccess = false;

        public FormImport(FormShelterInfo mainDialog)
        {
            InitializeComponent();
            this.importFilename.AllowDrop = true;
            this.importFilename.DragEnter += new DragEventHandler(FormImport_DragEnter);
            this.importFilename.DragDrop += new DragEventHandler(FormImport_DragDrop);

            m_frmWait.Text = "ファイル読み込み中";
            fileImportWorker.DoWork += new DoWorkEventHandler(fileImportWorker_DoWork);
            fileImportWorker.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(fileImportWorker_RunWorkerCompleted);
            importWorker.DoWork += new DoWorkEventHandler(importWorker_DoWork);
            importWorker.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(fileImportWorker_RunWorkerCompleted);

            m_mainDialog = mainDialog;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            // バックグラウンドでファイル読込み
            fileImportWorker.RunWorkerAsync();
            // ファイル読込み中ダイアログ表示
            m_frmWait.ShowDialog(this);
            if (m_personList.Count == 0)
            {
                // 読込み件数が0件
                if (!m_NotInstalledAccess)
                {
                    MessageBox.Show("インポートできるデータが1件もありません", "避難所情報システム",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.No;
                }
            }
            else
            {
                m_frmWait = new FormWait();
                m_frmWait.Text = "インポート中";
                importWorker.RunWorkerAsync();
                m_frmWait.ShowDialog(this);
                if (m_importCnt > 0 || m_importErrStr.Count > 0)
                {
                    string message = string.Format("インポート結果\r\n　成功　　: {0}\r\n　スキップ : {1}",
                        m_importCnt, m_importErrStr.Count);
                    if (m_importErrStr.Count > 0)
                    {
                        string errFile = createErrorFile();

                        message += string.Format("\r\n\r\nスキップしたデータは {0} を参照してください", errFile);
                    }
                    MessageBox.Show(message, "避難所情報システム",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("インポートできるデータが1件もありません", "避難所情報システム",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.No;
                }
            }
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void selectFile_Click(object sender, EventArgs e)
        {
            // 現在選択されているファイルが格納されているディレクトリが存在するかチェック
            // 存在すれば該当ディレクトリを開く
            try
            {
                string directory = Path.GetDirectoryName(importFilename.Text);
                if (Directory.Exists(directory))
                {
                    openImportFileDialog.InitialDirectory = directory;
                }
            }
            catch (ArgumentException)
            {
                // DO NOTHING
                openImportFileDialog.InitialDirectory = m_mainDialog.m_ImportDir;
            }

            if (openImportFileDialog.ShowDialog() == DialogResult.OK)
            {
                importFilename.Text = openImportFileDialog.FileName;
                m_mainDialog.m_ImportDir = System.IO.Path.GetDirectoryName(openImportFileDialog.FileName);
            }
        }

        private void FormImport_DragEnter(object sender, DragEventArgs e)
        {
            string[] fileName = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (!e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop))
            {
                /* ファイル以外のドラッグは受け入れない */
                e.Effect = DragDropEffects.None;
                return;
            }

            if (fileName.Length > 1)
            {
                /* 複数ファイルのドラッグは受け入れない */
                e.Effect = DragDropEffects.None;
                return;
            }

            string extension = Path.GetExtension(fileName[0]);
            if (extension != ".xlsx" && extension != ".xls" && extension != ".csv")
            {
                /* 拡張子が.xlsx, .xls .csv以外は受け入れない */
                e.Effect = DragDropEffects.None;
                return;
            }

            /* 上記以外は受け入れる */
            e.Effect = DragDropEffects.All;
        }

        private void FormImport_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileName = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (fileName.Length <= 0)
            {
                return;
            }
            importFilename.Text = fileName[0];
        }

        private void importFilename_TextChanged(object sender, EventArgs e)
        {
            // ファイル存在確認
            if (File.Exists(importFilename.Text))
            {
                string extension = Path.GetExtension(importFilename.Text);
                if (extension != ".xlsx" && extension != ".xls" && extension != ".csv")
                {
                    btnImport.Enabled = false;
                }
                else
                {
                    btnImport.Enabled = true;
                }

            }
            else
            {
                btnImport.Enabled = false;
            }
        }

        private void fileImportWorker_DoWork( object sender, DoWorkEventArgs e)
        {
            loadExcel();
        }

        private void fileImportWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            m_frmWait.Dispose();
        }

        private void importWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Program.m_objDbAccess.BeginTran();
            foreach (DbAccess.PersonInfo info in m_personList)
            {
                if (!string.IsNullOrEmpty(info.error_reason))
                {
                    m_importErrStr.Add(info.error_reason);
                    continue;
                }

                try
                {
                    Program.m_objDbAccess.UpsertPersonInfo(info);
                    m_importCnt++;
                }
                catch (Exception)
                {
                    string message = string.Format("{0}行目 : データベース登録エラー", info.index);
                    m_importErrStr.Add(message);
                }
                DbAccess.PersonLog log = new DbAccess.PersonLog();
                log.init();
                log.Set(info);
                Program.m_objDbAccess.InsertPersonLog(log);
            }
            Program.m_objDbAccess.CommitTran();
            System.Threading.Thread.Sleep(2000);
        }

        private void loadExcel()
        {
            // エクセルファイルの読み込み
            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
            {
                // ファイル拡張子確認
                string extension = Path.GetExtension(importFilename.Text);
                string commandText;
                DbConnectionStringBuilder builder;
                builder = factory.CreateConnectionStringBuilder();
                if (extension == ".xlsx")
                {
                    builder["Data Source"] = @importFilename.Text;
                    builder["Extended Properties"] = "Excel 12.0;HDR=YES;IMEX=1";
                    builder["Provider"] = "Microsoft.ACE.OLEDB.12.0";
                }
                else if (extension == ".xls")
                {
                    builder["Data Source"] = @importFilename.Text;
                    builder["Extended Properties"] = "Excel 8.0;HDR=YES;IMEX=1";
                    builder["Provider"] = "Microsoft.Jet.OLEDB.4.0";
                }
                else
                {
                    // CSV
                    builder["Data Source"] = @Path.GetDirectoryName(importFilename.Text);
                    builder["Extended Properties"] = "text;HDR=YES;FMT=Delimited;IMEX=1";
                    builder["Provider"] = "Microsoft.Jet.OLEDB.4.0";
                }

                try
                {
                    // Providerは読み込む拡張子によって使い分ける
                    // (*.xlsx→Microsoft.ACE.OLEDB.12.0　、その他→Microsoft.Jet.OLEDB.4.0)
                    // Microsoft.ACE.OLEDB.12.0はAccessRuntime(32bit版)が端末にインストールされていない場合エラーとなるので
                    // その際はAccessRuntime(32bit版)をインストールするように促すメッセージを表示する
                    //builder["Provider"] = "Microsoft.ACE.OLEDB.12.0";
                    //builder["Provider"] = "Microsoft.Jet.OLEDB.4.0";
                    OleDbConnection conn = new OleDbConnection();
                    conn.ConnectionString = builder.ToString();
                    conn.Open();

                    // エクセル形式の場合、先頭のシートの情報を取得する
                    if ((extension == ".xlsx")||(extension == ".xls"))
                    {
                        var dtTables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        String[] excelSheets = new String[dtTables.Rows.Count];
                        int i = 0;
                        foreach (DataRow row in dtTables.Rows)
                        {
                            excelSheets[i] = row["TABLE_NAME"].ToString();
                            i++;
                        }
                        commandText = excelSheets[0];
                    }
                    else
                    {
                        commandText = Path.GetFileName(importFilename.Text);
                        // CSVヘッダ行のチェック
                        if (checkCSVFile(builder["Data Source"].ToString(), commandText) == false)
                        {
                            Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormImport", "loadExcel", "CSVヘッダ行不正");
                            return;
                        }

                        // schema.ini作成
                        makeSchemaFile(builder["Data Source"].ToString(), commandText);
                    }

                    using (OleDbCommand command = conn.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM [" + commandText + "]";
                        DataTable table = new DataTable();
                        DbDataReader reader = command.ExecuteReader();
                        table.Load(reader);
                        foreach (DataRow row in table.Rows)
                        {
                            DbAccess.PersonInfo info = new DbAccess.PersonInfo();
                            info.init();
                            info.sid = Program.m_objActiveTermial.sid;
                            info.index = (table.Rows.IndexOf(row) + 2).ToString();
                            //                        info.id = narrowNum(row["電話番号"].ToString());
                            info.id = row["電話番号"].ToString();
                            if (!IsDigitString(info.id))
                            {
                                // 電話番号不正のためスキップ
                                info.error_reason = string.Format("{0}行目 : 電話番号不正 : {1}",
                                    info.index, info.id);
                                m_personList.Add(info);
                                continue;
                            }
                            if (info.id.Length > textTel_MaxLength)
                            {
                                // 電話番号不正（最大桁数超過）のためスキップ
                                info.error_reason = string.Format("{0}行目 : 電話番号不正 : {1}",
                                    info.index, info.id);
                                m_personList.Add(info);
                                continue;
                            }
                            // 半角数字に置換
                            info.id = narrowNum(info.id);

                            info.name = row["氏名"].ToString();
                            if (string.IsNullOrEmpty(info.name))
                            {
                                // 氏名不正のためスキップ
                                info.error_reason = string.Format("{0}行目 : 氏名不正 : {1}",
                                    info.index, info.name);
                                m_personList.Add(info);
                                continue;
                            }

                            // エクセル形式からインポートした際に[yyyy/mm/dd hh:mm:ss]の形式になってしまうので、これに対応 
                            string tmpBirthDay = row["生年月日"].ToString();
                            string[] strBirth = tmpBirthDay.Split('-');
                            if (strBirth.Length != 3)
                            {
                                // エクセル形式からインポートした際に[yyyy/mm/dd hh:mm:ss]の形式になってしまうので、これに対応
                                string[] tmpSplit = tmpBirthDay.Split(' ');
                                strBirth = tmpSplit[0].Split('/');
                                if (strBirth.Length == 3)
                                {
                                    info.txt02 = strBirth[0] + "-" + strBirth[1] + "-" + strBirth[2];
                                }
                            }
                            else
                            {
                                info.txt02 = tmpBirthDay;
                            }

                            // 生年月日フォーマットチェック
                            DateTime dt = DateTime.Now;
                            try
                            {
                                if (((dt.Year - (int.Parse(strBirth[0]))) < 0) || ((dt.Year - (int.Parse(strBirth[0]))) > 120))
                                {
                                    // 生年月日不正（最大桁数超過）のためスキップ
                                    info.error_reason = string.Format("{0}行目 : 生年月日不正 : {1}", info.index, tmpBirthDay);
                                    m_personList.Add(info);
                                    continue;
                                }
                                // 日付として成立しているか
                                DateTime.Parse(strBirth[0] + "/" + strBirth[1] + "/" + strBirth[2]);
                            }
                            catch (Exception)
                            {
                                // 生年月日不正（日付として成立していない）のためスキップ
                                info.error_reason = string.Format("{0}行目 : 生年月日不正 : {1}", info.index, tmpBirthDay);
                                m_personList.Add(info);
                                continue;
                            }

                            // 性別チェック
                            string gender = row["性別"].ToString();
                            if ("男".Equals(gender))
                            {
                                info.sel01 = "0";
                            }
                            else if ("女".Equals(gender))
                            {
                                info.sel01 = "1";
                            }
                            else
                            {
                                // 必須情報なしのためスキップ
                                info.error_reason = string.Format("{0}行目 : 性別不正 : {1}",
                                    info.index, gender);
                                m_personList.Add(info);
                                continue;
                            }

                            // 旧フォーマットも対応
                            try
                            {
                                // 入所/退所
                                string entryState = row["入所/退所"].ToString();
                                if ("入所".Equals(entryState))
                                {
                                    info.sel02 = "0";
                                }
                                else if ("退所".Equals(entryState))
                                {
                                    info.sel02 = "1";
                                }
                                else if ("在宅".Equals(entryState))
                                {
                                    info.sel02 = "2";
                                }
                                else
                                {
                                    // 必須情報なしのためスキップ
                                    info.error_reason = string.Format("{0}行目 : 入/退不正 : {1}",
                                        info.index, entryState);
                                    m_personList.Add(info);
                                    continue;
                                }
                            }
                            catch (Exception)
                            {
                                // 旧フォーマットの場合、必ず「退所」で登録される
                                info.sel02 = "1";
                            }

                            // 公表
                            string publication = row["公表"].ToString();
                            if ("許可".Equals(publication))
                            {
                                info.sel03 = "1";
                            }
                            else if ("拒否".Equals(publication))
                            {
                                info.sel03 = "0";
                            }
                            else
                            {
                                // 必須情報無しのためエラー
                                info.error_reason = string.Format("{0}行目 : 公表不正 : {1}",
                                    info.index, publication);
                                m_personList.Add(info);
                                continue;
                            }

                            // 住所
                            info.txt01 = row["住所"].ToString();

                            string injury = row["ケガ"].ToString();
                            if ("無し".Equals(injury))
                            {
                                info.sel04 = "0";
                            }
                            else if ("有り".Equals(injury))
                            {
                                info.sel04 = "1";
                            }
                            else
                            {
                                info.sel04 = "3";
                            }

                            string care = row["介護"].ToString();
                            if ("不要".Equals(care))
                            {
                                info.sel05 = "0";
                            }
                            else if ("必要".Equals(care))
                            {
                                info.sel05 = "1";
                            }
                            else
                            {
                                info.sel05 = "2";
                            }

                            string handicap = row["障がい"].ToString();
                            if ("無し".Equals(handicap))
                            {
                                info.sel06 = "0";
                            }
                            else if ("有り".Equals(handicap))
                            {
                                info.sel06 = "1";
                            }
                            else
                            {
                                info.sel06 = "2";
                            }

                            string parturient = row["妊産婦"].ToString();
                            if ("いいえ".Equals(parturient))
                            {
                                info.sel07 = "0";
                            }
                            else if ("はい".Equals(parturient))
                            {
                                info.sel07 = "1";
                            }
                            else
                            {
                                info.sel07 = "2";
                            }

                            // 避難所内外
                            string inout = row["避難所内外"].ToString();
                            if ("内".Equals(inout))
                            {
                                info.sel08 = "0";
                            }
                            else if ("外".Equals(inout))
                            {
                                info.sel08 = "1";
                            }
                            else
                            {
                                // 必須情報なしのためスキップ
                                info.error_reason = string.Format("{0}行目 : 避難所内外不正 : {1}",
                                    info.index, inout);
                                m_personList.Add(info);
                                continue;
                            }

                            info.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                            Console.WriteLine("upsert {0},{1}", info.id, info.name);
                            m_personList.Add(info);
                        }

                        // 表更新
                        m_mainDialog.UpdateListData(true);
                        this.DialogResult = DialogResult.OK;
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormImport", "loadExcel", ex.ToString());

                    if (ex.Message == "'Microsoft.ACE.OLEDB.12.0' プロバイダーはローカルのコンピューターに登録されていません。")
                    {
                        MessageBox.Show("インポートに失敗しました。" + System.Environment.NewLine +
                                        "MicrosoftAccessRuntime(32bit版)をお使いの端末に" + System.Environment.NewLine +
                                        "インストールしてください。", "避難所情報システム",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        m_NotInstalledAccess = true;
                    }
                }
            }
        }

        // 全角数字を半角数字に変換する
        private string narrowNum(string str)
        {
            Regex reg = new Regex("[０-９]+");
            string result = reg.Replace(str, replaceNarrow);

            return result;
        }

        private string replaceNarrow(Match match)
        {
            return Strings.StrConv(match.Value, VbStrConv.Narrow, 0);
        }

        /// <summary>
        /// 10進数文字列判別処理.
        /// 文字列が全て数値かどうかチェックする
        /// </summary>
        /// <param name="s">チェック文字列</param>
        /// <returns>true:全て10進数文字 false:10進数以外の文字を含む</returns>
        private bool IsDigitString(string s)
        {
            // 指定した文字列が10進数の数字かどうかを判定します。
            if (string.IsNullOrEmpty(s)) 
            {
                return false; 
            }

            foreach (char c in s)
            {
                if (!Char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private string createErrorFile()
        {
            String errorFile = Path.GetDirectoryName(importFilename.Text) + "\\"
                + Path.GetFileNameWithoutExtension(importFilename.Text)
//                + "_error_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                + "_result_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            StreamWriter writer = null;
            try
            {

                writer = new StreamWriter(errorFile);
                foreach (string err in m_importErrStr)
                {
                    writer.WriteLine(err);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (writer != null)
                {
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                }
            }
            return errorFile;
        }

        private void makeSchemaFile(string fpath, string csvname)
        {
            StreamWriter sw = new StreamWriter(fpath + "\\schema.ini", false, Encoding.GetEncoding("shift_jis"));
            sw.WriteLine("[" + csvname + "]");
            sw.WriteLine("ColNameHeader=True");
            sw.WriteLine("Format=CSVDelimited");
            sw.WriteLine("Col1=電話番号 Char");
            sw.WriteLine("Col2=氏名 Char");
            sw.WriteLine("Col3=生年月日 Char");
            sw.WriteLine("Col4=性別 Char");
            sw.WriteLine("Col5=入所/退所 Char");
            sw.WriteLine("Col6=公表 Char");
            sw.WriteLine("Col7=住所 Char");
            sw.WriteLine("Col8=ケガ Char");
            sw.WriteLine("Col9=介護 Char");
            sw.WriteLine("Col10=障がい Char");
            sw.WriteLine("Col11=妊産婦 Char");
            sw.WriteLine("Col12=避難所内外 Char");
            sw.Close();
        }

        private Boolean checkCSVFile(string fpath, string csvname)
        {
            Boolean bResult = false;
            try
            {
                StreamReader sr = new StreamReader(fpath + "\\" + csvname, Encoding.GetEncoding("shift_jis"));
                string stBuffer = sr.ReadLine();
                int iRes = stBuffer.CompareTo("電話番号,氏名,生年月日,性別,入所/退所,公表,住所,ケガ,介護,障がい,妊産婦,避難所内外");
                if (iRes == 0)
                {
                    bResult = true;
                }
                sr.Close();
            }
            catch (Exception)
            {
            }
            return bResult;
        }
    }
}
