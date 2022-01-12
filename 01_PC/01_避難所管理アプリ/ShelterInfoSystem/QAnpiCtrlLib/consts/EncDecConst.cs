/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAnpiCtrlLib.consts
{
    /// <summary>
    /// 3.6.5 1 エンコード・デコード用結果定義
    /// 3.6.6 1 エンコード・デコード用メッセージType定義
    /// 3.6.7 1 エンコード・デコード用システム状態定義
    /// 3.6.8 1 エンコード・デコード用スロット算出モード定義
    /// 3.6.9 1 エンコード・デコード用安否情報：救助要否定義
    /// 3.6.10 1 エンコード・デコード用安否情報：体調定義
    /// 3.6.11 1 エンコード・デコード用安否情報：現在地定義
    /// 3.6.12 1 エンコード・デコード用安否情報：同行者定義
    /// 3.6.13 1 エンコード・デコード用安否情報：移動定義
    /// 3.6.14 1 エンコード・デコード用安否情報：事故状況定義
    /// 3.6.15 1 エンコード・デコード用安否情報：退避状況定義
    /// </summary>
    public class EncDecConst
    {
        // 3.6.5 1 エンコード・デコード用結果定義
        /// <summary>
        ///OK
        /// </summary>
        public const int OK = 0;
        /// <summary>
        /// NG
        /// </summary>
        public const int NG = -1;
        /// <summary>
        ///エンコード・有効値チェックNG
        /// </summary>
        public const int ENC_VAL_NG = -1;
        /// <summary>
        ///エンコード・有効bit数範囲外NG
        /// </summary>
        public const int ENC_SIZE_NG = -2;
        /// <summary>
        ///メッセージType取得NG
        /// </summary>
        public const int DEC_GETTYPE_NG = -3;
        /// <summary>
        ///デコード・有効値チェックNG
        /// </summary>
        public const int DEC_VAL_NG = -4;

        // 3.6.6 1 エンコード・デコード用メッセージType定義
        /// <summary>
        ///Type0メッセージ
        /// </summary>
        public const int VAL_MSG_TYPE0 = 0;
        /// <summary>
        ///Type1メッセージ
        /// </summary>
        public const int VAL_MSG_TYPE1 = 1;
        /// <summary>
        ///Type2メッセージ
        /// </summary>
        public const int VAL_MSG_TYPE2 = 2;
        /// <summary>
        ///Type3メッセージ
        /// </summary>
        public const int VAL_MSG_TYPE3 = 3;
        /// <summary>
        ///Type100メッセージ
        /// </summary>
        public const int VAL_MSG_TYPE100 = 100;
        /// <summary>
        ///Type130メッセージ
        /// </summary>
        public const int VAL_MSG_TYPE130 = 130;
        /// <summary>
        ///Type150メッセージ
        /// </summary>
        public const int VAL_MSG_TYPE150 = 150;
        /// <summary>
        /// Type255メッセージ
        /// </summary>
        public const int VAL_MSG_TYPE255 = 255;

        // 3.6.7 1 エンコード・デコード用システム状態定義
        /// <summary>
        ///通常稼働
        /// </summary>
        public const int SYS_STATE_NOMAL = 1;
        /// <summary>
        ///メンテナンス中
        /// </summary>
        public const int SYS_STATE_MAINTENANCE = 2;

        // 3.6.8 1 エンコード・デコード用スロット算出モード定義
        /// <summary>
        ///スロット算出モード：ランダムモード
        /// </summary>
        public const int SYS_SCMODE_RAND = 1;
        /// <summary>
        ///スロット算出モード：ユーザIDベースモード
        /// </summary>
        public const int SYS_SCMODE_USERID = 2;

        // 3.6.9 1 エンコード・デコード用安否情報：救助要否定義
        /// <summary>
        ///救助・迎え不要
        /// </summary>
        public const int ANPI_INF_HELP_NO = 0;
        /// <summary>
        ///家族・同僚の迎え依頼
        /// </summary>
        public const int ANPI_INF_HELP_FAMILY = 1;
        /// <summary>
        ///救急車・救助隊の派遣依頼
        /// </summary>
        public const int ANPI_INF_HELP_RESCUE = 2;
        /// <summary>
        ///ヘリ等の緊急救助依頼
        /// </summary>
        public const int ANPI_INF_HELP_HELICOPTER = 3;

        // 3.6.10 1 エンコード・デコード用安否情報：体調定義
        /// <summary>
        ///全員良好
        /// </summary>
        public const int ANPI_INF_CND_WELL = 0;
        /// <summary>
        ///自分又は同行者が負傷
        /// </summary>
        public const int ANPI_INF_CND_INJURY = 1;
        /// <summary>
        ///自分又は同行者が発病
        /// </summary>
        public const int ANPI_INF_CND_ILL = 2;
        /// <summary>
        ///自分又は同行者が負傷および発病
        /// </summary>
        public const int ANPI_INF_CND_INJ_ILL = 3;

        // 3.6.11 1 エンコード・デコード用安否情報：現在地定義
        /// <summary>
        ///自宅・友人宅・親類宅
        /// </summary>
        public const int ANPI_INF_LOC_HOUSE = 0;
        /// <summary>
        ///職場・学校・塾
        /// </summary>
        public const int ANPI_INF_LOC_OFFICE = 1;
        /// <summary>
        ///駅・公共施設・商業施設
        /// </summary>
        public const int ANPI_INF_LOC_STATION = 2;
        /// <summary>
        ///避難所・山小屋
        /// </summary>
        public const int ANPI_INF_LOC_SHELTER = 3;
        /// <summary>
        ///自家用車
        /// </summary>
        public const int ANPI_INF_LOC_CAR = 4;
        /// <summary>
        ///電車・バスの車内、または船上
        /// </summary>
        public const int ANPI_INF_LOC_TRAIN = 5;
        /// <summary>
        ///屋外
        /// </summary>
        public const int ANPI_INF_LOC_OUTSIDE = 6;
        /// <summary>
        ///海上
        /// </summary>
        public const int ANPI_INF_LOC_SEA = 7;

        // 3.6.12 1 エンコード・デコード用安否情報：同行者定義
        /// <summary>
        ///自分１人
        /// </summary>
        public const int ANPI_INF_COMPANION_ONE = 0;
        /// <summary>
        ///自分含め２人
        /// </summary>
        public const int ANPI_INF_COMPANION_TWO = 1;
        /// <summary>
        ///自分含め３人
        /// </summary>
        public const int ANPI_INF_COMPANION_THREE = 2;
        /// <summary>
        ///自分含め４人以上
        /// </summary>
        public const int ANPI_INF_COMPANION_MANY = 3;

        // 3.6.13 1 エンコード・デコード用安否情報：移動定義
        /// <summary>
        ///移動可能だが待機中
        /// </summary>
        public const int ANPI_INF_MOVE_STANDBY = 0;
        /// <summary>
        ///目的地に向け移動中
        /// </summary>
        public const int ANPI_INF_MOVE_TOGOAL = 1;
        /// <summary>
        ///目的もなく移動中、漂流
        /// </summary>
        public const int ANPI_INF_MOVE_DRIFT = 2;
        /// <summary>
        ///渋滞、運休、操船不能等で移動困難
        /// </summary>
        public const int ANPI_INF_MOVE_HARDTO = 3;

        // 3.6.14 1 エンコード・デコード用安否情報：事故状況定義
        /// <summary>
        ///事故等なし
        /// </summary>
        public const int ANPI_INF_ACCIDENT_NO = 0;
        /// <summary>
        ///火災や爆発
        /// </summary>
        public const int ANPI_INF_ACCIDENT_FIRE = 1;
        /// <summary>
        ///浸水・転覆・沈没
        /// </summary>
        public const int ANPI_INF_ACCIDENT_SINK = 2;
        /// <summary>
        ///倒壊・衝突・座礁
        /// </summary>
        public const int ANPI_INF_ACCIDENT_COLLISION = 3;

        // 3.6.15 1 エンコード・デコード用安否情報：退避状況定義
        /// <summary>
        ///退避する状況にない
        /// </summary>
        public const int ANPI_INF_SHELTER_NONEED = 0;
        /// <summary>
        ///建物・乗物の内部に退避
        /// </summary>
        public const int ANPI_INF_SHELTER_INSIDE = 1;
        /// <summary>
        ///建物・乗物の外部に退避、船体放棄
        /// </summary>
        public const int ANPI_INF_SHELTER_OUTSIDE = 2;

        /// <summary>
        /// byteのビット長
        /// </summary>
        public const int BYTE_BIT_SIZE = 8;

        //エンコード・デコード用Ｓ帯モニタデータType定義
        /// <summary>
        ///設備ステータス 定義値
        /// </summary>
        public const int VAL_MSG_SBAND_EQU_REQ = 1002;
        /// <summary>
        ///設備ステータス応答 定義値
        /// </summary>
        public const int VAL_MSG_SBAND_EQU_RSP = 2002;
        /// <summary>
        ///回線試験データ 定義値
        /// </summary>
        public const int VAL_MSG_SBAND_TST_REQ = 1030;
        /// <summary>
        ///回線試験データ応答 定義値
        /// </summary>
        public const int VAL_MSG_SBAND_TST_RSP = 2030;
        /// <summary>
        ///Ｓ帯FWDデータ取得要求 定義値
        /// </summary>
        public const int VAL_MSG_SBAND_FWD_REQ = 1010;
        /// <summary>
        ///Ｓ帯FWDデータ取得応答 定義値
        /// </summary>
        public const int VAL_MSG_SBAND_FWD_RSP = 2010;
        /// <summary>
        ///Ｓ帯RTNデータ送信要求 定義値
        /// </summary>
        public const int VAL_MSG_SBAND_RTN_REQ = 3011;
        /// <summary>
        ///Ｓ帯RTNデータ送信応答 定義値
        /// </summary>
        public const int VAL_MSG_SBAND_RTN_RSP = 4011;
        /// <summary>
        ///装置固有情報設定応答 定義値
        /// </summary>
        public const int VAL_MSG_SBAND_DEV_RSP = 2100;

        /// <summary>
        /// Ｓ帯モニタデータType定義からデータIDを計算するための割り算の分母   
        ///   データID = Type定義 % 1000
        /// </summary>
        public const int CALC_DATAID_DENOMI = 1000;


        //Ｓ帯モニタデータサイズ(bit)
        /// <summary>
        ///設備ステータス サイズ
        /// </summary>
        public const int SIZE_MSG_SBAND_EQU_REQ = 20 * BYTE_BIT_SIZE;
        /// <summary>
        ///設備ステータス応答 サイズ
        /// </summary>
        public const int SIZE_MSG_SBAND_EQU_RSP = 80 * BYTE_BIT_SIZE;
        /// <summary>
        ///回線試験データ サイズ
        /// </summary>
        public const int SIZE_MSG_SBAND_TST_REQ = 24 * BYTE_BIT_SIZE;
        /// <summary>
        ///回線試験データ応答 サイズ
        /// </summary>
        public const int SIZE_MSG_SBAND_TST_RSP = 40 * BYTE_BIT_SIZE;
        /// <summary>
        ///Ｓ帯FWDデータ取得要求 サイズ
        /// </summary>
        public const int SIZE_MSG_SBAND_FWD_REQ = 32 * BYTE_BIT_SIZE;
        /// <summary>
        ///Ｓ帯FWDデータ取得応答 サイズ
        /// </summary>
        public const int SIZE_MSG_SBAND_FWD_RSP = 472 * BYTE_BIT_SIZE;
        /// <summary>
        ///Ｓ帯RTNデータ送信要求 サイズ
        /// </summary>
        public const int SIZE_MSG_SBAND_RTN_REQ = 44 * BYTE_BIT_SIZE;
        /// <summary>
        ///Ｓ帯RTNデータ送信応答 サイズ
        /// </summary>
        public const int SIZE_MSG_SBAND_RTN_RSP = 44 * BYTE_BIT_SIZE;
        /// <summary>
        ///装置固有情報設定応答 サイズ
        /// </summary>
        public const int SIZE_MSG_SBAND_DEV_RSP = 20 * BYTE_BIT_SIZE;

        /// <summary>
        ///L1Sデータ取得要求 定義値
        /// </summary>
        public const int VAL_MSG_L1S_REQ = 1040;
        /// <summary>
        ///L1Sデータ取得応答 定義値
        /// </summary>
        public const int VAL_MSG_L1S_RSP = 2040;

        /// <summary>
        ///L1Sデータ取得要求 サイズ
        /// </summary>
        public const int SIZE_MSG_L1S_REQ = 32 * BYTE_BIT_SIZE;
        /// <summary>
        ///L1Sデータ取得応答 サイズ
        /// </summary>
        public const int SIZE_MSG_L1S_RSP = 472 * BYTE_BIT_SIZE;
    }
}
