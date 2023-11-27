using PetaPoco;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class ComplianceDataProvider : BaseDataProvider, IComplianceDataProvider
    {
        public ComplianceDataProvider()
        {
        }

        public ComplianceDataProvider(string conString)
            : base(conString)
        {
        }

        public ServiceResponse AddCompliance(int userType)
        {
            var response = new ServiceResponse();
            ComplianceModel model = new ComplianceModel();

            List<NameValueData> detail = GetEntityList<NameValueData>(StoredProcedure.SetCompliancePage, new List<SearchValueData> {
                new SearchValueData("UserType",Convert.ToString(userType))
            });

            //model.SectionList = detail.SectionList;
            //model.SubSectionList = detail.SubSectionList;
            model.DirectoryList = detail;
            model.Compliance = new Compliance { DocumentationType = 1 };
            model.SearchComplianceListPage = new SearchComplianceListPage { UserType = -1, DocumentationType = -1, IsTimeBased = -1 };
            model.DeleteFilter = Common.SetDeleteFilter();
            model.UserTypeList = Common.SetUserTypeList();
            model.DocumentationTypeList = Common.SetDocumentationTypeList();
            model.ConfigEBFormModel = new ConfigEBFormModel();
            List<Role> dbRolesList = GetEntityList<Role>(null, "", "RoleName", "ASC");
            model.RolesList = dbRolesList.Select(x => new NameValueData
            {
                Name = x.RoleName,
                Value = x.RoleID
            }).ToList();
            response.Data = model;
            return response;
        }
        public ServiceResponse GetAssigneeList(int userType)
        {
            var response = new ServiceResponse();
            ComplianceModel model = new ComplianceModel();

            List<NameValueData> detail = GetEntityList<NameValueData>(StoredProcedure.GetAssigneeList, new List<SearchValueData> {
                new SearchValueData("UserType",Convert.ToString(userType))
            });

            //model.SectionList = detail.SectionList;
            //model.SubSectionList = detail.SubSectionList;
            model.DirectoryList = detail;
            List<Role> dbRolesList = GetEntityList<Role>(null, "", "RoleName", "ASC");
            model.RolesList = dbRolesList.Select(x => new NameValueData
            {
                Name = x.RoleName,
                Value = x.RoleID
            }).ToList();
            response.Data = model;
            return response;
        }

        public ServiceResponse SaveCompliance(Compliance compliance, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            try
            {
                bool isEditMode = compliance.ComplianceID > 0;

                searchList.Add(new SearchValueData("ComplianceID", Convert.ToString(compliance.ComplianceID)));
                searchList.Add(new SearchValueData("UserType", Convert.ToString(compliance.UserType)));
                searchList.Add(new SearchValueData("DocumentationType", Convert.ToString(compliance.DocumentationType)));
                searchList.Add(new SearchValueData("DocumentName", compliance.DocumentName));
                searchList.Add(new SearchValueData("IsTimeBased", Convert.ToString(compliance.IsTimeBased)));
                searchList.Add(new SearchValueData("Type", compliance.Type));
                searchList.Add(new SearchValueData("ParentID", Convert.ToString(compliance.ParentID)));
                searchList.Add(new SearchValueData("Value", compliance.Value));
                searchList.Add(new SearchValueData("EBFormID", Convert.ToString(compliance.EBFormID)));
                searchList.Add(new SearchValueData("CurrentDate", Common.GetOrgCurrentDateTime().ToString(Constants.DbDateFormat)));//Convert.ToString(Common.GetOrgCurrentDateTime())));
                searchList.Add(new SearchValueData("loggedInUserId", Convert.ToString(loggedInUserId)));
                searchList.Add(new SearchValueData("SystemID", Common.GetHostAddress()));
                searchList.Add(new SearchValueData("SelectedRoles", Convert.ToString(compliance.SelectedRoles)));
                searchList.Add(new SearchValueData("Assignee", Convert.ToString(compliance.Assignee)));
                searchList.Add(new SearchValueData("ShowToAll", Convert.ToString(compliance.ShowToAll)));

                int data = (int)GetScalar(StoredProcedure.SaveCompliance, searchList);
                if (data == -1)
                {
                    response.Message = Resource.ItemAlreadyExists;
                    response.IsSuccess = false;
                    return response;
                }
                response.IsSuccess = true;
                response.Message = isEditMode ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.DocumentInformation) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.DocumentInformation);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse GetComplianceList(SearchComplianceListPage searchComplianceListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchComplianceListPage != null)
                SetSearchFilterForComplianceList(searchComplianceListPage, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ComplianceListModel> totalData = GetEntityList<ComplianceListModel>(StoredProcedure.GetComplianceList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ComplianceListModel> ddMasterList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = ddMasterList;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse DeleteCompliance(SearchComplianceListPage searchComplianceListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
            SetSearchFilterForComplianceList(searchComplianceListPage, searchList);

            if (!string.IsNullOrEmpty(searchComplianceListPage.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchComplianceListPage.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });

            List<ComplianceListModel> totalData = GetEntityList<ComplianceListModel>(StoredProcedure.DeleteCompliance, searchList);

            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.DocumentInformation);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;
            Page<ComplianceListModel> list = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = list;
            return response;
        }

        private static void SetSearchFilterForComplianceList(SearchComplianceListPage searchComplianceListPage, List<SearchValueData> searchList)
        {

            if (!string.IsNullOrEmpty(searchComplianceListPage.DocumentName))
                searchList.Add(new SearchValueData { Name = "DocumentName", Value = Convert.ToString(searchComplianceListPage.DocumentName) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchComplianceListPage.IsDeleted) });
            searchList.Add(new SearchValueData { Name = "UserType", Value = Convert.ToString(searchComplianceListPage.UserType) });
            searchList.Add(new SearchValueData { Name = "DocumentationType", Value = Convert.ToString(searchComplianceListPage.DocumentationType) });
            searchList.Add(new SearchValueData { Name = "IsTimeBased", Value = Convert.ToString(searchComplianceListPage.IsTimeBased) });
            searchList.Add(new SearchValueData { Name = "Type", Value = searchComplianceListPage.Type });
            searchList.Add(new SearchValueData { Name = "ShowToAll", Value = Convert.ToString(searchComplianceListPage.ShowToAll) });
        }

        public ServiceResponse GetOrganizationFormList()
        {
            ServiceResponse response = new ServiceResponse();

            CacheHelper_MyezCare chMyezcareOrg = new CacheHelper_MyezCare();
            MyEzcareOrganization myOrg = chMyezcareOrg.GetCachedData<MyEzcareOrganization>();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(myOrg.OrganizationID) });
            List<FormListModel> data = GetEntityList<FormListModel>(StoredProcedure.GetOrganizationFormListForMapping, searchList);


            response.Data = data;
            response.IsSuccess = true;
            return response;
        }



        public ServiceResponse ChangeSortingOrder(ChangeSortingOrderModel model)
        {
            ServiceResponse response = new ServiceResponse();
            if (model!=null)
            {
              var searchParam = new List<SearchValueData>();
            searchParam.Add(new SearchValueData("ComplianceID", Convert.ToString(model.ComplianceID)));
            searchParam.Add(new SearchValueData("originID", Convert.ToString(model.originID)));
            searchParam.Add(new SearchValueData("destinationID", Convert.ToString(model.destinationID)));
                GetScalar(StoredProcedure.CompliancesListChangeSortingOrder, searchParam);
            }  
          
            response.IsSuccess = true;
            response.Message = "Sorting Updated Successfully";
            return response;
        }
    }
}
