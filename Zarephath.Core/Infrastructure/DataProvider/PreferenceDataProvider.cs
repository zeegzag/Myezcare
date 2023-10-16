using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class PreferenceDataProvider : BaseDataProvider, IPreferenceDataProvider
    {
        public ServiceResponse AddPreference(long preferenceId)
        {
            ServiceResponse response = new ServiceResponse();

            AddPreferenceModel addVisitTaskModel = GetMultipleEntity<AddPreferenceModel>(StoredProcedure.AddPreferencePageModel,
                new List<SearchValueData>
                {
                    new SearchValueData { Name = "PreferenceID", Value = Convert.ToString(preferenceId) }
                });
            if (addVisitTaskModel.Preference == null)
                addVisitTaskModel.Preference = new Preference();

            addVisitTaskModel.PreferenceTypes = Common.GetPreferenceKeyType();
            response.Data = addVisitTaskModel;
            return response;
        }

        public ServiceResponse AddPreference(AddPreferenceModel addPreferenceModel, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                bool isEditMode = addPreferenceModel.Preference.PreferenceID > 0;
                Preference model = addPreferenceModel.Preference;
                int data = (int)GetScalar(StoredProcedure.SavePreference, new List<SearchValueData>
                {
                    new SearchValueData {Name = "PreferenceID",Value = Convert.ToString(model.PreferenceID)},
                    new SearchValueData {Name = "PreferenceName",Value =model.PreferenceName },
                    new SearchValueData {Name = "KeyType",Value =model.KeyType},
                    new SearchValueData {Name = "loggedInUserId",Value = Convert.ToString(loggedInUserId)},
                    new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() }

                });
                if (data == -1)
                {
                    response.Message = string.Format(Resource.AlreadyExists, model.KeyType);
                    return response;
                }
                response.IsSuccess = true;
                response.Message = isEditMode ? string.Format(Resource.RecordUpdatedSuccessfully, model.KeyType) :
                    string.Format(Resource.RecordCreatedSuccessfully, model.KeyType);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse SetPreferenceListPage()
        {
            ServiceResponse response = new ServiceResponse();
            //SetPreferenceListPage model = GetMultipleEntity<SetPreferenceListPage>(StoredProcedure.SetVisitTaskListPage,
            //    new List<SearchValueData>
            //    {
            //        new SearchValueData { Name = "ServiceCodeTypeID", Value = Convert.ToString((int)ServiceCodeType.ServiceCodeTypes.HomeCare) }
            //    });
            SetPreferenceListPage model=new SetPreferenceListPage();
            model.PreferenceTypes = Common.GetPreferenceKeyType();
            model.DeleteFilter = Common.SetDeleteFilter();
            model.SearchPreferenceListPage = new SearchPreferenceListPage() { IsDeleted = 0 };
            response.Data = model;
            return response;
        }

        public ServiceResponse GetPreferenceList(SearchPreferenceListPage searchPreferenceListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchPreferenceListPage != null)
                SetSearchFilterForPrefernceList(searchPreferenceListPage, searchList);
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListPreferenceModel> totalData = GetEntityList<ListPreferenceModel>(StoredProcedure.GetPreferenceList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListPreferenceModel> data = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = data;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForPrefernceList(SearchPreferenceListPage searchPreferenceListPage, List<SearchValueData> searchList)
        {
            if (!string.IsNullOrEmpty(searchPreferenceListPage.PreferenceType))
                searchList.Add(new SearchValueData { Name = "PreferenceType", Value = Convert.ToString(searchPreferenceListPage.PreferenceType) });

            if (!string.IsNullOrEmpty(searchPreferenceListPage.PreferenceName))
                searchList.Add(new SearchValueData { Name = "PreferenceName", Value = Convert.ToString(searchPreferenceListPage.PreferenceName) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchPreferenceListPage.IsDeleted) });
        }

        public ServiceResponse DeletePreference(SearchPreferenceListPage searchPreferenceListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
            SetSearchFilterForPrefernceList(searchPreferenceListPage, searchList);

            if (!string.IsNullOrEmpty(searchPreferenceListPage.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchPreferenceListPage.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });

            List<ListPreferenceModel> totalData = GetEntityList<ListPreferenceModel>(StoredProcedure.DeletePreference, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListPreferenceModel> data = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = data;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Record);
            return response;
        }
    }
}
