using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElleWorld.Core.Costants;

namespace ElleWorld.Core.Objects
{
    public class SmartGuid
    {
        public ulong Low { get; set; }
        public ulong High { get; set; }

        public virtual GuidType Type
        {
            get { return (GuidType)(High >> 58); }
            set { High |= (ulong)value << 58; }
        }
    }
}
