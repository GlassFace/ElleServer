using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using ElleWorld;
using ElleWorld.Database;
using NetMQ;
using NetMQ.Sockets;

namespace ElleWorld.Core
{
    class Server
    {
        static Config_Manager conf = new Config_Manager();

        public Server() { }

        public void DoWork()
        {
            RouterSocket server = new RouterSocket("@tcp://" + conf.getValue("serverhost") + ":" + conf.getValue("serverport"));
            while (true)
            {
                Log.Message("Server is alive...");
                Thread.Sleep(10000);
            }
        }
    }
}
