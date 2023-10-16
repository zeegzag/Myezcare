using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using PetaPoco;
using Zarephath.Core.Controllers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralBillingSettings")]
    [PrimaryKey("ReferralBillingSettingID")]
    [Sort("ReferralBillingSettingID", "DESC")]
    public class ReferralBillingSetting : BaseEntity
    {
        public long ReferralBillingSettingID { get; set; }

        public long ReferralID { get; set; }

        //[Required(ErrorMessageResourceName = "ProfessionalAuthrizationCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string AuthrizationCode_CMS1500 { get; set; }

        //[Required(ErrorMessageResourceName = "FacilityCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int? POS_CMS1500 { get; set; }



        //[Required(ErrorMessageResourceName = "InstitutionalAuthrizationCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string AuthrizationCode_UB04 { get; set; }

        //[Required(ErrorMessageResourceName = "FacilityCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int? POS_UB04 { get; set; }

        //[Required(ErrorMessageResourceName = "AdmissionTypeCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int? AdmissionTypeCode_UB04 { get; set; }

        //[Required(ErrorMessageResourceName = "AdmissionSourceCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int? AdmissionSourceCode_UB04 { get; set; }

        //[Required(ErrorMessageResourceName = "PatientStatusCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int? PatientStatusCode_UB04 { get; set; }


        [Ignore]
        public string EncryptedReferralId { get; set; }
        [Ignore]
        public int AuthrizationCodeType { get; set; }
    }

    public enum AuthrizationCodeType
    {
        Professional = 1,
        Institutional = 2
    }
}
