/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2017. All rights reserved.
 */
/**
 * @file    DbAccessStep2.cs
 * @brief   STEP2開発用のDbAccess派生クラス
 */
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Data.SqlClient;

using QAnpiCtrlLib.log;
using QAnpiCtrlLib.consts;
using QAnpiCtrlLib.msg;

using ShelterInfoSystem.control;
namespace ShelterInfoSystem
{
    /**
     * @class DbAccessStep2
     * @brief STEP2開発用のDbAccess派生クラス
     */
    public class DbAccessStep2 : DbAccess
    {
        private const int ID_MAX = 99;

        /// <summary>
        /// 同報配信CID値
        /// </summary>
        private const int TYPE130_CID_DOHO = 0x1FFFFFF;
        /// <summary>
        /// 同報フラグ - 個別
        /// </summary>
        private const int TYPE130_FLG_KOBETSU = 0;
        /// <summary>
        /// 同報フラグ - 同報
        /// </summary>
        private const int TYPE130_FLG_DOHO = 1;
        /// <summary>
        /// 救助支援情報 メッセージ種類 - 個別 
        /// </summary>
        private const string TYPE130_STR_KOBETSU = "個別";
        /// <summary>
        /// 救助支援情報 メッセージ種類 - 同報 
        /// </summary>
        private const string TYPE130_STR_DOHO = "同報";
        /// <summary>
        /// 同報配信SMID値
        /// </summary>
        private const int TYPE130_SMID_DOHO = 0;
        /// <summary>
        /// 個別同報配信SMID値
        /// </summary>
        private const int TYPE130_SMID_KOBETSU_DOHO = 0;
        /// <summary>
        /// 救助支援情報 メッセージバイト長
        /// </summary>
        private const int TYPE130_MSG_BYTE_LEN = 120;
        /// <summary>
        /// 救助支援情報初期化値
        /// </summary>
        private const int TYPE130_INT_INIT = -1;

        /**
         * @brief DBアクセスデリゲート
         */
        public delegate void EventDbAccessDelegate(object sender, int code, string id, string outVal);
        public event EventDbAccessDelegate EventDbAdd;
        public delegate void EventReadFlgDelegate(object sender, int code, bool flg, string outVal);
        public event EventReadFlgDelegate EventReadFlg;

        /**
         * @brief 受信情報データ(L1S)
         */
        public struct RecvMsgInfo
        {
            public string id;   //  連番
            public string MT;   // メッセージタイプ 43,44
            public string Rc;   // 予備情報
            public string Dc;   // 予備情報
            public string rdate;        // 受信日時
            public int readflg;      //  既読
            public byte[] bitdata;      // 受信データ

            public void init()
            {
                id = "";
                MT = "";
                Rc = "";
                Dc = "";
                rdate = "";
                readflg = -1;
                bitdata = new byte[32];
            }
        }

        // テーブル作成(L1S)
        private void createTableRecvMsgInfo()
        {
            //MySqlCommand execCom = m_connection.CreateCommand();
            OleDbCommand execCom = m_connection.CreateCommand();
            execCom.Transaction = m_transaction;

            execCom.CommandText
              = "create table RecvMsgInfo"
                + "(id int,"
                + "MT int, "
                + "Rc int, "
                + "Dc int, "
                + "rdate varchar(14), "
                + "readflg int, "
                + "bitdata varchar(64))";
            execCom.ExecuteNonQuery();

            execCom.Dispose();
        }

