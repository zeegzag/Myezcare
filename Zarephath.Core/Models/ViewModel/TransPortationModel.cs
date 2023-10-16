using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class AddTransportationLocationModel
    {
        public AddTransportationLocationModel()
        {
            StateList = new List<StateDropdownModel>();
            TransportLocation = new TransportLocation();
            RegionList = new List<Region>();
        }
        public List<StateDropdownModel> StateList { get; set; }
        public TransportLocation TransportLocation { get; set; }
        public List<Region> RegionList { get; set; }
        [Ignore]
        public bool IsEditMode { get; set; }
        [Ignore]
        public AmazonSettingModel AmazonSettingModel { get; set; }
    }

    public class SetTransPortationListPage
    {
        public SetTransPortationListPage()
        {
            RegionList = new List<Region>();
            SearchTransPortationListPage = new SearchTransPortationListPage();
            DeleteFilter = new List<NameValueData>();
        }
        public List<Region> RegionList { get; set; }
        public SearchTransPortationListPage SearchTransPortationListPage { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
    }

    public class StateDropdownModel
    {
        public string StateCode { get; set; }
        public string StateName { get; set; }
    }

    public class SearchTransPortationListPage
    {
        public long TransportLocationID { get; set; }
        public string Location { get; set; }
        public string LocationCode { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ListOfIdsInCSV { get; set; }
        public int IsDeleted { get; set; }
        public long RegionID { get; set; }
    }

    public class ListTransportaionModel
    {
        public long TransportLocationID { get; set; }
        public string Location { get; set; }
        public string LocationCode { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string RegionName { get; set; }
        public string EncryptedTransportLocationID { get { return Crypto.Encrypt(Convert.ToString(TransportLocationID)); } }
        public int Count { get; set; }
        public int EmpCount { get; set; }
        public bool IsDeleted { get; set; }
    }
}
