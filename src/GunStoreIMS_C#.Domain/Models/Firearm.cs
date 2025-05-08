using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace GunStoreIMS.Domain.Models
{
    public abstract class Firearm : IValidatableObject
    {
        // ─── Identity ──────────────────────────────────────────────────────────
        [Key]
        public Guid Id { get; set; }

        // Manufacturer / Importer / Maker
        [Required, StringLength(100)]
        public string Manufacturer { get; set; } = default!;

        [StringLength(100)]
        public string? MakerName { get; set; } // For Form 1 (NFA) items

        [StringLength(100)]
        public string? Importer { get; set; }

        [StringLength(50)]
        public string? ImporterCity { get; set; }

        public USState? ImporterState { get; set; }

        [Required, StringLength(50)]
        public string ManufacturerCity { get; set; } = default!;

        [Required]
        public USState ManufacturerState { get; set; }

        // Model & serial
        [Required, StringLength(100)]
        public string Model { get; set; } = default!;

        [Required, StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9.\-/]+$", ErrorMessage = "Invalid serial number.")]
        public string SerialNumber { get; set; } = default!;  // encrypt at rest!

        [NotMapped]
        public string? DecryptedSerialNumber { get; set; } // logic handled in service layer

        [StringLength(100)]
        public string? AdditionalMarkings { get; set; }

        [Required]
        public bool IsSerialObliterated { get; set; }

        // Caliber
        [Required]
        public int CaliberId { get; set; }

        [ForeignKey(nameof(CaliberId))]
        public Caliber Caliber { get; set; } = default!;

        // ─── Classification (single source of truth) ──────────────────────────
        private static readonly Dictionary<FirearmType, (bool isNfa, NfaClassification?)> _map
            = new()
        {
            { FirearmType.ShortBarreledRifle,   (true, NfaClassification.ShortBarreledRifle) },
            { FirearmType.ShortBarreledShotgun, (true, NfaClassification.ShortBarreledShotgun) },
            { FirearmType.MachineGun,           (true, NfaClassification.MachineGun) },
            { FirearmType.Silencer,             (true, NfaClassification.Silencer) },
            { FirearmType.DestructiveDevice,    (true, NfaClassification.DestructiveDevice) },
            // everything else → non-NFA
        };

        private FirearmType _type;

        [Required]
        public FirearmType Type
        {
            get => _type;
            set
            {
                _type = value;
                // auto-sync the two “redundant” columns
                if (_map.TryGetValue(value, out var info))
                {
                    IsNFAItem = info.isNfa;
                    NfaClass = info.Item2;
                }
                else
                {
                    IsNFAItem = false;
                    NfaClass = null;
                }
            }
        }

        // still stored in the same table, but now ALWAYS consistent
        public bool IsNFAItem { get; private set; }

        public NfaClassification? NfaClass { get; private set; }

        [StringLength(200)]
        public string? OtherTypeDescription { get; set; }

        [NotMapped]
        public bool IsFrameOrReceiver => Type == FirearmType.Receiver;

        // Manufacture details
        [Required, DataType(DataType.Date)]
        public DateTime ManufactureDate { get; set; }

        [Required, StringLength(100)]
        public string CountryOfOrigin { get; set; } = default!;

        public bool IsAntique { get; set; }

        [Required]
        public bool IsImported { get; set; }

        // FFL tracking
        [Required]
        public int FFLId { get; set; }

        [ForeignKey(nameof(FFLId))]
        public FFL FFL { get; set; } = default!;

        [StringLength(50)]
        public string? YourFFLMarking { get; set; }

        [StringLength(50)]
        public string? YourMarkingLocation { get; set; }

        public bool IsMultiPieceFrame { get; set; }

        // ──────────────────────────────────────────────────────────────────────
        // Removed: The Firearm-level "Acquisition / Disposition" fields. 
        // The acquisition/disposition details now live entirely in the 
        // 'Acquisition' and 'Disposition' entities, removing redundancy. 
        // ──────────────────────────────────────────────────────────────────────

        // Law‑enforcement / gov’t
        public bool LawEnforcementSale { get; set; }
        public int? LawEnforcementDocumentId { get; set; }

        [ForeignKey(nameof(LawEnforcementDocumentId))]
        public Document? LawEnforcementDocument { get; set; }

        // Navigation
        public ICollection<Acquisition> Acquisitions { get; set; } = new List<Acquisition>();
        public ICollection<Disposition> Dispositions { get; set; } = new List<Disposition>();
        public ICollection<Recovery> Recoveries { get; set; } = new List<Recovery>();
        public ICollection<SerialNumberHistory> SerialHistory { get; set; } = new List<SerialNumberHistory>();

        // Concurrency / internal
        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;

        [JsonIgnore]
        public string? InternalNotes { get; set; }

        // ─── Validation (unchanged except DRY helper) ─────────────────────────
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext _)
        {
            if (ManufactureDate > DateTime.UtcNow)
            {
                yield return Fail(nameof(ManufactureDate), "Manufacture date cannot be in the future.");
            }

            if (IsImported)
            {
                if (string.IsNullOrWhiteSpace(Importer)
                    || string.IsNullOrWhiteSpace(ImporterCity)
                    || !ImporterState.HasValue)
                {
                    yield return Fail(nameof(Importer), "Importer details are required for imported firearms.");
                }

                if (CountryOfOrigin.Equals("USA", StringComparison.OrdinalIgnoreCase))
                {
                    yield return Fail(nameof(CountryOfOrigin), "Imported firearms cannot have 'USA' as Country of Origin.");
                }
            }

            if (SerialNumber?.Length < 3)
            {
                yield return Fail(nameof(SerialNumber), "Serial number must be at least 3 characters long.");
            }

            if (Type == FirearmType.Other && string.IsNullOrWhiteSpace(OtherTypeDescription))
            {
                yield return Fail(nameof(OtherTypeDescription), "Description is required when firearm type is 'Other'.");
            }
        }

        private static ValidationResult Fail(string member, string msg)
            => new(msg, new[] { member });
    }
}
