using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class EBMarketsModel
    {
        public EBMarketsModel()
        {
            EBMarkets = new EBMarkets();

        }
        public EBMarkets EBMarkets { get; set; }
        //public string Name { get; set; }
        //public string ID { get; set; }
        //public string EBCategoryID { get; set; }
        //public int IsDeleted { get; set; }
    }
    public class SearchEBMarketsListPage
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string EBMarketID { get; set; }
        public int IsDeleted { get; set; }
        public string ListOfIdsInCsv { get; set; }

    }

    public class SetEBMarketsListPage
    {
        public SetEBMarketsListPage()
        {
            SearchEBMarketsListPage = new SearchEBMarketsListPage();
            DeleteFilter = new List<NameValueData>();
        }
        public SearchEBMarketsListPage SearchEBMarketsListPage { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
    }
    public class ListEBMarketsModel
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string EncryptedMarketID { get { return Crypto.Encrypt(Convert.ToString(EBMarketID)); } }
        public string EBMarketID { get; set; }
        public int IsDeleted { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }


    }
}
