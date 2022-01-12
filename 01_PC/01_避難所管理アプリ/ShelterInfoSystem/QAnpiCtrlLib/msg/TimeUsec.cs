/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
 */
using System;
using System.Collections.Generic;
using QAnpiCtrlLib.consts;
using QAnpiCtrlLib.utils;
using QAnpiCtrlLib.log;

namespace QAnpiCtrlLib.msg
{
    /// <summary>
    /// TIME_US型時刻 管理
    /// </summary>
    public class TimeUsec : EncodeManager
    {
        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "TimeUsec";

        /// <summary>
        /// 通算日
        /// </summary>
        [System.Xml.Serialization.XmlElement("day")]
        public int dayOfYear;

        /// <summary>
        /// 時
        /// </summary>
        public int hour;

        /// <summary>
        /// 分
        /// </summary>
        [System.Xml.Serialization.XmlElement("minute")]
        public int min;

        /// <summary>
        /// 秒
        /// </summary>
        [System.Xml.Serialization.XmlElement("second")]
        public int sec;

        /// <summary>
        /// ミリ秒
        /// </summary>
        [System.Xml.Serialization.XmlElement("ms")]
        public int msec;

        /// <summary>
        /// マイクロ秒
        /// </summary>
        [System.Xml.Serialization.XmlElement("us")]
        public int usec;

        /// <summary>
        /// 通算日の最小値（1月1日は1、2月1日は32）
        /// </summary>
        public const int DAY_MIN = 1;

        /// <summary>
        /// 通算日の最大値（うるう年の12月31日は366）
        /// </summary>
        public const int DAY_MAX = 366;

        /// <summary>
        /// 時の最小値
        /// </summary>
        public const int HOUR_MIN = 0;

        /// <summary>
        /// 時の最大値
        /// </summary>
        public const int HOUR_MAX = 23;

        /// <summary>
        /// 分の最小値
        /// </summary>
        public const int MIN_MIN = 0;

        /// <summary>
        /// 分の最大値
        /// </summary>
        public const int MIN_MAX = 59;

        /// <summary>
        /// 秒の最小値
        /// </summary>
        public const int SEC_MIN = 0;

        /// <summary>
        /// 秒の最大値（うるう秒は60）
        /// </summary>
        public const int SEC_MAX = 60;

        /// <summary>
        /// ミリ秒の最小値
        /// </summary>
        public const int MSEC_MIN = 0;

        /// <summary>
        /// ミリ秒の最大値
        /// </summary>
        public const int MSEC_MAX = 999;

        /// <summary>
        /// マイクロ秒の最小値
        /// </summary>
        public const int USEC_MIN = 0;

        /// <summary>
        /// マイクロ秒の最大値（最下位桁は0固定）
        /// </summary>
        public const int USEC_MAX = 990;


        /// <summary>
        /// 通算日のサイズ(bit)
        /// </summary>
        public const int SIZE_DAYS = 2 * BYTE_BIT_SIZE;

        /// <summary>
        /// 時・分・秒の各サイズ(bit)
        /// </summary>
        public const int SIZE_HHMMSS = 1 * BYTE_BIT_SIZE;

        /// <summary>
        /// ミリ秒＋マイクロ秒のサイズ(bit)
        /// </summary>
        public const int SIZE_MSUS = 3 * BYTE_BIT_SIZE;

        /// <summary>
        /// 通算日の桁数
        /// </summary>
        public const int DIGIT_DAYS = 4;

        /// <summary>
        /// 時・分・秒の桁数
        /// </summary>
        public const int DIGIT_HHMMSS = 2;

        /// <summary>
        /// ミリ秒・マイクロ秒の桁数
        /// </summary>
        public const int DIGIT_MSUS = 3;


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
                // データ時刻：通算日
                if (dayOfYear.ToString("D").Length > DIGIT_DAYS)
                {
                    LogMng.AplLogError("TimeUsec encode() : dayOfYear is over digit : " + dayOfYear);
                    return 0;
                }

                enc_data = dayOfYear;
                enc_size = SIZE_DAYS;
                byte[] bcdData_day = intToBcd(enc_data, DIGIT_DAYS);
                if (bcdData_day == null)
                {
                    LogMng.AplLogError("TimeUsec encode() intToBcd : bcdData_day is null : " + dayOfYear);
                    return 0;
                }

                encode(bcdData_day, enc_size, encodedData, startPos);
                startPos += enc_size;