        /**
         * @brief 災害危機通報(L1S)データ取得
         * @param リストデータ
         */
        public bool GetRecvMsgInfo(ref List<RecvMsgInfo> selInfoList)
        {
            OleDbCommand selectCom = m_connection.CreateCommand();
            selectCom.Transaction = m_transaction;
            string strSql
                = "select ";
            strSql += "id,";
            strSql += "MT,";
            strSql += "Rc,";
            strSql += "Dc,";
            strSql += "rdate,";
            strSql += "readflg,";
            strSql += "bitdata ";
            strSql += " from RecvMsgInfo ";
            strSql += " where MT=43 or MT=44 ";
            strSql += " order by readflg,rdate desc "; // 既読未読, 受信時刻順に取得

            selectCom.CommandText = strSql;

            OleDbDataReader cReader = null;
            try
            {
                cReader = selectCom.ExecuteReader();
            }
            catch (OleDbException ex)
            {
                if (ex.Errors[0].SQLState == "3078")
                {
                    //テーブルがない
                    createTableRecvMsgInfo();
                    return false;
                }
                else
                {
                    // DBエラー
                    selectCom.Dispose();
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "GetRecvMsgInfo", ex.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "GetRecvMsgInfo", ex.Message);
                return false;
            }

            while (cReader.Read())
            {
                int nColIdx = 0;
                RecvMsgInfo info = new RecvMsgInfo();
                info.init();

                if (!cReader.IsDBNull(nColIdx))
                {
                    info.id = cReader[nColIdx].ToString();
                }
                nColIdx++;
                if (!cReader.IsDBNull(nColIdx))
                {
                    info.MT = cReader[nColIdx].ToString();
                }
                nColIdx++;
                if (!cReader.IsDBNull(nColIdx))
                {
                    info.Rc = cReader[nColIdx].ToString();
                }
                nColIdx++;
                if (!cReader.IsDBNull(nColIdx))
                {
                    info.Dc = cReader[nColIdx].ToString();
                }
                nColIdx++;
                if (!cReader.IsDBNull(nColIdx))
                {
                    info.rdate = cReader[nColIdx].ToString();
                }
                nColIdx++;
                if (!cReader.IsDBNull(nColIdx))
                {
                    info.readflg = cReader.GetInt32(nColIdx);
                }
                nColIdx++;
                if (!cReader.IsDBNull(nColIdx))
                {
                    string tmp = cReader[nColIdx].ToString();

                    // バイト文字列をバイトデータへ
                    if (tmp.Length > 63)
                    {
                        for (int i = 0; i < 32; i++)
                        {

                            string test = tmp.Substring(i * 2, 2);
                            info.bitdata[i] = (byte)Convert.ToInt32(test, 16);
                        }
                    }

                }
                selInfoList.Add(info);
            }
            cReader.Dispose();
            selectCom.Dispose();

            return true;
        }

        // 連番となるIDを取得する
        public int GetRecvMsgNextId()
        {
            OleDbCommand selectCom = m_connection.CreateCommand();
            selectCom.Transaction = m_transaction;
            string strSql
                = "select ";
            strSql += "id,";
            strSql += "MT,";
            strSql += "Rc,";
            strSql += "Dc,";
            strSql += "rdate,";
            strSql += "readflg,";
            strSql += "bitdata ";
            strSql += " from RecvMsgInfo ";
            strSql += " where MT=43 or MT=44 ";
            strSql += " order by rdate desc "; // 既読未読, 受信時刻順に取得

            selectCom.CommandText = strSql;

            OleDbDataReader cReader = null;
            int res = 0;
            try
            {
                cReader = selectCom.ExecuteReader();
                cReader.Read();
                if (!cReader.IsDBNull(0))
                {
                    int id = cReader.GetInt32(0);
                    if (id < ID_MAX)
                    {
                        res = id + 1;
                    }
                }
            }
            catch (OleDbException ex)
            {
                if (ex.Errors[0].SQLState == "3078")
                {
                    //テーブルがない
                    createTableRecvMsgInfo();
                    res = 0;
                }
                else
                {
                    // DBエラー
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "GetRecvMsgNextId", ex.Message);
                    res = 0;
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "GetRecvMsgNextId", ex.Message);
                return 0;
            }
            finally
            {
                if (cReader != null)
                {
                    cReader.Dispose();
                }
                selectCom.Dispose();
            }
            return res;
        }

