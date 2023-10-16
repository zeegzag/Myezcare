using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;
using ExpressiveAnnotations.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralPayorMappings")]
    [PrimaryKey("ReferralPayorMappingID")]
    [Sort("ReferralPayorMappingID", "DESC")]
    public class ReferralPayorMapping : BaseEntity
    {
        public long ReferralPayorMappingID { get; set; }
        public long ReferralID { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Payor")]
        [Required(ErrorMessageResourceName = "PayorRequired", ErrorMessageResourceType = typeof(Resource))]
        public long PayorID { get; set; }


        [Display(ResourceType = typeof(Resource), Name = "PayorEffectiveStartDate")]
        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? PayorEffectiveDate { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "PayorEffectiveEndDate")]
        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? PayorEffectiveEndDate { get; set; }

        [Required(ErrorMessageResourceName = "PrecedenceRequired", ErrorMessageResourceType = typeof(Resource))]
        public int? Precedence { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsPayorNotPrimaryInsured { get; set; }

        [RequiredIf("IsPayorNotPrimaryInsured == true",ErrorMessageResourceType = typeof(Resource),ErrorMessageResourceName = "InsuredNumberRequired")]
        public string InsuredId { get; set; }

        //[RequiredIf("IsPayorNotPrimaryInsured == true", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FirstNameRequired")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FirstNameRequired")]
        public string InsuredFirstName { get; set; }

        public string InsuredMiddleName { get; set; }

        //[RequiredIf("IsPayorNotPrimaryInsured == true", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "LastNameRequired")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "LastNameRequired")]
        public string InsuredLastName { get; set; }

        //[RequiredIf("IsPayorNotPrimaryInsured == true", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "AddressRequired")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "AddressRequired")]
        public string InsuredAddress { get; set; }

        //[RequiredIf("IsPayorNotPrimaryInsured == true", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "CityRequired")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "CityRequired")]
        public string InsuredCity { get; set; }

        //[RequiredIf("IsPayorNotPrimaryInsured == true", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "StateRequired")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "StateRequired")]
        public string InsuredState { get; set; }

        //[RequiredIf("IsPayorNotPrimaryInsured == true", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "CodeRequired")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "CodeRequired")]
        public string InsuredZipCode { get; set; }

        //[RequiredIf("IsPayorNotPrimaryInsured == true", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PhoneRequired")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PhoneRequired")]
        public string InsuredPhone { get; set; }

        //[RequiredIf("IsPayorNotPrimaryInsured == true", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "InsuredSPolicyGroupFECANumberRequired")]
        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "InsuredSPolicyGroupFECANumberRequired")]
        public string InsuredPolicyGroupOrFecaNumber { get; set; }

        //[RequiredIf("IsPayorNotPrimaryInsured == true", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "DobRequired")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "DobRequired")]
        public DateTime? InsuredDateOfBirth { get; set; }

        //[RequiredIf("IsPayorNotPrimaryInsured == true", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "GenderRequired")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "GenderRequired")]
        public string InsuredGender { get; set; }

        //[RequiredIf("IsPayorNotPrimaryInsured == true", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "EmployersNameSchoolNameRequired")]
        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "EmployersNameSchoolNameRequired")]
        public string InsuredEmployersNameOrSchoolName { get; set; }

        [Ignore]
        [IncludeInAudit]
        [GetTableNameAttribute("Payors")]
        [AuditMakeBeforeAfterAttribute]
        [Display(ResourceType = typeof(Resource), Name = "Payor")]
        public long TempPayorID { get; set; }

        [Ignore]
        public string EncryptedReferralId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "BeneficiaryTypeIDRequired")]
        public int BeneficiaryTypeID { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "BeneficiaryNumberRequired")]
        public string BeneficiaryNumber { get; set; }
        public string MemberID { get; set; }
        public int MasterJurisdictionID { get; set; }
        public int MasterTimezoneID { get; set; }
    }
}
