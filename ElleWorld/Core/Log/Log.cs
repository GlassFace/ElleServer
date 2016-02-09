using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElleWorld.Core
{
    class Log
    {
        public static void Error(string txt)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(txt);
            Console.ResetColor();
        }

        public static void Message(string txt)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(txt);
            Console.ResetColor();
        }
    }
}
