/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
 */
using System;
using System.Text;

namespace QAnpiCtrlLib.log
{
    /// <summary>
    /// ログ出力管理
    /// </summary>
    public class LogMng
    {
        /// <summary>
        /// intのビット長
        /// </summary>
        protected const int INT_BIT_SIZE = 32;

        /// <summary>
        /// メッセージLog情報通知のデータ
        /// </summary>
        public class MsgLogData:EventArgs
        {
            /// <summary>
            /// メッセージLog　フォーマットはメッセージLogと同じ
            /// ＜端末発・端末宛メッセージ＞
            ///   送受信時刻,送受信,メッセージタイプ,表示文字,生データ
            /// ＜Ｓ帯モニタデータ＞
            ///   送受信時刻,送受信,経路,データ名,データID,表示文字,生データ
            /// </summary>
            public string msgData = null;
        }

        /// <summary>
        /// メッセージLog情報通知用デリゲート宣言
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void MsgLogHandler(object sender, MsgLogData e);

        /// <summary>
        /// メッセージイベント
        /// メッセージイベントが必要ならここに登録する
        /// </summary>
        public static event MsgLogHandler msgEvent; 

        /// <summary>
        /// Ｓ帯モニタデータ一覧表示の経路選択フィルタ設定
        /// </summary>
        private static consts.CommonConst.SystemNumber sbandMsgLogRouteFilter = consts.CommonConst.SystemNumber.SYSTEMNONE;

        /// <summary>
        /// sbandMsgLogRouteFilterのLock用
        /// </summary>
        private static Object filterLockObject = new Object();

        /// <summary>
        /// メッセージログ読み出し用ストリーム
        /// </summary>
        private static System.IO.StreamReader sr = null;

        /// <summary>
        /// メッセージログ読み出し用ストリームLock用
        /// </summary>
        private static Object srLockObject = new Object();


        /// <summary>
        /// メッセージログ保存用ストリーム
        /// </summary>
        private static System.IO.StreamWriter sw = null;

        /// <summary>
        /// メッセージログ保存用ストリームLock用
        /// </summary>
        private static Object swLockObject = new Object();


        /// <summary>
        /// メッセージの受信
        /// </summary>
        public const int SEND = 0x3c;
        
        /// <summary>
        /// メッセージ送信
        /// </summary>
        public const int RECIVE = 0x3e;

        /// <summary>
        /// Log出力用のlogger
        /// </summary>
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// アプリLog出力　Fatalレベル 
        /// </summary>
        /// <param name="message">出力メッセージ</param>
        public static void AplLogFatal(object message)
        {
            logger.Fatal(getCaller() + message);
        }

        /// <summary>
        /// アプリLog出力　Errorレベル 
        /// </summary>
        /// <param name="message">出力メッセージ</param>
        public static void AplLogError(object message)
        {
            logger.Error(getCaller() + message);
        }

        /// <summary>
        /// アプリLog出力　Warnレベル 
        /// </summary>
        /// <param name="message">出力メッセージ</param>
        public static void AplLogWarn(object message)
        {
            logger.Warn(getCaller() + message);
        }

        /// <summary>
        /// アプリLog出力　Infoレベル 
        /// </summary>
        /// <param name="message">出力メッセージ</param>
        public static void AplLogInfo(object message)
        {
            logger.Info(getCaller() + message);
        }

        /// <summary>
        /// アプリLog出力　Debugレベル 
        /// </summary>
        /// <param name="message">出力メッセージ</param>
        public static void AplLogDebug(object message)
        {
            logger.Debug(getCaller() + message);
        }

        /// <summary>
        /// 呼び出し元名の取得
        /// </summary>
        /// <returns></returns>
        private static string getCaller()
        {
            StringBuilder sb = new StringBuilder();
            int lineNumber = 0;
            System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(2, true);

            try
            {
                lineNumber = sf.GetFileLineNumber();
                string[] folders = sf.GetFileName().Split('\\');
                bool add = false;
                foreach (string s in folders)
                {
                    if (s.Equals("ShelterInfoSystem") == true)
                    {
                        add = true;
                    }
                    else if (add == true)
                    {
                        if (sb.Length != 0)
                        {
                            sb.Append(@"\");
                        }
                        sb.Append(s);
                    }
                }
            }
            catch
            {
                //DO NOTHING
            }
            return sb.ToString() + "(" + lineNumber + ")";
        }


        /// <summary>
        /// メッセージLogファイルオープン(ファイル指定)
        /// </summary>
        /// <param name="fileName">送受信メッセージログのファイル名</param>
        /// <param name="access">ファイルアクセス方法 write または read</param>
        public static void MsgLogOpen(String fileName, System.IO.FileAccess access)
        {
            if (access == System.IO.FileAccess.Write)
            {
                lock (swLockObject)
                {
                    sw = new System.IO.StreamWriter(fileName, true);
                }
            }
            else if (access == System.IO.FileAccess.Read)
            {
                lock (srLockObject)
                {
                    sr = new System.IO.StreamReader(fileName);
                }
            }
            else 
            {
                LogMng.AplLogError("アクセスモード不正");
                throw (new System.ArgumentException("アクセスモード不正"));
            }
        }

        /// <summary>
        /// メッセージLogファイルクローズ
        /// </summary>
        public static void MsgLogClose()
        {
            lock (srLockObject)
            {
                if (sr != null)
                {
                    sr.Close();
                    sr = null;
                }
            }

            lock (swLockObject)
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                    sw = null;
                }
            }
        }

