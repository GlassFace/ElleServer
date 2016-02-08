using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using ElleWorld;
using ElleWorld.Database;

namespace ElleWorld.Core
{
    class ServerMGR
    {
        public ServerMGR() { }

        public void CreateAccount(string username, string clear_password, string email, int type = 1)
        {
            //Check if Account exists
            Account a = new Account(username, clear_password);
            if(a.GetID() == 0) //account doesn't exist
            {

            }
        }

        public string DoShaHashPassword(string _username, string _password)
        {
            byte[] passwordbyte = Encoding.ASCII.GetBytes(_username + ":" + _password);
            var sha_pass = SHA1.Create();
            byte[] bytehash = sha_pass.ComputeHash(passwordbyte);
            _password = HexStringFromBytes(bytehash);

            return _password;
        }

        private static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        public List<Account> LoadAccounts(/*DBConnection authDB*/)
        {
            List<Account> accountList = new List<Account>();
            
            return accountList;
        }
    }
}
