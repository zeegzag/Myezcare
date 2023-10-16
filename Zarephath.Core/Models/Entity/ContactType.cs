using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ContactTypes")]
    [PrimaryKey("ContactTypeID")]
    [Sort("ContactTypeID", "DESC")]
    public class ContactType
    {
        public long ContactTypeID { get; set; }
        public string ContactTypeName { get; set; }
        public int OrderNumber { get; set; }
        public bool IsDeleted { get; set; }

        public enum ContactTypes
        {
            Primary_Placement = 1,
            Legal_Guardian,
            Secondary_Placement,
            Relative,
            School_Teacher,
            Relative2,
            EmployeePrimary_Placement = 7
        }
    }
}
