using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElleWorld.Core.Network.Packets.Server.Authentication
{
    public class TransferInitiate : ServerPacket
    {
        public override void Write()
        {
            var serverToClient = "WORLD OF WARCRAFT CONNECTION - SERVER TO CLIENT";

            Packet.Write((ushort)(serverToClient.Length + 1));
            Packet.WriteString(serverToClient, true);
        }
    }
}

namespace ElleWorld.Core.Network.Packets.Client.Authentication
{
    public class TransferInitiate : ClientPacket
    {
        public string Msg { get; private set; }

        public override void Read()
        {
            Msg = Packet.ReadString(Packet.Header.Size);
        }
    }
}
