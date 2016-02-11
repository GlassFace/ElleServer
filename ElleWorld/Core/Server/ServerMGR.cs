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
            if (a.GetID() == 0) //account doesn't exist
            {
                string password = CreateBNetPassword(email, clear_password).ToUpper();
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

        public static List<Account> LoadAccounts()
        {
            List<Account> accountList = new List<Account>();
            MySqlCommand cmd = new MySqlCommand("", DBConnection.Auth);
            cmd.CommandText = "SELECT ID FROM account;";
            MySqlDataReader row = cmd.ExecuteReader();
            while (row.Read())
                accountList.Add(new Account(Convert.ToInt16(row["ID"])));
            row.Close();
            return accountList;
        }


        /**************************\
        *                          *
        *  Password Hashing System *
        *                          *
        \**************************/
        public static string DoShaHashPassword(string _username, string _password)
        {
            byte[] passwordbyte = Encoding.ASCII.GetBytes(_username + ":" + _password);
            var sha_pass = SHA1.Create();
            byte[] bytehash = sha_pass.ComputeHash(passwordbyte);
            _password = HexStringFromBytes(bytehash);

            return _password;
        }

        public static string CreateBNetPassword(string _email, string _password)
        {
            byte[] emailbyte = Encoding.ASCII.GetBytes(_email.ToUpper());
            var sha_email = SHA256.Create();
            byte[] bytehashemail = sha_email.ComputeHash(emailbyte);
            _email = HexStringFromBytes(bytehashemail);

            //now with password
            byte[] passbyte = Encoding.ASCII.GetBytes(_email.ToUpper() + ":" + _password.ToUpper());
            var sha_pass = SHA256.Create();
            byte[] bytehashpass = sha_pass.ComputeHash(passbyte);
            _password = HexStringFromBytes(bytehashpass).ToUpper();

            //hex2bin
            var bindata = hex2bin(_password);
            //strrev
            char[] chararray = bindata.ToCharArray();
            Array.Reverse(chararray);
            var reversedstring = new string(chararray);

            //bin2hex
            byte[] bytes = Encoding.GetEncoding(1252).GetBytes(reversedstring);
            string hexString = HexStringFromBytes(bytes);
            return hexString.ToUpper();

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

        private static string hex2bin(string hexdata)
        {
            if (hexdata == null)
                throw new ArgumentNullException("hexdata");
            if (hexdata.Length % 2 != 0)
                throw new ArgumentException("hexdata should have even length");

            byte[] bytes = new byte[hexdata.Length / 2];
            for (int i = 0; i < hexdata.Length; i += 2)
                bytes[i / 2] = (byte)(HexValue(hexdata[i]) * 0x10
                + HexValue(hexdata[i + 1]));
            return Encoding.GetEncoding(1252).GetString(bytes);
        }

        private static int HexValue(char c)
        {
            int ch = (int)c;
            if (ch >= (int)'0' && ch <= (int)'9')
                return ch - (int)'0';
            if (ch >= (int)'a' && ch <= (int)'f')
                return ch - (int)'a' + 10;
            if (ch >= (int)'A' && ch <= (int)'F')
                return ch - (int)'A' + 10;
            throw new ArgumentException("Not a hexadecimal digit.");
        }
    }
}
