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
    public class CaseManagerDataProvider : BaseDataProvider, ICaseManagerDataProvider
    {
        #region Add Case Manager

        public ServiceResponse SetAddCaseManagerPage(long caseManagerID, long agencyID, long agencyLocationID)
        {
            ServiceResponse response = new ServiceResponse();


            AddCaseManagerModel addCaseManagerModel = new AddCaseManagerModel();

            if (caseManagerID > 0)
            {
                List<SearchValueData> searchCaseManagerParam = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "CaseManagerID", Value = caseManagerID.ToString()},
                        //new SearchValueData {Name = "IsDeleted", Value = "0"},
                    };
                CaseManager caseManager = GetEntity<CaseManager>(searchCaseManagerParam);
                addCaseManagerModel.AgencyList = GetEntityList<AgencyDropDownModel>(null, string.Format("IsDeleted ={0} OR AgencyID = {1}", 0, caseManager.AgencyID),"NickName","ASC");



                if (caseManager != null && caseManager.CaseManagerID > 0)
                {
                    addCaseManagerModel.CaseManager = caseManager;

                    response.IsSuccess = true;
                }
                else
                {
                    response.ErrorCode = Constants.ErrorCode_NotFound;
                    return response;
                }
            }
            else
            {
                addCaseManagerModel.AgencyList = GetEntityList<AgencyDropDownModel>(null, string.Format("IsDeleted ={0}", 0),"NickName","ASC");
                if (agencyID > 0 && caseManagerID == 0)
                {
                    bool isExistRecord = false;
                    if (addCaseManagerModel.AgencyList.Count > 0)
                    {
                        isExistRecord = addCaseManagerModel.AgencyList.Any(m => m.AgencyID == agencyID);
                    }
                    if (isExistRecord)
                    {
                        addCaseManagerModel.CaseManager.AgencyID = agencyID;
                        addCaseManagerModel.AgencyLocationList = (List<AgencyLocation>)GetAgencyLocation(agencyID, 0).Data;
                    }
                }
                if (agencyLocationID > 0 && caseManagerID == 0)
                {
                    bool isExistRecord = false;
                    if (addCaseManagerModel.AgencyLocationList.Count > 0)
                    {
                        isExistRecord = addCaseManagerModel.AgencyLocationList.Any(m => m.AgencyLocationID == agencyLocationID);
                    }

                    if (isExistRecord)
                        addCaseManagerModel.CaseManager.AgencyLocationID = agencyLocationID;
                }
            }

            response.Data = addCaseManagerModel;
            return response;
        }


        public ServiceResponse GetAgencyLocation(long agencyID, long agencyLocationID)
        {
            ServiceResponse response = new ServiceResponse();

            if (agencyID > 0)
            {

                List<SearchValueData> searchParam = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "AgencyID", Value = agencyID.ToString()}
                    };
                List<AgencyLocation> agencyLocationList = GetEntityList<AgencyLocation>(searchParam, string.Format("IsDeleted ={0} OR AgencyLocationID={1}", 0, agencyLocationID));
                if (agencyLocationList.Any())
                {
                    response.Data = agencyLocationList;
                    response.IsSuccess = true;
                }
            }
            return response;
        }


        public ServiceResponse GetCaseManagers(int agencyID, long caseManagerID)
        {
            ServiceResponse response = new ServiceResponse();

            if (agencyID > 0)
            {
                List<SearchValueData> searchParam = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "AgencyID", Value = agencyID.ToString()},
                        //new SearchValueData { Name = "IsDeleted", Value = "0"}
                    };

                List<CaseManager> caseManagers = GetEntityList<CaseManager>(searchParam, string.Format("IsDeleted ={0} OR CaseManagerID={1}", 0, caseManagerID), "LastName","ASC");
                if (caseManagers.Any())
                {
                    response.Data = caseManagers;
                    response.IsSuccess = true;
                }
            }
            return response;
        }

        public ServiceResponse GetCaseManagerDetail(int caseManagerID)
        {
            ServiceResponse response = new ServiceResponse();

            if (caseManagerID > 0)
            {

                CaseManager caseManager = GetEntity<CaseManager>(caseManagerID);
                if (caseManager != null && caseManager.CaseManagerID > 0)
                {
                    response.Data = caseManager;
                    response.IsSuccess = true;
                }
            }
            return response;
        }

        public ServiceResponse AddCaseManager(AddCaseManagerModel addCaseManagerModel, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            if (loggedInUserID == 0)
            {
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.UserSessionExpired);
                return response;
            }


            if (addCaseManagerModel != null && addCaseManagerModel.CaseManager != null)
            {
                addCaseManagerModel.IsEditMode = addCaseManagerModel.CaseManager.CaseManagerID > 0;

                if (addCaseManagerModel.IsEditMode)
                {
                    List<SearchValueData> searchCaseManager = new List<SearchValueData>
                        {
                            new SearchValueData { Name = "CaseManagerID" , Value = addCaseManagerModel.CaseManager.CaseManagerID.ToString()},
                             //new SearchValueData { Name = "IsDeleted" , Value = "0"}
                        };
                    CaseManager caseManager = GetEntity<CaseManager>(searchCaseManager);
                    if (caseManager != null && caseManager.CaseManagerID > 0)
                    {
                        caseManager.AgencyID = addCaseManagerModel.CaseManager.AgencyID;
                        caseManager.AgencyLocationID = addCaseManagerModel.CaseManager.AgencyLocationID;
                        caseManager.FirstName = addCaseManagerModel.CaseManager.FirstName;
                        caseManager.LastName = addCaseManagerModel.CaseManager.LastName;
                        caseManager.Email = addCaseManagerModel.CaseManager.Email;
                        caseManager.Extension = addCaseManagerModel.CaseManager.Extension;
                        caseManager.Phone = addCaseManagerModel.CaseManager.Phone;
                        caseManager.Cell = addCaseManagerModel.CaseManager.Cell;
                        caseManager.Fax = addCaseManagerModel.CaseManager.Fax;
                        caseManager.Notes = addCaseManagerModel.CaseManager.Notes;
                        caseManager.CaseWorkerID = addCaseManagerModel.CaseManager.CaseWorkerID;
                        SaveObject(caseManager, loggedInUserID);
                    }
                }
                else
                {
                    SaveObject(addCaseManagerModel.CaseManager, loggedInUserID);
                }
                response.IsSuccess = true;
                response.Message = !addCaseManagerModel.IsEditMode ? string.Format(Resource.RecordCreatedSuccessfully, Resource.CaseManager) : string.Format(Resource.RecordUpdatedSuccessfully, Resource.CaseManager);
            }

            return response;
        }

        #endregion

        #region Case Manager List

        public ServiceResponse SetAddCaseManagerListPage()
        {
            ServiceResponse response = new ServiceResponse();

            SetCaseManagerListPage setCaseManagerListPage = GetMultipleEntity<SetCaseManagerListPage>(StoredProcedure.SetCaseManagerListPage);
            setCaseManagerListPage.DeleteFilter = Common.SetDeleteFilter();
            setCaseManagerListPage.SearchCaseManagerListPage.IsDeleted = 0;
            response.Data = setCaseManagerListPage;
            return response;
        }


        public ServiceResponse GetCaseManagerList(SearchCaseManagerListPage searchCaseManagerListPage, int pageIndex,
                                                  int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchCaseManagerListPage != null)
                SetSearchFilterForCaseMangerList(searchCaseManagerListPage, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListCaseManagerModel> totalData = GetEntityList<ListCaseManagerModel>(StoredProcedure.GetCaseManagerList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListCaseManagerModel> caseManagerList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = caseManagerList;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForCaseMangerList(SearchCaseManagerListPage searchCaseManagerListPage, List<SearchValueData> searchList)
        {
            if (!string.IsNullOrEmpty(searchCaseManagerListPage.Name))
                searchList.Add(new SearchValueData { Name = "Name", Value = Convert.ToString(searchCaseManagerListPage.Name) });

            if (!string.IsNullOrEmpty(searchCaseManagerListPage.Email))
                searchList.Add(new SearchValueData { Name = "Email", Value = Convert.ToString(searchCaseManagerListPage.Email) });

            if (!string.IsNullOrEmpty(searchCaseManagerListPage.Phone))
                searchList.Add(new SearchValueData { Name = "Phone", Value = Convert.ToString(searchCaseManagerListPage.Phone) });
            if (!string.IsNullOrEmpty(searchCaseManagerListPage.CaseWorkerID))
                searchList.Add(new SearchValueData { Name = "CaseWorkerID", Value = Convert.ToString(searchCaseManagerListPage.CaseWorkerID) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchCaseManagerListPage.IsDeleted) });

            if (searchCaseManagerListPage.AgencyID > 0)
                searchList.Add(new SearchValueData
                    {
                        Name = "AgencyID",
                        Value = Convert.ToString(searchCaseManagerListPage.AgencyID)
                    });

            if (searchCaseManagerListPage.AgencyLocationID > 0)
                searchList.Add(new SearchValueData
                    {
                        Name = "AgencyLocationID",
                        Value = Convert.ToString(searchCaseManagerListPage.AgencyLocationID)
                    });
        }


        public ServiceResponse DeleteCaseManager(SearchCaseManagerListPage searchCaseManagerListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForCaseMangerList(searchCaseManagerListPage, searchList);

            if (!string.IsNullOrEmpty(searchCaseManagerListPage.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchCaseManagerListPage.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

            List<ListCaseManagerModel> totalData = GetEntityList<ListCaseManagerModel>(StoredProcedure.DeleteCaseManager, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            //if (count == 0 && totalData != null && totalData.Count > 0)
            //{
            //    response.Message = Common.MessageWithTitle(string.Format(Resource.DeleteFailed, Resource.CaseManager), Resource.CaseManagerReferralExistMessage);
            //    return response;
            //}

            Page<ListCaseManagerModel> caseManagerList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = caseManagerList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.CaseManager);
            return response;
        }

        #endregion
    }
}
