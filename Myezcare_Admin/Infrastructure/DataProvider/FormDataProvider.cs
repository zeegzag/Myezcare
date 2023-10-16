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
    public class FormDataProvider : BaseDataProvider, IFormDataProvider
    {

        #region Form List

        public ServiceResponse SetFormListPage()
        {
            ServiceResponse respose = new ServiceResponse();

            List<SearchValueData> searchModel = new List<SearchValueData>();
            FormPageModel data = GetMultipleEntity<FormPageModel>(StoredProcedure.SetFormListPage, searchModel);
            respose.Data = data;
            respose.IsSuccess = true;
            return respose;
        }

        public ServiceResponse GetFormList(SearchFormModel model, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            SetSearchFilterForFormListPage(model, searchList);
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<FormListModel> totalData = GetEntityList<FormListModel>(StoredProcedure.GetFormList, searchList);

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
            searchList.Add(new SearchValueData { Name = "MarketID", Value = Convert.ToString(model.MarketID) });
            searchList.Add(new SearchValueData { Name = "FormCategoryID", Value = Convert.ToString(model.FormCategoryID) });
            searchList.Add(new SearchValueData { Name = "FormName", Value = model.FormName });
            searchList.Add(new SearchValueData { Name = "FormNumber", Value = model.FormNumber });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(model.IsDeleted) });
        }

        public ServiceResponse DeleteForm(SearchFormModel model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                SetSearchFilterForFormListPage(model, searchList);

                if (!string.IsNullOrEmpty(model.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = model.ListOfIdsInCSV });

                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

                List<FormListModel> totalData = GetEntityList<FormListModel>(StoredProcedure.DeleteForm, searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<FormListModel> listPayorModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = listPayorModel;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Form);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }


        public ServiceResponse UpdateFormPrice(FormListModel model, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();

                searchList.Add(new SearchValueData() { Name = "EBFormID", Value = model.EBFormID, IsEqual = true });
                searchList.Add(new SearchValueData() { Name = "FormPrice", Value = Convert.ToString(model.FormPrice)});
                searchList.Add(new SearchValueData() { Name = "LoggedInID", Value = Convert.ToString(loggedInUserID)});
                GetScalar(StoredProcedure.UpdateFormPrice, searchList);
                response.IsSuccess = true;
                response.Message = Resource.PriceUpdatedSuccessfully;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Resource.ErrorOccured;
            }
            return response;
        }

        


        #endregion 


        public ServiceResponse UpdateEbriggsFormDetails()
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchModel = new List<SearchValueData>();
            //HC_EBFormModel data = GetMultipleEntity<HC_EBFormModel>(StoredProcedure.HC_GetFormListPage, searchModel);

            FormApi formApi = new FormApi();

            ServiceResponse res = formApi.Authenticate();
            if (!res.IsSuccess) return res;
            FormApi_LoginResponseModel loginResponseModel = (FormApi_LoginResponseModel)res.Data;


            #region Get Market Data 


            ServiceResponse marResponse = formApi.GetMarkets(loginResponseModel.tenantGuid);
            if (!marResponse.IsSuccess) return marResponse;
            var marketList = (List<FormApi_MarketModel>)marResponse.Data;



            #endregion


            #region Get FormList

            ServiceResponse formResponse = formApi.GetFormList(loginResponseModel.tenantGuid);
            if (!formResponse.IsSuccess) return formResponse;
            var formList = (List<FormApi_FormModel>)formResponse.Data;



            List<FormApi_FormCategory> fromCategory = new List<FormApi_FormCategory>();
            foreach (var item in formList)
            {
                if (item.FormCategory != null && string.IsNullOrEmpty(item.FormCategory.Id) == false)
                {
                    item.EbCategoryID = item.FormCategory.Id;
                    if (fromCategory.Count(c => c.Id == item.FormCategory.Id) == 0)
                        fromCategory.Add(item.FormCategory);
                }

                item.FromUniqueID = item.UniqueFormID.Id;


                foreach (var package in item.FormPackages)
                {
                    if (!string.IsNullOrEmpty(package.PackageId))
                    {
                        if (string.IsNullOrEmpty(item.EbMarketIDs))
                            item.EbMarketIDs = package.PackageId;
                        else
                            item.EbMarketIDs = item.EbMarketIDs + "," + package.PackageId;
                    }

                }

            }

            #endregion


            #region Get Category & Save into Database

            fromCategory = fromCategory.OrderBy(c => c.Name).ToList();




            DataTable marketTbl = Common.ListToDataTable(marketList);
            DataTable categoryTbl = Common.ListToDataTable(fromCategory);
            DataTable formTbl = Common.ListToDataTable(formList);

            formTbl.Columns.Remove("UniqueFormID");
            formTbl.Columns.Remove("FormPackages");
            formTbl.Columns.Remove("FormCategory");

            string conStr = ConfigurationManager.ConnectionStrings[Constants.MyezcareOrganizationConnectionString].ConnectionString;
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            SqlCommand cmd = new SqlCommand(StoredProcedure.SyncEbFromRelatedAllData, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UDT_EBMarketTable", marketTbl);
            cmd.Parameters.AddWithValue("@UDT_EBCategoryTable", categoryTbl);
            cmd.Parameters.AddWithValue("@UDT_EBFromTable", formTbl);
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
                    response.Message = Resource.EbFormSysncingCompleted;
                    return response;
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = Resource.ErrorOccured;
                return response;
            }

            #endregion

            
            return response;
        }
    }
}
