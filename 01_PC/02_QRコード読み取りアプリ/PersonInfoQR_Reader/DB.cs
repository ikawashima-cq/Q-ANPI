using System;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Data.OleDb;
using System.Collections.Generic;


namespace EncryptedQRCodeReader
{
    public class DB
    {
        private const string TEMP_SHELTER_ID = "XXXYYYZZZ";

        private OleDbConnection m_DBConnection;

        // メッセージボックス出力用文字列
        private string m_DBErrorTitle = "DBアクセスエラー";

        public enum DBAccessRet : int
        {
            DB_OK = 0,
            DB_NG = 1,             // アクセス失敗
            DB_ALREADY = 2,         // 重複データ存在
            DB_ERR = 3              // DB処理エラー
        }

        public struct COLUMNINFO
        {
            public string sName;
            public int nUnique;
            public int nSingleQuote;
        }

        // iniファイルを読み込む際の情報
        private string m_Sec_DBInfo = "DBInfo";
        private string m_Key_DBProv = "DBProvider";
        private string m_Key_DataSrc = "DataSource";
        private string m_Key_DivCount = "DivCount";
        private string m_Key_ColumnList = "ColumnList";

        private string m_Sec_DebugInfo = "DebugInfo";
        private string m_Key_DebugMode = "DebugMode";


        // DBアクセス用情報
        private string m_DBProvider;
        private string m_DataSource;
        private string m_DBPass;
        private int m_DivCount;
        private List<KeyValuePair<int, COLUMNINFO>> m_ColumnInfo;

        private int m_DebugMode;


        // 初期化
        public bool Init(IniFile iniFile, string sDecode)
        {
            // パスワードの設定
            m_DBPass = sDecode;

            // デバッグ情報(取得できなくても問題ない)
            m_DebugMode = iniFile.GetInteger(m_Sec_DebugInfo, m_Key_DebugMode);

            // DBプロバイダーの取得
            m_DBProvider = iniFile.GetString(m_Sec_DBInfo, m_Key_DBProv);
            if (m_DBProvider.Length == 0)
            {
                iniFile.MsgBox(m_Sec_DBInfo, m_Key_DBProv);
                return false;
            }

            // DataSourceの取得
            m_DataSource = iniFile.GetString(m_Sec_DBInfo, m_Key_DataSrc);
            if (m_DataSource.Length == 0)
            {
                iniFile.MsgBox(m_Sec_DBInfo, m_Key_DataSrc);
                return false;
            }

            // カンマ数の取得
            m_DivCount = iniFile.GetInteger(m_Sec_DBInfo, m_Key_DivCount);
            if (m_DivCount == 0)
            {
                iniFile.MsgBox(m_Sec_DBInfo, m_Key_DivCount);
                return false;
            }

            // ColumnListの情報取得
            m_ColumnInfo = new List<KeyValuePair<int, COLUMNINFO>>();
            string sValue = iniFile.GetString(m_Sec_DBInfo, m_Key_ColumnList);
            if (sValue.Length == 0)
            {
                iniFile.MsgBox(m_Sec_DBInfo, m_Key_ColumnList);
                return false;
            }
            string[] sSplitList = sValue.Split(',');
            if (sSplitList.Length != m_DivCount + 1)
            {
                iniFile.MsgBox(m_Sec_DBInfo, m_Key_ColumnList);
                return false;
            }
            int iIdx = 0;
            int nTotalUnique = 0;
            foreach (string sSplitTmp2 in sSplitList)
            {
                string[] sSplitList2 = sSplitTmp2.Split(';');
                if (sSplitList2.Length != 3)
                {
                    // 3分割できなければ異常
                    iniFile.MsgBox(m_Sec_DBInfo, m_Key_ColumnList);
                    return false;
                }

                string sColumnName = sSplitList2[0];
                int nUnique;
                int nSingleQuote;
                try
                {
                    nUnique = Int32.Parse(sSplitList2[1]);
                    nSingleQuote = Int32.Parse(sSplitList2[2]);
                }
                catch (Exception exc)
                {
                    // 数字から数値への変換に失敗
                    iniFile.MsgBox(m_Sec_DBInfo, m_Key_ColumnList, exc.Message);
                    return false;
                }

                nTotalUnique = nTotalUnique + nUnique;

                COLUMNINFO colInfo = new COLUMNINFO();
                colInfo.sName = sColumnName;
                colInfo.nUnique = nUnique;
                colInfo.nSingleQuote = nSingleQuote;
                KeyValuePair<int, COLUMNINFO> tmpPair =
                    new KeyValuePair<int, COLUMNINFO>(iIdx, colInfo);
                m_ColumnInfo.Add(tmpPair);
                iIdx++;
            }

            // ユニークキーとなるカラムの数の確認
            if (nTotalUnique == 0)
            {
                // ユニークキーのカラムがないため、重複確認ができない
                // アプリを終了とする
                iniFile.MsgBox(m_Sec_DBInfo, m_Key_ColumnList);
                return false;
            }

            // 初期化のタイミングでDBアクセス可能か確認する
            bool bRet = this.OpenDB();
            if (bRet == false)
            {
                MessageBox.Show("DBオープンに失敗しました。", m_DBErrorTitle);
                return false;
            }
            this.CloseDB();

            return true;
        }

