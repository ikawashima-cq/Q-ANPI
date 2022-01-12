/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2017. All rights reserved.
 */
/**
 * @file    SubGHzFwdReceiver.cs
 * @brief   SubGHzFWDメッセージ受信クラス
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
     * @class SubGHzFwdReceiver
     * @brief SubGHzFWDメッセージ受信クラス
     */
    public class SubGHzFwdReceiver 
    {
        private static int connectedPort = 0;

        /**
         * @brief Ｓ帯モニタデータ（Ｓ帯FWDデータ受信要求）
         */
        private MsgSBandFwdGetReq fixedSBandFwdGetReq = new MsgSBandFwdGetReq();

        private MsgSendBase msb = new MsgSendBase();

        public bool m_sysSendRestriction = false;
        public bool m_sysFreqRestriction = false;
        public SystemInfo m_sysInfo = null;

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
        public void rcvFwdDataEvent(object sender, byte[] data)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "TcpFwdThread", "Recv Data : " + data.Length);
            MsgSBandFwdGetRsp msg = new MsgSBandFwdGetRsp();
            msg.encodedData = data;
            if (data.Length == 0)
            {
                // 応答ではなく切断
                disconnect();
                EventFwdResp(this, 10, data, "FWD Disconnect " );
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

            // 周波数送信制限フラグ
            if (m_sysInfo.sysStartFreqId == 15)
            {
                m_sysFreqRestriction = true;
            }
            else
            {
                m_sysFreqRestriction = false;
            }

            // フォームへ通知
            EventFwdResp(this, 10, data, "FWD受信時刻(" + min + ":" + sec + "." + msec + ") msgType:" + msgtype);
        }

        /**
         * @brief Ｓ帯FWDデータ取得要求作成
         */
        private void makeSBandFwdGetReq()
        {
            fixedSBandFwdGetReq = new MsgSBandFwdGetReq();

            // 装置ID
            fixedSBandFwdGetReq.eqId = MsgSBandData.FIXED_EQ_ID; ;

            fixedSBandFwdGetReq.encode(false);
        }

        /**
         * @brief 要求データを送信する
         */
        public void sendReq()
        {
            // FWD要求送信
            makeSBandFwdGetReq();
            byte[] msgData = fixedSBandFwdGetReq.encodedData;

            if (msgData == null)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHzFwdReceiver", "sendReq : msgData null");
                return;
            }
            try
            {
                MsgSubGHz sndData = new MsgSubGHz();
                sndData.MsgId = 0x11;
                sndData.MsgNo = Program.m_SubGHz.GetMsgNo(sndData.MsgId);

                string dsts = Program.m_SubGHzConfig.Dst_ID;
                if (dsts.Length < 8)
                {
                    dsts = dsts.PadLeft(8, '0');
                }
                for (int i = 0; i < 4; i++)
                {
                    string hex = dsts.Substring(i * 2, 2);
                    byte aa = Convert.ToByte(hex, 16);
                    sndData.DstID[i] = aa;
                }

                sndData.SrcID = null;

                //byte[] tmpbyte = dat80;
                int size = msgData.Length;
                byte[] tmpbyte = new byte[size + 2];
                tmpbyte[0] = 0x90; // port
                tmpbyte[1] = 0x00; // reserved
                Array.Copy(msgData, 0, tmpbyte, 2, size);
                sndData.Parameter = tmpbyte;

                sndData.encode();

                Program.m_SubGHz.Send(sndData.encodedData);

            }
            catch (Exception ex)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHzEquStat:sendRequestData", "送信エラー :" + ex.Message);
            }
        }

        /**
         * @brief 接続処理(文字列指定)
         * @param IPアドレス
         * @param ポート番号
         */
        public FwdReceiver.Result connect(string ipAddr, int port)
        {
            connectedPort = 3090;
            return FwdReceiver.Result.OK;
        }

        /**
         * @brief 切断処理
         */
        public void disconnect()
        {
            if (connectedPort == 0) return;
            connectedPort = 0;
        }


    }
}

