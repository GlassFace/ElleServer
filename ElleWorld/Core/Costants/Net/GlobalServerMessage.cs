using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElleWorld.Core.Costants.Net
{
    public enum GlobalServerMessage : ushort
    {
        //0xBADD = Not found
        AuthChallenge = 0x1EC,
        ConnectTo = 0xBADD,
        SuspendComms = 0x50F,
        ResumeComms = 0xBADD,
        Compression = 0xBADD,
        ResetCompressionContext = 0xBADD,
        Composite = 0xBADD,
        Pong = 0x1DD,
        CharacterLoginFailed = 0x041
    }
}
