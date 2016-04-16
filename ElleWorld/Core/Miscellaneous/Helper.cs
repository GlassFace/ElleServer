using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ElleWorld.Core.Miscellaneous
{
    public class Helper
    {
        public static uint Adler32(byte[] data)
        {
            var a = 0xD8F1u;
            var b = 0x9827u;

            for (var i = 0; i < data.Length; i++)
            {
                a = (a + data[i]) % 0xFFF1;
                b = (b + a) % 0xFFF1;
            }
            return (b << 16) + a;
        }
    }
}
