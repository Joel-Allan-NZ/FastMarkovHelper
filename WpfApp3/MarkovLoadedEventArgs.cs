using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    public class MarkovLoadedEventArgs : EventArgs
    {
        public MarkovOptionGenerator Markov { get; }
        public MarkovLoadedEventArgs(MarkovOptionGenerator markov)
        {
            Markov = markov;
        }
    }
}
