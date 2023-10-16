using System;
using System.Collections.Generic;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class AddCaseManagerModel
    {
        public AddCaseManagerModel()
        {
            CaseManager = new CaseManager();
            AgencyList = new List<AgencyDropDownModel>();
            AgencyLocationList = new List<AgencyLocation>();            
        }

        public CaseManager CaseManager { get; set; }
        public List<AgencyDropDownModel> AgencyList { get; set; }
        public List<AgencyLocation> AgencyLocationList { get; set; }
        public bool IsEditMode { get; set; }
        
    }

    public class SetCaseManagerListPage
    {
        public SetCaseManagerListPage()
        {
            AgencyList = new List<SetAgencyDropDown>();
            AgencyLocationList = new List<SetAgencyLocationDropDown>();
            SearchCaseManagerListPage = new SearchCaseManagerListPage();
            DeleteFilter = new List<NameValueData>();
        }

        public List<SetAgencyDropDown> AgencyList { get; set; }
        public List<SetAgencyLocationDropDown> AgencyLocationList { get; set; }
        public SearchCaseManagerListPage SearchCaseManagerListPage { get; set; }
        public List<NameValueData> DeleteFilter { get; set; } 
    }

    public class SearchCaseManagerListPage
    {
        public long AgencyLocationID { get; set; }
        public long AgencyID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public int IsDeleted { get; set; }
        public string CaseWorkerID { get; set; }
    }

    public class SetAgencyLocationDropDown
    {
        public long AgencyLocationID { get; set; }
        public string LocationName { get; set; }
    }

    public class SetAgencyDropDown
    {
        public long AgencyID { get; set; }
        public string NickName { get; set; }
    }

    public class ListCaseManagerModel
    {
        public long CaseManagerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AgencyName { get; set; }
        public string AgencyLocationName { get; set; }
        public string Phone { get; set; }
        public string EncryptedCaseManagerID { get { return Crypto.Encrypt(Convert.ToString(CaseManagerID)); } }
        public bool IsDeleted { get; set; }
        public long ReferralCount { get; set; }
        public string CaseWorkerID { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }
    }

   
}
