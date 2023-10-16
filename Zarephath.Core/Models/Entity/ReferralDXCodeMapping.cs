using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralDXCodeMappings")]
    [PrimaryKey("ReferralDXCodeMappingID")]
    [Sort("ReferralDXCodeMappingID", "DESC")]
    public class ReferralDXCodeMapping : BaseEntity
    {
        public long ReferralDXCodeMappingID { get; set; }
        public long ReferralID { get; set; }
        [GetTableNameAttribute("DXCodes")]
        [Display(ResourceType = typeof(Resource), Name = "DXCode")]
        public string DXCodeID { get; set; }
        public int? Precedence { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "StartDate")]
        public DateTime? StartDate { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "EndDate")]
        public DateTime? EndDate { get; set; }
    }
}
