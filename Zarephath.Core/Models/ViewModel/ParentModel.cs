using System;
using System.Collections.Generic;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class AddParentModel
    {
        public AddParentModel()
        {
            Contact = new Contact();
            Language = new List<Language>();
        }

        public Contact Contact { get; set; }
        public List<Language> Language { get; set; }
        
    }

    public class SetParentListPage
    {
        public SetParentListPage()
        {
            ContactTypeList = new List<ContactType>();
            SearchParentListPage = new SearchParentListPage();
            DeleteFilter = new List<NameValueData>();
        }

        public List<ContactType> ContactTypeList { get; set; }
        
        [Ignore]
        public SearchParentListPage SearchParentListPage { get; set; }
        [Ignore]
        public List<NameValueData> DeleteFilter { get; set; } 
    }

    public class StateList
    {
        public string StateCode { get; set; }
        public string StateName { get; set; }
    }



    public class SearchParentListPage
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public long ContactTypeID { get; set; }
        public string Phone1 { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public int IsDeleted { get; set; }
    }

    public class ListParentModel
    {
        public long ContactID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public bool IsDeletd { get; set; }
        

        //{{AddAndListContactInformation.Address}},&nbsp;{{AddAndListContactInformation.City}},&nbsp;{{AddAndListContactInformation.State}}- {{AddAndListContactInformation.ZipCode}}
        public string FullAddress
        {
            get
            {
                return String.Format("{0}, {1}, {2} - {3}",Address,City,State,ZipCode);
            }
        }

        public string Phone1 { get; set; }
        public string ContactTypeName { get; set; }
        
        public string EncryptedContactID { get { return Crypto.Encrypt(Convert.ToString(ContactID)); } }
        public bool IsDeleted { get; set; }
        public long ReferralCount { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }
    }

   
}
