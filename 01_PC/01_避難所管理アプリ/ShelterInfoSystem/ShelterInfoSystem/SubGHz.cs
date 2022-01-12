/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
 */
/**
 * @file    SubGHz.cs
 * @brief   Sub-GHz通信状態、Uart状態を管理するスレッド
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.IO;
using System.IO.Ports;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace ShelterInfoSystem
{
    /**
     * @class SubGHz
     * @brief Sub-GHz通信状態、Uart状態を管理するスレッドクラス
     */
    public class SubGHz : ThreadBase
    {
        /**
         * Sub-GHz状態定義
         */
        public enum statSubGHz
        {
            Disconnected = 0,   // 初期化前、初期化中
            Connected,          // 通常
            TestSending,        // テスト送信中
            TestRecving         // テスト受信中
        }

        // Q-ANPI端末メッセージ送信
        public const int MSG_SEND = 0;
        // Q-ANPI端末メッセージ受信
        public const int MSG_RECV = 1;

        // Q-ANPI端末メッセージIDのインデックス
        public const int MSGID_INDEX = 3;

        // Q-ANPI端末メッセージポートIDのインデックス
        public const int MSGPID_INDEX = 13;
        // Q-ANPI端末メッセージデータIDのインデックス
        public const int MSGDID_INDEX = 14;

        // Q-ANPI端末メッセージフォーマット長
        public const int MSGLEN_SUBG_CONNECT_SEND = 14;     // サブギガ接続(初回疎通通信)
        public const int MSGLEN_EQU_STATUS_SEND = 34;       // 設備ステータス要求
        public const int MSGLEN_EQU_STATUS_RECV = 95;       // 設備ステータス応答
        public const int MSGLEN_FWD_FORMAT_SEND = 46;       // FWDメッセージ受信要求
        public const int MSGLEN_FWD_FORMAT_RECV = 215;      // FWDメッセージ受信応答
        public const int MSGLEN_RTN_FORMAT_SEND = 58;       // RTNメッセージ送信要求
        public const int MSGLEN_RTN_FORMAT_RECV = 59;       // RTNメッセージ送信応答
        public const int MSGLEN_DISASTER_SEND = 47;         // 災害危機通報受信要求
        public const int MSGLEN_DISASTER_RECV = 73;         // 災害危機通報受信応答
        public const int MSGLEN_TERMINAL_SEND = 47;         // 端末情報取得要求
        public const int MSGLEN_TERMINAL_RECV = 95;         // 端末情報取得応答
        public const int MSGLEN_START_STOP_SEND = 16;       // 災害危機通報受信停止/再開要求
        public const int MSGLEN_START_STOP_RECV = 16;       // 災害危機通報受信停止/再開応答
        public const int MSGLEN_TIME_SYNC_SEND = 15;        // 時刻情報取得要求
        public const int MSGLEN_TIME_SYNC_RECV = 31;        // 時刻情報取得応答
        public const int MSGLEN_SUBG_TEST_SEND = 14;        // サブギガ送受信テスト クライアント発
        public const int MSGLEN_SUBG_TEST_RECV = 23;        // サブギガ送受信テスト サーバ発

        // Q-ANPI端末メッセージフォーマットポートID
        public const int MSGPID_EQU_STATUS = 0x05;          // 設備ステータス要求/応答
        public const int MSGPID_FWD_FORMAT = 0x90;          // FWDメッセージ受信要求/応答
        public const int MSGPID_RTN_FORMAT = 0x91;          // RTNメッセージ送信要求/応答
        public const int MSGPID_DISASTER_REPORT = 0x61;     // 災害危機通報受信要求/応答
        public const int MSGPID_GET_TERMINAL_INFO = 0x61;   // 端末情報取得要求/応答
        public const int MSGPID_DISASTER_REPORT_OPE = 0x61; // 災害危機通報受信停止/再開
        public const int MSGPID_TIME_SYNC = 0x00;           // 時刻情報取得要求/応答
        public const int MSGPID_SUBG_TEST = 0x00;           // サブギガ送受信テスト クライアント発/サーバ発

        // サブギガモジュールのメッセージフォーマットポートID
        public const int MSGPID_COM_CONNECT = 0x03;         // COMポート接続要求
        public const int MSGPID_GET_DEVICE_ID = 0x00;       // 無線モジュールデバイスID取得
        public const int MSGPID_GET_QANPI_DEVICE_ID = 0x01; // Q-ANPIデバイスID取得

        // Q-ANPI端末メッセージフォーマットデータID
        public const int MSGDID_DISASTER_REPORT = 0x28;     // 災害危機通報受信要求/応答
        public const int MSGDID_GET_TERMINAL_INFO = 0x29;   // 端末情報取得要求/応答
        public const int MSGDID_DISASTER_REPORT_OPE = 0x2A; // 災害危機通報受信停止/再開
        public const int MSGDID_TIME_SYNC = 0x50;           // 時刻情報取得要求/応答
        public const int MSGDID_SUBG_TEST = 0x00;           // サブギガ送受信テスト クライアント発/サーバ発

        // サブギガモジュールのメッセージID
        public const byte MSGID_ACK_00 = 0x00;
        public const byte MSGID_NACK_01 = 0x01;
        public const byte MSGID_SEARCH_10 = 0x10;
        public const byte MSGID_SEND_11 = 0x11;
        public const byte MSGID_RESEND_12 = 0x12;
        public const byte MSGID_GETID_7D = 0x7D;
        public const byte MSGID_CONNECT = 0x2A;

        // モジュールのヘッダサイズ
        private const int HEADER_SIZE = 13;

        // Anpiサブギガのヘッダサイズ(13 + port + num)
        private const int SUBGHZ_HEADER_SIZE = 15;

        // サブギガ衛星通信端末接続前状態
        public const int CONNECTING_CONNECT = 0;
        public const int CONNECTING_DISCONNECT = 1;
        public const int CONNECTING_SEARCHDEVICE = 2;
        public const int CONNECTING_GETDEVICE = 3;

        // テスト送受信状態
        public const int TEST_SEND_MODE = 0;
        public const int TEST_RECV_MODE = 1;

        // FWD結合用
        private const int FWD_RSP_SIZE = 472;
        private byte[] m_fwddata = new byte[FWD_RSP_SIZE];

        // 分割サイズ 200バイト(+ヘッダ15)
        private const int SUBGHZ_SPLIT_SIZE = 200;

        // 再送試行回数最大
        private const int SEND_RETRY_MAX = 5;

        // 呼び出し元用デリゲート
        public delegate void EventMessageDelegate(object sender, int code, string msg, int outVal);
        public delegate void EventTestMessageDelegate(object sender, int code, string msg, int outVal);

        // イベント定義
        public event EventMessageDelegate MessageEvent;
        public event EventTestMessageDelegate TestMessageEvent;

        // 装置ステータス
        public delegate void EventEquDelegate(object sender, byte[] msg);
        public event EventEquDelegate rcvEquDataEvent;

        // ポート3610 L1S/CID
        public delegate void EventL1sDelegate(object sender, byte[] msg);
        public event EventL1sDelegate rcvL1sDataEvent;

        // ポート3090 FWD
        public delegate void EventFwdDelegate(object sender, byte[] msg);
        public event EventFwdDelegate rcvFwdDataEvent;

        // ポート3091 RTN
        public delegate void EventRtnDelegate(object sender, byte[] msg);
        public event EventRtnDelegate rcvRtnDataEvent;

        // ポート0x00 時刻要求
        public delegate void EventTimeSyncDelegate(object sender, byte[] msg);
        public event EventTimeSyncDelegate rcvTimeSyncEvent;

        // ポート0x61 災害危機通報受信 停止/再開
        public delegate void EventDisasterReportDelegate(object sender, byte[] msg);
        public event EventDisasterReportDelegate rcvDisasterReportEvent;

        /**
         * メンバ変数
         */
        private static SubGHz instance = new SubGHz();    // インスタンス

        private Mutex m_Mutex = new System.Threading.Mutex();       // UARTへのアクセス用排他ミューテックス
        private static statSubGHz m_StatSubGHz = statSubGHz.Disconnected;                        // Sub-GHz状態

        private bool m_UartInitialized = false;     // UART初期化済みフラグ
        private int m_UartBaud = 115200;            // UART通信速度
        private const int MODULE_BAUD_DEFAULT = 38400;       // UARTモジュールデフォルト通信速度

        /**
         * @brief 送受信回数
         */
        private int m_TSCount = 0;
        private byte m_Channel = 24;        // デフォルト値
        private string m_PortName;

        /**
         * @brief MSGID <-> MSGNO
         */
        private int[] m_TableMsgID = new int[300];
        private byte m_MsgNo = 1;
        private readonly object m_MsgNoLock = new object();

        // 送信キュー 
        private readonly Queue<byte[]> m_sendQueue = new Queue<byte[]>();

        // 最小送信間隔(ms)
        private int m_Interval = 80;

        // 送信キューの変化を通知してくれるイベント 
        private readonly ManualResetEvent m_notifyEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent m_notifyEventRecv = new ManualResetEvent(false);
        // 送信ロック
        private readonly object m_sendLock = new object();
        private readonly static object m_recvLock = new object();
        /**
         * @brief 使用するシリアルポート
         */
        private SerialPort m_SerialPort;

        /**
         * @brief ログ書込用変数群
         */
        // ログ書込用サブギガ通信成功回数
        private int m_suceedCount = 0;
        // ログ出力用データテーブル
        private DataTable m_dataTable = new DataTable();
        // RSSI値格納用リスト
        private List<string> m_rssis = new List<string>(2);
        // ログ出力用コード(0の時は受信テスト, 1の時は送信テスト)
        private int m_logCode = 0;

        /**
         * @brief スレッド終了フラグ
         */
        public bool m_Finish;

        private bool m_SendRetry = false;

        /**
         * @brief シングルトンのためnewは禁止とする
         */
        private SubGHz()
        {

        }

        /**
         * @brief インスタンス取得
         * @return SubGHzインスタンス
         */
        public static SubGHz GetInstance()
        {
            return instance;
        }

        /**
         * @brief シリアルポートをオープンする
         */
        public Boolean OpenSerialPort(String portName, int baud)
        {
            if (portName.Equals(""))
            {
                return false;
            }
            if (m_SerialPort != null)
            {
                try
                {
                    m_SerialPort.Close();
                }
                catch
                {

                }
                m_SerialPort = null;
            }
            m_SerialPort = new SerialPort(portName, baud); // ボーレート
            m_SerialPort.ReadTimeout = 500;
            m_SerialPort.WriteTimeout = 500;
            m_SerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            m_SerialPort.PinChanged += new SerialPinChangedEventHandler(SerialErrHandler);

            // オープンしていたらクローズ
            if (m_SerialPort.IsOpen)
            {
                try
                {
                    m_SerialPort.Close();
                }
                catch
                {
                    // ポートがクローズできなかった場合
                   
                    return false;
                }
            }
            // ポートオープン
            try
            {
                m_SerialPort.Open();

                if (Program.m_SubGHzConfig.RtsEnable != null && Program.m_SubGHzConfig.RtsEnable.ToUpper() == "TRUE")
                {
                    // フロー制御
                    m_SerialPort.RtsEnable = true;
                }
                else
                {
                    m_SerialPort.RtsEnable = false;
                }

                m_SerialPort.DiscardOutBuffer();
                m_SerialPort.DiscardInBuffer();
            }
            catch(Exception ex)
            {
                // ポートがオープンできなかった場合
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "SubGHz", "OpenSerialPort", ex.Message);
                return false;
            }

            //m_StatSubGHz = statSubGHz.Connected;
            m_PortName = portName;

            return true;
        }

        /**
         * @brief シリアルポートをクローズする
         */
        private Boolean CloseSerialPort()
        {
            // オープンしていたらクローズ
            if (m_SerialPort != null && m_SerialPort.IsOpen)
            {
                try
                {
                    m_SerialPort.DiscardInBuffer();
                    m_SerialPort.DiscardOutBuffer();
                    m_SerialPort.Close();
                }
                catch(Exception ex)
                {
                    // ポートがクローズできなかった場合
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "SubGHz", "CloseSerialPort", ex.Message);
                    return false;
                }
            }
            return true;
        }

        /**
         * @brief メインスレッド開始処理
         */
        public override void Run()
        {
            m_Finish = false;
            SendLoop();

        }

        /**
         * @brief メインスレッド終了処理
         */
        public override void Exit()
        {
            m_Thread.Interrupt();

            // ポートクローズ
            CloseSerialPort();

            // 親クラスのメインスレッド処理
            base.Exit();
        }

        /**
         * @brief 接続処理設定(シリアルポートOPEN)
         * @param[in] comport COMポート名称文字列
         */
        public bool OpenSerialSubGHz(string comport)
        {
            if (m_StatSubGHz != statSubGHz.Disconnected)
            {
                // 再接続
                m_StatSubGHz = statSubGHz.Disconnected;
            }
            // UARTボーレート取得
            string baud = Program.m_SubGHzConfig.Baudrate;
            try
            {
                if (baud != null)
                {
                    m_UartBaud = int.Parse(baud);
                }
            }
            catch
            {
            }

            // ポート接続時はUART設定を初期化する
            if (!OpenSerialPort(comport, MODULE_BAUD_DEFAULT))
            {
                // シリアルポートオープン失敗のためエラー
                OnEvent(-1, "COMポート設定エラー", CONNECTING_CONNECT);
                return false;
            }

            // 初回UART設定を実施する
            try
            {
                m_UartInitialized = false;
                SendInitMsg();
                OnEvent(0, comport + " UART設定中", CONNECTING_CONNECT);
            }
            catch (Exception)
            {
                OnEvent(-1, "チャネルセットエラー", CONNECTING_CONNECT);
                return false;
            }
            return true;
        }

        /**
         * @brief 接続処理設定(チャネル)
         * @param[in] channel
         *
         */
        public bool SetChannelSubGHz(string channel)
        {
            try
            {
                byte bcha = byte.Parse(channel.Substring(0, 2));
                SetChannel(bcha);
                SendInitMsg();
            }
            catch (Exception)
            {
                OnEvent(-1, "チャネルセットエラー", CONNECTING_CONNECT);
                return false;
            }

            OnEvent(3, "チャネルセット", CONNECTING_CONNECT);
            return true;
        }

        /**
         * @brief 接続処理設定
         * @param[in] comport COMポート名称文字列
         * @param[in] channel チャネル
         */
        public void ConnectSubGHz(string comport, string channel)
        {
            // 接続
            SendInitData();
            return ;
        }

        /**
         * @brief Sub-GHz切断処理
         * return 
         */
        public void DisconnectSubGHz()
        {
            Program.m_RtnThreadSend.ClearSendQue();

            switch (m_StatSubGHz)
            {
                case statSubGHz.Disconnected:
                    m_StatSubGHz = statSubGHz.Disconnected;
                    OnEvent(1, "切断しました", CONNECTING_DISCONNECT);

                    break;
                default:
                    if (!CloseSerialPort())
                    {
                        OnEvent(-1, "切断失敗しました", CONNECTING_DISCONNECT);
                    }
                    else
                    {
                        m_StatSubGHz = statSubGHz.Disconnected;
                        OnEvent(1, "切断しました", CONNECTING_DISCONNECT);   
                    }
                    break;
            }
            Program.m_mainForm.ResetSendWaitingNo();
            Program.m_mainForm.m_nowSending = false;
        }

        // UARTに直接書き込み
        private bool UartWrite(byte[] inVal)
        {
            try
            {
                m_SerialPort.Write(inVal, 0, inVal.Length);
            }
            catch (TimeoutException ex)
            {
                // time out
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "SubGHz", "Write TIMEOUT", ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                // 引数ミス、ポート未オープン、バグ等
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "SubGHz", "UartWrite", ex.Message);
                return false;
            }

            return true;
        }

        // 送信：送信キューに追加
        public void Send(byte[] senddata)
        {
            // タスクを追加 
            lock (this.m_sendLock)
            {
                this.m_sendQueue.Enqueue(senddata);
            }
            // 知らせる
            this.m_notifyEvent.Set();
        }

        // 
        public void setSendInterval(int interval)
        {
            m_Interval = interval;
        }

        // 送信スレッドループ
        private void SendLoop()
        {
            int sendTryCount;
            // 送信
            while (!m_Finish)
            {
                try
                {
                    // キューへの追加を待つ 
                    this.m_notifyEvent.WaitOne();
                    this.m_notifyEvent.Reset();

                    while (!m_Finish)
                    {
                        // 送信中 
                        byte[] msgdata;

                        if (this.m_sendQueue.Count == 0)
                        {
                            // 送信完了
                            break;
                        }
                        // 
                        msgdata = this.m_sendQueue.Dequeue();
                        sendTryCount = SEND_RETRY_MAX;       // 5回まで再送信可能
                        while (!m_Finish)
                        {
                            m_SendRetry = false;
                            System.Threading.Thread.Sleep(50); // 50ms後に確認する
                            if (Program.m_statRecv == true && sendTryCount > 0)     // 受信中だった場合少し待つ(最大5回)
                            {
                                sendTryCount--;
                                System.Threading.Thread.Sleep(50);     // 50ms待つ
                                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "SendLoop [RecvContinue] SendTry:" + sendTryCount + " queue_count:" + this.m_sendQueue.Count);
                                continue; //break;  //continueかも
                            }
                            else if (sendTryCount <= 0)
                            {
                                // 再送試行回数を超過した場合、スキップして次のキューを見る。
                                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "SendLoop [SendTry Over]");
                                break;
                            }
                            else
                            {
                                // 送信を行う
                            }

                            // デバグログ
                            string writebuf = "";
                            for (int i = 0; i < msgdata.Length; i++)
                            {
                                string hsin = Convert.ToString(msgdata[i], 16);
                                hsin = hsin.PadLeft(2, '0');
                                writebuf += hsin + " ";
                            }

                            writebuf = JudgeMessageType(msgdata, MSG_SEND) + writebuf;

                            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "send", writebuf);

                            bool bre = UartWrite(msgdata);
                            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "UartWrite:" + bre);

                            if (bre == false)
                            {
                                // URATに書き込み失敗
                                sendTryCount--;
                                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "Retry [UartWrite Error] SendTry:" + sendTryCount + " queue_count:" + this.m_sendQueue.Count);
                                continue;
                            }
                            else
                            {
                                // UARTから応答を待つ
                                TimeSpan timeout = new TimeSpan(0, 0, 0, 0, Program.m_objShelterAppConfig.UARTAckTimeout + 1000);
                                DateTime starttime = DateTime.Now;
                                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "WaitOne Start");
                                m_notifyEventRecv.Reset();
                                bool bRet = m_notifyEventRecv.WaitOne(timeout);
                                m_notifyEventRecv.Reset();
                                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "WaitOne End");
                                double waittime = (DateTime.Now - starttime).TotalMilliseconds;

                                if (waittime > Program.m_objShelterAppConfig.UARTAckTimeout)
                                {
                                    if (!m_UartInitialized)
                                    {
                                        CloseSerialPort();
                                        if (OpenSerialPort(m_PortName, m_UartBaud))
                                        {
                                            m_UartInitialized = true;
                                            OnEvent(5, m_PortName + " 接続完了", CONNECTING_CONNECT);
                                        }
                                        else
                                        {
                                            DisconnectSubGHz();
                                        }
                                        break;
                                    }
#if DEBUG
# else
                                    // タイマー発動UARTの応答なしの場合切断する
                                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "UART Ack Timeout waittime:" + waittime);
                                    DisconnectSubGHz();
                                    break;
