/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using QAnpiCtrlLib.log;
using QAnpiCtrlLib.consts;
using QAnpiCtrlLib.utils;

namespace ShelterInfoSystem.control
{
    class ComAplMsgMng
    {
        /// <summary>
        /// 応答の種別
        ///   READ_ACK  :リードACK
        ///   WRITE_ACK :ライトACK
        ///   ERROR_ACK :エラーACK
        ///   MSG_RCV   :メッセージ受信
        ///   MOD_INTR  :モデムからの割り込み
        /// </summary>
        public enum MsgType { READ_ACK, WRITE_ACK, ERROR_ACK, MSG_RCV, MOD_INTR };

        /// <summary>
        /// メッセージの先頭/終端
        /// </summary>
        private const byte HEAD_TAIL = 0x7e;

        /// <summary>
        /// メッセージ種別識別子　WRITE
        /// </summary>
        private const byte ID_MSG_WRITE = 0x77;

        /// <summary>
        /// メッセージ種別識別子　READ
        /// </summary>
        private const byte ID_MSG_READ = 0x72;

        /// <summary>
        /// メッセージ種別識別子　COMMAND
        /// </summary>
        private const byte ID_MSG_COMMAND = 0x63;

        /// <summary>
        /// メッセージ種別識別子　ACK OK
        /// </summary>
        private const byte ID_ACK_OK = 0x6f;

        /// <summary>
        /// メッセージ種別識別子　ACK NG
        /// </summary>
        private const byte ID_ACK_NG = 0x78;

        /// <summary>
        /// メッセージ種別識別子　メッセージ受信
        /// </summary>
        private const byte ID_MSG_RCV = 0x6d;

        /// <summary>
        /// メッセージ種別識別子　モデムからの割り込み
        /// </summary>
        private const byte ID_MOD_INTR = 0x69;

        /// <summary>
        /// メッセージ保存識別子　メッセージ保存
        /// </summary>
        private const byte ID_SAV_TRUE = 0x73;

        /// <summary>
        /// メッセージ保存識別子　メッセージ非保存
        /// </summary>
        private const byte ID_SAV_FALSE = 0x00;

        /// <summary>
        /// エスケープ文字　フラグシーケンス
        /// </summary>
        private const byte ID_ESC_FLAG = 0x7e;

        /// <summary>
        /// エスケープ文字　コントロールエスケイプ
        /// </summary>
        private const byte ID_ESC_CONTROL = 0x7d;


        /// <summary>
        /// フラグシーケンスの置き換えbyte列
        /// </summary>
        private byte[] ESC_BYTE_FLAG = { 0x7d, 0x5e };

        /// <summary>
        /// コントロールエスケイプの置き換えbyte列
        /// </summary>
        private byte[] ESC_BYTE_CONTROL = { 0x7d, 0x5d };


        /// <summary>
        /// バイト列中のメッセージの先頭位置
        /// </summary>
        private const int POS_HEAD = 0;

        /// <summary>
        /// バイト列中のメッセージ種別識別子の位置　
        /// </summary>
        private const int POS_ID = 1;

        /// <summary>
        /// メッセージ受信コマンド内の送受信方向の位置　
        /// </summary>
        private const int POS_DIRECTION = 2;

        /// <summary>
        /// メッセージ受信コマンド内のメッセージの先頭位置　
        /// </summary>
        private const int POS_MSG_HEAD = 3;

        
        /// <summary>
        /// ライトACKの長さ　
        /// </summary>
        private const int LEN_WRITE_ACK = 3;

        /// <summary>
        /// メッセージ受信コマンド内のメッセージに関連しない部分の長さ　
        /// </summary>
        private const int LEN_NON_MSG = 4;

        /// <summary>
        /// メッセージのデータ部の最小長　
        /// </summary>
        private const int LEN_WDATA_MIN = 1;

        /// <summary>
        /// メッセージのデータ部の最大値　
        /// </summary>
        private const int LEN_WDATA_MAX = 64;

        /// <summary>
        /// メッセージのアドレス部の長さ　
        /// </summary>
        private const int LEN_MSG_ADR = 3;

        /// <summary>
        /// 時刻設定コマンド
        /// </summary>
        private byte[] BYTE_COMMAND_SETTIME = { (byte)'s', (byte)'e', (byte)'t', (byte)'t', (byte)'i', (byte)'m', (byte)'e' };
        /// <summary>
        /// 割り込みマスク設定コマンド
        /// </summary>
        private string STR_COMMAND_INTRMASK = "setinterruptmask";
        /// <summary>
        /// RSSI取得コマンド
        /// </summary>
        private string STR_COMMAND_GETRSSI = "getRSSI()";
        /// <summary>
        /// RFパラメータ取得コマンド
        /// </summary>
        private string STR_COMMAND_GETRFPARAMTER = "getrfparameter";
        /// <summary>
        /// "0"の文字コード
        /// </summary>
        private byte BYTE_ASCII_ZERO = (byte)'0';

        /// <summary>
        /// READ_ACKのData部の開始位置(byte)　
        /// </summary>
        public const int POS_READ_DATA_START = 2;

