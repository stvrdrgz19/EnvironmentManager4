using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4
{
    public class SettingsModel
    {
        public int Version { get; set; }
        public DbManagement DbManagement { get; set; }
        public BuildManagement BuildManagement { get; set; }
        public Other Other { get; set; }
    }

    public class DbManagement
    {
        public string DatabaseBackupDirectory { get; set; }
        public string Connection { get; set; }
        public List<Connection> ConnectionsList { get; set; }
        public string SQLServerUserName { get; set; }
        public string SQLServerPassword { get; set; }
        public bool ResetDatabaseAfterRestore { get; set; }
        public string DBToRestore { get; set; }
    }

    public class BuildManagement
    {
        public string SalesPadx86Directory { get; set; }
        public string SalesPadx64Directory { get; set; }
        public string DataCollectionDirectory { get; set; }
        public string SalesPadMobileDirectory { get; set; }
        public string ShipCenterDirectory { get; set; }
        public string GPWebDirectory { get; set; }
        public string WebAPIDirectory { get; set; }
    }

    public class Other
    {
        public string Mode { get; set; }
        public string DefaultVersion { get; set; }
        public bool ShowAlwaysOnTop { get; set; }
        public bool ShowVPNIP { get; set; }
        public bool ShowIP { get; set; }
        public bool EnableWaterBot { get; set; }
    }

    public class Connection
    {
        public string ConnectionName { get; set; }
        public string ConnectionUN { get; set; }
        public string ConnectionPW { get; set; }
    }

    public class SettingsUtilities
    {
        //Increment this when a settings migration needs to happen due to the file structure changing
        public const int SettingsVersion = 2;

        public static SettingsModel GetSettings()
        {
            string settingsFile = Utilities.GetFile("Settings.json");
            if (!File.Exists(settingsFile))
                GenerateSettingsFile();

            return JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(settingsFile));
        }

        public static int GetSettingsVersion()
        {
            return GetSettings().Version;
        }

        public static void UpdateSettingsFile(SettingsModel settings)
        {
            if (SettingsVersion != GetSettingsVersion())
                MigrateSettings(settings);
        }

        public static void MigrateSettings(SettingsModel settings)
        {
            settings.Version = SettingsVersion;

            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            try
            {
                File.WriteAllText(Utilities.GetFile("Settings.json"), json);
            }
            catch (Exception e)
            {
                ErrorHandling.DisplayExceptionMessage(e);
                return;
            }
        }

        public static void GenerateSettingsFile()
        {
            List<Connection> connectionList = new List<Connection>();

            var dbManagement = new DbManagement
            {
                DatabaseBackupDirectory = "",
                Connection = "",
                ConnectionsList = connectionList,
                SQLServerUserName = "",
                SQLServerPassword = "",
                ResetDatabaseAfterRestore = false,
                DBToRestore = ""
            };

            var buildManagement = new BuildManagement
            {
                SalesPadx86Directory = @"C:\Program Files (x86)\SalesPad.Desktop",
                SalesPadx64Directory = @"C:\Program Files\SalesPad.Desktop",
                DataCollectionDirectory = @"C:\Program Files (x86)\DataCollection",
                SalesPadMobileDirectory = @"C:\Program Files (x86)\SalesPad.GP.Mobile.Server",
                ShipCenterDirectory = @"C:\Program Files (x86)\ShipCenter",
                GPWebDirectory = @"C:\inetpub\wwwroot\SalesPadWebPortal",
                WebAPIDirectory = @"C:\inetpub\wwwroot\SalesPadWebAPI"
            };

            var other = new Other
            {
                Mode = "Standard",
                DefaultVersion = "x64",
                ShowAlwaysOnTop = true,
                ShowVPNIP = true,
                ShowIP = true,
                EnableWaterBot = false
            };

            var settings = new SettingsModel
            {
                Version = SettingsVersion,
                DbManagement = dbManagement,
                BuildManagement = buildManagement,
                Other = other
            };

            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            try
            {
                File.WriteAllText(Utilities.GetFile("Settings.json"), json);
            }
            catch (Exception e)
            {
                ErrorHandling.DisplayExceptionMessage(e);
                return;
            }
        }
    }
}
