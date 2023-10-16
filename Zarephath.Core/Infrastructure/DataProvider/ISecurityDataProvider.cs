using System.Web;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface ISecurityDataProvider
    {
        ServiceResponse GetLoginPageDetail();
        ServiceResponse CheckLogin(LoginModel login, bool isRegenerateSession);
        ServiceResponse SetSecurityQuestionPage(SecurityQuestionModel model, string userName);
        ServiceResponse SaveSecurityQuestion(SecurityQuestionModel securityQuestionModel, long loggedInUserID);
        ServiceResponse SetPassword(string id);
        ServiceResponse SaveSetPassword(SetPasswordModel model, long loggedInID);
        ServiceResponse SetForgotPasswordPage();
        ServiceResponse SaveForgotPassword(ForgotPasswordModel forgotPasswordModel);
        ServiceResponse UnlockAccount(ForgotPasswordModel forgotPasswordModel);
        ServiceResponse SetResetPasswordPage(long loggedInUserID);
        ServiceResponse SaveResetPassword(ResetPasswordModel model);
        ServiceResponse SetEditProfilePage(long loggedInUserID); 
         ServiceResponse SaveEditProfile(EditProfileModel editProfileModel, long loggedInUserID);
        ServiceResponse HC_SaveProfileImg(HttpRequestBase currentHttpRequest, bool isEmployeeDocument = false);

        Announcement GetAnnouncement();
        #region Verify Employee

        ServiceResponse AccountVerification(long encryptedValue);
        ServiceResponse RegenerateVerificationLink(string email);

        #endregion


        #region RolePermission

        //ServiceResponse SetRolePermissionPage(SearchRolePermissionModel searchRolePermissionModel);
        ServiceResponse GetRolePermission(SearchRolePermissionModel searchRolePermissionModel);
        ServiceResponse AddNewRole(Role roleModel, long loggedInUserId);

        ServiceResponse UpdateRolename(SearchRolePermissionModel searchRolePermissionModel, long loggedInUserId);
        ServiceResponse SaveRoleWisePermission(SearchRolePermissionModel searchRolePermissionModel, long loggedInUserId);
        ServiceResponse SaveMapReport(MapReportModel mapReportModel, long loggedInUserId);

        #endregion RolePermission

        #region HC_RolePermission
        
        ServiceResponse HC_GetRolePermission(SearchRolePermissionModel searchRolePermissionModel);
        ServiceResponse GetMapReport(MapReportModel mapReportModel);
        ServiceResponse HC_AddNewRole(Role roleModel, long loggedInUserId);
        ServiceResponse HC_UpdateRolename(SearchRolePermissionModel searchRolePermissionModel, long loggedInUserId);
        ServiceResponse HC_SaveRoleWisePermission(SearchRolePermissionModel searchRolePermissionModel, long loggedInUserId);
        ServiceResponse HC_SavePermission(SearchRolePermissionModel searchRolePermissionModel, long loggedInUserId);
        #endregion

        #region System Setting Chached Data
        ServiceResponse ChechedSettingData();
        //ServiceResponse ChechedMenuList();
        #endregion System Setting Chached Data
        ServiceResponse SetCreateLoginData(HC_AddEmployeeModel model);
        //20220711 RN
        ServiceResponse GetSecurityQuestion(ForgotPasswordDetailModel forgotPasswordModel);
    }
}
