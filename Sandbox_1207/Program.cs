using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox_1207
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                System.Console.WriteLine("Please enter a drv argument.");
                FileUtils fu = new FileUtils();
                // fu.getSnippetFromHugeFile(@"D:\한글 2014.iso", 1048576, 1048576*100);
                fu.explore(args[0]);

            }



        }
    }
}
