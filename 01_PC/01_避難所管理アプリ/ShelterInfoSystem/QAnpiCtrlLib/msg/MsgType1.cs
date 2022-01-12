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
    /// Type1：安否情報
    /// 2.1.8 1 メッセージエンコード・デコード
    /// Type1エンコード・デコードクラス
    /// Type1メッセージのエンコードを実行する。
    /// Type1メッセージのデコードを実行する。
    /// {@link #msgType}は{@link EncDecConst.VAL_MSG_TYPE1}固定
    /// 
    /// @see MsgFromTerminal
    /// </summary>
    public class MsgType1 : MsgFromTerminal
    {
        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "MsgType1";
        /// <summary>
        /// このクラスのメッセージタイプ
        /// </summary>
        private const int MSG_TYPE = EncDecConst.VAL_MSG_TYPE1;

        // Type 別端末発メッセージ

        /// <summary>
        /// 個人ID。 有効範囲は{@value #PERSONAL_ID_MIN}～{@value #PERSONAL_ID_MAX} 。
        /// （メッセージ用bitへの変換は、 エンコード関数内で実施）
        /// </summary>
        public long personalId = 0;
        /// <summary>
        /// {@link #personalId}の最小値
        /// </summary>
        public const long PERSONAL_ID_MIN = 1;
        /// <summary>
        /// {@link #personalId}の最大値
        /// </summary>
        public const long PERSONAL_ID_MAX = 999999999999;
        /// <summary>
        /// {@link #personalId}の有効ビット数
        /// </summary>
        public const int PERSONAL_ID_SIZE = 40;

        /// <summary>
        /// 安否情報公開可否。有効範囲は{@value #ANPIINFO_PO_MIN}～{@value #ANPIINFO_PO_MAX}。
        /// （メッセージ用bitへの変換は、 エンコード関数内で実施）
        /// </summary>
        public int anpiInfo_prohibitOpen = 0;
        /// <summary>
        /// {@link #anpiInfo_prohibitOpen}の最小値
        /// </summary>
        public const int ANPIINFO_PO_MIN = 0;
        /// <summary>
        /// {@link #anpiInfo_prohibitOpen}の最大値
        /// </summary>
        public const int ANPIINFO_PO_MAX = 1;
        /// <summary>
        /// {@link #anpiInfo_prohibitOpen}の有効ビット数
        /// </summary>
        public const int ANPIINFO_PO_SIZE = 1;

        /// <summary>
        /// 安否情報。範囲{@value #ANPIINFO_MIN}～
        /// {@value #ANPIINFO_MAX}。
        /// </summary>
        public int anpiInfo;
        /// <summary>
        /// {@link #anpiInfo}の最小値
        /// </summary>
        public const int ANPIINFO_MIN = 0;
        /// <summary>
        /// {@link #anpiInfo}の最大値
        /// </summary>
        public const int ANPIINFO_MAX = 31;
        /// <summary>
        /// {@link #anpiInfo}の有効ビット数
        /// </summary>
        public const int ANPIINFO_SIZE = 5;

        /// <summary>
        /// 安否補足情報。範囲{@value #ANPIINFO_SUP_MIN}～{@value #ANPIINFO_SUP_MAX}。
        /// </summary>
        public int anpiInfoSupplement;
        /// <summary>
        /// {@link #anpiInfoSupplement}の最小値
        /// </summary>
        public const int ANPIINFO_SUP_MIN = 0;
        /// <summary>
        /// {@link #anpiInfoSupplement}の最大値
        /// </summary>
        public const int ANPIINFO_SUP_MAX = 3;
        /// <summary>
        /// {@link #anpiInfoSupplement}の有効ビット数
        /// </summary>
        public const int ANPIINFO_SUP_SIZE = 2;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MsgType1() {
            msgType = MSG_TYPE;
        }

        /// <summary>
        /// 4.2.8.6 Type1エンコード要求
        /// 【制限事項】
        /// Type1エンコードクラスの以下のパラメータを、エンコード要求前に設定しておくこと。
        /// 設定する定義値については、3.6共通定義を参照
        /// {@link #groupId}
        /// {@link #personalId}
        /// {@link #anpiInfo_prohibitOpen}
        /// {@link #anpiInfo}
        /// {@link #anpiInfoSupplement}
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
            long datal;
            int size;

            // 個人ID(40bit)
            datal = personalId;
            size = PERSONAL_ID_SIZE;
            encode(datal, size, encodedData, startPos);
            startPos += size;

            // 安否情報公開可否(1bit)
            data = anpiInfo_prohibitOpen;
            size = ANPIINFO_PO_SIZE;
            encode(data, size, encodedData, startPos);
            startPos += size;

            // 安否情報(5bit)
            data = anpiInfo;
            size = ANPIINFO_SIZE;
            encode(data, size, encodedData, startPos);
            startPos += size;

            // 安否補足情報(2bit)
            data = anpiInfoSupplement;
            size = ANPIINFO_SUP_SIZE;
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
        /// 4.2.8.7 Type1デコード要求
        /// 
        /// 【制限事項】
        /// Type1デコードクラスの以下のパラメータを、デコード要求前に設定しておくこと。
        /// {@link #encodedData}
        /// 
        /// 【備考】
        /// デコードデータは以下に格納。
        /// 設定する定義値については、3.6共通定義を参照
        /// {@link #groupId}
        /// {@link #personalId}
        /// {@link #anpiInfo_prohibitOpen}
        /// {@link #anpiInfo}
        /// {@link #anpiInfoSupplement}
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

            // 個人ID(40bit)
            size = PERSONAL_ID_SIZE;
            personalId = decodeLong(encodedData, size, startPos);
            startPos += size;

            // 安否情報公開可否(1bit)
            size = ANPIINFO_PO_SIZE;
            anpiInfo_prohibitOpen = decodeInt(encodedData, size, startPos);
            startPos += size;

            // 安否情報(5bit)
            size = ANPIINFO_SIZE;
            anpiInfo = decodeInt(encodedData, size, startPos);
            startPos += size;

            // 安否補足情報(2bit)
            size = ANPIINFO_SUP_SIZE;
            anpiInfoSupplement = decodeInt(encodedData, size, startPos);
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

            // 個人ID
            if (CommonUtils.isOutOfRange(personalId, PERSONAL_ID_MIN, PERSONAL_ID_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : personalId is invalid : personalId = " + personalId);
                result = EncDecConst.NG;
            }

            // 安否情報公開可否
            if (CommonUtils.isOutOfRange(anpiInfo_prohibitOpen, ANPIINFO_PO_MIN, ANPIINFO_PO_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : anpiInfo_prohibitOpen is invalid : anpiInfo_prohibitOpen = " + anpiInfo_prohibitOpen);
                result = EncDecConst.NG;
            }

            // 安否情報
            if (CommonUtils.isOutOfRange(anpiInfo, ANPIINFO_MIN, ANPIINFO_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : anpiInfo is invalid : anpiInfo = "
                                + anpiInfo);
                result = EncDecConst.NG;
            }

            // 安否補足情報
            if (CommonUtils.isOutOfRange(anpiInfoSupplement, ANPIINFO_SUP_MIN, ANPIINFO_SUP_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : anpiInfoSupplement is invalid : anpiInfoSupplement = "
                                + anpiInfoSupplement);
                result = EncDecConst.NG;
            }

            return result;
        }
    }
}
