using ElleWorld.Database;
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
                            WorldServer.accountList.Add(new Database.Account(cmd[3].ToUpper(), cmd[2].ToUpper()));
                            Log.Message("Account " + cmd[1].ToUpper() + " created.");
                        }
                        else
                        {
                            Log.Error("There are missing parameters");
                            Log.Error("Hint: .accountcreate username password email account_type(1/2).");
                        }
                        break;
                    case "modifypassword":
                        if (cmd.Length == 4)
                        {
                            var username = cmd[1].ToUpper();
                            var oldpassword = cmd[2].ToUpper();
                            var newpassword = cmd[3].ToUpper();

                            try
                            {
                                Account acc = WorldServer.accountList.First(a => a.GetUsername() == username);

                                var email = acc.GetEmail();

                                var oldpasswordsha = ServerMGR.CreateBNetPassword(email, oldpassword);
                                acc = WorldServer.accountList.First(a => a.GetUsername() == username && a.GetEmail() == email && a.GetPassword() == oldpasswordsha);
                                acc.SetPassword(newpassword);
                                acc.update();
                                Log.Message("Password updated for account: " + acc.GetUsername() + ".");
                            }
                            catch(Exception ex)
                            {
                                Log.Error("Invalid data. Account not found");
                            }

                        }
                        else
                        {
                            Log.Error("There are missing parameters");
                            Log.Error("Hint: .modifypassword username oldpassword newpassword.");
                        }
                        break;
                    default:
                        Log.Error("Command " + cmd[0] + " doesn't exist");
                        break;
                }
            }
        }

        public ConsoleCommand() { }

        public void CheckForCommands()
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
