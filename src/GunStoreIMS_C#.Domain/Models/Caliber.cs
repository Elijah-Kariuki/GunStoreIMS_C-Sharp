using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Represents a cartridge designation or shotgun gauge.
    /// Provides enough data to validate ATF entries and to drive reports.
    /// </summary>
    public class Caliber : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Official designation, e.g., "9mm Luger", ".223 Remington", "12 Gauge".
        /// </summary>
        [Required, StringLength(100)]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Nominal bullet diameter in millimetres (optional).
        /// </summary>
        [Range(1, 30)]
        public decimal? DiameterMm { get; set; }

        /// <summary>
        /// Nominal bullet diameter in inches (optional).
        /// </summary>
        [Range(0.02, 1.5)]
        public decimal? DiameterInches { get; set; }

        /// <summary>
        /// Case length in millimetres (for cartridge), or shell length for gauge.
        /// </summary>
        [Range(1, 150)]
        public decimal? CaseLengthMm { get; set; }

        /// <summary>
        /// SAAMI or CIP official short code, e.g., "9x19", "223 Rem".
        /// </summary>
        [StringLength(20)]
        public string? StandardCode { get; set; }

        /// <summary>
        /// True for shotgun gauges (12 Ga, 20 Ga) where diameter is not used.
        /// </summary>
        public bool IsGauge { get; set; }

        /// <summary>
        /// Navigation – firearms chambered for this caliber.
        /// </summary>
        public virtual ICollection<Firearm> Firearms { get; set; } = new List<Firearm>();

        // Ensure uniqueness & sanity ------------------------------------------------------
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsGauge && Name?.Contains("Gauge", StringComparison.OrdinalIgnoreCase) == false)
            {
                yield return new ValidationResult(
                    "Gauge designations should include the word 'Gauge' (e.g., '12 Gauge').",
                    new[] { nameof(Name) });
            }

            if (!IsGauge && string.IsNullOrWhiteSpace(StandardCode))
            {
                yield return new ValidationResult(
                    "Cartridge calibers should include a standard code (e.g., '9x19').",
                    new[] { nameof(StandardCode) });
            }
        }
    }

}
