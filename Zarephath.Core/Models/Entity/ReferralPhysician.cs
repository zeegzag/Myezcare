using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralPhysicians")]
    [PrimaryKey("ReferralPhysicianID")]
    [Sort("ReferralPhysicianID", "DESC")]
    public class ReferralPhysician : BaseEntity
    {
        public long ReferralPhysicianID { get; set; }
        public long ReferralID { get; set; }
        public long PhysicianID { get; set; }
        public bool IsDeleted { get; set; }
    }
}
