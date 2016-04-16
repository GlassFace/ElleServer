using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ElleWorld.Core.ServerConn
{
    public class ServerBase : IDisposable
    {
        TcpListener listener;
        bool isRunning;

        public ServerBase(string ip, int port)
        {
            var bindIP = IPAddress.None;

            if (!IPAddress.TryParse(ip, out bindIP))
            {
                Log.Error($"Server can't be started: Invalid IP-Address ({ip})");
                Console.ReadKey(true);

                Environment.Exit(0);
            }

            try
            {
                listener = new TcpListener(bindIP, port);
                listener.Start();

                if (isRunning = listener.Server.IsBound)
                    new Thread(AcceptConnection).Start(5);
            }
            catch (Exception ex)
            {
                //ExceptionLog.Write(ex);

                Log.Error(ex.Message);
            }
        }

        async void AcceptConnection(object delay)
        {
            while (isRunning)
            {
                await Task.Delay((int)delay);

                if (listener.Pending())
                {
                    var clientSocket = listener.AcceptSocket();

                    if (clientSocket != null)
                        await DoWork(clientSocket);
                }
            }
        }

        public virtual Task DoWork(Socket client)
        {
            return null;
        }

        public void Dispose()
        {
            listener = null;
            isRunning = false;
        }
    }
}
