/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2017. All rights reserved.
 */
/**
 * @file    SubGHzRtnThread.cs
 * @brief   SubGHzRTNメッセージ送信スレッド
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
     * @class: SubGHzRtnThread
     * @brief: RTNメッセージ送信スレッド
     */
    public class SubGHzRtnThread 
    {
        public const int RTN_PORT = 3091;

        private bool connected = false;

        public bool mFinish = false;

        /**
         * @brief RTNデータの送信時刻加算秒
         */
        // private const double SENDTIME_ADD_SEC = 7; // 送る直前にセットすること
        // ---> 不要
        /**
         * @brief このプログラムのRTNデータの送信間隔
         */
        private const int SENDWAIT_TIME = 1700;

        /** 
         * @brief 送信時間計算用の送信間隔 
         */
        private const int SEND_INTERVAL = 1600;

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

        //public DateTime m_holdTime = new DateTime(Program.m_nowYear, DateTime.Now.Month, DateTime.Now.Day);
        public DateTime m_holdTime = DateTime.MinValue;

        /// <summary>
        ///　接続チェック
        /// </summary>
        public bool isConnected()
        {
            return connected;
        }

        public void setConnected(bool con)
        {
            connected = con;
            return ;
        }


        public bool checkConnection()
        {
            return connected;
        }

        public void connect()
        {
            /*
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "TcpRtnThread", "SendThread", "ReConnect");
            SBandComCtl sbcc = SBandComCtl.getInstance();

            rtnDataConnect(connectIP, RTN_PORT);
            SBandConnectionInfo sbci = sbcc.getRTNDataConnectionInfo();
            sbci.rcvDataEvent += rcvRtnDataEvent;
             */
        }

        /**
         * @brief RTN要求データを作成・送信する
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

        public void sendMessage(byte[] msgdata)
        {
            sendRequestData(msgdata, 0x91);
        }

        public void sendEvent(object sender, int code, byte[] msg, string outVal)
        {
            EventRtnResp(sender, code, msg, outVal);
        }



        /**
         * @brief Ｓ帯RTN送信応答
         */
        public void rcvRtnDataEvent(object sender, byte[] data)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SubGHzRtnThread", "Recv Data : " + data.Length);

            MsgSBandRtnSendRsp test = new MsgSBandRtnSendRsp();
            test.encodedData = data;

            if (data == null || data.Length == 0)
            {
                Program.m_thLog.PutErrorLog("SubGHzRtnThread", "Send rcvRtnDataEvent. length 0", "");
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
            EventRtnResp(this, code, data,
                "RTN送信時刻(" + min + ":" + sec + "." + msec + ")");
        }

        /// <summary>
        /// 時刻同期要求受信時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        public void rcvTimeSyncEvent(object sender, byte[] data)
        {
            // --- 送信時刻、受信時刻とGPSからの応用時刻から現在時刻を算出し、PCのシステム時刻に設定(変更)する ---
            try
            {
                // 遅延時間から地上施設の現在時刻を算出
                int startPos = 120;     // 時刻部分は120bit目から
                TimeUsec Q_ANPI_RcvTime = new TimeUsec();           //Q-ANPI端末が時刻同期要求を受け取った時刻
                TimeUsec Q_ANPI_SendTime = new TimeUsec();          //Q-ANPI端末が時刻同期応答を送信した時刻
                startPos = Q_ANPI_RcvTime.decode(data, startPos);
                startPos = Q_ANPI_SendTime.decode(data, startPos);

                // 補正時間の計算
                DateTime nowPCTime = DateTime.UtcNow;       // PCの現在時刻(世界標準時)
                DateTime nowPCTimeJST = DateTime.Now;       // PCの現在時刻(日本時間)

                // 案①：TCS提案資料
                // 補正時間(Q-ANPIからPCに電文が到着するまでの時間) = (PC受信時刻 - PC送信時刻)/2
                TimeSpan delayTime = new TimeSpan((nowPCTime.Ticks - Program.m_sendTime.Ticks) / 2);

                //2020.03.19 --->
                //// 案②：IS端末PC間インターフェース仕様書
                //// 補正時間(Q-ANPIからPCに電文が到着するまでの時間) = {(PC受信時刻 - PC送信時刻)-(Q-ANPI送信時刻 - Q-ANPI受信時刻)} / 2
                ////TimeSpan delayTime = new TimeSpan(((nowPCTime.Ticks - Program.m_sendTime.Ticks) - (Q_ANPI_SendTime.getDateTime().Ticks - Q_ANPI_RcvTime.getDateTime().Ticks)) / 2);

                //// GPS補正後時刻の計算(GPS送信時刻 + 補正時間)
                //DateTime revTime = Q_ANPI_SendTime.getDateTime() + delayTime;

                //// 年をまたぐとき、受信した時刻情報には年がないので、＋１する
                //if ((revTime.Month == 1) && (revTime.Day == 1))
                //{
                //    if ((nowPCTime.Month == 12)&&(nowPCTime.Day == 31))
                //    {
                //        Program.m_nowYear++;
                //    }
                //}

                // 前回同期からの経過時間(0.1us) [T2 - T1]
                //long ticksFromLastSync = nowPCTime.Ticks - Program.m_lastTimeSyncTicks;
                // 同期時間(JST)(0.1us) [T3]
                long syncTimeTicks = this.timeUsecToTicks(Q_ANPI_SendTime) + delayTime.Ticks + TimeZoneInfo.Local.BaseUtcOffset.Ticks;

                int year = Program.m_nowYear;

                // 前年の総Tick数
                long prevYearTicks = DateTime.Parse(year.ToString() + "/01/01 00:00:00").Ticks
                    - DateTime.Parse((year - 1).ToString() + "/01/01 00:00:00").Ticks;

                //入力年＋同期時間で日付を求める。(月判定用)
                DateTime tm = DateTime.Parse(year.ToString() + "/01/01 00:00:00");
                tm = tm.AddTicks(syncTimeTicks);

                //　月が下回る場合(＝年が変わった)
                if (tm.Month < Program.m_nowMonth)
                {
                    // 年を加算
                    year += 1;
                    // 前年の総Tick数を更新
                    prevYearTicks = DateTime.Parse(year.ToString() + "/01/01 00:00:00").Ticks
                        - DateTime.Parse((year - 1).ToString() + "/01/01 00:00:00").Ticks;

                    // 前年のTick数を引いて、今年初めからのTickにする。
                    if (syncTimeTicks >= prevYearTicks)
                    {
                        syncTimeTicks -= prevYearTicks;
                    }
                }
                else
                {
                    //入力月が1月且つ、年が変わらず前年を上回る（＝UTCでは去年であるが、JSTでは今年）
                    if (Program.m_nowMonth == 1 && syncTimeTicks >= prevYearTicks)
                    {
                        syncTimeTicks -= prevYearTicks;
                    }
                }

                // 時刻同期実行
                // 同期後時刻を生成(年は初回はログイン画面で設定したものを使用)
                DateTime revTime = DateTime.Parse(year.ToString() + "/01/01 00:00:00");
                revTime = revTime.AddTicks(syncTimeTicks);
                DateTime correctTime = new DateTime(revTime.Year, revTime.Month, revTime.Day,
                                                        revTime.Hour, revTime.Minute, revTime.Second);

                // 同期する時刻に合わせて年・月設定値を更新
                Program.m_nowYear = revTime.Year;
                Program.m_nowMonth = revTime.Month;

                // PCのシステム時刻を同期する
                Microsoft.VisualBasic.DateAndTime.Today = correctTime;
                Microsoft.VisualBasic.DateAndTime.TimeOfDay = correctTime;

                // 時刻同期応答有りフラグをON
                Program.m_mainForm.m_isTimeSyncOK = true;

                Program.m_mainForm.setToolStripStatusLabel(FormShelterInfo.LABEL.TIME_SYNC, "時刻同期済    |");

                Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "rcvTimeSyncEvent", "システム時刻更新", nowPCTimeJST.ToString() + " → " + correctTime.ToString());
                //2020.03.19 <---
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "rcvTimeSyncEvent", "システム時刻更新失敗", "PCのシステム時刻変更時にエラーが発生しました。:" + ex.Message);
            }
            Program.m_mainForm.updateActiveShelterInfo();
            Program.m_mainForm.UpdateDisplay();
        }

        /// <summary>
        /// 災害危機通報イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        public void rcvDisasterReportEvent(object sender, byte[] data)
        {
            // --- 地上端末側の現在の災害危機通報送信状況を取得 ---
            byte DisasterReportFlg = data[15];

            if (DisasterReportFlg == (byte)0)
            {
                // ボタンラベルを「受信再開」に変更し、ボタンを活性化状態にする
                Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "rcvDisasterReportEvent", "災害危機通報受信切り替え", "受信中 → 停止");
                //Program.m_ReceiveMessageView.m_isReceiving = false;
                //Program.m_ReceiveMessageView.updateButtonLabel();
            }
            else
            {
                // ボタンラベルを「受信停止」に変更し、ボタンを活性化状態にする
                Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "rcvDisasterReportEvent", "災害危機通報受信切り替え", "停止 → 受信中");
                //Program.m_ReceiveMessageView.m_isReceiving = true;
                //Program.m_ReceiveMessageView.updateButtonLabel();
            }
        }

        //2020.03.18 Add
        /// <summary>
        /// TimeUsec to Ticks
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="year"></param>
        /// <returns>DateTime value</returns>
        private DateTime getDateTime(TimeUsec time, int year)
        {
            DateTime dt = new DateTime();

            // 判定した年の初日のDateTimeを作成
            dt = DateTime.Parse(year.ToString() + "/01/01 00:00:00");

            // 通算日は1オリジンなので(通算日 - 1)を加算して置き換え
            dt = dt.AddDays(time.dayOfYear - 1);

            dt = dt.AddHours(time.hour);
            dt = dt.AddMinutes(time.min);
            dt = dt.AddSeconds(time.sec);
            dt = dt.AddMilliseconds(time.msec);

            return dt;

        }

        //2020.03.18 Add
        /// <summary>
        /// TimeUsec to Ticks
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>Ticks value</returns>
        private long timeUsecToTicks(TimeUsec time)
        {
            // 基準となる年
            // ※差分を求めるための基準にするだけなので、正しい年でなくてもよい。
            int year = DateTime.UtcNow.Year;

            // 基準年の先頭
            DateTime baseDate = DateTime.Parse(year.ToString() + "/01/01 00:00:00");

            // 対象時間(基準年ベース)
            DateTime targetDate = this.getDateTime(time, year);

            return targetDate.Ticks - baseDate.Ticks;
        }
    }
}

