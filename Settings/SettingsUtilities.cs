using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using ErrorHandling;

namespace Settings
{
    public class SettingsUtilities
    {
        //Increment this when a settings migration needs to happen due to the file structure changing
        public const int SettingsVersion = 2;

        public static SettingsModel GetSettings()
        {
            string settingsFile = Utils.GetFile("Settings.json");
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
                File.WriteAllText(Utils.GetFile("Settings.json"), json);
            }
            catch (Exception e)
            {
                ErrorHandle.DisplayExceptionMessage(e);
                return;
            }
        }

        public static void GenerateSettingsFile()
        {
            List<Connection> connectionList = new List<Connection>();

            var dbManagement = new DBManagement
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
                EnableWaterBot = false,
                EnableInstallToasts = false
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
                File.WriteAllText(Utils.GetFile("Settings.json"), json);
            }
            catch (Exception e)
            {
                ErrorHandle.DisplayExceptionMessage(e);
                return;
            }
        }
    }
}
