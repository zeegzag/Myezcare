using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralTimeSlotDates")]
    [PrimaryKey("ReferralTSDateID")]
    [Sort("ReferralTSDateID", "DESC")]
    public class ReferralTimeSlotDates : BaseEntity
    {
        public long ReferralTSDateID { get; set; }

        public long ReferralID { get; set; }

        public long ReferralTimeSlotMasterID { get; set; }

        public DateTime ReferralTSDate { get; set; }

        public DateTime ReferralTSStartTime { get; set; }

        public DateTime ReferralTSEndTime { get; set; }

        public bool UsedInScheduling { get; set; }
        public string Notes { get; set; }

        public int DayNumber { get; set; }
        public long ReferralTimeSlotDetailID { get; set; }
        public bool OnHold { get; set; }
        public long ReferralOnHoldDetailID { get; set; }
        public bool IsDenied { get; set; }

    }
}
