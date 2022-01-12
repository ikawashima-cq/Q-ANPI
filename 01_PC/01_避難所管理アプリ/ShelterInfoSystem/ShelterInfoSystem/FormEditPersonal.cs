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
    public partial class FormEditPersonal : Form
    {
        // 2016/04/16 DB更新処理をShelterInfoSystemに移動
        private bool m_RetBtn = false;
        private DbAccess.PersonInfo m_info;
        private int m_mode = 0;
        private DbAccess.PersonInfo m_selectInfo;

        /// <summary>
        /// ダイアログの結果(OK/キャンセル)
        /// </summary>
        /// <returns></returns>
        public bool GetDlgResult()
        {
            return m_RetBtn;
        }

        /// <summary>
        /// ダイアログで設定した個人安否情報を取得
        /// </summary>
        /// <returns></returns>
        public DbAccess.PersonInfo GetDlgPersonInfo()
        {
            return m_info;
        }

        /// <summary>
        /// ダイアログ起動時処理
        /// </summary>
        /// <param name="selectInfo"></param>
        public FormEditPersonal(DbAccess.PersonInfo selectInfo)
        {
            m_selectInfo = selectInfo;
            InitializeComponent();

            // 右クリックメニューを非表示にする
            txtTel.ContextMenu = new ContextMenu();
            txtName1.ContextMenu = new ContextMenu();
            txtName2.ContextMenu = new ContextMenu();
        }

        /// <summary>
        /// ダイアログ初期化処理
        /// </summary>
        /// <param name="info"></param>
        /// <param name="mode"></param>
        public void init(DbAccess.PersonInfo info,int mode)
        {
            // 設定
            m_info = info;

            // 呼び出しモード(0=新規、1=編集)
            m_mode = mode;

            txtTel.Text = m_info.id;
            //名前 -> 姓名を分離
            //txtName1.Text = m_info.name;
            string[] strName = m_info.name.ToString().Split('　');
            if (strName.Length > 1)
            {
                txtName1.Text = strName[0];
                txtName2.Text = strName[1];
            }
            else
            {
                // スペースがない（旧データ）場合は苗字に詰め込む
                // はみ出した分は名前に詰め込む
                if (strName[0].Length > 12)
                {
                    Array.Resize(ref strName, strName.Length + 1);
                    strName[strName.Length - 1] = strName[0].Substring(13);
                    txtName1.Text = strName[0];
                    txtName2.Text = strName[1];
                }
                else
                {
                    txtName1.Text = strName[0];
                    txtName2.Text = "";
                }
            }
            
            //住所
            txtAddress.Text = m_info.txt01;

            //sel02	入/退所  0:入所 1:退所 2:在宅
            switch( m_info.sel02 )
            {
                case "0":
                    rdbEnter.Checked = true;
                    break;
                case "1":
                    rdbRelese.Checked = true;
                    break;
                case "2":
                    rdbHome.Checked = true;
                    break;
            }
            
            //sel01	性別       0:男性 1:女性
            switch( m_info.sel01 )
            {
                case "0":
                    rdbMan.Checked = true;
                    break;
                case "1":
                    rdbWomen.Checked = true;
                    break;
            }
            
            //sel03	公表       0:しない 1:する
            switch( m_info.sel03 )
            {
                case "1":
                    rdbPublic.Checked = true;
                    break;
                case "0":
                    rdbPrivate.Checked = true;
                    break;
            }

            //sel04	怪我       0:無 1:有 2:未使用 3:未選択
            switch( m_info.sel04 )
            {
                case "0":
                    rdbNoInjuries.Checked = true;
                    break;
                case "1":
                    rdbInjuries.Checked = true;
                    break;
                default:
                    break;
            }

            //sel05	介護       2:未選択 0:否  1:要
            switch( m_info.sel05 )
            {
                case "0":
                    radioNonCare.Checked = true;
                    break;
                case "1":
                    radioCare.Checked = true;
                    break;
            }

            //sel06	障がい    2:未選択 0:無  1:有
            switch( m_info.sel06 )
            {
                case "0":
                    radioButton2.Checked = true;
                    break;
                case "1":
                    radioButton1.Checked = true;
                    break;
            }

            //sel07	妊産婦    2:未選択 0:いいえ  1:はい
            switch( m_info.sel07 )
            {
                case "0":
                    radioButton3.Checked = true;
                    break;
                case "1":
                    radioButton4.Checked = true;
                    break;
            }
        }

        /// <summary>
        /// 登録ボタン押下時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEntry_Click(object sender, EventArgs e)
        {
            string strName = m_info.name;
            string strId = m_info.id;

            // 個人安否情報登録時に避難所IDを設定する
            // メイン画面でアクティブになっている避難所を取得し、これを設定する
            m_info.sid = Program.m_objActiveTermial.sid;

            // 電話番号
            m_info.id = txtTel.Text;
            // 名前
            m_info.name = txtName1.Text + "　" + txtName2.Text;
            // 住所
            m_info.txt01 = txtAddress.Text;

            //sel02	入/退所  0:入所 1:退所 2:在宅
            if (rdbEnter.Checked)
            {
                m_info.sel02 = "0";
            }
            else if (rdbRelese.Checked)
            {
                m_info.sel02 = "1";
            }
            else if (rdbHome.Checked)
            {
                m_info.sel02 = "2";
            }

            //生年月日
            // 未選択の場合、null
            if (comboYear.SelectedItem != null && comboMonth.SelectedItem != null && comboDay.SelectedItem != null)
            {
                m_info.txt02 = comboYear.SelectedItem.ToString();
                // 生年月日のフォーマットを[yyyy-mm-dd]に合わせる
                m_info.txt02 += "-" + comboMonth.SelectedItem.ToString().PadLeft(2, '0');
                m_info.txt02 += "-" + comboDay.SelectedItem.ToString().PadLeft(2, '0');
            }
            else
            {
                m_info.txt02 = "";
            }

            //sel01	性別       0:男性 1:女性
            if (rdbMan.Checked)
            {
                m_info.sel01 = "0";
            }
            else if (rdbWomen.Checked)
            {
                m_info.sel01 = "1";
            }
            else
            {
                m_info.sel01 = "";
            }

            //sel03	公表       0:しない 1:する
            if (rdbPublic.Checked)
            {
                m_info.sel03 = "1";
            }
            else if (rdbPrivate.Checked)
            {
                m_info.sel03 = "0";
            }
            else
            {
                m_info.sel03 = "";
            }

            //sel04	怪我       0:無 1:有 2:未使用 3:未選択

            if (rdbNoInjuries.Checked)
            {
                m_info.sel04 = "0";
            }
            else if (rdbInjuries.Checked)
            {
                m_info.sel04 = "1";
            }
            else
            {
                m_info.sel04 = "3";
            }

            //sel05	介護       2:未選択 0:否  1:要
            if (radioNonCare.Checked)
            {
                m_info.sel05 = "0";
            }
            else if (radioCare.Checked)
            {
                m_info.sel05 = "1";
            }
            else
            {
                m_info.sel05 = "2";
            }

            //sel06	障がい    2:未選択 0:無  1:有
            if (radioButton2.Checked)
            {
                m_info.sel06 = "0";
            }
            else if (radioButton1.Checked)
            {
                m_info.sel06 = "1";
            }
            else
            {
                m_info.sel06 = "2";
            }

            //sel07	妊産婦    2:未選択 0:いいえ  1:はい
            if (radioButton3.Checked)
            {
                m_info.sel07 = "0";
            }
            else if (radioButton4.Checked)
            {
                m_info.sel07 = "1";
            }
            else
            {
                m_info.sel07 = "2";
            }

            //sel08 避難所内外 0:内　1:外
            // 画面上では編集不可　登録時の情報を保持し続ける
            //（PCで登録→「内」、スマホで登録→スマホ内に「内」「外」設定項目有り、インポート→項目内に「内」「外」項目有り）
            if (m_info.sel08 == "")
            {
                // 新規作成の場合は「内」に設定する
                m_info.sel08 = "0";
            }

            // 更新日時
            m_info.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            m_info.id = txtTel.Text;

            // 必須入力チェック
            String strMessage = "";
            if (m_info.id == "")
                strMessage += "電話番号を入力してください。\n";
            if ((m_info.name == "　")||(string.IsNullOrWhiteSpace(m_info.name)))
                strMessage += "名前を入力してください。\n";
            if (m_info.txt02 == "")
                strMessage += "生年月日を入力してください。\n";
            if (m_info.sel01 == "")
                strMessage += "性別を選択してください。\n";
            if (m_info.sel02 == "")
                strMessage += "入所/退所を選択してください。\n";
            if (m_info.sel03 == "")
                strMessage += "公表の可否を選択してください。\n";

            // 未入力の必須入力項目が存在する
            if (strMessage != "")
            {
                MessageBox.Show(strMessage, "避難所システム", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "person", "登録画面 必須入力未入力で登録失敗");
                return;
            }

            // 重複チェック---
            // 個人安否情報 取得
            List<DbAccess.PersonInfo> infoList = new List<DbAccess.PersonInfo>();
            Program.m_objDbAccess.GetPersonInfoList(Program.m_objActiveTermial.sid, ref infoList);

            // 重複チェック
            bool bExist;
            Program.m_objDbAccess.ExistPersonInfo(m_info, out bExist);
            List<KeyValuePair<string, string>> chkInfoPair = new List<KeyValuePair<string, string>>();
            foreach (var tmpList in infoList)
            {
                KeyValuePair<string, string> tmpPair = new KeyValuePair<string, string>(tmpList.id, tmpList.name);
                chkInfoPair.Add(tmpPair);
            }
            KeyValuePair<string, string> thisPair = new KeyValuePair<string, string>(m_info.id, m_info.name);

            // 新規登録と編集でチェック内容が異なる
            if (m_mode == 0)
            {
                // 新規作成時エラーチェック：DBに存在するかつリストに存在する場合、登録エラー
                if ((bExist) && (chkInfoPair.Contains(thisPair)))
                {
                    MessageBox.Show("すでに登録されている氏名と電話番号は登録できません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "person", "登録画面 同ID同名で登録失敗");
                    return;
                }
            }
            else
            {
                // 編集時エラーチェック：自分以外で同じ番号&氏名を持つものがあった場合、エラー
                KeyValuePair<string, string> selectPair = new KeyValuePair<string, string>(m_selectInfo.id, m_selectInfo.name);

                int sameCount = 0;
                foreach (var item in chkInfoPair)
                {
                    if (!item.Equals(selectPair))
                    {
                        if (item.Equals(thisPair))
                        {
                            sameCount++;
                        }
                    }
                }
                if (sameCount > 0)
                {
                    MessageBox.Show("すでに登録されている氏名と電話番号は登録できません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "person", "登録画面 同ID同名で登録失敗");
                    return;
                }
            }
            // 重複チェック---

            m_RetBtn = true;
            Close();
        }

        /// <summary>
        /// キャンセルボタン押下時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_RetBtn = false;
            Close();
        }

        /// <summary>
        /// 電話番号テキストボックス、キー入力時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTel_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPressEvent(e);
        }

        /// <summary>
        /// キー入力時イベント
        /// </summary>
        /// <param name="e"></param>
        private void KeyPressEvent(KeyPressEventArgs e)
        {
            // 制御文字は入力可
            if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            // 入力可能文字は0～9のみ
            else if ('0' <= e.KeyChar && e.KeyChar <= '9')
            {
                e.Handled = false;
            }
            // バックスペース(文字の削除)を許可
            else if (e.KeyChar == '\b')
            {
                e.Handled = false;
            }
            // 上記以外は入力不可
            else
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// ダイアログロード時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormEditPersonal_Load(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            // Year - 現在年 ～　120年
            for (int i = 0; i <= 120; i++)
            {
                comboYear.Items.Add(dt.Year - i);
            }

            // Month
            for (int i = 0; i < 12; i++)
            {
                comboMonth.Items.Add(i + 1);
            }

            // Day
            for (int i = 0; i < 31; i++)
            {
                comboDay.Items.Add(i + 1);
            }

            // 初期選択決定
            string[] strBirth = m_info.txt02.ToString().Split('-');
            if (strBirth.Length == 3)
            {
                // 年
                int year = int.Parse(strBirth[0]);
                int index = dt.Year - year;
                if (index > 120)
                {
                    index = 120;
                }
                else if (index < 0)
                {
                    index = 0;
                }
                comboYear.SelectedIndex = index;
                // 月
                int month = int.Parse(strBirth[1]);
                comboMonth.SelectedIndex = month - 1;
                // 日
                int day = int.Parse(strBirth[2]);
                comboDay.SelectedIndex = day - 1;
            }
            else
            {
                /* 旧データのため初期選択なしとする */
            }
        }

        /// <summary>
        /// 年が変わった時も日の表示を制御する(うるう年対応)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboMonth_SelectedIndexChanged(sender, e);
        }

        private int selectedIndex = 0;
        private int selectedIndexY = 0;

        /// <summary>
        /// 選択した月によって表示する日を変える処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboMonth.SelectedIndex + 1;
            int indexY = comboYear.SelectedIndex + 1;
            int dayIndex = comboDay.SelectedIndex;
            switch (index)
            {
                case 0:
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    if (selectedIndex == 2 || selectedIndex == 4 || selectedIndex == 6 || selectedIndex == 9 || selectedIndex == 11)
                    {
                        //日プルダウンを31日まで選択可能にする
                        comboDay.Items.Clear();
                        for (int i = 1; i <= 31; i++)
                        {
                            comboDay.Items.Add(i.ToString());
                        }
                    }
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    if (selectedIndex == 0 || selectedIndex == 1 || selectedIndex == 2 || selectedIndex == 3 || selectedIndex == 5 || selectedIndex == 7 || selectedIndex == 8 || selectedIndex == 10 || selectedIndex == 12)
                    {
                        //日プルダウンを30日まで選択可能にする
                        comboDay.Items.Clear();
                        for (int i = 1; i <= 30; i++)
                        {
                            comboDay.Items.Add(i.ToString());
                        }
                    }
                    break;
                case 2:
                    if ((selectedIndex != 2) || (selectedIndexY != indexY))
                    {
                        // 設定年の2月の日数を取得する
                        int iDaysInMonth = 0;
                        int iYear = 1000;
                        if (comboYear.SelectedItem != null)
                        {
                            iYear = int.Parse(comboYear.SelectedItem.ToString());
                        }
                        iDaysInMonth = DateTime.DaysInMonth(iYear, 2);

                        //日プルダウンを今年の2月の最終日まで選択可能にする
                        comboDay.Items.Clear();
                        for (int i = 1; i <= iDaysInMonth; i++)
                        {
                            comboDay.Items.Add(i.ToString());
                        }
                    }
                    break;
                default:
                    return;
            }

            // 日プルダウンが設定済みだった場合、
            // 選択していた項目が更新後の日プルダウンに含まれていた場合に値を復元する。
            if (comboDay.Items.Count > dayIndex)
            {
                comboDay.SelectedIndex = dayIndex;
            }
            else
            {
                comboDay.SelectedIndex = comboDay.Items.Count - 1;
            }
            selectedIndex = index;
            selectedIndexY = indexY;
        }

        /// <summary>
        /// 名前入力テキストボックス制御
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtName1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 空文字を禁止
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
            // タブ文字を禁止
            else if (e.KeyChar == '\t')
            {
                e.Handled = true;
            }
            // 上記以外は入力OK
            else
            {
                e.Handled = false;
            }
        }
    }
}
