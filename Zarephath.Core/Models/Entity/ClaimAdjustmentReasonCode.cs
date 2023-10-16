using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ClaimAdjustmentReasonCodes")]
    [PrimaryKey("ClaimAdjustmentReasonCodeID")]
    [Sort("ClaimAdjustmentReasonCodeID", "DESC")]
    public class ClaimAdjustmentReasonCode
    {
        public string ClaimAdjustmentReasonCodeID { get; set; }
        public string ClaimAdjustmentReasonDescription { get; set; }
        public string ClaimType { get; set; }
        public string IsDeleted { get; set; }
        public long OrderID { get; set; }
    }
}
