using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using ElleWorld;
using ElleWorld.Core;

namespace ElleWorld.Database
{
	class Account
	{
		int ID = 0; //Comment: 
		int OLD_ID = 0;

		string username = null; //Comment: 
		string OLD_username = null;

		string password = null; //Comment: Hashed Password
		string OLD_password = null;

		string email = null; //Comment: 
		string OLD_email = null;

		int isOnline = 0; //Comment: 0 - OffLine, 1 - Online
		int OLD_isOnline = 0;

		int type = 1; //Comment: 1 - Player, 2 - Admin
		int OLD_type = 1;
        
        static Config_Manager conf = new Config_Manager();

        MySqlConnection AuthConn = new MySqlConnection("server=" + conf.getValue("mysql_host") + ";user=" + conf.getValue("mysql_user") + ";database=" + conf.getValue("mysql_auth_db") + ";password=" + conf.getValue("mysql_password") + ";");

        public Account(int ID)
        {
            AuthConn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT ID, username, password, email, isOnline, type FROM account WHERE ID = @id;", AuthConn);
            MySqlParameter idParameter = new MySqlParameter("@id", MySqlDbType.Int16, 0);
            idParameter.Value = ID;
            cmd.Parameters.Add(idParameter);
            MySqlDataReader row = cmd.ExecuteReader();
            while (row.Read())
            {
                ID = Convert.ToInt32(row["ID"]);
                email = row["email"].ToString();
                username = row["username"].ToString();
                password = row["password"].ToString();
                isOnline = Convert.ToInt16(row["isOnline"]);
                type = Convert.ToInt16(row["type"]);
            }
            row.Close();
            AuthConn.Close();
            updateOldValues();
        }

		public Account(string _email, string _password)
		{
            email = _email;

            _password = ServerMGR.CreateBNetPassword(email, _password);

            AuthConn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT ID, username, password, email, isOnline, type FROM account WHERE email = @email AND password = @password;", AuthConn);
            MySqlParameter passwordParameter = new MySqlParameter("@password", MySqlDbType.VarChar, 0);
            MySqlParameter emailParameter = new MySqlParameter("@email", MySqlDbType.VarChar, 0);
            passwordParameter.Value = _password.ToUpper();
            emailParameter.Value = email.ToUpper();
            cmd.Parameters.Add(emailParameter);
            cmd.Parameters.Add(passwordParameter);
            MySqlDataReader row = cmd.ExecuteReader();
            while (row.Read())
            {
                ID = Convert.ToInt32(row["ID"]);
                username = row["username"].ToString();
            }
            row.Close();
            AuthConn.Close();

            updateOldValues();
        }

		public void delete()
		{
            AuthConn.Open();
			MySqlCommand cmd = new MySqlCommand("", AuthConn);
			cmd.CommandText = "DELETE FROM account WHERE ID = @ID AND username = @username AND password = @password AND email = @email AND isOnline = @isOnline AND type = @type;";
			MySqlParameter idParameter = new MySqlParameter("@ID", MySqlDbType.VarChar, 0);
			MySqlParameter usernameParameter = new MySqlParameter("@username", MySqlDbType.VarChar, 0);
			MySqlParameter passwordParameter = new MySqlParameter("@password", MySqlDbType.VarChar, 0);
			MySqlParameter emailParameter = new MySqlParameter("@email", MySqlDbType.VarChar, 0);
			MySqlParameter isonlineParameter = new MySqlParameter("@isOnline", MySqlDbType.VarChar, 0);
			MySqlParameter typeParameter = new MySqlParameter("@type", MySqlDbType.VarChar, 0);
			idParameter.Value = ID;
			usernameParameter.Value = username;
			passwordParameter.Value = password;
			emailParameter.Value = email;
			isonlineParameter.Value = isOnline;
			typeParameter.Value = type;
			cmd.Parameters.Add(idParameter);
			cmd.Parameters.Add(usernameParameter);
			cmd.Parameters.Add(passwordParameter);
			cmd.Parameters.Add(emailParameter);
			cmd.Parameters.Add(isonlineParameter);
			cmd.Parameters.Add(typeParameter);
			cmd.ExecuteNonQuery();
            AuthConn.Close();
		}

