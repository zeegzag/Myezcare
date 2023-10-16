using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class ParentDataProvider : BaseDataProvider, IParentDataProvider
    {
        #region Add Parent

        public ServiceResponse SetAddParentPage(long parentID)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData() { Name = "ContactID",Value = parentID.ToString()});
            AddParentModel model = GetMultipleEntity<AddParentModel>(StoredProcedure.SetAddParentPage, searchList);
            if (model.Contact==null)
                model.Contact=new Contact();

            response.Data = model;
            return response;
        }

        public ServiceResponse AddParent(Contact model, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            SaveObject(model, loggedInUserID);
            response.IsSuccess = true;
            response.Message = model.ContactID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Parent) : string.Format(Resource.RecordCreatedSuccessfully, Resource.Parent);
            return response;
        }

        #endregion

        #region Parent List

        public ServiceResponse SetAddParentListPage()
        {
            ServiceResponse response = new ServiceResponse();

            SetParentListPage setParentListPage = GetMultipleEntity<SetParentListPage>(StoredProcedure.SetParentListPage);
            setParentListPage.DeleteFilter = Common.SetDeleteFilter();
            setParentListPage.SearchParentListPage.IsDeleted = 0;
            setParentListPage.SearchParentListPage.ContactTypeID = (int)ContactType.ContactTypes.Primary_Placement;
            response.Data = setParentListPage;
            return response;
        }


        public ServiceResponse GetParentList(SearchParentListPage searchParentListPage, int pageIndex,
                                                  int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchParentListPage != null)
                SetSearchFilterForParentList(searchParentListPage, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListParentModel> totalData = GetEntityList<ListParentModel>(StoredProcedure.GetParentList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListParentModel> data = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = data;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForParentList(SearchParentListPage searchParentListPage, List<SearchValueData> searchList)
        {
            if (!string.IsNullOrEmpty(searchParentListPage.Name))
                searchList.Add(new SearchValueData { Name = "Name", Value = Convert.ToString(searchParentListPage.Name) });

            if (!string.IsNullOrEmpty(searchParentListPage.Email))
                searchList.Add(new SearchValueData { Name = "Email", Value = Convert.ToString(searchParentListPage.Email) });


            if (!string.IsNullOrEmpty(searchParentListPage.Address))
                searchList.Add(new SearchValueData { Name = "Address", Value = Convert.ToString(searchParentListPage.Address) });

            if (!string.IsNullOrEmpty(searchParentListPage.City))
                searchList.Add(new SearchValueData { Name = "City", Value = Convert.ToString(searchParentListPage.City) });

            if (!string.IsNullOrEmpty(searchParentListPage.ZipCode))
                searchList.Add(new SearchValueData { Name = "ZipCode", Value = Convert.ToString(searchParentListPage.ZipCode) });

            if (!string.IsNullOrEmpty(searchParentListPage.Phone1))
                searchList.Add(new SearchValueData { Name = "Phone", Value = Convert.ToString(searchParentListPage.Phone1) });

            searchList.Add(new SearchValueData { Name = "ContactTypeID", Value = Convert.ToString(searchParentListPage.ContactTypeID) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchParentListPage.IsDeleted) });

        }


        public ServiceResponse DeleteParent(SearchParentListPage searchParentListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForParentList(searchParentListPage, searchList);

            if (!string.IsNullOrEmpty(searchParentListPage.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchParentListPage.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

            List<ListParentModel> totalData = GetEntityList<ListParentModel>(StoredProcedure.DeleteParent, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListParentModel> data = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = data;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Parent);
            return response;
        }

        #endregion
    }
}
