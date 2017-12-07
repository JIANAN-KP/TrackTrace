using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TT_Match.tools;
using TT_Match.model;

namespace TT_Match.logic
{
    public class Reader
    {
        public void Read_96w_to_96w(string[] exportLines, ref MatchData fileData)
        {
            List<string> columns = new List<string>();
            /* read title */
            Read_ExtractionFile_Title(exportLines[0], ref fileData, Constant.Extrac_96w_96w);
            columns = exportLines[1].Split(Constant.Export_File_Delimiter).ToList();
            fileData.FileMaker= Quotes.RemoveQuotes(columns[2]).ToString();
            string expCode = "NONE";
            bool tillDes = false;
            MatchItem matchItem = new MatchItem();
            string result = Constant.MatchSucces;
            #region for loop
            foreach (string line in exportLines)
            {
                columns = line.Split(Constant.Export_File_Delimiter).ToList();
                
                switch (Quotes.RemoveQuotes(columns[0]).ToLower())
                {
                    case "source":
                        if (tillDes)
                        {
                            if (expCode.Equals("NONE"))
                            {
                                FileProcessor.GiveLog("Read Error, All Plate Empty");
                                result = Constant.AllEmptyError;
                            }

                            matchItem.itemResult = result;
                            fileData.MatchQueue.Enqueue(matchItem);
                            //clear data
                            expCode = "NONE";
                            tillDes = false;
                            result = Constant.MatchSucces;
                            matchItem = new MatchItem();                            
                        }
                        if (!(Quotes.RemoveQuotes(columns[2]).Equals(Constant.StrEmpty)))
                        {
                            if(expCode.Equals("NONE"))
                            {
                                expCode = Quotes.RemoveQuotes(columns[2]).GetExpCode();
                            }else
                            {
                                if(!expCode.Equals(Quotes.RemoveQuotes(columns[2]).GetExpCode()))
                                {
                                    FileProcessor.GiveLog("Source Experiment Code Error "+ Quotes.RemoveQuotes(columns[2]));
                                    result = Constant.ExpCodeError;
                                }
                            }
                            matchItem.itemQueue.Enqueue(new KeyValuePair<string, string>(Constant.StrSrc, Quotes.RemoveQuotes(columns[2])));
                        }
                        else
                        {
                            matchItem.itemQueue.Enqueue(new KeyValuePair<string, string>(Constant.StrSrc, Constant.StrEmpty));
                        }
                        break;                       
                    case "destination":
                        tillDes = true;
                        if (!(Quotes.RemoveQuotes(columns[2]).Equals(Constant.StrEmpty)))
                        {
                            if (expCode.Equals("NONE"))
                            {
                                expCode = Quotes.RemoveQuotes(columns[2]).GetExpCode();
                            }
                            else
                            {
                                if (!expCode.Equals(Quotes.RemoveQuotes(columns[2]).GetExpCode()))
                                {
                                    FileProcessor.GiveLog("Destination Experiment Code Error "+ Quotes.RemoveQuotes(columns[2]));
                                    result = Constant.ExpCodeError;
                                }
                            }
                            matchItem.itemQueue.Enqueue(new KeyValuePair<string, string>(Constant.StrDes, Quotes.RemoveQuotes(columns[2])));
                        }
                        else
                        {
                            matchItem.itemQueue.Enqueue(new KeyValuePair<string, string>(Constant.StrDes, Constant.StrEmpty));
                        }
                        break;
                }
            }
            #endregion
            if (expCode.Equals("NONE"))
            {
                FileProcessor.GiveLog("Read Error, All Plate Empty");
                result = Constant.AllEmptyError;
            }

            matchItem.itemResult = result;
            fileData.MatchQueue.Enqueue(matchItem);

            expCode = "NONE";
            tillDes = false;
            result = Constant.MatchSucces;
            matchItem = new MatchItem();
        }

