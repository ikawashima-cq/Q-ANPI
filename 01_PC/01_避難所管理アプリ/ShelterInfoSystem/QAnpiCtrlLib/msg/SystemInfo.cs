/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2017. All rights reserved.
 */
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
    /// システム情報クラス
    /// </summary>
    public class SystemInfo
    {
        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "SystemInfo";

        /// <summary>
        /// システム情報全体のビット長
        /// </summary>
        public const int SYSTEM_INFO_SIZE = 136;

        /// <summary>
        /// FWDリンクタイムスロット。範囲値{@value #SYS_FWD_TIME_SLOT_MIN}～{@value #SYS_FWD_TIME_SLOT_MAX}。右詰め
        /// {@value #SYS_FWD_TIME_SLOT_SIZE}bit分を使用する。
        /// </summary>
        public int sysFwdTimeSlot;
        /// <summary>
        /// {@link sysFwdTimeSlot}の最小値
        /// </summary>
        public const int SYS_FWD_TIME_SLOT_MIN = 0;
        /// <summary>
        /// {@link sysFwdTimeSlot}の最大値
        /// </summary>
        public const int SYS_FWD_TIME_SLOT_MAX = 29;
        /// <summary>
        /// {@link sysFwdTimeSlot}の有効ビット数
        /// </summary>
        public const int SYS_FWD_TIME_SLOT_SIZE = 6;

        /// <summary>
        /// 静止衛星位置軌道情報のX座標。範囲{@value #SYS_SATELLITE_POS_X_MIN}～
        /// {@value #SYS_SATELLITE_POS_X_MAX}。右詰め{@value #SYS_SATELLITE_POS_X_SIZE}
        /// bitを使用する。
        /// </summary>
        public int sysSatellitePosX;
        /// <summary>
        /// {@link sysSatellitePosX}の最小値
        /// </summary>
        public const int SYS_SATELLITE_POS_X_MIN = 0;
        /// <summary>
        /// {@link sysSatellitePosX}の最大値
        /// </summary>
        public const int SYS_SATELLITE_POS_X_MAX = 1048575;
        /// <summary>
        /// {@link sysSatellitePosX}の有効ビット数
        /// </summary>
        public const int SYS_SATELLITE_POS_X_SIZE = 20;

        /// <summary>
        /// 静止衛星位置軌道情報のY座標。範囲{@value #SYS_SATELLITE_POS_Y_MIN}～
        /// {@value #SYS_SATELLITE_POS_Y_MAX}。右詰め{@value #SYS_SATELLITE_POS_Y_SIZE}
        /// bitを使用する。
        /// </summary>
        public int sysSatellitePosY;
        /// <summary>
        /// {@link sysSatellitePosY}の最小値
        /// </summary>
        public const int SYS_SATELLITE_POS_Y_MIN = 0;
        /// <summary>
        /// {@link sysSatellitePosY}の最大値
        /// </summary>
        public const int SYS_SATELLITE_POS_Y_MAX = 1048575;
        /// <summary>
        /// {@link sysSatellitePosY}の有効ビット数
        /// </summary>
        public const int SYS_SATELLITE_POS_Y_SIZE = 20;

        /// <summary>
        /// 静止衛星位置軌道情報のZ座標。範囲{@value #SYS_SATELLITE_POS_Z_MIN}～
        /// {@value #SYS_SATELLITE_POS_Z_MAX}。右詰め{@value #SYS_SATELLITE_POS_Z_SIZE}
        /// bitを使用する。
        /// </summary>
        public int sysSatellitePosZ;
        /// <summary>
        /// {@link sysSatellitePosZ}の最小値
        /// </summary>
        public const int SYS_SATELLITE_POS_Z_MIN = 0;
        /// <summary>
        /// {@link sysSatellitePosZ}の最大値
        /// </summary>
        public const int SYS_SATELLITE_POS_Z_MAX = 1048575;
        /// <summary>
        /// {@link sysSatellitePosZ}の有効ビット数
        /// </summary>
        public const int SYS_SATELLITE_POS_Z_SIZE = 20;

        /// <summary>
        /// 地上局・衛星出力端遅延時間。範囲値{@value #SYS_DELAY_TIME_MIN}～
        /// {@value #SYS_DELAY_TIME_MAX}。右詰め{@value #SYS_DELAY_TIME_SIZE}bitを使用する
        /// </summary>
        public int sysDelayTime;
        /// <summary>
        /// {@link sysDelayTime}の最小値
        /// </summary>
        public const int SYS_DELAY_TIME_MIN = 0;
        /// <summary>
        /// {@link sysDelayTime}の最大値
        /// </summary>
        public const int SYS_DELAY_TIME_MAX = 2097151;
        /// <summary>
        /// {@link sysDelayTime}の有効ビット数
        /// </summary>
        public const int SYS_DELAY_TIME_SIZE = 21;

        /// <summary>
        /// メッセージ送信制限。範囲{@value #SYS_SEND_RESTRICTION_MIN}～
        /// {@value #SYS_SEND_RESTRICTION_MAX}。右詰め{@value #SYS_SEND_RESTRICTION_SIZE}
        /// bitを使用する。
        /// </summary>
        public int sysSendRestriction;
        /// <summary>
        /// {@link sysSendRestriction}の最小値
        /// </summary>
        public const int SYS_SEND_RESTRICTION_MIN = 0;
        /// <summary>
        /// {@link sysSendRestriction}の最大値
        /// </summary>
        public const int SYS_SEND_RESTRICTION_MAX = 1;
        /// <summary>
        /// {@link sysSendRestriction}の有効ビット数
        /// </summary>
        public const int SYS_SEND_RESTRICTION_SIZE = 1;

        /// <summary>
        /// アクセス制御基準時刻
        /// </summary>
        public int sysBaseTime;
        /// <summary>
        /// {@link sysBaseTime}の最小値
        /// </summary>
        public const int SYS_BASE_TIME_MIN = 0;
        /// <summary>
        /// {@link sysBaseTime}の最大値
        /// </summary>
        public const int SYS_BASE_TIME_MAX = 0x01FFFFFF;
        /// <summary>
        /// {@link sysBaseTime}の有効ビット数
        /// </summary>
        public const int SYS_BASE_TIME_SIZE = 25;

        /// <summary>
        /// 送信グループ数。範囲{@value #SYS_GROUP_NUM_MIN}～{@value #SYS_GROUP_NUM_MAX}。右詰め
        /// {@value #SYS_GROUP_NUM_SIZE}bitを使用する。
        /// </summary>
        public int sysGroupNum;
        /// <summary>
        /// {@link sysGroupNum}の最小値
        /// </summary>
        public const int SYS_GROUP_NUM_MIN = 1;
        /// <summary>
        /// {@link sysGroupNum}の最大値
        /// </summary>
        public const int SYS_GROUP_NUM_MAX = 4095;
        /// <summary>
        /// sysGroupNumの有効ビット数
        /// </summary>
        public const int SYS_GROUP_NUM_SIZE = 12;


        /// <summary>
        /// 送信スロットランダム選択幅。範囲{@value #SYS_RANDOM_SELECT_BAND_MIN}～{@value #SYS_RANDOM_SELECT_BAND_MAX}.。右詰め
        /// {@value #SYS_RANDOM_SELECT_BAND_SIZE}bitを使用する。
        /// </summary>
        public int sysRandomSelectBand;
        /// <summary>
        /// {@link sysRandomSelectBand}の最小値
        /// </summary>
        public const int SYS_RANDOM_SELECT_BAND_MIN = 1;
        /// <summary>
        /// {@link sysRandomSelectBand}の最大値
        /// </summary>
        public const int SYS_RANDOM_SELECT_BAND_MAX = 7;
        /// <summary>
        /// {@link #sysRandomSelectBand}の有効ビット数
        /// </summary>
        public const int SYS_RANDOM_SELECT_BAND_SIZE = 3;

        /// <summary>
        /// 開始周波数ID。範囲{@value #SYS_START_FREQ_ID_MIN}～
        /// {@value #SYS_START_FREQ_ID_MAX}、{@value #SYS_START_FREQ_ID_MAX2}.。右詰め
        /// {@value #SYS_START_FREQ_ID_SIZE}bitを使用する。
        /// </summary>
        public int sysStartFreqId;
        /// <summary>
        /// {@link sysStartFreqId}の最小値
        /// </summary>
        public const int SYS_START_FREQ_ID_MIN = 0;
        /// <summary>
        /// {@link sysStartFreqId}の最大値（範囲）
        /// </summary>
        public const int SYS_START_FREQ_ID_MAX = 13;
        /// <summary>
        /// {@link sysStartFreqId}の最大値
        /// </summary>
        public const int SYS_START_FREQ_ID_MAX2 = 15;
        /// <summary>
        /// {@link #sysStartFreqId}の有効ビット数
        /// </summary>
        public const int SYS_START_FREQ_ID_SIZE = 4;

        /// <summary>
        /// 最終周波数ID。範囲{@value #SYS_END_FREQ_ID_MIN}～{@value #SYS_END_FREQ_ID_MAX}.。右詰め
        /// {@value #SYS_END_FREQ_ID_SIZE}bitを使用する。
        /// </summary>
        public int sysEndFreqId;
        /// <summary>
        /// {@link sysEndFreqId}の最小値
        /// </summary>
        public const int SYS_END_FREQ_ID_MIN = 0;
        /// <summary>
        /// {@link sysEndFreqId}の最大値
        /// </summary>
        public const int SYS_END_FREQ_ID_MAX = 13;
        /// <summary>
        /// {@link #sysEndFreqId}の有効ビット数
        /// </summary>
        public const int SYS_END_FREQ_ID_SIZE = 4;

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

            /*
             * システム情報
             */
            // FWDリンクタイムスロット
            if (CommonUtils.isOutOfRange(sysFwdTimeSlot, SYS_FWD_TIME_SLOT_MIN, SYS_FWD_TIME_SLOT_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : sysFwdTimeSlot is invalid : sysFwdTimeSlot = "
                        + sysFwdTimeSlot);
                result = EncDecConst.NG;
            }

            // 静止衛星位置軌道情報のX座標
            if (CommonUtils.isOutOfRange(sysSatellitePosX, SYS_SATELLITE_POS_X_MIN,
                    SYS_SATELLITE_POS_X_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : sysSatellitePosX is invalid : sysSatellitePosX = "
                                + sysSatellitePosX);
                result = EncDecConst.NG;
            }

            // 静止衛星位置軌道情報のY座標
            if (CommonUtils.isOutOfRange(sysSatellitePosY, SYS_SATELLITE_POS_Y_MIN,
                    SYS_SATELLITE_POS_Y_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : sysSatellitePosY is invalid : sysSatellitePosY = "
                                + sysSatellitePosY);
                result = EncDecConst.NG;
            }

            // 静止衛星位置軌道情報のZ座標
            if (CommonUtils.isOutOfRange(sysSatellitePosZ, SYS_SATELLITE_POS_Z_MIN,
                    SYS_SATELLITE_POS_Z_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : sysSatellitePosZ is invalid : sysSatellitePosZ = "
                                + sysSatellitePosZ);
                result = EncDecConst.NG;
            }

            // 地上局・衛星出力端遅延時間
            if (CommonUtils.isOutOfRange(sysDelayTime, SYS_DELAY_TIME_MIN,
                    SYS_DELAY_TIME_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : sysDelayTime is invalid : sysDelayTime = "
                                + sysDelayTime);
                result = EncDecConst.NG;
            }

            // メッセージ送信制限
            if (CommonUtils.isOutOfRange(sysSendRestriction, SYS_SEND_RESTRICTION_MIN,
                    SYS_SEND_RESTRICTION_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : sysSendRestriction is invalid : sysSendRestriction = "
                                + sysSendRestriction);
                result = EncDecConst.NG;
            }

            // アクセス制御基準時刻
            if (CommonUtils.isOutOfRange(sysBaseTime, SYS_BASE_TIME_MIN,
                    SYS_BASE_TIME_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : sysBaseTime is invalid : sysBaseTime = "
                                + sysBaseTime);
                result = EncDecConst.NG;
            }

            // 送信グループ数
            if (CommonUtils.isOutOfRange(sysGroupNum, SYS_GROUP_NUM_MIN,
                    SYS_GROUP_NUM_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : sysGroupNum is invalid : sysGroupNum = "
                                + sysGroupNum);
                result = EncDecConst.NG;
            }

            // 送信スロットランダム選択幅
            if (CommonUtils.isOutOfRange(sysRandomSelectBand, SYS_RANDOM_SELECT_BAND_MIN,
                    SYS_RANDOM_SELECT_BAND_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : sysRandomSelectBand is invalid : sysRandomSelectBand = "
                                + sysRandomSelectBand);
                result = EncDecConst.NG;
            }

            // 開始周波数ID
            if (CommonUtils.isOutOfRange(sysStartFreqId, SYS_START_FREQ_ID_MIN, SYS_START_FREQ_ID_MAX)) // 0～13
            {
                if (CommonUtils.isOutOfRange(sysStartFreqId, SYS_START_FREQ_ID_MAX2, SYS_START_FREQ_ID_MAX2)) // 15
                {
                    LogMng.AplLogError(TAG +
                            "checkParam : sysStartFreqId is invalid : sysStartFreqId = "
                                    + sysStartFreqId);
                    result = EncDecConst.NG;
                }
            }

            // 最終周波数ID
            if (CommonUtils.isOutOfRange(sysEndFreqId, SYS_END_FREQ_ID_MIN,
                    SYS_END_FREQ_ID_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : sysEndFreqId is invalid : sysEndFreqId = "
                                + sysEndFreqId);
                result = EncDecConst.NG;
            }

            return result;
        }
    }
}
