using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;
using RestSharp.Extensions;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class TransportServiceDataProvider : BaseDataProvider, ITransportServiceDataProvider
    {
        #region Transport Service
        public ServiceResponse HC_SetAddTransportContactPage(long contactId)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "ContactID", Value = Convert.ToString(contactId) }
                    };

                SetTransportContactModel transportContactModel = GetMultipleEntityAdmin<SetTransportContactModel>(StoredProcedure.SetAddTransportContactPage, searchList);

                if (transportContactModel.TransportContactModel == null)
                    transportContactModel.TransportContactModel = new TransportContactModel();
                response.Data = transportContactModel;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_AddTransportContact(SetTransportContactModel transportContactModel)
        {
            var response = new ServiceResponse();
            try
            {
                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "ContactID", Value = Convert.ToString(transportContactModel.TransportContactModel.ContactID) });
                dataList.Add(new SearchValueData { Name = "FirstName", Value = Convert.ToString(transportContactModel.TransportContactModel.FirstName) });
                dataList.Add(new SearchValueData { Name = "LastName", Value = Convert.ToString(transportContactModel.TransportContactModel.LastName) });
                dataList.Add(new SearchValueData { Name = "Email", Value = Convert.ToString(transportContactModel.TransportContactModel.Email) });
                dataList.Add(new SearchValueData { Name = "Phone", Value = Convert.ToString(transportContactModel.TransportContactModel.Phone) });
                dataList.Add(new SearchValueData { Name = "MobileNumber", Value = Convert.ToString(transportContactModel.TransportContactModel.MobileNumber) });
                dataList.Add(new SearchValueData { Name = "Fax", Value = Convert.ToString(transportContactModel.TransportContactModel.Fax) });
                dataList.Add(new SearchValueData { Name = "ApartmentNo", Value = Convert.ToString(transportContactModel.TransportContactModel.ApartmentNo) });
                dataList.Add(new SearchValueData { Name = "Address", Value = Convert.ToString(transportContactModel.TransportContactModel.Address) });
                dataList.Add(new SearchValueData { Name = "City", Value = Convert.ToString(transportContactModel.TransportContactModel.City) });
                dataList.Add(new SearchValueData { Name = "State", Value = Convert.ToString(transportContactModel.TransportContactModel.State) });
                dataList.Add(new SearchValueData { Name = "ZipCode", Value = Convert.ToString(transportContactModel.TransportContactModel.ZipCode) });
                dataList.Add(new SearchValueData { Name = "ContactType", Value = Convert.ToString(transportContactModel.TransportContactModel.ContactType) });
                dataList.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(transportContactModel.TransportContactModel.OrganizationID) });
                dataList.Add(new SearchValueData { Name = "loggedInUserID", Value = SessionHelper.LoggedInID.ToString() });
                dataList.Add(new SearchValueData { Name = "SystemID ", Value = HttpContext.Current.Request.UserHostAddress });
                int data = (int)GetScalarAdmin(StoredProcedure.TransportContactAddUpdate, dataList);
                if (data == 1)
                {
                    response.IsSuccess = true;
                    response.Message = transportContactModel.TransportContactModel.ContactID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.TransportService) :
                        string.Format(Resource.RecordCreatedSuccessfully, Resource.TransportService);
                    return response;
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse HC_SetTransportContactListPage()
        {
            var response = new ServiceResponse();
            HC_SetTransportContactListModel setTransportContactListPage = GetMultipleEntityAdmin<HC_SetTransportContactListModel>(StoredProcedure.HC_SetTransportContactListPage);

            setTransportContactListPage.SearchTransportContactModel = new SearchTransportContactModel { IsDeleted = 0 };
            setTransportContactListPage.DeleteFilter = Common.SetDeleteFilter();
            response.Data = setTransportContactListPage;
            return response;
        }

        public ServiceResponse HC_GetTransportContactList(SearchTransportContactModel searchTransportContactModel, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            HC_SetSearchFilterForTransportContactListPage(searchTransportContactModel, searchList);
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<TransportContactModel> totalData = GetEntityListAdmin<TransportContactModel>(StoredProcedure.HC_GetTransportContactList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<TransportContactModel> getTransportContactList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.IsSuccess = true;
            response.Data = getTransportContactList;
            return response;
        }

        private static void HC_SetSearchFilterForTransportContactListPage(SearchTransportContactModel searchTransportContactModel, List<SearchValueData> searchList)
        {
            if (searchTransportContactModel.ContactID.HasValue && searchTransportContactModel.ContactID.Value != 0)
                searchList.Add(new SearchValueData { Name = "ContactID", Value = Convert.ToString(searchTransportContactModel.ContactID.Value) });
            searchList.Add(new SearchValueData { Name = "FirstName", Value = Convert.ToString(searchTransportContactModel.FirstName) });
            searchList.Add(new SearchValueData { Name = "Email", Value = Convert.ToString(searchTransportContactModel.Email) });
            searchList.Add(new SearchValueData { Name = "MobileNumber", Value = Convert.ToString(searchTransportContactModel.MobileNumber) });
            searchList.Add(new SearchValueData { Name = "Address", Value = Convert.ToString(searchTransportContactModel.Address) });
            searchList.Add(new SearchValueData { Name = "ContactType", Value = Convert.ToString(searchTransportContactModel.ContactType) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchTransportContactModel.IsDeleted) });
        }

        public ServiceResponse HC_DeleteTransportContact(SearchTransportContactModel searchTransportContactModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            HC_SetSearchFilterForTransportContactListPage(searchTransportContactModel, searchList);
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            if (!string.IsNullOrEmpty(searchTransportContactModel.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchTransportContactModel.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

            List<TransportContactModel> totalData = GetEntityListAdmin<TransportContactModel>(StoredProcedure.HC_DeleteTransportContact, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<TransportContactModel> getTransportContactList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getTransportContactList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.TransportService);
            return response;
        }
        public List<GetSearchOrganizationNameModel> GetSearchOrganizationName(int pageSize, string searchText = null)
        {
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "SearchText", Value = searchText},
                    new SearchValueData {Name = "PageSize", Value =pageSize.ToString()}
                };

            List<GetSearchOrganizationNameModel> model = GetEntityListAdmin<GetSearchOrganizationNameModel>(StoredProcedure.GetSearchOrganizationName, searchParam) ?? new List<GetSearchOrganizationNameModel>();

            if (model.Count == 0 || model.Count(c => c.OrganizationID.ToLower() == searchText.ToLower()) == 0)
            {
                model.Insert(0, new GetSearchOrganizationNameModel { OrganizationID = searchText });
            }

            return model;
        }
        #endregion

        #region Vehicle
        public ServiceResponse HC_SetAddVehiclePage(long vehicleId)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "VehicleID", Value = Convert.ToString(vehicleId) }
                    };

                SetVehicleModel vehicleModel = GetMultipleEntityAdmin<SetVehicleModel>(StoredProcedure.SetAddVehiclePage, searchList);

                if (vehicleModel.VehicleModel == null)
                    vehicleModel.VehicleModel = new VehicleModel();
                response.Data = vehicleModel;

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_AddVehicle(SetVehicleModel vehicleModel)
        {
            var response = new ServiceResponse();
            try
            {
                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "VehicleID", Value = Convert.ToString(vehicleModel.VehicleModel.VehicleID) });
                dataList.Add(new SearchValueData { Name = "VIN_Number", Value = Convert.ToString(vehicleModel.VehicleModel.VIN_Number) });
                dataList.Add(new SearchValueData { Name = "SeatingCapacity", Value = Convert.ToString(vehicleModel.VehicleModel.SeatingCapacity) });
                dataList.Add(new SearchValueData { Name = "VehicleType", Value = Convert.ToString(vehicleModel.VehicleModel.VehicleType) });
                dataList.Add(new SearchValueData { Name = "BrandName", Value = Convert.ToString(vehicleModel.VehicleModel.BrandName) });
                dataList.Add(new SearchValueData { Name = "Model", Value = Convert.ToString(vehicleModel.VehicleModel.Model) });
                dataList.Add(new SearchValueData { Name = "Color", Value = Convert.ToString(vehicleModel.VehicleModel.Color) });
                dataList.Add(new SearchValueData { Name = "Attendent", Value = Convert.ToString(vehicleModel.VehicleModel.Attendent) });
                dataList.Add(new SearchValueData { Name = "ContactID", Value = Convert.ToString(vehicleModel.VehicleModel.ContactID) });
                dataList.Add(new SearchValueData { Name = "loggedInUserID", Value = SessionHelper.LoggedInID.ToString() });
                dataList.Add(new SearchValueData { Name = "SystemID ", Value = HttpContext.Current.Request.UserHostAddress });
                dataList.Add(new SearchValueData { Name = "note ", Value = vehicleModel.VehicleModel.note });
                dataList.Add(new SearchValueData { Name = "EmployeeID ", Value = Convert.ToString(vehicleModel.VehicleModel.EmployeeID) });
                int data = (int)GetScalarAdmin(StoredProcedure.VehicleAddUpdate, dataList);
                if (data == 1)
                {
                    response.IsSuccess = true;
                    response.Message = vehicleModel.VehicleModel.VehicleID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.TransportService) :
                        string.Format(Resource.RecordCreatedSuccessfully, Resource.TransportService);
                    return response;
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse HC_SetVehicleListPage()
        {
            var response = new ServiceResponse();
            HC_SetVehicleListModel setVehicleListPage = GetMultipleEntityAdmin<HC_SetVehicleListModel>(StoredProcedure.HC_SetVehicleListPage);

            setVehicleListPage.SearchVehicleModel = new SearchVehicleModel { IsDeleted = 0 };
            setVehicleListPage.DeleteFilter = Common.SetDeleteFilter();
            response.Data = setVehicleListPage;
            return response;
        }

        public ServiceResponse HC_GetVehicleList(SearchVehicleModel searchVehicleModel, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            HC_SetSearchFilterForVehicleListPage(searchVehicleModel, searchList);
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<VehicleModel> totalData = GetEntityListAdmin<VehicleModel>(StoredProcedure.HC_GetVehicleList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<VehicleModel> getVehicleList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.IsSuccess = true;
            response.Data = getVehicleList;
            return response;
        }

        private static void HC_SetSearchFilterForVehicleListPage(SearchVehicleModel searchVehicleModel, List<SearchValueData> searchList)
        {
            if (searchVehicleModel.VehicleID.HasValue && searchVehicleModel.VehicleID.Value != 0)
                searchList.Add(new SearchValueData { Name = "VehicleID", Value = Convert.ToString(searchVehicleModel.VehicleID.Value) });
            searchList.Add(new SearchValueData { Name = "VIN_Number", Value = Convert.ToString(searchVehicleModel.VIN_Number) });
            searchList.Add(new SearchValueData { Name = "ContactID", Value = Convert.ToString(searchVehicleModel.ContactID) });
            searchList.Add(new SearchValueData { Name = "TransportService", Value = Convert.ToString(searchVehicleModel.TransportService) });
            searchList.Add(new SearchValueData { Name = "Attendent", Value = Convert.ToString(searchVehicleModel.Attendent) });
            searchList.Add(new SearchValueData { Name = "Model", Value = Convert.ToString(searchVehicleModel.Model) });
            searchList.Add(new SearchValueData { Name = "BrandName", Value = Convert.ToString(searchVehicleModel.BrandName) });
            searchList.Add(new SearchValueData { Name = "Color", Value = Convert.ToString(searchVehicleModel.Color) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchVehicleModel.IsDeleted) });
        }

        public ServiceResponse HC_DeleteVehicle(SearchVehicleModel searchVehicleModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            HC_SetSearchFilterForVehicleListPage(searchVehicleModel, searchList);
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            if (!string.IsNullOrEmpty(searchVehicleModel.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchVehicleModel.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

            List<VehicleModel> totalData = GetEntityListAdmin<VehicleModel>(StoredProcedure.HC_DeleteVehicle, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<VehicleModel> getVehicleList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getVehicleList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.TransportService);
            return response;
        }
        #endregion

        public ServiceResponse HC_SetTransportAssignmentPage(long transportId)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "TransportID", Value = Convert.ToString(transportId) }
                    };

                SetTransportAssignmentModel transportassignmentModel = GetMultipleEntityAdmin<SetTransportAssignmentModel>(StoredProcedure.HC_SetAddTransportAssignment, searchList);

                if (transportassignmentModel.TransportAssignmentModel == null)
                    transportassignmentModel.TransportAssignmentModel = new TransportAssignmentModel();
                response.Data = transportassignmentModel;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }
        public ServiceResponse SaveTransportAssignment(TransportAssignmentModel transportAssignmentModel)
        {
            var response = new ServiceResponse();
            try
            {
                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "TransportID", Value = Convert.ToString(transportAssignmentModel.TransportID) });
                dataList.Add(new SearchValueData { Name = "FacilityID", Value = Convert.ToString(transportAssignmentModel.FacilityID) });
                dataList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(transportAssignmentModel.StartDate) });
                dataList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(transportAssignmentModel.EndDate) });
                dataList.Add(new SearchValueData { Name = "Attendent", Value = Convert.ToString(transportAssignmentModel.Attendent) });
                dataList.Add(new SearchValueData { Name = "VehicleID", Value = Convert.ToString(transportAssignmentModel.VehicleID) });
                dataList.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(transportAssignmentModel.OrganizationID) });
                dataList.Add(new SearchValueData { Name = "RouteCode", Value = Convert.ToString(transportAssignmentModel.RouteCode) });

                dataList.Add(new SearchValueData { Name = "loggedInUserID", Value = SessionHelper.LoggedInID.ToString() });
                dataList.Add(new SearchValueData { Name = "SystemID ", Value = HttpContext.Current.Request.UserHostAddress });
                int data = (int)GetScalar(StoredProcedure.SaveTransportAssignment, dataList);
                if (data == 1)
                {
                    response.IsSuccess = true;
                    response.Message = transportAssignmentModel.TransportID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.TransportService) :
                        string.Format(Resource.RecordCreatedSuccessfully, Resource.TransportService);
                    return response;
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }
        public ServiceResponse GetTransportAssignmentList(SearchTransportAssignmentModel searchTransportAssignmentModel, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            SearchTransportAssignmentListPage(searchTransportAssignmentModel, searchList);
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<TransportAssignmentList> totalData = GetEntityList<TransportAssignmentList>(StoredProcedure.TransportAssignmentList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<TransportAssignmentList> getVehicleList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.IsSuccess = true;
            response.Data = getVehicleList;
            return response;
        }
        private static void SearchTransportAssignmentListPage(SearchTransportAssignmentModel searchTransportAssignmentModel, List<SearchValueData> searchList)
        {
            //if (searchTransportAssignmentModel.FacilityID.HasValue && searchTransportAssignmentModel.FacilityID.value != 0)
            searchList.Add(new SearchValueData { Name = "FacilityID", Value = Convert.ToString(searchTransportAssignmentModel.FacilityID) });
            searchList.Add(new SearchValueData { Name = "VehicleID", Value = Convert.ToString(searchTransportAssignmentModel.VehicleID) });
            searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(searchTransportAssignmentModel.StartDate) });
            searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(searchTransportAssignmentModel.EndDate) });
            searchList.Add(new SearchValueData { Name = "Attendent", Value = Convert.ToString(searchTransportAssignmentModel.Attendent) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchTransportAssignmentModel.IsDeleted) });
            searchList.Add(new SearchValueData { Name = "RouteCode", Value = Convert.ToString(searchTransportAssignmentModel.RouteCode) });
        }

        public ServiceResponse DeleteTransportAssignment(SearchTransportAssignmentModel searchTransportAssignmentModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            SearchTransportAssignmentListPage(searchTransportAssignmentModel, searchList);
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            if (!string.IsNullOrEmpty(searchTransportAssignmentModel.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchTransportAssignmentModel.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

            List<TransportAssignmentList> totalData = GetEntityList<TransportAssignmentList>(StoredProcedure.DeleteTransportAssignment, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<TransportAssignmentList> getVehicleList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getVehicleList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.TransportService);
            return response;
        }

        public ServiceResponse SaveTransportAssignPatient(TransportAssignPatientModel transportAssignPatientModel)
        {
            var response = new ServiceResponse();
            try
            {
                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "TransportAssignPatientID", Value = Convert.ToString(transportAssignPatientModel.TransportAssignPatientID) });
                dataList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(transportAssignPatientModel.ReferralID) });
                dataList.Add(new SearchValueData { Name = "TransportID", Value = Convert.ToString(transportAssignPatientModel.TransportID) });
                if (transportAssignPatientModel.Startdate != null)
                {
                    dataList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(transportAssignPatientModel.Startdate) });
                }
                if (transportAssignPatientModel.EndDate != null)
                {
                    dataList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(transportAssignPatientModel.EndDate) });
                }
                dataList.Add(new SearchValueData { Name = "Note", Value = Convert.ToString(transportAssignPatientModel.Note) });
                // dataList.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(transportAssignPatientModel.OrganizationID) });
                dataList.Add(new SearchValueData { Name = "IsBillable", Value = Convert.ToString(transportAssignPatientModel.IsBillable) });

                dataList.Add(new SearchValueData { Name = "loggedInUserID", Value = SessionHelper.LoggedInID.ToString() });
                //dataList.Add(new SearchValueData { Name = "SystemID ", Value = HttpContext.Current.Request.UserHostAddress });
                int data = (int)GetScalar(StoredProcedure.SaveTransportAssignPatient, dataList);
                if (data == 1)
                {
                    response.IsSuccess = true;
                    response.Message = transportAssignPatientModel.TransportAssignPatientID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.TransportService) :
                        string.Format(Resource.RecordCreatedSuccessfully, Resource.TransportService);
                    return response;
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse HC_SetTransportAssignmentList(long loggedInID)
        {
            ServiceResponse response = new ServiceResponse();

            SetTransaportAssignPatientListModel setReferralListModel = GetMultipleEntity<SetTransaportAssignPatientListModel>(StoredProcedure.HC_TransportAssignmentListPage, new List<SearchValueData>
            {
                new SearchValueData { Name = "DDType_PatientSystemStatus", Value = Convert.ToString((int)Common.DDType.PatientSystemStatus)},
                new SearchValueData { Name = "DDType_LanguagePreference", Value = Convert.ToString((int)Common.DDType.LanguagePreference)}
            });
            setReferralListModel.NotifyCaseManager = Common.SetYesNoAllList();
            setReferralListModel.Checklist = Common.SetYesNoAllList();
            setReferralListModel.ClinicalReview = Common.SetYesNoAllList();
            setReferralListModel.Services = Common.SetServicesFilter();
            setReferralListModel.Draft = Common.SetDraftFilter();
            setReferralListModel.DeleteFilter = Common.SetDeleteFilter();
            setReferralListModel.SearchTransportAssignPatientListModel.IsDeleted = 0;
            setReferralListModel.SearchTransportAssignPatientListModel.ServiceID = -1;
            setReferralListModel.SearchTransportAssignPatientListModel.ChecklistID = -1;
            setReferralListModel.SearchTransportAssignPatientListModel.ClinicalReviewID = -1;
            setReferralListModel.SearchTransportAssignPatientListModel.NotifyCaseManagerID = -1;
            setReferralListModel.SearchTransportAssignPatientListModel.IsSaveAsDraft = -1;
            // setReferralListModel.SearchReferralListModel.ReferralStatusID = -1;//(int)ReferralStatus.ReferralStatuses.Active;
            if (Common.HasPermission(Constants.Permission_View_Assinged_Referral) &&
                !Common.HasPermission(Constants.Permission_View_All_Referral))
            {
                setReferralListModel.SearchTransportAssignPatientListModel.AssigneeID = loggedInID;
            }
            response.Data = setReferralListModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse SearchTransportAssignPatientListModel(SearchTransportAssignPatientListModel searchTransportAssignPatientListModel, int pageIndex, int pageSize,
                                        string sortIndex, string sortDirection, long loggedInId)
        {

            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            List<TransportAssignPatientList> totalData = new List<TransportAssignPatientList>();
            if (searchTransportAssignPatientListModel != null)
                SetSearchFilterForTransportAssignPatientPage(searchTransportAssignPatientListModel, searchList, loggedInId, true);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            totalData = GetEntityList<TransportAssignPatientList>(StoredProcedure.GetTransportAssignPatient, searchList);


            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<TransportAssignPatientList> getReferralList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);

            response.Data = getReferralList;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse DeleteTransportAssignmentReferral(SearchTransportAssignPatientListModel searchTransportAssignPatientListModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            SetSearchFilterForTransportAssignPatientPage(searchTransportAssignPatientListModel, searchList, loggedInUserID, false);
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            if (!string.IsNullOrEmpty(searchTransportAssignPatientListModel.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchTransportAssignPatientListModel.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

            List<TransportAssignPatientList> totalData = GetEntityList<TransportAssignPatientList>(StoredProcedure.DeleteTransportAssignmentPatient, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<TransportAssignPatientList> getVehicleList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getVehicleList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.TransportService);
            return response;
        }
        private static void SetSearchFilterForTransportAssignPatientPage(SearchTransportAssignPatientListModel searchTransportAssignPatientListModel, List<SearchValueData> searchList, long loggedInId, bool isList)
        {
            if (!string.IsNullOrEmpty(searchTransportAssignPatientListModel.ClientName))
                searchList.Add(new SearchValueData { Name = "ClientName", Value = Convert.ToString(searchTransportAssignPatientListModel.ClientName) });

            searchList.Add(new SearchValueData { Name = "NotifyCaseManagerID", Value = Convert.ToString(searchTransportAssignPatientListModel.NotifyCaseManagerID) });

            if (searchTransportAssignPatientListModel.AgencyID > 0)
                searchList.Add(new SearchValueData { Name = "AgencyID", Value = Convert.ToString(searchTransportAssignPatientListModel.AgencyID) });

            if (searchTransportAssignPatientListModel.AgencyLocationID > 0)
                searchList.Add(new SearchValueData { Name = "AgencyLocationID", Value = Convert.ToString(searchTransportAssignPatientListModel.AgencyLocationID) });

            if (searchTransportAssignPatientListModel.AssigneeID > 0)
            {
                searchList.Add(new SearchValueData { Name = "AssigneeID", Value = Convert.ToString(searchTransportAssignPatientListModel.AssigneeID) });
            }
            if (searchTransportAssignPatientListModel.CaseManagerID > 0)
                searchList.Add(new SearchValueData { Name = "CaseManagerID", Value = Convert.ToString(searchTransportAssignPatientListModel.CaseManagerID) });

            searchList.Add(new SearchValueData { Name = "ChecklistID", Value = Convert.ToString(searchTransportAssignPatientListModel.ChecklistID) });

            if (!string.IsNullOrEmpty(searchTransportAssignPatientListModel.AHCCCSID))
            {
                searchList.Add(new SearchValueData { Name = "AHCCCSID", Value = Convert.ToString(searchTransportAssignPatientListModel.AHCCCSID) });
            }

            if (!string.IsNullOrEmpty(searchTransportAssignPatientListModel.CISNumber))
            {
                searchList.Add(new SearchValueData { Name = "CISNumber", Value = Convert.ToString(searchTransportAssignPatientListModel.CISNumber) });
            }

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchTransportAssignPatientListModel.IsDeleted) });

            searchList.Add(new SearchValueData { Name = "ClinicalReviewID", Value = Convert.ToString(searchTransportAssignPatientListModel.ClinicalReviewID) });
            if (searchTransportAssignPatientListModel.TransportID != -1)
                searchList.Add(new SearchValueData { Name = "TransportID", Value = Convert.ToString(searchTransportAssignPatientListModel.TransportID) });

            if (searchTransportAssignPatientListModel.ReferralStatusID > 0)
                searchList.Add(new SearchValueData { Name = "ReferralStatusID", Value = Convert.ToString(searchTransportAssignPatientListModel.ReferralStatusID) });

            if (searchTransportAssignPatientListModel.PayorID > 0)
                searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchTransportAssignPatientListModel.PayorID) });

            if (searchTransportAssignPatientListModel.ServiceID != -1)
                searchList.Add(new SearchValueData { Name = "ServiceID", Value = Convert.ToString(searchTransportAssignPatientListModel.ServiceID) });

            searchList.Add(new SearchValueData { Name = "IsSaveAsDraft", Value = Convert.ToString(searchTransportAssignPatientListModel.IsSaveAsDraft) });

            if (searchTransportAssignPatientListModel.ParentName != null)
                searchList.Add(new SearchValueData { Name = "ParentName", Value = Convert.ToString(searchTransportAssignPatientListModel.ParentName) });

            if (searchTransportAssignPatientListModel.ParentPhone != null)
                searchList.Add(new SearchValueData { Name = "ParentPhone", Value = Convert.ToString(searchTransportAssignPatientListModel.ParentPhone) });

            if (searchTransportAssignPatientListModel.CaseManagerPhone != null)
                searchList.Add(new SearchValueData { Name = "CaseManagerPhone", Value = Convert.ToString(searchTransportAssignPatientListModel.CaseManagerPhone) });

            if (searchTransportAssignPatientListModel.LanguageID > 0)
                searchList.Add(new SearchValueData { Name = "LanguageID", Value = Convert.ToString(searchTransportAssignPatientListModel.LanguageID) });

            if (searchTransportAssignPatientListModel.CommaSeparatedIds != null)
                searchList.Add(new SearchValueData { Name = "Groupdids", Value = Convert.ToString(searchTransportAssignPatientListModel.CommaSeparatedIds) });

            if (searchTransportAssignPatientListModel.CareTypeID != null)
                searchList.Add(new SearchValueData { Name = "CareTypeID", Value = Convert.ToString(searchTransportAssignPatientListModel.CareTypeID) });

            if (searchTransportAssignPatientListModel.RegionID > 0)
                searchList.Add(new SearchValueData { Name = "RegionID", Value = Convert.ToString(searchTransportAssignPatientListModel.RegionID) });

            searchList.Add(new SearchValueData { Name = "ServicetypeId", Value = Convert.ToString(searchTransportAssignPatientListModel.ServiceTypeID) });
            searchList.Add(new SearchValueData { Name = "DDType_PatientSystemStatus", Value = Convert.ToString((int)Common.DDType.PatientSystemStatus) });

            if (isList)
            {
                searchList.Add(new SearchValueData { Name = "EmployeeId", Value = Convert.ToString(loggedInId) });
                searchList.Add(new SearchValueData { Name = "ServerDateTime", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat) });
                searchList.Add(new SearchValueData { Name = "RecordAccess", Value = Common.HasPermission(Constants.AllRecordAccess) ? "-1" : Common.HasPermission(Constants.SameGroupAndLimitedPatientRecordAccess) ? "1" : "0" });
            }
        }

        public ServiceResponse HC_SetTransportAssignmentGroupPage(long transportId)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "TransportID", Value = Convert.ToString(transportId) }
                    };

                SetTransportAssignmentGroupModel transportassignmentModel = GetMultipleEntityAdmin<SetTransportAssignmentGroupModel>(StoredProcedure.HC_SetAddTransportAssignmentGroup, searchList);

                //if (transportassignmentModel.TransportAssignmentModel == null)
                //    transportassignmentModel.TransportAssignmentModel = new TransportAssignmentModel();
                response.Data = transportassignmentModel;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse SearchTransportAssignmentGroupList(SearchTransportAssignmentGroupModel searchTransportAssignmentGroupModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            List<TransportAssignPatientList> totalData = new List<TransportAssignPatientList>();
            if (searchTransportAssignmentGroupModel != null)
            {
                SetSearchFilterForTransportAssignmentGroupListPage(searchTransportAssignmentGroupModel, searchList, loggedInUserID, true);
            }

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            totalData = GetEntityList<TransportAssignPatientList>(StoredProcedure.TransportGroupAssignmentGroupList, searchList);


            int count = 0;
            if (totalData != null && totalData.Count > 0)
            {
                count = totalData.First().Count;
            }

            Page<TransportAssignPatientList> getReferralList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);

            response.Data = getReferralList;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForTransportAssignmentGroupListPage(SearchTransportAssignmentGroupModel searchTransportAssignmentGroupModel, List<SearchValueData> searchList, long loggedInId, bool isList)
        {
            if (!string.IsNullOrEmpty(searchTransportAssignmentGroupModel.Address))
                searchList.Add(new SearchValueData { Name = "Address", Value = Convert.ToString(searchTransportAssignmentGroupModel.Address) });

            if (!string.IsNullOrEmpty(searchTransportAssignmentGroupModel.ClientName))
                searchList.Add(new SearchValueData { Name = "ClientName", Value = Convert.ToString(searchTransportAssignmentGroupModel.ClientName) });

            if (searchTransportAssignmentGroupModel.FacilityID > 0)
            {
                searchList.Add(new SearchValueData { Name = "FacilityID", Value = Convert.ToString(searchTransportAssignmentGroupModel.FacilityID) });
            }
            if (searchTransportAssignmentGroupModel.StartDate != null)
            {
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(searchTransportAssignmentGroupModel.StartDate) });
            }
            if (searchTransportAssignmentGroupModel.EndDate != null)
            {
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(searchTransportAssignmentGroupModel.EndDate) });
            }
            if (searchTransportAssignmentGroupModel.RegionID > 0)
            {
                searchList.Add(new SearchValueData { Name = "RegionID", Value = Convert.ToString(searchTransportAssignmentGroupModel.RegionID) });
            }
            if (searchTransportAssignmentGroupModel.TransportGroupID > 0)
            {
                searchList.Add(new SearchValueData { Name = "TransportGroupID", Value = Convert.ToString(searchTransportAssignmentGroupModel.TransportGroupID) });
            }
        }

        public ServiceResponse GetTransportGroup(long FacilityID, string StartDate, string EndDate)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "FacilityID", Value = Convert.ToString(FacilityID) });
            if (!string.IsNullOrEmpty(StartDate))
            {
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(StartDate) });
            }
            if (!string.IsNullOrEmpty(EndDate))
            {
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(EndDate) });
            }
            List<TransportGroupModel> totalData = GetEntityList<TransportGroupModel>(StoredProcedure.GetTransportGroup, searchList);

            response.IsSuccess = true;
            response.Data = totalData;
            return response;
        }
        public ServiceResponse GetTransportGroupDetail(long TransportGroupID)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "TransportGroupID", Value = Convert.ToString(TransportGroupID) });
            List<TransportGroupAssignPatientModel> totalData = GetEntityList<TransportGroupAssignPatientModel>(StoredProcedure.GetTransportGroupDetail, searchList);

            response.IsSuccess = true;
            response.Data = totalData;
            return response;
        }

        public ServiceResponse SaveTransportGroup(TransportGroupModel transportGroupModel)
        {
            var response = new ServiceResponse();
            try
            {
                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "TransportGroupID", Value = Convert.ToString(transportGroupModel.TransportGroupID) });
                dataList.Add(new SearchValueData { Name = "Name", Value = Convert.ToString(transportGroupModel.Name) });
                dataList.Add(new SearchValueData { Name = "FacilityID", Value = Convert.ToString(transportGroupModel.FacilityID) });
                dataList.Add(new SearchValueData { Name = "TripDirection", Value = Convert.ToString(transportGroupModel.TripDirection) });
                dataList.Add(new SearchValueData { Name = "VehicleID", Value = Convert.ToString(transportGroupModel.VehicleID) });
                dataList.Add(new SearchValueData { Name = "RouteDesc", Value = Convert.ToString(transportGroupModel.RouteDesc) });

                if (transportGroupModel.StartDate != null)
                {
                    dataList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(transportGroupModel.StartDate) });
                }
                if (transportGroupModel.EndDate != null)
                {
                    dataList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(transportGroupModel.EndDate) });
                }
                dataList.Add(new SearchValueData { Name = "loggedInUserID", Value = SessionHelper.LoggedInID.ToString() });
                //
                int data = (int)GetScalar(StoredProcedure.SaveTransportGroup, dataList);
                if (data == 1)
                {
                    response.IsSuccess = true;
                    response.Message = transportGroupModel.TransportGroupID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.TransportAssignmentGroup) :
                        string.Format(Resource.RecordCreatedSuccessfully, Resource.TransportAssignmentGroup);
                    return response;
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse SaveTransportGroupAssignPatient(SearchTransportAssignmentGroupModel searchTransportAssignmentGroupModel)
        {
            var response = new ServiceResponse();
            try
            {
                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "TransportGroupID", Value = Convert.ToString(searchTransportAssignmentGroupModel.TransportGroupID) });
                dataList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = Convert.ToString(searchTransportAssignmentGroupModel.ListOfIdsInCsv) });

                dataList.Add(new SearchValueData { Name = "loggedInID", Value = SessionHelper.LoggedInID.ToString() });
                //
                int data = (int)GetScalar(StoredProcedure.SaveTransportGroupAssignPatient, dataList);
                if (data == 1)
                {
                    response.IsSuccess = true;
                    response.Message = string.Format(Resource.RecordCreatedSuccessfully, Resource.TransportAssignmentGroup);
                    return response;
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse SaveTransportGroupAssignPatientNote(TransportGroupAssignPatientModel transportGroupAssignPatientModel)
        {
            var response = new ServiceResponse();
            try
            {
                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "TransportGroupID", Value = Convert.ToString(transportGroupAssignPatientModel.TransportGroupID) });
                dataList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(transportGroupAssignPatientModel.ReferralID) });
                dataList.Add(new SearchValueData { Name = "Note", Value = Convert.ToString(transportGroupAssignPatientModel.Note) });

                dataList.Add(new SearchValueData { Name = "loggedInID", Value = SessionHelper.LoggedInID.ToString() });
                //
                int data = (int)GetScalar(StoredProcedure.SaveTransportGroupAssignPatientNote, dataList);
                if (data == 1)
                {
                    response.IsSuccess = true;
                    response.Message = string.Format(Resource.RecordCreatedSuccessfully, Resource.PatientNote);
                    return response;
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse DeleteTransportGroupAssignPatientNote(TransportGroupAssignPatientModel transportGroupAssignPatientModel)
        {
            var response = new ServiceResponse();
            try
            {
                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "TransportGroupID", Value = Convert.ToString(transportGroupAssignPatientModel.TransportGroupID) });
                dataList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(transportGroupAssignPatientModel.ReferralID) });
                dataList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(transportGroupAssignPatientModel.IsDeleted) });
                //dataList.Add(new SearchValueData { Name = "Note", Value = Convert.ToString(transportGroupAssignPatientModel.Note) });

                dataList.Add(new SearchValueData { Name = "loggedInID", Value = SessionHelper.LoggedInID.ToString() });
                //
                int data = (int)GetScalar(StoredProcedure.DeleteTransportGroupAssignPatient, dataList);
                if (data == 1)
                {
                    response.IsSuccess = true;
                    response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.TransportService);
                    return response;
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
