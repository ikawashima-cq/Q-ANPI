using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.IO;

namespace ShelterInfoSystem
{
    // ログ スレッド
    public class LogThread : ThreadBase
    {
        // ログ キュー
        private Queue m_LogQue = new Queue();

        // ログファイル名
        private string s_sLogFilePath = null;

        // ディレクトリ名
        private string s_sLogDirPath = null;

        // ログファイルへのアクセス用排他ミューテックス
        private Mutex m_Mutex = new System.Threading.Mutex();

        private int m_timeCount = 0;
        private int m_DataLength = 0;

        // メインスレッド処理
        public override void Run()
        {
            // タイマ起動
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(OnTimer);
            timer.Interval = 1000;
            timer.Start();

            // 親クラスのメインスレッド処理
            base.Run();
        }

        // スレッド終了メソッド
        public override void Exit()
        {
            // ロック
            m_Mutex.WaitOne();
            Output();
            // アンロック
            m_Mutex.ReleaseMutex();
            // 親クラスのメインスレッド処理
            base.Exit();
        }

        // メッセージ処理
        public override void OnMessage(int message, int wParam, long lParam)
        {
        }

        // タイマー処理
        private void OnTimer(object sender, EventArgs e)
        {
            m_timeCount++;
//            if( ( m_timeCount >= 50 ) ||
            if( ( m_timeCount >= 5 ) ||
                (m_DataLength >= global::ShelterInfoSystem.Properties.Settings.Default.LogFileMaxSize))
            {
                Output();

                m_timeCount = 0;
            }
        }

        // ログ 設定:詳細無
        public void PutLog(int nLogLevel, string sBlockName, string sLog)
        {
            PutLog(nLogLevel, sBlockName, sLog, String.Empty, 0);
        }

        public void PutLog(int nLogLevel, string sBlockName, string sLog, int nType)
        {
            PutLog(nLogLevel, sBlockName, sLog, String.Empty, nType);
        }

        // ログ 設定:詳細有
        public void PutLog(int nLogLevel, string sBlockName, string sLog, string sLogDetail)
        {
            PutLog(nLogLevel, sBlockName, sLog, sLogDetail, 0);
        }
        public void PutLog(int nLogLevel, string sBlockName, string sLog, string sLogDetail, int nType)
        {
            if (nLogLevel <= global::ShelterInfoSystem.Properties.Settings.Default.LogLevel)
            {
                // ロック
                m_Mutex.WaitOne();

                DateTime time = DateTime.Now;

                ProtectField(ref sBlockName);
                ProtectField(ref sLog);
                ProtectField(ref sLogDetail);

                string sType1 = string.Empty;
                string sType2 = string.Empty;
                SetIncidenceAndRestitutionStr(nType, ref sType1, ref sType2);

//                string sTemp = String.Format("{0},{1},{2},{3},{4},{5},{6}", time.ToString(), sType1, sType2, nLogLevel, sBlockName, sLog, sLogDetail);
                string sTemp = String.Format("{0},{1},{2},{3},{4}", time.ToString() + "." + time.Millisecond.ToString("000"), nLogLevel, sBlockName, sLog, sLogDetail);
                // キューにログの内容を貯める
                m_LogQue.Enqueue(sTemp);

                // キュー内のデータサイズを計算する
                m_DataLength += sTemp.Length;

                // アンロック
                m_Mutex.ReleaseMutex();
            }
        }

        // エラーログ
        public void PutErrorLog(string sBlockName, string sLog, string sLogDetail)
        {
            PutErrorLog(sBlockName, sLog, sLogDetail, 0 );
        }
        public void PutErrorLog(string sBlockName, string sLog, string sLogDetail, int nType )
        {
            // ロック
            m_Mutex.WaitOne();

            DateTime time = DateTime.Now;

            ProtectField(ref sBlockName);
            ProtectField(ref sLog);
            ProtectField(ref sLogDetail);

            string sType1 = string.Empty;
            string sType2 = string.Empty;
            SetIncidenceAndRestitutionStr(nType, ref sType1, ref sType2);

//            string sTemp = String.Format("{0},{1},{2},-1,{3},{4},{5}", time.ToString(), sType1, sType2, sBlockName, sLog, sLogDetail);
            string sTemp = String.Format("{0},ERR,{1},{2},{3}", time.ToString(), sBlockName, sLog, sLogDetail);
            // キューにログの内容を貯める
            m_LogQue.Enqueue(sTemp);

            // キュー内のデータサイズを計算する
            m_DataLength += sTemp.Length;

            // アンロック
            m_Mutex.ReleaseMutex();
        }

