using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT_Match.tools;

namespace TT_Match.model
{
    public class MatchData
    {
        #region common parameters
        public string ProcessName { get; set; } = "";
        public string OperatorName { get; set; } = "";
        public string MachineUsed { get; set; } = "";
        public string ScriptUsed { get; set; } = "";        
        public Queue<MatchItem> MatchQueue = new Queue<MatchItem>(); 
        public string FileMaker { get; set; } = "";
        public string CompleteStatus { get; set; } = "No";
        #endregion
        #region extraction parameters
        public string PlatesNum { get; set; } = "";
        public string Volume { get; set; } = "";
        public string RoundsNum { get; set; } = "";
        /* will be blank in the 48to96 */
        public string MixRoundsNum { get; set; } = "";
        public string ReturnTips { get; set; } = "";
        #endregion
        #region daughter parameters
        public string PlateReplicateNum { get; set; } = "";
        public string SampleNum { get; set; } = "";
        public string TotalVolume { get; set; } = "";
        public string DilutionFactor { get; set; } = "";
        public string TipWetting { get; set; } = "";
        public string SetsNum { get; set; } = "";
        public string ControlAddition { get; set; } = "";
        public string VolumeOfControls { get; set; } = "";
        public string Plex1_Set1 { get; set; } = "";
        public string Plex1_Set2 { get; set; } = "";
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        #endregion
    }
}
