using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElleWorld.Core.Costants.Account
{
    [Flags]
    public enum SessionState
    {
        None = 0,
        Initiated = 1,
        Authenticated = 2,
        Redirected = 4,
        InWorld = 8,
        All = Initiated | Authenticated | Redirected | InWorld,
    }
}
