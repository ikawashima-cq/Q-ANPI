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
    /// 端末宛メッセージ共通部
    /// 各Typeの端末宛メッセージは本クラスを継承して利用すること
    /// 共通フィールドはType：{@link #msgType}、システム情報：{@link #sysInfo}とし、
    /// Type別端末宛メッセージは各Type用の子クラスで処理を実装すること
    /// 
    /// @see EncodeManager
    /// @see DecodeManager
    /// </summary>
    public abstract class MsgToTerminal : EncodeManager
    {
        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "MsgToTerminal";

        /// <summary>
        /// 端末宛メッセージ長
        /// </summary>
        public const int SIZE = 3440;

        /// <summary>
        /// エンコード後データの格納エリア
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public byte[] encodedData;

        /// <summary>
        /// Type({@value #MSG_TYPE_SIZE}bit)
        /// 端末発メッセージに含まれるType 別端末発メッセージの種別を示す。
        /// </summary>
        public int msgType;

        /// <summary>
        /// {@link msgType}の有効ビット数
        /// </summary>
        public const int MSG_TYPE_SIZE = 8;

        /// <summary>
        /// システム情報({@link SystemInfo#SYSTEM_INFO_SIZE}bit)
        /// </summary>
        public SystemInfo sysInfo;
        
        /// <summary>
        /// Type別端末宛メッセージの有効ビット数
        /// </summary>
        public const int MSG_FOR_TYPE_SIZE = 3440;

        /// <summary>
        /// コンストラクタ
        ///    encodedDataのインスタンスを生成する
        /// </summary>
        public MsgToTerminal()
        {
            int len = sizeToLength(SIZE);
            encodedData = new byte[len]; // 左詰め3440bitが有効値
            sysInfo = new SystemInfo();
        }

        /// <summary>
        /// 端末宛メッセージの共通フィールドをエンコードする
        /// エンコードデータは{@link #encodedData}に格納される
        /// 【制約事項】実行前に{@link #msgType}、{@link #sysInfo}を設定すること
        /// </summary>
        /// <returns>エンコードしたビット数</returns>
        public virtual int encodeCommonField() {
            int data;
            int size;
            int startPos = 0;

            // Type
            data = msgType;
            size = MSG_TYPE_SIZE;
            encode(data, size, encodedData, startPos);
            startPos += size;

            // システム情報
            if (sysInfo != null) {
                startPos += encode(sysInfo, startPos);
            }
            else{
                startPos += SystemInfo.SYSTEM_INFO_SIZE;
            }

            return startPos;
        }

        /// <summary>
        /// システム情報をエンコードする
        /// エンコードデータは{@link #encodedData}にstartPos位置から書き込まれる
        /// </summary>
        /// <param name="sysInfo">エンコードするシステム情報</param>
        /// <param name="startPos">エンコードデータエリアの保存開始位置(bit数)</param>
        /// <returns>エンコードしたビット数</returns>
        private int encode(SystemInfo sysInfo, int startPos) {
            int data;
            int size;
            int pos = startPos;

            /*
             * システム情報
             */
            // FWDリンクタイムスロット
            data = sysInfo.sysFwdTimeSlot;
            size = SystemInfo.SYS_FWD_TIME_SLOT_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            // 静止衛星位置軌道情報のX座標
            data = sysInfo.sysSatellitePosX;
            size = SystemInfo.SYS_SATELLITE_POS_X_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            // 静止衛星位置軌道情報のY座標
            data = sysInfo.sysSatellitePosY;
            size = SystemInfo.SYS_SATELLITE_POS_Y_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            // 静止衛星位置軌道情報のZ座標
            data = sysInfo.sysSatellitePosZ;
            size = SystemInfo.SYS_SATELLITE_POS_Z_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            // 地上局・衛星出力端遅延時間
            data = sysInfo.sysDelayTime;
            size = SystemInfo.SYS_DELAY_TIME_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            // メッセージ送信制限
            data = sysInfo.sysSendRestriction;
            size = SystemInfo.SYS_SEND_RESTRICTION_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            // アクセス制御基準時刻
            data = sysInfo.sysBaseTime;
            size = SystemInfo.SYS_BASE_TIME_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            // 送信グループ数
            data = sysInfo.sysGroupNum;
            size = SystemInfo.SYS_GROUP_NUM_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            // 送信スロットランダム選択幅
            data = sysInfo.sysRandomSelectBand;
            size = SystemInfo.SYS_RANDOM_SELECT_BAND_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            // 開始周波数ID
            data = sysInfo.sysStartFreqId;
            size = SystemInfo.SYS_START_FREQ_ID_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            // 最終周波数ID
            data = sysInfo.sysEndFreqId;
            size = SystemInfo.SYS_END_FREQ_ID_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            return pos - startPos;
        }

        /// <summary>
        /// 端末宛メッセージの共通フィールドをデコードする
        /// デコードしたデータは{@link #msgType}、{@link #sysInfo}に格納される
        /// 【制約事項】実行前に{@link #encodedData}にエンコード後のデータ（受信データ）を設定しておくこと
        /// </summary>
        /// <param name="sysInfoDecode">
        /// true：システム情報デコード実施
        /// false：システム情報デコード未実施
        /// </param>
        /// <returns>デコードしたビット数</returns>
        public virtual int decodeCommonField(Boolean sysInfoDecode) {
            int startPos = 0;

            // Typeとシステム情報をまとめて読み出し
            TypeAndSystemInfo ts =
                    decodeTypeAndSystemInfo(encodedData, SIZE, sysInfoDecode);

            // Type
            msgType = ts.msgType;
            startPos += MSG_TYPE_SIZE;

            // システム情報
            if (sysInfoDecode) {
                sysInfo = ts.sysInfo;
            }
            // システム情報デコード未実施でもデコード扱いとしてstartPosに加算する
            startPos += SystemInfo.SYSTEM_INFO_SIZE;

            return startPos;
        }

        /// <summary>
        /// エンコード要求
        /// </summary>
        /// <param name="paramCheck">
        /// True：有効値パラメータチェック実施
        /// False：有効値パラメータチェック未実施
        /// ※型変換時のサイズチェックは実施。bit範囲に収まっている値かどうか。
        /// </param>
        /// <returns>
        /// エンコード結果。結果の定義は、3.6章共通定義を参照
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#ENC_VAL_NG}
        /// {@link EncDecConst#ENC_SIZE_NG}
        /// </returns>
        public abstract int encode(Boolean paramCheck);

        /// <summary>
        /// デコード要求
        /// </summary>
        /// <param name="paramCheck">
        /// True：有効値パラメータチェック実施
        /// False：有効値パラメータチェック未実施
        /// </param>
        /// <param name="sysInfoDecode">
        /// True：システム情報デコード実施
        /// False：システム情報デコード未実施
        /// </param>
        /// <returns>
        /// デコード結果。結果の定義は、3.6章共通定義を参照
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#DEC_GETTYPE_NG}
        /// {@link EncDecConst#DEC_VAL_NG}
        /// </returns>
        public abstract int decode(Boolean paramCheck, Boolean sysInfoDecode);

        /// <summary>
        /// 有効値パラメータチェック(エンコード用)
        /// Typeに依存しないパラメータはこのメソッドにチェック処理を実装する
        /// </summary>
        /// <param name="type">正解のType値</param>
        /// <returns>
        /// 有効値パラメータチェック結果
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#ENC_VAL_NG}
        /// {@link EncDecConst#ENC_SIZE_NG}
        /// </returns>
        protected virtual int checkEncParam(int type)
        {
            int result = EncDecConst.OK;

            // システム情報
            if (sysInfo == null)
            {
                LogMng.AplLogError(TAG +
                        "checkEncParam : sysInfo is invalid : sysInfo = null");
                result = EncDecConst.ENC_VAL_NG;
            }
            else if (sysInfo.checkParam() != EncDecConst.OK)
            {
                LogMng.AplLogError(TAG + "checkEncParam : sysInfo has invalid parameter");
                result = EncDecConst.ENC_VAL_NG;
            }

            // Type別パラメータ
            if (checkParam() == EncDecConst.NG)
            {
                // checkParam()内でエラーログを出すのでここではログ出力不要
                result = EncDecConst.ENC_VAL_NG;
            }

            // Type(範囲チェックはしない)
            if (msgType != type)
            {
                LogMng.AplLogError(TAG + "checkEncParam : msgType is invalid : msgType = "
                                + msgType);
                result = EncDecConst.ENC_VAL_NG;
            }

            return result;
        }

        /// <summary>
        /// 有効値パラメータチェック(デコード用)
        /// Typeに依存しないパラメータはこのメソッドにチェック処理を実装する
        /// </summary>
        /// <param name="sysInfoCheck">
        /// True：システム情報チェック実施
        /// False：システム情報チェック未実施
        /// </param>
        /// <param name="type">正解のType値</param>
        /// <returns>
        /// 有効値パラメータチェック結果
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#DEC_GETTYPE_NG}
        /// {@link EncDecConst#DEC_VAL_NG}
        /// </returns>
        protected virtual int checkDecParam(Boolean sysInfoCheck, int type)
        {
            int result = EncDecConst.OK;

            // システム情報
            if (sysInfoCheck)
            {
                if (sysInfo == null)
                {
                    LogMng.AplLogError(TAG +
                            "checkDecParam : sysInfo is invalid : sysInfo = null");
                    result = EncDecConst.DEC_VAL_NG;
                }
                else if (sysInfo.checkParam() != EncDecConst.OK)
                {
                    LogMng.AplLogError(TAG +
                            "checkEncParam : sysInfo has invalid parameter");
                    result = EncDecConst.DEC_VAL_NG;
                }
            }

            // Type別パラメータ
            if (checkParam() == EncDecConst.NG)
            {
                // checkParam()内でエラーログを出すのでここではログ出力不要
                result = EncDecConst.DEC_VAL_NG;
            }

            // Type(範囲チェックはしない)
            // DEC_GETTYPE_NGの判定
            if (msgType != type)
            {
                LogMng.AplLogError(TAG + "checkDecParam : msgType is invalid : msgType = "
                                + msgType);
                result = EncDecConst.DEC_GETTYPE_NG;
            }

            return result;
        }

        /// <summary>
        /// 有効値パラメータチェック
        /// Typeに依存するパラメータをチェックする
        /// </summary>
        /// <returns>
        /// 有効値パラメータチェック結果
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#NG}
        /// </returns>
        protected abstract int checkParam();


    }
}
