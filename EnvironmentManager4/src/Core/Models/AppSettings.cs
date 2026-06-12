using EnvironmentManager4.src.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Storage.Search;

namespace EnvironmentManager4.src.Core.Models
{
    public class AppSettings
    {
        // Database Management
        public string DBBackupDirectory
        {
            get { return Properties.Settings.Default.DBBackupDirectory; }
            set { Properties.Settings.Default.DBBackupDirectory = value; Properties.Settings.Default.Save(); }
        }
        public string DBConnection
        {
            get { return Properties.Settings.Default.DBConnection; }
            set { Properties.Settings.Default.DBConnection = value; Properties.Settings.Default.Save(); }
        }
        public string DBUserName
        {
            get { return Properties.Settings.Default.DBUserName; }
            set { Properties.Settings.Default.DBUserName = value; Properties.Settings.Default.Save(); }
        }
        public string DBPassword
        {
            get { return Properties.Settings.Default.DBPassword; }
            set { Properties.Settings.Default.DBPassword = value; Properties.Settings.Default.Save(); }
        }
        public string SavedConnections
        {
            get { return Properties.Settings.Default.SavedConnections; }
            set { Properties.Settings.Default.SavedConnections = value; Properties.Settings.Default.Save(); }
        }

        // Build Management
        public string BMSalesPadx86
        {
            get { return Properties.Settings.Default.BMSalesPadx86; }
            set { Properties.Settings.Default.BMSalesPadx86 = value; Properties.Settings.Default.Save(); }
        }
        public string BMSalesPadx64
        {
            get { return Properties.Settings.Default.BMSalesPadx64; }
            set { Properties.Settings.Default.BMSalesPadx64 = value; Properties.Settings.Default.Save(); }
        }
        public string BMDataCollection
        {
            get { return Properties.Settings.Default.BMDataCollection; }
            set { Properties.Settings.Default.BMDataCollection = value; Properties.Settings.Default.Save(); }
        }
        public string BMSalesPadMobile
        {
            get { return Properties.Settings.Default.BMSalesPadMobile; }
            set { Properties.Settings.Default.BMSalesPadMobile = value; Properties.Settings.Default.Save(); }
        }
        public string BMShipCenterx86
        {
            get { return Properties.Settings.Default.BMShipCenterx86; }
            set { Properties.Settings.Default.BMShipCenterx86 = value; Properties.Settings.Default.Save(); }
        }
        public string BMShipCenterx64
        {
            get { return Properties.Settings.Default.BMShipCenterx64; }
            set { Properties.Settings.Default.BMShipCenterx64 = value; Properties.Settings.Default.Save(); }
        }
        public string GPWeb
        {
            get { return Properties.Settings.Default.GPWeb; }
            set { Properties.Settings.Default.GPWeb = value; Properties.Settings.Default.Save(); }
        }
        public string WebAPI
        {
            get { return Properties.Settings.Default.WebAPI; }
            set { Properties.Settings.Default.WebAPI = value; Properties.Settings.Default.Save(); }
        }

        // Other
        public string Mode
        {
            get { return Properties.Settings.Default.Mode; }
            set { Properties.Settings.Default.Mode = value; Properties.Settings.Default.Save(); }
        }
        public bool OShowAlwaysOnTop
        {
            get { return Properties.Settings.Default.OShowAlwaysOnTop; }
            set { Properties.Settings.Default.OShowAlwaysOnTop = value; Properties.Settings.Default.Save(); }
        }
        public bool OShowVPNIP
        {
            get { return Properties.Settings.Default.OShowVPNIP; }
            set { Properties.Settings.Default.OShowVPNIP = value; Properties.Settings.Default.Save(); }
        }
        public bool OShowWIFIIP
        {
            get { return Properties.Settings.Default.OShowWIFIIP; }
            set { Properties.Settings.Default.OShowWIFIIP = value; Properties.Settings.Default.Save(); }
        }
        public bool OEnableWaterBot
        {
            get { return Properties.Settings.Default.OEnableWaterBot; }
            set { Properties.Settings.Default.OEnableWaterBot = value; Properties.Settings.Default.Save(); }
        }
        public bool OEnableInstallToasts
        {
            get { return Properties.Settings.Default.OEnableInstallToasts; }
            set { Properties.Settings.Default.OEnableInstallToasts = value; Properties.Settings.Default.Save(); }
        }

