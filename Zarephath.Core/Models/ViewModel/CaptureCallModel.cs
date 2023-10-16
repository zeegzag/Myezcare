using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class CaptureCallModel
    {
        public CaptureCallModel()
        {
            CaptureCall = new CaptureCall();
            StateList = new List<State>();
            CallType = new List<CallType>();
            PatientList = new List<PatientList>();
            RoleList = new List<RoleList>();
            CRMStatusList = new List<CRMStatusList>();
            ReferralGroupList = new List<ReferralGroupList>();
        }

        public CaptureCall CaptureCall { get; set; }
        public List<State> StateList { get; set; }
        public List<CallType> CallType { get; set; }
        public List<PatientList> PatientList { get; set; }
        public List<RoleList> RoleList { get; set; }
        public List<CRMStatusList> CRMStatusList { get; set; }
        public List<ReferralGroupList> ReferralGroupList { get; set; }
    }

    public class ReferralGroupList
    {
        public string Name { get; set; }
        public long Value { get; set; }
    }
    public class RoleList
    {
        public string RoleName { get; set; }
        public long RoleID { get; set; }
    }
    public class CRMStatusList
    {
        public string Name { get; set; }
        public long Value { get; set; }
    }
    public class PatientList
    {
        public string ReferralName { get; set; }
        public long ReferralID { get; set; }
    }
    public class CallType
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class SetCaptureCallListPage
    {
        public SetCaptureCallListPage()
        {
            SearchCaptureCallListPage = new SearchCaptureCallListPage();
            DeleteFilter = new List<NameValueData>();
        }
        public SearchCaptureCallListPage SearchCaptureCallListPage { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
    }

    public class SearchCaptureCallListPage
    {
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int Flag { get; set; }
        public int IsDeleted { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
    }

    public class ListCaptureCallModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string EncryptedId { get { return Crypto.Encrypt(Convert.ToString(Id)); } }
        public int Flag { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string FullAddress
        {
            get
            {
                if (!string.IsNullOrEmpty(Address))
                    return String.Format("{0}, {1} - {2} {3}", Address, City, ZipCode, StateCode);

                return "";
            }
        }
        public string Notes { get; set; }
        public string AssigneeName { get; set; }
        public string RoleIds { get; set; }
        public string EmployeesIDs { get; set; }
        public string CallType { get; set; }
        public string RelatedWithPatient { get; set; }
        public DateTime? InquiryDate { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string OrbeonID { get; set; }
        public string Status { get; set; }
        public int Count { get; set; }
    }
}
