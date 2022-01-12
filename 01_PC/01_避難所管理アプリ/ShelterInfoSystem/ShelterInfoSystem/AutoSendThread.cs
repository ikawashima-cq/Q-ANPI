using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ShelterInfoSystem
{
    /// <summary>
    /// 自動送信スレッドクラス
    /// </summary>
    class AutoSendThread
    {
        public const int ALARM_MODE_ONCE = 0;
        public const int ALARM_MODE_MONTHLY = 1;
        public const int ALARM_MODE_DAILY = 2;
        public const int ALARM_MODE_HOURLY = 3;
        public const int ALARM_MODE_REPEAT = 4;
        public const int ALARM_MODE_ERR = -1;

        private FormShelterInfo m_mainForm;

        // 稼働中の自動送信タスクIDリスト
        private List<int?> workIdList = new List<int?>();
        // 稼働停止する自動送信タスクIDリスト
        private List<int?> killIdList = new List<int?>();
        
        public struct AutoSendSetting
        {
            public bool isCancel;
            public string Id;
            public string Type;
            public string Month;
            public string Day;
            public string Hour;
            public string Min;
            public bool Enabled;
            public int AlarmMode;
            public string PassedHour;
            public string PassedMin;
            public string SendType;

            /// <summary>
            /// 自動送信設定初期化
            /// </summary>
            public void init()
            {
                isCancel = false;
                Id = "";
                Type = "";
                Month = "";
                Day = "";
                Hour = "";
                Min = "";
                Enabled = false;
                AlarmMode = ALARM_MODE_ERR;
                PassedHour = "";
                PassedMin = "";
                SendType = "";
            }
        }

        /// <summary>
        /// 自動送信スレッド初期化
        /// </summary>
        /// <param name="mainForm"></param>
        public void Initialize(FormShelterInfo mainForm)
        {
            m_mainForm = mainForm;

            // 自動送信設定を初期化
            Program.m_AutoSendSetting = new List<AutoSendSetting>();

            // DBから設定値を取得
            List<DbAccess.AutoSendTime> dbAutoSendSetting;
            Program.m_objDbAccess.GetAutoSendTimeAll(out dbAutoSendSetting);
            foreach (var item in dbAutoSendSetting)
            {
                AutoSendSetting dbSetting = new AutoSendSetting();
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
                Program.m_AutoSendSetting.Add(dbSetting);
            }

            // DBに設定値がある場合、自動送信スレッドを開始
            Program.m_AutoThreadSend.StartAutoSendThreadAll();
        }

        /// <summary>
        /// すべての自動送信スレッドを開始する
        /// </summary>
        public void StartAutoSendThreadAll()
        {
            // 自動送信設定の数ループ
            // (現行の仕様では1つまでしか自動送信設定ができないが、今後の拡張性を考えてループ処理とする)2018/11/01
            for (int aic = 0; aic < Program.m_AutoSendSetting.Count ; aic++)
            {
                // 自動送信設定のEnabledがTrueのもののみ自動送信する
                if (Program.m_AutoSendSetting[aic].Enabled)
                {
                    AutoSendSetting thisSetting = Program.m_AutoSendSetting[aic];
                    
                    // 自動送信設定毎にスレッドを開始する
                    Task.Factory.StartNew(() =>
                    {
                        Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "StartAutoSendThreadAll(TaskID：" + Task.CurrentId + ")", "自動送信", "自動送信タスク開始");
                        // 稼働中の自動送信タスクIDリストに自タスクIDが存在しない場合
                        if(!workIdList.Contains(Task.CurrentId))
                        {
                            // 稼働中の自動送信タスクIDリストに自タスクIDを追加する
                            workIdList.Add(Task.CurrentId);
                        }

                        DateTime nowTime = DateTime.Now;

                        // 自動送信時刻を取得
                        DateTime nextSendTime = new DateTime();
                        bool nextOK = false;
                        nextOK = getNextSendTime(thisSetting, ref nextSendTime);
                        
                        // 送信時刻監視ループ
                        while (true)
                        {
 
                            // 次回送信時刻の取得に失敗した場合、自動送信終了
                            if (!nextOK)
                            {
                                break;
                            }

                            // 送信時刻になったor過ぎた
                            nowTime = DateTime.Now;
                            if (nowTime >= nextSendTime)
                            {
                                Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "StartAutoSendThreadAll(TaskID：" + Task.CurrentId + ")", "自動送信", "自動送信開始");

                                // 他で手動通信を行っている場合はスキップ
                                if (Program.m_SendFlag != Program.NOT_SENDING)
                                {
                                    Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "StartAutoSendThreadAll(TaskID：" + Task.CurrentId + ")", "自動送信", "他で送信中の為、自動送信スキップ");

                                    // 次回の自動送信時刻を取得
                                    nextOK = getNextSendTime(thisSetting, ref nextSendTime);
                                    continue;
                                }

                                // Q-ANPIターミナル未接続の場合はスキップ
                                if (!Program.m_EquStat.isConnected())
                                {
                                    Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "StartAutoSendThreadAll(TaskID：" + Task.CurrentId + ")", "自動送信", "Q-ANPIターミナル未接続の為、自動送信スキップ");

                                    // 次回の自動送信時刻を取得
                                    nextOK = getNextSendTime(thisSetting, ref nextSendTime);
                                    continue;
                                }

                                // 共通送信数の初期化
                                m_mainForm.initCommonSendingBuff();
                                m_mainForm.initSending(false);

                                // 自動送信フラグ初期化
                                Program.m_mainForm.m_nowAutoSending = false;

                                // 実行コマンドをセット
                                if (thisSetting.SendType == "0")
                                {
                                    m_mainForm.m_NowCommand = FormShelterInfo.SendMessageCommand.AUTO_SEND_NUM;
                                }
                                else if (thisSetting.SendType == "3")
                                {
                                    m_mainForm.m_NowCommand = FormShelterInfo.SendMessageCommand.AUTO_SEND_DETAIL;
                                }
                                else if (thisSetting.SendType == "1")
                                {
                                    m_mainForm.m_NowCommand = FormShelterInfo.SendMessageCommand.AUTO_SEND_NUM_DETAIL;
                                }
                                else
                                {
                                    m_mainForm.m_NowCommand = FormShelterInfo.SendMessageCommand.COMMAND_NONE;
                                    break;
                                }

                                // DBに登録済みの「開設」状態の避難所すべての情報を送信する
                                DbAccess.TerminalInfo[] AllTernimalInfo = new DbAccess.TerminalInfo[0];
                                Program.m_objDbAccess.GetTerminalInfoAll(ref AllTernimalInfo);

                                // 自動送信時の最大送信数を計算
                                int openCount = 0;
                                
                                foreach (var item in AllTernimalInfo)
                                {
                                    if (item.open_flag == FormShelterInfo.SHELTER_STATUS.OPEN)
                                    {
                                        openCount++;
                                    }
                                }

                                int maxCountOpen = 0;
                                int maxCountOpenInfo = 0;
                                int maxCountNameInfo = 0;
                                int maxCountShelterInfo = 0;

                                if ((thisSetting.SendType == "0") || (thisSetting.SendType == "1"))
                                {
                                    maxCountOpenInfo = 2 * openCount;
                                }

                                if ((thisSetting.SendType == "1") || (thisSetting.SendType == "3"))
                                {
                                    maxCountNameInfo = (9 * openCount);
                                    maxCountShelterInfo = (9 * openCount);
                                }

                                maxCountOpen = maxCountOpenInfo + maxCountNameInfo + maxCountShelterInfo;

                                int autsendCount = 0;

                                foreach (var item in AllTernimalInfo)
                                {
                                    // 「開設」状態の避難所の情報のみ自動送信
                                    if (item.open_flag == FormShelterInfo.SHELTER_STATUS.OPEN)
                                    {

                                        autsendCount++;
                                        Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "StartAutoSendThreadAll(TaskID：" + Task.CurrentId + ")", "自動送信", "自動送信実施(" + autsendCount + "個目)");

                                        // 避難所詳細情報(開設情報,避難者数)を送信

                                        // 自動送信実施中フラグをON
                                        m_mainForm.m_nowAutoSending = true;

                                        // 通信端末と接続されていない場合は自動送信しない
                                        if (!Program.m_EquStat.isConnected())
                                        {
                                            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "StartAutoSendThreadAll(TaskID：" + Task.CurrentId + ")", "自動送信", "端末未接続の為、未送信");
                                            break;
                                        }
                                        // 避難所詳細情報を送信する設定の時のみ送信
                                        if ((thisSetting.SendType == "1") || (thisSetting.SendType == "3"))
                                        {
                                            // 避難所安否情報を送信
                                            // SendShelterNameInfoの送信中チェックを回避するために一瞬送信状態を解除(画面更新はしないのでボタンは活性化しないハズ)
                                            Program.m_SendFlag = Program.NOT_SENDING;
                                            m_mainForm.SendShelterNameInfo(item, true, maxCountOpen);   // 自動送信の場合、必ず避難所名は送信する
                                            m_mainForm.SendShelter(item, true, maxCountOpen);
                                        }

                                        // 避難所詳細情報を送信する設定の時のみ送信
                                        if ((thisSetting.SendType == "0") || (thisSetting.SendType == "1"))
                                        {
                                            // 開設情報(避難者数)を送信
                                            m_mainForm.SendShelterOpenCloseInfo(item, true, maxCountOpen);
                                        }
                                        System.Threading.Thread.Sleep(1000);
                                    }
                                }

                                Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "StartAutoSendThreadAll(TaskID：" + Task.CurrentId + ")", "自動送信", "自動送信終了");

                                // 「指定なし」の場合、1回送信した後は自動送信を終了する
                                if (thisSetting.AlarmMode == AutoSendThread.ALARM_MODE_ONCE)
                                {
                                    // DBに送信OFFを登録
                                    thisSetting.Enabled = false;
                                    Program.m_objDbAccess.UpsertAutoSendTime(thisSetting.Id, thisSetting.Type, thisSetting.Month, thisSetting.Day, thisSetting.Hour, thisSetting.Min
                                                                                , thisSetting.PassedHour, thisSetting.PassedMin, thisSetting.Enabled, thisSetting.SendType);
                                    break;
                                }


                                // 次回の自動送信時刻を取得
                                nextOK = getNextSendTime(thisSetting, ref nextSendTime);
                            }
                            else
                            {
                                // まだなので1秒待機
                                System.Threading.Thread.Sleep(1000);
                            }

                            // 稼働停止する自動送信タスクIDリストに自タスクIDが存在する場合
                            if (killIdList.Contains(Task.CurrentId))
                            {
                                Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "StartAutoSendThreadAll(TaskID：" + Task.CurrentId + ")", "自動送信", "自動送信タスクキル");
                                break;
                            }
                        }

                        // 各タスクIDリストから自タスクIDを削除して稼働を停止する
                        killIdList.Remove(Task.CurrentId);
                        workIdList.Remove(Task.CurrentId);

                        Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "StartAutoSendThreadAll(TaskID：" + Task.CurrentId + ")", "自動送信", "自動送信タスク終了");
                        Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "StartAutoSendThreadAll(TaskID：" + Task.CurrentId + ")", "自動送信", "killIdList.Count：" + killIdList.Count + " workIdList.Count：" + workIdList.Count);

                    });
                    System.Threading.Thread.Sleep(100);
                }
            }
        }

        /// <summary>
        /// すべての自動送信スレッドを停止する
        /// </summary>
        public void StopAutoSendThreadAll()
        {
            // 稼働中の自動送信タスクIDを稼働停止タスクIDリストに追加する
            foreach (int? id in workIdList)
            {
                if (!killIdList.Contains(id))
                {
                    killIdList.Add(id);
                    Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "StopAutoSendThreadAll", "自動送信停止", "稼働停止タスクID追加：" + id);
                }
            }
        }

        /// <summary>
        /// 次回送信時刻を取得する
        /// </summary>
        /// <param name="SendSetting"></param>
        /// <param name="nextSendTime"></param>
        private bool getNextSendTime(AutoSendThread.AutoSendSetting SendSetting, ref DateTime nextSendTime)
        {
            bool retCode = false;

            // 現在時刻の取得
            DateTime nowTime = DateTime.Now;
            int settingMonth = nowTime.Month;
            int settingDay = nowTime.Day;
            int settingHour = nowTime.Hour;
            int settingMinute = nowTime.Minute;

            DateTime compTime;

            // 次回送信時刻の生成
            switch (SendSetting.AlarmMode)
            {
                //指定なし(1回)
                case ALARM_MODE_ONCE:
                    compTime = new DateTime(nowTime.Year, int.Parse(SendSetting.Month), int.Parse(SendSetting.Day),
                                            int.Parse(SendSetting.Hour), int.Parse(SendSetting.Min), 0);
                    while (true)
                    {
                        if (nowTime < compTime)
                        {
                            retCode = true;
                            break;
                        }
                        compTime = compTime.AddYears(1);
                    }
                    nextSendTime = compTime;
                    break;
                //毎月
                case ALARM_MODE_MONTHLY:
                    compTime = new DateTime(nowTime.Year, nowTime.Month, int.Parse(SendSetting.Day),
                                            int.Parse(SendSetting.Hour), int.Parse(SendSetting.Min), 0);
                    while (true)
                    {
                        if (nowTime < compTime)
                        {
                            retCode = true;
                            break;
                        }
                        compTime = compTime.AddMonths(1);
                    }
                    nextSendTime = compTime;
                    break;
                //毎日
                case ALARM_MODE_DAILY:
                    compTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day,
                                            int.Parse(SendSetting.Hour), int.Parse(SendSetting.Min), 0);
                    while (true)
                    {
                        if (nowTime < compTime)
                        {
                            retCode = true;
                            break;
                        }
                        compTime = compTime.AddDays(1);
                    }
                    nextSendTime = compTime;
                    break;
                //毎時
                case ALARM_MODE_HOURLY:
                    compTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day,
                                            nowTime.Hour, int.Parse(SendSetting.Min), 0);
                    while (true)
                    {
                        if (nowTime < compTime)
                        {
                            retCode = true;
                            break;
                        }
                        compTime = compTime.AddHours(1);
                    }
                    nextSendTime = compTime;
                    break;
                //指定時間
                case ALARM_MODE_REPEAT:
                    compTime = new DateTime(nowTime.Year, int.Parse(SendSetting.Month),int.Parse(SendSetting.Day),
                                            int.Parse(SendSetting.Hour),
                                            int.Parse(SendSetting.Min), 0);
                    // 開始日から間隔時間を足していき、直近の未来を次の送信予定日時とする
                    //compTime = compTime.AddHours(double.Parse(SendSetting.PassedHour));
                    //compTime = compTime.AddMinutes(double.Parse(SendSetting.PassedMin));
                    while (true)
                    {
                        if (nowTime < compTime)
                        {
                            retCode = true;
                            break;
                        }
                        compTime = compTime.AddHours(int.Parse(SendSetting.PassedHour));
                        compTime = compTime.AddMinutes(int.Parse(SendSetting.PassedMin));
                    }
                    nextSendTime = compTime;
                    break;
                case ALARM_MODE_ERR:
                default:
                    compTime = nowTime; 
                    retCode = false; 
                    break;
            }

            return retCode;
        }
    }
}
