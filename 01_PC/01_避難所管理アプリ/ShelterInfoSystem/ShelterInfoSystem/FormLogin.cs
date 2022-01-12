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
    public partial class FormLogin : Form
    {
        private const string LOGIN_ID = "qzss";        // ログインID
        private const string LOGIN_PASS = "q-anpi";    // ログインPASS
        
        public FormLogin()
        {
            InitializeComponent();

            // EnterキーはOKクリックとする
            this.AcceptButton = this.btnOk;

            // 現在年設定のデフォルト値はPCの現在年
            //2020.03.25 : 直接設定すると、設定値がMinより小さい場合に異常終了する。
            //             デフォルトを2019 -> 2020に変更。
            //yearSetting.Value = DateTime.Now.Year;
            //if (yearSetting.Value < 2019)
            //{
            //    // PCのシステム時刻が2019年以前の場合は2019に合わせる
            //    // (アプリのリリースが2019年の為、時間が遡ることはない)
            //    yearSetting.Value = 2019;
            //}
            int year = DateTime.Now.Year;
            if (year < 2020)
            {
                // PCのシステム時刻が2019年以前の場合は2019に合わせる
                // (アプリのリリースが2019年の為、時間が遡ることはない)
                year = 2020;
            }
            yearSetting.Value = year;

            //2020.03.25 Add
            // 現在月設定
            monthSetting.Value = DateTime.Now.Month;


#if DEBUG
            // DEBUG:ログインID入力を省略
            txtUser.Text = LOGIN_ID;
            txtPassword.Text = LOGIN_PASS;
            // DEBUG:ログインID入力を省略
#endif
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // ユーザID、パスワード判定
            if ((string.Compare(txtUser.Text, LOGIN_ID, true) == 0) &&
                (string.Compare(txtPassword.Text, LOGIN_PASS, true) == 0))
            {
                Cursor.Current = Cursors.WaitCursor;

                //DB生成と接続
                // step2 Dbクラス変更
                /*
                                Program.m_objDbAccess = new DbAccess();
                */
                Program.m_objDbAccess = new DbAccessStep2();
                int nRet = Program.m_objDbAccess.Connect();

                if (nRet < 0)
                {
                    MessageBox.Show("Database 接続ができませんでした。",
                                    "DB エラー",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
                else
                {

                    // 現在年を保持
                    Program.m_nowYear = int.Parse(yearSetting.Value.ToString());

                    // 現在月を保持   //2020.03.24 Add
                    Program.m_nowMonth = int.Parse(monthSetting.Value.ToString());

                    Cursor.Current = Cursors.Default;

                    FormShelterInfo frmForm = new FormShelterInfo();

                    frmForm.m_frmLogin = this;

                    Program.m_mainForm = frmForm;

                    frmForm.Show();

                    Hide();

                    Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormLogin", "", "ログイン OK");
                }

            }
            else
            {
                MessageBox.Show("ユーザ名、又はパスワードが違います。",
                                "ログイン エラー",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormLogin", "ID:" + txtUser.Text + " , PASS:" + txtPassword.Text, "ログイン ユーザ名、又はパスワードが違います");
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void Close_EXE()
        {
            Close();
        }

    }
}
