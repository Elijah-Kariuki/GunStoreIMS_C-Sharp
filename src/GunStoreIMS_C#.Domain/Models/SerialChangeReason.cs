namespace GunStoreIMS.Domain.Models
{
    public enum SerialChangeReason
    {
        ClericalCorrection,
        ATFVariance,          // approved variance / re‑marking
        ManufacturerRecall,
        ReplacementReceiver,
        Other
    }
}
