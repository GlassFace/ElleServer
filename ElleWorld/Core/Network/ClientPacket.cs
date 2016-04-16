using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElleWorld.Core.Network
{
    public abstract class ClientPacket
    {
        public Packet Packet { protected get; set; }
        public bool IsReadComplete { get { return Packet.IsReadComplete; } }

        public abstract void Read();
    }
}