        ~DB()
        {
        }

        private void ExceptionMsg(Exception exc, string sSQL="")
        {
            if (sSQL == "")
            {
                this.ExceptionMsg(exc.Message);
            }
            else
            {
                this.ExceptionMsg(exc.Message + "\n" + sSQL);
            }
        }

        private void ExceptionMsg(string errMsg)
        {
            if (m_DebugMode == 1)
            {
                MessageBox.Show(errMsg, m_DBErrorTitle);
            }
        }

        // DB オープン
        public bool OpenDB()
        {
            bool bRet = true;
            try
            {
                m_DBConnection = new OleDbConnection("Provider = " + m_DBProvider +
                                                 ";Data Source = " + m_DataSource +
                                                 ";Jet OLEDB:Database Password=" + m_DBPass);
                m_DBConnection.Open();
            }
            catch (Exception exc)
            {
                this.ExceptionMsg(exc);
                bRet = false;
                m_DBConnection = null;
            }
            return bRet;
        }

        // DB クローズ
        public void CloseDB()
        {
            if (m_DBConnection != null)
            {
                m_DBConnection.Close();
                m_DBConnection = null;
            }
        }

        public DBAccessRet DBRegister(string sData)
        {
            DBAccessRet ret = DBAccessRet.DB_ERR;

            // カンマによる分割後のデータ数確認
            string[] sColData = sData.Split(',');
            for (int ic = 0; ic < sColData.Length - 1; ic++)
            {
                sColData[ic] = sColData[ic].Replace("\n", "");
                sColData[ic] = sColData[ic].Replace("\r", "");
            }

            // 一時対応：スマホからのデータに避難所内外の項目がある場合とない場合の両パターンに対応
            // →スマホに避難所内外の項目を追加したら[(sColData.Length + 1 != m_ColumnInfo.Count - 1)&&]を削除する
            if (
                (sColData.Length + 1 != m_ColumnInfo.Count - 1)&&
                (sColData.Length + 1 != m_ColumnInfo.Count)
                )
            {
                return DBAccessRet.DB_ERR;
            }

            // DBオープン
            bool bRet = this.OpenDB();
            if (bRet == false)
            {
                return ret;
            }

            // すでにDBに登録されているか確認
            ret = CheckDBExist(sColData, true);

            // 重複したデータは上書き保存する
            {
                string sSQLInsert;

                // 削除フラグがOFFになった同一データが存在するか確認
                ret = CheckDBExist(sColData, false);
                if (ret != DBAccessRet.DB_ALREADY)
                {
                    sSQLInsert = this.CreateInsertSQL(sColData);
                }
                else
                {
                    sSQLInsert = this.CreateUpdateSQL(sColData);
                }
                ret = DBAccessRet.DB_NG;
                
                OleDbTransaction transaction = m_DBConnection.BeginTransaction();
                OleDbCommand DBCommandInsert = new OleDbCommand();
                DBCommandInsert.Connection = m_DBConnection;
                DBCommandInsert.CommandText = sSQLInsert;
                DBCommandInsert.Transaction = transaction;
                try
                {
                    DBCommandInsert.ExecuteNonQuery();
                    ret = DBAccessRet.DB_OK;
                    transaction.Commit();

                    // 履歴DBに書き込み
                    string sSQLInsertLog = this.CreateInsertLogSQL(sColData);
                    OleDbTransaction transactionLog = m_DBConnection.BeginTransaction();
                    OleDbCommand DBCommandInsertLog = new OleDbCommand();
                    DBCommandInsertLog.Connection = m_DBConnection;
                    DBCommandInsertLog.CommandText = sSQLInsertLog;
                    DBCommandInsertLog.Transaction = transactionLog;
                    DBCommandInsertLog.ExecuteNonQuery();
                    transactionLog.Commit();
                }
                catch (OverflowException ex)
                {
                    Console.Write(ex.Message);
                }
                catch (Exception exc)
                {
                    transaction.Rollback();
                    DBCommandInsert = null;
                    this.CloseDB();
                    this.ExceptionMsg(exc, sSQLInsert);
                    return ret;
                }
                DBCommandInsert = null;
            }

            // DBクローズ
            this.CloseDB();
            return ret;
        }

