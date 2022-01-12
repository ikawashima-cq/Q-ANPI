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
    /// 航法メッセージ情報
    /// </summary>
    public class MsgType200 : MsgToTerminal
    {
        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "MsgType200";
        /// <summary>
        /// このクラスのメッセージタイプ
        /// </summary>
        private const int MSG_TYPE = EncDecConst.VAL_MSG_TYPE200;

	    // Type 別端末宛メッセージ
	    /// <summary>
	    /// エフェメリス情報。
	    /// 詳細パラメータは対応しない。
	    /// </summary>
        [System.Xml.Serialization.XmlIgnore]
	    public byte[] info;
	    /// <summary>
	    /// {@link #info}の有効ビット数
	    /// </summary>
	    public const int INFO_SIZE = 8 + (584 * 5) + 192;
	    /// <summary>
	    /// {@link #info}の有効バイト数
	    /// </summary>
	    public static readonly int INFO_LENGTH = sizeToLength(INFO_SIZE);

        /// <summary>
        /// エフェメリス情報
        /// XML出力用
        /// </summary>
        [System.Xml.Serialization.XmlElement("info")]
        public string infoStr = "";

	    /// <summary>
	    /// Padding。初期値は、0(仕様は0保証)とする。
	    /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public byte[] padding;
	    /// <summary>
	    /// {@value #padding}の有効ビット数
	    /// </summary>
	    public const int PADDING_SIZE = 160;
	    /// <summary>
	    /// {@value #padding}の有効バイト数
	    /// </summary>
        public static readonly int PADDING_LENGTH = sizeToLength(PADDING_SIZE);

        /// <summary>
        /// 巡回冗長検査
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
	    /// コンストラクタ
	    /// </summary>
	    public MsgType200() {
            msgType = MSG_TYPE;
	        info = new byte[INFO_LENGTH];
	        padding = new byte[PADDING_LENGTH];
	    }

	    /// <summary>
        /// Type200エンコード要求
        /// 【制限事項】
        /// Type200エンコードクラスの以下のパラメータを、エンコード要求前に設定しておくこと。
        /// 設定する定義値については、3.6共通定義を参照
        /// {@link MsgToTerminal#sysInfo}
        /// {@link #info}
        /// {@link #padding}
        /// {@link #crc}
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

	        // エフェメリス情報
	        size = INFO_SIZE;
	        encode(info, size, encodedData, startPos);
	        startPos += size;

	        // Padding
	        size = PADDING_SIZE;
	        encode(padding, size, encodedData, startPos);
	        startPos += size;

            //巡回冗長検査
            size = CRC_SIZE;
            encode(crc, size, encodedData, startPos);
            startPos += size;

            // エンコードサイズチェック
            if (startPos != SIZE)
            {
                return EncDecConst.ENC_SIZE_NG;
            }

	        return EncDecConst.OK;
	    }

	    /// <summary>
        /// Type200デコード要求
        /// 
        /// 【制限事項】
        /// Type200デコードクラスの以下のパラメータを、デコード要求前に設定しておくこと。
        /// {@link #encodedData}
        /// 
        /// 【備考】
        /// デコードデータは以下に格納。
        /// 設定する定義値については、3.6共通定義を参照
        /// {@link MsgToTerminal#sysInfo}
        /// {@link #info}
        /// {@link #paddpadding}
        /// {@link #crc}
        /// 
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
	        startPos = decodeCommonField(sysInfoDecode);

	        // エフェメリス情報
	        size = INFO_SIZE;
	        decodeByteArray(encodedData, size, startPos, info);
	        startPos += size;

	        // Padding
	        size = PADDING_SIZE;
	        decodeByteArray(encodedData, size, startPos, padding);
	        startPos += size;

            //crc
            size = CRC_SIZE;
            crc = decodeInt(encodedData, size, startPos);
            startPos += size;

	        // 有効値パラメータチェック
	        if (paramCheck) {
                int result = checkDecParam(sysInfoDecode, MSG_TYPE);
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

            // エフェメリス情報(not check)

            // Padding
            int num = padding.Length;
            for (int i = 0; i < num; i++)
            {
                if (padding[i] != 0)
                {
                    LogMng.AplLogError(TAG +
                            "checkParam : padding[] is invalid : padding[" + i
                                    + "] = " + padding[i]);
                    result = EncDecConst.NG;
                }
            }

            //巡回冗長検査
            if (CommonUtils.isOutOfRange(crc, CRC_MIN, CRC_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : crc is invalid : crc = "
                                + crc);
                result = EncDecConst.NG;
            }

	        return result;
	    }
    }
}
