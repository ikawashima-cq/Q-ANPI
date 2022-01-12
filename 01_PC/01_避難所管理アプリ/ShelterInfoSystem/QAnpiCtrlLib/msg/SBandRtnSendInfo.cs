/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
 */
using System;
using QAnpiCtrlLib.consts;
using QAnpiCtrlLib.utils;
using QAnpiCtrlLib.log;

namespace QAnpiCtrlLib.msg
{
    /// <summary>
    /// Ｓ帯RTNデータ送信要求・応答
    /// データ部 Ｓ帯RTN送信情報 管理
    /// </summary>
    public class SBandRtnSendInfo : EncodeManager
    {
        // データ構造
        // --------------------------------------
        // Offset  Size    項目
        // --------------------------------------
        //     0    64     送信時刻
        // (  64     7     空き            )
        //    71     9     送信PN符号
        // (  80    12     空き            )
        //    92     4     送信周波数
        //    96    84     Ｓ帯RTNデータ
        // ( 180    12 )   0補填           )
        // --------------------------------------
        //   合計  192 bit (24 byte)

        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "SBandRtnSendInfo";

        /// <summary>
        /// 送信時刻
        /// </summary>
        public TimeUsec sendTime;

        /// <summary>
        /// 送信PN符号
        /// </summary>
        public int pn;

        /// <summary>
        /// 送信周波数
        /// </summary>
        public int freq;

        /// <summary>
        /// Ｓ帯RTNデータ＝端末発メッセージ相当
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public byte[] rtnData;

        /// <summary>
        /// Ｓ帯RTNデータの端末発メッセージType
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public int rtnDataMsgType;


        /// <summary>
        /// Ｓ帯RTNデータ名＝端末発メッセージ相当
        /// </summary>
        [System.Xml.Serialization.XmlElement("msg")]
        public string rtnDataStr = "";
        
        /// <summary>
        /// 空き１のサイズ(bit)
        /// </summary>
        public const int SIZE_BLANK_1 = 7;

        /// <summary>
        /// 送信PN符号のサイズ(bit)
        /// </summary>
        public const int SIZE_PN = 9;

        /// <summary>
        /// 空き２のサイズ(bit)
        /// </summary>
        public const int SIZE_BLANK_2 = 12;

        /// <summary>
        /// 送信周波数のサイズ(bit)
        /// ※エンデコ時は8bitで処理して上位4bitはマスクして扱う
        /// </summary>
        public const int SIZE_FREQ = 4; 

        /// <summary>
        /// Ｓ帯RTNデータのサイズ(bit)
        /// </summary>
        public const int SIZE_RTN_DATA = 84;

        /// <summary>
        /// ０補填のサイズ(bit)
        /// </summary>
        public const int SIZE_ZERO_FILL = 12;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SBandRtnSendInfo()
        {
            sendTime = new TimeUsec();

            int len = sizeToLength(SIZE_RTN_DATA);
            rtnData = new byte[len];
        }

        /// <summary>
        /// エンコード
        /// </summary>
        /// <param name="encodedData">エンコードデータの保存先</param>
        /// <param name="startPos">エンコードデータの保存位置</param>
        /// <returns>
        /// エンコード終了位置
        /// 0の場合は処理失敗
        /// </returns>
        public int encode(byte[] encodedData, int startPos)
        {
            int result = 0;
            int enc_data;
            int enc_size;

            try
            {
                // Ｓ帯RTN送信情報：送信時刻
                startPos = this.sendTime.encode(encodedData, startPos);
                if (startPos == 0)
                {
                    return result;
                }

                // Ｓ帯RTN送信情報：空き１
                startPos += SIZE_BLANK_1;

                // Ｓ帯RTN送信情報：送信PN符号
                enc_data = this.pn;
                enc_size = SIZE_PN;
                encode(enc_data, enc_size, encodedData, startPos);
                startPos += enc_size;

                // Ｓ帯RTN送信情報：空き２
                startPos += SIZE_BLANK_2;

                // Ｓ帯RTN送信情報：送信周波数
                enc_data = this.freq;
                enc_size = SIZE_FREQ;
                encode(enc_data, enc_size, encodedData, startPos);
                startPos += SIZE_FREQ;

                // Ｓ帯RTN送信情報：Ｓ帯RTNデータ
                enc_size = SIZE_RTN_DATA;
                encode(this.rtnData, enc_size, encodedData, startPos);
                startPos += enc_size;

                // Ｓ帯RTN送信情報：０補填
                startPos += SIZE_ZERO_FILL;
                
                result = startPos;
            }
            catch (Exception ex)
            {
                result = 0;
                LogMng.AplLogError("SBandRtnSendInfo encode() error:" + ex.Message);
            }

            return result;
        }

