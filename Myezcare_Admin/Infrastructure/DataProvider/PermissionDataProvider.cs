using Myezcare_Admin.Infrastructure.DataProvider;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.Entity;
using Myezcare_Admin.Models.ViewModel;
using Myezcare_Admin.Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Myezcare_Admin.Infrastructure.Utility;
using PetaPoco;


namespace Myezcare_Admin.Infrastructure.DataProvider
{
    public class PermissionDataProvider : BaseDataProvider, IPermissionDataProvider
    {

        public ServiceResponse SetAddPermissionsPage(long PermissionID)
        {
            var response = new ServiceResponse();
            try
            {
                var searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "PermissionID", Value = Convert.ToString(PermissionID) });
                PermissionsModule PermissionData = GetEntity<PermissionsModule>(StoredProcedure.GetPermissionsData, searchList);
                if (PermissionData == null)
                {
                    response.Message = string.Format(Resource.nNotFound, Resource.Permissions);
                }
                else
                {
                    response.Data = PermissionData;
                    response.IsSuccess = true;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
            //var response = new ServiceResponse();
            //SetAddPermissionsModel addpermissionsModel = new SetAddPermissionsModel();
            //try
            //{
            //    List<SearchValueData> searchList = new List<SearchValueData>();
            //    List<PermissionsListModule> totalData = GetEntityList<PermissionsListModule>(StoredProcedure.SetPermissionsList, searchList);

            //    int count = 0;
            //    if (totalData != null && totalData.Count > 0)
            //        count = totalData.First().Count;

            //    response.IsSuccess = true;
            //    response.Data = totalData;
            //    return response;


            //}
        }
        public ServiceResponse AddPermission(PermissionsModule permissions, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "PermissionID", Value = Convert.ToString(permissions.PermissionID) });
                dataList.Add(new SearchValueData { Name = "PermissionName", Value = permissions.PermissionName });
                dataList.Add(new SearchValueData { Name = "Description", Value = permissions.Description });
                dataList.Add(new SearchValueData { Name = "ParentID", Value = Convert.ToString(permissions.ParentID) });
                dataList.Add(new SearchValueData { Name = "OrderID", Value = Convert.ToString(permissions.OrderID) });
                dataList.Add(new SearchValueData { Name = "PermissionCode", Value = permissions.PermissionCode });
                dataList.Add(new SearchValueData { Name = "PermissionPlatform", Value = permissions.PermissionPlatform });
                int data = (int)GetScalar(StoredProcedure.SavePermissions, dataList);
                if (data == -1)
                {
                    response.Message = Resource.ReleaseNoteAlreadyExists;
                    return response;
                }

                if (data == 1)
                {
                    ServiceResponse res = new ServiceResponse();
                    List<SearchValueData> searchList = new List<SearchValueData>();
                    List<PermissionsListModule> totalData = GetEntityList<PermissionsListModule>(StoredProcedure.GetPermissionsList, searchList);
                    response.Data = totalData;
                    response.IsSuccess = true;
                    response.Message = permissions.PermissionID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Permissions) :
                        string.Format(Resource.RecordCreatedSuccessfully, Resource.Permissions);
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse DeletePermission(PermissionsModule permissions)
        {
            var response = new ServiceResponse();
            try
            {
                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "PermissionID", Value = Convert.ToString(permissions.PermissionID) });
                int data = (int)GetScalar(StoredProcedure.DeletePermission, dataList);
                if (data == -1)
                {
                    response.Message = string.Format(Resource.nNotFound, Resource.Permissions);
                    return response;
                }

                if (data == 1)
                {
                    ServiceResponse res = new ServiceResponse();
                    List<SearchValueData> searchList = new List<SearchValueData>();
                    //List<PermissionsListModule> totalData = GetEntityList<PermissionsListModule>(StoredProcedure.GetPermissionsList, searchList);
                    //response.Data = totalData;
                    response.IsSuccess = true;
                    response.Message = permissions.PermissionID > 0 ? string.Format(Resource.RecordDeletedSuccessfully, Resource.Permissions) :
                        string.Format(Resource.RecordCreatedSuccessfully, Resource.Permissions);
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse SetAddPermissionList(long PermissionID)
        {
            var response = new ServiceResponse();
            PermissionsModule permissionModule = new PermissionsModule();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "PermissionID", Value = Convert.ToString(PermissionID), IsEqual = true });
                permissionModule = GetEntity<PermissionsModule>(searchParam);
                if (permissionModule == null)
                    permissionModule = new PermissionsModule();

