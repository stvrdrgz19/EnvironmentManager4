using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public class GPManagement
    {
        public static string gpInstallPath = @"C:\Program Files (x86)\Microsoft Dynamics\";

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
    }
}
