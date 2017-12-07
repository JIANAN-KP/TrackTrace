namespace TT_Panel
{
    partial class SetPath
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.cbPathName = new System.Windows.Forms.ComboBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.lbPathName = new System.Windows.Forms.Label();
            this.lbDirNameErr = new System.Windows.Forms.Label();
            this.lbDirValErr = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Sitka Text", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.Maroon;
            this.btnCancel.Location = new System.Drawing.Point(257, 167);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(118, 40);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Font = new System.Drawing.Font("Sitka Text", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.ForeColor = System.Drawing.Color.Maroon;
            this.btnConfirm.Location = new System.Drawing.Point(133, 167);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(118, 40);
            this.btnConfirm.TabIndex = 0;
            this.btnConfirm.Text = "confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // cbPathName
            // 
            this.cbPathName.FormattingEnabled = true;
            this.cbPathName.Items.AddRange(new object[] {
            "<Select Directory>",
            "Export File Directory",
            "Magenta File Directory",
            "Result File Directory",
            "Output File Directory",
            "Backup File Directory"});
            this.cbPathName.Location = new System.Drawing.Point(55, 50);
            this.cbPathName.Name = "cbPathName";
            this.cbPathName.Size = new System.Drawing.Size(425, 26);
            this.cbPathName.TabIndex = 2;
            this.cbPathName.SelectedIndexChanged += new System.EventHandler(this.cbPathName_SelectedIndexChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.ForeColor = System.Drawing.Color.Maroon;
            this.btnBrowse.Location = new System.Drawing.Point(407, 111);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 30);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbPath
            // 
            this.tbPath.Location = new System.Drawing.Point(55, 112);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(350, 28);
            this.tbPath.TabIndex = 4;
            // 
            // lbPathName
            // 
            this.lbPathName.AutoSize = true;
            this.lbPathName.Font = new System.Drawing.Font("Sitka Text", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPathName.ForeColor = System.Drawing.Color.Maroon;
            this.lbPathName.Location = new System.Drawing.Point(29, 15);
            this.lbPathName.Name = "lbPathName";
            this.lbPathName.Size = new System.Drawing.Size(196, 32);
            this.lbPathName.TabIndex = 5;
            this.lbPathName.Text = "Directory Name:";
            // 
            // lbDirNameErr
            // 
            this.lbDirNameErr.AutoSize = true;
            this.lbDirNameErr.BackColor = System.Drawing.Color.Transparent;
            this.lbDirNameErr.ForeColor = System.Drawing.Color.Red;
            this.lbDirNameErr.Location = new System.Drawing.Point(222, 25);
            this.lbDirNameErr.Name = "lbDirNameErr";
            this.lbDirNameErr.Size = new System.Drawing.Size(260, 18);
            this.lbDirNameErr.TabIndex = 6;
            this.lbDirNameErr.Text = "Please select directory name";
            this.lbDirNameErr.Visible = false;
            // 
            // lbDirValErr
            // 
            this.lbDirValErr.AutoSize = true;
            this.lbDirValErr.BackColor = System.Drawing.Color.Transparent;
            this.lbDirValErr.ForeColor = System.Drawing.Color.Red;
            this.lbDirValErr.Location = new System.Drawing.Point(53, 146);
            this.lbDirValErr.Name = "lbDirValErr";
            this.lbDirValErr.Size = new System.Drawing.Size(269, 18);
            this.lbDirValErr.TabIndex = 6;
            this.lbDirValErr.Text = "Directory should not be empty";
            this.lbDirValErr.Visible = false;
            // 
            // SetPath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 228);
            this.Controls.Add(this.lbDirValErr);
            this.Controls.Add(this.lbDirNameErr);
            this.Controls.Add(this.lbPathName);
            this.Controls.Add(this.tbPath);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.cbPathName);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SetPath";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SetPath";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.ComboBox cbPathName;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Label lbPathName;
        private System.Windows.Forms.Label lbDirNameErr;
        private System.Windows.Forms.Label lbDirValErr;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}