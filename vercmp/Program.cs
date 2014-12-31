using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CKANUtils;

namespace vercmp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("vercmp <version A> <version B>");
                return;
            }

            Console.WriteLine(VersionCompare.CKANPackageVersionCompare(args[0], args[1]));
        }
    }
}
