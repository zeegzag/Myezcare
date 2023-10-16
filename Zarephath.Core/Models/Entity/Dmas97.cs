using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("DMAS97AB")]
    [PrimaryKey("Dmas97ID")]
    [Sort("Dmas97ID", "DESC")]

    public class Dmas97 : BaseEntity
    {
        public long Dmas97ID { get; set; }
        public string JsonData { get; set; }
        public long EmployeeID { get; set; }
        public long ReferralID { get; set; }
        public bool IsDeleted { get; set; }
    }
}
