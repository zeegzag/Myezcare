using ExpressiveAnnotations.Attributes;
using PetaPoco;
using System;
using System.ComponentModel.DataAnnotations;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralBillingAuthorizations")]
    [PrimaryKey("ReferralBillingAuthorizationID")]
    [Sort("ReferralBillingAuthorizationID", "DESC")]
    public class ReferralBillingAuthorization : BaseEntity
    {
        public long ReferralBillingAuthorizationID { get; set; }
        public long ReferralID { get; set; }
        public string Type { get; set; }
        [Required(ErrorMessageResourceName = "AuthorizationCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string AuthorizationCode { get; set; }
        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? EndDate { get; set; }


        public long PayorID { get; set; }



        //public long? ServiceCodeID { get; set; }

        [Ignore]
        [Required(ErrorMessageResourceName = "ServiceCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string StrServiceCodeIDs { get; set; }

        [RegularExpression(Constants.RegxNumericSixDigit, ErrorMessageResourceName = "InvalideAllowedTime", ErrorMessageResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "AllowedTimeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? AllowedTime { get; set; }


        #region New Change In Billing - Kundan>Kumar>Rai

        [Required(ErrorMessageResourceName = "ServiceCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        [Range(1, long.MaxValue, ErrorMessageResourceName = "ServiceCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long ServiceCodeID { get; set; }

        //[Required(ErrorMessageResourceName = "RateRequired", ErrorMessageResourceType = typeof(Resource))]
        //[RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessageResourceName = "Ratemustbenumber", ErrorMessageResourceType = typeof(Resource))]
        public float Rate { get; set; }

        public long? RevenueCode { get; set; }

        [Required(ErrorMessageResourceName = "UnitTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int UnitType { get; set; }
        [Ignore]
        public string UnitTypeText
        {
            get { return Common.GetUnitType(UnitType); }
        }

        [Required(ErrorMessageResourceName = "CareTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long CareType { get; set; }

        //[Required(ErrorMessageResourceName = "ModifierCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long ModifierID { get; set; }

        [Required(ErrorMessageResourceName = "PerUnitQuantityRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxNumericSixDigit, ErrorMessageResourceName = "PerUnitQuantityInvalid", ErrorMessageResourceType = typeof(Resource))]
        public float PerUnitQuantity { get; set; }

        [Required(ErrorMessageResourceName = "RoundUpUnitRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxNumericSixDigit, ErrorMessageResourceName = "RoundUpUnitInvalid", ErrorMessageResourceType = typeof(Resource))]
        [AssertThat("RoundUpUnit <= PerUnitQuantity", ErrorMessageResourceName = "RoundUpUnitLessThanValidation", ErrorMessageResourceType = typeof(Resource))]
        public int RoundUpUnit { get; set; }

        [Required(ErrorMessageResourceName = "MaxUnitRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxNumericSixDigit, ErrorMessageResourceName = "MaxUnitInvalid", ErrorMessageResourceType = typeof(Resource))]
        public int MaxUnit { get; set; }

        [Required(ErrorMessageResourceName = "DailyUnitRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxNumericSixDigit, ErrorMessageResourceName = "DailyUnitInvalid", ErrorMessageResourceType = typeof(Resource))]
        public int DailyUnitLimit { get; set; }

        public int? UnitLimitFrequency { get; set; }
        [Ignore]
        public string UnitLimitFrequencyText
        {
            get { return Common.GetUnitLimitFrequency(UnitLimitFrequency); }
        }

        //[Required(ErrorMessage = "Taxonomy code required")]
        public long TaxonomyID { get; set; }
        #endregion

        public string AttachmentFileName { get; set; }
        public string AttachmentFilePath { get; set; }

        [Ignore]
        public string AllowedTimeType { get; set; }
        [Ignore]
        public int AuthType { get; set; }

        [Ignore]
        public string TempType { get; set; }

        [Ignore]
        public long PriorAuthorizationFrequencyType { get; set; }
        public string DxCode { get; set; }
        public string DxCodeID { get; set; }

        //[RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessageResourceName = "Ratemustbenumber", ErrorMessageResourceType = typeof(Resource))]
        public float PayRate { get; set; }

        public string FacilityCode { get; set; }
    }




    [TableName("ReferralBillingAuthorizationServiceCodes")]
    [PrimaryKey("ReferralBillingAuthorizationServiceCodeID")]
    [Sort("ReferralBillingAuthorizationID", "DESC")]
    public class ReferralBillingAuthorizationServiceCode : BaseEntity
    {
        public long ReferralBillingAuthorizationServiceCodeID { get; set; }
        public long ReferralBillingAuthorizationID { get; set; }
        public long ServiceCodeID { get; set; }

        [Required(ErrorMessageResourceName = "DailyUnitLimitRequired", ErrorMessageResourceType = typeof(Resource))]
        public int DailyUnitLimit { get; set; }

        [Required(ErrorMessageResourceName = "MaxUnitLimitRequired", ErrorMessageResourceType = typeof(Resource))]
        public int MaxUnitLimit { get; set; }


        [Ignore]
        [Required(ErrorMessageResourceName = "ServiceCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string StrServiceCodeIDs { get; set; }
    }


    public enum AuthType
    {
        CMS1500 = 1,
        UB04 = 2
    }
}
