using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TT_Panel
{
    public static class Log
    {
        public static void GiveLog(string str)
        {
            if (!File.Exists(@"C:\TT_Panel_Log.text"))
            {
                //File.WriteAllText(@"C:\TT_Panel_Log.text", str);
            }else
            {
                //File.AppendAllText(@"C:\TT_Panel_Log.text", str);
            }
        }
    }
}
