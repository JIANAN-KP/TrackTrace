using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TT_Match.model;
using Newtonsoft.Json;

namespace TT_Match.tools
{
    public class FileProcessor
    {
        public static string GetMagentaFileCode(string fileName)
        {
            string[] fileNames = fileName.Split(new char[] { '_', '-' });
            return fileNames[0];
        }

        public static Dictionary<string, Queue<KeyValuePair<string, string>>> CopyDictionary(Dictionary<string, Queue<KeyValuePair<string, string>>> orgDict)
        {
            Dictionary<string, Queue<KeyValuePair<string, string>>> desDict = new Dictionary<string, Queue<KeyValuePair<string, string>>>();
            foreach(KeyValuePair<string,Queue<KeyValuePair<string,string>>> pair in orgDict)
            {
                desDict.Add(pair.Key, new Queue<KeyValuePair<string, string>>(pair.Value));
            }
            return desDict;
        }

        public static Queue<MatchItem> CopyKVQueue(Queue<MatchItem> srcQueue)
        {
            Queue<MatchItem> orgQueue = new Queue<MatchItem>();
            foreach(MatchItem matchItem in srcQueue)
            {
                MatchItem item = new MatchItem();
                foreach(KeyValuePair<string,string> pair in matchItem.itemQueue)
                {
                    item.itemQueue.Enqueue(new KeyValuePair<string, string>(pair.Key, pair.Value));
                }
                item.itemResult = matchItem.itemResult;
                orgQueue.Enqueue(item);
            }
            return orgQueue;
        }

        public static bool CheckCodeExist(Queue<KeyValuePair<string,string>> srcQueue,KeyValuePair<string,string> srcPair)
        {
            bool flag = false;
            foreach(KeyValuePair<string,string> pair in srcQueue)
            {
                if(pair.Key.Equals(srcPair.Key)&&(pair.Value.Equals(srcPair.Value.GetNumCode())))
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        /* find the first position of the specific string */
        public static int FindPosition(string[] orgStrs, string spcStr)
        {
            int i = -1;
            for (int j = 0; j < orgStrs.Length; j++)
            {
                if (spcStr.Equals(orgStrs[j]))
                {
                    i = j;
                    break;
                }
            }
            /* return -1 if not found */
            /* return position if found */
            return i;
        }

        public static void GenerateResult(MatchData fileData,string makerString,string resultFileDir,string outputFileDir)
        {
            string fileName = "";
            bool flag = true;
            if ((makerString.Equals(Constant.Extraction96_MarkerString)) || (makerString.Equals(Constant.Extraction48_MarkerString)))
            {
                fileName = "Ext-" + fileData.FileMaker + "-InComplete"+fileData.TimeStamp.ToString("yyyyMMdd-HHmmss");
            }
            else
            {
                FileProcessor.GiveLog("Generating fileName");
                fileName = "Daug-" + fileData.FileMaker + "-InComplete"+fileData.TimeStamp.ToString("yyyyMMdd-HHmmss");
            }
            string filePath = resultFileDir + "\\"+ fileName+".txt";
            FileProcessor.GiveLog("Transfering Json File");
            string json = JsonConvert.SerializeObject(fileData, Formatting.Indented);
            FileProcessor.GiveLog("Generating Result File");
            File.WriteAllText(filePath, json);  // Generate Result File 
            foreach(MatchItem item in fileData.MatchQueue)
            {
                if(!item.itemResult.Equals(Constant.MatchSucces))
                {
                    flag = false;
                    break;
                }
            }
            string outPutPath = outputFileDir + "\\" + "MatchResult.txt";
            if(flag == true)
            {
                File.WriteAllText(outPutPath,Constant.MatchSucces);
            }
            else
            {
                File.WriteAllText(outPutPath, Constant.MatchFail);
            }
        }

        public static void GiveLog(string log)
        {            
            using (FileStream fs = new FileStream(Constant.LogFileDir, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter sr = new StreamWriter(fs))
                {
                    sr.WriteLine(log);
                }
            }
        }

    }
}
