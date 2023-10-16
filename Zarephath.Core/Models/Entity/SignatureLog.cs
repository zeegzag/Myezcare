using System;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("SignatureLogs")]
    [PrimaryKey("SignatureLogID")]
    [Sort("SignatureLogID", "DESC")]
    public class SignatureLog
    {
        public long SignatureLogID { get; set; }
        public long NoteID { get; set; }
        public string Signature { get; set; }
        public long EmployeeSignatureID { get; set; }
        public long SignatureBy { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public string MacAddress { get; set; }

        [SetIp]
        public string SystemID { get; set; }
        public bool IsActive { get; set; }
    }
}