        public void Read_48w_to_96w(string[] exportLines, ref MatchData fileData)
        {
            List<string> columns = new List<string>();
            //title
            Read_ExtractionFile_Title(exportLines[0], ref fileData, Constant.Extrac_48w_96w);
            columns = exportLines[1].Split(Constant.Export_File_Delimiter).ToList();
            fileData.FileMaker = Quotes.RemoveQuotes(columns[2]).ToString();
            string expCode = "NONE";
            bool tillDes = false;
            string result = Constant.MatchSucces;
            MatchItem matchItem = new MatchItem();
            //Source&Destination
            #region for loop
            foreach (string line in exportLines)
            {
                columns = line.Split(Constant.Export_File_Delimiter).ToList();
                switch (Quotes.RemoveQuotes(columns[0]).ToLower())
                {
                    case "source":
                        if (tillDes)
                        {
                            if (expCode.Equals("NONE"))
                            {
                                FileProcessor.GiveLog("Read Error, All Plate Empty");
                                result = Constant.AllEmptyError;
                            }

                            matchItem.itemResult = result;
                            fileData.MatchQueue.Enqueue(matchItem);

                            expCode = "NONE";
                            tillDes = false;
                            result = Constant.MatchSucces;
                            matchItem = new MatchItem();

                        }

                        
                        if (!(Quotes.RemoveQuotes(columns[2]).Equals(Constant.StrEmpty)))
                        {
                            if (expCode.Equals("NONE"))
                            {
                                expCode = Quotes.RemoveQuotes(columns[2]).GetExpCode();
                            }
                            else
                            {
                                if (!expCode.Equals(Quotes.RemoveQuotes(columns[2]).GetExpCode()))
                                {
                                    FileProcessor.GiveLog("Source Experiment Code Error "+ Quotes.RemoveQuotes(columns[2]));
                                    result = Constant.ExpCodeError;
                                }
                            }
                            matchItem.itemQueue.Enqueue(new KeyValuePair<string, string>(Constant.StrSrc, Quotes.RemoveQuotes(columns[2])));
                        }
                        else
                        {
                            matchItem.itemQueue.Enqueue(new KeyValuePair<string, string>(Constant.StrSrc, Constant.StrEmpty));
                        }
                        break;
                    case "destination":
                        tillDes = true;
                        if (!(Quotes.RemoveQuotes(columns[2]).Equals(Constant.StrEmpty)))
                        {
                            if (expCode.Equals("NONE"))
                            {
                                expCode = Quotes.RemoveQuotes(columns[2]).GetExpCode();
                            }
                            else
                            {
                                if (!expCode.Equals(Quotes.RemoveQuotes(columns[2]).GetExpCode()))
                                {
                                    FileProcessor.GiveLog("Destination Experiment Code Error "+ Quotes.RemoveQuotes(columns[2]));
                                    result = Constant.ExpCodeError;
                                }
                            }
                            matchItem.itemQueue.Enqueue(new KeyValuePair<string, string>(Constant.StrDes, Quotes.RemoveQuotes(columns[2])));
                        }
                        else
                        {
                            matchItem.itemQueue.Enqueue(new KeyValuePair<string, string>(Constant.StrDes, Constant.StrEmpty));
                        }
                        break;
                }
            }
            #endregion
            if (expCode.Equals("NONE"))
            {
                FileProcessor.GiveLog("Read Error, All Plate Empty");
                result = Constant.AllEmptyError;
            }

            matchItem.itemResult = result;
            fileData.MatchQueue.Enqueue(matchItem);

            expCode = "NONE";
            tillDes = false;
            result = Constant.MatchSucces;
            matchItem = new MatchItem();
        }

