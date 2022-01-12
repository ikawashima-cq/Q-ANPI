/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2017. All rights reserved.
 */
/**
 * @file    SubGHzL1sReceiver.cs
 * @brief   L1S/CIDのサブギガ受信クラス
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
     * @class SubGHzL1sReceiver
     * @brief L1Sのサブギガ受信クラス
     */
    public class SubGHzL1sReceiver 
    {
        /**
         * @brief 結果定義
         */
        public enum Result
        {
            OK = 0,
            NG,
            NOT_OK,
            NOT_NG,
        };

        // 接続情報
        private static byte[] connectedIp = {0,0,0,0};
        private static int connectedPort = 0;

        /// <summary>
        /// L1Sデータ（L1Sデータ送信要求）
        /// </summary>
        private MsgL1sGetReq m_L1sGetReq = new MsgL1sGetReq(L1sReceiver.MSG_TYPE_L1S);
        //private int sendingflg = 0; // 要求送信中（要求送っていいフラグ）

        private MsgSendBase msb = new MsgSendBase();
        private L1sParser m_Parse = new L1sParser();

        /**
         * @brief L1S応答デリゲート
         */
        public delegate void EventL1sDelegate(object sender, int code, byte[] msg, string outVal);
        public event L1sReceiver.EventL1sDelegate EventL1sResp;

        /**
         * @brief 接続処理(文字列指定)
         * @param 接続先IPアドレス
         * @param 接続ポート
         * @return 接続結果
         */
        public L1sReceiver.Result connect(string ipAddr, int port)
        {
            connectedPort = 3610;

            return L1sReceiver.Result.OK;
        }

        /**
         * @brief 接続処理(Byte配列指定)
         * @param 接続先IPアドレス
         * @param 接続ポート
         * @return 接続結果
         */
        public L1sReceiver.Result connect(byte[] ipBytes, int port)
        {
            L1sReceiver.Result bRet = L1sReceiver.Result.NG;
            return bRet;
        }

        public bool isConnected()
        {
            if (connectedPort > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// データ取得要求 (L1S以外の汎用)
        /// </summary>
        public void sendReq(int reqcode)
        {
            if (isConnected() == false)
            {
                // 未接続
                //EventL1sResp(this, 10, new byte[0], " Not Connect(3610)");
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHzL1sReceiver:sendReq", "Not Connected : ");
                return;
            }


            try
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHzL1sReceiver", "Send Data : ");
                makeL1sGetReq(reqcode);
                byte[] msgData = m_L1sGetReq.encodedData;

                if (msgData != null)
                {
                    // 送信
                    sendL1sRequestData(msgData, 0x61, (byte)reqcode);
                    //msb.sendMonStatusMsg(msgData, CommonConst.SystemNumber.SYSTEM1);
                }
            }
            catch (Exception ex)
            {
                string sLog = String.Format("ERR CODE {0}, Err {1} ", 10, ex.Message);
                Program.m_thLog.PutErrorLog("SubGHzL1sReceiver", "Post Faild.", sLog);
            }
        }


        /**
         * @brief 要求データを送信する
         */
        void sendL1sRequestData(byte[] dat3610, byte port, byte reqcode)
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
                int size = dat3610.Length;
                byte[] tmpbyte = new byte[size + 2];
                tmpbyte[0] = port;
                tmpbyte[1] = reqcode;
                Array.Copy(dat3610, 0, tmpbyte, 2, size);


                sndData.Parameter = tmpbyte;

                sndData.encode();

                Program.m_SubGHz.Send(sndData.encodedData);
                //sendingflg = 1;

            }
            catch (Exception ex)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHzEquStat:sendRequestData", "送信エラー :" + ex.Message);
            }
        }

        /// <summary>
        /// L1S送信応答が来た
        /// </summary>
        public void rcvL1sDataEvent(object sender, byte[] data)
        {

            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHzL1sReceiver", "Recv Data : size(" + data.Length);

            if (data.Length == 0)
            {
                // 応答ではなく切断
                disconnect();
                EventL1sResp(this, 10, data, "L1S Disconnect ");
                return;
            }

            if (data.Length == 40)
            {
                // 回線確認
                return;
            }

            //OnEvent(10, "data get", "");
            MsgL1sGetRsp msg = new MsgL1sGetRsp();
            msg.encodedData = data;

            msg.decode(false);

            // 応答コード
            int code = msg.msgType % 1000;

            if (code == L1sReceiver.MSG_TYPE_CID)
            {
                // 端末IDその他取得要求
                L1sReceiver.readCidInfo(data);
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHzL1sReceiver", "端末ID、位置情報取得");
            }
            else if (code == L1sReceiver.MSG_TYPE_L1S)
            {
                // 表示時刻
                DateTime dt;

                // メッセージタイプ
                int msgtype = msg.l1sRcvInfo.l1sDataMsgType;

                // L1Sメッセージ取得
                byte[] payload = new byte[32];
                Array.Copy(data, 20, payload, 0, 32);

                // ヘッダ取得
                DbAccessStep2.RecvMsgInfo info = Program.m_BitsToMsg.getL1sInfo(payload);
                info.bitdata = payload;

                // 端末受信時時刻が有効であれば上書きする
                if (msg.l1sRcvInfo.rcvTime.checkParam() == EncDecConst.OK)
                {
                    dt = msg.l1sRcvInfo.rcvTime.getDateTime();
                    info.rdate = dt.ToString("yyyyMMddHHmmss");
                }
                else
                {
                    // 無効であれば受信時のPC時刻を参照する
                    dt = DateTime.Now;
                }

                // すでに同情報がDBに登録されていれば登録しない
                if (Program.m_objDbAccess.isSetRecvMsgInfo(info) == false)
                {
                    // 登録
                    if( !Program.m_objDbAccess.SetRecvMsgInfo(info) )
                    {
                        // DBエラーによりセット失敗した場合、
                        EventL1sResp(this, 10, data, "L1S受信時刻(" + dt.Minute + ":" + dt.Second + "." + dt.Millisecond + ") DB登録失敗 ");
                        return;
                    }
                }

                // dump
                //string strdump = Program.m_thLog.dumpDBG("L1S",ref payload, (uint)0, payload.Length);

                // 
                EventL1sResp(this, 10, data, "L1S受信時刻(" + dt.Minute + ":" + dt.Second + "." + dt.Millisecond + ") msgType:" + code);
            }
            else
            {
                // N/A
            }
        }

        /**
         * @brief L1Sデータ取得要求作成
         */
        private void makeL1sGetReq(int data11)
        {

            if (data11 == L1sReceiver.MSG_TYPE_CID)
            {
                m_L1sGetReq = new MsgL1sGetReq(L1sReceiver.MSG_TYPE_CID);
            }
            else
            {
                m_L1sGetReq = new MsgL1sGetReq(L1sReceiver.MSG_TYPE_L1S);
            }

            // 装置ID
            m_L1sGetReq.eqId = MsgSBandData.FIXED_EQ_ID; ;
            m_L1sGetReq.encode(false);
        }

        /**
         * @brief 切断処理
         */
        public void disconnect()
        {
            //connectedIp, connectedPort
            if (connectedPort == 0) return;
            //L1SDataDisconnect(connectedIp, connectedPort);
            connectedPort = 0;
        }

    }
}