        // エラーログ
        // ※ロックなしの内部用：内部でロック後にコールすること
        private void PutErrorLogWithOutLock(string sBlockName, string sLog, string sLogDetai)
        {
            PutErrorLogWithOutLock(sBlockName, sLog, sLogDetai, 0);
        }

        private void PutErrorLogWithOutLock(string sBlockName, string sLog, string sLogDetail, int nType )
        {
            DateTime time = DateTime.Now;

            ProtectField(ref sBlockName);
            ProtectField(ref sLog);
            ProtectField(ref sLogDetail);

            string sType1 = string.Empty;
            string sType2 = string.Empty;
            SetIncidenceAndRestitutionStr(nType, ref sType1, ref sType2);

//            string sTemp = String.Format("{0},{1},{2},-1,{3},{4},{5}", time.ToString(), sType1, sType2, sBlockName, sLog, sLogDetail);
            string sTemp = String.Format("{0},ERR,{1},{2},{3}", time.ToString(), sBlockName, sLog, sLogDetail);
            // キューにログの内容を貯める
            m_LogQue.Enqueue(sTemp);

            // キュー内のデータサイズを計算する
            m_DataLength += sTemp.Length;
        }

        // ログスレッド 実行
        private void Output()
        {
            // ログ出力処理を以下に記述

            // ロック
            m_Mutex.WaitOne();

            // ディレクトリを生成
            // （既に実行日付のディレクトリが存在していれば生成しない）
//            s_sLogDirPath = CreateLogFilePath(global::ShelterInfoSystem.Properties.Settings.Default.LogFolder, ref s_sLogFilePath);
            // 2016/04/21
            // ログ出力パス　
            s_sLogDirPath = CreateLogFilePath(Program.m_sCurrentLogPath, ref s_sLogFilePath);

            // ストリームライタを生成
            StreamWriter writeer = null;
            try
            {
                // ストリームライタとの関連付け
                writeer = File.AppendText(s_sLogFilePath);

                // ログが登録されている数を取得する
                int nQueSize = m_LogQue.Count;

                // キューにたまっている分処理を行う。
                for (int nIdx = 0; nIdx < nQueSize; nIdx++)
                {
                    // キューからログの内容を取り出す
                    string sLog = (string)m_LogQue.Dequeue();

                    // ストリームに対してログの内容を書き込む
                    writeer.WriteLine(sLog);
                }
                writeer.Flush();

                writeer.Close();

                writeer.Dispose();

                // 2016/04/22
                m_DataLength = 0;
            }
            catch (Exception ex)
            {
                if (writeer != null)
                {
                    writeer.Close();

                    writeer.Dispose();
                }

                PutErrorLogWithOutLock("LogThread", "Faild : Output LogFile.", ex.ToString());
            }

            // アンロック
            m_Mutex.ReleaseMutex();
        }

        // ログディレクトリ作成
        private string CreateLogFilePath(string sLogRootPath, ref string sLogFilePath)
        {
            //string sLogDirName = DateTime.Now.ToString("yyyyMMdd");
            //string sLogDirFullPath = sLogRootPath + "\\" + sLogDirName;
            string sLogDirFullPath = sLogRootPath;
            sLogFilePath = sLogRootPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";

            DirectoryInfo dirInfo = new DirectoryInfo(sLogDirFullPath);

            string sLogDirPath = string.Empty;
            try
            {
                // ディレクトリが存在しない
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                // ファイル情報を取得
                FileInfo logFileInfo = new FileInfo(sLogFilePath);

                // ファイル存在しない場合
                if (!logFileInfo.Exists)
                {
                    // ファイルが作成された＝日付が変わったので一定期間以上前のファイルを削除する
                    DeleteLogFile(sLogRootPath);
                }
            }
            catch (Exception ex)
            {
                PutErrorLogWithOutLock("LogThread", "Faild : Create Dir.", ex.ToString());
            }

            return sLogDirPath;
        }