        // 重複確認用SELECT文の作成
        // 起動時のユニークキー数確認により、一つ以上の条件が保障されている
        private string CreateSelectSQL(string[] sSplitData , bool chkDelFlg)
        {
            string sSQLSelect = "";

            string sCondition = "";
            foreach (KeyValuePair<int, COLUMNINFO> pairDat in m_ColumnInfo)
            {
                if (pairDat.Value.nUnique == 0)
                {
                    continue;
                }

                if (sCondition.Length != 0)
                {
                    // where句に "and" を追加する
                    sCondition = sCondition + " and ";
                }
                string sQuote = this.AddSingleQuote(sSplitData[pairDat.Key], pairDat.Value.nSingleQuote);
                sCondition = sCondition + pairDat.Value.sName + "=" + sQuote;
            }

            // sidが仮IDのものがあるかチェック
            sCondition = sCondition + " and " + "sid=" + "'" + TEMP_SHELTER_ID +"'";

            // 削除フラグ確認がONの場合、削除フラグがOFFのものを見つける
            if (chkDelFlg)
            {
                sCondition = sCondition + " and " + "del_flag=0";
            }

            // sCondition には必ず一つ以上の条件が設定されている
            sSQLSelect = "select * from person_info where " + sCondition;
            return sSQLSelect;
        }

        /// <summary>
        /// 個人安否情報DBにデータを新規登録
        /// </summary>
        /// <param name="sSplitData"></param>
        /// <returns></returns>
        private string CreateInsertSQL(string[] sSplitData)
        {
            string sSQLInsert = "";
            string sColumn = "";
            string sValues = "";
            int iIdx = 0;
            for (iIdx = 0; iIdx < sSplitData.Length; iIdx++)
            {
                if (sSplitData[iIdx].Length == 0)
                {
                    continue;
                }
                string sQuote = this.AddSingleQuote(sSplitData[iIdx], m_ColumnInfo[iIdx].Value.nSingleQuote);
                // 生年月日の場合はフォーマットを直す
                if (m_ColumnInfo[iIdx].Value.sName == "txt02")
                {
                    sQuote = sQuote.Substring(0, 5) + "-" + sQuote.Substring(5, 2) + "-" + sQuote.Substring(7, 3);
                }

                // 読み込んだ情報の最後(妊産婦)の値が文字化けしていた場合、値を無しにする
                if (m_ColumnInfo[iIdx].Value.sName == "sel07")
                {
                    if ((sQuote != "0") && (sQuote != "1"))
                    {
                        continue;
                    }
                }

                // PC版のアプリで読み取ったデータはすべて避難所内外が「内」になる
                if (m_ColumnInfo[iIdx].Value.sName == "sel08")
                {
                    sQuote = "0";
                }

                sValues = sValues + sQuote + ",";
                sColumn = sColumn + m_ColumnInfo[iIdx].Value.sName + ",";
            }

            // 一時対応：スマホからのデータに避難所内外の項目がある場合とない場合の両パターンに対応
            // →スマホに避難所内外の項目を追加したら削除する
            if (sSplitData.Length < 12)
            {
                //// 避難所内外のカラム名と値を設定(これはQRコードには乗ってこない。PC版のアプリで読み取ったデータはすべて避難所内外が「内」になる)
                sColumn = sColumn + m_ColumnInfo[iIdx].Value.sName + ",";
                iIdx++;
                sValues = sValues + "'0'" + ",";
                //---
            }

            // 避難所IDに仮IDをつける
            sColumn = sColumn + "sid," ;
            sValues = sValues + "'" + TEMP_SHELTER_ID + "',";

            // 最後のカラム名を追加(現状現在時刻のみ)
            sColumn = sColumn + m_ColumnInfo[iIdx].Value.sName;

            // 現在時刻の取得(これはQRコードには乗ってこない)
            DateTime dt = DateTime.Now;
            sValues = sValues + this.AddSingleQuote(dt.ToString("yyyy/MM/dd HH:mm:ss"), m_ColumnInfo[iIdx].Value.nSingleQuote);

            sSQLInsert = "INSERT INTO person_info (" + sColumn + ")" + "VALUES(" + sValues + ")";

            return sSQLInsert;
        }

