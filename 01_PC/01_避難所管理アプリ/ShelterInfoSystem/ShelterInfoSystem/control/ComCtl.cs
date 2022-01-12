/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QAnpiCtrlLib.log;
using QAnpiCtrlLib.consts;
using QAnpiCtrlLib.msg;

namespace ShelterInfoSystem.control
{
    /// <summary>
    /// 通信管理
    /// </summary>
    class ComCtl
    {
        /// <summary>
        /// 接続先のポート番号　60000固定
        /// </summary>
        private const int PORT_NO = 60000; 

        /// <summary>
        /// 使用中のTcpCliectを内部で保存する
        /// </summary>
        private static System.Net.Sockets.TcpClient tcp = null;

        /// <summary>
        /// 使用中のNetworkStreamを内部で保存する
        /// </summary>
        private static System.Net.Sockets.NetworkStream ns = null;

        /// <summary>
        /// データを読み出すためのバッファ
        /// </summary>
        private static byte[] buff = new byte[1024];

        /// <summary>
        /// メッセージを組み立てるためのバッファ
        /// 基本的には一度の読み出しでコマンドの応答は完結するが念のため用意
        /// </summary>
        private static System.IO.MemoryStream ms = null;

        /// <summary>
        /// 受信したAckを保存する
        /// </summary>
        private static System.IO.MemoryStream msAck = null;

        /// <summary>
        /// 待ち合わせ用の変数
        /// </summary>
        private static System.Threading.AutoResetEvent are = new System.Threading.AutoResetEvent(false);

        /// <summary>
        /// 応答待ちのタイマ値(ミリ秒)
        /// </summary>
        private static int respwaittime = 10000;

        /// <summary>
        /// 排他ロック用変数(送信のLock)
        /// </summary>
        private static object lockForSend = new object();

        /// <summary>
        /// 排他ロック用変数その２（受信処理のLock）
        /// </summary>
        private static object lockForRecive = new object();

        /// <summary>
        /// 排他ロック用変数その３(モデムの割り込みLock)
        /// </summary>
        public static object modemInterruptHandlerLock = new object();

        /// <summary>
        /// 排他ロック用変数その４（クローズのLock）
        /// </summary>
        private static object lockForClose = new object();

        /// <summary>
        /// 排他ロック用変数その５（待ち合わせ変数のLock）
        /// </summary>
        private static object lockForAre = new object();

        /// <summary>
        /// 通信アプリからのモデム割り込みイベント通知用パラメータ
        /// </summary>
        public class modemInterruptEventArgs : EventArgs
        {
            /// <summary>
            /// 割り込み要因
            /// </summary>
            public int intr_cause = 0;
        }


        /// <summary>
        /// 通信アプリからのモデム割り込みイベント通知用デリゲート宣言
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void modemInterruptHandler(object sender, modemInterruptEventArgs e);

        /// <summary>
        /// モデム割り込みイベント
        /// モデム割り込みイベントが必要ならここに登録する
        /// </summary>
        public static event modemInterruptHandler modemInterruptEvent;


        /// <summary>
        /// メッセージLog情報通知用デリゲート宣言
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void conStateHandler(object sender, EventArgs e);

        /// <summary>
        /// メッセージイベント
        /// メッセージイベントが必要ならここに登録する
        /// </summary>
        public static event conStateHandler conStateEvent;


        /// <summary>
        /// config fileに設定するName(keyString)
        /// </summary>
        private const string keyString = "connectPortNo";

        /// <summary>
        /// 対向への接続要求
        /// </summary>
        /// <param name="ipAddressByte">IPアドレス</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        /// <exception cref="System.Net.Sockets.SocketException"></exception>
        /// <exception cref="System.ObjectDisposedException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        public static void connect(Byte[] ipAddressByte)
        {

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            System.Net.IPAddress ipAddress = null;

            //IPアドレス型変換
            try
            {
                ipAddress = new System.Net.IPAddress(ipAddressByte);
            }
            catch(System.ArgumentNullException e)
            {
                LogMng.AplLogError("IP Address is null");
                LogMng.AplLogError(e.StackTrace);
                throw (e);
            }
            catch(System.ArgumentException e)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("IP Address is not valid.");
                sb.Append(" len:" + ipAddressByte.Length + " ");
                foreach (byte b in ipAddressByte)
                {
                    sb.Append(b.ToString() + ".");
                }
                LogMng.AplLogError( sb.ToString() );
                LogMng.AplLogError(e.StackTrace);
                throw (e);
            }

            //接続済みチェック
            if(tcp != null)
            {
                LogMng.AplLogError("TCP/IP接続済み");
                throw (new System.InvalidOperationException("TCP/IP接続済み"));
            }

