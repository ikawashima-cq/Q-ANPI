/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2017. All rights reserved.
 */
/**
 * @file    SlotSubSlot.cs
 * @brief   スロット管理(現在どのスロットか＋クリアタイミング調整)
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.IO;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using QAnpiCtrlLib.msg;

namespace ShelterInfoSystem
{
    /**
     * @class SlotSubSlot
     * @brief スロット管理
     */
    public class SlotSubSlot : ThreadBase
    {

        /**
         * メンバ変数
         */
        public delegate void EventSlotDelegate(object sender, int type);
        public event EventSlotDelegate EventTimeOut;

        private static SlotSubSlot instance = new SlotSubSlot();    // インスタンス
        private Mutex m_Mutex = new System.Threading.Mutex();      
        private int m_timeCount = 0;                                // タイマーカウント

        private int[][] m_slotsubslot = new int[3][];
        private int[][] m_old_slotsubslot = new int[3][];
        private int m_old_count = 0;
        private int m_pre_slot = 0;

        private int[][] m_received_slotsubslot = new int[3][];
        private int[][] m_received_old_slotsubslot = new int[3][];

        // Type0 保存値
        public const int TYPE0 = 10;
        public const int TYPE1 = 1;
        public const int TYPE2 = 2;
        public const int TYPE2_0 = 20;
        public const int TYPE2_1 = 21;
        public const int TYPE3 = 3;

        // Type0 サブタイプ
        private int[] m_Seq0 = new int[1000];

        // 送達確認時に参照できる番号
        private int[] m_Seq1 = new int[1000];
        // Type2 シーケンス番号
        private int[] m_Seq2 = new int[1000];

        private object m_lockslot = new object();

        private static int m_tsuchislot = -1;

        /**
         * @brief シングルトン
         */
        private SlotSubSlot()
        {

        }

        /**
         * @brief インスタンス取得
         * @return SlotSubSlotインスタンス
         */
        public static SlotSubSlot GetInstance()
        {
            return instance;
        }

        /**
         * @brief メインスレッド開始処理
         */
        public override void Run()
        {
            // タイマ起動
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(OnTimer);
            timer.Interval = 1600; // 1.6秒ごと
            timer.Start();

            //
            for (int i = 0; i < 3; i++)
            {
                m_slotsubslot[i] = new int[MsgType100.Ack.SUBSLOTBITMAP_SIZE];
                m_old_slotsubslot[i] = new int[MsgType100.Ack.SUBSLOTBITMAP_SIZE];
                m_received_slotsubslot[i] = new int[MsgType100.Ack.SUBSLOTBITMAP_SIZE];
            }

            // 親クラスのメインスレッド処理
            base.Run();
        }

        /**
         * @brief メインスレッド終了処理
         */
        public override void Exit()
        {
            // 親クラスのメインスレッド処理
            base.Exit();
        }

        /// <summary>
        /// タイマー処理
        /// 送達確認タイムアウト
        /// </summary>
        private void OnTimer(object sender, EventArgs e)
        {
            m_timeCount++;
            int min = DateTime.Now.Minute;
            int sec = DateTime.Now.Second;
            int msec = DateTime.Now.Millisecond;

            lock (m_lockslot)
            {
                updateNewSlotSub(min, sec, msec);
            }

            int slot = calcSlot(min, sec, msec);
            int subslot = calcSubSlot(min, sec, msec);

            // タイムアウト
            if (m_old_slotsubslot[slot][subslot] != 0) // 送信応答あり（消しこまれてない）
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SlotSubSlot"
                    , "送達確認タイムアウト slot:" + slot + " sub:" + subslot);
                if (m_tsuchislot != slot)
                {
                    // フォームに通知
                    EventTimeOut(this, m_old_slotsubslot[slot][subslot]);
                    m_tsuchislot = slot;
                }
            }
        }

        /**
         * @brief 現在のスロット取得 
         */
        public int getSlot()
        {
            int min = DateTime.Now.Minute;
            int sec = DateTime.Now.Second;
            int msec = DateTime.Now.Millisecond;

            updateNewSlotSub(min, sec, msec);

            return calcSlot(min, sec, msec);
        }

        /**
         * @brief 現在のサブスロット取得 
         */
        public int getSubSlot()
        {
            return calcSubSlot(DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
        }

        // スロットを分秒ミリ秒から計算する
        public int calcSlot(int min, int sec, int msec)
        {
            int tmpsec = min * 60 * 1000 + sec * 1000 + msec;
            int slot = ((int)(tmpsec / 16000)) % 3;
            return slot;
        }        
        
        // サブスロットを分秒ミリ秒から計算する
        public int calcSubSlot(int min, int sec, int msec)
        {
            int tmpsec = min * 60 * 1000 + sec * 1000 + msec;
            int subslot = (int)((tmpsec % 16000) / 1600);
            return subslot;
        }

        /**
         * @brief スロットの代替わり
         */
        private void updateNewSlotSub()
        {
            int min = DateTime.Now.Minute;
            int sec = DateTime.Now.Second;
            int msec = DateTime.Now.Millisecond;

            updateNewSlotSub(min, sec, msec);
        }
        private void updateNewSlotSub(int min, int sec, int msec)
        {
            int slot = calcSlot(min, sec, msec);
            int subslot = calcSubSlot(min, sec, msec);

            // 念のため古いのを保存
            // スロットが新しくなっていれば前回の同スロット更新
            if (slot != m_pre_slot)
            {
                //Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SlotSubSlot"
                //    , "clear slot:" + slot + " timercount:" + m_timeCount);
                m_old_slotsubslot[slot] = m_slotsubslot[slot];
                m_received_old_slotsubslot[slot] = m_received_slotsubslot[slot];
                m_slotsubslot[slot] = new int[10];
                m_received_slotsubslot[slot] = new int[10];
                m_old_count = m_timeCount;
                m_tsuchislot = -1;
            }

            m_pre_slot = slot;
        }

        /// <summary>
        /// リクエストデータを送った送信の時間（分、秒）でセット
        /// </summary>
        public void setRTNReqSlots(int type, int seq, int min, int sec, int msec)
        {
            // 送信時間の計算
            // スロット、サブスロット取得
            int slot = calcSlot(min, sec, msec);
            int subslot = calcSubSlot(min, sec, msec);

            // スロットの変わり目なら新しいほうに
            lock (m_lockslot)
            {
                updateNewSlotSub();//min, sec, msec);

                m_slotsubslot[slot][subslot] = type;
            }
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SlotSubSlot", "type "
                + type + " slot " + slot + " subslot " + subslot + " " + seq);

            if (type == 0)
            {
                m_slotsubslot[slot][subslot] = TYPE0;
                m_Seq0[slot * 100 + subslot] = seq;
            }
            else if (type == 1)
            {
                // type1 は何かしらの番号を
                m_Seq1[slot * 100 + subslot] = seq;
            }
            else if (type == 2)
            {
                // type2 はシーケンス番号を
                m_Seq2[slot * 100 + subslot] = seq;
            }
            else if (type == 20)
            {
                // type2 はシーケンス番号を
                m_Seq2[slot * 100 + subslot] = seq;
            }
            else if (type == 21)
            {
                // type2 はシーケンス番号を
                m_Seq2[slot * 100 + subslot] = seq;
            }

            else
            {
                // type3はそのまま
                // N/A
            }
        }

        /**
         * @brief 端末から来た送達確認をセット
         * @param[in] databu 送達確認のバイナリデータ
         * @return  string slot 2 sub 10 ビット配列文字列
         */
        public string setFWDRspSlots(byte[] databu)
        {
            string sRet = null;
            // 送達確認
            // 端末IDでスロットサブスロットを取得
            int pos = 0;
            string[] sdata = new string[MsgType100.ACKNUM_MAX + 10];
            int sdatacnt = 0;
            sdata[0] = "";

            // gid2進数文字列に
            string gid02 = Program.ConvGidSendData();

            // 430バイトのうちはじめの19バイトは Type 8ビット システム情報 136ビット 送達確認数 8ビット
            for (int i = 19; i < 430; i++)
            {
                byte chkpos = 0x80;
                for (int j = 0; j < 8; j++)
                {
                    byte chk = databu[i];

                    chk &= chkpos;
                    chkpos /= 2;

                    if (chk != 0)
                    {
                        // bit on
                        sdata[sdatacnt] += "1";
                    }
                    else
                    {
                        sdata[sdatacnt] += "0";
                    }

                    pos++;
                    if (pos == 37)
                    {
                        pos = 0;
                        sdatacnt++;
                        sdata[sdatacnt] = "";
                    }
                }// for j 8
            }// for i 430

            // CID を探す 37bit * 88
            for (int i = 0; i < MsgType100.ACKNUM_MAX; i++)
            {
                if (sdata[i].StartsWith(gid02)) //cid[i] == my_cid)
                {
                    // あった
                    // スロットサブスロット取得
                    string strSlotSubslot = sdata[i];
                    if (sdata[i].Length != 37)
                    {
                        // 37bitじゃない
                        break;
                    }
                    string strSlot = sdata[i].Substring(25, 2);
                    string strSubSlot = sdata[i].Substring(27, 10);

                    // 戻り値（スロットサブスロット）
                    if (sRet == null)
                    {
                        sRet = strSlot + strSubSlot;
                    }
                    else
                    {
                        sRet += "," + strSlot + strSubSlot;
                    }

                    // もう少し詳しく持っておく
                    int slot = 3;// int.Parse(strSlot);
                    if ("00".Equals(strSlot))
                    {
                        slot = 0;
                    }
                    else if ("01".Equals(strSlot))
                    {
                        slot = 1;
                    }
                    else if ("10".Equals(strSlot))
                    {
                        slot = 2;
                    }
                    else
                    {
                        // slot 3
                        Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SlotSubSlot", "slot 3 error " + sdata[i]);
                        continue;
                    }

                    m_received_slotsubslot[slot] = new int[MsgType100.Ack.SUBSLOTBITMAP_SIZE];
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SlotSubSlot", "slot data " + sdata[i]);
                    lock (m_lockslot)
                    {
                        updateNewSlotSub(); // 変わり目なら更新

                        for (int j = 0; j < MsgType100.Ack.SUBSLOTBITMAP_SIZE; j++)
                        {     
                            int subslot = j;
                            if ("1".Equals(strSubSlot.Substring(j,1))) {
                                m_received_slotsubslot[slot][subslot] = 1;
                            }
                            else
                            {
                                m_received_slotsubslot[slot][subslot] = 0;
                            }
                        }
                    }
                }
                else
                {
                    //Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "SlotSubSlot", "gid02 " );
                }
            }

            return sRet;
        }

        // 送達確認が来たものを保存
        public bool setFWDRspSubSlotON(int slot, int received)
        {
            m_received_slotsubslot[slot] = new int[MsgType100.Ack.SUBSLOTBITMAP_SIZE];
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, 
                "SlotSubSlot", "setFWDRspSubSlotON " + slot + " " + received);

            int mask = 0x200; // 10bits
            for (int subslot = 0; subslot < MsgType100.Ack.SUBSLOTBITMAP_SIZE; subslot++)
            {
                if ((received & mask) > 0)
                {
                    m_received_slotsubslot[slot][subslot] = 1;
                }
                else
                {
                    m_received_slotsubslot[slot][subslot] = 0;
                }
                mask >>= 1;
            }

            return true;
        }

        // サブスロットのBitmapをわかりやすい文字列に
        public string getSubSlotString(int subbits)
        {
            string ret = "";
            int chkpos = 0x200;

            for (int j = 0; j < 10; j++)
            {
                int chk = subbits;

                chk &= chkpos;
                chkpos /= 2;

                if (chk != 0)
                {
                    // bit on
                    ret += "1";
                }
                else
                {
                    ret += "0";
                }
            }

            return ret;
        }

        // 
        /// <summary>
        /// 送達確認チェック、来ていたらtrue
        /// </summary>
        public bool isReceived(int code)
        {
            bool bRet = false;
            int type = 0;
            int slot = 0;
            int sub_slot = 0;
            // Dictionary で　code から slot subslot typeを取得
            if(m_slotsubslot[slot][sub_slot] == type
                && m_received_slotsubslot[slot][sub_slot] == 1)
            {
                bRet = true;
            }

            return bRet;
        }

        /**
         * @brief 送達確認のスロットデータからメッセージタイプ取得
         * @param[in] slosub02 2進数文字列
         * @return  type
         */
        public int getMsgType(string slosub02)
        {
            int slot = 0;
            int subslot = 0;
            if (slosub02.Length < 12)
            {
                return -1;
            }
            string strSlot = slosub02.Substring(0, 2);
            string strSubSlot = slosub02.Substring(2, 10);

            try
            {
                slot = int.Parse(strSlot);
                subslot = int.Parse(strSubSlot);
            }
            catch (Exception)
            {
                return -1;
            }
            return getMsgType(slot, subslot);
        }

        // 
        /// <summary>
        /// メッセージタイプ取得　送達確認のスロットとサブスロットから
        /// </summary>
        public int getMsgType(int slot, int subslot)
        {
            if (slot > 2)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "getMsgType", "slot error " + slot);
                return -1;
            }

            updateNewSlotSub();
            int type = m_slotsubslot[slot][subslot];

            int nowslot = getSlot();
            int nowsubslot = getSubSlot();
            if (slot == nowslot && subslot >= nowsubslot)
            {
                // いまフラグ立て中、または数秒後のスロットのためoldから
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "getMsgType"
                    , "いま、または数秒後のslot/subslotのためoldから取得 slot:" + slot + " sub:" + subslot);
                type = m_old_slotsubslot[slot][subslot];
            }

            // 初期値０
            if (type == 0)
            {
                return -1;
            }
            // TYPE0
            if (type == TYPE0)
            {
                return 0;
            }

            return type;
        }

        // Type0のシーケンス番号取得
        public int getType0Seq(int slot, int subslot)
        {
            return m_Seq0[slot * 100 + subslot];
        }

        /// <summary>
        /// Type1のシーケンス番号取得
        /// </summary>
        public int getType1Seq(int slot, int subslot)
        {
            return m_Seq1[slot * 100 + subslot];
        }
        /// <summary>
        /// Type２のシーケンス番号取得
        /// </summary>
        public int getType2Seq(int slot, int subslot)
        {
            return m_Seq2[slot * 100 + subslot];
        }

        // スロット保持データの消しこみ
        public void clearOne(int slot, int subslot)
        {
            updateNewSlotSub();
            int nowslot = getSlot();
            int nowsubslot = getSubSlot();
            if (slot == nowslot && subslot >= nowsubslot)
            {
                //  いま、もしくは数秒先のスロット（送達確認）
                m_old_slotsubslot[slot][subslot] = 0;
            }
            else
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "getMsgType", "clearOne slot:" + slot + " sub:" + subslot);
                m_slotsubslot[slot][subslot] = 0;
            }
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void clear()
        {
            m_slotsubslot = new int[3][];
            m_old_slotsubslot = new int[3][];
            m_old_count = 0;
            m_pre_slot = 0;

            m_received_slotsubslot = new int[3][];
            m_received_old_slotsubslot = new int[3][];

            // 送達確認時に参照できる番号
            m_Seq1 = new int[1000];
            // Type2 シーケンス番号
            m_Seq2 = new int[1000];

            for (int i = 0; i < 3; i++)
            {
                m_slotsubslot[i] = new int[MsgType100.Ack.SUBSLOTBITMAP_SIZE];
                m_old_slotsubslot[i] = new int[MsgType100.Ack.SUBSLOTBITMAP_SIZE];
                m_received_slotsubslot[i] = new int[MsgType100.Ack.SUBSLOTBITMAP_SIZE];
            }
        }

        // 
        //public string getType2Seq()
    }
}
