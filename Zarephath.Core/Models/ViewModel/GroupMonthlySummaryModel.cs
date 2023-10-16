using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using PetaPoco;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class GroupMonthlySummaryModel
    {

        public GroupMonthlySummaryModel()
        {
            SearchClientForMonthlySummary = new SearchClientForMonthlySummary();
            CommonMonthlySummary = new ReferralMonthlySummary();
            Coordinationofcare=new List<NameValueDataInString>();
            SignatureDetails = new SignatureDetails();
        }
        public List<NameValueData> Facilities { get; set; }
        public List<NameValueData> AllFacilities { get; set; }
        public List<NameValueData> Payors { get; set; }
        public SignatureDetails SignatureDetails { get; set; }


        [Ignore]
        public SearchClientForMonthlySummary SearchClientForMonthlySummary { get; set; }

        [Ignore]
        public ReferralMonthlySummary CommonMonthlySummary { get; set; }


        [Ignore]
        public ReferralMonthlySummary MonthlySummaryModel { get; set; }


        [Ignore]
        public List<NameValueDataInString> BindMealsandSummaryofFood { get; set; }

        [Ignore]
        public List<NameValueDataInString> EnumCoordinationofCare { get; set; }

        [Ignore]
        public List<NameValueDataInString> SummaryofFood { get; set; }

        [Ignore]
        public List<NameValueDataInString> Coordinationofcare { get; set; }

    }

    public class SearchClientForMonthlySummary
    {
        public SearchClientForMonthlySummary()
        {
            PageSize = 50;
        }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ClientName { get; set; }
        public long PayorID { get; set; }
        public long FacilityID { get; set; }
        public int PageSize { get; set; }
    }

    public class GroupMonthlySummaryStatus
    {
        public GroupMonthlySummaryStatus()
        {
            SuccessMsg = "<ul>";
            ErrorMsg = "<ul>";
        }
        public string SuccessMsg { get; set; }
        public string ErrorMsg { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public List<long> ReferralMonthlySummariesIDs { get; set; }
    }


    public class SignatureDetails
    {
        public string SignatureBy { get; set; }
        public string SignaturePath { get; set; }
    }


    public class ReferralMonthlySummaryModel
    {
        public ReferralMonthlySummaryModel()
        {
            ReferralMonthlySummary = new ReferralMonthlySummary();
            FacilityList = new List<NameValueData>();
        }
        public ReferralMonthlySummary ReferralMonthlySummary { get; set; }
        public List<NameValueData> FacilityList { get; set; }
    }

    public class FindScheduleWithFaciltyAndServiceDateModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string FacilityID { get; set; }
        public string ReferralID { get; set; }
    }
    

}
