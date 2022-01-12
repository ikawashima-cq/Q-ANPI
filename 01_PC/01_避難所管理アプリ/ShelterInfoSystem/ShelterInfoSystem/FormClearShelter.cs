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
    public partial class FormClearShelter : Form
    {
        private bool m_RetBtn = false;
        private DbAccess.TerminalInfo m_ClearInfo = new DbAccess.TerminalInfo();
        private DbAccess.TerminalInfo[] m_AllTerminalInfo = new DbAccess.TerminalInfo[0];

        /// <summary>
        /// ダイアログの結果(OK/キャンセル)
        /// </summary>
        /// <returns></returns>
        public bool GetDlgResult()
        {
            return m_RetBtn;
        }

        /// <summary>
        /// ダイアログで設定した避難所情報
        /// </summary>
        /// <returns></returns>
        public DbAccess.TerminalInfo GetDlgShelterInfo()
        {
            return m_ClearInfo;
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="_shelterInfoList"></param>
        /// <param name="shelterName"></param>
        public FormClearShelter(List<DbAccess.TerminalInfo> _shelterInfoList, string shelterName)
        {
            InitializeComponent();

            // 避難所管理画面で選択中の避難所を設定
            foreach (var item in _shelterInfoList)
            {
                if (item.sid == Program.m_objActiveTermial.sid)
                {
                    // 避難所ID
                    txtShelterID.Text = item.sid;
                    
                    // 避難所名
                    lblShelterName.Text = item.name;

                    // 開設日
                    lblOpenDate.Text = item.open_datetime;
                    
                    // 閉鎖日
                    lblCloseDate.Text = item.close_datetime;
                }
            }

        }

        /// <summary>
        /// OKボタン押下処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("一度削除した情報は復元することができません。" + Environment.NewLine +
                            "削除を実行しますか？", "避難所情報システム", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                // 情報クリアする避難所情報を取得
                Program.m_objDbAccess.GetTerminalInfoAll(ref m_AllTerminalInfo);
                foreach (var item in m_AllTerminalInfo)
                {
                    if (item.sid == txtShelterID.Text)
                    {
                        m_ClearInfo = item;
                    }
                }
                m_RetBtn = true;  
                Close();
            }
            else
            {
                m_RetBtn = false;
            }
        }

        /// <summary>
        /// キャンセル押下処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_RetBtn = false;
            Close();
        }
    }
}
