using Dapper;
using ErrorHandling;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Utilities;

namespace Settings
{
    public class SQL
    {
        public static List<string> GetCompanyDatabases()
        {
            List<string> databaseList = RetrieveSQLDatabases();
            List<string> companyDatabaseList = new List<string>();
            foreach (string database in databaseList)
            {
                if (!database.Contains("DYNAMICS"))
                {
                    companyDatabaseList.Add(database);
                }
            }
            return companyDatabaseList;
        }

        public static List<string> RetrieveSQLDatabases()
        {
            SettingsModel settings = SettingsUtilities.GetSettings();
            List<string> databaseList = new List<string>();
            string script = @"SELECT name FROM master.dbo.sysdatabases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb', 'toolbox')";
            try
            {
                //get list of databases.
                SqlConnection sqlCon = new SqlConnection(String.Format(@"Data Source={0};Initial Catalog=MASTER;User ID={1};Password={2};",
                    settings.DbManagement.Connection, settings.DbManagement.SQLServerUserName,
                    Utils.ToInsecureString(Utils.DecryptString(settings.DbManagement.SQLServerPassword))));
                databaseList.AddRange(sqlCon.Query<string>(script).AsList());
            }
            catch (Exception e)
            {
                ErrorHandle.LogException(e);
                ErrorHandle.DisplayExceptionMessage(e);
            }
            return databaseList;
        }
    }
}
