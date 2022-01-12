using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EncryptedQRCodeReader
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Mutexクラスの作成
            System.Threading.Mutex mutex = new System.Threading.Mutex(false, "EncryptedQRCodeReader");
            // ミューテックスの所有権を要求する
            if (mutex.WaitOne(0, false) == false)
            {
                // すでに起動していると判断して終了
                MessageBox.Show("Already executed this application.", "EncryptedQRCodeReader", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new QR読み取り());

            // ミューテックスを解放する
            mutex.ReleaseMutex();
        }
    }
}
