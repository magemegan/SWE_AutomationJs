using System;
using System.Configuration;
using System.IO;
using Microsoft.Data.Sqlite;

namespace SWE_AutomationJs_UI_Design.Data
{
    internal static class Db
    {
        private const string ConnectionStringName = "Restaurant";
        private const string DbPathKey = "Restaurant.DbPath";

        static Db()
        {
            SQLitePCL.Batteries_V2.Init();
        }

        public static string ConnectionString
        {
            get
            {
                ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[ConnectionStringName];
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
            return connection;
        }

        private static string ResolveDbPath()
        {
            string configuredPath = ConfigurationManager.AppSettings[DbPathKey];
            if (!string.IsNullOrWhiteSpace(configuredPath))
            {
                return Path.GetFullPath(configuredPath);
            }

            DirectoryInfo current = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            while (current != null)
            {
                string candidate = Path.Combine(current.FullName, "Database", "data", "restaurant.db");
                if (File.Exists(candidate))
                {
                    return candidate;
                }

                current = current.Parent;
            }

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "restaurant.db");
        }
    }
}
