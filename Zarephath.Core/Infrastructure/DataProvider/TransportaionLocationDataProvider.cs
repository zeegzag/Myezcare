using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Amazon.S3;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class TransportaionLocationDataProvider : BaseDataProvider, ITransportaionLocation
    {
        #region Add TransportaionLocation

        public ServiceResponse SetAddTransportaionLocationPage(long transportLocationID, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>
                          {
                              new SearchValueData { Name = "TransportLocationID" , Value = transportLocationID.ToString()}
                          };
                AddTransportationLocationModel transportlocation = GetMultipleEntity<AddTransportationLocationModel>(StoredProcedure.GetSetAddTransportationLopcationPage, searchParam);

                if (transportlocation.TransportLocation == null)
                {
                    transportlocation.TransportLocation = new TransportLocation();
                }

                if (transportLocationID > 0 && transportlocation.TransportLocation == null)
                {
                    response.ErrorCode = Constants.ErrorCode_NotFound;
                    response.IsSuccess = true;
                    return response;
                }
                //if (transportlocation.TransportLocation != null && transportLocationID > 0)
                //{
                //    transportlocation.TransportLocation.MapImage = string.IsNullOrEmpty(transportlocation.TransportLocation.MapImage) ? ConfigSettings.NoImageAvailable : transportlocation.TransportLocation.MapImage;
                //    response.Data = transportlocation;
                //    response.IsSuccess = true;
                //    return response;
                //}
                transportlocation.AmazonSettingModel = AmazonFileUpload.GetAmazonModelForClientSideUpload(
                    loggedInUserID, ConfigSettings.AmazoneUploadPath + ConfigSettings.TempFiles + loggedInUserID + "/", ConfigSettings.PublicAcl);
                response.Data = transportlocation;
                return response;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse AddTransportaionLocation(AddTransportationLocationModel transportLocation, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            if (loggedInUserID == 0)
            {
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.UserSessionExpired);
                return response;
            }

            if (transportLocation != null && transportLocation.TransportLocation != null)
            {
                transportLocation.IsEditMode = transportLocation.TransportLocation.TransportLocationID > 0;

                if (transportLocation.IsEditMode)
                {
                    List<SearchValueData> searchCaseManager = new List<SearchValueData>
                        {
                            new SearchValueData{Name = "TransportLocationID",Value = transportLocation.TransportLocation.TransportLocationID.ToString()},
                        };
                    TransportLocation transportlocation = GetEntity<TransportLocation>(searchCaseManager);

                    if (transportlocation != null && transportLocation.TransportLocation.TransportLocationID > 0)
                    {
                        if (transportlocation.MapImage != transportLocation.TransportLocation.MapImage)
                        {

                            string destPath = null;
                            if (!string.IsNullOrEmpty(transportLocation.TransportLocation.MapImage))
                            {
                                AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                                string bucket = ConfigSettings.ZarephathBucket;
                                destPath = ConfigSettings.AmazoneUploadPath + ConfigSettings.Transportation + Path.GetFileName(transportLocation.TransportLocation.MapImage);
                                amazonFileUpload.MoveFile(bucket, transportLocation.TransportLocation.MapImage, bucket, destPath, S3CannedACL.PublicRead);
                            }
                            //else
                            //{
                            //    destPath = ConfigSettings.SiteBaseURL + Constants.NoMapImageUrl;
                            //}


                            transportLocation.TransportLocation.MapImage = destPath;
                        }
                        transportlocation.Location = transportLocation.TransportLocation.Location;
                        transportlocation.LocationCode = transportLocation.TransportLocation.LocationCode;
                        transportlocation.Address = transportLocation.TransportLocation.Address;
                        transportlocation.City = transportLocation.TransportLocation.City;
                        transportlocation.State = transportLocation.TransportLocation.State;
                        transportlocation.Zip = transportLocation.TransportLocation.Zip;
                        transportlocation.Phone = transportLocation.TransportLocation.Phone;
                        transportlocation.MapImage = transportLocation.TransportLocation.MapImage;
                        transportlocation.RegionID = transportLocation.TransportLocation.RegionID;

                        transportlocation.MondayPickUp = transportLocation.TransportLocation.MondayPickUp;
                        transportlocation.MondayDropOff = transportLocation.TransportLocation.MondayDropOff;

                        transportlocation.TuesdayPickUp = transportLocation.TransportLocation.TuesdayPickUp;
                        transportlocation.TuesdayDropOff = transportLocation.TransportLocation.TuesdayDropOff;

                        transportlocation.WednesdayPickUp = transportLocation.TransportLocation.WednesdayPickUp;
                        transportlocation.WednesdayDropOff = transportLocation.TransportLocation.WednesdayDropOff;

                        transportlocation.ThursdayPickUp = transportLocation.TransportLocation.ThursdayPickUp;
                        transportlocation.ThursdayDropOff = transportLocation.TransportLocation.ThursdayDropOff;

                        transportlocation.FridayPickUp = transportLocation.TransportLocation.FridayPickUp;
                        transportlocation.FridayDropOff = transportLocation.TransportLocation.FridayDropOff;

                        transportlocation.SaturdayPickUp = transportLocation.TransportLocation.SaturdayPickUp;
                        transportlocation.SaturdayDropOff = transportLocation.TransportLocation.SaturdayDropOff;

                        transportlocation.SundayPickUp = transportLocation.TransportLocation.SundayPickUp;
                        transportlocation.SundayDropOff = transportLocation.TransportLocation.SundayDropOff;
                        response = SaveObject(transportlocation, loggedInUserID);
                    }
                    else
                    {
                        response.IsSuccess = false;
                        return response;
                    }
                }
                else
                {
                    //Remove File
                    string destPath = null;

                    if (!string.IsNullOrEmpty(transportLocation.TransportLocation.MapImage))
                    {
                        AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                        string bucket = ConfigSettings.ZarephathBucket;

                        destPath = ConfigSettings.AmazoneUploadPath + ConfigSettings.Transportation +
                                          transportLocation.TransportLocation.MapImage.Substring(transportLocation.TransportLocation.MapImage.LastIndexOf('/'));
                        amazonFileUpload.MoveFile(bucket, transportLocation.TransportLocation.MapImage, bucket, destPath, S3CannedACL.PublicRead);
                    }
                    //else
                    //{
                    //    destPath = ConfigSettings.SiteBaseURL + Constants.NoMapImageUrl;
                    //}
                    transportLocation.TransportLocation.MapImage = destPath;
                    response = SaveObject(transportLocation.TransportLocation, loggedInUserID);
                }
                if (!response.IsSuccess)
                {
                    response.Message = string.Format(Resource.RecordAlreadyExists, Resource.LocationCode);
                }
                else
                {
                    response.Message = !transportLocation.IsEditMode ? string.Format(Resource.RecordCreatedSuccessfully, Resource.TransportationLocation)
                                             : string.Format(Resource.RecordUpdatedSuccessfully, Resource.TransportationLocation);
                }
            }
            return response;
        }

        #endregion

        #region TransportaionLocation List

        public ServiceResponse SetTransportaionLocationListPage()
        {
            var response = new ServiceResponse();
            SetTransPortationListPage setTransPortationListPage = new SetTransPortationListPage();
            setTransPortationListPage.DeleteFilter = Common.SetDeleteFilter();
            setTransPortationListPage.RegionList = GetEntityList<Region>();
            setTransPortationListPage.SearchTransPortationListPage.IsDeleted = 0;
            response.Data = setTransPortationListPage;
            return response;
        }

        public ServiceResponse GetTransportaionLocationList(SearchTransPortationListPage searchTransportLocationModel, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();

                if (searchTransportLocationModel != null)
                    SetSearchFilterForTransportLocationListPage(searchTransportLocationModel, searchList);
                Page<ListTransportaionModel> transportlocationlist = GetEntityPageList<ListTransportaionModel>(StoredProcedure.GetTransportaLocationList, searchList, pageSize,
                                                              pageIndex, sortIndex, sortDirection);
                response.Data = transportlocationlist;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.Department), Resource.ExceptionMessage);
            }
            return response;
        }

        private static void SetSearchFilterForTransportLocationListPage(SearchTransPortationListPage searchTransportLocationModel, List<SearchValueData> searchList)
        {
            if (searchTransportLocationModel.TransportLocationID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "TransportLocationID",
                    Value = Convert.ToString(searchTransportLocationModel.TransportLocationID)
                });

            if (!string.IsNullOrEmpty(searchTransportLocationModel.Location))
                searchList.Add(new SearchValueData { Name = "Location", Value = Convert.ToString(searchTransportLocationModel.Location) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchTransportLocationModel.IsDeleted) });

            if (!string.IsNullOrEmpty(searchTransportLocationModel.Address))
                searchList.Add(new SearchValueData { Name = "Address", Value = Convert.ToString(searchTransportLocationModel.Address) });

            if (!string.IsNullOrEmpty(searchTransportLocationModel.LocationCode))
                searchList.Add(new SearchValueData { Name = "LocationCode", Value = Convert.ToString(searchTransportLocationModel.LocationCode) });

            if (!string.IsNullOrEmpty(searchTransportLocationModel.State))
                searchList.Add(new SearchValueData { Name = "State", Value = Convert.ToString(searchTransportLocationModel.State) });

            if (!string.IsNullOrEmpty(searchTransportLocationModel.Phone))
                searchList.Add(new SearchValueData { Name = "Phone", Value = Convert.ToString(searchTransportLocationModel.Phone) });

            if (searchTransportLocationModel.RegionID > 0)
                searchList.Add(new SearchValueData { Name = "RegionID", Value = searchTransportLocationModel.RegionID.ToString() });
        }

        public ServiceResponse DeleteTransportaionLocation(SearchTransPortationListPage searchTransportLocationModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

                SetSearchFilterForTransportLocationListPage(searchTransportLocationModel, searchList);

                if (!string.IsNullOrEmpty(searchTransportLocationModel.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchTransportLocationModel.ListOfIdsInCSV });

                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

                List<ListTransportaionModel> totalData = GetEntityList<ListTransportaionModel>(StoredProcedure.DeleteTransportLocation, searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                //if (count == 0 && totalData != null && totalData.Count > 0)
                //{
                //    response.Message = Common.MessageWithTitle(string.Format(Resource.DeleteFailed, Resource.TransportationLocation), Resource.TransportEmployeeExistMessage);
                //    return response;
                //}

                Page<ListTransportaionModel> getTransportaionList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);

                response.Data = getTransportaionList;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.TransportationLocation);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }

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
