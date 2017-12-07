using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;

namespace TT_Scan_Upload
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string suLogFile = @"C:\ProgramData\TT_log\scanuploadlog.txt";
            //string suLogFile = @"D:\TTMatchTest\scanuploadlog.txt";
            try
            {
                string srcDriveloc = "";
                string cDriveBakLoc = "";
                string qDriveLoc = @"Q:\WL\TT_Result files\Current";
                RegistryKey key = Registry.CurrentUser.OpenSubKey("software\\tt_panel", true);
                if (!(key.GetValue("bfDir") == null))
                {
                    cDriveBakLoc = key.GetValue("bfDir").ToString();
                }
                if (!(key.GetValue("rfDir") == null))
                {
                    srcDriveloc = key.GetValue("rfDir").ToString();
                }
                GiveLog("=================================", suLogFile);
                GiveLog(DateTime.Now.ToString(), suLogFile);
                DirectoryInfo dirInfo = new DirectoryInfo(srcDriveloc);
                List<FileInfo> list = dirInfo.GetFiles("*.txt").ToList();
                #region Q drive backup
                foreach (FileInfo file in list)
                {
                    string desFile = Path.Combine(qDriveLoc, file.Name);
                    File.Copy(file.FullName, desFile, true);
                }
                GiveLog("Upload Successfully", suLogFile);
                #endregion
                #region C drive backup
                foreach (FileInfo file in list)
                {
                    string desFile = Path.Combine(cDriveBakLoc, file.Name);
                    File.Copy(file.FullName, desFile, true);
                }
                GiveLog("Backup Successfully", suLogFile);
                #endregion
                #region delete source files
                foreach (FileInfo file in list)
                {
                    File.Delete(file.FullName);
                }
                #endregion
                GiveLog("Source Folder Cleaned", suLogFile);
            }catch(Exception exp)
            {
                GiveLog(exp.ToString(), suLogFile);
            }
        }

        public static void GiveLog(string log,string suLogFile)
        {
            if (!File.Exists(suLogFile))
            {
                File.WriteAllText(suLogFile, log + Environment.NewLine);
            }
            else
            {
                File.AppendAllText(suLogFile, log + Environment.NewLine);
            }
        }
    }
}
