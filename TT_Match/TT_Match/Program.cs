using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT_Match.logic;
using TT_Match.tools;
using System.IO;
using Microsoft.Win32;

namespace TT_Match
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                FileProcessor.GiveLog("===============");
                FileProcessor.GiveLog("Start Matching.");
                #region [1]. get file foder path and read all texts
                /* HKEYC_CURRENT_USER//software//tt_panel */
                /* value set by tt_panel */
                RegistryKey key = Registry.CurrentUser.OpenSubKey("software\\tt_panel", true);                
                string exportFileDir = key.GetValue("efDir").ToString();
                string magentaFileDir = key.GetValue("mfDir").ToString();
                string resultFileDir = key.GetValue("rfDir").ToString();
                string outputFileDir = key.GetValue("ofDir").ToString();
                if (File.Exists(outputFileDir + "\\" + "MatchResult.txt"))
                {
                    FileProcessor.GiveLog("Deleting Exist Output File");
                    File.Delete(outputFileDir + "\\" + "MatchResult.txt");
                }
                FileProcessor.GiveLog(DateTime.Now.ToString());
                Process process = new Process(magentaFileDir, resultFileDir, outputFileDir);
                string expPath = exportFileDir;
                DirectoryInfo dInfo = new DirectoryInfo(expPath);
                /* only one file under export folder every time */
                FileInfo file = dInfo.GetFiles("*.txt").First();            
                string[] lines = File.ReadAllLines(file.FullName);
                /* the fifth parameter in the first line is the scriptCode, will be used to classify match type  */
                string scriptCode = Quotes.RemoveQuotes((lines[0].Split(','))[4]);
                FileProcessor.GiveLog("Processing");
                string lastLine = lines.Last();
                #endregion
                /* if the script is completed, word "complete" will be add on last line
                 * run this app again will change result file completeStatus attribute to "Yes" 
                 */

                #region [2]. new matching or change status
                if (Quotes.RemoveQuotes(lastLine.ToLower()).Equals("complete"))               
                {
                    string expCode = Quotes.RemoveQuotes((lines[1].Split(Constant.Export_File_Delimiter))[2].ToString());
                    DirectoryInfo dirInfo = new DirectoryInfo(resultFileDir);
                    List<FileInfo> fileInfos = dirInfo.GetFiles().ToList();
                    FileProcessor.GiveLog("FileMaker = " + expCode);
                    fileInfos = fileInfos.OrderByDescending(p => p.CreationTime).ToList();                
                    foreach(FileInfo f in fileInfos)
                    {
                        if(f.Name.Contains(expCode))
                        {
                            string text = File.ReadAllText(f.FullName);
                            text = text.Replace("No", "Yes");
                            File.WriteAllText(f.FullName, text);
                            string desName = f.FullName.Replace("InComplete", "Complete");
                            File.Move(f.FullName, desName);                      //If 30 minutes later the machine operate successfully, change file name
                            break;
                        }
                    }                 
                }
                else
                {
                    switch (scriptCode)
                    {
                        case "96to96":
                            FileProcessor.GiveLog("Script: 96to96");
                            process.ProcessFile(lines, Constant.Extraction96_MarkerString, "96to96");
                            break;
                        case "48to96":
                            FileProcessor.GiveLog("Script: 48to96");
                            process.ProcessFile(lines, Constant.Extraction48_MarkerString, "48to96");
                            break;
                        case "384.000000":
                            FileProcessor.GiveLog("Script: Daug 384");
                            process.ProcessFile(lines, Constant.DaughterPlate1_MarkerString, "384");
                            break;
                        case "192.000000":
                            FileProcessor.GiveLog("Script: Daug 192");
                            process.ProcessFile(lines, Constant.DaughterPlate2_MarkerString, "192");
                            break;
                        case "96.000000":
                            FileProcessor.GiveLog("Script: Daug 96");
                            process.ProcessFile(lines, Constant.DaughterPlate4_MarkerString, "96");
                            break;
                        default:
                            FileProcessor.GiveLog("Script: Others");
                            process.ProcessFile(lines, Constant.DaughterPlate1_MarkerString,"000");
                            break;
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                FileProcessor.GiveLog("Exception:  "+e.ToString());
            }
        }
    }
}
