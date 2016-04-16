using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElleWorld.Core.Costants.Net;

namespace ElleWorld.Core.Network.Packets
{
    public class Compression : ServerPacket
    {
        public int UncompressedSize { get; set; }
        public uint UncompressedAdler { get; set; }
        public uint CompressedAdler { get; set; }
        public byte[] CompressedData { get; set; }

        public Compression() : base(GlobalServerMessage.Compression) { }

        public override void Write()
        {
            Packet.Write(UncompressedSize);
            Packet.Write(UncompressedAdler);
            Packet.Write(CompressedAdler);
            Packet.Write(CompressedData);
        }
    }
}
