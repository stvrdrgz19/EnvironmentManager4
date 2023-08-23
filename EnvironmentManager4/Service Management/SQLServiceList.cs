using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4.Service_Management
{
    public class SQLServiceList
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string DisplayNameTrimmed { get; set; }
        public string ServiceStatus { get; set; }

        public static string TrimDisplayName(string displayName)
        {
            return displayName.Split('(', ')')[1];
        }

        public static List<SQLServiceList> GetSQLServices()
        {
            List<SQLServiceList> sQLServiceLists = new List<SQLServiceList>();
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController service in services)
                if (service.ServiceName.Contains("MSSQL$") && !service.ServiceName.Contains("Agent") && !service.ServiceName.Contains("TELEMETRY"))
                {
                    SQLServiceList s = new SQLServiceList();
                    s.Name = service.ServiceName;
                    s.DisplayName = service.DisplayName;
                    s.DisplayNameTrimmed = TrimDisplayName(service.DisplayName);
                    sQLServiceLists.Add(s);
                }
            return sQLServiceLists;
        }

        public static List<SQLServiceList> GetSalesPadServices()
        {
            List<SQLServiceList> sQLServiceLists = new List<SQLServiceList>();
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController service in services)
                if (service.ServiceName.Contains("SalesPad"))
                {
                    SQLServiceList s = new SQLServiceList();
                    s.Name = service.ServiceName;
                    s.DisplayName = service.DisplayName;
                    s.DisplayNameTrimmed = service.DisplayName;
                    sQLServiceLists.Add(s);
                }
            return sQLServiceLists;
        }

        public static bool IsServiceRunning(string serviceName)
        {
            ServiceController selectedService = new ServiceController(serviceName);
            try
            {
                return selectedService.Status.Equals(ServiceControllerStatus.Running) ? true : false;
            }
            catch
            {
                return false;
            }
        }

        public static string GetServiceName(string serviceDisplayName)
        {
            List<SQLServiceList> serviceList = GetSQLServices();
            serviceList.AddRange(GetSalesPadServices());

            string serviceName = "";

            foreach (SQLServiceList service in serviceList)
                if (service.DisplayNameTrimmed == serviceDisplayName)
                    serviceName = service.Name;

            return serviceName;
        }

        public static string GetServiceFromConnection(string connection)
        {
            string serviceName = "";
            List<SQLServiceList> sQLServiceLists = GetSQLServices();
            foreach (SQLServiceList service in sQLServiceLists)
                if (connection.Contains(service.DisplayNameTrimmed))
                    serviceName = service.DisplayNameTrimmed;
            return serviceName;
        }
    }
}
