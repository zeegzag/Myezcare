using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class EBCategoryModel
    {
        public EBCategoryModel()
        {
            EBCategory = new EBCategory();
           
        }
        public EBCategory EBCategory { get; set; }
        //public string Name { get; set; }
        //public string ID { get; set; }
        //public string EBCategoryID { get; set; }
        //public int IsDeleted { get; set; }
    }
    public class SearchEBCategoryListPage
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string EBCategoryID { get; set; }
        public int IsDeleted { get; set; }
        public string ListOfIdsInCsv { get; set; }

    }

    public class SetEBCategoryListPage
    {
        public SetEBCategoryListPage()
        {
            SearchEBCategoryListPage = new SearchEBCategoryListPage();
            DeleteFilter = new List<NameValueData>();
        }
        public SearchEBCategoryListPage SearchEBCategoryListPage { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
    }
    public class ListEBCategoryModel
    {
        public string Name { get; set; }

        public string EncryptedEBCategoryID { get { return Crypto.Encrypt(Convert.ToString(EBCategoryID)); } }
        public string ID { get; set; }
        public string EBCategoryID { get; set; }
        public int IsDeleted { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }

        
    }
}
