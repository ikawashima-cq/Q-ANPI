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
    /// 設備ステータス応答
    /// データ部 設備ステータス情報 管理
    /// </summary>
    public class EquStatInfo : EncodeManager
    {
        // データ構造
        // --------------------------------------
        // Offset  Size    項目
        // --------------------------------------
        //     0     32    運用状態
        //    32     32    Ｓ帯送受信状態
        //    64     32    GPS受信状態
        //    96     32    有線LAN状態
        //   128     32    FAN状態
        //   160     32    電源電圧状態
        //   192     32    温度状態
        //   224     32    ソフトウェアアラーム
        // --------------------------------------
        //   合計   256 bit (32 byte)
        
        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "EquStatInfo";

        /// <summary>
        /// 運用状態
        /// </summary>
        [System.Xml.Serialization.XmlElement("operationStatus")]
        public int opeStat;

        /// <summary>
        /// Ｓ帯送受信状態
        /// </summary>
        [System.Xml.Serialization.XmlElement("sbandSendReciveStatus")]
        public int sndrcvStat;

        /// <summary>
        /// GPS受信状態
        /// </summary>
        [System.Xml.Serialization.XmlElement("gpsReciveStatus")]
        public int gpsStat;

        /// <summary>
        /// 有線LAN状態
        /// </summary>
        [System.Xml.Serialization.XmlElement("wiredLanStatus")]
        public int lanStat;

        /// <summary>
        /// FAN状態
        /// </summary>
        [System.Xml.Serialization.XmlElement("fanStatus")]
        public int fanStat;

        /// <summary>
        /// 電源電圧状態
        /// </summary>
        [System.Xml.Serialization.XmlElement("voltageStatus")]
        public int voltStat;

        /// <summary>
        /// 温度状態
        /// </summary>
        [System.Xml.Serialization.XmlElement("temperatureStatus")]
        public int tmpStat;

        /// <summary>
        /// ソフトウェアアラーム
        /// </summary>
        [System.Xml.Serialization.XmlElement("softAlarm")]
        public int swAlarm;

        /// <summary>
        /// 各情報のサイズ(bit)
        /// </summary>
        public const int SIZE_AREA = 4 * BYTE_BIT_SIZE;

        /// <summary>
        /// 運用状態の最小値
        /// </summary>
        public const int OPE_STAT_MIN = 0;

        /// <summary>
        /// 運用状態の最大値
        /// </summary>
        public const int OPE_STAT_MAX = 6;

        /// <summary>
        /// Ｓ帯送受信状態の最小値
        /// </summary>
        public const int SND_RCV_STAT_MIN = 0;

        /// <summary>
        /// Ｓ帯送受信状態の最大値（bit0-2:使用、bit3-31:未使用/0保証、LSBの場合）
        /// </summary>
        public const int SND_RCV_STAT_MAX = 7;

        /// <summary>
        /// GPS受信状態の最小値
        /// </summary>
        public const int GPS_STAT_MIN = 0;

        /// <summary>
        /// GPS受信状態の最大値
        /// </summary>
        public const int GPS_STAT_MAX = 4;

        /// <summary>
        /// 有線LAN状態の最小値
        /// </summary>        
        public const int LAN_STAT_MIN = 0;

        /// <summary>
        /// 有線LAN状態の最大値
        /// </summary>
        public const int LAN_STAT_MAX = 2;

        /// <summary>
        /// FAN状態の最小値
        /// </summary>
        public const int FAN_STAT_MIN = 0;

        /// <summary>
        /// FAN状態の最大値
        /// </summary>        
        public const int FAN_STAT_MAX = 1;

        /// <summary>
        /// 電源電圧状態の最小値
        /// </summary>
        public const int VLT_STAT_MIN = 0;

        /// <summary>
        /// 電源電圧状態の最大値
        /// </summary>
        public const int VLT_STAT_MAX = 1;

        /// <summary>
        /// 温度状態の最小値
        /// </summary>
        public const int TMP_STAT_MIN = 0;

        /// <summary>
        /// 温度状態の最大値
        /// </summary>
        public const int TMP_STAT_MAX = 1;

        /// <summary>
        /// ソフトウェアアラームの最小値
        /// </summary>
        public const int SW_ALARM_MIN = 0;

        /// <summary>
        /// ソフトウェアアラームの最大値（bit0-4:使用、bit5-31:未使用/0保証、LSBの場合）
        /// </summary>
        public const int SW_ALARM_MAX = 0x1F;

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
            int result;
            int enc_data;
            int enc_size;

            try
            {
                // 設備ステータス情報：運用状態
                enc_data = this.opeStat;
                enc_size = SIZE_AREA;
                encode(enc_data, enc_size, encodedData, startPos);
                startPos += enc_size;

                // 設備ステータス情報：Ｓ帯送受信状態
                enc_data = this.sndrcvStat;
                encode(enc_data, enc_size, encodedData, startPos);
                startPos += enc_size;

                // 設備ステータス情報：GPS受信状態
                enc_data = this.gpsStat;
                encode(enc_data, enc_size, encodedData, startPos);
                startPos += enc_size;

                // 設備ステータス情報：有線LAN状態
                enc_data = this.lanStat;
                encode(enc_data, enc_size, encodedData, startPos);
                startPos += enc_size;

                // 設備ステータス情報：FAN状態
                enc_data = this.fanStat;
                encode(enc_data, enc_size, encodedData, startPos);
                startPos += enc_size;

                // 設備ステータス情報：電源電圧状態
                enc_data = this.voltStat;
                encode(enc_data, enc_size, encodedData, startPos);
                startPos += enc_size;

                // 設備ステータス情報：温度状態
                enc_data = this.tmpStat;
                encode(enc_data, enc_size, encodedData, startPos);
                startPos += enc_size;

                // 設備ステータス情報：ソフトウェアアラーム
                enc_data = this.swAlarm;
                encode(enc_data, enc_size, encodedData, startPos);
                startPos += enc_size;

                result = startPos;
            }
            catch (Exception ex)
            {
                result = 0;
                LogMng.AplLogError("EquStatInfo encode() error:" + ex.Message);
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
            int result;
            int dec_size = 0;

            try
            {
                // 設備ステータス情報：運用状態
                dec_size = SIZE_AREA;
                this.opeStat = decodeInt(encodedData, dec_size, startPos);
                startPos += dec_size;

                // 設備ステータス情報：Ｓ帯送受信状態
                this.sndrcvStat = decodeInt(encodedData, dec_size, startPos);
                startPos += dec_size;

                // 設備ステータス情報：GPS受信状態
                this.gpsStat = decodeInt(encodedData, dec_size, startPos);
                startPos += dec_size;

                // 設備ステータス情報：有線LAN状態
                this.lanStat = decodeInt(encodedData, dec_size, startPos);
                startPos += dec_size;

                // 設備ステータス情報：FAN状態
                this.fanStat = decodeInt(encodedData, dec_size, startPos);
                startPos += dec_size;

                // 設備ステータス情報：電源電圧状態
                this.voltStat = decodeInt(encodedData, dec_size, startPos);
                startPos += dec_size;

                // 設備ステータス情報：温度状態
                this.tmpStat = decodeInt(encodedData, dec_size, startPos);
                startPos += dec_size;

                // 設備ステータス情報：ソフトウェアアラーム
                this.swAlarm = decodeInt(encodedData, dec_size, startPos);
                startPos += dec_size;

                result = startPos;
            }
            catch (Exception ex)
            {
                result = 0;
                LogMng.AplLogError("EquStatInfo decode() error:" + ex.Message);
            }

            return result;
        }


        /// <summary>
        /// 有効値パラメータチェック
        /// </summary>
        /// <returns>
        /// 有効値パラメータチェック結果
        /// </returns>
        public int checkParam()
        {
            int result = EncDecConst.OK;

            if (CommonUtils.isOutOfRange(opeStat, OPE_STAT_MIN, OPE_STAT_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : opeStat is invalid : " + opeStat);
                result = EncDecConst.NG;
            }

            // LSB数値変換
            int sndrcvStat_rev = utils.CommonUtils.bitReverse(sndrcvStat);
            if (CommonUtils.isOutOfRange(sndrcvStat_rev, SND_RCV_STAT_MIN, SND_RCV_STAT_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : sndrcvStat_rev is invalid : " + sndrcvStat_rev);
                result = EncDecConst.NG;
            }

            if (CommonUtils.isOutOfRange(gpsStat, GPS_STAT_MIN, GPS_STAT_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : gpsStat is invalid : " + gpsStat);
                result = EncDecConst.NG;
            }

            if (CommonUtils.isOutOfRange(lanStat, LAN_STAT_MIN, LAN_STAT_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : lanStat is invalid : " + lanStat);
                result = EncDecConst.NG;
            }

            if (CommonUtils.isOutOfRange(fanStat, FAN_STAT_MIN, FAN_STAT_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : fanStat is invalid : " + fanStat);
                result = EncDecConst.NG;
            }

            if (CommonUtils.isOutOfRange(voltStat, VLT_STAT_MIN, VLT_STAT_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : voltStat is invalid : " + voltStat);
                result = EncDecConst.NG;
            }

            if (CommonUtils.isOutOfRange(tmpStat, TMP_STAT_MIN, TMP_STAT_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : tmpStat is invalid : " + tmpStat);
                result = EncDecConst.NG;
            }

            // LSB数値変換
            int swAlarm_rev = utils.CommonUtils.bitReverse(swAlarm);
            if (CommonUtils.isOutOfRange(swAlarm_rev, SW_ALARM_MIN, SW_ALARM_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : swAlarm_rev is invalid : " + swAlarm_rev);
                result = EncDecConst.NG;
            }

            return result;
        }

    }

}
