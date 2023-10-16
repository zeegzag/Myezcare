using Myezcare_Admin.Models;
using Myezcare_Admin.Models.ViewModel;
using System.Collections.Generic;

namespace Myezcare_Admin.Infrastructure.DataProvider
{
    public interface IPermissionDataProvider
    {
        ServiceResponse SetAddPermissionsPage(long PermissionID);
        
        ServiceResponse AddPermission(PermissionsModule permissions, long loggedInUserId);
        ServiceResponse SetPermissionListPage();
        //ServiceResponse GetFormList(SearchFormModel model, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse GetGetPermissionList(SearchPermissionsModel searchPermissionsModel, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        //List<ParentPermissionsModel> GetParentPermissionList();
        ServiceResponse DeletePermission(PermissionsModule permissions);
    }
}
