using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Captures each individual firearm line in Section A of ATF Form 4473.
    /// If more than 3 firearms are on a single 4473, those additional lines
    /// are also recorded in this entity (for continuation sheets).
    /// </summary>
    public class Form4473FirearmLine
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid Form4473RecordId { get; set; }

        [ForeignKey(nameof(Form4473RecordId))]
        public virtual Form4473Record Form4473Record { get; set; } = default!;

        // 1. Manufacturer & Importer (if different) or “Privately Made Firearm”
        [Required, StringLength(150)]
        public string ManufacturerOrPMF { get; set; } = default!;

        // 2. Model (if designated)
        [StringLength(100)]
        public string? Model { get; set; }

        // 3. Serial Number
        [Required, StringLength(50)]
        public string SerialNumber { get; set; } = default!;

        // 4. Type (pistol, revolver, rifle, shotgun, receiver, frame, etc.)
        [Required, StringLength(50)]
        public string FirearmType { get; set; } = default!;

        // 5. Caliber or Gauge
        [Required, StringLength(50)]
        public string CaliberOrGauge { get; set; } = default!;
    }
}