        /// <summary>
        /// MOD_INTRの割り込み要因部の開始位置(byte)　
        /// </summary>
        public const int POS_CAUSE_START = 2;

        /// <summary>
        /// MOD_INTRの割り込み要因部の長さ(byte)　
        /// </summary>
        public const int CAUSE_DATA_LENGTH = 4;

        /// <summary>
        /// RSSIのデータの長さ(byte)　
        /// </summary>
        public const int RSSI_DATA_LENGTH = 8;

        /// <summary>
        /// RFへデータ書き込み
        /// </summary>
        public const int RF_WRITE = 1;
        /// <summary>
        /// RFからのデータ読み出し
        /// </summary>
        public const int RF_READ = 0;

        /// <summary>
        /// RFへデータ書き込み
        /// </summary>
        public const int HMC831_WRITE = 1;
        /// <summary>
        /// RFからのデータ読み出し
        /// </summary>
        public const int HMC831_READ = 0;

        /// <summary>
        /// メッセージ受信コマンド解析結果
        /// </summary>
        public class MsgRcvCommad
        {
            public int    dirction;
            public byte[] message;
        }

        /// <summary>
        /// GPS ON/OFF設定コマンド名
        /// </summary>
        private string STR_COMMAND_SETGPSONOFF = "setGPSONOFF";
        /// <summary>
        /// GPS ON/OFF取得コマンド名
        /// </summary>
        private string STR_COMMAND_GETGPSONOFF = "getGPSONOFF";
        /// <summary>
        /// 位置設定コマンド名
        /// </summary>
        private string STR_COMMAND_SETLOCATION = "setLOCATION";
        /// <summary>
        /// 位置取得コマンド名
        /// </summary>
        private string STR_COMMAND_GETLOCATION = "getLOCATION";

        /// <summary>
        /// GPS ON/OFFコマンド 値の長さ
        /// </summary>
        private const int GPSONOFF_VAL_LENGTH = 1;
        /// <summary>
        /// GPS ON/OFF値  GPS機能ON
        /// </summary>
        private const byte GPS_ON = (byte)'1';
        /// <summary>
        /// GPS ON/OFF値  GPS機能OFF
        /// </summary>
        private const byte GPS_OFF = (byte)'0';
        /// <summary>
        /// 位置コマンド値の長さ
        /// </summary>
        private const int LOCATION_VAL_LENGTH = 32;

        /// <summary>
        /// 位置情報
        /// </summary>
        public class Location
        {
            /// <summary>
            /// 緯度
            /// </summary>
            public double latitude;
            /// <summary>
            /// 経度
            /// </summary>
            public double longitude;
        }

        /// <summary>
        /// double型のバイト長
        /// </summary>
        private const int DOUBLE_BYTE_LENGTH = 8;

        /// <summary>
        /// RSCP取得コマンド
        /// </summary>
        private string STR_COMMAND_GETRSCP = "getRSCP()";

        /// <summary>
        /// 周波数オフセット取得コマンド
        /// </summary>
        private string STR_COMMAND_GETFRQOFFSET = "getFREQOFFSET()";

        /// <summary>
        /// RFパラメータ設定コマンド
        /// </summary>
        private string STR_COMMAND_SETRFPARAM = "setrfparameter";

        /// <summary>
        /// string型の場合のコマンドの開始位置(文字数)
        /// [Head][ID][LEN]で3byte 1byteが2文字なので6文字
        /// </summary>
        private int POS_STR_COMMAND_START = 6;

        /// <summary>
        /// 受信したデータのフォーマットチェックを行います
        /// フォーマットがCPU-BOARD⇒制御PCのメッセージかチェックしその種別を返します。
        /// </summary>
        /// <param name="reciveData">受信したメッセージ</param>
        /// <returns>メッセージの種別</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        public MsgType CheckRcvMsgFormat(byte[] reciveData)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

        　　//null check
            if (reciveData == null)
            {
                LogMng.AplLogError("入力値　null");
                throw(new System.ArgumentNullException());
            }

            if (reciveData.Length < LEN_WRITE_ACK)
            {
                LogMng.AplLogError("メッセージ長がライトアックより短い");
                throw (new System.FormatException("メッセージ長がライトアックより短い"));
            }

            //メッセージの先頭終端チェック
            if (reciveData[POS_HEAD] != HEAD_TAIL || reciveData[reciveData.Length - 1] != HEAD_TAIL)
            {
                LogMng.AplLogError("メッセージの先頭終端チェックエラー");
                throw (new System.FormatException("メッセージの先頭終端チェックエラー"));
            }

