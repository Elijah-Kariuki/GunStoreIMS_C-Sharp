// Silencer.cs
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// 26 U.S.C. § 5845(a)(7) — any device for silencing, muffling,
    /// or diminishing the report of a portable firearm.
    /// </summary>
    public sealed class Silencer : NfaItem
    {
        public Silencer()
        {
            Type = FirearmType.Silencer;   // the ONLY classification we need
        }

        [Range(1, 40)]
        public decimal OverallLengthInches { get; set; }
    }
}
