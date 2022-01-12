using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace ShelterInfoSystem
{
    public partial class FormCloseShelter : Form
    {
        public int m_RetBtn = 0;
        DbAccess.TerminalInfo m_info = new DbAccess.TerminalInfo();
        // QCID,Lat,Lon更新時のタイムアウト
        private const int GETQCID_TIMEOUT = 3; // (sec)

        public FormCloseShelter()
        {
            InitializeComponent();

            //string sId = Program.m_objShelterConfig.TermId;
            string sId = Program.m_objActiveTermial.sid;

            bool bExist = false;

            // DBよりデータを取得
            Program.m_objDbAccess.GetTerminalInfo(sId, out bExist, ref m_info);

            // データが存在する
            if (bExist)
            {
                // 取得データを画面表示
                txtShelterID.Text = m_info.gid;
                txtShelterName.Text = m_info.name;
                txtShelterLatitude.Text = m_info.lat;
                txtShelterLongitude.Text = m_info.lon;
            }
            else
            {
                // XMLデータを画面表示
                txtShelterID.Text = m_info.gid;
                txtShelterName.Text = m_info.name;
                txtShelterLatitude.Text = m_info.lat;
                txtShelterLongitude.Text = m_info.lon;
            }

            if (Program.m_EquStat.mQCID != null && Program.m_EquStat.mQCID != "")
            {
                // 衛星通信端末から避難所ID、経度緯度情報を再取得する
                txtShelterID.Text = Program.m_EquStat.mQCID;
                //txtShelterName.Text = m_info.name;
                txtShelterLatitude.Text = Program.m_EquStat.mLatitude.ToString();
                txtShelterLongitude.Text = Program.m_EquStat.mLongitude.ToString();
            }
        }

        public DbAccess.TerminalInfo GetTerminalInfo()
        {
            return m_info;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //string sId = Program.m_objShelterConfig.TermId;
            string sId = Program.m_objActiveTermial.sid;

            m_RetBtn = 1;
            try
            {
                double lat = double.Parse(txtShelterLatitude.Text);
                double lon = double.Parse(txtShelterLongitude.Text);

                if (lat < 0 || lon < 0)
                {
                    MessageBox.Show("緯度・経度が不正です。",
                        "緯度・経度 エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    m_RetBtn = 0;
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("緯度・経度が取得できませんでした。",
                    "緯度・経度 エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                m_RetBtn = 0;
                return;
            }

            m_info.sid = sId;
            m_info.open_flag = FormShelterInfo.SHELTER_STATUS.CLOSE;
            m_info.lat = txtShelterLatitude.Text;
            m_info.lon = txtShelterLongitude.Text;
            m_info.close_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            m_info.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            // 2016/04/20 送信成功後に更新するように移動
            // DBよりデータを取得
            //Program.m_objDbAccess.UpdateTerminalInfo(m_info);
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_RetBtn = 0;
            Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormCloseShelter", "btnUpdate_Click", "更新クリック");
            // 衛星通信端末から避難所ID、経度緯度情報を再取得する
            string qcid = this.txtShelterID.Text;
            string lat = this.txtShelterLatitude.Text;
            string lon = this.txtShelterLongitude.Text;

            Program.m_L1sRecv.sendReq(L1sReceiver.MSG_TYPE_CID);

            // 取得できたら表示
            Task task = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < GETQCID_TIMEOUT; i++)
                {
                    System.Threading.Thread.Sleep(1000);

                    if (qcid != Program.m_EquStat.mQCID
                        || lat != Program.m_EquStat.mLatitude.ToString()
                         || lon != Program.m_EquStat.mLongitude.ToString())
                    {
                        // 取得した
                        if (qcid != Program.m_EquStat.mQCID)
                        {
                            this.txtShelterID.Text = Program.m_EquStat.mQCID;
                        }
                        if (Program.m_EquStat.mLatitude >= 0 && lat != Program.m_EquStat.mLatitude.ToString())
                        {
                            this.txtShelterLatitude.Text = Program.m_EquStat.mLatitude.ToString();
                        }
                        if (Program.m_EquStat.mLongitude >= 0 && lon != Program.m_EquStat.mLongitude.ToString())
                        {
                            this.txtShelterLongitude.Text = Program.m_EquStat.mLongitude.ToString();
                        }

                        break;
                    }
                }
            });
        }
    }
}
