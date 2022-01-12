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
    public partial class FormUpdateLocationInfo : Form
    {
        private bool isUpdate = false; // 更新フラグ
        private List<DbAccess.TerminalInfo> shelterInfoList = null; // 避難所情報
        private FormShelterInfo m_mainForm;
        private DbAccess.TerminalInfo m_updateInfo;

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
            return m_updateInfo;
        }

        /// <summary>
        /// ダイアログ起動時処理
        /// </summary>
        /// <param name="_shelterInfoList"></param>
        /// <param name="name"></param>
        /// <param name="mainForm"></param>
        public FormUpdateLocationInfo(List<DbAccess.TerminalInfo> _shelterInfoList, string name, FormShelterInfo mainForm)
        {
            InitializeComponent();

            shelterInfoList = _shelterInfoList;
            m_mainForm = mainForm;

            shelterName.Text = name;
            foreach (var info in shelterInfoList)
            {
                if (info.sid == Program.m_objActiveTermial.sid)
                {
                    shelterID.Text = info.sid;
                    break;
                }
            }
        }

        // キャンセル処理
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormUpdateLocationInfo", "btnCancel_Click", "キャンセル　クリック");
            Close();
        }

        // 更新処理
        private void btnRegist_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormUpdateLocationInfo", "btnRegist_Click", "更新　クリック");
            
            // 対象となる避難所の位置情報を更新
            for (int i = 0; i < shelterInfoList.Count; i++)
            {
                if (shelterInfoList[i].sid == shelterID.Text)
                {
                    // Q-ANPIターミナルから取得した経度緯度でDBの避難所情報データを更新する
                    m_updateInfo = shelterInfoList[i];
                    m_updateInfo.lat = Program.m_EquStat.mLatitude.ToString();
                    m_updateInfo.lon = Program.m_EquStat.mLongitude.ToString();

                    isUpdate = true;
                    break;
                }
            }

            Close();
        }
    }
}
