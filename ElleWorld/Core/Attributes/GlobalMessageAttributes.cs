using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElleWorld.Core.Costants.Net;
using ElleWorld.Core.Costants.Account;

namespace ElleWorld.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class GlobalMessageAttribute : Attribute
    {
        public GlobalClientMessage Message { get; }
        public SessionState State { get; }

        public GlobalMessageAttribute(GlobalClientMessage message, SessionState state)
        {
            Message = message;
            State = state;
        }
    }
}