        // Application Upgrading
        public bool UpgradePrompt
        {
            get { return Properties.Settings.Default.UpgradePrompt; }
            set { Properties.Settings.Default.UpgradePrompt = value; Properties.Settings.Default.Save(); }
        }
        public string UpgradeTypeThreshold
        {
            get { return Properties.Settings.Default.UpgradeTypeThreshold; }
            set { Properties.Settings.Default.UpgradeTypeThreshold = value; Properties.Settings.Default.Save(); }
        }

        // Versioning
        public int Version
        {
            get { return Properties.Settings.Default.Version; }
            set { Properties.Settings.Default.Version = value; Properties.Settings.Default.Save(); }
        }

        //Increment this when a settings migration needs to happen
        public const int SettingsVersion = 5;

        public static void CheckForSettingsUpdate()
        {
            if (SettingsVersion != GetSettingsVersion())
                MigrateSettings();
        }

        private static int GetSettingsVersion()
        {
            // settings.json file path
            string settingsPath = DirectoryUtilities.GetFile("Settings.json");

            int savedSettingsVersion;
            if (File.Exists(settingsPath))
            {
                SettingsModel jsonSettings = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(settingsPath));
                savedSettingsVersion = jsonSettings.Version;
            }
            else
            {
                AppSettings settings = new AppSettings();
                savedSettingsVersion = settings.Version;
            }
            return savedSettingsVersion;
        }

        public bool SQLSettingsConfigured()
        {
            if (String.IsNullOrWhiteSpace(this.DBConnection) ||
                String.IsNullOrWhiteSpace(this.DBUserName) ||
                String.IsNullOrWhiteSpace(this.DBPassword))
                return false;
            else
                return true;
        }

