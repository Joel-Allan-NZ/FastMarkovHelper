using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    public static class HashSetExtensions
    {
        public static bool TryAdd<T>(this HashSet<T> set, T item)
        {
            if (!set.Contains(item))
            {
                set.Add(item);
                return true;
            }

            else return false;
        }
    }
}
