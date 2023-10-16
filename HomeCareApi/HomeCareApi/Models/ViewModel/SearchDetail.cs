using HomeCareApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeCareApi.Models.ViewModel
{
    public class SearchDetail
    {
        public long ReferralID { get; set; }
        public long EmployeeID { get; set; }
        public bool IsCompletedVisit { get; set; }
    }

    public class SearchPatient
    {
        public long EmployeeID { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long FacilityID { get; set; }
    }

    public class SearchVisitHistory
    {
        public long EmployeeID { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class SearchIMModel
    {
        public long EmployeeID { get; set; }
        public string MessageType { get; set; }
    }

    public class IMResolvedModel
    {
        public string ResolvedComment { get; set; }
        public long ReferralInternalMessageID { get; set; }
    }

    public class EmpRefList
    {
        public EmpRefList()
        {
            EmployeeList = new List<EmployeesForSMS>();
            PatientList = new List<PatientsForSMS>();
        }
        public List<EmployeesForSMS> EmployeeList { get; set; }
        public List<PatientsForSMS> PatientList { get; set; }
    }

    public class EmployeesForSMS
    {
        public string EmployeeName { get; set; }
        public long EmployeeID { get; set; }
    }

    public class PatientsForSMS
    {
        public string PatientName { get; set; }
        public long ReferralID { get; set; }
    }

    public class InternalMessage
    {
        public long Assignee { get; set; }
        public long EmployeeID   { get; set; }
        public long ReferralID { get; set; }
        public string Message { get; set; }
    }

    public class EmpProfile
    {
        public long EmployeeID { get; set; }
    }

    public class EmployeeProfileDetails
    {
        public long EmployeeID { get; set; }
        public string EncryptedEmployeeID { get { return Crypto.Encrypt(Convert.ToString(EmployeeID)); } }
        public long? EmployeeSignatureID { get; set; }
        public bool IsFingerPrintAuth { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string EmployeeSignatureURL { get; set; }
        public string EmployeeProfileImgURL { get; set; }
        public string TimeZone { get; set; }
        public string MobileNumber { get; set; }
        public string IVRPin { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string OrganizationTwilioNumber { get; set; }
        public string AndroidMinimumVersion { get; set; }
        public string AndroidCurrentVersion { get; set; }
        public string HireDates { get; set; }
        public string HireDate
        {
            get {
                if (!string.IsNullOrEmpty(HireDates))
                {
                    return Convert.ToDateTime(HireDates).ToShortDateString();
                }
                return string.Empty;
            }
        }
    }

    public class EmployeeIdCardDetails
    {
        public long EmployeeID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string EmployeeUniqueID { get; set; }
        public string EmployeeSignatureURL { get; set; }
        public string EmployeeProfileImgURL { get; set; }
        public string NPIID { get; set; }
        public string OrganizationName { get; set; }
        public bool IsActive { get; set; }
        public string VerificationURL {
            get
            {
                return string.Format("{0}{1}{2}", string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()), Constants.GenerateCertificateForEmployeeURL, Crypto.Encrypt(Convert.ToString(EmployeeID)));
                //return "http://192.168.1.153:5555/hc/employee/GenerateCertificateForEmployee/" + Crypto.Encrypt(Convert.ToString(EmployeeID));
            }
        }
        public DateTime JoiningDate { get; set; }
        public string SiteLogo { get; set; }
    }

    public class FacilityDetails
    {
        public long FacilityID { get; set; }
        public string FacilityName { get; set; }
        public long EmployeeID { get; set; }
    }
    public class FacilityListModel
    {
        public List<FacilityDetails> FacilityList { get; set; }
    }

    public class CovidSurveyQuestionList
    {
        public long QuestionID { get; set; }
        public string Question { get; set; }
    }

    public class CovidSurveyQuestionModel
    {
        public List<CovidSurveyQuestionList> CovidSurveyQuestionList { get; set; }
    }

    public class CovidSurveySaveModel
    {
        //public long CovidSurveyID { get; set; }
        public long EmployeeID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<EmpSurveyAnsModel> Answer { get; set; }
    }

    public class GetCovidSurveyModel
    {
        public long EmployeeID { get; set; }
        public string CreatedDate { get; set; }
    }

    public class EmpSurveyAnsModel
    {
        public long CovidSurveyID { get; set; }
        public long QuestionID { get; set; }
        public long AnswersID { get; set; }
    }
    public class CovidSurveyListModel
    {
        public List<CovidSurveyList> CovidSurveyList { get; set; }

    }
    public class CovidSurveyList
    {
        public long CovidSurveyID { get; set; }
        public string EmployeeName { get; set; }
        public string Question { get; set; }
        public long AnswersID { get; set; }
    }

    public class SearchReferralGroup
    {
        public long EmployeeID { get; set; }
        public string GroupName { get; set; }
    }

}