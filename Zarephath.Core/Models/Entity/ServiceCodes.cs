using System;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ServiceCodes")]
    [PrimaryKey("ServiceCodeID")]
    [Sort("ServiceCodeID", "DESC")]
    public class ServiceCodes
    {
        public long ServiceCodeID { get; set; }
        public string ModifierID { get; set; }

        [Required(ErrorMessageResourceName = "ServiceCodeTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int ServiceCodeType { get; set; }

        [Required(ErrorMessageResourceName = "CareTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long CareType { get; set; }

        [Required(ErrorMessageResourceName = "ServiceCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ServiceCode { get; set; }

        [Required(ErrorMessageResourceName = "ServiceNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ServiceName { get; set; }

        [Required(ErrorMessageResourceName = "DescriptionRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "UnitTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int UnitType { get; set; }


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

        public bool IsBillable { get; set; }
        public bool IsDeleted { get; set; }
        public string RandomGroupID { get; set; }

        [ResultColumn]
        public string ModifierName { get; set; }

        [Ignore]
        public string EncryptedServiceCodeID { get { return Crypto.Encrypt(Convert.ToString(ServiceCodeID)); } }


        [Required(ErrorMessageResourceName = "DefaultUnitRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxNumericSixDigit, ErrorMessageResourceName = "DefaultUnitInvalid", ErrorMessageResourceType = typeof(Resource))]
        public int DefaultUnitIgnoreCalculation { get; set; }

        public bool HasGroupOption { get; set; }

        public bool CheckRespiteHours { get; set; }

        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? ServiceCodeStartDate { get; set; }

        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? ServiceCodeEndDate { get; set; }

        public string AccountCode { get; set; }
        public long VisitTypeId { get; set; }

        [ResultColumn]
        public string CareTypeTitle { get; set; }



        //[Ignore]
        //public bool CheckRoundUpUnit
        //{
        //    get
        //    {
        //        return (this.RoundUpUnit <= this.PerUnitQuantity);
        //    }
        //}
    }

    public enum EnumServiceCodeType
    {
        [Display(ResourceType = typeof(Resource), Name = "CM")]
        CM = 1,
        [Display(ResourceType = typeof(Resource), Name = "Resedential")]
        Resedential = 2,
        [Display(ResourceType = typeof(Resource), Name = "Other")]
        Other = 3
    }

    public enum EnumCareType
    {
        [Display(ResourceType = typeof(Resource), Name = "PCA")]
        PCA = 1,
        [Display(ResourceType = typeof(Resource), Name = "Respite")]
        Respite = 2,
    }

    public enum EnumUnitType
    {
        [Display(ResourceType = typeof(Resource), Name = "Time")]
        Time = 1,
        [Display(ResourceType = typeof(Resource), Name = "Visit")]
        Visit = 2,
        [Display(ResourceType = typeof(Resource), Name = "DistanceInMiles")]
        DistanceInMiles = 3,
        [Display(ResourceType = typeof(Resource), Name = "Stop")]
        Stop = 4,
        [Display(ResourceType = typeof(Resource), Name = "UnitTypePerDay_FlatRate")]
        PerDay_FlatRate = 5

    }

}