        /// <summary>
        /// MsgLogOpen()でopenしたメッセージLogファイルへの書き込み
        /// 送受信済みメッセージ一覧表示画面への反映も行う
        /// </summary>
        /// <param name="message">安否メッセ―ジ(全タイプ)</param>
        /// <param name="direction">送受信　送信 LogMng.SEND、受信 LogMng.RECIVE</param>
        public static void MsgLogWrite(byte[] message, int direction)
        {
            // 生データを別に保存
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ms.Write(message, 0, message.Length);
            byte[] tmp_msg = ms.ToArray();
            ms.Close();

            // RSエラー情報ありの端末宛メッセージのbyteサイズ
            int rsMsgSize = msg.MsgToTerminal.sizeToLength(msg.MsgToTerminal.SIZE) + 1;
            // RSエラーあり？
            bool isRsError = false;


            // メッセージサイズ：端末宛メッセージ
            int msgSize = msg.MsgToTerminal.SIZE;
            if(message.Length == msg.MsgFromTerminal.sizeToLength(msg.MsgFromTerminal.SIZE))
            {
                // メッセージサイズ：端末発メッセージ
                msgSize = msg.MsgFromTerminal.SIZE;
            }
            // サイズが端末宛メッセージ＋1byteの場合、RSエラー情報ありの端末宛メッセージと判断
            else if (message.Length == rsMsgSize)
            {
                if (Convert.ToInt32(message[rsMsgSize - 1]) != 0)
                {
                    // RSエラー情報の内容が0以外ならRSエラー
                    isRsError = true;
                }
                else
                {
                    // RSエラーなし → RSエラー情報を削除してデコード処理
                    Array.Resize(ref tmp_msg, tmp_msg.Length - 1);
                }
            }
            else
            {
                // DO NOTHING
            }

            //メッセージタイプ、表示内容
            string msgtype = "";
            string dispData = "";
            try
            {
                if (isRsError)
                {
                    msgtype = Properties.Resources.W_common_msg_typ_rserr;
                    dispData = Properties.Resources.W_common_msg_typ_rserr;
                }
                else
                {
                    // ヘッダ部デコード
                    msg.DecodeManager dcm = new msg.DecodeManager();
                    msg.TypeAndSystemInfo typeAndSys = dcm.decodeTypeAndSystemInfo(tmp_msg, msgSize, false);

                    msgtype = typeAndSys.msgType.ToString();
                    dispData = getDispString(typeAndSys.msgType, tmp_msg);                
                }
            }
            catch(Exception e)
            {
                //サマリ文字列の取得失敗(失敗しても処理中断しない)
                LogMng.AplLogError(e.Message);
                LogMng.AplLogError(e.StackTrace);
            }

            //時間取得
            DateTime dt = DateTime.Now;

            //送受信方向
            string str_direction = Properties.Resources.W_common_dat_ttl_016;
            if(direction == RECIVE)
            {
                str_direction = Properties.Resources.W_common_dat_ttl_017;
            }

            //rawdata
            string str_message = "";
            foreach(byte b in message)
            {
                str_message += b.ToString("x2");
            }

            //データ結合
            StringBuilder sb = new StringBuilder();
            sb.Append(dt.ToString(@"yyyy/MM/dd HH:mm:ss.fff"));
            sb.Append(",");
            sb.Append(str_direction);
            sb.Append(",");
            sb.Append(msgtype);
            sb.Append(",");
            sb.Append(dispData);
            sb.Append(",");
            sb.Append(str_message);

            //イベント送信
            MsgLogData mld = new MsgLogData();
            mld.msgData = sb.ToString();
            if(msgEvent != null)
            {
                msgEvent(null, mld);
            }

            //MsgLogFileへの書き込み
            lock (swLockObject)
            {
                if (sw != null)
                {
                    sw.WriteLine(mld.msgData);
                }
            }

            //AplLogへの書き込み
            LogMng.AplLogInfo(mld.msgData);
        }

