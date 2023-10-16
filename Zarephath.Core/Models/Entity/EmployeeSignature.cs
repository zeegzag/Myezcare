using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("EmployeeSignatures")]
    [PrimaryKey("EmployeeSignatureID")]
    [Sort("EmployeeSignatureID", "DESC")]
    public class EmployeeSignature
    {
        public long EmployeeSignatureID { get; set; }
        public string SignaturePath { get; set; }
        public long EmployeeID { get; set; }
    }
}
