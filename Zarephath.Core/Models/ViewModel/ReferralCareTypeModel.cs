using PetaPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class RefCareTypeSlotsModel
    {
        public RefCareTypeSlotsModel()
        {
            ReferralList = new List<NameValueData>();
            CareTypeList = new List<CareType>();
            SearchCTSchedule = new SearchCTSchedule();
            AddTimeSlots = new CareTypeTimeSlot();
        }
        public List<NameValueData> ReferralList { get; set; }
        public List<CareType> CareTypeList { get; set; }
        [Ignore]
        public SearchCTSchedule SearchCTSchedule { get; set; }
        [Ignore]
        public CareTypeTimeSlot AddTimeSlots { get; set; }
        [Ignore]
        public bool IsPartial { get; set; }
    }

    public class SearchCTSchedule
    {
        public long ReferralID { get; set; }
        public int Frequency { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class ListCareTypeSchedule
    {
        public long CareTypeTimeSlotID { get; set; }
        public long ReferralID { get; set; }
        public long CareTypeID { get; set; }
        public string CareTypeName { get; set; }
        public string Name { get; set; }
        public int NumOfTime { get; set; }
        public int Frequency { get; set; }
        public string FrequencyValue { get { return Enum.GetName(typeof(Common.CareTypeFrequency), Frequency); } }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsDeleted { get; set; }
        public int Count { get; set; }

    }

    public class CertificateAuthorityModel
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }
}
