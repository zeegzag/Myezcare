using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class AgencyDataProvider : BaseDataProvider, IAgencyDataProvider
    {
        #region Add Agency

        public ServiceResponse SetAddAgencyPage(long agencyId)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchAgencyParam = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "AgencyId", Value = agencyId.ToString()},
                      };

            AddAgencyModel addAgencyModel = GetMultipleEntity<AddAgencyModel>(StoredProcedure.SetAddAgencyPage, searchAgencyParam);

            if (addAgencyModel.Agency == null)
            {
                addAgencyModel.Agency = new Agency();
            }
            if (agencyId > 0 && addAgencyModel.Agency == null)
            {
                response.ErrorCode = Constants.ErrorCode_NotFound;
                response.IsSuccess = false;
                return response;
            }
            response.Data = addAgencyModel;

            return response;
        }

        public ServiceResponse AddAgency(AddAgencyModel addAgencyModel, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            if (addAgencyModel != null)
            {
                bool isEditMode = addAgencyModel.Agency.AgencyID > 0;

                if (isEditMode)
                {
                    List<SearchValueData> searchAgency = new List<SearchValueData>
                        {
                            new SearchValueData { Name = "AgencyID" , Value = addAgencyModel.Agency.AgencyID.ToString()},
                        };

                    Agency agency = GetEntity<Agency>(searchAgency);

                    if (agency != null && agency.AgencyID > 0)
                    {
                        agency.AgencyID = addAgencyModel.Agency.AgencyID;
                        agency.NickName = addAgencyModel.Agency.NickName;
                        agency.Phone = addAgencyModel.Agency.Phone;
                        agency.ShortName = addAgencyModel.Agency.ShortName;
                        agency.RegionID = addAgencyModel.Agency.RegionID;
                        agency.ContactName = addAgencyModel.Agency.ContactName;
                        agency.Email = addAgencyModel.Agency.Email;
                        agency.Fax = addAgencyModel.Agency.Fax;
                        agency.Address = addAgencyModel.Agency.Address;
                        agency.City = addAgencyModel.Agency.City;
                        agency.StateCode = addAgencyModel.Agency.StateCode;
                        agency.ZipCode = addAgencyModel.Agency.ZipCode;
                        SaveObject(agency, loggedInUserID);
                    }
                }
                else
                {
                    SaveObject(addAgencyModel.Agency, loggedInUserID);
                }
                response.IsSuccess = true;
                response.Message = !isEditMode ? string.Format(Resource.RecordCreatedSuccessfully, Resource.Agency) : string.Format(Resource.RecordUpdatedSuccessfully, Resource.Agency);
            }

            return response;
        }

        #endregion

        #region Agency List

        public ServiceResponse SetAddAgencyListPage()
        {
            ServiceResponse response = new ServiceResponse();

            SetAgencyListPage setAgencyListPage = GetMultipleEntity<SetAgencyListPage>(StoredProcedure.SetAgencyListPage);

            setAgencyListPage.DeleteFilter = Common.SetDeleteFilter();
            setAgencyListPage.SearchAgencyListPage.IsDeleted = 0;
            response.Data = setAgencyListPage;
            return response;
        }

        public ServiceResponse GetAgencyList(SearchAgencyListPage searchAgencyListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchAgencyListPage != null)
                SetSearchFilterForAgencyList(searchAgencyListPage, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListAgencyModel> totalData = GetEntityList<ListAgencyModel>(StoredProcedure.GetAgencyList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListAgencyModel> listAgencyModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = listAgencyModel;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForAgencyList(SearchAgencyListPage searchAgencyListPage, List<SearchValueData> searchList)
        {
            if (!string.IsNullOrEmpty(searchAgencyListPage.AgencyType))
                searchList.Add(new SearchValueData { Name = "AgencyType", Value = Convert.ToString(searchAgencyListPage.AgencyType) });

            if (!string.IsNullOrEmpty(searchAgencyListPage.NickName))
                searchList.Add(new SearchValueData { Name = "NickName", Value = Convert.ToString(searchAgencyListPage.NickName) });

            if (!string.IsNullOrEmpty(searchAgencyListPage.Email))
                searchList.Add(new SearchValueData { Name = "Email", Value = Convert.ToString(searchAgencyListPage.Email) });

            if (!string.IsNullOrEmpty(searchAgencyListPage.Phone))
                searchList.Add(new SearchValueData { Name = "Phone", Value = Convert.ToString(searchAgencyListPage.Phone) });

            if (!string.IsNullOrEmpty(searchAgencyListPage.ShortName))
                searchList.Add(new SearchValueData { Name = "ShortName", Value = Convert.ToString(searchAgencyListPage.ShortName) });

            if (!string.IsNullOrEmpty(searchAgencyListPage.ContactName))
                searchList.Add(new SearchValueData { Name = "ContactName", Value = Convert.ToString(searchAgencyListPage.ContactName) });

            if (!string.IsNullOrEmpty(searchAgencyListPage.Address))
                searchList.Add(new SearchValueData { Name = "Address", Value = Convert.ToString(searchAgencyListPage.Address) });

            if (!string.IsNullOrEmpty(searchAgencyListPage.TIN))
                searchList.Add(new SearchValueData { Name = "TIN", Value = Convert.ToString(searchAgencyListPage.TIN) });

            if (!string.IsNullOrEmpty(searchAgencyListPage.EIN))
                searchList.Add(new SearchValueData { Name = "EIN", Value = Convert.ToString(searchAgencyListPage.EIN) });

            if (!string.IsNullOrEmpty(searchAgencyListPage.Mobile))
                searchList.Add(new SearchValueData { Name = "Mobile", Value = Convert.ToString(searchAgencyListPage.Mobile) });

            if (searchAgencyListPage.RegionID > 0)
                searchList.Add(new SearchValueData { Name = "RegionID", Value = Convert.ToString(searchAgencyListPage.RegionID) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchAgencyListPage.IsDeleted) });

        }

        public ServiceResponse DeleteAgency(SearchAgencyListPage searchAgencyListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            if (searchAgencyListPage != null)
                SetSearchFilterForAgencyList(searchAgencyListPage, searchList);

            if (searchAgencyListPage != null && !string.IsNullOrEmpty(searchAgencyListPage.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchAgencyListPage.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

            List<ListAgencyModel> totalData = GetEntityList<ListAgencyModel>(StoredProcedure.DeleteAgency, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListAgencyModel> listAgencyModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = listAgencyModel;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Agency);
            return response;
        }

        #endregion

        #region Home Care

        #region Add Agency

        public ServiceResponse HC_SetAddAgencyPage(long agencyId)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchAgencyParam = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "AgencyId", Value = agencyId.ToString()},
                      };

            AddAgencyModel addAgencyModel = GetMultipleEntity<AddAgencyModel>(StoredProcedure.HC_SetAddAgencyPage, searchAgencyParam);

            if (addAgencyModel.Agency == null)
            {
                addAgencyModel.Agency = new Agency();
            }
            if (agencyId > 0 && addAgencyModel.Agency == null)
            {
                response.ErrorCode = Constants.ErrorCode_NotFound;
                response.IsSuccess = false;
                return response;
            }
            response.Data = addAgencyModel;

            return response;
        }

        public ServiceResponse HC_AddAgency(AddAgencyModel addAgencyModel, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            if (addAgencyModel != null)
            {
                bool isEditMode = addAgencyModel.Agency.AgencyID > 0;

                addAgencyModel.Agency.AgencyType = !SessionHelper.IsCaseManagement ? Common.AgencyTypeEnum.ReferralSource.ToString() : addAgencyModel.Agency.AgencyType;

                if (isEditMode)
                {
                    List<SearchValueData> searchAgency = new List<SearchValueData>
                        {
                            new SearchValueData { Name = "AgencyID" , Value = addAgencyModel.Agency.AgencyID.ToString()},
                        };

                    Agency agency = GetEntity<Agency>(searchAgency);

                    if (agency != null && agency.AgencyID > 0)
                    {
                        agency.AgencyID = addAgencyModel.Agency.AgencyID;
                        agency.AgencyType = addAgencyModel.Agency.AgencyType;
                        agency.NickName = addAgencyModel.Agency.NickName;
                        agency.Phone = addAgencyModel.Agency.Phone;
                        agency.ShortName = addAgencyModel.Agency.ShortName;
                        agency.RegionID = addAgencyModel.Agency.RegionID;
                        agency.TIN = addAgencyModel.Agency.TIN;
                        agency.EIN = addAgencyModel.Agency.EIN;
                        agency.Mobile = addAgencyModel.Agency.Mobile;
                        agency.ContactName = addAgencyModel.Agency.ContactName;
                        agency.Email = addAgencyModel.Agency.Email;
                        agency.Fax = addAgencyModel.Agency.Fax;
                        agency.Address = addAgencyModel.Agency.Address;
                        agency.City = addAgencyModel.Agency.City;
                        agency.StateCode = addAgencyModel.Agency.StateCode;
                        agency.ZipCode = addAgencyModel.Agency.ZipCode;
                        agency.NPI = addAgencyModel.Agency.NPI;
                        response = SaveObject(agency, loggedInUserID, string.Format(Resource.RecordUpdatedSuccessfully, Resource.Agency));
                    }
                    else
                    {
                        response.Message = Resource.RecordNotFound; 
                        response.IsSuccess = false;
                        return response;
                    }
                }
                else
                {
                    string NPI = !string.IsNullOrEmpty(addAgencyModel.Agency.NPI) ? Convert.ToString(addAgencyModel.Agency.NPI) : "111111111111111"; 

                    List <SearchValueData> searchAgency = new List<SearchValueData>
                        {
                           new SearchValueData { Name = "NPI" , Value = Convert.ToString(NPI) },
                           new SearchValueData { Name = "IsDeleted" , Value = "0"}
                        };
                    Agency agency = GetEntity<Agency>(searchAgency);
                    if (agency != null && agency.AgencyID > 0)
                    {
                        response.Message = string.Format(Resource.NPIAlreadyExists, Resource.Agency);
                        response.IsSuccess = false;
                        return response;
                    }
                    string message = string.Format(Resource.RecordCreatedSuccessfully, Resource.Agency);
                    response = SaveObject(addAgencyModel.Agency, loggedInUserID, message);
                    if (response.IsSuccess && addAgencyModel.AgencyTaxonomies.Count > 0)
                    {
                        string addOnMessage= string.Empty;
                        response = HC_AddAgencyTaxonomies(addAgencyModel, loggedInUserID);
                        if(!response.IsSuccess) { addOnMessage = string.Format("w/ {0}, {1}", Resource.Error, response.Message);  }
                        response.Message = string.Format("{0}{1}", message, addOnMessage);
                    }
                }
                //response.IsSuccess = true;
            }

            return response;
        }

        private ServiceResponse HC_AddAgencyTaxonomies(AddAgencyModel agency, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            foreach (var taxonomy in agency.AgencyTaxonomies)
            {
                taxonomy.AgencyID = agency.Agency.AgencyID;
                response = SaveObject(taxonomy, loggedInUserID);
            }
            return response;
        }

        #endregion

        #region Agency List

        public ServiceResponse HC_SetAddAgencyListPage()
        {
            ServiceResponse response = new ServiceResponse();

            SetAgencyListPage setAgencyListPage = GetMultipleEntity<SetAgencyListPage>(StoredProcedure.HC_SetAgencyListPage);

            setAgencyListPage.DeleteFilter = Common.SetDeleteFilter();
            setAgencyListPage.SearchAgencyListPage.IsDeleted = 0;
            response.Data = setAgencyListPage;
            return response;
        }

        public ServiceResponse HC_GetAgencyList(SearchAgencyListPage searchAgencyListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchAgencyListPage != null)
                SetSearchFilterForAgencyList(searchAgencyListPage, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListAgencyModel> totalData = GetEntityList<ListAgencyModel>(StoredProcedure.HC_GetAgencyList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListAgencyModel> listAgencyModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = listAgencyModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_DeleteAgency(SearchAgencyListPage searchAgencyListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            if (searchAgencyListPage != null)
                SetSearchFilterForAgencyList(searchAgencyListPage, searchList);

            if (searchAgencyListPage != null && !string.IsNullOrEmpty(searchAgencyListPage.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchAgencyListPage.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

            List<ListAgencyModel> totalData = GetEntityList<ListAgencyModel>(StoredProcedure.HC_DeleteAgency, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListAgencyModel> listAgencyModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = listAgencyModel;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Agency);
            return response;
        }

        #endregion

        #endregion

        public List<ZipCodes> GetZipCodeList(string searchText, string state, int pageSize)
        {
            ServiceResponse response = new ServiceResponse();
            string customWhere = string.Format("(ZipCode like '%{0}%' or StateCode like '%{0}%' or City like '%{0}%')", searchText);

            Page<ZipCodes> page = GetEntityPageList<ZipCodes>(new List<SearchValueData> { }, pageSize, 1, "", "", customWhere);
            long zipcode;

            if (searchText.Length == 5 && long.TryParse(searchText, out zipcode))
            {
                page.Items.Add(new ZipCodes
                {
                    ZipCode = searchText,
                    City = "",
                    County = "",
                    StateCode = ""
                });
            }
            return page.Items;
        }
    }
}
