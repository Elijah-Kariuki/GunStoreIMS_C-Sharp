using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Federal Firearms Licensee (ATF Form 7).  
    /// Holds the core data you must record whenever you acquire from
    /// or dispose to another FFL (27 CFR § 478.123 & 125).
    /// </summary>
    public class FFL : IValidatableObject
    {
        // ─── Identity ──────────────────────────────────────────────────────────
        [Key] public int Id { get; set; }

        /// <summary>Business or trade name on the license.</summary>
        [Required, StringLength(120)]
        public string BusinessName { get; set; } = default!;

        /// <summary>FFL number in the canonical 3‑2‑5 format (e.g., 1‑23‑45678).</summary>
        [Required, StringLength(15)]
        [RegularExpression(@"^\d{1,3}-\d{2}-\d{5}$",
            ErrorMessage = "FFL number must be in #‑##‑##### format.")]
        public string FflNumber { get; set; } = default!;

        /// <summary>01 Dealer, 07 Manufacturer, 08 Importer, etc.</summary>
        [Required]
        public FflLicenseType LicenseType { get; set; }

        [Required]
        public DateTime ExpirationDateUtc { get; set; }

        // ─── Address ───────────────────────────────────────────────────────────
        [Required, StringLength(150)] public string AddressLine1 { get; set; } = default!;
        [StringLength(150)] public string? AddressLine2 { get; set; }
        [Required, StringLength(100)] public string City { get; set; } = default!;
        [Required] public USState State { get; set; }
        [Required, StringLength(15)] public string ZipCode { get; set; } = default!;

        // ─── Contact (optional but handy) ──────────────────────────────────────
        [Phone, StringLength(25)] public string? PhoneNumber { get; set; }
        [EmailAddress, StringLength(100)]
        public string? Email { get; set; }

        // ─── Navigation ────────────────────────────────────────────────────────
        public ICollection<Acquisition> Acquisitions { get; set; } = new List<Acquisition>();
        public ICollection<Disposition> Dispositions { get; set; } = new List<Disposition>();

        // ─── Validation ────────────────────────────────────────────────────────
        public IEnumerable<ValidationResult> Validate(ValidationContext _)
        {
            if (ExpirationDateUtc <= DateTime.UtcNow)
                yield return new ValidationResult(
                    "FFL license is expired.",
                    new[] { nameof(ExpirationDateUtc) });
        }
    }

    
}
