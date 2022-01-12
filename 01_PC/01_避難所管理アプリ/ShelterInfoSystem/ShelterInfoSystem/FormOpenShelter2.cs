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
    public partial class FormOpenShelter2 : Form
    {
        private bool isOpen = false; // 開設フラグ
        private List<DbAccess.TerminalInfo> shelterInfoList = null; // 避難所情報
        private DbAccess.TerminalInfo openShelter = new DbAccess.TerminalInfo(); // 開設する避難所

        /// <summary>
        /// ダイアログの結果(OK/キャンセル)
        /// </summary>
        /// <returns></returns>
        public bool GetDlgResult()
        {
            return isOpen;
        }

        /// <summary>
        /// ダイアログで設定した避難所情報
        /// </summary>
        /// <returns></returns>
        public DbAccess.TerminalInfo GetDlgShelterInfo()
        {
            return openShelter;
        }

        /// <summary>
        /// ダイアログ起動時処理
        /// </summary>
        /// <param name="_shelterInfoList"></param>
        public FormOpenShelter2(List<DbAccess.TerminalInfo> _shelterInfoList)
        {
            InitializeComponent();

            // DB内の全避難所情報を取得
            DbAccess.TerminalInfo[] AllTernimalInfo = new DbAccess.TerminalInfo[0];
            Program.m_objDbAccess.GetTerminalInfoAll(ref AllTernimalInfo);

            // 避難所情報
            shelterInfoList = _shelterInfoList;

            // 避難所管理画面で選択中の避難所情報を表示する
            foreach (var item in _shelterInfoList)
            {
                if (item.sid == Program.m_objActiveTermial.sid)
                {
                    // 避難所ID
                    txtShelterID.Text = item.sid;

                    // 避難所名
                    lblShelterName.Text = item.name;

                    // 緯度経度を設定(Q-ANPIターミナルの緯度経度を取得)
                    txtShelterLatitude.Text = Program.m_EquStat.mLatitude.ToString();
                    txtShelterLongitude.Text = Program.m_EquStat.mLongitude.ToString();
                }
            }
        }

        /// <summary>
        /// キャンセル処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 終了
            Close();
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormOpenShelter2", "btnCancel_Click", "キャンセル　クリック");
        }

        /// <summary>
        /// 開設処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormOpenShelter2", "btnOpen_Click", "開設　クリック");

            // 開設情報を更新
            for (int i = 0; i < shelterInfoList.Count; i++)
            {
                if (shelterInfoList[i].sid == txtShelterID.Text)
                {
                    DbAccess.TerminalInfo tmpInfo = shelterInfoList[i];


                    // 閉鎖状態の避難所を開設したとき、前の情報をクリアするかどうか確認する
                    if (tmpInfo.open_flag == FormShelterInfo.SHELTER_STATUS.CLOSE)
                    {
                        FormConfirmOpenShelter frmComfirmOpenShelter = new FormConfirmOpenShelter();
                        frmComfirmOpenShelter.ShowDialog();


                        if (frmComfirmOpenShelter.isCanceled)
                        {
                            // 確認ダイアログでキャンセルが押された場合、開設処理を中断
                            Close();
                            return;
                        }

                        if (frmComfirmOpenShelter.isClearInfomation)
                        {
                            // 確認ダイアログでクリアするを選択した場合、DBをクリアする

                            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "", "情報クリア処理開始");

                            // 避難所情報のクリア
                            Program.m_mainForm.ClearShelterInfo(tmpInfo);

                            //2020.03.27 Add : 開設の応答受信時にtmpInfoの内容でDB登録するため、DBと一致させる。
                            bool bExist = false;
                            Program.m_objDbAccess.GetTerminalInfo(tmpInfo.sid, out bExist, ref tmpInfo);
                        }
                    }

                    // ダイアログに表示された内容で避難所開設用の一時データを生成する
                    tmpInfo.lat = txtShelterLatitude.Text;
                    tmpInfo.lon = txtShelterLongitude.Text;
                    tmpInfo.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    tmpInfo.open_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    tmpInfo.open_flag = FormShelterInfo.SHELTER_STATUS.OPEN;

                    // 外部受け渡し用変数に格納
                    openShelter = tmpInfo;

                    isOpen = true;

                    break;
                }
            }

            // 終了
            Close();
        }
    }
}