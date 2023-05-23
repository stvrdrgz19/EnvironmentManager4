using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using Utilities;

namespace SqliteDatabaseUtilities
{
    public class DatabaseUtilities
    {
        public static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public static void GetDatabaseFile()
        {
            string dbFile = Utils.GetFile("Database.db");
            if (!File.Exists(dbFile))
                GenerateDatabaseFile();
        }

        public static void GenerateDatabaseFile()
        {
            SQLiteConnection.CreateFile(String.Format(@"{0}\Files\Database.db", Environment.CurrentDirectory));
            CreateTables();
        }

        public static void CreateTables()
        {
            SQLiteConnection conn = new SQLiteConnection(LoadConnectionString());
            conn.Open();

            string sqlDatabaseActivity = @"CREATE TABLE DatabaseActivity (
                Id INTEGER NOT NULL UNIQUE,
                TimeStamp TEXT NOT NULL,
            	Action TEXT NOT NULL,
            	Backup TEXT NOT NULL,
            	PRIMARY KEY(Id AUTOINCREMENT)
            );";

            string sqlInstalledBuilds = @"CREATE TABLE InstalledBuilds (
                Id INTEGER NOT NULL UNIQUE,
                Path TEXT NOT NULL,
                Version TEXT,
                EntryDate TEXT NOT NULL,
                Product TEXT NOT NULL,
                InstallPath TEXT NOT NULL,
                PRIMARY KEY(Id AUTOINCREMENT)
            );";

            string sqlInstalledDLLs = @"CREATE TABLE InstalledDLLs (
                Id INTEGER NOT NULL UNIQUE,
                Parent_Id INTEGER NOT NULL,
                Name TEXT NOT NULL,
                Type TEXT NOT NULL,
                Version TEXT NOT NULL,
                EntryDate TEXT NOT NULL,
                PRIMARY KEY(Id AUTOINCREMENT),
                FOREIGN KEY(Parent_Id) REFERENCES InstalledBuilds(Id)
            );";

            List<string> sqlScripts = new List<string> { sqlDatabaseActivity, sqlInstalledBuilds, sqlInstalledDLLs };
            foreach (string script in sqlScripts)
            {
                SQLiteCommand command = new SQLiteCommand(script, conn);
                command.ExecuteNonQuery();
            }
        }
    }
}
