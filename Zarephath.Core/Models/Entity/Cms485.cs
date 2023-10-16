using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("CMS485")]
    [PrimaryKey("Cms485ID")]
    [Sort("Cms485ID", "DESC")]

    public class Cms485 : BaseEntity
    {
        public long Cms485ID { get; set; }
        public string JsonData { get; set; }
        public long EmployeeID { get; set; }
        public long ReferralID { get; set; }
        public bool IsDeleted { get; set; }
    }
}
