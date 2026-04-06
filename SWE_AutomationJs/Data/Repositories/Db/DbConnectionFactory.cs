using System.Data.SQLite;

namespace SWE_AutomationJs.Data.Db
{
    public static class DbConnectionFactory
    {
        private static readonly string ConnectionString =
            @"Data Source=..\..\..\Database\data\restaurant.db;Version=3;";

        public static SQLiteConnection Create()
        {
            return new SQLiteConnection(ConnectionString);
        }
    }
}