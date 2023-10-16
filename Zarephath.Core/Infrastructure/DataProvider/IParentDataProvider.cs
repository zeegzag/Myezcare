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
    public interface IParentDataProvider
    {
        #region Add Parent
        ServiceResponse SetAddParentPage(long parentID);
        ServiceResponse AddParent(Contact model, long loggedInUserID);

        #endregion

        #region Parent List

        ServiceResponse SetAddParentListPage();
        ServiceResponse GetParentList(SearchParentListPage searchParentListPage, int pageIndex,
                                           int pageSize, string sortIndex, string sortDirection);

        ServiceResponse DeleteParent(SearchParentListPage searchParentListPage, int pageIndex,
                                          int pageSize, string sortIndex, string sortDirection,long loggedInUserID);

        #endregion
    }
}