            //メッセージ種別チェック
            switch (reciveData[POS_ID])
            {
                case ID_ACK_OK:
                    //処理正常終了のACK　長さからリードかライトかの種類を判断する。
                    if (reciveData.Length == LEN_WRITE_ACK)
                    {
                        //データを含まないACKなのでライトのACKと判断
                        LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
                        return MsgType.WRITE_ACK;
                    }
                    else
                    {
                        //データを含むのACKなのでリードのACKと判断
                        LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
                        return MsgType.READ_ACK;
                    }
                    
                case ID_ACK_NG:
                    //処理異常終了のACK
                    LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
                    return MsgType.ERROR_ACK;
                case ID_MSG_RCV:
                    //メッセージ受信応答
                    LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
                    return MsgType.MSG_RCV;
                case ID_MOD_INTR:
                    //モデムからの割り込み
                    LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
                    return MsgType.MOD_INTR;
                default:
                    //CPU-BOARD⇒制御PCのメッセージでないため例外を投げる
                    LogMng.AplLogError("メッセージの種別対象外");
                    throw (new System.FormatException("メッセージの種別対象外"));
            }
        }


        /// <summary>
        /// メッセージ受信コマンドからメッセージの送受信方向とメッセージ自身を切り出す
        /// </summary>
        /// <param name="commadMsgRcv">メッセージ受信コマンド</param>
        /// <returns>メッセージの送受信方向とメッセージ自身</returns>
        /// <exception cref="System.FormatException"></exception>
        public MsgRcvCommad GetMsgFromReciveData(byte[] commadMsgRcv)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            MsgType msgtype; 

            //メッセージ種別の確認
            try
            {
                msgtype = CheckRcvMsgFormat(commadMsgRcv);
            }
            catch(System.Exception e)
            {
                throw (e);
            }

            if (msgtype != MsgType.MSG_RCV )
            {
                //メッセージ受信コマンドではないで例外を投げる
                LogMng.AplLogError("メッセージの種別対象外");
                throw (new System.FormatException("メッセージの種別対象外"));
            }

            MsgRcvCommad msgrcvcommand = new MsgRcvCommad();
                
