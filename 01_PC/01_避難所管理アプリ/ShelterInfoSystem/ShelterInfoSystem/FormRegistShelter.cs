using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShelterInfoSystem
{
    public partial class FormRegistShelter : Form
    {
        private bool isRegist = false; // 登録フラグ
        List<DbAccess.TerminalInfo> shelterInfoList = null; // 避難所情報
        private DbAccess.TerminalInfo m_RegisterInfo;    //登録した避難所情報

        private FormShelterInfo m_Mainform;
        private string m_gid;
        private string m_smid;

        /// <summary>
        /// ダイアログの結果(OK/キャンセル)
        /// </summary>
        /// <returns></returns>
        public bool GetDlgResult()
        {
            return isRegist;
        }

        /// <summary>
        /// ダイアログで設定した避難所情報
        /// </summary>
        /// <returns></returns>
        public DbAccess.TerminalInfo GetDlgShelterInfo()
        {
            return m_RegisterInfo;
        }

        /// <summary>
        /// ダイアログ起動時の処理
        /// </summary>
        /// <param name="_shelterInfoList"></param>
        /// <param name="Mainform"></param>
        public FormRegistShelter(List<DbAccess.TerminalInfo> _shelterInfoList, FormShelterInfo Mainform)
        {
            InitializeComponent();

            // 避難所情報
            shelterInfoList = _shelterInfoList;

            // メインフォーム情報
            m_Mainform = Mainform;

            // 情報を初期化
            m_RegisterInfo.init();

            // Q-ANPIターミナルIDが取得できない場合、仮IDで避難所登録する
            string tmpGID = "";
            //
            if ((Program.m_EquStat.mQCID == null)||(Program.m_EquStat.mQCID == ""))
            {
                tmpGID = Program.TEMP_QANPI_ID;
            }
            else
            {
                tmpGID = Program.m_EquStat.mQCID;
            }

            // SMIDは登録可能な番号の一番若い番号を表示する
            int newSmid = 0;
            for (int ic = 1; ic <= 255; ic++)
            {
                bool findSmid = false;
                foreach (var item in _shelterInfoList)
                {
                    if (item.gid == tmpGID)
                    {
                        if (item.smid == ic.ToString())
                        {
                            findSmid = true;
                            break;
                        }
                    }
                }
                if (!findSmid)
                {
                    newSmid = ic;
                    break;
                }
            }

            // 避難所IDを設定
            shelterID.Text = tmpGID;

            // 追番リストを作成
            for (int ic = 1; ic <= 255; ic++)
            {
                bool findSmid = false;
                foreach (var item in _shelterInfoList)
                {
                    if (item.gid == tmpGID)
                    {
                        if (item.smid == ic.ToString())
                        {
                            findSmid = true;
                            break;
                        }
                    }
                }
                if (!findSmid)
                {
                    comboBoxSMID.Items.Add(ic.ToString().PadLeft(3, '0'));
                }
                else
                {
                    if (newSmid == ic)
                    {
                        newSmid += 1;
                    }
                }
            }

            // smidと同じINDEXを選択させる
            for (int ic = 0; ic < comboBoxSMID.Items.Count - 1; ic++)
            {
                if (comboBoxSMID.Items[ic].ToString() == newSmid.ToString().PadLeft(3, '0'))
                {
                    comboBoxSMID.SelectedIndex = ic;
                }
            }
            m_gid = tmpGID;
            m_smid = newSmid.ToString("000");
        }

        private void FormRegistShelter_Load(object sender, EventArgs e)
        {
        }

        // キャンセル処理
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormRegistShelter", "btnCancel_Click", "キャンセル クリック");
            // 終了
            Close();
        }

        // 登録処理
        private void btnRegist_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormRegistShelter", "btnRegist_Click", "登録 クリック");

            // 入力エラーチェック
            //避難所名は入力されているか(スペース文字も禁止)
            if ((txtShelterName.Text == "")||(string.IsNullOrWhiteSpace(txtShelterName.Text)))
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormRegistShelter", "btnRegist_Click", "避難所名が入力されていません。");
                MessageBox.Show("避難所名が入力されていません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
            // 登録する避難所情報を変数に格納
            DbAccess.TerminalInfo NewTernimalInfo = new DbAccess.TerminalInfo();
            NewTernimalInfo.sid = shelterID.Text + "-" + comboBoxSMID.Text;
            NewTernimalInfo.gid = m_gid;
            NewTernimalInfo.smid = comboBoxSMID.Text;
            NewTernimalInfo.name = txtShelterName.Text;
            NewTernimalInfo.lat = "0.00";
            NewTernimalInfo.lon = "0.00";
            NewTernimalInfo.open_flag = FormShelterInfo.SHELTER_STATUS.NO_OPEN;
            NewTernimalInfo.open_datetime = "";
            NewTernimalInfo.close_datetime = "";
            NewTernimalInfo.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            NewTernimalInfo.memo = txtMemo.Text;
            NewTernimalInfo.status = "";
            NewTernimalInfo.text_flag = "1";
            NewTernimalInfo.dummy_num = "";
            NewTernimalInfo.dummy_num_flag = false;

            m_RegisterInfo = NewTernimalInfo;

            isRegist = true;

            Close();
        }

        /// <summary>
        /// 避難所リストから該当のSMIDを持つ避難所が存在するかどうか
        /// </summary>
        /// <param name="shelterInfoList"></param>
        /// <param name="smid"></param>
        /// <returns></returns>
        private bool isIncludedSMID_ShelterInfo(List<DbAccess.TerminalInfo> shelterInfoList, int smid)
        {
            bool retCode = false;
            foreach (var item in shelterInfoList)
            {
                if (item.smid == smid.ToString())
                {
                    retCode = true;
                    break;
                }
            }
            return retCode;
        }
    }
}
