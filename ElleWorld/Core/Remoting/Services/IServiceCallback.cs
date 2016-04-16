using System;
using System.Collections.Concurrent;
using System.ServiceModel;
using ElleWorld.Core.Remoting.Objects;

namespace ElleWorld.Core.Remoting.Services
{
    public interface IServiceCallback
    {
        ConcurrentDictionary<uint, ServerInfoBase> Servers
        {
            [OperationContract]
            get;
            [OperationContract]
            set;
        }

        ConcurrentDictionary<ulong, Tuple<uint, ulong>> Redirects
        {
            [OperationContract]
            get;
            [OperationContract]
            set;
        }

        [OperationContract(IsOneWay = true)]
        void Register(ServerInfoBase info);

        [OperationContract(IsOneWay = true)]
        void Update(ServerInfoBase info);

        [OperationContract(IsOneWay = true)]
        void Unregister(uint sessionId);

        [OperationContract(IsOneWay = true)]
        void NotifyClients(uint sessionId, ServerInfoBase info);

        [OperationContract(Name = "NotifyClientsRedirects", IsOneWay = true)]
        void NotifyClients(ulong key, Tuple<uint, ulong> data);
    }
}
