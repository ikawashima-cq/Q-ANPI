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
    /// Ｓ帯FWDデータ取得応答
    /// データ部 Ｓ帯FWD受信情報 管理
    /// </summary>
    public class SBandFwdRcvInfo : EncodeManager
    {
        // データ構造
        // --------------------------------------
        // Offset  Size    項目
        // --------------------------------------
        //     0     64    受信時刻
        //    64     32    RSSI
        //    96     32    RSCP
        //   128     32    周波数オフセット
        // ( 160     16    空き            )
        //   176      8    RS結果
        //   184      8    CRC結果
        //   192   3440    Ｓ帯安否確認信号(FWD)
        // (3632     16 )  0補填           )
        // --------------------------------------
        //   合計  3648 bit (456 byte)

        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "SBandFwdRcvInfo";

        /// <summary>
        /// 受信時刻
        /// </summary>
        [System.Xml.Serialization.XmlElement("reviceTime")]
        public TimeUsec rcvTime;

        /// <summary>
        /// RSSI
        /// </summary>
        public float rssi;

        /// <summary>
        /// RSCP
        /// </summary>
        public float rscp;

        /// <summary>
        /// 周波数オフセット
        /// </summary>
        [System.Xml.Serialization.XmlElement("freqOffset")]
        public float frqOffset;

        /// <summary>
        /// RS結果
        /// </summary>
        public int rsResult;

        /// <summary>
        /// CRC結果
        /// </summary>
        public int crcResult;

        /// <summary>
        /// RS結果の最小値
        /// </summary>
        public const int RS_RESULT_MIN = 0;

        /// <summary>
        /// RS結果の最大値
        /// </summary>
        public const int RS_RESULT_MAX = 1;

        /// <summary>
        /// CRC結果の最小値
        /// </summary>
        public const int CRC_RESULT_MIN = 0;

        /// <summary>
        /// CRC結果の最大値
        /// </summary>
        public const int CRC_RESULT_MAX = 1;

        /// <summary>
        /// Ｓ帯安否確認信号(FWD)＝端末宛メッセージ相当
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public byte[] fwdData;


        /// <summary>
        /// Ｓ帯安否確認信号(FWD)の端末宛メッセージType
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public int fwdDataMsgType;


        /// <summary>
        /// Ｓ帯安否確認信号(FWD)＝端末宛メッセージ相当
        /// XML出力用
        /// </summary>
        [System.Xml.Serialization.XmlElement("msg")]
        public string fwdDataStr = "";
        
        /// <summary>
        /// RSSIのサイズ(bit)
        /// </summary>
        public const int SIZE_RSSI = 4 * BYTE_BIT_SIZE;

        /// <summary>
        /// RSCPのサイズ(bit)
        /// </summary>
        public const int SIZE_RSCP = 4 * BYTE_BIT_SIZE;

        /// <summary>
        /// 周波数オフセットのサイズ(bit)
        /// </summary>
        public const int SIZE_FREQ_OFFSET = 4 * BYTE_BIT_SIZE;

        /// <summary>
        /// 空きのサイズ(bit)
        /// </summary>
        public const int SIZE_BLANK = 2 * BYTE_BIT_SIZE;

        /// <summary>
        /// RS結果のサイズ(bit)
        /// </summary>
        public const int SIZE_RS_RESULT = 1 * BYTE_BIT_SIZE;

        /// <summary>
        /// CRC結果のサイズ(bit)
        /// </summary>
        public const int SIZE_CRC_RESULT = 1 * BYTE_BIT_SIZE;

        /// <summary>
        /// Ｓ帯安否確認信号(FWD)のサイズ(bit)
        /// </summary>
        public const int SIZE_FWD_DATA = 430 * BYTE_BIT_SIZE;

        /// <summary>
        /// ０補填のサイズ(bit)
        /// </summary>
        public const int SIZE_ZERO_FILL = 2 * BYTE_BIT_SIZE;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SBandFwdRcvInfo()
        {
            rcvTime = new TimeUsec();

            int len = sizeToLength(SIZE_FWD_DATA);
            fwdData = new byte[len];
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
            float enc_data_f;
            int enc_size;

            try
            {
                // Ｓ帯FWD受信情報：受信時刻
                startPos = this.rcvTime.encode(encodedData, startPos);
                if (startPos == 0)
                {
                    return result;
                }

                // Ｓ帯FWD受信情報：RSSI
                enc_data_f = this.rssi;
                enc_size = SIZE_RSSI;
                byte[] rssiData = BitConverter.GetBytes(enc_data_f);
                Array.Reverse(rssiData); // ビッグエンディアン
                encode(rssiData, enc_size, encodedData, startPos);
                startPos += enc_size;

                // Ｓ帯FWD受信情報：RSCP
                enc_data_f = this.rscp;
                enc_size = SIZE_RSCP;
                byte[] rscpData = BitConverter.GetBytes(enc_data_f);
                Array.Reverse(rscpData); // ビッグエンディアン
                encode(rscpData, enc_size, encodedData, startPos);
                startPos += enc_size;

                // Ｓ帯FWD受信情報：周波数オフセット
                enc_data_f = this.frqOffset;
                enc_size = SIZE_FREQ_OFFSET;
                byte[] foData = BitConverter.GetBytes(enc_data_f);
                Array.Reverse(foData); // ビッグエンディアン
                encode(foData, enc_size, encodedData, startPos);
                startPos += enc_size;

                // Ｓ帯FWD受信情報：空き
                startPos += SIZE_BLANK;

                // Ｓ帯FWD受信情報：RS結果			
                enc_data = this.rsResult;
                enc_size = SIZE_RS_RESULT;
                encode(enc_data, enc_size, encodedData, startPos);
                startPos += enc_size;

                // Ｓ帯FWD受信情報：CRC結果			
                enc_data = this.crcResult;
                enc_size = SIZE_CRC_RESULT;
                encode(enc_data, enc_size, encodedData, startPos);
                startPos += enc_size;

                // Ｓ帯FWD受信情報：Ｓ帯安否確認信号(FWD)
                enc_size = SIZE_FWD_DATA;
                encode(this.fwdData, enc_size, encodedData, startPos);
                startPos += enc_size;

                // Ｓ帯FWD受信情報：０補填
                startPos += SIZE_ZERO_FILL;
                
                result = startPos;
            }
            catch (Exception ex)
            {
                result = 0;
                LogMng.AplLogError("SBandFwdRcvInfo encode() error:" + ex.Message);
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
                startPos = this.rcvTime.decode(encodedData, startPos);
                if (startPos == 0)
                {
                    return result;
                }

                // Ｓ帯FWD受信情報：RSSI
                dec_size = SIZE_RSSI;
                byte[] rssiData = new byte[4];
                decodeByteArray(encodedData, dec_size, startPos, rssiData);
                Array.Reverse(rssiData); // リトルエンディアン
                this.rssi = BitConverter.ToSingle(rssiData, 0);
                startPos += dec_size;

                // Ｓ帯FWD受信情報：RSCP
                dec_size = SIZE_RSCP;
                byte[] rscpData = new byte[4];
                decodeByteArray(encodedData, dec_size, startPos, rscpData);
                Array.Reverse(rscpData); // リトルエンディアン
                this.rscp = BitConverter.ToSingle(rscpData, 0);
                startPos += dec_size;

                // Ｓ帯FWD受信情報：周波数オフセット
                dec_size = SIZE_FREQ_OFFSET;
                byte[] foData = new byte[4];
                decodeByteArray(encodedData, dec_size, startPos, foData);
                Array.Reverse(foData); // リトルエンディアン
                this.frqOffset = BitConverter.ToSingle(foData, 0);
                startPos += dec_size;

                // Ｓ帯FWD受信情報：空き
                startPos += SIZE_BLANK;

                // Ｓ帯FWD受信情報：RS結果
                dec_size = SIZE_RS_RESULT;
                this.rsResult = decodeInt(encodedData, dec_size, startPos);
                startPos += dec_size;

                // Ｓ帯FWD受信情報：CRC結果
                dec_size = SIZE_CRC_RESULT;
                this.crcResult = decodeInt(encodedData, dec_size, startPos);
                startPos += dec_size;

                // Ｓ帯FWD受信情報：Ｓ帯安否確認信号(FWD)
                dec_size = SIZE_FWD_DATA;
                decodeByteArray(encodedData, dec_size, startPos, this.fwdData);
                startPos += dec_size;

                // Ｓ帯FWD受信情報：０補填
                startPos += SIZE_ZERO_FILL;

                result = startPos;

                // Ｓ帯安否確認信号(FWD)の端末宛メッセージのType取得
                msg.DecodeManager dm = new msg.DecodeManager();
                msg.TypeAndSystemInfo tasi = dm.decodeTypeAndSystemInfo(
                    this.fwdData, MsgToTerminal.SIZE, false);
                fwdDataMsgType = tasi.msgType;
            }
            catch (Exception ex)
            {
                result = 0;
                LogMng.AplLogError("SBandFwdRcvInfo decode() error:" + ex.Message);
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

            // 受信時刻の値チェック
            int chk_time = rcvTime.checkParam();
            if (chk_time != EncDecConst.OK)
            {
                LogMng.AplLogError(TAG + "checkParam : rcvTime is invalid");
                result = EncDecConst.NG;
            }

            // RS結果の値チェック
            if (CommonUtils.isOutOfRange(rsResult, RS_RESULT_MIN, RS_RESULT_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : rsResult is invalid : " + rsResult);
                result = EncDecConst.NG;
            }

            // CRC結果の値チェック
            if (CommonUtils.isOutOfRange(crcResult, CRC_RESULT_MIN, CRC_RESULT_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : crcResult is invalid : " + crcResult);
                result = EncDecConst.NG;
            }

            return result;
        }


    }
}
