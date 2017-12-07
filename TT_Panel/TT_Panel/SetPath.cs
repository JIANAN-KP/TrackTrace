using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TT_Panel
{
    public partial class SetPath : Form
    {
        public string PathName { get; set; }
        public string PathValue { get; set; } 
        public SetPath()
        {
            InitializeComponent();
            this.lbDirValErr.Visible = false;
            this.lbDirNameErr.Visible = false;
            this.cbPathName.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog.ShowDialog()==DialogResult.OK)
            {
                string fileName = folderBrowserDialog.SelectedPath.ToString();
                tbPath.Text = fileName;
                this.lbDirValErr.Visible = false;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if(cbPathName.SelectedIndex==0)
            {
                this.lbDirNameErr.Visible = true;
            }else if(folderBrowserDialog.SelectedPath.ToString().Equals(""))
            {
                this.lbDirValErr.Visible = true;
            }else
            {
                this.PathName = this.cbPathName.SelectedItem.ToString();
                this.PathValue = this.folderBrowserDialog.SelectedPath.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }           
        }

        private void cbPathName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lbDirNameErr.Visible = false;
        }
    }
}
