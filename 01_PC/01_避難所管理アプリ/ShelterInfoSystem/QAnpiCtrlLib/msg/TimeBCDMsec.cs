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
    /// ミリ秒ありのBCD表記時刻
    /// </summary>
    public class TimeBCDMsec:TimeBCD
    {
        private String TAG = "TimeBCDMsec";

        /// <summary>
        /// ミリ秒
        /// </summary>
        public uint ms;
        /// <summary>
        /// ミリ秒の最小値
        /// </summary>
        public const int MS_MIN = 0;
        /// <summary>
        /// ミリ秒の最大値
        /// </summary>
        public const int MS_MAX = 99;
        /// <summary>
        /// ミリ秒のサイズ(bit)
        /// </summary>
        public const int MS_SIZE = 8;

        /// <summary>
        /// BCD形式時のデータ長(バイト)
        /// </summary>
        public new const int BCD_DATA_LENGTH = 8;
        /// <summary>
        /// ミリが格納されている場所
        /// </summary>
        private const int POS_MILLI_SECOND = 7;

                /// <summary>
        /// 有効値パラメータチェック
        /// </summary>
        /// <returns>
        /// 有効値パラメータチェック結果
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#NG}
        /// </returns>
        public new int checkParam()
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            int result = EncDecConst.OK;

            if(base.checkParam() == EncDecConst.NG)
            {
                //エラー箇所のLogは継承元のクラスで出力するのでここではログなし
                result = EncDecConst.NG;
            }

            //ミリ秒
            if (CommonUtils.isOutOfRange(ms, MS_MIN, MS_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : ms is invalid : ms = "
                        + ms);
                result = EncDecConst.NG;
            }
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return result;
        }
        /// <summary>
        /// 設定されているデータをBCD形式のbyte配列で返す
        /// </summary>
        /// <returns>BCD形式のbyte配列</returns>
        public new byte[] getBCDData()
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            List<byte> dataList = new List<byte>();
            dataList.AddRange(base.getBCDData());
            dataList.AddRange(utils.CommonUtils.ToByteArray((ms % 100).ToString("D2")));
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return dataList.ToArray();
        }

                /// <summary>
        /// BCD形式のbyte配列をメンバ変数に設定する
        /// </summary>
        /// <param name="bcdData">BCD形式のbyte配列</param>
        public new void setBCDData(byte[] bcdData)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            if (bcdData.Length < BCD_DATA_LENGTH)
            {
                return;
            }
            base.setBCDData(bcdData);
            //ミリ秒
            ms = (uint)convBCDToIntOneByte(bcdData[POS_MILLI_SECOND]);
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
        }
    }
}
