using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ElleWorld.Core.Network
{
    public static class PacketReaderExtensions
    {
        static Dictionary<Type, Func<BinaryReader, object>> ReadValue = new Dictionary<Type, Func<BinaryReader, object>>()
        {
            { typeof(bool),      br => br.ReadBoolean() },
            { typeof(sbyte),     br => br.ReadSByte()   },
            { typeof(byte),      br => br.ReadByte()    },
            { typeof(char),      br => br.ReadChar()    },
            { typeof(short),     br => br.ReadInt16()   },
            { typeof(ushort),    br => br.ReadUInt16()  },
            { typeof(int),       br => br.ReadInt32()   },
            { typeof(uint),      br => br.ReadUInt32()  },
            { typeof(float),     br => br.ReadSingle()  },
            { typeof(long),      br => br.ReadInt64()   },
            { typeof(ulong),     br => br.ReadUInt64()  },
            { typeof(double),    br => br.ReadDouble()  },
        };

        public static T Read<T>(this BinaryReader br)
        {
            var type = typeof(T);
            var finalType = type.IsEnum ? type.GetEnumUnderlyingType() : type;

            return (T)ReadValue[finalType](br);
        }
    }
}
