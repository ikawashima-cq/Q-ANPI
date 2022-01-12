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
    public partial class FormConfirmUpdateLocation : Form
    {
        public bool isUpdateLocation = false;       // 位置情報更新フラグ

        public FormConfirmUpdateLocation()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isUpdateLocation = true;
            Close();
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            isUpdateLocation = false;  
            Close();
        }
    }
}
