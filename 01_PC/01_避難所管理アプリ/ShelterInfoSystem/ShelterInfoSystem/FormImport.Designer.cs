namespace ShelterInfoSystem
{
    partial class FormImport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnImport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.importFilename = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.selectFile = new System.Windows.Forms.Button();
            this.openImportFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.fileImportWorker = new System.ComponentModel.BackgroundWorker();
            this.importWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // btnImport
            // 
            this.btnImport.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnImport.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnImport.Enabled = false;
            this.btnImport.Font = new System.Drawing.Font("Meiryo UI", 15.75F);
            this.btnImport.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnImport.Location = new System.Drawing.Point(110, 107);
            this.btnImport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(120, 38);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "インポート";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Meiryo UI", 15.75F);
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCancel.Location = new System.Drawing.Point(253, 107);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 38);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // importFilename
            // 
            this.importFilename.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.importFilename.Location = new System.Drawing.Point(27, 53);
            this.importFilename.Multiline = false;
            this.importFilename.Name = "importFilename";
            this.importFilename.Size = new System.Drawing.Size(344, 30);
            this.importFilename.TabIndex = 0;
            this.importFilename.Text = "";
            this.importFilename.TextChanged += new System.EventHandler(this.importFilename_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(23, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(255, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "インポートするファイルを選択してください";
            // 
            // selectFile
            // 
            this.selectFile.Location = new System.Drawing.Point(377, 53);
            this.selectFile.Name = "selectFile";
            this.selectFile.Size = new System.Drawing.Size(86, 30);
            this.selectFile.TabIndex = 1;
            this.selectFile.Text = "ファイルを選択";
            this.selectFile.UseVisualStyleBackColor = true;
            this.selectFile.Click += new System.EventHandler(this.selectFile_Click);
            // 
            // openImportFileDialog
            // 
            this.openImportFileDialog.Filter = "Excel ブック|*.xlsx|Excel 97-2003 ブック|*.xls|CSV（カンマ区切り)|*.csv";
            // 
            // FormImport
            // 
            this.AcceptButton = this.btnImport;
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(482, 165);
            this.ControlBox = false;
            this.Controls.Add(this.selectFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.importFilename);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnImport);
            this.Name = "FormImport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "避難所情報システム  －ファイル選択";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormImport_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormImport_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RichTextBox importFilename;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button selectFile;
        private System.Windows.Forms.OpenFileDialog openImportFileDialog;
        private System.ComponentModel.BackgroundWorker fileImportWorker;
        private System.ComponentModel.BackgroundWorker importWorker;
    }
}