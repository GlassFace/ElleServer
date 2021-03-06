﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using NetMQ;
using NetMQ.Sockets;
using ElleWorld.Database;
using ElleWorld.Core;
using System.Security.Cryptography;
using ElleWorld.Core.Network.Packets;
using ElleWorld.Core.Managers;
using ElleWorld.Core.Miscellaneous;
using ElleWorld.Core.Remoting.Objects;

namespace ElleWorld
{
    class WorldServer
    {
        static Config_Manager conf = new Config_Manager();
        public static ServerInfo Info { get; private set; }

        public static List<Account> accountList = new List<Account>();
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("                                                            ");
            Console.WriteLine("                                  ;@                        ");
            Console.WriteLine("    @@@@@@  @  +@         @       @;               @     `  ");
            Console.WriteLine("    @@      @  @`        ;@       @                @     @  ");
            Console.WriteLine("   :@      `@  @         @,   @   @               `@     @  ");
            Console.WriteLine("   @;      @;  @         @   @;  `@    @@:  :` @  @;   ;@@  ");
            Console.WriteLine("   @ :@@;  @  ++  +@@@  @@   @   @,  ,@@,@  @@@@@ @   @@@@  ");
            Console.WriteLine("   @@@     @  @` ;@ :@  @   @@   @   @   @  @@    @  @;  @  ");
            Console.WriteLine("  ++      ,@  @ `@@@@   @  `+@  +:  @`  `@  @    ,@ ;@  :@  ");
            Console.WriteLine("  @`   @  @; `@  @     ,@  @ @  @   @   @`  @    @; @   @@  ");
            Console.WriteLine("  @ `@@   @` ;@  @@@@@ `@ @  +@@    @@@@;  :+    @` @@@@@@  ");
            Console.WriteLine("  @@`     @  `,   @@    @@           @@    ,,    @   @;     ");
            Console.WriteLine("                                                            ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;


            Info.Realm = 1;
            Info.Port = (ushort)Convert.ToUInt32(conf.getValue("serverport"));
            Info.Address = conf.getValue("serverhost");
            Info.Maps[0] = -1;


            string authConnectionString = DBConnection.CreateConnectionString(conf.getValue("mysql_host"), conf.getValue("mysql_user"), conf.getValue("mysql_password"), conf.getValue("mysql_auth_db"), Convert.ToInt16(conf.getValue("mysql_port")), Convert.ToInt16(conf.getValue("mysql_min_pool_size")), Convert.ToInt16(conf.getValue("mysql_max_pool_size")));
            DBConnection.Auth.ConnectionString = authConnectionString;

            try
            {
                DBConnection.Auth.Open();
            }
            catch(MySql.Data.MySqlClient.MySqlException ex)
            {
                Log.Error(ex.ToString());
            }
            
            if (DBConnection.Auth.State == System.Data.ConnectionState.Open)
            {
                Log.Message("Auth Database Successfully initialized");
            }
            else
            {
                Log.Error("Error: Check your configuration file.");
            }

            //Load accounts.
            accountList = ServerMGR.LoadAccounts();
            Log.Message("Loaded " + accountList.Count + " accounts.");

            //Start Console Command Manager.
            ConsoleCommand consoleCommand = new ConsoleCommand();
            Thread consoleCommandThread = new Thread(consoleCommand.CheckForCommands);
            consoleCommandThread.Start();

            //Start server.
            using (var server = new Server(conf.getValue("serverhost"), Convert.ToInt32(conf.getValue("serverport"))))
            {
                Server.NodeService.Register(null);

                Server.ServerInfo = new Core.Remoting.Objects.WorldServerInfo
                {
                    RealmId = Info.Realm,
                    Maps = Info.Maps,
                    IPAddress = Info.Address,
                    Port = Info.Port,
                };

                Server.WorldService.Register(Server.ServerInfo);

                PacketManager.DefineMessageHandler();

                Manager.Initialize();
                while (true)
                {
                    //Log.Message("Server is alive...");
                    Thread.Sleep(1);
                }
            }
            
        }
    }
}
