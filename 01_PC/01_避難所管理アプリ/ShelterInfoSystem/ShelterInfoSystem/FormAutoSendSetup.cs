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
    public partial class FormAutoSendSetup : Form
    {
        private int selectedIndex = 0;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="mainForm"></param>
        public FormAutoSendSetup()
        {
            InitializeComponent();

            // 自動送信設定をダイアログに復元
            RestoreAutoSendSetting();
        }

        /// <summary>
        /// 自動送信設定値の復元
        /// </summary>
        private void RestoreAutoSendSetting()
        {
            // DBの設定を取得
            List<AutoSendThread.AutoSendSetting> DBAutoSendSetting = new List<AutoSendThread.AutoSendSetting>();
            List<DbAccess.AutoSendTime> dbAutoSendSetting;
            Program.m_objDbAccess.GetAutoSendTimeAll(out dbAutoSendSetting);
            foreach (var item in dbAutoSendSetting)
            {
                AutoSendThread.AutoSendSetting dbSetting = new AutoSendThread.AutoSendSetting();
                dbSetting.init();
                dbSetting.Id = item.id;
                dbSetting.Month = item.month;
                dbSetting.Day = item.day;
                dbSetting.Hour = item.hour;
                dbSetting.Min = item.min;
                dbSetting.Type = item.type;
                dbSetting.PassedHour = item.hour2;
                dbSetting.PassedMin = item.min2;
                if (item.enabled == "True")
                {
                    dbSetting.Enabled = true;
                    dbSetting.isCancel = false;
                }
                else
                {
                    dbSetting.Enabled = false;
                    dbSetting.isCancel = true;
                }
                dbSetting.AlarmMode = int.Parse(item.type);
                dbSetting.SendType = item.send_type;
                DBAutoSendSetting.Add(dbSetting);
            }


            comboBoxMonth.SelectedIndex = 0;
            comboBoxDate.SelectedIndex = 0;
            comboBoxHour.SelectedIndex = 0;
            comboBoxMinute.SelectedIndex = 0;
            radioButtonUnspecified.Checked = true;
            comboBoxHour2.SelectedIndex = 0;
            comboBoxMin2.SelectedIndex = 0;

            if (DBAutoSendSetting.Count > 0)
            {
                // 各設定値を復元する。
                if ((DBAutoSendSetting[0].Month != "") && (DBAutoSendSetting[0].Month != "-"))
                {
                    comboBoxMonth.SelectedIndex = int.Parse(DBAutoSendSetting[0].Month);
                }
                if ((DBAutoSendSetting[0].Day != "") && (DBAutoSendSetting[0].Day != "-"))
                {
                    comboBoxDate.SelectedIndex = int.Parse(DBAutoSendSetting[0].Day);
                }
                if ((DBAutoSendSetting[0].Hour != "") && (DBAutoSendSetting[0].Hour != "-"))
                {
                    comboBoxHour.SelectedIndex = int.Parse(DBAutoSendSetting[0].Hour) + 1;
                }
                if ((DBAutoSendSetting[0].Min != "") && (DBAutoSendSetting[0].Min != "-"))
                {
                    comboBoxMinute.SelectedIndex = int.Parse(DBAutoSendSetting[0].Min) + 1;
                }
                if ((DBAutoSendSetting[0].PassedHour != "") && (DBAutoSendSetting[0].PassedHour != "-"))
                {
                    comboBoxHour2.SelectedIndex = int.Parse(DBAutoSendSetting[0].PassedHour) + 1;
                }
                if ((DBAutoSendSetting[0].PassedMin != "") && (DBAutoSendSetting[0].PassedMin != "-"))
                {
                    comboBoxMin2.SelectedIndex = int.Parse(DBAutoSendSetting[0].PassedMin) + 1;
                }

                switch (DBAutoSendSetting[0].AlarmMode)
                {
                    // 毎月
                    case AutoSendThread.ALARM_MODE_MONTHLY:
                        radioButtonMonthly.Checked = true;
                        break;
                    // 毎日
                    case AutoSendThread.ALARM_MODE_DAILY:
                        radioButtonDaily.Checked = true;
                        break;
                    // 毎時
                    case AutoSendThread.ALARM_MODE_HOURLY:
                        radioButtonHourly.Checked = true;
                        break;
                    // 時間指定
                    case AutoSendThread.ALARM_MODE_REPEAT:
                        radioButtonTimeSpan.Checked = true;
                        break;
                    // 指定なし(1回のみ)
                    case AutoSendThread.ALARM_MODE_ONCE:
                        radioButtonUnspecified.Checked = true;
                        break;
                    default:
                        radioButtonUnspecified.Checked = true;
                        break;
                }

                // 自動送信設定の復元
                if (DBAutoSendSetting[0].Enabled)
                {
                    radioEnable.Checked = true;
                }
                else
                {
                    radioDisable.Checked = true;
                }

                // 送信内容設定の復元
                switch (DBAutoSendSetting[0].SendType)
                {
                    // 避難所情報のみ
                    case "0":
                        checkTerminalInfo.Checked = true;
                        checkTerminalDetail.Checked = false;
                        break;
                    // 避難所情報、避難所集計情報
                    case "1":
                        checkTerminalInfo.Checked = true;
                        checkTerminalDetail.Checked = true;
                        break;
                    // 避難所集計情報のみ（未使用）
                    case "2":
                        checkTerminalInfo.Checked = false;
                        checkTerminalDetail.Checked = true;
                        break;
                    // 避難所情報、避難所集計情報どちらも送信せず（未使用）
                    case "3":
                        checkTerminalInfo.Checked = false;
                        checkTerminalDetail.Checked = false;
                        break;
                    default:
                        checkTerminalInfo.Checked = true;
                        checkTerminalDetail.Checked = true;
                        break;
                }

                // 送信内容チェック復元
                switch (DBAutoSendSetting[0].SendType)
                {
                    case "0":
                        checkTerminalInfo.Checked = true;
                        checkTerminalDetail.Checked = false;
                        break;
                    case "1":
                        checkTerminalInfo.Checked = true;
                        checkTerminalDetail.Checked = true;
                        break;
                    case "2":
                        checkTerminalInfo.Checked = false;
                        checkTerminalDetail.Checked = false;
                        break;
                    case "3":
                        checkTerminalInfo.Checked = false;
                        checkTerminalDetail.Checked = true;
                        break;
                    default:
                        break;
                }
            }
            // DBに自動送信設定が存在しない場合はデフォルトを設定
            else
            {
                // 無効にチェック
                radioDisable.Checked = true;
            }
        }

        /// <summary>
        /// 自動送信時間設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegist_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormAutoSendSetup", "btnRegist_Click", "個人安否/避難所情報自動送信設定ダイアログ [設定] クリック");

            int alarmMode = 0;
            bool errMes = false;
            bool errMes2 = false;
            bool chkMon = false;
            bool chkDay = false;
            bool chkHor = false;
            bool chkMin = false;

            // 有効/無効で無効が選択されていた場合はチェック無し
            if (radioEnable.Checked)
            {
                // モード判定
                // 指定なし
                if (radioButtonUnspecified.Checked)
                {
                    chkMon = true;
                    chkDay = true;
                    chkHor = true;
                    chkMin = true;
                }
                // 毎月送信
                else if (radioButtonMonthly.Checked)
                {
                    chkDay = true;
                    chkHor = true;
                    chkMin = true;
                }
                // 毎日送信
                else if (radioButtonDaily.Checked)
                {
                    chkHor = true;
                    chkMin = true;
                }
                // 毎時送信
                else if (radioButtonHourly.Checked)
                {
                    chkMin = true;
                }
                // 時間指定送信
                else if (radioButtonTimeSpan.Checked)
                {
                    chkMon = true;
                    chkDay = true;
                    chkHor = true;
                    chkMin = true;
                }

                // 入力値チェック
                if ((chkMon) && (comboBoxMonth.SelectedIndex == 0))
                {
                    errMes = true;
                }
                if ((chkDay) && (comboBoxDate.SelectedIndex == 0))
                {
                    errMes = true;
                }
                if ((chkHor) && (comboBoxHour.SelectedIndex == 0))
                {
                    errMes = true;
                }
                if ((chkMin) && (comboBoxMinute.SelectedIndex == 0))
                {
                    errMes = true;
                }

                // エラーがあったばあい、メッセージ表示
                if (errMes)
                {
                    MessageBox.Show("送信開始日時に自動送信を行う開始日時を設定してください。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 時間指定送信の場合、間隔時間と分もチェック
                if (radioButtonTimeSpan.Checked)
                {
                    if (comboBoxHour2.SelectedIndex == 0)
                    {
                        errMes2 = true;
                    }
                    if (comboBoxMin2.SelectedIndex == 0)
                    {
                        errMes2 = true;
                    }
                    if (comboBoxHour2.SelectedIndex == 1 &&comboBoxMin2.SelectedIndex == 1)
                    {
                        errMes2 = true;
                    }
                    if (errMes2)
                    {
                        MessageBox.Show("指定時間送信の場合、送信間隔を設定してください。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            // 自動送信間隔のタイプを取得
            GetAlarmMode(out alarmMode);

            // 送信情報を取得
            int sendType = 0;
            if((checkTerminalInfo.Checked)&&(!checkTerminalDetail.Checked))
            {
                sendType = 0;
            }
            else if((checkTerminalInfo.Checked)&&(checkTerminalDetail.Checked))
            {
                sendType = 1;
            }
            else if((!checkTerminalInfo.Checked)&&(!checkTerminalDetail.Checked))
            {
                sendType = 2;
            }
            else if((!checkTerminalInfo.Checked)&&(checkTerminalDetail.Checked))
            {
                sendType = 3;
            }

            // 設定値に正しく値が入力されていた場合、設定値をDBに登録に自動送信スレッド開始
            // 自動送信設定値を保存
            AutoSendThread.AutoSendSetting tmpSetting = new AutoSendThread.AutoSendSetting();
            tmpSetting.init();
            tmpSetting.isCancel = false;
            tmpSetting.Month = comboBoxMonth.Text;
            tmpSetting.Day = comboBoxDate.Text;
            tmpSetting.Hour = comboBoxHour.Text;
            tmpSetting.Min = comboBoxMinute.Text;
            tmpSetting.AlarmMode = alarmMode;
            tmpSetting.PassedHour = comboBoxHour2.Text;
            tmpSetting.PassedMin = comboBoxMin2.Text;
            tmpSetting.Enabled = true;
            tmpSetting.Id = "77777";    //仮自動送信ID(自動送信設定を複数対応させる場合はそれぞれにIDを振り分ける)
            tmpSetting.Type = alarmMode.ToString();
            tmpSetting.SendType = sendType.ToString();

            // 自動送信設定が有効の場合のみEnabledをtrueにする
            if (radioEnable.Checked)
            {
                tmpSetting.Enabled = true;
            }
            else
            {
                tmpSetting.Enabled = false;
            }

            // すべての自動送信スレッドを停止
            Program.m_AutoThreadSend.StopAutoSendThreadAll();


            // 現状、自動送信時刻は1つしか設定できない仕様なので、リストをクリア後に設定値をセット
            Program.m_AutoSendSetting.Clear();
            Program.m_AutoSendSetting.Add(tmpSetting);

            // DBに自動送信時刻を登録
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormAutoSendSetup", "btnRegist_Click", "DB登録");
            Program.m_objDbAccess.UpsertAutoSendTime(tmpSetting.Id, tmpSetting.Type, tmpSetting.Month, tmpSetting.Day, tmpSetting.Hour, tmpSetting.Min
                                            , tmpSetting.PassedHour, tmpSetting.PassedMin, tmpSetting.Enabled, tmpSetting.SendType);

            // すべての時刻監視スレッドを開始
            Program.m_AutoThreadSend.Initialize(Program.m_mainForm);
            Close();

        }

        /// <summary>
        /// キャンセルボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 「月」に応じて「日」に表示する日数を制御
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBoxMonth.SelectedIndex;
            int dayIndex = comboBoxDate.SelectedIndex;
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
                        comboBoxDate.Items.Clear();
                        comboBoxDate.Items.Add("-");
                        for (int i = 1; i <= 31; i++)
                        {
                            comboBoxDate.Items.Add(i.ToString());
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
                        comboBoxDate.Items.Clear();
                        comboBoxDate.Items.Add("-");
                        for (int i = 1; i <= 30; i++)
                        {
                            comboBoxDate.Items.Add(i.ToString());
                        }
                    }
                    break;
                case 2:
                    if (selectedIndex != 2)
                    {
                        // 今年の2月の日数を取得する
                        int iDaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, 2);

                        //日プルダウンを今年の2月の最終日まで選択可能にする
                        comboBoxDate.Items.Clear();
                        comboBoxDate.Items.Add("-");
                        for (int i = 1; i <= iDaysInMonth; i++)
                        {
                            comboBoxDate.Items.Add(i.ToString());
                        }
                    }
                    break;
                default:
                    return;
            }

            // 日プルダウンが設定済みだった場合、
            // 選択していた項目が更新後の日プルダウンに含まれていた場合に値を復元する。
            if (comboBoxDate.Items.Count > dayIndex)
            {
                comboBoxDate.SelectedIndex = dayIndex;
            }
            else
            {
                comboBoxDate.SelectedIndex = 0;
            }
            selectedIndex = index;
        }

        /// <summary>
        /// 自動送信間隔取得
        /// </summary>
        /// <param name="alarmMode"></param>
        private void GetAlarmMode(out int alarmMode)
        {
            alarmMode = AutoSendThread.ALARM_MODE_ERR;

            if (radioButtonUnspecified.Checked)
            {
                alarmMode = AutoSendThread.ALARM_MODE_ONCE;
            }
            else if (radioButtonMonthly.Checked)
            {
                alarmMode = AutoSendThread.ALARM_MODE_MONTHLY;
            }
            else if (radioButtonDaily.Checked)
            {
                alarmMode = AutoSendThread.ALARM_MODE_DAILY;
            }
            else if (radioButtonHourly.Checked)
            {
                alarmMode = AutoSendThread.ALARM_MODE_HOURLY;
            }
            else if (radioButtonTimeSpan.Checked)
            {
                alarmMode = AutoSendThread.ALARM_MODE_REPEAT;
            }
            else
            {
                alarmMode = AutoSendThread.ALARM_MODE_ERR;
            }
            return;
        }

        /// <summary>
        /// ラジオボタン操作時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonTimeSpan_CheckedChanged(object sender, EventArgs e)
        {
            // 指定時間送信の間隔時間プルダウン活性化制御
            if (!radioButtonTimeSpan.Checked)
            {
                // 指定時間送信にチェックがついていない場合、間隔時間の設定を無効にする
                comboBoxHour2.Enabled = false;
                comboBoxMin2.Enabled = false;
            }
            else
            {
                // 指定時間送信にチェックがついている場合、間隔時間の設定を有効にする
                comboBoxHour2.Enabled = true;
                comboBoxMin2.Enabled = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioEnable.Checked)
            {
                comboBoxMonth.Enabled = true;
                comboBoxDate.Enabled = true;
                comboBoxHour.Enabled = true;
                comboBoxMinute.Enabled = true;
                radioButtonUnspecified.Enabled = true;
                radioButtonMonthly.Enabled = true;
                radioButtonDaily.Enabled = true;
                radioButtonHourly.Enabled = true;
                radioButtonTimeSpan.Enabled = true;
                checkTerminalDetail.Enabled = true;
                checkTerminalInfo.Enabled = true;  
                if (radioButtonTimeSpan.Checked)
                {
                    comboBoxHour2.Enabled = true;
                    comboBoxMin2.Enabled = true;
                }
                else
                {
                    comboBoxHour2.Enabled = false;
                    comboBoxMin2.Enabled = false;
                }
            }
            else
            {
                comboBoxMonth.Enabled = false;
                comboBoxDate.Enabled = false;
                comboBoxHour.Enabled = false;
                comboBoxMinute.Enabled = false;
                radioButtonUnspecified.Enabled = false;
                radioButtonMonthly.Enabled = false;
                radioButtonDaily.Enabled = false;
                radioButtonHourly.Enabled = false;
                radioButtonTimeSpan.Enabled = false;
                comboBoxHour2.Enabled = false;
                comboBoxMin2.Enabled = false;
                checkTerminalDetail.Enabled = false;
                checkTerminalInfo.Enabled = false; 
            }
        }
    }
}
