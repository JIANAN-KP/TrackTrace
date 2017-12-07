using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT_Match.tools
{
    public class Constant
    {
        //script name
        public static string Extrac_96w_96w = "96to96";
        public static string Extrac_48w_96w = "48to96";
        public static string Daug_96w_384w_plex1 = "384";
        public static string Daug_96w_384w_plex2 = "192";
        public static string Daug_96w_384w_plex3 = "96";
        public static char Export_File_Delimiter = ',';
        public static char Magenta_File_Delimiter = ',';

        public static List<DirectoryInfo> magentaFolderList = new List<DirectoryInfo>();

        public static string Extraction96_MarkerString = "EXTRACT N";
        public static string Extraction48_MarkerString = "48 TO 96 WELL";
        public static string DaughterPlate1_MarkerString = "DNA DAUGH 384 N";
        public static string DaughterPlate2_MarkerString = "DNA DAUGH 384 N";
        public static string DaughterPlate4_MarkerString = "DNA DAUGH 384 N";

        public static string Extrac_96w_96w_ProcessName = "DNA Extraction 96 to 96";
        public static string Extrac_48w_96w_ProcessName = "DNA Extraction 48 to 96";
        public static string Daug_Mplx1_ProcessName = "384 Daughter Plate Preparation Mplx 1";
        public static string Daug_Mplx2_ProcessName = "384 Daughter Plate Preparation Mplx 2";
        public static string Daug_Mplx4_ProcessName = "384 Daughter Plate Preparation Mplx 4";

        public static string MatchSucces = "Success";
        public static string MatchFail = "Fail";
        public static string MatchComplete = "Complete";
        public static string MatchIncomplete = "InComplete";

        public static string StrSrc = "source";
        public static string StrDes = "destination";
        public static string StrEmpty = "EMPTY";

        public static string LogFileDir = "C:\\ProgramData\\TT_log\\matchlog.txt";

        public static string DesPlateNumError = "Fail:Wrong Destination Number Code";
        public static string PlateNumMatchError = "Fail:Plate Number Not Match";
        public static string MatchingFail = "Fail:Match Failed";
        public static string MagentaNotFound = "Fail:No Magenta file found";
        public static string VersionNumDupError = "Fail:Version Number Duplicate";
        public static string ExpCodeError = "Fail:Experiment Code Error";
        public static string EmptyPositionError = "Fail:Empty Position Error";
        public static string AllEmptyError = "Fail:All Plates Empty";
    }
}
