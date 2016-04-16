using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElleWorld.Core.Costants.Net;
using ElleWorld.Core.Network;

namespace ElleWorld.Core.Network.Packets.Server.Authentication
{
    public class AuthChallenge : ServerPacket
    {
        public uint Challenge { get; set; }
        public byte[] DosChallenge { get; set; } = new byte[32];
        public byte DosZeroBits { get; set; } = 1;

        public AuthChallenge() : base(GlobalServerMessage.AuthChallenge, true) { }

        public override void Write()
        {
            Packet.Write(Challenge);
            Packet.WriteBytes(DosChallenge);
            Packet.Write(DosZeroBits);
        }
    }
}
