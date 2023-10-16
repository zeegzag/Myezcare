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


namespace Myezcare_Admin.Infrastructure.DataProvider
{
    public class CronJobDataProvider : BaseDataProvider, ICronJobDataProvider
    {
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

            string conStr = ConfigurationManager.ConnectionStrings[Constants.ConnectionStringName].ConnectionString;
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
