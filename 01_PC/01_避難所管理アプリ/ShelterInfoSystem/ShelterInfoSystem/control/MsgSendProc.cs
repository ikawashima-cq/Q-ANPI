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
    class MsgSendProc : IDisposable
    {

        /// <summary>
        /// Modemの状態
        /// </summary>
        public class ModemStatus
        {
            /// <summary>
            ///  AGC Lockステータス　　※プロト端末のみ
            ///　false：Unlock　true：Lock"							
            /// </summary>
            public Boolean AGCLock = false;
            /// <summary>
            /// TDMA同期状態表示　(true:同期)　※プロト端末のみ
            /// </summary>
            public Boolean TDMASync = false;
            /// <summary>
            /// フレーム同期状態表示　(true:同期)　※プロト端末のみ							
            /// </summary>
            public Boolean FrameSync = false;
            /// <summary>
            /// シンボル同期状態表示　(1:同期)　※プロト端末/疑似地上局共通							
            /// </summary>
            public Boolean SymbolSync = false;
            /// <summary>
            /// "キャリア同期検出状態表示　(1:同期)　※プロト端末のみ						
            /// </summary>
            public Boolean CarrierSync = false;
        }

        /// <summary>
        /// 割り込みマスク
        /// </summary>
        public class InterruptMask
        {
            /// <summary>
            /// RF送信PLL同期状態変化(PLL Lock Detect)
            /// </summary>
            public Boolean txPllSyncChg = true;
            /// <summary>
            /// SPI通信完了通知（LTC2611 for VGA）　（未使用）
            /// </summary>
            public Boolean comCompVgaBit = true;
            /// <summary>
            /// SPI通信完了通知（HMC831）
            /// </summary>
            public Boolean comCompHMC831 = true;
            /// <summary>
            /// SPI通信完了通知（LTC1197）
            /// </summary>
            public Boolean comCompLTC1197 = true;
            /// <summary>
            /// 受信TDMA割り込み
            /// </summary>
            public Boolean rcvTDMA = true;
            /// <summary>
            /// 送信TDMA割り込み
            /// </summary>
            public Boolean sndTDMA = true;
            /// <summary>
            /// 同期状態変化(TDMA同期)
            /// </summary>
            public Boolean syncTDMA = true;
            /// <summary>
            /// 同期状態変化(フレーム同期)
            /// </summary>
            public Boolean syncFrame = true;
            /// <summary>
            /// 同期状態変化(シンボル同期)
            /// </summary>
            public Boolean syncSymbol = true;
            /// <summary>
            /// 同期状態変化(キャリア同期)
            /// </summary>
            public Boolean syncCarrier = true;
            /// <summary>
            /// 同期状態変化(AGC Lock)
            /// </summary>
            public Boolean lockAGC = true;
            /// <summary>
            /// 送信パワーの上限閾値超えによるAMP強制停止通知
            /// </summary>
            public Boolean forceStopAMP = true;
            /// <summary>
            /// 受信データバッファ 受信完了（刈取り要求）通知
            /// </summary>
            public Boolean rcvComp = true;
            /// <summary>
            /// 送信データバッファ 送信完了通知
            /// </summary>
            public Boolean sndComp = true;
            /// <summary>
            /// SPI通信完了通知（AD9364）
            /// </summary>
            public Boolean comCompAD9364 = true;
            /// <summary>
            /// SPI通信完了通知（LTC2611）
            /// </summary>
            public Boolean comCompLTC2611 = true;
            /// <summary>
            /// 受信データバッファオーバーフロー通知
            /// </summary>
            public Boolean rcvBuffOverFlow = true;
            /// <summary>
            /// （プロト端末）：フレーム廃棄通知（RSエラー）　　1=エラー発生
            /// （擬似地上局）：No function
            /// </summary>
            public Boolean errRS = true;
            /// <summary>
            /// （プロト端末）：No function
            /// （擬似地上局）：フレーム廃棄通知（CRCエラー）　　1=エラー発生
            /// </summary>
            public Boolean errCRC = true;
            /// <summary>
            /// System CLK生成用MMCM Lock状態t通知							
            /// </summary>
            public Boolean lockMMCM = true;

        }

        /// <summary>
        /// 割り込みマスク
        /// </summary>
        private InterruptMask im = new InterruptMask();

        /// <summary>
        /// メッセージ送信Lockオブジェクト
        /// </summary>
        private  object msgSendLock = new object();

        /// <summary>
        /// 待ち合わせ用の変数(バッファ空き)
        /// </summary>
        private System.Threading.AutoResetEvent are = null;

        /// <summary>
        /// 待ち合わせ用の変数(RF設定)
        /// </summary>
        private System.Threading.AutoResetEvent areRF = null;

        /// <summary>
        /// 待ち合わせ用の変数(HMC831設定)
        /// </summary>
        private System.Threading.AutoResetEvent areHMC831 = null;

        /// <summary>
        /// ComCtlに登録するモデムからの割り込みイベント用ハンドラ
        /// </summary>
        private ComCtl.modemInterruptHandler mIhandler = null;

        /// <summary>
        /// メッセージ送信間隔　端末時
        /// </summary>
        private const int WAIT_TIME_TERM_MSG = 1600;
        /// <summary>
        /// メッセージ送信間隔　疑似地上局
        /// </summary>
        private const int WAIT_TIME_STATION_MSG = 800;

        /// <summary>
        /// このクラスのインスタンスを格納する
        /// </summary>
        private static MsgSendProc myInstance = null;

        /// <summary>
        /// RF設定時のタイムアウトかどうかを確認するフラグ
        /// </summary>
        private Boolean isTimeOut = false;

        /// <summary>
        /// RF設定時のタイムアウトかどうかを確認するフラグ用のLock
        /// </summary>
        private object timelockobj = new object();

        /// <summary>
        /// HMC831設定時のタイムアウトかどうかを確認するフラグ用のLock
        /// </summary>
        private object timelockobjHMC831 = new object();

        /// <summary>
        /// RF Synthesizerのリトライ回数
        /// </summary>
        private const int RFSYNTHESIZER_RETRY_MAX = 2;

        /// <summary>
        /// RF Lock判定用マスク
        /// </summary>
        private const byte RF_LOCK_MASK = 0x02;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        MsgSendProc()
        {
            //イベントハンドラ作成
            mIhandler = new ComCtl.modemInterruptHandler(this.reciveModemIntr);
        }

        public static MsgSendProc getInstance()
        {
            if (myInstance == null)
            {
                myInstance = new MsgSendProc();
            }
            return myInstance;
        }
        
        /// <summary>
        /// TDMA同期状態取得
        /// </summary>
        /// <returns> TDMA同期状態 true:同期</returns>
        private Boolean isTDMASync()
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            ModemStatus ms = getModemState();
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return ms.TDMASync;
        }

        /// <summary>
        /// モデムの状態(フレーム同期状態)取得
        /// </summary>
        /// <returns>フレーム同期状態</returns>
        public ModemStatus getModemState()
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            int readLength =DecodeManager.sizeToLength(ModemRegisters.FRM_SYN_STATE_IND_SIZE);
            byte[] result = ComCtl.read(ModemRegisters.FRM_SYN_STATE_IND, readLength);
            ComAplMsgMng camm = new ComAplMsgMng();
            ComAplMsgMng.MsgType mt = camm.CheckRcvMsgFormat(result);
            ModemStatus ms = new ModemStatus();
            if (mt == ComAplMsgMng.MsgType.READ_ACK)
            {
                //0x05_0001				フレーム同期状態表示									
                //Bit	Bit Name			Function							
                //7-5	Reserved			-							
                //4	    TDM_SYNC_STATUS	    TDMA同期状態表示　(1:同期)　※プロト端末のみ							
                //3	    FSC_FSYNC_DET		フレーム同期状態表示　(1:同期)　※プロト端末のみ							
                //2	    RSS_SSYNC_DET		シンボル同期状態表示　(1:同期)　※プロト端末/疑似地上局共通							
                //1	    AFC_SYNC_DET		キャリア同期検出状態表示　(1:同期)　※プロト端末のみ"							
                //0	    AGC_LOCK_STATUS		AGC Lockステータス　　※プロト端末のみ
                //                          0：Unlock　1：Lock	
                if ((result[ComAplMsgMng.POS_READ_DATA_START] & ModemRegisters.AGC_LOCK_MASK) != 0)
                {
                    ms.AGCLock = true;
                }

                if ((result[ComAplMsgMng.POS_READ_DATA_START] & ModemRegisters.CARRIER_SYNC_MASK) != 0)
                {
                    ms.CarrierSync = true;
                }

                if ((result[ComAplMsgMng.POS_READ_DATA_START] & ModemRegisters.SYMBOL_SYNC_MASK) != 0)
                {
                    ms.SymbolSync = true;
                }

                if ((result[ComAplMsgMng.POS_READ_DATA_START] & ModemRegisters.FRAME_SYNC_MASK) != 0)
                {
                    ms.FrameSync = true;
                }

                if ((result[ComAplMsgMng.POS_READ_DATA_START] & ModemRegisters.TDMA_SYNC_MASK) != 0)
                {
                    ms.TDMASync = true;
                }

            }
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return ms;
        }

        /// <summary>
        /// 割り込みマスク設定
        /// </summary>
        /// <param name="im">割り込みマスク</param>
        /// <returns>設定結果　true:設定成功</returns>
        private Boolean setInterruptMask(InterruptMask im)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            int payload = 0;
            payload = ((im.lockMMCM ? 1 : 0) << ModemRegisters.LOCK_MMCM_BIT)
                      | ((im.errCRC ? 1 : 0) << ModemRegisters.ERR_CRC_BIT)
                      | ((im.errRS ? 1 : 0) << ModemRegisters.ERR_RS_BIT)
                      | ((im.rcvBuffOverFlow ? 1 : 0) << ModemRegisters.RCV_BUFF_OVER_FLOW_BIT)
                      | ((im.comCompLTC2611 ? 1 : 0) << ModemRegisters.COM_COMP_LTC2611_BIT)
                      | ((im.comCompAD9364 ? 1 : 0) << ModemRegisters.COM_COMP_AD9364_BIT)
                      | ((im.sndComp ? 1 : 0) << ModemRegisters.SND_COMP_BIT)
                      | ((im.rcvComp ? 1 : 0) << ModemRegisters.RCV_COMP_BIT)
                      | ((im.forceStopAMP ? 1 : 0) << ModemRegisters.FORCE_STOP_AMP_BIT)
                      | ((im.lockAGC ? 1 : 0) << ModemRegisters.LOCK_AGC_BIT)
                      | ((im.syncCarrier ? 1 : 0) << ModemRegisters.SYNC_CARRIER_BIT)
                      | ((im.syncSymbol ? 1 : 0) << ModemRegisters.SYNC_SYMBOL_BIT)
                      | ((im.syncFrame ? 1 : 0) << ModemRegisters.SYNC_FRAME_BIT)
                      | ((im.syncTDMA ? 1 : 0) << ModemRegisters.SYNC_TDMA_BIT)
                      | ((im.sndTDMA ? 1 : 0) << ModemRegisters.SND_TDMA_BIT)
                      | ((im.rcvTDMA ? 1 : 0) << ModemRegisters.RCV_TDMA_BIT)
                      | ((im.comCompLTC1197 ? 1 : 0) << ModemRegisters.COM_COMP_LTC1197_BIT)
                      | ((im.comCompHMC831 ? 1 : 0) << ModemRegisters.COM_COMP_HMC831_BIT)
                      | ((im.comCompVgaBit ? 1 : 0) << ModemRegisters.COM_COMP_VGA_BIT)
                      | ((im.txPllSyncChg ? 1 : 0) << ModemRegisters.TX_PLL_SYNC_CHG);


            ComAplMsgMng camm = new ComAplMsgMng();
            byte[] msg = camm.SetInterruptMaskBuilder(payload);
            byte[] result = ComCtl.sendEscpedMsg(msg);
            ComAplMsgMng.MsgType mt = camm.CheckRcvMsgFormat(result);
            if (mt == ComAplMsgMng.MsgType.WRITE_ACK)
            {
                LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
                return true;
            }
            else
            {
                LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
                return false;
            }
        }

        /// <summary>
        /// 送信バッファの空き状態チェック
        /// </summary>
        /// <returns>送信バッファの空き状態　true:バッファフル</returns>
        private Boolean isSendBuffFull()
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            Boolean isFull = true;
            int readLen = DecodeManager.sizeToLength(ModemRegisters.SND_DATA_BUF_STS_REPO_SIZE);

            byte[] result = ComCtl.read(ModemRegisters.SND_DATA_BUF_STS_REPO, readLen);
            ComAplMsgMng camm = new ComAplMsgMng();
            ComAplMsgMng.MsgType mt = camm.CheckRcvMsgFormat(result);
            if (mt == ComAplMsgMng.MsgType.READ_ACK)
            {
                if ((result[ComAplMsgMng.POS_READ_DATA_START] & ModemRegisters.BUFF_FULL) == 0)
                {
                    isFull = false;
                }
            }
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return isFull;
        }

        /// <summary>
        /// メッセージをバッファに書きこむ
        /// </summary>
        /// <param name="message">書き込むメッセージ</param>
        /// <returns>書き込み結果　true:書き込み成功</returns>
        private Boolean writeToBuff(byte[] message)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            if (message == null)
            {
                throw (new System.ArgumentNullException("メッセージがNULL"));
            }

            if (message.Length != DecodeManager.sizeToLength(MsgFromTerminal.SIZE) &&
                message.Length != DecodeManager.sizeToLength(MsgToTerminal.SIZE))
            {
                throw (new System.ArgumentOutOfRangeException("メッセージサイズ不正"));
            }

            int restLen = message.Length;
            int pos = 0;
            Boolean ret = false;
            while (restLen != 0)
            {
                int sendLen = 0;
                if (restLen < ModemRegisters.WRITE_DATA_MAX)
                {
                    sendLen = restLen;
                }
                else
                {
                    sendLen = ModemRegisters.WRITE_DATA_MAX;
                }
                byte[] wdata = new byte[sendLen];
                Array.Copy(message, pos, wdata, 0, sendLen);

                byte[] result = ComCtl.write(calBuffPosition(pos), wdata, false);
                ComAplMsgMng camm = new ComAplMsgMng();
                if (ComAplMsgMng.MsgType.WRITE_ACK != camm.CheckRcvMsgFormat(result))
                {
                    break;
                }
                restLen -= sendLen;
                pos += sendLen;
            }

            if (restLen == 0)
            {
                ret = true;
            }

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return ret;
        }


        /// <summary>
        /// メッセージを書き込む先頭アドレスの計算
        /// </summary>
        /// <param name="pos">バッファの先頭アドレスからの位置</param>
        /// <returns>メッセージを書き込む先頭アドレス</returns>
        private byte[] calBuffPosition(int pos)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            if (pos < 0 || (DecodeManager.sizeToLength(ModemRegisters.SND_DATA_BUF_SIZE) - 1) < pos)
            {
                throw (new System.ArgumentOutOfRangeException("メッセージ書き込み位置不正"));
            }

            int intAddr = ModemRegisters.SND_DATA_BUF_INT + pos;
            String strAddr = intAddr.ToString("X6");

            byte[] bAddr = new byte[3];
            bAddr[0] = Convert.ToByte(strAddr.Substring(0, 2), 16);
            bAddr[1] = Convert.ToByte(strAddr.Substring(2, 2), 16);
            bAddr[2] = Convert.ToByte(strAddr.Substring(4, 2), 16);

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return bAddr;
        }

        /// <summary>
        /// 送信要求を行う
        /// </summary>
        /// <returns>要求結果 true:書き込み成功</returns>
        private Boolean writeMsgSendReq()
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            Boolean ret = false;
            byte[] result = ComCtl.write(ModemRegisters.SND_REQ, ModemRegisters.WDATA_SED_REQ, false);
            ComAplMsgMng camm = new ComAplMsgMng();
            if (ComAplMsgMng.MsgType.WRITE_ACK == camm.CheckRcvMsgFormat(result))
            {
                ret = true;
            }
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "修了");
            return ret;
        }

        /// <summary>
        /// 送信処理の実態
        /// </summary>
        public void sendMsgPreProcess()
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            //TDMA同期確認
            if (ObjectKeeper.mode == ObjectKeeper.MODE_TERM)
            {
                if (isTDMASync() != true)
                {
                    throw (new System.OperationCanceledException("TDMA同期未確立"));
                }
            }

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "修了");
        }

        /// <summary>
        /// メッセージ送信実処理
        /// </summary>
        /// <param name="message"></param>
        public void sendMsgProcess(byte[] message)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            lock (msgSendLock)
            {
                waitSendBufNonFull();

                //メッセージの書き込み
                if (writeToBuff(message) == false)
                {
                    throw (new System.OperationCanceledException("メッセージ書き込み失敗"));
                }

                if (writeMsgSendReq() == false)
                {
                    throw (new System.OperationCanceledException("メッセージ送信要求書き込み失敗"));
                }

            }
            LogMng.MsgLogWrite(message, LogMng.SEND);
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "修了");
        }

        /// <summary>
        /// モデム割り込み受信処理
        /// </summary>
        /// <param name="sender">割り込み元</param>
        /// <param name="e">割り込み時のパラメータ</param>
        private void reciveModemIntr(object sender, ComCtl.modemInterruptEventArgs e)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            //送信完了か確認する
            int sendCompMask = 1 << ModemRegisters.SND_COMP_BIT;
            int comCompAD9364Mask = 1 << ModemRegisters.COM_COMP_AD9364_BIT;
            if ((e.intr_cause & sendCompMask) != 0)
            {
                if(are != null)
                {
                    //送信完了割り込み
                    are.Set();
                }
            }
            
            if ((e.intr_cause & comCompAD9364Mask) != 0)
            {
                if (areRF != null)
                {
                    lock (timelockobj)
                    {
                        isTimeOut = false;
                    }
                    //RF設定完了
                    areRF.Set();
                }

            }
            
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "修了");
        }

        /// <summary>
        /// 送信バッファの空きを確認する
        /// </summary>
        private void waitSendBufNonFull()
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            if(isSendBuffFull() == false)
            {
                return;
            }

            lock (ComCtl.modemInterruptHandlerLock)
            {
                ComCtl.modemInterruptEvent += mIhandler;
            }

            //割り込みマスク解除
            try
            {
                im.sndComp = false;
                if (setInterruptMask(im) != true)
                {
                    throw (new System.OperationCanceledException("割り込みマスク設定失敗"));
                }

                int wait_time = WAIT_TIME_TERM_MSG;
                if (ObjectKeeper.mode == ObjectKeeper.MODE_STATION)
                {
                    wait_time = WAIT_TIME_STATION_MSG;
                }

                //バッファ空き待ち
                are = new System.Threading.AutoResetEvent(false);
                are.WaitOne(wait_time * 2);

            }
            catch(Exception e)
            {
                throw(e);
            }
            finally
            {
                //待ち解除
                lock (ComCtl.modemInterruptHandlerLock)
                {
                    ComCtl.modemInterruptEvent -= mIhandler;
                }

                //割り込みマスク設定
                im.sndComp = true;
                if (setInterruptMask(im) != true)
                {
                    throw (new System.OperationCanceledException("割り込みマスク設定失敗"));
                }
            }

            if (isSendBuffFull() == true)
            {
                throw (new System.OperationCanceledException("送信バッファがあきませんでした"));
            }

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "修了");

        }

        /// <summary>
        /// 受信周波数設定
        /// </summary>
        /// <param name="frq"></param>
        public void setRcvFreq(int frq)
        {
            //2.8	RF Synthesizer
            doRFSynthesizer_RX(frq);
        }

        /// <summary>
        /// 受信PN設定
        /// </summary>
        /// <param name="pn">PN符号</param>
        public void setRcvPn( int pn)
        {
            //アップリンクデータ拡散コード0～31
            setUplinkDataSpreadCode(pn);
            //アップリンクプリアンブル拡散コード（LSFR初期値）
            setUplinkPreambleSpreadCodeSet(pn);
        }



        /// <summary>
        /// RFへの設定を送信する
        /// </summary>
        /// <param name="rw">
        /// 読み出し書き込み
        /// 読み出し：ComAplMsgMng.RF_READ　書き込み：ComAplMsgMng.RF_WRITE
        /// </param>
        /// <param name="addr">書き込み/読み出しアドレス</param>
        /// <param name="data">書き込みデータ、読み出しの場合は0を設定すること</param>
        private void sendRFSetting(int addr, int rw, int data)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            lock (ComCtl.modemInterruptHandlerLock)
            {
                ComCtl.modemInterruptEvent += mIhandler;
            }

            try
            {
                //割り込みマスク解除
                im.comCompAD9364 = false;
                if (setInterruptMask(im) != true)
                {
                    throw (new System.OperationCanceledException("割り込みマスク設定失敗"));
                }

                int wait_time = WAIT_TIME_TERM_MSG;
                if (ObjectKeeper.mode == ObjectKeeper.MODE_STATION)
                {
                    wait_time = WAIT_TIME_STATION_MSG;
                }

                ComAplMsgMng camm = new ComAplMsgMng();
                byte[] msg = camm.RFSettingBuilder(rw, addr, data);
                byte[] result = ComCtl.sendEscpedMsg(msg);

                if (camm.CheckRcvMsgFormat(result) == ComAplMsgMng.MsgType.WRITE_ACK)
                {
                    areRF = new System.Threading.AutoResetEvent(false);
                    byte[] modemAddr = ModemRegisters.AD9364_SPI_PROC_REQ;
                    byte[] wdata = ModemRegisters.WDATA_AD9364_SPI_PROC_REQ;
                    lock (timelockobj)
                    {
                        isTimeOut = true;
                    }
                    result = ComCtl.write(modemAddr, wdata, false);
                    if (camm.CheckRcvMsgFormat(result) == ComAplMsgMng.MsgType.WRITE_ACK)
                    {
                        //SPI通信完了待ち(AD9364)
                        areRF.WaitOne(wait_time * 2);
                    }
                    else
                    {
                        throw (new System.OperationCanceledException("RF SPI処理要求失敗"));
                    }
                }
                else
                {
                    throw (new System.OperationCanceledException("RF設定書き込み失敗"));
                }
            }
            catch(Exception e)
            {
                throw(e);
            }
            finally
            {
                //待ち解除
                lock (ComCtl.modemInterruptHandlerLock)
                {
                    ComCtl.modemInterruptEvent -= mIhandler;
                }

                //割り込みマスク設定
                im.comCompAD9364 = true;
                if (setInterruptMask(im) != true)
                {
                    throw (new System.OperationCanceledException("割り込みマスク設定失敗"));
                }
            }
            
            lock (timelockobj)
            {
                if (isTimeOut == true)
                {
                    isTimeOut = false;
                    throw (new System.TimeoutException("SPI通信完了待ち(AD9364)"));
                }
            }

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "修了");
        }

        /// <summary>
        /// RFへの書き込みを行う
        /// </summary>
        /// <param name="addr">書き込みアドレス</param>
        /// <param name="data">書き込みデータ</param>
        private void writeRFSetting(int addr, int data)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            sendRFSetting(addr, ComAplMsgMng.RF_WRITE, data);
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "修了");
        }

        /// <summary>
        /// RFからの読み出しを行う
        /// </summary>
        /// <param name="addr">読み出しアドレス</param>
        /// <returns></returns>
        private byte readRFSetting(int addr)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            sendRFSetting(addr, ComAplMsgMng.RF_READ, 0);
            byte[] readAddr = ModemRegisters.AD9364_SPI_READ_DATA;
            int readSize = DecodeManager.sizeToLength(ModemRegisters.AD9364_SPI_READ_DATA_SIZE);
            byte[] result = ComCtl.read(readAddr, readSize);
            ComAplMsgMng camm = new ComAplMsgMng();
            if(camm.CheckRcvMsgFormat(result) == ComAplMsgMng.MsgType.READ_ACK)
            {
                LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "修了");
                return result[ComAplMsgMng.POS_READ_DATA_START];
            }
            else
            {
                throw (new System.OperationCanceledException("応答異常"));
            }
            
        }


        /// <summary>
        /// RF制御 2.8.2	RF Synthesizer RX channel（CPU）の実行
        /// </summary>
        private void doRFSynthesizer_RX(int frq)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            RFParamters rfp = RFParamters.getInstance();
            Boolean sucessRX = false;
            for (int j = 0; j < RFSYNTHESIZER_RETRY_MAX && sucessRX == false ; j++)
            {
                writeRFSetting(0x261, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[0]"));
                writeRFSetting(0x248, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[2]"));
                writeRFSetting(0x246, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[4]"));
                writeRFSetting(0x249, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[6]"));
                writeRFSetting(0x23B, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[8]"));
                writeRFSetting(0x243, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[10]"));
                writeRFSetting(0x23D, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[12]"));
                writeRFSetting(0x015, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[14]"));
                writeRFSetting(0x013, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[16]"));
                System.Threading.Thread.Sleep(rfp.getValue("VND_RFPLL_WaitTime_0"));
                writeRFSetting(0x23D, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[17]"));
                int VND_RFPLL_WaitTime_1 = rfp.getValue("VND_RFPLL_WaitTime_1") / 1000; //us→ms変換
                System.Threading.Thread.Sleep(VND_RFPLL_WaitTime_1 + 1);//+1msは切り捨て分
                writeRFSetting(0x23D, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[19]"));
                writeRFSetting(0x23A, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[21]"));
                writeRFSetting(0x239, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[22]"));
                writeRFSetting(0x242, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[23]"));
                writeRFSetting(0x238, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[24]"));
                writeRFSetting(0x245, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[25]"));
                writeRFSetting(0x251, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[26]"));
                writeRFSetting(0x250, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[27]"));
                writeRFSetting(0x23B, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[28]"));
                writeRFSetting(0x23E, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[29]"));
                writeRFSetting(0x23F, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[30]"));
                writeRFSetting(0x240, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[31]"));
                writeRFSetting(0x233, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[43]"));
                writeRFSetting(0x234, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[44]"));
                writeRFSetting(0x235, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[45]"));
                writeRFSetting(0x232, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[46]"));
                writeRFSetting(0x231, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[47]"));
                writeRFSetting(0x005, rfp.getValue("VND_RFIC_RFSyn_" + frq + "-[48]"));
                System.Threading.Thread.Sleep(rfp.getValue("VND_RFIC_RfSynthLockWaitTime"));

                //read
                int read1 = readRFSetting(0x247);
                if ((read1 & RF_LOCK_MASK) == 0)
                {
                    int read11 = readRFSetting(0x247);
                    if ((read11 & RF_LOCK_MASK) == 0)
                    {
                        LogMng.AplLogDebug("RX RF PLL lock status is not locked");
                        continue;
                    }
                }
                sucessRX = true;
                LogMng.AplLogDebug("RX RF PLL lock status is locked");
                break;
            }

            if (sucessRX == false)
            {
                throw (new System.OperationCanceledException("RF Synthesizer(RX)が失敗しました"));
            }
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "修了");
        }


        /// <summary>
        /// アップリンクデータ拡散コードの設定
        /// 現状32個なのでこれ個別で行う
        /// </summary>
        /// <param name="pn">PN符号</param>
        private void setUplinkDataSpreadCode(int pn)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            EncodeManager em = new EncodeManager();

            RFParamters rfp = RFParamters.getInstance();
            int oneWdataSize = ModemRegisters.UP_DATA_SPREAD_C_00_SIZE;
            byte[] oneWdata = new byte[DecodeManager.sizeToLength(oneWdataSize)];
            byte[] sendAddr = (byte[])ModemRegisters.UP_DATA_SPREAD_C_00.Clone();

            //wdata作成
            int oneData = rfp.getValue("UP_SS_LSFR_INIT_D-" + pn);
            em.encode(oneData, oneWdataSize, oneWdata, 0); 
            byte[] result = ComCtl.write(sendAddr, oneWdata, false);


            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "修了");
        }

        /// <summary>
        /// アップリンクプリアンブル拡散コード（LSFR初期値）の設定
        /// </summary>
        /// <param name="pn">PN符号</param>
        private void setUplinkPreambleSpreadCodeSet(int pn)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            int oneWdataSize = ModemRegisters.UPLPRE_SPREAD_C_SIZE;
            byte[] oneWdata = new byte[DecodeManager.sizeToLength(oneWdataSize)];
            RFParamters rfp = RFParamters.getInstance();
            EncodeManager em = new EncodeManager();
            int oneData = rfp.getValue("UP_SS_LSFR_INIT_P-" + pn);
            em.encode(oneData, oneWdataSize, oneWdata, 0);
            ComCtl.write(ModemRegisters.UPLPRE_SPREAD_C, oneWdata, false);

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "修了");
        }


        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                if (are != null)
                {
                    are.Dispose();
                    are = null;
                }

                if (areRF != null)
                {
                    areRF.Dispose();
                    areRF = null;
                }

                if(areHMC831 != null)
                {
                    areHMC831.Dispose();
                    areHMC831 = null;
                }
            }
            // free native resources
        }

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }



    }
}
