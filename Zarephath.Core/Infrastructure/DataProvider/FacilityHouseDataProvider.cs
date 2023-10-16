using System;
using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class FacilityHouseDataProvider : BaseDataProvider, IFacilityHouseDataProvider
    {
        public ServiceResponse SetAddFacilityHousePage(long facilityId)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "FacilityID", Value = Convert.ToString(facilityId) },
                        new SearchValueData { Name = "ExcludedStatus", Value = Convert.ToString(Convert.ToInt32(ScheduleStatus.ScheduleStatuses.Cancelled) +","+ Convert.ToInt32(ScheduleStatus.ScheduleStatuses.No_Show) +"," + Convert.ToInt32(ScheduleStatus.ScheduleStatuses.No_Confirmation) ) },
                    };

                SetFacilityHouseModel facilityHouseModel = GetMultipleEntity<SetFacilityHouseModel>(StoredProcedure.SetAddFacilityHousePage, searchList);

                if (facilityId > 0)
                {
                    if (facilityHouseModel.FacilityHouseModel != null && facilityHouseModel.FacilityHouseModel.FacilityID > 0)
                    {
                        facilityHouseModel.FacilityHouseModel.SelectedPayors = !string.IsNullOrEmpty(facilityHouseModel.FacilityHouseModel.SetSelectedPayors) ? facilityHouseModel.FacilityHouseModel.SetSelectedPayors.Split(',').ToList() : null;
                        response.Data = facilityHouseModel;
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
                    facilityHouseModel.FacilityHouseModel = new FacilityHouseModel();
                    response.Data = facilityHouseModel;
                }
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse GetParentFacilityHouse(long facilityId)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData> { new SearchValueData { Name = "FacilityID", Value = Convert.ToString(facilityId) }, };
                FacilityHouseModel facilityHouseModel = GetEntity<FacilityHouseModel>(StoredProcedure.GetParentFacilityHouse, searchList);

                if (facilityHouseModel != null)
                {
                    facilityHouseModel.SelectedPayors = !string.IsNullOrEmpty(facilityHouseModel.SetSelectedPayors) ? facilityHouseModel.SetSelectedPayors.Split(',').ToList() : null;
                    response.Data = facilityHouseModel;
                    response.IsSuccess = true;
                }
                else
                {
                    response.ErrorCode = Constants.ErrorCode_NotFound;
                    return response;
                }
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse AddFacility(Facility facility, long loggedInUserId)
        {
            var response = new ServiceResponse();

            if (facility != null)
            {
                #region  Check

                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "FacilityID", Value = facility.FacilityID.ToString() });
                searchList.Add(new SearchValueData
                    {
                        Name = "ParentFacilityID",
                        Value = facility.ParentFacilityID.ToString()
                    });
                searchList.Add(new SearchValueData { Name = "AHCCCSID", Value = Convert.ToString(facility.AHCCCSID) });
                searchList.Add(new SearchValueData { Name = "NPI", Value = Convert.ToString(facility.NPI) });
                searchList.Add(new SearchValueData { Name = "EIN", Value = Convert.ToString(facility.EIN) });

                string message = "";
                Facility ahcccsidmodel = GetEntity<Facility>(StoredProcedure.GetAlreadyExistFacility, searchList) ??
                                         new Facility();
                if (facility.ParentFacilityID == 0)
                {
                    if (ahcccsidmodel.AHCCCSID != null)
                    {
                        if (facility.NPI == ahcccsidmodel.NPI)
                            message = Resource.NPI;
                        if (facility.AHCCCSID == ahcccsidmodel.AHCCCSID)
                            message = string.IsNullOrEmpty(message)
                                          ? Resource.AHCCCSID
                                          : message + ", " + Resource.AHCCCSID;

                        //if (facility.EIN == ahcccsidmodel.EIN)
                        //    message = string.IsNullOrEmpty(message)
                        //                  ? Resource.EIN
                        //                  : message + ", " + Resource.EIN;

                        if (!string.IsNullOrEmpty(message))
                        {
                            response.Message = String.Format(Resource.AlreadyExists, message);
                            return response;
                        }
                    }

                }
                else
                {
                    if (ahcccsidmodel.AHCCCSID != null)
                    {
                        if (facility.NPI != ahcccsidmodel.NPI)
                            message = Resource.NPI;
                        if (facility.AHCCCSID != ahcccsidmodel.AHCCCSID)
                            message = string.IsNullOrEmpty(message)
                                          ? Resource.AHCCCSID
                                          : message + ", " + Resource.AHCCCSID;

                        //if (facility.EIN != ahcccsidmodel.EIN)
                        //    message = string.IsNullOrEmpty(message)
                        //                  ? Resource.EIN
                        //                  : message + ", " + Resource.EIN;

                        if (!string.IsNullOrEmpty(message))
                        {
                            response.Message = String.Format(Resource.ParentDetailsDoesnotMatch, message);
                            return response;
                        }
                    }
                }

                #endregion

                if (facility.ParentFacilityID == 0)
                    facility.FacilityBillingName = facility.FacilityName;

                SaveObject(facility, loggedInUserId);

                response.Message = facility.FacilityID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.FacilityHouse) : string.Format(Resource.RecordCreatedSuccessfully, Resource.FacilityHouse);

                string customWhere = string.Format("(FacilityID={0})", facility.FacilityID);
                List<FacilityApprovedPayor> facilityApprovedPayorList = GetEntityList<FacilityApprovedPayor>(null, customWhere);
                foreach (var facilityApprovedPayor in facilityApprovedPayorList)
                {
                    DeleteEntity<FacilityApprovedPayor>(facilityApprovedPayor.FacilityApprovedPayorID);
                }
                foreach (var payorId in facility.SelectedPayors)
                {
                    FacilityApprovedPayor facilityApprovedPayor = new FacilityApprovedPayor()
                    {
                        FacilityID = facility.FacilityID,
                        PayorID = Convert.ToInt64(payorId)
                    };
                    SaveObject(facilityApprovedPayor, loggedInUserId);
                }
                response.IsSuccess = true;
                response.Data = Crypto.Encrypt(facility.FacilityID.ToString());
            }
            else
                response.Message = Common.MessageWithTitle(string.Format(Resource.CreateFailed, Resource.FacilityHouse), Resource.ExceptionMessage);

            return response;
        }

        public ServiceResponse SetFacilityHousePage()
        {
            var response = new ServiceResponse();

            SetFacilityHouseListModel setEmployeeListPage = GetMultipleEntity<SetFacilityHouseListModel>(StoredProcedure.SetFacilityHouseListPage);
            setEmployeeListPage.DeleteFilter = Common.SetDeleteFilter();
            setEmployeeListPage.SearchFacilityHouseModel.IsDeleted = 0;
            response.Data = setEmployeeListPage;
            return response;
        }

        public ServiceResponse SetFacilityHouseList(SearchFacilityHouseModel searchFacilityHouseModel, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            SetSearchFilterForFacilityHouseListPage(searchFacilityHouseModel, searchList);
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<FacilityHouseModel> totalData = GetEntityList<FacilityHouseModel>(StoredProcedure.GetFacilityHouseList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<FacilityHouseModel> getFacilityHouseList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.IsSuccess = true;
            response.Data = getFacilityHouseList;
            return response;
        }

        public ServiceResponse DeleteFacilityHouse(SearchFacilityHouseModel searchFacilityHouseModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            SetSearchFilterForFacilityHouseListPage(searchFacilityHouseModel, searchList);
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            if (!string.IsNullOrEmpty(searchFacilityHouseModel.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchFacilityHouseModel.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

            List<FacilityHouseModel> totalData = GetEntityList<FacilityHouseModel>(StoredProcedure.DeleteFacilityHouse, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            //if (count == 0 && totalData != null && totalData.Count > 0)
            //{
            //    response.Message = Common.MessageWithTitle(string.Format(Resource.DeleteFailed, Resource.FacilityHouse), Resource.FacilityHouseScheduleExistMessage);
            //    return response;
            //}

            Page<FacilityHouseModel> getFacilityHouseList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getFacilityHouseList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.FacilityHouse);
            return response;
        }

        private static void SetSearchFilterForFacilityHouseListPage(SearchFacilityHouseModel searchFacilityHouseModel, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "FacilityName", Value = searchFacilityHouseModel.FacilityName });
            searchList.Add(new SearchValueData { Name = "County", Value = searchFacilityHouseModel.County });
            searchList.Add(new SearchValueData { Name = "RegionID", Value = Convert.ToString(searchFacilityHouseModel.RegionID) });
            searchList.Add(new SearchValueData { Name = "Phone", Value = Convert.ToString(searchFacilityHouseModel.Phone) });
            searchList.Add(new SearchValueData { Name = "NPI", Value = Convert.ToString(searchFacilityHouseModel.NPI) });
            searchList.Add(new SearchValueData { Name = "AHCCCSID", Value = Convert.ToString(searchFacilityHouseModel.AHCCCSID) });
            searchList.Add(new SearchValueData { Name = "EIN", Value = Convert.ToString(searchFacilityHouseModel.EIN) });
            searchList.Add(new SearchValueData { Name = "PayorApproved", Value = Convert.ToString(searchFacilityHouseModel.PayorApproved) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchFacilityHouseModel.IsDeleted) });
        }

        public ServiceResponse GetFacilityTransportLocationMapping(long facilityID, long transportLocationID)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData{Name = "FacilityID",Value = facilityID.ToString()},
                    new SearchValueData{Name = "TransportLocationID",Value = transportLocationID.ToString()}
                };

            FacilityTransportLocationMapping facilityTransportLocationMapping =
                GetEntity<FacilityTransportLocationMapping>(searchParam);

            if (facilityTransportLocationMapping != null)
            {
                response.IsSuccess = true;
                response.Data = facilityTransportLocationMapping;
            }
            else
            {
                facilityTransportLocationMapping = new FacilityTransportLocationMapping();
                response.Data = facilityTransportLocationMapping;
                response.IsSuccess = false;
            }

            return response;
        }

        public ServiceResponse SaveFacilityTransportLocationMapping(FacilityTransportLocationMapping model)
        {
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "FacilityID", Value = model.FacilityID.ToString(), IsEqual = true},
                    new SearchValueData{Name = "TransportLocationID",Value = model.TransportLocationID.ToString(),IsEqual = true}
                };

            FacilityTransportLocationMapping facilityTransportLocationMapping =
                GetEntity<FacilityTransportLocationMapping>(searchParam);
            if (facilityTransportLocationMapping == null)
            {
                return SaveObject(model, 0, Resource.FacilityTransportLocationMappingSavedSuccessfully);
            }
            facilityTransportLocationMapping.MondayDropOff = model.MondayDropOff;
            facilityTransportLocationMapping.MondayPickUp = model.MondayPickUp;

            facilityTransportLocationMapping.TuesdayDropOff = model.TuesdayDropOff;
            facilityTransportLocationMapping.TuesdayDropOff = model.TuesdayDropOff;

            facilityTransportLocationMapping.WednesdayDropOff = model.WednesdayDropOff;
            facilityTransportLocationMapping.WednesdayPickUp = model.WednesdayPickUp;

            facilityTransportLocationMapping.ThursdayDropOff = model.ThursdayDropOff;
            facilityTransportLocationMapping.ThursdayPickUp = model.ThursdayPickUp;

            facilityTransportLocationMapping.FridayDropOff = model.FridayDropOff;
            facilityTransportLocationMapping.FridayPickUp = model.FridayPickUp;

            facilityTransportLocationMapping.SaturdayDropOff = model.SaturdayDropOff;
            facilityTransportLocationMapping.SaturdayPickUp = model.SaturdayPickUp;

            facilityTransportLocationMapping.SundayDropOff = model.SundayDropOff;
            facilityTransportLocationMapping.SundayPickUp = model.SundayPickUp;

            return SaveObject(facilityTransportLocationMapping, 0, Resource.FacilityTransportLocationMappingSavedSuccessfully);
        }

        #region HomeCare Related Code
        public ServiceResponse HC_SetAddFacilityHousePage(long facilityId)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "FacilityID", Value = Convert.ToString(facilityId) }
                        //,new SearchValueData { Name = "ExcludedStatus", Value = Convert.ToString(Convert.ToInt32(ScheduleStatus.ScheduleStatuses.Cancelled) +","+ Convert.ToInt32(ScheduleStatus.ScheduleStatuses.No_Show) +"," + Convert.ToInt32(ScheduleStatus.ScheduleStatuses.No_Confirmation) ) },
                    };

                HC_SetFacilityHouseModel facilityHouseModel = GetMultipleEntity<HC_SetFacilityHouseModel>(StoredProcedure.HC_SetAddFacilityHousePage, searchList);

                if(facilityHouseModel.FacilityHouseModel == null)
                    facilityHouseModel.FacilityHouseModel = new HC_FacilityHouseModel();

                response.Data = facilityHouseModel;

                //if (facilityId > 0)
                //{
                //    if (facilityHouseModel.FacilityHouseModel != null && facilityHouseModel.FacilityHouseModel.FacilityID > 0)
                //    {
                //        facilityHouseModel.FacilityHouseModel.SelectedPayors = !string.IsNullOrEmpty(facilityHouseModel.FacilityHouseModel.SetSelectedPayors) ? facilityHouseModel.FacilityHouseModel.SetSelectedPayors.Split(',').ToList() : null;
                //        response.Data = facilityHouseModel;
                //        response.IsSuccess = true;
                //    }
                //    else
                //    {
                //        response.ErrorCode = Constants.ErrorCode_NotFound;
                //        return response;
                //    }
                //}
                //else
                //{
                //    facilityHouseModel.FacilityHouseModel = new FacilityHouseModel();
                //    response.Data = facilityHouseModel;
                //}
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_AddFacility(HC_AddFacilityHouseModel facilityModel, long loggedInUserId)
        {
            var response = new ServiceResponse();
            HC_Facility facility = facilityModel.Facility;
            bool EditMode = facility.FacilityID > 0;
            if (facility != null)
            {
                #region  Check

                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "FacilityID", Value = facility.FacilityID.ToString() });
                //searchList.Add(new SearchValueData { Name = "ParentFacilityID", Value = facility.ParentFacilityID.ToString() });
                searchList.Add(new SearchValueData { Name = "AHCCCSID", Value = Convert.ToString(facility.AHCCCSID) });
                searchList.Add(new SearchValueData { Name = "NPI", Value = Convert.ToString(facility.NPI) });
                searchList.Add(new SearchValueData { Name = "EIN", Value = Convert.ToString(facility.EIN) });

                string message = "";
                Facility temp_facility = GetEntity<Facility>(StoredProcedure.HC_GetAlreadyExistFacility, searchList);

                if(temp_facility != null)
                {
                    if (temp_facility.AHCCCSID != null)
                    {
                        if (facility.AHCCCSID.Equals(temp_facility.AHCCCSID))
                            message = Resource.AHCCCSID;
                    }
                    if (temp_facility.NPI != null)
                    {
                        if (facility.NPI.Equals(temp_facility.NPI))
                            message = string.IsNullOrEmpty(message)
                                          ? Resource.NPI
                                          : message + ", " + Resource.NPI;
                    }
                    if (!string.IsNullOrEmpty(message))
                    {
                        response.Message = String.Format(Resource.AlreadyExists, message);
                        return response;
                    }
                }
                #endregion

                //if (facility.ParentFacilityID == 0)
                //    facility.FacilityBillingName = facility.FacilityName;

                SaveObject(facility, loggedInUserId);

                try
                {
                    string systemId = System.Web.HttpContext.Current.Request.UserHostAddress;
                    searchList = new List<SearchValueData>();
                    searchList.Add(new SearchValueData { Name = "FacilityID", Value = facility.FacilityID.ToString() });
                    searchList.Add(new SearchValueData { Name = "LoggedInUserId", Value = Convert.ToString(loggedInUserId) });
                    searchList.Add(new SearchValueData { Name = "SystemID", Value = systemId });
                    searchList.Add(new SearchValueData { Name = "EquipmentIDs", Value = facilityModel.EquipmentList != null && facilityModel.EquipmentList.Count() > 0 ? string.Join(",", facilityModel.EquipmentList.Select(m => m.EquipmentID)) : "" });
                    FacilityHouseEquipmentModel facilityEquipment = GetEntity<FacilityHouseEquipmentModel>(StoredProcedure.SaveFacilityEquipment, searchList);
                }
                catch (Exception ex)
                {
                    
                }               

                response.Message = EditMode ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.FacilityHouse) : string.Format(Resource.RecordCreatedSuccessfully, Resource.FacilityHouse);
                response.IsSuccess = true;
                return response;

                //string customWhere = string.Format("(FacilityID={0})", facility.FacilityID);
                //List<FacilityApprovedPayor> facilityApprovedPayorList = GetEntityList<FacilityApprovedPayor>(null, customWhere);
                //foreach (var facilityApprovedPayor in facilityApprovedPayorList)
                //{
                //    DeleteEntity<FacilityApprovedPayor>(facilityApprovedPayor.FacilityApprovedPayorID);
                //}
                //foreach (var payorId in facility.SelectedPayors)
                //{
                //    FacilityApprovedPayor facilityApprovedPayor = new FacilityApprovedPayor()
                //    {
                //        FacilityID = facility.FacilityID,
                //        PayorID = Convert.ToInt64(payorId)
                //    };
                //    SaveObject(facilityApprovedPayor, loggedInUserId);
                //}
                //response.IsSuccess = true;
                //response.Data = Crypto.Encrypt(facility.FacilityID.ToString());
            }
            else
                response.Message = Common.MessageWithTitle(string.Format(Resource.CreateFailed, Resource.FacilityHouse), Resource.ExceptionMessage);

            return response;
        }

        public List<FacilityHouseEquipmentModel> GetEquipment(int pageSize, string ignoreIds, string searchText = null)
        {
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "SearchText", Value = searchText},
                    new SearchValueData {Name = "PageSize", Value =pageSize.ToString()},
                    new SearchValueData {Name = "IgnoreIDs", Value = ignoreIds ?? "" },
                    new SearchValueData {Name = "ItemTypeID", Value = Convert.ToString(Common.DDType.SubSection.GetHashCode())}
                };

            List<FacilityHouseEquipmentModel> model = GetEntityList<FacilityHouseEquipmentModel>(StoredProcedure.GetEquipment, searchParam) ?? new List<FacilityHouseEquipmentModel>();

            //if (model.Count == 0 || model.Count(c => c.ItemTypeName == searchText) == 0)
            //    model.Insert(0, new ServicePlanComponentModel { ItemTypeName = searchText });

            return model;
        }

        public ServiceResponse HC_SetFacilityHouseListPage()
        {
            var response = new ServiceResponse();
            //SetFacilityHouseListModel setEmployeeListPage = GetMultipleEntity<SetFacilityHouseListModel>(StoredProcedure.SetFacilityHouseListPage);
            HC_SetFacilityHouseListModel setEmployeeListPage = new HC_SetFacilityHouseListModel();
            setEmployeeListPage.SearchFacilityHouseModel = new SearchFacilityHouseModel{ IsDeleted=0 };
            setEmployeeListPage.DeleteFilter = Common.SetDeleteFilter();
            response.Data = setEmployeeListPage;
            return response;
        }

        public ServiceResponse HC_GetFacilityHouseList(SearchFacilityHouseModel searchFacilityHouseModel, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            HC_SetSearchFilterForFacilityHouseListPage(searchFacilityHouseModel, searchList);
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<FacilityHouseModel> totalData = GetEntityList<FacilityHouseModel>(StoredProcedure.HC_GetFacilityHouseList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<FacilityHouseModel> getFacilityHouseList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.IsSuccess = true;
            response.Data = getFacilityHouseList;
            return response;
        }

        public ServiceResponse HC_DeleteFacilityHouse(SearchFacilityHouseModel searchFacilityHouseModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            HC_SetSearchFilterForFacilityHouseListPage(searchFacilityHouseModel, searchList);
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            if (!string.IsNullOrEmpty(searchFacilityHouseModel.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchFacilityHouseModel.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

            List<FacilityHouseModel> totalData = GetEntityList<FacilityHouseModel>(StoredProcedure.HC_DeleteFacilityHouse, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            //if (count == 0 && totalData != null && totalData.Count > 0)
            //{
            //    response.Message = Common.MessageWithTitle(string.Format(Resource.DeleteFailed, Resource.FacilityHouse), Resource.FacilityHouseScheduleExistMessage);
            //    return response;
            //}

            Page<FacilityHouseModel> getFacilityHouseList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getFacilityHouseList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.FacilityHouse);
            return response;
        }

        private static void HC_SetSearchFilterForFacilityHouseListPage(SearchFacilityHouseModel searchFacilityHouseModel, List<SearchValueData> searchList)
        {
            if(searchFacilityHouseModel.AgencyID.HasValue && searchFacilityHouseModel.AgencyID.Value != 0)
                searchList.Add(new SearchValueData { Name = "AgencyID", Value = Convert.ToString(searchFacilityHouseModel.AgencyID.Value) });

            searchList.Add(new SearchValueData { Name = "FacilityName", Value = searchFacilityHouseModel.FacilityName });
            searchList.Add(new SearchValueData { Name = "County", Value = searchFacilityHouseModel.County });
            searchList.Add(new SearchValueData { Name = "RegionID", Value = Convert.ToString(searchFacilityHouseModel.RegionID) });
            searchList.Add(new SearchValueData { Name = "Phone", Value = Convert.ToString(searchFacilityHouseModel.Phone) });
            searchList.Add(new SearchValueData { Name = "NPI", Value = Convert.ToString(searchFacilityHouseModel.NPI) });
            searchList.Add(new SearchValueData { Name = "AHCCCSID", Value = Convert.ToString(searchFacilityHouseModel.AHCCCSID) });
            searchList.Add(new SearchValueData { Name = "EIN", Value = Convert.ToString(searchFacilityHouseModel.EIN) });
            searchList.Add(new SearchValueData { Name = "PayorApproved", Value = Convert.ToString(searchFacilityHouseModel.PayorApproved) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchFacilityHouseModel.IsDeleted) });
        }

        #endregion
    }
}
