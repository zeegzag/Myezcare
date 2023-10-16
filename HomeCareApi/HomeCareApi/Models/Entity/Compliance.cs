using PetaPoco;
using HomeCareApi.Infrastructure.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeCareApi.Models.Entity
{
    [TableName("Compliances")]
    [PrimaryKey("ComplianceID")]
    [Sort("ComplianceID", "DESC")]
    public class Compliance
    {
        public long ComplianceID { get; set; }
        public int UserType { get; set; }
        public int DocumentationType { get; set; }
        public string DocumentName { get; set; }
        public bool IsTimeBased { get; set; }
        public bool IsDeleted { get; set; }
        public string EBFormID { get; set; }
        public long ParentID { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string SelectedRoles { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
        public string SystemID { get; set; }
    }
}