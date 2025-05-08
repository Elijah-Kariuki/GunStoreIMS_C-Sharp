// SerialNumberHistory.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Tracks any change or correction to the firearm’s serial number
    /// (ATF Ruling 79‑7; bound‑book accuracy).
    /// </summary>
    public class SerialNumberHistory : IValidatableObject
    {
        [Key] public Guid Id { get; set; }

        // ─── Link back to the firearm ─────────────────────────────────────────
        [Required] public Guid FirearmId { get; set; }
        [ForeignKey(nameof(FirearmId))] public Firearm Firearm { get; set; } = default!;

        // ─── Change details ───────────────────────────────────────────────────
        [Required, StringLength(50)]
        public string PreviousSerial { get; set; } = default!;

        [Required, StringLength(50)]
        public string NewSerial { get; set; } = default!;

        [Required] public DateTime ChangeDateUtc { get; set; }

        [Required] public SerialChangeReason Reason { get; set; }

        /// <summary>Optional supporting document (variance letter, etc.).</summary>
        public Guid? DocumentId { get; set; }
        [ForeignKey(nameof(DocumentId))] public Document? Document { get; set; }

        [StringLength(300)] public string? Notes { get; set; }

        // ─── Validation ───────────────────────────────────────────────────────
        public IEnumerable<ValidationResult> Validate(ValidationContext _)
        {
            if (PreviousSerial.Equals(NewSerial, StringComparison.OrdinalIgnoreCase))
                yield return Fail(nameof(NewSerial), "New serial must differ from previous serial.");

            if (NewSerial.Length < 3)
                yield return Fail(nameof(NewSerial), "Serial number must be at least 3 characters long.");
        }

        private static ValidationResult Fail(string member, string msg) => new(msg, new[] { member });
    }

}
