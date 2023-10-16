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
    public class AddPreferenceModel
    {
        public AddPreferenceModel()
        {
            Preference = new Preference();
            PreferenceTypes = new List<NameValueDataInString>();
        }
        public Preference Preference { get; set; }
     
        [Ignore]
        public List<NameValueDataInString> PreferenceTypes { get; set; }
    }

    public class SetPreferenceListPage
    {
        public SetPreferenceListPage()
        {
            SearchPreferenceListPage = new SearchPreferenceListPage();
            DeleteFilter = new List<NameValueData>();
            PreferenceTypes = new List<NameValueDataInString>();
        }
        [Ignore]
        public SearchPreferenceListPage SearchPreferenceListPage { get; set; }
        [Ignore]
        public List<NameValueData> DeleteFilter { get; set; }
        [Ignore]
        public List<NameValueDataInString> PreferenceTypes { get; set; }
    }

    public class SearchPreferenceListPage
    {
        public string PreferenceType { get; set; }
        public string PreferenceName { get; set; }
        public int IsDeleted { get; set; }
        public string ListOfIdsInCsv { get; set; }
    }

    public class RefPreferenceModel
    {
        public long ReferralTaskMappingID { get; set; }
        public long PreferenceID { get; set; }
        public bool IsRequired  { get; set; }
        public long ReferralID { get; set; }
        public string EncryptedReferralID { get; set; }
      
    }

    public class ListPreferenceModel
    {
        public long PreferenceID { get; set; }
        public string PreferenceName { get; set; }
        public string KeyType { get; set; }
        public bool IsDeleted { get; set; }
        public string EncryptedPreferenceID { get { return Crypto.Encrypt(Convert.ToString(PreferenceID)); } }
        public int Row { get; set; }
        public int Count { get; set; }
    }


 
}
