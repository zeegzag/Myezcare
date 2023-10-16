using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("EmployeePreferences")]
    [PrimaryKey("EmployeePreferenceID")]
    [Sort("EmployeePreferenceID", "DESC")]
    public class EmployeePreference
    {
        public long EmployeePreferenceID { get; set; }
        public long EmployeeID { get; set; }
        public long PreferenceID { get; set; }

        //[Ignore]
        //public string PreferenceName { get; set; }
    }
}
