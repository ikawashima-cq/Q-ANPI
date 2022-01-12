/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2017. All rights reserved.
 */
/**
 * @file    FormFormReceiveMessageView.cs
 * @brief   受信メッセージ表示画面フォーム
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Windows.Forms;

//using MySql.Data;

namespace ShelterInfoSystem
{
    /**
     * @class FormReceiveMessageView
     * @brief 受信メッセージ(救助支援情報、災害危機通報)表示画面クラス
     */
    public partial class FormReceiveMessageView : Form
    {
        public bool m_isReceiving = false;

        private const int LISTVIEW_130 = 0;
        private const int LISTVIEW_L1S = 1;

        private int selected = LISTVIEW_130;

        private int LISTVIEW_MAX = 20;          // 表示メッセージ上限値
        /**
         * @brief リストデータ
         */
        List<DbAccessStep2.RecvMsgInfo> l1sList = new List<DbAccessStep2.RecvMsgInfo>();
        List<DbAccessStep2.RescueMsgInfo> RescueList = new List<DbAccessStep2.RescueMsgInfo>();///

        /**
         * @brief コンストラクタ
         */
        public FormReceiveMessageView()
        {
            InitializeComponent();
            this.listMsgCategory.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            //this.listMsg.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            //Program.m_ReceiveMessageView = this;
        }

        /**
         * @brief Form呼び出し時処理
         */
        private void FormReceiveMessageView_Load(object sender, EventArgs e)
        {
            listMsgCategory.Items[0].Selected = true;

            // イベント設定
            Program.m_objDbAccess.EventDbAdd += new DbAccessStep2.EventDbAccessDelegate(listMsg_update);

            if (Program.m_EquStat.isConnected() == false)
            {
                m_isReceiving = false;
                btnIsReceive.Text = "受信再開";
                btnIsReceive.Enabled = false;
                btnType3.Enabled = false;
            }
            else
            {
                m_isReceiving = true;
                btnIsReceive.Text = "受信停止";
                btnIsReceive.Enabled = true;
                btnType3.Enabled = true;
            }

            // 通信中は非活性化
            if (Program.m_SendFlag != Program.NOT_SENDING)
            {
                btnType3.Enabled = false;
                btnIsReceive.Enabled = false;
            }
        }
        
        /**
         * @brief 新着確認デリゲート
         */
        public delegate void EventRtnRescueDelegate(object sender, string outVal);
        public event EventRtnRescueDelegate EventRtnRescue;


        /**
         * @brief メッセージ情報一覧取得
         */
        public void loadList(int type)
        {
            lock (this)
            {
                // 画面のリストクリア
                listMsg.Items.Clear();
                l1sList.Clear();
                RescueList.Clear();
                int listnum = 0;

                textMsg.Text = "";


                // リスト取得(type 1:災害危機 2:支援情報)
                if (type == 1)
                {
                    Program.m_objDbAccess.GetRecvMsgInfo(ref l1sList);
                    if (l1sList.Count == 0)
                    {
                        // データなし
                        return;
                    }
                    // リスト件数MAX20件分表示する（表示優先順はGetRecvMsgInfoにて設定)
                    if (l1sList.Count >= LISTVIEW_MAX)
                    {
                        listnum = LISTVIEW_MAX;
                    }
                    else
                    {
                        listnum = l1sList.Count;
                    }

                    for (int idx = 0; idx < listnum; idx++)
                    {
                        DbAccessStep2.RecvMsgInfo l1sInfo = l1sList[idx];

                        string[] strItems = new string[4];
                        strItems[0] = l1sInfo.id;
                        if (l1sInfo.rdate.Length == 14)
                        {
                            strItems[1] = l1sInfo.rdate.Substring(0, 4)
                                + "/" + l1sInfo.rdate.Substring(4, 2)
                                + "/" + l1sInfo.rdate.Substring(6, 2)
                                + " " + l1sInfo.rdate.Substring(8, 2)
                                + ":" + l1sInfo.rdate.Substring(10, 2)
                                + ":" + l1sInfo.rdate.Substring(12, 2);
                        }
                        else
                        {
                            strItems[1] = l1sInfo.rdate;
                        }
                        strItems[2] = l1sInfo.MT;
                        string dc = l1sInfo.Dc;
                        if ("44".Equals(l1sInfo.MT))
                        {
                            dc = "";
                        }

                        strItems[3] = Program.m_L1sConv.getName("MT.mt00", l1sInfo.MT + dc);

                        // 未読項目はBold体で表示する
                        ListViewItem listItem = new ListViewItem(strItems);

                        if (l1sInfo.readflg == 0)
                        {
                            Font nowFont = listMsg.Font;
                            listItem.Font = new Font(nowFont, nowFont.Style | FontStyle.Bold);
                        }

                        // リストアイテムを追加する
                        listMsg.Items.Add(listItem);
                    }
                }
                else if (type == 2)
                {
                    if (!Program.m_objDbAccess.GetRescueMsgInfo(Program.m_mainForm.GetActiveTerminalInfo().gid, int.Parse(Program.m_mainForm.GetActiveTerminalInfo().smid),ref RescueList))
                    {
                        // DBエラーにて取得できなかった場合
                        return;
                    }
                    if (RescueList.Count == 0)
                    {
                        // データなし
                        return;
                    }
                    // リスト件数MAX20件分表示する（表示優先順はGetRescueMsgInfoにて設定)
                    if (RescueList.Count >= LISTVIEW_MAX)
                    {
                        listnum = LISTVIEW_MAX;
                    }
                    else
                    {
                        listnum = RescueList.Count;
                    }

                    for (int idx = 0; idx < listnum; idx++)
                    {
                        DbAccessStep2.RescueMsgInfo RescueInfo = RescueList[idx];

                        string[] strItems = new string[4];
                        strItems[0] = RescueInfo.id;
                        if (RescueInfo.rdate.Length == 14)
                        {
                            strItems[1] = RescueInfo.rdate.Substring(0, 4)
                                + "/" + RescueInfo.rdate.Substring(4, 2)
                                + "/" + RescueInfo.rdate.Substring(6, 2)
                                + " " + RescueInfo.rdate.Substring(8, 2)
                                + ":" + RescueInfo.rdate.Substring(10, 2)
                                + ":" + RescueInfo.rdate.Substring(12, 2);
                        }
                        else
                        {
                            strItems[1] = RescueInfo.rdate;
                        }
                        if (RescueInfo.dohoflg == 1)
                        {
                            strItems[2] = "同報";
                        }
                        else
                        {
                            strItems[2] = "個別";
                        }

                        if (RescueInfo.Dt == 1)    //テキスト表示
                        {
                            strItems[3] = RescueInfo.message;
                        }
                        else                      //バイナリ表示
                        {
                            strItems[3] = "バイナリデータ" + (RescueInfo.msglength) + "byte";
                        }

                        // 未読項目はBold体で表示する
                        ListViewItem listItem = new ListViewItem(strItems);
                        if (RescueInfo.readflg == 0)
                        {
                            Font nowFont = listMsg.Font;
                            listItem.Font = new Font(nowFont, nowFont.Style | FontStyle.Bold);
                        }

                        // リストアイテムを追加する
                        listMsg.Items.Add(listItem);
                    }

                }
                else
                {
                    // N/A
                }
            }
        }

        /**
         * @brief ListViewヘッダ描画メソッド
         */
        private void listView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            // 描画はOSに任せる
            e.DrawDefault = true;
        }

        /**
         * @brief ListView描画メソッド
         */
        private void listView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            StringFormat stringFormat = new StringFormat();
            Rectangle objStrRect = e.Bounds;

            // 選択されているアイテムの描画
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(
                                SystemBrushes.Highlight,
                                e.Bounds);
                e.Graphics.DrawString(
                                e.SubItem.Text,
                                e.Item.Font,
                                SystemBrushes.HighlightText,
                                objStrRect,
                                stringFormat);
                return;
            }

            // 奇数行は背景色を変更して描画
            if ((e.Item.Index % 2) != 0)
            {
                SolidBrush brushes = new SolidBrush(Color.AliceBlue);

                e.Graphics.FillRectangle(
                                brushes,
                                e.Bounds);

                e.Graphics.DrawString(
                                e.SubItem.Text,
                                e.Item.Font,
                                SystemBrushes.WindowText,
                                e.Bounds,
                                stringFormat);
                return;
            }
            else
            {
                e.Graphics.FillRectangle(
                                SystemBrushes.Window,
                                e.Bounds);
                e.Graphics.DrawString(
                                e.SubItem.Text,
                                e.Item.Font,
                                SystemBrushes.WindowText,
                                objStrRect,
                                stringFormat);
            }
            return;
        }


        /**
         * @brief 閉じるボタンメソッド
         */
        private void btnClose_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormReceiveMessageView", "btnClose_Click", "閉じる クリック");
            Close();
        }

        /**
         * @brief メッセージリスト選択イベント
         */
        private void listMsg_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            int idx = 0;
            if (listMsg.SelectedItems.Count > 0)
            {
                idx = listMsg.SelectedItems[0].Index;
                if (selected == LISTVIEW_L1S)
                {
                    byte[] bitD = l1sList[idx].bitdata; // 受信したバイナリデータ
                    string msg = Program.m_BitsToMsg.getMsg(bitD);
                    this.textMsg.Text = msg;

                    // 未読メッセージであれば既読とする
                    if (l1sList[idx].readflg == 0)      // 未読
                    {
                        // DBテーブルのReadFlgを変更
                        Program.m_objDbAccess.SetReadFlg(0, l1sList[idx].id, l1sList[idx].rdate,1);
                        // 選択リストアイテムのBold表示をRegular表示に変更
                        listMsg.Items[idx].Font = new Font(listMsg.Items[idx].Font, FontStyle.Regular);
                    }
                    else if (l1sList[idx].readflg == 1) // 既読
                    {
                        // 何もしない
                    }
                    else
                    {
                        // N/A
                    }

                }
                else if (selected == LISTVIEW_130)
                {
                    if (RescueList[idx].Dt == 1)    // テキスト表示
                    {
                        this.textMsg.Text = RescueList[idx].message;
                        btnOutputBinary.Visible = false;
                    }
                    else if (RescueList[idx].Dt == 0)    // バイナリ表示
                    {
                        this.textMsg.Text = "バイナリデータ" + (RescueList[idx].msglength) + "byte";
                        btnOutputBinary.Visible = true;
                    }
                    else
                    {
                        // N/A
                    }

                    // 未読メッセージであれば既読とする
                    if (RescueList[idx].readflg == 0)      // 未読
                    {
                        // DBテーブルのReadFlgを変更
                        Program.m_objDbAccess.SetReadFlg(1, RescueList[idx].id, RescueList[idx].rdate, 1);
                        // 選択リストアイテムのBold表示をRegular表示に変更
                        listMsg.Items[idx].Font = new Font(listMsg.Items[idx].Font, FontStyle.Regular);
                    }
                    else if (RescueList[idx].readflg == 1) // 既読
                    {
                        // 何もしない
                    }
                    else
                    {
                        // N/A
                    }
                }
            }
            
        }

        /**
         * @brief バイナリ保存ボタン押下イベント
         */
        private void btnOutputBinary_Click(object sender, EventArgs e)
        {

            if (selected == LISTVIEW_L1S)
            {
                // 救助支援情報のみのため災害危機通報選択中は抜ける
                return;
            }

            // ファイルにデータを保存する
            if (listMsg.SelectedItems.Count > 0)
            {
                int idx = listMsg.SelectedItems[0].Index;
                byte[] bitD = new byte[RescueList[idx].msglength];
                bool success = false;

                for (int i = 0; i < bitD.Length; i++)
                {
                    bitD[i] = RescueList[idx].bitMessage[i];
                }

                // ファイル名（yyyymmdd_hhmmss.bin）
                string filename = "./"
                    + RescueList[idx].rdate.Substring(0, 8)
                    + "_"
                    + RescueList[idx].rdate.Substring(8)
                    + ".bin";

                try
                {
                    File.WriteAllBytes(filename, bitD);
                    success = true;
                }
                catch (Exception ex)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormReceiveMessageView", "btnOutputBinary_Click", ex.Message);
                }

                if (success)
                {
                    // 保存成功
                    MessageBox.Show("メッセージバイナリを保存しました。\r\n" + filename);
                }
                else
                {
                    // 保存失敗
                    MessageBox.Show("メッセージバイナリの保存に失敗しました");
                }
            }
        }

        // Type3 送信
        private void btnType3_Click(object sender, EventArgs e)
        {

            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormReceiveMessageView", "btnType3_Click", "新着確認 クリック");

            // 未接続であれば処理しない
            if (Program.m_EquStat.isConnected() == false)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormReceiveMessageView", "btnType3_Click", "未接続です クリック");
                MessageBox.Show("Q-ANPIターミナル未接続の為、救助支援情報の新着確認はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // ボタン押下不可
            btnType3.Enabled = false;
            
            EventRtnRescue(this, "新着確認開始  ");
        }

        public void setBtnEnable(bool on)
        {
            btnType3.Enabled = on;
        }

        /**
         * @brief カテゴリリスト選択イベント
         */
        private void listMsgCategory_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            bool issel = e.IsSelected;
            if (issel == true)
            {
                int type = 0;
                int idx = e.ItemIndex;

                if (idx == LISTVIEW_130)
                {
                    type = 2;
                    listMsg.Columns[2].Width = 60;
                    selected = LISTVIEW_130;
                    // ボタン表示切替
                    btnType3.Visible = true;
                    btnOutputBinary.Visible = false;    // バイナリ情報メッセージ選択時にtrueとなる
                    btnClose.Visible = true;
                    btnIsReceive.Visible = false;
                }
                else if (idx == LISTVIEW_L1S)
                {
                    type = 1;
                    listMsg.Columns[2].Width = 0;
                    selected = LISTVIEW_L1S;
                    btnType3.Visible = false;
                    btnOutputBinary.Visible = false;
                    btnClose.Visible = true;
                    btnIsReceive.Visible = true;
                    
                }
                else
                {
                    type = 0;
                }
                // 2 救助支援　1 災害危機  0 すべて
                loadList(type);
            }
        }


        /**
         * @brief:メッセージ受信時のリスト再生成
         */
        public void listMsg_update(object sender, int code, string id, string outVal)
        {
            if (selected == LISTVIEW_L1S && code == 0)
            {
                loadList(1);
            }
            else if (selected == LISTVIEW_130 && code == 1)
            {
                loadList(2);
            }
            else
            {
                // N/A
            }
        }

        /// <summary>
        /// 受信状態切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIsReceive_CheckedChanged(object sender, EventArgs e)
        {
            //「受信再開」ボタン押下
            if (btnIsReceive.Checked)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormReceiveMessageView", "btnIsReceive_CheckedChanged", "受信状態切り替え 受信停止中→受信中");
                btnIsReceive.Enabled = false;
                btnClose.Enabled = false;   
                m_isReceiving = true;
                //btnIsReceive.Text = "受信停止";
                // 受信再開メッセージをQ-ANPIターミナルに投げる
                Program.m_SubGHz.SendDisasterReport(true);
            }
            //「受信停止」ボタン押下
            else
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormReceiveMessageView", "btnIsReceive_CheckedChanged", "受信状態切り替え 受信中→受信停止中");
                btnIsReceive.Enabled = false;
                btnClose.Enabled = false;
                m_isReceiving = false;
                //btnIsReceive.Text = "受信再開";
                // 受信停止メッセージをQ-ANPIターミナルに投げる
                Program.m_SubGHz.SendDisasterReport(false);
            }

            //タイムアウトの処理
            Task.Factory.StartNew(() =>
            {
                ButtonTimeout();
            });

        }

        /// <summary>
        /// 受信停止/再開ボタンのラベルを設定
        /// </summary>
        public void updateButtonLabel()
        {
            if (m_isReceiving)
            {
                //受信中
                btnIsReceive.Text = "受信停止";
                btnIsReceive.Enabled = true;
                btnClose.Enabled = true;
            }
            else
            {
                btnIsReceive.Text = "受信再開";
                btnIsReceive.Enabled = true;
                btnClose.Enabled = true;
            }
        }

        /// <summary>
        /// 受信停止/再開タイムアウト処理
        /// </summary>
        private void ButtonTimeout()
        {
            const int TIMEOUT_TIME = 60;    // タイムアウトは60秒
            int timeoutCount = 0;
            while (true)
            {
                //Q-ANPIターミナルからの受信処理で受信停止/再開ボタンが活性化されているかチェック
                if (btnIsReceive.Enabled != false)
                {
                    break;
                }
                //タイムアウト
                if (timeoutCount > TIMEOUT_TIME)
                {
                    //受信状態切り替えタイムアウト
                    //ボタンを再活性化
                    if (m_isReceiving)
                    {
                        btnIsReceive.Checked = true;
                        updateButtonLabel();
                    }
                    else
                    {
                        btnIsReceive.Checked = false;
                        updateButtonLabel();
                    }
                    btnIsReceive.Enabled = true;
                    break;
                }

                //途中切断
                if (Program.m_EquStat.isConnected() == false)
                {
                    btnIsReceive.Checked = false;
                    updateButtonLabel();
                    btnIsReceive.Enabled = false;
                    break;
                }

                //1秒後に再チェック
                System.Threading.Thread.Sleep(1000);
                timeoutCount++;
            }
        }        
    }
}
