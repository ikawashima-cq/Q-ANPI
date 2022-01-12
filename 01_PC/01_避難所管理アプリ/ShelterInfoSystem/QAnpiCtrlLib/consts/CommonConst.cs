using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAnpiCtrlLib.consts
{
    /// <summary>
    /// 制御ツール内共通　定数定義用クラス
    /// </summary>
    public class CommonConst
    {
        /// <summary>
        /// 定数定義（値：０）
        /// </summary>
        public const int INT_VALUE_0 = 0;
        /// <summary>
        /// 定数定義（値：１）
        /// </summary>
        public const int INT_VALUE_1 = 1;
        /// <summary>
        /// 定数定義（値：２）
        /// </summary>
        public const int INT_VALUE_2 = 2;
        /// <summary>
        /// 定数定義（値：３）
        /// </summary>
        public const int INT_VALUE_3 = 3;
        /// <summary>
        /// 定数定義（値：４）
        /// </summary>
        public const int INT_VALUE_4 = 4;
        /// <summary>
        /// 定数定義（値：５）
        /// </summary>
        public const int INT_VALUE_5 = 5;
        /// <summary>
        /// 定数定義（値：６）
        /// </summary>
        public const int INT_VALUE_6 = 6;
        /// <summary>
        /// 定数定義（値：７）
        /// </summary>
        public const int INT_VALUE_7 = 7;
        /// <summary>
        /// 定数定義（値：８）
        /// </summary>
        public const int INT_VALUE_8 = 8;
        /// <summary>
        /// 定数定義（値：９）
        /// </summary>
        public const int INT_VALUE_9 = 9;
        /// <summary>
        /// 定数定義（値：Null）
        /// </summary>
        public const string STR_VALUER_NULL = null;
        /// <summary>
        /// 定数定義（値：0）
        /// </summary>
        public const string STR_VALUER_0 = "0";
        /// <summary>
        /// 定数定義（値：1）
        /// </summary>
        public const string STR_VALUER_1 = "1";

        /// <summary>
        /// メソッドリターン値（ＯＫ）
        /// </summary>
        public const int RET_OK = 0;
        /// <summary>
        /// メソッドリターン値（ＮＧ）
        /// </summary>
        public const int RET_NG = -1;

        /// <summary>
        /// 設定可能な周波数の最小値
        /// </summary>
        public const int MIN_FREQUENCY = 0;
        /// <summary>
        /// 設定可能な周波数の最大値
        /// </summary>
        public const int MAX_FREQUENCY = 13;
        /// <summary>
        /// 端末宛てメッセージの送信開始タイミングに設定可能な最小値
        /// </summary>
        public const int MIN_SEND_STARTTIME = 0;
        /// <summary>
        /// 端末宛てメッセージの送信開始タイミングに設定可能な最大値
        /// </summary>
        public const int MAX_SEND_STARTTIME = 300;
        /// <summary>
        /// 送信開始タイミングの要素名の初期値
        /// </summary>
        public const string DEFAULT_SENDSTARTTIME = "0";
        /// <summary>
        /// ユーザＩＤの初期値
        /// </summary>
        public const string DEFAULT_USERID = "000000001";
        /// <summary>
        /// メッセージ送信(sendMsg)の端末宛メッセージ送信用第二引数（送信周波数）
        /// </summary>
        public const int FREQ_TO_TERMINAL = 0;
        /// <summary>
        /// メッセージ送信(sendMsg)の端末宛メッセージ送信用第三引数（送信ＰＮ符号）
        /// </summary>
        public const int PN_TO_TERMINAL = 0;
        /// <summary>
        /// アクションモード（保存）の定義
        /// </summary>
        public const String ACT_SAVE = "Save";
        /// <summary>
        /// アクションモード（読出）の定義
        /// </summary>
        public const String ACT_LOAD = "Load";
        /// <summary>
        /// アクションモード（送信）の定義
        /// </summary>
        public const String ACT_SEND = "Send";
        /// <summary>
        /// アクションモード（クリア）の定義
        /// </summary>
        public const String ACT_CLEAR = "Clear";

        /// <summary>
        /// 系番号
        /// </summary>
        public enum SystemNumber
        {
            /// <summary>
            /// 系未設置
            /// </summary>
            SYSTEMNONE,
            /// <summary>
            /// 1系
            /// </summary>
            SYSTEM1,
            /// <summary>
            /// 2系
            /// </summary>
            SYSTEM2,
            /// <summary>
            /// 1系、2系両方
            /// </summary>
            SYSTEM1_2
        };

        /// <summary>
        /// 装置ステータス用のポート番号
        /// </summary>
        public const int EQU_STATUS_PORT = 3005;

        /// <summary>
        /// FWDデータ用のポート番号
        /// </summary>
        public const int FWD_DATA_PORT = 3090;

        /// <summary>
        /// RTNデータ用のポート番号１
        /// </summary>
        public const int RTN_DATA_PORT_1 = 3091;
        /// <summary>
        /// RTNデータ用のポート番号２
        /// </summary>
        public const int RTN_DATA_PORT_2 = 3092;
        /// <summary>
        /// RTNデータ用のポート番号３
        /// </summary>
        public const int RTN_DATA_PORT_3 = 3093;
        /// <summary>
        /// RTNデータ用のポート番号４
        /// </summary>
        public const int RTN_DATA_PORT_4 = 3094;
// STEP2
        /// <summary>
        /// L1Sデータ用のポート番号
        /// </summary>
        public const int L1S_DATA_PORT = 3610;

        /// <summary>
        /// S帯モニタデータ　開始コード
        /// </summary>
        public const int SBAND_MSG_START_CODE =  1234567890;
        /// <summary>
        /// S帯モニタデータ　終了コード
        /// </summary>
        public const int SBAND_MSG_END_CODE = -1234567890;

        /// <summary>
        /// tickの秒表現　1.6秒
        /// </summary>
        public const double TICK_MILLISEC = 1600;

        /// <summary>
        /// crcの16サイズ(bit)
        /// </summary>
        public const int CRC16_SIZE = 16;

        /// <summary>
        /// 1bit　シフト後の先頭位置
        /// </summary>
        public const int ONE_BIT_SHIFT_TOP = 1;

        /// <summary>
        /// int ＜－＞ boolean 変換用
        /// </summary>
        public const int TRUE_VALUE = 1;

        /// <summary>
        /// int ＜－＞ boolean 変換用
        /// </summary>
        public const int FALSE_VALUE = 0;

        /// <summary>
        /// {@link #msg}のエンコード時の文字コード1文字の有効バイト数
        /// </summary>
        public const int MSG_ENC_CHARSET_LENGTH = 2;

        /// <summary>
        /// double型のバイト長
        /// </summary>
        public const int DOUBLE_BYTE_LENGTH = 8;

        /// <summary>
        /// 周波数の最小値
        /// </summary>
        public const int FREQ_MIN = 0;

        /// <summary>
        /// 周波数の最大値
        /// </summary>
        public const int FREQ_MAX = 13;

        /// <summary>
        /// PN符号の最小値
        /// </summary>
        public const int PN_MIN = 0;

        /// <summary>
        /// PN符号の最大値
        /// </summary>
        public const int PN_MAX = 399;

// STEP2
        /// <summary>
        /// 衛星通信端末の識別子の最小値
        /// </summary>
        public const int CID_MIN = 0;

        /// <summary>
        /// 衛星通信端末の識別子の最大値
        /// </summary>
        public const int CID_MAX = 0x1FFFFFF;





    }
}
