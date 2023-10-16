using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class EBFormsModel
    {
        public EBFormsModel()
        {
            EBForms = new EBForms();
            MarketList = new List<MarketModel>();
            FormCategoryList = new List<CategoryModel>();
           
        }
        public EBForms EBForms { get; set; }
        public List<MarketModel> MarketList { get; set; }
        public List<CategoryModel> FormCategoryList { get; set; }

    }
    public class SearchEBFormsListPage
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string FormID { get; set; }
        public int IsDeleted { get; set; }
        public string ListOfIdsInCsv { get; set; }

    }

    public class SetEBFormsListPage
    {
        public SetEBFormsListPage()
        {
            SearchEBFormsListPage = new SearchEBFormsListPage();
            DeleteFilter = new List<NameValueData>();
        }
        public SearchEBFormsListPage SearchEBFormsListPage { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
    }
    public class ListEBFormsModel
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string EncryptedFormID { get { return Crypto.Encrypt(Convert.ToString(FormID)); } }
        public string FormID { get; set; }
        public int IsDeleted { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }       
    }

    public class SearchOrbeonFormSearch
    {
        DateTime CreatedDate { get; set; }
        public string FormName { get; set; }
        public string FormApp { get; set; }
        public string DocumentID { get; set; }
        public string OrganizationID { get; set; }
        public int EmployeeID { get; set; }
        public int ReferralID { get; set; }
    }
}
