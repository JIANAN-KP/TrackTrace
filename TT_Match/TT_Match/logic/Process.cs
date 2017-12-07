using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT_Match.model;
using TT_Match.tools;
using Microsoft.Win32;

namespace TT_Match.logic
{
    public class Process
    {
        Reader reader;
        Compare compare;
        string magentaFileDir;
        string resultFileDir;
        string outputFileDir;
        public Process(string magentaFileDir, string resultFileDir,string outputFileDir)
        {
            reader = new Reader();
            compare = new Compare();
            this.magentaFileDir = magentaFileDir;
            this.resultFileDir = resultFileDir;
            this.outputFileDir = outputFileDir;
        }

        public void ProcessFile(string[] exportLines,string markerStr,string sampleCode)
        {
            #region [3]. read export file
            MatchData fileData = new MatchData();
            MagentaData magentaData = new MagentaData();
            string errorMsg = string.Empty;
            /* flag to mark whether the match is already failed */
            switch(sampleCode)
            {
                case "96to96":
                    fileData.ProcessName = Constant.Extrac_96w_96w_ProcessName;
                    break;
                case "48to96":
                    fileData.ProcessName = Constant.Extrac_48w_96w_ProcessName;
                    break;
                case "384":
                    fileData.ProcessName = Constant.Daug_Mplx1_ProcessName;
                    break;
                case "192":
                    fileData.ProcessName = Constant.Daug_Mplx2_ProcessName;
                    break;
                case "96":
                    fileData.ProcessName = Constant.Daug_Mplx4_ProcessName;
                    break;
                default:
                    break;
            }
            /*---- get ExprtFile Data ----*/
            if (markerStr.Equals(Constant.Extraction96_MarkerString))
            {
                FileProcessor.GiveLog("Reading Export File");
                reader.Read_96w_to_96w(exportLines, ref fileData);
                ValidateEmpty(fileData);            
            }
            else if(markerStr.Equals(Constant.Extraction48_MarkerString))
            {
                FileProcessor.GiveLog("Reading Export File");
                reader.Read_48w_to_96w(exportLines, ref fileData);
                ValidateEmpty(fileData);
            }
            else
            {
                FileProcessor.GiveLog("Reading Daug Export File");
                reader.Read_Daug(exportLines, ref fileData);
                ValidateEmpty(fileData);
                ValidateDesDuplication(fileData);
                ValidateVersionNumber(fileData);
                ValidateDaugPlatesNum(fileData);         
            }
            #endregion


            FileProcessor.GiveLog("Reading Magenta File");
            /* loop dictionary, compare values*/
            // Dictionary<string, Queue<KeyValuePair<string, string>>> dict = new Dictionary<string, Queue<KeyValuePair<string, string>>>(fileData.ExpDict);           
            /* set successful at first*/
            foreach (MatchItem item in fileData.MatchQueue)
            {
                if(item.itemResult.Equals(Constant.MatchSucces))
                {
                    string expCode = item.itemQueue.First().Value.GetExpCode();
                    FileProcessor.GiveLog("Comparing   " + expCode);
                    magentaData = reader.Read_Magenta(expCode, markerStr, magentaFileDir);
                    /* new queue by remove version number, original one not affected */
                    Queue<KeyValuePair<string, string>> queue = compare.RemoveVersionNum(item.itemQueue);
                    queue = compare.RemoveDuplicateDestination(queue);                    
                    if (magentaData != null)
                    {
                        if (!(compare.CompareSrcDes(queue, magentaData, sampleCode)))
                        {
                            item.itemResult = Constant.MatchingFail;
                        }
                    }
                    else
                    {
                        FileProcessor.GiveLog("Can not find Magenta file");
                        item.itemResult = Constant.MagentaNotFound;
                    }
                }               
            }   

            FileProcessor.GiveLog("Generating Result File  ");
            FileProcessor.GenerateResult(fileData, markerStr, resultFileDir, outputFileDir);
        }


