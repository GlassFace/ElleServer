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

        public static void CreateAccount(string username, string clear_password, string email, int type = 1)
        {
            //Check if Account exists
            Account a = new Account(username, clear_password);
            if(a.GetID() == 0) //account doesn't exist
            {
                string password = DoShaHashPassword(username, clear_password).ToUpper();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO account(username, password, email, type) VALUES(@username, @password, @email, @type);", DBConnection.Auth);
                MySqlParameter usernameParameter = new MySqlParameter("@username", MySqlDbType.VarChar, 0);
                MySqlParameter passwordParameter = new MySqlParameter("@password", MySqlDbType.VarChar, 0);
                MySqlParameter emailParameter = new MySqlParameter("@email", MySqlDbType.VarChar, 0);
                MySqlParameter typeParameter = new MySqlParameter("@type", MySqlDbType.Int16, 0);
                usernameParameter.Value = username;
                passwordParameter.Value = password;
                emailParameter.Value = email;
                typeParameter.Value = type;
                cmd.Parameters.Add(usernameParameter);
                cmd.Parameters.Add(passwordParameter);
                cmd.Parameters.Add(emailParameter);
                cmd.Parameters.Add(typeParameter);
                cmd.ExecuteNonQuery();
            }
        }

        public static string DoShaHashPassword(string _username, string _password)
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

        public static List<Account> LoadAccounts()
        {
            List<Account> accountList = new List<Account>();
            MySqlCommand cmd = new MySqlCommand("", DBConnection.Auth);
            cmd.CommandText = "SELECT ID FROM account;";
            MySqlDataReader row = cmd.ExecuteReader();
            while(row.Read())
                accountList.Add(new Account(Convert.ToInt16(row["ID"])));
            row.Close();
            return accountList;
        }
    }
}
