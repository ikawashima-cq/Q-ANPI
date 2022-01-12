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
    public partial class FormWait : Form
    {
        public FormWait()
        {
            InitializeComponent();
        }

        private void FormWait_Load(object sender, EventArgs e)
        {
        }

        private void FormWait_Activated(object sender, EventArgs e)
        {
            this.Location = new Point(
                this.Owner.Location.X + (this.Owner.Width - this.Width) / 2,
                this.Owner.Location.Y + (this.Owner.Height - this.Height) / 2);

        }
    }
}
