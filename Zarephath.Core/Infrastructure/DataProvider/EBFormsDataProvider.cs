using PetaPoco;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class EBFormsDataProvider : BaseDataProvider, IEBFormsDataProvider
    {
        public EBFormsDataProvider(string conString)
            : base(conString)
        {
        }
        public ServiceResponse AddEBForms(string FormID, int IsINSUp)
        {
            var response = new ServiceResponse();
            var isEditMode = IsINSUp > 0;

            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "EBFormId", Value = Convert.ToString(FormID) });
                EBFormsModel model = GetMultipleEntity<EBFormsModel>(StoredProcedure.GetEBFormsDetail, searchParam);
                if (model.EBForms == null)
                    model.EBForms = new EBForms { FormPrice = "0" };
                response.Data = model;
                response.IsSuccess = true;
                return response;


            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, e.Message);
            }
            return response;
        }

        public ServiceResponse AddEBForms(EBForms Forms, int IsINSUp, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {

                //CacheHelper_MyezCare chMyezcareOrg = new CacheHelper_MyezCare();
                //MyEzcareOrganization myOrg = chMyezcareOrg.GetCachedData<MyEzcareOrganization>();

                //string conStr = ConfigurationManager.ConnectionStrings[Constants.MyezcareOrganizationConnectionString].ConnectionString;
                //SqlConnection con = new SqlConnection(conStr);
                //con.Open();
                //SqlCommand cmd = new SqlCommand(StoredProcedure.SaveEBForms, con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("FormId", Forms.FormId);
                //cmd.Parameters.AddWithValue("Name", Forms.Name);
                //cmd.Parameters.AddWithValue("FormLongName", Forms.FormLongName);
                //cmd.Parameters.AddWithValue("NameForUrl", Forms.NameForUrl);
                //cmd.Parameters.AddWithValue("Version", Forms.Version);
                //cmd.Parameters.AddWithValue("IsActive", Convert.ToString(Forms.IsActive));
                //cmd.Parameters.AddWithValue("EBCategoryID", Forms.EBCategoryID);
                //cmd.Parameters.AddWithValue("EbMarketIDs", Forms.EbMarketID);
                //cmd.Parameters.AddWithValue("FormPrice", Forms.FormPrice);

                //cmd.Parameters.AddWithValue("IsDeleted", Convert.ToString(Forms.IsDeleted));
                //cmd.Parameters.AddWithValue("IsINSUp", Convert.ToString(IsINSUp));
                //int data = Convert.ToInt32(cmd.ExecuteScalar());
                //con.Close();


                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "FormId", Value = Forms.FormId });
                dataList.Add(new SearchValueData { Name = "Name", Value = Forms.Name });
                dataList.Add(new SearchValueData { Name = "FormLongName", Value = Forms.FormLongName });
                dataList.Add(new SearchValueData { Name = "NameForUrl", Value = Forms.NameForUrl });
                dataList.Add(new SearchValueData { Name = "Version", Value = Forms.Version });
                dataList.Add(new SearchValueData { Name = "IsActive", Value = Convert.ToString(Forms.IsActive) });
                dataList.Add(new SearchValueData { Name = "EBCategoryID", Value = Forms.EBCategoryID });
                dataList.Add(new SearchValueData { Name = "EbMarketIDs", Value = Forms.EbMarketIDs });
                dataList.Add(new  SearchValueData { Name = "FormPrice", Value = Forms.FormPrice });
                dataList.Add(new SearchValueData { Name = "InternalFormPath", Value = Forms.InternalFormPath });
                dataList.Add(new SearchValueData { Name = "NewPdfURI", Value = Forms.NewPdfURI });
                dataList.Add(new SearchValueData { Name = "IsInternalForm", Value = Convert.ToString(Forms.IsInternalForm) });
                dataList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(Forms.IsDeleted) });
                dataList.Add(new SearchValueData { Name = "IsINSUp", Value = Convert.ToString(IsINSUp) });
                dataList.Add(new SearchValueData { Name = "IsOrbeonForm", Value = Convert.ToString(Forms.IsOrbeonForm) });

                int data = Convert.ToInt32(GetScalar(StoredProcedure.SaveEBForms, dataList));
                if (data == -1)
                {
                    response.Message = Resource.Form + " " + Resource.AlreadyExists;
                    return response;
                }
                if (data == -2)
                {
                    response.IsSuccess = true;
                    response.Message = data > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Form) :
                        string.Format(Resource.RecordUpdatedSuccessfully, Resource.Form);

                    return response;
                }
                response.IsSuccess = true;
                response.Message = data > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Form) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.Form);

                //if (data == -1)
                //{
                //    response.Message = Resource.MarketAlreadyExists;
                //    return response;
                //}
                //response.IsSuccess = true;
                //response.Message = data > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Forms) :
                //    string.Format(Resource.RecordCreatedSuccessfully, Resource.Forms);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse DeleteEBForms(SearchEBFormsListPage searchEBFormsListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForEBFormsList(searchEBFormsListPage, searchList);

            if (!string.IsNullOrEmpty(searchEBFormsListPage.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchEBFormsListPage.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });


            List<ListEBFormsModel> totalData = GetEntityList<ListEBFormsModel>(StoredProcedure.Deleteebforms, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListEBFormsModel> ebCategoryModelList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = ebCategoryModelList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Category);
            return response;
        }

        public ServiceResponse GetEBFormsList(SearchEBFormsListPage searchEBFormsListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchEBFormsListPage != null)
            {
                SetSearchFilterForEBFormsList(searchEBFormsListPage, searchList);
            }

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListEBFormsModel> totalData = GetEntityList<ListEBFormsModel>(StoredProcedure.Getebformslist, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListEBFormsModel> FormsList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = FormsList;
            response.IsSuccess = true;
            return response;
        }
        public void SetSearchFilterForEBFormsList(SearchEBFormsListPage searchEBFormsListPage, List<SearchValueData> searchList)
        {
            if (!string.IsNullOrEmpty(searchEBFormsListPage.Name))
                searchList.Add(new SearchValueData { Name = "Name", Value = Convert.ToString(searchEBFormsListPage.Name) });
            if (!string.IsNullOrEmpty(searchEBFormsListPage.ID))
                searchList.Add(new SearchValueData { Name = "ID", Value = Convert.ToString(searchEBFormsListPage.ID) });
            if (!string.IsNullOrEmpty(searchEBFormsListPage.FormID))
                searchList.Add(new SearchValueData { Name = "FormID", Value = Convert.ToString(searchEBFormsListPage.FormID) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchEBFormsListPage.IsDeleted) });
        }
        public List<EBForms> HC_GetEBFormsListForAutoComplete(string searchText, int pageSize)
        {
            throw new NotImplementedException();
        }

        public ServiceResponse SetEBFormsListPage()
        {
            ServiceResponse response = new ServiceResponse();
            SetEBFormsListPage model = new SetEBFormsListPage();

            model.DeleteFilter = Common.SetDeleteFilter();
            model.SearchEBFormsListPage = new SearchEBFormsListPage() { IsDeleted = 0 };
            response.Data = model;

            return response;
        }
    }
}
