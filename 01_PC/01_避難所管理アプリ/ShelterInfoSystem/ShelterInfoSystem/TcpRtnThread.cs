/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2017. All rights reserved.
 */
/**
 * @file    TcpRtnThread.cs
 * @brief   TcpRTNメッセージ送信スレッド
 */
using System;
using System.Net;
using System.IO;
using System.Diagnostics;       // Debug.WriteLine用
using System.Threading;
using System.Collections.Generic;

using QAnpiCtrlLib.log;
using QAnpiCtrlLib.consts;
using QAnpiCtrlLib.msg;

using ShelterInfoSystem.control;

namespace ShelterInfoSystem
{
    /**
     * @class: TcpRtnThread
     * @brief: RTNメッセージ送信スレッド
     */
    public class TcpRtnThread 
    {
        public const int RTN_PORT = 3091;

        private static byte[] connectIP = {127,0,0,1};
        private bool connected = false;
        
        /**
         * @brief Ｓ帯モニタデータ（Ｓ帯RTNデータ送信要求）
         */
        private MsgSBandRtnSendReq fixedSBandRtnSendReq = new MsgSBandRtnSendReq();
        private MsgSendBase msb = new MsgSendBase();

        // 送信キュー 
        private readonly Queue<byte[]> m_sendQueue = new Queue<byte[]>();

        // 送信キューの変化を通知してくれるイベント 
        private readonly ManualResetEvent m_notifyEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent m_notifyEventRecv = new ManualResetEvent(false);
        // 送信ロック
        private readonly object m_sendLock = new object(); 

        /**
         * @brief RTN応答デリゲート
         */
        public delegate void EventRtnDelegate(object sender, int code, byte[] msg, string outVal);
        public event RtnThread.EventRtnDelegate EventRtnResp;

        /// <summary>
        /// 時刻同期要求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="outVal"></param>
        public delegate void EventTimeSyncDelegate(object sender, int code, byte[] msg, string outVal);

        /// <summary>
        /// 災害危機通報
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="outVal"></param>
        public delegate void EventDisasterReportDelegate(object sender, int code, byte[] msg, string outVal);


        /// <summary>
        ///　接続チェック
        /// </summary>
        public bool isConnected()
        {
            connected = checkConnection();
            return connected;
        }

        public void setConnected(bool con)
        {
            connected = con;
            return ;
        }

        /// <summary>
        ///　IPセット
        /// </summary>
        public static RtnThread.Result setIP(string ip)
        {
            string[] test;
            byte[] btest = { 0, 0, 0, 0 };
            char[] splt = { '.' };

            try
            {
                test = ip.Split(splt);
                for (int i = 0; i < 4; i++)
                {
                    btest[i] = byte.Parse(test[i]);
                }
                connectIP = btest;
            }
            catch (Exception ex)
            {
                string sLog = String.Format("ERR CODE {0}, Err {1} ", 0, ex.Message);
                return RtnThread.Result.NG;
            }

            return RtnThread.Result.OK;
        }

        /**
         * @brief IPアドレス設定(Byte配列指定)
         * @param IPアドレス
         * @return 結果
         */
        public static RtnThread.Result setIP(byte[] ip)
        {
            connectIP = ip;
            return RtnThread.Result.OK;
        }

        public bool checkConnection()
        {
            SBandComCtl sbcc = SBandComCtl.getInstance();
            SBandConnectionInfo sci = sbcc.getRTNDataConnectionInfo();
            if (sci == null || sci.client == null || sci.client.Connected == false)
            {
                return false;
            }
            return true;
        }

        public void connect()
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "TcpRtnThread", "SendThread", "ReConnect");
            SBandComCtl sbcc = SBandComCtl.getInstance();

            rtnDataConnect(connectIP, RTN_PORT);
            SBandConnectionInfo sbci = sbcc.getRTNDataConnectionInfo();
            sbci.rcvDataEvent += rcvRtnDataEvent;
        }

        public void sendMessage(byte[] msgdata)
        {
            msb.sendRtnMsg(msgdata);
        }

        public void sendEvent(object sender, int code, byte[] msg, string outVal)
        {
            EventRtnResp(sender, code, msg, outVal);
        }

        /**
         * @brief Ｓ帯RTN送信応答
         */
        private void rcvRtnDataEvent(object sender, SBandConnectionInfo.SBandDataEvnt e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "TcpRtnThread", "Recv Data : " + e.data.Length);

            MsgSBandRtnSendRsp test = new MsgSBandRtnSendRsp();
            test.encodedData = e.data;
            if (e.data == null || e.data.Length == 0)
            {
                Program.m_thLog.PutErrorLog("TcpRtnThread", "Send rcvRtnDataEvent. length 0", "");
                return;
            }
            test.decode(false);
            
            int hour = test.rtnSendInfo.sendTime.hour;
            int min = test.rtnSendInfo.sendTime.min;
            int sec = test.rtnSendInfo.sendTime.sec;
            int msec = test.rtnSendInfo.sendTime.msec;

            // 応答コード 8005 8013 など
            int code = test.rspResult;
            string smsg = RtnThread.getRTNMsg(code);

            // メッセージタイプ (type)2 など
            int mt = test.rtnSendInfo.rtnDataMsgType;

            // 
            EventRtnResp(this, code, e.data, 
                "RTN送信時刻(" + min + ":" + sec + "." + msec + ")");
        }

        /**
         * @brief 接続処理
         * @param 接続先IPアドレス
         * @param 接続先ポート
         * @param 制御対象のボタン
         */
        private void connect(byte[] ipBytes, int port)
        {
            try
            {
                SBandComCtl.getInstance().connect(ipBytes, port, CommonConst.SystemNumber.SYSTEM1);
                connected = true;

            }
            catch (Exception ex)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "RTNThread", ex.Message);
                throw (ex);
            }
        }

        /**
         * @brief 切断処理
         * @param 切断するソケットのIP
         * @param 切断するソケットのポート(接続時に指定した物)
         */
        private void disconnect(byte[] ipBytes, int port)
        {
            SBandComCtl.getInstance().disconnect(ipBytes, port);
            connected = false;
        }

        /**
         * @brief RTNデータ接続
         * @param 接続先IP
         * @param 接続先ポート
         */
        private void rtnDataConnect(byte[] ipBytes, int port)
        {
            connect(ipBytes, port);
        }

        /**
         * @brief RTNデータ切断
         * @param 接続中IP
         * @param 接続時に指定したポート
         */
        private void rtnDataDisconnect(byte[] ipBytes, int port)
        {
            disconnect(ipBytes, port);
        }

    }
}

