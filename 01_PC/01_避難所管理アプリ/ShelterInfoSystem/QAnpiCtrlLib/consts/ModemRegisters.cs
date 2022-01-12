using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAnpiCtrlLib.consts
{
    public class ModemRegisters
    {
        /// <summary>
        /// 疑似地上局時のアドレスの先頭に付加するビット
        /// </summary>
        public const byte STATION_ADDR_BIT = 0x80;

        /// <summary>
        ///ソフトリセット（全体）
        /// </summary>
        public static readonly byte[] SOFT_RESET =　{ (byte)0x00, (byte)0x00, (byte)0x00 };
        /// <summary>
        ///ソフトリセット（全体）のサイズ(bit)
        /// </summary>
        public const int SOFT_RESET_SIZE = 8;
        /// <summary>
        ///イネーブル制御
        /// </summary>
        public static readonly byte[] ENABLE_CTRL = { (byte)0x00, (byte)0x00, (byte)0x01 };
        /// <summary>
        ///イネーブル制御のサイズ(bit)
        /// </summary>
        public const int ENABLE_CTRL_SIZE = 8;
        /// <summary>
        ///アップリンクコード相関検出しきい値(プリアンブル)
        /// </summary>
        public static readonly byte[] UP_C_COREL_DETE_THRE_PRE = { (byte)0x01, (byte)0x10, (byte)0x00 };
        /// <summary>
        ///アップリンクコード相関検出しきい値(プリアンブル)のサイズ(bit)
        /// </summary>
        public const int UP_C_COREL_DETE_THRE_PRE_SIZE = 16;
        /// <summary>
        ///アップリンクコード相関検出しきい値(データ)
        /// </summary>
        public static readonly byte[] UP_C_COREL_DETE_THRE_DATA = { (byte)0x01, (byte)0x10, (byte)0x02 };
        /// <summary>
        ///アップリンクコード相関検出しきい値(データ)のサイズ(bit)
        /// </summary>
        public const int UP_C_COREL_DETE_THRE_DATA_SIZE = 16;
        /// <summary>
        ///アップリンク相関演算開始しきい値
        /// </summary>
        public static readonly byte[] UP_COREL_CAL_S_THRE = { (byte)0x01, (byte)0x10, (byte)0x04 };
        /// <summary>
        ///アップリンク相関演算開始しきい値のサイズ(bit)
        /// </summary>
        public const int UP_COREL_CAL_S_THRE_SIZE = 16;
        /// <summary>
        ///アップリンク相関演算開始タイミング
        /// </summary>
        public static readonly byte[] UP_COREL_CAL_S_TIM = { (byte)0x01, (byte)0x10, (byte)0x08 };
        /// <summary>
        ///アップリンク相関演算開始タイミングのサイズ(bit)
        /// </summary>
        public const int UP_COREL_CAL_S_TIM_SIZE = 32;
        /// <summary>
        ///アップリンクTDMA補正値（RF+モデム受信処理遅延量：⊿2）
        /// </summary>
        public static readonly byte[] UP_TDMA_COR_PRO = { (byte)0x01, (byte)0x10, (byte)0x0C };
        /// <summary>
        ///アップリンクTDMA補正値（RF+モデム受信処理遅延量：⊿2）のサイズ(bit)
        /// </summary>
        public const int UP_TDMA_COR_PRO_SIZE = 8;
        /// <summary>
        ///アップリンクTDMA割り込みタイミング補正量（受信割り込み調整値：α）
        /// </summary>
        public static readonly byte[] UP_TDMA_INTR_COR_AUG_A = { (byte)0x01, (byte)0x10, (byte)0x0D };
        /// <summary>
        ///アップリンクTDMA割り込みタイミング補正量（受信割り込み調整値：α）のサイズ(bit)
        /// </summary>
        public const int UP_TDMA_INTR_COR_AUG_A_SIZE = 8;
        /// <summary>
        ///アップリンクTDMA割り込みタイミング補正量（RF+モデム送信処理遅延：β）
        /// </summary>
        public static readonly byte[] UP_TDMA_INTR_COR_AUG_B = { (byte)0x01, (byte)0x10, (byte)0x0E };
        /// <summary>
        ///アップリンクTDMA割り込みタイミング補正量（RF+モデム送信処理遅延：β）のサイズ(bit)
        /// </summary>
        public const int UP_TDMA_INTR_COR_AUG_B_SIZE = 8;
        /// <summary>
        ///ダウンリンク拡散コード0
        /// </summary>
        public static readonly byte[] DWON_SPREAD_C_0 = { (byte)0x01, (byte)0x10, (byte)0x10 };
        /// <summary>
        ///ダウンリンク拡散コード0のサイズ(bit)
        /// </summary>
        public const int DWON_SPREAD_C_0_SIZE = 32;
        /// <summary>
        ///ダウンリンク拡散コード1
        /// </summary>
        public static readonly byte[] DWON_SPREAD_C_1 = { (byte)0x01, (byte)0x10, (byte)0x14 };
        /// <summary>
        ///ダウンリンク拡散コード1のサイズ(bit)
        /// </summary>
        public const int DWON_SPREAD_C_1_SIZE = 32;
        /// <summary>
        ///ダウンリンクコード相関検出しきい値1(検出)
        /// </summary>
        public static readonly byte[] DOWN_C_COREL_DETE_THER_DETE = { (byte)0x01, (byte)0x10, (byte)0x18 };
        /// <summary>
        ///ダウンリンクコード相関検出しきい値1(検出)のサイズ(bit)
        /// </summary>
        public const int DOWN_C_COREL_DETE_THER_DETE_SIZE = 16;
        /// <summary>
        ///ダウンリンクコード相関検出しきい値2(未検出)
        /// </summary>
        public static readonly byte[] DOWN_C_COREL_DETE_THER_NON = { (byte)0x01, (byte)0x10, (byte)0x1A };
        /// <summary>
        ///ダウンリンクコード相関検出しきい値2(未検出)のサイズ(bit)
        /// </summary>
        public const int DOWN_C_COREL_DETE_THER_NON_SIZE = 16;
        /// <summary>
        ///ダウンリンクコード相関検出保護段数
        /// </summary>
        public static readonly byte[] DOWN_C_COREL_DETE_PRO_STEP = { (byte)0x01, (byte)0x10, (byte)0x1C };
        /// <summary>
        ///ダウンリンクコード相関検出保護段数のサイズ(bit)
        /// </summary>
        public const int DOWN_C_COREL_DETE_PRO_STEP_SIZE = 8;
        /// <summary>
        ///ダウンリンクコード相関検出ウィンドウ幅
        /// </summary>
        public static readonly byte[] DOWN_C_COREL_DETE_WIN_WIDTH = { (byte)0x01, (byte)0x10, (byte)0x1D };
        /// <summary>
        ///ダウンリンクコード相関検出ウィンドウ幅のサイズ(bit)
        /// </summary>
        public const int DOWN_C_COREL_DETE_WIN_WIDTH_SIZE = 8;
        /// <summary>
        ///ダウンリンク同期検出(相関検出)しきい値1(検出)
        /// </summary>
        public static readonly byte[] DOWN_SYNC_DETE_THRE_1 = { (byte)0x01, (byte)0x10, (byte)0x1E };
        /// <summary>
        ///ダウンリンク同期検出(相関検出)しきい値1(検出)のサイズ(bit)
        /// </summary>
        public const int DOWN_SYNC_DETE_THRE_1_SIZE = 8;
        /// <summary>
        ///ダウンリンク同期検出(相関検出)しきい値2(未検出)
        /// </summary>
        public static readonly byte[] DOWN_SYNC_DETE_THRE_2 = { (byte)0x01, (byte)0x10, (byte)0x1F };
        /// <summary>
        ///ダウンリンク同期検出(相関検出)しきい値2(未検出)のサイズ(bit)
        /// </summary>
        public const int DOWN_SYNC_DETE_THRE_2_SIZE = 8;
        /// <summary>
        ///ダウンリンク同期コード
        /// </summary>
        public static readonly byte[] DWON_SYNC_C = { (byte)0x01, (byte)0x10, (byte)0x20 };
        /// <summary>
        ///ダウンリンク同期コードのサイズ(bit)
        /// </summary>
        public const int DWON_SYNC_C_SIZE = 32;
        /// <summary>
        ///ダウンリンク同期検出保護段数
        /// </summary>
        public static readonly byte[] DWON_SYNC_PRO_STEP = { (byte)0x01, (byte)0x10, (byte)0x24 };
        /// <summary>
        ///ダウンリンク同期検出保護段数のサイズ(bit)
        /// </summary>
        public const int DWON_SYNC_PRO_STEP_SIZE = 8;
        /// <summary>
        ///AFCループゲイン
        /// </summary>
        public static readonly byte[] ARG_LOOP_GAIN = { (byte)0x01, (byte)0x20, (byte)0x00 };
        /// <summary>
        ///AFCループゲインのサイズ(bit)
        /// </summary>
        public const int ARG_LOOP_GAIN_SIZE = 8;
        /// <summary>
        ///初期周波数推定時間
        /// </summary>
        public static readonly byte[] INIT_FRQ_EST_TIME = { (byte)0x01, (byte)0x20, (byte)0x02 };
        /// <summary>
        ///初期周波数推定時間のサイズ(bit)
        /// </summary>
        public const int INIT_FRQ_EST_TIME_SIZE = 16;
        /// <summary>
        ///初期周波数推定ゲイン
        /// </summary>
        public static readonly byte[] INIT_FRQ_EST_GAIN = { (byte)0x01, (byte)0x20, (byte)0x04 };
        /// <summary>
        ///初期周波数推定ゲインのサイズ(bit)
        /// </summary>
        public const int INIT_FRQ_EST_GAIN_SIZE = 16;
        /// <summary>
        ///キャリア同期収束閾値
        /// </summary>
        public static readonly byte[] CAR_SYNC_CONV_THRE = { (byte)0x01, (byte)0x20, (byte)0x06 };
        /// <summary>
        ///キャリア同期収束閾値のサイズ(bit)
        /// </summary>
        public const int CAR_SYNC_CONV_THRE_SIZE = 16;
        /// <summary>
        ///キャリア同期タイムアウト設定
        /// </summary>
        public static readonly byte[] CAR_SYNC_TO_SET = { (byte)0x01, (byte)0x20, (byte)0x08 };
        /// <summary>
        ///キャリア同期タイムアウト設定のサイズ(bit)
        /// </summary>
        public const int CAR_SYNC_TO_SET_SIZE = 32;
        /// <summary>
        ///TCXO用オフセット
        /// </summary>
        public static readonly byte[] TCXO_OFFSET = { (byte)0x01, (byte)0x20, (byte)0x0C };
        /// <summary>
        ///TCXO用オフセットのサイズ(bit)
        /// </summary>
        public const int TCXO_OFFSET_SIZE = 16;
        /// <summary>
        ///キャリア同期保護段数
        /// </summary>
        public static readonly byte[] CAR_SYNC_PRO_STEP = { (byte)0x01, (byte)0x20, (byte)0x0E };
        /// <summary>
        ///キャリア同期保護段数のサイズ(bit)
        /// </summary>
        public const int CAR_SYNC_PRO_STEP_SIZE = 8;
        /// <summary>
        ///AGC手動モード（On/Off）
        /// </summary>
        public static readonly byte[] AGC_MAN_MODE = { (byte)0x01, (byte)0x20, (byte)0x10 };
        /// <summary>
        ///AGC手動モード（On/Off）のサイズ(bit)
        /// </summary>
        public const int AGC_MAN_MODE_SIZE = 8;
        /// <summary>
        ///AGC 設定ゲイン初期値
        /// </summary>
        public static readonly byte[] AGC_SET_GAIN_INIT_VAL = { (byte)0x01, (byte)0x20, (byte)0x11 };
        /// <summary>
        ///AGC 設定ゲイン初期値のサイズ(bit)
        /// </summary>
        public const int AGC_SET_GAIN_INIT_VAL_SIZE = 8;
        /// <summary>
        ///AGC　Manualモード時設定値
        /// </summary>
        public static readonly byte[] AGC_MAN_MODE_SET_VALUE = { (byte)0x01, (byte)0x20, (byte)0x12 };
        /// <summary>
        ///AGC　Manualモード時設定値のサイズ(bit)
        /// </summary>
        public const int AGC_MAN_MODE_SET_VALUE_SIZE = 8;
        /// <summary>
        ///AGC　ゲイン変更設定Step
        /// </summary>
        public static readonly byte[] AGC_GAIN_MOD_SET_STEP = { (byte)0x01, (byte)0x20, (byte)0x13 };
        /// <summary>
        ///AGC　ゲイン変更設定Stepのサイズ(bit)
        /// </summary>
        public const int AGC_GAIN_MOD_SET_STEP_SIZE = 8;
        /// <summary>
        ///AGC収束目標値(振幅単位)
        /// </summary>
        public static readonly byte[] AGC_CONV_TGT_VAL = { (byte)0x01, (byte)0x20, (byte)0x14 };
        /// <summary>
        ///AGC収束目標値(振幅単位)のサイズ(bit)
        /// </summary>
        public const int AGC_CONV_TGT_VAL_SIZE = 32;
        /// <summary>
        ///AGC 収束目標上限範囲
        /// </summary>
        public static readonly byte[] AGC_CONV_TGT_U_THRE = { (byte)0x01, (byte)0x20, (byte)0x18 };
        /// <summary>
        ///AGC 収束目標上限範囲のサイズ(bit)
        /// </summary>
        public const int AGC_CONV_TGT_U_THRE_SIZE = 32;
        /// <summary>
        ///AGC 収束目標下限範囲
        /// </summary>
        public static readonly byte[] AGC_CONV_TGT_L_THRE = { (byte)0x01, (byte)0x20, (byte)0x1C };
        /// <summary>
        ///AGC 収束目標下限範囲のサイズ(bit)
        /// </summary>
        public const int AGC_CONV_TGT_L_THRE_SIZE = 32;
        /// <summary>
        ///AGC UnLock上限範囲
        /// </summary>
        public static readonly byte[] AGC_UNLOCK_U_THRE = { (byte)0x01, (byte)0x20, (byte)0x20 };
        /// <summary>
        ///AGC UnLock上限範囲のサイズ(bit)
        /// </summary>
        public const int AGC_UNLOCK_U_THRE_SIZE = 32;
        /// <summary>
        ///AGC UnLock下限範囲
        /// </summary>
        public static readonly byte[] AGC_UNLOCK_L_THRE = { (byte)0x01, (byte)0x20, (byte)0x24 };
        /// <summary>
        ///AGC UnLock下限範囲のサイズ(bit)
        /// </summary>
        public const int AGC_UNLOCK_L_THRE_SIZE = 32;
        /// <summary>
        ///AGC 無信号状態判定閾値
        /// </summary>
        public static readonly byte[] AGC_NOSIG_STATE_THRE = { (byte)0x01, (byte)0x20, (byte)0x28 };
        /// <summary>
        ///AGC 無信号状態判定閾値のサイズ(bit)
        /// </summary>
        public const int AGC_NOSIG_STATE_THRE_SIZE = 32;
        /// <summary>
        ///AGC積分チップ数
        /// </summary>
        public static readonly byte[] AGC_INTE_CHIP_NUM = { (byte)0x01, (byte)0x20, (byte)0x2C };
        /// <summary>
        ///AGC積分チップ数のサイズ(bit)
        /// </summary>
        public const int AGC_INTE_CHIP_NUM_SIZE = 8;
        /// <summary>
        ///AGCウェイト時間(［ｕｓ］)
        /// </summary>
        public static readonly byte[] AGC_WAIT_TIME = { (byte)0x01, (byte)0x20, (byte)0x2D };
        /// <summary>
        ///AGCウェイト時間(［ｕｓ］)のサイズ(bit)
        /// </summary>
        public const int AGC_WAIT_TIME_SIZE = 8;
        /// <summary>
        ///ALC手動モード（On/Off）
        /// </summary>
        public static readonly byte[] ALC_MAN_MODE = { (byte)0x01, (byte)0x20, (byte)0x30 };
        /// <summary>
        ///ALC手動モード（On/Off）のサイズ(bit)
        /// </summary>
        public const int ALC_MAN_MODE_SIZE = 8;
        /// <summary>
        ///ALC送信パワー取り込み周期
        /// </summary>
        public static readonly byte[] ALC_SND_POW_CAP_PERI = { (byte)0x01, (byte)0x20, (byte)0x31 };
        /// <summary>
        ///ALC送信パワー取り込み周期のサイズ(bit)
        /// </summary>
        public const int ALC_SND_POW_CAP_PERI_SIZE = 8;
        /// <summary>
        ///ALC送信パワー平均算出サンプル数
        /// </summary>
        public static readonly byte[] ALC_SND_POW_AVE_CAL_SAMP_MUN = { (byte)0x01, (byte)0x20, (byte)0x32 };
        /// <summary>
        ///ALC送信パワー平均算出サンプル数のサイズ(bit)
        /// </summary>
        public const int ALC_SND_POW_AVE_CAL_SAMP_MUN_SIZE = 8;
        /// <summary>
        ///ALC補正ループ係数
        /// </summary>
        public static readonly byte[] ALC_COR_LOOP_COEFF = { (byte)0x01, (byte)0x20, (byte)0x33 };
        /// <summary>
        ///ALC補正ループ係数のサイズ(bit)
        /// </summary>
        public const int ALC_COR_LOOP_COEFF_SIZE = 8;
        /// <summary>
        ///ALC送信パワー上限値
        /// </summary>
        public static readonly byte[] ALC_SND_POW_U_L = { (byte)0x01, (byte)0x20, (byte)0x34 };
        /// <summary>
        ///ALC送信パワー上限値のサイズ(bit)
        /// </summary>
        public const int ALC_SND_POW_U_L_SIZE = 16;
        /// <summary>
        ///ALC補正値初期値（手動時設定値）
        /// </summary>
        public static readonly byte[] ALC_SND_POW_L_L = { (byte)0x01, (byte)0x20, (byte)0x36 };
        /// <summary>
        ///ALC補正値初期値（手動時設定値）のサイズ(bit)
        /// </summary>
        public const int ALC_SND_POW_L_L_SIZE = 16;
        /// <summary>
        ///ALC送信パワー収束上限閾値
        /// </summary>
        public static readonly byte[] ALC_SND_CON_U_THRE = { (byte)0x01, (byte)0x20, (byte)0x38 };
        /// <summary>
        ///ALC送信パワー収束上限閾値のサイズ(bit)
        /// </summary>
        public const int ALC_SND_CON_U_THRE_SIZE = 16;
        /// <summary>
        ///ALC送信パワー収束下限閾値
        /// </summary>
        public static readonly byte[] ALC_SND_POW_TGT_L_THRE = { (byte)0x01, (byte)0x20, (byte)0x3A };
        /// <summary>
        ///ALC送信パワー収束下限閾値のサイズ(bit)
        /// </summary>
        public const int ALC_SND_POW_TGT_L_THRE_SIZE = 16;
        /// <summary>
        ///ALC用TxATT安定Wait時間
        /// </summary>
        public static readonly byte[] ALC_TｘTTA_WAIT_TIME = { (byte)0x01, (byte)0x20, (byte)0x3C };
        /// <summary>
        ///ALC用TxATT安定Wait時間のサイズ(bit)
        /// </summary>
        public const int ALC_TｘTTA_WAIT_TIME_SIZE = 8;
        /// <summary>
        ///Turbo復号Iteration数
        /// </summary>
        public static readonly byte[] TURBO_C_ITER_NUM = { (byte)0x01, (byte)0x20, (byte)0x3D };
        /// <summary>
        ///Turbo復号Iteration数のサイズ(bit)
        /// </summary>
        public const int TURBO_C_ITER_NUM_SIZE = 8;
        /// <summary>
        ///アップリンクデータ拡散コード0
        /// </summary>
        public static readonly byte[] UP_DATA_SPREAD_C_00 = { (byte)0x02, (byte)0x10, (byte)0x04 };
        /// <summary>
        ///アップリンクデータ拡散コード0のサイズ(bit)
        /// </summary>
        public const int UP_DATA_SPREAD_C_00_SIZE = 32;
        /// <summary>
        ///アップリンクデータ拡散コード0
        /// </summary>
        public const int UP_DATA_SPREAD_C_00_INT = 0x021004;
        /// <summary>
        ///アップリンクプリアンブル拡散コード（LSFR初期値）
        /// </summary>
        public static readonly byte[] UPLPRE_SPREAD_C = { (byte)0x02, (byte)0x10, (byte)0x00 };
        /// <summary>
        ///アップリンクプリアンブル拡散コード（LSFR初期値）のサイズ(bit)
        /// </summary>
        public const int UPLPRE_SPREAD_C_SIZE = 32;
        /// <summary>
        ///アップリンクTDMA補正値（衛星位置から演算した伝搬遅延：⊿1）
        /// </summary>
        public static readonly byte[] UPL_TDMA_COR_VAL_PROP = { (byte)0x02, (byte)0x10, (byte)0x84 };
        /// <summary>
        ///アップリンクTDMA補正値（衛星位置から演算した伝搬遅延：⊿1）のサイズ(bit)
        /// </summary>
        public const int UPL_TDMA_COR_VAL_PROP_SIZE = 32;
        /// <summary>
        ///現在時刻情報（00m00sからの経過秒をmod 8）
        /// </summary>
        public static readonly byte[] CUR_TIME_INFO = { (byte)0x02, (byte)0x10, (byte)0x88 };
        /// <summary>
        ///現在時刻情報（00m00sからの経過秒をmod 8）のサイズ(bit)
        /// </summary>
        public const int CUR_TIME_INFO_SIZE = 8;
        /// <summary>
        ///アップリンクTDMAデバッグ用設定
        /// </summary>
        public static readonly byte[] UP_TDMA_DEBUG_SET = { (byte)0x02, (byte)0x10, (byte)0x89 };
        /// <summary>
        ///アップリンクTDMAデバッグ用設定のサイズ(bit)
        /// </summary>
        public const int UP_TDMA_DEBUG_SET_SIZE = 8;
        /// <summary>
        ///ALC補正パラメータ（f特性）
        /// </summary>
        public static readonly byte[] ALC_COR_PARA = { (byte)0x02, (byte)0x20, (byte)0x00 };
        /// <summary>
        ///ALC補正パラメータ（f特性）のサイズ(bit)
        /// </summary>
        public const int ALC_COR_PARA_SIZE = 16;
        /// <summary>
        ///FEC動作イネーブル（COV/VTB）
        /// </summary>
        public static readonly byte[] FEC_ACT_ENABLE_COV_VTB = { (byte)0x02, (byte)0x20, (byte)0x02 };
        /// <summary>
        ///FEC動作イネーブル（COV/VTB）のサイズ(bit)
        /// </summary>
        public const int FEC_ACT_ENABLE_COV_VTB_SIZE = 8;
        /// <summary>
        ///インタリーブ機能イネーブル
        /// </summary>
        public static readonly byte[] INTER_FUNC_ENABLE = { (byte)0x02, (byte)0x20, (byte)0x03 };
        /// <summary>
        ///インタリーブ機能イネーブルのサイズ(bit)
        /// </summary>
        public const int INTER_FUNC_ENABLE_SIZE = 8;
        /// <summary>
        ///スクランブル機能イネーブル
        /// </summary>
        public static readonly byte[] SCR_FUNC_ENABLE = { (byte)0x02, (byte)0x20, (byte)0x04 };
        /// <summary>
        ///スクランブル機能イネーブルのサイズ(bit)
        /// </summary>
        public const int SCR_FUNC_ENABLE_SIZE = 8;
        /// <summary>
        ///FEC動作イネーブル（Turbo）
        /// </summary>
        public static readonly byte[] FEC_ACT_ENABLE_TURBO = { (byte)0x02, (byte)0x20, (byte)0x05 };
        /// <summary>
        ///FEC動作イネーブル（Turbo）のサイズ(bit)
        /// </summary>
        public const int FEC_ACT_ENABLE_TURBO_SIZE = 8;
        /// <summary>
        ///FEC動作イネーブル（ReedSolomon）
        /// </summary>
        public static readonly byte[] FEC_ACT_ENABLE_REESOL = { (byte)0x02, (byte)0x20, (byte)0x06 };
        /// <summary>
        ///FEC動作イネーブル（ReedSolomon）のサイズ(bit)
        /// </summary>
        public const int FEC_ACT_ENABLE_REESOL_SIZE = 8;
        /// <summary>
        ///AMP制御
        /// </summary>
        public static readonly byte[] AMP_CTRL = { (byte)0x02, (byte)0x30, (byte)0x04 };
        /// <summary>
        ///AMP制御のサイズ(bit)
        /// </summary>
        public const int AMP_CTRL_SIZE = 8;
        /// <summary>
        ///送信データバッファ#0～#859
        /// </summary>
        public static readonly byte[] SND_DATA_BUF = { (byte)0x03, (byte)0x00, (byte)0x00 };
        /// <summary>
        /// 送信データバッファ#0～#859(int)
        /// </summary>
        public const int SND_DATA_BUF_INT = 0x030000;
        /// <summary>
        ///送信データバッファ#0～#859のサイズ(bit)
        /// </summary>
        public const int SND_DATA_BUF_SIZE = 6880;
        /// <summary>
        ///送信要求
        /// </summary>
        public static readonly byte[] SND_REQ = { (byte)0x03, (byte)0x03, (byte)0x60 };
        /// <summary>
        ///送信要求のサイズ(bit)
        /// </summary>
        public const int SND_REQ_SIZE = 8;
        /// <summary>
        ///送信データバッファクリア
        /// </summary>
        public static readonly byte[] SND_DATA_BUF_CLE = { (byte)0x03, (byte)0x03, (byte)0x64 };
        /// <summary>
        ///送信データバッファクリアのサイズ(bit)
        /// </summary>
        public const int SND_DATA_BUF_CLE_SIZE = 8;
        /// <summary>
        ///送信データバッファステータス通知
        /// </summary>
        public static readonly byte[] SND_DATA_BUF_STS_REPO = { (byte)0x03, (byte)0x03, (byte)0x68 };
        /// <summary>
        ///送信データバッファステータス通知のサイズ(bit)
        /// </summary>
        public const int SND_DATA_BUF_STS_REPO_SIZE = 8;
        /// <summary>
        ///送信データバッファモード設定
        /// </summary>
        public static readonly byte[] SND_DATA_BUF_MODE_SET = { (byte)0x03, (byte)0x03, (byte)0x6C };
        /// <summary>
        ///送信データバッファモード設定のサイズ(bit)
        /// </summary>
        public const int SND_DATA_BUF_MODE_SET_SIZE = 8;
        /// <summary>
        ///受信データバッファ#0～#859
        /// </summary>
        public static readonly byte[] RCV_DATA_BUF = { (byte)0x03, (byte)0x10, (byte)0x00 };
        /// <summary>
        ///受信データバッファ#0～#859のサイズ(bit)
        /// </summary>
        public const int RCV_DATA_BUF_SIZE = 6880;
        /// <summary>
        ///受信データバッファRead完了通知
        /// </summary>
        public static readonly byte[] RCV_DATA_BUF_READ_COMP_REPO = { (byte)0x03, (byte)0x13, (byte)0x60 };
        /// <summary>
        ///受信データバッファRead完了通知のサイズ(bit)
        /// </summary>
        public const int RCV_DATA_BUF_READ_COMP_REPO_SIZE = 8;
        /// <summary>
        ///受信データバッファクリア
        /// </summary>
        public static readonly byte[] RCV_DATA_BUF_CLE = { (byte)0x03, (byte)0x13, (byte)0x64 };
        /// <summary>
        ///受信データバッファクリアのサイズ(bit)
        /// </summary>
        public const int RCV_DATA_BUF_CLE_SIZE = 8;
        /// <summary>
        ///受信データバッファステータス通知
        /// </summary>
        public static readonly byte[] RCV_DATA_BUF_STS_REPO = { (byte)0x03, (byte)0x13, (byte)0x68 };
        /// <summary>
        ///受信データバッファステータス通知のサイズ(bit)
        /// </summary>
        public const int RCV_DATA_BUF_STS_REPO_SIZE = 8;
        /// <summary>
        ///割り込み要因
        /// </summary>
        public static readonly byte[] INTR_CAUSE = { (byte)0x04, (byte)0x00, (byte)0x00 };
        /// <summary>
        ///割り込み要因のサイズ(bit)
        /// </summary>
        public const int INTR_CAUSE_SIZE = 32;
        /// <summary>
        ///割り込み要因マスク（To INTR[0]）
        /// </summary>
        public static readonly byte[] INTR_CAUSE_MSK_0 = { (byte)0x04, (byte)0x00, (byte)0x04 };
        /// <summary>
        ///割り込み要因マスク（To INTR[0]）のサイズ(bit)
        /// </summary>
        public const int INTR_CAUSE_MSK_0_SIZE = 32;
        /// <summary>
        ///割り込み要因マスク（To INTR[1]）
        /// </summary>
        public static readonly byte[] INTR_CAUSE_MSK_1 = { (byte)0x04, (byte)0x00, (byte)0x08 };
        /// <summary>
        ///割り込み要因マスク（To INTR[1]）のサイズ(bit)
        /// </summary>
        public const int INTR_CAUSE_MSK_1_SIZE = 32;
        /// <summary>
        ///割り込み要因マスク（To INTR[2]）
        /// </summary>
        public static readonly byte[] INTR_CAUSE_MSK_2 = { (byte)0x04, (byte)0x00, (byte)0x0C };
        /// <summary>
        ///割り込み要因マスク（To INTR[2]）のサイズ(bit)
        /// </summary>
        public const int INTR_CAUSE_MSK_2_SIZE = 32;
        /// <summary>
        ///FPGA内部MMCM(PLL) Lock状態表示
        /// </summary>
        public static readonly byte[] FPGA_MMCM_LCK_STATE_IND = { (byte)0x05, (byte)0x00, (byte)0x00 };
        /// <summary>
        ///FPGA内部MMCM(PLL) Lock状態表示のサイズ(bit)
        /// </summary>
        public const int FPGA_MMCM_LCK_STATE_IND_SIZE = 8;
        /// <summary>
        ///フレーム同期状態表示
        /// </summary>
        public static readonly byte[] FRM_SYN_STATE_IND = { (byte)0x05, (byte)0x00, (byte)0x01 };
        /// <summary>
        ///フレーム同期状態表示のサイズ(bit)
        /// </summary>
        public const int FRM_SYN_STATE_IND_SIZE = 8;
        /// <summary>
        ///位相検出値モニタ
        /// </summary>
        public static readonly byte[] PHASE_DETE_VAL_MON = { (byte)0x05, (byte)0x20, (byte)0x00 };
        /// <summary>
        ///位相検出値モニタのサイズ(bit)
        /// </summary>
        public const int PHASE_DETE_VAL_MON_SIZE = 32;
        /// <summary>
        ///周波数オフセット補正値モニタ1
        /// </summary>
        public static readonly byte[] FRQ_OFFSET_COR_VAL_MON_1 = { (byte)0x05, (byte)0x20, (byte)0x04 };
        /// <summary>
        ///周波数オフセット補正値モニタ1のサイズ(bit)
        /// </summary>
        public const int FRQ_OFFSET_COR_VAL_MON_1_SIZE = 32;
        /// <summary>
        ///周波数オフセット補正値モニタ2
        /// </summary>
        public static readonly byte[] FRQ_OFFSET_COR_VAL_MON_2 = { (byte)0x05, (byte)0x20, (byte)0x08 };
        /// <summary>
        ///周波数オフセット補正値モニタ2のサイズ(bit)
        /// </summary>
        public const int FRQ_OFFSET_COR_VAL_MON_2_SIZE = 32;
        /// <summary>
        ///キャリア同期カウントモニタ
        /// </summary>
        public static readonly byte[] CAR_SYN_CNT_MON = { (byte)0x05, (byte)0x20, (byte)0x0C };
        /// <summary>
        ///キャリア同期カウントモニタのサイズ(bit)
        /// </summary>
        public const int CAR_SYN_CNT_MON_SIZE = 16;
        /// <summary>
        ///AGC検出パワー
        /// </summary>
        public static readonly byte[] AGC_DETE_POW = { (byte)0x05, (byte)0x20, (byte)0x10 };
        /// <summary>
        ///AGC検出パワーのサイズ(bit)
        /// </summary>
        public const int AGC_DETE_POW_SIZE = 32;
        /// <summary>
        ///AGC演算補正値（RF)
        /// </summary>
        public static readonly byte[] AGC_CAL_COR_VAL = { (byte)0x05, (byte)0x20, (byte)0x14 };
        /// <summary>
        ///AGC演算補正値（RF)のサイズ(bit)
        /// </summary>
        public const int AGC_CAL_COR_VAL_SIZE = 8;
        /// <summary>
        ///ALC検出パワー
        /// </summary>
        public static readonly byte[] ALC_DETE_POW = { (byte)0x05, (byte)0x20, (byte)0x20 };
        /// <summary>
        ///ALC検出パワーのサイズ(bit)
        /// </summary>
        public const int ALC_DETE_POW_SIZE = 16;
        /// <summary>
        ///ALC演算補正値
        /// </summary>
        public static readonly byte[] ALC_CAL_COR_VAL = { (byte)0x05, (byte)0x20, (byte)0x22 };
        /// <summary>
        ///ALC演算補正値のサイズ(bit)
        /// </summary>
        public const int ALC_CAL_COR_VAL_SIZE = 16;
        /// <summary>
        ///CRCコード表示
        /// </summary>
        public static readonly byte[] CRC_C_IND = { (byte)0x05, (byte)0x20, (byte)0x24 };
        /// <summary>
        ///CRCコード表示のサイズ(bit)
        /// </summary>
        public const int CRC_C_IND_SIZE = 32;
        /// <summary>
        ///受信データ平均レベル表示（正規化検出パワー）
        /// </summary>
        public static readonly byte[] RCV_DATA_AVE_LVL_IND = { (byte)0x05, (byte)0x20, (byte)0x28 };
        /// <summary>
        ///受信データ平均レベル表示（正規化検出パワー）のサイズ(bit)
        /// </summary>
        public const int RCV_DATA_AVE_LVL_IND_SIZE = 16;
        /// <summary>
        ///ビタビステータス表示
        /// </summary>
        public static readonly byte[] VITERBI_STS_IND = { (byte)0x05, (byte)0x20, (byte)0x2A };
        /// <summary>
        ///ビタビステータス表示のサイズ(bit)
        /// </summary>
        public const int VITERBI_STS_IND_SIZE = 8;
        /// <summary>
        ///受信フレームカウンタ
        /// </summary>
        public static readonly byte[] RCV_FRM_COUNT = { (byte)0x06, (byte)0x00, (byte)0x00 };
        /// <summary>
        ///受信フレームカウンタのサイズ(bit)
        /// </summary>
        public const int RCV_FRM_COUNT_SIZE = 16;
        /// <summary>
        ///受信フレームエラー(廃棄)カウンタ
        /// </summary>
        public static readonly byte[] RCV_FRM_ERR_COUNT = { (byte)0x06, (byte)0x00, (byte)0x02 };
        /// <summary>
        ///受信フレームエラー(廃棄)カウンタのサイズ(bit)
        /// </summary>
        public const int RCV_FRM_ERR_COUNT_SIZE = 16;
        /// <summary>
        ///テスト（CW）送信制御
        /// </summary>
        public static readonly byte[] TST_SND_CTRL_CW = { (byte)0x07, (byte)0x00, (byte)0x00 };
        /// <summary>
        ///テスト（CW）送信制御のサイズ(bit)
        /// </summary>
        public const int TST_SND_CTRL_CW_SIZE = 8;
        /// <summary>
        ///テスト（BW）送信制御
        /// </summary>
        public static readonly byte[] TST_SND_CTRL_BW = { (byte)0x07, (byte)0x00, (byte)0x01 };
        /// <summary>
        ///テスト（BW）送信制御のサイズ(bit)
        /// </summary>
        public const int TST_SND_CTRL_BW_SIZE = 8;
        /// <summary>
        ///BER送信機能設定
        /// </summary>
        public static readonly byte[] BER_SND_FUNC_SET = { (byte)0x07, (byte)0x00, (byte)0x02 };
        /// <summary>
        ///BER送信機能設定のサイズ(bit)
        /// </summary>
        public const int BER_SND_FUNC_SET_SIZE = 8;
        /// <summary>
        ///BER受信機能設定
        /// </summary>
        public static readonly byte[] BER_RCV_FUNC_SET = { (byte)0x07, (byte)0x00, (byte)0x03 };
        /// <summary>
        ///BER受信機能設定のサイズ(bit)
        /// </summary>
        public const int BER_RCV_FUNC_SET_SIZE = 8;
        /// <summary>
        ///BER測定のステータス表示
        /// </summary>
        public static readonly byte[] BER_MEAS_STS_IND = { (byte)0x07, (byte)0x00, (byte)0x04 };
        /// <summary>
        ///BER測定のステータス表示のサイズ(bit)
        /// </summary>
        public const int BER_MEAS_STS_IND_SIZE = 8;
        /// <summary>
        ///BER測定の受信ビット数表示
        /// </summary>
        public static readonly byte[] BER_MEAS_RCV_BIT_IND = { (byte)0x07, (byte)0x00, (byte)0x08 };
        /// <summary>
        ///BER測定の受信ビット数表示のサイズ(bit)
        /// </summary>
        public const int BER_MEAS_RCV_BIT_IND_SIZE = 32;
        /// <summary>
        ///BER測定のエラービット数表示
        /// </summary>
        public static readonly byte[] BER_MEAS_ERR_BIT_IND = { (byte)0x07, (byte)0x00, (byte)0x0C };
        /// <summary>
        ///BER測定のエラービット数表示のサイズ(bit)
        /// </summary>
        public const int BER_MEAS_ERR_BIT_IND_SIZE = 32;
        /// <summary>
        ///データダンプイネーブル設定
        /// </summary>
        public static readonly byte[] DATA_DUMP_ENABLE_SET = { (byte)0x07, (byte)0x00, (byte)0x10 };
        /// <summary>
        ///データダンプイネーブル設定のサイズ(bit)
        /// </summary>
        public const int DATA_DUMP_ENABLE_SET_SIZE = 8;
        /// <summary>
        ///ダンプデータ選択
        /// </summary>
        public static readonly byte[] DUMP_DATA_SELECT = { (byte)0x07, (byte)0x00, (byte)0x11 };
        /// <summary>
        ///ダンプデータ選択のサイズ(bit)
        /// </summary>
        public const int DUMP_DATA_SELECT_SIZE = 8;
        /// <summary>
        ///ダンプバッファ状態表示
        /// </summary>
        public static readonly byte[] DUMP_BUFF_STATE = { (byte)0x07, (byte)0x00, (byte)0x14 };
        /// <summary>
        ///ダンプバッファ状態表示のサイズ(bit)
        /// </summary>
        public const int DUMP_BUFF_STATE_SIZE = 16;
        /// <summary>
        ///LED出力制御
        /// </summary>
        public static readonly byte[] LED_OUTPUT_CTRL = { (byte)0x07, (byte)0x00, (byte)0x18 };
        /// <summary>
        ///LED出力制御のサイズ(bit)
        /// </summary>
        public const int LED_OUTPUT_CTRL_SIZE = 8;
        /// <summary>
        ///受信側ブロック強制イネーブル制御
        /// </summary>
        public static readonly byte[] RCV_BLOCK_FORCE_ENABLE_CTRL = { (byte)0x07, (byte)0x00, (byte)0x20 };
        /// <summary>
        ///受信側ブロック強制イネーブル制御(bit)
        /// </summary>
        public const int RCV_BLOCK_FORCE_ENABLE_CTRL_SIZE = 8;
        /// <summary>
        ///AFC用SPI 手動モード（On/Off）
        /// </summary>
        public static readonly byte[] AFC_SPI_MAN_MODE = { (byte)0x08, (byte)0x00, (byte)0x00 };
        /// <summary>
        ///AFC用SPI 手動モード（On/Off）のサイズ(bit)
        /// </summary>
        public const int AFC_SPI_MAN_MODE_SIZE = 8;
        /// <summary>
        ///AFC用SPI 周波数セレクト
        /// </summary>
        public static readonly byte[] AFC_SPI_FRQ_SEL = { (byte)0x08, (byte)0x00, (byte)0x01 };
        /// <summary>
        ///AFC用SPI 周波数セレクトのサイズ(bit)
        /// </summary>
        public const int AFC_SPI_FRQ_SEL_SIZE = 8;
        /// <summary>
        ///AFC用SPI 手動ライトデータ
        /// </summary>
        public static readonly byte[] AFC_SPI_MAN_WRITE_DATA = { (byte)0x08, (byte)0x00, (byte)0x04 };
        /// <summary>
        ///AFC用SPI 手動ライトデータのサイズ(bit)
        /// </summary>
        public const int AFC_SPI_MAN_WRITE_DATA_SIZE = 32;
        /// <summary>
        ///AFC用SPI 処理要求
        /// </summary>
        public static readonly byte[] AFC_SPI_PROC_REQ = { (byte)0x08, (byte)0x00, (byte)0x08 };
        /// <summary>
        ///AFC用SPI 処理要求のサイズ(bit)
        /// </summary>
        public const int AFC_SPI_PROC_REQ_SIZE = 8;
        /// <summary>
        ///ALC用SPI 手動モード（On/Off）									
        /// </summary>
        public static readonly byte[] ALC_SPI_MAN_MODE = { (byte)0x08, (byte)0x10, (byte)0x00 };
        /// <summary>
        ///ALC用SPI 処理要求のサイズ(bit)
        /// </summary>
        public const int ALC_SPI_MAN_MODE_SIZE = 8;
        /// <summary>
        ///ALC用SPI 周波数セレクト
        /// </summary>
        public static readonly byte[] ALC_SPI_FRQ_SEL = { (byte)0x08, (byte)0x10, (byte)0x01 };
        /// <summary>
        ///ALC用SPI 周波数セレクトのサイズ(bit)
        /// </summary>
        public const int ALC_SPI_FRQ_SEL_SIZE = 8;
        /// <summary>
        ///ALC用SPI 処理要求
        /// </summary>
        public static readonly byte[] ALC_SPI_PROC_REQ = { (byte)0x08, (byte)0x10, (byte)0x08 };
        /// <summary>
        ///ALC用SPI 処理要求のサイズ(bit)
        /// </summary>
        public const int ALC_SPI_PROC_REQ_SIZE = 8;
        /// <summary>
        ///ALC用SPI リードデータ
        /// </summary>
        public static readonly byte[] ALC_SPI_READ_DATA = { (byte)0x08, (byte)0x10, (byte)0x0C };
        /// <summary>
        ///ALC用SPI 処理要求のサイズ(bit)
        /// </summary>
        public const int ALC_SPI_READ_DATA_SIZE = 16;
        /// <summary>
        ///AD9364用SPI 手動モード（On/Off）
        /// </summary>
        public static readonly byte[] AD9364_SPI_MAN_MODE = { (byte)0x08, (byte)0x20, (byte)0x00 };
        /// <summary>
        ///AD9364用SPI 手動モード（On/Off）のサイズ(bit)
        /// </summary>
        public const int AD9364_SPI_MAN_MODE_SIZE = 8;
        /// <summary>
        ///AD9364用SPI 周波数セレクト
        /// </summary>
        public static readonly byte[] AD9364_SPI_FRQ_SEL = { (byte)0x08, (byte)0x20, (byte)0x01 };
        /// <summary>
        ///AD9364用SPI 周波数セレクトのサイズ(bit)
        /// </summary>
        public const int AD9364_SPI_FRQ_SEL_SIZE = 8;
        /// <summary>
        ///AD9364用SPI 手動ライトデータ
        /// </summary>
        public static readonly byte[] AD9364_SPI_MAN_WRITE_DATA = { (byte)0x08, (byte)0x20, (byte)0x04 };
        /// <summary>
        ///AD9364用SPI 手動ライトデータのサイズ(bit)
        /// </summary>
        public const int AD9364_SPI_MAN_WRITE_DATA_SIZE = 32;
        /// <summary>
        ///AD9364用SPI 処理要求
        /// </summary>
        public static readonly byte[] AD9364_SPI_PROC_REQ = { (byte)0x08, (byte)0x20, (byte)0x08 };
        /// <summary>
        ///AD9364用SPI 処理要求のサイズ(bit)
        /// </summary>
        public const int AD9364_SPI_PROC_REQ_SIZE = 8;
        /// <summary>
        ///AD9364用SPI リードデータ
        /// </summary>
        public static readonly byte[] AD9364_SPI_READ_DATA = { (byte)0x08, (byte)0x20, (byte)0x0C };
        /// <summary>
        ///AD9364用SPI リードデータのサイズ(bit)
        /// </summary>
        public const int AD9364_SPI_READ_DATA_SIZE = 8;
        /// <summary>
        ///AD9364用CTRL OUTモニタ
        /// </summary>
        public static readonly byte[] AD9364_CTRL_OUT_MON = { (byte)0x08, (byte)0x20, (byte)0x10 };
        /// <summary>
        ///AD9364用CTRL OUTモニタのサイズ(bit)
        /// </summary>
        public const int AD9364_CTRL_OUT_MON_SIZE = 8;
        /// <summary>
        ///HMC831用SPI 周波数セレクト
        /// </summary>
        public static readonly byte[] HMC831_SPI_FRQ_SEL = { (byte)0x08, (byte)0x30, (byte)0x01 };
        /// <summary>
        ///HMC831用SPI 周波数セレクトのサイズ(bit)
        /// </summary>
        public const int HMC831_SPI_FRQ_SEL_SIZE = 8;
        /// <summary>
        ///HMC831用SPI 設定データ
        /// </summary>
        public static readonly byte[] HMC831_SPI_MAN_WRITE_DATA = { (byte)0x08, (byte)0x30, (byte)0x04 };
        /// <summary>
        ///HMC831用SPI 設定データ(bit)
        /// </summary>
        public const int HMC831_SPI_MAN_WRITE_DATA_SIZE = 32;
        /// <summary>
        ///HMC831用SPI 処理要求
        /// </summary>
        public static readonly byte[] HMC831_SPI_PROC_REQ = { (byte)0x08, (byte)0x30, (byte)0x08 };
        /// <summary>
        ///HMC831用SPI 処理要求のサイズ(bit)
        /// </summary>
        public const int HMC831_SPI_PROC_REQ_SIZE = 8;
        /// <summary>
        ///HMC831用SPI リードデータ
        /// </summary>
        public static readonly byte[] HMC831_SPI_READ_DATA = { (byte)0x08, (byte)0x30, (byte)0x0C };
        /// <summary>
        ///HMC831用SPI リードデータのサイズ(bit)
        /// </summary>
        public const int HMC831_SPI_READ_DATA_SIZE = 32;
        /// <summary>
        /// VGA用SPI 手動モード（On/Off）
        /// </summary>
        public static readonly byte[] VGA_SPI_MAN_MODE = { (byte)0x08, (byte)0x40, (byte)0x00 };
        /// <summary>
        /// VGA用SPI 手動モード（On/Off）のサイズ(bit)
        /// </summary>
        public const int VGA_SPI_MAN_MODE_SIZE = 8;
        /// <summary>
        /// VGA用SPI 周波数セレクト
        /// </summary>
        public static readonly byte[] VGA_SPI_FRQ_SEL = { (byte)0x08, (byte)0x40, (byte)0x01 };
        /// <summary>
        /// VGA用SPI 周波数セレクトのサイズ(bit)
        /// </summary>
        public const int VGA_SPI_FRQ_SEL_SIZE = 8;
        /// <summary>
        /// VGA用SPI 手動ライトデータ
        /// </summary>
        public static readonly byte[] VGA_SPI_MAN_WRITE_DATA = { (byte)0x08, (byte)0x40, (byte)0x04 };
        /// <summary>
        /// VGA用SPI 手動ライトデータのサイズ(bit)
        /// </summary>
        public const int VGA_SPI_MAN_WRITE_DATA_SIZE = 32;
        /// <summary>
        /// VGA用SPI 処理要求
        /// </summary>
        public static readonly byte[] VGA_SPI_PROC_REQ = { (byte)0x08, (byte)0x40, (byte)0x08 };
        /// <summary>
        /// VGA用SPI 処理要求のサイズ(bit)
        /// </summary>
        public const int VGA_SPI_PROC_REQ_SIZE = 8;
        /// <summary>
        ///データダンプバッファ									
        /// </summary>
        public static readonly byte[] DATA_DUMP_BUFF = { (byte)0x0A, (byte)0x00, (byte)0x00 };
        /// <summary>
        ///データダンプバッファのサイズ(bit)
        /// </summary>
        public const int DATA_DUMP_BUFF_SIZE = (0x0DFFFC - 0x0A000) * 8;
        /// <summary>
        ///FPGA Version
        /// </summary>
        public static readonly byte[] FPGA_VERSION = { (byte)0x0F, (byte)0x00, (byte)0x00 };
        /// <summary>
        ///FPGA Versionのサイズ(bit)
        /// </summary>
        public const int FPGA_VERSION_SIZE = 16;

        /// <summary>
        /// AGC LOCK 取得用bitマスク
        /// </summary>
        public const byte AGC_LOCK_MASK = (byte)0x01;
        /// <summary>
        /// フレーム同期取得用bitマスク
        /// </summary>
        public const byte FRAME_SYNC_MASK = (byte)0x08;
        /// <summary>
        /// シンボル同期取得用bitマスク
        /// </summary>
        public const byte SYMBOL_SYNC_MASK = (byte)0x04;
        /// <summary>
        /// キャリア同期取得用取得用bitマスク
        /// </summary>
        public const byte CARRIER_SYNC_MASK = (byte)0x02;
        /// <summary>
        /// TDMA同期取得用ビットマスク
        /// </summary>
        public const byte TDMA_SYNC_MASK = (byte)0x10;

        /// <summary>
        /// イネーブル制御	送信回路　ENABLE								
        /// </summary>
        public const byte SND_ENABLE = 0x1;
        /// <summary>
        /// イネーブル制御	受信回路　ENABLE								
        /// </summary>
        public const byte RCV_ENABLE =  0x2;
        /// <summary>
        /// RF送信PLL同期状態変化(PLL Lock Detect)
        /// </summary>
        public const int TX_PLL_SYNC_CHG = 19;
        /// <summary>
        /// SPI通信完了通知（LTC2611 for VGA）
        /// </summary>
        public const int COM_COMP_VGA_BIT = 18;
        /// <summary>
        /// SPI通信完了通知（HMC831）
        /// </summary>
        public const int COM_COMP_HMC831_BIT = 17;
        /// <summary>
        /// SPI通信完了通知（LTC1197）
        /// </summary>
        public const int COM_COMP_LTC1197_BIT = 16;
        /// <summary>
        /// 受信TDMA割り込みbit
        /// </summary>
        public const int RCV_TDMA_BIT = 15;
        /// <summary>
        /// 送信TDMA割り込みbit
        /// </summary>
        public const int SND_TDMA_BIT = 14;
        /// <summary>
        /// 同期状態変化(TDMA同期)
        /// </summary>
        public const int SYNC_TDMA_BIT = 13;
        /// <summary>
        /// 同期状態変化(フレーム同期)
        /// </summary>
        public const int SYNC_FRAME_BIT = 12;
        /// <summary>
        /// 同期状態変化(シンボル同期)
        /// </summary>
        public const int SYNC_SYMBOL_BIT = 11;
        /// <summary>
        /// 同期状態変化(キャリア同期)
        /// </summary>
        public const int SYNC_CARRIER_BIT = 10;
        /// <summary>
        /// 同期状態変化(AGC Lock)
        /// </summary>
        public const int LOCK_AGC_BIT = 9;
        /// <summary>
        /// 送信パワーの上限閾値超えによるAMP強制停止通知
        /// </summary>
        public const int FORCE_STOP_AMP_BIT = 8;
        /// <summary>
        /// 受信データバッファ 受信完了（刈取り要求）通知
        /// </summary>
        public const int RCV_COMP_BIT = 7;
        /// <summary>
        /// 送信データバッファ 送信完了通知
        /// </summary>
        public const int SND_COMP_BIT = 6;
        /// <summary>
        /// SPI通信完了通知（AD9364）
        /// </summary>
        public const int COM_COMP_AD9364_BIT = 5;
        /// <summary>
        /// SPI通信完了通知（LTC2611）
        /// </summary>
        public const int COM_COMP_LTC2611_BIT = 4;
        /// <summary>
        /// 受信データバッファオーバーフロー通知
        /// </summary>
        public const int RCV_BUFF_OVER_FLOW_BIT = 3;
        /// <summary>
        /// （プロト端末）：フレーム廃棄通知（RSエラー）　　1=エラー発生
        /// （擬似地上局）：No function
        /// </summary>
        public const int ERR_RS_BIT = 2;
        /// <summary>
        /// （プロト端末）：No function
        /// （擬似地上局）：フレーム廃棄通知（CRCエラー）　　1=エラー発生
        /// </summary>
        public const int ERR_CRC_BIT = 1;
        /// <summary>
        /// System CLK生成用MMCM Lock状態t通知							
        /// </summary>
        public const int LOCK_MMCM_BIT = 0;

        /// <summary>
        /// バッファステータス　FULL　0x2 or 0x3:Full
        /// </summary>
        public const int BUFF_FULL     = 0x2;
        /// <summary>
        /// バッファステータス　NON FULL 0x1:Not Full/Not Empty
        /// </summary>
        public const int BUFF_NON_FULL = 0x1;
        /// <summary>
        /// バッファステータス　EMPTY 0x0:Empty
        /// </summary>
        public const int BUFF_EMPTY    = 0x0;

        /// <summary>
        /// モデムに一度に書き込める最大サイズ
        /// </summary>
        public const int WRITE_DATA_MAX = 64;

        /// <summary>
        /// 送信要求時に設定するデータ部
        /// </summary>
        public static readonly byte[] WDATA_SED_REQ = {(byte)0x01};

        /// <summary>
        /// AD9364用SPI 処理要求に設定するデータ部
        /// </summary>
        public static readonly byte[] WDATA_AD9364_SPI_PROC_REQ = { (byte)0x01 };

        /// <summary>
        /// アップリンクデータ拡散コードの個数
        /// </summary>
        public const int UP_LINK_DATA_SPREAD_CODE_COUNT = 1;

        /// <summary>
        /// HMC831用SPI 処理要求に設定するデータ部
        /// </summary>
        public static readonly byte[] WDATA_HMC831_SPI_PROC_REQ = { (byte)0x01 };

        /// <summary>
        /// HMC831用SPI 読み出したデータの有効サイズ
        /// </summary>
        public const int HMC831_SPI_READ_DATA_VAILD_SIZE = 24;


    }
}
