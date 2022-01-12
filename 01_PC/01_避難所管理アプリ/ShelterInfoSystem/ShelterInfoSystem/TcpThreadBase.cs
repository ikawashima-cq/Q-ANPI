/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2017. All rights reserved.
 */
/**
 * @file    TcpThreadBase.cs
 * @brief   Tcp送信スレッド管理ベースクラス
 */
using System;

using System.Windows.Forms;

namespace ShelterInfoSystem
{
    /**
     * @class TcpThreadBase
     * @brief Tcp送信スレッド管理ベースクラス
     */
    public class TcpThreadBase : ThreadBase
    {
        // スレッド
        //private System.Threading.Thread m_Thread = null;
        private bool m_CommFlag = false;

        // イベントメッセージデリゲート
        public delegate void EventMessageDelegate(object sender, int code, string msg, string outVal);
        public event EventMessageDelegate MessageEvent;
        
        /**
         * @brief Tcp状態設定
         * @param TCP接続状態
         */
        public void SetTcpStatus(bool val)
        {
            m_CommFlag = val;
        }

        /**
         * @brief Tcp状態取得
         * @return TCP接続状態
         */
        public bool GetTcpStatus()
        {
            return m_CommFlag;
        }

        /**
         * @brief Tcpスレッド開始
         * @param Ipアドレス
         */
        public void TcpProc(byte[] val)
        {
            // スレッド作成
            m_Thread = new System.Threading.Thread(tcpfunc);

            // スレッドを開始
            m_Thread.Start(val);

        }

        /**
         * @brief Tcpスレッド処理
         */
        private void tcpfunc(object val)
        {
            SetTcpStatus(true);

            byte[] inVal = (byte[])val;
            int nCode;
            string sErr;
            string sDmy;
            while (GetTcpStatus())
            {
                Boolean nRet = TcpReqRes(inVal, out nCode, out sErr, out sDmy);

                if (nRet == false)
                {

                }
                else
                {
                    OnEvent(nCode, sErr, sDmy);
                }

                //System.Threading.Thread.Sleep(2000);
                break;
            }

            SetTcpStatus(false);
            m_Thread = null;
        }

        /**
         * @brief TCPスレッド継承先用仮想関数
         */
        public virtual Boolean TcpReqRes(byte[] inData, out int code, out string errMsg, out string outVal)
        {
            code = 0;
            errMsg = "";
            outVal = "";
            return true;
        }

        /**
         * @brief メッセージイベント呼び出し関数
         */
        public virtual void OnEvent(int code, string msg, string outVal)
        {
            if (MessageEvent != null)
            {
                // 実行
                MessageEvent(this, code, msg, outVal);
            }
        }

    }
}

