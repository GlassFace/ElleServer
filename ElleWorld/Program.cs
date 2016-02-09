using System;
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

namespace ElleWorld
{
    class WorldServer
    {
        static Config_Manager conf = new Config_Manager();
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

            Server s = new Server();
            Thread serverThread = new Thread(s.DoWork);
            serverThread.Start();

            

            while (true)
            {
                Thread.Sleep(1);
                var cmdline = Console.ReadLine().Split(new string[] { " " }, StringSplitOptions.None);

                if (cmdline.Length > 0)
                {
                    //Handle console commands with a manager
                    ConsoleCommand.ParseCommand(cmdline);
                }
            }
        }
    }
}
