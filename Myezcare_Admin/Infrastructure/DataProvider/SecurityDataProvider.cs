using Myezcare_Admin.Infrastructure.DataProvider;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.Entity;
using Myezcare_Admin.Models.ViewModel;
using Myezcare_Admin.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Myezcare_Admin.Infrastructure.DataProvider
{
    public class SecurityDataProvider : BaseDataProvider, ISecurityDataProvider
    {
        #region Login
        public ServiceResponse CheckLogin(LoginModel login, bool isRegenerateSession)
        {
            ServiceResponse response = new ServiceResponse();
            LoginResponseModel loginResponse = new LoginResponseModel();

            List<SearchValueData> searchParam = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "UserName", Value = login.UserName, IsEqual = true}
                    };

            Admin admin = GetEntity<Admin>(searchParam);
            if (admin != null)
            {
                if (!admin.IsActive)
                {
                    response.Data = Constants.AccountLocked;
                    response.Message = Common.MessageWithTitle(Resource.LoginFailed, Resource.InactiveAccount);
                    return response;
                }

                if(admin.Password.Equals(login.Password))
                {
                    SessionValueData sessionValueData = new SessionValueData
                    {
                        Email = admin.Email,
                        AdminID = admin.AdminID,
                        FirstName = admin.FirstName,
                        LastName = admin.LastName,
                        UserName = admin.UserName
                    };
                    loginResponse.SessionValueData = sessionValueData;
                    response.Data = loginResponse;
                    response.IsSuccess = true;
                    response.Message = Common.MessageWithTitle(Resource.LoginSuccess,
                                                               Resource.LoginSuccessMessage);
                    return response;
                }
                else
                {
                    response.Message = Common.MessageWithTitle(Resource.LoginFailed, Resource.UsernamePasswordIncorrect);
                    return response;
                }
            }

            response.Message = Common.MessageWithTitle(Resource.LoginFailed, Resource.UsernamePasswordIncorrect);
            return response;
        }
        #endregion

        #region OrganizationPermission

        public ServiceResponse HC_GetRolePermission(SearchRolePermissionModel searchRolePermissionModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "PermissionID", Value = Constants.Permission_Administrative_Permission},
                    new SearchValueData {Name = "OrganizationID", Value = Convert.ToString(searchRolePermissionModel.OrganizationID)},
                };
            SetRolePermissionModel setRolePermissionModel = GetMultipleEntity<SetRolePermissionModel>(StoredProcedure.SetOrgPermissionPage, searchList);
            response.Data = setRolePermissionModel;

            return response;
        }

        public ServiceResponse HC_SaveRoleWisePermission(SearchRolePermissionModel searchRolePermissionModel, long loggedInUserId)
        {
            string systemId = HttpContext.Current.Request.UserHostAddress;
            var response = new ServiceResponse();
            return response;
        }

        public ServiceResponse HC_SavePermission(SearchRolePermissionModel searchRolePermissionModel, long loggedInUserId)
        {
            string systemId = HttpContext.Current.Request.UserHostAddress;
            var response = new ServiceResponse();
            if (searchRolePermissionModel.OrganizationID > 0 && !string.IsNullOrEmpty(searchRolePermissionModel.ListOfPermissionIdInCsv))
            {
                List<SearchValueData> searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(loggedInUserId)},
                    new SearchValueData {Name = "OrganizationID", Value = Convert.ToString(searchRolePermissionModel.OrganizationID)},
                    new SearchValueData {Name = "ListOfPermissionIdInCsvSelected", Value = searchRolePermissionModel.ListOfPermissionIdInCsvSelected},
                    new SearchValueData {Name = "ListOfPermissionIdInCsvNotSelected", Value = searchRolePermissionModel.ListOfPermissionIdInCsvNotSelected},
                    new SearchValueData { Name = "SystemID", Value = Convert.ToString(systemId) }
                };
                //response = GetEntity<ServiceResponse>(StoredProcedure.SaveRoleWisePermission, searchList);
                List<RolePermissionModel> model = GetEntityList<RolePermissionModel>(StoredProcedure.SaveOrganizationPermission, searchList);
                response.IsSuccess = true;
                response.Data = model;
                response.Message = "Permissions Saved Successfully";
            }
            else
                response.Message = Resource.ErrorOccured;

            return response;
        }
        #endregion
    }
}
