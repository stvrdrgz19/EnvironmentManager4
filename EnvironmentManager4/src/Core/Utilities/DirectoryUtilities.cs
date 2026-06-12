using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4.src.Core.Utilities
{
    public class DirectoryUtilities
    {
        public const string debugPath = @"C:\Users\steve.rodriguez\source\repos\EnvironmentManager4\EnvironmentManager4\bin\Debug";
        public const string localPath = @"C:\Program Files (x86)\Environment Manager";

        public static bool DevEnvironment()
        {
            return Environment.CurrentDirectory == debugPath ? true : false;
        }

        public static string GetFile(string fileName)
        {
            return DevEnvironment() ? String.Format(@"{0}\Files\{1}", localPath, fileName) : String.Format(@"{0}\Files\{1}", Environment.CurrentDirectory, fileName);
        }

        public static string GetFolder(string folderName)
        {
            return DevEnvironment() ? String.Format(@"{0}\{1}", localPath, folderName) : String.Format(@"{0}\{1}", Environment.CurrentDirectory, folderName);
        }

        public static string GetCurrentDirectory()
        {
            return DevEnvironment() ? localPath : Environment.CurrentDirectory;
        }

        public static string GetDirectory(string selectedPath = @"C:\")
        {
            using (FolderBrowserDialog folderBrowser = new FolderBrowserDialog())
            {
                folderBrowser.SelectedPath = selectedPath;
                return folderBrowser.ShowDialog() == DialogResult.OK ? folderBrowser.SelectedPath : selectedPath;
            }
        }
    }
}
