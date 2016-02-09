using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ElleWorld.Database
{
    class DBConnection
    {
        public static MySqlConnection Auth = new MySqlConnection();
        public static MySqlConnection Character = new MySqlConnection();
        public static MySqlConnection World = new MySqlConnection();

        public static string CreateConnectionString(string host, string user, string password, string database, int port, int minPoolSize, int maxPoolSize)
        {
            return $"Server={host};User Id={user};Port={port};Password={password};Database={database};Allow Zero Datetime=True;Pooling=True;Min Pool Size={minPoolSize};Max Pool Size={maxPoolSize};CharSet=utf8";
        }
    }
}