        /// <summary>
        /// メッセージLogファイルからの読み出し
        ///１行分のデータを読み出します
        /// </summary>
        /// <returns>メッセージLogファイルから読み出したデータ</returns>
        public static string MsgLogRead()
        {
            lock (srLockObject)
            {
                if (sr != null)
                {
                    return sr.ReadLine();
                }
                else
                {
                    throw (new System.InvalidOperationException("MsgLogOpen()がされてません"));
                }
            }
        }

        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得
        /// </summary>
        /// <param name="msgType">メッセージ種別</param>
        /// <param name="message">メッセージ</param>
        /// <returns></returns>
        private static string getDispString(int msgType, byte[] message)
        {
            switch(msgType)
            {
                case consts.EncDecConst.VAL_MSG_TYPE0:
                case consts.EncDecConst.VAL_MSG_TYPE1:
                    return getDispStringMsgType0_1(message);
                case consts.EncDecConst.VAL_MSG_TYPE2:
                    return getDispStringMsgType2(message);
                case consts.EncDecConst.VAL_MSG_TYPE3:
                    return getDispStringMsgType3(message);
                case consts.EncDecConst.VAL_MSG_TYPE100:
                    return getDispStringMsgType100(message);

                case consts.EncDecConst.VAL_MSG_TYPE130:
                    return getDispStringMsgType130(message);
                case consts.EncDecConst.VAL_MSG_TYPE150:
                    return getDispStringMsgType150(message);

                case consts.EncDecConst.VAL_MSG_TYPE255:
                    return getDispStringMsgType255(message);

                case consts.EncDecConst.VAL_MSG_SBAND_EQU_REQ:
                    return getDispStringMsgSBandEquReq(message);
                case consts.EncDecConst.VAL_MSG_SBAND_EQU_RSP:
                    return getDispStringMsgSBandEquRsp(message);
                case consts.EncDecConst.VAL_MSG_SBAND_TST_REQ:
                    return getDispStringMsgSBandTstReq(message);
                case consts.EncDecConst.VAL_MSG_SBAND_TST_RSP:
                    return getDispStringMsgSBandTstReq(message);
                case consts.EncDecConst.VAL_MSG_SBAND_FWD_REQ:
                    return getDispStringMsgSBandFwdReq(message);
                case consts.EncDecConst.VAL_MSG_SBAND_FWD_RSP:
                    return getDispStringMsgSBandFwdRsp(message);
                case consts.EncDecConst.VAL_MSG_SBAND_RTN_REQ:
                    return getDispStringMsgSBandRtnReq(message);
                case consts.EncDecConst.VAL_MSG_SBAND_RTN_RSP:
                    return getDispStringMsgSBandRtnRsp(message);
                case consts.EncDecConst.VAL_MSG_SBAND_DEV_RSP:
                    return getDispStringMsgSBandDevInfoSetRsp(message);

                default:
                    throw (new System.ArgumentException("不明なmsgType"));
            }

        }

        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得(メッセージタイプ０／１用)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>サマリストリング</returns>
        private static string getDispStringMsgType0_1(byte[] message)
        {
            // Type0はType1と同等なので問題なし
            msg.MsgType1 msgType1 = new msg.MsgType1();
            Array.Copy(message, msgType1.encodedData, message.Length);
            msgType1.decode(false);

            // 表示フォーマット
            //   Type:001 GID:ggg LAT:xxxxxx LON:yyyyyyy a, b, c, d, e			
            // 表示内容
            //   ggg       組番号
            //   xxxxxx    緯度(LSB:0.0001、北緯 20度を起点とした相対位置)
            //   yyyyyyy   経度(LSB:0.0001、東経110度を起点とした相対位置)
            //   a         体調
            //   b         同行者
            //   c         事故状況
            //   d         現在地
            //   e         救助要否
            StringBuilder sb = new StringBuilder();

            sb.Append("Type:" + msgType1.msgType.ToString("D3"));
            sb.Append(" GID:" + msgType1.gId.ToString("X3"));
#if false
            sb.Append(" LAT:" + msgType1.latitude);
            sb.Append(" LON:" + msgType1.longitude);

            sb.Append(" " + msgType1.healthStatus);
            sb.Append(", " + msgType1.companion);
            sb.Append(", " + msgType1.accidentSituation);
            sb.Append(", " + msgType1.location);
            sb.Append(", " + msgType1.rescueRequest);
#else
            sb.Append(" PID:" + msgType1.personalId);
            sb.Append(" " + msgType1.anpiInfo_prohibitOpen);
            sb.Append(", " + msgType1.anpiInfo.ToString("X1"));
            sb.Append(", " + msgType1.anpiInfoSupplement.ToString("X1"));
#endif

            return sb.ToString();
        }

        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得(メッセージタイプ２用)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>サマリストリング</returns>
        private static string getDispStringMsgType2(byte[] message)
        {
            msg.MsgType2 msgType2 = new msg.MsgType2();
            Array.Copy(message, msgType2.encodedData, message.Length);
            msgType2.decode(false);

            // 表示フォーマット
            //   Type:002 GID:ggg FM:f LM:l SN:nn
            // 表示内容
            //   ggg       組番号
            //   f         メッセージ先頭フラグ
            //   l         メッセージ最終フラグ
            //   nn        シーケンス番号
            StringBuilder sb = new StringBuilder();

            sb.Append("Type:" + msgType2.msgType.ToString("D3"));
            sb.Append(" GID:" + msgType2.gId.ToString("X3"));

            sb.Append(" FM:" + Convert.ToInt32(msgType2.firstMsgFlag));
            sb.Append(" LM:" + Convert.ToInt32(msgType2.lastMsgFlag));
            sb.Append(" SN:" + msgType2.sequenceNumber);

            return sb.ToString();
        }

        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得(メッセージタイプ3用)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>サマリストリング</returns>
        private static string getDispStringMsgType3(byte[] message)
        {
            msg.MsgType3 msgType3 = new msg.MsgType3();
            Array.Copy(message, msgType3.encodedData, message.Length);
            msgType3.decode(false);

            // 表示フォーマット
            //   Type:003 GID:ggg NRID:n RID:rid0, rid1, rid2 SIR:s			
            // 表示内容
            //   ggg       組番号
            //   n         救助支援情報ID数
            //   ridx      救助支援情報ID
            //   s         支援情報要求
            StringBuilder sb = new StringBuilder();

            sb.Append("Type:" + msgType3.msgType.ToString("D3"));
            sb.Append(" GID:" + msgType3.gId.ToString("X3"));

            sb.Append(" NRID:" + msgType3.rescueSupportInfoNumber);
            sb.Append(" RID:" + msgType3.rescueSupportInfoId0);
            sb.Append(", " + msgType3.rescueSupportInfoId1);
            sb.Append(", " + msgType3.rescueSupportInfoId2);
            sb.Append(" SIR:" + msgType3.supportInfoRequest);

            return sb.ToString();
        }

        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得(メッセージタイプ101用)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>サマリストリング</returns>
        private static string getDispStringMsgType100(byte[] message)
        {
            msg.MsgType100 MsgType100 = new msg.MsgType100();
            Array.Copy(message, MsgType100.encodedData, message.Length);
            MsgType100.decode(false, true);

            // 表示フォーマット
            //   Type:101 Sysinfo NCA:nn CA0:sf sc type gid, CA1:sf sc type gid, CA2:sf sc type gid,…
            // 表示内容
            //   Sysinfo   システム情報
            //   nn        送達確認数
            //   sf        送信周波数
            //   sc        送信PN符号
            //   type      端末発メッセージタイプ
            //   gid       組番号
            StringBuilder sb = new StringBuilder();
            
            sb.Append("Type:" + MsgType100.msgType.ToString("D3"));
            sb.Append(" " + getDispStringSystemInfo(MsgType100.sysInfo));

            sb.Append(" NCA:" + MsgType100.ackNum);
            if (MsgType100.ackNum > 0)
            {
                for (int i = 0; i < MsgType100.ackNum && i < MsgType100.acks.Length; i++)
                {
                    sb.Append(" CA" + i + ":");
                    sb.Append(MsgType100.acks[i].cid);
                    sb.Append(" " + MsgType100.acks[i].slotNum);
                    sb.Append(" " + MsgType100.acks[i].subslotBitmap);
                    sb.Append(",");
                }

                //最終カンマの削除
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }

#if false
        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得(メッセージタイプ102用)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>サマリストリング</returns>
        private static string getDispStringMsgType102(byte[] message)
        {
            msg.MsgType102 msgType102 = new msg.MsgType102();
            Array.Copy(message, msgType102.encodedData, message.Length);
            msgType102.decode(false, true);

            // 表示フォーマット
            //   Type:102 Sysinfo NAGID:nn MAGID:mm GID:ggg UB0:u Ubit:uuuuuuuuuuuuuuuuuuuuu…			
            // 表示内容
            //   Sysinfo   システム情報
            //   nn        送達確認組番号数
            //   mm        最大送達確認組番号
            //   gid       組番号
            //   u         ユーザビットオフセット
            //   uuuuuuu.. ユーザビット
            StringBuilder sb = new StringBuilder();

            sb.Append("Type:" + msgType102.msgType.ToString("D3"));
            sb.Append(" " + getDispStringSystemInfo(msgType102.sysInfo));

            sb.Append(" NAGID:" + msgType102.ackGIdNum);
            sb.Append(" MAGID:" + msgType102.ackGIdNumMax);
            sb.Append(" GID:" + msgType102.gId);
            sb.Append(" UBO:" + msgType102.userBitOffset);

            string userBitStr = "";
            foreach (byte b in msgType102.userBit)
            {
                // 1byte目を2進数文字列に変換          例：0xfe       0x0e       0x00
                string oneBitStr = Convert.ToString(b, 2);
                // 2進数文字列を数値(10進数)に変換     例：11111110   1110       0
                int oneBitNum = Convert.ToInt32(oneBitStr);
                // 数値を８桁で追加                    例：11111110   00001110   00000000
                userBitStr += oneBitNum.ToString("D8");
            }
            sb.Append(" Ubit:" + userBitStr);
            
            return sb.ToString();
        }
#endif

        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得(メッセージタイプ130用)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>サマリストリング</returns>
        private static string getDispStringMsgType130(byte[] message)
        {
            msg.MsgType130 msgType130 = new msg.MsgType130();
            Array.Copy(message, msgType130.encodedData, message.Length);
            msgType130.decode(false, true);

            // 表示フォーマット
            //   Type:130 Sysinfo NRSI:n Uid:UUUUUUUU t, Uid:UUUUUUUU t, Uid:UUUUUUUU t	
            // 表示内容
            //   Sysinfo   システム情報
            //   n         救助支援情報数
            //   UUUUUUUU  救助支援情報のユーザID
            //   t         救助支援情報の救助支援情報ID
            StringBuilder sb = new StringBuilder();

            sb.Append("Type:" + msgType130.msgType.ToString("D3"));
            sb.Append(" " + getDispStringSystemInfo(msgType130.sysInfo));

            sb.Append(" NRSI:" + msgType130.rescueSupportInfoNum);
            if (msgType130.rescueSupportInfoNum > 0)
            {
                for (int i = 0; i < msgType130.rescueSupportInfoNum && i < msgType130.rescueSupportInfos.Length; i++)
                {
                    sb.Append(" Cid:" + msgType130.rescueSupportInfos[i].cid);
                    sb.Append(" " + msgType130.rescueSupportInfos[i].rescueSupportInfoId);
                    sb.Append(",");
                }

                //最終カンマの削除
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }

        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得(メッセージタイプ150用)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>サマリストリング</returns>
        private static string getDispStringMsgType150(byte[] message)
        {
            msg.MsgType150 MsgType150 = new msg.MsgType150();
            Array.Copy(message, MsgType150.encodedData, message.Length);
            MsgType150.decode(false, true);

            // 表示フォーマット
            //   Type:150 Sysinfo Len:nnn ＭＭＭ...
            // 表示内容
            //   Sysinfo   システム情報
            //   nnn       メッセージ長
            //   ＭＭＭ..  報知テキスト
            StringBuilder sb = new StringBuilder();

            sb.Append("Type:" + MsgType150.msgType.ToString("D3"));
            sb.Append(" " + getDispStringSystemInfo(MsgType150.sysInfo));

            sb.Append(" Len:" + MsgType150.msgLength);
            sb.Append(" " + MsgType150.msg);

            return sb.ToString();
        }

#if false
        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得(メッセージタイプ200用)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>サマリストリング</returns>
        private static string getDispStringMsgType200(byte[] message)
        {
            msg.MsgType200 MsgType200 = new msg.MsgType200();
            Array.Copy(message, MsgType200.encodedData, message.Length);
            MsgType200.decode(false, true);

            // 表示フォーマット
            //   Type:200 Sysinfo
            // 表示内容
            //   Sysinfo   システム情報
            StringBuilder sb = new StringBuilder();

            sb.Append("Type:" + MsgType200.msgType.ToString("D3"));
            sb.Append(" " + getDispStringSystemInfo(MsgType200.sysInfo));

            return sb.ToString();
        }
#endif

        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得(メッセージタイプ255用)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>サマリストリング</returns>
        private static string getDispStringMsgType255(byte[] message)
        {
            msg.MsgType255 MsgType255 = new msg.MsgType255();
            Array.Copy(message, MsgType255.encodedData, message.Length);
            MsgType255.decode(false, true);

            // 表示フォーマット
            //   Type:255
            // 表示内容
            //   なし
            StringBuilder sb = new StringBuilder();

            sb.Append("Type:" + MsgType255.msgType.ToString("D3"));

            return sb.ToString();
        }

        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得(msg.SystemInfo用)
        /// </summary>
        /// <param name="sysInfo">msg.SystemInfo</param>
        /// <returns>サマリストリング</returns>
        private static string getDispStringSystemInfo(msg.SystemInfo sysInfo)
        {
            //RTACの算出
            DateTime dtToday = DateTime.Today;
            DateTime dtNewYearDay = new DateTime(dtToday.Year, 1, 1);

            DateTime dt = dtNewYearDay.AddMilliseconds(sysInfo.sysBaseTime * (consts.CommonConst.TICK_MILLISEC));

            long elapsedTicks = dtToday.Ticks - dt.Ticks;
            if(elapsedTicks < 0)
            {
                DateTime dtLastNewYearDay = new DateTime(dtToday.Year - 1 , 1, 1);
                dt = dtLastNewYearDay.AddMilliseconds(sysInfo.sysBaseTime * (consts.CommonConst.TICK_MILLISEC));
            }

            //FTS:ff, TDSG:tttttttttt, SRST:a, RTAC:yyyy/MM/dd HH:mm:ss.f, TSG:tt, RGT:r, SFQID:n, EFQID:n
            StringBuilder sb = new StringBuilder();
            sb.Append("FTS:" + sysInfo.sysFwdTimeSlot + ", ");
            sb.Append("TDSG:" + sysInfo.sysDelayTime + ", ");
//            sb.Append("ACK:" + sysInfo.sysAckType + ", ");
            sb.Append("SRST:" + sysInfo.sysSendRestriction + ", ");
            sb.Append("RTAC:" + dt.ToString(@"yyyy/MM/dd HH:mm:ss.f") + ", ");
            sb.Append("TSG:" + sysInfo.sysGroupNum + ", ");
            sb.Append("RGT:" + sysInfo.sysRandomSelectBand + ", ");
            sb.Append("SFQID:" + sysInfo.sysStartFreqId + ", ");
            sb.Append("EFQID:" + sysInfo.sysEndFreqId );

            return sb.ToString();
        }

        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得
        /// (Ｓ帯モニタデータ：設備ステータス)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>サマリストリング</returns>
        private static string getDispStringMsgSBandEquReq(byte[] message)
        {
            msg.MsgSBandEquStatReq sdata = new msg.MsgSBandEquStatReq();
            Array.Copy(message, sdata.encodedData, message.Length);
            sdata.decode(false);

            // 表示フォーマット
            //   EID:eeee
            // 表示内容
            //   eeee    装置ID
            StringBuilder sb = new StringBuilder();
            sb.Append("EID:" + sdata.eqId);

            return sb.ToString();
        }

        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得
        /// (Ｓ帯モニタデータ：設備ステータス応答)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>サマリストリング</returns>
        private static string getDispStringMsgSBandEquRsp(byte[] message)
        {
            msg.MsgSBandEquStatRsp sdata = new msg.MsgSBandEquStatRsp();
            Array.Copy(message, sdata.encodedData, message.Length);
            sdata.decode(false);

            // 表示フォーマット
            //   Date:days HH:mm:ss.fff,ggg SEQ:0xssssssss STS:a,0xbbbbbbbb,c,d,e,h,g,0xffffffff
            // 表示内容
            //   days HH:mm:ss.fff,ggg       データ時刻
            //   ssssssss				     シーケンスカウンタ(16進)		
            //   a                           運用状態
            //   bbbbbbbb                    Ｓ帯送受信状態
            //   c                           GPS受信状態
            //   d                           有線LAN状態
            //   e                           FAN状態
            //   ffffffff                    ソフトウェアアラーム(16進)
            //   g                           温度状態
            //   h                           電源電圧状態
            StringBuilder sb = new StringBuilder();

            sb.Append("Date:" + sdata.dataTime.dayOfYear.ToString("D3"));
            sb.Append(" " + sdata.dataTime.hour.ToString("D2"));
            sb.Append(":" + sdata.dataTime.min.ToString("D2"));
            sb.Append(":" + sdata.dataTime.sec.ToString("D2"));
            sb.Append("." + sdata.dataTime.msec.ToString("D3"));
            sb.Append("," + sdata.dataTime.usec.ToString("D3"));

            sb.Append(" SEQ:0x" + sdata.seqNum.ToString("X8"));
            sb.Append(" STS:" + sdata.equStatInfo.opeStat.ToString("D1"));
            sb.Append(",0x" + sdata.equStatInfo.sndrcvStat.ToString("X8"));

            sb.Append("," + sdata.equStatInfo.gpsStat.ToString("D1"));
            sb.Append("," + sdata.equStatInfo.lanStat.ToString("D1"));
            sb.Append("," + sdata.equStatInfo.fanStat.ToString("D1"));
            sb.Append("," + sdata.equStatInfo.voltStat.ToString("D1"));
            sb.Append("," + sdata.equStatInfo.tmpStat.ToString("D1"));
            sb.Append(",0x" + sdata.equStatInfo.swAlarm.ToString("X8"));

            return sb.ToString();
        }

        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得
        /// (Ｓ帯モニタデータ：回線試験データ)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>サマリストリング</returns>
        private static string getDispStringMsgSBandTstReq(byte[] message)
        {
            return "";
        }

        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得
        /// (Ｓ帯モニタデータ：回線試験データ応答)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>サマリストリング</returns>
        private static string getDispStringMsgSBandTstRsp(byte[] message)
        {
            return "";
        }

        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得
        /// (Ｓ帯モニタデータ：S帯FWDデータ取得要求)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>サマリストリング</returns>
        private static string getDispStringMsgSBandFwdReq(byte[] message)
        {
            msg.MsgSBandFwdGetReq sdata = new msg.MsgSBandFwdGetReq();
            Array.Copy(message, sdata.encodedData, message.Length);
            sdata.decode(false);

            // 表示フォーマット
            //   EID:eeee
            // 表示内容
            //   eeee    装置ID
            StringBuilder sb = new StringBuilder();
            sb.Append("EID:" + sdata.eqId);

            return sb.ToString();
        }

        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得
        /// (Ｓ帯モニタデータ：S帯FWDデータ取得応答)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>サマリストリング</returns>
        private static string getDispStringMsgSBandFwdRsp(byte[] message)
        {
            msg.MsgSBandFwdGetRsp sdata = new msg.MsgSBandFwdGetRsp();
            Array.Copy(message, sdata.encodedData, message.Length);
            sdata.decode(false);

            // 表示フォーマット
            //   RSSI:sss.ss RSCP:ccc.cc Fof:fff.ff CRC:c (XXXXX)		
            // 表示内容
            //   sss.ss    RSSI
            //   ccc.cc    RSCP
            //   fff.ff    周波数オフセット
            //   c         CRC結果
            //   XXXXX     Ｓ帯モニタ端末が受信した端末宛メッセージ
            StringBuilder sb = new StringBuilder();

            sb.Append("RSSI:" + sdata.fwdRcvInfo.rssi.ToString("F2"));
            sb.Append(" RSCP:" + sdata.fwdRcvInfo.rscp.ToString("F2"));
            sb.Append(" Fof:" + sdata.fwdRcvInfo.frqOffset.ToString("F2"));
            sb.Append(" CRC:" + sdata.fwdRcvInfo.crcResult);

            string msgStr = "";
            try
            {
                msgStr = getDispString(sdata.fwdRcvInfo.fwdDataMsgType, sdata.fwdRcvInfo.fwdData);
                sb.Append(" (" + msgStr + ")");
            }
            catch (Exception e)
            {
                LogMng.AplLogError(e.Message);
                LogMng.AplLogError(e.StackTrace);
            }

            return sb.ToString();
        }

        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得
        /// (Ｓ帯モニタデータ：S帯RTNデータ送信要求)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>サマリストリング</returns>
        private static string getDispStringMsgSBandRtnReq(byte[] message)
        {
            msg.MsgSBandRtnSendReq sdata = new msg.MsgSBandRtnSendReq();
            Array.Copy(message, sdata.encodedData, message.Length);
            sdata.decode(false);

            // 表示フォーマット
            //   EID:eee STIM:days HH:mm:ss.fff,ggg SPN:ppp SFQ:ff (XXXXXX)	
            // 表示内容
            //   eee                      装置ID
            //   days HH:mm:ss.fff,ggg    送信時刻
            //   ppp                      送信PN符号
            //   ff                       送信周波数
            //   XXXXX                    Ｓ帯モニタ端末が送信した端末発メッセージ
            StringBuilder sb = new StringBuilder();

            sb.Append("EID:" + sdata.eqId.ToString());

            sb.Append(" STIM:" + sdata.rtnSendInfo.sendTime.dayOfYear.ToString("D3"));
            sb.Append(" " + sdata.rtnSendInfo.sendTime.hour.ToString("D2"));
            sb.Append(":" + sdata.rtnSendInfo.sendTime.min.ToString("D2"));
            sb.Append(":" + sdata.rtnSendInfo.sendTime.sec.ToString("D2"));
            sb.Append("." + sdata.rtnSendInfo.sendTime.msec.ToString("D3"));
            sb.Append("," + sdata.rtnSendInfo.sendTime.usec.ToString("D3"));

            sb.Append(" SPN:" + sdata.rtnSendInfo.pn.ToString("D3"));
            sb.Append(" SFQ:" + sdata.rtnSendInfo.freq.ToString("D2"));

            string msgStr = "";
            try
            {
                msgStr = getDispString(sdata.rtnSendInfo.rtnDataMsgType, sdata.rtnSendInfo.rtnData);
                sb.Append(" (" + msgStr + ")");
            }
            catch (Exception e)
            {
                LogMng.AplLogError(e.Message);
                LogMng.AplLogError(e.StackTrace);
            }

            return sb.ToString();
        }

        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得
        /// (Ｓ帯モニタデータ：S帯RTNデータ送信応答)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>サマリストリング</returns>
        private static string getDispStringMsgSBandRtnRsp(byte[] message)
        {
            msg.MsgSBandRtnSendRsp sdata = new msg.MsgSBandRtnSendRsp();
            Array.Copy(message, sdata.encodedData, message.Length);
            sdata.decode(false);

            // 表示フォーマット
            //   REST:xx (XXXXXX)			
            // 表示内容
            //   xx                       応答結果コード
            //   XXXXX                    Ｓ帯モニタ端末が送信した端末発メッセージ
            StringBuilder sb = new StringBuilder();

            sb.Append("REST:" + sdata.rspResult.ToString());

            string msgStr = "";
            try
            {
                msgStr = getDispString(sdata.rtnSendInfo.rtnDataMsgType, sdata.rtnSendInfo.rtnData);
                sb.Append(" (" + msgStr + ")");
            }
            catch (Exception e)
            {
                LogMng.AplLogError(e.Message);
                LogMng.AplLogError(e.StackTrace);
            }

            return sb.ToString();
        }

        /// <summary>
        /// ログ/履歴画面に表示するメッセージのサマリストリングの取得
        /// (Ｓ帯モニタデータ：装置固有情報設定応答)
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>サマリストリング</returns>
        private static string getDispStringMsgSBandDevInfoSetRsp(byte[] message)
        {
            msg.MsgSBandDevInfoSetRsp sdata = new msg.MsgSBandDevInfoSetRsp();
            Array.Copy(message, sdata.encodedData, message.Length);
            sdata.decode(false);

            // 表示フォーマット
            //   REST:xx
            // 表示内容
            //   xx     応答結果コード
            StringBuilder sb = new StringBuilder();
            sb.Append("REST:" + sdata.rspResult.ToString());

            return sb.ToString();
        }


        /// <summary>
        /// メッセージ送受信のイベントが登録されているかを確認する
        /// </summary>
        /// <returns>
        /// true：登録されている
        /// false:登録されていない
        /// </returns>
        public static Boolean isEventResgistered()
        {
            if(msgEvent != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// MsgLogOpen()でopenしたメッセージLogファイルへの書き込み
        /// Ｓ帯モニタ送受信済みメッセージ一覧表示画面への反映も行う
        /// </summary>
        /// <param name="message">安否メッセ―ジ(全タイプ)</param>
        /// <param name="direction">送受信　送信 LogMng.SEND、受信 LogMng.RECIVE</param>
        /// <param name="sysNum">系番号</param>
        public static void SBandMsgLogWrite(byte[] message, int direction, consts.CommonConst.SystemNumber sysNum)
        {

            int dataType, dataSize, dataId;
            string dataName = "";
            int result;

            msg.DecodeManager dcm = new msg.DecodeManager();

            // ヘッダ部デコード
            result = dcm.decodeSBandDataHeader(message, out dataType, out dataSize, out dataName);
            if (result == 0 && dataType == 0 && dataSize == 0 && dataName == "")
            {
                LogMng.AplLogError("decodeSBandDataHeader error!");
                return;
            }

            // Ｓ帯RTNデータ送信要求 かつ 受信 の場合
            // Ｓ帯RTNデータ送信応答 に変更
            if ((consts.EncDecConst.SIZE_MSG_SBAND_RTN_RSP == consts.EncDecConst.SIZE_MSG_SBAND_RTN_REQ) &&
                (dataType == consts.EncDecConst.VAL_MSG_SBAND_RTN_REQ) &&
                (direction == RECIVE))
            {
                dataType = consts.EncDecConst.VAL_MSG_SBAND_RTN_RSP;
                dataName = "W_CTF202_dat_nam_08";// Properties.Resources.W_CTF202_dat_nam_08;
            }

            // データID不明ならIDをそのまま設定
            if (result == 0)
            {
                dataId = dataType;
            }
            else
            {
                dataId = dataType % consts.EncDecConst.CALC_DATAID_DENOMI;
            }

            string dispData = "";
            // サマリ文字列の取得
            try
            {
                dispData = getDispString(dataType, message);
            }
            catch (Exception e)
            {
                // 取得失敗(失敗しても処理中断しない)
                LogMng.AplLogError(e.Message);
                LogMng.AplLogError(e.StackTrace);

                // 不明なデータのサイズを出力
                StringBuilder strbuf = new StringBuilder();
                strbuf.Append("Size:" + dataSize);
                dispData = strbuf.ToString();
            }


            // 時間取得
            DateTime dt = DateTime.Now;

            // 送受信方向
            string str_direction = Properties.Resources.W_common_dat_ttl_016;
            if (direction == RECIVE)
            {
                str_direction = Properties.Resources.W_common_dat_ttl_017;
            }

            // 生データ
            string rawData = "";
            foreach (byte b in message)
            {
                rawData += b.ToString("x2");
            }

            // データ結合
            StringBuilder sb = new StringBuilder();
            sb.Append(dt.ToString(@"yyyy/MM/dd HH:mm:ss.fff"));
            sb.Append("," + str_direction);
            sb.Append("," + (int)sysNum);
            sb.Append("," + dataName);
            sb.Append("," + dataId);
            sb.Append("," + dispData);
            sb.Append("," + rawData);


            // イベント送信
            MsgLogData mld = new MsgLogData();
            mld.msgData = sb.ToString();
            if (msgEvent != null)
            {
                msgEvent(null, mld);
            }

            // MsgLogFileへの書き込み
            lock (swLockObject)
            {
                if (sw != null)
                {
                    // 送受信経路選択によるフィルタ
                    if ((sbandMsgLogRouteFilter == consts.CommonConst.SystemNumber.SYSTEM1_2) ||
                        (sbandMsgLogRouteFilter == sysNum))
                    {
                        sw.WriteLine(mld.msgData);
                    }
                }
            }

            // AplLogへの書き込み
            LogMng.AplLogInfo(mld.msgData);

        }

        /// <summary>
        /// 送受信経路選択フィルタ設定
        /// </summary>
        /// <param name="route">系番号</param>
        public static void setSBandMsgLogRouteFilter(consts.CommonConst.SystemNumber route)
        {
            lock (filterLockObject)
            {
                sbandMsgLogRouteFilter = route;
            }
        }


    }
}
