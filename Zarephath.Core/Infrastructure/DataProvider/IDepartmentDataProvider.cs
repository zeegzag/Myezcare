using System.Collections.Generic;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IDepartmentDataProvider
    {
        #region Add Department

        ServiceResponse AddDepartment(Department department, long loggedInUserID);
        ServiceResponse SetAddDepartmentPage(long departmentId);

        #endregion

        #region Department List

        ServiceResponse GetDepartmentList(SearchDepartmentModel searchDepartmentModel, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse SetDepartmentListPage();
        ServiceResponse DeleteDepartment(SearchDepartmentModel searchDepartmentModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);

        #endregion

        List<ZipCodes> GetZipCodeList(string searchText, string state, int pageSize);
    }
}
