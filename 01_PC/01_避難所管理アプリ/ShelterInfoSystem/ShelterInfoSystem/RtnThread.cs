/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2017. All rights reserved.
 */
/**
 * @file    RtnThread.cs
 * @brief   RTNメッセージ送信スレッド
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

using System.Threading.Tasks;

namespace ShelterInfoSystem
{
    /**
     * @class: RtnThread
     * @brief: RTNメッセージ送信スレッド
     */
    public class RtnThread : ThreadBase
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

        private TcpRtnThread m_TcpRtn = new TcpRtnThread();
        private SubGHzRtnThread m_SGRtn = new SubGHzRtnThread();

        // ECEF 座標系
        //bool ecefReady = false;
        //double mEcefX = 0.0;
        //double mEcefY = 0.0;
        //double mEcefZ = 0.0;

        public const int RTN_PORT = 3091;

        public static int RESULT_TIMEOUT = -2;

        public static int RESULT_SEND_RESTRICT = -3;
        public static int RESULT_FREQ_RESTRICT = -4;

        /** 応答までのタイムアウト */
        public const int RTN_TIMEOUT_SEC = 30; // 6 - 30

        /** 無効値の定義 */
        private const double UNAVAILABLE_VALUE = -1.0;
        private const double INVALID_VALUE = -1.0;

        //// 伝播遅延時間計算用
        /** 光速 [m/s] */
        private const double LIGHT_SPEED =  (299792458.0);

        /** ECEF直交座標XYZ基準距離 [km] */
        private const double ECEF_BASE_RANGE = (-52428.8);
        private const double PI = (3.141592);

        /** 角度数値定義 */
        private const double DEG_0 = (0.0000);
        private const double DEG_90 = (90.0000);
        private const double DEG_180 = (180.0000);

        /** 楕円体扁平率 */
        private const double FLATTENING_VAL = (0.003352811);
        /** 赤道半径 [m] */
        private const double EQUATORIAL_RADIUS = (6378137.0);
        
        /** us→ns/us→ms 単位変換用 */
        private const double CHANGING_UNIT = 1000.0;
        private const double NUM_1000000 = 1000000.0;

        /** 今年＝０ */
        private const int THIS_YEAR = 0;
        
        private static int m_timeCount = 0;                                // タイマーカウント
        /** m_waiting = false でタイムアウトキャンセル */
        public static bool m_waiting = false;
        private static int m_waitCount = 0;

        public bool mFinish = false;

        /**
         * @brief RTNデータの送信時刻加算秒
         */
        // private const double SENDTIME_ADD_SEC = 8; // 送る直前にセットすること
        private const double SENDTIME_ADD_SEC = 16; // for 8001対策

        /**
         * @brief このプログラムのRTNデータの送信間隔
         */
        private const int SENDWAIT_TIME = 1600; // 2000

        /**
         * @brief 連続送信10件後の休止時間
         */
        private const int RENZOKUWAIT_TIME = 6000;

        /** 
         * @brief 送信時間計算用の送信間隔 (16s)
         */
        private const int SEND_INTERVAL = 16000;

        /** 
         * @brief サブスロット単位計算用の送信間隔 (1.6s)
         */
        static int SUBSLOT_INTERVAL = 1600;
        
        /**
         * @brief Ｓ帯モニタデータ（Ｓ帯RTNデータ送信要求）
         */
        private MsgSBandRtnSendReq fixedSBandRtnSendReq = new MsgSBandRtnSendReq();

        // 送信キュー 
        private readonly Queue<byte[]> m_sendQueue = new Queue<byte[]>();

        // 送信キューの変化を通知してくれるイベント 
        private readonly ManualResetEvent m_notifyEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent m_notifyEventRecv = new ManualResetEvent(false);
        // 送信ロック
        private readonly object m_sendLock = new object();

        // ランダム値（連続送信中は一定）
        private int m_randomVal = 0;

        // RTNコードから内容の文字列へ（ログ表示用）
        public static string getRTNMsg(int code)
        {
            string smsg = "";
            switch (code)
            {
                case 8001:
                    smsg = "送信時刻不正";
                    break;
                case 8002:
                    smsg = "PN不正";
                    break;
                case 8003:
                    smsg = "周波数不正";
                    break;
                case 8004:
                    smsg = "要求超過";
                    break;
                case 8005:
                    smsg = "TDMA同期外れ";
                    break;
                case 8006:
                    smsg = "システム情報不正";
                    break;
                case 8007:
                    smsg = "温度異常";
                    break;
                case 8008:
                    smsg = "保守中";
                    break;
                case 8009:
                    smsg = "モデム不可";
                    break;
                case 8010:
                    smsg = "GPS時刻未取得";
                    break;
                case 8011:
                    smsg = "現在地未取得";
                    break;
                case 8012:
                    smsg = "PLLアラーム";
                    break;
                case 8013:
                    smsg = "キャリア送受信無効";
                    break;
                case 8014:
                    smsg = "メッセージ送信規制あり";
                    break;
                default:
                    break;            
            }
            return smsg;

        }

        /**
         * @brief RTN応答デリゲート
         */
        public delegate void EventRtnDelegate(object sender, int code, byte[] msg, string outVal);
        public event EventRtnDelegate EventRtnResp
        {
            add
            {
                if (m_Mode == Mode.TCPIP)
                {
                    m_TcpRtn.EventRtnResp += value;
                }
                else
                {
                    // SubGHZ
                    m_SGRtn.EventRtnResp += value;
                    Program.m_SubGHz.rcvRtnDataEvent += new SubGHz.EventRtnDelegate(m_SGRtn.rcvRtnDataEvent);
                }
            }
            remove
            {
                if (m_Mode == Mode.TCPIP)
                {
                    m_TcpRtn.EventRtnResp -= value;
                }
                else
                {
                    // SubGHZ
                    m_SGRtn.EventRtnResp -= value;
                    Program.m_SubGHz.rcvRtnDataEvent -= new SubGHz.EventRtnDelegate(m_SGRtn.rcvRtnDataEvent);
                }
            }
            
        }
        /// <summary>
        /// 時刻同期要求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="outVal"></param>
        public delegate void EventTimeSyncDelegate(object sender, int code, byte[] msg, string outVal);
        public event EventTimeSyncDelegate EventTimeSync
        {
            add
            {
                if (m_Mode == Mode.SUBGHZ)
                {
                    // SubGHZ
                    Program.m_SubGHz.rcvTimeSyncEvent += new SubGHz.EventTimeSyncDelegate(m_SGRtn.rcvTimeSyncEvent);
                }
            }
            remove
            {
                if (m_Mode == Mode.SUBGHZ)
                {
                    // SubGHZ
                    Program.m_SubGHz.rcvTimeSyncEvent -= new SubGHz.EventTimeSyncDelegate(m_SGRtn.rcvTimeSyncEvent);
                }
            }

        }

        /// <summary>
        /// 災害危機通報
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="outVal"></param>
        public delegate void EventDisasterReportDelegate(object sender, int code, byte[] msg, string outVal);
        public event EventDisasterReportDelegate EventDisasterReport
        {
            add
            {
                if (m_Mode == Mode.SUBGHZ)
                {
                    // SubGHZ
                    Program.m_SubGHz.rcvDisasterReportEvent += new SubGHz.EventDisasterReportDelegate(m_SGRtn.rcvDisasterReportEvent);
                }
            }
            remove
            {
                if (m_Mode == Mode.SUBGHZ)
                {
                    // SubGHZ
                    Program.m_SubGHz.rcvDisasterReportEvent -= new SubGHz.EventDisasterReportDelegate(m_SGRtn.rcvDisasterReportEvent);
                }
            }

        }

        public event EventRtnDelegate EventSendStart;

        public RtnThread()
        {
            // タイマ起動
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(OnTimer);
            timer.Interval = 1000;
            timer.Start();
        }

        /**
         * @brief IPアドレス設定(文字列指定)
         * @param IPアドレス
         * @return 結果
         */
        public override void Run()
        {
            SendThread();
        }

        /**
         * @brief スレッド終了処理
         */
        public override void Exit()
        {
            if (m_Thread != null)
            {
                m_Thread.Interrupt();
            }
            // 親クラスのメインスレッド処理
            base.Exit();
        }

        /// <summary>
        ///　タイマー処理
        /// </summary>
        private void OnTimer(object sender, EventArgs e)
        {
            // 送信応答までのタイムアウト（送達確認でなく）
            m_timeCount++;

            if (m_waiting && m_timeCount >= m_waitCount)
            {
                m_waiting = false;
                // タイムアウト
                sendEvent(this, RESULT_TIMEOUT, null,
                    "RTN応答待ち タイムアウト ");
            }
        }

        /// <summary>
        ///　タイムアウトセット
        /// </summary>
        public void setTimeOut(int count)
        {
            m_waitCount = count;
            m_timeCount = 0;
            m_waiting = true;
        }

        /** タイムアウトしない */
        public void setWaiting(bool onoff)
        {
            m_waiting = onoff;
        }

        /// <summary>
        ///　接続チェック
        /// </summary>
        public bool isConnected()
        {
            if (m_Mode == Mode.TCPIP)
            {
                return m_TcpRtn.isConnected();
            }
            else
            {
                return m_SGRtn.isConnected();
            }
        }

        public void setConnected(bool con)
        {
            if (m_Mode == Mode.TCPIP)
            {
                m_TcpRtn.setConnected(con);
            }
            else
            {
                m_SGRtn.setConnected(con);
            }
        }

        /// <summary>
        ///　IPセット
        /// </summary>
        public static Result setIP(string ip)
        {
            return TcpRtnThread.setIP(ip);
        }

        /**
         * @brief IPアドレス設定(Byte配列指定)
         * @param IPアドレス
         * @return 結果
         */
        public static Result setIP(byte[] ip)
        {
            return TcpRtnThread.setIP(ip);
        }

        public bool checkConnection()
        {
            if (m_Mode == Mode.TCPIP)
            {
                return m_TcpRtn.checkConnection();
            }
            else
            {
                return m_SGRtn.checkConnection();
            }
        }

        public void connect()
        {
            if (m_Mode == Mode.TCPIP)
            {
                m_TcpRtn.connect();
            }
            else
            {
                m_SGRtn.connect();
            }
        }

        public void sendMessage(byte[] msgdata)
        {
            if (m_Mode == Mode.TCPIP)
            {
                m_TcpRtn.sendMessage(msgdata);
            }
            else
            {
                m_SGRtn.sendMessage(msgdata);
            }
        }

        public void sendEvent(object sender, int code, byte[] msg, string outVal)
        {
            if (m_Mode == Mode.TCPIP)
            {
                m_TcpRtn.sendEvent(sender, code, msg, outVal);
            }
            else
            {
                m_SGRtn.sendEvent(sender, code, msg, outVal);
            }
        }

        public int getQueCount()
        {
            return this.m_sendQueue.Count;

        }

        // キューのクリア
        public void ClearSendQue()
        {
            //lock (this.m_sendLock)
            {
                this.m_sendQueue.Clear();
                this.m_notifyEvent.Set();
            }
        }

        // 送信：送信キューに追加
        public void AddSendQue(byte[] senddata)
        {
            // タスクを追加 
            lock (this.m_sendLock)
            {
                this.m_sendQueue.Enqueue(senddata);
            }
            // 知らせる
            this.m_notifyEvent.Set();
        }

        // 送信スレッドループ
        private void SendThread()
        {
            // 送信
            while (mFinish == false)
            {
                try
                {
                    // キューへの追加を待つ 
                    this.m_notifyEvent.WaitOne();
                    this.m_notifyEvent.Reset();

                    setConnected(checkConnection());

                    if (this.m_sendQueue.Count == 0)
                    {
                        // m_notifyEventのみでキューなし
                        continue;
                    }

                    // ランダム値更新
                    m_randomVal = new Random().Next(100);
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "RtnThread", "ランダム値 m_randomVal:" + m_randomVal);

                    // 接続切れてる
                    if (isConnected() == false)
                    {
                        connect();
                    }

                    bool renzoku = false;
                    int renzokucount = 0;
                    int restrict = 0;
                    while (mFinish == false)
                    {
                        // 送信中 
                        if (this.m_sendQueue.Count == 0)
                        {
                            // 送信完了
                            break;
                        }
                        // 
                        byte[] indata = this.m_sendQueue.Dequeue();
                        makeSBandRtnSendReq(indata, renzoku);
                        byte[] msgdata = fixedSBandRtnSendReq.encodedData;
                        // デバグログ
                        string writebuf = "";
                        for (int i = 0; i < msgdata.Length; i++)
                        {
                            string hsin = Convert.ToString(msgdata[i], 16);
                            hsin = hsin.PadLeft(2, '0');
                            writebuf += hsin + " ";
                        }
                        // メッセージ制限
                        if (Program.m_FwdRecv.isSendRestrict())
                        {
                            // type1とtype3はエラー
                            byte type = (byte)((indata[0] >> 6) & 0x03);
                            if (type == 1 || type == 3)
                            {
                                restrict = RESULT_SEND_RESTRICT;
                                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "RtnThread", "メッセージ制限で送信不可 Type" + type);
                                break;
                            }
                        }
                        // 送信周波数制限
                        if (Program.m_FwdRecv.isFreqRestrict())
                        {
                            // 開始周波数=15の場合、送信不可状態
                            restrict = RESULT_FREQ_RESTRICT;
                            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "RtnThread", "周波数制限で送信不可");
                            break;
                        }

                        // 送信予定時刻を見て、30秒以上前に送らないように
                        DateTime sendT = fixedSBandRtnSendReq.rtnSendInfo.sendTime.getDateTime();
                        // TEST
                        Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "calcSendTime", "sendT=" + sendT.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                        if (sendT >= DateTime.UtcNow + new TimeSpan(0, 0, (int)SENDTIME_ADD_SEC + 2)) // 30ぴったりだと8001
                        {
                            // 未来過ぎる
                            renzokucount = 0;
                            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "RtnThread", "SendThread", "送信時刻 utc:" + sendT.ToLongTimeString() + "まで待ちます。");
                            bool okflg = false;
                            while (!okflg)
                            {
                                setWaiting(false);
                                System.Threading.Thread.Sleep(SENDWAIT_TIME);
                                if (sendT < DateTime.UtcNow + new TimeSpan(0, 0, (int)SENDTIME_ADD_SEC + 2))
                                {
                                    okflg = true;
                                }
                                makeSBandRtnSendReq(indata, false);
                                sendT = fixedSBandRtnSendReq.rtnSendInfo.sendTime.getDateTime(); // 送信時刻計算しなおし
                            }

                            msgdata = fixedSBandRtnSendReq.encodedData;
                        }
                        Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SendThread", "送信時刻(" +
                        sendT.Minute + ":" + sendT.Second + "." + sendT.Millisecond + ")");
                        Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "RtnThread", "SendThread", writebuf);

                        int code = 0;
                        string msg = "送信時刻(" + sendT.Minute + ":" + sendT.Second + "." + sendT.Millisecond + ")";
                        try
                        {
                            if (isConnected() == false)
                            {
                                connect();
                            }
                            sendMessage(msgdata);
                        }
                        catch(Exception e)
                        {
                            code = -1;
                            msg = "SEND 失敗 " +  " : " + e.Message ;

                            Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "RtnThread", "SendThread", msg);
                            break;

                        }
                        // 送信したことを通知
                        EventSendStart(this, code, msgdata, msg);

                        // 送信間隔
                        System.Threading.Thread.Sleep(SENDWAIT_TIME);

                        //次の送信キューを見る
                        if (this.m_sendQueue.Count == 0)
                        {
                            // 送信完了
                            break;
                        }
                        else
                        {
                            // 連続送信
                            renzoku = true;
                            renzokucount++;
                            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "RtnThread", "SendThread", "連続送信中 count:" + renzokucount);
                        }
                        if (renzokucount >= 10)
                        {
                            // 休む
                            setWaiting(false);
                            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "RtnThread", "SendThread", "連続送信10件に達したので休止します");
                            System.Threading.Thread.Sleep(RENZOKUWAIT_TIME);
                            renzokucount = 0;
                        }
                    }

                    if (restrict == 0)
                    {
                        // 送信完了 タイムアウト設定
                        setTimeOut(RTN_TIMEOUT_SEC);
                    }
                    else
                    {
                        // 送信制限時にて送信失敗した場合に通知
                        m_waiting = false;
                        sendEvent(this, restrict, null, "送信制限中");
                    }

                }
                catch (ThreadInterruptedException)
                {
                    //終了
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "RtnThread", "SendThread Interrupted");
                    mFinish = true;
                    break;
                }
                catch (Exception ex)
                {
                    //切断
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "RtnThread", "SendThread", ex.Message);
                    System.Threading.Thread.Sleep(2000);
                    continue;
                }
            }

        }

