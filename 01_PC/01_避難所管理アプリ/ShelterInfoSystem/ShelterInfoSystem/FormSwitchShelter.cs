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
    public partial class FormSwitchShelter : Form
    {
        public List<DbAccess.TerminalInfo> shelterInfoList = null; // 避難所情報
        private int selectShelterIndex = -1;

        public FormSwitchShelter(List<DbAccess.TerminalInfo> _shelterInfoList, string selectShelterID)
        {
            InitializeComponent();

            // DB内の全避難所情報を取得
            DbAccess.TerminalInfo[] AllTernimalInfo = new DbAccess.TerminalInfo[0];
            Program.m_objDbAccess.GetTerminalInfoAll(ref AllTernimalInfo);

            // 避難所情報
            shelterInfoList = _shelterInfoList;

            // 避難所管理画面で選択中の避難所情報を表示する
            foreach (var item in shelterInfoList.Select((value, index) => new { value, index }))
            {
                dataGridView1.Rows.Add(item.index, item.value.sid, item.value.name, getState(item.value.open_flag), item.value.memo);

                if (item.value.sid == selectShelterID)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[item.index].Cells[1];
                    dataGridView1.Rows[item.index].Selected = true;
                }
            }
        }

        private string getState(FormShelterInfo.SHELTER_STATUS openFlg)
        {
            string state = "";

            switch (openFlg)
            {
                case FormShelterInfo.SHELTER_STATUS.NO_OPEN:
                    state = "未開設";
                    break;
                case FormShelterInfo.SHELTER_STATUS.OPEN:
                    state = "開設";
                    break;
                case FormShelterInfo.SHELTER_STATUS.CLOSE:
                    state = "閉鎖";
                    break;
                default:
                    state = "不明";
                    break;
            }

            return state;
        }

        public int getSelectShelterIndex()
        {
            return selectShelterIndex;
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            //selectShelterIndex = dataGridView1.CurrentCell.RowIndex;
            if (dataGridView1.CurrentRow != null)
            {
                selectShelterIndex = (int)dataGridView1.CurrentRow.Cells[0].Value;

            }
            else
            {
                selectShelterIndex = -1;
            }

            Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
