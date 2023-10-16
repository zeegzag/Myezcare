using Myezcare_Admin.Models;
using Myezcare_Admin.Models.Entity;
using Myezcare_Admin.Models.ViewModel;

namespace Myezcare_Admin.Infrastructure.DataProvider
{
    public interface ISecurityDataProvider
    {
        ServiceResponse CheckLogin(LoginModel login, bool isRegenerateSession);

        #region HC_RolePermission
        ServiceResponse HC_GetRolePermission(SearchRolePermissionModel searchRolePermissionModel);
        ServiceResponse HC_SaveRoleWisePermission(SearchRolePermissionModel searchRolePermissionModel, long loggedInUserId);
        ServiceResponse HC_SavePermission(SearchRolePermissionModel searchRolePermissionModel, long loggedInUserId);
        #endregion
    }
}
