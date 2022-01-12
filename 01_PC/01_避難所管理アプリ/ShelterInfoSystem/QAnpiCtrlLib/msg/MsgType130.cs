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
    /// 安否支援情報
    /// </summary>
    public class MsgType130 : MsgToTerminal
    {
        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "MsgType130";
        /// <summary>
        /// このクラスのメッセージタイプ
        /// </summary>
        private const int MSG_TYPE = EncDecConst.VAL_MSG_TYPE130;

        /// <summary>
        /// 救助支援情報
        /// </summary>
        public class RescueSupportInfo
        {
            //CID：通信ID
            /// <summary>
            /// CID：通信ID
            /// </summary>
            public int cid;
            /// <summary>
            /// CIDの最小値
            /// </summary>
            public const int CID_MIN = 0;
            /// <summary>
            /// CIDの最大値
            /// </summary>
            public const int CID_MAX = 33554431;
            /// <summary>
            /// CIDのサイズ(bit)
            /// </summary>
            public const int CID_SIZE = 25;

            //ORID：利用機関ID
            /// <summary>
            /// ORID：利用機関ID
            /// </summary>
            public int orid;
            /// <summary>
            /// ORIDの最小値
            /// </summary>
            public const int ORID_MIN = 0;
            /// <summary>
            /// ORIDの最大値
            /// </summary>
            public const int ORID_MAX = 16777215;
            /// <summary>
            /// ORIDのサイズ(bit)
            /// </summary>
            public const int ORID_SIZE = 24;

            //未受信メッセージ数
            /// <summary>
            /// 未受信メッセージ数
            /// </summary>
            public int unreceivedMsgNum;
            /// <summary>
            /// 未受信メッセージ数の最小値
            /// </summary>
            public const int UNRECEVED_MSG_NUM_MIN = 0;
            /// <summary>
            /// 未受信メッセージ数の最大値
            /// </summary>
            public const int UNRECEVED_MSG_NUM_MAX = 31;
            /// <summary>
            /// 未受信メッセージ数のサイズ(bit)
            /// </summary>
            public const int UNRECEVED_MSG_NUM_SIZE = 5;

            //未使用領域
            /// <summary>
            /// 未使用領域
            /// </summary>
            public int reserved;
            /// <summary>
            /// 未使用領域のサイズ(bit)
            /// </summary>
            public const int RESERVED_SIZE = 3;

            //救助支援情報ID
            /// <summary>
            /// 救助支援情報ID
            /// </summary>
            public int rescueSupportInfoId;
            /// <summary>
            /// 救助支援情報IDの最小値
            /// </summary>
            public const int RESCUE_SUPPORT_INFO_ID_MIN = 1;
            /// <summary>
            /// 救助支援情報IDの最大値
            /// </summary>
            public const int RESCUE_SUPPORT_INFO_ID_MAX = 63;
            /// <summary>
            /// 救助支援情報IDのサイズ(bit)
            /// </summary>
            public const int RESCUE_SUPPORT_INFO_ID_SIZE = 6;

            //データ種別
            /// <summary>
            /// データ種別
            /// </summary>
            public int dataType;
            /// <summary>
            /// データ種別の最小値
            /// </summary>
            public const int DATA_TYPE_MIN = 0;
            /// <summary>
            /// データ種別の最大値
            /// </summary>
            public const int DATA_TYPE_MAX = 1;
            /// <summary>
            /// データ種別のサイズ(bit)
            /// </summary>
            public const int DATA_TYPE_SIZE = 1;

            /// <summary>
            /// 受付時刻
            /// </summary>
            public TimeBCD time = new TimeBCD();

            /// <summary>
            /// メッセージ長
            /// </summary>
            public int msgLength;
            /// <summary>
            /// メッセージ長の最小値
            /// </summary>
            public const int MSG_LENGTH_MIN = 0;
            /// <summary>
            /// メッセージ長の最大値
            /// </summary>
            public const int MSG_LENGTH_MAX = 120;
            /// <summary>
            /// メッセージ長のサイズ(bit)
            /// </summary>
            public const int MSG_LENGTH_SIZE = 8;
            /// <summary>
            /// 救助支援テキスト
            /// </summary>
            public String msg = "";
            /// <summary>
            /// 救助支援情報バイナリ
            /// </summary>
            public Byte[] msgbin;
            /// <summary>
            /// 救助支援テキストのサイズ(bit)
            /// </summary>
            public const int MSG_SIZE = 944;
            /// <summary>
            /// 避難所管理ID
            /// </summary>
            public int smid;
            /// <summary>
            /// 避難所管理IDのサイズ(bit)
            /// </summary>
            public const int MSG_SMID_SIZE = 8;
            /// <summary>
            /// Reservedのサイズ
            /// </summary>
            public const int MSG_RESERVED_SIZE = 8;

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
                int res = EncDecConst.OK;

                //通信ID
                if (CommonUtils.isOutOfRange(cid, CID_MIN, CID_MAX))
                {
                    LogMng.AplLogError(TAG + "checkParam : cid is invalid : cid = "
                            + cid);
                    res = EncDecConst.NG;
                }

                //利用機関ID
                if (CommonUtils.isOutOfRange(orid, ORID_MIN, ORID_MAX))
                {
                    LogMng.AplLogError(TAG + "checkParam : orid is invalid : orid = "
                            + orid);
                    res = EncDecConst.NG;
                }

                //未受信メッセージ数
                if (CommonUtils.isOutOfRange(unreceivedMsgNum, UNRECEVED_MSG_NUM_MIN, UNRECEVED_MSG_NUM_MAX))
                {
                    LogMng.AplLogError(TAG + "checkParam : unreceivedMsgNum is invalid : unreceivedMsgNum = "
                            + unreceivedMsgNum);
                    res = EncDecConst.NG;
                }

                //救助支援情報ID
                if (CommonUtils.isOutOfRange(rescueSupportInfoId, RESCUE_SUPPORT_INFO_ID_MIN, RESCUE_SUPPORT_INFO_ID_MAX))
                {
                    LogMng.AplLogError(TAG + "checkParam : rescueSupportInfoId is invalid : rescueSupportInfoId = "
                            + rescueSupportInfoId);
                    res = EncDecConst.NG;
                }

                //データ種別
                if (CommonUtils.isOutOfRange(dataType, DATA_TYPE_MIN, DATA_TYPE_MAX))
                {
                    LogMng.AplLogError(TAG + "checkParam : dataType is invalid : dataType = "
                            + dataType);
                    res = EncDecConst.NG;
                }

                //受付時刻
                if(time.checkParam() == EncDecConst.NG)
                {
                    res = EncDecConst.NG;
                }

                //メッセージ長
                if (CommonUtils.isOutOfRange(msgLength, MSG_LENGTH_MIN, MSG_LENGTH_MAX))
                {
                    LogMng.AplLogError(TAG + "checkParam : msgLength is invalid : msgLength = "
                            + msgLength);
                    res = EncDecConst.NG;
                }

                return res;
            }
        }

        /// <summary>
        /// 救助支援情報数
        /// </summary>
        public int rescueSupportInfoNum;
        /// <summary>
        /// 救助支援情報数の最小値
        /// </summary>
        public const int RESCUE_SUPPORT_INFO_NUM_MIN = 1;
        /// <summary>
        /// 救助支援情報数の最大値
        /// </summary>
        public const int RESCUE_SUPPORT_INFO_NUM_MAX = 3;
        /// <summary>
        /// 救助支援情報数のサイズ(bit)
        /// </summary>
        public const int RESCUE_SUPPORT_INFO_NUM_SIZE = 8;

        /// <summary>
        /// 救助支援情報配列
        /// </summary>
        public RescueSupportInfo[] rescueSupportInfos;

        /// <summary>
        /// Padding。有効bitは右詰{@value #PADDING_SIZE}bitとなる
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public byte[] padding;
        /// <summary>
        /// {@link #padding}の有効ビット数
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
	    public MsgType130() {
            msgType = MSG_TYPE;
            rescueSupportInfos = new RescueSupportInfo[RESCUE_SUPPORT_INFO_NUM_MAX];
            for (int i = 0; i < RESCUE_SUPPORT_INFO_NUM_MAX; i++ )
            {
                rescueSupportInfos[i] = new RescueSupportInfo();
            }
            padding = new byte[sizeToLength(PADDING_SIZE)];
	    }

        	    /// <summary>
        /// Type130エンコード要求
        /// 【制限事項】
        /// Type130エンコードクラスの以下のパラメータを、エンコード要求前に設定しておくこと。
        /// 設定する定義値については、3.6共通定義を参照
        /// {@link MsgToTerminal#sysInfo}
        /// {@link #rescueSupportInfoNum}
        /// {@link #rescueSupportInfos}
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
        public override int encode(Boolean paramCheck)
        {
            // 有効値パラメータチェック
            if (paramCheck)
            {
                int result = checkEncParam(MSG_TYPE);
                if (result != EncDecConst.OK)
                {
                    return result;
                }
            }

            // 共通フィールドのエンコード
            int startPos = encodeCommonField();
            int data;
            int size;

            //救助支援情報数
            data = rescueSupportInfoNum;
            size = RESCUE_SUPPORT_INFO_NUM_SIZE;
            encode(data, size, encodedData, startPos);
            startPos += size;

            //救助支援情報
            for(int i = 0; i < RESCUE_SUPPORT_INFO_NUM_MAX; i++)
            {
                size = encode(rescueSupportInfos[i], startPos);
                startPos += size;
            }

            //未使用領域
            size = PADDING_SIZE;
            encode(padding, size, encodedData, startPos);
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
        /// 救助支援情報のエンコード
        /// </summary>
        /// <param name="rsi">救助支援情報</param>
        /// <param name="startPos">エンコード開始位置（ビット数）</param>
        /// <returns>エンコードしたビット数</returns>
        int encode(RescueSupportInfo rsi, int startPos)
        {
            int pos = startPos;
            int data;
            int size;

            //通信ID
            data = rsi.cid;
            size = RescueSupportInfo.CID_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            //利用機関ID
            data = rsi.orid;
            size = RescueSupportInfo.ORID_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            //未受信メッセージ数
            data = rsi.unreceivedMsgNum;
            size = RescueSupportInfo.UNRECEVED_MSG_NUM_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            //未使用領域
            data = rsi.reserved;
            size = RescueSupportInfo.RESERVED_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            //救助支援情報ID
            data = rsi.rescueSupportInfoId;
            size = RescueSupportInfo.RESCUE_SUPPORT_INFO_ID_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            //データ種別
            data = rsi.dataType;
            size = RescueSupportInfo.DATA_TYPE_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            //受付時刻
            byte[] bcdData = rsi.time.getBCDData();
            size = bcdData.Length * BYTE_BIT_SIZE;
            encode(bcdData, size, encodedData, pos);
            pos += size;

            //メッセージ長
            data = rsi.msgLength;
            size = RescueSupportInfo.MSG_LENGTH_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            //メッセージ
            // ユーザメッセージ
            byte[] charBytes = null;
            if (rsi.dataType == 0)
            {
                // バイナリ情報
                if (rsi.msgbin != null && rsi.msgbin.Length != 0)
                {
                    charBytes = rsi.msgbin;
                }
            }
            else if (rsi.dataType == 1)
            {
                // テキスト情報
                if (rsi.msg != null && rsi.msg.Length != 0)
                {
                    UnicodeEncoding encoding = new UnicodeEncoding(true, false);
                    charBytes = encoding.GetBytes(rsi.msg);
                }

            }
            else
            {
                // N/A
            }

            if (charBytes != null)
            {
                size = Math.Min(charBytes.Length * BYTE_BIT_SIZE, RescueSupportInfo.MSG_SIZE);
                encode(charBytes, size, encodedData, pos);
                pos += size;
            }
            else
            {
                size = 0;
            }

            size = RescueSupportInfo.MSG_SIZE - size;
            if (size > 0)
            {
                // 残りの領域を0で埋める
                charBytes = new byte[sizeToLength(size)];
                encode(charBytes, size, encodedData, pos);
                pos += size;
            }

            return pos - startPos;
        }

        /// <summary>
        ///  Type130デコード要求
        /// 【制限事項】
        /// Type130デコードクラスの以下のパラメータを、デコード要求前に設定しておくこと。
        /// {@link #encodedData}
        ///
        /// 【備考】
        /// デコードデータは以下に格納。
        /// 設定する定義値については、3.6共通定義を参照
        /// {@link MsgToTerminal#sysInfo}
        /// {@link #rescueSupportInfoNum}
        /// {@link #rescueSupportInfos}
        /// {@link #padding}
        /// {@link #crc}
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
        public override int decode(Boolean paramCheck, Boolean sysInfoDecode)
        {
            int size;
            int startPos;

            // 共通フィールドのデコード
            startPos = decodeCommonField(sysInfoDecode);

            //救助支援情報数
            size = RESCUE_SUPPORT_INFO_NUM_SIZE;
            rescueSupportInfoNum = decodeInt(encodedData, size, startPos);
            startPos += size;

            //救助支援情報
            for (int i = 0; i < RESCUE_SUPPORT_INFO_NUM_MAX; i++ )
            {
                size = decode(rescueSupportInfos[i], startPos);
                startPos += size;
            }

            //未使用領域
            size = PADDING_SIZE;
            decodeByteArray(encodedData, size, startPos, padding);
            startPos += size;

            //crc
            size = CRC_SIZE;
            crc = decodeInt(encodedData, size, startPos);
            startPos += size;


            // 有効値パラメータチェック
            if (paramCheck)
            {
                int result = checkDecParam(sysInfoDecode, MSG_TYPE);
                if (result != EncDecConst.OK)
                {
                    return result;
                }
            }
            return EncDecConst.OK;
        }

        /// <summary>
        /// RescueSupportInfoデコード要求
        /// </summary>
        /// <param name="rsi">データ格納用RescueSupportInfo</param>
        /// <param name="startPos">コード開始位置（ビット数）</param>
        /// <returns>デコードしたビット数</returns>
        private int decode(RescueSupportInfo rsi, int startPos)
        {
            int pos = startPos;
            int size;

            //通信ID
            size = RescueSupportInfo.CID_SIZE;
            rsi.cid = decodeInt(encodedData, size, pos);
            pos += size;

            //利用機関ID
            size = RescueSupportInfo.ORID_SIZE;
            rsi.orid = decodeInt(encodedData, size, pos);
            pos += size;

            //未受信メッセージ数
            size = RescueSupportInfo.UNRECEVED_MSG_NUM_SIZE;
            rsi.unreceivedMsgNum = decodeInt(encodedData, size, pos);
            pos += size;

            //未使用領域
            size = RescueSupportInfo.RESERVED_SIZE;
            rsi.reserved = decodeInt(encodedData, size, pos);
            pos += size;

            //救助支援情報ID
            size = RescueSupportInfo.RESCUE_SUPPORT_INFO_ID_SIZE;
            rsi.rescueSupportInfoId = decodeInt(encodedData, size, pos);
            pos += size;

            //データ種別
            size = RescueSupportInfo.DATA_TYPE_SIZE;
            rsi.dataType = decodeInt(encodedData, size, pos);
            pos += size;

            //受付時刻
            size = TimeBCD.BCD_DATA_LENGTH * BYTE_BIT_SIZE;
            byte[] bcdData = new byte[TimeBCD.BCD_DATA_LENGTH];
            decodeByteArray(encodedData, size, pos, bcdData);
            rsi.time.setBCDData(bcdData);
            pos += size;

            //メッセージ長
            size = RescueSupportInfo.MSG_LENGTH_SIZE;
            rsi.msgLength = decodeInt(encodedData, size, pos);
            pos += size;

            //救助支援テキスト
            // ユーザメッセージ
            rsi.msgbin = new byte[rsi.msgLength];// * CommonConst.MSG_ENC_CHARSET_LENGTH]; // 20170525
            size = rsi.msgbin.Length * BYTE_BIT_SIZE;
            decodeByteArray(encodedData, size, pos, rsi.msgbin);
            pos += RescueSupportInfo.MSG_SIZE;
            UnicodeEncoding encoding = new UnicodeEncoding(true, false);
            string tmpEncodeMes = encoding.GetString(rsi.msgbin);
            tmpEncodeMes = tmpEncodeMes.Replace("\0", "");
            rsi.msg = tmpEncodeMes;

            // 避難所管理ID
            size = RescueSupportInfo.MSG_SMID_SIZE;
            rsi.smid = decodeInt(encodedData, size, pos);
            pos += size;

            // Reserved
            size = RescueSupportInfo.MSG_RESERVED_SIZE;
            pos += size;
            
            return pos - startPos;

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
        protected override int checkParam()
        {
            int result = EncDecConst.OK;

            //救助支援情報数
            int num = rescueSupportInfoNum;
            if (CommonUtils.isOutOfRange(rescueSupportInfoNum, RESCUE_SUPPORT_INFO_NUM_MIN, RESCUE_SUPPORT_INFO_NUM_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : rescueSupportInfoNum is invalid : rescueSupportInfoNum = "
                                + rescueSupportInfoNum);
                result = EncDecConst.NG;
                num = 0;
            }

            //救助支援情報
            for (int i = 0; i < num; i++ )
            {
                if(rescueSupportInfos[i].checkParam() == EncDecConst.NG)
                {
                    //logはNGになった場所で出るので出さない
                    result = EncDecConst.NG;
                    break;
                }
            }

            // Padding
            for (int i = 0; i < padding.Length; i++)
            {
                if (padding[i] != 0)
                {
                    LogMng.AplLogError(TAG + "checkParam : padding is invalid : padding = "
                            + padding);
                    result = EncDecConst.NG;
                    break;
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
