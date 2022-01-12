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
    /// Type2：リソース確保要求
    /// 2.1.8 1 メッセージエンコード・デコード
    /// Type2エンコード・デコードクラス
    /// Type2メッセージのエンコードを実行する。
    /// Type2メッセージのデコードを実行する。
     /// </summary>
    public class MsgType2 : MsgFromTerminal
    {
        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "MsgType2";
        /// <summary>
        /// このクラスのメッセージタイプ
        /// </summary>
        private const int MSG_TYPE = EncDecConst.VAL_MSG_TYPE2;

	    // Type 別端末発メッセージ

	    /// <summary>
        /// メッセージ先頭フラグ。有効範囲は{@value #FIRSTMSGFLAG_MIN}～{@value #FIRSTMSGFLAG_MAX} 。
	    /// </summary>
        public Boolean firstMsgFlag;
	    /// <summary>
        /// {@link #firstMsgFlag}の最小値
	    /// </summary>
        public const int FIRSTMSGFLAG_MIN = 0;
	    /// <summary>
	    /// {@link #firstMsgFlag}の最大値
	    /// </summary>
        public const int FIRSTMSGFLAG_MAX = 1;
        /// <summary>
        /// {@link #firstMsgFlag}の有効ビット数
        /// </summary>
        public const int FIRSTMSGFLAG_SIZE = 1;

        /// <summary>
        /// メッセージ最終フラグ。有効範囲は{@value #FIRSTMSGFLAG_MIN}～{@value #FIRSTMSGFLAG_MAX} 。
        /// </summary>
        public Boolean lastMsgFlag;
        /// <summary>
        /// {@link #lastMsgFlag}の最小値
        /// </summary>
        public const int LASTMSGFLAG_MIN = 0;
        /// <summary>
        /// {@link #lastMsgFlag}の最大値
        /// </summary>
        public const int LASTMSGFLAG_MAX = 1;
        /// <summary>
        /// {@link #lastMsgFlag}の有効ビット数
        /// </summary>
        public const int LASTMSGFLAG_SIZE = 1;

	    /// <summary>
	    ///SeqNo。範囲{@value #SEQNUM_MIN}～{@value #SEQNUM_MAX}。
	    /// </summary>
        public int sequenceNumber;
	    /// <summary>
	    ///{@link #sequenceNumber}の最小値
	    /// </summary>
        public const int SEQNUM_MIN = 0;
	    /// <summary>
	    ///{@link #sequenceNumber}の最大値
	    /// </summary>
        public const int SEQNUM_MAX = 8;
	    /// <summary>
	    ///{@link #sequenceNumber}の有効ビット数
	    /// </summary>
        public const int SEQNUM_SIZE = 5;

        /// <summary>
        /// 分割避難所詳細情報
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public byte[] msg;

        /// <summary>
        /// 分割避難所詳細情報
        /// </summary>
        [System.Xml.Serialization.XmlElement("msg")]
        public string msgStr = "";

        /// <summary>
        ///{@link #msg}の有効ビット数
        /// </summary>
        public const int MSG_SIZE = 40;

	    /// <summary>
	    ///Reserve領域
	    /// </summary>
        [System.Xml.Serialization.XmlIgnore]
	    public int reserved1 = 0;
	    /// <summary>
	    ///{@link #reserved1}の有効ビット数
	    /// </summary>
        public const int RESERVED1_SIZE = 1;

	    /// <summary>
	    ///コンストラクタ
	    /// </summary>
	    public MsgType2() {
	        msgType = MSG_TYPE;
            msg = new byte[sizeToLength(MSG_SIZE)];
	    }

        /// <summary>
        /// 4.2.8.8 Type2エンコード要求
        /// 【制限事項】
        /// Type2エンコードクラスの以下のパラメータを、エンコード要求前に設定しておくこと。
        /// 設定する定義値については、3.6共通定義を参照
        /// {@link #groupId}
        /// {@link #firstMsgFlag}
        /// {@link #lastMsgFlag}
        /// {@link #sequenceNumber}
        /// {@link #msg}
        /// {@link #reserved1}
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

            // メッセージ先頭フラグ
            data = firstMsgFlag ? 1 : 0;
            size = FIRSTMSGFLAG_SIZE;
            encode(data, size, encodedData, startPos);
            startPos += size;

            // メッセージ終端フラグ
            data = lastMsgFlag ? 1 : 0;
            size = LASTMSGFLAG_SIZE;
            encode(data, size, encodedData, startPos);
            startPos += size;

	        // SeqNo
	        data = sequenceNumber;
	        size = SEQNUM_SIZE;
	        encode(data, size, encodedData, startPos);
	        startPos += size;

            //分割避難所詳細情報
            size = MSG_SIZE;
            encode(msg, size, encodedData, startPos);
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
        /// 4.2.8.9 Type2デコード要求
        /// 
        /// 【制限事項】
        /// Type2デコードクラスの以下のパラメータを、デコード要求前に設定しておくこと。
        /// {@link #encodedData}
        /// 
        /// 【備考】
        /// デコードデータは以下に格納。
        /// 設定する定義値については、3.6共通定義を参照
        /// {@link #groupId}
        /// {@link #firstMsgFlag}
        /// {@link #lastMsgFlag}
        /// {@link #sequenceNumber}
        /// {@link #msg}
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
        public override int decode(Boolean paramCheck) {
	        int size;
	        int startPos;

	        // 共通フィールドのデコード
	        startPos = decodeCommonField();

            // メッセージ先頭フラグ
	        size = FIRSTMSGFLAG_SIZE;
	        firstMsgFlag = (decodeInt(encodedData, size, startPos) == CommonConst.TRUE_VALUE);
	        startPos += size;

            // メッセージ終端フラグ
            size = LASTMSGFLAG_SIZE;
            lastMsgFlag = (decodeInt(encodedData, size, startPos) == CommonConst.TRUE_VALUE);
            startPos += size;

	        // SeqNo
	        size = SEQNUM_SIZE;
	        sequenceNumber = decodeInt(encodedData, size, startPos);
	        startPos += size;

            //分割避難所詳細情報
            size = MSG_SIZE;
            decodeByteArray(encodedData, size, startPos, msg);

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

            // 最終SeqNo
            if (CommonUtils.isOutOfRange(sequenceNumber, SEQNUM_MIN, SEQNUM_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : sequenceNumber is invalid : sequenceNumber = "
                                + sequenceNumber);
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
