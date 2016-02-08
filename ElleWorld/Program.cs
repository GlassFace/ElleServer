using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ElleWorld.Database;
using ElleWorld.Core;
using Lappa_ORM;

namespace ElleWorld
{
    class Program
    {
        static Config_Manager conf = new Config_Manager();
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

            string authConnectionString = DBConnection.CreateConnectionString(conf.getValue("mysql_host"), conf.getValue("mysql_user"), conf.getValue("mysql_password"), conf.getValue("mysql_auth_db"), Convert.ToInt16(conf.getValue("mysql_port")), Convert.ToInt16(conf.getValue("mysql_min_pool_size")), Convert.ToInt16(conf.getValue("mysql_max_pool_size")), (DatabaseType)Convert.ToInt16(conf.getValue("database_type")));
            
            if (DBConnection.Auth.Initialize(authConnectionString, (DatabaseType)Convert.ToInt16(conf.getValue("database_type"))))
            {
                Console.WriteLine("Auth Database Successfully initialized");
            }
            else
            {
                Console.WriteLine("Error: Check your configuration file.");
            }

            //Load accounts.

            while (true)
            {
                Thread.Sleep(1);
                var cmdline = Console.ReadLine().Split(new string[] { " " }, StringSplitOptions.None);

                if (cmdline.Length > 0)
                {
                    //Handle console commands with a manager
                }
            }
        }
    }
}
