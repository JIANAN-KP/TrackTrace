using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Xml;

namespace TT_Scan
{
    class Program
    {
        static int Main(string[] args)
        {
            #region initialze parameters
            string currentPath = "";
            string archivePath = "";
            string duplicatePath = "";
            string logFilePath = "";
            #endregion
            #region load configuration
            //load configuration
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Path.Combine(@"C:\TTScanFolder", "config.xml"));
                XmlNodeList xnList = xml.SelectNodes("/root");
                foreach (XmlNode xNode in xnList)
                {
                    foreach (XmlNode childNode in xNode.ChildNodes)
                    {
                        string attrValue = childNode.Attributes["path"].Value;
                        string nodeName = childNode.Name;
                        switch (nodeName)
                        {
                            case "current":
                                currentPath = attrValue;
                                break;
                            case "archive":
                                archivePath = attrValue;
                                break;
                            case "duplicate":
                                duplicatePath = attrValue;
                                break;
                            case "log_file":
                                logFilePath = attrValue;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return 1;
            }
            #endregion

            FileProcessor fProcessor = new FileProcessor();
            fProcessor.GiveLog("========================================================", logFilePath);
            fProcessor.GiveLog(DateTime.Now.ToString(), logFilePath);
            List<FileInfo> fileLists = new List<FileInfo>();
            DBOperator dbOperator = new DBOperator();

            #region retrieve data
            try
            {
                if (!Directory.Exists(currentPath)||!Directory.Exists(archivePath))
                {
                    fProcessor.GiveLog("Q drive connection lost.............",logFilePath);
                    return 1;
                }
                else
                {
                    //step 1, get out all files information                 
                    DirectoryInfo dirInfo = new DirectoryInfo(currentPath);
                    fileLists = dirInfo.GetFiles().ToList();
                    dbOperator.OpenConnection();
                    foreach (FileInfo fileInfo in fileLists)
                    {
                        bool dupSrc = false;
                        // step 2, update database
                        using (StreamReader reader = new StreamReader(fileInfo.FullName))
                        {
                            string json = reader.ReadToEnd();
                            MatchData matchData = JsonConvert.DeserializeObject<MatchData>(json);
                            string updateTitle = "";
                            string matchType = fileInfo.Name.Split('-').ToArray().First();
                            string completeStatus = matchData.CompleteStatus;
                            foreach(MatchItem item in matchData.MatchQueue)
                            { 
                                fProcessor.GiveLog("Processing Experiment +  " , logFilePath);
                                Queue<KeyValuePair<string, string>> qu = fProcessor.FilterEmptyItem(item.itemQueue);
                                Dictionary<string, string> dict = fProcessor.ConvertQueue(qu,ref dupSrc);
                                if(dupSrc==true)
                                {                               
                                    break;
                                }
                                int iuFlag = 0; //extraction items changed
                                int duFlag = 0; //daughter items changed
                                Dictionary<string,int> updateDic = dbOperator.InsertUpdateResults(dict, matchData, item, ref iuFlag, ref duFlag, matchType);
                                if (iuFlag > 0)
                                {
                                    if (!item.itemResult.Equals(Constant.MatchSucces))
                                    {
                                        updateTitle = Constant.ExtFailMatchTitle;
                                    }
                                    else if (completeStatus.Equals(Constant.ScriptFail))
                                    {
                                        updateTitle = Constant.ExtFailScriptTitle;
                                    }
                                    else
                                    {
                                        updateTitle = Constant.ExtSuccessCompleteTitle;
                                    }
                                    dbOperator.InsertUpdateSummaryInfo(item, updateDic, updateTitle, true);
                                }
                                else if (duFlag > 0)
                                {
                                    if (!item.itemResult.Equals(Constant.MatchSucces))
                                    {
                                        updateTitle = Constant.DaugFailMatchTitle;

                                    }
                                    else if (completeStatus.Equals(Constant.ScriptFail))
                                    {
                                        updateTitle = Constant.DaugFailScriptTitle;
                                    }
                                    else
                                    {
                                        updateTitle = Constant.DaugSuccessCompleteTitle;
                                    }
                                    dbOperator.InsertUpdateSummaryInfo(item, updateDic, updateTitle, false);
                                }
                            }
                        }
                        if(dupSrc==true)
                        {
                            fProcessor.GiveLog("Moving files from current to duplicate", logFilePath);
                            File.Copy(fileInfo.FullName, duplicatePath + "\\" + fileInfo.Name, true);
                            File.Delete(fileInfo.FullName);
                            fProcessor.GiveLog("Duplicate +1", logFilePath);
                        }
                        else
                        {
                            // step 3, move files, network disconnection will affect here
                            fProcessor.GiveLog("Moving files from current to archive", logFilePath);
                            File.Copy(fileInfo.FullName, archivePath + "\\" + fileInfo.Name, true);
                            File.Delete(fileInfo.FullName);
                            fProcessor.GiveLog("Scan upload completed +1", logFilePath);
                        }
                        
                    }
                    dbOperator.CloseConnection();       
                }
            }
            catch(Exception e)
            {
                fProcessor.GiveLog(e.ToString(),logFilePath);
                return 1;
            }
            #endregion
            return 0;
        }
    }
}
