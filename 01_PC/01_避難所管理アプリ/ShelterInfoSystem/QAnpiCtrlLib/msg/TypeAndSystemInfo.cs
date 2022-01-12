/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAnpiCtrlLib.msg
{
    /// <summary>
    /// Type, システム情報の格納クラス
    /// 端末宛、端末発のメッセージフォーマットに合わせて差分のある定義値を適宜選択すること
    /// </summary>
    public class TypeAndSystemInfo
    {
        /// <summary>
        /// メッセージタイプ
        /// </summary>
        public int msgType;

        /// <summary>
        /// msgTypeの有効ビット数（端末発メッセージ）
        /// </summary>
        public const int MSG_TYPE_FROM_TERMINAL_SIZE =
                MsgFromTerminal.MSG_TYPE_SIZE;

        /// <summary>
        /// msgTypeの有効ビット数（端末宛メッセージ）
        /// </summary>
        public const int MSG_TYPE_TO_TERMINAL_SIZE =
                MsgToTerminal.MSG_TYPE_SIZE;

        /// <summary>
        /// システム情報クラスの設定値。 （端末発メッセージでは未使用）
        /// </summary>
        public SystemInfo sysInfo;
    }
}
