using System;
using System.Configuration;
using System.IO;
using Microsoft.Data.Sqlite;

namespace SWE_AutomationJs_UI_Design.Data
{
    internal static class Db
    {
        private const string ConnectionStringName = "RestaurantDb";

        public static string DatabasePath
        {
            get { return ResolveDbPath(); }
        }

        public static string ConnectionString
        {
            get
            {
                ConnectionStringSettings settings =
                    ConfigurationManager.ConnectionStrings[ConnectionStringName];

                SqliteConnectionStringBuilder builder = settings != null
                    ? new SqliteConnectionStringBuilder(settings.ConnectionString)
                    : new SqliteConnectionStringBuilder();

                builder.DataSource = ResolveDbPath();
                builder.ForeignKeys = true;

                return builder.ToString();
            }
        }

        public static SqliteConnection Open()
        {
            SqliteConnection connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = "PRAGMA foreign_keys = ON;";
                command.ExecuteNonQuery();
            }

            return connection;
        }

        private static string ResolveDbPath()
        {
            DirectoryInfo current = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            while (current != null)
            {
                string databaseFolder = Path.Combine(current.FullName, "Database");

                if (Directory.Exists(databaseFolder))
                {
                    return Path.Combine(databaseFolder, "data", "restaurant.db");
                }

                current = current.Parent;
            }

            return Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Database",
                "data",
                "restaurant.db"
            );
        }
    }
}