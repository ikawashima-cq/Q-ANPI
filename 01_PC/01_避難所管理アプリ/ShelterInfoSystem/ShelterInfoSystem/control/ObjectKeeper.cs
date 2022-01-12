/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShelterInfoSystem.control
{
    /// <summary>
    /// 共通で使うインスタンスを保持するクラス
    /// </summary>
    class ObjectKeeper
    {
        /// <summary>
        /// メイン画面のStatusLabel
        /// </summary>
        public static ToolStripStatusLabel mainFormStatusLabel = null;
        
        /// <summary>
        /// メイン画面のリストビュー(メニュー)
        /// </summary>
        public static ListView lsv_mainFormListView = null;

        /// <summary>
        /// 送受信済メッセージ一覧表示フォームクラス
        /// </summary>
//        public static MsgJournal_CTF201 form_Journal = null;

        /// <summary>
        /// 送受信済Ｓ帯モニタデータ一覧表示フォームクラス
        /// </summary>
//        public static SBandMsgJournal_CTF202 form_JournalSBandMsg = null;

        /// <summary>
        /// 制御ツールのモード　未決定
        /// </summary>
        public const int MODE_NONE = 0;

        /// <summary>
        /// 制御ツールのモード　Ｓ帯モニタ端末
        /// </summary>
        public const int MODE_TERM = 1;
        
        /// <summary>
        /// 制御ツールのモード　疑似地上局
        /// </summary>
        public const int MODE_STATION = 2;

        /// <summary>
        /// 制御ツールのモード　疑似安否確認SS
        /// </summary>
        public const int MODE_ANPISS = 3;
        
        /// <summary>
        /// 制御ツールのモード
        /// </summary>
        public static int mode = MODE_NONE;

        /// <summary>
        /// メイン画面の左側
        /// </summary>
        public static Panel pnl_left = null;
        /// <summary>
        /// メイン画面の右
        /// </summary>
        public static Panel pnl_right = null;

        /// <summary>
        /// モニタパラメータ値表示画面クラス
        /// </summary>
//        public static MonParaDisp_CTF024 form_monitor = null;
    }
}