#if false
        /**
         * @brief RTNデータ取得要求送信
         */
        public override Boolean TcpReqRes(byte[] inData, out int code, out string errMsg, out string outVal)
        {    
            // アクセス制限 メッセージ制限
            if (Program.m_FwdRecv.isSendRestrict())
            {
                // type1とtype3はエラー
                byte type = (byte)((inData[0] >> 6) & 0x03);
                if (type == 1 || type == 3)
                {
                    code = 8100;
                    errMsg = "混雑によりアクセス制限中です";
                    outVal = "";
                    sendEvent(this, code, null, errMsg);
                    return true;
                }
            }

            // 送信
            code = 0;
            errMsg = "";
            outVal = "";

            Boolean bRet = true;
            try
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "RtnThread", "Send Data : " + inData);            
                // 
                makeSBandRtnSendReq(inData);
                byte[] msgData = fixedSBandRtnSendReq.encodedData;

                if (msgData != null)
                {
                    SetTimeOut(RTN_TIMEOUT_SEC);

                    // 送信
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "RtnThread", "Send time " 
                        + fixedSBandRtnSendReq.rtnSendInfo.sendTime.min + ":"
                        + fixedSBandRtnSendReq.rtnSendInfo.sendTime.sec);

                    AddSendQue(inData);
                }
            }
            catch (Exception ex)
            {
                // エラーコード
                code = 10;
                errMsg = ex.Message;
                bRet = false;
                string sLog = String.Format("ERR CODE {0}, Err {1}, Data {2} ", code, errMsg, inData);
                Program.m_thLog.PutErrorLog("RtnThread", "Send Faild.", sLog);
            }
            finally
            {

            }

            return bRet;
        }
