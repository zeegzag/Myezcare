using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("AuditLogTable")]
    [PrimaryKey("AuditLogID")]
    [Sort("AuditLogID", "DESC")]
    public class AuditLogTable : BaseEntity
    {
        public long AuditLogID { get; set; }
        public long ParentKeyFieldID { get; set; }
        public long? ChildKeyFieldID { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string DataModel { get; set; }
        public string ValueBefore { get; set; }
        public string ValueAfter { get; set; }
        public string Changes { get; set; }
        public string AuditActionType { get; set; }

    }


    public class AuditDeltaModel
    {
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string ValueBefore { get; set; }
        public string ValueAfter { get; set; }
    }
}
