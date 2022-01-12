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
    public partial class FormTermStatus : Form
    {
        public enum ConnectStat
        {
            Disconnect = 0,     // 未接続
            Connect,
        }

        public enum ModemStat
        {
            Disconnect = 0,     // 未接続
            Run_01,             // 1:稼働中(S帯キャリア送受信無効)
            Run_02,             // 2:稼働中(S帯送受信無効)
            Run_03,             // 3:稼働中(S帯同期待ち)
            Run_04,             // 4:稼働中(S帯送受信可能)
            Tarm_Conserv,       // 5:端末保守中
            Update_Error,       // 6:端末更新失敗
        }
        public enum GpsStat
        {
            
            Normal = 0,         // 0:正常
            NotInit,            // 1:GPS未初期化
            InitError,          // 2:GPS初期化NG
            NotRecv,            // 3:データ受信無し
            NotTime,            // 4:時刻情報無し
            NotPosition,        // 5:位置情報無し
            Disconnect,         // 未接続
        }
        public enum VoltStat
        {
            
            Normal = 0,         // 0:正常
            Exception,          // 1:異常(電源電圧降下検出)
            Disconnect,         // 未接続
        }
        public enum TempStat
        {

            Normal = 0,         // 0:正常
            Exception,          // 1:異常(高温検出)
            Disconnect,         // 未接続
        }

        public enum SBandSWAlarmStat
        {
            Bit0 = 0,
            Bit1,
            Bit2,
            Bit3,
            Bit4,
            Bit5,
        }

        private String[] strConnectStat =
        {
            "未接続",
            "接続中",
        };

        private String[] strModemStat = 
        {
            "未接続",
            "1:稼働中(S帯キャリア送受信無効)",
            "2:稼働中(S帯送受信無効)",
            "3:稼働中(S帯同期待ち)",
            "4:稼働中(S帯送受信可能)",
            "5:端末保守中",
            "6:端末更新失敗",
        };

        private String[] strGpsStat = 
        {
            "0:正常",
            "1:GPS未初期化",
            "2:GPS初期化NG",
            "3:データ受信無し",
            "4:時刻情報無し",
            "5:位置情報無し",
            "未接続",
        };

        private String[] strVoltStat = 
        {
            "0:正常",
            "1:異常(電源電圧降下検出)",
            "未接続",
        };

        private  String[] strTempStat = 
        {
            "0:正常",
            "1:異常(高温検出)",
            "未接続",
        };

        private String[] strSBandStat = 
        {
            "bit0:CPU-Modem通信アラーム",
            "bit1:受信PLLアラーム",
            "bit2:送信PLLアラーム",
        };

        private String[] strSWAlarmStat = 
        {
            "bit0:CPU使用率異常",
            "bit1:メモリ枯渇",
            "bit2:保存領域空き容量枯渇",
            "bit3:端末更新失敗",
            "bit4:ソフトウェア起動失敗",
        };

        public FormTermStatus()
        {
            InitializeComponent();
            // 初期状態
            SetDisconnect();
        }

        private void FormTermStatus_Load(object sender, EventArgs e)
        {

        }

        public void SetDisconnect()
        {
            SetConnectStats(ConnectStat.Disconnect);
            SetModemStat(ModemStat.Disconnect);
            SetGpsStat(GpsStat.Disconnect);
            SetVoltStat(VoltStat.Disconnect);
            SetTempStat(TempStat.Disconnect);

            SetSBandStats(0);
            SetSWAlarmStats(0);
        }

        /**
         * @brief 接続状態設定
         * @param : stat : 
         */
        public void SetConnectStats(ConnectStat stat)
        {
            if (stat == ConnectStat.Disconnect) 
            {
                lblTermConnect.ForeColor = SystemColors.ControlDark;
                bgTermConnect.BackColor = SystemColors.Control;
                lblTermConnect.Text = strConnectStat[(int)stat];
            }
            else if (stat == ConnectStat.Connect)
            {
                lblTermConnect.ForeColor = SystemColors.ActiveCaptionText;
                bgTermConnect.BackColor = Color.Lime;
                lblTermConnect.Text = strConnectStat[(int)stat];
            }
            else {

            }

        }

        /**
         * @brief 運用状態設定
         * @param : stat : 
         */
        public void SetModemStat(ModemStat stat)
        {
            switch (stat)
            {
                // 未接続
                case ModemStat.Disconnect:
                    lblModemStat.ForeColor = SystemColors.ControlDark;
                    bgModemStat.BackColor = SystemColors.Control;
                    lblModemStat.Text = strModemStat[(int)stat];
                    break;

                // 正常
                case ModemStat.Run_01:
                case ModemStat.Run_03:
                case ModemStat.Run_04:
                case ModemStat.Tarm_Conserv:
                    lblModemStat.ForeColor = SystemColors.ActiveCaptionText;
                    bgModemStat.BackColor = Color.Lime;
                    lblModemStat.Text = strModemStat[(int)stat];
                    break;

                // 故障
                case ModemStat.Run_02:
                case ModemStat.Update_Error:
                    lblModemStat.ForeColor = SystemColors.ActiveCaptionText;
                    bgModemStat.BackColor = Color.Red;
                    lblModemStat.Text = strModemStat[(int)stat];
                    break;

                default:
                    break;
            }
        }

        /**
         * @brief GPS受信状態設定
         * @param : stat : 
         */
        public void SetGpsStat(GpsStat stat)
        {
            switch (stat)
            {
                // 未接続
                case GpsStat.Disconnect:
                    lblGpsStat.ForeColor = SystemColors.ControlDark;
                    bgGpsStat.BackColor = SystemColors.Control;
                    lblGpsStat.Text = strGpsStat[(int)stat];
                    break;

                // 正常
                case GpsStat.Normal:
                case GpsStat.NotInit:
                case GpsStat.NotPosition:
                case GpsStat.NotRecv:
                case GpsStat.NotTime:
                    lblGpsStat.ForeColor = SystemColors.ActiveCaptionText;
                    bgGpsStat.BackColor = Color.Lime;
                    lblGpsStat.Text = strGpsStat[(int)stat];
                    break;

                // 故障
                case GpsStat.InitError:
                    lblGpsStat.ForeColor = SystemColors.ActiveCaptionText;
                    bgGpsStat.BackColor = Color.Red;
                    lblGpsStat.Text = strGpsStat[(int)stat];
                    break;

                default:
                    break;
            }
        }

        /**
         * @brief 電源電圧状態設定
         * @param : stat : 
         */
        public void SetVoltStat(VoltStat stat)
        {
            switch (stat)
            {
                // 未接続
                case VoltStat.Disconnect:
                    lblVoltStat.ForeColor = SystemColors.ControlDark;
                    bgVoltStat.BackColor = SystemColors.Control;
                    lblVoltStat.Text = strVoltStat[(int)stat];
                    break;

                // 正常
                case VoltStat.Normal:
                    lblVoltStat.ForeColor = SystemColors.ActiveCaptionText;
                    bgVoltStat.BackColor = Color.Lime;
                    lblVoltStat.Text = strVoltStat[(int)stat];
                    break;

                // 故障
                case VoltStat.Exception:
                    lblVoltStat.ForeColor = SystemColors.ActiveCaptionText;
                    bgVoltStat.BackColor = Color.Red;
                    lblVoltStat.Text = strVoltStat[(int)stat];
                    break;

                default:
                    break;
            }
        }

         /**
          * @brief 温度状態設定
          * @param : stat : 
          */
        public void SetTempStat(TempStat stat)
        {
            switch (stat)
            {
                // 未接続
                case TempStat.Disconnect:
                    lblTempStat.ForeColor = SystemColors.ControlDark;
                    bgTempStat.BackColor = SystemColors.Control;
                    lblTempStat.Text = strTempStat[(int)stat];
                    break;

                // 正常
                case TempStat.Normal:
                    lblTempStat.ForeColor = SystemColors.ActiveCaptionText;
                    bgTempStat.BackColor = Color.Lime;
                    lblTempStat.Text = strTempStat[(int)stat];
                    break;

                // 故障
                case TempStat.Exception:
                    lblTempStat.ForeColor = SystemColors.ActiveCaptionText;
                    bgTempStat.BackColor = Color.Red;
                    lblTempStat.Text = strTempStat[(int)stat];
                    break;

                default:
                    break;
            }
        }


        /**
         * @brief S帯状態設定
         * @param : stat : 
         */
        public void SetSBandStats(int stat)
        {
            if ((stat & 0x80000000) > 0)        // CPU_MODEM_BIT
            {
                lblSBandStat1.ForeColor = SystemColors.ActiveCaptionText;
                bgSBandStat1.BackColor = Color.Red;
                lblSBandStat1.Text = strSBandStat[(int)SBandSWAlarmStat.Bit0];
            }
            else
            {
                lblSBandStat1.ForeColor = SystemColors.ControlDark;
                bgSBandStat1.BackColor = SystemColors.Control;
                lblSBandStat1.Text = strSBandStat[(int)SBandSWAlarmStat.Bit0];
            }

            if ((stat & 0x40000000) > 0)        // RX_PLL_BIT
            {
                lblSBandStat2.ForeColor = SystemColors.ActiveCaptionText;
                bgSBandStat2.BackColor = Color.Red;
                lblSBandStat2.Text = strSBandStat[(int)SBandSWAlarmStat.Bit1];
            }
            else
            {
                lblSBandStat2.ForeColor = SystemColors.ControlDark;
                bgSBandStat2.BackColor = SystemColors.Control;
                lblSBandStat2.Text = strSBandStat[(int)SBandSWAlarmStat.Bit1];
            }

            if ((stat & 0x20000000) > 0)        // TX_PLL_BIT
            {
                lblSBandStat3.ForeColor = SystemColors.ActiveCaptionText;
                bgSBandStat3.BackColor = Color.Red;
                lblSBandStat3.Text = strSBandStat[(int)SBandSWAlarmStat.Bit2];
            }
            else
            {
                lblSBandStat3.ForeColor = SystemColors.ControlDark;
                bgSBandStat3.BackColor = SystemColors.Control;
                lblSBandStat3.Text = strSBandStat[(int)SBandSWAlarmStat.Bit2];
            }
        }

        /**
         * @brief ソフトウェアアラーム状態設定
         * @param : stat : 
         */
        public void SetSWAlarmStats(int stat)
        {
            if ((stat & 0x80000000) > 0)        // CPU_BUSY_BIT
            {
                lblSWAlarm1.ForeColor = SystemColors.ActiveCaptionText;
                bgSWAlarm1.BackColor = Color.Red;
                lblSWAlarm1.Text = strSWAlarmStat[(int)SBandSWAlarmStat.Bit0];
            }
            else
            {
                lblSWAlarm1.ForeColor = SystemColors.ControlDark;
                bgSWAlarm1.BackColor = SystemColors.Control;
                lblSWAlarm1.Text = strSWAlarmStat[(int)SBandSWAlarmStat.Bit0];
            }

            if ((stat & 0x40000000) > 0)        // SHORTMEM_BIT
            {
                lblSWAlarm2.ForeColor = SystemColors.ActiveCaptionText;
                bgSWAlarm2.BackColor = Color.Red;
                lblSWAlarm2.Text = strSWAlarmStat[(int)SBandSWAlarmStat.Bit1];
            }
            else
            {
                lblSWAlarm2.ForeColor = SystemColors.ControlDark;
                bgSWAlarm2.BackColor = SystemColors.Control;
                lblSWAlarm2.Text = strSWAlarmStat[(int)SBandSWAlarmStat.Bit1];
            }

            if ((stat & 0x20000000) > 0)        // SHORTSTR_BIT
            {
                lblSWAlarm3.ForeColor = SystemColors.ActiveCaptionText;
                bgSWAlarm3.BackColor = Color.Red;
                lblSWAlarm3.Text = strSWAlarmStat[(int)SBandSWAlarmStat.Bit2];
            }
            else
            {
                lblSWAlarm3.ForeColor = SystemColors.ControlDark;
                bgSWAlarm3.BackColor = SystemColors.Control;
                lblSWAlarm3.Text = strSWAlarmStat[(int)SBandSWAlarmStat.Bit2];
            }

            if ((stat & 0x10000000) > 0)        // UPDATING_FAILURE_BIT
            {
                lblSWAlarm4.ForeColor = SystemColors.ActiveCaptionText;
                bgSWAlarm4.BackColor = Color.Red;
                lblSWAlarm4.Text = strSWAlarmStat[(int)SBandSWAlarmStat.Bit3];
            }
            else
            {
                lblSWAlarm4.ForeColor = SystemColors.ControlDark;
                bgSWAlarm4.BackColor = SystemColors.Control;
                lblSWAlarm4.Text = strSWAlarmStat[(int)SBandSWAlarmStat.Bit3];
            }

            if ((stat & 0x08000000) > 0)        // SW_STARTING_NG_BIT
            {
                lblSWAlarm5.ForeColor = SystemColors.ActiveCaptionText;
                bgSWAlarm5.BackColor = Color.Red;
                lblSWAlarm5.Text = strSWAlarmStat[(int)SBandSWAlarmStat.Bit4];
            }
            else
            {
                lblSWAlarm5.ForeColor = SystemColors.ControlDark;
                bgSWAlarm5.BackColor = SystemColors.Control;
                lblSWAlarm5.Text = strSWAlarmStat[(int)SBandSWAlarmStat.Bit4];
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
