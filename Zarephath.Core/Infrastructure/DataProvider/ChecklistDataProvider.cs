using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using PetaPoco;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class ChecklistDataProvider : BaseDataProvider, IChecklistDataProvider
    {
        public ServiceResponse GetChecklistItemTypes()
        {
            var response = new ServiceResponse();

            List<ChecklistItemType> checklistItemTypes = GetEntityList<ChecklistItemType>();

            response.Data = checklistItemTypes;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetChecklistItems(ChecklistItemModel model)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            searchList.Add(new SearchValueData { Name = "ChecklistItemTypeID", Value = Convert.ToString(model.ChecklistItemTypeID) });
            searchList.Add(new SearchValueData { Name = "ChecklistItemTypePrimaryID", Value = Convert.ToString(model.ChecklistItemTypePrimaryID) });

            ChklistItemModel result = GetMultipleEntity<ChklistItemModel>(StoredProcedure.GetChecklistItems, searchList);
            response.Data = result;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse SaveChecklistItems(SaveChecklistItemModel model, long loggedInUserId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if(model.ChecklistItems.Count > 0)
            {
                model.ChecklistItems.ForEach(m => m.ChecklistItemTypePrimaryID = Convert.ToInt64(Crypto.Decrypt(model.EncryptedPrimaryID)));
                model.ChecklistItems.ForEach(m => m.ChecklistItemTypeID = model.ChecklistItemTypeID);
            }
                

            string systemId = System.Web.HttpContext.Current.Request.UserHostAddress;
            DataTable dataTbl = Common.ListToDataTable(model.ChecklistItems);

            //var connectionString = ConfigurationManager.ConnectionStrings[Constants.ZarephathConnectionString].ConnectionString;

            MyEzcareOrganization orgData = GetOrganizationConnectionString();
            if (orgData != null)
            {
                //var connectionString = string.Format("Server={0};Database={1}; User ID={2};Password={3}", orgData.DBServer, orgData.DBName, orgData.DBUserName, orgData.DBPassword);
                SqlConnection con = new SqlConnection(orgData.CurrentConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand(StoredProcedure.SaveChecklistItems, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UDTChecklistItems", dataTbl);
                cmd.Parameters.AddWithValue("@LoggedInUserId", loggedInUserId);
                cmd.Parameters.AddWithValue("@SystemID", systemId);

                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = cmd;
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["TransactionResultId"]) == -1)
                    {
                        response.IsSuccess = false;
                        response.Message = Resource.ErrorOccured;
                        return response;
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = Resource.ErrorOccured;
                    return response;
                }
                response.Message = string.Format(Resource.RecordCreatedSuccessfully, Resource.CheckList);
                response.IsSuccess = true;
            }
            else
            {
                response.Message = Resource.DomainNotExist;
                response.IsSuccess = false;
            }
            return response;
        }

        public ServiceResponse GetIsChecklistRemaining(ChecklistItemModel model)
        {
            var response = new ServiceResponse();
            var checklistSearchList = new List<SearchValueData>
                                {
                                    new SearchValueData { Name = "ChecklistItemTypeID", Value = Convert.ToString(Common.ChecklistType.PatientIntake.GetHashCode())},
                                    new SearchValueData { Name = "ChecklistItemTypePrimaryID", Value = Convert.ToString(model.ChecklistItemTypePrimaryID) }
                                };
            TransactionResult checklistResult = GetEntity<TransactionResult>(StoredProcedure.GetIsChecklistRemaining, checklistSearchList);

            if (checklistResult != null)
            {
                response.IsSuccess = true;
                response.Data = checklistResult.TransactionResultId == 1;
            }
            return response;
        }

        public ServiceResponse GetVisitChecklistItems(ChecklistItemModel model)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            int noOfDays = model.VisitType == 1 ? (int)Common.CareTypeFrequency.Month : (int)Common.CareTypeFrequency.Year;

            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ChecklistItemTypePrimaryID) });
            searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(model.StartDate).ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(model.EndDate).ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "NoOfDays", Value = Convert.ToString(noOfDays) });

            List<VisitChecklistItemModel> result = GetEntityList<VisitChecklistItemModel>(StoredProcedure.GetVisitChecklistItems, searchList);
            response.Data = result;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetVisitChecklistItemDetail(VisitChecklistItemModel model)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            searchList.Add(new SearchValueData { Name = "EmployeeVisitID", Value = Convert.ToString(model.EmployeeVisitID) });
            searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });

            List<VisitChecklistItemDetail> result = GetEntityList<VisitChecklistItemDetail>(StoredProcedure.GetVisitChecklistItemDetail, searchList);
            response.Data = result;
            response.IsSuccess = true;
            return response;
        }
    }
}
