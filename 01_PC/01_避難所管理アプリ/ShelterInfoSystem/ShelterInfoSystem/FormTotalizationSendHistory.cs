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
    public partial class FormTotalizationSendHistory : Form
    {
        private string[,] m_testData1 =
        {
            {"1", "2016/03/01 12:40",　"○", "55", "0", "30", "25", "1", "0", "1", "5", "1", "1", "3","フリーテキスト2" },
            {"2", "2016/03/01 12:30",　"○", "55", "0", "30", "25", "1", "0", "1", "1", "1", "1", "1","フリーテキスト1" },
        };

        public FormTotalizationSendHistory()
        {
            InitializeComponent();

            // 行データの値を設定
            string[] strItems = new string[16];

            List<DbAccess.TotalSendLog> logList = new List<DbAccess.TotalSendLog>();
            // ログリスト取得
            Program.m_objDbAccess.GetTotalSendLogList(Program.m_objActiveTermial.sid, ref logList);

            // 行データの追加
            int nIdx;
            int nItemCnt = logList.Count;
            for (nIdx = 0; nIdx < nItemCnt; nIdx++)
            {
                // person_infoをstring配列に変換
                strItems = ConvTotalSendLogToStringList(logList[nIdx]);

                listData.Items.Add(new ListViewItem(strItems));
            }
            // テストデータのためコメント化
            //int nIdx;
            //int nItemCnt = m_testData1.Length / strItems.Length;
            //for (nIdx = 0; nIdx < nItemCnt; nIdx++)
            //{
            //    if ((m_testData1.Length / strItems.Length) > nIdx)
            //    {
            //        int nColIdx;
            //        for (nColIdx = 0; nColIdx < strItems.Length; nColIdx++)
            //        {
            //            strItems[nColIdx] = m_testData1[nIdx, nColIdx];
            //        }
            //    }
            //    else
            //    {
            //        int nColIdx;
            //        for (nColIdx = 0; nColIdx < strItems.Length; nColIdx++)
            //        {
            //            strItems[nColIdx] = "";
            //        }
            //    }
            //    listData.Items.Add(new ListViewItem(strItems));
            //}
        }

        private string[] ConvTotalSendLogToStringList(DbAccess.TotalSendLog log)
        {
            string[] strItems = new string[16];
            int nIdx;

            // 初期化
            for (nIdx = 0; nIdx < strItems.Length; nIdx++)
            {
                strItems[nIdx] = "";
            }

            int nStrListIdx = 0;
            // No
            strItems[nStrListIdx] = "0";
            nStrListIdx++;

            // 登録日時
            strItems[nStrListIdx] = log.update_datetime;
            nStrListIdx++;

            // 結果
            switch (log.sendresult)
            {
                case "0":
                    strItems[nStrListIdx] = "×";
                    break;
                case "1":
                    strItems[nStrListIdx] = "○";
                    break;
                default:
                    strItems[nStrListIdx] = "×";
                    break;
            }
            nStrListIdx++;
            
            // 避難者数
            strItems[nStrListIdx] = log.num01;
            nStrListIdx++;

            //在宅数
            strItems[nStrListIdx] = log.num02;
            nStrListIdx++;

            //男性
            strItems[nStrListIdx] = log.num03;
            nStrListIdx++;

            //女性
            strItems[nStrListIdx] = log.num04;
            nStrListIdx++;

            //ケガ
            strItems[nStrListIdx] = log.num05;
            nStrListIdx++;

            //要介護者
            strItems[nStrListIdx] = log.num06;
            nStrListIdx++;

            //障がい者
            strItems[nStrListIdx] = log.num07;
            nStrListIdx++;

            //妊産婦
            strItems[nStrListIdx] = log.num08;
            nStrListIdx++;

            //高齢者
            strItems[nStrListIdx] = log.num09;
            nStrListIdx++;

            //乳児
            strItems[nStrListIdx] = log.num10;
            nStrListIdx++;

            //避難所内
            strItems[nStrListIdx] = log.num11;
            nStrListIdx++;

            //避難所外
            strItems[nStrListIdx] = log.num12;
            nStrListIdx++;

            //避難所情報
            strItems[nStrListIdx] = log.txt01;
            nStrListIdx++;
            

            return strItems;
        }

        private void listPersonal_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            // 描画はOSに任せる
            e.DrawDefault = true;
        }

        private void listPersonal_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            StringFormat stringFormat = new StringFormat();
            Rectangle objStrRect = e.Bounds;

            switch (e.ColumnIndex)
            {
                default:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    objStrRect = e.Bounds;
                    break;
                //case :
                //    stringFormat.Alignment = StringAlignment.Near;
                //    stringFormat.LineAlignment = StringAlignment.Center;
                //    objStrRect = new Rectangle(e.Bounds.X + FormMain.LIST_PADDING, e.Bounds.Y, e.Bounds.Width - FormMain.LIST_PADDING, e.Bounds.Height);
                //    break;
                //case :
                //    stringFormat.Alignment = StringAlignment.Far;
                //    stringFormat.LineAlignment = StringAlignment.Center;
                //    objStrRect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - FormMain.LIST_PADDING, e.Bounds.Height);
                //    break;
            }

            // 選択されているアイテムの描画
            //if ((e.ItemState & ListViewItemStates.Selected)
            //                == ListViewItemStates.Selected)
            if(e.Item.Selected) 
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
//                SolidBrush brushes = new SolidBrush(Color.LightBlue);
                SolidBrush brushes = new SolidBrush(Color.AliceBlue);
//                SolidBrush brushes = new SolidBrush(Color.Azure);

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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
