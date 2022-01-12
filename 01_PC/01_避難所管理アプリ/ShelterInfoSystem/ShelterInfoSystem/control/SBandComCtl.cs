/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2017. All rights reserved.
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
    /// S帯データ用の通信管理
    /// </summary>
    class SBandComCtl
    {

        /// <summary>
        /// 1系用の接続保存用リスト
        /// </summary>
        private List<SBandConnectionInfo> sys1ClientList = new List<SBandConnectionInfo>();

        /// <summary>
        /// 接続保存用リストのLockオブジェクト
        /// </summary>
        private Object ClientListLockObject = new Object();

        public class statusEventArgs : EventArgs
        {
            /// <summary>
            /// 接続先IP
            /// </summary>
            public byte[] connectIP = { 0, 0, 0, 0 };
            /// <summary>
            /// 接続先ポート
            /// </summary>
            public int port = 0;
            /// <summary>
            /// 接続切断状態 true:接続、false:切断
            /// </summary>
            public Boolean isConnected = true;
        }
        /// <summary>
        /// メッセージLog情報通知用デリゲート宣言
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void conStateHandler(object sender, statusEventArgs e);

        /// <summary>
        /// メッセージイベント
        /// メッセージイベントが必要ならここに登録する
        /// </summary>
        public event conStateHandler conStateEvent;

        /// <summary>
        /// このクラスのインスタンスを格納する
        /// </summary>
        private static SBandComCtl myInstance = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        SBandComCtl()
        {
            //何もしない
        }

        /// <summary>
        /// インスタンス取得
        /// </summary>
        /// <returns>このクラスのインスタンス</returns>
        public static SBandComCtl getInstance()
        {
            if (myInstance == null)
            {
                myInstance = new SBandComCtl();
            }
            return myInstance;

        }

        /// <summary>
        /// 対向への接続
        /// </summary>
        /// <param name="ipAddressByte">接続先のIPアドレス</param>
        /// <param name="port">接続先のポート番号</param>
        /// <param name="sys">系番号</param>
        public void connect(Byte[] ipAddressByte, int port, CommonConst.SystemNumber sys)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            SBandConnectionInfo connectiion = new SBandConnectionInfo();
            try
            {
                // 接続していたら切断
                if (getSBandConnectionInfo(port, sys1ClientList) != null)
                {
                    disconnect(ipAddressByte, port);
                }
                connectiion.connect(ipAddressByte, port); 
                sys1ClientList.Add(connectiion);

            }
            catch(Exception ex)
            {
                if (connectiion.client.Connected == true)
                {
                    connectiion.client.Close();
                }
                LogMng.AplLogError(ex.Message);
                LogMng.AplLogError(ex.StackTrace);
                throw (ex);
            }
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return;
        }

        /// <summary>
        /// メッセージ受信
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">データ</param>
        void connectiion_rcvDataEvent(object sender, SBandConnectionInfo.SBandDataEvnt e)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            if(e.data.Length == 0)
            {
                //切断された
                disconnect(e.ipaddr.GetAddressBytes(), e.port);
            }
            else
            {
                if(sys1ClientList.IndexOf((SBandConnectionInfo)sender) >= 0)
                {
                    LogMng.SBandMsgLogWrite(e.data, LogMng.RECIVE,CommonConst.SystemNumber.SYSTEM1 );
                }
            }

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

        }

        /// <summary>
        /// 対向との切断要求
        /// </summary>
        /// <param name="ipAddressByte">対向のIPアドレス</param>
        /// <param name="port">対向のポート</param>
        public void disconnect(Byte[] ipAddressByte, int port)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            try
            {
                disconnect(ipAddressByte, port, sys1ClientList);
            }
            catch(Exception ex)
            {
                LogMng.AplLogError(ex.Message);
                LogMng.AplLogError(ex.StackTrace);
                
            }

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return;
        }

        /// <summary>
        /// 対向との切断要求
        /// </summary>
        /// <param name="ipAddressByte">対向のIPアドレス</param>
        /// <param name="port">対向のポート</param>
        /// <param name="clist">接続保存用リスト</param>
        /// <returns></returns>
        private bool disconnect(Byte[] ipAddressByte, int port, List<SBandConnectionInfo> clist)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            bool isDisconnect = false;
            IPAddress ipaddr = new IPAddress(ipAddressByte);
            SBandConnectionInfo c = getSBandConnectionInfo(ipAddressByte, port, clist);
            if (c != null)
            {
                lock (ClientListLockObject)
                {
                    c.client.Close();
                    clist.Remove(c);
                }
                isDisconnect = true;
                statusEventArgs sea = new statusEventArgs();
                sea.connectIP = ipAddressByte;
                sea.port = port;
                sea.isConnected = false;
                if (conStateEvent != null)
                {
                    conStateEvent(this, sea);
                }
            }
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return isDisconnect;
        }


        /// <summary>
        /// 指定されたSBandConnectionInfoを返す
        /// </summary>
        /// <param name="ipAddressByte">接続先IPアドレス</param>
        /// <param name="port">接続先のポート</param>
        /// <param name="clist">接続保存用リスト</param>
        /// <returns>SBandConnectionInfo 指定されたものが無い場合はnullを返す</returns>
        private SBandConnectionInfo getSBandConnectionInfo(Byte[] ipAddressByte, int port, List<SBandConnectionInfo> clist)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            SBandConnectionInfo retSci = null;
            System.Net.IPAddress ipaddr = new System.Net.IPAddress(ipAddressByte);
            lock (ClientListLockObject)
            {
                if(clist.Count > 0)
                {
                    retSci = getSBandConnectionInfo(port, clist);
                }
            }

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

            return retSci;
        }


        /// <summary>
        /// 指定されたSBandConnectionInfoを返す
        /// </summary>
        /// <param name="port">接続先のポート</param>
        /// <param name="clist">接続保存用リスト</param>
        /// <returns>SBandConnectionInfo 指定されたものが無い場合はnullを返す</returns>
        private SBandConnectionInfo getSBandConnectionInfo(int port, List<SBandConnectionInfo> clist)
        {
            SBandConnectionInfo retSci = null;
            foreach (SBandConnectionInfo c in clist)
            {
                if (c.port == port)
                {
                    LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "発見");
                    retSci = c;
                    break;
                }
            }
            return retSci;
        }

        /// <summary>
        /// 装置ステータス用のSBandConnectionInfoを返す
        /// </summary>
        /// <returns></returns>
        public SBandConnectionInfo getEquStatusConnectionInfo()
        {
            List<SBandConnectionInfo> clist = sys1ClientList;
            return getSBandConnectionInfo(CommonConst.EQU_STATUS_PORT, clist);
        }

        /// <summary>
        /// FWDデータ用のSBandConnectionInfoを返す
        /// </summary>
        /// <param name="sys">系番号</param>
        /// <returns></returns>
        public SBandConnectionInfo getFWDDataConnectionInfo()
        {
            List<SBandConnectionInfo> clist = sys1ClientList;
            return getSBandConnectionInfo(CommonConst.FWD_DATA_PORT, clist);
        }

        /// <summary>
        /// RTNデータ用のSBandConnectionInfoを返す
        /// </summary>
        /// <returns></returns>
        public SBandConnectionInfo getRTNDataConnectionInfo()
        {
            SBandConnectionInfo retSci = getSBandConnectionInfo(CommonConst.RTN_DATA_PORT_1, sys1ClientList);
            return retSci;
        }

        /// <summary>
        /// L1Sデータ用のSBandConnectionInfoを返す
        /// </summary>
        /// <returns></returns>
        public SBandConnectionInfo getL1SDataConnectionInfo()
        {
            //s2
            List<SBandConnectionInfo> clist = sys1ClientList;
            return getSBandConnectionInfo(CommonConst.L1S_DATA_PORT, clist);
        }
    }
}