        /* Extraction file title example:
         * 4.000000,"Admin","TAM1","Extraction_Sup_Pick_48to96_Corn_Sweetcorn_Melon","48to96",20.000000,1.000000,"Y"
         *  */
        public void Read_ExtractionFile_Title(string exportTitle, ref MatchData fileData, string type)
        {
            List<string> columns = new List<string>();
            columns = exportTitle.Split(Constant.Export_File_Delimiter).ToList();
            fileData.PlatesNum = Quotes.RemoveQuotes(columns[0]);
            fileData.OperatorName = Quotes.RemoveQuotes(columns[1]);
            fileData.MachineUsed = Quotes.RemoveQuotes(columns[2]);
            fileData.ScriptUsed = Quotes.RemoveQuotes(columns[3]);
            fileData.Volume = Quotes.RemoveQuotes(columns[5]);
            fileData.RoundsNum = Quotes.RemoveQuotes(columns[6]);
            if (type == Constant.Extrac_96w_96w)
            {
                fileData.MixRoundsNum = Quotes.RemoveQuotes(columns[7]);
                fileData.ReturnTips = Quotes.RemoveQuotes(columns[8]);
            }
            else
            {
                fileData.ReturnTips = Quotes.RemoveQuotes(columns[7]);
            }           
        }

        public void Read_Daug(string[] exportLines, ref MatchData fileData)
        {
            List<string> columns = new List<string>();
            Read_DaughterFile_Title(exportLines, ref fileData);
            columns = exportLines[1].Split(Constant.Export_File_Delimiter).ToList();
            fileData.FileMaker = Quotes.RemoveQuotes(columns[2]).ToString();
            string expCode = "NONE";
            bool tillDes = false;
            string result = Constant.MatchSucces;
            MatchItem matchItem = new MatchItem();
            #region for loop
            foreach (string line in exportLines)
            {
                columns = line.Split(Constant.Export_File_Delimiter).ToList();
                switch (Quotes.RemoveQuotes(columns[0]).ToLower())
                {
                    case "source":
                        /* start second round, store previous data */
                        if (tillDes) 
                        {
                            if (expCode.Equals("NONE"))
                            {
                                FileProcessor.GiveLog("Read Error, All Plate Empty");
                                result = Constant.AllEmptyError;
                            }
                            matchItem.itemResult = result;
                            fileData.MatchQueue.Enqueue(matchItem);

                            expCode = "NONE";
                            tillDes = false;
                            result = Constant.MatchSucces;
                            matchItem = new MatchItem();
                        }

                        if (!(Quotes.RemoveQuotes(columns[2]).Equals(Constant.StrEmpty)))
                        {
                            if (expCode.Equals("NONE"))
                            {
                                expCode = Quotes.RemoveQuotes(columns[2]).GetExpCode();
                            }
                            else
                            {
                                if (!expCode.Equals(Quotes.RemoveQuotes(columns[2]).GetExpCode()))
                                {
                                    FileProcessor.GiveLog("Source Experiment Code Error "+ Quotes.RemoveQuotes(columns[2]));
                                    result = Constant.ExpCodeError;
                                }
                            }
                            matchItem.itemQueue.Enqueue(new KeyValuePair<string, string>(Constant.StrSrc, Quotes.RemoveQuotes(columns[2])));
                        }
                        else
                        {
                            matchItem.itemQueue.Enqueue(new KeyValuePair<string, string>(Constant.StrSrc, Constant.StrEmpty));
                        }
                        break;
                    case "destination":
                        tillDes = true;
                        if (!(Quotes.RemoveQuotes(columns[2]).Equals(Constant.StrEmpty)))
                        {
                            if (expCode.Equals("NONE"))
                            {
                                expCode = Quotes.RemoveQuotes(columns[2]).GetExpCode();
                            }
                            else
                            {
                                if (!expCode.Equals(Quotes.RemoveQuotes(columns[2]).GetExpCode()))
                                {
                                    FileProcessor.GiveLog("Destination Experiment Code Error "+ Quotes.RemoveQuotes(columns[2]));
                                    result = Constant.ExpCodeError;
                                }
                            }
                            matchItem.itemQueue.Enqueue(new KeyValuePair<string, string>(Constant.StrDes, Quotes.RemoveQuotes(columns[2])));                   
                        }
                        else
                        {
                            matchItem.itemQueue.Enqueue(new KeyValuePair<string, string>(Constant.StrDes, Constant.StrEmpty));
                        }
                        break;
                }
            }
            #endregion            
            if (expCode.Equals("NONE"))
            {
                FileProcessor.GiveLog("Read Error, All Plate Empty");
                result = Constant.AllEmptyError;
            }

            matchItem.itemResult = result;
            fileData.MatchQueue.Enqueue(matchItem);

            expCode = "NONE";
            tillDes = false;
            result = Constant.MatchSucces;
            matchItem = new MatchItem();
        }