            int portNo = PORT_NO;
            try
            {
                string portNoStr = ConfigFileMng.getValue(keyString);
                portNo = int.Parse(portNoStr);

            }
            catch (System.ArgumentException e)
            {
                //Do Nothing
                LogMng.AplLogDebug(keyString + "がコンフィグファイルに無い");
                LogMng.AplLogDebug(e.Message);
                LogMng.AplLogDebug(e.StackTrace);
            }
            catch (System.Exception e)
            {
                //Do Nothing
                LogMng.AplLogDebug("コンフィグファイルの" + keyString + "値不正" );
                LogMng.AplLogDebug(e.Message);
                LogMng.AplLogDebug(e.StackTrace);
            }

            //TCP/IP接続
            tcp = new System.Net.Sockets.TcpClient();
            try
            {
                tcp.Connect(ipAddress, portNo);
                ns = tcp.GetStream();
                ms = new System.IO.MemoryStream();
                ns.BeginRead(buff, 0, buff.Length, new AsyncCallback(reciveTcpMsg), ns);
            }
            catch(System.Exception e)
            {
                // 発生する可能性がある例外は以下
                disconnect();
                LogMng.AplLogError(e.Message);
                LogMng.AplLogError(e.StackTrace);
                throw (e);
            }

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

        }

        /// <summary>
        /// 対向からのメッセージ受信
        /// ackを受信した場合には、待ちを解除してwirte/Readの応答を返す
        /// メッセージを受信した場合には、メッセージLogに書き込む
        /// (一覧画面への反映は、メッセージLogの書き込み処理内で行う)
        /// </summary>
        /// <param name="result">タイマ発行じに設定したパラメータ</param>
        public static void reciveTcpMsg(IAsyncResult result) 
        {
            lock (lockForRecive)
            {
                LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

                int dataLen = 0;
                System.Net.Sockets.NetworkStream myNs = (System.Net.Sockets.NetworkStream)result.AsyncState;

                try
                {
                    //届いたデータの読み出し
                    dataLen = myNs.EndRead(result);
                    if (dataLen == 0)
                    {
                        //サーバ側から切断された
                        LogMng.AplLogDebug("サーバ側から切断された");
                        disconnect();
                        LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
                        return;
                    }
                    ms.Write(buff, 0, dataLen);

                    //まだデータが残っているか？
                    if (myNs.DataAvailable == false)
                    {
                        //全データは届いているので内容チェック
                        ComAplMsgMng camm = new ComAplMsgMng();
                        byte[] revicedData = ms.ToArray();

                        LogMng.AplLogInfo(BitConverter.ToString(revicedData, 0));

                        //受信データ保持用のストリームの作り直し
                        ms.Close();
                        //ms.Dispose();
                        ms = new System.IO.MemoryStream();

                        List<byte[]> datas = camm.spriteMessages(revicedData, ms);

                        foreach (byte[] oneData in datas)
                        {
                            LogMng.AplLogDebug(BitConverter.ToString(oneData, 0));

                            System.IO.MemoryStream oneDataStream = new System.IO.MemoryStream();
                            oneDataStream.Write(oneData, 0, oneData.Length);
                            byte[] recivedData = camm.deEscapeMsg(oneDataStream);

                            try
                            {
                                //データフォーマット確認

                                ComAplMsgMng.MsgType mt = camm.CheckRcvMsgFormat(recivedData);
                                if (mt == ComAplMsgMng.MsgType.MSG_RCV)
                                {
                                    //メッセージの場合はLog書き込み
                                    ComAplMsgMng.MsgRcvCommad msgrcvcommand = camm.GetMsgFromReciveData(recivedData);
                                    LogMng.MsgLogWrite(msgrcvcommand.message, msgrcvcommand.dirction);
                                }
                                else if (mt == ComAplMsgMng.MsgType.MOD_INTR)
                                {
                                    lock (modemInterruptHandlerLock)
                                    {
                                        //割り込みの場合ハンドラコール
                                        if (modemInterruptEvent != null)
                                        {
                                            DecodeManager dm = new DecodeManager();
                                            modemInterruptEventArgs ea = new modemInterruptEventArgs();
                                            int caseDataSize = ComAplMsgMng.CAUSE_DATA_LENGTH * 8;
                                            int caseDataStart = ComAplMsgMng.POS_CAUSE_START * 8;
                                            ea.intr_cause = dm.decodeInt(recivedData, caseDataSize, caseDataStart);
                                            modemInterruptEvent(null, ea);
                                        }
                                    }
                                }
                                else
                                {
                                    //データのコピー
                                    msAck = new System.IO.MemoryStream(recivedData);
                                    lock (lockForAre)
                                    {
                                        //ACKの場合 待ち解除
                                        are.Set();
                                    }
                                }
                            }
                            catch (System.Exception e)
                            {
                                Console.WriteLine(e.GetType().ToString());
                                //受信メッセージのチェックエラー
                                //エラーを出力するだけで処理継続
                                LogMng.AplLogError(e.GetType().ToString());
                                LogMng.AplLogError(e.Message);
                                LogMng.AplLogError(e.StackTrace);
                            }
                        }
                    }

                    //非同期の読み出し開始要求
                    myNs.BeginRead(buff, 0, buff.Length, new AsyncCallback(reciveTcpMsg), myNs);

                    LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");


                }
                catch (System.Exception e)
                {
                    disconnect();
                    LogMng.AplLogError(e.Message);
                    LogMng.AplLogError(e.StackTrace);
                }
            }
        }

