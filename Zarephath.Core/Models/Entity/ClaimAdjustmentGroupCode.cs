using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ClaimAdjustmentGroupCodes")]
    [PrimaryKey("ClaimAdjustmentGroupCodeID")]
    [Sort("ClaimAdjustmentGroupCodeID", "DESC")]
    public class ClaimAdjustmentGroupCode
    {
         public string ClaimAdjustmentGroupCodeID { get; set; }
         public string ClaimAdjustmentGroupCodeName { get; set; }
         public string ClaimAdjustmentGroupCodeDescription { get; set; }
         public bool IsDeleted { get; set; }

    }
}