        /// <summary>
        /// デコード
        /// </summary>
        /// <param name="encodedData">デコード対象のデータ</param>
        /// <param name="startPos">デコード開始位置</param>
        /// <returns>
        /// デコード終了位置
        /// 0の場合は処理失敗
        /// </returns>
        public int decode(byte[] encodedData, int startPos)
        {
            int result = 0;
            int dec_size = 0;

            try
            {
                // Ｓ帯FWD受信情報：受信時刻
                startPos = this.sendTime.decode(encodedData, startPos);
                if (startPos == 0)
                {
                    return result;
                }

                // Ｓ帯RTN送信情報：空き１
                startPos += SIZE_BLANK_1;

                // Ｓ帯RTN送信情報：送信PN符号
                dec_size = SIZE_PN;
                this.pn = decodeInt(encodedData, dec_size, startPos);
                startPos += dec_size;

                // Ｓ帯RTN送信情報：空き２
                startPos += SIZE_BLANK_2;

                // Ｓ帯RTN送信情報：送信周波数
                dec_size = SIZE_FREQ;
                this.freq = decodeInt(encodedData, dec_size, startPos);
                startPos += dec_size;

                // Ｓ帯RTN送信情報：Ｓ帯RTNデータ
                dec_size = SIZE_RTN_DATA;
                decodeByteArray(encodedData, dec_size, startPos, this.rtnData);
                startPos += dec_size;

                // Ｓ帯RTN送信情報：０補填
                startPos += SIZE_ZERO_FILL;

                result = startPos;

                // Ｓ帯RTNデータの端末発メッセージのType取得
                msg.DecodeManager dm = new msg.DecodeManager();
                msg.TypeAndSystemInfo tasi = dm.decodeTypeAndSystemInfo(
                    this.rtnData, MsgFromTerminal.SIZE, false);
                rtnDataMsgType = tasi.msgType;
            }
            catch (Exception ex)
            {
                result = 0;
                LogMng.AplLogError("SBandRtnSendInfo decode() error:" + ex.Message);
            }

            return result;
        }


        /// <summary>
        /// 有効値パラメータチェック
        /// </summary>
        /// <returns>
        /// 有効値パラメータチェック結果
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#NG}
        /// </returns>
        public int checkParam()
        {
            int result = EncDecConst.OK;

            // 送信時刻の値チェック
            int chk_time = sendTime.checkParam();
            if (chk_time != EncDecConst.OK)
            {
                LogMng.AplLogError(TAG + "checkParam : sendTime is invalid");
                result = EncDecConst.NG;
            }
            
            // 送信PN符号の値チェック
            if (CommonUtils.isOutOfRange(pn, CommonConst.PN_MIN, CommonConst.PN_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : pn is invalid : " + pn);
                result = EncDecConst.NG;
            }

            // 送信周波数の値チェック
            if (CommonUtils.isOutOfRange(freq, CommonConst.FREQ_MIN, CommonConst.FREQ_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : freq is invalid : " + freq);
                result = EncDecConst.NG;
            }

            return result;
        }


    }
}
