using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GunStoreIMS.Domain.Models
{
    /// <summary>
    /// Represents the entire ATF Form 4473 transaction record.
    /// This entity captures Sections A–E. Each 4473 can have multiple
    /// firearms (Section A lines) stored in Form4473FirearmLine.
    /// 
    /// This merged version:
    ///  - Uses separate fields for PCS details (PcsBaseName, etc.).
    ///  - Uses an enum (NicsResponseType?) for InitialNicsResponse.
    ///  - Retains TransferorsTransactionNumber, PawnRedemptionLineNumbers,
    ///    TransfereeCountyParishBorough, structured Seller info, and
    ///    separate NICS date fields (NicsProceedDate, etc.).
    /// </summary>
    public class Form4473Record : IValidatableObject
    {
        [Key]
        public Guid Id { get; set; }

        // ──────────────────────────────
        // Optional "Transferor’s/Seller’s Transaction Number" from top-right header
        // ──────────────────────────────
        [StringLength(50)]
        public string? TransferorsTransactionNumber { get; set; }

        // ──────────────────────────────
        // SECTION A - Transferor/Seller (Lines 1-6, plus 7-8)
        // ──────────────────────────────

        /// <summary>
        /// Firearms (lines 1–5). If more than 3 firearms,
        /// the continuation sheet (5300.9A) is used.
        /// Stored in a separate child table: Form4473FirearmLine.
        /// </summary>
        public virtual ICollection<Form4473FirearmLine> FirearmLines { get; set; }
            = new List<Form4473FirearmLine>();

        /// <summary>
        /// 6. Total Number of Firearms to be Transferred (spelled out, e.g. "one", "two")
        /// </summary>
        [Required, StringLength(50)]
        public string TotalNumberOfFirearms { get; set; } = default!;

        /// <summary>
        /// 7. Check if any part of this transaction is a pawn redemption
        /// </summary>
        public bool IsPawnRedemption { get; set; }

        /// <summary>
        /// If IsPawnRedemption == true, record line #s from Q1 that apply
        /// e.g., "Line 2"
        /// </summary>
        [StringLength(50)]
        public string? PawnRedemptionLineNumbers { get; set; }

        /// <summary>
        /// 8. Check if any part is to facilitate a private party transfer
        /// </summary>
        public bool IsPrivatePartyTransfer { get; set; }

        // ──────────────────────────────
        // SECTION B - Transferee/Buyer Info (Lines 9–20, plus Q21)
        // ──────────────────────────────

        // 9. Transferee’s/Buyer’s Full Name
        [Required, StringLength(50)]
        public string TransfereeLastName { get; set; } = default!;

        [Required, StringLength(50)]
        public string TransfereeFirstName { get; set; } = default!;

        [StringLength(50)]
        public string? TransfereeMiddleName { get; set; }

        // 10. Current State of Residence and Address
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

        /// <summary>
        /// "Reside in City Limits?" – user indicates yes/no
        /// </summary>
        [Required]
        public bool? ResideInCityLimits { get; set; }

        // 11. Place of Birth
        [Required, StringLength(100)]
        public string TransfereePlaceOfBirth { get; set; } = default!;

        // 12. Height
        [Required, Range(0, 7)]
        public int? HeightFeet { get; set; }

        [Required, Range(0, 11)]
        public int? HeightInches { get; set; }

        // 13. Weight
        [Required, Range(1, 999)]
        public int? Weight { get; set; }

        // 14. Sex
        [Required]
        public SexOption? Sex { get; set; }

        // 15. Birth Date
        [Required]
        public DateTime DateOfBirth { get; set; }

        // 16. SSN (optional)
        [StringLength(11)]
        public string? SSN { get; set; }

        // 17. UPIN or AMD ID
        [StringLength(25)]
        public string? UpinOrAmdId { get; set; }

        // 18.a. Hispanic/Latino?
        [Required]
        public bool? IsHispanicOrLatino { get; set; }

        // 18.b. Race checkboxes
        public bool RaceAmericanIndianOrAlaskaNative { get; set; }
        public bool RaceAsian { get; set; }
        public bool RaceBlackOrAfricanAmerican { get; set; }
        public bool RaceNativeHawaiianOrPacificIslander { get; set; }
        public bool RaceWhite { get; set; }

        // 19. Country of Citizenship
        [Required, StringLength(100)]
        public string CountryOfCitizenship { get; set; } = default!;

        // 20. Alien # (if relevant)
        [StringLength(50)]
        public string? AlienNumberOrAdmissionNumber { get; set; }

        // ──────────────────────────────
        // Q21 (a–n): "Yes/No" fields
        // ──────────────────────────────
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
        /// <summary>21.m.2 if NonImmigrantVisa == true</summary>
        public bool? NonImmigrantVisaException { get; set; }
        [Required]
        public bool? WillDisposeToProhibitedPerson { get; set; }

        // ──────────────────────────────
        // Lines 22–23
        // ──────────────────────────────
        [Required, StringLength(200)]
        public string TransfereeSignature { get; set; } = default!;

        [Required]
        public DateTime CertificationDate { get; set; }

        // ──────────────────────────────
        // 24. Category of firearm(s)
        // ──────────────────────────────
        public bool CategoryHandgun { get; set; }
        public bool CategoryLongGun { get; set; }
        public bool CategoryOther { get; set; }

        // ──────────────────────────────
        // 25. Gun show/event
        // ──────────────────────────────
        [StringLength(200)]
        public string? GunShowOrEventName { get; set; }

        [StringLength(200)]
        public string? GunShowOrEventCityStateZip { get; set; }

        // ──────────────────────────────
        // 26. Identification
        // ──────────────────────────────
        [Required, StringLength(100)]
        public string IdentificationType { get; set; } = default!;

        [Required, StringLength(50)]
        public string IdentificationNumber { get; set; } = default!;

        [Required]
        public DateTime IdentificationExpDate { get; set; }

        [StringLength(200)]
        public string? SupplementalGovtDoc { get; set; }

        /// <summary>
        /// PCS Info splitted: BaseName, EffectiveDate, OrderNumber
        /// </summary>
        [StringLength(200)]
        public string? PcsBaseName { get; set; }

        public DateTime? PcsEffectiveDate { get; set; }

        [StringLength(50)]
        public string? PcsOrderNumber { get; set; }

        /// <summary>
        /// If NonImmigrantVisa == true and NonImmigrantVisaException == true,
        /// user must provide docs (e.g. a hunting license).
        /// </summary>
        [StringLength(200)]
        public string? NonImmigrantExceptionDocumentation { get; set; }

        // ──────────────────────────────
        // SECTION C - NICS info (27)
        // ──────────────────────────────

        public DateTime? NicsCheckInitiatedDate { get; set; }

        [StringLength(50)]
        public string? NicsOrStateTransactionNumber { get; set; }

        /// <summary>
        /// If no response was received yet, can be null.
        /// </summary>
        public NicsResponseType? InitialNicsResponse { get; set; }

        public DateTime? NicsDelayedEligibleDate { get; set; }

        [StringLength(200)]
        public string? SubsequentNicsResponses { get; set; }

        [StringLength(200)]
        public string? PostTransferNicsResponse { get; set; }

        public DateTime? NicsProceedDate { get; set; }
        public DateTime? NicsDeniedDate { get; set; }
        public DateTime? NicsCancelledDate { get; set; }
        public DateTime? NicsOverturnedDate { get; set; }

        public bool IsNfaBackgroundCheckAlready { get; set; }
        public bool IsStatePermitExemption { get; set; }

        [StringLength(100)]
        public string? StatePermitType { get; set; }
        public DateTime? StatePermitIssueDate { get; set; }
        public DateTime? StatePermitExpirationDate { get; set; }

        [StringLength(50)]
        public string? StatePermitNumber { get; set; }

        // ──────────────────────────────
        // SECTION D - Recert (30–31)
        // ──────────────────────────────
        [StringLength(200)]
        public string? TransfereeRecertSignature { get; set; }

        public DateTime? TransfereeRecertDate { get; set; }

        // ──────────────────────────────
        // SECTION E - Transferor/Seller (32–36)
        // ──────────────────────────────
        [StringLength(500)]
        public string? LicenseeUseRemarks { get; set; }

        [StringLength(200)]
        public string? SellerTradeName { get; set; }

        [StringLength(200)]
        public string? SellerStreetAddress { get; set; }

        [StringLength(200)]
        public string? SellerCityStateZip { get; set; }

        [StringLength(20)]
        public string? SellerFFLNumber { get; set; }

        [StringLength(100)]
        public string? TransferorName { get; set; }

        [StringLength(200)]
        public string? TransferorSignature { get; set; }

        public DateTime? TransferDate { get; set; }

        // ──────────────────────────────
        // Implement IValidatableObject
        // ──────────────────────────────
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            DateTime today = DateTime.Today; // Use for consistent date checks

            // ─────────────────────────────────────────────────────────────────────
            // PART 1: BASIC FIELD & LOGIC CHECKS (Mostly Section A & B)
            // ─────────────────────────────────────────────────────────────────────

            // Check: At least one race must be selected (Q18.b)
            if (!RaceAmericanIndianOrAlaskaNative
                && !RaceAsian
                && !RaceBlackOrAfricanAmerican
                && !RaceNativeHawaiianOrPacificIslander
                && !RaceWhite)
            {
                results.Add(Fail("RaceSelection", // Use a general key or list all race properties
                    "At least one race must be selected in Question 18.b.",
                    new[] { nameof(RaceAmericanIndianOrAlaskaNative), nameof(RaceAsian), nameof(RaceBlackOrAfricanAmerican), nameof(RaceNativeHawaiianOrPacificIslander), nameof(RaceWhite) }));
            }

            // Check: Pawn Redemption Line Numbers required if applicable (Q7)
            if (IsPawnRedemption && string.IsNullOrWhiteSpace(PawnRedemptionLineNumbers))
            {
                results.Add(Fail(nameof(PawnRedemptionLineNumbers),
                    "When the transaction is a pawn redemption (Q7), you must record the line number(s) from Question 1."));
            }

            // Check: Date of Birth cannot be in the future (Q15)
            if (DateOfBirth > today)
            {
                results.Add(Fail(nameof(DateOfBirth), "Date of Birth cannot be in the future."));
            }

            // ─────────────────────────────────────────────────────────────────────
            // PART 2: Q21 PROHIBITION CHECKS (Section B)
            // ─────────────────────────────────────────────────────────────────────
            // These checks determine if the transferee's answers immediately prohibit the sale.

            // 21.a MUST be yes (except for a narrow gunsmith/repair scenario - assumes not applicable here).
            if (IsActualTransfereeBuyer == false)
            {
                results.Add(Fail(nameof(IsActualTransfereeBuyer),
                    "You indicated you are NOT the actual transferee/buyer (Q21.a). This prohibits the transaction."));
            }
            // 21.b => If "Yes", prohibited
            if (WillDisposeToFelony == true)
            {
                results.Add(Fail(nameof(WillDisposeToFelony),
                    "You answered 'Yes' to Q21.b (Intend to dispose for felony). This is prohibited."));
            }
            // 21.c => Under Indictment => prohibited
            if (UnderIndictment == true)
            {
                results.Add(Fail(nameof(UnderIndictment),
                    "You answered 'Yes' to Q21.c (Under Indictment). This is prohibited."));
            }
            // 21.d => Ever Convicted Felony => prohibited
            if (EverConvictedFelony == true)
            {
                results.Add(Fail(nameof(EverConvictedFelony),
                    "You answered 'Yes' to Q21.d (Convicted of a felony). This is prohibited."));
            }
            // 21.e => Fugitive => prohibited
            if (FugitiveFromJustice == true)
            {
                results.Add(Fail(nameof(FugitiveFromJustice),
                    "You answered 'Yes' to Q21.e (Fugitive From Justice). This is prohibited."));
            }
            // 21.f => Unlawful user of controlled substances => prohibited
            if (UnlawfulUserOfControlledSubstance == true)
            {
                results.Add(Fail(nameof(UnlawfulUserOfControlledSubstance),
                    "You answered 'Yes' to Q21.f (Unlawful User). This is prohibited."));
            }
            // 21.g => Adjudicated Mentally Defective => prohibited
            if (AdjudicatedMentallyDefective == true)
            {
                results.Add(Fail(nameof(AdjudicatedMentallyDefective),
                    "You answered 'Yes' to Q21.g (Mental Defect/Committed). This is prohibited."));
            }
            // 21.h => Dishonorable Discharge => prohibited
            if (DishonorableDischarge == true)
            {
                results.Add(Fail(nameof(DishonorableDischarge),
                    "You answered 'Yes' to Q21.h (Dishonorable Discharge). This is prohibited."));
            }
            // 21.i => Subject to Restraining Order => prohibited
            if (SubjectToRestrainingOrder == true)
            {
                results.Add(Fail(nameof(SubjectToRestrainingOrder),
                    "You answered 'Yes' to Q21.i (Restraining Order). This is prohibited."));
            }
            // 21.j => Misdemeanor Domestic Violence => prohibited
            if (ConvictedMisdemeanorDomesticViolence == true)
            {
                results.Add(Fail(nameof(ConvictedMisdemeanorDomesticViolence),
                    "You answered 'Yes' to Q21.j (Misdemeanor Domestic Violence). This is prohibited."));
            }
            // 21.k => Renounced Citizenship => prohibited
            if (RenouncedUSCitizenship == true)
            {
                results.Add(Fail(nameof(RenouncedUSCitizenship),
                    "You answered 'Yes' to Q21.k (Renounced US Citizenship). This is prohibited."));
            }
            // 21.l => Alien Illegally => prohibited
            if (AlienIllegally == true)
            {
                results.Add(Fail(nameof(AlienIllegally),
                    "You answered 'Yes' to Q21.l (Illegal Alien Status). This is prohibited."));
            }
            // 21.n => "Do you intend to sell or dispose to a prohibited person?" => if "Yes", prohibited
            if (WillDisposeToProhibitedPerson == true)
            {
                results.Add(Fail(nameof(WillDisposeToProhibitedPerson),
                    "You answered 'Yes' to Q21.n (Will dispose to a prohibited person). This is prohibited."));
            }

            // Optional: Short-circuit if already prohibited by a Q21 answer
            // if (results.Any(r => r.ErrorMessage.Contains("prohibited."))) return results;

            // ─────────────────────────────────────────────────────────────────────
            // PART 3: NON-IMMIGRANT VISA EXCEPTION HANDLING (Q21.m, Q26.d)
            // ─────────────────────────────────────────────────────────────────────
            if (NonImmigrantVisa == true)
            {
                if (NonImmigrantVisaException == null)
                {
                    results.Add(Fail(nameof(NonImmigrantVisaException), "You must answer Q21.m.2 if 21.m.1 is Yes."));
                }
                else if (NonImmigrantVisaException == false)
                {
                    results.Add(Fail(nameof(NonImmigrantVisaException), "You indicated 'No' to Q21.m.2 while 'Yes' to 21.m.1 => prohibited."));
                }
                else // NonImmigrantVisaException == true
                {
                    if (string.IsNullOrWhiteSpace(NonImmigrantExceptionDocumentation))
                    {
                        results.Add(Fail(nameof(NonImmigrantExceptionDocumentation), "Nonimmigrant Alien Exception (Q21.m.2) requires documentation (Q26.d)."));
                    }
                }
            }

            // ─────────────────────────────────────────────────────────────────────
            // PART 4: AGE VERIFICATION (Q15 vs Q24/Firearm Type)
            // ─────────────────────────────────────────────────────────────────────
            // Use CertificationDate as the reference for age check at time of form completion.
            DateTime referenceDate = CertificationDate;
            int age = CalculateAge(DateOfBirth, referenceDate);

            // Determine if a handgun or "other" type is being transferred. Requires inspecting firearm lines.
            bool transferringHandgunOrOther = CategoryHandgun || CategoryOther || FirearmLinesContainHandgunOrOther();

            if (transferringHandgunOrOther)
            {
                if (age < 21)
                {
                    results.Add(Fail(nameof(DateOfBirth), "Transferee must be 21 or older to purchase a handgun or 'other' type firearm."));
                }
            }
            else // Only transferring long guns (rifles/shotguns)
            {
                if (age < 18)
                {
                    results.Add(Fail(nameof(DateOfBirth), "Transferee must be 18 or older to purchase a rifle or shotgun."));
                }
            }
            // Note: State laws may impose higher age limits. This validation reflects federal minimums.

            // ─────────────────────────────────────────────────────────────────────
            // PART 5: RESIDENCY & LOCATION RULES (Q10 vs FFL Location/Transfer Location)
            // ─────────────────────────────────────────────────────────────────────
            // Note: Full residency compliance checks (e.g., handgun transfers generally require
            // transferee to be a resident of the same state as the FFL) cannot be fully validated
            // solely within this model without external context (FFL's licensed state, transfer location).
            // Basic address format checks (e.g., ZIP code) could be added if needed.

            // ─────────────────────────────────────────────────────────────────────
            // PART 6: ID DOCUMENT CHECKS (Q26)
            // ─────────────────────────────────────────────────────────────────────
            // Check ID expiration date
            if (IdentificationExpDate < today)
            {
                results.Add(Fail(nameof(IdentificationExpDate), "Identification document (Q26.a) is expired."));
            }
            // Note: Determining the *necessity* of Supplemental Government Documentation (Q26.b)
            // typically requires comparing the address on the physical ID (not stored in this model)
            // with the address provided in Q10. This validation cannot perform that comparison.
            // Similar context applies to needing PCS Orders (Q26.c).

            // ─────────────────────────────────────────────────────────────────────
            // PART 7: NICS / EXEMPTIONS LOGIC (Q27, Q28, Q29)
            // ─────────────────────────────────────────────────────────────────────
            bool noNicsExemption = (!IsNfaBackgroundCheckAlready && !IsStatePermitExemption);
            if (noNicsExemption)
            {
                // If no exemption, NICS check date and initial response are required.
                if (!NicsCheckInitiatedDate.HasValue)
                {
                    results.Add(Fail(nameof(NicsCheckInitiatedDate), "NICS Check is required unless NFA (Q28) or State permit (Q29) exemption applies. Date must be recorded (Q27.a)."));
                }
                if (InitialNicsResponse == null)
                {
                    results.Add(Fail(nameof(InitialNicsResponse), "Must record the initial NICS response (Q27.c) if no NFA or permit exemption is claimed."));
                }

                // Check the outcome of the Initial NICS Response
                if (InitialNicsResponse.HasValue)
                {
                    switch (InitialNicsResponse.Value)
                    {
                        case NicsResponseType.Denied:
                        case NicsResponseType.Cancelled:
                            // A Denied/Cancelled response prohibits the transfer.
                            results.Add(Fail(nameof(InitialNicsResponse), $"Initial NICS response was {InitialNicsResponse.Value}. Transfer is prohibited."));
                            break;
                        case NicsResponseType.Delayed:
                            // If Delayed, the eligible date should be recorded.
                            if (!NicsDelayedEligibleDate.HasValue)
                            {
                                results.Add(Fail(nameof(NicsDelayedEligibleDate), "Delayed NICS response requires the Missing Disposition Information (MDI) / eligible transfer date to be recorded (Q27.c)."));
                            }
                            // IMPORTANT: The check ensuring the transfer *does not occur before* NicsDelayedEligibleDate
                            // must happen *at the time of the transfer attempt* (comparing against TransferDate or current time).
                            // This model validation cannot enforce that timing unless TransferDate is set AND in the past.
                            break;
                            // Proceed, Overturned (if applicable in initial response) are generally OK to proceed.
                    }
                }

                // Check NICS 30-day validity window. The check is valid *up to* day 29. Day 30 requires a new check.
                // Use TransferDate if available, otherwise 'today', as the point of comparison.
                DateTime nicsComparisonDate = TransferDate ?? today;
                if (NicsCheckInitiatedDate.HasValue && (nicsComparisonDate.Date - NicsCheckInitiatedDate.Value.Date).TotalDays >= 30)
                {
                    results.Add(Fail(nameof(NicsCheckInitiatedDate), "NICS check is only valid for 30 calendar days. The check initiated on " + NicsCheckInitiatedDate.Value.ToShortDateString() + " has expired. A new check is required."));
                }
            }
            else // Exemption applies
            {
                // If State Permit Exemption is claimed, check permit details (Q29)
                if (IsStatePermitExemption)
                {
                    if (string.IsNullOrWhiteSpace(StatePermitType))
                    {
                        results.Add(Fail(nameof(StatePermitType), "State Permit Type must be specified if claiming permit exemption (Q29)."));
                    }
                    if (!StatePermitExpirationDate.HasValue)
                    {
                        results.Add(Fail(nameof(StatePermitExpirationDate), "State Permit Expiration Date must be specified if claiming permit exemption (Q29)."));
                    }
                    else if (StatePermitExpirationDate.Value < today)
                    {
                        results.Add(Fail(nameof(StatePermitExpirationDate), "State Permit (Q29) is expired; cannot claim exemption."));
                    }
                    // Consider adding checks for StatePermitNumber and IssueDate if required by policy.
                }
                // Note: NFA Exemption (Q28) relies on the existence of an approved NFA form, which is external to this record.
            }

            // ─────────────────────────────────────────────────────────────────────
            // PART 8: RECERTIFICATION LOGIC (Section D - Q30, Q31)
            // ─────────────────────────────────────────────────────────────────────
            // If TransferDate is set and is on a different calendar day than CertificationDate, Recertification is required.
            if (TransferDate.HasValue && TransferDate.Value.Date != CertificationDate.Date)
            {
                if (string.IsNullOrWhiteSpace(TransfereeRecertSignature))
                {
                    results.Add(Fail(nameof(TransfereeRecertSignature), "Transferee must recertify (Q30 Signature) if transfer date differs from the original certification date (Q23)."));
                }
                if (!TransfereeRecertDate.HasValue)
                {
                    results.Add(Fail(nameof(TransfereeRecertDate), "Transferee must provide Recertification Date (Q31) if transfer date differs from the original certification date (Q23)."));
                }
                else if (TransfereeRecertDate.Value.Date != TransferDate.Value.Date)
                {
                    // Technically, recertification should happen *immediately prior* to transfer on that day.
                    results.Add(Fail(nameof(TransfereeRecertDate), "Recertification Date (Q31) must match the Transfer Date (Q36)."));
                }
            }

            // ─────────────────────────────────────────────────────────────────────
            // PART 9: SELLER CERTIFICATION (Section E - Q32-36)
            // ─────────────────────────────────────────────────────────────────────
            // If the transfer is recorded as completed (TransferDate is set), ensure Seller details are present.
            if (TransferDate.HasValue)
            {
                // Basic checks for required fields in Section E at time of transfer
                if (string.IsNullOrWhiteSpace(TransferorName))
                {
                    results.Add(Fail(nameof(TransferorName), "Transferor/Seller Name (Q34) must be completed at time of transfer."));
                }
                if (string.IsNullOrWhiteSpace(TransferorSignature))
                {
                    results.Add(Fail(nameof(TransferorSignature), "Transferor/Seller Signature (Q35) must be completed at time of transfer."));
                }
                // Could add checks for Seller Trade Name, Address, FFL# (Q33) if deemed essential for record completion validation.
                if (string.IsNullOrWhiteSpace(SellerTradeName) || string.IsNullOrWhiteSpace(SellerStreetAddress) || string.IsNullOrWhiteSpace(SellerCityStateZip) || string.IsNullOrWhiteSpace(SellerFFLNumber))
                {
                    results.Add(Fail(nameof(SellerTradeName), "Seller Trade Name, Address, and FFL# (Q33) should be completed for transfer.")); // Adjust severity/requirement as needed
                }
            }

            // ─────────────────────────────────────────────────────────────────────
            // FINAL NOTES ON COMPLIANCE
            // ─────────────────────────────────────────────────────────────────────
            // - This validation checks data consistency within the Form 4473 record itself based on federal regulations reflected on the form.
            // - Full compliance also requires adherence to State and Local laws, which may be stricter (e.g., waiting periods, specific permits, different age limits). These are not validated here.
            // - Process compliance (e.g., verifying ID against the person, ensuring transfer happens *after* NICS delay period, correctly handling multiple handgun sales reporting) occurs outside this model validation.

            return results;
        }

        // Helper method for generating ValidationResults (Consider adding overload for multiple members)
        private ValidationResult Fail(string member, string msg, string[]? memberNames = null)
        {
            return new ValidationResult(msg, memberNames ?? new[] { member });
        }

        // Helper to calculate age
        private int CalculateAge(DateTime birthDate, DateTime referenceDate)
        {
            int age = referenceDate.Year - birthDate.Year;
            // Adjust if birthday hasn't occurred yet in the reference year
            if (birthDate.Date > referenceDate.Date.AddYears(-age)) age--;
            return age;
        }

        // Placeholder: Needs actual implementation based on Form4473FirearmLine data
        private bool FirearmLinesContainHandgunOrOther()
        {
            if (FirearmLines == null || !FirearmLines.Any())
            {
                // If no lines, rely solely on Category checkboxes? Or is this an invalid state?
                // Assuming for now if no lines, Category checkboxes are primary.
                return CategoryHandgun || CategoryOther;
            }

            // TODO: Implement actual logic to check the 'Type' property of each Form4473FirearmLine
            // Example (assuming Form4473FirearmLine has a 'FirearmType' property/enum):
            // return FirearmLines.Any(line => line.FirearmType == FirearmTypes.Pistol ||
            //                                line.FirearmType == FirearmTypes.Revolver ||
            //                                line.FirearmType == FirearmTypes.ReceiverFrame ||
            //                                line.FirearmType == FirearmTypes.Other);

            // Fallback to Category checkboxes if lines exist but type isn't checked (adjust as needed)
            return CategoryHandgun || CategoryOther || FirearmLines.Any(IsHandgunOrOtherType);
        }

        // Example helper for the above (replace with actual logic)
        private bool IsHandgunOrOtherType(Form4473FirearmLine line)
        {
            // CORRECTED: Changed line.Type to line.FirearmType
            string typeLower = line.FirearmType?.ToLowerInvariant() ?? ""; // Use the correct property name

            // These checks should align with types requiring age 21+
            return typeLower == "pistol" ||
                   typeLower == "revolver" ||
                   typeLower == "receiver" || // Treat receivers/frames as 'other' for age check
                   typeLower == "frame" ||
                   // Add other specific string values that ATF considers "other" or requires age 21
                   // e.g., "pistol grip firearm", "silencer", "short-barreled rifle", etc.
                   // Using .Contains("other") is a fallback and might be too broad or narrow depending on input.
                   typeLower.Contains("other");
        }

        

    }
}
