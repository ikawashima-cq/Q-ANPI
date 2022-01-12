/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2017. All rights reserved.
 */
/**
 * @file    TcpFwdReceiver.cs
 * @brief   TcpFWDメッセージ受信クラス
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
     * @class TcpFwdReceiver
     * @brief TcpFWDメッセージ受信クラス
     */
    public class TcpFwdReceiver 
    {
        private static byte[] connectedIp = { 0, 0, 0, 0 };
        private static int connectedPort = 0;

        /**
         * @brief Ｓ帯モニタデータ（Ｓ帯FWDデータ受信要求）
         */
        private MsgSBandFwdGetReq fixedSBandFwdGetReq = new MsgSBandFwdGetReq();

        private MsgSendBase msb = new MsgSendBase();

        public SystemInfo m_sysInfo = null;

        public bool m_sysSendRestriction = false;
        public bool m_sysFreqRestriction = false;

        /**
         * @brief FWD応答デリゲート
         */
        public delegate void EventFwdDelegate(object sender, int code, byte[] msg, string outVal);
        public event FwdReceiver.EventFwdDelegate EventFwdResp;

        public bool isConnected()
        {
            if (connectedPort > 0)
            {
                return true;
            }

            return false;
        }

        /**
         * @brief 受け取っているFWDデータのシステム情報はアクセス制限ありかなしか
         */
        public bool isSendRestrict()
        {
            return m_sysSendRestriction;
        }

        /**
         * @brief 受け取っているFWDデータのシステム情報は周波数制限ありかなしか
         */
        public bool isFreqRestrict()
        {
            return m_sysFreqRestriction;
        }

        /**
         * @brief Ｓ帯FWD受信
         */
        private void rcvFwdDataEvent(object sender, SBandConnectionInfo.SBandDataEvnt e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "TcpFwdReceiver", "Recv Data : " + e.data.Length);
            MsgSBandFwdGetRsp msg = new MsgSBandFwdGetRsp();
            msg.encodedData = e.data;
            if (e.data.Length == 0)
            {
                // 応答ではなく切断
                disconnect();
                EventFwdResp(this, 10, e.data, "FWD Disconnect " );
                return;

            }

            msg.decode(false);

            int hour = msg.fwdRcvInfo.rcvTime.hour;
            int min = msg.fwdRcvInfo.rcvTime.min;
            int sec = msg.fwdRcvInfo.rcvTime.sec;
            int msec = msg.fwdRcvInfo.rcvTime.msec;

            // 応答コード
            int msgtype = msg.fwdRcvInfo.fwdDataMsgType;// test.msgType % 1000;

            // システム情報(136bit)デコード
            m_sysInfo = msg.decodeSystemInfo(msg.fwdRcvInfo.fwdData, 8);

            // メッセージ送信制限フラグ
            if (m_sysInfo.sysSendRestriction == 0)
            {
                m_sysSendRestriction = false;
            }
            else
            {
                m_sysSendRestriction = true;
            }

            // 周波数制限フラグ
            if (m_sysInfo.sysStartFreqId == 15)
            {
                m_sysFreqRestriction = true;
            }
            else
            {
                m_sysFreqRestriction = false;
            }

            // フォームへ通知
            EventFwdResp(this, 10, e.data, "FWD受信時刻(" + min + ":" + sec + "." + msec + ") msgType:" + msgtype);
        }

        /**
         * @brief Ｓ帯FWDデータ取得要求作成
         */
        private void makeSBandFwdGetReq(byte[] data11)
        {
            fixedSBandFwdGetReq = new MsgSBandFwdGetReq();

            // 装置ID
            fixedSBandFwdGetReq.eqId = MsgSBandData.FIXED_EQ_ID; ;

            fixedSBandFwdGetReq.encode(false);
        }

        /**
         * @brief 接続処理(文字列指定)
         * @param IPアドレス
         * @param ポート番号
         */
        public FwdReceiver.Result connect(string ipAddr, int port)
        {
            string[] strIpAddr;
            byte[] byteIpAddr = { 0, 0, 0, 0 };
            char[] splt = { '.' };

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
                return FwdReceiver.Result.NG;
            }
            return connect(byteIpAddr, port);
        }

        /**
         * @brief 接続処理(Byte配列指定)
         */
        private FwdReceiver.Result connect(byte[] ipBytes, int port)
        {
            FwdReceiver.Result bRet = FwdReceiver.Result.OK;
            byte[] bRequest = { 0 };
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "TcpFwdReceiver", "connect : " + port);

            try
            {
                SBandComCtl sbcc = SBandComCtl.getInstance();
                FWDDataConnect(ipBytes, port);

                SBandConnectionInfo sbci = sbcc.getFWDDataConnectionInfo();
                sbci.rcvDataEvent += rcvFwdDataEvent;
                // 
                byte[] inData = { 0 };
                makeSBandFwdGetReq(inData);
                byte[] msgData = fixedSBandFwdGetReq.encodedData;

                if (msgData != null)
                {
                    // 送信
                    msb.sendFwdMsg(msgData);

                }
            }
            catch (Exception ex)
            {
                bRet = FwdReceiver.Result.NG;

                string sLog = String.Format("ERR CODE {0}, Err {1} ", 0, ex.Message);
                Program.m_thLog.PutErrorLog("TcpFwdReceiver", "Connect Faild.", sLog);
            }

            return bRet;
        }

        /**
         * @brief 切断処理
         */
        public void disconnect()
        {
            if (connectedPort == 0) return;
            FWDDataDisconnect(connectedIp, connectedPort);
            connectedPort = 0;
        }

        /**
         * @brief FWDデータ接続
         * @param 接続先IP
         * @param 接続先ポート
         */
        private void FWDDataConnect(byte[] ipBytes, int port)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            try
            {
                SBandComCtl.getInstance().connect(ipBytes, port, CommonConst.SystemNumber.SYSTEM1);

            }
            catch (Exception ex)
            {
                throw (ex);

            }
            connectedIp = ipBytes;
            connectedPort = port;
        }

        /**
         * @brief FWDデータ切断
         * @param 接続中IP
         * @param 接続時に指定したポート
         */
        private void FWDDataDisconnect(byte[] ipBytes, int port)
        {
            SBandComCtl.getInstance().disconnect(ipBytes, port);
        }
    }
}