#endif

        /** 
         * @brief 送信予定時間、送信タイミング算出
         *
         */
        private DateTime calcSendTime(bool renzoku)
        {
            DateTime dt = DateTime.UtcNow;
            dt = dt.AddSeconds(SENDTIME_ADD_SEC); // いまから「SENDTIME_ADD_SEC」秒後
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "calcSendTime", "now (" +
                dt.Minute + ":" + dt.Second + "." + dt.Millisecond + ")");

            SystemInfo sysInfo = Program.m_FwdRecv.m_sysInfo;
#if false
            // TEST DATA start
            if (sysInfo != null)
            {
                sysInfo.sysGroupNum = 1;
                sysInfo.sysRandomSelectBand = 1;
            }
            // TEST DATA end
#endif
            if (sysInfo == null
                || sysInfo.sysRandomSelectBand == 0
                || sysInfo.sysGroupNum == 0 )
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "calcSendTime", "SystemInfo null");
                // まだFWDを受け取っていない
                return dt;
            }

            // 伝搬遅延時間は不要（地上局到着時刻をセットするため）
            double td = 0;
#if false
            // 伝搬遅延時間を取得する. usをmsに変換
            double td = getDelayTime(sysInfo.sysDelayTime,
                    sysInfo.sysSatellitePosX, sysInfo.sysSatellitePosY,
                    sysInfo.sysSatellitePosZ) / CHANGING_UNIT;
            if (td < 0)
            {
                // 伝搬遅延時間が負数の場合は、GPS情報なしのため、送信しない？
                td = 0;
            }
