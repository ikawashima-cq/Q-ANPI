/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2017. All rights reserved.
 */
/**
 * @file    FwdReceiver.cs
 * @brief   FWDメッセージ受信クラス
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
     * @class FwdReceiver
     * @brief FWDメッセージ受信クラス
     */
    public class FwdReceiver 
    {
        /**
         * @brief 送信タイプ
         */
        private enum SendType
        {
            SEND_MON,
            SEND_FWD,
            SEND_RTN,
        };

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

        public enum Mode
        {
            TCPIP = 0,
            SUBGHZ,
        };

        public Mode m_Mode = Mode.TCPIP;

        // TcpとSubGHz
        TcpFwdReceiver m_TcpFwd = new TcpFwdReceiver();
        SubGHzFwdReceiver m_SGFwd = new SubGHzFwdReceiver();

        // メッセージ制限　アクセス制限
        // public bool m_sysSendRestriction = false;

        /**
         * @brief Ｓ帯モニタデータ（Ｓ帯FWDデータ受信要求）
         */
        private MsgSBandFwdGetReq fixedSBandFwdGetReq = new MsgSBandFwdGetReq();

        private MsgSendBase msb = new MsgSendBase();

        public SystemInfo m_sysInfo
        {
            set
            {
                if (m_Mode == Mode.TCPIP)
                {
                    //TCP
                    m_TcpFwd.m_sysInfo = value;
                }
                else
                {
                    // SubGHZ
                    m_SGFwd.m_sysInfo = value;
                }
            }

            get
            {
                if (m_Mode == Mode.TCPIP)
                {
                    //TCP
                    return m_TcpFwd.m_sysInfo;
                }
                else
                {
                    // SubGHZ
                    return m_SGFwd.m_sysInfo;
                }
            }
        }

        /**
         * @brief FWD応答デリゲート
         */
        public delegate void EventFwdDelegate(object sender, int code, byte[] msg, string outVal);
        public event EventFwdDelegate EventFwdResp
        {
            add
            {
                if (m_Mode == Mode.TCPIP)
                {
                    m_TcpFwd.EventFwdResp += value;
                }
                else
                {
                    // SubGHZ
                    m_SGFwd.EventFwdResp += value;
                    Program.m_SubGHz.rcvFwdDataEvent += new SubGHz.EventFwdDelegate(m_SGFwd.rcvFwdDataEvent);
                }
            }
            remove
            {
                if (m_Mode == Mode.TCPIP)
                {
                    m_TcpFwd.EventFwdResp -= value;
                }
                else
                {
                    // SubGHZ
                    m_SGFwd.EventFwdResp -= value;
                    Program.m_SubGHz.rcvFwdDataEvent -= new SubGHz.EventFwdDelegate(m_SGFwd.rcvFwdDataEvent);
                }
            }
        }

        public bool isConnected()
        {
            if (m_Mode == Mode.TCPIP)
            {
                return m_TcpFwd.isConnected();
            }
            else
            {
                return m_SGFwd.isConnected();
            }
        }

        /**
         * @brief 受け取っているFWDデータのシステム情報はメッセージ送信制限ありかなしか
         */
        public bool isSendRestrict()
        {
            if (m_Mode == Mode.TCPIP)
            {
                //TCP
                return m_TcpFwd.isSendRestrict();
            }
            else
            {
                // SubGHZ
                return m_SGFwd.isSendRestrict();
            }
        }

        /**
         * @brief 受け取っているFWDデータのシステム情報は周波数制限ありかなしか
         */
        public bool isFreqRestrict()
        {
            if (m_Mode == Mode.TCPIP)
            {
                //TCP
                return m_TcpFwd.isFreqRestrict();
            }
            else
            {
                // SubGHZ
                return m_SGFwd.isFreqRestrict();
            }
        }

        /**
         * @brief FWD要求を送る
         */
        public void sendReq()
        {
            if (m_Mode == Mode.TCPIP)
            {
                // connect時に要求送信済
                return ;
            }
            else
            {
                m_SGFwd.sendReq();
                return;
            }
        }

        /**
         * @brief 接続処理(文字列指定)
         * @param IPアドレス
         * @param ポート番号
         */
        public Result connect(string ipAddr, int port)
        {
            if (m_Mode == Mode.TCPIP)
            {
                return m_TcpFwd.connect(ipAddr, port);
            }
            else
            {
                return m_SGFwd.connect("",3090);
            }
        }

        /**
         * @brief 切断処理
         */
        public void disconnect()
        {
            if (m_Mode == Mode.TCPIP)
            {
                m_TcpFwd.disconnect();
            }
            else
            {
                m_SGFwd.disconnect();
            }
        }
    }
}