		public void update()
		{
            AuthConn.Open();
			MySqlCommand cmd = new MySqlCommand("", AuthConn);
			cmd.CommandText = "UPDATE account SET ID = @newID, username = @newusername, password = @newpassword, email = @newemail, isOnline = @newisOnline, type = @newtype WHERE ID = @ID AND username = @username AND password = @password AND email = @email AND isOnline = @isOnline AND type = @type;";
			MySqlParameter OLD_idParameter = new MySqlParameter("@ID", MySqlDbType.VarChar, 0);
			MySqlParameter OLD_usernameParameter = new MySqlParameter("@username", MySqlDbType.VarChar, 0);
			MySqlParameter OLD_passwordParameter = new MySqlParameter("@password", MySqlDbType.VarChar, 0);
			MySqlParameter OLD_emailParameter = new MySqlParameter("@email", MySqlDbType.VarChar, 0);
			MySqlParameter OLD_isonlineParameter = new MySqlParameter("@isOnline", MySqlDbType.VarChar, 0);
			MySqlParameter OLD_typeParameter = new MySqlParameter("@type", MySqlDbType.VarChar, 0);
			MySqlParameter idParameter = new MySqlParameter("@newID", MySqlDbType.VarChar, 0);
			MySqlParameter usernameParameter = new MySqlParameter("@newusername", MySqlDbType.VarChar, 0);
			MySqlParameter passwordParameter = new MySqlParameter("@newpassword", MySqlDbType.VarChar, 0);
			MySqlParameter emailParameter = new MySqlParameter("@newemail", MySqlDbType.VarChar, 0);
			MySqlParameter isonlineParameter = new MySqlParameter("@newisOnline", MySqlDbType.VarChar, 0);
			MySqlParameter typeParameter = new MySqlParameter("@newtype", MySqlDbType.VarChar, 0);
			idParameter.Value = ID;
			usernameParameter.Value = username;

            password = ServerMGR.CreateBNetPassword(username, password);

			passwordParameter.Value = password;
			emailParameter.Value = email;
			isonlineParameter.Value = isOnline;
			typeParameter.Value = type;
			OLD_idParameter.Value = OLD_ID;
			OLD_usernameParameter.Value = OLD_username;
			OLD_passwordParameter.Value = OLD_password;
			OLD_emailParameter.Value = OLD_email;
			OLD_isonlineParameter.Value = OLD_isOnline;
			OLD_typeParameter.Value = OLD_type;
			cmd.Parameters.Add(idParameter);
			cmd.Parameters.Add(usernameParameter);
			cmd.Parameters.Add(passwordParameter);
			cmd.Parameters.Add(emailParameter);
			cmd.Parameters.Add(isonlineParameter);
			cmd.Parameters.Add(typeParameter);
			cmd.Parameters.Add(OLD_idParameter);
			cmd.Parameters.Add(OLD_usernameParameter);
			cmd.Parameters.Add(OLD_passwordParameter);
			cmd.Parameters.Add(OLD_emailParameter);
			cmd.Parameters.Add(OLD_isonlineParameter);
			cmd.Parameters.Add(OLD_typeParameter);
			cmd.ExecuteNonQuery();
            AuthConn.Close();
			updateOldValues();
		}

		private void updateOldValues()
		{
			OLD_ID = ID;
			OLD_username = username;
			OLD_password = password;
			OLD_email = email;
			OLD_isOnline = isOnline;
			OLD_type = type;
		}

        public bool Validate()
        {
            if (ID > 0)
                return true;
            else
                return false;
        }

        public string GetUsername()
        {
            return username;
        }

        public string GetPassword()
        {
            return password;
        }

        public void SetPassword(string newpassword)
        {
            password = ServerMGR.CreateBNetPassword(this.username, newpassword);
        }

        public int GetID()
        {
            return ID;
        }

        public string GetEmail()
        {
            return email;
        }

        public void SetOnline()
        {
            AuthConn.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE accounts SET isOnline = 1 WHERE idAccount = @id;", AuthConn);
            MySqlParameter idParameter = new MySqlParameter("@id", MySqlDbType.Int32, 0);
            idParameter.Value = ID;
            cmd.Parameters.Add(idParameter);
            cmd.ExecuteNonQuery();
            AuthConn.Close();
        }

        public void SetOffline()
        {
            AuthConn.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE accounts SET isOnline = 0 WHERE idAccount = @id;", AuthConn);
            MySqlParameter idParameter = new MySqlParameter("@id", MySqlDbType.Int32, 0);
            idParameter.Value = ID;
            cmd.Parameters.Add(idParameter);
            cmd.ExecuteNonQuery();
            AuthConn.Close();
        }
    }
}