        // 入力された文字列をシングルクオートで囲む
        private string AddSingleQuote(string sValue, int nQuote)
        {
            if (nQuote != 0)
            {
                return "'" + sValue + "'";
            }
            else
            {
                // シングルクオート不要
                return sValue;
            }
        }

        /// <summary>
        /// 個人安否情報DBに登録済みのデータを更新(削除フラグがONになったデータの登録用)
        /// </summary>
        /// <param name="sSplitData"></param>
        /// <returns></returns>
        private string CreateUpdateSQL(string[] sSplitData)
        {
            string sSQLInsert = "";
            string sValues = "";
            string valueSet = "";

            string sWhere = string.Format("id = '{0}' and name = '{1}' and sid = '{2}'", sSplitData[0], sSplitData[1], TEMP_SHELTER_ID);

            int iIdx = 0;
            for (iIdx = 0; iIdx < sSplitData.Length; iIdx++)
            {
                if (sSplitData[iIdx].Length == 0)
                {
                    continue;
                }

                string sQuote = this.AddSingleQuote(sSplitData[iIdx], m_ColumnInfo[iIdx].Value.nSingleQuote);
                // 生年月日の場合はフォーマットを直す
                if (m_ColumnInfo[iIdx].Value.sName == "txt02")
                {
                    sQuote = sQuote.Substring(0, 5) + "-" + sQuote.Substring(5, 2) + "-" + sQuote.Substring(7, 3);
                }

                // 読み込んだ情報の最後(妊産婦)の値が文字化けしていた場合、値を無しにする
                if (m_ColumnInfo[iIdx].Value.sName == "sel07")
                {
                    if ((sQuote != "0") && (sQuote != "1"))
                    {
                        continue;
                    }
                }

                // PC版のアプリで読み取ったデータはすべて避難所内外が「内」になる
                if (m_ColumnInfo[iIdx].Value.sName == "sel08")
                {
                    sQuote = "0";
                }

                valueSet = valueSet + m_ColumnInfo[iIdx].Value.sName + " = " + sQuote + ", ";
            }

            // 一時対応：スマホからのデータに避難所内外の項目がある場合とない場合の両パターンに対応
            // →スマホに避難所内外の項目を追加したら削除する
            if (sSplitData.Length < 12)
            {
                // 避難所内外のカラム名と値を設定(これはQRコードには乗ってこない。PC版のアプリで読み取ったデータはすべて避難所内外が「内」になる)
                string sel02 = this.AddSingleQuote(sSplitData[5], m_ColumnInfo[iIdx].Value.nSingleQuote);
                valueSet = valueSet + m_ColumnInfo[iIdx].Value.sName + " = '0', ";
                iIdx++;
                //---
            }

            // 避難所IDに仮IDをセット
            valueSet = valueSet + "sid = " + "'" + TEMP_SHELTER_ID + "',";

            // 現在時刻の取得(これはQRコードには乗ってこない)
            DateTime dt = DateTime.Now;
            sValues = sValues + this.AddSingleQuote(dt.ToString("yyyy/MM/dd HH:mm:ss"), m_ColumnInfo[iIdx].Value.nSingleQuote);
            valueSet = valueSet + m_ColumnInfo[iIdx].Value.sName + " = " + this.AddSingleQuote(dt.ToString("yyyy/MM/dd HH:mm:ss"), m_ColumnInfo[iIdx].Value.nSingleQuote) + ", ";

            // 削除フラグをOFFにする
            valueSet = valueSet + "del_flag = 0";

            // SQL文の生成
            sSQLInsert = "UPDATE person_info SET " + valueSet + " WHERE " + sWhere;

            return sSQLInsert;
        }

