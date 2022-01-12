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
    public partial class FormCloseShelter2 : Form
    {
        private bool isClose = false; // 閉鎖フラグ
        private List<DbAccess.TerminalInfo> shelterInfoList = null; // 避難所情報
        private DbAccess.TerminalInfo closeShelter = new DbAccess.TerminalInfo(); // 閉鎖する避難所

        /// <summary>
        /// ダイアログの結果(OK/キャンセル)
        /// </summary>
        /// <returns></returns>
        public bool GetDlgResult()
        {
            return isClose;
        }

        /// <summary>
        /// ダイアログで設定した避難所情報
        /// </summary>
        /// <returns></returns>
        public DbAccess.TerminalInfo GetDlgShelterInfo()
        {
            return closeShelter;
        }


        /// <summary>
        /// ダイアログ起動時処理
        /// </summary>
        /// <param name="_shelterInfoList"></param>
        /// <param name="currentShelter"></param>
        public FormCloseShelter2(List<DbAccess.TerminalInfo> _shelterInfoList,string currentShelter)
        {
            InitializeComponent();

            // 避難所情報
            shelterInfoList = _shelterInfoList;

            // 避難所情報を取得
            foreach (var info in shelterInfoList)
            {
                // メイン画面でアクティブ状態になっている避難所を選択状態にする
                if (info.sid == Program.m_objActiveTermial.sid)
                {
                    // 避難所IDを設定
                    txtShelterID.Text = info.sid;

                    // 避難所名を設定
                    lblShelterName.Text = info.name;

                    // 緯度経度を設定
                    txtShelterLatitude.Text = info.lat;
                    txtShelterLongitude.Text = info.lon;
                }
            }
        }

        /// <summary>
        /// 閉鎖ボタン押下時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormCloseShelter2", "btnClose_Click", "閉鎖　クリック");

            // 閉鎖情報を更新
            for (int i = 0; i < shelterInfoList.Count; i++)
            {
                if (shelterInfoList[i].sid == txtShelterID.Text)
                {
                    // ダイアログにユーザが入力した内容でDBの避難所情報データを更新する
                    DbAccess.TerminalInfo tmpInfo = shelterInfoList[i];
                    tmpInfo.lat = txtShelterLatitude.Text;
                    tmpInfo.lon = txtShelterLongitude.Text;
                    tmpInfo.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    tmpInfo.close_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    tmpInfo.open_flag = FormShelterInfo.SHELTER_STATUS.CLOSE;

                    // Q-ANPIターミナルに閉鎖した避難所の情報を送信する
                    closeShelter = tmpInfo;

                    isClose = true;

                    break;
                }
            }

            // 終了
            Close();
        }

        /// <summary>
        /// キャンセル
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 終了
            Close();
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormCloseShelter2", "btnCancel_Click", "キャンセル　クリック");
        }
    }
}
