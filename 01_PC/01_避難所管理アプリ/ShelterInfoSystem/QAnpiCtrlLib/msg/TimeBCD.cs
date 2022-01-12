using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QAnpiCtrlLib.consts;
using QAnpiCtrlLib.utils;
using QAnpiCtrlLib.log;

namespace QAnpiCtrlLib.msg
{
    /// <summary>
    /// 年月日時分秒を保持する　BCD形式との変換も行う
    /// </summary>
    public class TimeBCD
    {
        private String TAG = "TimeBCD";

        /// <summary>
        /// 年
        /// </summary>
        public uint year;
        /// <summary>
        /// 年の最小値
        /// </summary>
        public const int YEAR_MIN = 0;
        /// <summary>
        /// 年の最大値
        /// </summary>
        public const int YEAR_MAX = 9999;
        /// <summary>
        /// 年のサイズ(bit)
        /// </summary>
        public const int YEAR_SIZE = 16;
        /// <summary>
        /// 月
        /// </summary>
        public uint month;
        /// <summary>
        /// 月の最小値
        /// </summary>
        public const int MONTH_MIN = 1;
        /// <summary>
        /// 月の最大値
        /// </summary>
        public const int MONTH_MAX = 12;
        /// <summary>
        /// 月のサイズ(bit)
        /// </summary>
        public const int MONTH_SIZE = 8;
        /// <summary>
        /// 日
        /// </summary>
        public uint day;
        /// <summary>
        /// 日の最小値
        /// </summary>
        public const int DAY_MIN = 1;
        /// <summary>
        /// 日の最大値
        /// </summary>
        public const int DAY_MAX = 31;
        /// <summary>
        /// 日のサイズ(bit)
        /// </summary>
        public const int DAY_SIZE = 8;
        /// <summary>
        /// 時
        /// </summary>
        public uint hour;
        /// <summary>
        /// 時の最小値
        /// </summary>
        public const int HOUR_MIN = 0;
        /// <summary>
        /// 時の最大値
        /// </summary>
        public const int HOUR_MAX = 23;
        /// <summary>
        /// 時のサイズ(bit)
        /// </summary>
        public const int HOUR_SIZE = 8;
        /// <summary>
        /// 分
        /// </summary>
        public uint minute;
        /// <summary>
        /// 分の最小値
        /// </summary>
        public const int MINUTE_MIN = 0;
        /// <summary>
        /// 分の最大値
        /// </summary>
        public const int MINUTE_MAX = 59;
        /// <summary>
        /// 分のサイズ(bit)
        /// </summary>
        public const int MINUTE_SIZE = 8;
        /// <summary>
        /// 秒
        /// </summary>
        public uint second;
        /// <summary>
        /// 秒の最小値
        /// </summary>
        public const int SECOND_MIN = 0;
        /// <summary>
        /// 秒の最大値
        /// </summary>
        public const int SECOND_MAX = 60;
        /// <summary>
        /// 秒のサイズ(bit)
        /// </summary>
        public const int SECOND_SIZE = 8;

        /// <summary>
        /// BCD形式時のデータ長(バイト)
        /// </summary>
        public const int BCD_DATA_LENGTH = 7;

        /// <summary>
        /// 10の位に変換するための係数
        /// </summary>
        private const int COEF_FOR_10  = 10;
        /// <summary>
        /// 100の位に変換するための係数
        /// </summary>
        private const int COEF_FOR_100 = 100;

        /// <summary>
        /// 年の千と百の位が格納されている場所
        /// </summary>
        private const int POS_YEAR_UPPER = 0;
        /// <summary>
        /// 年の十と一の位が格納されている場所
        /// </summary>
        private const int POS_YEAR_LOWER = 1;
        /// <summary>
        /// 月が格納されている場所
        /// </summary>
        private const int POS_MONTH      = 2;
        /// <summary>
        /// 日が格納されている場所
        /// </summary>
        private const int POS_DAY        = 3;
        /// <summary>
        /// 時が格納されている場所
        /// </summary>
        private const int POS_HOUR       = 4;
        /// <summary>
        /// 分が格納されている場所
        /// </summary>
        private const int POS_MINUTE     = 5;
        /// <summary>
        /// 秒が格納されている場所
        /// </summary>
        private const int POS_SECOND     = 6;

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

            //年
            if (CommonUtils.isOutOfRange(year, YEAR_MIN, YEAR_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : year is invalid : year = "
                        + year);
                result = EncDecConst.NG;
            }

            //月
            if (CommonUtils.isOutOfRange(month, MONTH_MIN, MONTH_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : month is invalid : month = "
                        + month);
                result = EncDecConst.NG;
            }

            //日
            if (CommonUtils.isOutOfRange(day, DAY_MIN, DAY_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : day is invalid : day = "
                        + day);
                result = EncDecConst.NG;
            }

            //時
            if (CommonUtils.isOutOfRange(hour, HOUR_MIN, HOUR_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : hour is invalid : hour = "
                        + hour);
                result = EncDecConst.NG;
            }

            //分
            if (CommonUtils.isOutOfRange(minute, MINUTE_MIN, MINUTE_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : minute is invalid : minute = "
                        + minute);
                result = EncDecConst.NG;
            }

            //秒
            if (CommonUtils.isOutOfRange(second, SECOND_MIN, SECOND_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : second is invalid : second = "
                        + second);
                result = EncDecConst.NG;
            }

            return result;
        }

        /// <summary>
        /// 設定されているデータをBCD形式のbyte配列で返す
        /// </summary>
        /// <returns>BCD形式のbyte配列</returns>
        public byte[] getBCDData()
        {
            List<byte> bcdData = new List<byte>();

            String bcdString = (year % 10000).ToString("D4") + (month % 100).ToString("D2") + (day % 100).ToString("D2") + (hour % 100).ToString("D2") + (minute % 100).ToString("D2") + (second % 100).ToString("D2");

            return utils.CommonUtils.ToByteArray(bcdString);
        }

        /// <summary>
        /// BCD形式のbyte配列をメンバ変数に設定する
        /// </summary>
        /// <param name="bcdData">BCD形式のbyte配列</param>
        public void setBCDData(byte[] bcdData)
        {
            if(bcdData.Length < BCD_DATA_LENGTH)
            {
                return;
            }

            //年
            year = (uint)convBCDToIntOneByte(bcdData[POS_YEAR_UPPER]) * COEF_FOR_100;
            year += (uint)convBCDToIntOneByte(bcdData[POS_YEAR_LOWER]);
            //月
            month = (uint)convBCDToIntOneByte(bcdData[POS_MONTH]);
            //日
            day = (uint)convBCDToIntOneByte(bcdData[POS_DAY]);
            //時
            hour = (uint)convBCDToIntOneByte(bcdData[POS_HOUR]);
            //分
            minute = (uint)convBCDToIntOneByte(bcdData[POS_MINUTE]);
            //秒
            second = (uint)convBCDToIntOneByte(bcdData[POS_SECOND]);
        }

        /// <summary>
        /// BCD形式の1byteのデータをint形式の値に変換する
        /// </summary>
        /// <param name="oneByte">BCD形式の1byteのデータ</param>
        /// <returns>int形式の値</returns>
        protected int convBCDToIntOneByte(byte oneByte)
        {
            byte highMask = 0xF0;
            byte lowMask = 0x0F;

            int up = ((oneByte & highMask) >> 4) * COEF_FOR_10;
            int low = ((oneByte & lowMask));

            return up + low;
        }

    }
}