        /* Daughter plate title sample: 
         * 1.000000,"Admin","TAM1","Nexar_DNA96to384_GPIS_2Sets",384.000000,70.000000,3.000000,0.000000,1.000000,"N",30.000000
         */
        public void Read_DaughterFile_Title(string[] exportLines, ref MatchData fileData)
        {
            FileProcessor.GiveLog("Reading Title");
            List<string> columns = new List<string>();
            columns = exportLines[0].Split(Constant.Export_File_Delimiter).ToList();
            fileData.PlateReplicateNum = Quotes.RemoveQuotes(columns[0]);
            fileData.OperatorName = Quotes.RemoveQuotes(columns[1]);
            fileData.MachineUsed = Quotes.RemoveQuotes(columns[2]);
            fileData.ScriptUsed = Quotes.RemoveQuotes(columns[3]);
            fileData.SampleNum = Quotes.RemoveQuotes(columns[4]);
            fileData.TotalVolume = Quotes.RemoveQuotes(columns[5]);
            fileData.DilutionFactor = Quotes.RemoveQuotes(columns[6]);
            fileData.TipWetting = Quotes.RemoveQuotes(columns[7]);
            fileData.SetsNum = Quotes.RemoveQuotes(columns[8]);
            fileData.ControlAddition = Quotes.RemoveQuotes(columns[9]);
            fileData.VolumeOfControls = Quotes.RemoveQuotes(columns[10]);
            fileData.Plex1_Set1 = Quotes.RemoveQuotes(columns[11]);
            fileData.Plex1_Set2 = Quotes.RemoveQuotes(columns[12]);
        }

        public MagentaData Read_Magenta(string expCode,string markerString,string magentaFileDir)
        {
            MagentaData magentaData = null;
            DirectoryInfo mfdInfo = new DirectoryInfo(magentaFileDir);
            List<DirectoryInfo> subDirs = mfdInfo.GetDirectories().ToList();
            subDirs = subDirs.OrderByDescending(p => p.CreationTime).ToList();
            foreach (DirectoryInfo dInfo in subDirs)
            {
                if (expCode.Equals(FileProcessor.GetMagentaFileCode(dInfo.Name)))
                {
                    magentaData = new MagentaData();                 
                    FileInfo[] files = dInfo.GetFiles("*.txt");
                    FileInfo fInfo = files.First();
                    string[] texts = File.ReadAllText(fInfo.FullName).Split('\t');
                    int strPlace = texts.FindFirst(markerString);                   
                    while(strPlace!=-1)
                    {
                        //get source1, destination 1, add 6 and 5 according to format
                        string[] src = (texts[strPlace + 5]).Split(Constant.Magenta_File_Delimiter).ToArray();
                        string[] des = (texts[strPlace + 4]).Split(Constant.Magenta_File_Delimiter).ToArray();
                        foreach(string strSrc in src)
                        {
                            KeyValuePair<string, string> pair = new KeyValuePair<string, string>(Constant.StrSrc.ToLower(), strSrc.Trim());
                            magentaData.Queue.Enqueue(pair);
                        }
                        foreach(string strDes in des)
                        {
                            KeyValuePair<string, string> pair = new KeyValuePair<string, string>(Constant.StrDes.ToLower(), strDes.Trim());
                            magentaData.Queue.Enqueue(pair);
                        }
                        string sampleCode = texts[strPlace + 1].Trim();
                        magentaData.SampleCodeQ.Enqueue(sampleCode);
                        strPlace = texts.FindNext(markerString, strPlace);
                    }
                    break;
                }
            }
            return magentaData;
        }

