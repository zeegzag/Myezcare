using System;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("NoteDXCodeMappings")]
    [PrimaryKey("NoteDXCodeMappingID")]
    [Sort("DXCodeName", "ASC")]
    public class NoteDxCodeMapping
    {
        public long NoteDXCodeMappingID { get; set; }
        public long ReferralDXCodeMappingID { get; set; }
        public long ReferralID { get; set; }
        public long NoteID { get; set; }
        public long DXCodeID { get; set; }
        public string DXCodeName { get; set; }
        public int Precedence { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string DXCodeWithoutDot { get; set; }
    }
}
