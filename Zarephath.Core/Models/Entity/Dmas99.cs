using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("DMAS99")]
    [PrimaryKey("Dmas99ID")]
    [Sort("Dmas99ID", "DESC")]

    public class Dmas99 : BaseEntity
    {
        public long Dmas99ID { get; set; }
        public string JsonData { get; set; }
        public long EmployeeID { get; set; }
        public long ReferralID { get; set; }
        public bool IsDeleted { get; set; }
    }
}