#endif
            // GIDを計算
            string qcid = Program.m_EquStat.mQCID;
            int startFreq = sysInfo.sysStartFreqId;
            int endFreq = sysInfo.sysEndFreqId;

            int gid = Qcid.convGID(qcid, startFreq, endFreq);
            Program.m_EquStat.mGID = gid;

            // 平常時
            if (sysInfo.sysGroupNum == 1 && sysInfo.sysRandomSelectBand == 1)
            {
                // 任意の時刻（ただし1.6秒単位の旧ロジック）
                long Ms_now = getNowMs(0) + (long)(SENDTIME_ADD_SEC * 1000) + SUBSLOT_INTERVAL;//+ 1600; // ms
                long Ms_n = (long)(Ms_now / 1600) ;
                long Ms_uid = Ms_n * 1600;

                // MOD-S
                //                DateTime dt_send = new DateTime(Ms_uid * 10000, DateTimeKind.Utc); // ticks = 100ナノ秒（0.0000001秒）
                DateTime dt_now = DateTime.UtcNow;
                DateTime dt_thisYearJan1 = new DateTime(dt_now.Year, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                DateTime dt_send = new DateTime(Ms_uid * 10000 + dt_thisYearJan1.Ticks, DateTimeKind.Utc); // ticks = 100ナノ秒（0.0000001秒）
                // MOD-E

                dt = new DateTime(DateTime.UtcNow.Year, dt_send.Month, dt_send.Day,
                    dt_send.Hour, dt_send.Minute, dt_send.Second, dt_send.Millisecond, DateTimeKind.Utc);
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "calcSendTime", "uid (" +
                dt.Minute + ":" + dt.Second + "." + dt.Millisecond + ")");
                return dt;
            }

            // 連続送信時
            DateTime dt_renzoku = dt;
            if (renzoku)
            {
                // 任意の時刻（ただし1.6秒単位の旧ロジック）
                long Ms_now = getNowMs(0) + (long)(SENDTIME_ADD_SEC * 1000) + SUBSLOT_INTERVAL; // ms
                long Ms_n = (long)(Ms_now / 1600);
                long Ms_uid = Ms_n * 1600;

                // MOD-S
                // DateTime dt_send = new DateTime(Ms_uid * 10000, DateTimeKind.Utc); // ticks = 100ナノ秒（0.0000001秒）
                DateTime dt_now = DateTime.UtcNow;
                DateTime dt_thisYearJan1 = new DateTime(dt_now.Year, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                DateTime dt_send = new DateTime(Ms_uid * 10000 + dt_thisYearJan1.Ticks, DateTimeKind.Utc); // ticks = 100ナノ秒（0.0000001秒）
                // MOD-E

                dt_renzoku = new DateTime(DateTime.UtcNow.Year, dt_send.Month, dt_send.Day,
                    dt_send.Hour, dt_send.Minute, dt_send.Second, dt_send.Millisecond, DateTimeKind.Utc);
                
                dt = dt.AddSeconds(SEND_INTERVAL/1000);
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "calcSendTime", "uid (" +
                dt.Minute + ":" + dt.Second + "." + dt.Millisecond + ")");
                

            }

            // 以下はアクセス制限時
            // ”送信グループ数”または”送信スロットランダム選択幅”の少なくとも一方が「1」以外の値

            // FWD のシステム情報から
            // アクセス制御基準時刻
            int basetime = sysInfo.sysBaseTime;
            //Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "calcSendTime", "basetime (" + basetime + ")");
            long cRTAC = (long)(basetime) * (long)SUBSLOT_INTERVAL;// SEND_INTERVAL;
            //送信スロットランダム選択幅
            int RSTSG = sysInfo.sysRandomSelectBand;
            //送信グループ数
            int TSG = sysInfo.sysGroupNum;;
            //端末発メッセージ送信周期
            int S = SEND_INTERVAL;
            //送信タイミングオフセット
            int Toffset = ((int)(gid % TSG)) * RSTSG * S;
            //0 以上RSTSG 未満の整数値（ランダム）
            //System.Random rand = new Random();
            int R = m_randomVal % sysInfo.sysRandomSelectBand;
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "RtnThread"
                , "R = " + R + "(" + m_randomVal + " % " + sysInfo.sysRandomSelectBand + ")");

            //現在時刻(ms)
            long Tnow = getNowMs(cRTAC);

            Tnow += (long)(SENDTIME_ADD_SEC * 1000); // 8秒後にとっての送信時刻

            //nの計算
            //現在時刻+SENDTIME_ADD_SECms<送信時刻となるnを算出
            //n>(Tnow+td-R*S-RTAC-Toffset)/RSTSG*TSG*S

            if (RSTSG * TSG * S == 0)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "calcSendTime", "(" + 
                    RSTSG +" * "+ TSG + " * " + S + " == 0)");
                return dt;
            }

            long n = (long)(
                    (double)(Tnow + td - R * S - cRTAC - Toffset) / (RSTSG * TSG * S))
                    +1;

            //送信時刻(ms)
            long Tuid = cRTAC + Toffset + (RSTSG * TSG * n + R) * S - (long)td;

            // DateTime型に
            long time = getYearMs();
            int year = DateTime.UtcNow.Year;
            bool thisYearFlag = checkRtacYear(time, cRTAC);
            if (!thisYearFlag)
            {
                year--;
            }
            DateTime sakunen = new DateTime(year, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime gannen = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan elapsedTime = sakunen - gannen;
            long sk = (long)elapsedTime.TotalMilliseconds;
           
            DateTime dtTsend = new DateTime((Tuid+sk) * 10000, DateTimeKind.Utc); // ticks = 100ナノ秒（0.0000001秒）
            // TEST
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "calcSendTime", "dtTsend=" + dtTsend.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            dt = new DateTime(DateTime.UtcNow.Year
                , dtTsend.Month, dtTsend.Day, dtTsend.Hour, dtTsend.Minute, dtTsend.Second, dtTsend.Millisecond, DateTimeKind.Utc);
            // TEST
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "calcSendTime", "dt=" + dt.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            if (renzoku)
            {
                long Tuid2 = cRTAC + Toffset + (RSTSG * TSG * (n-1) + R) * S - (long)td;

                // TEST
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "calcSendTime", "Tuid2=" + Tuid2.ToString());

                // MOD-S
                //DateTime dtTsend2 = new DateTime(Tuid2 * 10000, DateTimeKind.Utc);
                DateTime dtTsend2 = new DateTime((Tuid2+sk) * 10000, DateTimeKind.Utc);
                // MOD-E

                // TEST
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "calcSendTime", "dtTsend2="+ dtTsend2.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                DateTime dt2 = new DateTime(DateTime.UtcNow.Year
                , dtTsend2.Month, dtTsend2.Day, dtTsend2.Hour, dtTsend2.Minute, dtTsend2.Second, dtTsend2.Millisecond, DateTimeKind.Utc);

                // TEST
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "calcSendTime", "dt2=" + dt2.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                // アクセス制限時にいま送っていい時間かチェック
                if (DateTime.UtcNow <= dt2 + new TimeSpan(0, 0, 16 - (int)SENDTIME_ADD_SEC))
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "calcSendTime", "連続送信 送信OK時刻" 
                        +"(" + dt2.Minute + ":" + dt2.Second + "." + dt2.Millisecond + ")");
                }
                else
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "calcSendTime", "連続送信 送信NG時刻"
                        + "(" + dt2.Minute + ":" + dt2.Second + "." + dt2.Millisecond + ")");
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "calcSendTime", "snd_n (" +
                        dt_renzoku.Minute + ":" + dt_renzoku.Second + "." + dt_renzoku.Millisecond + ")");
                    return dt;
                }

                if (dt_renzoku < dt + new TimeSpan(0, 0, 16 - (int)SENDTIME_ADD_SEC))
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "calcSendTime", "snd_r (" +
                        dt_renzoku.Minute + ":" + dt_renzoku.Second + "." + dt_renzoku.Millisecond + ")");
                    return dt_renzoku;
                }
                else
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "calcSendTime", "snd_n (" +
                        dt.Minute + ":" + dt.Second + "." + dt.Millisecond + ")");
                    return dt;
                }
            }

            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "calcSendTime", "snd (" +
                dtTsend.Minute + ":" + dtTsend.Second + "." + dtTsend.Millisecond + ")");

            return dt;
        }

        /**
         * 現在時刻取得要求<br>
         * RTACと比較する現在時刻を算出する<br>
         * RTACが昨年のもののときは、昨年度の経過時刻も加算する
         *
         * @arg cRTAC アクセス制御基準時刻をnsオーダに変換した値
         * @return 経過ミリ秒
         */
        private long getNowMs(long cRTAC) {
            //今年の経過msを算出
            long time = getYearMs();

            //もしRTACが昨年の物だったら、今年の経過msに昨年の年単位の経過ミリ秒を加算する
            bool thisYearFlag = checkRtacYear(time, cRTAC);
            if (!thisYearFlag) {
                time += getLastYearMs();
            }

            return time;
        }

        /**
         * 経過ミリ秒取得要求<br>
         * 今年の1月1日から現時刻までの経過ミリ秒を算出する
         *
         * @return 経過ミリ秒
         */
        private long getYearMs() 
        {
            DateTime ty = DateTime.UtcNow;

            //1970年から現在時刻までの経過ミリ秒を算出
            //カレンダーを秒変換したものと、ミリ秒をたす
            long time = getUnixTime(ty);

            //今年の1月1日までの経過時刻経過秒算出
            //現在時刻までの経過ミリ秒と指定した年の1月1日の差を取る
            int year = ty.Year;

            DateTime pnow = new DateTime(year, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            long timeJan1 = getUnixTime(pnow);

            time -= timeJan1;

            return time;

        }
        /**
         * 昨年度経過ミリ秒取得要求<br>
         * 昨年度の1月1日から12月31までの経過msを算出する
         *
         * @return 昨年度の1月1日から12月31までの経過ms
         */
        private long getLastYearMs() 
        {
            long time;
            time = 0;
            DateTime ty = DateTime.UtcNow;
            int year = ty.Year;

            //昨年の1月1日までの経過時刻経過秒算出
            //今年と昨年の1月1日の差を取る
            DateTime kotoshi = new DateTime(year, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime sakunen = new DateTime(year-1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            long lk = getUnixTime(kotoshi);
            long sk = getUnixTime(sakunen);

            time = lk - sk;

            //経過ミリ秒を返す
            return time;
        }

        /**
         * RTAC年判定チェック要求<br>
         * 現在時刻とRTACの時分秒の通算ミリ秒を比較し、RTACが今年をさしているかを判定する
         * @arg now   現在時刻
         * @arg cRTAC アクセス制御基準時刻をmsオーダに変換した値
         * @retval true 今年
         * @retval false 昨年
         */
        bool checkRtacYear(long now, long cRTAC) {

            //現在時刻とRTACの時分秒の通算ミリ秒を比較
            if (now > cRTAC) { 
                return true;
            } else {
                return false;
            }
        }

        // 1970/1/1 からのミリセカンド
        private DateTime UNIX_197011 = 
            new DateTime(1970, 1, 1, 0, 0, 0, 0);

        public long getUnixTime(DateTime targetTime)
        {
            // UTC時間に変換
            targetTime = targetTime.ToUniversalTime();

            // 経過時間を取得
            TimeSpan elapsedTime = targetTime - UNIX_197011;

            double test1 = elapsedTime.TotalSeconds;
            double test2 = elapsedTime.TotalMilliseconds;

            // 経過秒数
            return (long)elapsedTime.TotalMilliseconds;// TotalSeconds;
        }
#if false
        /**
         * 伝搬遅延時間要求
         *
         * @param sysDelayTime 地上局・衛星出力端遅延時間。システム情報から取得した値を設定。
         * @param sysSatellitePosX 静止衛星位置軌道情報のX座標。システム情報から取得した値を設定。
         * @param sysSatellitePosY 静止衛星位置軌道情報のY座標。システム情報から取得した値を設定。
         * @param sysSatellitePosZ 静止衛星位置軌道情報のZ座標。システム情報から取得した値を設定。
         * @retval 0以上の値                                           伝搬遅延時間（マイクロ秒）
         * @retval #INVALID_VALUE            位置情報が取得できていない場合
         */
        private double getDelayTime(int sysDelayTime, int sysSatellitePosX,
                int sysSatellitePosY, int sysSatellitePosZ)
        {
            if (ecefReady == false)
            {
                return 0;
            }
            // doubleに変換
            double alignDelayTime = (double) sysDelayTime;

            // 桁合わせ：静止衛星位置軌道情報は「100メートル」なので、メートルに補正
            // 基準補正：“0” →“-52428.8km”、“1,048,575”→“52528.7km”
            double alignSatellitePosX = (sysSatellitePosX * 100)
                    + (ECEF_BASE_RANGE * 1000);
            double alignSatellitePosY = (sysSatellitePosY * 100)
                    + (ECEF_BASE_RANGE * 1000);
            double alignSatellitePosZ = (sysSatellitePosZ * 100)
                    + (ECEF_BASE_RANGE * 1000);
            // ECEFはメートルで通知なので、補正は不要

            double squareX = (mEcefX - alignSatellitePosX)
                    * (mEcefX - alignSatellitePosX);
            double squareY = (mEcefY - alignSatellitePosY)
                    * (mEcefY - alignSatellitePosY);
            double squareZ = (mEcefZ - alignSatellitePosZ)
                    * (mEcefZ - alignSatellitePosZ);
            // 光速は[m/s]なので[m/us]に変換してから算出
            double delay = (Math.Sqrt(squareX + squareY + squareZ)
                    / ((double) LIGHT_SPEED / NUM_1000000)) + alignDelayTime;

            return delay;
        }

        /**
         * ECEF変換要求<br>
         * 緯度経度をECEF系座標に変換する
         */
        private void convertEcef(double mLatitude, double mLongitude)
        {
            double e2 = FLATTENING_VAL * (2 - FLATTENING_VAL);
            double sinLat = Math.Sin(mLatitude * PI / DEG_180);
            double cosLat = Math.Cos(mLatitude * PI / DEG_180);
            double sinLon = Math.Sin(mLongitude * PI / DEG_180);
            double cosLon = Math.Cos(mLongitude * PI / DEG_180);
            double tmpDouble = 1 - e2 * sinLat * sinLat;

            //卯酉線半径
            double N =
            EQUATORIAL_RADIUS / Math.Sqrt(tmpDouble);
            //海抜高度は0として計算
            double h = 0.0;
            mEcefX = (N + h) * cosLat * cosLon;
            mEcefY = (N + h) * cosLat * sinLon;
            mEcefZ = (N + h - e2 * N) * sinLat;

            ecefReady = true;
        }

#endif
        /**
         * @brief データ送信用のＳ帯RTN送信要求作成
         */
        private void makeSBandRtnSendReq(byte[] data11, bool renzoku)
        {
            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            fixedSBandRtnSendReq = new MsgSBandRtnSendReq();
            fixedSBandRtnSendReq.eqId = MsgSBandData.FIXED_EQ_ID;

            DateTime dt = DateTime.UtcNow;
            dt = dt.AddSeconds(SENDTIME_ADD_SEC);

            dt = calcSendTime(renzoku);

            fixedSBandRtnSendReq.rtnSendInfo.sendTime.dayOfYear = dt.DayOfYear;
            fixedSBandRtnSendReq.rtnSendInfo.sendTime.hour = dt.Hour;
            fixedSBandRtnSendReq.rtnSendInfo.sendTime.min = dt.Minute;
            fixedSBandRtnSendReq.rtnSendInfo.sendTime.sec = dt.Second;
            fixedSBandRtnSendReq.rtnSendInfo.sendTime.msec = dt.Millisecond;
            fixedSBandRtnSendReq.rtnSendInfo.sendTime.usec = 0;

            fixedSBandRtnSendReq.rtnSendInfo.pn = 0;
            fixedSBandRtnSendReq.rtnSendInfo.freq = 0; // 送信周波数
            // SF = 端末IDのBF 通信端末でセット予定

            // 11byte データ部分
            int len = Math.Min(data11.Length, fixedSBandRtnSendReq.rtnSendInfo.rtnData.Length);

            Array.Copy(data11, fixedSBandRtnSendReq.rtnSendInfo.rtnData, len);
            fixedSBandRtnSendReq.encode(false);

            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
        }

    }
}

