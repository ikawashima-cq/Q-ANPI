/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2017. All rights reserved.
 */
/**
 * @file    EquStat.cs
 * @brief   端末情報管理
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
     * @class EquStat
     * @brief 端末監視ステータス通信クラス
     */
    public class TermInfo 
    {
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

        private TcpEquStat m_TcpEquStat = new TcpEquStat();
        private SubGHzEquStat m_SGEquStat = new SubGHzEquStat();

        /**
         * @brief: タイマー
         */
        System.Windows.Forms.Timer timer = null;
        private System.Threading.Thread m_Thread = null;

        /**
         * @brief 監視ステータスデータの送信時刻加算秒
         */
        //private const double SENDTIME_ADD_SEC = 6.5;

        /**
         * @brief 監視ステータスデータ送信要求）
         */
        private MsgSBandEquStatReq fixedSBandEquStatReq = new MsgSBandEquStatReq();

        //private MsgSendBase msb = new MsgSendBase();

        // EquStat応答
        public delegate void EventEquDelegate(object sender, int code, byte[] msg, string outVal);
        public event EventEquDelegate EventEquResp
        {
            add
            {
                if (m_Mode == Mode.TCPIP)
                {
                    m_TcpEquStat.EventEquResp += value;
                }
                else
                {
                    // SubGHZ
                    m_SGEquStat.EventEquResp += value;
                    Program.m_SubGHz.rcvEquDataEvent += new SubGHz.EventEquDelegate(m_SGEquStat.rcvEquDataEvent);
                }
            }
            remove
            {
                if (m_Mode == Mode.TCPIP)
                {
                    m_TcpEquStat.EventEquResp -= value;
                }
                else
                {
                    // SubGHZ
                    m_SGEquStat.EventEquResp -= value;
                    Program.m_SubGHz.rcvEquDataEvent -= new SubGHz.EventEquDelegate(m_SGEquStat.rcvEquDataEvent);
                }
            }
        }
            

        // 端末ステータス
        public string mQCID;                    // 端末ID
        public int mCID;                        // 端末ID(BF<<9+BC)数値版
        public int mGID;
        //public int mORGID;       // 利用機関ID
        public int[] mORGID = new int[3];       // 利用機関ID
        public double mLatitude;
        public double mLongitude;

        // 装置ステータス
        public int mVolt;           // 電圧状態
        public int mOpe;            // 運用状態

        public TermInfo()
        {
        }

        /**
         * @brief コネクトしていない場合 : 0
         */
        public bool isConnected()
        {
            if (m_Mode == Mode.TCPIP)
            {
                return m_TcpEquStat.isConnected();
            }
            else
            {
                return m_SGEquStat.isConnected();
            }
        }

        /**
         * @brief タイマー起動
         */
        public void startTimer(int interval)
        {
            if (timer == null)
            {
                timer = new System.Windows.Forms.Timer();
                timer.Tick += new EventHandler(OnTimer);
                timer.Interval = interval;
                timer.Start();
            }
        }

        /**
         * @brief タイマー終了
         */
        public void stopTimer()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
                timer = null;
            }
        }

        private void OnTimer(object sender, EventArgs e)
        {
            // スレッド作成
            m_Thread = new System.Threading.Thread(sendReq);

            // スレッドを開始
            m_Thread.Start();
        }

        /**
         * @brief EquStatデータ取得要求(スレッド)
         */
        public void sendReq()
        {
            sendReq(false);
        }
        public void sendReq(bool force)
        {
            if (isConnected() == false)
            {
                return;
            }
            

            if (m_Mode == Mode.TCPIP)
            {
                m_TcpEquStat.sendReq();
            }
            else
            {
                // SubGHZ
                if (Program.m_SubGHz.GetStatSubGHz() == SubGHz.statSubGHz.Connected)
                {
                    m_SGEquStat.sendReq();
                }
                else if (force) 
                {
                    m_SGEquStat.sendReq();
                }
                else
                {

                }
            }

            m_Thread = null;
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
            if (m_Mode == Mode.TCPIP)
            {
                return m_TcpEquStat.connect(ipAddr, port);
            }
            else
            {
                // SubGHZ
                return m_SGEquStat.connect(ipAddr, port);
            }
        }

        /**
         * @brief 接続処理(IPアドレスbyte配列指定)
         * @param 接続先IPアドレス
         * @param 接続先ポート
         * @return 接続結果
         */
        public Result connect(byte[] ipBytes, int port)
        {
            if (m_Mode == Mode.TCPIP)
            {
                return m_TcpEquStat.connect(ipBytes, port);
            }
            else
            {
                // SubGHZ
                return TermInfo.Result.NG;
            }
        }

        /**
         * @brief 切断処理
         */
        public void disconnect()
        {
            if (m_Mode == Mode.TCPIP)
            {
                m_TcpEquStat.disconnect();
            }
            else
            {
                // SubGHZ
                m_SGEquStat.disconnect();
            }
        }

        /**
         * @brief スレッド終了メソッド
         */
        public void Exit()
        {
            if (m_Thread != null)
            {
                m_Thread.Join();
            }
        }

    }
}

