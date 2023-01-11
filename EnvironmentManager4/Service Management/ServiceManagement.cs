using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public class ServiceManagement
    {
        public static void EnableSQLControls(bool tf, Button start, Button stop, Button stopAll, Button install)
        {
            ToggleButton(tf, start);
            ToggleButton(tf, stop);
            ToggleButton(tf, stopAll);
            //ToggleButton(tf, install);
        }

        public static void ToggleButton(bool tf, Button btn)
        {
            btn.Enabled = tf;
        }

        public static void PopulateSQLServerList(ListView lv, List<ListViewProperties> lvp)
        {
            lv.Items.Clear();
            ListViewProperties.UpdateListViewProperties(lvp);

            List<string> services = InstalledSQLServerInstanceNames();
            services.AddRange(GetSalesPadServices());
            string serverStatus = "";
            foreach (string service in services)
            {
                string serviceName = service;
                //Format as SQL service if it is a SQL Service
                if (IsSQLService(service))
                    serviceName = String.Format("{0}{1}", "MSSQL$", serviceName);

                bool status = IsServiceRunning(serviceName);
                ListViewItem item = new ListViewItem(service);
                switch (status)
                {
                    case true:
                        item.ForeColor = Color.Green;
                        item.Font = new Font(item.Font, FontStyle.Bold);
                        serverStatus = "RUNNING";
                        break;
                    case false:
                        item.ForeColor = Color.Gray;
                        item.Font = new Font(item.Font, FontStyle.Italic);
                        serverStatus = "NOT RUNNING";
                        break;
                }
                item.SubItems.Add(serverStatus);
                lv.Items.Add(item);
            }
            Utilities.ResizeListViewColumnWidthForScrollBar(lv, 6, 0);
        }

        public static bool IsSQLService(string service)
        {
            return service.Contains("SQL") ? true: false;
        }

        public static bool IsServiceRunning(string service)
        {
            ServiceController selectedService = new ServiceController(service);
            try
            {
                return selectedService.Status.Equals(ServiceControllerStatus.Running) ? true : false;
            }
            catch
            {
                return false;
            }
        }

        public static List<string> InstalledSQLServerInstanceNames()
        {
            List<string> sqlServerList = new List<string>();
            RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
            using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
            {
                RegistryKey instanceKey = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", false);
                if (instanceKey != null)
                    sqlServerList.AddRange(instanceKey.GetValueNames());
            }
            Environment.SpecialFolder.ApplicationData.ToString();
            return sqlServerList;
        }
        public static List<string> GetSalesPadServices()
        {
            List<string> serverList = new List<string>();
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController service in services)
                if (service.DisplayName.Contains("SalesPad"))
                    serverList.Add(service.DisplayName);
            return serverList;
        }

        public static void UpdateServices(string status, ListView lv, List<ListViewProperties> lvp)
        {
            for (int i = 0; i < lv.SelectedItems.Count; i++)
            {
                string service = lv.SelectedItems[i].Text;
                switch (status)
                {
                    case "Start":
                        StartService(service);
                        break;
                    case "Stop":
                        StopService(service);
                        break;
                    case "Restart":
                        StopService(service);
                        StartService(service);
                        break;
                }
            }
            PopulateSQLServerList(lv, lvp);
        }

        public static void StartService(string service)
        {
            //Do nothing if there are no services selected
            if (String.IsNullOrWhiteSpace(service))
                return;

            //Format as SQL service if it is a SQL Service
            if (IsSQLService(service))
                service = String.Format("{0}{1}", "MSSQL$", service);

            bool status = IsServiceRunning(service);
            //Start the service if it isn't running
            if (!status)
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
                    ErrorHandling.LogException(e);
                    ErrorHandling.DisplayExceptionMessage(e);
                }
            }
        }

        public static void StopService(string service)
        {
            //Do nothing if there are no services selected
            if (String.IsNullOrWhiteSpace(service))
                return;

            //Format as SQL service if it is a SQL Service
            if (IsSQLService(service))
                service = String.Format("{0}{1}", "MSSQL$", service);

            bool status = IsServiceRunning(service);
            //Stop the service if it is running
            if (status)
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
                    ErrorHandling.LogException(e);
                    ErrorHandling.DisplayExceptionMessage(e);
                }
            }
        }
    }
}
