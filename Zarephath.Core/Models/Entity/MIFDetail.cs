using PetaPoco;
using System;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("MIFDetails")]
    [PrimaryKey("MIFFormID")]
    [Sort("MIFFormID", "DESC")]
    public class MIFDetail: BaseEntity
    {
        public long MIFFormID { get; set; }
        public long ReferralID { get; set; }
        public string FormName { get; set; }
        public string FromWhoToWho { get; set; }
        public string Type { get; set; }
        public bool? IsResponseRequired { get; set; }

        public bool ServiceType_AHD { get; set; }
        public bool ServiceType_ALS { get; set; }
        public bool ServiceType_ERS { get; set; }
        public bool ServiceType_HDM { get; set; }
        public bool ServiceType_HDS { get; set; }
        public bool ServiceType_PSS { get; set; }
        public bool ServiceType_EPS { get; set; }

        public bool? IsInitialServiceOffered { get; set; }
        public string InitialServiceNoReason { get; set; }
        public DateTime? InitialServiceDate { get; set; }
        public long InitialServiceFrequencyID { get; set; }

        public bool ChangeFYI_RecommendationForChange { get; set; }
        public bool ChangeFYI_ChangeInHealthFuncStatus { get; set; }
        public bool ChangeFYI_Hospitalization { get; set; }
        public bool ChangeFYI_ServiceNotDelivered { get; set; }
        public bool ChangeFYI_ChangeInFrequencyByCM { get; set; }
        public bool ChangeFYI_ChangeInPhysician { get; set; }
        public bool ChangeFYI_Other { get; set; }
        public bool ChangeFYI_FYI { get; set; }

        public string Explanation { get; set; }
        public DateTime? EffectiveDateOfChange { get; set; }
        public string DischargeReason { get; set; }
        public DateTime? DateOfDischarge { get; set; }
        public string Comments { get; set; }
        public DateTime? PriorAuthorizationDateFrom { get; set; }
        public DateTime? PriorAuthorizationDateTo { get; set; }
        public string PriorAuthorizationNo { get; set; }
        public string SignaturePath { get; set; }
        public bool IsDeleted { get; set; }

        [Ignore]
        public string TempSignaturePath { get; set; }
        [Ignore]
        public string EncryptedMIFFormID { get { return Crypto.Encrypt(Convert.ToString(MIFFormID)); } }
        public string CreatedByName { get; set; }
    }
}