#endif
                                }
                                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "UART Ack waittime:" + waittime);
                            }

                            if (m_SendRetry)
                            {
                                // 0x12等で再送判定された場合再送する
                                sendTryCount--;
                                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "Retry [SendRetry] SendTry:" + sendTryCount + " queue_count:" + this.m_sendQueue.Count);
                                System.Threading.Thread.Sleep(50); // 50ms+50ms後に再送する
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                        Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "SendLoop nextLoop");

                        //次の送信キューを見る 
                        System.Threading.Thread.Sleep(m_Interval);
                    }
                }
                catch (ThreadInterruptedException)
                {
                    //終了
                    m_Finish = true;
                }
                catch (Exception ex)
                {
                    //エラー
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "SubGHz", "send", ex.Message);
                    System.Threading.Thread.Sleep(2000); ;
                }
            }
        }

        // 受信：別スレッドで呼ばれるシリアルポート内のデータ処理
        private static void DataReceivedHandler(
                        object sender,
                        SerialDataReceivedEventArgs e)
        {
            Program.m_statRecv = true;              // UART通信状態によって受信ON
            SerialPort sp = (SerialPort)sender;
            if (sp.BytesToRead == 0)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "size 0");
                Program.m_statRecv = false;              // UART通信状態によって受信OFF
                return;
            }
            byte[] data = null;
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "DataReceivedHandler");
            lock (m_recvLock)
            {
                try
                {
                    while (true)
                    {
                        // ヘッダ読み込み
                        byte[] bheader = new byte[HEADER_SIZE];
                        for (int i = 0; i < HEADER_SIZE; i++)
                        {
                            bheader[i] = (byte)sp.ReadByte();
                        }

                        // チェック
                        if (bheader[0] == 0x0F && bheader[1] == 0x5A)
                        {
                            // OK
                            // ACKNACK
                            if (bheader[3] == MSGID_ACK_00 || bheader[3] == MSGID_NACK_01 || bheader[3] == MSGID_RESEND_12)
                            {
                                // 応答待ちのスレッドに通知
                                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "DataReceivedHandler msgId:" + MSGID_RESEND_12);
                                // instance.m_notifyEventRecv.Set();
                            }
                        }
                        else
                        {
                            // NG
                            Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "SubGHz", "recv",
                                "ヘッダエラー（0x0F5A）");
                            Program.m_statRecv = false;              // UART通信状態によって受信OFF
                            return;
                        }

                        // データ部サイズ
                        int length = bheader[2];
                        if (length >= 0xFF)
                        {
                            // NG
                            Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "SubGHz", "recv",
                                "サイズエラー");
                            Program.m_statRecv = false;              // UART通信状態によって受信OFF
                            return;
                        }
                        data = new byte[length];
                        Array.Copy(bheader, data, HEADER_SIZE);

                        // データ部読み込み
                        for (int i = HEADER_SIZE; i < length; i++)
                        {
                            data[i] = (byte)sp.ReadByte();
                        }

                        Program.m_statRecv = false;              // UART通信状態によって受信OFF
                        
                        // デバグログ
                        string writebuf = "";
                        for (int i = 0; i < data.Length; i++)
                        {
                            string hsin = Convert.ToString(data[i], 16);
                            hsin = hsin.PadLeft(2, '0');
                            writebuf += hsin + " ";
                        }

                        writebuf = JudgeMessageType(data, MSG_RECV) + writebuf;

                        Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "recv", writebuf);

                        if (data != null)
                        {
                            instance.DataProc(data);
                            break;
                        }
                    }
                }
                catch (TimeoutException)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "recv TIMEOUT");
                    Program.m_statRecv = false;              // UART通信状態によって受信OFF
                }
                catch (Exception ex)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "recv ERROR " + ex.Message);
                    Program.m_statRecv = false;              // UART通信状態によって受信OFF
                }
            }
        }

        // シリアルポートエラー処理
        private static void SerialErrHandler(object sender, SerialPinChangedEventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "SubGHz", "ErrReceivedHandler", "シリアルポートエラー" + e.EventType);
        }

        // コマンド解析＋振り分け処理へ
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void DataProc(byte[] indata)
        {
            byte msgid = indata[3];
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "ID " + msgid, "");

            // 振り分け処理
            OnCommand(indata);
        }

        // 送信タイムアウト
        private void SendTimeOut(byte msgid)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "SubGHz", "SendTimeOut", "ID:" + msgid + " stat:" + m_StatSubGHz);

            if (m_StatSubGHz == statSubGHz.Connected && msgid == MSGID_GETID_7D)
            {
                // DEVICE_ID 取得
                OnTestEvent(-1, "デバイスID取得エラー", 3);
            }
            else if (m_StatSubGHz == statSubGHz.Connected && msgid == MSGID_RESEND_12)
            {
                // 再送信
                OnTestEvent(0, "再送信タイムアウト", 0);
            }
            else if (m_StatSubGHz == statSubGHz.TestSending && msgid == MSGID_SEND_11)
            {
                // テスト送信
                OnTestEvent(0, "メッセージ送信エラー", 0);
            }
            else if (m_StatSubGHz == statSubGHz.Disconnected && msgid == 0x21)
            {
                // チャネル設定（未接続）
                OnEvent(-1, "通信ドングルに接続できませんでした", CONNECTING_CONNECT);
            }
            else if (m_StatSubGHz == statSubGHz.Disconnected && msgid == MSGID_GETID_7D)
            {
                // デバイス取得（未接続）
                OnEvent(-1, "通信ドングルに接続できませんでした", CONNECTING_CONNECT);
            }
            else if (m_StatSubGHz == statSubGHz.Disconnected && msgid == MSGID_SEARCH_10)
            {
                // デバイス取得（端末検索タイムアウト）
                OnEvent(-1, "端末検索タイムアウトしました", CONNECTING_SEARCHDEVICE);
            }
            else if (m_StatSubGHz == statSubGHz.Disconnected)
            {
                // 未接続
                OnEvent(-1, "端末との接続ができませんでした", CONNECTING_CONNECT);
            }
            else
            {
                OnEvent(0, "メッセージ送信エラー(タイムアウト" + msgid + ")", CONNECTING_CONNECT);
            }

            Program.m_mainForm.ResetSendWaitingNo();
            Program.m_mainForm.m_nowSending = false;
        }

        // 振り分け処理（コマンド、受信データ）
        private void OnCommand(byte[] command)
        {
            switch (m_StatSubGHz)
            {
                // 未接続、接続中
                case statSubGHz.Disconnected:
                    OnConnecting(command);
                    break;
                // 通常
                case statSubGHz.Connected:
                    OnConnected(command);
                    break;
                // テスト送信中
                case statSubGHz.TestSending:
                    OnTestSend(command);
                    break;
                // テスト受信中
                case statSubGHz.TestRecving:
                    OnTestRecv(command);
                    break;
                // それ以外
                default:
                    break;
            }
        }

        // イベント呼び出し
        public virtual void OnEvent(int code, string msg, int outVal)
        {
            if (MessageEvent != null)
            {
                // 実行
                MessageEvent(this, code, msg, outVal);
            }
        }

        // イベント呼び出し（テスト送受信画面）
        public virtual void OnTestEvent(int code, string msg, int outVal)
        {
            if (TestMessageEvent != null)
            {
                // 実行
                TestMessageEvent(this, code, msg, outVal);
            }
        }

        // set get
        public statSubGHz GetStatSubGHz()
        {
            return m_StatSubGHz;
        }

        // MsgIDをMsgNoにひも付けしつつMsgNo取得
        public byte GetMsgNo(byte msgid)
        {
            byte ret;
            lock (m_MsgNoLock)
            {
                m_MsgNo++;
                if (m_MsgNo >= 0xFF) // 最大２５４
                {
                    m_MsgNo = 1;
                }

                ret = m_MsgNo;
                m_TableMsgID[m_MsgNo] = msgid;
            }
            return ret;
        }

        public void ResetStatSubGHz()
        {
            m_StatSubGHz = statSubGHz.Disconnected;
            return;
        }

        /**
         * @brief 初期化イベント処理
         */
        private void OnConnecting(byte[] indata)
        {
            MsgSubGHz recvdata = new MsgSubGHz();
            recvdata.encodedData = indata;
            recvdata.decode();

            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz"
                , "OnConnecting", "MSGID " + recvdata.MsgId + " " + m_TableMsgID[recvdata.MsgNo]);

            // 問い合わせ応答 MsgID=0x00(OK) 0x01(NG)
            // ACK
            if (recvdata.MsgId == 0x00)
            {
                // チャネル設定（パワー、RF_BAUD、CS＿MODEも）
                if (m_TableMsgID[recvdata.MsgNo] == 0x21 || m_TableMsgID[recvdata.MsgNo] == 0x2A)
                {
                    if (m_UartInitialized)
                    {
                        // チャネルセット完了
                        OnEvent(3, "チャネル設定完了", CONNECTING_CONNECT);
                    }
                    else
                    {
                        // UART再接続を実施する
                        CloseSerialPort();
                        if (OpenSerialPort(m_PortName, m_UartBaud))
                        {
                            m_UartInitialized = true;
                            OnEvent(5, m_PortName + " 接続完了", CONNECTING_CONNECT);
                        }
                        else
                        {
                            DisconnectSubGHz();
                        }
                    }
                }
                // データ送信応答
                else if (m_TableMsgID[recvdata.MsgNo] == MSGID_SEND_11)
                {
                    // 疎通確認応答
                    OnEvent(2, "", CONNECTING_CONNECT);
                    m_StatSubGHz = statSubGHz.Connected;
                }
                
                // デバイス検索応答
                else if (m_TableMsgID[recvdata.MsgNo] == MSGID_SEARCH_10)
                {
                    string msg = recvdata.makeMsg10();
                    setRssi(msg);
                    OnEvent(MSGID_SEARCH_10, msg, CONNECTING_SEARCHDEVICE);
                }
                // デフォルト設定読み込み
                else if (m_TableMsgID[recvdata.MsgNo] == MSGID_GETID_7D)
                {
                    string sDevice = recvdata.makeMsg7dDeviceID();
                    OnEvent(1, sDevice, CONNECTING_CONNECT);
                }
                else
                {
                    // 初期化エラー
                    OnEvent(4, "端末との接続ができませんでした err2", CONNECTING_CONNECT);
                }
                // ACK待ち状態に通知
                instance.m_notifyEventRecv.Set();
            }
            // 送信＆問い合わせ応答 MsgID=0x00(OK) 0x01(NG)
            // NACK
            else if (recvdata.MsgId == MSGID_NACK_01)
            {
                OnEvent(4, "OnConnecting NACK " + m_TableMsgID[recvdata.MsgNo], CONNECTING_CONNECT);
                // ACK待ち状態に通知
                instance.m_notifyEventRecv.Set();
            }
            // データ送信応答(再送)
            else if (recvdata.MsgId == MSGID_RESEND_12)
            {
                // 初期化時はデバイス検索応答終了　または　疎通確認
                
                // 16 で　デバイス検索　17 で疎通確認
                int motonomsgid = m_TableMsgID[recvdata.MsgNo];
                if (motonomsgid == MSGID_SEARCH_10)
                {
                    // デバイス検索終了
                    //OnEvent(4, "", 0);
                    OnEvent(MSGID_RESEND_12, "", CONNECTING_SEARCHDEVICE);
                }
                else if (motonomsgid == MSGID_SEND_11)
                {
                    // 疎通確認失敗
                    OnEvent(-1, "", CONNECTING_CONNECT);
                    m_SendRetry = true;
                }
                else
                {
                    // なぞのMSGID
                    OnEvent(4, "データ送信応答(再送) エラー" + motonomsgid, CONNECTING_CONNECT);
                    m_SendRetry = true;
                }
                // ACK待ち状態に通知
                instance.m_notifyEventRecv.Set();

                //OnEvent(0x12, "", 2);
                //m_StatSubGHz = statSubGHz.Connected;
            }
            else
            {
                OnEvent(4, "OnConnecting err " + recvdata.MsgId, CONNECTING_CONNECT);
                m_SendRetry = true;
            }
        }

        /**
         * @brief 通常処理
         */
        private void OnConnected(byte[] indata) 
        {
            MsgSubGHz recvdata = new MsgSubGHz();
            recvdata.encodedData = indata;
            recvdata.decode();

            // ACK 問い合わせ応答 MsgID=0x00(OK) 0x01(NG)
            if (recvdata.MsgId == MSGID_ACK_00) 
            {
                // DEVICE_ID 取得
                if (m_TableMsgID[recvdata.MsgNo] == MSGID_GETID_7D)
                {
                    string sDevice = recvdata.makeMsg7dDeviceID();
                    OnEvent(0, sDevice, 3);
                    // ACK待ち状態に通知
                    instance.m_notifyEventRecv.Set();
                }
                // データ送信応答
                else if (m_TableMsgID[recvdata.MsgNo] == MSGID_SEND_11)
                {
                    // 送信応答
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "MSGID_ACK_00, MSGID_SEND_11");
                    // ACK待ち状態に通知
                    instance.m_notifyEventRecv.Set();
                }
                // デバイス検索応答
                else if (m_TableMsgID[recvdata.MsgNo] == MSGID_SEARCH_10)
                {
                    string msg = recvdata.makeMsg10();
                    setRssi(msg);
                    OnEvent(MSGID_SEARCH_10, msg, 2);
                    // ACK待ち状態に通知
                    instance.m_notifyEventRecv.Set();
                }
                else
                {
                    // N/A
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "MSGID_ACK_00, その他");
                }
            }
            // NACK 送信＆問い合わせ応答 MsgID=0x00(OK) 0x01(NG)
            else if (recvdata.MsgId == MSGID_NACK_01)
            {
                if (m_TableMsgID[recvdata.MsgNo] == MSGID_SEND_11)
                {
                    //OnTestEvent(0, "送信失敗しました", 0);
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "MSGID_NACK_01, MSGID_SEND_11");
                    // ACK待ち状態に通知
                    instance.m_notifyEventRecv.Set();
                }
                else
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "MSGID_NACK_01, その他");
                }
            }
            // データ受信
            else if (recvdata.MsgId == MSGID_SEND_11)
            {
                // size を取得してから new byte[]
                if (recvdata.Port == 0x05)
                {
                    // if 3005
                    byte[] equdata = new byte[indata.Length];
                    // todo indata size check
                    Array.Copy(indata, SUBGHZ_HEADER_SIZE, equdata, 0, indata.Length - SUBGHZ_HEADER_SIZE);
                    rcvEquDataEvent(this, equdata);
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "MSGID_SEND_11, port = 0x05");
                }
                else if (recvdata.Port == 0x00 && recvdata.Num == 0x50)
                {
                    // 時刻同期要求の返答
                    rcvTimeSyncEvent(this, indata);
                }
                else if (recvdata.Port == 0x61 && recvdata.Num == 0x2A)
                {
                    // 災害危機通報　受信停止/再開
                    rcvDisasterReportEvent(this, indata);
                }

                else if (recvdata.Port == 0x61)
                {
                    // 災害危機通報が来ても何も処理しないようにする
                    
                    // if 3610
                    byte[] l1sdata = new byte[indata.Length];
                    // todo indata size check
                    Array.Copy(indata, SUBGHZ_HEADER_SIZE, l1sdata, 0, indata.Length - SUBGHZ_HEADER_SIZE);
                    rcvL1sDataEvent(this, l1sdata);
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "MSGID_SEND_11, port = 0x61");
                    
                }
                else if (recvdata.Port == 0x90)
                {
                    // if 3090
                    //  分割番号順の処理
                    if (m_fwddata == null)
                    {
                        Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "m_fwddata null ", "");
                    }
                    else if (recvdata.Num == 1)
                    {
                        if (indata.Length != SUBGHZ_SPLIT_SIZE + SUBGHZ_HEADER_SIZE)
                        {
                            Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "SubGHz", "分割サイズエラー(1) "
                                , "size:" + indata.Length);
                        }
                        else
                        {
                            m_fwddata = new byte[FWD_RSP_SIZE];
                            Array.Copy(indata, SUBGHZ_HEADER_SIZE, m_fwddata, 0
                                , indata.Length - SUBGHZ_HEADER_SIZE);
                        }
                    }
                    else if (recvdata.Num == 2)
                    {
                        if (indata.Length != SUBGHZ_SPLIT_SIZE + SUBGHZ_HEADER_SIZE)
                        {
                            Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "SubGHz", "分割サイズエラー(2) "
                                , "size:" + indata.Length);
                        }
                        else
                        {
                            Array.Copy(indata, SUBGHZ_HEADER_SIZE, m_fwddata, SUBGHZ_SPLIT_SIZE
                                , indata.Length - SUBGHZ_HEADER_SIZE);
                        }
                    }
                    else if (recvdata.Num == 3)
                    {
                        if (indata.Length != 72 + SUBGHZ_HEADER_SIZE)
                        {
                            // FWD 3番目 サイズ72 以外
                            Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "SubGHz", "分割サイズエラー(3) "
                                , "size:" + indata.Length);
                        }
                        else
                        {
                            Array.Copy(indata, SUBGHZ_HEADER_SIZE, m_fwddata, SUBGHZ_SPLIT_SIZE * 2
                                , indata.Length - SUBGHZ_HEADER_SIZE);
                            rcvFwdDataEvent(this, m_fwddata);
                        }
                    }
                    else
                    {
                        Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "分割番号 > 3 ", "" + recvdata.Num);
                    }


                    //byte[] fwddata = new byte[indata.Length]; ///////////// 472
                }
                else if (recvdata.Port == 0x91)
                {
                    // if 3091
                    byte[] rtndata = new byte[indata.Length]; //
                    // todo indata size check
                    Array.Copy(indata, 15, rtndata, 0, 59 - 15);
                    rcvRtnDataEvent(this, rtndata);
                }
                else
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "OnConnected port " + recvdata.Port, "");
                }
            }
            // 再送完了
            else if (recvdata.MsgId == MSGID_RESEND_12)
            {                
                // 16 で　デバイス検索　17 で疎通確認
                int motonomsgid = m_TableMsgID[recvdata.MsgNo];
                if (motonomsgid == MSGID_SEARCH_10)
                {
                    // デバイス検索終了
                    OnEvent(MSGID_RESEND_12, "", 2);
                }
                else if (motonomsgid == MSGID_SEND_11)
                {
                    // ACKの代わりに再送
                    OnEvent(MSGID_RESEND_12, "データ送信応答(0x12)のため再送", 0);
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "データ送信応答(0x12) エラー");
                    m_SendRetry = true;
                    // ACK待ち状態に通知
                    instance.m_notifyEventRecv.Set();
                }
                else
                {
                    // なぞのMSGID
                    OnEvent(4, "データ送信応答(再送) エラー" + motonomsgid, CONNECTING_CONNECT);
                }
            }
            else
            {
                // N/A
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHz", "データ送信応答(その他)");
            }
        }

        /**
         * @brief 初期設定書き込み要求）送信
         * return 
         */
        public void SendInitMsg()
        {
            MsgSubGHz msgh = new MsgSubGHz();
            msgh.MsgId = 0x2A;
            byte[] id_0xff = { 0xFF, 0xFF, 0xFF, 0xFF };
            msgh.MsgNo = GetMsgNo(msgh.MsgId);
            msgh.SrcID = id_0xff;
            msgh.DstID = id_0xff;
            msgh.Parameter = new byte[23];	// モジュール設定書き込みの場合23byte
            msgh.Parameter[0] = 0x03; // Power
            if (Program.m_SubGHzConfig.Power != null)
            {
                try
                {
                    msgh.Parameter[0] = byte.Parse(Program.m_SubGHzConfig.Power);
                }
                catch
                {
                    msgh.Parameter[0] = 0x03;
                }
            }

            msgh.Parameter[1] = m_Channel; // 25; // Channel

            msgh.Parameter[2] = 0x07; // RF_Baud
            if (Program.m_SubGHzConfig.RF_Baud != null)
            {
                try
                {
                    msgh.Parameter[2] = byte.Parse(Program.m_SubGHzConfig.RF_Baud);
                }
                catch
                {
                    msgh.Parameter[2] = 0x07;
                }
            }
            msgh.Parameter[3] = 0x05; // CS_Mode
            if (Program.m_SubGHzConfig.CS_Mode != null)
            {
                try
                {
                    msgh.Parameter[3] = byte.Parse(Program.m_SubGHzConfig.CS_Mode);
                }
                catch
                {
                    msgh.Parameter[3] = 0x05;
                }
            }
            msgh.Parameter[4] = 0x01; // Cmd_Enable
            msgh.Parameter[5] = 0x01; // Rsp_Enable
            msgh.Parameter[6] = 0x00; // Retry_Count
            if (Program.m_SubGHzConfig.Retry_Count != null)
            {
                try
                {
                    msgh.Parameter[6] = byte.Parse(Program.m_SubGHzConfig.Retry_Count);
                }
                catch
                {
                    msgh.Parameter[6] = 0x00;
                }
            }
            msgh.Parameter[7] = 0x08; // Uart_Baud
            msgh.Parameter[8] = 0x00; // Sleep_Time
            msgh.Parameter[9] = 0xFF; // Rcv_Time 1
            msgh.Parameter[10] = 0xFF; // Rcv_Time 2
            msgh.Parameter[11] = 0xFF; // Forward_ID1 1
            msgh.Parameter[12] = 0xFF; // Forward_ID1 2
            msgh.Parameter[13] = 0xFF; // Forward_ID1 3
            msgh.Parameter[14] = 0xFF; // Forward_ID1 4
            msgh.Parameter[15] = 0xFF; // Forward_ID2 1
            msgh.Parameter[16] = 0xFF; // Forward_ID2 2
            msgh.Parameter[17] = 0xFF; // Forward_ID2 3
            msgh.Parameter[18] = 0xFF; // Forward_ID2 4
            msgh.Parameter[19] = 0xFF; // System_ID 1
            msgh.Parameter[20] = 0xFF; // System_ID 2
            msgh.Parameter[21] = 0xFF; // Product_ID 1
            msgh.Parameter[22] = 0xFF; // Product_ID 2
            msgh.encode();

            byte[] buf = msgh.encodedData;
            Send(buf);
        }

        public void SetChannel(byte ch)
        {
            m_Channel = ch;
        }
        public byte GetChannel()
        {
            return m_Channel;
        }

        public string GetPortName()
        {
            return m_PortName;
        }

        /**
         * @brief DEVICE_ID 取得要求（デフォルト設定要求）送信
         * return 
         */
        public void GetDeviceID()
        {
            MsgSubGHz msgh = new MsgSubGHz();
            msgh.MsgId = MSGID_GETID_7D;
            byte[] id_0xff = { 0xFF, 0xFF, 0xFF, 0xFF };
            msgh.MsgNo = GetMsgNo(MSGID_GETID_7D);
            msgh.SrcID = id_0xff;
            msgh.DstID = id_0xff;
            msgh.Parameter = new byte[1];
            msgh.Parameter[0] = 0x00; // Reserved
            msgh.encode();

            byte[] buf = msgh.encodedData;
            Send(buf);
        }

        /**
         * @brief デバイス検索要求送信
         * return 
         */
        public void SearchDevice(int searchmode)
        {
            MsgSubGHz msgh = new MsgSubGHz();
            msgh.MsgId = MSGID_SEARCH_10;
            byte[] id_0xff = { 0xFF, 0xFF, 0xFF, 0xFF };
            msgh.MsgNo = GetMsgNo(MSGID_SEARCH_10);
            msgh.SrcID = id_0xff;
            msgh.DstID = id_0xff;
            msgh.Parameter = new byte[1];
            msgh.Parameter[0] = 0x00; // Rsp 0x00(受信時再送終了)
            if (searchmode == 1)
            {
                msgh.Parameter[0] = 0x01; // Rsp 0x01(複数検索)
            }
            msgh.encode();

            byte[] buf = msgh.encodedData;
            Send(buf);
        }

        /** 初回疎通確認 */
        void SendInitData()
        {
            try
            {
                MsgSubGHz sndData = new MsgSubGHz();
                sndData.MsgId = MSGID_SEND_11;
                sndData.MsgNo = GetMsgNo(sndData.MsgId);

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

                byte[] tmpbyte = { 0 };
                sndData.Parameter = tmpbyte;
                sndData.encode();

                Send(sndData.encodedData);
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "SubGHz", "SendInitData Err " + ex.Message, "");
            }
        }


        /******************* テスト送受信 ******************/

        /**
         * @brief テスト送信開始
         * return 
         */
        public void StartTestSend()
        {
            StartTestSend("");
        }
        public void StartTestSend(string data) // data : テスト送信データ内容の予定
        {
            m_StatSubGHz = statSubGHz.TestSending;
            m_TSCount = 0;
        }
        /**
         * @brief テスト送信停止
         * return 
         */
        public void StopTestSend()
        {
            m_StatSubGHz = statSubGHz.Connected;
        }

        /**
         * @brief テスト受信開始
         * return 
         */
        public void StartTestRecv()
        {
            m_StatSubGHz = statSubGHz.TestRecving;
            m_TSCount = 0;
        }
        /**
         * @brief テスト送信停止
         * return 
         */
        public void StopTestRecv()
        {
            m_StatSubGHz = statSubGHz.Connected;
        }

        /**
         * @brief テスト送信イベント処理
         */
        private void OnTestSend(byte[] indata)
        {
            MsgSubGHz recvdata = new MsgSubGHz();
            recvdata.encodedData = indata;
            recvdata.decode();

            // 送信＆問い合わせ応答 MsgID=0x00(OK) 0x01(NG)
            if (recvdata.MsgId == MSGID_ACK_00) // && 送ったMsgID==7D) MsgNoから
            {
                // DEVICE_ID 取得
                if (m_TableMsgID[recvdata.MsgNo] == MSGID_GETID_7D)
                {
                    string sDevice = recvdata.makeMsg7dDeviceID();
                    // 設定読み出し応答　DEVICE_ID 取得
                    OnTestEvent(0, sDevice, 3);
                }
                // データ送信応答
                else if (m_TableMsgID[recvdata.MsgNo] == MSGID_SEND_11)
                {
                    // 送信応答
                    OnTestEvent(0, "", TEST_SEND_MODE);
                }
                else
                {
                    // N/A
                }
                // ACK待ち状態に通知
                instance.m_notifyEventRecv.Set();

                return;
            }
            // NACK　送信＆問い合わせ応答 MsgID=0x00(OK) 0x01(NG)
            else if (recvdata.MsgId == MSGID_NACK_01)
            {
                if (m_TableMsgID[recvdata.MsgNo] == MSGID_SEND_11)
                {
                    // 送信応答
                    OnTestEvent(0, "メッセージ送信エラー(NACK)", TEST_SEND_MODE);
                }
                else
                {
                    OnTestEvent(0, "メッセージ送信エラー ID(" + m_TableMsgID[recvdata.MsgNo] + ")", TEST_SEND_MODE);
                }
                // ACK待ち状態に通知
                instance.m_notifyEventRecv.Set();
            }
            // データ受信
            else if (recvdata.MsgId == MSGID_SEND_11)
            {
                // テスト送信中にデータ受信
                OnTestEvent(0, "データ受信", TEST_SEND_MODE);
            }
            // 再送完了
            else if (recvdata.MsgId == 0x12)
            {
                //OnEvent(0x12, "", 2);
                OnTestEvent(0, "(0x12) 再送しました", TEST_SEND_MODE);
                m_SendRetry = true;
                // ACK待ち状態に通知
                instance.m_notifyEventRecv.Set();
            }
            else
            {
                // N/A
            }

            if (indata[0] == 0)
            {
                // 予定外の応答
                OnTestEvent(m_TSCount, "メッセージ送信エラー " + indata.Length, TEST_SEND_MODE);
                return;
            }
            else
            {
                OnTestEvent(m_TSCount, "", TEST_SEND_MODE);

                m_TSCount++;
            }
        }
        /**
         * @brief テスト受信イベント処理
         */
        private void OnTestRecv(byte[] indata)
        {
            MsgSubGHz recvdata = new MsgSubGHz();
            recvdata.encodedData = indata;
            recvdata.decode();
            if (indata[0] == 0) 
            {
                // 予定外の応答
                OnTestEvent(m_TSCount, "Error " + indata.Length, TEST_RECV_MODE);
                return;
            }
            else if (recvdata.MsgId == MSGID_ACK_00)
            {
                // デバイス検索応答
                if (m_TableMsgID[recvdata.MsgNo] == MSGID_SEARCH_10)
                {
                    string msg = recvdata.makeMsg10();
                    setRssi(msg);
                    OnEvent(MSGID_SEARCH_10, msg, 2);
                }
                // 何かのACK
                else
                {
                    OnTestEvent(m_TSCount, "メッセージ受信エラー(MSG_NO:" + recvdata.MsgNo + ")", TEST_RECV_MODE);

                    // ACK待ち状態に通知
                    instance.m_notifyEventRecv.Set();
                }
            }
            else if (recvdata.MsgId == MSGID_NACK_01)
            {

                // 何かのNACK
                OnTestEvent(m_TSCount, "メッセージ受信エラー(MSG_NO:" + recvdata.MsgNo + ")", TEST_RECV_MODE);

                // ACK待ち状態に通知
                instance.m_notifyEventRecv.Set();
            }
            // データ受信
            else if (recvdata.MsgId == MSGID_SEND_11)
            {
                OnTestEvent(m_TSCount, "", TEST_RECV_MODE);
            }
            // 再送完了
            else if (recvdata.MsgId == 0x12)
            {
                //OnEvent(0x12, "", 2);
                OnTestEvent(m_TSCount, "(0x12) 再送しました", TEST_RECV_MODE);

                // ACK待ち状態に通知
                instance.m_notifyEventRecv.Set();
            }
            else
            {
                OnTestEvent(m_TSCount, "メッセージ受信エラー(" + recvdata.MsgId + ")", TEST_RECV_MODE);
            }

            m_TSCount++;

        }


        // ログ出力用rssi値抜き出し
        private void setRssi(string msg)
        {

            var re = new System.Text.RegularExpressions.Regex(
                    "-[0-9]{1,2}dBm"
                );

            var m = re.Match(msg);
            while (m.Success)
            {
                m_rssis.Add(m.Value);
                m = m.NextMatch();
            }
        }

        // 送信成功回数追加
        public int SucceedCount { set { m_suceedCount = value; } get { return m_suceedCount; } }
        // ログ出力用コード
        public int LogCode { set { m_logCode = value; } get { return m_logCode; } }

        // ログ出力用データテーブル作成
        public void GenerateDataTable()
        {
            // データをクリア
            Program.m_SubGHz.m_dataTable.Clear();
            // 既に列が存在しなければ列を追加
            if (!Program.m_SubGHz.m_dataTable.Columns.Contains("デバイスID"))
            {
                Program.m_SubGHz.m_dataTable.Columns.Add("デバイスID", Type.GetType("System.String"));
                Program.m_SubGHz.m_dataTable.Columns.Add("通信成功回数", Type.GetType("System.String"));
                Program.m_SubGHz.m_dataTable.Columns.Add("RSSI1", Type.GetType("System.String"));
                Program.m_SubGHz.m_dataTable.Columns.Add("RSSI2", Type.GetType("System.String"));
            }

            // 値設定
            string deviceId = Program.m_SubGHzConfig.Dst_ID;
            // 10回以上通信に成功している場合は切り捨てる
            if (SucceedCount >= 10)
            {
                SucceedCount = 10;
            }
            string succeedCount = Program.m_SubGHz.SucceedCount.ToString();
            string rssi1 = Program.m_SubGHz.m_rssis[0];
            string rssi2 = Program.m_SubGHz.m_rssis[1];
            DataRow row = Program.m_SubGHz.m_dataTable.NewRow();

            // 行追加
            row["デバイスID"] = deviceId;
            row["通信成功回数"] = succeedCount;
            row["RSSI1"] = rssi1;
            row["RSSI2"] = rssi2;
            Program.m_SubGHz.m_dataTable.Rows.Add(row);
        }

        // サブギガテストのログをcsvファイルに出力

        public void PutLog2Csv()
        {
            // テーブル生成
            Program.m_SubGHz.GenerateDataTable();
            // パス設定
            var csvPath = "";
            // エンコード設定
            var enc = Encoding.GetEncoding("Shift_JIS");
            // m_logCodeが0の時は(端末から見て)受信テストログ, codeが1の時は送信(端末から見て)テストログを出力
            if (m_logCode == 0)
            {
                csvPath = @".\Log\subghz_receive_result.csv";
                if (!File.Exists(csvPath))
                {
                    using (StreamWriter output = new StreamWriter(csvPath, true, enc))
                    {
                        output.WriteLine("#,端末デバイスID,端末受信成功回数,RSSI1,RSSI2");
                    }
                }
            }
            else
            {
                csvPath = @".\Log\subghz_send_result.csv";
                if (!File.Exists(csvPath))
                {
                    using (StreamWriter output = new StreamWriter(csvPath, true, enc))
                    {
                        output.WriteLine("#,端末デバイスID,端末送信成功回数,RSSI1,RSSI2");
                    }
                }
            }


            using (StreamWriter output = new StreamWriter(csvPath, true, enc))
            {
                // 最初の列はセパレータを書き込まないよう設定
                var separator = "";
                // ログ書込
                foreach (DataRow row in Program.m_SubGHz.m_dataTable.Rows)
                {
                    var builder = new StringBuilder();
                    foreach (DataColumn col in Program.m_SubGHz.m_dataTable.Columns)
                    {
                        builder.Append(separator).Append(row[col.ColumnName]);
                        // 2列目以降の区切り文字を設定
                        separator = ",";
                    }

                    output.WriteLine(builder.ToString());
                }
            }
        }

        /// <summary>
        /// 災害危機通報 送信
        /// </summary>
        /// <param name="reportSend">true=再開,false=停止</param>
        public void SendDisasterReport(bool reportSend)
        {
            try
            {
                // ヘッダ情報生成
                MsgSubGHz sndData = new MsgSubGHz();
                sndData.MsgId = MSGID_SEND_11;
                sndData.MsgNo = GetMsgNo(sndData.MsgId);

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

                // 災害危機通報 停止=0 , 再開=1
                int disasterReport = 0;
                if (reportSend)
                {
                    disasterReport = 1;
                }
                else
                {
                    disasterReport = 0;
                }

                // サブギガデータ部
                byte[] tmpbyte = { 0x61, 0x2A, (byte)disasterReport };
                sndData.Parameter = tmpbyte;

                // データのエンコード
                sndData.encode();

                // 送信
                Send(sndData.encodedData);

                // 災害危機通報　送信済みフラグをOFF
                Program.m_mainForm.m_isNowReceiving = false;
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "SubGHz", "SendDisasterReport Err " + ex.Message, "");
            }
        }

        /// <summary>
        /// 時刻同期通信
        /// </summary>
        public void SendSyncTime()
        {
            try
            {
                // 送信時刻取得
                Program.m_sendTime = DateTime.UtcNow;

                // 送信データ準備
                MsgSubGHz sndData = new MsgSubGHz();
                sndData.MsgId = MSGID_SEND_11;
                sndData.MsgNo = GetMsgNo(sndData.MsgId); 

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

                // サブギガデータ部
                byte[] tmpbyte = { 0x00, 0x50 };
                sndData.Parameter = tmpbyte;

                // データのエンコード
                sndData.encode();

                // 送信
                Send(sndData.encodedData);
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "SubGHz", "SendSyncTime Err " + ex.Message, "");
            }
        }

        /// <summary>
        /// 電文からメッセージフォーマットの種類を判別
        /// </summary>
        /// <param name="mesData">メッセージデータ</param>
        /// <param name="mode">送信/受信</param>
        /// <returns></returns>
        public static string JudgeMessageType(byte[] mesData, int mode)
        {
            // ログレベルがデバッグの時のみ、ログに電文フォーマットの種類を追記する
            if (global::ShelterInfoSystem.Properties.Settings.Default.LogLevel < LogThread.P_LOG_DEBUG)
            {
                return string.Empty;
            }

            string retType = string.Empty;
            string afterTxt = string.Empty;

            if (mode.Equals(MSG_SEND))
            {
                afterTxt = "要求(PC):";
            }
            else if (mode.Equals(MSG_RECV))
            {
                afterTxt = "応答(Q-ANPI):";
            }

            try
            {
                // メッセージの長さが初回疎通通信と同じ且つ送信時は初回通信とみなす
                if (mesData.Length.Equals(MSGLEN_SUBG_CONNECT_SEND) && mode.Equals(MSG_SEND))
                {
                    retType = "初回疎通通信(PC):";
                }
                // メッセージの長さが初回疎通通信より長い(ポートID有)場合
                else if (mesData.Length >= MSGPID_INDEX)
                {
                    if (mesData[MSGID_INDEX] == MSGID_SEND_11)
                    {
                        // ポート番号でメッセージの種類を判別
                        switch (mesData[MSGPID_INDEX])
                        {
                            case MSGPID_EQU_STATUS:
                                retType = "設備ステータス" + afterTxt;
                                break;

                            case MSGPID_FWD_FORMAT:
                                retType = "FWDメッセージ受信" + afterTxt;
                                break;

                            case MSGPID_RTN_FORMAT:
                                retType = "RTNメッセージ送信" + afterTxt;
                                break;

                            case MSGPID_DISASTER_REPORT:
                                //case MSGPID_GET_TERMINAL_INFO:
                                //case MSGPID_DISASTER_REPORT_OPE:
                                // データIDが無い場合は不明
                                if (mesData.Length < MSGDID_INDEX) break;

                                if (mesData[MSGDID_INDEX].Equals(MSGDID_DISASTER_REPORT))
                                {
                                    retType = "災害危機通報受信" + afterTxt;
                                }
                                else if (mesData[MSGDID_INDEX].Equals(MSGDID_GET_TERMINAL_INFO))
                                {
                                    retType = "端末情報取得" + afterTxt;
                                }
                                else if (mesData[MSGDID_INDEX].Equals(MSGDID_DISASTER_REPORT_OPE))
                                {
                                    retType = "災害危機通報受信停止/再開" + afterTxt;
                                }

                                break;

                            case MSGPID_TIME_SYNC:
                                //case MSGPID_SUBG_TEST:
                                if (mesData.Length.Equals(MSGLEN_SUBG_TEST_SEND))
                                {
                                    retType = "サブギガ送受信テスト クライアント発メッセージ(PC):";
                                }
                                else if (mesData.Length.Equals(MSGLEN_SUBG_TEST_RECV) && mesData[MSGDID_INDEX].Equals(MSGDID_SUBG_TEST))
                                {
                                    retType = "サブギガ送受信テスト サーバ発メッセージ(Q-ANPI):";
                                }
                                else if (mesData.Length >= MSGDID_INDEX && mesData[MSGDID_INDEX].Equals(MSGDID_TIME_SYNC))
                                {
                                    retType = "時刻同期" + afterTxt;
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    else if (mesData[MSGID_INDEX] == MSGID_CONNECT && mesData[MSGPID_INDEX] == MSGPID_COM_CONNECT)
                    {
                        retType = "COMポート接続" + afterTxt;
                    }
                    else if (mesData[MSGID_INDEX] == MSGID_GETID_7D && mesData[MSGPID_INDEX] == MSGPID_GET_DEVICE_ID)
                    {
                        retType = "デバイスID取得" + afterTxt;
                    }
                    else if (mesData[MSGID_INDEX] == MSGID_SEARCH_10 && mesData[MSGPID_INDEX] == MSGPID_GET_QANPI_DEVICE_ID)
                    {
                        retType = "端末検索取得" + afterTxt;
                    }
                    else if (mesData[MSGID_INDEX] == MSGID_ACK_00)
                    {
                        retType = "ACK" + afterTxt;
                    }
                    else if (mesData[MSGID_INDEX] == MSGID_RESEND_12)
                    {
                        retType = "メッセージ再送" + afterTxt;
                    }
                }

                // メッセージ判別エラー
                if (retType.Equals(string.Empty)) throw new Exception();

            }
            catch (Exception)
            {
                retType = "(I/F仕様書未記載フォーマット)" + afterTxt;
            }

            return retType;
        }
        
    }
}
