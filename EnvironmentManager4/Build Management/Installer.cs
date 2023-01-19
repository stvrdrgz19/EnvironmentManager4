using System;
using System.IO;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public class Installer
    {
        public string Executable { get; set; }
        public string Product { get; set; }
        public string Version { get; set; }

        /// <summary>
        /// Prompts the useer to select an exe based off of Product/Version. Returns Installer Object (exe path, product, version)
        /// </summary>
        /// <param name="path">Pulled from the clipboard, attempts to detect target build if a path is copied</param>
        /// <param name="product">SalesPad Product selected (SalesPad, Inventory Manager, etc)</param>
        /// <param name="version">Version of Product (x86/x64/Pre, only applicable to SalesPad atm)</param>
        /// <returns></returns>
        public static Installer GetInstallerFile(string path, string product, string version)
        {
            //Retrieve information based on the selected product
            ProductInfo pi = ProductInfo.GetProductInfo(product, version);
            Installer installer = new Installer();
            string initialDir = pi.FileserverDirectory;
            installer.Product = product;
            installer.Version = version;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (product == Products.SalesPad)
                {
                    switch (version)
                    {
                        case "x64":
                        case "x86":
                            openFileDialog.Filter = String.Format("Executable Files (*.exe)|*{0}.exe", version);
                            break;
                        case "Pre":
                            openFileDialog.Filter = "Executable Files (*.exe)|*.exe";
                            break;
                    }
                }
                else if (product == Products.GPWeb)
                {
                    openFileDialog.Filter = "ZIP Folder (.zip)|*.zip";
                }
                else if (product == Products.WebAPI)
                {
                    openFileDialog.Filter = "Windows Installer Package (.msi)|*.msi";
                }
                else
                {
                    openFileDialog.Filter = "Executable Files (*.exe)|*.exe";
                }
                if (!Directory.Exists(path))
                {
                    string newPath = String.Format(@"{0}{1}", "\\", path);
                    if (Directory.Exists(newPath))
                    {
                        if (newPath.Contains(initialDir))
                            openFileDialog.InitialDirectory = newPath;
                        else
                            openFileDialog.InitialDirectory = initialDir;
                    }
                    else
                        openFileDialog.InitialDirectory = initialDir;
                }
                else
                {
                    if (path.Contains(initialDir))
                        openFileDialog.InitialDirectory = path;
                    else
                        openFileDialog.InitialDirectory = initialDir;
                }

                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = String.Format("Installing {0}", product);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    installer.Executable = openFileDialog.FileName;
                }
                else
                    installer.Executable = "EXIT";
            }
            return installer;
        }
    }
}