        // ログファイル削除
        private void DeleteLogFile(string sLogRootPath)
        {
            // ログディレクトリの情報を取得
            DirectoryInfo dirInfo = new DirectoryInfo(sLogRootPath);

            // ログディレクトリ下の情報を取得
            FileInfo[] fileList = dirInfo.GetFiles();

            // 全ファイルを確認
            for (int nIdx = 0; nIdx < fileList.Length; nIdx++)
            {
                // ディレクトリ名を取得
                string sFileName = fileList[nIdx].Name;
                DateTime fileDate = DateTime.Now;

                // ディレクトリ名を日付に変換
                try
                {
                    string sDate = sFileName.Substring(0, 8);

                    fileDate = DateTime.ParseExact(sDate,
                                            "yyyyMMdd",
                                            System.Globalization.DateTimeFormatInfo.InvariantInfo,
                                            System.Globalization.DateTimeStyles.None);

                }
                catch (Exception ex)
                {
                    // 変換失敗した場合、ログに残す
                    string sLog = string.Format("不明なファイル({0})があります。", sFileName);
                    PutErrorLogWithOutLock("LogThread", sLog, ex.ToString());
                    // 次のディレクトリを確認
                    continue;
                }

                // ディレクトリと現在時刻との日付の差分を計算
                TimeSpan timeSpan = DateTime.Now - fileDate;

                // 設定以上の日数が空いていれば、ディレクトリ内のファイルを含む全削除
                if ((int)timeSpan.TotalDays > global::ShelterInfoSystem.Properties.Settings.Default.LogDeleteDays)
                {
                    fileList[nIdx].Delete();
                }
            }
        }

        // CSVとして読み込めるように、
        // ""で囲む必要がある場合、""による保護を行う
        private void ProtectField(ref string sField){
            //"で囲む必要があるか調べる
            if (sField.IndexOf('"') > -1 ||
                sField.IndexOf(',') > -1 ||
                sField.IndexOf('\r') > -1 ||
                sField.IndexOf('\n') > -1 ||
                sField.StartsWith(" ") || sField.StartsWith("\t") ||
                sField.EndsWith(" ") || sField.EndsWith("\t"))
            {
                if (sField.IndexOf('"') > -1)
                {
                    //"を""とする
                    sField = sField.Replace("\"", "\"\"");
                }
                sField = "\"" + sField + "\"";
            }
        }
        // 発生/復帰の文字列設定
        private void SetIncidenceAndRestitutionStr(int nType, ref string sIncidence, ref string sRestitution)
        {
            if (nType == 0)
            {
                sIncidence = "発生";
            }
            else if (nType == 1)
            {
                sRestitution = "復帰";
            }
        }

        // デバグ用
        public string dumpDBG(string sinfo, ref byte[] binaryData, uint offset, int readLength)
        {
            string retdat = "";
            
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(sinfo + Environment.NewLine);
            uint start = offset - (offset % 0x10);
            sb.Append("         : ");
            if (binaryData.Length > 0)
            {
                for (int i=0;i<=0x0f;i++)
                {
                    sb.Append(i.ToString("X2"));

                    if (i == 0x0f)
                    {
                        sb.Append(System.Environment.NewLine);
                    }
                    else
                    {
                        sb.Append(" ");
                    }
                }

                sb.Append("---------:---------------" +
                    "---------------------------------" + Environment.NewLine);

                for (uint k = start; k < binaryData.Length; k++)
                {
                    if (k > readLength + offset)
                    {
                        sb.Append(System.Environment.NewLine);
                        break;
                    }

                    if (k % 0x10 == 0)
                    {
                        sb.Append(k.ToString("X8") + " : ");
                    }

                    if (k < offset)
                    {
                        sb.Append("  ");
                    }
                    else
                    {
                        sb.Append(binaryData[k].ToString("X2"));
                    }
                    if ((k + 0x01) % 0x10 == 0)
                    {
                        sb.Append(System.Environment.NewLine);
                    }
                    else
                    {
                        sb.Append(" ");
                    }
                }
                retdat = sb.ToString();
            }
            else
            {
                retdat = "No Data.";
            }

            m_Mutex.WaitOne();
            s_sLogDirPath = CreateLogFilePath(Program.m_sCurrentLogPath, ref s_sLogFilePath);
            StreamWriter writeer = null;
            try
            {
                writeer = File.AppendText(s_sLogFilePath + ".dmp");
                writeer.WriteLine(retdat);
                writeer.Flush();
                writeer.Close();
                writeer.Dispose();
            }
            catch (Exception)
            {
                if (writeer != null)
                {
                    writeer.Close();
                    writeer.Dispose();
                }
            }
            m_Mutex.ReleaseMutex();

            return retdat;
        }
    }
}
