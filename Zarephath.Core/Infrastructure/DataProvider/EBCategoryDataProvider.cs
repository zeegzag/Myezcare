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
    public class EBCategoryDataProvider : BaseDataProvider, IEBCategoryDataProvider
    {
        public EBCategoryDataProvider(string conString)
            : base(conString)
        {
        }
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

                int data = Convert.ToInt32(GetScalar(StoredProcedure.SaveEBCategory, dataList));


                if (data == -1)
                {
                    response.Message = Resource.CategoryAlreadyExists;
                    return response;
                }
                if (data == -2)
                {
                    response.IsSuccess = true;
                    response.Message = data > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Category) :
                        string.Format(Resource.RecordUpdatedSuccessfully, Resource.Category);

                    return response;
                }
                response.IsSuccess = true;
                response.Message = data > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Category) :
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

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });


            List<ListEBCategoryModel> totalData = GetEntityList<ListEBCategoryModel>(StoredProcedure.DeleteEBCategory, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListEBCategoryModel> ebCategoryModelList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
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
