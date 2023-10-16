using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralMonthlySummaries")]
    [PrimaryKey("ReferralMonthlySummariesID")]
    [Sort("ReferralMonthlySummariesID", "DESC")]
    public class ReferralMonthlySummary : BaseEntity
    {
        public ReferralMonthlySummary()
        {
            BelongingsandMedicationsReturned = true;
            CurrentServicePlan = true;
        }

        public long ReferralMonthlySummariesID { get; set; }

        public long ReferralID { get; set; }

        public bool Medication { get; set; }

        public string Breakfast { get; set; }
        public string BreakfastTxt { get; set; }

        public string Lunch { get; set; }
        public string LunchTxt { get; set; }

        public string Dinner { get; set; }
        public string DinnerTxt { get; set; }

        
        public string MoodforThroughoutWeekend { get; set; }

        public string MoodforThroughoutWeekendTxt { get; set; }

        [Required(ErrorMessageResourceName = "OutingDetailsRequired", ErrorMessageResourceType = typeof(Resource))]
        public string OutingDetails { get; set; }

        [Required(ErrorMessageResourceName = "PCIInformationRequired", ErrorMessageResourceType = typeof(Resource))]
        public string PCIInformation { get; set; }

        [Required(ErrorMessageResourceName = "TreatmentPlanRequired", ErrorMessageResourceType = typeof(Resource))]
        public string TreatmentPlan { get; set; }

        public bool MedicationsDispensed { get; set; }
        public string MedicationsDispensedTxt { get; set; }

        public string Nextvisit { get; set; }
        public bool CurrentServicePlan { get; set; }

        public bool BelongingsandMedicationsReturned { get; set; }

        [Required(ErrorMessageResourceName = "CoordinationofCareatPickupRequired", ErrorMessageResourceType = typeof(Resource))]
        public string CoordinationofCareatPickup { get; set; }
        public string CoordinationofCareatPickupOption { get; set; }
        public string CoordinationofCareatPickupTxt { get; set; }

        [Required(ErrorMessageResourceName = "CoordinationofCareatDropOffRequired", ErrorMessageResourceType = typeof(Resource))]
        public string CoordinationofCareatDropOff { get; set; }
        public string CoordinationofCareatDropOffOption { get; set; }
        public string CoordinationofCareatDropOffTxt { get; set; }


        [Required(ErrorMessageResourceName = "ReferralMonthlySummaryDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime MonthlySummaryStartDate { get; set; }

        [Required(ErrorMessageResourceName = "ReferralMonthlySummaryDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime MonthlySummaryEndDate { get; set; }

        [Required(ErrorMessageResourceName = "FacilityRequired", ErrorMessageResourceType = typeof(Resource))]
        public long FacilityID { get; set; }

        public bool IsDeleted { get; set; }


        [ResultColumn]
        public string CreatedName { get; set; }

        [ResultColumn]
        public string Signature { get; set; }

        [ResultColumn]
        public string CompletedBy { get; set; }




        [ResultColumn]
        public string ClientName { get; set; }
        [ResultColumn]
        public string AHCCCSIdNumber { get; set; }
        [ResultColumn]
        public string CaseManager { get; set; }



        [Ignore]
        public string Name { get; set; }

        [Ignore]
        public string AHCCCSID { get; set; }

        [Ignore]
        public List<string> BreakfastIds { get; set; }

        [Ignore]
        public List<string> LunchIds { get; set; }

        [Ignore]
        public List<string> DinnerIds { get; set; }

        [Ignore]
        [Required(ErrorMessageResourceName = "PleaseSelectAtleast1Box", ErrorMessageResourceType = typeof(Resource))]
        public List<string> MoodforThroughoutWeekendIds { get; set; }

        [Ignore]
        [Required(ErrorMessageResourceName = "PleaseSelectAtleast1Box", ErrorMessageResourceType = typeof(Resource))]
        public List<string> CoordinationofCareatPickupIds { get; set; }

        [Ignore]
        [Required(ErrorMessageResourceName = "PleaseSelectAtleast1Box", ErrorMessageResourceType = typeof(Resource))]
        public List<string> CoordinationofCareatDropOffIds { get; set; }



        [Ignore]
        public bool IsMonthlySummaryListView { get; set; }

    }

    //public enum EnumThroughoutWeekend
    //{
    //    [Display(ResourceType = typeof(Resource), Name = "Happy")]
    //    Happy = 1,
    //    [Display(ResourceType = typeof(Resource), Name = "Sad")]
    //    Sad = 2,
    //    [Display(ResourceType = typeof(Resource), Name = "Quiet")]
    //    Quiet = 3,
    //    [Display(ResourceType = typeof(Resource), Name = "Talkative")]
    //    Talkative = 4,
    //    [Display(ResourceType = typeof(Resource), Name = "Sleepy")]
    //    Sleepy = 5,
    //    [Display(ResourceType = typeof(Resource), Name = "FeelingSick")]
    //    Feeling_Sick = 6,
    //    [Display(ResourceType = typeof(Resource), Name = "Helpful")]
    //    Helpful = 7,
    //    [Display(ResourceType = typeof(Resource), Name = "Whiny")]
    //    Whiny = 8,
    //    [Display(ResourceType = typeof(Resource), Name = "Overactive")]
    //    Overactive = 9,
    //    [Display(ResourceType = typeof(Resource), Name = "Bossy")]
    //    Bossy = 10,
    //    [Display(ResourceType = typeof(Resource), Name = "Aggressive")]
    //    Aggressive = 11,
    //    [Display(ResourceType = typeof(Resource), Name = "onwStaff")]
    //    onwStaff = 12,

    //    [Display(ResourceType = typeof(Resource), Name = "Playful")]
    //    Playful = 13,

    //    [Display(ResourceType = typeof(Resource), Name = "Demanding")]
    //    Demanding = 14,

    //    [Display(ResourceType = typeof(Resource), Name = "Cuddly")]
    //    Cuddly = 15,

    //    [Display(ResourceType = typeof(Resource), Name = "Silly")]
    //    Silly = 16,
    //    [Display(ResourceType = typeof(Resource), Name = "Angry")]
    //    Angry = 17,
    //    [Display(ResourceType = typeof(Resource), Name = "Inquisitive")]
    //    Inquisitive = 18,
    //    [Display(ResourceType = typeof(Resource), Name = "Sociable")]
    //    Sociable = 19,

    //    [Display(ResourceType = typeof(Resource), Name = "Excitable")]
    //    Excitable = 20,

    //    [Display(ResourceType = typeof(Resource), Name = "OtherDetails")]
    //    Other_Details = 21,
    //}

    //public enum EnumCoordinationofCare
    //{
    //    [Display(ResourceType = typeof(Resource), Name = "InPerson")]
    //    In_Person = 1,
    //    [Display(ResourceType = typeof(Resource), Name = "ViaPhone")]
    //    Via_Phone = 2,
    //    [Display(ResourceType = typeof(Resource), Name = "MedicationLable")]
    //    Medication = 3,
    //    [Display(ResourceType = typeof(Resource), Name = "BXReport")]
    //    BX_Report = 4,
    //    [Display(ResourceType = typeof(Resource), Name = "Other")]
    //    Other = 5
    //}

}
