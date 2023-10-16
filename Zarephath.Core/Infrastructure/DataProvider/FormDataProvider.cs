using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PetaPoco;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Models;
using Zarephath.Core.Resources;
using Zarephath.Core.Infrastructure.Utility.eBriggsForms;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models.Entity;


namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class FormDataProvider : BaseDataProvider, IFormDataProvider
    {
        public FormDataProvider()
        {
        }

        public FormDataProvider(string conString)
            : base(conString)
        {
        }


        #region Form List For All Users To Save For The Patients & Employees

        public ServiceResponse GetPatientEmpInfoModel()
        {

            ServiceResponse respose = new ServiceResponse();
            List<SearchValueData> searchModel = new List<SearchValueData>();
            GetPatientEmpInfoModel data = GetMultipleEntity<GetPatientEmpInfoModel>(StoredProcedure.GetPatientEmpInfo, searchModel);
            respose.Data = data;
            respose.IsSuccess = true;
            return respose;
        }


        public ServiceResponse SetFormListPage()
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchModel = new List<SearchValueData>();
            FormPageModel data = GetMultipleEntity<FormPageModel>(StoredProcedure.SetOrganizationFormListPage, searchModel);
            response.Data = data;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetFormList(SearchFormModel model, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            SetSearchFilterForFormListPage(model, searchList);
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<FormListModel> totalData = GetEntityList<FormListModel>(StoredProcedure.GetOrganizationFormList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<FormListModel> getOrganizationList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.IsSuccess = true;
            response.Data = getOrganizationList;
            return response;


        }
        private static void SetSearchFilterForFormListPage(SearchFormModel model, List<SearchValueData> searchList)
        {

            CacheHelper_MyezCare ch_MyezcareOrg = new CacheHelper_MyezCare();
            MyEzcareOrganization myOrg = ch_MyezcareOrg.GetCachedData<MyEzcareOrganization>();

            searchList.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(myOrg.OrganizationID) });
            searchList.Add(new SearchValueData { Name = "MarketID", Value = Convert.ToString(model.MarketID) });
            searchList.Add(new SearchValueData { Name = "FormCategoryID", Value = Convert.ToString(model.FormCategoryID) });
            searchList.Add(new SearchValueData { Name = "FormName", Value = model.FormName });
            searchList.Add(new SearchValueData { Name = "FormNumber", Value = model.FormNumber });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(model.IsDeleted) });
        }


        public ServiceResponse GetSavedFormMappings(SearchFormModel model)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchModel = new List<SearchValueData>();
            searchModel.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchModel.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
            List<UDT_EBFromMappingTable> data = GetEntityList<UDT_EBFromMappingTable>(StoredProcedure.GetSavedFormMappings, searchModel);
            response.Data = data;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetSavedFormList(List<UDT_EBFromMappingTable> mappingData, SearchFormModel model, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();


            CacheHelper_MyezCare chMyezcareOrg = new CacheHelper_MyezCare();
            MyEzcareOrganization myOrg = chMyezcareOrg.GetCachedData<MyEzcareOrganization>();
            DataTable mappingTbl = Common.ListToDataTable(mappingData);



            string conStr = ConfigurationManager.ConnectionStrings[Constants.MyezcareOrganizationConnectionString].ConnectionString;
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            SqlCommand cmd = new SqlCommand(StoredProcedure.GetSavedFormList, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UDT_EBFromMappingTable", mappingTbl);
            cmd.Parameters.AddWithValue("@OrganizationID", myOrg.OrganizationID);
            cmd.Parameters.AddWithValue("@MarketID", model.MarketID);
            cmd.Parameters.AddWithValue("@FormCategoryID", model.FormCategoryID);
            cmd.Parameters.AddWithValue("@FormName", model.FormName);
            cmd.Parameters.AddWithValue("@FormNumber", model.FormNumber);
            cmd.Parameters.AddWithValue("@IsDeleted", model.IsDeleted);

            cmd.Parameters.AddWithValue("@SortExpression", sortIndex);
            cmd.Parameters.AddWithValue("@SortType", sortDirection);
            cmd.Parameters.AddWithValue("@FromIndex", pageIndex);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);


            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            da.SelectCommand = cmd;
            da.Fill(dt);
            con.Close();

            List<SavedFormListModel> totalData = Common.DataTableToList<SavedFormListModel>(dt);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.Count; ;//totalData.First().Count;

            Page<SavedFormListModel> getOrganizationList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.IsSuccess = true;
            response.Data = getOrganizationList;
            return response;


        }

        #endregion


        #region Organition Form Mapping Screen Page - This page Organization will use and add more forms for the Organization's Users

        public ServiceResponse SetOrganizationFormsPage()
        {
            ServiceResponse response = new ServiceResponse();

            CacheHelper_MyezCare chMyezcareOrg = new CacheHelper_MyezCare();
            MyEzcareOrganization myOrg = chMyezcareOrg.GetCachedData<MyEzcareOrganization>();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(myOrg.OrganizationID) });

            OrganizationFormPageModel data = GetMultipleEntity<OrganizationFormPageModel>(StoredProcedure.SetOrganizationFormPage, searchList);
            response.Data = data;
            response.IsSuccess = true;
            return response;
        }



        public ServiceResponse SaveOrganizationFormDetails(List<OrganizationFormModel> model, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();

            CacheHelper_MyezCare chMyezcareOrg = new CacheHelper_MyezCare();
            MyEzcareOrganization myOrg = chMyezcareOrg.GetCachedData<MyEzcareOrganization>();


            if (model == null)
                model = new List<OrganizationFormModel>();

            DataTable dataTbl = Common.ListToDataTable(model);
            string conStr = ConfigurationManager.ConnectionStrings[Constants.MyezcareOrganizationConnectionString].ConnectionString;
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            SqlCommand cmd = new SqlCommand(StoredProcedure.SaveOrganizationSelectedForms, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrganizationID", myOrg.OrganizationID);
            cmd.Parameters.AddWithValue("@UDT_OrganizationFormsTable", dataTbl);
            cmd.Parameters.AddWithValue("@LoggedInUserId", loggedInId);

            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            da.SelectCommand = cmd;
            da.Fill(dt);

            con.Close();

            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["TransactionResultId"]) == 1)
                {
                    response.IsSuccess = true;
                    response.Message = Resource.DetaiSavedSuccessfully;
                    return response;
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = Resource.ErrorOccured;
                return response;
            }



            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse SaveOrganizationFormName(OrganizationForm model, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();

            CacheHelper_MyezCare chMyezcareOrg = new CacheHelper_MyezCare();
            MyEzcareOrganization myOrg = chMyezcareOrg.GetCachedData<MyEzcareOrganization>();

            string conStr = ConfigurationManager.ConnectionStrings[Constants.MyezcareOrganizationConnectionString].ConnectionString;
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            SqlCommand cmd = new SqlCommand(StoredProcedure.SaveOrganizationFormName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrganizationFormID", model.OrganizationFormID);
            cmd.Parameters.AddWithValue("@OrganizationFriendlyFormName", model.OrganizationFriendlyFormName);
            cmd.Parameters.AddWithValue("@LoggedInUserId", loggedInId);
            int result = cmd.ExecuteNonQuery();
            con.Close();

            if (result == 0)
            {
                //Not updated.
                response.IsSuccess = false;
                response.Message = Resource.ErrorOccured;
            }
            else
            {
                response.IsSuccess = true;
                response.Message = Resource.DetaiSavedSuccessfully;
            }
            //if (dt.Rows.Count > 0)
            //{
            //    if (Convert.ToInt32(dt.Rows[0]["TransactionResultId"]) == 1)
            //    {
            //        response.IsSuccess = true;
            //        response.Message = Resource.DetaiSavedSuccessfully;
            //        return response;
            //    }
            //}
            //else
            //{
            //    response.IsSuccess = false;
            //    response.Message = Resource.ErrorOccured;
            //    return response;
            //}
            return response;
        }

        #endregion


        #region Form Tag
        public List<FormTagModel> GetSearchTag(int pageSize, string searchText = null)
        {
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "SearchText", Value = searchText},
                    new SearchValueData {Name = "PageSize", Value =pageSize.ToString()}
                };

            List<FormTagModel> model = GetEntityList<FormTagModel>(StoredProcedure.GetSearchTag, searchParam) ?? new List<FormTagModel>();

            if (model.Count == 0 || model.Count(c => c.FormTagName == searchText) == 0)
                model.Insert(0, new FormTagModel { FormTagName = searchText });

            return model;
        }

        public ServiceResponse GetOrgFormTagList(long OrganizationFormID)
        {
            ServiceResponse response = new ServiceResponse();
            List<FormTagModel> model = GetEntityList<FormTagModel>(StoredProcedure.GetOrgFormTagList, new List<SearchValueData>
                {
                    new SearchValueData {Name = "OrganizationFormID", Value = Convert.ToString(OrganizationFormID)}
                }) ?? new List<FormTagModel>();
            response.Data = model;
            response.IsSuccess = true;

            return response;
        }

        public ServiceResponse AddOrgFormTag(FormTagModel model)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "OrganizationFormID", Value = Convert.ToString(model.OrganizationFormID)},
                    new SearchValueData {Name = "FormTagID", Value = Convert.ToString(model.FormTagID)},
                    new SearchValueData {Name = "FormTagName", Value = model.FormTagName}
                };

            TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.AddOrgFormTag, searchParam);
            if (result.TransactionResultId > 0)
            {
                response.IsSuccess = true;
                response.Message = "Tag added";
            }
            else
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse DeleteFormTag(long OrganizationFormTagID)
        {
            ServiceResponse response = new ServiceResponse();
            if (OrganizationFormTagID > 0)
            {
                var searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData() { Name = "OrganizationFormTagID", Value = Convert.ToString(OrganizationFormTagID) });
                GetScalar(StoredProcedure.DeleteFormTag, searchList);
                response.IsSuccess = true;
                response.Message = "Tag deleted";
            }

            if (!response.IsSuccess)
                response.Message = Resource.ExceptionMessage;

            return response;
        }
        #endregion


        #region Load Internal HTML Forms

        public ServiceResponse GetSavedHtmlFormContent(long ebriggsFormMppingID)
        {

            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "EbriggsFormMppingID", Value = Convert.ToString(ebriggsFormMppingID) });
            NameValueDataInString data = GetEntity<NameValueDataInString>(StoredProcedure.HC_GetSavedHtmlFormContent, searchList);
            response.Data = data;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetHTMLFormTokenReplaceModel(long referralID)
        {

            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(referralID) });
            HTMLFormTokenReplaceModel data = GetEntity<HTMLFormTokenReplaceModel>(StoredProcedure.HC_GetHTMLFormTokenReplaceModel, searchList);
            response.Data = data;
            response.IsSuccess = true;
            return response;
        }


        public ServiceResponse GetPdfFieldsData(long? id, int typeId)
        {
            string conStr = ConfigurationManager.ConnectionStrings[Constants.MyezcareOrganizationConnectionString].ConnectionString;
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(conStr);
            string database = builder.InitialCatalog;
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "AdminDBName", Value = database });
            searchList.Add(new SearchValueData { Name = "Id", Value = id.ToString() });
            searchList.Add(new SearchValueData { Name = "TypeId", Value = typeId.ToString() });
            var data = GetEntityList<PDFFieldMapping>(StoredProcedure.HC_GetPdfFieldsData, searchList);
            response.Data = data;
            response.IsSuccess = true;
            return response;
        }


        #endregion



        public ServiceResponse AddEBCategory(string CategoryID, int IsINSUp)
        {
            var response = new ServiceResponse();
            var isEditMode = IsINSUp > 0;

            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "EBCategoryID", Value = Convert.ToString(CategoryID) });
                var model = GetMultipleEntity<EBCategoryModel>(StoredProcedure.GetEBCategoryDetail, searchParam);

                if (model.EBCategory == null)
                    model.EBCategory = new EBCategory();

                response.IsSuccess = true;
                response.Data = model;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, e.Message);
            }
            return response;
        }

        public ServiceResponse AddEBCategory(EBCategory Category, int IsINSUp, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {

                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "EBCategoryID", Value = Convert.ToString(Category.EBCategoryID) });
                dataList.Add(new SearchValueData { Name = "Id", Value = Category.ID });
                dataList.Add(new SearchValueData { Name = "Name", Value = Category.Name });
                dataList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(Category.IsDeleted) });
                dataList.Add(new SearchValueData { Name = "IsINSUp", Value = Convert.ToString(IsINSUp) });

                int data = (int)GetScalar(StoredProcedure.SaveEBCategory, dataList);


                if (data == -1)
                {
                    response.Message = Resource.CategoryAlreadyExists;
                    return response;
                }
                response.IsSuccess = true;
                response.Message = int.Parse(Category.EBCategoryID) > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Category) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.Category);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse DeleteCategory(SearchEBCategoryListPage searchEBCategoryListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForEBCategoryList(searchEBCategoryListPage, searchList);

            if (!string.IsNullOrEmpty(searchEBCategoryListPage.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchEBCategoryListPage.ListOfIdsInCsv });



            List<ListEBCategoryModel> totalData = GetEntityList<ListEBCategoryModel>(StoredProcedure.DeleteEBCategory, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListEBCategoryModel> ebCategoryModelList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = ebCategoryModelList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Physician);
            return response;
        }

        public ServiceResponse GetCategoryList(SearchEBCategoryListPage searchEBCategoryListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchEBCategoryListPage != null)
            {
                SetSearchFilterForEBCategoryList(searchEBCategoryListPage, searchList);
            }

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListEBCategoryModel> totalData = GetEntityList<ListEBCategoryModel>(StoredProcedure.GetEBCategoryList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListEBCategoryModel> physicianList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = physicianList;
            response.IsSuccess = true;
            return response;
        }
        public void SetSearchFilterForEBCategoryList(SearchEBCategoryListPage searchEBCategoryListPage, List<SearchValueData> searchList)
        {
            if (!string.IsNullOrEmpty(searchEBCategoryListPage.Name))
                searchList.Add(new SearchValueData { Name = "Name", Value = Convert.ToString(searchEBCategoryListPage.Name) });
            if (!string.IsNullOrEmpty(searchEBCategoryListPage.ID))
                searchList.Add(new SearchValueData { Name = "ID", Value = Convert.ToString(searchEBCategoryListPage.ID) });
            if (!string.IsNullOrEmpty(searchEBCategoryListPage.EBCategoryID))
                searchList.Add(new SearchValueData { Name = "EBCategoryID", Value = Convert.ToString(searchEBCategoryListPage.EBCategoryID) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchEBCategoryListPage.IsDeleted) });
        }
        public List<EBCategory> HC_GetEBCategoryListForAutoComplete(string searchText, int pageSize)
        {
            throw new NotImplementedException();
        }

        public ServiceResponse SetEBCategoryListPage()
        {
            ServiceResponse response = new ServiceResponse();
            SetEBCategoryListPage model = new SetEBCategoryListPage();

            model.DeleteFilter = Common.SetDeleteFilter();
            model.SearchEBCategoryListPage = new SearchEBCategoryListPage() { IsDeleted = 0 };
            response.Data = model;

            return response;
        }

    }
}
