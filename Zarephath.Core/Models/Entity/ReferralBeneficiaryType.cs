using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralBeneficiaryTypes")]
    [PrimaryKey("ReferralBeneficiaryTypeID")]
    [Sort("ReferralBeneficiaryTypeID", "DESC")]
    public class ReferralBeneficiaryType : BaseEntity
    {
        public long ReferralBeneficiaryTypeID { get; set; }
        public long ReferralID { get; set; }
        public long BeneficiaryTypeID { get; set; }
        public string BeneficiaryNumber { get; set; }
        public bool IsDeleted { get; set; }
    }
}
