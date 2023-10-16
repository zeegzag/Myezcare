using System;
using System.Collections.Generic;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;

namespace Zarephath.Core.Models.ViewModel
{
    public class AttandanceMonthlySummaryModel
    {
        public AttandanceMonthlySummaryModel()
        {
            Attandance = new List<AttandanceNotificationEmailListModel>();
            NonAttandance = new List<AttandanceNotificationEmailListModel>();
        }
        public List<AttandanceNotificationEmailListModel> Attandance { get; set; }
        public List<AttandanceNotificationEmailListModel> NonAttandance { get; set; }
    }

    public class EmailWiseattandanceNotificationList
    {
        public string RecordRequestEmail { get; set; }
        public List<AttandanceNotificationEmailListModel> ClientList { get; set; }
        
    }


    public class AhcccsidWiseGroupList
    {
        public string AHCCCSID { get; set; }
        public string SentEmailTemplate { get; set; }
        public List<AttandanceNotificationEmailListModel> Client { get; set; }
    }

    public class AttandanceNotificationEmailListModel
    {
        public long ReferralID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AHCCCSID { get; set; }
        public DateTime Dob { get; set; }
        public string RecordRequestEmail { get; set; }
        public string CaseManager { get; set; }
        public string ClientName { get; set; }

        //public string ClientName
        //{
        //    get { return Common.GetGeneralNameFormat(FirstName, LastName); }
        //}

        public string CompletedBy { get; set; }
        public long ReferralMonthlySummariesID { get; set; }
        public string Medication { get; set; }
        public string Breakfast { get; set; }
        public string BreakfastTxt { get; set; }
        public string Lunch { get; set; }
        public string LunchTxt { get; set; }
        public string Dinner { get; set; }
        public string DinnerTxt { get; set; }
        public string MoodforThroughoutWeekend { get; set; }
        public string MoodforThroughoutWeekendTxt { get; set; }
        public string OutingDetails { get; set; }
        public string PCIInformation { get; set; }
        public string TreatmentPlan { get; set; }
        public string MedicationsDispensed { get; set; }
        public string MedicationsDispensedTxt { get; set; }
        public string Nextvisit { get; set; }
        public string CurrentServicePlan { get; set; }
        public string BelongingsandMedicationsReturned { get; set; }

        public string CoordinationofCareatPickup { get; set; }
        public string CoordinationofCareatPickupOption { get; set; }
        public string CoordinationofCareatPickupTxt { get; set; }

        public string CoordinationofCareatDropOff { get; set; }
        public string CoordinationofCareatDropOffOption { get; set; }
        public string CoordinationofCareatDropOffTxt { get; set; }

        public string MonthlySummaryDate { get; set; }
        public string MonthlySummaryStartDate { get; set; }
        public string MonthlySummaryEndDate { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedDate { get; set; }
        
        public string MoodforThroughoutWeekendExplanation { get; set; }
        public string MedicationsDispensedExplanation { get; set; }
        public string CoordinationofCareatPickupExplanation { get; set; }
        public string CoordinationofCareatDropOffExplanation { get; set; }
        
        public string ZerpathLogoImage
        {
            get
            {
                CacheHelper cacheHelper = new CacheHelper();
                return "<img src='" + cacheHelper.SiteBaseURL + Constants.ZerpathLogoImage + "' width='150px' style='float:right;'/>";
            }
        }

        public string AttendStatus { get; set; }

        public string Signature { get; set; }
        public string ImageText { get; set; }

        
    }
}
