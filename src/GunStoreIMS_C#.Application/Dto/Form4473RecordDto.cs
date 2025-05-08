using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GunStoreIMS.Domain.Models;

namespace GunStoreIMS.Application.Dto
{
    public class Form4473RecordDto
    {
        public Guid? Id { get; set; }

        [StringLength(50)]
        public string? TransferorsTransactionNumber { get; set; }

        // Section A
        [Required, StringLength(50)]
        public string TotalNumberOfFirearms { get; set; } = default!;

        public bool IsPawnRedemption { get; set; }

        [StringLength(50)]
        public string? PawnRedemptionLineNumbers { get; set; }

        public bool IsPrivatePartyTransfer { get; set; }

        [Required]
        public List<Form4473FirearmLineDto> FirearmLines { get; set; } = new();

        // Section B - Transferee Info
        [Required, StringLength(50)]
        public string TransfereeLastName { get; set; } = default!;

        [Required, StringLength(50)]
        public string TransfereeFirstName { get; set; } = default!;

        [StringLength(50)]
        public string? TransfereeMiddleName { get; set; }

        [Required, StringLength(150)]
        public string TransfereeStreetAddress { get; set; } = default!;

        [Required, StringLength(100)]
        public string TransfereeCity { get; set; } = default!;

        [StringLength(100)]
        public string? TransfereeCountyParishBorough { get; set; }

        [Required]
        public USState TransfereeState { get; set; }

        [Required, StringLength(15)]
        public string TransfereeZipCode { get; set; } = default!;

        public bool? ResideInCityLimits { get; set; }

        [Required, StringLength(100)]
        public string TransfereePlaceOfBirth { get; set; } = default!;

        [Required]
        public int? HeightFeet { get; set; }

        [Required]
        public int? HeightInches { get; set; }

        [Required]
        public int? Weight { get; set; }

        [Required]
        public SexOption? Sex { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [StringLength(11)]
        public string? SSN { get; set; }

        [StringLength(25)]
        public string? UpinOrAmdId { get; set; }

        [Required]
        public bool? IsHispanicOrLatino { get; set; }

        public bool RaceAmericanIndianOrAlaskaNative { get; set; }
        public bool RaceAsian { get; set; }
        public bool RaceBlackOrAfricanAmerican { get; set; }
        public bool RaceNativeHawaiianOrPacificIslander { get; set; }
        public bool RaceWhite { get; set; }

        [Required, StringLength(100)]
        public string CountryOfCitizenship { get; set; } = default!;

        [StringLength(50)]
        public string? AlienNumberOrAdmissionNumber { get; set; }

        // Q21
        [Required]
        public bool? IsActualTransfereeBuyer { get; set; }
        [Required]
        public bool? WillDisposeToFelony { get; set; }
        [Required]
        public bool? UnderIndictment { get; set; }
        [Required]
        public bool? EverConvictedFelony { get; set; }
        [Required]
        public bool? FugitiveFromJustice { get; set; }
        [Required]
        public bool? UnlawfulUserOfControlledSubstance { get; set; }
        [Required]
        public bool? AdjudicatedMentallyDefective { get; set; }
        [Required]
        public bool? DishonorableDischarge { get; set; }
        [Required]
        public bool? SubjectToRestrainingOrder { get; set; }
        [Required]
        public bool? ConvictedMisdemeanorDomesticViolence { get; set; }
        [Required]
        public bool? RenouncedUSCitizenship { get; set; }
        [Required]
        public bool? AlienIllegally { get; set; }
        [Required]
        public bool? NonImmigrantVisa { get; set; }
        public bool? NonImmigrantVisaException { get; set; }
        [Required]
        public bool? WillDisposeToProhibitedPerson { get; set; }

        // Section B Footer
        [Required, StringLength(200)]
        public string TransfereeSignature { get; set; } = default!;
        [Required]
        public DateTime CertificationDate { get; set; }

        // Optional: Section C–E, if needed in client
        // Otherwise, create Admin-only DTOs that expose full review data
    }
}
