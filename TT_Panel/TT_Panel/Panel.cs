using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Security.Permissions;
using System.IO;

namespace TT_Panel
{
    public partial class Panel : Form
    {
        SetPath spForm;
        RegistryKey key;
        public Panel()
        {
            try
            {
                InitializeComponent();
                spForm = new SetPath();
                key = Registry.CurrentUser.OpenSubKey("software\\tt_panel", true);
                if(key == null)
                {
                    key = Registry.CurrentUser.OpenSubKey("software", true).CreateSubKey("tt_panel");
                }
                //Directory Paths are stored in Registry - CurrentUser subfolder
                SetValue();
                
            }
            catch(Exception e)
            {
                Log.GiveLog(e.ToString());
            } 
        }

        private void SetValue()
        {
            Log.GiveLog("Setting Value");
            if (!(key.GetValue("efDir") == null))
            {
                this.lbEFDir.Text = key.GetValue("efDir").ToString();
            }
            if (!(key.GetValue("mfDir") == null))
            {
                this.lbMFDir.Text = key.GetValue("mfDir").ToString();
            }
            if (!(key.GetValue("rfDir") == null))
            {
                this.lbRFDir.Text = key.GetValue("rfDir").ToString();
            }
            if (!(key.GetValue("ofDir") == null))
            {
                this.lbOFDir.Text = key.GetValue("ofDir").ToString();
            }
            if (!(key.GetValue("bfDir") == null))
            {
                this.lbBFDir.Text = key.GetValue("bfDir").ToString();
            }
        }

        private void btnSetPath_Click(object sender, EventArgs e)
        {
            DialogResult dr = spForm.ShowDialog();
            if(dr==DialogResult.OK)
            {
                string pathName = spForm.PathName;
                string pathValue = spForm.PathValue;
                switch(pathName)
                {
                    case "Export File Directory":
                        lbEFDir.Text = pathValue;
                        key.SetValue("efDir", lbEFDir.Text.ToString());
                        break;
                    case "Magenta File Directory":
                        lbMFDir.Text = pathValue;
                        key.SetValue("mfDir", lbMFDir.Text.ToString());
                        break;
                    case "Result File Directory":
                        lbRFDir.Text = pathValue;
                        key.SetValue("rfDir", lbRFDir.Text.ToString());
                        break;
                    case "Output File Directory":
                        lbOFDir.Text = pathValue;
                        key.SetValue("ofDir", lbOFDir.Text.ToString());
                        break;
                    case "Backup File Directory":
                        lbBFDir.Text = pathValue;
                        key.SetValue("bfDir", lbBFDir.Text.ToString());
                        break;
                }               
            }else if(dr==DialogResult.Cancel)
            {
                
            }
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
      
        private void btnCleanText_Click(object sender, EventArgs e)
        {
            this.richTextBox.Clear();
        }

        private void btnLastLog_Click(object sender, EventArgs e)
        {
            string path = @"C:\ProgramData\TT_log\matchlog.txt";
            try
            {
                string[] lines = File.ReadAllLines(path);
                int fstart = 0;
                for(int i = lines.Length-1;i>0;i--)
                {
                    if(lines[i].Equals("==============="))
                    {
                        fstart = i;
                        break;
                    }
                }
                for(int j = fstart;j<lines.Length;j++)
                {
                    this.richTextBox.AppendText(lines[j]+Environment.NewLine);
                }
            }
            catch(Exception exp)
            {
                this.richTextBox.AppendText(exp.ToString());
            }
        }
    }
}
