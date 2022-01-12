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
    /// 報知テキスト
    /// </summary>
    public class MsgType150 : MsgToTerminal
    {
        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "MsgType150";

        /// <summary>
        /// このクラスのメッセージタイプ
        /// </summary>
        private const int MSG_TYPE = EncDecConst.VAL_MSG_TYPE150;

        // Type 別端末宛メッセージ
        /// <summary>
        /// 送信時刻。
        /// </summary>
        public TimeBCDMsec time = new TimeBCDMsec();

        /// <summary>
        /// メッセージ長。
        /// 範囲{@value #MSGLENGTH_MIN}～{@value #MSGLENGTH_MAX}。右詰め
        /// {@value #MSGLENGTH_SIZE}bitを使用する。
        /// </summary>
        public int msgLength;
        /// <summary>
        /// {@link #msgLength}の最小値
        /// </summary>
        public const int MSGLENGTH_MIN = 0;
        /// <summary>
        /// {@link #msgLength}の最大値
        /// </summary>
        public const int MSGLENGTH_MAX = 200;
        /// <summary>
        /// {@link #msgLength}の有効ビット数
        /// </summary>
        public const int MSGLENGTH_SIZE = 8;

        /// <summary>
        /// システムメッセージ。
        /// UTF-8で設定。UCS2への変換は、エンコード要求内で実施。{@value #MSG_CHAR_MIN}～
        /// {@value #MSG_CHAR_MAX}文字。
        /// </summary>
        public String msg = "";

        /// <summary>
        /// {@link #msg}の有効ビット数
        /// </summary>
        public const int MSG_SIZE = 3200;

        /// <summary>
        /// Padding。初期値は、0(仕様は0保証)とする。
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public int padding;

        /// <summary>
        /// Paddingの有効ビット数
        /// </summary>
        public const int PADDING_SIZE = 8;

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
        public MsgType150() {
            msgType = MSG_TYPE;
        }

        /// <summary>
        /// Type150エンコード要求
        /// 【制限事項】
        /// Type150エンコードクラスの以下のパラメータを、エンコード要求前に設定しておくこと。
        /// 設定する定義値については、3.6共通定義を参照
        /// {@link MsgToTerminal#sysInfo}
        /// {@link #msgLength}
        /// {@link #time}
        /// {@link #msg}
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
            int data;
            int size;

            // メッセージ長
            data = msgLength;
            size = MSGLENGTH_SIZE;
            encode(data, size, encodedData, startPos);
            startPos += size;

            // 送信時刻
            byte[] bcdData = time.getBCDData();
            size = bcdData.Length * BYTE_BIT_SIZE;
            encode(bcdData, size, encodedData, startPos);
            startPos += size;

            // システムメッセージ
            byte[] charBytes;
            if (msg != null && msg.Length != 0) {
                UnicodeEncoding encoding = new UnicodeEncoding(true, false);
                charBytes = encoding.GetBytes(msg);
                size = Math.Min(charBytes.Length * BYTE_BIT_SIZE, MSG_SIZE);
                encode(charBytes, size, encodedData, startPos);
                startPos += size;
            } else {
                size = 0;
            }
            size = MSG_SIZE - size;
            if (size > 0) {
                // 残りの領域を0で埋める
                charBytes = new byte[sizeToLength(size)];
                encode(charBytes, size, encodedData, startPos);
                startPos += size;
            }

            // Padding
            data = padding;
            size = PADDING_SIZE;
            encode(data, size, encodedData, startPos);
            startPos += size;

            //巡回冗長検査
            data = crc;
            size = CRC_SIZE;
            encode(data, size, encodedData, startPos);
            startPos += size;

            // エンコードサイズチェック
            if (startPos != SIZE)
            {
                return EncDecConst.ENC_SIZE_NG;
            }

            return EncDecConst.OK;
        }

        /// <summary>
        ///  Type150デコード要求
        /// 
        /// 【制限事項】
        /// Type150デコードクラスの以下のパラメータを、デコード要求前に設定しておくこと。
        /// {@link #encodedData}
        /// 
        /// 【備考】
        /// デコードデータは以下に格納。
        /// 設定する定義値については、3.6共通定義を参照
        /// {@link MsgToTerminal#sysInfo}
        /// {@link #time}
        /// {@link #msgLength}
        /// {@link #msg}
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
            startPos = decodeCommonField(sysInfoDecode);

            // メッセージ長
            size = MSGLENGTH_SIZE;
            msgLength = decodeInt(encodedData, size, startPos);
            startPos += size;

            // 送信時刻
            size = TimeBCDMsec.BCD_DATA_LENGTH * BYTE_BIT_SIZE;
            byte[] bcdData = new byte[TimeBCDMsec.BCD_DATA_LENGTH];
            decodeByteArray(encodedData, size, startPos, bcdData);
            time.setBCDData(bcdData);
            startPos += size;


            // システムメッセージ
            byte[] charBytes = new byte[msgLength * CommonConst.MSG_ENC_CHARSET_LENGTH];
            size = charBytes.Length * BYTE_BIT_SIZE;
            decodeByteArray(encodedData, size, startPos, charBytes);
            startPos += MSG_SIZE;
            // Stringに変換する
            UnicodeEncoding encoding = new UnicodeEncoding(true, false);
            msg = encoding.GetString(charBytes);

            // Padding
            size = PADDING_SIZE;
            padding = decodeInt(encodedData, size, startPos);
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


            // メッセージ長
            if (CommonUtils.isOutOfRange(msgLength, MSGLENGTH_MIN, MSGLENGTH_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : msgLength is invalid : msgLength = "
                                + msgLength);
                result = EncDecConst.NG;
            }

            // 送信時刻
            if (time.checkParam() == EncDecConst.NG)
            {
                result = EncDecConst.NG;
            }

            // システムメッセージ
            // msgLengthと一致するかチェックする
            int len = 0; // デコード後のテキストサイズ（Byte)
            if (msg != null)
            {
                len = msg.Length;
            }
            // msgがnull, empty時はlen=0となる
            if (len != msgLength)
            {
                LogMng.AplLogError(TAG + "checkParam : msg is invalid : msg size = "
                        + len + ", msgLength = " + msgLength);
                result = EncDecConst.NG;
            }

            // Padding
            if (padding != 0)
            {
                LogMng.AplLogError(TAG + "checkParam : padding is invalid : padding = "
                        + padding);
                result = EncDecConst.NG;
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
