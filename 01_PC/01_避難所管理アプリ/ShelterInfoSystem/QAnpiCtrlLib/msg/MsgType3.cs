/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
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
    /// Type3：安否テキスト
    /// 2.1.8 1 メッセージエンコード・デコード
    /// Type3エンコード・デコードクラス
    /// Type3メッセージのエンコードを実行する。
    /// Type3メッセージのデコードを実行する。
    /// </summary>
    public class MsgType3 : MsgFromTerminal
    {
        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "MsgType3";

        /// <summary>
        /// このクラスのメッセージタイプ
        /// </summary>
        private const int MSG_TYPE = EncDecConst.VAL_MSG_TYPE3;

	    // Type 別端末発メッセージ

	    /// <summary>
        /// 救助支援情報ID数。右詰め{@value #RESCUE_SUPPORT_INFO_NUMBER_SIZE}bit分を使用する。
	    /// </summary>
        public int rescueSupportInfoNumber;
	    /// <summary>
	    /// {@link #seqNo}の有効ビット数
	    /// </summary>
        public const int RESCUE_SUPPORT_INFO_NUMBER_SIZE = 2;
        /// <summary>
        /// {@link #seqNo}の最小値
        /// </summary>
        public const int RESCUE_SUPPORT_INFO_NUMBER_MIN = 0;
        /// <summary>
        /// {@link #seqNo}の最大値
        /// </summary>
        public const int RESCUE_SUPPORT_INFO_NUMBER_MAX= 3;

        /// <summary>
        /// 救助支援情報ID0。右詰め{@value #RESCUE_SUPPORT_INFO_ID0_SIZE}bit分を使用する。
        /// </summary>
        public int rescueSupportInfoId0;
        /// <summary>
        /// {@link #rescueSupportInfoId0}の有効ビット数
        /// </summary>
        public const int RESCUE_SUPPORT_INFO_ID0_SIZE = 6;
        /// <summary>
        /// {@link #rescueSupportInfoId0}の最小値
        /// </summary>
        public const int RESCUE_SUPPORT_INFO_ID0_MIN = 0;
        /// <summary>
        /// {@link #rescueSupportInfoId0}の最大値
        /// </summary>
        public const int RESCUE_SUPPORT_INFO_ID0_MAX = 63;

        /// <summary>
        /// 救助支援情報ID1。右詰め{@value #RESCUE_SUPPORT_INFO_ID1_SIZE}bit分を使用する。
        /// </summary>
        public int rescueSupportInfoId1;
        /// <summary>
        /// {@link #rescueSupportInfoId1}の有効ビット数
        /// </summary>
        public const int RESCUE_SUPPORT_INFO_ID1_SIZE = 6;
        /// <summary>
        /// {@link #rescueSupportInfoId1}の最小値
        /// </summary>
        public const int RESCUE_SUPPORT_INFO_ID1_MIN = 0;
        /// <summary>
        /// {@link #rescueSupportInfoId1}の最大値
        /// </summary>
        public const int RESCUE_SUPPORT_INFO_ID1_MAX = 63;

        /// <summary>
        /// 救助支援情報ID2。右詰め{@value #RESCUE_SUPPORT_INFO_ID2_SIZE}bit分を使用する。
        /// </summary>
        public int rescueSupportInfoId2;
        /// <summary>
        /// {@link #rescueSupportInfoId2}の有効ビット数
        /// </summary>
        public const int RESCUE_SUPPORT_INFO_ID2_SIZE = 6;
        /// <summary>
        /// {@link #rescueSupportInfoId2}の最小値
        /// </summary>
        public const int RESCUE_SUPPORT_INFO_ID2_MIN = 0;
        /// <summary>
        /// {@link #rescueSupportInfoId2}の最大値
        /// </summary>
        public const int RESCUE_SUPPORT_INFO_ID2_MAX = 63;

        /// <summary>
        /// 支援情報要求。右詰め{@value #SUPPORT_INFO_REQUEST_MAX}bit分を使用する。
        /// </summary>
        public int supportInfoRequest;
        /// <summary>
        /// {@link #supportInfoRequest}の有効ビット数
        /// </summary>
        public const int SUPPORT_INFO_REQUEST_SIZE = 2;
        /// <summary>
        /// {@link #supportInfoRequest}の最小値
        /// </summary>
        public const int SUPPORT_INFO_REQUEST_MIN = 0;
        /// <summary>
        /// {@link #supportInfoRequest}の最大値
        /// </summary>
        public const int SUPPORT_INFO_REQUEST_MAX = 3;

	    /// <summary>
	    /// Reserveエリア。右詰め{@value #RESERVED1_SIZE}bit分を使用する。初期値は、0(仕様は0保証)とする。
	    /// </summary>
        [System.Xml.Serialization.XmlIgnore]
	    public int reserved1 = 0;
	    /// <summary>
	    /// Reserveエリアの有効ビット数
	    /// </summary>
	    public const int RESERVED1_SIZE = 26;

	    /// <summary>
	    /// コンストラクタ
	    /// </summary>
	    public MsgType3() {
            msgType = MSG_TYPE;
	    }

        /// <summary>
        /// Type3デコード要求
        ///
        /// 【制限事項】
        /// Type3デコードクラスの以下のパラメータを、デコード要求前に設定しておくこと。
        /// {@link #encodedData}
        ///
        /// 【備考】
        /// デコードデータは以下に格納。
        /// 設定する定義値については、3.6共通定義を参照
        /// {@link #rescueSupportInfoNumber}
        /// {@link #rescueSupportInfoId0}
        /// {@link #rescueSupportInfoId1}
        /// {@link #rescueSupportInfoId2}
        /// {@link #supportInfoRequest}
        /// {@link #reserved1}
        /// </summary>
        /// <param name="paramCheck">
        /// True：有効値パラメータチェック実施
        /// False：有効値パラメータチェック未実施
        /// </param>
        /// <returns>
        /// デコード結果。結果の定義は、3.6章共通定義を参照
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#DEC_GETTYPE_NG}
        /// {@link EncDecConst#DEC_VAL_NG}
        /// </returns>
	    public override int encode(Boolean paramCheck) {
	        // 有効値パラメータチェック
	        if (paramCheck) {
                int result = checkEncParam(MSG_TYPE);
	            if (result != EncDecConst.OK) {
	                return result;
	            }
	        }

	        // 共通フィールドのエンコード
	        int startPos = encodeCommonField();
	        int data;
	        int size;

	        // 救助支援情報ID数
            data = rescueSupportInfoNumber;
	        size = RESCUE_SUPPORT_INFO_NUMBER_SIZE;
	        encode(data, size, encodedData, startPos);
	        startPos += size;

            // 救助支援情報ID0
	        data = rescueSupportInfoId0;
	        size = RESCUE_SUPPORT_INFO_ID0_SIZE;
	        encode(data, size, encodedData, startPos);
	        startPos += size;

            // 救助支援情報ID1
            data = rescueSupportInfoId1;
            size = RESCUE_SUPPORT_INFO_ID1_SIZE;
            encode(data, size, encodedData, startPos);
            startPos += size;

            // 救助支援情報ID2
            data = rescueSupportInfoId2;
            size = RESCUE_SUPPORT_INFO_ID2_SIZE;
            encode(data, size, encodedData, startPos);
            startPos += size;

            //支援情報要求
            data = supportInfoRequest;
            size = SUPPORT_INFO_REQUEST_SIZE;
            encode(data, size, encodedData, startPos);
            startPos += size;

	        // Reserve
	        data = reserved1;
	        size = RESERVED1_SIZE;
	        encode(data, size, encodedData, startPos);
	        startPos += size;

            // エンコードサイズチェック
            if (startPos != MSG_FOR_TYPE_SIZE)
            {
                return EncDecConst.ENC_SIZE_NG;
            }

	        return EncDecConst.OK;
	    }

	    /// <summary>
        /// 4.2.8.12 Type3デコード要求
        /// 
        /// 【制限事項】
        /// Type3デコードクラスの以下のパラメータを、デコード要求前に設定しておくこと。
        /// {@link #encodedData}
        /// 
        /// 【備考】
        /// デコードデータは以下に格納。
        /// 設定する定義値については、3.6共通定義を参照
        /// {@link #testBit}
        /// {@link #userId}
        /// {@link #longMsg}
        /// {@link #seqNo}
        /// {@link #reserved1}
        /// </summary>
        /// <param name="paramCheck">
        /// True：有効値パラメータチェック実施
        /// False：有効値パラメータチェック未実施
        /// </param>
        /// <returns>
        /// デコード結果。結果の定義は、3.6章共通定義を参照
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#DEC_GETTYPE_NG}
        /// {@link EncDecConst#DEC_VAL_NG}
        /// </returns>
        public override int decode(Boolean paramCheck)
        {
	        int size;
	        int startPos;

	        // 共通フィールドのデコード
	        startPos = decodeCommonField();

            // 救助支援情報ID数
            size = RESCUE_SUPPORT_INFO_NUMBER_SIZE;
            rescueSupportInfoNumber = decodeInt(encodedData, size, startPos);
	        startPos += size;

            // 救助支援情報ID0
            size = RESCUE_SUPPORT_INFO_ID0_SIZE;
            rescueSupportInfoId0 = decodeInt(encodedData, size, startPos);
            startPos += size;

            // 救助支援情報ID1
            size = RESCUE_SUPPORT_INFO_ID1_SIZE;
            rescueSupportInfoId1 = decodeInt(encodedData, size, startPos);
            startPos += size;

            // 救助支援情報ID2
            size = RESCUE_SUPPORT_INFO_ID2_SIZE;
            rescueSupportInfoId2 = decodeInt(encodedData, size, startPos);
            startPos += size;

            // 支援情報要求
            size = SUPPORT_INFO_REQUEST_SIZE;
            supportInfoRequest = decodeInt(encodedData, size, startPos);
            startPos += size;

	        // Reserve
	        size = RESERVED1_SIZE;
	        reserved1 = decodeInt(encodedData, size, startPos);
	        startPos += size;

	        // 有効値パラメータチェック
	        if (paramCheck) {
                int result = checkDecParam(MSG_TYPE);
	            if (result != EncDecConst.OK) {
	                return result;
	            }
	        }
	        return EncDecConst.OK;
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
        protected override int checkParam() {
	        int result = EncDecConst.OK;
            // 救助支援情報ID数
            if (CommonUtils.isOutOfRange(rescueSupportInfoNumber, RESCUE_SUPPORT_INFO_NUMBER_MIN, RESCUE_SUPPORT_INFO_NUMBER_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : rescueSupportInfoNumber is invalid : rescueSupportInfoNumber = "
                        + rescueSupportInfoNumber);
                result = EncDecConst.NG;
            }

            // 救助支援情報ID0
            if (CommonUtils.isOutOfRange(rescueSupportInfoId0, RESCUE_SUPPORT_INFO_ID0_MIN, RESCUE_SUPPORT_INFO_ID0_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : rescueSupportInfoId0 is invalid : rescueSupportInfoId0 = "
                        + rescueSupportInfoId0);
                result = EncDecConst.NG;
            }

            // 救助支援情報ID1
            if (CommonUtils.isOutOfRange(rescueSupportInfoId1, RESCUE_SUPPORT_INFO_ID1_MIN, RESCUE_SUPPORT_INFO_ID1_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : rescueSupportInfoId1 is invalid : rescueSupportInfoId1 = "
                        + rescueSupportInfoId1);
                result = EncDecConst.NG;
            }

            // 救助支援情報ID2
            if (CommonUtils.isOutOfRange(rescueSupportInfoId2, RESCUE_SUPPORT_INFO_ID2_MIN, RESCUE_SUPPORT_INFO_ID2_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : rescueSupportInfoId2 is invalid : rescueSupportInfoId2 = "
                        + rescueSupportInfoId2);
                result = EncDecConst.NG;
            }

            // 支援情報要求
            if (CommonUtils.isOutOfRange(supportInfoRequest, SUPPORT_INFO_REQUEST_MIN, SUPPORT_INFO_REQUEST_MAX))
            {
                LogMng.AplLogError(TAG + "checkParam : supportInfoRequest is invalid : supportInfoRequest = "
                        + supportInfoRequest);
                result = EncDecConst.NG;
            }

            // Reserve
            if (reserved1 != 0)
            {
                LogMng.AplLogError(TAG +
                        "checkParam : reserved1 is invalid : reserved1 = "
                                + reserved1);
                result = EncDecConst.NG;
            }

            return result;
	    }
    }
}
