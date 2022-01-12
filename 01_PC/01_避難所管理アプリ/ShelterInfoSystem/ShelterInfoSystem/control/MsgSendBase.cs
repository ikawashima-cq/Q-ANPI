/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QAnpiCtrlLib.log;
using QAnpiCtrlLib.consts;
using QAnpiCtrlLib.msg;
using QAnpiCtrlLib.Properties;

namespace ShelterInfoSystem.control
{
    /// <summary>
    /// メッセージ送信ベースクラス
    /// </summary>
    public partial class MsgSendBase
    {
        /// <summary>
        /// 送信中フラグ
        /// true:送信中
        /// false:送信中ではない
        /// </summary>
        public static Boolean isSending = false;

        /// <summary>
        /// メッセージ送信のパラメータを格納するクラス
        /// </summary>
        class MsgData : EventArgs
        {
            public List<byte[]> payload;
            public int frq;
            public int pn;
            public DateTime sendTime;
        }

        /// <summary>
        /// 送信タイミング待ちのための変数
        /// </summary>
        private System.Threading.AutoResetEvent are = null;

        /// <summary>
        /// 送信タイミング待ちのための変数のlock用変数
        /// </summary>
        private object lockobject = new object(); 

        /// <summary>
        /// メッセージ送信ベースクラスのコンストラクタ
        /// </summary>
        public MsgSendBase()
        {
        }

        /// <summary>
        /// メッセージの送信開始要求
        /// </summary>
        /// <param name="payload">メッセージのペイロード</param>
        /// <param name="frq">送信周波数</param>
        /// <param name="pn">送信PN符号</param>
        /// <param name="sendTime">送信時刻</param>
        public void sendMsg(List<byte[]> payload, int frq, int pn, DateTime sendTime)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            MsgData msgdata  = new MsgData();
            msgdata.payload  = payload;
            msgdata.frq      = frq;
            msgdata.pn       = pn;
            msgdata.sendTime = sendTime.ToUniversalTime();

            isSending = true;
            if (ObjectKeeper.pnl_left != null && ObjectKeeper.pnl_right != null)
            {
                ObjectKeeper.pnl_left.Enabled = false;
                ObjectKeeper.pnl_right.Enabled = false;
            }

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
        }

        /// <summary>
        /// メッセージの送信開始要求
        /// </summary>
        /// <param name="payload">メッセージのペイロード</param>
        /// <param name="frq">送信周波数</param>
        /// <param name="pn">送信PN符号</param>
        /// <param name="second">何秒後に送信するか</param>
        public void sendMsg(List<byte[]> payload, int frq, int pn, int second)
        {
            DateTime sendTime = DateTime.UtcNow;
            sendTime = sendTime.AddSeconds(second);
            sendMsg(payload, frq, pn, sendTime);
        }
        
        /// <summary>
        /// 送信処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SendStart(object sender, DoWorkEventArgs e)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");


            MsgSendProc msp = MsgSendProc.getInstance();
            MsgData msgdata = (MsgData)e.Argument;
            BackgroundWorker mybgw = (BackgroundWorker)sender;
            
            //前処理
            msp.sendMsgPreProcess();

            TimeSpan wait = msgdata.sendTime - DateTime.UtcNow;
            Console.WriteLine(wait.TotalMilliseconds);
            if (wait.TotalMilliseconds > 0)
            {
                are = new System.Threading.AutoResetEvent(false);
                are.WaitOne(wait);
                lock(lockobject)
                {
                    are.Dispose();
                    are = null;
                }
                Console.WriteLine((msgdata.sendTime - DateTime.UtcNow).TotalMilliseconds);
            }

            for (int i = 0; i < msgdata.payload.Count; i++)
            {
                if (mybgw.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                mybgw.ReportProgress(msgdata.payload.Count, i);
                msp.sendMsgProcess(msgdata.payload.ElementAt(i));
            }

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

        }