        /// <summary>
        /// 個人安否情報履歴DBへの書き込み
        /// </summary>
        /// <param name="sSplitData"></param>
        /// <returns></returns>
        private string CreateInsertLogSQL(string[] sSplitData)
        {
            string sSQLInsert = "";
            string sColumn = "";
            string sValues = "";
            int iIdx = 0;
            for (iIdx = 0; iIdx < sSplitData.Length; iIdx++)
            {
                if (sSplitData[iIdx].Length == 0)
                {
                    continue;
                }
                string sQuote = this.AddSingleQuote(sSplitData[iIdx], m_ColumnInfo[iIdx].Value.nSingleQuote);

                // 生年月日の場合はフォーマットを直す
                if (m_ColumnInfo[iIdx].Value.sName == "txt02")
                {
                    sQuote = sQuote.Substring(0, 5) + "-" + sQuote.Substring(5, 2) + "-" + sQuote.Substring(7, 3);
                }

                // 読み込んだ情報の最後(妊産婦)の値が文字化けしていた場合、値を無しにする
                if (m_ColumnInfo[iIdx].Value.sName == "sel07")
                {
                    if ((sQuote != "0") && (sQuote != "1"))
                    {
                        continue;
                    }
                }

                // PC版のアプリで読み取ったデータはすべて避難所内外が「内」になる
                if (m_ColumnInfo[iIdx].Value.sName == "sel08")
                {
                    sQuote = "0";
                }

                sValues = sValues + sQuote + ",";
                sColumn = sColumn + m_ColumnInfo[iIdx].Value.sName + ",";
            }

            // 一時対応：スマホからのデータに避難所内外の項目がある場合とない場合の両パターンに対応
            if (sSplitData.Length < 12)
            {
                // 避難所内外のカラム名と値を設定(これはQRコードには乗ってこない。PC版のアプリで読み取ったデータはすべて避難所内外が「内」になる)
                sColumn = sColumn + m_ColumnInfo[iIdx].Value.sName + ",";
                iIdx++;
                string sel02 = this.AddSingleQuote(sSplitData[5], m_ColumnInfo[iIdx].Value.nSingleQuote);
                sValues = sValues + "'0'" + ",";
                //---
            }

            // 避難所IDに仮IDをつける
            sColumn = sColumn + "sid,";
            sValues = sValues + "'" + TEMP_SHELTER_ID + "',";

            // 最後のカラム名を追加(現状現在時刻のみ)
            sColumn = sColumn + m_ColumnInfo[iIdx].Value.sName;

            // 現在時刻の取得(これはQRコードには乗ってこない)
            DateTime dt = DateTime.Now;
            sValues = sValues + this.AddSingleQuote(dt.ToString("yyyy/MM/dd HH:mm:ss"), m_ColumnInfo[iIdx].Value.nSingleQuote);

            sSQLInsert = "INSERT INTO person_log (" + sColumn + ")" + "VALUES(" + sValues + ")";

            return sSQLInsert;
        }


        /// <summary>
        /// データがDBに登録済みか確認する
        /// </summary>
        /// <param name="sColData">確認するデータ</param>
        /// <param name="chkDelFlg">削除フラグを含めるか</param>
        /// <returns></returns>
        private DBAccessRet CheckDBExist(string[] sColData , bool chkDelFlg)
        {
            DBAccessRet ret = DBAccessRet.DB_NG;

            // すでにDBに登録されているか確認
            string sSQLSelect = this.CreateSelectSQL(sColData, chkDelFlg);
            OleDbCommand DBCommandSelect = new OleDbCommand();
            DBCommandSelect.Connection = m_DBConnection;
            DBCommandSelect.CommandText = sSQLSelect;
            OleDbDataReader dataReader;
            try
            {
                dataReader = DBCommandSelect.ExecuteReader();
            }
            catch (Exception exc)
            {
                DBCommandSelect = null;
                this.CloseDB();
                this.ExceptionMsg(exc, sSQLSelect);
                return ret;
            }
            while (dataReader.Read())
            {
                // 条件に合致したデータが一レコード以上存在した場合
                ret = DBAccessRet.DB_ALREADY;
                break;
            }
            dataReader.Close();
            DBCommandSelect = null;

            return ret;
        }

    }
}
