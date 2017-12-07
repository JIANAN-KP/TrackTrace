using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT_Match.tools
{
    public static class MyExtensions
    {      

        //string array extension method
        public static int FindFirst(this string[] array,string str)
        {
            int i = -1;
            for(int j = 0;j<array.Length;j++)
            {
                if(array[j].Equals(str))
                {
                    i = j;
                    break;
                }
            }
            return i;
        }


        public static int FindNext(this string[] array,string str,int lastPosition)
        {
            int i = -1;
            for(int j = lastPosition + 1;j<array.Length;j++)
            {
                if(array[j].Equals(str))
                {
                    i = j;
                    break;
                }
            }
            return i;
        }
        //Format ExpCode-OrgCode  -91V1
        public static string GetOrgCode(this String str)
        {
            string[] strs = str.Split('-');
            return strs.Last();
        }

        // get Experiment code like 'NXC116C01'
        public static string GetExpCode(this String str)
        {
            string[] strs = str.Split('-');
            return strs.First();
        }

        // get Destination plate code  like '13V1'
        public static string GetNumCode(this String str)
        {
            int pos = str.IndexOf('V');
            if(pos!=-1)
            {
                str = str.Substring(0, pos);
                return str;
            }else
            {
                return str;
            }            
        }

        public static string GetVersionCode(this String str)
        {
            int pos = str.IndexOf('V');
            string s = null;
            if(pos!=-1)
            {
                s = str.Substring(pos+1);
            }
            return s;
        }
    }
}