                // データ時刻：時
                if (hour.ToString("D").Length > DIGIT_HHMMSS)
                {
                    LogMng.AplLogError("TimeUsec encode() : hour is over digit : " + hour);
                    return 0;
                }

                enc_data = hour;
                enc_size = SIZE_HHMMSS;
                byte[] bcdData_hour = intToBcd(enc_data, DIGIT_HHMMSS);
                if (bcdData_hour == null)
                {
                    LogMng.AplLogError("TimeUsec encode() intToBcd : bcdData_hour is null : " + hour);
                    return 0;
                }

                encode(bcdData_hour, enc_size, encodedData, startPos);
                startPos += enc_size;

                // データ時刻：分
                if (min.ToString("D").Length > DIGIT_HHMMSS)
                {
                    LogMng.AplLogError("TimeUsec encode() : min is over digit : " + min);
                    return 0;
                }

                enc_data = min;
                enc_size = SIZE_HHMMSS;
                byte[] bcdData_min = intToBcd(enc_data, DIGIT_HHMMSS);
                if (bcdData_min == null)
                {
                    LogMng.AplLogError("TimeUsec encode() intToBcd : bcdData_min is null : " + min);
                    return 0;
                }

                encode(bcdData_min, enc_size, encodedData, startPos);
                startPos += enc_size;

                // データ時刻：秒
                if (sec.ToString("D").Length > DIGIT_HHMMSS)
                {
                    LogMng.AplLogError("TimeUsec encode() : sec is over digit : " + sec);
                    return 0;
                }

                enc_data = sec;
                enc_size = SIZE_HHMMSS;
                byte[] bcdData_sec = intToBcd(enc_data, DIGIT_HHMMSS);
                if (bcdData_sec == null)
                {
                    LogMng.AplLogError("TimeUsec encode() intToBcd : bcdData_sec is null : " + sec);
                    return 0;
                }

                encode(bcdData_sec, enc_size, encodedData, startPos);
                startPos += enc_size;

                // データ時刻：ミリ秒＋マイクロ秒
                if (msec.ToString("D").Length > DIGIT_MSUS)
                {
                    LogMng.AplLogError("TimeUsec encode() : msec is over digit : " + msec);
                    return 0;
                }
                if (usec.ToString("D").Length > DIGIT_MSUS)
                {
                    LogMng.AplLogError("TimeUsec encode() : usec is over digit : " + usec);
                    return 0;
                }

                string msec_usec = msec.ToString("D3") + usec.ToString("D3");
                enc_size = SIZE_MSUS;
                byte[] bcdData_msec_usec = intToBcd(Convert.ToInt32(msec_usec), DIGIT_MSUS * 2);
                if (bcdData_msec_usec == null)
                {
                    LogMng.AplLogError("TimeUsec encode() intToBcd : bcdData_msec_usec is null : " + msec_usec);
                    return 0;
                }

                encode(bcdData_msec_usec, enc_size, encodedData, startPos);
                startPos += enc_size;

                result = startPos;
            }
            catch (Exception ex)
            {
                result = 0;
                LogMng.AplLogError("TimeUsec encode() error:" + ex.Message);
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
                // データ時刻：通算日
                dec_size = SIZE_DAYS;
                byte[] bcd_day = new byte[sizeToLength(dec_size)];
                decodeByteArray(encodedData, dec_size, startPos, bcd_day);
                dayOfYear = bcdToInt(bcd_day[0]) * 100;
                dayOfYear += bcdToInt(bcd_day[1]);
                
                startPos += dec_size;

                // データ時刻：時
                dec_size = SIZE_HHMMSS;
                byte[] bcd_hour = new byte[sizeToLength(dec_size)];
                decodeByteArray(encodedData, dec_size, startPos, bcd_hour);
                hour = bcdToInt(bcd_hour[0]);

                startPos += dec_size;

                // データ時刻：分
                dec_size = SIZE_HHMMSS;
                byte[] bcd_min = new byte[sizeToLength(dec_size)];
                decodeByteArray(encodedData, dec_size, startPos, bcd_min);
                min = bcdToInt(bcd_min[0]);

                startPos += dec_size;

                // データ時刻：秒
                dec_size = SIZE_HHMMSS;
                byte[] bcd_sec = new byte[sizeToLength(dec_size)];
                decodeByteArray(encodedData, dec_size, startPos, bcd_sec);
                sec = bcdToInt(bcd_sec[0]);

                startPos += dec_size;

                // データ時刻：ミリ秒＋マイクロ秒
                dec_size = SIZE_MSUS;
                byte[] bcd_musec = new byte[sizeToLength(dec_size)];
                decodeByteArray(encodedData, dec_size, startPos, bcd_musec);

                msec = bcdToInt(bcd_musec[0]) * 10;
                msec += bcdToInt(bcd_musec[1]) / 10;

                usec = (bcdToInt(bcd_musec[1]) % 10) * 100;
                usec += bcdToInt(bcd_musec[2]);

                startPos += dec_size;

                result = startPos;
            }
            catch (Exception ex)
            {
                result = 0;
                LogMng.AplLogError("TimeUsec decode() error:" + ex.Message);
            }

