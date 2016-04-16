using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElleWorld.Core.Miscellaneous
{
    public class ServerInfo
    {
        public uint Realm { get; set; }
        public int[] Maps { get; set; }
        public string Address { get; set; }
        public ushort Port { get; set; }
    }
}
