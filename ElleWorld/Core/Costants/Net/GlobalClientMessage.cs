using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElleWorld.Core.Costants.Net
{
    public enum GlobalClientMessage : ushort
    {
        #region UserRouterClient
        SuspendCommsAck = 0x0CB7,
        AuthSession = 0x0977,
        AuthContinuedSession = 0x0C33,
        Ping = 0x0828,
        LogDisconnect = 0x0C67,
        #endregion

        PlayerLogin = 0x03F9,
    }
}
