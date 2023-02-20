using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public class TestClass
    {
        public string Field1 { get; set; }
        public string Field2 { get; set; }

        public static SettingsModel settings = SettingsUtilities.GetSettings();

        public static TestClass tc = new TestClass()
        {
            Field1 = SettingsUtilities.GetSettings().DbManagement.DatabaseBackupDirectory,
            Field2 = "Class"
        };

        public void GetTestClassValues()
        {
            MessageBox.Show(this.Field1);
        }
    }
}
