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
    /// Type0：避難所情報
    /// 2.1.8 1 メッセージエンコード・デコード
    /// Type0エンコード・デコードクラス
    /// Type0メッセージのエンコードを実行する。
    /// Type0メッセージのデコードを実行する。
    /// {@link #msgType}は{@link EncDecConst.VAL_MSG_TYPE0}固定
    /// 
    /// @see MsgFromTerminal
    /// </summary>
    public class MsgType0 : MsgFromTerminal
    {
        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "MsgType0";
        /// <summary>
        /// このクラスのメッセージタイプ
        /// </summary>
        private const int MSG_TYPE = EncDecConst.VAL_MSG_TYPE0;

        // Type 別端末発メッセージ
        /// <summary>
        /// 経度。 有効範囲は{@value #LONGITUDE_MIN}～{@value #LONGITUDE_MAX} 。
        /// （メッセージ用bitへの変換は、 エンコード関数内で実施）
        /// </summary>
        public int longitude = 0;
        /// <summary>
        /// {@link #longitude}の最小値
        /// </summary>
        public const int LONGITUDE_MIN = 0;
        /// <summary>
        /// {@link #longitude}の最大値
        /// </summary>
        public const int LONGITUDE_MAX = 524287;
        /// <summary>
        /// 経度の有効ビット数
        /// </summary>
        public const int LONGITUDE_SIZE = 19;

        /// <summary>
        /// 緯度。有効範囲は{@value #LATITUDE_MIN}～{@value #LATITUDE_MAX}。
        /// （メッセージ用bitへの変換は、 エンコード関数内で実施）
        /// </summary>
        public int latitude = 0;
        /// <summary>
        /// {@link #latitude}の最小値
        /// </summary>
        public const int LATITUDE_MIN = 0;
        /// <summary>
        /// {@link #latitude}の最大値
        /// </summary>
        public const int LATITUDE_MAX = 262143;
        /// <summary>
        /// 緯度の有効ビット数
        /// </summary>
        public const int LATITUDE_SIZE = 18;

        /// <summary>
        /// 避難所開設状況。有効範囲は{@value #ESS_MIN}～{@value #ESS_MAX}。
        /// （メッセージ用bitへの変換は、 エンコード関数内で実施）
        /// </summary>
        public int ess = 0;
        /// <summary>
        /// {@link #ess}の最小値
        /// </summary>
        public const int ESS_MIN = 0;
        /// <summary>
        /// {@link #ess}の最大値
        /// </summary>
        public const int ESS_MAX = 1;
        /// <summary>
        /// 避難所開設情報の有効ビット数
        /// </summary>
        public const int ESS_SIZE = 1;

        /// <summary>
        /// 避難者数。範囲{@value #NUM_OF_RESCUEE_MIN}～
        /// {@value #NUM_OF_RESCUEE_MAX}、{@value #NUM_OF_RESCUEE_MAX2}。
        /// </summary>
        public int numOfRescuee = 0;
        /// <summary>
        /// {@link #numOfRescuee}の最小値
        /// </summary>
        public const int NUM_OF_RESCUEE_MIN = 0;
        /// <summary>
        /// {@link #numOfRescuee}の最大値（範囲）
        /// </summary>
        public const int NUM_OF_RESCUEE_MAX = 1000;
        /// <summary>
        /// {@link #numOfRescuee}の最大値
        /// </summary>
        public const int NUM_OF_RESCUEE_MAX2 = 1023;
        /// <summary>
        /// {@link #numOfRescuee}の有効ビット数
        /// </summary>
        public const int NUM_OF_RESCUEE_SIZE = 10;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MsgType0() {
            msgType = MSG_TYPE;
        }

        /// <summary>
        /// 4.2.8.6 Type0エンコード要求
        /// 【制限事項】
        /// Type0エンコードクラスの以下のパラメータを、エンコード要求前に設定しておくこと。
        /// 設定する定義値については、3.6共通定義を参照
        /// {@link #gId}
        /// {@link #longitude}
        /// {@link #latitude}
        /// {@link #ess}
        /// {@link #numofRescuee}
        /// 
        /// 【備考】
        /// エンコードデータは以下に格納。
        /// {@link #encodedData}
        /// 左詰め{@link MsgFromTerminal#SIZE}bitが有効値となる。
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
        public override int encode(Boolean paramCheck)
        {
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

            // 位置情報 - 経度X(19bit)
            data = longitude;
            size = LONGITUDE_SIZE;
            encode(data, size, encodedData, startPos);
            startPos += size;

            // 位置情報 - 緯度Y(18bit)
            data = latitude;
            size = LATITUDE_SIZE;
            encode(data, size, encodedData, startPos);
            startPos += size;

            // 避難所開設状況(1bit)
            data = ess;
            size = ESS_SIZE;
            encode(data, size, encodedData, startPos);
            startPos += size;

            // 避難者数(10bit)
            data = numOfRescuee;
            size = NUM_OF_RESCUEE_SIZE;
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
        /// 4.2.8.7 Type0デコード要求
        /// 
        /// 【制限事項】
        /// Type0デコードクラスの以下のパラメータを、デコード要求前に設定しておくこと。
        /// {@link #encodedData}
        /// 
        /// 【備考】
        /// デコードデータは以下に格納。
        /// 設定する定義値については、3.6共通定義を参照
        /// {@link #gId}
        /// {@link #longitude}
        /// {@link #latitude}
        /// {@link #ess}
        /// {@link #numOfRescuee}
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

            // 位置情報 - 経度X(19bit)
            size = LONGITUDE_SIZE;
            longitude = decodeInt(encodedData, size, startPos);
            startPos += size;

            // 位置情報 - 緯度Y(18bit)
            size = LATITUDE_SIZE;
            latitude = decodeInt(encodedData, size, startPos);
            startPos += size;

            // 避難所開設状況(1bit)
            size = ESS_SIZE;
            ess = decodeInt(encodedData, size, startPos);
            startPos += size;

            // 避難者数(10bit)
            size = NUM_OF_RESCUEE_SIZE;
            numOfRescuee = decodeInt(encodedData, size, startPos);
            startPos += size;

            // 有効値パラメータチェック
            int result = EncDecConst.OK;
            if (paramCheck) {
                result = checkDecParam(MSG_TYPE);
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
        /// {@link EncDecConst#ENC_VAL_NG}
        /// {@link EncDecConst#ENC_SIZE_NG}
        /// </returns>
        protected override int checkParam()
        {
            int result = EncDecConst.OK;

            // 経度X
            if (CommonUtils.isOutOfRange(longitude, LONGITUDE_MIN, LONGITUDE_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : longitude is invalid : longitude = " + longitude);
                result = EncDecConst.NG;
            }

            // 緯度Y
            if (CommonUtils.isOutOfRange(latitude, LATITUDE_MIN, LATITUDE_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : latitude is invalid : latitude = " + latitude);
                result = EncDecConst.NG;
            }

            // 避難所開設状況
            if (CommonUtils.isOutOfRange(ess, ESS_MIN, ESS_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : ess is invalid : ess = "
                                + ess);
                result = EncDecConst.NG;
            }

            // 避難者数
            if (CommonUtils.isOutOfRange(numOfRescuee, NUM_OF_RESCUEE_MIN, NUM_OF_RESCUEE_MAX)) // 0～1000
            {
                if (CommonUtils.isOutOfRange(numOfRescuee, NUM_OF_RESCUEE_MAX2, NUM_OF_RESCUEE_MAX2)) // 1023
	            {
	                LogMng.AplLogError(TAG +
    	                    "checkParam : numOfRescuee is invalid : numOfRescuee = "
        	                        + numOfRescuee);
            	    result = EncDecConst.NG;
            	}
            }

            return result;
        }
    }
}
