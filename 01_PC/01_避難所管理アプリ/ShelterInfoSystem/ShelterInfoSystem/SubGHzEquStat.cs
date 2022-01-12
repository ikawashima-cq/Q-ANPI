/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2017. All rights reserved.
 */
/**
 * @file    SubGHzEquStat.cs
 * @brief   SubGHz監視ステータス通信管理
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
     * @class SubGHzEquStat
     * @brief SubGHz接続による端末監視ステータス通信クラス
     */
    public class SubGHzEquStat 
    {
        /**
         * @brief 監視ステータスデータ送信要求）
         */
        private MsgSBandEquStatReq fixedSBandEquStatReq = new MsgSBandEquStatReq();

        private MsgSendBase msb = new MsgSendBase();

        private int mSendingCount = 0;  // 0:未送信 1:要求送信中 2～:多重要求送信中
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
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHzEquStat", "Not Connected : ");
                EventEquResp(this, 10, new byte[0], " Disconnect(3005)");
                mSendingCount = 0;
                return;
            }

            // 要求送信中だった場合、多重要求送信となる
            if (mSendingCount == 1)
            {
                // 一度目の要求送信重複は許容する
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHzEquStat", "EquStat 3005 NG 1回目 ");
                mSendingCount++;
            }
            else if (mSendingCount > 1)
            {
                // 多重に要求送信をしていた場合、サブギガ通信を切断する
                EventEquResp(this, 10, new byte[0], " No Resp. Disconnect(3005)");
                mSendingCount = 0;
                // サブギガ通信スレッドを取得
                SubGHz m_SubG = SubGHz.GetInstance();
                m_SubG.DisconnectSubGHz();
                return;
            }
            else
            {
                // OK
                mSendingCount = 1;
            }

            try
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHzEquStat", "Send Data : ");
                makeSBandEquStatReq();
                byte[] msgData = fixedSBandEquStatReq.encodedData;

                if (msgData != null)
                {
                    // 送信
                    sendRequestData(msgData, 0x05);
                    //msb.sendMonStatusMsg(msgData, CommonConst.SystemNumber.SYSTEM1);
                }
            }
            catch (Exception ex)
            {
                string sLog = String.Format("ERR CODE {0}, Err {1} ", 10, ex.Message);
                Program.m_thLog.PutErrorLog("SubGHzEquStat", "Post Faild.", sLog);
            }
        }

        /**
         * @brief 要求データを送信する
         */
        void sendRequestData(byte[] dat80, byte port)
        {
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
                int size = dat80.Length;
                byte[] tmpbyte = new byte[size + 1];
                tmpbyte[0] = port;
                Array.Copy(dat80, 0, tmpbyte, 1, size);


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
         * @brief Ｓ帯EquStat送信応答
         */
        public void rcvEquDataEvent(object sender, byte[] data)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHzEquStat", "Recv Data : " + data.Length);
            mSendingCount = 0;

            if (data.Length == 0)
            {
                // disconnect
                EventEquResp(this, 10, data, " Disconnect(3005)" );
                disconnect();
                return;
            }
#if DEBUG
            MsgSBandEquStatRsp pack3005 = new MsgSBandEquStatRsp();
            pack3005.encodedData = data;
            pack3005.decode(false);

            int gps = pack3005.equStatInfo.gpsStat;
            int lan = pack3005.equStatInfo.lanStat;
            int sw = pack3005.equStatInfo.swAlarm;

            //mOpe = test.equStatInfo.opeStat;
            //mVolt = test.equStatInfo.voltStat;

            // 応答コード
            int code = pack3005.eqId;
#endif
            // フォームに通知
            EventEquResp(this, 10, data, "");
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
            connectedPort = 3005;
            return TermInfo.Result.OK;
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
            connectedPort = 3005;

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
            connectedPort = 0;
            mSendingCount = 0;
        }

    }
}

