using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    public class MarkovDatum
    {
        public string Name { get; private set; }
        public Dictionary<string, long> TrailingTokens;
        public int TotalCount;

        public MarkovDatum(string name)
        {
            TotalCount = 0;
            Name = name.ToLower();
            TrailingTokens = new Dictionary<string, long>();
        }

        public void RecordTrailing(MarkovDatum trailing)
        {
            TrailingTokens[trailing.Name] = (TrailingTokens.ContainsKey(trailing.Name) ? TrailingTokens[trailing.Name] + 1 : 1);
            TotalCount++;
        }
    }
}
