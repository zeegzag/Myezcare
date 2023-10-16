using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralCaseloads")]
    [PrimaryKey("ReferralCaseloadID")]
    [Sort("ReferralCaseloadID", "DESC")]
    public class ReferralCaseload : BaseEntity
    {
        public long ReferralCaseloadID { get; set; }
        
        public long ReferralID { get; set; }

        public long EmployeeID { get; set; }

        public string CaseLoadType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsDeleted { get; set; }         
    }
}
