// Recovery.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Records a loss, theft, seizure, or subsequent recovery of a firearm
    /// (27 CFR § 478.39a & ATF P 3317.7).
    /// </summary>
    public class Recovery : IValidatableObject
    {
        [Key] public Guid Id { get; set; }

        // ─── Link back to the firearm ─────────────────────────────────────────
        [Required] public Guid FirearmId { get; set; }
        [ForeignKey(nameof(FirearmId))] public Firearm Firearm { get; set; } = default!;

        // ─── Event details ────────────────────────────────────────────────────
        [Required] public RecoveryEventType EventType { get; set; }

        /// <summary>Date/time the event occurred (UTC).</summary>
        [Required] public DateTime EventDateUtc { get; set; }

        /// <summary>Law‑enforcement agency involved, if any.</summary>
        [StringLength(120)] public string? Agency { get; set; }

        /// <summary>Police case #, ATF incident #, etc.</summary>
        [StringLength(50)] public string? ReportNumber { get; set; }

        [StringLength(500)] public string? Notes { get; set; }

        // ─── Validation ───────────────────────────────────────────────────────
        public IEnumerable<ValidationResult> Validate(ValidationContext _)
        {
            bool leRequired = EventType is RecoveryEventType.Stolen
                                           or RecoveryEventType.Lost
                                           or RecoveryEventType.Seized;

            if (leRequired && string.IsNullOrWhiteSpace(Agency))
                yield return Fail(nameof(Agency), "Law‑enforcement agency is required for this event type.");

            if (leRequired && string.IsNullOrWhiteSpace(ReportNumber))
                yield return Fail(nameof(ReportNumber), "Report/case number is required for this event type.");
        }

        private static ValidationResult Fail(string member, string msg) => new(msg, new[] { member });
    }

}