            return result;
        }


        /// <summary>
        /// DateTimeクラスに変換して取得する
        /// </summary>
        /// <returns>DateTimeクラス</returns>
        public DateTime getDateTime()
        {
            DateTime dt = new DateTime();

            // データ時刻の通算日が現時刻通算日よりも未来の場合は去年を設定
            int year;
            if (DateTime.UtcNow.DayOfYear < dayOfYear)
            {
                year = DateTime.UtcNow.Year - 1;
            }
            else
            {
                year = DateTime.UtcNow.Year;
            }

            // 判定した年の初日のDateTimeを作成
            dt = DateTime.Parse(year.ToString() + "/01/01 00:00:00");
            
            // 通算日は1オリジンなので(通算日 - 1)を加算して置き換え
            dt = dt.AddDays(dayOfYear - 1);

            dt = dt.AddHours(hour);
            dt = dt.AddMinutes(min);
            dt = dt.AddSeconds(sec);
            dt = dt.AddMilliseconds(msec);

            return dt;
        }

        /// <summary>
        /// DataTimeクラスの時刻を設定する 
        /// ※通算日にはsetTimeの年に対応する通算日が設定される
        /// </summary>
        /// <param name="setTime">設定時刻</param>
        /// <param name="setUsec">マイクロ秒(DataTimeクラスにないので個別指定)</param>
        public void setDateTime(DateTime setTime, int setUsec)
        {
            DateTime utcTime = setTime.ToUniversalTime();
            dayOfYear = utcTime.DayOfYear;
            hour = utcTime.Hour;
            min = utcTime.Minute;
            sec = utcTime.Second;
            msec = utcTime.Millisecond;
            usec = setUsec;
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

            if (CommonUtils.isOutOfRange(dayOfYear, DAY_MIN, DAY_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : dayOfYear is invalid : " + dayOfYear);
                result = EncDecConst.NG;
            }

            if (CommonUtils.isOutOfRange(hour, HOUR_MIN, HOUR_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : hour is invalid : " + hour);
                result = EncDecConst.NG;
            }

            if (CommonUtils.isOutOfRange(min, MIN_MIN, MIN_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : min is invalid : " + min);
                result = EncDecConst.NG;
            }

            if (CommonUtils.isOutOfRange(sec, SEC_MIN, SEC_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : sec is invalid : " + sec);
                result = EncDecConst.NG;
            }

            if (CommonUtils.isOutOfRange(msec, MSEC_MIN, MSEC_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : msec is invalid : " + msec);
                result = EncDecConst.NG;
            }

            if (CommonUtils.isOutOfRange(usec, USEC_MIN, USEC_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : usec is invalid : " + usec);
                result = EncDecConst.NG;
            }

            return result;
        }

        /// <summary>
        /// 桁数指定の数値をBCD形式のbyte配列で返す
        /// </summary>
        /// <param name="value">数値</param>
        /// <param name="digit">桁数（偶数を指定する）</param>
        /// <returns></returns>
        public byte[] intToBcd(int value, int digit)
        {
            byte[] result;
            List<byte> bcdData = new List<byte>();

            string fmt = "D" + digit;
            string bcdString = value.ToString(fmt);

            try
            {
                result = utils.CommonUtils.ToByteArray(bcdString);
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// BCD形式の1byteのデータをint形式の値に変換する
        /// </summary>
        /// <param name="oneByte">BCD形式の1byteのデータ</param>
        /// <returns>int形式の値</returns>
        protected int bcdToInt(byte oneByte)
        {
            byte highMask = 0xF0;
            byte lowMask = 0x0F;

            int up = ((oneByte & highMask) >> 4) * 10;
            int low = ((oneByte & lowMask));

            return up + low;
        }

    }


}
