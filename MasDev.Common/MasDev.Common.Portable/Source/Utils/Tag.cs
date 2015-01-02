using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasDev.Utils
{
    public class Holder<T>
    {
        public T Item { get; private set; }
        public dynamic Tag { get; private set; }

        public Holder(T item, dynamic tag)
        {
            Item = item;
            Tag = tag;
        }
    }
 
    public static class Holder
    {
        public static Holder<T> Create<T>(T item, dynamic tag)
        {
            return new Holder<T>(item, tag);
        }
    }
}
