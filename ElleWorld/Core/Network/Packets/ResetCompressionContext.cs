using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElleWorld.Core.Costants.Net;

namespace ElleWorld.Core.Network.Packets.Server.Net
{
    public class ResetCompressionContext : ServerPacket
    {
        public ResetCompressionContext() : base(GlobalServerMessage.ResetCompressionContext) { }

        public override void Write()
        {
        }
    }
}
