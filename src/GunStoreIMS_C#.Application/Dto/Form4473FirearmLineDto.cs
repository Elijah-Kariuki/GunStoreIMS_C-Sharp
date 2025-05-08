using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GunStoreIMS.Domain.Models;

namespace GunStoreIMS.Application.Dto
{
    public class Form4473FirearmLineDto
    {
        public Guid? Id { get; set; } // Optional for POST; required for PUT

        [Required]
        public Guid Form4473RecordId { get; set; }

        [Required, StringLength(150)]
        public string ManufacturerOrPMF { get; set; } = default!;

        [StringLength(100)]
        public string? Model { get; set; }

        [Required, StringLength(50)]
        public string SerialNumber { get; set; } = default!;

        [Required, StringLength(50)]
        public string FirearmType { get; set; } = default!;

        [Required, StringLength(50)]
        public string CaliberOrGauge { get; set; } = default!;
    }
}