                response.IsSuccess = true;
                response.Data = permissionModule;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, e.Message);
            }
            return response;
        }

        public ServiceResponse GetPermission(int PermissionID)
        {
            var response = new ServiceResponse();
            try
            {
                var searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "PermissionID", Value = Convert.ToString(PermissionID) });
                PermissionsModule PermissionData = GetEntity<PermissionsModule>(StoredProcedure.GetPermissionsList, searchList);
                if (PermissionData == null)
                {
                    response.Message = string.Format(Resource.nNotFound, Resource.Permissions);
                }
                else
                {
                    response.Data = PermissionData;
                    response.IsSuccess = true;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }
        //        public ServiceResponse AddPermission(PermissionsModel permissions, long loggedInUserId)
        //        {
        //            string systemId = System.Web.HttpContext.Current.Request.UserHostAddress;
        //            var response = new ServiceResponse();
        //            try
        //            {
        //                bool editMode = permissions.Permissions.PermissionID > 0;

        //                DataTable dataTbl = Common.ListToDataTable(permissions.PermissionsModules);

        //                string conStr = ConfigurationManager.ConnectionStrings[Constants.MyezcareOrganizationConnectionString].ConnectionString;
        //                SqlConnection con = new SqlConnection(conStr);
        //                con.Open();
        //                SqlCommand cmd = new SqlCommand(StoredProcedure.SaveServicePlanData, con);
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@PermissionID", permissions.Permissions.PermissionID);
        //                cmd.Parameters.AddWithValue("@PermissionName", permissions.Permissions.PermissionName);
        //                cmd.Parameters.AddWithValue("@Description", permissions.Permissions.Description);
        //                cmd.Parameters.AddWithValue("@ParentID", permissions.Permissions.ParentID);
        //                cmd.Parameters.AddWithValue("@PermissionCode", permissions.Permissions.PermissionCode);
        //                cmd.Parameters.AddWithValue("@IsDeleted", permissions.Permissions.IsDeleted);
        //                cmd.Parameters.AddWithValue("@UDTServicePlanModules", dataTbl);
        //                cmd.Parameters.AddWithValue("@LoggedInUserId", loggedInUserId);
        //                cmd.Parameters.AddWithValue("@SystemID", systemId);

        //                SqlDataAdapter da = new SqlDataAdapter();
        //                DataTable dt = new DataTable();
        //                da.SelectCommand = cmd;
        //                da.Fill(dt);

        //                con.Close();

        //                if (dt.Rows.Count > 0)
        //                {
        //                    if (Convert.ToInt32(dt.Rows[0]["TransactionResultId"]) == -1)
        //                    {
        //                        response.IsSuccess = false;
        //                        response.Message = Resource.ServicePlanDuplicateErrorMessage;
        //                        return response;
        //                    }
        //                }
        //                else
        //                {
        //                    response.IsSuccess = false;
        //                    response.Message = Resource.ErrorOccured;
        //                    return response;
        //                }

        //                response.Message = editMode ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.ServicePlan) : string.Format(Resource.RecordCreatedSuccessfully, Resource.ServicePlan);
        //                response.IsSuccess = true;
        //                return response;
        //            }
        //            catch (Exception ex)
        //            {
        //                string message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
        //#if DEBUG
        //                message += ex.Message;
        //#endif
        //                response.IsSuccess = false;
        //                response.Message = message;
        //            }
        //            return response;
        //        }

        #region Form List

        public ServiceResponse SetPermissionListPage()
        {
            var response = new ServiceResponse();
            SetPermissionsListModel setPermissionsListModel = new SetPermissionsListModel();
            setPermissionsListModel.SearchPermissionsModel = new SearchPermissionsModel { IsDeleted = 0 };
            setPermissionsListModel.ActiveFilter = Common.SetDeleteFilter();
            response.Data = setPermissionsListModel;
            return response;
        }
        public ServiceResponse GetGetPermissionList(SearchPermissionsModel searchServicePlanModel, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            SetSearchFilterForPermissionsListPage(searchServicePlanModel, searchList);
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<PermissionsListModule> totalData = GetEntityList<PermissionsListModule>(StoredProcedure.GetPermissionsList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<PermissionsListModule> getServicePlanList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.IsSuccess = true;
            response.Data = getServicePlanList;
            return response;
        }
        private static void SetSearchFilterForPermissionsListPage(SearchPermissionsModel searchPermissionsModel, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "PermissionName", Value = searchPermissionsModel.PermissionName });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchPermissionsModel.IsDeleted) });
        }

        private static List<ParentPermissionsModel> GetParentPermissionList()
        {
            List<ParentPermissionsModel> parentpermission = new List<ParentPermissionsModel>();
            List<SearchValueData> searchList = new List<SearchValueData>();
            //SearchPermissionsModel searchServicePlanModel = new SearchPermissionsModel();
            //SetSearchFilterForPermissionsListPage(searchServicePlanModel, searchList);
            //List<PermissionsListModule> totalData = GetEntityList<PermissionsListModule>(StoredProcedure.GetPermissionsList, searchList);
            return parentpermission;
        }
        #endregion

    }
}
