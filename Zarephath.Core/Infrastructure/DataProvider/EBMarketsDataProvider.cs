using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class EBMarketsDataProvider : BaseDataProvider, IEBMarketsDataProvider
    {
        public EBMarketsDataProvider(string conString)
            : base(conString)
        {
        }
        public ServiceResponse AddEBMarkets(string EBMarketID, int IsINSUp)
        {
            var response = new ServiceResponse();
            var isEditMode = IsINSUp > 0;

            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "EBMarketID", Value = Convert.ToString(EBMarketID) });
                var model = GetMultipleEntity<EBMarketsModel>(StoredProcedure.GetEBMarketsDetail, searchParam);

                if (model.EBMarkets == null)
                    model.EBMarkets = new EBMarkets();

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

        public ServiceResponse AddEBMarkets(EBMarkets markets, int IsINSUp, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {

                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "EBMarketID", Value = Convert.ToString(markets.EBMarketID) });
                dataList.Add(new SearchValueData { Name = "Id", Value = markets.ID });
                dataList.Add(new SearchValueData { Name = "Name", Value = markets.Name });
                dataList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(markets.IsDeleted) });
                dataList.Add(new SearchValueData { Name = "IsINSUp", Value = Convert.ToString(IsINSUp) });

                int data = Convert.ToInt32(GetScalar(StoredProcedure.SaveEBMarkets, dataList));


                if (data == -1)
                {
                    response.Message = Resource.MarketAlreadyExists;
                    return response;
                }
                response.IsSuccess = true;
                response.Message = data > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Markets) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.Markets);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse DeleteEBMarkets(SearchEBMarketsListPage searchEBMarketsListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForEBMarketsList(searchEBMarketsListPage, searchList);

            if (!string.IsNullOrEmpty(searchEBMarketsListPage.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchEBMarketsListPage.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });


            List<ListEBMarketsModel> totalData = GetEntityList<ListEBMarketsModel>(StoredProcedure.Deleteebmarkets, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListEBMarketsModel> ebCategoryModelList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = ebCategoryModelList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Category);
            return response;
        }
        //[HttpPost]
        //[CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        //public ContentResult GetFormList(SearchFormModel model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        //{
        //    _formDataProvider = new FormDataProvider(Constants.MyezcareOrganizationConnectionString);
        //    var response = _formDataProvider.GetFormList(model, pageIndex, pageSize, sortIndex, sortDirection);
        //    return CustJson(response);
        //}

        public ServiceResponse GetEBMarketsList(SearchEBMarketsListPage searchEBMarketsListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchEBMarketsListPage != null)
            {
                SetSearchFilterForEBMarketsList(searchEBMarketsListPage, searchList);
            }

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListEBMarketsModel> totalData = GetEntityList<ListEBMarketsModel>(StoredProcedure.Getebmarketslist, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListEBMarketsModel> MarketsList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = MarketsList;
            response.IsSuccess = true;
            return response;
        }
        public void SetSearchFilterForEBMarketsList(SearchEBMarketsListPage searchEBMarketsListPage, List<SearchValueData> searchList)
        {
            if (!string.IsNullOrEmpty(searchEBMarketsListPage.Name))
                searchList.Add(new SearchValueData { Name = "Name", Value = Convert.ToString(searchEBMarketsListPage.Name) });
            if (!string.IsNullOrEmpty(searchEBMarketsListPage.ID))
                searchList.Add(new SearchValueData { Name = "ID", Value = Convert.ToString(searchEBMarketsListPage.ID) });
            if (!string.IsNullOrEmpty(searchEBMarketsListPage.EBMarketID))
                searchList.Add(new SearchValueData { Name = "EBMarketID", Value = Convert.ToString(searchEBMarketsListPage.EBMarketID) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchEBMarketsListPage.IsDeleted) });
        }
        public List<EBMarkets> HC_GetEBMarketsListForAutoComplete(string searchText, int pageSize)
        {
            throw new NotImplementedException();
        }

        public ServiceResponse SetEBMarketsListPage()
        {
            ServiceResponse response = new ServiceResponse();
            SetEBMarketsListPage model = new SetEBMarketsListPage();

            model.DeleteFilter = Common.SetDeleteFilter();
            model.SearchEBMarketsListPage = new SearchEBMarketsListPage() { IsDeleted = 0 };
            response.Data = model;

            return response;
        }
    }
}
