using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Controllers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralSuspentions")]
    [PrimaryKey("ReferralSuspentionID")]
    [Sort("ReferralSuspentionID", "DESC")]
    public class ReferralSuspention : BaseEntity
    {
        public long ReferralSuspentionID { get; set; }

        [Required(ErrorMessageResourceName = "SuspentionTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string SuspentionType { get; set; }

        [Required(ErrorMessageResourceName = "SuspentionLengthRequired", ErrorMessageResourceType = typeof(Resource))]
        public int SuspentionLength { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? ReturnEligibleDate { get; set; }

        public long ReferralID { get; set; }

        public bool IsDeleted { get; set; }

        [Ignore]
        public long EncryptedReferralID { get; set; }

        [Ignore]
        public bool MakeClientInActive { get; set; }

    }
}
