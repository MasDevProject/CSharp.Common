using MasDev.Utils;
using System;
using System.Linq;

namespace MasDev.Common.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = null;//"1,2,3,4,5,6,7,8,9";
            var splitted = s.ReadCsv().ToList();

            foreach(var value in splitted)
                Console.Write("[{0}] ", value);
            Console.WriteLine();
            var sAgain = splitted.AsCsv();
            Console.WriteLine(sAgain);

            Console.ReadLine();
        }
    }
}
