using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TT_Scan
{
    public class FileProcessor
    {
        public void GiveLog(string log,string logPath)
        {
            if(!File.Exists(logPath))
            {
                File.WriteAllText(logPath,log + Environment.NewLine);
            }
            else
            {
                File.AppendAllText(logPath, log + Environment.NewLine);
            }
        }

        public Dictionary<string,string> ConvertQueue(Queue<KeyValuePair<string,string>> queue,ref bool dupSrc)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Queue<KeyValuePair<string, string>> tempQueue = DequeueOneLine(ref queue);
            List<string> srcs = new List<string>();
            StringBuilder des = new StringBuilder();            
            foreach (KeyValuePair<string, string> pair in tempQueue)
            {                
                if (pair.Key.ToLower().Equals(Constant.StrSrc))
                {
                    srcs.Add(pair.Value);
                }
                else
                {
                    des.Append(pair.Value).Append(','); //append destination value and one space destination 1 2 3
                }
            }
            foreach(string s in srcs)
            {
                if(!dict.ContainsKey(s))
                {
                    dict.Add(s, des.ToString().Trim(','));
                }else
                {
                    dupSrc = true;
                }           
            }
            return dict;
        }

        public Queue<KeyValuePair<string, string>> FilterEmptyItem(Queue<KeyValuePair<string, string>> queue)
        {
            Queue<KeyValuePair<string, string>> temp = new Queue<KeyValuePair<string, string>>();
            while(queue.Count>0)
            {
                KeyValuePair<string, string> kvp = queue.Dequeue();
                if(!kvp.Value.Equals(Constant.StrEmpty))
                {
                    temp.Enqueue(kvp);
                }
            }
            return temp;
        }

        public Queue<KeyValuePair<string, string>> DequeueOneLine(ref Queue<KeyValuePair<string, string>> orgQueue)
        {
            Queue<KeyValuePair<string, string>> queue = new Queue<KeyValuePair<string, string>>();
            int mark = 0;
            bool loop = true;
            while (loop && (orgQueue.Count != 0))
            {
                if ((orgQueue.First().Key.Equals(Constant.StrSrc)) && (mark == 0))
                {
                    queue.Enqueue(orgQueue.Dequeue());
                }
                else if (orgQueue.First().Key.Equals(Constant.StrDes))
                {
                    queue.Enqueue(orgQueue.Dequeue());
                    mark = 1;
                }
                else //key equalto src whil mark =1 , next line start 
                {
                    loop = false;
                }
            }
            return queue;
        }


        public Queue<KeyValuePair<string, string>> RemoveSrcVersion(Queue<KeyValuePair<string, string>> srcQueue)
        {
            Queue<KeyValuePair<string, string>> queue = new Queue<KeyValuePair<string, string>>();
            foreach (KeyValuePair<string, string> pair in srcQueue)
            {
                KeyValuePair<string, string> p = new KeyValuePair<string, string>(pair.Key, GetNumCode(pair.Value));
                queue.Enqueue(p);
            }
            return queue;
        }

        public string GetNumCode(string org)
        {
            int pos = org.IndexOf('V');
            if (pos != -1)
            {
                org = org.Substring(0, pos);
            }
            return org;
        }
    }
}
