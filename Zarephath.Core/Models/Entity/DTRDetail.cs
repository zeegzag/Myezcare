using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("DTRDetails")]
    [PrimaryKey("DTRDetailID")]
    [Sort("DTRDetailID", "DESC")]
    public class DTRDetail : BaseEntity
    {
        public long DTRDetailID { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleType { get; set; }
        public string LocationAddress { get; set; }
        public int DTRDetailType { get; set; }
        public bool  IsDeleted { get; set; }
        public int OrderNumber { get; set; }


        public enum DTRDetailTypes
        {
            Vehicle = 1,
            VehicleType,
            LocationAddress,
            Others
        }
    }
}
