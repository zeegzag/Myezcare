using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Agencies")]
    [PrimaryKey("AgencyID")]
    [Sort("NickName", "ASC")]
    public class AgencyDropDownModel
    {
        public long AgencyID { get; set; }
        public string NickName { get; set; }    

    }
	
	
}
