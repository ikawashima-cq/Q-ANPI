/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2017. All rights reserved.
 */
/**
 * @file    FormSendShelterInfo.cs
 * @brief   避難所情報送信確認画面フォーム
 */
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
    /**
     * @class FormSendShelterInfo
     * @brief 避難所情報送信確認画面フォームクラス
     */
    public partial class FormSendShelterInfo : Form
    {
        private int m_EntryNum = 0;
        private bool m_RetBtn = false;
        private DbAccess.TerminalInfo m_info = new DbAccess.TerminalInfo();

        // QCID,Lat,Lon更新時のタイムアウト
        private const int GETQCID_TIMEOUT = 3; // (sec)
        private int personCnt = -1;

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
            return m_info;
        }

        /**
         * @brief コンストラクタ
         */
        public FormSendShelterInfo(int _personCnt = -1)
        {
            InitializeComponent();

            if (_personCnt != -1)
            {
                personCnt = _personCnt;
            }
        }

        /**
         * @brief フォーム呼び出し時処理
         */
        private void FormSendShelterInfo_Load(object sender, EventArgs e)
        {
            string sId = Program.m_objActiveTermial.sid;
            bool bExist = false;

            // DBよりデータを取得
            Program.m_objDbAccess.GetTerminalInfo(sId, out bExist, ref m_info);

            // データが存在する
            // 避難所ID表示
            // 避難所名表示
            // 緯度表示
            // 経度表示
            // 開設状態表示

            // 取得データを画面表示
            txtShelterID.Text = m_info.sid;
            txtShelterName.Text = m_info.name;
            txtShelterLatitude.Text = m_info.lat;
            txtShelterLongitude.Text = m_info.lon;
        }

        /**
         * 避難者数セット
         */
        public void SetEntryNum(int num)
        {
            if (personCnt != -1)
            {
                txtEntryNum.Text = personCnt.ToString();
            }
            else
            {
                // 避難者登録人数表示
                txtEntryNum.Text = num.ToString();
            }
        }

        public void SetOpenDate(string opendate)
        {
            // 開設時間表示
            txtOpenDate.Text = opendate;
        }


        /**
         * @brief キャンセルボタン押下時処理
         */
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormSendShelterInfo", "btnCancel_Click", "キャンセル　クリック");
            Close();
        }

        /**
         * @brief 送信ボタン押下時処理
         */
        private void btnSend_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormSendShelterInfo", "btnCancel_Click", "送信　クリック");
            
            try
            {
                double lat = double.Parse(txtShelterLatitude.Text);
                double lon = double.Parse(txtShelterLongitude.Text);

                if (lat < 0 || lon < 0)
                {
                    MessageBox.Show("緯度・経度が不正です。位置情報更新を実行してください。",
                        "緯度・経度 エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormSendShelterInfo", "btnCancel_Click", "緯度・経度 エラー");
                    m_RetBtn = false;
                    Close();
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("緯度・経度が取得できませんでした。位置情報更新を実行してください。",
                    "緯度・経度 エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormSendShelterInfo", "btnCancel_Click", "緯度・経度 エラー");
                m_RetBtn = false;
                Close();
                return;
            }

            try
            {
                // ダイアログに表示していた人数(=任意の避難者数 or 登録避難者数)をセット
                m_EntryNum = int.Parse(txtEntryNum.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("避難者数が不正です。入力した避難者数を確認してください。",
                    "緯度・経度 エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormSendShelterInfo", "", "避難者数不正");
                m_RetBtn = false;
                Close();
                return;
            }

            m_RetBtn = true;
            Close();
        }

        /**
         * @brief 更新ボタン押下時処理
         */
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // 現在未使用
            /*
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormSendShelterInfo", "btnUpdate_Click", "更新クリック");
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
                        if (Program.m_EquStat.mLatitude >= 0 && lat != Program.m_EquStat.mLatitude.ToString())
                        {
                            this.txtShelterLatitude.Text = Program.m_EquStat.mLatitude.ToString();
                        }
                        if (Program.m_EquStat.mLongitude >= 0 && lon != Program.m_EquStat.mLongitude.ToString())
                        {
                            this.txtShelterLongitude.Text = Program.m_EquStat.mLongitude.ToString();
                        }
                        btnSend.Enabled = true;
                        break;
                    }
                }
            });
             * */
        }

    }
}