        /**
         * @brief 災害危機通報データ登録
         * @param 登録する災害危機通報データ
         */
        public bool SetRecvMsgInfo(RecvMsgInfo selInfo)
        {
            OleDbCommand selectCom = m_connection.CreateCommand();
            selectCom.Transaction = m_transaction;

            string sSQL = "insert into RecvMsgInfo(";
            sSQL += "id, ";
            sSQL += "MT, ";
            sSQL += "Rc, ";
            sSQL += "Dc, ";
            sSQL += "rdate, ";
            sSQL += "readflg, ";
            sSQL += "bitdata";
            sSQL += ") ";
            sSQL += "values( ";
            sSQL += "" + selInfo.id + ",";
            sSQL += "'" + selInfo.MT + "',";
            sSQL += "'" + selInfo.Rc + "',";
            sSQL += "'" + selInfo.Dc + "',";
            sSQL += "'" + selInfo.rdate + "',";
            sSQL += "'" + selInfo.readflg + "',";
            string hex32 = "";
            for (int i = 0; i < 32; i++)
            {
                if (i >= selInfo.bitdata.Length)
                {
                    hex32 += "00";
                }
                else
                {
                    hex32 += Convert.ToString(selInfo.bitdata[i], 16).PadLeft(2, '0');
                }
            }

            sSQL += "'" + hex32 + "')";
            selectCom.CommandText = sSQL;
            try
            {
                selectCom.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                if (ex.Errors[0].SQLState == "3078")
                {
                    //テーブルがない   
                    createTableRecvMsgInfo();
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "SetRecvMsgInfo", ex.Message);
                    return false;
                }
                else
                {
                    // DBエラー
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "SetRecvMsgInfo", ex.Message);
                    return false;
                }
            }
            selectCom.Dispose();

            // DBアクセスイベント通知
            EventDbAdd(this, 0, selInfo.id, "SetRecvMsgInfo");
            CheckReadFlgMessage();
            return true;
        }

        /**
         * @brief 受信情報データ(救助支援情報(TYPE130))
         */
        public struct RescueMsgInfo
        {
            public string id;        // 連番
            public string MT;        // メッセージタイプ 130
            public int Dt;           // データ種別
            public string rdate;     // 受信日時
            public int readflg;      // 既読
            public int dohoflg;      // 同報
            public int msglength;    // メッセージ長
            public string message;   // 救助支援情報(テキスト)
            public byte[] bitMessage;   // 救助支援情報(バイナリ)
            public int smid;         // 避難所管理ID
            public string gid;       // Q-ANPIターミナルID
            public int orid;         // 利用機関ID
            public int cid;          // 通信ID

            public void init()
            {
                id = "";
                MT = "";
                Dt = TYPE130_INT_INIT;
                rdate = "";
                readflg = TYPE130_INT_INIT;
                dohoflg = TYPE130_INT_INIT;
                msglength = TYPE130_INT_INIT;
                message = "";
                bitMessage = new byte[TYPE130_MSG_BYTE_LEN];
                smid = TYPE130_SMID_KOBETSU_DOHO;
                gid = "";
                orid = TYPE130_INT_INIT;
                cid = TYPE130_INT_INIT;
            }
        }

        /**
         * @brief 災害危機DBへの同一メッセージ確認
         * @return true:登録あり,false:登録なし
         */
        public bool isSetRecvMsgInfo(RecvMsgInfo info)
        {
            // 災害危機情報のリストを取得
            List<DbAccessStep2.RecvMsgInfo> RecvList = new List<DbAccessStep2.RecvMsgInfo>();
            RecvList.Clear();
            if (!GetRecvMsgInfo(ref RecvList))
            {
                return false;
            }

            // 災害危機情報のリストから一致する情報を検索する
            for (int i = 0; i < RecvList.Count; i++)
            {
                if (info.bitdata.SequenceEqual(RecvList[i].bitdata))
                {
                    // DB内のbitdataが一致していたら重複メッセージありと判断
                    return true;
                }
            }
            return false;
        }

        // テーブル作成(救助支援情報(TYPE130))
        private void createTableRescueMsgInfo()
        {
            OleDbCommand execCom = m_connection.CreateCommand();
            execCom.Transaction = m_transaction;

            try
            {
                execCom.CommandText
                  = "create table RescueMsgInfo"
                    + "(id int,"
                    + "MT int, "
                    + "Dt int, "
                    + "rdate varchar(14), "
                    + "readflg int, "
                    + "dohoflg int, "
                    + "msglength int, "
                    //+ "message varchar(300), "
                    + "message varchar(200),"
                    + "bitdata varchar(240))";
                execCom.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "GetRescueMsgInfo", ex.Message);
            }
            execCom.Dispose();
        }

        /**
         * @brief 救助支援情報取得
         * @param リストデータ
         */
        public bool GetRescueMsgInfo(string myGid, int mySmid, ref List<RescueMsgInfo> selInfoList, bool getAllFlg = false)
        {
            OleDbCommand selectCom = m_connection.CreateCommand();
            selectCom.Transaction = m_transaction;
            string strSql
                = "select ";
            strSql += "id,";
            strSql += "MT,";
            strSql += "Dt,";
            strSql += "rdate,";
            strSql += "readflg,";
            strSql += "dohoflg,";
            strSql += "msglength,";
            strSql += "message,";
            strSql += "gid,";
            strSql += "smid,";
            strSql += "bitdata ";
            strSql += " from RescueMsgInfo ";
            if (!getAllFlg)
            {
                strSql += " where (gid='" + myGid + "' and smid=0) or (gid='" + myGid + "' and smid=" + mySmid + ") ";     // GID,SMIDが避難所管理画面で表示中のものを取得する
            }
            strSql += " order by readflg,rdate desc ";         // 既読未読, 受信日時順に取得
            selectCom.CommandText = strSql;

            OleDbDataReader cReader = null;
            try
            {
                cReader = selectCom.ExecuteReader();
            }
            catch (OleDbException ex)
            {
                if (ex.Errors[0].SQLState == "3078")
                {
                    //テーブルがない
                    createTableRescueMsgInfo();
                    return false;
                }
                else
                {
                    // DBエラー
                    selectCom.Dispose();
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "GetRescueMsgInfo", ex.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "GetRescueMsgInfo", ex.Message);
                return false;
            }

            while (cReader.Read())
            {
                int nColIdx = 0;
                RescueMsgInfo info = new RescueMsgInfo();
                info.init();

                if (!cReader.IsDBNull(nColIdx))
                {
                    info.id = cReader[nColIdx].ToString();
                }
                nColIdx++;
                if (!cReader.IsDBNull(nColIdx))
                {
                    info.MT = cReader[nColIdx].ToString();
                }
                nColIdx++;
                if (!cReader.IsDBNull(nColIdx))
                {
                    info.Dt = cReader.GetInt32(nColIdx);
                }
                nColIdx++;
                if (!cReader.IsDBNull(nColIdx))
                {
                    info.rdate = cReader[nColIdx].ToString();
                }
                nColIdx++;
                if (!cReader.IsDBNull(nColIdx))
                {
                    info.readflg = cReader.GetInt32(nColIdx);
                }
                nColIdx++;
                if (!cReader.IsDBNull(nColIdx))
                {
                    info.dohoflg = cReader.GetInt32(nColIdx);
                }
                nColIdx++;
                if (!cReader.IsDBNull(nColIdx))
                {
                    info.msglength = cReader.GetInt32(nColIdx);
                }
                nColIdx++;
                if (!cReader.IsDBNull(nColIdx))
                {
                    info.message = cReader[nColIdx].ToString();
                }
                nColIdx++;
                if (!cReader.IsDBNull(nColIdx))
                {
                    info.gid = cReader[nColIdx].ToString();
                }
                nColIdx++;
                if (!cReader.IsDBNull(nColIdx))
                {
                    info.smid = cReader.GetInt32(nColIdx);
                }
                nColIdx++;
                if (!cReader.IsDBNull(nColIdx))
                {

                    string tmp = cReader[nColIdx].ToString();

                    // バイト文字列をバイトデータへ
                    if (tmp.Length == (TYPE130_MSG_BYTE_LEN * 2))
                    {
                        for (int i = 0; i < TYPE130_MSG_BYTE_LEN; i++)
                        {
                            string test = tmp.Substring(i * 2, 2);
                            info.bitMessage[i] = (byte)Convert.ToInt32(test, 16);
                        }
                    }
                }
                selInfoList.Add(info);
            }
            cReader.Dispose();
            selectCom.Dispose();

            return true;
        }

        /**
         * @brief 救助支援情報データ登録
         * @param 登録する救助支援情報データ
         */
        public bool SetRescueMsgInfo(RescueMsgInfo selInfo)
        {
            OleDbCommand selectCom = m_connection.CreateCommand();
            selectCom.Transaction = m_transaction;

            string sSQL = "insert into RescueMsgInfo(";
            sSQL += "id, ";
            sSQL += "MT, ";
            sSQL += "Dt, ";
            sSQL += "rdate, ";
            sSQL += "readflg, ";
            sSQL += "dohoflg, ";
            sSQL += "msglength, ";
            sSQL += "message, ";
            sSQL += "gid,";
            sSQL += "smid,";
            sSQL += "bitdata";
            sSQL += ") ";
            sSQL += "values( ";
            sSQL += "" + selInfo.id + ",";
            sSQL += "'" + selInfo.MT + "',";
            sSQL += "'" + selInfo.Dt + "',";
            sSQL += "'" + selInfo.rdate + "',";
            sSQL += "'" + selInfo.readflg + "',";
            sSQL += "'" + selInfo.dohoflg + "',";
            sSQL += "'" + selInfo.msglength + "',";
            sSQL += "'" + selInfo.message + "',";
            sSQL += "'" + selInfo.gid + "',";
            sSQL += "'" + selInfo.smid + "',";
            // bitdataを120Byte分入力する
            string bitdata = "";
            for (int i = 0; i < TYPE130_MSG_BYTE_LEN; i++)
            {
                if (i >= selInfo.bitMessage.Length)
                {
                    bitdata += "00";
                }
                else
                {
                    bitdata += Convert.ToString(selInfo.bitMessage[i], 16).PadLeft(2, '0');
                }
            }

            sSQL += "'" + bitdata + "')";
            selectCom.CommandText = sSQL;
            try
            {
                selectCom.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                if (ex.Errors[0].SQLState == "3078")
                {
                    //テーブルがない   
                    createTableRescueMsgInfo();
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "SetRescueMsgInfo", ex.Message);
                    return false;
                }
                else
                {
                    // DBエラー
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "SetRescueMsgInfo", ex.Message);
                    return false;
                }
            }
            selectCom.Dispose();

            // DBアクセスイベント通知
            EventDbAdd(this, 1, selInfo.id, "SetRescueMsgInfo");
            CheckReadFlgMessage();

            return true;
        }


        // Type 130 救助支援情報
        // return type3を返さなければならないID
        public string[] submitType130(byte[] bits, out bool dohoFlg)
        {
            // type (8
            // システム情報 (136
            // 救助支援情報数 (8
            // 救助支援情報 (1088*3
            // Reserved (8
            // CRC (16

            dohoFlg = false;

            int received = 0;
            int unreceived = 0;

            ArrayList data = new ArrayList();

            MsgType130 msg130 = new MsgType130();
            msg130.encodedData = bits;
            msg130.decode(false, false);

            // 救助支援情報
            int infonum = msg130.rescueSupportInfoNum;
            for (int i = 0; i < infonum; i++)
            {
                // ---DB登録用メッセージ変数生成---
                RescueMsgInfo rescueMsgInfo = new RescueMsgInfo();

                //CID 同報配信の場合、最大値（33554431）
                rescueMsgInfo.cid = msg130.rescueSupportInfos[i].cid;

                //利用機関ID
                rescueMsgInfo.orid = msg130.rescueSupportInfos[i].orid;

                //未受信メッセージ数
                int unreceivedMsgNum = msg130.rescueSupportInfos[i].unreceivedMsgNum;

                //救助支援情報ID
                rescueMsgInfo.id = msg130.rescueSupportInfos[i].rescueSupportInfoId.ToString();

                //データ種別 0：バイナリ 1：テキスト
                rescueMsgInfo.Dt = msg130.rescueSupportInfos[i].dataType;

                //メッセージ長
                rescueMsgInfo.msglength = msg130.rescueSupportInfos[i].msgLength;

                //救助支援テキスト(本文)
                rescueMsgInfo.message = msg130.rescueSupportInfos[i].msg;

                //救助支援情報バイナリ
                rescueMsgInfo.bitMessage = msg130.rescueSupportInfos[i].msgbin;

                //避難所管理
                rescueMsgInfo.smid = msg130.rescueSupportInfos[i].smid;

                //メッセージタイプ
                rescueMsgInfo.MT = "130";

                //受付時刻
                String time = "";
                TimeBCD tbcd = msg130.rescueSupportInfos[i].time;
                // 受付時刻の保存
                time = String.Format("{0:D4}", tbcd.year);
                time += String.Format("{0:D2}", tbcd.month);
                time += String.Format("{0:D2}", tbcd.day);
                time += String.Format("{0:D2}", tbcd.hour);
                time += String.Format("{0:D2}", tbcd.minute);
                time += String.Format("{0:D2}", tbcd.second);
                if (time.Length == 14)
                {
                    rescueMsgInfo.rdate = time;
                }
                else
                {
                    // 受付時刻が取得できなかった場合(N/A)
                    rescueMsgInfo.rdate = DateTime.Now.ToString("yyyyMMddHHmmss");
                }

                // 同報/個別判定
                string isDoho = TYPE130_STR_KOBETSU;
                if (rescueMsgInfo.cid == TYPE130_CID_DOHO) // 25bits = 1 + 4*6
                {
                    // 同報
                    rescueMsgInfo.dohoflg = TYPE130_FLG_DOHO;
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "DbAccessStep2", "submitType130", "支援情報（同報）受信");
                    isDoho = TYPE130_STR_DOHO;
                }
                else
                {
                    // 個別
                    isDoho = TYPE130_STR_KOBETSU;
                    rescueMsgInfo.dohoflg = TYPE130_FLG_KOBETSU;
                    if (unreceivedMsgNum.Equals(0))
                    {
                        Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "submitType130", "未受信件数エラーの為、受信キャンセル");
                        Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "submitType130", "ID:" + rescueMsgInfo.id + "　宛先CID:" + rescueMsgInfo.cid + " 宛先SMID:" + rescueMsgInfo.smid +
                                                    " 宛先利用機関ID:" + rescueMsgInfo.orid + " メッセージ登録日:" + rescueMsgInfo.rdate + " 種類:" + isDoho + " メッセージ:" + rescueMsgInfo.message);
                        break;
                    }
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "DbAccessStep2", "submitType130", "支援情報（個別）受信");
                }

                // 未読に設定
                rescueMsgInfo.readflg = 0;

                // GID(Q-ANPIターミナルID)
                rescueMsgInfo.gid = Program.m_EquStat.mQCID;

                // 受信メッセージをログに出力
                {
                    string txtOrBin = "テキスト";
                    if (rescueMsgInfo.Dt != 1) txtOrBin = "バイナリ";
                    Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "DbAccessStep2", "submitType130"
                        , "メッセージ受信[" + i + "] 通信ID(cid):" + rescueMsgInfo.cid
                        + " 宛先利用機関ID(orid):" + rescueMsgInfo.orid
                        + " 未受信メッセージ数:" + unreceivedMsgNum
                        + " 救助支援情報ID:" + rescueMsgInfo.id
                        + " データタイプ(テキスト/バイナリ):" + txtOrBin
                        + " メッセージ受付時刻(tbcd):" + tbcd.year + "/" + tbcd.month + "/" + tbcd.day
                            + " " + tbcd.hour + ":" + tbcd.minute + ":" + tbcd.second
                        + " メッセージ長(msgLength):" + rescueMsgInfo.msglength
                        + " メッセージ内容(msg):" + rescueMsgInfo.message
                        + " bitdata(size):" + rescueMsgInfo.bitMessage.Length
                        + " 避難所管理ID(smid):" + rescueMsgInfo.smid.ToString()
                        + " 種類(個別/同報)：" + isDoho
                        );
                }

                // メッセージ長確認
                if (rescueMsgInfo.msglength > MsgType130.RescueSupportInfo.MSG_LENGTH_MAX)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "DbAccessStep2", "submitType130"
                        , "メッセージ長エラー size:" + rescueMsgInfo.msglength);
                    continue;
                }

                //受付時刻確認
                if (!(unreceivedMsgNum == 0 && rescueMsgInfo.cid != TYPE130_CID_DOHO)
                    && tbcd.checkParam() == EncDecConst.NG)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "submitType130"
                        , "受付時刻エラー tbcd:" + tbcd);
                    continue;
                }

                // メッセージ判定処理 & DB登録用変数生成
                // (通信IDがMAX && (上位)利用機関IDが一致 && 避難所管理IDが0) || 通信IDが一致 && (避難所管理IDが一致 || 避難所管理IDが0)
                List<RescueMsgInfo> rescueMsgInfoList = CreateRegisterMsgInfo(Program.m_EquStat.mCID, Program.m_EquStat.mQCID, rescueMsgInfo);

                // メッセージが受信OKのものだった場合、DBに登録する
                foreach (RescueMsgInfo info in rescueMsgInfoList)
                {
                    // 登録
                    bool chk = SetRescueMsgInfo(info);

                    // DBエラーでセットできなかった場合、未受信としてreceivedに入れない(Type130再送を依頼する)
                    if (!chk)
                    {
                        Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "submitType130", "DBエラーにて未受信");
                        Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "submitType130", "ID:" + info.id + "　宛先CID:" + rescueMsgInfo.cid + " 宛先SMID:" + info.smid +
                                                    " 宛先利用機関ID:" + info.orid + " メッセージ登録日:" + info.rdate + " 種類:" + isDoho + " メッセージ:" + info.message);
                        continue;
                    }

                    // 個別メッセージ受信の場合、未受信メッセージ数を取得(未受信メッセージ数が0出ない場合、再度[新着確認→メッセージ受信]を繰り返す)
                    if (rescueMsgInfo.cid.Equals(TYPE130_CID_DOHO) && rescueMsgInfo.smid.Equals(TYPE130_SMID_DOHO))
                    {
                        // 同報
                        dohoFlg = true;
                    }
                    else
                    {
                        string sdata = Convert.ToString(int.Parse(info.id), 2); // 2進数
                        // 6桁に変換
                        sdata = sdata.PadLeft(6, '0');
                        data.Add(sdata);
                        received++;

                        // 未受信メッセージ数
                        unreceived = unreceivedMsgNum;
                    }
                }
                if (rescueMsgInfoList.Count < 1)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "submitType130", "宛先避難所が存在しない為、受信をキャンセル");
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "submitType130", "ID:" + rescueMsgInfo.id + "　宛先CID:" + rescueMsgInfo.cid + " 宛先SMID:" + rescueMsgInfo.smid +
                                                " 宛先利用機関ID:" + rescueMsgInfo.orid + " メッセージ登録日:" + rescueMsgInfo.rdate + " 種類:" + isDoho + " メッセージ:" + rescueMsgInfo.message);
                }
            }

            // 受信可能メッセージがないなら終了(新着確認を自動送信しない)
            if (received == 0 || data.Count == 0)
            {
                return null;
            }

            // 戻り値として、新着確認で送信するIDを設定する
            string[] ret = new string[data.Count];
            for (int i = 0; i < data.Count; i++)
            {
                ret[i] = (string)data[i];
            }
            return ret;

        }

        /// <summary>
        /// DBに登録する救助支援メッセージのリストを生成する
        /// </summary>
        /// <param name="activeCid"></param>
        /// <param name="rescueMsgInfo"></param>
        /// <param name="infoList"></param>
        private List<RescueMsgInfo> CreateRegisterMsgInfo(int activeCid, string activeGid, RescueMsgInfo _rescueMsgInfo)
        {
            RescueMsgInfo rescueMsgInfo = _rescueMsgInfo;
            List<RescueMsgInfo> rescueMsgInfoList = new List<RescueMsgInfo>();
            List<string> gidList = new List<string>();

            // 受信したメッセージが同報配信の場合
            if (rescueMsgInfo.cid.Equals(TYPE130_CID_DOHO) && rescueMsgInfo.smid.Equals(TYPE130_SMID_DOHO))
            {
                foreach (int orid in Program.m_EquStat.mORGID)
                {
                    // 上位利用機関ID
                    int highOrid = orid - (orid % 10000);
                    // (上位)利用機関IDが一致する場合
                    if (rescueMsgInfo.orid.Equals(orid) || rescueMsgInfo.orid.Equals(highOrid))
                    {
                        List<DbAccess.TerminalInfo> terminalInfoList = Program.m_mainForm.GetAllTerminalInfoList();
                        // 登録されているGIDを全てリストに追加
                        foreach (DbAccess.TerminalInfo terminalInfo in terminalInfoList)
                        {
                            if (!gidList.Contains(terminalInfo.gid)) gidList.Add(terminalInfo.gid);
                        }
                    }
                }
            }
            // 受信したメッセージが個別配信または端末宛同報配信の場合
            else if (rescueMsgInfo.cid.Equals(activeCid))
            {
                // 端末宛同報
                if (rescueMsgInfo.smid.Equals(TYPE130_SMID_KOBETSU_DOHO))
                {
                    if (!gidList.Contains(activeGid)) gidList.Add(activeGid);
                }
                // 個別
                else
                {
                    List<DbAccess.TerminalInfo> terminalInfoList = Program.m_mainForm.GetAllTerminalInfoList();
                    foreach (DbAccess.TerminalInfo terminalInfo in terminalInfoList)
                    {
                        if (activeGid.Equals(terminalInfo.gid) && rescueMsgInfo.smid.Equals(int.Parse(terminalInfo.smid)))
                        {
                            if (!gidList.Contains(activeGid)) gidList.Add(activeGid);
                        }
                    }
                }
            }

            // 登録するGID分ループし、メッセージリストを生成する
            foreach (string gid in gidList)
            {
                rescueMsgInfo.gid = gid;
                rescueMsgInfoList.Add(rescueMsgInfo);
            }

            return rescueMsgInfoList;
        }

        /**
         * @brief 救助支援DBの同一メッセージ確認
         * @return true:登録あり, false:登録なし
         */
        private bool isSetRescueMsgInfo(RescueMsgInfo info)
        {
            // 救助支援情報のリストを取得
            List<DbAccessStep2.RescueMsgInfo> RescueList = new List<DbAccessStep2.RescueMsgInfo>();
            RescueList.Clear();
            if (!GetRescueMsgInfo(Program.m_mainForm.GetActiveTerminalInfo().gid, int.Parse(Program.m_mainForm.GetActiveTerminalInfo().smid), ref RescueList))
            {
                // DBエラーで取得できなかった場合
                return false;
            }

            // 救助支援情報のリストから一致する情報を検索する
            for (int i = 0; i < RescueList.Count; i++)
            {
                if (info.bitMessage.SequenceEqual(RescueList[i].bitMessage) || info.message == RescueList[i].message)
                {
                    if (info.rdate == RescueList[i].rdate
                        && info.dohoflg == RescueList[i].dohoflg
                        && info.orid == RescueList[i].orid
                        && info.gid == RescueList[i].gid
                        && info.smid == RescueList[i].smid)
                    {
                        // DB内の各条件が一致していたら重複メッセージありと判断
                        return true;
                    }
                }
            }
            return false;
        }

        /**
         * @brief 未読/既読フラグの更新
         * @param type:0:災害危機通報、　1:救助支援情報
         * @param infoId:DBテーブルID(string)
         * @param setFlg: 0:未読 1:既読
         */
        public void SetReadFlg(int type, string infoId, string rdate, int setFlg)
        {
            OleDbCommand selectCom = m_connection.CreateCommand();
            selectCom.Transaction = m_transaction;

            // DBコマンド生成
            string sSQL = "UPDATE ";
            if (type == 0)
            {
                sSQL += "RecvMsgInfo ";
            }
            else if (type == 1)
            {
                sSQL += "RescueMsgInfo ";
            }
            else
            {
                // N/A
            }
            sSQL += "SET readflg = " + setFlg.ToString() + " WHERE id = " + infoId + " AND rdate = '" + rdate + "'";

            selectCom.CommandText = sSQL;
            try
            {
                selectCom.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                if (ex.Errors[0].SQLState == "3078")
                {
                    //テーブルがない   
                    createTableRecvMsgInfo();
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "UpdateRecvMsgInfo", ex.Message);
                    return;
                }
                else
                {
                    // DBエラー
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "UpdateRecvMsgInfo", ex.Message);
                    return;
                }
            }
            selectCom.Dispose();
            CheckReadFlgMessage();
        }


        /**
         * @brief:未読メッセージ有無の確認
         * @param:type=0:災害危機通報, 1:救助支援情報
         */
        public void CheckReadFlgMessage()
        {
            // 災害危機通報のリストを取得
            bool readFlg = true;
            List<DbAccessStep2.RecvMsgInfo> RecvList = new List<DbAccessStep2.RecvMsgInfo>();
            RecvList.Clear();
            if (!GetRecvMsgInfo(ref RecvList))
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "CheckReadFlgMessage", "RecvMsgInfo Error");
                return;
            }
            for (int i = 0; i < RecvList.Count; i++)
            {
                // 未読メッセージがあったらtrue
                if (RecvList[i].readflg == 0)
                {
                    readFlg = false;
                    break;
                }
            }
            EventReadFlg(this, 0, readFlg, "CheckReadFlgMessage RecvList");


            // 救助支援情報のリストを取得
            readFlg = true;
            List<DbAccessStep2.RescueMsgInfo> RescueList = new List<DbAccessStep2.RescueMsgInfo>();
            RescueList.Clear();
            if (!GetRescueMsgInfo(Program.m_mainForm.GetActiveTerminalInfo().gid, int.Parse(Program.m_mainForm.GetActiveTerminalInfo().smid), ref RescueList, true))
            {
                // DBエラーにて取得できなかった場合
                return;
            }
            for (int i = 0; i < RescueList.Count; i++)
            {
                // 未読メッセージがあったらtrue
                if (RescueList[i].readflg == 0)
                {
                    readFlg = false;
                    break;
                }
            }
            EventReadFlg(this, 1, readFlg, "CheckReadFlgMessage RescueList");

            return;
        }

        // RescueMsgInfoテーブルの全削除
        public void DeleteAllRescueMsgInfo()
        {
            DeleteAllMsgInfo("RescueMsgInfo");
        }


        // RecvMsgInfoテーブルの全削除
        public void DeleteAllRecvMsgInfo()
        {
            DeleteAllMsgInfo("RecvMsgInfo");
        }


        private void DeleteAllMsgInfo(string sTable)
        {
            try
            {
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    string sInsert = String.Format("DELETE FROM {0}", sTable);

                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    updCom.ExecuteNonQuery();            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (OleDbException ex)
            {
                if (ex.Errors[0].SQLState == "3078")
                {
                    //テーブルがない   
                    createTableRecvMsgInfo();
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "DeleteAllMsgInfo", ex.Message);
                    return;
                }
                else
                {
                    // DBエラー
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "DeleteAllMsgInfo", ex.Message);
                    return;
                }
            }
        }

        // RecvMsgInfoテーブルの削除
        public void DeleteRescueMsgInfo(string delGid, string delSmid)
        {
            DeleteMsgInfo("RescueMsgInfo", delGid, delSmid);
        }

        public void DeleteMsgInfo(string sTable, string delGid, string delSmid)
        {
            try
            {
                using (OleDbCommand updCom = m_connection.CreateCommand())
                {
                    // トランザクション設定
                    updCom.Transaction = m_transaction;

                    string sWhere = string.Format("(gid = {0}) AND (smid = {1})", ConvCharData(delGid), delSmid);
                    string sInsert = String.Format("DELETE FROM {0} WHERE ({1})", sTable, sWhere);

                    updCom.CommandText = sInsert;

                    // 作成したSQL文を設定/実行/破棄
                    updCom.ExecuteNonQuery();            // 実行
                    // デッドロック対応
                    updCom.Dispose();                   // 破棄
                }
            }
            catch (OleDbException ex)
            {
                if (ex.Errors[0].SQLState == "3078")
                {
                    //テーブルがない   
                    createTableRecvMsgInfo();
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "DeleteAllMsgInfo", ex.Message);
                    return;
                }
                else
                {
                    // DBエラー
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "DbAccessStep2", "DeleteAllMsgInfo", ex.Message);
                    return;
                }
            }
        }
    }
}
