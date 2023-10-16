using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("TransportationFilters")]
    [PrimaryKey("TransportationFilterID")]
    [Sort("TransportationFilterID", "DESC")]
    public class TransportationFilter
    {
        public long TransportationFilterID { get; set; }
        public string TransportationFilterName { get; set; }
        public string ShortName { get; set; }
        public bool IsDeleted { get; set; }

        public enum TransportationFilters
        {
            Booster_Seat=1,
            Private_Room,
            Car_Seat,
            Saturday_Only,
            Consent_Packet_Service_Plan,
            DTR_Fri_Sun,
            DTR_Life_Skills
        }
    }
}
