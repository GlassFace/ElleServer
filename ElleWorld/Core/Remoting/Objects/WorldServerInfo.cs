using System.Runtime.Serialization;
using ElleWorld.Core.Miscellaneous;

namespace ElleWorld.Core.Remoting.Objects
{
    [DataContract]
    public class WorldServerInfo : ServerInfoBase
    {
        [DataMember]
        public int[] Maps { get; set; }

        public bool Compare(WorldServerInfo info)
        {
            return base.Compare(info) && Maps.Compare(info.Maps);
        }
    }
}
