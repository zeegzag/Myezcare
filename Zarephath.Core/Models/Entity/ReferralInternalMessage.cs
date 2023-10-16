using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralInternalMessages")]
    [PrimaryKey("ReferralInternalMessageID")]
    [Sort("ReferralInternalMessageID", "DESC")]
    public class ReferralInternalMessage : BaseEntity
    {
        public long ReferralInternalMessageID { get; set; }

        [Required(ErrorMessageResourceName = "MessageRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Note { get; set; }
        public long ReferralID { get; set; }

        [Required(ErrorMessageResourceName = "AssigneeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long Assignee { get; set; }
        public bool IsResolved { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? ResolveDate { get; set; }

        public string ResolvedComment { get; set; }

        public bool MarkAsResolvedRead { get; set; }

        [Ignore]
        public string EncryptedReferralID { get; set; }
    }
}
