using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Infrastructure.Utility.CareGiverApi;
using Zarephath.Core.Models; 
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class PhysicianDataProvider: BaseDataProvider, IPhysicianDataProvider
    {
        public ServiceResponse AddPhysician(long physicianID)
        {
            var response = new ServiceResponse();
            var isEditMode = physicianID > 0;

            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "PhysicianID", Value = Convert.ToString(physicianID) });
                var model = GetMultipleEntity<PhysicianModel>(StoredProcedure.GetPhysicianDetail, searchParam);

                if (model.Physician == null)
                    model.Physician = new Physician();

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

        public ServiceResponse AddPhysician(Physicians physician, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {

                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "PhysicianID", Value = Convert.ToString(physician.PhysicianID) });
                dataList.Add(new SearchValueData { Name = "FirstName", Value = physician.FirstName });
                dataList.Add(new SearchValueData { Name = "MiddleName", Value = physician.MiddleName });
                dataList.Add(new SearchValueData { Name = "LastName", Value = physician.LastName });
                dataList.Add(new SearchValueData { Name = "Address", Value = physician.Address });
                dataList.Add(new SearchValueData { Name = "City", Value = physician.City });
                dataList.Add(new SearchValueData { Name = "StateCode", Value = physician.StateCode });
                dataList.Add(new SearchValueData { Name = "ZipCode", Value = physician.ZipCode });
                dataList.Add(new SearchValueData { Name = "Email", Value = physician.Email });
                dataList.Add(new SearchValueData { Name = "Phone", Value = physician.Phone });
                dataList.Add(new SearchValueData { Name = "Mobile", Value = physician.Mobile });
                dataList.Add(new SearchValueData { Name = "NPINumber", Value = physician.NPINumber });
                dataList.Add(new SearchValueData { Name = "PhysicianTypeID", Value = physician.PhysicianTypeID });
                dataList.Add(new SearchValueData { Name = "loggedInUserId", Value = Convert.ToString(loggedInUserId) });
                dataList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });

                int data = (int)GetScalar(StoredProcedure.SavePhysician, dataList);


                if (data == -1)
                {
                    response.Message = Resource.PhysicianAlreadyExists;
                    return response;
                }
                response.IsSuccess = true;
                response.Message = physician.PhysicianID>0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Physician) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.Physician);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse SetPhysicianListPage()
        {
            ServiceResponse response = new ServiceResponse();
            SetPhysicianListPage model = new SetPhysicianListPage();
            
            model.DeleteFilter = Common.SetDeleteFilter();
            model.SearchPhysicianListPage = new SearchPhysicianListPage() { IsDeleted = 0 };
            response.Data = model;

            return response;
        }


        public ServiceResponse GetPhysicianList(SearchPhysicianListPage searchPhysicianListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchPhysicianListPage != null)
                SetSearchFilterForPhysicianList(searchPhysicianListPage, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListPhysicianModel> totalData = GetEntityList<ListPhysicianModel>(StoredProcedure.GetPhysicianList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListPhysicianModel> physicianList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = physicianList;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForPhysicianList(SearchPhysicianListPage searchPhysicianListPage, List<SearchValueData> searchList)
        {
            if (!string.IsNullOrEmpty(searchPhysicianListPage.PhysicianName))
                searchList.Add(new SearchValueData { Name = "PhysicianName", Value = Convert.ToString(searchPhysicianListPage.PhysicianName) });
            if (!string.IsNullOrEmpty(searchPhysicianListPage.NPINumber))
                searchList.Add(new SearchValueData { Name = "NPINumber", Value = Convert.ToString(searchPhysicianListPage.NPINumber) });
            if (!string.IsNullOrEmpty(searchPhysicianListPage.Email))
                searchList.Add(new SearchValueData { Name = "Email", Value = Convert.ToString(searchPhysicianListPage.Email) });
            if (!string.IsNullOrEmpty(searchPhysicianListPage.Address))
                searchList.Add(new SearchValueData { Name = "Address", Value = Convert.ToString(searchPhysicianListPage.Address) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchPhysicianListPage.IsDeleted) });
        }


        public ServiceResponse DeletePhysician(SearchPhysicianListPage searchPhysicianListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForPhysicianList(searchPhysicianListPage, searchList);

            if (!string.IsNullOrEmpty(searchPhysicianListPage.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchPhysicianListPage.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });

            List<ListPhysicianModel> totalData = GetEntityList<ListPhysicianModel>(StoredProcedure.DeletePhysician, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListPhysicianModel> physicianList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = physicianList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Physician);
            return response;
        }

        public List<Physicians> HC_GetPhysicianListForAutoComplete(string searchText, int pageSize = 10)
        {
            List<Physicians> physicianList = GetEntityList<Physicians>(StoredProcedure.HC_GetPhysicianListForAutoComplete,
                                            new List<SearchValueData>
                                                {
                                                    new SearchValueData {Name = "SearchText", Value = searchText},
                                                    new SearchValueData {Name = "PageSize", Value = pageSize.ToString()}
                                                });
            return physicianList;
        }
        public List<Specialist> GetSpecialistListForAutoComplete(string searchText, string ignoreIds, int pageSize)
        {
            List<Specialist> Specialist = new List<Specialist>();
            // string[] strArr = null;
            if (searchText != "")
            {
                ServiceResponse response = new ServiceResponse();
                CareGiverApi careGiverApi = new CareGiverApi();
                var results = careGiverApi.GetSpecialist(searchText);
                foreach (var item in results)
                {
                    Specialist.Add(new Specialist
                    {
                        Name = item.Name,
                        NPI = item.NPI,
                        Type = item.Type,
                        PracticeAddress = item.PracticeAddress,
                    });
                }
            }
            return Specialist;

        }
        public ServiceResponse SaveSpecialist(string Specialist, string Name, string NPI, string PracticeAddress)
        {
            ServiceResponse response = new ServiceResponse();

            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "Specialist", Value = Convert.ToString(Specialist)},
                    new SearchValueData {Name = "Name", Value = Convert.ToString(Name)},
                    new SearchValueData {Name = "NPI", Value = Convert.ToString(Name)},
                    new SearchValueData {Name = "PracticeAddress", Value = Convert.ToString(PracticeAddress)},
                };


            GetScalar(StoredProcedure.SaveSpecialist, searchlist);
            response.IsSuccess = true;
            //  response.Message = "DxCodeSavedSuccessfully";
            APIPhysicianModel SpecialistLists = GetEntity<APIPhysicianModel>(StoredProcedure.GetSpecialist, searchlist

                );
            if (SpecialistLists != null)
            {
                var SpecialistList = SpecialistLists;
            }
            else
            {
                var SpecialistList = "";
            }

            response.Data = SpecialistLists;
            return response;
        }
    }
}
