using System;
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Domain.Models
{
    public abstract class NfaItem : Firearm
    {
        // No need to flip IsNFAItem here; the Type setter will do it
        protected NfaItem() { }

        [Required, StringLength(30)]
        public string NfaRegistryId { get; set; } = default!;

        [Required]
        public DateTime NfaRegistrationDateUtc { get; set; }

        // e.g. Form1, Form2, Form3, Form4, Form5
        [Required, StringLength(10)]
        public string RegistrationFormType { get; set; } = default!;

        [StringLength(40)]
        public string? TaxStampNumber { get; set; }
    }
}
