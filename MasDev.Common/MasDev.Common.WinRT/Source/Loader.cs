using MasDev.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace MasDev.Common.WinRT
{
    public static class Loader
    {
        static ResourceManager _strings;

        public static void Prepare(Assembly assembly)
        {
            _strings = new ResourceManager("Strings", assembly);
        }

        public static string String(string name)
        {
            var s = Assert.NotNull(_strings).GetString(name);
            return s;
        }
    }
}
