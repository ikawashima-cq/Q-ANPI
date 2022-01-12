using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace ShelterInfoSystem
{
    public class DbAccess : IDisposable
    {
        // ブロック名定義
        const string m_strBlockName = "DbAccess";
        const string m_dbPass = "shelter2018";
        // step2 DbAccessStep2を使用するためpublicに変更
        // SQL接続
        //public MySqlConnection m_connection = null;
        public OleDbConnection m_connection = null;
        // SQLトランザクション
        //public MySqlTransaction m_transaction = null;
        public OleDbTransaction m_transaction = null;

        // terminal_infoテーブル
        public struct TerminalInfo
        {
            public string sid;
            public string gid;
            public string smid;
            public string name;
            public string lat;
            public string lon;
            public FormShelterInfo.SHELTER_STATUS open_flag;
            public string open_datetime;
            public string close_datetime;
            public string update_datetime;
            public string memo;
            public string status;
            public string text_flag;
            public bool select_flag;
            public string dummy_num;
            public bool dummy_num_flag;

            public void init()
            {
                sid = "";
                gid = "";
                smid = "";
                name = "";
                lat = "";
                lon = "";
                open_flag = FormShelterInfo.SHELTER_STATUS.NO_OPEN;
                open_datetime = "";
                close_datetime = "";
                update_datetime = "";
                memo = "";
                status = "";
                text_flag = "0";
                select_flag = false;
                dummy_num = "";
                dummy_num_flag = false;
            }
        }

        // person_infoテーブル
        public struct PersonInfo
        {
            public string sid;
            public string id;
            public string name;
            public string num01;
            public string txt01;
            public string txt02;
            public string sel01;
            public string sel02;
            public string sel03;
            public string sel04;
            public string sel05;
            public string sel06;
            public string sel07;
            // 2016/04/18 救護を削除 → 2018/08/08 避難所内外として復活
            public string sel08;
            public string send_datetime;
            public string update_datetime;

            public string index;
            public string error_reason;

            public void init()
            {
                sid = "";
                id = "";
                name = "";
                num01 = "";
                txt01 = "";
                txt02 = "";
                sel01 = "";
                sel02 = "";
                sel03 = "";
                sel04 = "";
                sel05 = "";
                sel06 = "";
                sel07 = "";
                // 2016/04/18 救護を削除 → 2018/08/08 避難所内外として復活
                sel08 = "";
                send_datetime = "";
                update_datetime = "";
                index = "";
                error_reason = "";
            }

            // 2016/04/18 送信中表示の対応
            public static bool operator ==(PersonInfo a, PersonInfo b)
            {
                // 参照元が一緒なら同
                if (System.Object.ReferenceEquals(a, b))
                {
                    return true;
                }

                // 一方がnullならば異
                if (((object)a == null) || ((object)b == null))
                {
                    return false;
                }

                // メンバ変数の比較
                return (a.id == b.id)
                        && (a.name == b.name)
                        && (a.num01 == b.num01)
                        && (a.txt01 == b.txt01)
                        && (a.txt02 == b.txt02)
                        && (a.sel01 == b.sel01)
                        && (a.sel02 == b.sel02)
                        && (a.sel03 == b.sel03)
                        && (a.sel04 == b.sel04)
                        && (a.sel05 == b.sel05)
                        && (a.sel06 == b.sel06)
                        && (a.sel07 == b.sel07)
                    // 2016/04/18 救護を削除 → 2018/08/08 避難所内外として復活
                        && (a.sel08 == b.sel08)
                        && (a.send_datetime == b.send_datetime)
                        && (a.update_datetime == b.update_datetime)
                        && (a.sid == b.sid);
            }

            public static bool operator !=(PersonInfo a, PersonInfo b)
            {
                return !(a == b);
            }

            // 2016/04/18 未使用：定義していないと警告が発生するため作成
            public override bool Equals(System.Object obj)
            {
                // 比較対象がnullならば不一致
                if (obj == null)
                {
                    return false;
                }

                PersonInfo p = (PersonInfo)obj;
                return Equals(p);
            }

            // 2016/04/18 未使用：定義していないと警告が発生するため作成
            public bool Equals(PersonInfo p)
            {
                // 比較対象がnullならば不一致
                if (p == null)
                {
                    return false;
                }

                // メンバ変数の比較
                return (id == p.id)
                        && (name == p.name)
                        && (sid == p.sid)
                        && (num01 == p.num01)
                        && (txt01 == p.txt01)
                        && (txt02 == p.txt02)
                        && (sel01 == p.sel01)
                        && (sel02 == p.sel02)
                        && (sel03 == p.sel03)
                        && (sel04 == p.sel04)
                        && (sel05 == p.sel05)
                        && (sel06 == p.sel06)
                        && (sel07 == p.sel07)
                    // 2016/04/18 救護を削除 → 2018/08/08 避難所内外として復活
                        && (sel08 == p.sel08)
                        && (send_datetime == p.send_datetime)
                        && (update_datetime == p.update_datetime);
            }

            // 2016/04/18 未使用：定義していないと警告が発生するため作成
            public override int GetHashCode()
            {
                int nRet = 0;
                int.TryParse(id, out nRet);
                return nRet;
            }
        }

        // person_send_logテーブル
        public struct PersonSendLog
        {
            public string sid;
            public string id;
            public string name;
            public string num01;
            public string txt01;
            public string txt02;
            public string sel01;
            public string sel02;
            public string sel03;
            public string sel04;
            public string sel05;
            public string sel06;
            public string sel07;
            // 2016/04/18 救護を削除 → 2018/08/08 避難所内外として復活
            public string sel08;
            public string sendresult;
            public string update_datetime;

            public void init()
            {
                sid = "";
                id = "";
                name = "";
                num01 = "";
                txt01 = "";
                txt02 = "";
                sel01 = "";
                sel02 = "";
                sel03 = "";
                sel04 = "";
                sel05 = "";
                sel06 = "";
                sel07 = "";
                // 2016/04/18 救護を削除 → 2018/08/08 避難所内外として復活
                sel08 = "";
                sendresult = "";
                update_datetime = "";
            }

            public void Set(PersonInfo info)
            {
                sid = info.sid;
                id = info.id;
                name = info.name;
                num01 = info.num01;
                txt01 = info.txt01;
                txt02 = info.txt02;
                sel01 = info.sel01;
                sel02 = info.sel02;
                sel03 = info.sel03;
                sel04 = info.sel04;
                sel05 = info.sel05;
                sel06 = info.sel06;
                sel07 = info.sel07;
                // 2016/04/18 救護を削除 → 2018/08/08 避難所内外として復活
                sel08 = info.sel08;
                update_datetime = info.update_datetime;
            }
        }

        // person_logテーブル
        public struct PersonLog
        {
            public string sid;
            public string id;
            public string name;
            public string num01;
            public string txt01;
            public string txt02;
            public string sel01;
            public string sel02;
            public string sel03;
            public string sel04;
            public string sel05;
            public string sel06;
            public string sel07;
            // 2016/04/18 救護を削除 → 2018/08/08 避難所内外として復活
            public string sel08;
            public string update_datetime;

            public void init()
            {
                sid = "";
                id = "";
                name = "";
                num01 = "";
                txt01 = "";
                txt02 = "";
                sel01 = "";
                sel02 = "";
                sel03 = "";
                sel04 = "";
                sel05 = "";
                sel06 = "";
                sel07 = "";
                // 2016/04/18 救護を削除 → 2018/08/08 避難所内外として復活
                sel08 = "";
                update_datetime = "";
            }

            public void Set(PersonInfo info)
            {
                sid = info.sid;
                id = info.id;
                name = info.name;
                num01 = info.num01;
                txt01 = info.txt01;
                txt02 = info.txt02;
                sel01 = info.sel01;
                sel02 = info.sel02;
                sel03 = info.sel03;
                sel04 = info.sel04;
                sel05 = info.sel05;
                sel06 = info.sel06;
                sel07 = info.sel07;
                // 2016/04/18 救護を削除 → 2018/08/08 避難所内外として復活
                sel08 = info.sel08;
                update_datetime = info.update_datetime;
            }
        }

        // total_send_logテーブル
        public struct TotalSendLog
        {
            public string sid;
            public string num01;
            public string num02;
            public string num03;
            public string num04;
            public string num05;
            public string num06;
            public string num07;
            public string num08;
            public string num09;
            public string num10;
            public string num11;
            public string num12;
            public string txt01;
            public string sendresult;
            public string update_datetime;

            public void init()
            {
                sid = "";
                num01 = "";
                num02 = "";
                num03 = "";
                num04 = "";
                num05 = "";
                num06 = "";
                num07 = "";
                num08 = "";
                num09 = "";
                num10 = "";
                num11 = "";
                num12 = "";
                txt01 = "";
                sendresult = "";
                update_datetime = "";
            }
        }

        // auto_send_timeテーブル
        public struct AutoSendTime
        {
            public string id;
            public string type;
            public string month;
            public string day;
            public string hour;
            public string min;
            public string enabled;
            public string hour2;
            public string min2;
            public string send_type;

            public void init()
            {
                id = "";
                type = "";
                month = "";
                day = "";
                hour = "";
                min = "";
                enabled = "";
                hour2 = "";
                min2 = "";
                send_type = "";
            }
        }


        // コンストラクタ
        public DbAccess()
        {
            // 接続は明示的にConnectメソッドを呼び出して行う
        }

        // 破棄（自動で呼ばれる）
        public void Dispose()
        {
            // 切断する
            Disconnect();
        }

        // 接続
        public int Connect()
        {
            int nRet = 0;
            // 接続対象のDB
            // ※config [ShelterInfoSystem]に定義された文字列
            string connectionString = global::ShelterInfoSystem.Properties.Settings.Default.DbConnect;
            connectionString += m_dbPass;
            connectionString += ";";

            try
            {
                // 接続を生成する
                //m_connection = new MySqlConnection(connectionString);
                m_connection = new OleDbConnection(connectionString);

                // オープン
                m_connection.Open();
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "Connect", ex.ToString());
                // 例外を投げる
                // throw ex;

                nRet = -1;
            }

            return nRet;
        }

        // 切断
        public void Disconnect()
        {
            try
            {
                // 接続が生成されている場合、綺麗に片付ける
                if (m_connection != null)
                {
                    m_connection.Close();
                    m_connection.Dispose();
                    m_connection = null;
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "Disconnect", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        // トランザクションの開始
        public void BeginTran()
        {
            try
            {
                // コネクションが有効の場合
                if (m_connection != null)
                {
                    // トランザクション開始
                    m_transaction = m_connection.BeginTransaction();
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "BeginTran", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        // トランザクションのコミット
        public void CommitTran()
        {
            try
            {
                // トランザクションが有効の場合
                if (m_transaction.Connection != null)
                {
                    // コミット
                    m_transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "CommitTran", ex.ToString());
                // 例外を投げる
                // throw ex;
            }

            // トランザクション初期化
            m_transaction = null;
        }

        // トランザクションのロールバック
        public void RollbackTran()
        {
            try
            {
                // トランザクションが有効の場合
                if (m_transaction.Connection != null)
                {
                    // ロールバック
                    m_transaction.Rollback();
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "RollbackTran", ex.ToString());
                // 例外を投げる
                // throw ex;
            }

            // トランザクション初期化
            m_transaction = null;
        }

        // step2 DbAccessStep2を使用するためpublicに変更
        // 抽出コマンド実行（デットロック対応）
        //public MySqlDataReader ExecuteReader(MySqlCommand com)
        public OleDbDataReader ExecuteReader(OleDbCommand com)
        {
            //MySqlDataReader cReader = null;
            OleDbDataReader cReader = null;

            // リトライ回数分繰り返す
            int nRetryCount = 0;
            while (true)
            {
                try
                {
                    // SQLコマンド実行
                    cReader = com.ExecuteReader();
                    // 成功
                    break;
                }
                //catch (MySqlException ex)
                catch (OleDbException ex)
                {
                    // タイムアウトは即エラー
                    //                    // デッドロック 又は タイムアウトエラーの場合
                    //                    if (ex.Number == 1205 || ex.Number == -2)
                    // デッドロックエラーの場合
                    //if (ex.Number == 1205)
                    if (ex.Errors[0].SQLState == "3008")
                    // タイムアウトは即エラー
                    {
                        // リトライ回数に達していない場合
                        if (nRetryCount < global::ShelterInfoSystem.Properties.Settings.Default.SqlExecRetryCount)
                        {
                            // リトライ
                            nRetryCount++;
                            continue;
                        }
                    }
                    // エラーを投げる
                    Program.m_thLog.PutErrorLog(m_strBlockName, "ExecuteReader", ex.ToString());
                    // 例外を投げる
                    // throw ex;
                    break;
                }
            }

            return cReader;
        }

        // 更新コマンド実行（デットロック対応）
        //private int ExecuteNonQuery(MySqlCommand com)
        private int ExecuteNonQuery(OleDbCommand com)
        {
            int rowCount = 0;

            // リトライ回数分繰り返す
            int nRetryCount = 0;
            while (true)
            {
                try
                {
                    // SQLコマンド実行
                    rowCount = com.ExecuteNonQuery();
                    // 成功
                    break;
                }
                //catch (MySqlException ex)
                catch (OleDbException ex)
                {
                    // タイムアウトは即エラー
                    //                    // デッドロック 又は タイムアウトエラーの場合
                    //                    if (ex.Number == 1205 || ex.Number == -2)
                    // デッドロックエラーの場合
                    //if (ex.Number == 1205)
                    if (ex.Errors[0].SQLState == "3008")
                    // タイムアウトは即エラー
                    {
                        // リトライ回数に達していない場合
                        if (nRetryCount < global::ShelterInfoSystem.Properties.Settings.Default.SqlExecRetryCount)
                        {
                            // リトライ
                            nRetryCount++;
                            continue;
                        }
                    }
                    // エラーを投げる
                    Program.m_thLog.PutErrorLog(m_strBlockName, "ExecuteNonQuery", ex.ToString());
                    // 例外を投げる
                    throw ex;
                }
            }

            return rowCount;
        }

        // terminal_infoテーブルの更新
        public void UpsertTerminalInfo(TerminalInfo selInfo)
        {

            //terminal_infoの存在確認
            string sTable = "terminal_info";
            string sWhere = String.Format("id = {0}", ConvCharData(selInfo.sid));
            bool bExist = ExistInfo(sTable, sWhere);

            // 更新時間設定
            selInfo.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            if (bExist)
            {
                /* 既存レコードが存在するならUPDATE */
                Update_TerminalInfo(selInfo);
            }
            else
            {
                /* 既存レコードが存在しないならINSERT */
                Insert_TerminalInfo(selInfo);

            }

            #region upsert
            //try
            //{
            //    //using (MySqlCommand updCom = m_connection.CreateCommand())
            //    using(OleDbCommand updCom = m_connection.CreateCommand())
            //    {
            //        // トランザクション設定
            //        updCom.Transaction = m_transaction;

            //        // SQLコマンド設定
            //        string sTable = "terminal_info";

            //        string sValues = String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
            //                                        ConvCharData(selInfo.id), ConvCharData(selInfo.gid), ConvCharData(selInfo.name), selInfo.lat, selInfo.lon, selInfo.open_flag, ConvDateTimeData(selInfo.open_datetime), ConvDateTimeData(selInfo.close_datetime), ConvDateTimeData(selInfo.update_datetime));
            //        string sSet = String.Format("id = {0}, gid = {1}, name = {2}, lat = {3}, lon = {4}, open_flag = {5}, open_datetime = {6}, close_datetime = {7}, update_datetime = {8}",
            //                                        ConvCharData(selInfo.id), ConvCharData(selInfo.gid), ConvCharData(selInfo.name), selInfo.lat, selInfo.lon, selInfo.open_flag, ConvDateTimeData(selInfo.open_datetime), ConvDateTimeData(selInfo.close_datetime), ConvDateTimeData(selInfo.update_datetime));

            //        string sInsert = String.Format("INSERT INTO {0} (id, gid, name, lat, lon, open_flag, open_datetime, close_datetime, update_datetime) VALUES ( {1} ) ON DUPLICATE KEY UPDATE {2}", sTable, sValues, sSet);

            //        updCom.CommandText = sInsert;

            //        // 作成したSQL文を設定/実行/破棄
            //        ExecuteNonQuery(updCom);            // 実行
            //        // デッドロック対応
            //        updCom.Dispose();                   // 破棄

            //    }
            //}
            //catch (Exception ex)
            //{
            //    Program.m_thLog.PutErrorLog(m_strBlockName, "UpdateTerminalInfo", ex.ToString());
            //    // 例外を投げる
            //    // throw ex;
            //}
            #endregion
        }

        // 20180130
        // terminal_infoテーブルの存在取得
        public bool ExistInfo(string sTable, string sWhere)
        {
            bool bExist = false;
            try
            {
                using (OleDbCommand selectCom = m_connection.CreateCommand())
                {
                    selectCom.Transaction = m_transaction;
                    selectCom.CommandText = String.Format("SELECT count(*) FROM {0} WHERE {1}", sTable, sWhere);

                    //指定したSQLコマンドを実行してSqlDataReaderを構築する
                    using (OleDbDataReader cReader = ExecuteReader(selectCom))
                    {
                        //次のレコードに進める(次のレコードがない場合、実行されない)
                        while (cReader.Read())
                        {
                            int nColIdx = 0;
                            int nCount = 0;

                            //読み込んだデータを元に設定
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                nCount = int.Parse(cReader[nColIdx].ToString());
                            }
                            nColIdx++;

                            //データ有
                            if (nCount > 0)
                            {
                                bExist = true;
                            }

                            break;
                        }

                        // cReaderの破棄
                        cReader.Dispose();
                    }

                    selectCom.Dispose();
                }

            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "ExistTerminalInfo", ex.ToString());
                bExist = false;
            }

            return bExist;

        }
        public void Insert_TerminalInfo(TerminalInfo selInfo)
        {
            try
            {
                // null文字対策
                if (selInfo.open_datetime == null)
                {
                    selInfo.open_datetime = "";
                }

                if (selInfo.close_datetime == null)
                {
                    selInfo.close_datetime = "";
                }

                if (selInfo.memo == null)
                {
                    selInfo.memo = "";
                }
                if (selInfo.update_datetime == null)
                {
                    selInfo.update_datetime = "";
                }

                // シングルコーテーション置換
                selInfo = (TerminalInfo)ChkSglQuot(selInfo);

                using (OleDbCommand insCom = m_connection.CreateCommand())
                {
                    // 更新日時の設定
                    selInfo.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                    // トランザクション設定
                    insCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sTable = "terminal_info";

                    string sValues = String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}",
                                                    ConvCharData(selInfo.sid), ConvCharData(selInfo.gid), ConvCharData(selInfo.smid), ConvCharData(selInfo.name),
                                                    selInfo.lat, selInfo.lon, ConvCharData(selInfo.memo), ConvCharData(selInfo.status), (int)selInfo.open_flag, ConvDateTimeData(selInfo.open_datetime),
                                                    ConvDateTimeData(selInfo.close_datetime), ConvDateTimeData(selInfo.update_datetime), ConvCharData(selInfo.text_flag), selInfo.select_flag, ConvCharData(selInfo.dummy_num), selInfo.dummy_num_flag);

                    string sInsert = String.Format("INSERT INTO {0} (id, gid, smid, name, lat, lon, memo_str, status_str, open_flag, open_datetime, close_datetime, update_datetime, text_flag, select_flag, dummy_num, dummy_num_flag) VALUES ( {1} )", sTable, sValues);

                    insCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(insCom);            // 実行
                    // デッドロック対応
                    insCom.Dispose();                   // 破棄

                }

                // 送信端末情報テーブルに最終smidを保存
                int maxSmid = 0;
                TerminalInfo[] allTerminalInfo = new TerminalInfo[0];
                GetTerminalInfoAll(ref allTerminalInfo);
                foreach (var item in allTerminalInfo)
                {
                    if (item.gid == selInfo.gid)
                    {
                        if (maxSmid < int.Parse(selInfo.smid))
                        {
                            maxSmid = int.Parse(selInfo.smid);
                        }
                    }
                }
                UpsertQANPIDeviceInfo(selInfo.gid, maxSmid);
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "UpdateTerminalInfo", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }
        public void Update_TerminalInfo(TerminalInfo selInfo)
        {
            try
            {
                // シングルコーテーション置換
                selInfo = (TerminalInfo)ChkSglQuot(selInfo);

                //using (MySqlCommand updCom = m_connection.CreateCommand())
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // null文字対策
                    if (selInfo.open_datetime == null)
                    {
                        selInfo.open_datetime = "";
                    }

                    if (selInfo.close_datetime == null)
                    {
                        selInfo.close_datetime = "";
                    }

                    if (selInfo.memo == null)
                    {
                        selInfo.memo = "";
                    }
                    if (selInfo.update_datetime == null)
                    {
                        selInfo.update_datetime = "";
                    }

                    // 更新日時の設定
                    selInfo.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                    // SQLコマンド設定
                    string sTable = "terminal_info";

                    string sSet = String.Format("id = {0}, gid = {1}, name = {2}, lat = {3}, lon = {4}, open_flag = {5}, open_datetime = {6}, close_datetime = {7}, update_datetime = {8}, memo_str = {9}, smid = {10}, text_flag = {11}, status_str = {12}, select_flag = {13}, dummy_num = {14},dummy_num_flag = {15}",
                                                    ConvCharData(selInfo.sid), ConvCharData(selInfo.gid), ConvCharData(selInfo.name), selInfo.lat, selInfo.lon, (int)selInfo.open_flag,
                                                    ConvDateTimeData(selInfo.open_datetime), ConvDateTimeData(selInfo.close_datetime), ConvDateTimeData(selInfo.update_datetime),
                                                    ConvDateTimeData(selInfo.memo), ConvCharData(selInfo.smid), selInfo.text_flag, ConvCharData(selInfo.status), selInfo.select_flag, ConvCharData(selInfo.dummy_num), selInfo.dummy_num_flag);

                    string sWhere = String.Format("id = {0}", ConvCharData(selInfo.sid));

                    string sUpdate = String.Format("UPDATE {0} SET {1} WHERE {2}", sTable, sSet, sWhere);

                    updCom.CommandText = sUpdate;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄

                }

                // 送信端末情報テーブルに最終smidを保存
                TerminalInfo[] allTerminalInfo = new TerminalInfo[0];
                GetTerminalInfoAll(ref allTerminalInfo);
                int lastSmid = 0;
                string updateDate = "";
                GetRegister_QANPIDeviceInfo(selInfo.gid, ref lastSmid, ref updateDate);
                int maxSmid = lastSmid;
                foreach (var item in allTerminalInfo)
                {
                    if (item.gid == selInfo.gid)
                    {
                        if (maxSmid < int.Parse(selInfo.smid))
                        {
                            maxSmid = int.Parse(selInfo.smid);
                        }
                    }
                }
                UpsertQANPIDeviceInfo(selInfo.gid, maxSmid);
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "UpdateTerminalInfo", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }
        // -- 20180130

        // terminal_infoテーブルのデータ状態取得
        public void GetTerminalInfo(string sId, out bool bExist, ref TerminalInfo upInfo)
        {
            upInfo.init();
            bExist = false;
            try
            {
                //using (MySqlCommand selectCom = m_connection.CreateCommand())
                using (OleDbCommand selectCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    selectCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sSelect = "id, gid, smid, name, lat, lon, open_flag, open_datetime, close_datetime, update_datetime, memo_str, status_str, text_flag, select_flag, dummy_num, dummy_num_flag";
                    string sTable = "terminal_info";
                    string sWhere = string.Format("id = {0}", ConvCharData(sId));

                    if (sId.Length > 0)
                    {
                        selectCom.CommandText = String.Format("SELECT {0} FROM {1} WHERE {2}", sSelect, sTable, sWhere);
                    }
                    else
                    {
                        selectCom.CommandText = String.Format("SELECT {0} FROM {1}", sSelect, sTable);
                    }

                    // 指定した SQL コマンドを実行して SqlDataReader を構築する
                    //using (MySqlDataReader cReader = ExecuteReader(selectCom))
                    using (OleDbDataReader cReader = ExecuteReader(selectCom))
                    {
                        // 次のレコードに進める (次のレコードがない場合は false になるため実行されない)
                        while (cReader.Read())
                        {
                            int nColIdx = 0;
                            // 読み込んだデータを元にデータを設定
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                upInfo.sid = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                upInfo.sid = "";
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                upInfo.gid = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                upInfo.gid = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                upInfo.smid = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                upInfo.smid = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                upInfo.name = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                upInfo.name = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                upInfo.lat = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                upInfo.lat = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                upInfo.lon = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                upInfo.lon = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                switch (cReader[nColIdx].ToString())
                                {
                                    case "0":
                                        upInfo.open_flag = FormShelterInfo.SHELTER_STATUS.NO_OPEN;
                                        break;
                                    case "1":
                                        upInfo.open_flag = FormShelterInfo.SHELTER_STATUS.OPEN;
                                        break;
                                    case "2":
                                        upInfo.open_flag = FormShelterInfo.SHELTER_STATUS.CLOSE;
                                        break;
                                    default:
                                        upInfo.open_flag = FormShelterInfo.SHELTER_STATUS.NO_OPEN;
                                        break;
                                }
                            }
                            else
                            {
                                upInfo.open_flag = FormShelterInfo.SHELTER_STATUS.NO_OPEN;
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                upInfo.open_datetime = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                upInfo.open_datetime = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                upInfo.close_datetime = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                upInfo.close_datetime = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                upInfo.update_datetime = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                upInfo.update_datetime = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                upInfo.memo = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                upInfo.memo = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                upInfo.status = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                upInfo.status = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                upInfo.text_flag = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                upInfo.text_flag = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                if (cReader[nColIdx].ToString() == "True")
                                {
                                    upInfo.select_flag = true;
                                }
                                else
                                {
                                    upInfo.select_flag = false;
                                }
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                upInfo.dummy_num = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                upInfo.dummy_num = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                if (cReader[nColIdx].ToString() == "True")
                                {
                                    upInfo.dummy_num_flag = true;
                                }
                                else
                                {
                                    upInfo.dummy_num_flag = false;
                                }
                            }
                            else
                            {
                                upInfo.dummy_num_flag = false;
                            }

                            // データ有り
                            bExist = true;

                            break;
                        }

                        // cReader を破棄する
                        cReader.Dispose();
                    }

                    // selectCom を破棄する (正しくは オブジェクトの破棄を保証する を参照)
                    selectCom.Dispose();
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "GetTerminalInfo", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        // terminal_infoテーブルの全データ取得
        public void GetTerminalInfoAll(ref TerminalInfo[] terminalInfo)
        {
            try
            {
                using (OleDbCommand selectCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    selectCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sSelect = "id, gid, smid, name, lat, lon, open_flag, open_datetime, close_datetime, update_datetime, memo_str, status_str, text_flag, select_flag, dummy_num, dummy_num_flag";
                    string sTable = "terminal_info";
                    selectCom.CommandText = String.Format("SELECT {0} FROM {1} ORDER BY id ASC", sSelect, sTable);

                    // 出力変数初期化
                    terminalInfo = new TerminalInfo[0];

                    // 指定した SQL コマンドを実行して SqlDataReader を構築する
                    //using (MySqlDataReader cReader = ExecuteReader(selectCom))
                    using (OleDbDataReader cReader = ExecuteReader(selectCom))
                    {
                        // 次のレコードに進める (次のレコードがない場合は false になるため実行されない)
                        while (cReader.Read())
                        {
                            // 結果変数増設
                            Array.Resize(ref terminalInfo, terminalInfo.Length + 1);

                            int nColIdx = 0;
                            // 読み込んだデータを元にデータを設定
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                terminalInfo[terminalInfo.Length - 1].sid = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                terminalInfo[terminalInfo.Length - 1].sid = "";
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                terminalInfo[terminalInfo.Length - 1].gid = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                terminalInfo[terminalInfo.Length - 1].gid = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                terminalInfo[terminalInfo.Length - 1].smid = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                terminalInfo[terminalInfo.Length - 1].smid = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                terminalInfo[terminalInfo.Length - 1].name = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                terminalInfo[terminalInfo.Length - 1].name = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                terminalInfo[terminalInfo.Length - 1].lat = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                terminalInfo[terminalInfo.Length - 1].lat = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                terminalInfo[terminalInfo.Length - 1].lon = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                terminalInfo[terminalInfo.Length - 1].lon = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                switch (cReader[nColIdx].ToString())
                                {
                                    case "0":
                                        terminalInfo[terminalInfo.Length - 1].open_flag = FormShelterInfo.SHELTER_STATUS.NO_OPEN;
                                        break;
                                    case "1":
                                        terminalInfo[terminalInfo.Length - 1].open_flag = FormShelterInfo.SHELTER_STATUS.OPEN;
                                        break;
                                    case "2":
                                        terminalInfo[terminalInfo.Length - 1].open_flag = FormShelterInfo.SHELTER_STATUS.CLOSE;
                                        break;
                                    default:
                                        terminalInfo[terminalInfo.Length - 1].open_flag = FormShelterInfo.SHELTER_STATUS.NO_OPEN;
                                        break;
                                }
                            }
                            else
                            {
                                terminalInfo[terminalInfo.Length - 1].open_flag = FormShelterInfo.SHELTER_STATUS.NO_OPEN;
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                terminalInfo[terminalInfo.Length - 1].open_datetime = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                terminalInfo[terminalInfo.Length - 1].open_datetime = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                terminalInfo[terminalInfo.Length - 1].close_datetime = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                terminalInfo[terminalInfo.Length - 1].close_datetime = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                terminalInfo[terminalInfo.Length - 1].update_datetime = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                terminalInfo[terminalInfo.Length - 1].update_datetime = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                terminalInfo[terminalInfo.Length - 1].memo = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                terminalInfo[terminalInfo.Length - 1].memo = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                terminalInfo[terminalInfo.Length - 1].status = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                terminalInfo[terminalInfo.Length - 1].status = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                terminalInfo[terminalInfo.Length - 1].text_flag = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                terminalInfo[terminalInfo.Length - 1].text_flag = "0";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                if (cReader[nColIdx].ToString() == "True")
                                {
                                    terminalInfo[terminalInfo.Length - 1].select_flag = true;
                                }
                                else
                                {
                                    terminalInfo[terminalInfo.Length - 1].select_flag = false;
                                }
                            }
                            else
                            {
                                terminalInfo[terminalInfo.Length - 1].select_flag = false;
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                terminalInfo[terminalInfo.Length - 1].dummy_num = cReader[nColIdx].ToString();
                            }
                            else
                            {
                                terminalInfo[terminalInfo.Length - 1].dummy_num = "";
                            }

                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                if (cReader[nColIdx].ToString() == "True")
                                {
                                    terminalInfo[terminalInfo.Length - 1].dummy_num_flag = true;
                                }
                                else
                                {
                                    terminalInfo[terminalInfo.Length - 1].dummy_num_flag = false;
                                }
                            }
                            else
                            {
                                terminalInfo[terminalInfo.Length - 1].dummy_num_flag = false;
                            }

                        }

                        // cReader を破棄する
                        cReader.Dispose();
                    }

                    // selectCom を破棄する (正しくは オブジェクトの破棄を保証する を参照)
                    selectCom.Dispose();
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "GetTerminalInfo", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        // 2016/04/19
        // person_infoテーブルの送信日時更新(Update)
        public void UpdateSendDateTimePersonInfo(string sId, string sName, PersonInfo upInfo)
        {
            //idの一致するレコードのupdatetime
            string sUpdatetime = getUpdatetime((upInfo.id), (upInfo.name), (upInfo.sid));

            try
            {
                // シングルコーテーション置換
                upInfo = (PersonInfo)ChkSglQuot(upInfo);

                //using (MySqlCommand updCom = m_connection.CreateCommand())
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // 更新日時の設定
                    upInfo.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sWhere = string.Format("id = {0} and name = {1} and sid = {2}", ConvCharData(sId), ConvCharData(sName), ConvCharData(upInfo.sid));
                    string sTable = "person_info";

                    //string sSet = string.Format("send_datetime = if( update_datetime > {0}, null, {1})", ConvDateTimeData(upInfo.update_datetime), ConvDateTimeData(upInfo.send_datetime));
                    //db.updatetimeとupInfo.updatetimeの比較
                    String sSendDatetime = ConvDateTimeData(upInfo.send_datetime);
                    if (sUpdatetime == "null")
                    {
                        /* 何もしない */
                    }
                    else
                    {
                        // sUpdatetimeとupInfo.update_datetimeの比較を行う
                        // sUpdatetimeの方が大きいときは "null"に変更
                        string strTemp = compareDatetime(sUpdatetime, upInfo.update_datetime);
                        if (strTemp == "null")
                        {
                            sSendDatetime = "null";
                        }
                        else
                        {
                            sSendDatetime = ConvDateTimeData(upInfo.send_datetime);
                        }

                    }

                    string sSet = string.Format("send_datetime = {0}", sSendDatetime);

                    string sInsert = String.Format("UPDATE {0} SET {1} WHERE {2}", sTable, sSet, sWhere);
                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "UpdatePersonInfo", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        //person_info.id,person_info.nameで検索し、検索結果が存在するならUpdatetimeを返す
        public String getUpdatetime(string sId, string sName, string sSid)
        {
            PersonInfo selInfo = new PersonInfo();
            bool bExist = false;

            try
            {
                // シングルコーテーション置換
                sName = (string)ChkSglQuot(sName);

                //using (MySqlCommand selectCom = m_connection.CreateCommand())
                using (OleDbCommand selectCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    selectCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sSelect = "update_datetime";
                    string sTable = "person_info";
                    string sWhere = string.Format("id = {0} and name = {1} and sid = {2}", ConvCharData(sId), ConvCharData(sName), ConvCharData(sSid));

                    selectCom.CommandText = String.Format("SELECT {0} FROM {1} WHERE {2}", sSelect, sTable, sWhere);

                    // 指定した SQL コマンドを実行して SqlDataReader を構築する
                    //using (MySqlDataReader cReader = ExecuteReader(selectCom))
                    using (OleDbDataReader cReader = ExecuteReader(selectCom))
                    {
                        // 次のレコードに進める (次のレコードがない場合は false になるため実行されない)
                        while (cReader.Read())
                        {
                            int nColIdx = 0;
                            // 読み込んだデータを元にデータを設定
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.update_datetime = cReader[nColIdx].ToString();
                            }
                            nColIdx++;

                            // データ有り
                            bExist = true;

                            break;
                        }

                        // cReader を破棄する
                        cReader.Dispose();
                    }

                    // selectCom を破棄する (正しくは オブジェクトの破棄を保証する を参照)
                    selectCom.Dispose();
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "GetPersonInfo", ex.ToString());
                // 例外を投げる
                // throw ex;
            }

            if (bExist)
            {
                return selInfo.update_datetime;
            }
            else
            {
                return "null";
            }
        }
        public string compareDatetime(string sTime_one, string sTime_two)
        {
            DateTime d1 = DateTime.Parse(sTime_one);
            DateTime d2 = DateTime.Parse(sTime_two);

            if (d1 > d2)
            {
                return "null";
            }
            else
            {
                return sTime_two;
            }

        }


        // person_infoテーブルの更新(Upsert)
        public void UpsertPersonInfo(PersonInfo upInfo)
        {

            //存在確認
            bool bExist = false;
            ExistPersonInfo(upInfo, out bExist);

            if (bExist)
            {
                //存在するならUpdate
                UpdatePersonInfo(upInfo);
            }
            else
            {
                //存在しないならInsert
                InsertPersonInfo(upInfo);
            }

            return;


            #region Upsert 変更前
            //            try
            //            {
            //                //using (MySqlCommand updCom = m_connection.CreateCommand())
            //                using (OleDbCommand updCom = m_connection.CreateCommand())
            //                {
            //                    // トランザクション設定
            //                    updCom.Transaction = m_transaction;

            //                    // SQLコマンド設定
            //                    string sTable = "person_info";
            //                    // 2016/04/18 救護を削除
            //                    // 2016/04/27 削除フラグ追加
            //#if true
            //                    string sValues = String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}",
            //                                                    ConvCharData(upInfo.id), ConvCharData(upInfo.name), upInfo.num01, ConvCharData(upInfo.txt01), upInfo.sel01, upInfo.sel02, upInfo.sel03, upInfo.sel04, upInfo.sel05, upInfo.sel06, upInfo.sel07, 0, ConvDateTimeData(upInfo.send_datetime), ConvDateTimeData(upInfo.update_datetime));
            //                    string sSet = String.Format("id = {0}, name = {1}, num01 = {2}, txt01 = {3}, sel01 = {4}, sel02 = {5}, sel03 = {6}, sel04 = {7}, sel05 = {8}, sel06 = {9}, sel07 = {10}, del_flag = 0, send_datetime = {11}, update_datetime = {12}",
            //                                                    ConvCharData(sId), ConvCharData(upInfo.name), upInfo.num01, ConvCharData(upInfo.txt01), upInfo.sel01, upInfo.sel02, upInfo.sel03, upInfo.sel04, upInfo.sel05, upInfo.sel06, upInfo.sel07, ConvDateTimeData(upInfo.send_datetime), ConvDateTimeData(upInfo.update_datetime));
            //                    string sInsert = String.Format("INSERT INTO {0} (id, name, num01, txt01, sel01, sel02, sel03, sel04, sel05, sel06, sel07, del_flag, send_datetime, update_datetime) VALUES ( {1} ) ON DUPLICATE KEY UPDATE {2};", sTable, sValues, sSet);
            //                    updCom.CommandText = sInsert;
            //#else
            //                    updCom.CommandText = "INSERT INTO person_info " +
            //                        "(@id, @name, @num01, @txt01, @sel01, @sel02, @sel03, @sel04, " +
            //                        "@sel05, @sel06, @sel07, @del_flag, @send_datetime, @update_datetime) " +
            //                        " ON DUPLICATE KEY UPDATE id = @id, name = @name, " +
            //                        "num01 = @num01, txt01 = @txt01, sel01 = @sel01, " +
            //                        "sel02 = @sel02, sel03 = @sel03, sel04 = @sel04, " +
            //                        "sel05 = @sel05, sel06 = @sel06, sel07 = @sel07, " +
            //                        "del_flag = 0, send_datetime = @send_datetime, " +
            //                        "update_datetime = @update_datetime";
            //                    updCom.Parameters.Add(new MySqlParameter("id", ConvCharData(upInfo.id)));
            //                    updCom.Parameters.Add(new MySqlParameter("name", ConvCharData(upInfo.name)));
            //                    updCom.Parameters.Add(new MySqlParameter("num01", upInfo.num01));
            //                    updCom.Parameters.Add(new MySqlParameter("txt01", ConvCharData(upInfo.txt01)));
            //                    updCom.Parameters.Add(new MySqlParameter("sel01", upInfo.sel01));
            //                    updCom.Parameters.Add(new MySqlParameter("sel02", upInfo.sel02));
            //                    updCom.Parameters.Add(new MySqlParameter("sel03", upInfo.sel03));
            //                    updCom.Parameters.Add(new MySqlParameter("sel04", upInfo.sel04));
            //                    updCom.Parameters.Add(new MySqlParameter("sel05", upInfo.sel05));
            //                    updCom.Parameters.Add(new MySqlParameter("sel06", upInfo.sel06));
            //                    updCom.Parameters.Add(new MySqlParameter("sel07", upInfo.sel07));
            //                    updCom.Parameters.Add(new MySqlParameter("del_flag", 0));
            //                    updCom.Parameters.Add(
            //                        new MySqlParameter("send_datetime", ConvDateTimeData(upInfo.send_datetime)));
            //                    updCom.Parameters.Add(
            //                        new MySqlParameter("update_datetime", ConvDateTimeData(upInfo.update_datetime)));
            //#endif
            //                    Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "DbAccess", "person", updCom.CommandText);
            //                    // 作成したSQL文を設定/実行/破棄
            //                    ExecuteNonQuery(updCom);            // 実行
            //                    // デッドロック対応
            //                    updCom.Dispose();                   // 破棄
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                Program.m_thLog.PutErrorLog(m_strBlockName, "UpdatePersonInfo", ex.ToString());
            //                // 例外を投げる
            //                throw ex;
            //            }
            #endregion
        }

        //20180301 - person_infoの追加
        public void InsertPersonInfo(PersonInfo upInfo)
        {
            try
            {
                // シングルコーテーション置換
                upInfo = (PersonInfo)ChkSglQuot(upInfo);

                //using (MySqlCommand updCom = m_connection.CreateCommand())
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // 更新日時の設定
                    upInfo.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sTable = "person_info";
                    string sValues = String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}",
                                                    ConvCharData(upInfo.id), ConvCharData(upInfo.name), ConvCharData(upInfo.txt01), ConvCharData(upInfo.txt02),
                                                    ConvCharData(upInfo.sel01), ConvCharData(upInfo.sel02), ConvCharData(upInfo.sel03), ConvCharData(upInfo.sel04),
                                                    ConvCharData(upInfo.sel05), ConvCharData(upInfo.sel06), ConvCharData(upInfo.sel07), ConvCharData(upInfo.sel08), 0,
                                                    ConvDateTimeData(upInfo.send_datetime), ConvDateTimeData(upInfo.update_datetime), ConvCharData(upInfo.index), ConvCharData(upInfo.error_reason), ConvCharData(upInfo.sid));
                    //string sSet = String.Format("id = {0}, name = {1}, num01 = {2}, txt01 = {3}, sel01 = {4}, sel02 = {5}, sel03 = {6}, sel04 = {7}, sel05 = {8}, sel06 = {9}, sel07 = {10}, del_flag = 0, send_datetime = {11}, update_datetime = {12}",
                    //                                ConvCharData(upInfo.id), ConvCharData(upInfo.name), upInfo.num01, ConvCharData(upInfo.txt01), upInfo.sel01, upInfo.sel02, upInfo.sel03, upInfo.sel04, upInfo.sel05, upInfo.sel06, upInfo.sel07, ConvDateTimeData(upInfo.send_datetime), ConvDateTimeData(upInfo.update_datetime));
                    string sInsert = String.Format("INSERT INTO {0} (id, name, txt01, txt02, sel01, sel02, sel03, sel04, sel05, sel06, sel07,sel08, del_flag, send_datetime, update_datetime, rowindex, error_reason, sid) VALUES ( {1} )", sTable, sValues);
                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "InsertPerson", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }
        //20180301 - person_infoの更新
        public void UpdatePersonInfo(PersonInfo upInfo)
        {
            try
            {
                // シングルコーテーション置換
                upInfo = (PersonInfo)ChkSglQuot(upInfo);

                //using (MySqlCommand updCom = m_connection.CreateCommand())
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // 更新日時の設定
                    upInfo.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sTable = "person_info";
                    //string sValues = String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}",
                    //                                ConvCharData(upInfo.id), ConvCharData(upInfo.name), upInfo.num01, ConvCharData(upInfo.txt01), upInfo.sel01, upInfo.sel02, upInfo.sel03, upInfo.sel04, upInfo.sel05, upInfo.sel06, upInfo.sel07, 0, ConvDateTimeData(upInfo.send_datetime), ConvDateTimeData(upInfo.update_datetime),upInfo.index,upInfo.error_reason);
                    string sSet = String.Format("id = {0}, name = {1}, txt01 = {2}, txt02 = {3}, sel01 = {4}, sel02 = {5}, sel03 = {6}, sel04 = {7}, sel05 = {8}, sel06 = {9}, sel07 = {10},sel08 = {11}, del_flag = 0, send_datetime = {12}, update_datetime = {13}, rowindex = {14}, error_reason = {15}, sid = {16}",
                                                    ConvCharData(upInfo.id), ConvCharData(upInfo.name), ConvCharData(upInfo.txt01), ConvCharData(upInfo.txt02),
                                                    ConvCharData(upInfo.sel01), ConvCharData(upInfo.sel02), ConvCharData(upInfo.sel03), ConvCharData(upInfo.sel04), ConvCharData(upInfo.sel05),
                                                    ConvCharData(upInfo.sel06), ConvCharData(upInfo.sel07), ConvCharData(upInfo.sel08),
                                                    ConvDateTimeData(upInfo.send_datetime), ConvDateTimeData(upInfo.update_datetime), ConvCharData(upInfo.index),
                                                    ConvCharData(upInfo.error_reason), ConvCharData(upInfo.sid));

                    //string sWhere = String.Format("id = {0}", ConvCharData(upInfo.id));
                    string sWhere = string.Format("id = {0} and name = {1} and sid = {2}", ConvCharData(upInfo.id), ConvCharData(upInfo.name), ConvCharData(upInfo.sid));

                    string sUpdate = String.Format("UPDATE {0} SET {1} WHERE {2}", sTable, sSet, sWhere);

                    updCom.CommandText = sUpdate;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄

                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "UpdateTerminalInfo", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        // person_infoテーブルの削除
        public void DeletePersonInfo(string sId, string sName, string sSid)
        {
            try
            {
                // シングルコーテーション削除
                sName = (string)ChkSglQuot(sName);

                //using (MySqlCommand updCom = m_connection.CreateCommand())
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    //2016/04/27 レコード削除でなく、削除フラグONとする
                    // SQLコマンド設定
                    string sTable = "person_info";
                    string sWhere = string.Format("id = {0} and name = {1} and sid = {2}", ConvCharData(sId), ConvCharData(sName), ConvCharData(sSid));

                    string sSet = "del_flag = 1";

                    //string sInsert = String.Format("DELETE FROM {0} WHERE {1}", sTable, sWhere);
                    string sInsert = String.Format("UPDATE {0} SET {1} WHERE {2}", sTable, sSet, sWhere);

                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "UpdatePersonInfo", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        // person_infoテーブルの全削除
        public void DeleteAllPersonInfo()
        {
            try
            {
                //using (MySqlCommand updCom = m_connection.CreateCommand())
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sTable = "person_info";

                    string sInsert = String.Format("DELETE FROM {0}", sTable);

                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "DeleteAllPersonInfo", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        /// <summary>
        /// person_infoから指定したsidのデータをすべて削除する
        /// </summary>
        /// <param name="targetSid"></param>
        public void DeleteShelterPersonInfo(TerminalInfo selInfo)
        {
            try
            {
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sTable = "person_info";
                    string sWhere = string.Format("sid = {0}", ConvCharData(selInfo.sid));
                    string sInsert = String.Format("DELETE FROM {0} WHERE {1}", sTable, sWhere);

                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "DeleteAllPersonInfo", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }


        // person_infoテーブルのデータ存在確認
        public void ExistPersonInfo(PersonInfo info, out bool bExist)
        {
            bExist = false;
            try
            {
                // シングルコーテーション置換
                info = (PersonInfo)ChkSglQuot(info);

                //using (MySqlCommand selectCom = m_connection.CreateCommand())
                using (OleDbCommand selectCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    selectCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sSelect = "count(*)";
                    string sTable = "person_info";
                    //                    string sWhere = string.Format("id    = {0} and name  = {1} and num01 <=> {2} and (txt01 <=> null or txt01 <=> {3}) and (sel01 <=> null or sel01 <=> {4}) and (sel02 <=> null or sel02 <=> {5}) and (sel03 <=> null or sel03 <=> {6}) and (sel04 <=> null or sel04 <=> {7}) and (sel05 <=> null or sel05 <=> {8}) and (sel06 <=> null or sel06 <=> {9}) and (sel07 <=> null or sel07 <=> {10}) and (sel08 <=> null or sel08 <=> {11}) and update_datetime = {12}",
                    //                                                ConvCharData(info.id), ConvCharData(info.name), info.num01, ConvCharData(info.txt01), info.sel01, info.sel02, info.sel03, info.sel04, info.sel05, info.sel06, info.sel07, info.sel08, ConvDateTimeData(info.update_datetime));
                    string sWhere = string.Format("id = {0} and name  = {1} and sid = {2}",
                                                   ConvCharData(info.id), ConvCharData(info.name), ConvCharData(info.sid));
                    selectCom.CommandText = String.Format("SELECT {0} FROM {1} WHERE {2} ", sSelect, sTable, sWhere);

                    // 指定した SQL コマンドを実行して SqlDataReader を構築する
                    //using (MySqlDataReader cReader = ExecuteReader(selectCom))
                    using (OleDbDataReader cReader = ExecuteReader(selectCom))
                    {
                        // 次のレコードに進める (次のレコードがない場合は false になるため実行されない)
                        while (cReader.Read())
                        {
                            int nColIdx = 0;
                            int nCount = 0;
                            // 読み込んだデータを元にデータを設定
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                nCount = int.Parse(cReader[nColIdx].ToString());
                            }
                            nColIdx++;

                            // データ有り
                            if (nCount > 0)
                            {
                                bExist = true;
                            }

                            break;
                        }

                        // cReader を破棄する
                        cReader.Dispose();
                    }

                    // selectCom を破棄する (正しくは オブジェクトの破棄を保証する を参照)
                    selectCom.Dispose();
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "GetPersonInfo", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        // person_infoテーブルのデータ状態取得
        public void GetPersonInfoList(string sid, ref List<PersonInfo> selInfoList)
        {
            selInfoList.Clear();
            try
            {
                //using (MySqlCommand selectCom = m_connection.CreateCommand())
                using (OleDbCommand selectCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    selectCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    // 2016/04/18 救護を削除
                    //string sSelect = "id, name, num01, txt01, sel01, sel02, sel03, sel04, sel05, sel06, sel07, sel08, send_datetime, update_datetime";
                    string sSelect = "id, name, num01, txt01, txt02, sel01, sel02, sel03, sel04, sel05, sel06, sel07, sel08, send_datetime, update_datetime, rowindex, error_reason, sid";
                    string sTable = "person_info";
                    string sWhere = "";
                    if (sid != "")
                    {
                        sWhere = string.Format("del_flag = 0 and sid = {0}", ConvCharData(sid));
                    }
                    else
                    {
                        sWhere = string.Format("del_flag = 0 and sid is {0}", ConvCharData(sid));
                    }

                    // 削除フラグOFFを対象とする
                    selectCom.CommandText = String.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY update_datetime DESC,update_datetime ASC", sSelect, sTable, sWhere);

                    // 指定した SQL コマンドを実行して SqlDataReader を構築する
                    //using (MySqlDataReader cReader = ExecuteReader(selectCom))
                    using (OleDbDataReader cReader = ExecuteReader(selectCom))
                    {
                        // 次のレコードに進める (次のレコードがない場合は false になるため実行されない)
                        while (cReader.Read())
                        {
                            PersonInfo selInfo = new PersonInfo();
                            selInfo.init();

                            int nColIdx = 0;
                            // 読み込んだデータを元にデータを設定
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.id = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.name = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.num01 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.txt01 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.txt02 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel01 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel02 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel03 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel04 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel05 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel06 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel07 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            // 2016/04/18 救護を削除 → 2018/08/08 避難所内外として復活
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel08 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.send_datetime = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.update_datetime = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.index = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.error_reason = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sid = cReader[nColIdx].ToString();
                            }

                            // データ有り
                            selInfoList.Add(selInfo);
                        }

                        // cReader を破棄する
                        cReader.Dispose();
                    }

                    // selectCom を破棄する (正しくは オブジェクトの破棄を保証する を参照)
                    selectCom.Dispose();
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "GetPersonInfoList", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        // person_logテーブルの追加
        public void InsertPersonLog(PersonLog upInfo)
        {
            try
            {
                // シングルコーテーション置換
                upInfo = (PersonLog)ChkSglQuot(upInfo);

                //using (MySqlCommand updCom = m_connection.CreateCommand())
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // 更新日時の設定
                    upInfo.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sTable = "person_log";
                    // 2016/04/18 救護を削除
                    //string sValues = String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}",
                    //                                ConvCharData(upInfo.id), ConvCharData(upInfo.name), upInfo.num01, ConvCharData(upInfo.txt01), upInfo.sel01, upInfo.sel02, upInfo.sel03, upInfo.sel04, upInfo.sel05, upInfo.sel06, upInfo.sel07, upInfo.sel08, ConvDateTimeData(upInfo.update_datetime));
                    string sValues = String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}",
                                                    ConvCharData(upInfo.id), ConvCharData(upInfo.name), ConvCharData(upInfo.txt01), ConvCharData(upInfo.txt02),
                                                    ConvCharData(upInfo.sel01), ConvCharData(upInfo.sel02), ConvCharData(upInfo.sel03), ConvCharData(upInfo.sel04),
                                                    ConvCharData(upInfo.sel05), ConvCharData(upInfo.sel06), ConvCharData(upInfo.sel07), ConvCharData(upInfo.sel08), ConvDateTimeData(upInfo.update_datetime), ConvCharData(upInfo.sid));

                    //string sInsert = String.Format("INSERT INTO {0} (id, name, num01, txt01, sel01, sel02, sel03, sel04, sel05, sel06, sel07, sel08, update_datetime) VALUES ( {1} )", sTable, sValues);
                    string sInsert = String.Format("INSERT INTO {0} (id, name, txt01, txt02, sel01, sel02, sel03, sel04, sel05, sel06, sel07, sel08, update_datetime, sid) VALUES ( {1} )", sTable, sValues);

                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "UpdatePersonLog", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        // person_logテーブルの削除
        public void DeletePersonLog(string sId, string sName)
        {
            try
            {
                // シングルコーテーション削除
                sName = (string)ChkSglQuot(sName);

                //using (MySqlCommand updCom = m_connection.CreateCommand())
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sTable = "person_log";
                    string sWhere = string.Format("id = {0} and name = {1}", ConvCharData(sId), ConvCharData(sName));

                    string sInsert = String.Format("DELETE FROM {0} WHERE {1}", sTable, sWhere);

                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "UpdatePersonLog", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        // person_logテーブルの全削除
        public void DeleteAllPersonLog()
        {
            try
            {
                //using (MySqlCommand updCom = m_connection.CreateCommand())
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sTable = "person_log";

                    string sInsert = String.Format("DELETE FROM {0}", sTable);

                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "DeleteAllPersonLog", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        /// <summary>
        /// person_logから指定したsidのデータをすべて削除
        /// </summary>
        /// <param name="targetSid"></param>
        public void DeleteShelterPersonLog(TerminalInfo selInfo)
        {
            try
            {
                //using (MySqlCommand updCom = m_connection.CreateCommand())
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sTable = "person_log";
                    string sWhere = string.Format("sid = {0}", ConvCharData(selInfo.sid));

                    string sInsert = String.Format("DELETE FROM {0} WHERE {1}", sTable, sWhere);

                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "DeleteAllPersonLog", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        // person_logテーブルのデータ状態取得
        public void GetPersonLogList(string id, string sName, string sid, ref List<PersonLog> selInfoList)
        {
            selInfoList.Clear();
            try
            {
                // シングルコーテーション置換
                sName = (string)ChkSglQuot(sName);

                //using (MySqlCommand selectCom = m_connection.CreateCommand())
                using (OleDbCommand selectCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    selectCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    // 2016/04/18 救護を削除
                    //string sSelect = "id, name, num01, txt01, sel01, sel02, sel03, sel04, sel05, sel06, sel07, sel08, update_datetime";
                    string sSelect = "id, name, num01, txt01, txt02, sel01, sel02, sel03, sel04, sel05, sel06, sel07, sel08, update_datetime, sid";
                    string sTable = "person_log";
                    string sWhere = string.Format("id = {0} and name = {1} and sid = {2}", ConvCharData(id), ConvCharData(sName), ConvCharData(sid));

                    selectCom.CommandText = String.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY update_datetime DESC", sSelect, sTable, sWhere);

                    // 指定した SQL コマンドを実行して SqlDataReader を構築する
                    //using (MySqlDataReader cReader = ExecuteReader(selectCom))
                    using (OleDbDataReader cReader = ExecuteReader(selectCom))
                    {
                        // 次のレコードに進める (次のレコードがない場合は false になるため実行されない)
                        while (cReader.Read())
                        {
                            PersonLog selInfo = new PersonLog();
                            selInfo.init();

                            int nColIdx = 0;
                            // 読み込んだデータを元にデータを設定
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.id = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.name = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.num01 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.txt01 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.txt02 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel01 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel02 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel03 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel04 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel05 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel06 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel07 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel08 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.update_datetime = cReader[nColIdx].ToString();
                            }
                            nColIdx++;

                            // データ有り
                            selInfoList.Add(selInfo);
                        }

                        // cReader を破棄する
                        cReader.Dispose();
                    }

                    // selectCom を破棄する (正しくは オブジェクトの破棄を保証する を参照)
                    selectCom.Dispose();
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "GetPersonLogList", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        // person_send_logテーブルの更新
        public void InsertPersonSendLog(PersonSendLog upInfo)
        {
            try
            {
                // シングルコーテーション置換
                upInfo = (PersonSendLog)ChkSglQuot(upInfo);

                //using (MySqlCommand updCom = m_connection.CreateCommand())
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // 更新日時の設定
                    upInfo.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sTable = "person_send_log";
                    // 2016/04/18 救護を削除
                    //string sValues = String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}",
                    //                                ConvCharData(upInfo.id), ConvCharData(upInfo.name), upInfo.num01, ConvCharData(upInfo.txt01), upInfo.sel01, upInfo.sel02, upInfo.sel03, upInfo.sel04, upInfo.sel05, upInfo.sel06, upInfo.sel07, upInfo.sel08, upInfo.sendresult, ConvDateTimeData(upInfo.update_datetime));
                    string sValues = String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}",
                                                    ConvCharData(upInfo.id), ConvCharData(upInfo.name), ConvCharData(upInfo.txt01),
                                                    ConvCharData(upInfo.txt02), ConvCharData(upInfo.sel01), ConvCharData(upInfo.sel02), ConvCharData(upInfo.sel03), ConvCharData(upInfo.sel04),
                                                    ConvCharData(upInfo.sel05), ConvCharData(upInfo.sel06), ConvCharData(upInfo.sel07), ConvCharData(upInfo.sel08), ConvCharData(upInfo.sendresult), ConvDateTimeData(upInfo.update_datetime), ConvCharData(upInfo.sid));

                    //string sInsert = String.Format("INSERT INTO {0} (id, name, num01, txt01, sel01, sel02, sel03, sel04, sel05, sel06, sel07, sel08, sendresult, update_datetime) VALUES ( {1} )", sTable, sValues);
                    string sInsert = String.Format("INSERT INTO {0} (id, name, txt01, txt02, sel01, sel02, sel03, sel04, sel05, sel06, sel07, sel08, sendresult, update_datetime, sid) VALUES ( {1} )", sTable, sValues);

                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "UpdatePersonSendLog", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        // person_send_logテーブルの削除
        public void DeletePersonSendLog(string sId)
        {
            try
            {
                //using (MySqlCommand updCom = m_connection.CreateCommand())
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sTable = "person_send_log";
                    string sWhere = string.Format("id = {0}", ConvCharData(sId));
                    string sInsert = String.Format("DELETE FROM {0} WHERE {1}", sTable, sWhere);

                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "UpdatePersonSendLog", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        // person_send_logテーブルの全削除
        public void rDeleteAllPersonSendLog()
        {
            try
            {
                //using (MySqlCommand updCom = m_connection.CreateCommand())
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sTable = "person_send_log";

                    string sInsert = String.Format("DELETE FROM {0}", sTable);

                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "DeleteAllPersonSendLog", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        /// <summary>
        /// person_send_logから指定したsidのデータをすべて削除する
        /// </summary>
        /// <param name="targetSid"></param>
        public void DeleteShelterPersonSendLog(TerminalInfo selInfo)
        {
            try
            {
                //using (MySqlCommand updCom = m_connection.CreateCommand())
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sTable = "person_send_log";
                    string sWhere = string.Format("sid = {0}", ConvCharData(selInfo.sid));

                    string sInsert = String.Format("DELETE FROM {0} WHERE {1}", sTable, sWhere);

                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "DeleteAllPersonSendLog", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        //2016/04/27 個人送信ログは、選択行のログのみ表示
        // person_send_logテーブルのデータ状態取得
        //        public void GetPersonSendLogList(ref List<PersonSendLog> selInfoList)
        public void GetPersonSendLogList(string id, string sName, string sid, ref List<PersonSendLog> selInfoList)
        {
            selInfoList.Clear();
            try
            {
                // シングルコーテーション置換
                sName = (string)ChkSglQuot(sName);

                //using (MySqlCommand selectCom = m_connection.CreateCommand())
                using (OleDbCommand selectCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    selectCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    // 2016/04/18 救護を削除
                    //string sSelect = "id, name, num01, txt01, sel01, sel02, sel03, sel04, sel05, sel06, sel07, sel08, sendresult, update_datetime";
                    string sSelect = "id, name, num01, txt01, txt02, sel01, sel02, sel03, sel04, sel05, sel06, sel07, sel08, sendresult, update_datetime, sid";
                    string sTable = "person_send_log";
                    string sWhere = string.Format("id = {0} and name = {1} and sid = {2}", ConvCharData(id), ConvCharData(sName), ConvCharData(sid));

                    selectCom.CommandText = String.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY update_datetime DESC", sSelect, sTable, sWhere);

                    // 指定した SQL コマンドを実行して SqlDataReader を構築する
                    //using (MySqlDataReader cReader = ExecuteReader(selectCom))
                    using (OleDbDataReader cReader = ExecuteReader(selectCom))
                    {
                        // 次のレコードに進める (次のレコードがない場合は false になるため実行されない)
                        while (cReader.Read())
                        {
                            PersonSendLog selInfo = new PersonSendLog();
                            selInfo.init();

                            int nColIdx = 0;
                            // 読み込んだデータを元にデータを設定
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.id = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.name = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.num01 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.txt01 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.txt02 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel01 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel02 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel03 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel04 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel05 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel06 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel07 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sel08 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sendresult = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.update_datetime = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sid = cReader[nColIdx].ToString();
                            }
                            nColIdx++;

                            // データ有り
                            selInfoList.Add(selInfo);
                        }

                        // cReader を破棄する
                        cReader.Dispose();
                    }

                    // selectCom を破棄する (正しくは オブジェクトの破棄を保証する を参照)
                    selectCom.Dispose();
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "GetPersonSendLogList", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        // total_send_logテーブルの更新
        public void InsertTotalSendLog(TotalSendLog upInfo)
        {
            try
            {
                // シングルコーテーション置換
                upInfo = (TotalSendLog)ChkSglQuot(upInfo);

                //using (MySqlCommand updCom = m_connection.CreateCommand())
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // 更新日時の設定
                    upInfo.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sTable = "total_send_log";
                    string sValues = String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}",
                                                    upInfo.num01, upInfo.num02, upInfo.num03, upInfo.num04, upInfo.num05, upInfo.num06,
                                                    upInfo.num07, upInfo.num08, upInfo.num09, upInfo.num10, upInfo.num11, upInfo.num12, ConvCharData(upInfo.txt01),
                                                    upInfo.sendresult, ConvDateTimeData(upInfo.update_datetime), ConvCharData(upInfo.sid));

                    string sInsert = String.Format("INSERT INTO {0} (num01, num02, num03, num04, num05, num06, num07, num08, num09, num10, num11, num12, txt01, sendresult, update_datetime, sid) VALUES ( {1} )", sTable, sValues);

                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "UpdateTotalSendLog", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        // total_send_logテーブルの削除
        public void DeleteTotalSendLog(string sUpdateDatetime)
        {
            try
            {
                //using (MySqlCommand updCom = m_connection.CreateCommand())
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sTable = "total_send_log";
                    string sWhere = string.Format("update_datetime = {0}", ConvDateTimeData(sUpdateDatetime));

                    string sInsert = String.Format("DELETE FROM {0} WHERE {1}", sTable, sWhere);

                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "UpdateTotalSendLog", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        // total_send_logテーブルの全削除
        public void DeleteAllTotalSendLog()
        {
            try
            {
                //using (MySqlCommand updCom = m_connection.CreateCommand())
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sTable = "total_send_log";

                    string sInsert = String.Format("DELETE FROM {0}", sTable);

                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "DeleteAllTotalSendLog", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        /// <summary>
        /// total_send_logからsidが指定したデータをすべて削除
        /// </summary>
        /// <param name="targetSid"></param>
        public void DeleteShelterTotalSendLog(TerminalInfo selInfo)
        {
            try
            {
                //using (MySqlCommand updCom = m_connection.CreateCommand())
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sTable = "total_send_log";
                    string sWhere = string.Format("sid = {0}", ConvCharData(selInfo.sid));

                    string sInsert = String.Format("DELETE FROM {0} WHERE {1}", sTable, sWhere);

                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "DeleteAllTotalSendLog", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        // total_send_logテーブルのデータ状態取得
        public void GetTotalSendLogList(string sId, ref List<TotalSendLog> selInfoList)
        {
            selInfoList.Clear();
            try
            {
                //using (MySqlCommand selectCom = m_connection.CreateCommand())
                using (OleDbCommand selectCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    selectCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sSelect = "num01, num02, num03, num04, num05, num06, num07, num08, num09, num10, num11, num12, txt01, sendresult, update_datetime, sid";
                    string sTable = "total_send_log";
                    string sWhere = string.Format("sid = {0}", ConvCharData(sId));
                    selectCom.CommandText = String.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY update_datetime DESC", sSelect, sTable, sWhere);

                    // 指定した SQL コマンドを実行して SqlDataReader を構築する
                    //using (MySqlDataReader cReader = ExecuteReader(selectCom))
                    using (OleDbDataReader cReader = ExecuteReader(selectCom))
                    {
                        // 次のレコードに進める (次のレコードがない場合は false になるため実行されない)
                        while (cReader.Read())
                        {
                            TotalSendLog selInfo = new TotalSendLog();
                            selInfo.init();

                            int nColIdx = 0;
                            // 読み込んだデータを元にデータを設定
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.num01 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.num02 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.num03 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.num04 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.num05 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.num06 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.num07 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.num08 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.num09 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.num10 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.num11 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.num12 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.txt01 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sendresult = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.update_datetime = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                selInfo.sid = cReader[nColIdx].ToString();
                            }
                            nColIdx++;

                            // データ有り
                            selInfoList.Add(selInfo);
                        }

                        // cReader を破棄する
                        cReader.Dispose();
                    }

                    // selectCom を破棄する (正しくは オブジェクトの破棄を保証する を参照)
                    selectCom.Dispose();
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "GetTotalSendLogList", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }


        // terminal_infoテーブルの全削除
        public void DeleteAllTerminalInfo()
        {
            try
            {
                //using (MySqlCommand updCom = m_connection.CreateCommand())
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sTable = "terminal_info";

                    string sInsert = String.Format("DELETE FROM {0}", sTable);

                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "DeleteAllPersonInfo", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }

        /// <summary>
        /// terminal_infoテーブルで指定したものの情報をクリアする(開設日、閉鎖日、更新日、開設フラグ)
        /// </summary>
        /// <param name="selInfo"></param>
        public void ClearTerminalInfo(TerminalInfo selInfo)
        {
            try
            {
                // シングルコーテーション置換
                selInfo = (TerminalInfo)ChkSglQuot(selInfo);

                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // 情報クリア操作
                    selInfo.status = "";
                    selInfo.open_datetime = "";
                    selInfo.close_datetime = "";
                    selInfo.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    selInfo.open_flag = FormShelterInfo.SHELTER_STATUS.NO_OPEN;
                    selInfo.lat = "0.0";
                    selInfo.lon = "0.0";
                    selInfo.text_flag = "1";        //2020.03.26 "0" -> "1"
                    selInfo.dummy_num = "";         //2020.03.26 Add
                    selInfo.dummy_num_flag = false; //2020.03.26 Add

                    // SQLコマンド設定
                    string sTable = "terminal_info";

                    string sSet = String.Format("id = {0}, gid = {1}, name = {2}, lat = {3}, lon = {4}, open_flag = {5}, open_datetime = {6}, close_datetime = {7}, update_datetime = {8}, memo_str = {9}, status_str = {10}, smid = {11}, text_flag = {12}, dummy_num = {13},dummy_num_flag = {14}",
                                                    ConvCharData(selInfo.sid), ConvCharData(selInfo.gid), ConvCharData(selInfo.name), selInfo.lat, selInfo.lon, (int)selInfo.open_flag,
                                                    ConvDateTimeData(selInfo.open_datetime), ConvDateTimeData(selInfo.close_datetime), ConvDateTimeData(selInfo.update_datetime),
                                                    ConvCharData(selInfo.memo), ConvCharData(selInfo.status), ConvCharData(selInfo.smid), ConvCharData(selInfo.text_flag),
                                                    ConvCharData(selInfo.dummy_num), selInfo.dummy_num_flag);     //2020.03.26 : Add dummy_num,dummy_num_flag

                    string sWhere = String.Format("id = {0}", ConvCharData(selInfo.sid));

                    string sUpdate = String.Format("UPDATE {0} SET {1} WHERE {2}", sTable, sSet, sWhere);

                    updCom.CommandText = sUpdate;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄

                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "ClearTerminalInfo", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }


        /// <summary>
        /// 指定した避難所情報の削除
        /// </summary>
        /// <param name="sid"></param>
        public void DeleteTerminalInfo(string sId)
        {
            try
            {
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    // SQLコマンド設定
                    string sTable = "terminal_info";
                    string sWhere = string.Format("id = {0}", ConvCharData(sId));
                    string sInsert = String.Format("DELETE FROM {0} WHERE {1}", sTable, sWhere);
                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(updCom);            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "DeleteTerminalInfo", ex.ToString());
                // 例外を投げる
                // throw ex;
            }
        }



        private string ConvDateTimeData(string sData)
        {
            if (sData.Trim() == "")
            {
                return "null";
            }
            else
            {
                //return string.Format("cast('{0}' as datetime)", sData);
                return string.Format("'{0}'", sData);
            }
        }

        public string ConvCharData(string sData)
        {

            if (sData.Trim() == "")
            {
                return "null";
            }
            else
            {
                return string.Format("'{0}'", sData);
            }
        }

        /// <summary>
        /// q_anpi_device_infoに通信端末の情報を取得する
        /// </summary>
        /// <param name="sGid"></param>
        public bool GetRegister_QANPIDeviceInfo(string sGid, ref int lastSmid, ref string updateTime)
        {
            bool find = false;

            try
            {
                using (OleDbCommand insCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    insCom.Transaction = m_transaction;

                    // 指定されたQ-ANPIターミナルIDがDBに登録済みかチェック
                    string sSelect = "gid, updatetime";
                    string sTable = "q_anpi_device_info";
                    string sWhere = string.Format("gid = {0}", ConvCharData(sGid));
                    insCom.CommandText = String.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY updatetime DESC", sSelect, sTable, sWhere);

                    string findsGid = "";
                    string findsLastsmid = "0";
                    string findsUpdateTime = "";
                    using (OleDbDataReader cReader = ExecuteReader(insCom))
                    {
                        // DBに存在するデータを取得（存在しない場合は何もせずループを抜ける）
                        while (cReader.Read())
                        {
                            int nColIdx = 0;
                            // 読み込んだデータを元にデータを設定
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                findsGid = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                findsUpdateTime = cReader[nColIdx].ToString();
                            }
                            find = true;
                        }
                    }

                    lastSmid = int.Parse(findsLastsmid);
                    updateTime = findsUpdateTime;

                    insCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "GetRegister_QANPIDeviceInfo", ex.ToString());
            }

            return find;
        }

        /// <summary>
        /// q_anpi_device_infoに通信端末の情報を登録/更新する
        /// </summary>
        /// <param name="sGid"></param>
        /// <param name="lastSmid"></param>
        public void UpsertQANPIDeviceInfo(string sGid, int lastSmid)
        {
            try
            {
                using (OleDbCommand insCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    insCom.Transaction = m_transaction;

                    // 指定されたQ-ANPIターミナルIDがDBに登録済みかチェック
                    string sSelect = "gid, updatetime";
                    string sTable = "q_anpi_device_info";
                    string sWhere = string.Format("gid = {0}", ConvCharData(sGid));
                    insCom.CommandText = String.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY updatetime DESC", sSelect, sTable, sWhere);

                    bool findGid = false;
                    string findsGid = "";
                    string findsUpdateTime = "";
                    using (OleDbDataReader cReader = ExecuteReader(insCom))
                    {
                        // DBに存在するデータを取得（存在しない場合は何もせずループを抜ける）
                        while (cReader.Read())
                        {
                            int nColIdx = 0;
                            // 読み込んだデータを元にデータを設定
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                findsGid = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                findsUpdateTime = cReader[nColIdx].ToString();
                            }
                            findGid = true;
                        }
                    }

                    string updateTime;
                    string sValues;
                    string sSqlCommand;
                    // DBにすでに端末IDの登録がある場合、update処理で更新日時を変更
                    if (findGid)
                    {
                        updateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        sValues = String.Format("gid = {0}, updatetime = {1}", ConvCharData(sGid), ConvCharData(updateTime));
                        sWhere = String.Format("gid = {0}", ConvCharData(sGid));
                        sSqlCommand = String.Format("UPDATE {0} SET {1} WHERE {2}", sTable, sValues, sWhere);
                    }
                    // DBに端末IDの登録がない場合、insert処理でデータを追加
                    else
                    {
                        updateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        sValues = String.Format("{0}, {1}", ConvCharData(sGid), ConvCharData(updateTime));
                        sSqlCommand = String.Format("INSERT INTO {0} (gid, updatetime) VALUES ( {1} )", sTable, sValues);
                        lastSmid = 0;
                    }
                    insCom.CommandText = sSqlCommand;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(insCom);            // 実行
                    // デッドロック対応
                    insCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "UpsertQANPIDeviceInfo", ex.ToString());
            }
        }



        /// <summary>
        /// 最近使用した通信端末情報を取得する
        /// </summary>
        /// <param name="sGid"></param>
        /// <param name="lastSmid"></param>
        public void GetRecentlyQANPIDeviceInfo(ref string sGid)
        {
            try
            {
                using (OleDbCommand insCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    insCom.Transaction = m_transaction;

                    // 指定されたQ-ANPIターミナルIDがDBに登録済みかチェック
                    string sSelect = "gid, updatetime";
                    string sTable = "q_anpi_device_info";
                    insCom.CommandText = String.Format("SELECT {0} FROM {1} ORDER BY updatetime DESC", sSelect, sTable);

                    List<List<string>> deviceData = new List<List<string>>();
                    using (OleDbDataReader cReader = ExecuteReader(insCom))
                    {
                        // DBに存在するデータを取得（存在しない場合は何もせずループを抜ける）
                        while (cReader.Read())
                        {
                            List<string> tmpData = new List<string>();
                            int nColIdx = 0;
                            // 読み込んだデータを元にデータを設定
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                tmpData.Add(cReader[nColIdx].ToString());
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                tmpData.Add(cReader[nColIdx].ToString());
                            }
                            deviceData.Add(tmpData);
                        }
                    }

                    // updateTimeが一番新しいものを取得
                    DateTime recentlyTime = new DateTime();
                    foreach (var item in deviceData)
                    {
                        if (recentlyTime < DateTime.Parse(item[1]))
                        {
                            recentlyTime = DateTime.Parse(item[1]);
                            // 最近使用したGIDを取得
                            sGid = item[0];
                        }
                    }

                    insCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "GetRecentlyQANPIDeviceInfo", ex.ToString());
            }
        }

        /// <summary>
        /// 自動送信時刻設定をDBに登録/更新する
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="min"></param>
        /// <param name="enabled"></param>
        public void UpsertAutoSendTime(string id, string type, string month, string day, string hour, string min,
                                        string passedHour, string passedMin, bool enabled, string sendType)
        {
            try
            {
                using (OleDbCommand upsCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    upsCom.Transaction = m_transaction;

                    // 指定されたQ-ANPIターミナルIDがDBに登録済みかチェック
                    string sSelect = "id, type, set_month, set_day, set_hour, set_min, enabled, interval_hour, interval_min, send_type";
                    string sTable = "auto_send_time";
                    string sWhere = string.Format("id = {0}", id);
                    upsCom.CommandText = String.Format("SELECT {0} FROM {1} WHERE {2}", sSelect, sTable, sWhere);

                    bool findId = false;
                    string findsId = "";
                    string findsType = "";
                    string findsMonth = "";
                    string findsDay = "";
                    string findsHour = "";
                    string findsMin = "";
                    string findsEnabled = "";
                    string findsHour2 = "";
                    string findsMin2 = "";
                    string findsSendType = "";
                    using (OleDbDataReader cReader = ExecuteReader(upsCom))
                    {
                        // DBに存在するデータを取得（存在しない場合は何もせずループを抜ける）
                        while (cReader.Read())
                        {
                            int nColIdx = 0;
                            // 読み込んだデータを元にデータを設定
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                findsId = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                findsType = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                findsMonth = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                findsDay = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                findsHour = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                findsMin = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                findsEnabled = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                findsHour2 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                findsMin2 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                findsSendType = cReader[nColIdx].ToString();
                            }
                            findId = true;
                        }
                    }

                    string sValues;
                    string sSqlCommand;

                    string regMonth = "";
                    string regDay = "";
                    string regHour = "";
                    string regMin = "";
                    string regHour2 = "";
                    string regMin2 = "";

                    if (month == "-")
                    {
                        regMonth = "";
                    }
                    else
                    {
                        regMonth = month;
                    }
                    if (day == "-")
                    {
                        regDay = "";
                    }
                    else
                    {
                        regDay = day;
                    }
                    if (hour == "-")
                    {
                        regHour = "";
                    }
                    else
                    {
                        regHour = hour;
                    }
                    if (min == "-")
                    {
                        regMin = "";
                    }
                    else
                    {
                        regMin = min;
                    }
                    if (passedHour == "-")
                    {
                        regHour2 = "";
                    }
                    else
                    {
                        regHour2 = passedHour;
                    }
                    if (passedMin == "-")
                    {
                        regMin2 = "";
                    }
                    else
                    {
                        regMin2 = passedMin;
                    }

                    // DBにすでに端末IDの登録がある場合、update処理で各設定値を更新
                    if (findId)
                    {
                        sValues = String.Format("id = {0}, type = {1}, set_month = {2}, set_day = {3}, set_hour = {4}, set_min = {5}, enabled = {6}, interval_hour = {7}, interval_min = {8}, send_type = {9}",
                                               ConvCharData(id), ConvCharData(type), ConvCharData(regMonth),
                                                ConvCharData(regDay), ConvCharData(regHour), ConvCharData(regMin), enabled, ConvCharData(regHour2), ConvCharData(regMin2), ConvCharData(sendType));
                        sWhere = String.Format("id = {0}", id);
                        sSqlCommand = String.Format("UPDATE {0} SET {1} WHERE {2}", sTable, sValues, sWhere);
                    }
                    // DBに端末IDの登録がない場合、insert処理でデータを追加
                    else
                    {
                        sValues = String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", ConvCharData(id), ConvCharData(type),
                                                ConvCharData(regMonth), ConvCharData(regDay), ConvCharData(regHour), ConvCharData(regMin),
                                                enabled, ConvCharData(regHour2), ConvCharData(regMin2), ConvCharData(sendType));
                        sSqlCommand = String.Format("INSERT INTO {0} (id, type, set_month, set_day, set_hour, set_min, enabled, interval_hour, interval_min, send_type) VALUES ( {1} )", sTable, sValues);
                    }
                    upsCom.CommandText = sSqlCommand;

                    // 作成したSQL文を設定/実行/破棄
                    ExecuteNonQuery(upsCom);            // 実行
                    // デッドロック対応
                    upsCom.Dispose();                   // 破棄
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "UpsertAutoSendTime", ex.ToString());
            }
        }

        /// <summary>
        /// 自動送信時刻テーブルの取得
        /// </summary>
        /// <param name="autoSendTimeInfo"></param>
        public void GetAutoSendTimeAll(out List<AutoSendTime> autoSendTimeInfo)
        {
            autoSendTimeInfo = new List<AutoSendTime>();
            try
            {
                using (OleDbCommand selCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    selCom.Transaction = m_transaction;

                    // 指定されたQ-ANPIターミナルIDがDBに登録済みかチェック
                    string sSelect = "id, type, set_month, set_day, set_hour, set_min, enabled, interval_hour, interval_min, send_type";
                    string sTable = "auto_send_time";
                    selCom.CommandText = String.Format("SELECT {0} FROM {1}", sSelect, sTable);

                    List<List<string>> deviceData = new List<List<string>>();
                    using (OleDbDataReader cReader = ExecuteReader(selCom))
                    {
                        // DBに存在するデータを取得（存在しない場合は何もせずループを抜ける）
                        while (cReader.Read())
                        {
                            AutoSendTime tmpData = new AutoSendTime();
                            tmpData.init();

                            int nColIdx = 0;
                            // 読み込んだデータを元にデータを設定
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                tmpData.id = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                tmpData.type = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                tmpData.month = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                tmpData.day = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                tmpData.hour = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                tmpData.min = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                tmpData.enabled = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                tmpData.hour2 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                tmpData.min2 = cReader[nColIdx].ToString();
                            }
                            nColIdx++;
                            if (!cReader.IsDBNull(nColIdx))
                            {
                                tmpData.send_type = cReader[nColIdx].ToString();
                            }
                            autoSendTimeInfo.Add(tmpData);
                        }
                    }
                    // 破棄
                    selCom.Dispose();
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog(m_strBlockName, "GetAutoSendTimeAll", ex.ToString());
            }
        }

        /// <summary>
        /// 各DB格納変数のシングルコーテーション置換処理
        /// </summary>
        /// <param name="chkInfo"></param>
        /// <returns></returns>
        /// <remarks>
        /// DB操作時、シングルコーテーションがデータ内に存在すると、その部分でSQL文が途切れてしまい、DB操作が失敗してしまう
        /// これを回避するために、データ内にシングルコーテーションが含まれる場合はシングルコーテーション2個に置き換えを行う
        /// </remarks>
        private object ChkSglQuot(object chkInfo)
        {
            // string型
            if (chkInfo is string)
            {
                string tmpInfo = (string)chkInfo;
                if (tmpInfo.Contains('\''))
                {
                    tmpInfo = tmpInfo.Replace("\'", "\'\'");
                }
                return tmpInfo;
            }
            // TerminalInfo型
            else if (chkInfo is TerminalInfo)
            {
                TerminalInfo tmpInfo = (TerminalInfo)chkInfo;
                if (tmpInfo.memo.Contains('\''))
                {
                    tmpInfo.memo = tmpInfo.memo.Replace("\'", "\'\'");
                }
                if (tmpInfo.name.Contains('\''))
                {
                    tmpInfo.name = tmpInfo.name.Replace("\'", "\'\'");
                }
                if (tmpInfo.status.Contains('\''))
                {
                    tmpInfo.status = tmpInfo.status.Replace("\'", "\'\'");
                }
                return tmpInfo;
            }
            // PersonInfo型
            else if (chkInfo is PersonInfo)
            {
                PersonInfo tmpInfo = (PersonInfo)chkInfo;
                if (tmpInfo.name.Contains('\''))
                {
                    tmpInfo.name = tmpInfo.name.Replace("\'", "\'\'");
                }
                if (tmpInfo.error_reason.Contains('\''))
                {
                    tmpInfo.error_reason = tmpInfo.error_reason.Replace("\'", "\'\'");
                }
                if (tmpInfo.txt01.Contains('\''))
                {
                    tmpInfo.txt01 = tmpInfo.txt01.Replace("\'", "\'\'");
                }
                return tmpInfo;
            }
            // PersonSendLog型
            else if (chkInfo is PersonSendLog)
            {
                PersonSendLog tmpInfo = (PersonSendLog)chkInfo;
                if (tmpInfo.name.Contains('\''))
                {
                    tmpInfo.name = tmpInfo.name.Replace("\'", "\'\'");
                }
                if (tmpInfo.txt01.Contains('\''))
                {
                    tmpInfo.txt01 = tmpInfo.txt01.Replace("\'", "\'\'");
                }
                return tmpInfo;
            }
            // TotalSendLog型
            else if (chkInfo is TotalSendLog)
            {
                TotalSendLog tmpInfo = (TotalSendLog)chkInfo;
                if (tmpInfo.txt01.Contains('\''))
                {
                    tmpInfo.txt01 = tmpInfo.txt01.Replace("\'", "\'\'");
                }
                return tmpInfo;
            }
            // PersonLog型
            else if (chkInfo is PersonLog)
            {
                PersonLog tmpInfo = (PersonLog)chkInfo;
                if (tmpInfo.name.Contains('\''))
                {
                    tmpInfo.name = tmpInfo.name.Replace("\'", "\'\'");
                }
                if (tmpInfo.txt01.Contains('\''))
                {
                    tmpInfo.txt01 = tmpInfo.txt01.Replace("\'", "\'\'");
                }
                return tmpInfo;
            }
            return chkInfo;
        }
    }
}