        /// <summary>
        /// 対向へのデータの書き込み
        /// </summary>
        /// <param name="adr">書き込み先のアドレス</param>
        /// <param name="wdata">書き込むデータ(最大64byte)</param>
        /// <param name="save">書き込み内容を通信アプリ起動時に実施するかしないか　true:実施、fales:実施しない</param>
        /// <returns>書き込み結果</returns>
        public static byte[] write(byte[] adr, byte[] wdata, Boolean save)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");


            byte[] msg;     //送信メッセージ用のバッファ
 
            //メッセージ作成
            ComAplMsgMng camm = new ComAplMsgMng();
            try
            {
                msg = camm.WriteMsgBuilder(adr, wdata, save);
            }
            catch(System.Exception e)
            {
                LogMng.AplLogError("メッセージ作成　エラー");
                throw (e);
            }

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

            return sendEscpedMsg(msg);
        }

        /// <summary>
        /// 対向からのデータ読み出し
        /// </summary>
        /// <param name="adr">読み出し元アドレス</param>
        /// <param name="len">読み出すデータのサイズ</param>
        /// <returns>読み出されたデータ</returns>
        public static byte[] read(byte[] adr, int len)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            byte[] msg;     //送信メッセージ用のバッファ

            //メッセージ作成
            ComAplMsgMng camm = new ComAplMsgMng();
            try
            {
                msg = camm.ReadMsgBuilder(adr, len);
            }
            catch (System.Exception e)
            {
                LogMng.AplLogError("メッセージ作成　エラー");
                throw (e);
            }

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

            return sendEscpedMsg(msg);
        }

        /// <summary>
        /// 対向との切断要求
        /// </summary>
        public static void disconnect()
        {
            lock (lockForClose)
            {
                LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
                lock(lockForAre)
                {
                    //タイマ停止
                    are.Set();
                }

                //メモリストリームクローズ
                if (ms != null)
                {
                    ms.Close();
                    ms = null;
                }
                if (msAck != null)
                {
                    msAck.Close();
                    msAck = null;
                }

                //tcpの切断
                if (ns != null)
                {
                    ns.Close();
                    ns = null;
                }

                if (tcp != null)
                {
                    tcp.Close();
                    tcp = null;
                }

                if (conStateEvent != null)
                {
                    conStateEvent(null, null);
                }

                LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            }

            return;
        }

        /// <summary>
        /// バイナリ形式のメッセージ送信
        /// </summary>
        /// <param name="message">エスケープされていないメッセージ</param>
        /// <returns>メッセージの応答(ACK)</returns>
        /// <exception cref="System.TimeoutException">処理のタイムアウト</exception>
        public static byte[] send(byte[] message)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            ComAplMsgMng camm = new ComAplMsgMng();
            List<byte> listNonEscMsg = new List<byte>();
            listNonEscMsg.AddRange(message);
            byte[] escMsg = camm.EscapeMsg(listNonEscMsg);

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

            return sendEscpedMsg(escMsg);

        }



        /// <summary>
        /// 対向へのメッセージ送信
        /// </summary>
        /// <param name="message">制御PC-CPU-BOARD間IFで定義されたコマンド</param>
        /// <returns>制御PC-CPU-BOARD間IFで定義されたリードACK、ライトACK、エラーACKのいずれか</returns>
        public static byte[] sendEscpedMsg(byte[] message)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            byte[] byteAck; //受信メッセージ用バッファ
            lock (lockForSend)
            {
                //データ書き込み
                try
                {
                    LogMng.AplLogInfo(BitConverter.ToString(message, 0));
                    lock (lockForAre)
                    {
                        ns.Write(message, 0, message.Length);
                        //待ち合わせ変数再設定
                        are.Dispose();
                        are = new System.Threading.AutoResetEvent(false);
                    }
                    //応答待ち
                    are.WaitOne(respwaittime);


                }
                catch (System.Exception e)
                {
                    disconnect();
                    LogMng.AplLogError("データ書き込み　エラー");
                    LogMng.AplLogError(e.StackTrace);
                    throw (e);
                }


                //応答受信
                if (msAck == null)
                {
                    disconnect();
                    throw (new System.TimeoutException("ライトメッセージ応答タイムアウト"));
                }
                byteAck = msAck.ToArray();
                msAck.Close();
                msAck = null;
            }

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

            return byteAck;
        }


        /// <summary>
        /// 接続状態を返す
        /// </summary>
        /// <returns>接続状態　true-接続中 false-接続されていない</returns>
        public static bool isConnected()
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            if (tcp == null)
            {
                LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
                return false;
            }

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

            return tcp.Connected;
        }
    }
}
