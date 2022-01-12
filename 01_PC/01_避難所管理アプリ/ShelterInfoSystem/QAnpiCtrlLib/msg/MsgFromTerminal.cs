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
    /// 端末発メッセージ共通部
    /// 各Typeの端末発メッセージは本クラスを継承して利用すること
    /// 共通フィールドはTest、Type、ユーザIDとし、
    /// Type別端末発メッセージは各Type用の子クラスで処理を実装すること
    /// 
    /// @see EncodeManager
    /// @see DecodeManager
    /// </summary>
    public abstract class MsgFromTerminal : EncodeManager
    {
        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "MsgFromTerminal";

        /// <summary>
        ///端末発メッセージ長
        /// </summary>
        public const int SIZE = 84;

        /// <summary>
        ///エンコード後データの格納エリア
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public byte[] encodedData;
        /// <summary>
        /// Type({@value #MSG_TYPE_SIZE}bit)
        /// 端末発メッセージに含まれるType 別端末発メッセージの種別を示す。
        /// </summary>
        public int msgType;
        /// <summary>
        /// {@link #msgType}の最小値
        /// </summary>
        public const int MSG_TYPE_MIN = 0;
        /// <summary>
        /// {@link #msgType}の最大値
        /// </summary>
        public const int MSG_TYPE_MAX = 3;
        /// <summary>
        ///{@link msgType}の有効ビット数
        /// </summary>
        public const int MSG_TYPE_SIZE = 2;
        /// <summary>
        ///組番号。有効bitは右詰{@value #GROUP_ID_SIZE}bitとなる。{@value #GROUP_ID_MIN}～
        ///{@value #GROUP_ID_MAX}
        /// </summary>
        public int gId;
        /// <summary>
        ///{@link groupId}の最小値
        /// </summary>
        public const int GROUP_ID_MIN = 0x00000000;
        /// <summary>
        ///{@link groupId}の最大値
        /// </summary>
        public const int GROUP_ID_MAX = 0x00000FFF;
        /// <summary>
        ///{@link groupId}の有効ビット数
        /// </summary>
        public const int GROUP_ID_SIZE = 12;

        /// <summary>
        ///巡回冗長検査。有効bitは右詰{@value #CRC_SIZE}bitとなる。{@value #CRC_MIN}～
        ///{@value #CRC_MAX}
        /// </summary>
        public int crc;
        /// <summary>
        ///{@link crc}の最小値
        /// </summary>
        public const int CRC_MIN = 0x00000000;
        /// <summary>
        ///{@link crc}の最大値
        /// </summary>
        public const int CRC_MAX = 0x0000FFFF;
        /// <summary>
        ///{@link crc}の有効ビット数
        /// </summary>
        public const int CRC_SIZE = 16;

        /// <summary>
        ///Type別端末発メッセージの有効ビット数
        /// </summary>
        public const int MSG_FOR_TYPE_SIZE = 62;

        /// <summary>
        ///コンストラクタ
        ///encodedDataのインスタンスを生成する
        /// </summary>
        public MsgFromTerminal() {
            int len = sizeToLength(SIZE);
            encodedData = new byte[len]; // 左詰め84bitが有効値
        }

        /// <summary>
        /// 端末発メッセージの共通フィールドをエンコードする
        /// エンコードデータは{@link #encodedData} に格納される
        /// 
        /// 【制約事項】
        /// 実行前に{@link #testBit}、{@link #msgType}、{@link #userId}を設定すること
        /// </summary>
        /// <return>エンコードしたビット数</return>
        public int encodeCommonField() {
            int data;
            int size;
            int startPos = 0;

            // Type
            data = msgType;
            size = MSG_TYPE_SIZE;
            encode(data, size, encodedData, startPos);
            startPos += size;

            //グループID
            data = gId;
            size = GROUP_ID_SIZE;
            encode(data, size, encodedData, startPos);
            startPos += size;

            //crc
            data = crc;
            size = CRC_SIZE;
            encode(data, size, encodedData, MSG_FOR_TYPE_SIZE);


            return startPos;
        }

        /// <summary>
        /// 端末発メッセージの共通フィールドをデコードする
        /// デコードしたデータは{@link #testBit}、{@link #msgType}、{@link #userId}に格納される
        /// 
        ///【制約事項】
        /// 実行前に{@link #encodedData}にエンコード後のデータ（受信データ）を設定しておくこと
        /// </summary>
        /// <return>デコードしたビット数</return>
        public int decodeCommonField() {
            int size;
            int startPos = 0;

            // Type
            size = MSG_TYPE_SIZE;
            msgType = decodeInt(encodedData, size, startPos);
            startPos += size;

            // 組番号
            size = GROUP_ID_SIZE;
            gId = decodeInt(encodedData, size, startPos);
            startPos += size;

            //CRC
            size = CRC_SIZE;
            crc = decodeInt(encodedData, size, MSG_FOR_TYPE_SIZE);

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
        /// <returns>
        /// デコード結果。結果の定義は、3.6章共通定義を参照
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#DEC_GETTYPE_NG}
        /// {@link EncDecConst#DEC_VAL_NG}
        /// </returns>
        public abstract int decode(Boolean paramCheck);

        /// <summary>
        /// 有効値パラメータチェック(エンコード用)
        /// Typeに依存しないパラメータはこのメソッドにチェック処理を実装する
        /// </summary>
        /// <param name="type">正解のType値</param>
        /// <returns>
        /// 有効値パラメータチェック結果
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#ENC_VAL_NG}
        /// </returns>
         protected int checkEncParam(int type) {
             int result = EncDecConst.OK;

             // グループID
             if (CommonUtils.isOutOfRange(gId, GROUP_ID_MIN, GROUP_ID_MAX))
             {
                 LogMng.AplLogError(TAG + "checkEncParam : groupId is invalid : groupId = "
                         + gId);
                 result = EncDecConst.ENC_VAL_NG;
             }

             // Type別パラメータ
             if (checkParam() != EncDecConst.OK)
             {
                 result = EncDecConst.ENC_VAL_NG;
             }

             // Type(範囲チェックはしない)
             if (msgType != type)
             {
                 LogMng.AplLogError(TAG + "checkEncParam : msgType is invalid : msgType = "
                                 + msgType);
                 result = EncDecConst.ENC_VAL_NG;
             }

             //crc
             if (CommonUtils.isOutOfRange(crc, CRC_MIN, CRC_MAX))
             {
                 LogMng.AplLogError(TAG + "checkEncParam : crc is invalid : crc = "
                         + gId);
                 result = EncDecConst.ENC_VAL_NG;
             }

             return result;
         }

        /// <summary>
        /// 有効値パラメータチェック
        /// </summary>
        /// <returns>
        /// 有効値パラメータチェック結果
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#DEC_GETTYPE_NG}
        /// {@link EncDecConst#DEC_VAL_NG}
        /// </returns>
        protected int checkDecParam(int type) {
            int result = EncDecConst.OK;

            // ユーザID
            if (CommonUtils.isOutOfRange(gId, GROUP_ID_MIN, GROUP_ID_MAX))
            {
                LogMng.AplLogError(TAG + "checkDecParam : gId is invalid : gId = "
                        + gId);
                result = EncDecConst.DEC_VAL_NG;
            }

            // Type別パラメータ
            if (checkParam() == EncDecConst.NG) {
                // checkParam()内でエラーログを出すのでここではログ出力不要
                result = EncDecConst.DEC_VAL_NG;
            }

            // Type(範囲チェックはしない)
            // DEC_GETTYPE_NGの判定
            if (msgType != type) {
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