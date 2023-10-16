using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ClaimStatusCodes")]
    [PrimaryKey("ClaimStatusCodeID")]
    [Sort("ClaimStatusCodeID", "DESC")]
    public class ClaimStatusCode
    {

        public long ClaimStatusCodeID { get; set; }
        public string ClaimStatusName { get; set; }
        public string ClaimStatusCodeDescription { get; set; }
        public bool IsDeleted { get; set; }

    }
}
