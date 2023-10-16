using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("LU_OutcomeMeasurementOptions")]
    [PrimaryKey("OutcomeMeasurementOptionID")]
    [Sort("OutcomeMeasurementOptionID", "ASC")]
    public class LU_OutcomeMeasurementOption
    {
        public int OutcomeMeasurementOptionID { get; set; }
        public string OutcomeMeasurementOption { get; set; }
        public enum OutcomeMeasurementOptions
        {
            Yes = 1,
            MostlyYes,
            Somewhat,
            Mostly_No,
            No
        }
    }
}
