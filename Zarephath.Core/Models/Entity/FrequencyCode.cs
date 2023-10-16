using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("FrequencyCodes")]
    [PrimaryKey("FrequencyCodeID")]
    [Sort("FrequencyCodeID", "DESC")]
    public class FrequencyCode
    {
        public long FrequencyCodeID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public enum FrequencyCodes
        {
            OneWeek = 1,
            TwoWeek,
            ThreeWeek,
            FourWeek,
            OneDay,
            TwoDay,
            ThreeDay,
            FourDay,
            RequestOnly,
            DNS,
            OneOvernight,
            TwoOvernight,
            ThreeOvernight,
            FourOvernight,
            SaturdayOnly,
            OneWeekendADay,
            ZeroToFive,
            Daily,
            Weekly,
            Monthly

        }
    }
}
