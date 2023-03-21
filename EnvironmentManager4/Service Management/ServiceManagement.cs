using EnvironmentManager4.Service_Management;
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
            List<SQLServiceList> services = SQLServiceList.GetSQLServices();
            services.AddRange(SQLServiceList.GetSalesPadServices());
            foreach (SQLServiceList service in services)
            {
                bool status = IsServiceRunning(service.Name);
                ListViewItem item = new ListViewItem(service.DisplayNameTrimmed);
                switch (status)
                {
                    case true:
                        item.ForeColor = Color.Green;
                        item.Font = new Font(item.Font, FontStyle.Bold);
                        service.ServiceStatus = "RUNNING";
                        break;
                    case false:
                        item.ForeColor = Color.Gray;
                        item.Font = new Font(item.Font, FontStyle.Italic);
                        service.ServiceStatus = "NOT RUNNING";
                        break;
                }
                item.SubItems.Add(service.ServiceStatus);
                lv.Items.Add(item);
            }
            Utilities.ResizeListViewColumnWidthForScrollBar(lv, 6, 0);
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

            string serviceName = SQLServiceList.GetServiceName(service);
            bool status = IsServiceRunning(serviceName);
            //Start the service if it isn't running
            if (!status)
            {
                ServiceController serviceToStart = new ServiceController(serviceName);
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

            string serviceName = SQLServiceList.GetServiceName(service);
            bool status = IsServiceRunning(serviceName);
            //Stop the service if it is running
            if (status)
            {
                ServiceController serviceToStart = new ServiceController(serviceName);
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
