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
    public partial class FormImportQR : Form
    {
        private List<DbAccess.PersonInfo> m_IndependentPerson;
        FormShelterInfo m_mainForm;

        public FormImportQR(DbAccess.TerminalInfo info, List<DbAccess.PersonInfo> personInfo, FormShelterInfo mainForm)
        {
            InitializeComponent();
            m_mainForm = mainForm;

            // 避難所IDの設定
            txtShelterID.Text = info.sid;

            // 避難所名の設定
            lblShelterName.Text = info.name;

            // 受信データリストに受信データ（=sidの設定されていない個人安否情報）を表示する。
            int count = 0;
            foreach (var item in personInfo)
            {
                count++;

                // person_infoをstring配列に変換
                string[] strItems = new string[3];
                strItems[0] = count.ToString();
                strItems[1] = item.name;
                strItems[2] = item.id;

                // リストにセット
                listPersonal.Items.Add(new ListViewItem(strItems));
            }

            m_IndependentPerson = personInfo;

            lblResultCount.Text = count.ToString() + "件";

            // 受信データが0件の場合、登録ボタンを非活性化する
            if (count < 1)
            {
                btnImport.Enabled = false;
            }
            else
            {
                btnImport.Enabled = true;
            }
        }

        /// <summary>
        /// 受信データを避難所の個人安否情報として登録する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            foreach (var item in m_IndependentPerson)
            {
                lock (m_mainForm)
                {
                    var tmpPersonal = item;
                    Program.m_objDbAccess.DeletePersonInfo(tmpPersonal.id, tmpPersonal.name, tmpPersonal.sid);
                    tmpPersonal.sid = Program.m_objActiveTermial.sid;
                    Program.m_objDbAccess.UpsertPersonInfo(tmpPersonal);

                    // 登録履歴にデータ追加
                    DbAccess.PersonLog log = new DbAccess.PersonLog();
                    log.init();
                    log.Set(tmpPersonal);
                    Program.m_objDbAccess.InsertPersonLog(log);
                }
            }
            Close();
        }

        private void listPersonal_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            // 描画はOSに任せる
            e.DrawDefault = true;
        }

        private void listPersonal_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            // 描画はOSに任せる
            e.DrawDefault = true;
        }
    }
}
