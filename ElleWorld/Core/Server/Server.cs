using System.Net.Sockets;
using System.Threading.Tasks;
using ElleWorld.Core.ServerConn;
using ElleWorld.Core.Network;
using ElleWorld.Core.Remoting;
using ElleWorld.Core.Remoting.Objects;
using ElleWorld.Core.Managers;

namespace ElleWorld.Core
{
    class Server : ServerBase
    {
        public static IPCClient WorldService;
        public static IPCClient NodeService;
        public static WorldServerInfo ServerInfo;

        static Config_Manager conf = new Config_Manager();

        public Server(string ip, int port) : base(ip, port)
        {
            WorldService = new IPCClient(ip, "WorldServer");
        }

        public override async Task DoWork(Socket client)
        {
            var worker = new WorldSession(client);

            worker.Id = ++Manager.Session.LastSessionId;

            if (Manager.Session.Add(worker.Id, worker))
                await Task.Factory.StartNew(Manager.Session.Sessions[worker.Id].Accept);
        }
    }
}
