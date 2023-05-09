using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public class GPManagement
    {
        public static string gpInstallPath = @"C:\Program Files (x86)\Microsoft Dynamics\";
        public static string availableGPsPath = @"\\sp-fileserv-01\Shares\Autotesting\VM Setup\Microsoft Dynamics\";

        public static void LoadGPInsatlls(ListBox lb)
        {
            lb.Items.Clear();
            var gpFolderList = Directory.GetDirectories(gpInstallPath).Select(folder => folder.Remove(0, gpInstallPath.Length));
            foreach (string folder in gpFolderList)
            {
                if (folder != "Business Analyzer")
                    lb.Items.Add(folder);
            }
        }

        public static void LoadAvailableGPs(ComboBox cb)
        {
            cb.Items.Clear();
            cb.Text = "Select a GP Version to Install";
            try
            {
                cb.Items.AddRange(Utilities.GetFilesFromDirectoryByExtension(availableGPsPath, "zip"));
            }
            catch (Exception e)
            {
                cb.Text = Constants.CouldNotConnect;
                ErrorHandling.LogException(e, false, "There were errors connecting to the network.");
                //ErrorHandling.DisplayExceptionMessage(e, false, "There were errors connecting to the network.");
            }
        }

        public static void LaunchGP(string gp)
        {
            //Do nothing if no GP install is selected
            if (String.IsNullOrWhiteSpace(gp))
                return;

            //Open the install folder for the selected GP install if shift-clicked
            if (Control.ModifierKeys == Keys.Shift)
            {
                try
                {
                    Process.Start(String.Format("{0}{1}", gpInstallPath, gp));
                }
                catch (Exception e)
                {
                    ErrorHandling.LogException(e);
                    ErrorHandling.DisplayExceptionMessage(e);
                }
            }
            else
                Process.Start(
                    String.Format(@"{0}{1}\Dynamics.exe", gpInstallPath, gp),
                    String.Format(@"""{0}{1}\DYNAMICS.SET""", gpInstallPath, gp)
                    );
        }

        public static void LaunchGPUtilities(string gp)
        {
            //Do nothing if no GP install is selected
            if (String.IsNullOrWhiteSpace(gp))
                return;

            Process.Start(
                String.Format(@"{0}{1}\DynUtils.exe", gpInstallPath, gp),
                String.Format(@"""{0}{1}\DYNUTILS.SET""", gpInstallPath, gp)
                );
        }

        public static void DeleteGPInstall(string path)
        {
            Directory.Delete(path, true);
        }

        public static void InstallGP(string selectedGP)
        {
            Form1.EnableGPInstallButton(false);

            //Build paths
            string newGP = String.Format("{0}{1}.zip", availableGPsPath, selectedGP);
            string newPath = String.Format(@"{0}\{1}.zip", Utilities.GetFolder("DLLs"), selectedGP);
            string destination = String.Format("{0}{1}", gpInstallPath, selectedGP);

            //Copy the zipped build to the DLLs directory to extract
            File.Copy(newGP, newPath);

            //unzip the file to the dynamics path
            using (ZipArchive zip = ZipFile.Open(newPath, ZipArchiveMode.Read))
            {
                try
                {
                    zip.ExtractToDirectory(destination);
                }
                catch (Exception e)
                {
                    ErrorHandling.DisplayExceptionMessage(e);
                    ErrorHandling.LogException(e);
                    zip.Dispose();
                    File.Delete(newPath);
                    Form1.EnableGPInstallButton(true);
                    return;
                }
            }

            //reload the gp path listbox
            Form1.EnableGPInstallButton(true);

            //delete the zipped file
            File.Delete(newPath);
        }
    }
}