        /// <summary>
        /// 送信処理の進行状況を画面の動作結果を表示する領域に反映する処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SendProgress(object sender, ProgressChangedEventArgs e)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            if(ObjectKeeper.mainFormStatusLabel != null)
            {
                string progress = Resources.W_common_prg_msg_004;
                if(e.ProgressPercentage != 1)
                {
                    //Type3なので送信の(m/n)を作成する
                    progress = Resources.W_common_prg_msg_002;
                    progress = progress.Replace("n", e.ProgressPercentage.ToString());
                    int sendIndex = ((int)e.UserState) + 1;
                    progress = progress.Replace("m", sendIndex.ToString());
                }
                ObjectKeeper.mainFormStatusLabel.Text = progress; 
            }
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
        }

        /// <summary>
        /// 送信完了時に呼び出される処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SendFinish(object sender, RunWorkerCompletedEventArgs e)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            String result;
            if(e.Error != null)
            {
                LogMng.AplLogError("メッセージ送信でエラー発生");
                LogMng.AplLogError(e.Error.Message);
                LogMng.AplLogError(e.Error.StackTrace);
                result = e.Error.Message;

            }
            else if(e.Cancelled == true)
            {
                LogMng.AplLogDebug("ユーザによるキャンセル");
                result = Resources.W_common_rst_okmsg_006;
            }
            else 
            {
                LogMng.AplLogDebug("正常終了");
                result = Resources.W_common_rst_okmsg_005;
            }

            if (ObjectKeeper.mainFormStatusLabel != null)
            {
                ObjectKeeper.mainFormStatusLabel.Text = result;
            }

            if (ObjectKeeper.pnl_left != null && ObjectKeeper.pnl_right != null)
            {
                ObjectKeeper.pnl_left.Enabled = true;
                ObjectKeeper.pnl_right.Enabled = true;
            }
            isSending = false;

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
        }

        /// <summary>
        /// メッセージ送信のキャンセル
        /// </summary>
        public void cancelMsg()
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            lock(lockobject)
            {
                if(are != null)
                {
                    Console.WriteLine("cancel wait");
                    are.Set();
                }
            }
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

        }

        /// <summary>
        /// S 帯モニタデータ（RTN）送信
        /// </summary>
        /// <param name="payload">送信データ</param>
        /// <param name="sys">送信する系番号</param>
        public void sendRtnMsg(byte[] payload)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            SBandComCtl sbcc = SBandComCtl.getInstance();
            SBandConnectionInfo sci = sbcc.getRTNDataConnectionInfo();
            sendSBandMsg(payload, sci);
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
        }

        /// <summary>
        /// S 帯モニタデータ（FWD）送信
        /// </summary>
        /// <param name="payload">送信データ</param>
        /// <param name="sys">送信する系番号</param>
        public void sendFwdMsg(byte[] payload)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            SBandComCtl sbcc = SBandComCtl.getInstance();
            SBandConnectionInfo sci = sbcc.getFWDDataConnectionInfo();
            sendSBandMsg(payload,sci);
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
        }

        /// <summary>
        /// S 帯モニタ監視ステータス送信
        /// </summary>
        /// <param name="payload">送信データ</param>
        /// <param name="sys">送信する系番号</param>
        public void sendMonStatusMsg(byte[] payload, CommonConst.SystemNumber sys)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            SBandComCtl sbcc = SBandComCtl.getInstance();
            SBandConnectionInfo sci = sbcc.getEquStatusConnectionInfo();
            sendSBandMsg(payload, sci);
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
        }


        //s2
        /// <summary>
        /// モニタデータ（L1s）送信
        /// </summary>
        /// <param name="payload">送信データ</param>
        /// <param name="sys">送信する系番号</param>
        public void sendL1sMsg(byte[] payload)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            SBandComCtl sbcc = SBandComCtl.getInstance();
            SBandConnectionInfo sci = sbcc.getL1SDataConnectionInfo();
            sendSBandMsg(payload, sci);
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
        }

        /// <summary>
        /// S帯メッセージ送信
        /// </summary>
        /// <param name="payload">送信データ</param>
        /// <param name="sci">送信する接続情報</param>
        /// <param name="sys">系番号</param>
        private void sendSBandMsg(byte[] payload, SBandConnectionInfo sci)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            // デバグログ
            string writebuf = "";
            for (int i = 0; i < payload.Length; i++)
            {
                string hsin = Convert.ToString(payload[i], 16);
                hsin = hsin.PadLeft(2, '0');
                writebuf += hsin + " ";
            }
            Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "Tcp send", " payload:" + writebuf);


            if (sci != null)
            {
                try
                {
                    if (sci.client.Connected == false)
                    {
                        // 
                        Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "sendSBandMsg", "disconnected ");
                    }
                    sci.write(payload, 0, payload.Length);
                    if(ObjectKeeper.mainFormStatusLabel != null)
                    {
                        ObjectKeeper.mainFormStatusLabel.Text = Resources.W_common_rst_okmsg_005;
                    }
                    LogMng.SBandMsgLogWrite(payload, LogMng.SEND,CommonConst.SystemNumber.SYSTEM1);
                }
                catch(Exception ex)
                {
                    LogMng.AplLogDebug(ex.Message);
                    LogMng.AplLogDebug(ex.StackTrace);
                    if (ObjectKeeper.mainFormStatusLabel != null)
                    {
                        ObjectKeeper.mainFormStatusLabel.Text = ex.Message;
                    }

                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, ex.Message, "");

                    throw (ex);
                }
            }
            else
            {
                if(ObjectKeeper.mainFormStatusLabel != null)
                {
                    ObjectKeeper.mainFormStatusLabel.Text = Resources.W_common_rst_ngmsg_005;
                }
            }
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
        }
    }
}