        public static void MigrateSettings()
        {
            try
            {
                // load Settings.json file settings
                string settingsFile = DirectoryUtilities.GetFile("Settings.json");
                if (File.Exists(settingsFile))
                {
                    SettingsModel jsonSettings = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(settingsFile));

                    // serialize connections list to store in appsettings
                    var connections = ConnectionInfo.EncodeConnections(jsonSettings.DBManagement.ConnectionsList);

                    // write settings to app settings
                    AppSettings appSettings = new AppSettings
                    {
                        // database management
                        DBBackupDirectory = jsonSettings.DBManagement.DatabaseBackupDirectory,
                        DBConnection = jsonSettings.DBManagement.Connection,
                        DBUserName = jsonSettings.DBManagement.SQLServerUserName,
                        DBPassword = jsonSettings.DBManagement.SQLServerPassword,
                        SavedConnections = connections,

                        // build management
                        BMSalesPadx86 = jsonSettings.BuildManagement.SalesPadx86Directory,
                        BMSalesPadx64 = jsonSettings.BuildManagement.SalesPadx64Directory,
                        BMDataCollection = jsonSettings.BuildManagement.DataCollectionDirectory,
                        BMSalesPadMobile = jsonSettings.BuildManagement.SalesPadMobileDirectory,
                        BMShipCenterx86 = jsonSettings.BuildManagement.ShipCenterx86Directory,
                        BMShipCenterx64 = jsonSettings.BuildManagement.ShipCenterx64Directory,
                        GPWeb = jsonSettings.BuildManagement.GPWebDirectory,
                        WebAPI = jsonSettings.BuildManagement.WebAPIDirectory,

                        // other
                        Mode = jsonSettings.Other.Mode,
                        OShowAlwaysOnTop = jsonSettings.Other.ShowAlwaysOnTop,
                        OShowVPNIP = jsonSettings.Other.ShowVPNIP,
                        OShowWIFIIP = jsonSettings.Other.ShowIP,
                        OEnableWaterBot = jsonSettings.Other.EnableWaterBot,
                        OEnableInstallToasts = jsonSettings.Other.EnableInstallToasts,
                        OPromptForUpgrade = jsonSettings.Other.PromptForUpdate,

                        // version
                        Version = SettingsVersion,
                    };
                    // delete the settings json file
                    File.Delete(settingsFile);
                }
                Toasts.Toast("SUCCESS", "Settings were migrated successfully.");
            }
            catch (Exception e)
            {
                Toasts.Toast("FAILURE", "There were issues migrating Settings.json to Application Settings.");
                ErrorHandling.DisplayExceptionMessage(e);
                return;
            }
        }

        public static void SaveSettings(SettingsModel values)
        {
            // serialize connections list to store in appsettings
            var connections = ConnectionInfo.EncodeConnections(values.DBManagement.ConnectionsList);

            // write settings to app settings
            new AppSettings
            {
                // database management
                DBBackupDirectory = values.DBManagement.DatabaseBackupDirectory,
                DBConnection = values.DBManagement.Connection,
                DBUserName = values.DBManagement.SQLServerUserName,
                DBPassword = values.DBManagement.SQLServerPassword,
                SavedConnections = connections,

                // build management
                BMSalesPadx86 = values.BuildManagement.SalesPadx86Directory,
                BMSalesPadx64 = values.BuildManagement.SalesPadx64Directory,
                BMDataCollection = values.BuildManagement.DataCollectionDirectory,
                BMSalesPadMobile = values.BuildManagement.SalesPadMobileDirectory,
                BMShipCenterx86 = values.BuildManagement.ShipCenterx86Directory,
                BMShipCenterx64 = values.BuildManagement.ShipCenterx64Directory,
                GPWeb = values.BuildManagement.GPWebDirectory,
                WebAPI = values.BuildManagement.WebAPIDirectory,

                // other
                Mode = values.Other.Mode,
                OShowAlwaysOnTop = values.Other.ShowAlwaysOnTop,
                OShowVPNIP = values.Other.ShowVPNIP,
                OShowWIFIIP = values.Other.ShowIP,
                OEnableWaterBot = values.Other.EnableWaterBot,
                OEnableInstallToasts = values.Other.EnableInstallToasts,
                OPromptForUpgrade = values.Other.PromptForUpdate,
            };
        }

        public SettingsModel GetAllSettings()
        {
            // load connections
            List<ConnectionInfo> connections = new List<ConnectionInfo>();
            DataProtectionScope Scope = DataProtectionScope.CurrentUser;
            string base64 = this.SavedConnections;
            if (!string.IsNullOrWhiteSpace(base64))
            {
                byte[] encryptedBytes = Convert.FromBase64String(base64);
                byte[] decryptedBytes = ProtectedData.Unprotect(encryptedBytes, null, Scope);
                string json = Encoding.UTF8.GetString(decryptedBytes);
                connections = JsonConvert.DeserializeObject<List<ConnectionInfo>>(json);
            }

            var dbManagement = new DBManagement
            {
                DatabaseBackupDirectory = this.DBBackupDirectory,
                Connection = this.DBConnection,
                ConnectionsList = connections,
                SQLServerUserName = this.DBUserName,
                SQLServerPassword = UtilitiesOld.EncryptString(UtilitiesOld.ToSecureString(this.DBPassword)),
            };

            var buildManagement = new BuildManagement
            {
                SalesPadx86Directory = this.BMSalesPadx86,
                SalesPadx64Directory = this.BMSalesPadx64,
                DataCollectionDirectory = this.BMDataCollection,
                SalesPadMobileDirectory = this.BMSalesPadMobile,
                ShipCenterx86Directory = this.BMShipCenterx86,
                ShipCenterx64Directory = this.BMShipCenterx64,
                GPWebDirectory = this.GPWeb,
                WebAPIDirectory = this.WebAPI
            };

            var other = new Other
            {
                Mode = this.Mode,
                ShowAlwaysOnTop = this.OShowAlwaysOnTop,
                ShowVPNIP = this.OShowVPNIP,
                ShowIP = this.OShowWIFIIP,
                EnableWaterBot = this.OEnableWaterBot,
                EnableInstallToasts = this.OEnableInstallToasts,
                PromptForUpdate = this.OPromptForUpgrade,
            };

            var settings = new SettingsModel
            {
                Version = this.Version,
                DBManagement = dbManagement,
                BuildManagement = buildManagement,
                Other = other
            };

            return settings;
        }
    }
}
