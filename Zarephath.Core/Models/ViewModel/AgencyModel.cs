using System;
using System.Collections.Generic;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class AddAgencyModel
    {
        public AddAgencyModel()
        {
            RegionListModel = new List<RegionListModel>();
            StateCodeListModel = new List<StateCodeListModel>();
            Agency = new Agency();
            AgencyTaxonomies = new List<AgencyTaxonomy>();
        }
        public List<RegionListModel> RegionListModel { get; set; }
        public List<StateCodeListModel> StateCodeListModel { get; set; }
        public Agency Agency { get; set; }
        public List<AgencyTaxonomy> AgencyTaxonomies { get; set; }
    }

    public class RegionList
    {
        public long RegionID { get; set; }
        public string RegionName { get; set; }
    }

    public class StateCodeListModel
    {
        public string StateCode { get; set; }
        public string StateName { get; set; }
    }

    public class SetAgencyListPage
    {
        public SetAgencyListPage()
        {
            RegionListModel = new List<RegionListModel>();
            SearchAgencyListPage = new SearchAgencyListPage();
            DeleteFilter = new List<NameValueData>();
        }
        public List<RegionListModel> RegionListModel { get; set; }
        public SearchAgencyListPage SearchAgencyListPage { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
    }

    public class SearchAgencyListPage
    {
        public long AgencyID { get; set; }
        public string AgencyType { get; set; }
        public string NickName { get; set; }
        public string ShortName { get; set; }
        public long   RegionID { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string EIN { get; set; }
        public string TIN { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public int IsDeleted { get; set; }
    }

    public class ListAgencyModel
    {
        public long AgencyID { get; set; }
        public string AgencyType { get; set; }
        public string TIN { get; set; }
        public string EIN { get; set; }
        public string Mobile { get; set; }
        public string NickName { get; set; }
        public string ShortName { get; set; }
        public long RegionID { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string RegionName { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string EncryptedAgencyID { get { return Crypto.Encrypt(Convert.ToString(AgencyID)); } }
        public bool IsDeleted { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }
    }

}
