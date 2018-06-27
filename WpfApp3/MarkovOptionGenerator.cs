using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    public class MarkovOptionGenerator
    {
        public MarkovOptionGenerator(Markov markovData)
        {
            _MarkovData = markovData;
            _rand = new Random();
        }

        public string GetOption(MarkovDatum token)
        {
            long target = _rand.Next(token.TotalCount);
            string NextTokenKey = token.TrailingTokens.Last().Key;

            foreach(var nextToken in token.TrailingTokens)
            {
                target -= nextToken.Value;
                if (target <= 0)
                {
                    NextTokenKey = nextToken.Key;
                    break;
                }
            }
            return NextTokenKey;
        }

        public IEnumerable<string> GetOptions(MarkovDatum token, int optionCount)
        {
            if (optionCount <= 0) throw new ArgumentException("You must require at least one option.", "optionCount");
            if (token.TrailingTokens.Count <= optionCount) //uncommon token, very few trailing tokens
            {
                foreach (var Trailing in token.TrailingTokens.Keys)
                {
                    yield return Trailing;
                }
            }
            else
            {
                HashSet<string> Options = new HashSet<string>();
                do
                {
                    var Option = GetOption(token);
                    if (Options.TryAdd(Option))
                    {
                        yield return Option;
                    }

                } while (Options.Count < optionCount);
            }
        }

        public bool TryGetOptions(string token, int optionCount, out IEnumerable<string> options)
        {
            options = new List<string>();
            MarkovDatum MarkovToken;
            if (_MarkovData.Tokens.TryGetValue(token, out MarkovToken))
                options = GetOptions(MarkovToken, optionCount);

            return options.Count() > 0;
        }



        private Markov _MarkovData;
        private Random _rand;
    }
}
