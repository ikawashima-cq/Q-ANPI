/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

using QAnpiCtrlLib.log;
using QAnpiCtrlLib.consts;

namespace ShelterInfoSystem.control
{
    /// <summary>
    /// 接続情報
    /// </summary>
    class SBandConnectionInfo
    {
        private const int NOT_FOUND = -1;
        /// <summary>
        /// 受信データ
        /// </summary>
        public class SBandDataEvnt:EventArgs
        {
            /// <summary>
            /// 接続先のIPアドレス
            /// </summary>
            public IPAddress ipaddr;
            /// <summary>
            /// 接続先のポート番号
            /// </summary>
            public int port;
            /// <summary>
            /// 受信データ
            /// "1234567890"で始まり"-1234567890"で終わるS帯データ
            /// </summary>
            public byte[] data;
        }

        /// <summary>
        /// 受信データ通知用のデリゲート宣言
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void rcvDataHandler(object sender, SBandDataEvnt e);

        /// <summary>
        /// メッセージイベント
        /// メッセージイベントが必要ならここに登録する
        /// </summary>
        public event rcvDataHandler rcvDataEvent;

        IPAddress ipaddr;
        /// <summary>
        /// 接続先ポート番号
        /// </summary>
        public int port = 0;
        /// <summary>
        /// TCPクライアント
        /// </summary>
        public TcpClient client = new TcpClient();

        /// <summary>
        /// メッセージを組み立てるためのバッファ
        /// 基本的には一度の読み出しでコマンドの応答は完結するが念のため用意
        /// </summary>
        private System.IO.MemoryStream ms = new System.IO.MemoryStream();

        /// <summary>
        /// メッセージ読み出し用のバッファ
        /// </summary>
        byte[] myReadBuffer = new byte[1024];

        /// <summary>
        /// 接続
        /// </summary>
        /// <param name="ipAddressByte">接続先のIPアドレス</param>
        /// <param name="port">接続先のポート番号</param>
        public void connect(Byte[] ipAddressByte, int port)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            IPAddress ipaddr = new IPAddress(ipAddressByte);
            client.Connect(ipaddr, port);
            client.GetStream().BeginRead(myReadBuffer, 0, myReadBuffer.Length, new AsyncCallback(reciveTcpMsg), client.GetStream());
            
            this.ipaddr = ipaddr;
            this.port = port;

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
        }

        /// <summary>
        /// 対向からのメッセージ受信
        /// ackを受信した場合には、待ちを解除してwirte/Readの応答を返す
        /// メッセージを受信した場合には、メッセージLogに書き込む
        /// (一覧画面への反映は、メッセージLogの書き込み処理内で行う)
        /// </summary>
        /// <param name="result">読出し開始時に設定したパラメータ</param>
        public void reciveTcpMsg(IAsyncResult result)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            int dataLen = 0;
            SBandDataEvnt sde = new SBandDataEvnt();
            sde.ipaddr = this.ipaddr;
            sde.port = this.port;

            System.Net.Sockets.NetworkStream myNs = (System.Net.Sockets.NetworkStream)result.AsyncState;

