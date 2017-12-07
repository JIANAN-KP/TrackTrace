using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TT_Scan_Upload
{
    public class Constant
    {
        public string qDriveLoc = @"Q:\WL\TT_Result files\Current";
        public string cDriveBakLoc = "";
        public string suLogFile = "";
        public void GiveLog(string log)
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
