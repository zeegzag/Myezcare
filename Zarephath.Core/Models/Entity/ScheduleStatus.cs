using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ScheduleStatuses")]
    [PrimaryKey("SchedukeStatusID")]
    [Sort("SchedukeStatusName", "ASC")]
    public class ScheduleStatus
    {
        public int ScheduleStatusID { get; set; }
        public string ScheduleStatusName { get; set; }

        
        public enum ScheduleStatuses
        {
            Unconfirmed=1,
            Confirmed,
            Left_Message,
            No_Answer,
            No_Phone,
            Cancelled,
            No_Confirmation,
            Waiting_List,
            No_Show
        }
    }
}
