using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShelterInfoSystem
{
    public partial class FormEditShelter : Form
    {
        private bool isUpdate = false; // 更新フラグ
        private string m_NewShelterName = ""; // 変更後避難所名
        private List<DbAccess.TerminalInfo> m_ShelterInfoList = null; // 避難所情報
        private DbAccess.TerminalInfo m_UpdateShelter = new DbAccess.TerminalInfo(); // 更新した避難所情報

        /// <summary>
        /// ダイアログの結果(OK/キャンセル)
        /// </summary>
        /// <returns></returns>
        public bool GetDlgResult()
        {
            return isUpdate;
        }

        /// <summary>
        /// ダイアログで設定した避難所情報
        /// </summary>
        /// <returns></returns>
        public DbAccess.TerminalInfo GetDlgShelterInfo()
        {
            return m_UpdateShelter;
        }

        /// <summary>
        /// ダイアログ起動時の処理
        /// </summary>
        /// <param name="_shelterInfoList"></param>
        /// <param name="shelterName"></param>
        public FormEditShelter(List<DbAccess.TerminalInfo> _shelterInfoList, string shelterName)
        {
            InitializeComponent();

            // 避難所情報
            m_ShelterInfoList = _shelterInfoList;

            // 変数の初期化
            shelterID.Text = "";
            m_UpdateShelter.init();

            // 避難所管理画面で選択中の避難所の情報を表示する
            lblShelterName.Text = shelterName;
            for (int i = 0; i < m_ShelterInfoList.Count; i++)
            {
                // 選択中の避難所名と一致した場合
                if (m_ShelterInfoList[i].sid == Program.m_objActiveTermial.sid)
                {
                    // 各情報を表示
                    shelterID.Text = m_ShelterInfoList[i].sid; // 避難所ID
                    txtEditShelterName.Text = m_ShelterInfoList[i].name; // 避難所名(変更後)
                    txtMemo.Text = m_ShelterInfoList[i].memo; // メモ
                }
            }
        }

        // キャンセル処理
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 終了
            Close();
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormEditShelter", "btnCancel_Click", "キャンセル　クリック");
        }

        // 更新処理
        private void btnRegist_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormEditShelter", "btnRegist_Click", "更新　クリック");

            // 変更前の避難所名
            m_NewShelterName = txtEditShelterName.Text;

            // 入力エラーチェック
            //変更後の避難所名が空ではないか
            if ((txtEditShelterName.Text == "")||(string.IsNullOrWhiteSpace(txtEditShelterName.Text)))
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormEditShelter", "btnRegist_Click", "変更後の避難所名が入力されていません。");
                MessageBox.Show("変更後の避難所名が入力されていません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 避難所名/メモを更新する
            for (int i = 0; i < m_ShelterInfoList.Count; i++)
            {
                if (m_ShelterInfoList[i].sid == shelterID.Text)
                {
                    // ダイアログにユーザが入力した避難所名とメモでDBの避難所情報データを更新する
                    DbAccess.TerminalInfo tmpInfo = m_ShelterInfoList[i];
                    tmpInfo.name = m_NewShelterName;
                    tmpInfo.memo = txtMemo.Text;

                    // 送信する避難所情報を取得
                    m_UpdateShelter = tmpInfo;

                    // 更新フラグを立てる
                    isUpdate = true;

                    break;
                }
            }

            // 選択中の避難所が開設済みだが未接続の場合、編集は不可能
            if ((m_UpdateShelter.open_flag == FormShelterInfo.SHELTER_STATUS.OPEN) && (Program.m_EquStat.isConnected() == false))
            {
                // 更新フラグをOFF
                isUpdate = false;

                // エラーメッセージを表示し、ダイアログを終了しない
                MessageBox.Show("開設済みの避難所はQ-ANPIターミナルに接続中のみ編集可能です。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormEditShelter", "btnRegist_Click", "開設済みの避難所はQ-ANPIターミナルに接続中のみ編集可能です。");
            }
            else
            {
                // 終了
                Close();
            }            
        }
    }
}