        public bool Check_Sample_Number_96(string expCode, string markerString, string magentaFileDir)
        {
            bool flag = true;
            DirectoryInfo mfdInfo = new DirectoryInfo(magentaFileDir);
            List<DirectoryInfo> subDirs = mfdInfo.GetDirectories().ToList();
            subDirs = subDirs.OrderByDescending(p => p.CreationTime).ToList();
            foreach (DirectoryInfo dInfo in subDirs)
            {
                if (expCode.Equals(FileProcessor.GetMagentaFileCode(dInfo.Name)))
                {
                    FileInfo[] files = dInfo.GetFiles("*.txt");
                    FileInfo fInfo = files.First();
                    string[] texts = File.ReadAllText(fInfo.FullName).Split('\t');
                    int strPlace = texts.FindFirst(markerString);
                    while (strPlace != -1)
                    {
                        //get source1, destination 1, add 6 and 5 according to format
                        string num = (texts[strPlace + 1]).Trim();
                        if(Convert.ToInt32(num)<=96)
                        {
                            strPlace = texts.FindNext(markerString, strPlace);
                        }else
                        {
                            flag = false;
                            break;
                        }
                    }
                    break;
                }
            }
            return flag;
        }

        public bool Check_Sample_Number_192(string expCode, string markerString, string magentaFileDir)
        {
            bool flag = true;
            DirectoryInfo mfdInfo = new DirectoryInfo(magentaFileDir);
            List<DirectoryInfo> subDirs = mfdInfo.GetDirectories().ToList();
            subDirs = subDirs.OrderByDescending(p => p.CreationTime).ToList();
            foreach (DirectoryInfo dInfo in subDirs)
            {
                if (expCode.Equals(FileProcessor.GetMagentaFileCode(dInfo.Name)))
                {
                    FileInfo[] files = dInfo.GetFiles("*.txt");
                    FileInfo fInfo = files.First();
                    string[] texts = File.ReadAllText(fInfo.FullName).Split('\t');
                    int strPlace = texts.FindFirst(markerString);
                    while (strPlace != -1)
                    {
                        //get source1, destination 1, add 6 and 5 according to format
                        string num = (texts[strPlace + 1]).Trim();
                        if ((Convert.ToInt32(num) <= 192)&&(Convert.ToInt32(num)>96))
                        {
                            strPlace = texts.FindNext(markerString, strPlace);
                        }
                        else
                        {
                            flag = false;
                            break;
                        }
                    }
                    break;
                }
            }
            return flag;
        }

        public bool Check_Sample_Number_384(string expCode, string markerString, string magentaFileDir)
        {
            bool flag = true;
            DirectoryInfo mfdInfo = new DirectoryInfo(magentaFileDir);
            List<DirectoryInfo> subDirs = mfdInfo.GetDirectories().ToList();
            subDirs = subDirs.OrderByDescending(p => p.CreationTime).ToList();
            foreach (DirectoryInfo dInfo in subDirs)
            {
                if (expCode.Equals(FileProcessor.GetMagentaFileCode(dInfo.Name)))
                {
                    FileInfo[] files = dInfo.GetFiles("*.txt");
                    FileInfo fInfo = files.First();
                    string[] texts = File.ReadAllText(fInfo.FullName).Split('\t');
                    int strPlace = texts.FindFirst(markerString);
                    while (strPlace != -1)
                    {
                        //get source1, destination 1, add 6 and 5 according to format
                        string num = (texts[strPlace + 1]).Trim();
                        if ((Convert.ToInt32(num) > 192))
                        {
                            strPlace = texts.FindNext(markerString, strPlace);
                        }
                        else
                        {
                            flag = false;
                            break;
                        }
                    }
                    break;
                }
            }
            return flag;
        }
    }
}
