using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElleWorld.Core
{
    class ConsoleCommand
    {
        private static void ParseCommand(string[] cmd)
        {
            if(cmd[0].ToLower().StartsWith("."))
            {
                cmd[0] = cmd[0].ToLower().Substring(1);
                switch(cmd[0])
                {
                    case "accountcreate":
                        if(cmd.Length == 5)
                        {
                            ServerMGR.CreateAccount(cmd[1].ToUpper(), cmd[2].ToUpper(), cmd[3].ToUpper(), Convert.ToInt16(cmd[4]));
                            WorldServer.accountList.Add(new Database.Account(cmd[1].ToUpper(), cmd[2].ToUpper()));
                            Log.Message("Account " + cmd[1].ToUpper() + " created.");
                        }
                        else
                        {
                            Log.Error("There are missing parameters");
                            Log.Error("Hint: .accountcreate username password email account_type(1/2).");
                        }
                        break;
                    default:
                        Log.Error("Command " + cmd[0] + " doesn't exist");
                        break;
                }
            }
        }

        public ConsoleCommand() { }

        public void DoWork()
        {
            while (true)
            {
                Thread.Sleep(1);
                var cmdline = Console.ReadLine().Split(new string[] { " " }, StringSplitOptions.None);

                if (cmdline.Length > 0)
                {
                    ParseCommand(cmdline);
                }
            }
        }
    }
}
