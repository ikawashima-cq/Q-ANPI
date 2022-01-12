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
    public partial class FormOpenShelter : Form
    {
        /* // 現在未使用
        // QCID,Lat,Lon更新時のタイムアウト
        private const int GETQCID_TIMEOUT = 3; // (sec)

        public int m_RetBtn = 0;
        private DbAccess.TerminalInfo m_info = new DbAccess.TerminalInfo();

        public FormOpenShelter()
        {
            InitializeComponent();

            string sId = "";
            if (String.IsNullOrEmpty(Program.m_objShelterConfig.TermId) == false )
            {
                sId = Program.m_objShelterConfig.TermId;
            }

            bool bExist = false;

            if (Program.m_objShelterConfig.GID != null)
            {
                m_info.init();

                m_info.sid = Program.m_objShelterConfig.TermId;
                m_info.gid = Program.m_objShelterConfig.GID;
                m_info.name = Program.m_objShelterConfig.ShelterName;
                m_info.lat = Program.m_objShelterConfig.Lat;
                m_info.lon = Program.m_objShelterConfig.Lon;
                m_info.open_flag = FormShelterInfo.SHELTER_STATUS.CLOSE;

                // 取得データを画面表示
                txtShelterID.Text = m_info.gid;
                txtShelterName.Text = m_info.name;
                txtShelterLatitude.Text = m_info.lat;
                txtShelterLongitude.Text = m_info.lon;
            }
            else
            {
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
            }

            // step2
            if (Program.m_EquStat.mQCID != null && Program.m_EquStat.mQCID != "")
            {
                // 衛星通信端末から避難所ID、経度緯度情報を再取得する
                txtShelterID.Text = Program.m_EquStat.mQCID;
                //txtShelterName.Text = m_info.name;
                txtShelterLatitude.Text = Program.m_EquStat.mLatitude.ToString();
                txtShelterLongitude.Text = Program.m_EquStat.mLongitude.ToString();
                btnOpen.Enabled = true;
            }
            else
            {
                // データが未受信の場合は空白表示し、送信ボタンをDisableに
                txtShelterID.Text = "";
                txtShelterLatitude.Text = "";
                txtShelterLongitude.Text = "";
                btnOpen.Enabled = false;
            }
        }

        public DbAccess.TerminalInfo GetTerminalInfo()
        {
            return m_info;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormOpenShelter", "btnOpen_Click", "開設クリック");

            // 戻ったときに端末情報（開設）を送信するかどうか
            m_RetBtn = 1;

            try
            {
                double lat = double.Parse(txtShelterLatitude.Text);
                double lon = double.Parse(txtShelterLongitude.Text);

                if (lat < 0 || lon < 0)
                {
                    MessageBox.Show("緯度・経度が取得できませんでした。",
                        "緯度・経度 エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    m_RetBtn = 0;
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("緯度・経度が不正です。",
                    "緯度・経度 エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                m_RetBtn = 0;
                return;
            }
            string sId = Program.m_objShelterConfig.TermId;

            m_info.sid = sId;
            m_info.gid = txtShelterID.Text;
            m_info.name = txtShelterName.Text;
            m_info.lat = txtShelterLatitude.Text;
            m_info.lon = txtShelterLongitude.Text;
            m_info.open_flag = FormShelterInfo.SHELTER_STATUS.OPEN;
            m_info.open_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            m_info.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            // 201604/14 送信成功後に更新するように移動
            // 移動 DBよりデータを取得
            // Program.m_objDbAccess.UpdateTerminalInfo(m_info);

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_RetBtn = 0;
            Close();
        }

        private void txtShelterLongitude_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPressEvent(sender, e);
        }

        private void txtShelterLatitude_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPressEvent(sender, e);
        }

        private void txtShelterID_KeyPress(object sender, KeyPressEventArgs e)
        {

            //// 制御文字は入力可
            //if (char.IsControl(e.KeyChar))
            //{
            //    e.Handled = false;
            //    return;
            //}

            //if (e.KeyChar < 'A' || 'Z' < e.KeyChar)
            //{
            //    // 上記以外は入力不可
            //    e.Handled = true;
            //}


        }

        private void KeyPressEvent(object sender, KeyPressEventArgs e)
        {

            // 制御文字は入力可
            if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
                return;
            }

            //// 数字(0-9)は入力可
            //if (e.KeyChar < '0' || '9' < e.KeyChar)
            //{
            //    // 上記以外は入力不可
            //    e.Handled = true;
            //}

            // 数字(0-9)は入力可
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
                return;
            }
            // 小数点は１つだけ入力可
            if (e.KeyChar == '.')
            {
                TextBox target = sender as TextBox;
                if (target.Text.IndexOf('.') < 0)
                {
                    // 複数のピリオド入力はNG
                    e.Handled = false;
                    return;
                }
                else
                {
                    e.Handled = true;

                }
            }
            else
            {
                e.Handled = true;
            }

        }

        //
        // @brief : 更新ボタン押下
        ///
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormOpenShelter", "btnUpdate_Click", "更新クリック");
            // 衛星通信端末から避難所ID、経度緯度情報を再取得する
            string qcid = this.txtShelterID.Text;
            string lat = this.txtShelterLatitude.Text;
            string lon = this.txtShelterLongitude.Text;
            Program.m_L1sRecv.sendReq(L1sReceiver.MSG_TYPE_CID);

            // 取得できたら表示
            Task task = Task.Factory.StartNew(() => {
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
                        btnOpen.Enabled = true;
                        break;
                    }
                }
            });
        }
         */
    }
}
