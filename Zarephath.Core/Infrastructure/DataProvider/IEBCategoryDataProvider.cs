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
    public interface IEBCategoryDataProvider
    {
        ServiceResponse AddEBCategory(string CategoryID,int IsINSUp);
        ServiceResponse AddEBCategory(EBCategory Category,  int IsINSUp, long loggedInUserId);
        ServiceResponse SetEBCategoryListPage();
        ServiceResponse GetCategoryList(SearchEBCategoryListPage searchEBCategoryListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection);
        ServiceResponse DeleteCategory(SearchEBCategoryListPage searchEBCategoryListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId);

        List<EBCategory> HC_GetEBCategoryListForAutoComplete(string searchText, int pageSize);
    }
}
