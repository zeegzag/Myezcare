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
    public interface IAgencyDataProvider
    {
        #region Add Agency

        ServiceResponse SetAddAgencyPage(long agencyId);
        ServiceResponse AddAgency(AddAgencyModel addAgencyModel, long loggedInUserId);

        #endregion

        #region Case Manager List

        ServiceResponse SetAddAgencyListPage();
        ServiceResponse GetAgencyList(SearchAgencyListPage searchAgencyListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteAgency(SearchAgencyListPage searchAgencyListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);

        #endregion

        #region Home Care

        #region Add Agency

        ServiceResponse HC_SetAddAgencyPage(long agencyId);
        ServiceResponse HC_AddAgency(AddAgencyModel addAgencyModel, long loggedInUserId);

        #endregion

        #region Case Manager List

        ServiceResponse HC_SetAddAgencyListPage();
        ServiceResponse HC_GetAgencyList(SearchAgencyListPage searchAgencyListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeleteAgency(SearchAgencyListPage searchAgencyListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);

        #endregion

        #endregion

        List<ZipCodes> GetZipCodeList(string searchText, string state, int pageSize);
    }
}
