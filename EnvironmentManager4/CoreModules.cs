using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public class CoreModules
    {
        public string Product { get; set; }
        public ProductKey Key { get; set; }
        public List<string> CoreModulesList { get; set; }
        public enum ProductKey
        {
            SalesPad,
            DataCollection,
            SalesPadMobile,
            ShipCenter
        }

        private const int Version = 1;

        public CoreModules(string Product, ProductKey Key, List<string> CoreModulesList)
        {
            this.Product = Product;
            this.Key = Key;
            this.CoreModulesList = CoreModulesList;
        }

        public static void UpdateCoreModulesFile()
        {
            string coreModulesFile = Utilities.GetFile("CoreModules.json");
            if (!File.Exists(coreModulesFile))
            {
                GenerateCoreModulesFile();
                return;
            }
            List<CoreModules> newCoreModules = new List<CoreModules>();
            List<CoreModules> coreModulesList = JsonConvert.DeserializeObject<List<CoreModules>>(File.ReadAllText(coreModulesFile));
            foreach (CoreModules moduleList in coreModulesList)
            {
                switch (moduleList.Key)
                {
                    case ProductKey.SalesPad:
                        moduleList.Product = Products.SalesPad;
                        break;
                    case ProductKey.DataCollection:
                        moduleList.Product = Products.DataCollection;
                        break;
                    case ProductKey.SalesPadMobile:
                        moduleList.Product = Products.SalesPadMobile;
                        break;
                    case ProductKey.ShipCenter:
                        moduleList.Product = Products.ShipCenter;
                        break;
                }
                newCoreModules.Add(moduleList);
            }
            string json = JsonConvert.SerializeObject(newCoreModules, Formatting.Indented);
            File.WriteAllText(String.Format(@"{0}\Files\CoreModules.json", Environment.CurrentDirectory), json);
        }

        public static string[] GetCoreModules(string product)
        {
            string coreModulesFile = Utilities.GetFile("CoreModules.json");
            if (!File.Exists(coreModulesFile))
                GenerateCoreModulesFile();

            List<CoreModules> coreModulesList = JsonConvert.DeserializeObject<List<CoreModules>>(File.ReadAllText(coreModulesFile));
            List<string> modulesRequested = new List<string>();
            foreach (CoreModules moduleList in coreModulesList)
            {
                if (moduleList.Product == product)
                    modulesRequested.AddRange(moduleList.CoreModulesList);
            }
            return modulesRequested.ToArray();
        }

        public static void GenerateCoreModulesFile()
        {
            List<string> salesPadCoreModules = new List<string>
            {
                "SalesPad.Module.App.dll",
                "SalesPad.Module.ARTransactionEntry.dll",
                "SalesPad.Module.AvaTax.dll",
                "SalesPad.Module.Ccp.dll",
                "SalesPad.Module.CRM.dll",
                "SalesPad.Module.Dashboard.dll",
                "SalesPad.Module.DistributionBOM.dll",
                "SalesPad.Module.DocumentManagement.dll",
                "SalesPad.Module.EquipmentManagement.dll",
                "SalesPad.Module.FedExServiceManager.dll",
                "SalesPad.Module.GP2010.dll",
                "SalesPad.Module.GP2010SP2.dll",
                "SalesPad.Module.GP2013.dll",
                "SalesPad.Module.GP2013R2.dll",
                "SalesPad.Module.GP2015.dll",
                "SalesPad.Module.Inventory.dll",
                "SalesPad.Module.NodusPayFabric.dll",
                "SalesPad.Module.Printing.dll",
                "SalesPad.Module.Purchasing.dll",
                "SalesPad.Module.QuickReports.dll",
                "SalesPad.Module.Reporting.dll",
                "SalesPad.Module.ReturnsManagement.dll",
                "SalesPad.Module.Sales.dll",
                "SalesPad.Module.SalesEntryQuickPick.dll",
                "SalesPad.Module.SignaturePad.dll",
                "SalesPad.Module.SquareIntegration.dll"
            };

            List<string> dataCollectionCoreModules = new List<string>
            {
                "SalesPad.DataCollection.Bus.dll",
                "SalesPad.DataCollection.Common.dll",
                "SalesPad.DataCollection.Inventory.dll",
                "SalesPad.DataCollection.Localization.dll",
                "SalesPad.DataCollection.Manufacturing.dll",
                "SalesPad.DataCollection.Printing.dll",
                "SalesPad.DataCollection.Purchasing.dll",
                "SalesPad.DataCollection.Sales.dll",
                "SalesPad.DataCollection.Service.dll",
                "SalesPad.DataCollection.Service.Winforms.dll",
                "SalesPad.DataCollection.WMS.dll"
            };

            List<string> salesPadMobileCoreModules = new List<string>
            {
                "SalesPad.Mobile.Bus.dll",
                "SalesPad.Mobile.Contracts.dll",
                "SalesPad.Mobile.Contracts.Tests.dll",
                "SalesPad.Mobile.Localization.dll",
                "SalesPad.Mobile.Scripts.dll",
                "SalesPad.Mobile.SharedConstants.dll",
                "SalesPad.Module.AvaTax.dll",
                "SalesPad.Module.BinaryStream.dll",
                "SalesPad.Module.Ccp.dll",
                "SalesPad.Module.Dispatch.dll",
                "SalesPad.Module.GP2010.dll",
                "SalesPad.Module.GP2010SP2.dll",
                "SalesPad.Module.GP2013.dll",
                "SalesPad.Module.GP2013R2.dll",
                "SalesPad.Module.GP2015.dll",
                "SalesPad.Module.NodusPayFabric.dll",
                "SalesPad.Module.Printing.dll",
                "SalesPad.Module.QuickReports.dll",
                "SalesPad.Module.Sales.dll",
                "SalesPad.Module.SignaturePad.dll"
            };

            List<string> shipCenterCoreModules = new List<string>
            {
                "SalesPad.Module.App.dll",
                "SalesPad.Module.AvaTax.dll",
                "SalesPad.Module.Ccp.dll",
                "SalesPad.Module.CRM.dll",
                "SalesPad.Module.DocumentManagement.dll",
                "SalesPad.Module.GP2010.dll",
                "SalesPad.Module.GP2013.dll",
                "SalesPad.Module.GP2013R2.dll",
                "SalesPad.Module.Inventory.dll",
                "SalesPad.Module.NodusPayFabric.dll",
                "SalesPad.Module.Printing.dll",
                "SalesPad.Module.Purchasing.dll",
                "SalesPad.Module.QuickReports.dll",
                "SalesPad.Module.Reporting.dll",
                "SalesPad.Module.Sales.dll",
                "SalesPad.Module.SalesPadEDI.dll",
                "SalesPad.Module.ThomsonReuters.dll",
                "SalesPad.ShipCenter.API.dll",
                "SalesPad.ShipCenter.Bus.dll",
                "SalesPad.ShipCenter.CanadaPost.dll",
                "SalesPad.ShipCenter.CanadaPost.Windows.dll",
                "SalesPad.ShipCenter.DHLExpress.dll",
                "SalesPad.ShipCenter.DHLExpress.Windows.dll",
                "SalesPad.ShipCenter.FedEx.dll",
                "SalesPad.ShipCenter.FedEx.WebServices.dll",
                "SalesPad.ShipCenter.FedEx.Windows.dll",
                "SalesPad.ShipCenter.GSO.dll",
                "SalesPad.ShipCenter.GSO.WebService.dll",
                "SalesPad.ShipCenter.GSO.Windows.dll",
                "SalesPad.ShipCenter.Integrator.GP.dll",
                "SalesPad.ShipCenter.Integrator.SalesPadRemoteLibrary.dll",
                "SalesPad.ShipCenter.Localization.dll",
                "SalesPad.ShipCenter.Purolator.dll",
                "SalesPad.ShipCenter.Purolator.WebService.dll",
                "SalesPad.ShipCenter.Purolator.Windows.dll",
                "SalesPad.ShipCenter.RandL.dll",
                "SalesPad.ShipCenter.RandL.WebServices.dll",
                "SalesPad.ShipCenter.RandL.Windows.dll",
                "SalesPad.ShipCenter.RIST.dll",
                "SalesPad.ShipCenter.RIST.Windows.dll",
                "SalesPad.ShipCenter.SAIA.dll",
                "SalesPad.ShipCenter.SAIA.WebService.dll",
                "SalesPad.ShipCenter.SAIA.Windows.dll",
                "SalesPad.ShipCenter.Service.dll",
                "SalesPad.ShipCenter.Stamps.dll",
                "SalesPad.ShipCenter.Stamps.WebServices.dll",
                "SalesPad.ShipCenter.Stamps.Windows.dll",
                "SalesPad.ShipCenter.Ups.dll",
                "SalesPad.ShipCenter.Ups.Windows.dll",
                "SalesPad.ShipCenter.USFHolland.dll",
                "SalesPad.ShipCenter.USFHolland.WebService.dll",
                "SalesPad.ShipCenter.USFHolland.Windows.dll",
                "SalesPad.ShipCenter.WardTrucking.dll",
                "SalesPad.ShipCenter.WardTrucking.Windows.dll",
                "SalesPad.ShipCenter.Wizard.dll",
                "SalesPad.ShipCenter.Xpo.dll",
                "SalesPad.ShipCenter.Xpo.WebServices.dll",
                "SalesPad.ShipCenter.Xpo.Windows.dll",
                "SalesPad.ShipCenter.YRC.dll",
                "SalesPad.ShipCenter.YRC.WebService.dll",
                "SalesPad.ShipCenter.YRC.Windows.dll"
            };

            List<CoreModules> coreModules = new List<CoreModules>
            {
                new CoreModules(Products.SalesPad, ProductKey.SalesPad, salesPadCoreModules),
                new CoreModules(Products.DataCollection, ProductKey.DataCollection, dataCollectionCoreModules),
                new CoreModules(Products.SalesPadMobile, ProductKey.SalesPadMobile, salesPadMobileCoreModules),
                new CoreModules(Products.ShipCenter, ProductKey.ShipCenter, shipCenterCoreModules)
            };

            CoreModulesFile cmFile = new CoreModulesFile();
            cmFile.Version = Version;
            cmFile.CoreModulesList = coreModules;

            string json = JsonConvert.SerializeObject(cmFile, Formatting.Indented);
            File.WriteAllText(String.Format(@"{0}\Files\CoreModules.json", Environment.CurrentDirectory), json);
        }
    }

    public class CoreModulesFile
    {
        public int Version { get; set; }
        public List<CoreModules> CoreModulesList { get; set; }
    }
}
