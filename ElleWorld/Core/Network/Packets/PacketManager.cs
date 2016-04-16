using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ElleWorld.Core.Network;
using ElleWorld.Core.Costants.Account;
using ElleWorld.Core.Costants.Net;
using ElleWorld.Core.Miscellaneous;
using ElleWorld.Core.Attributes;

namespace ElleWorld.Core.Network.Packets
{
    class PacketManager
    {
        static ConcurrentDictionary<ushort, Tuple<MethodInfo, Type, SessionState>> MessageHandlers = new ConcurrentDictionary<ushort, Tuple<MethodInfo, Type, SessionState>>();

        public static void DefineMessageHandler()
        {
            var currentAsm = Assembly.GetExecutingAssembly();
            var globalAsm = typeof(GlobalMessageAttribute).Assembly;

            foreach (var type in currentAsm.GetTypes().Concat(globalAsm.GetTypes()))
            {
                foreach (var methodInfo in type.GetMethods())
                {
                    foreach (dynamic msgAttr in methodInfo.GetCustomAttributes())
                    {
                        if (msgAttr is GlobalMessageAttribute || msgAttr is MessageAttribute)
                            MessageHandlers.TryAdd((ushort)msgAttr.Message, Tuple.Create(methodInfo, methodInfo.GetParameters()[0].ParameterType, msgAttr.State));
                    }
                }
            }
        }

        public static async Task InvokeHandler<T>(Packet reader, ElleWorld.Core.Network.WorldSession session)
        {
            var message = reader.Header.Message;

            Tuple<MethodInfo, Type, SessionState> data;

            if (MessageHandlers.TryGetValue(message, out data))
            {
                if ((session.State & data.Item3) == SessionState.None)
                {
                    var clientInfo = session.GetClientInfo();

                    Log.Error($"Client '{clientInfo}': Received not allowed packet for state '{session.State}'.");
                    Log.Error($"Disconnecting '{clientInfo}'.");

                    session.Dispose();

                    return;
                }

                var handlerObj = Activator.CreateInstance(data.Item2) as ClientPacket;

                handlerObj.Packet = reader;

                await Task.Run(() => handlerObj.Read());

                if (handlerObj.IsReadComplete)
                    data.Item1.Invoke(null, new object[] { handlerObj, session });
                else
                    Log.Error($"Packet read for '{data.Item2.Name}' failed.");
            }
            else
            {
                var msgName = Enum.GetName(typeof(ClientMessage), message) ?? Enum.GetName(typeof(GlobalClientMessage), message);

                if (msgName == null)
                    Log.Error($"Received unknown opcode '0x{message:X}, Length: {reader.Data.Length}'.");
                else
                    Log.Message($"Packet handler for '{msgName} (0x{message:X}), Length: {reader.Data.Length}' not implemented.");
            }
        }
    }
}
