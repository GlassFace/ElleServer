using System;
using System.Collections.Concurrent;
using System.ServiceModel;
using ElleWorld.Core.Remoting.Services;
using ElleWorld.Core.Remoting.Objects;

namespace ElleWorld.Core.Remoting
{
    public class IPCClient : IService, IServiceCallback
    {
        public uint LastSessionId { get; set; }
        public ConcurrentDictionary<uint, ServerInfoBase> Servers { get; set; }
        public ConcurrentDictionary<ulong, Tuple<uint, ulong>> Redirects { get; set; }

        IService proxy;
        IClientChannel proxyChannel;

        public IPCClient(string serverIP, string hostName)
        {
            var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None)
            {
                // Set 'infinite' timeouts.
                SendTimeout = TimeSpan.MaxValue,
                ReceiveTimeout = TimeSpan.MaxValue
            };

            var factory = new DuplexChannelFactory<IService>(new InstanceContext(this), binding, new EndpointAddress($"net.pipe://{serverIP}/{hostName}"));

            proxy = factory.CreateChannel();
            proxyChannel = proxy as IClientChannel;

            proxyChannel.Opened += (o, e) =>
            {
                Servers = proxy.Servers;
                Redirects = proxy.Redirects;
            };

            proxyChannel.Open();
        }

        public void Register(ServerInfoBase info)
        {
            proxy.Register(info);
        }

        public void Update(ServerInfoBase info)
        {
            proxy.Update(info);
        }

        public void Update(ulong key, Tuple<uint, ulong> data)
        {
            proxy.Update(key, data);
        }

        public void Unregister(uint sessionId)
        {
            proxy.Unregister(sessionId);
        }

        public void NotifyClients(uint sessionId, ServerInfoBase info)
        {
            var serverName = "";

            if (info is WorldServerInfo)
                serverName = "WorldServer";
            else if (info is WorldNodeInfo)
                serverName = "NodeServer";

            if (info == null)
            {
                if (Servers.TryRemove(sessionId, out info))
                    Log.Message($"{serverName} (Realm: {info.RealmId}, Host: {info.IPAddress}, Port: {info.Port}) disconnected.");
            }
            else
            {
                var status = "connected";

                if (Servers.ContainsKey(sessionId))
                    status = "updated";

                Servers.AddOrUpdate(sessionId, info, (k, v) => info);

                Log.Message($"{serverName} (Host: {info.IPAddress}, Port: {info.Port}, Connections: {info.ActiveConnections}) {status}.");
            }
        }

        public void NotifyClients(ulong key, Tuple<uint, ulong> data)
        {
            Tuple<uint, ulong> dummy;

            if (data == null)
                Redirects.TryRemove(key, out dummy);
            else
                Redirects.TryAdd(key, data);
        }
    }
}
