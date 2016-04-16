using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElleWorld.Core.Costants.Account;
using ElleWorld.Core.Costants.Net;

namespace ElleWorld.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class MessageAttribute : Attribute
    {
        public ClientMessage Message { get; }
        public SessionState State { get; }

        public MessageAttribute(ClientMessage message, SessionState state)
        {
            Message = message;
            State = state;
        }
    }
}
