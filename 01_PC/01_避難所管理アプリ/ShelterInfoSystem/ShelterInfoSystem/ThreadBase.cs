using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using QAnpiCtrlLib.log;

namespace ShelterInfoSystem
{
// デリゲート呼び出し start
/*
    // メッセージ処理用のデリゲート
    public delegate void MessageDelegate(int message, int wParam, long lParam);
*/
// デリゲート呼び出し end

    // 各スレッドの基本クラス
    public class ThreadBase
    {
        // Win32 API
        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostThreadMessage(uint threadId, uint msg, UIntPtr wParam, IntPtr lParam);

        // 状態フラグ
        public const int FL_NO = 0;		    // OFF
        public const int FL_YES = 1;	    // ON

        // ログ出力レベル
        public const int P_LOG_ERR = -1;	// エラー
        public const int P_LOG_OPE = 0;	    // 運用
        public const int P_LOG_TRANS = 1;	// 電文
        public const int P_LOG_DEBUG = 2;	// デバッグ

        // メッセージ引数（WPARAM）
        public const int P_COMMAND = 0;		// 指令
        public const int P_RCV_OK = 1;		// 受付OK
        public const int P_RCV_NG = 2;		// 受付NG
        public const int P_RES_COMP = 3;	// 完了
        public const int P_RES_ERR = 4;		// 実行中ERR

        // ユーザメッセージ
        public const int WM_USER = 0x0400;
        // 終了メッセージ
        public const int WM_QUIT = 0x0012;

        // 自身のスレッド
        public Thread m_Thread = null;
        // 自身のスレッドID
        public uint m_ThreadId = 0;

        // 共有メモリへのアクセス用排他ミューテックス
        private Mutex m_Mutex;

        // スレッドメッセージ受信（メッセージ通信用）
        class ThreadMessageProc : IMessageFilter
        {
            // スレッドクラス
            private ThreadBase m_ThreadBase = null;

            // コンストラクタ
            public ThreadMessageProc(ThreadBase threadBase)
            {
                // スレッドクラス保持
                m_ThreadBase = threadBase;
            }

            // メッセージ受信
            public bool PreFilterMessage(ref Message m)
            {
                // ユーザ定義メッセージの場合
                if (m.Msg >= WM_USER && m.Msg <= 0x7FFF)
                {
                    // メッセージ処理
                    m_ThreadBase.OnMessage(m.Msg, (int)m.WParam, (long)m.LParam);
                }
                return false;
            }
        }

        // スレッド開始メソッド
        public virtual void Start()
        {
            // ミューテックスを作成
            m_Mutex = new System.Threading.Mutex();

            // スレッド作成
            if (m_Thread == null)
            {
                m_Thread = new Thread(new ThreadStart(Run));
            }
            // スレッド開始
            m_Thread.Start();
        }

        // スレッド終了メソッド
        public virtual void Exit()
        {
            // メッセージループの終了
            PostThreadMessage(m_ThreadId, WM_QUIT, (UIntPtr)0, (IntPtr)0);
            // スレッドが終了するまで待機
            if (m_Thread != null)
            {
                m_Thread.Join();
            }
        }

        // メインスレッド処理
        public virtual void Run()
        {
            // スレッドID取得
            m_ThreadId = GetCurrentThreadId();
            // メッセージフィルタ追加（メッセージ通信用）
            Application.AddMessageFilter(new ThreadMessageProc(this));
            // メッセージループ開始
            Application.Run();
        }

        // メッセージ処理
        public virtual void OnMessage(int message, int wParam, long lParam)
        {
        }

        // メッセージ送信メソッド
        public virtual void PostMessage(int message, int wParam, long lParam)
        {
// デリゲート呼び出し start
/*
            // メッセージ処理用デリゲート作成
            MessageDelegate method = new MessageDelegate(OnMessage);

            // メッセージ処理用デリゲート起動
            method.BeginInvoke(message, wParam, lParam, null, null);
*/
// デリゲート呼び出し end
            // メッセージ送信（メッセージ通信用）
            PostThreadMessage(m_ThreadId, (uint)message, (UIntPtr)wParam, (IntPtr)lParam);
        }
    }
}
