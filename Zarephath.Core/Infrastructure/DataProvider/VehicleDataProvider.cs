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
    public class VehicleDataProvider : BaseDataProvider, IVehicleDataProvider
    {
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

    }
}