            //メッセージの送受信方向の切り出し
            msgrcvcommand.dirction = commadMsgRcv[POS_DIRECTION];
            //メッセージ自身の切り出し
            System.IO.MemoryStream ms = new System.IO.MemoryStream(commadMsgRcv);
            msgrcvcommand.message = new byte[commadMsgRcv.Length - LEN_NON_MSG];
            ms.Position = POS_MSG_HEAD;
            ms.Read(msgrcvcommand.message, 0, commadMsgRcv.Length - LEN_NON_MSG);
            ms.Close();

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return msgrcvcommand;
        }

        /// <summary>
        /// メッセージの中のエスケープ対象のエスケープを行う
        /// </summary>
        /// <param name="listNonEscMsg">エスケープされていないメッセージ</param>
        /// <returns>エスケープされたメッセージ</returns>
        public byte[] EscapeMsg(List<byte> listNonEscMsg)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            byte[] arrayNonEscMsg = listNonEscMsg.ToArray();
            List<byte> listEscMsg = new List<byte>();

            //1byte目はエスケース対象外
            listEscMsg.Add(arrayNonEscMsg[0]);

            //2byte目から最終-1byte目まではエスケープ対象
            for(int i = 1; i < arrayNonEscMsg.Length - 1; i++)
            {
                if(arrayNonEscMsg[i] == ID_ESC_FLAG)
                {
                    listEscMsg.AddRange(ESC_BYTE_FLAG);
                }
                else if(arrayNonEscMsg[i] == ID_ESC_CONTROL)
                {
                    listEscMsg.AddRange(ESC_BYTE_CONTROL);
                }
                else
                {
                    listEscMsg.Add(arrayNonEscMsg[i]);
                }
            }

            //最終byteはエスケース対象外
            listEscMsg.Add(arrayNonEscMsg[arrayNonEscMsg.Length - 1]);

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return listEscMsg.ToArray();
        }

        /// <summary>
        /// エスケープされたメッセージをエスケープされていない状態にする
        /// </summary>
        /// <param name="msEscMsg">エスケープされたメッセージ</param>
        /// <returns>エスケープされていないメッセージ</returns>
        public byte[] deEscapeMsg(System.IO.MemoryStream msEscMsg)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            List<byte> listDeEscMsg = new List<byte>();
            byte[] byteEscMsg = msEscMsg.ToArray();

            //1byte目は対象外
            listDeEscMsg.Add(byteEscMsg[0]);

            //2byte目から最終-2byte目までは復元対象対象
            Boolean isLast = false;
            for (int i = 1; i < byteEscMsg.Length - 2; i++)
            {
                if (byteEscMsg[i] == ESC_BYTE_CONTROL[0] && byteEscMsg[i+1] == ESC_BYTE_CONTROL[1])
                {
                    //コントロールエスケイプ
                    listDeEscMsg.Add(ID_ESC_CONTROL);
                    if (i == byteEscMsg.Length - 3)
                    {
                        //最後のbyteがエスケープ対象
                        isLast = true;
                    }
                    i++;

                }
                else if (byteEscMsg[i] == ESC_BYTE_FLAG[0] && byteEscMsg[i + 1] == ESC_BYTE_FLAG[1])
                {
                    //フラグシーケンス
                    listDeEscMsg.Add(ID_ESC_FLAG);
                    if(i == byteEscMsg.Length - 3)
                    {
                        //データの最後のbyteがエスケープ対象
                        isLast = true;
                    }
                    i++;
                }
                else
                {
                    listDeEscMsg.Add(byteEscMsg[i]);
                }
            }

            
            if (isLast == false)
            {
                //データの最後のbyteがエスケープ対象でなかったので追加
                listDeEscMsg.Add(byteEscMsg[byteEscMsg.Length - 2]);
            }

            //最終byteは対象外
            listDeEscMsg.Add(byteEscMsg[byteEscMsg.Length - 1]);

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return listDeEscMsg.ToArray();
        }

        /// <summary>
        /// 制御PC->CPUボードのメッセージの内、ライトメッセージを作成する
        /// </summary>
        /// <param name="adr">書き込み先アドレス</param>
        /// <param name="wdata">書き込むデータ</param>
        /// <param name="save">書き込み内容をCPUボードに記録するか</param>
        /// <returns>ライトメッセージ</returns>
        public byte[] WriteMsgBuilder(byte[] adr, byte[] wdata, Boolean save)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            if (wdata.Length < LEN_WDATA_MIN || LEN_WDATA_MAX < wdata.Length)
            {
                LogMng.AplLogError("wdataの長さ不正");
                throw (new System.FormatException("wdataの長さ不正"));
            }

            if (adr.Length != LEN_MSG_ADR)
            {
                LogMng.AplLogError("adrの長さ不正");
                throw (new System.FormatException("adrの長さ不正"));
            }


            List<byte> listMsg = new List<byte>();
 
            //ライトメッセージのフォーマット
            //[7e] [77] [<len=1byte>] [<adr=3byte>] [<wdata=n byte>][<save=1 byte>] [7e]
            listMsg.Add(HEAD_TAIL);
            listMsg.Add(ID_MSG_WRITE);
            listMsg.Add((byte)wdata.Length);
            listMsg.AddRange(getAddr(adr));
            listMsg.AddRange(wdata);
            if (save)
            {
                listMsg.Add(ID_SAV_TRUE);
            }
            else
            {
                listMsg.Add(ID_SAV_FALSE);
            }
            listMsg.Add(HEAD_TAIL);

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return EscapeMsg(listMsg);
        }

        /// <summary>
        /// 制御PC->CPUボードのメッセージの内、リードメッセージを作成する
        /// </summary>
        /// <param name="adr">読み出し元のアドレス</param>
        /// <param name="len">読み出すサイズ</param>
        /// <returns>リードメッセージ</returns>
        public byte[] ReadMsgBuilder(byte[] adr, int len)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            if (len < LEN_WDATA_MIN || LEN_WDATA_MAX < len)
            {
                LogMng.AplLogError("lenの長さ不正");
                throw (new System.FormatException("lenの長さ不正"));
            }

            if (adr.Length != LEN_MSG_ADR)
            {
                LogMng.AplLogError("adrの長さ不正");
                throw (new System.FormatException("adrの長さ不正"));
            }


            List<byte> listMsg = new List<byte>();

            //リードメッセージのフォーマット
            //[7e] [72] [<len=1byte>] [<adr=3byte>] [7e]
            listMsg.Add(HEAD_TAIL);
            listMsg.Add(ID_MSG_READ);
            listMsg.Add((byte)len);
            listMsg.AddRange(getAddr(adr));
            listMsg.Add(HEAD_TAIL);

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return EscapeMsg(listMsg);
        }


        /// <summary>
        /// 時刻設定メッセージ作成
        /// </summary>
        /// <param name="settime"></param>
        /// <returns></returns>
        public byte[] TimeSetBuilder(DateTime settime)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            List<byte> listMsg = new List<byte>();
            //時刻設定メッセージのフォーマット
            //[7e] [63] [<len=1byte>] [<cmd= n byte>] [7e]
            //cmd = "settime(yyyy:mm:dd:hh:mm:ss)"
            //len = 28
            listMsg.Add(HEAD_TAIL);
            listMsg.Add(ID_MSG_COMMAND);
            listMsg.Add((byte)("settime(yyyy:mm:dd:hh:mm:ss)".Length));
            listMsg.AddRange(BYTE_COMMAND_SETTIME);
            listMsg.Add((byte)'(');
            //年の分解
            int year = settime.Year;
            listMsg.Add((byte)(BYTE_ASCII_ZERO + (year / 1000)));
            year -= (year / 1000) * 1000;
            listMsg.Add((byte)(BYTE_ASCII_ZERO + ( year / 100 )));
            year -= (year / 100) * 100;
            listMsg.Add((byte)(BYTE_ASCII_ZERO + (year / 10)));
            listMsg.Add((byte)(BYTE_ASCII_ZERO + (year % 10)));

            //セパレータ
            listMsg.Add((byte)':');
            
            //月の分解
            int month = settime.Month;
            listMsg.Add((byte)(BYTE_ASCII_ZERO + (month / 10)));
            listMsg.Add((byte)(BYTE_ASCII_ZERO + (month % 10)));

            //セパレータ
            listMsg.Add((byte)':');

            //日の分解
            int day = settime.Day;
            listMsg.Add((byte)(BYTE_ASCII_ZERO + (day / 10)));
            listMsg.Add((byte)(BYTE_ASCII_ZERO + (day % 10)));

            //セパレータ
            listMsg.Add((byte)':');

            //時の分解
            int hour = settime.Hour;
            listMsg.Add((byte)(BYTE_ASCII_ZERO +(hour / 10)));
            listMsg.Add((byte)(BYTE_ASCII_ZERO +(hour % 10)));

            //セパレータ
            listMsg.Add((byte)':');

            //分の分解
            int mimute = settime.Minute;
            listMsg.Add((byte)(BYTE_ASCII_ZERO +(mimute / 10)));
            listMsg.Add((byte)(BYTE_ASCII_ZERO +(mimute % 10)));

            //セパレータ
            listMsg.Add((byte)':');

            //秒の分解
            int second = settime.Second;
            listMsg.Add((byte)(BYTE_ASCII_ZERO + (second / 10)));
            listMsg.Add((byte)(BYTE_ASCII_ZERO + (second % 10)));

            listMsg.Add((byte)')');

            //終端
            listMsg.Add(HEAD_TAIL);

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

            return EscapeMsg(listMsg);
        }

        /// <summary>
        /// 割り込みマスク設定メッセージ作成
        /// </summary>
        /// <param name="mask">割り込みマスク</param>
        /// <returns>エスケープされたメッセージ</returns>
        public byte[] SetInterruptMaskBuilder(int mask)
        {
            List<byte> listMsg = new List<byte>();
            //割り込みマスク設定メッセージのフォーマット
            //[7e] [63] [<len=1byte>] [<cmd= n byte>] [7e]
            //cmd = "setinterruptmask(0xXXXXXXXX)"
            //len = 28
            listMsg.Add(HEAD_TAIL);
            listMsg.Add(ID_MSG_COMMAND);
            listMsg.Add((byte)("setinterruptmask(0xXXXXXXXX)".Length));
            listMsg.AddRange(System.Text.Encoding.GetEncoding("ascii").GetBytes(STR_COMMAND_INTRMASK));
            listMsg.Add((byte)'(');
            listMsg.AddRange(System.Text.Encoding.GetEncoding("ascii").GetBytes("0x" + mask.ToString("X8")));
            listMsg.Add((byte)')');

            //終端
            listMsg.Add(HEAD_TAIL);

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

            return EscapeMsg(listMsg);

        }

        /// <summary>
        /// 制御ツールの動作モードに合わせたモデムのレジスタのアドレスを返す
        /// </summary>
        /// <param name="srcAddr">プロトタイプ端末動作時のアドレス</param>
        /// <returns></returns>
        public byte[] getAddr(byte[] srcAddr)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            byte[] distAddr = new byte[3];
            Array.Copy(srcAddr, distAddr, distAddr.Length);
            if(ObjectKeeper.mode == ObjectKeeper.MODE_STATION)
            {
                distAddr[0] = (byte)(ModemRegisters.STATION_ADDR_BIT | distAddr[0]);
            }
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return distAddr;
        }

        /// <summary>
        /// RSSI取得コマンドの作成
        /// </summary>
        /// <returns></returns>
        public byte[] GetRSSIBuilder()
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            List<byte> listMsg = new List<byte>();
            //RSSI取得コマンドのフォーマット
            //[7e] [63] [<len=1byte>] [<cmd= n byte>] [7e]
            //cmd = "getRSSI()"
            //len = 9
            listMsg.Add(HEAD_TAIL);
            listMsg.Add(ID_MSG_COMMAND);
            listMsg.Add((byte)(STR_COMMAND_GETRSSI.Length));
            listMsg.AddRange(System.Text.Encoding.GetEncoding("ascii").GetBytes(STR_COMMAND_GETRSSI));
            listMsg.Add(HEAD_TAIL);

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return EscapeMsg(listMsg);
        }

        /// <summary>
        /// PF設定コマンドの作成
        /// </summary>
        /// <param name="rw">読み出し：RF_READ　書き込み：RF_WRITE</param>
        /// <param name="addr">RFのレジスタのアドレス</param>
        /// <param name="value">RFに設定する値</param>
        /// <returns></returns>
        public byte[] RFSettingBuilder(int rw, int addr, int value)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            int data_rw = rw << 23;
            int data_adr = addr <<8;
            int data_value = value;

            int data = data_rw | data_adr | data_value;

            byte[] buffer = BitConverter.GetBytes(data);
            List<byte> list = new List<byte>();
            list.AddRange(buffer);
            list.Reverse();
            
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return WriteMsgBuilder(ModemRegisters.AD9364_SPI_MAN_WRITE_DATA, list.ToArray(), false);
        }

        /// <summary>
        /// RFパラメータ取得コマンドの作成
        /// </summary>
        /// <param name="parameters">パラメータ名</param>
        /// <returns>RFパラメータ取得コマンド</returns>
        public byte[] GetRfParameterBuilder(string[] parameters)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            List<byte> listMsg = new List<byte>();
            //RFパラメータ取得のフォーマット
            //[7e] [63] [<len=1byte>] [<cmd= n byte>] [7e]
            //cmd = "getrfparameter(xxxxxxx)"
            //xxxxxxxx：システム仕様書_RF制御_別紙1_パラメータファイル.xlsの変数名
            //          変数名の指定が無い場合はすべての変数について応答を返す。
            //          応答では、変数名と値を”=”で区切り、変数と変数を”,”で区切る
            //len = 14 + 1 + 全変数の文字数の合計 + (変数の個数-1)(”,”の数) + 1

            //lenの計算
            int len = 0;
            len = STR_COMMAND_GETRFPARAMTER.Length;
            len = len + "(".Length;
            if (parameters != null)
            {
                //1個目
                len = len + parameters[0].Length;
                for (int i = 1; i < parameters.Length; i++)
                {
                    //二個目以降
                    len = len + ",".Length;
                    len = len + parameters[i].Length;
                }
            }
            len = len + ")".Length;

            if(len > byte.MaxValue)
            {
                throw (new System.OverflowException("パラメータが多過ぎます"));
            }

            listMsg.Add(HEAD_TAIL);
            listMsg.Add(ID_MSG_COMMAND);
            listMsg.Add((byte)(len));
            listMsg.AddRange(System.Text.Encoding.GetEncoding("ascii").GetBytes(STR_COMMAND_GETRFPARAMTER));
            listMsg.Add((byte)'(');
            if (parameters != null)
            {
                //1個目
                listMsg.AddRange(System.Text.Encoding.GetEncoding("ascii").GetBytes(parameters[0]));
                for (int i = 1; i < parameters.Length; i++)
                {
                    listMsg.Add((byte)',');
                    listMsg.AddRange(System.Text.Encoding.GetEncoding("ascii").GetBytes(parameters[i]));
                }
            }
            listMsg.Add((byte)')');
            listMsg.Add(HEAD_TAIL);

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return EscapeMsg(listMsg);

        }

        /// <summary>
        /// 通信アプリから受け取ったRFのパラメータを格納するクラス
        /// </summary>
        public class RFData
        {
            public string paraName;
            public long value;
        }

        /// <summary>
        /// 通信アプリから受け取ったREAD ACKからRFのパラメータをを取り出す
        /// </summary>
        /// <param name="readAck">READ ACK</param>
        /// <returns>RFのパラメータ</returns>
        public RFData[] getRFParamFromReadAck(byte[] readAck)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            List<RFData> listParam = new List<RFData>();
            //READ ACKの書式
            //[7e][6f]<rdata>[7e]
            //rdata=<parameterName>=<value>,<parameterName>=<value>,...,<parameterName>=<value>
            int rdataLen = readAck.Length - POS_READ_DATA_START - 1; //(-1)は終端の分
            string rdata = System.Text.Encoding.GetEncoding("utf-8").GetString(readAck, POS_READ_DATA_START, rdataLen);
            string[] rparams = rdata.Split(',');
            foreach(string onedata in rparams)
            {
                string[] splitdata = onedata.Split('=');
                if(splitdata.Length == 2)
                {
                    RFData rfd = new RFData();
                    rfd.paraName = splitdata[0];
                    try
                    {
                        rfd.value = Convert.ToInt64(splitdata[1], 16);
                    }
                    catch
                    {
                        LogMng.AplLogDebug("getRFParamFromReadAck valueが数字ではありません。" + splitdata[1]);
                        continue;
                    }
                    listParam.Add(rfd);
                }
            }

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return listParam.ToArray();
        }

        /// <summary>
        /// 受信したデータに2個以上のメッセージが入っている可能性がある。
        /// 2個以上のメッセージが入っていたら分割して返す
        /// </summary>
        /// <param name="recivedData">受信したデータ</param>
        /// <param name="ms">メッセージとして成立していないデータを書き込むためのメモリストリーム</param>
        /// <returns>分割されたメッセージ</returns>
        public List<byte[]> spriteMessages(byte[] recivedData, System.IO.MemoryStream ms)
        {
            List<byte[]> spriteData = new List<byte[]>();
            List<byte> oneData = new List<byte>();
            Boolean head = false;
            for(int i = 0; i < recivedData.Length; i++)
            {
                if(recivedData[i] == HEAD_TAIL)
                {
                    if(head == false)
                    {
                        head = true;
                    }
                    else if (oneData.Count == 1)
                    {
                        //HEAD_TAIL(7E)が連続している場合
                        //後ろのHEAD_TAIL(7E)が先頭
                        continue;
                    }
                    else
                    {
                        oneData.Add(recivedData[i]);
                        spriteData.Add(oneData.ToArray());
                        head = false;
                        oneData.Clear();
                    }
                }

                if(head == true)
                {
                    oneData.Add(recivedData[i]);
                }
            }
            if(oneData.Count != 0)
            {
                ms.Write(oneData.ToArray(), 0, oneData.Count);
            }
            return spriteData;
        }

        /// <summary>
        /// GPS ON/OFF設定コマンド作成
        /// </summary>
        /// <param name="onoff">true:ON false:OFF</param>
        /// <returns>GPS ON/OFF設定コマンド</returns>
        public byte[] gpsOnoffSetBuilder( bool onoff)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            List<byte> listMsg = new List<byte>();
            //GPS ON/OFF設定コマンドのフォーマット
            //[7e] [63] [<len=1byte>] [<cmd= n byte>] [7e]
            //cmd = "setGPSONOFF([para=1byte])"
            //para = "30"(on) or "31"(off)
            //len = 14
            listMsg.Add(HEAD_TAIL);
            listMsg.Add(ID_MSG_COMMAND);
            listMsg.Add((byte)(STR_COMMAND_SETGPSONOFF.Length + "(".Length + GPSONOFF_VAL_LENGTH + ")".Length));
            listMsg.AddRange(System.Text.Encoding.GetEncoding("ascii").GetBytes(STR_COMMAND_SETGPSONOFF));
            listMsg.Add((byte)'(');
            listMsg.Add(onoff ? GPS_ON : GPS_OFF);
            listMsg.Add((byte)')');
            listMsg.Add(HEAD_TAIL); 
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

            return EscapeMsg(listMsg);
        }

        /// <summary>
        /// GPS ON/OFF取得コマンド作成
        /// </summary>
        /// <returns>GPS ON/OFF取得コマンド</returns>
        public byte[] gpsOnoffGetBuilder()
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            List<byte> listMsg = new List<byte>();
            //GPS ON/OFF取得コマンドのフォーマット
            //[7e] [63] [<len=1byte>] [<cmd= n byte>] [7e]
            //cmd = "getGPSONOFF()"
            //len = 13
            listMsg.Add(HEAD_TAIL);
            listMsg.Add(ID_MSG_COMMAND);
            listMsg.Add((byte)(STR_COMMAND_GETGPSONOFF.Length + "(".Length + ")".Length));
            listMsg.AddRange(System.Text.Encoding.GetEncoding("ascii").GetBytes(STR_COMMAND_GETGPSONOFF));
            listMsg.Add((byte)'(');
            listMsg.Add((byte)')');
            listMsg.Add(HEAD_TAIL); 
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

            return EscapeMsg(listMsg);
        }

        /// <summary>
        /// 位置情報設定コマンド作成
        /// </summary>
        /// <param name="latitude">緯度</param>
        /// <param name="longitude">経度</param>
        /// <returns>位置情報設定コマンド</returns>
        public byte[] locationSetBuilder(double latitude, double longitude)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            List<byte> listMsg = new List<byte>();
            //位置情報設定コマンドのフォーマット
            //[7e] [63] [<len=1byte>] [<cmd= n byte>] [7e]
            //cmd = "setLOCATION([para=32byte])"
            //para = [longitude=16byte][latitude=16byte]
            //len = 45
            
            //コマンド部分組立
            List<byte> listCmd = new List<byte>();
            listCmd.AddRange(System.Text.Encoding.GetEncoding("ascii").GetBytes(STR_COMMAND_SETLOCATION));
            listCmd.Add((byte)'(');
            listCmd.AddRange(CommonUtils.doubleToBytes(longitude));
            listCmd.AddRange(CommonUtils.doubleToBytes(latitude));
            listCmd.Add((byte)')');
 
            //メッセージ全体組立
            listMsg.Add(HEAD_TAIL);
            listMsg.Add(ID_MSG_COMMAND);
            listMsg.Add((byte)(listCmd.Count()));
            listMsg.AddRange(listCmd.ToArray());
            listMsg.Add(HEAD_TAIL);

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

            return EscapeMsg(listMsg);
        }

        /// <summary>
        /// 位置情報取得コマンド作成
        /// </summary>
        /// <returns>位置情報取得コマンド</returns>
        public byte[] locationGetBuilder()
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            List<byte> listMsg = new List<byte>();
            //GPS ON/OFF取得コマンドのフォーマット
            //[7e] [63] [<len=1byte>] [<cmd= n byte>] [7e]
            //cmd = "getLOCATION()"
            //len = 13
            listMsg.Add(HEAD_TAIL);
            listMsg.Add(ID_MSG_COMMAND);
            listMsg.Add((byte)(STR_COMMAND_GETLOCATION.Length + "(".Length + ")".Length));
            listMsg.AddRange(System.Text.Encoding.GetEncoding("ascii").GetBytes(STR_COMMAND_GETLOCATION));
            listMsg.Add((byte)'(');
            listMsg.Add((byte)')');
            listMsg.Add(HEAD_TAIL);
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

            return EscapeMsg(listMsg);
        }

        /// <summary>
        /// ReadAckからGPSのON/OFFを取得する
        /// </summary>
        /// <param name="ack">ReadAck</param>
        /// <returns>true:ON false:OFF</returns>
        public bool getGpsOnOffFormReadAck(byte[] ack)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            bool ret = false;
            if(ack[POS_READ_DATA_START] == GPS_ON)
            {
                ret = true;
            }
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

            return ret;
        }

        /// <summary>
        /// ReadAckから位置情報を読み出す。
        /// </summary>
        /// <param name="ack">ReadAck</param>
        /// <returns>location(緯度、経度)</returns>
        public Location getLocationFormReadAck(byte[] ack)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            Location ret = new Location();
            byte[] Bytes = new byte[DOUBLE_BYTE_LENGTH];

            //longitude
            Array.Copy(ack, POS_READ_DATA_START, Bytes, 0, DOUBLE_BYTE_LENGTH);
            Array.Reverse(Bytes, 0, DOUBLE_BYTE_LENGTH);
            ret.longitude = BitConverter.ToDouble(Bytes, 0);

            //latitude
            Array.Copy(ack, POS_READ_DATA_START + DOUBLE_BYTE_LENGTH, Bytes, 0, DOUBLE_BYTE_LENGTH);
            Array.Reverse(Bytes, 0, DOUBLE_BYTE_LENGTH);
            ret.latitude = BitConverter.ToDouble(Bytes, 0);

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

            return ret;
        }

        /// <summary>
        /// RSCP取得コマンドの作成
        /// </summary>
        /// <returns></returns>
        public byte[] GetRSCPBuilder()
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            List<byte> listMsg = new List<byte>();
            //RSSI取得コマンドのフォーマット
            //[7e] [63] [<len=1byte>] [<cmd= n byte>] [7e]
            //cmd = "getRSCP()"
            //len = 9
            listMsg.Add(HEAD_TAIL);
            listMsg.Add(ID_MSG_COMMAND);
            listMsg.Add((byte)(STR_COMMAND_GETRSCP.Length));
            listMsg.AddRange(System.Text.Encoding.GetEncoding("ascii").GetBytes(STR_COMMAND_GETRSCP));
            listMsg.Add(HEAD_TAIL);

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return EscapeMsg(listMsg);
        }

        /// <summary>
        /// 周波数オフセット取得コマンドの作成
        /// </summary>
        /// <returns></returns>
        public byte[] GetFrqOffsetBuilder()
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            List<byte> listMsg = new List<byte>();
            //RSSI取得コマンドのフォーマット
            //[7e] [63] [<len=1byte>] [<cmd= n byte>] [7e]
            //cmd = "getFRQOFFSET()"
            //len = 14
            listMsg.Add(HEAD_TAIL);
            listMsg.Add(ID_MSG_COMMAND);
            listMsg.Add((byte)(STR_COMMAND_GETFRQOFFSET.Length));
            listMsg.AddRange(System.Text.Encoding.GetEncoding("ascii").GetBytes(STR_COMMAND_GETFRQOFFSET));
            listMsg.Add(HEAD_TAIL);

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return EscapeMsg(listMsg);
        }

        /// <summary>
        /// RFパラメータ設定コマンドからパラメータ名と値を取り出す
        /// </summary>
        /// <param name="setRfParameterCmd">RFパラメータ設定コマンド</param>
        /// <returns>パラメータ名と値</returns>
        public RFData parseSetRfParameter(string setRfParameterCmd)
        {
            //Format check
            //[7e] [63] [<len=1byte>] [<cmd= n byte>] [7e]
            //cmd = "setrfparameter([<para=n byte>])"
            //para = [<name>],[<vale>]

            StringBuilder sb = new StringBuilder();
            sb.Append(HEAD_TAIL.ToString("X2"));
            sb.Append(ID_MSG_COMMAND.ToString("X2"));
            if(setRfParameterCmd.StartsWith(sb.ToString()) == false)
            {
                LogMng.AplLogError("メッセージ不正" + setRfParameterCmd);
                throw (new ArgumentException("コマンドではありません"));
            }

            String dataLine = setRfParameterCmd.Substring(POS_STR_COMMAND_START);
            
            List<byte> setRfCmdByteList = new List<byte>();
            setRfCmdByteList.AddRange(System.Text.Encoding.GetEncoding("ascii").GetBytes(STR_COMMAND_SETRFPARAM));
            setRfCmdByteList.Add((byte)'(');
            sb.Clear();
            foreach(byte b in setRfCmdByteList.ToArray())
            {
                sb.Append(b.ToString("X2"));
            }
            
            if (dataLine.StartsWith(sb.ToString()))
            {
                //"setrfparameter(" で始まっている
                //パラメータ名抜出
                String[] splited = setRfParameterCmd.Split(new String[] { ((byte)'(').ToString("X2"), ((byte)')').ToString("X2") }, 3, System.StringSplitOptions.None);
                String[] nameValue = splited[1].Split(new String[] { ((byte)'=').ToString("X2") }, 2, System.StringSplitOptions.None);
                RFData rfData = new RFData();
                UTF8Encoding encoding = new UTF8Encoding(true, false);
                rfData.paraName = encoding.GetString(CommonUtils.ToByteArray(nameValue[0]));
                string valStr = encoding.GetString(CommonUtils.ToByteArray(nameValue[1]));

                Int32Converter i32Ccoverter = new Int32Converter();

                rfData.value = Convert.ToInt32(valStr, 16);

                return rfData;
            }
            LogMng.AplLogError("メッセージ不正" + setRfParameterCmd);
            throw (new ArgumentException("RFパラメータ設定コマンドではありません"));
        }

    }
}
