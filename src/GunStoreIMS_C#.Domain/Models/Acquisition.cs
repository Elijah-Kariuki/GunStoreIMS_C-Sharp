using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Domain.Models
{
    // ───────────────────────────── Acquisition Class ───────────────────────────

    /// <summary>
    /// Records the inbound transfer of a firearm to the licensee.
    /// Must capture the name *and* either the address or FFL# of the transferor.
    /// </summary>
    public class Acquisition : IValidatableObject
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid FirearmId { get; set; }

        [ForeignKey(nameof(FirearmId))]
        public virtual Firearm Firearm { get; set; } = default!;

        [Required]
        public DateTime DateUtc { get; set; }

        // Transferor --------------------------------------------------------------------
        [Required, StringLength(150)]
        public string TransferorName { get; set; } = default!;

        // Either FFL number *or* full address must be provided
        [StringLength(20)]
        public string? TransferorFFLNumber { get; set; }

        [StringLength(150)] public string? TransferorAddressLine1 { get; set; }
        [StringLength(150)] public string? TransferorAddressLine2 { get; set; }
        [StringLength(100)] public string? TransferorCity { get; set; }
        public USState? TransferorState { get; set; }
        [StringLength(15)] public string? TransferorZip { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool hasAddress = !string.IsNullOrWhiteSpace(TransferorAddressLine1) &&
                              !string.IsNullOrWhiteSpace(TransferorCity) &&
                              TransferorState.HasValue &&
                              !string.IsNullOrWhiteSpace(TransferorZip);

            if (string.IsNullOrWhiteSpace(TransferorFFLNumber) && !hasAddress)
            {
                yield return new ValidationResult(
                    "Either the Transferor FFL# or full address must be provided.",
                    new[] { nameof(TransferorFFLNumber) });
            }
        }
    }

}