            try
            {
                //届いたデータの読み出し
                dataLen = myNs.EndRead(result);
                if (dataLen == 0)
                {
                    //サーバ側から切断された
                    LogMng.AplLogDebug("サーバ側から切断された");
                    sendRcvEvent(new byte[]{});
                    disconnect();
                    LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
                    return;
                }

                ms.Write(myReadBuffer, 0, dataLen);

                //まだデータが残っているか？
                if (myNs.DataAvailable == false)
                {
                    //全データ受信
                    //メッセージになっているか確認
                    List<byte[]> msgs = getMsgs();
                    
                    //メッセージ受信イベントの送信
                    foreach(byte[] msg in msgs)
                    {
                        sendRcvEvent(msg);
                    }
                }

                //非同期の読み出し開始要求
                myNs.BeginRead(myReadBuffer, 0, myReadBuffer.Length, new AsyncCallback(reciveTcpMsg), myNs);

                LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");


            }
            catch(Exception ex)
            {
                LogMng.AplLogError(ex.Message);
                LogMng.AplLogError(ex.StackTrace);
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "ConInfo", ex.Message);
                sendRcvEvent(new byte[] { });
                disconnect();
            }

        }

        /// <summary>
        /// 切断
        /// </summary>
        public void disconnect()
        {
            if(ms != null)
            {
                ms.Close();
                ms = null;
            }
            client.Close();
        }

        /// <summary>
        /// 書込み
        /// </summary>
        /// <param name="data">データ</param>
        /// <param name="offset">書込みのoffset</param>
        /// <param name="size">書込みサイズ</param>
        public void write(byte[] data, int offset, int size)
        {
            client.GetStream().Write(data, offset, size);
        }

        /// <summary>
        /// メッセージの抜出
        /// </summary>
        /// <returns>S帯メッセージのリスト</returns>
        private List<byte[]> getMsgs()
        {

            List<byte[]> msgList = new List<byte[]>(); 
            List<byte> startCodeList = new List<byte>();
            startCodeList.AddRange(BitConverter.GetBytes(CommonConst.SBAND_MSG_START_CODE));
            startCodeList.Reverse();
            byte[] startCade = startCodeList.ToArray();

            List<byte> endCodeList = new List<byte>();
            endCodeList.AddRange(BitConverter.GetBytes(CommonConst.SBAND_MSG_END_CODE));
            endCodeList.Reverse();
            byte[] endCade = endCodeList.ToArray();

            byte[] data = ms.ToArray();
            
            for (int i = 0; i < data.Length - startCade.Length; )
            {
                int startpos = getPos(data, i, startCade);
                if (startpos == NOT_FOUND)
                {
                    ms.Close();
                    ms = new System.IO.MemoryStream();
                    ms.Write(data, data.Length - startCade.Length, startCade.Length - 1);
                    break;
                }
                i = startpos;
                int endpos = getPos(data, i, endCade);
                if (endpos == NOT_FOUND)
                {
                    break;
                }
                else
                {
                    int len = endpos + endCade.Length - startpos;
                    System.IO.MemoryStream msg = new System.IO.MemoryStream(data, startpos, len);
                    msgList.Add(msg.ToArray());
                    i = endpos + endCade.Length;
                    ms.Close();
                    ms = new System.IO.MemoryStream();
                    ms.Write(data, i, data.Length - i);
                }

            }
            return msgList;


        }

        /// <summary>
        /// conpDataで指定されたデータが含まれる先頭位置を返す
        /// </summary>
        /// <param name="data">検索対象</param>
        /// <param name="offset">検索開始位置</param>
        /// <param name="conpData">検索するデータ</param>
        /// <returns></returns>
        private int getPos(byte[] data, int offset, byte[] conpData)
        {
            int i;
            int pos = NOT_FOUND;
            for (i = offset; i <= data.Length - conpData.Length; i++)
            {
                int j;
                for (j = 0; j < conpData.Length; j++)
                {
                    if (data[i + j] != conpData[j])
                    {
                        break;
                    }
                }

                if (j >= conpData.Length)
                {
                    //発見
                    pos = i;
                    break;
                }
            }
            return pos;
        }

        /// <summary>
        /// 受信データ送信
        /// </summary>
        /// <param name="data">受信データ</param>
        void sendRcvEvent(byte[] data)
        {

            if (rcvDataEvent != null)
            {
                SBandDataEvnt sde = new SBandDataEvnt();
                sde.ipaddr = this.ipaddr;
                sde.port = this.port;
                sde.data = data;

                // デバグログ
                string writebuf = "";
                for (int i = 0; i < data.Length; i++)
                {
                    string hsin = Convert.ToString(data[i], 16);
                    hsin = hsin.PadLeft(2, '0');
                    writebuf += hsin + " ";
                }
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "Tcp recv", "ipaddr:" + this.ipaddr + " port:" + this.port + " data:" + writebuf);
                rcvDataEvent(this, sde);
            }

        }
    }
}
