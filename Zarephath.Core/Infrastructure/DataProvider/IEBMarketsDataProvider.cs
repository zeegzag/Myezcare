using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IEBMarketsDataProvider
    {
        ServiceResponse AddEBMarkets(string CategoryID,int IsINSUp);
        ServiceResponse AddEBMarkets(EBMarkets Category,  int IsINSUp, long loggedInUserId);
        ServiceResponse SetEBMarketsListPage();
        ServiceResponse GetEBMarketsList(SearchEBMarketsListPage searchEBMarketsListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection);
        ServiceResponse DeleteEBMarkets(SearchEBMarketsListPage searchEBMarketsListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId);

        List<EBMarkets> HC_GetEBMarketsListForAutoComplete(string searchText, int pageSize);
    }
}
