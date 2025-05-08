using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Domain.Models
{
    // ───────────────────────────── Disposition Class ───────────────────────────

    /// <summary>
    /// Records the outbound transfer of a firearm from the licensee.
    /// Includes Form 4473 Serial Number when disposition is to a non‑FFL transferee.
    /// </summary>
    public class Disposition : IValidatableObject
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid FirearmId { get; set; }

        [ForeignKey(nameof(FirearmId))]
        public virtual Firearm Firearm { get; set; } = default!;

        [Required]
        public DateTime DateUtc { get; set; }

        // Transferee --------------------------------------------------------------------
        [Required, StringLength(150)]
        public string TransfereeName { get; set; } = default!;

        [StringLength(20)]
        public string? TransfereeFFLNumber { get; set; }

        [StringLength(150)] public string? TransfereeAddressLine1 { get; set; }
        [StringLength(150)] public string? TransfereeAddressLine2 { get; set; }
        [StringLength(100)] public string? TransfereeCity { get; set; }
        public USState? TransfereeState { get; set; }
        [StringLength(15)] public string? TransfereeZip { get; set; }

        // Form 4473 ---------------------------------------------------------------------
        [StringLength(50)]
        public string? Form4473SerialNumber { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool hasAddress = !string.IsNullOrWhiteSpace(TransfereeAddressLine1) &&
                              !string.IsNullOrWhiteSpace(TransfereeCity) &&
                              TransfereeState.HasValue &&
                              !string.IsNullOrWhiteSpace(TransfereeZip);

            if (string.IsNullOrWhiteSpace(TransfereeFFLNumber) && !hasAddress)
            {
                yield return new ValidationResult(
                    "Either the Transferee FFL# or full address must be provided.",
                    new[] { nameof(TransfereeFFLNumber) });
            }

            // Non‑FFL transferees require Form 4473 Serial Number
            if (string.IsNullOrWhiteSpace(TransfereeFFLNumber) && string.IsNullOrWhiteSpace(Form4473SerialNumber))
            {
                yield return new ValidationResult(
                    "Form 4473 Serial Number is required for dispositions to non‑FFL transferees.",
                    new[] { nameof(Form4473SerialNumber) });
            }
        }
    }

}
