namespace EnvironmentManager4
{
    public class CoreModules
    {
        public static string[] GetCoreModulesByProduct(string product)
        {
            string[] coreModules = new string[] { };
            switch (product)
            {
                case Products.SalesPad:
                    return salesPadCoreModules;
                case Products.DataCollection:
                    return dataCollectionCoreModules;
                case Products.SalesPadMobile:
                    return salesPadMobileCoreModules;
                case Products.ShipCenter:
                    return shipCenterCoreModules;
            }
            return coreModules;
        }

        private static string[] salesPadCoreModules = {
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

        private static string[] dataCollectionCoreModules = {
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

        private static string[] salesPadMobileCoreModules = {
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

        private static string[] shipCenterCoreModules = {
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
    }
}
