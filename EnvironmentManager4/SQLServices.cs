using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public class SQLServices
    {
        public static List<string> InstalledSQLServerInstanceNames()
        {
            List<string> sqlServerList = new List<string>();
            RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
            using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
            {
                RegistryKey instanceKey = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", false);
                if (instanceKey != null)
                {
                    sqlServerList.AddRange(instanceKey.GetValueNames());
                }
            }
            Environment.SpecialFolder.ApplicationData.ToString();
            return sqlServerList;
        }

        public static bool IsServiceRunning(string serviceName)
        {
            bool status = false;
            ServiceController selectedService = new ServiceController(serviceName);
            try
            {
                if (selectedService.Status.Equals(ServiceControllerStatus.Running))
                {
                    status = true;
                }
            }
            catch
            {
                status = false;
            }
            return status;
        }

        public static string FormatServiceName(string name)
        {
            return String.Format("{0}{1}", "MSSQL$", name);
        }

        public static void StartSQLServer(string service)
        {
            ServiceController serviceToStart = new ServiceController(service);
            try
            {
                if (serviceToStart.Status.Equals(ServiceControllerStatus.Stopped))
                {
                    serviceToStart.Start();
                    serviceToStart.WaitForStatus(ServiceControllerStatus.Running);
                }
            }
            catch (Exception e)
            {
                string message = e.ToString();
                string caption = "ERROR";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;

                MessageBox.Show(message, caption, buttons, icon);
            }
        }

        public static void StopSQLServer(string service)
        {
            ServiceController serviceToStart = new ServiceController(service);
            try
            {
                if (serviceToStart.Status.Equals(ServiceControllerStatus.Running))
                {
                    serviceToStart.Stop();
                    serviceToStart.WaitForStatus(ServiceControllerStatus.Stopped);
                }
            }
            catch (Exception e)
            {
                string message = e.ToString();
                string caption = "ERROR";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;

                MessageBox.Show(message, caption, buttons, icon);
            }
        }

        public static List<string> GetSalesPadServices()
        {
            List<string> serverList = new List<string>();
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController service in services)
            {
                if (service.DisplayName.Contains("SalesPad"))
                {
                    serverList.Add(service.DisplayName);
                }
            }
            return serverList;
        }

        public static bool IsSQLService(string serviceName)
        {
            bool tf = false;
            if (serviceName.Contains("SQL"))
                tf = true;
            return tf;
        }
    }
}
