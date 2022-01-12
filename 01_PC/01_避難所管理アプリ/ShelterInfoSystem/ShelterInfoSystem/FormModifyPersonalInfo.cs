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
    public partial class FormModifyPersonalInfo : Form
    {
        public List<DbAccess.TerminalInfo> shelterInfoList = null; // 避難所情報
        private List<DbAccess.PersonInfo> personInfo = new List<DbAccess.PersonInfo>();

        private DbAccess.TerminalInfo m_selectShelter;

        private List<DbAccess.TerminalInfo> sourceShelterListDetail = new List<DbAccess.TerminalInfo>();
        private List<DbAccess.TerminalInfo> destShelterListDetail = new List<DbAccess.TerminalInfo>();

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="_shelterInfoList"></param>
        /// <param name="selectShelter"></param>
        public FormModifyPersonalInfo(List<DbAccess.TerminalInfo> _shelterInfoList, DbAccess.TerminalInfo selectShelter)
        {
            InitializeComponent();

            shelterInfoList = _shelterInfoList;
            m_selectShelter = selectShelter;

            // 避難所管理画面で選択中の避難所情報を表示する
            int count = -1;
            int openCount = 0;
            bool findSelectedShelter = false;
            foreach (var item in shelterInfoList.Select((value, index) => new { value, index }))
            {
                
                // 登録元/先避難所一覧リストは「開設」状態になっているもののみ表示する
                if (item.value.open_flag == FormShelterInfo.SHELTER_STATUS.OPEN)
                {
                    sourceShelterList.Items.Add(item.value.name);
                    destShelterList.Items.Add(item.value.name);
                    sourceShelterListDetail.Add(item.value);
                    destShelterListDetail.Add(item.value);

                    count++;
                    openCount++;

                    // ダイアログ開始時の登録先避難所は、現在選択中の避難所とする
                    if (item.value.sid == m_selectShelter.sid)
                    {
                        sourceShelterList.SelectedIndex = count;
                        destShelterList.SelectedIndex = count;
                        findSelectedShelter = true;
                    }
                }
            }

            // 「開設」の避難所が存在しない場合、何も表示しない
            if (openCount > 0)
            {
                // 現在選択中の避難所が開設状態でない場合、デフォルト選択は一番若いIDの避難所とする
                if (!findSelectedShelter)
                {
                    sourceShelterList.SelectedIndex = 0;
                    destShelterList.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// 登録元避難所プルダウン変更時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sourceShelterList_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateListPersonal();
        }

        /// <summary>
        /// チェックボックス状態チェック
        /// </summary>
        /// <returns></returns>
        private bool checkRegist()
        {
            if (listPersonInfo.CheckedItems.Count == 0)
            {
                MessageBox.Show("修正する情報にチェックを入れて下さい。", "避難所修正", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (sourceShelterList.SelectedIndex == destShelterList.SelectedIndex)
            {
                MessageBox.Show("同じ避難所には登録できません。\n登録先避難所名を変更して下さい。", "避難所修正", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 個人安否情報リスト更新処理
        /// </summary>
        private void updateListPersonal()
        {
            listPersonInfo.Items.Clear();

            string shelterID = String.Empty;

            int shIndex = sourceShelterList.SelectedIndex;

            shelterID = sourceShelterListDetail[shIndex].sid;
            
            Program.m_objDbAccess.GetPersonInfoList(shelterID, ref personInfo);

            // 更新日時でソート
            personInfo.Sort((b, a) => a.update_datetime.CompareTo(b.update_datetime));
            
            foreach (var item in personInfo.Select((value, index) => new { value, index }))
            {
                string[] selInfo = { (item.index + 1).ToString(), item.value.name, item.value.id, item.value.update_datetime };
                listPersonInfo.Items.Add(new ListViewItem(selInfo));
            }
        }

        /// <summary>
        /// 登録ボタン押下時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (!checkRegist())
            {
                return;
            }

            string srcShelterID = String.Empty;
            string destShelterID = String.Empty;

            // 登録元避難所取得
            int shIndex = sourceShelterList.SelectedIndex;
            srcShelterID = sourceShelterListDetail[shIndex].sid;

            // 登録先避難所取得
            int deIndex = destShelterList.SelectedIndex;
            destShelterID = destShelterListDetail[deIndex].sid;

            List<int> CheckedItemsIndex = new List<int>();

            foreach (ListViewItem item in listPersonInfo.CheckedItems)
            {
                CheckedItemsIndex.Add(int.Parse(item.Text) - 1);
            }

            foreach (int index in CheckedItemsIndex)
            {
                // 変更前の情報を取得
                DbAccess.PersonInfo updatePersonInfo = personInfo[index];

                // 避難所IDを変更後のIDに設定
                updatePersonInfo.sid = destShelterID;

                // 避難所変更前のデータを削除
                Program.m_objDbAccess.DeletePersonInfo(personInfo[index].id, personInfo[index].name, personInfo[index].sid);

                // 避難所変更後のデータを登録
                Program.m_objDbAccess.UpsertPersonInfo(updatePersonInfo);

                // 変更後の避難所で登録履歴をつける
                DbAccess.PersonLog updatePersonInfoLog = new DbAccess.PersonLog();
                updatePersonInfoLog.Set(updatePersonInfo);
                Program.m_objDbAccess.InsertPersonLog(updatePersonInfoLog);
            }

            Close();
        }

        /// <summary>
        /// キャンセルボタン押下時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
