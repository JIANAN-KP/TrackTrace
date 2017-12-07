using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT_Scan
{
    public class MatchItem
    {
        public string itemResult { get; set; } = Constant.MatchSucces;
        public Queue<KeyValuePair<string, string>> itemQueue = new Queue<KeyValuePair<string, string>>();
    }
}
