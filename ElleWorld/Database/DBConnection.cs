using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lappa_ORM;

namespace ElleWorld.Database
{
    class DBConnection
    {
        public static Lappa_ORM.Database Auth = new Lappa_ORM.Database();
        public static Lappa_ORM.Database Character = new Lappa_ORM.Database();
        public static Lappa_ORM.Database Data = new Lappa_ORM.Database();
        public static Lappa_ORM.Database World = new Lappa_ORM.Database();

        public static string CreateConnectionString(string host, string user, string password, string database, int port, int minPoolSize, int maxPoolSize, DatabaseType connType)
        {
            if (connType == DatabaseType.MySql)
                return $"Server={host};User Id={user};Port={port};Password={password};Database={database};Allow Zero Datetime=True;Pooling=True;Min Pool Size={minPoolSize};Max Pool Size={maxPoolSize};CharSet=utf8";

            if (connType == DatabaseType.MSSql)
                return $"Data Source={host}; Initial Catalog = {database}; User ID = {user}; Password = {password};Pooling=True;Min Pool Size={minPoolSize};Max Pool Size={maxPoolSize}";

            return null;
        }
    }
}
