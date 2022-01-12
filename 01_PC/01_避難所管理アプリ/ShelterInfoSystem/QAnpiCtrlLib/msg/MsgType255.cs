using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QAnpiCtrlLib.consts;
using QAnpiCtrlLib.log;

namespace QAnpiCtrlLib.msg
{

	/// <summary>
	/// Type255：エンプティ
	/// 2.1.8 1 メッセージエンコード・デコード
	/// Type255エンコード・デコードクラス
	/// Type255メッセージのエンコードを実行する。
	/// Type255メッセージのデコードを実行する。
	/// </summary>
	public class MsgType255 : MsgToTerminal {
        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
	    private const String TAG = "MsgType255";
	    /// <summary> 
        /// このクラスのメッセージタイプ
        /// </summary>
	    private const int MSG_TYPE = EncDecConst.VAL_MSG_TYPE255;

	    // Type 別端末宛メッセージ
	    /// <summary>
	    /// Padding。初期値は、0(仕様は0保証)とする。
	    /// </summary>
        [System.Xml.Serialization.XmlIgnore]
	    public byte[] padding;
	    /// <summary>
	    /// {@value #padding}の有効ビット数
	    /// </summary>
	    public const int PADDING_SIZE = 3432;
	    /// <summary>
	    /// {@value #padding}の有効バイト数
	    /// </summary>
	    public readonly int PADDING_LENGTH = sizeToLength(PADDING_SIZE);

	    /// <summary>
	    /// コンストラクタ
	    /// </summary>
	    public MsgType255() {
	        msgType = MSG_TYPE;
	        padding = new byte[PADDING_LENGTH];
	    }

	    /// <summary>
	    /// Type255エンコード要求
	    /// 【制限事項】
	    /// Type255エンコードクラスの以下のパラメータを、エンコード要求前に設定しておくこと。
	    /// 設定する定義値については、3.6共通定義を参照
	    /// {@link #padding}
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
	        int size;

	        // Padding
	        size = PADDING_SIZE;
	        encode(padding, size, encodedData, startPos);
	        startPos += size;

	        // エンコードサイズチェック
	        if (startPos != SIZE) {
	            return EncDecConst.ENC_SIZE_NG;
	        }

	        return EncDecConst.OK;
	    }

	    /// <summary>
	    /// Type255デコード要求
	    /// 
	    /// 【制限事項】
	    /// Type255デコードクラスの以下のパラメータを、デコード要求前に設定しておくこと。
	    /// {@link #encodedData}
	    /// 
	    /// 【備考】
	    /// デコードデータは以下に格納。
	    /// 設定する定義値については、3.6共通定義を参照
	    /// {@link #padding}
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
	    public override int decode(Boolean paramCheck, Boolean sysInfoDecode) {
	        int size;
	        int startPos;

	        // 共通フィールドのデコード
	        startPos = decodeCommonField(false);

	        // Padding
	        size = PADDING_SIZE;
	        decodeByteArray(encodedData, size, startPos, padding);
	        startPos += size;

	        // 有効値パラメータチェック
	        if (paramCheck) {
	            int result = checkDecParam(false, MSG_TYPE);
	            if (result != EncDecConst.OK) {
	                return result;
	            }
	        }
	        return EncDecConst.OK;
	    }

	    /// <summary>
	    /// 共通部のSystemInfoを除外するためOverrideする
	    /// </summary>
        /// <returns>
        /// デコードしたビット数
        /// </returns>
	    public override int encodeCommonField() {
	        int data;
	        int size;
	        int startPos = 0;

	        // Type
	        data = msgType;
	        size = MSG_TYPE_SIZE;
	        encode(data, size, encodedData, startPos);
	        startPos += size;

	        return startPos;
	    }

	    /// <summary>
	    /// 共通部のSystemInfoを除外するためOverrideする
	    /// </summary>
        /// <param name="sysInfoDecode">
        /// true：システム情報デコード実施
        /// false：システム情報デコード未実施
        /// </param>
        /// <returns>デコードしたビット数</returns>
	    public override int decodeCommonField(Boolean sysInfoDecode) {
	        int startPos = 0;

	        // Typeのみ読み出し
	        TypeAndSystemInfo ts =
	                decodeTypeAndSystemInfo(encodedData, SIZE, false);

	        // Type
	        msgType = ts.msgType;
	        startPos += MSG_TYPE_SIZE;

	        return startPos;
	    }

	    /// <summary>
	    /// 共通部のSystemInfoを除外するためOverrideする
	    /// </summary>
        /// <param name="type">正解のType値</param>
        /// <returns>
        /// 有効値パラメータチェック結果
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#ENC_VAL_NG}
        /// {@link EncDecConst#ENC_SIZE_NG}
        /// </returns>
	    protected override int checkEncParam(int type) {
	        int result = EncDecConst.OK;

	        if (checkParam() == EncDecConst.NG) {
	            result = EncDecConst.ENC_VAL_NG;
	        }

	        if (msgType != type) {
	            LogMng.AplLogError(TAG + "checkEncParam : msgType is invalid : msgType = "
	                            + msgType);
	            result = EncDecConst.ENC_VAL_NG;
	        }

	        return result;
	    }

	    /// <summary>
	    /// 共通部のSystemInfoを除外するためOverrideする
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
	    protected override int checkDecParam(Boolean sysInfoCheck, int type) {
	        int result = EncDecConst.OK;

	        if (checkParam() == EncDecConst.NG) {
	            result = EncDecConst.DEC_VAL_NG;
	        }

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
	    protected override int checkParam() {
	        int result = EncDecConst.OK;

	        // Padding
	        int num = padding.Length;
	        for (int i = 0; i < num; i++) {
	            if (padding[i] != 0) {
	                LogMng.AplLogError(TAG +
	                        "checkParam : padding[] is invalid : padding[" + i
	                                + "] = " + padding[i]);
	                result = EncDecConst.NG;
	            }
	        }

	        return result;
	    }
	}
}