        #region Validate ['empty' position] [for both daughter and extraction]
        public void ValidateEmpty(MatchData fileData)
        {
            /* Validate the 'Empty' not followed by other destination codes */
            Queue<MatchItem> iQueue = fileData.MatchQueue;
            foreach(MatchItem item in iQueue)
            {
                if(item.itemResult.Equals(Constant.MatchSucces))
                {
                    bool desHasEmpty = false;
                    bool srcHasEmpty = false;
                    foreach (KeyValuePair<string, string> pair in item.itemQueue)
                    {
                        string key = pair.Key;
                        string value = pair.Value;
                        if (key.Equals(Constant.StrSrc))
                        {
                            if (value.Equals(Constant.StrEmpty))
                            {
                                srcHasEmpty = true;
                            }
                            else
                            { //value not equals to 'Empty'
                                if (srcHasEmpty == true)
                                {
                                    FileProcessor.GiveLog("Export File Format Error, ==== Empty Position Issue");
                                    item.itemResult = Constant.EmptyPositionError;
                                    break;
                                }
                            }
                        }
                        else if (key.Equals(Constant.StrDes))
                        {
                            if (value.Equals(Constant.StrEmpty))
                            {
                                desHasEmpty = true;
                            }
                            else
                            {  //value not equals to 'Empty'
                                if (desHasEmpty == true)
                                {
                                    FileProcessor.GiveLog("Export File Format Error, ==== Empty Position Issue");
                                    item.itemResult = Constant.EmptyPositionError;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            #endregion
        }

        #region Validate [version number]  [for daughter only]
        public void ValidateVersionNumber(MatchData fileData)
        {
            /* Validate the 'Empty' not followed by other destination codes */
            Queue<MatchItem> iQueue = fileData.MatchQueue;
            foreach (MatchItem item in iQueue)
            {
                if(item.itemResult.Equals(Constant.MatchSucces))
                {
                    List<string> list = new List<string>();
                    foreach (KeyValuePair<string, string> pair in item.itemQueue)
                    {
                        if (pair.Key.Equals(Constant.StrDes))
                        {
                            string versionNumber = pair.Value.GetVersionCode();
                            if (versionNumber != null)
                            {
                                list.Add(versionNumber);
                            }
                        }
                    }

                    for (int i = 0; i < list.Count - 1; i++)
                    {
                        if (item.itemResult.Equals(Constant.MatchSucces))
                        {
                            for (int j = i + 1; j < list.Count; j++)
                            {
                                if (list[i].Equals(list[j]))
                                {
                                    FileProcessor.GiveLog("Version Number Duplicate Error");
                                    item.itemResult = Constant.VersionNumDupError;
                                    break;
                                }
                            }
                        }else
                        {
                            break;
                        }
                    }
                }                
            }
        }
        #endregion

        public void ValidateDesDuplication(MatchData fileData)
        {
            Queue<MatchItem> itemQueue = fileData.MatchQueue;
            foreach (MatchItem item in itemQueue)
            {
                if(item.itemResult.Equals(Constant.MatchSucces))
                {
                    List<string> expCodeList = new List<string>();
                    foreach (KeyValuePair<string, string> pair in item.itemQueue)
                    {
                        string key = pair.Key;
                        string value = pair.Value;
                        if (key.Equals(Constant.StrDes) && !value.Equals(Constant.StrEmpty))
                        {
                            expCodeList.Add(value.GetOrgCode().GetNumCode());
                        }
                    }
                    bool same = false;
                    for (int i = 0; i < expCodeList.Count - 1; i++)
                    {
                        for (int j = i + 1; j < expCodeList.Count; j++)
                        {
                            if (expCodeList[i] == expCodeList[j])
                            {
                                same = true;
                            }
                        }
                    }
                    if (same == true)  //if two destinations are same, then all the destination should be same.
                    {
                        for (int k = 0; k < expCodeList.Count - 1; k++)
                        {
                            if (expCodeList[k] != expCodeList[k + 1])
                            {
                                FileProcessor.GiveLog("Destination Plate Number Error  "+expCodeList[k+1]);
                                item.itemResult = Constant.DesPlateNumError;
                                break;
                            }
                        }
                    }
                }
                
            }
        }

        /* validate daughter plate number, the first parameter in first line */
        public void ValidateDaugPlatesNum(MatchData fileData)
        {
            int plateRepNum = Convert.ToInt32(Convert.ToDouble(fileData.PlateReplicateNum));
            Queue<MatchItem> itemQueue = FileProcessor.CopyKVQueue(fileData.MatchQueue);
            
            for(int j = 0;j<itemQueue.Count;j++)
            {
                if (itemQueue.ElementAt(j).itemResult.Equals(Constant.MatchSucces))
                {
                    Queue<KeyValuePair<string, string>> queue = itemQueue.ElementAt(j).itemQueue;
                    int i = 0;
                    while (queue.Count > 0)
                    {
                        KeyValuePair<string, string> kpair = queue.Dequeue();
                        if ((kpair.Key.Equals(Constant.StrDes)) && (!kpair.Value.Equals(Constant.StrEmpty)))
                        {
                            i = i + 1;
                        }
                    }
                    if (i != plateRepNum)
                    {
                        FileProcessor.GiveLog("Daughter Plate Number Validation Failed");
                        fileData.MatchQueue.ElementAt(j).itemResult = Constant.PlateNumMatchError;
                        break;
                    }
                }
            }
        }        

    }
}
