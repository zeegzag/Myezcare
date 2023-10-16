using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface ICaseManagerDataProvider
    {
        #region Add Case Manager

        ServiceResponse SetAddCaseManagerPage(long caseManagerID,long agencyID,long agencyLocationID);
        ServiceResponse GetAgencyLocation(long agencyID, long agencyLocationID);
        ServiceResponse AddCaseManager(AddCaseManagerModel addCaseManagerModel, long loggedInUserID);
        ServiceResponse GetCaseManagers(int agencyID, long caseManagerID);
        ServiceResponse GetCaseManagerDetail(int caseManagerID);

        #endregion

        #region Case Manager List

        ServiceResponse SetAddCaseManagerListPage();
        ServiceResponse GetCaseManagerList(SearchCaseManagerListPage searchCaseManagerListPage, int pageIndex,
                                           int pageSize, string sortIndex, string sortDirection);

        ServiceResponse DeleteCaseManager(SearchCaseManagerListPage searchCaseManagerListPage, int pageIndex,
                                          int pageSize, string sortIndex, string sortDirection,long loggedInUserID);

        #endregion
    }
}
