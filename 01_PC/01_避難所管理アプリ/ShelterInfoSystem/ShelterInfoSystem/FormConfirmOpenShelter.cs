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
    public partial class FormConfirmOpenShelter : Form
    {
        public bool isClearInfomation = false;      // 情報削除フラグ
        public bool isCanceled = false;             // キャンセルフラグ

        public FormConfirmOpenShelter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isClearInfomation = true;
            isCanceled = false;
            Close();
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            isClearInfomation = false;
            isCanceled = false;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isClearInfomation = false;
            isCanceled = true;
            Close();
        }
    }
}
