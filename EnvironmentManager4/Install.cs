﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public partial class Install : Form
    {
        public Install()
        {
            InitializeComponent();
        }

        public static string product = "";
        public static string installerPath = "";
        public static string version = "";
        public static string installer = "";
        public static string defaultInstallPath = "";

        public static void InstallProduct(string product, string defaultSettingsFile, string version = null)
        {
            string installer = "";
            string installerPath = "";
            string initialDir = "";
            string defaultInstallPath = "";
            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(defaultSettingsFile));
            switch (product)
            {
                case "SalesPad GP":
                    initialDir = @"\\sp-fileserv-01\Shares\Builds\SalesPad.GP\";
                    switch (version)
                    {
                        case "x86":
                            defaultInstallPath = settingsModel.BuildManagement.SalesPadx86Directory;
                            break;
                        case "x64":
                            defaultInstallPath = settingsModel.BuildManagement.SalesPadx64Directory;
                            break;
                    }
                    break;
                case "DataCollection":
                    initialDir = @"\\sp-fileserv-01\Shares\Builds\Ares\DataCollection\";
                    defaultInstallPath = settingsModel.BuildManagement.DataCollectionDirectory;
                    break;
                case "SalesPad Mobile":
                    initialDir = @"\\sp-fileserv-01\Shares\Builds\Ares\Mobile-Server\";
                    defaultInstallPath = settingsModel.BuildManagement.SalesPadMobileDirectory;
                    break;
                case "ShipCenter":
                    initialDir = @"\\sp-fileserv-01\Shares\Builds\ShipCenter\";
                    defaultInstallPath = settingsModel.BuildManagement.ShipCenterDirectory;
                    break;
                case "Customer Portal Web":
                    initialDir = @"\\sp-fileserv-01\Shares\Builds\Web-Portal\GP";
                    defaultInstallPath = settingsModel.BuildManagement.GPWebDirectory;
                    break;
                case "Customer Portal API":
                    initialDir = @"\\sp-fileserv-01\Shares\Builds\SalesPad.WebApi";
                    defaultInstallPath = settingsModel.BuildManagement.WebAPIDirectory;
                    break;
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (product == "SalesPad GP")
                {
                    openFileDialog.Filter = String.Format("Executable Files (*.exe)|*{0}.exe", version);
                }
                else
                {
                    openFileDialog.Filter = "Executable Files (*.exe)|*.exe";
                }

                openFileDialog.InitialDirectory = initialDir;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    installer = openFileDialog.FileName;
                    installerPath = Path.GetDirectoryName(installer);
                }
                else
                {
                    return;
                }
            }

            Install install = new Install();
            Install.product = product;
            Install.installerPath = installerPath;
            Install.installer = installer;
            Install.version = version;
            Install.defaultInstallPath = defaultInstallPath;
            install.Show();
        }

        public void LoadInstallPath()
        {
            if (product == "Customer Portal Web" || product == "Customer Portal API")
            {
                return;
            }
            int charCount = 0;
            switch (product)
            {
                case "SalesPad GP":
                    charCount = 43;
                    break;
                case "DataCollection":
                    charCount = 51;
                    break;
                case "SalesPad Mobile":
                    charCount = 50;
                    break;
                case "ShipCenter":
                    charCount = 42;
                    break;
            }
            tbInstallLocation.Text = GetInstallPath(defaultInstallPath, installerPath, charCount);
        }

        public static string GetInstallPath(string defaultPath, string installerPath, int charCount)
        {
            string pathFromInstaller = installerPath.Remove(0, charCount);
            return String.Format(@"{0}\{1}", defaultPath, pathFromInstaller);
        }

        public void LoadModules(string product, string installerPath, string productVersion, string installer)
        {
            string extModulesPath = "";
            string custModulesPath = "";
            if (product == "DataCollection" || product == "ShipCenter" || product == "WebPortal" || product == "SalesPad Mobile")
            {
                lbExtendedModules.Enabled = false;
                checkRunDatabaseUpdate.Enabled = false;
            }
            if (product == "SalesPad Mobile")
            {
                lbCustomModules.Enabled = false;
                cbConfigurationList.Enabled = false;
                btnAddConfiguration.Enabled = false;
                btnRemoveConfiguration.Enabled = false;
            }
            switch (product)
            {
                case "SalesPad GP":
                    extModulesPath = String.Format(@"{0}\ExtModules\{1}", installerPath, productVersion);
                    custModulesPath = String.Format(@"{0}\CustomModules\{1}", installerPath, productVersion);
                    break;
                case "WebAPI":
                    extModulesPath = String.Format(@"{0}\ExtModules", installerPath);
                    custModulesPath = String.Format(@"{0}\CustomModules", installerPath);
                    break;
                case "DataCollection":
                    custModulesPath = String.Format(@"{0}\CustomModules", installerPath);
                    break;
                case "SalesPad Mobile":
                    break;
                case "ShipCenter":
                    custModulesPath = String.Format(@"{0}\Custom", installerPath);
                    break;
                case "WebPortal":
                    custModulesPath = String.Format(@"{0}\Plugins", installerPath);
                    break;
            }
            if (!String.IsNullOrWhiteSpace(extModulesPath))
            {
                lbExtendedModules.Items.AddRange(Modules.RetrieveDLLs(extModulesPath, installerPath, product, installer));
            }
            lbCustomModules.Items.AddRange(Modules.RetrieveDLLs(custModulesPath, installerPath, product, installer));
        }

        public void InstallBuild(string installPath, List<string> extendedModules, List<string> customModules, bool launchAfterInstall, bool openInstallFolder, bool runDatabaseUpdate, bool resetDatabaseVersion)
        {
            //Disable Install button on form1.
            Form1.EnableInstallButton(false);
            //string startTime = DateTime.Now.ToString();

            //global variable installer = the installer file path
            //global variable installerpath = the path to the installer, excluding the file name
            string installerFileName = Path.GetFileName(installer);     //the actual file name without it's path
            //string tempInstaller = String.Format(@"{0}\Installers\{1}", Environment.CurrentDirectory, installerFileName);
            string tempInstaller = String.Format(@"{0}\{1}", Utilities.GetInstallerFolder(), installerFileName);
            File.Copy(installer, tempInstaller, true);

            //SILENTLY INSTALL PRODUCT
            Process installProduct = new Process();
            installProduct.StartInfo.FileName = tempInstaller;
            installProduct.StartInfo.Arguments = @"/S /D=" + installPath;
            installProduct.Start();
            installProduct.WaitForExit();

            //Implement sqlite database to store installed build as well as the last installed build

            File.Delete(tempInstaller);

            //Implement DLL Install

            if (resetDatabaseVersion)
            {
                DatabaseManagement.ResetDatabaseVersion();
            }

            if (runDatabaseUpdate)
            {
                if (!resetDatabaseVersion)
                {
                    DatabaseManagement.ResetDatabaseVersion();
                }
                Process dbUpdate = new Process();
                dbUpdate.StartInfo.FileName = installPath + @"\SalesPad.exe";
                dbUpdate.StartInfo.Arguments = @"/dbUpdate /userfields /conn=TWO";      //write sql to pull databases and prompt user for which database to update. Maybe prompt before installation
                dbUpdate.StartInfo.UseShellExecute = false;
                dbUpdate.Start();
                dbUpdate.WaitForExit();
            }

            if (openInstallFolder)
            {
                Process.Start(installPath);
            }

            if (launchAfterInstall)
            {
                Process.Start(String.Format(@"{0}\SalesPad.exe", installPath));
            }
            Form1.EnableInstallButton(true);
        }

        private void LoadConfigurations()
        {

        }

        private void Install_Load(object sender, EventArgs e)
        {
            if (product != "SalesPad GP")
            {
                checkResetDBVersion.Enabled = false;
                checkRunDatabaseUpdate.Enabled = false;
            }
            if (product == "SalesPad Mobile")
            {
                cbConfigurationList.Enabled = false;
                btnAddConfiguration.Enabled = false;
                btnRemoveConfiguration.Enabled = false;
            }
            this.Text = String.Format("Install {0}", product);
            tbSelectedBuild.Text = installerPath;
            LoadModules(product, installerPath, version, installer);
            LoadInstallPath();
            LoadConfigurations();
            return;
        }

        private void btnAddConfiguration_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you want to create a new Configuration?";
            string caption = "CONFIRM";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons, icon);

            if (result == DialogResult.Yes)
            {
                TextPrompt addConfiguration = new TextPrompt();
                TextPrompt.title = "Add Configuration";
                TextPrompt.label = "Please enter the name of the new configuration:";
                addConfiguration.FormClosing += new FormClosingEventHandler(AddConfigurationClose);
                addConfiguration.Show();
            }
            return;
        }

        private void AddConfigurationClose(object sender, FormClosingEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(TextPrompt.output))
            {
                return;
            }
            //get list of selected ext and custom dlls
        }

        private void btnRemoveConfiguration_Click(object sender, EventArgs e)
        {
            //
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Utilities.GetSettingsFile()));
            List<string> defaultPaths = new List<string>
            {
                settingsModel.BuildManagement.SalesPadx86Directory,
                settingsModel.BuildManagement.SalesPadx64Directory,
                settingsModel.BuildManagement.DataCollectionDirectory,
                settingsModel.BuildManagement.SalesPadMobileDirectory,
                settingsModel.BuildManagement.ShipCenterDirectory,
                settingsModel.BuildManagement.GPWebDirectory,
                settingsModel.BuildManagement.WebAPIDirectory
            };

            DirectoryInfo di = new DirectoryInfo(Utilities.GetInstallerFolder());
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            string installPath = tbInstallLocation.Text;
            if (defaultPaths.Contains(installPath))
            {
                MessageBox.Show("Please enter an install location different than the default:\n\n" + installPath);
                return;
            }

            if (Directory.Exists(installPath))
            {
                string existsMessage = String.Format("{0} is already installed in the specified location, do you want to overwrite this install?", product);
                string existsCaption = "EXISTS";
                MessageBoxButtons existsButtons = MessageBoxButtons.YesNo;
                MessageBoxIcon existsIcon = MessageBoxIcon.Warning;
                DialogResult existsResult;

                existsResult = MessageBox.Show(existsMessage, existsCaption, existsButtons, existsIcon);
                if (existsResult == DialogResult.Yes)
                {
                    Directory.Delete(installPath, true);
                }
                else
                {
                    return;
                }
            }
            this.Close();

            List<string> selectedExtendedModules = new List<string>();
            foreach (string dll in lbExtendedModules.SelectedItems)
            {
                selectedExtendedModules.Add(dll);
            }
            List<string> selectedCustomModules = new List<string>();
            foreach (string dll in lbCustomModules.SelectedItems)
            {
                selectedCustomModules.Add(dll);
            }

            Thread installBuild = new Thread(() => InstallBuild(installPath, selectedExtendedModules, selectedCustomModules, checkLaunchAfterInstall.Checked, checkInstallFolder.Checked, checkRunDatabaseUpdate.Checked, checkResetDBVersion.Checked));
            installBuild.Start();
            return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}