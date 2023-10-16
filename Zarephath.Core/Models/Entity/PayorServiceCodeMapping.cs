using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;
using ExpressiveAnnotations.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("PayorServiceCodeMapping")]
    [PrimaryKey("PayorServiceCodeMappingID")]
    [Sort("PayorServiceCodeMappingID", "DESC")]
    public class PayorServiceCodeMapping
    {
        public long PayorServiceCodeMappingID { get; set; }

        public long PayorID { get; set; }

        [Required(ErrorMessageResourceName = "CareTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long CareType { get; set; }
        
        [Required(ErrorMessageResourceName = "ServiceCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        [Range(1, long.MaxValue, ErrorMessageResourceName = "ServiceCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long ServiceCodeID { get; set; }

        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? POSStartDate { get; set; }

        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? POSEndDate { get; set; }

        //[Required(ErrorMessageResourceName = "RevenueCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? RevenueCode { get; set; }

        //[Required(ErrorMessageResourceName = "UMRequired", ErrorMessageResourceType = typeof(Resource))]
        //public long UM { get; set; }

        //[Required(ErrorMessageResourceName = "NegRateRequired", ErrorMessageResourceType = typeof(Resource))]
        //[RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessageResourceName = "NegRatemustbenumber", ErrorMessageResourceType = typeof(Resource))]
        //public float NegRate { get; set; }

        //[Required(ErrorMessageResourceName = "UMRequired", ErrorMessageResourceType = typeof(Resource))]
        //public long UM { get; set; }

        public bool IsDeleted { get; set; }

        [Ignore]
        public string EncryptedPayorId { get; set; }


        public long? ModifierID { get; set; }

        [Required(ErrorMessageResourceName = "RateRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessageResourceName = "Ratemustbenumber", ErrorMessageResourceType = typeof(Resource))]
        public float Rate { get; set; }

        //[Required(ErrorMessageResourceName = "POSRequired", ErrorMessageResourceType = typeof(Resource))]
        public long PosID { get; set; }

        //[RegularExpression(Constants.RegxNumericSixDigit, ErrorMessageResourceName = "BillingUnitLimitInvalid", ErrorMessageResourceType = typeof(Resource))]
        public int? BillingUnitLimit { get; set; }

        [Required(ErrorMessageResourceName = "UnitTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int? UnitType { get; set; }

        [Required(ErrorMessageResourceName = "PerUnitQuantityRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxNumericSixDigit, ErrorMessageResourceName = "PerUnitQuantityInvalid", ErrorMessageResourceType = typeof(Resource))]
        public decimal? PerUnitQuantity { get; set; }

        [Required(ErrorMessageResourceName = "RoundUpUnitRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxNumericSixDigit, ErrorMessageResourceName = "RoundUpUnitInvalid", ErrorMessageResourceType = typeof(Resource))]
        [AssertThat("RoundUpUnit <= PerUnitQuantity", ErrorMessageResourceName = "RoundUpUnitLessThanValidation", ErrorMessageResourceType = typeof(Resource))]
        public int? RoundUpUnit { get; set; }

        [Required(ErrorMessageResourceName = "MaxUnitRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxNumericSixDigit, ErrorMessageResourceName = "MaxUnitInvalid", ErrorMessageResourceType = typeof(Resource))]
        public int? MaxUnit { get; set; }

        [Required(ErrorMessageResourceName = "DailyUnitRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxNumericSixDigit, ErrorMessageResourceName = "DailyUnitInvalid", ErrorMessageResourceType = typeof(Resource))]
        public int? DailyUnitLimit { get; set; }

        [Ignore]
        public int[] SelectedPayors { get; set; }
    }

    public enum EnumPlaceOfServices
    {
        [Display(ResourceType = typeof(Resource), Name = "CommunityMentalHealthCenter")]
        CommunityMentalHealthCenter = 53,
        [Display(ResourceType = typeof(Resource), Name = "Other99")]
        Confirmed = 99,
    }

    public enum ServiceCodeGroups
    {
        [Display(ResourceType = typeof(Resource), Name = "CM")]
        CM = 1,
        [Display(ResourceType = typeof(Resource), Name = "Resedential")]
        Resedential,
        //[Display(ResourceType = typeof(Resource), Name = "GroupNote")]
        //GroupNote,
        [Display(ResourceType = typeof(Resource), Name = "Other")]
        Other
    }

    public enum EnumModifiers
    {
        [Display(ResourceType = typeof(Resource), Name = "HN")]
        HN = 1,
        [Display(ResourceType = typeof(Resource), Name = "HO")]
        HO = 2,
        [Display(ResourceType = typeof(Resource), Name = "HQ")]
        HQ = 3,
        [Display(ResourceType = typeof(Resource), Name = "HR")]
        HR = 4,
        [Display(ResourceType = typeof(Resource), Name = "HS")]
        HS = 5,
    }

    public static class GetModifiersName
    {
        public static string GetName(long modifierId)
        {
            return ((EnumModifiers)modifierId).ToString();
        }


    }
}
