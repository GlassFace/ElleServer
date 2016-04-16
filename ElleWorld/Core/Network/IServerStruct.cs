using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElleWorld.Core.Network
{
    public interface IServerStruct
    {
        void Write(Packet packet);
    }
}
