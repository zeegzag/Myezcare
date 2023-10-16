using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Payors")]
    [PrimaryKey("PayorID")]
    [Sort("PayorID", "DESC")]
    public class Payor : BaseEntity
    {
        public long PayorID { get; set; }

        //Resource.PayorAlreadyExist
        [Unique("Payor name is already exist")]
        [Required(ErrorMessageResourceName = "PayorNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string PayorName { get; set; }

        //Resource.PayorShortNameAlreadyExist
        [Unique("Payor short name is already exist.")]
        [Required(ErrorMessageResourceName = "ShortNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ShortName { get; set; }

        public string PayorSubmissionName { get; set; }

        [Required(ErrorMessageResourceName = "PayorIdentificationNumberRequired", ErrorMessageResourceType = typeof(Resource))]
        public string PayorIdentificationNumber { get; set; }

        [Required(ErrorMessageResourceName = "AddressRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Address { get; set; }

        [Required(ErrorMessageResourceName = "CityRequired", ErrorMessageResourceType = typeof(Resource))]
        public string City { get; set; }

        [Required(ErrorMessageResourceName = "StateRequired", ErrorMessageResourceType = typeof(Resource))]
        public string StateCode { get; set; }

        [Display(Name = "ZipCode", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "ZipCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ZipCode { get; set; }

        [Required(ErrorMessageResourceName = "PayorTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long PayorTypeID { get; set; }

        [Required(ErrorMessageResourceName = "PayorBillingTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string PayorBillingType { get; set; }

        [Required(ErrorMessageResourceName = "PayorInvoiceTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string PayorInvoiceType { get; set; } //TODO: this is int datatype but due to used in dropdown i have converted string

        public long? BillingProviderID { get; set; }
        public long? RenderingProviderID { get; set; }


        public bool IsBillingActive { get; set; }

        public bool IsDeleted { get; set; }

        [Required(ErrorMessageResourceName = "PayorIdRequired", ErrorMessageResourceType = typeof(Resource))]
        public string AgencyNPID { get; set; }

        public string AgencyTaxNumber { get; set; }

        public int NPIOption { get; set; }

        public string Taxenomy { get; set; }

        public int PayerGroup { get; set; }

        public int BussinessLine { get; set; }
        public long NPINumber { get; set; }
        public string ClaimProcessor { get; set; }
        public string VisitBilledBy { get; set; }
        [Ignore]
        public string EncryptedPayorID { get; set; }

        [Required(ErrorMessageResourceName = "PayorIdentificationNumberRequired", ErrorMessageResourceType = typeof(Resource))]
        public string EraPayorID { get; set; }




        public string ERAEnroll_Status { get; set; }
        public string ERAEnroll_Log { get; set; }
        public string CMS1500Enroll_Status { get; set; }
        public string CMS1500Enroll_Log { get; set; }


        public enum PayorCode
        {
            CAZ = 1,
            MMIC = 2,
            PY = 3,
            UHC = 4,
            Pro_Bono = 5,
            HCIC = 6,
            NoPayor = 0
        }

    }
}
