/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2017. All rights reserved.
 */
/**
 * @file    TcpEquStat.cs
 * @brief   Tcp監視ステータス通信管理
 */
using System;
using System.Net;
using System.IO;
using System.Diagnostics;       // Debug.WriteLine用

using QAnpiCtrlLib.log;
using QAnpiCtrlLib.consts;
using QAnpiCtrlLib.msg;

using ShelterInfoSystem.control;

namespace ShelterInfoSystem
{
    /**
     * @class TcpEquStat
     * @brief TCP接続による端末監視ステータス通信クラス
     */
    public class TcpEquStat 
    {
        /**
         * @brief 監視ステータスデータ送信要求）
         */
        private MsgSBandEquStatReq fixedSBandEquStatReq = new MsgSBandEquStatReq();

        private MsgSendBase msb = new MsgSendBase();

        private byte[] connectedIP = null;
        private int connectedPort = 0;

        // EquStat応答
        public delegate void EventEquDelegate(object sender, int code, byte[] msg, string outVal);
        public event TermInfo.EventEquDelegate EventEquResp;

        /**
         * @brief コネクトしていない場合 : 0
         */
        public bool isConnected()
        {
            if (connectedPort > 0)
            {
                return true;
            }
            return false;
        }

        /**
         * @brief EquStatデータ取得要求(スレッド)
         */
        public void sendReq()
        {           
            if (isConnected() == false)
            {
                // 未接続
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "TcpEquStatThread", "Not Connected : ");
                return;
            }
            try
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "TcpEquStatThread", "Send Data : ");
                makeSBandEquStatReq();
                byte[] msgData = fixedSBandEquStatReq.encodedData;

                if (msgData != null)
                {
                    // 送信
                    msb.sendMonStatusMsg(msgData, CommonConst.SystemNumber.SYSTEM1);
                }
            }
            catch (Exception ex)
            {
                string sLog = String.Format("ERR CODE {0}, Err {1} ", 10, ex.Message);
                Program.m_thLog.PutErrorLog("TcpEquStatThread", "Post Faild.", sLog);
            }
        }

        /**
         * @brief Ｓ帯EquStat送信応答（デリゲート）
         */
        void rcvEquDataEvent(object sender, SBandConnectionInfo.SBandDataEvnt e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "TcpEquStatThread", "Recv Data : " + e.data.Length);
            MsgSBandEquStatRsp pack3005 = new MsgSBandEquStatRsp();
            pack3005.encodedData = e.data;
            if (e.data.Length == 0)
            {
                // disconnect
                EventEquResp(this, 10, e.data, " Disconnect(3005)" );
                disconnect();
                return;
            }

            pack3005.decode(false);

            int gps = pack3005.equStatInfo.gpsStat;
            int lan = pack3005.equStatInfo.lanStat;
            int sw = pack3005.equStatInfo.swAlarm;

            //mOpe = test.equStatInfo.opeStat;
            //mVolt = test.equStatInfo.voltStat;

            // 応答コード
            int code = pack3005.eqId;

            // 
            EventEquResp(this, 10, e.data, "");
        }

        /**
         * @brief Ｓ帯EquStatデータ取得要求作成
         */
        private void makeSBandEquStatReq()
        {
            fixedSBandEquStatReq = new MsgSBandEquStatReq();

            // 装置ID
            fixedSBandEquStatReq.eqId = MsgSBandData.FIXED_EQ_ID; ;

            fixedSBandEquStatReq.encode(false);
        }

        /**
         * @brief 接続処理(IPアドレスstring指定)
         * @param 接続先IPアドレス
         * @param 接続先ポート
         * @return 接続結果
         */
        public TermInfo.Result connect(string ipAddr, int port)
        {
            string[] strIpAddr;
            byte[] byteIpAddr = { 0, 0, 0, 0 };
            char[] splt = {'.'};

            try
            {
                strIpAddr = ipAddr.Split(splt);
                for (int i = 0; i < 4; i++)
                {
                    byteIpAddr[i] = byte.Parse(strIpAddr[i]);
                }
            }
            catch (Exception ex)
            {
                string sLog = String.Format("ERR CODE {0}, Err {1} ", 0, ex.Message);
                return TermInfo.Result.NG;
            }
            return connect(byteIpAddr, port);
        }

        /**
         * @brief 接続処理(IPアドレスbyte配列指定)
         * @param 接続先IPアドレス
         * @param 接続先ポート
         * @return 接続結果
         */
        public TermInfo.Result connect(byte[] ipBytes, int port)
        {
            TermInfo.Result bRet = TermInfo.Result.OK;
            SBandComCtl sbcc = SBandComCtl.getInstance();
            try
            {
                EquStatDataConnect(ipBytes, port);

                SBandConnectionInfo sbci = sbcc.getEquStatusConnectionInfo();
                sbci.rcvDataEvent += rcvEquDataEvent;
            }
            catch (Exception ex)
            {
                bRet = TermInfo.Result.NG;

                string sLog = String.Format("ERR CODE {0}, Err {1} ", 0, ex.Message);
                Program.m_thLog.PutErrorLog("TcpL1sEqu", "Connect Faild.", sLog);
            }

            return bRet;
        }

        /**
         * @brief 切断処理
         */
        public void disconnect()
        {
            if (connectedPort == 0)
            {
                return;
            }
            EquStatDataDisconnect(connectedIP, connectedPort);
        }

        /**
         * @brief EquStatデータ接続
         */
        private void EquStatDataConnect(byte[] ipBytes, int port)
        {
            try
            {
                SBandComCtl.getInstance().connect(ipBytes, port, CommonConst.SystemNumber.SYSTEM1);

            }
            catch (Exception ex)
            {
                LogMng.AplLogDebug(ex.Message);
                LogMng.AplLogDebug(ex.StackTrace);
                throw (ex);

            }
            connectedIP = ipBytes;
            connectedPort = port;
        }

        /**
         * @brief EquStatデータ切断
         */
        private void EquStatDataDisconnect(byte[] ipBytes, int port)
        {
            SBandComCtl.getInstance().disconnect(ipBytes, port);
            connectedPort = 0;
        }
    }
}

