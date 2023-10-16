using System;
using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class DepartmentDataProvider : BaseDataProvider, IDepartmentDataProvider
    {
        #region Add Department

        public ServiceResponse SetAddDepartmentPage(long departmentId)
        {
            var response = new ServiceResponse();
            try
            {
                if (departmentId > 0)
                {
                    List<SearchValueData> searchParam = new List<SearchValueData>
                        {
                            new SearchValueData { Name = "DepartmentID" , Value = departmentId.ToString()},
                            //new SearchValueData { Name = "IsDeleted" , Value = "0"},
                        };
                    Department department = GetEntity<Department>(searchParam);
                    if (department != null && department.DepartmentID > 0)
                    {
                        response.Data = department;
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.ErrorCode = Constants.ErrorCode_NotFound;
                        return response;
                    }
                }
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse AddDepartment(Department department, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {
                if (loggedInUserID == 0)
                {
                    response.Message = Common.MessageWithTitle(Resource.Error, Resource.UserSessionExpired);
                    return response;
                }

                if (department != null)
                {
                    response.Message = department.DepartmentID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Department) : string.Format(Resource.RecordCreatedSuccessfully, Resource.Department);
                    SaveObject(department, loggedInUserID);
                    response.IsSuccess = true;
                }
                else
                    response.Message = Common.MessageWithTitle(string.Format(Resource.CreateFailed, Resource.Department), Resource.ExceptionMessage);

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
                return response;
            }
            return response;
        }

        #endregion

        #region Department List

        public ServiceResponse SetDepartmentListPage()
        {
            var response = new ServiceResponse();
            try
            {

                SetDepartmentListPage setDepartmentListPage = GetMultipleEntity<SetDepartmentListPage>("SetDepartmentListPage");
                setDepartmentListPage.SearchDepartmentModel = new SearchDepartmentModel();
                setDepartmentListPage.DeleteFilter = Common.SetDeleteFilter();
                setDepartmentListPage.SearchDepartmentModel.IsDeleted = 0;
                response.Data = setDepartmentListPage;
                return response;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.SetDepartmentListPageFailed, Resource.ExceptionMessage);
                return response;
            }
        }

        public ServiceResponse GetDepartmentList(SearchDepartmentModel searchDepartmentModel, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

                if (searchDepartmentModel != null)
                    SetSearchFilterForDepartmentListPage(searchDepartmentModel, searchList);

                List<ListDepartmentModel> totalData = GetEntityList<ListDepartmentModel>("GetDepartmentList", searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListDepartmentModel> departmentList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = departmentList;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.Department), Resource.ExceptionMessage);
            }
            return response;
        }

        private static void SetSearchFilterForDepartmentListPage(SearchDepartmentModel searchDepartmentModel, List<SearchValueData> searchList)
        {
            if (searchDepartmentModel.DepartmentID > 0)
                searchList.Add(new SearchValueData
                    {
                        Name = "DepartmentID",
                        Value = Convert.ToString(searchDepartmentModel.DepartmentID)
                    });

            if (searchDepartmentModel.EmployeeID > 0)
                searchList.Add(new SearchValueData
                    {
                        Name = "EmployeeID",
                        Value = Convert.ToString(searchDepartmentModel.EmployeeID)
                    });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchDepartmentModel.IsDeleted) });

            if (!string.IsNullOrEmpty(searchDepartmentModel.Location))
                searchList.Add(new SearchValueData { Name = "Location", Value = Convert.ToString(searchDepartmentModel.Location) });

            if (!string.IsNullOrEmpty(searchDepartmentModel.Address))
                searchList.Add(new SearchValueData { Name = "Address", Value = Convert.ToString(searchDepartmentModel.Address) });
        }

        public ServiceResponse DeleteDepartment(SearchDepartmentModel searchDepartmentModel, int pageIndex, int pageSize, string sortIndex, string sortDirection,long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {

                List<SearchValueData> searchList = SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

                SetSearchFilterForDepartmentListPage(searchDepartmentModel, searchList);

                if (!string.IsNullOrEmpty(searchDepartmentModel.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchDepartmentModel.ListOfIdsInCSV });

                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

                List<ListDepartmentModel> totalData = GetEntityList<ListDepartmentModel>(StoredProcedure.DeleteDepartment, searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                //if (count == 0 && totalData != null && totalData.Count > 0)
                //{
                //    response.Message = Common.MessageWithTitle(string.Format(Resource.DeleteFailed, Resource.Department), Resource.DepartmentEmployeeExistMessage);
                //    return response;
                //}

                Page<ListDepartmentModel> getDepartmentList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);

                response.Data = getDepartmentList;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Department);

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }

        public List<SearchValueData> SetPagerValues(int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            List<SearchValueData> searchList = new List<SearchValueData>();

            var searchValueData = new SearchValueData { Name = "SortExpression", Value = Convert.ToString(sortIndex) };
            searchList.Add(searchValueData);

            searchValueData = new SearchValueData { Name = "SortType", Value = Convert.ToString(sortDirection) };
            searchList.Add(searchValueData);

            searchValueData = new SearchValueData { Name = "FromIndex", Value = Convert.ToString(pageIndex) };
            searchList.Add(searchValueData);

            searchValueData = new SearchValueData { Name = "PageSize", Value = Convert.ToString(pageSize) };
            searchList.Add(searchValueData);

            return searchList;
        }

        #endregion

        public List<ZipCodes> GetZipCodeList(string searchText, string state, int pageSize)
        {
            ServiceResponse response = new ServiceResponse();

            string customWhere = string.Format("(ZipCode like '%{0}%' or StateCode like '%{0}%' or City like '%{0}%')", searchText);

            Page<ZipCodes> page = GetEntityPageList<ZipCodes>(new List<SearchValueData> { }, pageSize, 1, "", "", customWhere);
            long zipcode;
            if (searchText.Length == 5 && long.TryParse(searchText, out zipcode))
            {
                page.Items.Add(new ZipCodes
                    {
                        ZipCode = searchText,
                        City = "",
                        County = "",
                        StateCode = ""
                    });
            }

            return page.Items;
        }

    }
}
