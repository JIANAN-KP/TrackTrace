using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TT_Match.tools;

namespace TT_Match.model
{
    public class MatchItem
    {
        public string itemResult { get; set; } = Constant.MatchSucces;
        public Queue<KeyValuePair<string, string>> itemQueue = new Queue<KeyValuePair<string, string>>();
    }
}
