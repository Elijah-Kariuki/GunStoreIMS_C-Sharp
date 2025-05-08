namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Common FFL license codes from ATF Form 7.
    /// Expand as needed for your operation.
    /// </summary>
    public enum FflLicenseType
    {
        _01_Dealer_Gunsmith = 1,
        _02_Pawnbroker,
        _03_Collector_CuriosRelics,
        _06_Manufacturer_Ammunition,
        _07_Manufacturer_Firearms,
        _08_Importer_Firearms,
        _09_Dealer_DestructiveDevices,
        _10_Manufacturer_DestructiveDevices,
        _11_Importer_DestructiveDevices
    }
}
