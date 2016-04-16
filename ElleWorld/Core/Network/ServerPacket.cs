using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElleWorld.Core.Network
{
    public abstract class ServerPacket
    {
        public Packet Packet { get; private set; }

        protected ServerPacket()
        {
            Packet = new Packet();
        }

        protected ServerPacket(object netMessage, bool authHeader = false)
        {
            Packet = new Packet(netMessage, authHeader);
        }

        public abstract void Write();
    }
}
