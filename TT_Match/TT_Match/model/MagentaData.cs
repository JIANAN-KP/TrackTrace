using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT_Match.model
{
    public class MagentaData
    {
        public Queue<KeyValuePair<string, string>> Queue { get; set; } = new Queue<KeyValuePair<string, string>>();
        public Queue<string> SampleCodeQ = new Queue<string>();
    }
}
