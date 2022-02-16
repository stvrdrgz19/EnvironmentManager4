using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4
{
    public class SqliteDataAccess
    {
        //https://www.youtube.com/watch?v=ayp3tHEkRc0&t=1046s

        /// <summary>
        /// Retrieves the default connection for the sqlite database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Sqlite Database Connection String</returns>
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        /// <summary>
        /// Writes the installed build to the InstalledBuilds table of the sqlite database
        /// </summary>
        /// <param name="build">the build information to be written to InstalledBuilds</param>
        public static void SaveBuild(BuildModel build)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("INSERT INTO InstalledBuilds (Path, Version, EntryDate, Product, InstallPath) VALUES (@Path, @Version, @EntryDate, @Product, @InstallPath)", build);
            }
        }

        /// <summary>
        /// Loading the InstalledBuilds data into the InstalledBuilds form/table
        /// </summary>
        /// <returns>a list of build installation information from InstalledBuilds</returns>
        public static List<BuildModel> LoadBuilds()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<BuildModel>("SELECT Path, Version, EntryDate, Product, InstallPath FROM InstalledBuilds ORDER BY Id DESC", new DynamicParameters());
                return output.ToList();
            }
        }

        /// <summary>
        /// Saving selected dlls from the install to the InstalledDlls table
        /// </summary>
        /// <param name="dll">dll information (name/type/version) to be written to the InstalledDlls table</param>
        public static void SaveDlls(DllModel dll)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("INSERT INTO InstalledDlls (Parent_Id, Name, Type, Version, EntryDate) VALUES (@Parent_Id, @Name, @Type, @Version, @EntryDate)", dll);
            }
        }

        /// <summary>
        /// Loading the InstalledDlls data into the InstalledBuilds form/dlls table
        /// </summary>
        /// <param name="parentID">The parent ID value of the build the selected dlls belong to</param>
        /// <returns>Dll with information</returns>
        public static List<DllModel> LoadDlls(int parentID)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<DllModel>("SELECT Name, Type FROM InstalledDlls WHERE Parent_Id = " + parentID, new DynamicParameters());
                return output.ToList();
            }
        }

        /// <summary>
        /// Getting the last installed build of the selected product from the InstalledBuilds table
        /// </summary>
        /// <param name="product">The product to be searched for (SalesPad, DataCollection, etc)</param>
        /// <returns>The last recording of an installed build for the selected product</returns>
        public static string LastInstalledBuild(string product, string version)
        {
            string path = "";
            SQLiteConnection conn = new SQLiteConnection(LoadConnectionString());
            conn.Open();
            string statement = "SELECT InstallPath FROM InstalledBuilds Where Product = '" + product + "' ORDER BY Id DESC LIMIT 1";
            string stmt = String.Format("SELECT InstallPath FROM InstalledBuilds WHERE Product = '{0}' AND Version = '{1}' ORDER BY Id DESC LIMIT 1", product, version);
            SQLiteCommand command = new SQLiteCommand(statement, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                path = Convert.ToString(reader["InstallPath"]);
            }
            return path;
        }

        /// <summary>
        /// Retrieving the parent ID of the selected build
        /// </summary>
        /// <param name="entryDate">The date/time value the selected build was installed</param>
        /// <returns>The Id value of the selected build</returns>
        public static int GetParentId(string entryDate)
        {
            int parentId = 0;
            SQLiteConnection conn = new SQLiteConnection(LoadConnectionString());
            conn.Open();
            string stmt = "SELECT Id FROM InstalledBuilds WHERE EntryDate = '" + entryDate + "'";
            SQLiteCommand command = new SQLiteCommand(stmt, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                parentId = Convert.ToInt32(reader["Id"]);
            }
            return parentId;
        }

        /// <summary>
        /// Retrieves the most recent parent id to assign Dlls to their parent build
        /// </summary>
        /// <returns>the Id of the last installed build</returns>
        public static int GetLastParentId()
        {
            int parentId = 0;
            SQLiteConnection conn = new SQLiteConnection(LoadConnectionString());
            conn.Open();
            string stmt = "SELECT Id FROM InstalledBuilds ORDER BY Id DESC LIMIT 1";
            SQLiteCommand command = new SQLiteCommand(stmt, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                parentId = Convert.ToInt32(reader["Id"]);
            }
            return parentId;
        }

        /// <summary>
        /// Writing the database activity that was performed to the DatabaseActivityLog table
        /// </summary>
        /// <param name="databaseActivity">The database activity that was performed</param>
        public static void SaveDatabaseActivity(DatabaseActivityLogModel databaseActivity)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("INSERT INTO DatabaseActivity (TimeStamp, Action, Backup) VALUES (@TimeStamp, @Action, @Backup)", databaseActivity);
            }
        }

        /// <summary>
        /// Loading the database activity from the DatabaseActivityLog table into the database log form
        /// </summary>
        /// <returns>Database activity</returns>
        public static List<DatabaseActivityLogModel> LoadDatabaseActivity()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<DatabaseActivityLogModel>("SELECT TimeStamp, Action, Backup FROM DatabaseActivity ORDER BY Id DESC", new DynamicParameters());
                return output.ToList();
            }
        }
    }
}
