using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure.Utility;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;
//using static Zarephath.Core.Models.ViewModel.HC_AddPayorModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class PayorDataProvider : BaseDataProvider, IPayorDataProvider
    {
        #region Add Payor Detail

        public ServiceResponse SetAddPayorPage(long payorId)
        {
            var response = new ServiceResponse();
            try
            {
                bool isEditMode = payorId > 0;
                List<SearchValueData> searchParam = new List<SearchValueData> { new SearchValueData { Name = "PayorID", Value = payorId.ToString() } };
                AddPayorModel addpayormodel = GetMultipleEntity<AddPayorModel>(StoredProcedure.GetSetAddPayorPage, searchParam);

                if (isEditMode && addpayormodel.Payor == null)
                {
                    response.ErrorCode = Constants.ErrorCode_NotFound;
                    return response;
                }

                if (addpayormodel.Payor == null)
                    addpayormodel.Payor = new Payor();

                if (addpayormodel.PayorEdi837Setting == null)
                    addpayormodel.PayorEdi837Setting = new PayorEdi837Setting();

                addpayormodel.PayorServiceCodeMapping = new PayorServiceCodeMapping();
                //addpayormodel.ModifierList = Common.GetListFromEnum<EnumModifiers>();
                //addpayormodel.POSList = Common.GetListFromEnum<EnumPlaceOfServices>();

                addpayormodel.DeleteFilter = Common.SetDeleteFilter();
                addpayormodel.SearchServiceCodeMappingList.IsDeleted = 0;

                if (isEditMode)
                    addpayormodel.Payor.EncryptedPayorID = Crypto.Encrypt(Convert.ToString(payorId));

                response.Data = addpayormodel;
                response.IsSuccess = true;
                return response;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse AddPayorDetail(AddPayorModel addPayorModel, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();
            if (addPayorModel.Payor != null)
            {
                bool isEditMode = addPayorModel.Payor.PayorID > 0;

                string customWhere = string.Format("PayorName='{0}' OR ShortName='{1}' OR PayorIdentificationNumber='{2}'", Convert.ToString(addPayorModel.Payor.PayorName).Replace("'", "''"), Convert.ToString(addPayorModel.Payor.ShortName).Replace("'", "''"), addPayorModel.Payor.PayorIdentificationNumber.Replace("'", "''"));

                List<SearchValueData> searchModel = new List<SearchValueData> { new SearchValueData { Name = "PayorID", Value = addPayorModel.Payor.PayorID.ToString(), IsNotEqual = true } };
                int payorCount = GetEntityCount<Payor>(isEditMode ? searchModel : null, customWhere);

                if (payorCount > 0)
                {
                    response.IsSuccess = false;
                    response.Message = Resource.PayorAlreadyExists;
                    return response;
                }
                if (isEditMode)
                {
                    Payor tempPayor = GetEntity<Payor>(addPayorModel.Payor.PayorID);
                    tempPayor.PayorName = addPayorModel.Payor.PayorName;
                    tempPayor.ShortName = addPayorModel.Payor.ShortName;
                    tempPayor.PayorSubmissionName = addPayorModel.Payor.PayorSubmissionName;
                    tempPayor.PayorIdentificationNumber = addPayorModel.Payor.PayorIdentificationNumber;
                    tempPayor.Address = addPayorModel.Payor.Address;
                    tempPayor.City = addPayorModel.Payor.City;
                    tempPayor.StateCode = addPayorModel.Payor.StateCode;
                    tempPayor.ZipCode = addPayorModel.Payor.ZipCode;
                    tempPayor.PayorTypeID = addPayorModel.Payor.PayorTypeID;
                    tempPayor.BillingProviderID = addPayorModel.Payor.BillingProviderID;
                    tempPayor.RenderingProviderID = addPayorModel.Payor.RenderingProviderID;
                    tempPayor.EraPayorID = addPayorModel.Payor.EraPayorID;
                    response.IsSuccess = true;
                    response = SaveObject(tempPayor, loggedInUserId);
                    response.Data = Crypto.Encrypt(Convert.ToString(tempPayor.PayorID));
                }
                else
                {
                    response.IsSuccess = true;
                    response = SaveObject(addPayorModel.Payor, loggedInUserId);
                    response.Data = Crypto.Encrypt(Convert.ToString(addPayorModel.Payor.PayorID));
                }
                if (!response.IsSuccess)
                    response.Message = string.Format(Resource.RecordAlreadyExists, Resource.PayorNamealrearedyexist);
                else
                    response.Message = !isEditMode ? string.Format(Resource.RecordCreatedSuccessfully, Resource.PayorDetail)
                                           : string.Format(Resource.RecordUpdatedSuccessfully, Resource.PayorDetail);
            }
            return response;
        }

        #endregion

        #region  Add Service Code

        public ServiceResponse AddServiceCode(PayorServiceCodeMapping addPayorServiceCodeMappingModel, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                bool isEditMode = addPayorServiceCodeMappingModel.PayorServiceCodeMappingID > 0;
                addPayorServiceCodeMappingModel.PayorID = Convert.ToInt64(Crypto.Decrypt(addPayorServiceCodeMappingModel.EncryptedPayorId));

                List<SearchValueData> searchModel = new List<SearchValueData>
                {
                    new SearchValueData { Name = "PayorServiceCodeMappingID", Value = addPayorServiceCodeMappingModel.PayorServiceCodeMappingID.ToString(), IsNotEqual = true },
                    new SearchValueData{Name = "PayorID",Value =addPayorServiceCodeMappingModel.PayorID.ToString(),IsEqual = true},
                    new SearchValueData{Name = "ServiceCodeID",Value = addPayorServiceCodeMappingModel.ServiceCodeID.ToString(),IsEqual = true},
                    new SearchValueData{Name = "PosID",Value = addPayorServiceCodeMappingModel.PosID.ToString(),IsEqual = true},
                    new SearchValueData{Name = "StartDate",Value =Convert.ToDateTime(addPayorServiceCodeMappingModel.POSStartDate).ToString(Constants.DbDateFormat),IsEqual = true},
                    new SearchValueData{Name = "EndDate",Value =Convert.ToDateTime(addPayorServiceCodeMappingModel.POSEndDate).ToString(Constants.DbDateFormat),IsEqual = true}
                };

                if ((int)GetScalar(StoredProcedure.CheckForDuplicatePayorServiceCodeMapping, searchModel) > 0)
                {
                    response.Message = Resource.PSCMRecordAlreadyExists;
                    return response;
                }

                PayorServiceCodeMapping payorServiceCodeMapping = isEditMode ? GetEntity<PayorServiceCodeMapping>(addPayorServiceCodeMappingModel.PayorServiceCodeMappingID) : new PayorServiceCodeMapping();
                payorServiceCodeMapping.PayorID = addPayorServiceCodeMappingModel.PayorID;
                payorServiceCodeMapping.ServiceCodeID = addPayorServiceCodeMappingModel.ServiceCodeID;
                payorServiceCodeMapping.ModifierID = addPayorServiceCodeMappingModel.ModifierID == 0 ? null : addPayorServiceCodeMappingModel.ModifierID;
                payorServiceCodeMapping.Rate = addPayorServiceCodeMappingModel.Rate;
                payorServiceCodeMapping.PosID = addPayorServiceCodeMappingModel.PosID;
                payorServiceCodeMapping.POSStartDate = addPayorServiceCodeMappingModel.POSStartDate;
                payorServiceCodeMapping.POSEndDate = addPayorServiceCodeMappingModel.POSEndDate;
                payorServiceCodeMapping.BillingUnitLimit = addPayorServiceCodeMappingModel.BillingUnitLimit;
                response = SaveObject(payorServiceCodeMapping, loggedInUserId);
                response.IsSuccess = true;
                response.Message = isEditMode ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.ServiceCodeMapping) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.ServiceCodeMapping);

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse GetServiceCodeMappingList(string encryptedPayorId, SearchServiceCodeMappingList searchServiceCodeMappingList, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchServiceCodeMappingList.PayorID = Convert.ToInt64(encryptedPayorId);

                SetSearchFilterForServiceCodeMappingListPage(searchServiceCodeMappingList, searchList);

                Page<PayorServiceCodeMappingListPage> listPayorServiceModel = GetEntityPageList<PayorServiceCodeMappingListPage>(StoredProcedure.GetPayorServiceCodeMappingList,
                                                                    searchList, pageSize, pageIndex, sortIndex, sortDirection);
                response.IsSuccess = true;
                response.Data = listPayorServiceModel;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse DeleteServiceCodeMapping(string encryptedPayorId, SearchServiceCodeMappingList searchServiceCodeMappingList, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                searchServiceCodeMappingList.PayorID = Convert.ToInt64(encryptedPayorId);
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                SetSearchFilterForServiceCodeMappingListPage(searchServiceCodeMappingList, searchList);

                if (!string.IsNullOrEmpty(searchServiceCodeMappingList.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchServiceCodeMappingList.ListOfIdsInCSV });

                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

                List<PayorServiceCodeMappingListPage> totalData = GetEntityList<PayorServiceCodeMappingListPage>(StoredProcedure.DeletePayorMappingCode, searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<PayorServiceCodeMappingListPage> listPayorModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = listPayorModel;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.ServiceCode);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }

        private static void SetSearchFilterForServiceCodeMappingListPage(SearchServiceCodeMappingList searchServiceCodeMappingList, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "ServiceCode", Value = Convert.ToString(searchServiceCodeMappingList.ServiceCode) });
            searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchServiceCodeMappingList.PayorID), IsEqual = true });
            //if (searchServiceCodeMappingList.ModifierID > 0)
            searchList.Add(new SearchValueData { Name = "ModifierID", Value = Convert.ToString(searchServiceCodeMappingList.ModifierID) });
            if (searchServiceCodeMappingList.PosID > 0)
                searchList.Add(new SearchValueData { Name = "PosID", Value = Convert.ToString(searchServiceCodeMappingList.PosID) });
            if (searchServiceCodeMappingList.POSStartDate != null)
                searchList.Add(new SearchValueData { Name = "POSStartDate", Value = Convert.ToDateTime(searchServiceCodeMappingList.POSStartDate).ToString(Constants.DbDateFormat) });
            if (searchServiceCodeMappingList.POSEndDate != null)
                searchList.Add(new SearchValueData { Name = "POSEndDate", Value = Convert.ToDateTime(searchServiceCodeMappingList.POSEndDate).ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "IsDeleted ", Value = Convert.ToString(searchServiceCodeMappingList.IsDeleted) });
        }

        #endregion

        #region Service Code Mapping
        public ServiceResponse HC_AddPayorServiceCodeNew(PayorServiceCodeMapping addPayorServiceCodeMappingModel, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                if (addPayorServiceCodeMappingModel.SelectedPayors != null && addPayorServiceCodeMappingModel.SelectedPayors.Count() > 0)
                {
                    foreach (var _PayorID in addPayorServiceCodeMappingModel.SelectedPayors)
                    {
                        bool isEditMode = addPayorServiceCodeMappingModel.PayorServiceCodeMappingID > 0;

                        List<SearchValueData> searchModel = new List<SearchValueData>
                        {
                            new SearchValueData { Name = "PayorServiceCodeMappingID", Value = addPayorServiceCodeMappingModel.PayorServiceCodeMappingID.ToString(), IsNotEqual = true },
                            new SearchValueData{Name = "PayorID",Value =addPayorServiceCodeMappingModel.PayorID.ToString(),IsEqual = true},
                            new SearchValueData{Name = "CareType",Value =addPayorServiceCodeMappingModel.CareType.ToString(),IsEqual = true},
                            new SearchValueData{Name = "ServiceCodeID",Value = addPayorServiceCodeMappingModel.ServiceCodeID.ToString(),IsEqual = true},
                            new SearchValueData{Name = "RevenuCode",Value = addPayorServiceCodeMappingModel.RevenueCode.ToString(),IsEqual = true},
                            new SearchValueData{Name = "StartDate",Value =Convert.ToDateTime(addPayorServiceCodeMappingModel.POSStartDate).ToString(Constants.DbDateFormat),IsEqual = true},
                            new SearchValueData{Name = "EndDate",Value =Convert.ToDateTime(addPayorServiceCodeMappingModel.POSEndDate).ToString(Constants.DbDateFormat),IsEqual = true}
                        };
                        int check = (int)GetScalar(StoredProcedure.HC_CheckForDuplicatePayorServiceCodeMapping, searchModel);
                        if (check > 0)
                        {
                            response.Message = Resource.PSCMRecordAlreadyExists;
                            return response;
                        }

                        PayorServiceCodeMapping payorServiceCodeMapping = isEditMode ? GetEntity<PayorServiceCodeMapping>(addPayorServiceCodeMappingModel.PayorServiceCodeMappingID) : new PayorServiceCodeMapping();
                        payorServiceCodeMapping.PayorID = _PayorID;
                        payorServiceCodeMapping.ServiceCodeID = addPayorServiceCodeMappingModel.ServiceCodeID;
                        payorServiceCodeMapping.POSStartDate = addPayorServiceCodeMappingModel.POSStartDate;
                        payorServiceCodeMapping.POSEndDate = addPayorServiceCodeMappingModel.POSEndDate;
                        payorServiceCodeMapping.CareType = addPayorServiceCodeMappingModel.CareType;
                        payorServiceCodeMapping.RevenueCode = addPayorServiceCodeMappingModel.RevenueCode;
                        payorServiceCodeMapping.Rate = addPayorServiceCodeMappingModel.Rate;
                        payorServiceCodeMapping.IsDeleted = addPayorServiceCodeMappingModel.IsDeleted;
                        payorServiceCodeMapping.UnitType = addPayorServiceCodeMappingModel.UnitType;
                        if (payorServiceCodeMapping.UnitType == Convert.ToInt32(EnumUnitType.Time))
                        {
                            payorServiceCodeMapping.PerUnitQuantity = addPayorServiceCodeMappingModel.PerUnitQuantity;
                            payorServiceCodeMapping.RoundUpUnit = addPayorServiceCodeMappingModel.RoundUpUnit;
                            payorServiceCodeMapping.MaxUnit = addPayorServiceCodeMappingModel.MaxUnit;
                            payorServiceCodeMapping.DailyUnitLimit = addPayorServiceCodeMappingModel.DailyUnitLimit;
                        }
                        else
                        {
                            payorServiceCodeMapping.PerUnitQuantity = null;
                            payorServiceCodeMapping.RoundUpUnit = null;
                            payorServiceCodeMapping.MaxUnit = null;
                            payorServiceCodeMapping.DailyUnitLimit = null;
                        }
                        response = SaveObject(payorServiceCodeMapping, loggedInUserId);
                        response.IsSuccess = true;
                        response.Message = isEditMode ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.ServiceCodeMapping) :
                            string.Format(Resource.RecordCreatedSuccessfully, Resource.ServiceCodeMapping);
                    }
                }
                else
                {
                    bool isEditMode = addPayorServiceCodeMappingModel.PayorServiceCodeMappingID > 0;

                    List<SearchValueData> searchModel = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "PayorServiceCodeMappingID", Value = addPayorServiceCodeMappingModel.PayorServiceCodeMappingID.ToString(), IsNotEqual = true },
                        new SearchValueData{Name = "PayorID",Value =addPayorServiceCodeMappingModel.PayorID.ToString(),IsEqual = true},
                        new SearchValueData{Name = "CareType",Value =addPayorServiceCodeMappingModel.CareType.ToString(),IsEqual = true},
                        new SearchValueData{Name = "ServiceCodeID",Value = addPayorServiceCodeMappingModel.ServiceCodeID.ToString(),IsEqual = true},
                        new SearchValueData{Name = "RevenuCode",Value = addPayorServiceCodeMappingModel.RevenueCode.ToString(),IsEqual = true},
                        new SearchValueData{Name = "StartDate",Value =Convert.ToDateTime(addPayorServiceCodeMappingModel.POSStartDate).ToString(Constants.DbDateFormat),IsEqual = true},
                        new SearchValueData{Name = "EndDate",Value =Convert.ToDateTime(addPayorServiceCodeMappingModel.POSEndDate).ToString(Constants.DbDateFormat),IsEqual = true}
                    };
                    int check = (int)GetScalar(StoredProcedure.HC_CheckForDuplicatePayorServiceCodeMapping, searchModel);
                    if (check > 0)
                    {
                        response.Message = Resource.PSCMRecordAlreadyExists;
                        return response;
                    }

                    PayorServiceCodeMapping payorServiceCodeMapping = isEditMode ? GetEntity<PayorServiceCodeMapping>(addPayorServiceCodeMappingModel.PayorServiceCodeMappingID) : new PayorServiceCodeMapping();
                    payorServiceCodeMapping.PayorID = addPayorServiceCodeMappingModel.PayorID;
                    payorServiceCodeMapping.ServiceCodeID = addPayorServiceCodeMappingModel.ServiceCodeID;
                    payorServiceCodeMapping.POSStartDate = addPayorServiceCodeMappingModel.POSStartDate;
                    payorServiceCodeMapping.POSEndDate = addPayorServiceCodeMappingModel.POSEndDate;
                    payorServiceCodeMapping.CareType = addPayorServiceCodeMappingModel.CareType;
                    payorServiceCodeMapping.RevenueCode = addPayorServiceCodeMappingModel.RevenueCode;
                    payorServiceCodeMapping.Rate = addPayorServiceCodeMappingModel.Rate;
                    payorServiceCodeMapping.IsDeleted = addPayorServiceCodeMappingModel.IsDeleted;
                    payorServiceCodeMapping.UnitType = addPayorServiceCodeMappingModel.UnitType;
                    if (payorServiceCodeMapping.UnitType == Convert.ToInt32(EnumUnitType.Time))
                    {
                        payorServiceCodeMapping.PerUnitQuantity = addPayorServiceCodeMappingModel.PerUnitQuantity;
                        payorServiceCodeMapping.RoundUpUnit = addPayorServiceCodeMappingModel.RoundUpUnit;
                        payorServiceCodeMapping.MaxUnit = addPayorServiceCodeMappingModel.MaxUnit;
                        payorServiceCodeMapping.DailyUnitLimit = addPayorServiceCodeMappingModel.DailyUnitLimit;
                    }
                    else
                    {
                        payorServiceCodeMapping.PerUnitQuantity = null;
                        payorServiceCodeMapping.RoundUpUnit = null;
                        payorServiceCodeMapping.MaxUnit = null;
                        payorServiceCodeMapping.DailyUnitLimit = null;
                    }
                    response = SaveObject(payorServiceCodeMapping, loggedInUserId);
                    response.IsSuccess = true;
                    response.Message = isEditMode ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.ServiceCodeMapping) :
                        string.Format(Resource.RecordCreatedSuccessfully, Resource.ServiceCodeMapping);

                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_GetServiceCodeMappingListNew(SearchServiceCodeMappingList searchServiceCodeMappingList, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();

                HC_SetSearchFilterForServiceCodeMappingListPage(searchServiceCodeMappingList, searchList);

                Page<PayorServiceCodeMappingListPage> listPayorServiceModel = GetEntityPageList<PayorServiceCodeMappingListPage>(StoredProcedure.HC_GetPayorServiceCodeMappingList,
                                                                    searchList, pageSize, pageIndex, sortIndex, sortDirection);
                response.IsSuccess = true;
                response.Data = listPayorServiceModel;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                //response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
                response.Message = e.Message;
            }
            return response;
        }

        public ServiceResponse HC_DeleteServiceCodeMappingNew(SearchServiceCodeMappingList searchServiceCodeMappingList, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                HC_SetSearchFilterForServiceCodeMappingListPage(searchServiceCodeMappingList, searchList);

                if (!string.IsNullOrEmpty(searchServiceCodeMappingList.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchServiceCodeMappingList.ListOfIdsInCSV });

                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

                List<PayorServiceCodeMappingListPage> totalData = GetEntityList<PayorServiceCodeMappingListPage>(StoredProcedure.HC_DeletePayorMappingCode, searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<PayorServiceCodeMappingListPage> listPayorModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = listPayorModel;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.ServiceCode);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }
        #endregion  

        #region  Payor Listing

        public ServiceResponse SetPayorListPage()
        {
            var response = new ServiceResponse();
            try
            {
                SetPayorListPage setPayorListPage = GetMultipleEntity<SetPayorListPage>(StoredProcedure.SetPayorListPage);
                setPayorListPage.DeleteFilter = Common.SetDeleteFilter();
                setPayorListPage.SearchPayorListPage.IsDeleted = 0;
                response.Data = setPayorListPage;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse GetPayorList(SearchPayorListPage searchPayorListPageModel, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();

                if (searchPayorListPageModel != null)
                    SetSearchFilterForPayorListPage(searchPayorListPageModel, searchList);
                if (string.IsNullOrEmpty(sortIndex))
                {
                    sortIndex = "FYStartDate";
                }
                if (string.IsNullOrEmpty(sortDirection))
                {
                    sortIndex = "ASC";
                }

                Page<ListPayorModel> listPayorModel = GetEntityPageList<ListPayorModel>(StoredProcedure.GetPayorList, searchList, pageSize,
                                                              pageIndex, sortIndex, sortDirection);
                response.Data = listPayorModel;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.Payor), Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse DeletePayor(SearchPayorListPage searchPayorListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                SetSearchFilterForPayorListPage(searchPayorListPage, searchList);

                if (!string.IsNullOrEmpty(searchPayorListPage.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchPayorListPage.ListOfIdsInCSV });

                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

                List<ListPayorModel> totalData = GetEntityList<ListPayorModel>(StoredProcedure.DeletePayor, searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListPayorModel> listPayorModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = listPayorModel;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Payor);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }

        private static void SetSearchFilterForPayorListPage(SearchPayorListPage searchPayorListPage, List<SearchValueData> searchList)
        {
            if (searchPayorListPage.PayorID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "PayorID",
                    Value = Convert.ToString(searchPayorListPage.PayorID)
                });

            if (searchPayorListPage.PayorTypeID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "PayorTypeID",
                    Value = Convert.ToString(searchPayorListPage.PayorTypeID)
                });

            if (!string.IsNullOrEmpty(searchPayorListPage.PayorName))
                searchList.Add(new SearchValueData { Name = "PayorName", Value = Convert.ToString(searchPayorListPage.PayorName) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchPayorListPage.IsDeleted) });

            if (!string.IsNullOrEmpty(searchPayorListPage.ShortName))
                searchList.Add(new SearchValueData { Name = "ShortName", Value = Convert.ToString(searchPayorListPage.ShortName) });

            if (!string.IsNullOrEmpty(searchPayorListPage.PayorSubmissionName))
                searchList.Add(new SearchValueData { Name = "PayorSubmissionName", Value = Convert.ToString(searchPayorListPage.PayorSubmissionName) });

            if (!string.IsNullOrEmpty(searchPayorListPage.PayorIdentificationNumber))
                searchList.Add(new SearchValueData { Name = "PayorIdentificationNumber", Value = Convert.ToString(searchPayorListPage.PayorIdentificationNumber) });

            if (!string.IsNullOrEmpty(searchPayorListPage.Address))
                searchList.Add(new SearchValueData { Name = "Address", Value = Convert.ToString(searchPayorListPage.Address) });

        }

        #endregion

        #region AutoCompleter Funcation

        public List<ZipCodes> GetZipCodeList(string searchText, string state, int pageSize)
        {
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

        public List<ServiceCodes> GetServiceCodeList(string searchText, int pageSize = 10)
        {
            List<ServiceCodes> contactlist = GetEntityList<ServiceCodes>(StoredProcedure.GetPayorServiceCodeListForAutoCompleter,
                                            new List<SearchValueData>
                                                {
                                                    new SearchValueData {Name = "SearchText", Value = searchText},
                                                    new SearchValueData {Name = "PageSize", Value = pageSize.ToString()}
                                                });
            return contactlist;
        }

        #endregion AutoCompleter Funcation

        #region Homecare Related Dode

        #region Add Payor Detail

        public ServiceResponse HC_SetAddPayorPage(long payorId)
        {
            var response = new ServiceResponse();
            try
            {
                bool isEditMode = payorId > 0;
                List<SearchValueData> searchParam = new List<SearchValueData> {
                    new SearchValueData { Name = "PayorID", Value = payorId.ToString() },
                    new SearchValueData { Name = "DDType_NPIOptions", Value = Convert.ToString((int)Common.DDType.NPIOptions)},
                    new SearchValueData { Name = "DDType_PayerGroup", Value = Convert.ToString((int)Common.DDType.PayerGroup)},
                    new SearchValueData { Name = "DDType_BussinessLine", Value = Convert.ToString((int)Common.DDType.BussinessLine)},
                    new SearchValueData { Name = "DDType_RevenueCode", Value = Convert.ToString((int)Common.DDType.RevenueCode)},
                  //  new SearchValueData { Name = "DDType_CareType", Value = Convert.ToString((int)Common.DDType.CareType)}
                    new SearchValueData { Name = "DDType_CareType", Value = Convert.ToString((int)Common.DDType.CareType)},
                    new SearchValueData { Name = "DDType_NPINumber", Value = Convert.ToString((int)Common.DDType.NPINumber)}

                };
                HC_AddPayorModel addpayormodel = GetMultipleEntity<HC_AddPayorModel>(StoredProcedure.HC_GetSetAddPayorPage, searchParam);

                if (isEditMode && addpayormodel.Payor == null)
                {
                    response.ErrorCode = Constants.ErrorCode_NotFound;
                    return response;
                }

                if (addpayormodel.Payor == null)
                    addpayormodel.Payor = new Payor();

                addpayormodel.PayorServiceCodeMapping = new PayorServiceCodeMapping();
                addpayormodel.SearchServiceCodeMappingList = new SearchServiceCodeMappingList();
                addpayormodel.PayorBillingTypeList = Common.SetEDIFileTypeList();
                addpayormodel.PayorInvoiceTypeList = Common.SetPayorInvoiceTypeList();
                addpayormodel.PayorClaimProcessorList = Common.SetPayorClaimProcessorList();
                addpayormodel.PayorVisitBilledByList = Common.SetPayorVisitBilledByList();

                addpayormodel.DeleteFilter = Common.SetDeleteFilter();
                addpayormodel.SearchServiceCodeMappingList.IsDeleted = 0;
                addpayormodel.SearchServiceCodeMappingList.CareType = -1;

                if (isEditMode)
                    addpayormodel.Payor.EncryptedPayorID = Crypto.Encrypt(Convert.ToString(payorId));

                response.Data = addpayormodel;
                response.IsSuccess = true;
                return response;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse SearchPayor(HC_SearchPayorModel model)
        {
            var response = new ServiceResponse();
            try
            {
                ClaimMDApiHelper helper = new ClaimMDApiHelper();
                PayerListResponse data = helper.GetPayerList(model.PayorID, model.PayorName);
                if (data != null && data.result != null && data.result.payer != null)
                {
                    response.Data = data.result.payer;
                }
                response.IsSuccess = true;
                return response;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse PayorEnrollment(HC_PayorEnrollmentModel model)
        {
            var response = new ServiceResponse();
            try
            {
                OrganizationSetting os = GetEntity<OrganizationSetting>();

                model.ProviderTaxID = os != null ? os.BillingProvider_REF02_ReferenceIdentification : "";
                model.ProviderNPI = os != null ? os.Submitter_NM109_IdCode :"";

                if (string.IsNullOrEmpty(model.ProviderTaxID) || string.IsNullOrEmpty(model.ProviderNPI))
                {
                    response.Message = Common.MessageWithTitle(Resource.Missing, Resource.EnrollmentTaxIdAlert);
                    return response;
                }
                
                

                ClaimMDApiHelper helper = new ClaimMDApiHelper();
                PayerEnrollResponse data = helper.PayerEnroll(model.EraPayorID, model.EnrollType.ToLower(), model.ProviderTaxID, model.ProviderNPI);

                string EnrollStatus = "0";
                string EnrollLogMessage = "";
                if (data != null && data.result != null)
                {
                    EnrollStatus = data.result.success == null ? "0" : data.result.success;
                    EnrollLogMessage = data.LogMessage;

                    if (data.result.success != null && data.result.success == "1")
                        response.IsSuccess = true;
                    else if(data.result.error != null) { 
                        response.Message = string.Format("{0} : {1}", data.result.error.error_code, data.result.error.error_mesg);
                    }
                }

                GetScalar(StoredProcedure.HC_PayorEnrollment_Update,
                                            new List<SearchValueData>
                                                {

                                                    new SearchValueData {Name = "PayorID", Value = Convert.ToString(model.PayorID)},
                                                    new SearchValueData {Name = "EnrollType", Value = Convert.ToString(model.EnrollType)},
                                                    new SearchValueData {Name = "EnrollStatus", Value = Convert.ToString(EnrollStatus) },
                                                    new SearchValueData {Name = "EnrollLogMessage", Value =Convert.ToString(EnrollLogMessage)},
                                                    new SearchValueData {Name = "EnrollType_ERA", Value =Convert.ToString(Constants.Enroll_ERA)},
                                                    new SearchValueData {Name = "EnrollType_CMS1500", Value =Convert.ToString(Constants.Enroll_CMS1500)}

                                                });



                return response;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }
        public ServiceResponse HC_AddPayorDetail(AddPayorModel addPayorModel, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();
            if (addPayorModel.Payor != null)
            {
                bool isEditMode = addPayorModel.Payor.PayorID > 0;

                string customWhere = string.Format("PayorName='{0}' OR ShortName='{1}' AND EraPayorID='{2}'", Convert.ToString(addPayorModel.Payor.PayorName).Replace("'", "''"), Convert.ToString(addPayorModel.Payor.ShortName).Replace("'", "''"), Convert.ToString(addPayorModel.Payor.EraPayorID).Replace("'", "''"));

                List<SearchValueData> searchModel = new List<SearchValueData> { new SearchValueData { Name = "PayorID", Value = addPayorModel.Payor.PayorID.ToString(), IsNotEqual = true } };
                int payorCount = GetEntityCount<Payor>(isEditMode ? searchModel : null, customWhere);

                if (payorCount > 0)
                {
                    response.IsSuccess = false;
                    response.Message = Resource.PayorAlreadyExists;
                    return response;
                }



                searchModel = new List<SearchValueData> { new SearchValueData { Name = "PayorID", Value = addPayorModel.Payor.PayorID.ToString()} };
                Payor oldPayorDetails = GetEntity<Payor>(searchModel);
               if(oldPayorDetails!=null)
                {
                    if (oldPayorDetails.EraPayorID != addPayorModel.Payor.EraPayorID)
                    {
                        addPayorModel.Payor.ERAEnroll_Status = null;
                        addPayorModel.Payor.ERAEnroll_Log = null;
                        addPayorModel.Payor.CMS1500Enroll_Status = null;
                        addPayorModel.Payor.CMS1500Enroll_Log = null;
                    }
                }


                addPayorModel.Payor.IsBillingActive = true;
                response = SaveObject(addPayorModel.Payor, loggedInUserId);
                int data = 0;
                if (response.IsSuccess)
                {
                    List<SearchValueData> searchparam = new List<SearchValueData> {
                        new SearchValueData { Name = "PayorID", Value = addPayorModel.Payor.PayorID.ToString(), IsEqual = true }
                    };
                    int PS_count = GetEntityCount<PayorEdi837Setting>(searchparam);

                    if (PS_count == 0)
                    {
                        data = (int)GetScalar(StoredProcedure.HC_SavePayorBillingSetting, new List<SearchValueData> { new SearchValueData
                        {Name = "PayorID", Value = Convert.ToString(addPayorModel.Payor.PayorID)}});
                    }
                }
                response.IsSuccess = true;


                response.Data = Crypto.Encrypt(Convert.ToString(addPayorModel.Payor.PayorID));
                response.Message = !isEditMode ? string.Format(Resource.RecordCreatedSuccessfully, Resource.PayorDetail)
                                           : string.Format(Resource.RecordUpdatedSuccessfully, Resource.PayorDetail);
            }
            return response;
        }

        #endregion

        #region  Add Service Code
        /// <summary>
        /// This Is Used For Adding Payor Service Code
        /// </summary>
        public ServiceResponse HC_AddPayorServiceCode(PayorServiceCodeMapping addPayorServiceCodeMappingModel, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                bool isEditMode = addPayorServiceCodeMappingModel.PayorServiceCodeMappingID > 0;
                addPayorServiceCodeMappingModel.PayorID = Convert.ToInt64(Crypto.Decrypt(addPayorServiceCodeMappingModel.EncryptedPayorId));

                List<SearchValueData> searchModel = new List<SearchValueData>
                {
                    new SearchValueData { Name = "PayorServiceCodeMappingID", Value = addPayorServiceCodeMappingModel.PayorServiceCodeMappingID.ToString(), IsNotEqual = true },
                    new SearchValueData{Name = "PayorID",Value =addPayorServiceCodeMappingModel.PayorID.ToString(),IsEqual = true},
                    new SearchValueData{Name = "CareType",Value =addPayorServiceCodeMappingModel.CareType.ToString(),IsEqual = true},
                    new SearchValueData{Name = "ServiceCodeID",Value = addPayorServiceCodeMappingModel.ServiceCodeID.ToString(),IsEqual = true},
                    new SearchValueData{Name = "RevenuCode",Value = addPayorServiceCodeMappingModel.RevenueCode.ToString(),IsEqual = true},
                    new SearchValueData{Name = "StartDate",Value =Convert.ToDateTime(addPayorServiceCodeMappingModel.POSStartDate).ToString(Constants.DbDateFormat),IsEqual = true},
                    new SearchValueData{Name = "EndDate",Value =Convert.ToDateTime(addPayorServiceCodeMappingModel.POSEndDate).ToString(Constants.DbDateFormat),IsEqual = true}
                };
                int check = (int)GetScalar(StoredProcedure.HC_CheckForDuplicatePayorServiceCodeMapping, searchModel);
                if (check > 0)
                {
                    response.Message = Resource.PSCMRecordAlreadyExists;
                    return response;
                }

                PayorServiceCodeMapping payorServiceCodeMapping = isEditMode ? GetEntity<PayorServiceCodeMapping>(addPayorServiceCodeMappingModel.PayorServiceCodeMappingID) : new PayorServiceCodeMapping();
                payorServiceCodeMapping.PayorID = addPayorServiceCodeMappingModel.PayorID;
                payorServiceCodeMapping.ServiceCodeID = addPayorServiceCodeMappingModel.ServiceCodeID;
                payorServiceCodeMapping.POSStartDate = addPayorServiceCodeMappingModel.POSStartDate;
                payorServiceCodeMapping.POSEndDate = addPayorServiceCodeMappingModel.POSEndDate;
                payorServiceCodeMapping.CareType = addPayorServiceCodeMappingModel.CareType;
                payorServiceCodeMapping.RevenueCode = addPayorServiceCodeMappingModel.RevenueCode;
                payorServiceCodeMapping.Rate = addPayorServiceCodeMappingModel.Rate;
                payorServiceCodeMapping.IsDeleted = addPayorServiceCodeMappingModel.IsDeleted;
                payorServiceCodeMapping.UnitType = addPayorServiceCodeMappingModel.UnitType;
                if (payorServiceCodeMapping.UnitType == Convert.ToInt32(EnumUnitType.Time))
                {
                    payorServiceCodeMapping.PerUnitQuantity = addPayorServiceCodeMappingModel.PerUnitQuantity;
                    payorServiceCodeMapping.RoundUpUnit = addPayorServiceCodeMappingModel.RoundUpUnit;
                    payorServiceCodeMapping.MaxUnit = addPayorServiceCodeMappingModel.MaxUnit;
                    payorServiceCodeMapping.DailyUnitLimit = addPayorServiceCodeMappingModel.DailyUnitLimit;
                }
                else
                {
                    payorServiceCodeMapping.PerUnitQuantity = null;
                    payorServiceCodeMapping.RoundUpUnit = null;
                    payorServiceCodeMapping.MaxUnit = null;
                    payorServiceCodeMapping.DailyUnitLimit = null;
                }
                response = SaveObject(payorServiceCodeMapping, loggedInUserId);
                response.IsSuccess = true;
                response.Message = isEditMode ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.ServiceCodeMapping) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.ServiceCodeMapping);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_GetServiceCodeMappingList(string encryptedPayorId, SearchServiceCodeMappingList searchServiceCodeMappingList, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchServiceCodeMappingList.PayorID = Convert.ToInt64(encryptedPayorId);

                HC_SetSearchFilterForServiceCodeMappingListPage(searchServiceCodeMappingList, searchList);

                Page<PayorServiceCodeMappingListPage> listPayorServiceModel = GetEntityPageList<PayorServiceCodeMappingListPage>(StoredProcedure.HC_GetPayorServiceCodeMappingList,
                                                                    searchList, pageSize, pageIndex, sortIndex, sortDirection);
                response.IsSuccess = true;
                response.Data = listPayorServiceModel;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                //response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
                response.Message = e.Message;
            }
            return response;
        }

        public ServiceResponse HC_DeleteServiceCodeMapping(string encryptedPayorId, SearchServiceCodeMappingList searchServiceCodeMappingList, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                searchServiceCodeMappingList.PayorID = Convert.ToInt64(encryptedPayorId);
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                HC_SetSearchFilterForServiceCodeMappingListPage(searchServiceCodeMappingList, searchList);

                if (!string.IsNullOrEmpty(searchServiceCodeMappingList.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchServiceCodeMappingList.ListOfIdsInCSV });

                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

                List<PayorServiceCodeMappingListPage> totalData = GetEntityList<PayorServiceCodeMappingListPage>(StoredProcedure.HC_DeletePayorMappingCode, searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<PayorServiceCodeMappingListPage> listPayorModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = listPayorModel;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.ServiceCode);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }

        private static void HC_SetSearchFilterForServiceCodeMappingListPage(SearchServiceCodeMappingList searchServiceCodeMappingList, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "ServiceCode", Value = Convert.ToString(searchServiceCodeMappingList.ServiceCode) });
            searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchServiceCodeMappingList.PayorID), IsEqual = true });
            searchList.Add(new SearchValueData { Name = "IsDeleted ", Value = Convert.ToString(searchServiceCodeMappingList.IsDeleted) });

            searchList.Add(new SearchValueData { Name = "ModifierID", Value = Convert.ToString(searchServiceCodeMappingList.ModifierID) });

            if (searchServiceCodeMappingList.POSStartDate != null)
                searchList.Add(new SearchValueData { Name = "POSStartDate", Value = Convert.ToDateTime(searchServiceCodeMappingList.POSStartDate).ToString(Constants.DbDateFormat) });
            if (searchServiceCodeMappingList.POSEndDate != null)
                searchList.Add(new SearchValueData { Name = "POSEndDate", Value = Convert.ToDateTime(searchServiceCodeMappingList.POSEndDate).ToString(Constants.DbDateFormat) });

            searchList.Add(new SearchValueData { Name = "CareType", Value = Convert.ToString(searchServiceCodeMappingList.CareType) });
            searchList.Add(new SearchValueData { Name = "RevenueCode", Value = Convert.ToString(searchServiceCodeMappingList.RevenueCode) });
            searchList.Add(new SearchValueData { Name = "Rate", Value = Convert.ToString(searchServiceCodeMappingList.Rate) });

            if (searchServiceCodeMappingList.UnitType.HasValue)
                searchList.Add(new SearchValueData("UnitType", Convert.ToString(searchServiceCodeMappingList.UnitType)));

            if (searchServiceCodeMappingList.PerUnitQuantity.HasValue)
                searchList.Add(new SearchValueData("PerUnitQuantity", Convert.ToString(searchServiceCodeMappingList.PerUnitQuantity)));

            if (searchServiceCodeMappingList.RoundUpUnit.HasValue)
                searchList.Add(new SearchValueData("RoundUpUnit", Convert.ToString(searchServiceCodeMappingList.RoundUpUnit)));

            if (searchServiceCodeMappingList.MaxUnit.HasValue)
                searchList.Add(new SearchValueData("MaxUnit", Convert.ToString(searchServiceCodeMappingList.MaxUnit)));

            if (searchServiceCodeMappingList.DailyUnitLimit.HasValue)
                searchList.Add(new SearchValueData("DailyUnitLimit", Convert.ToString(searchServiceCodeMappingList.DailyUnitLimit)));
        }


        public ServiceResponse HC_AddServiceCode(SerivceCodeModifierModel serivceCodeModifierModel, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchModel = new List<SearchValueData>
                {
                    new SearchValueData{Name = "modifier1", Value = serivceCodeModifierModel.Modifier1 },
                    new SearchValueData{Name = "modifier2",Value =serivceCodeModifierModel.Modifier2},
                    new SearchValueData{Name = "modifier3",Value = serivceCodeModifierModel.Modifier3},
                    new SearchValueData{Name = "modifier4",Value = serivceCodeModifierModel.Modifier4},
                    new SearchValueData{Name = "CurrentDate",Value = Convert.ToString(Common.GetOrgCurrentDateTime())},
                    new SearchValueData{Name = "loggedInUserId",Value =Convert.ToString(loggedInUserId) },
                    new SearchValueData{Name = "SystemID",Value = HttpContext.Current.Request.UserHostAddress}
                };
                List<ModifierCheckModel> modifierCheckModel = GetEntityList<ModifierCheckModel>(StoredProcedure.HC_CheckModifierAvailableOrNot, searchModel);

                bool checkNewModifier = modifierCheckModel.Any(q => q.IsAvailable == false);
                long[] IDs = modifierCheckModel.Select(q => q.ModifierID).ToArray();
                string ModifierIDs = string.Empty;

                if (IDs.Length > 0)
                    ModifierIDs = string.Join(",", IDs);
                else
                    ModifierIDs = null;

                searchModel.Clear();
                searchModel.Add(new SearchValueData { Name = "ServiceCode", Value = serivceCodeModifierModel.ServiceCode });

                if (checkNewModifier)
                {
                    //New Modifier Added in Modifier Table
                    searchModel.Add(new SearchValueData { Name = "ModifierIDCombinations", Value = null });
                    searchModel.Add(new SearchValueData { Name = "ModifierID", Value = ModifierIDs });
                }
                else
                {
                    // All Modifier available in Modifier Table
                    string strModifierIds = string.Empty;

                    if (!string.IsNullOrWhiteSpace(ModifierIDs))
                    {
                        string[] ListmodifierIds = ModifierIDs.Split(',').ToArray();
                        List<string> combinationOfSubAttribute = Common.CombinationGeneraton(ListmodifierIds, 0, ListmodifierIds.Length - 1);
                        for (int i = 0; i < combinationOfSubAttribute.Count; i++)
                        {
                            strModifierIds = strModifierIds + combinationOfSubAttribute[i] + "|";
                        }
                        if (strModifierIds.Length > 0)
                            strModifierIds = strModifierIds.Remove(strModifierIds.Length - 1);
                    }
                    else
                    {
                        strModifierIds = null;
                    }

                    searchModel.Add(new SearchValueData { Name = "ModifierIDCombinations", Value = strModifierIds });
                    searchModel.Add(new SearchValueData { Name = "ModifierID", Value = ModifierIDs });
                }

                searchModel.Add(new SearchValueData { Name = "ServiceCodeID", Value = Convert.ToString(0) });
                searchModel.Add(new SearchValueData { Name = "ServiceName", Value = serivceCodeModifierModel.ServiceCode });
                searchModel.Add(new SearchValueData { Name = "Description", Value = serivceCodeModifierModel.ServiceCode });
                searchModel.Add(new SearchValueData { Name = "IsBillable", Value = Convert.ToString(true) });

                TransactionResult transactionResult = GetEntity<TransactionResult>(StoredProcedure.HC_SaveServiceCode, searchModel);

                if (transactionResult.TransactionResultId == -1)
                {
                    response.IsSuccess = false;
                    response.Message = Resource.ServiceCodeAlreadyExists;
                    return response;
                }
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordCreatedSuccessfully, Resource.ServiceCode);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }




        #endregion

        #region  Payor Listing

        public ServiceResponse HC_SetPayorListPage()
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData> {
                    new SearchValueData { Name = "DDType_PayerGroup", Value = Convert.ToString((int)Common.DDType.PayerGroup)}
                };
                HC_SetPayorListPage setPayorListPage = GetMultipleEntity<HC_SetPayorListPage>(StoredProcedure.HC_SetPayorListPage, searchParam);
                setPayorListPage.DeleteFilter = Common.SetDeleteFilter();
                setPayorListPage.PayorBillingTypeList = Common.SetEDIFileTypeList();
                setPayorListPage.SearchPayorListPage.IsDeleted = 0;
                setPayorListPage.SearchPayorListPage.PayerGroup = -1;
                setPayorListPage.SearchPayorListPage.PayorBillingType = "";
                response.Data = setPayorListPage;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_GetPayorList(SearchPayorListPage searchPayorListPageModel, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();

                if (searchPayorListPageModel != null)
                    HC_SetSearchFilterForPayorListPage(searchPayorListPageModel, searchList);

                Page<ListPayorModel> listPayorModel = GetEntityPageList<ListPayorModel>(StoredProcedure.HC_GetPayorList, searchList, pageSize,
                                                              pageIndex, sortIndex, sortDirection);
                response.Data = listPayorModel;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.Payor), Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_DeletePayor(SearchPayorListPage searchPayorListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                HC_SetSearchFilterForPayorListPage(searchPayorListPage, searchList);

                if (!string.IsNullOrEmpty(searchPayorListPage.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchPayorListPage.ListOfIdsInCSV });

                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

                List<ListPayorModel> totalData = GetEntityList<ListPayorModel>(StoredProcedure.HC_DeletePayor, searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListPayorModel> listPayorModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = listPayorModel;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.Payor);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }

        private static void HC_SetSearchFilterForPayorListPage(SearchPayorListPage searchPayorListPage, List<SearchValueData> searchList)
        {
            if (!string.IsNullOrEmpty(searchPayorListPage.PayorName))
                searchList.Add(new SearchValueData { Name = "PayorName", Value = Convert.ToString(searchPayorListPage.PayorName) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchPayorListPage.IsDeleted) });

            if (!string.IsNullOrEmpty(searchPayorListPage.ShortName))
                searchList.Add(new SearchValueData { Name = "ShortName", Value = Convert.ToString(searchPayorListPage.ShortName) });

            if (!string.IsNullOrEmpty(searchPayorListPage.Address))
                searchList.Add(new SearchValueData { Name = "Address", Value = Convert.ToString(searchPayorListPage.Address) });

            if (!string.IsNullOrEmpty(searchPayorListPage.AgencyNPID))
                searchList.Add(new SearchValueData { Name = "AgencyNPID", Value = Convert.ToString(searchPayorListPage.AgencyNPID) });

            if (!string.IsNullOrEmpty(searchPayorListPage.PayorBillingType))
                searchList.Add(new SearchValueData { Name = "PayorBillingType", Value = Convert.ToString(searchPayorListPage.PayorBillingType) });

            searchList.Add(new SearchValueData { Name = "PayerGroup", Value = Convert.ToString(searchPayorListPage.PayerGroup) });

        }

        #endregion


        public List<ServiceCodes> HC_GetServiceCodeList(string searchText, int pageSize = 10)
        {
            List<ServiceCodes> contactlist = GetEntityList<ServiceCodes>(StoredProcedure.HC_GetPayorServiceCodeListForAutoCompleter,
                                            new List<SearchValueData>
                                                {
                                                    new SearchValueData {Name = "SearchText", Value = searchText},
                                                    new SearchValueData {Name = "PageSize", Value = pageSize.ToString()}
                                                });
            return contactlist;
        }
        #endregion

        #region Save Billing Setting for 837 File
        public ServiceResponse HC_GetPayorBillingSetting(long PayorID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData> {
                    new SearchValueData { Name = "PayorID", Value = Convert.ToString(PayorID)}
                };
                PayorEdi837Setting model = GetEntity<PayorEdi837Setting>(StoredProcedure.HC_GetPayorBillingSetting, searchParam);

                if (model == null)
                    model = new PayorEdi837Setting();

                response.Data = model;
                response.IsSuccess = true;
                return response;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_SavePayorBillingSetting(PayorEdi837Setting payorEdi837Setting)
        {
            var response = new ServiceResponse();
            try
            {

                List<SearchValueData> searchModel = new List<SearchValueData>
                {
                    new SearchValueData { Name = "PayorEdi837SettingId", Value = Convert.ToString(payorEdi837Setting.PayorEdi837SettingId)},
                    new SearchValueData { Name = "PayorID", Value = Convert.ToString(payorEdi837Setting.PayorID)},
                    new SearchValueData { Name = "ISA06_InterchangeSenderId", Value = payorEdi837Setting.ISA06_InterchangeSenderId},
                    new SearchValueData { Name = "ISA08_InterchangeReceiverId", Value = payorEdi837Setting.ISA08_InterchangeReceiverId},
                    new SearchValueData { Name = "ISA11_RepetitionSeparator", Value = payorEdi837Setting.ISA11_RepetitionSeparator},
                    new SearchValueData { Name = "ISA16_ComponentElementSeparator", Value = payorEdi837Setting.ISA16_ComponentElementSeparator},

                    new SearchValueData { Name = "CMS1500HourRounding", Value = Convert.ToString(payorEdi837Setting.CMS1500HourRounding)},
                    new SearchValueData { Name = "UB04HourRounding", Value = Convert.ToString(payorEdi837Setting.UB04HourRounding)},
                    //new SearchValueData { Name = "BillingProvider_PRV01_ProviderCode", Value = payorEdi837Setting.BillingProvider_PRV01_ProviderCode},
                    //new SearchValueData { Name = "BillingProvider_PRV02_ReferenceIdentificationQualifier", Value = payorEdi837Setting.BillingProvider_PRV02_ReferenceIdentificationQualifier},
                    new SearchValueData { Name = "BillingProvider_PRV03_ProviderTaxonomyCode", Value = payorEdi837Setting.BillingProvider_PRV03_ProviderTaxonomyCode}
                };

                var data = (int)GetScalar(StoredProcedure.HC_SavePayorBillingSetting, searchModel);

                if (data == 1)
                {
                    response.IsSuccess = true;
                    response.Message = (payorEdi837Setting.PayorEdi837SettingId > 0) ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.BillingSettings) :
                        string.Format(Resource.RecordCreatedSuccessfully, Resource.BillingSettings);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = Resource.ExceptionMessage;
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, e.Message);
            }
            return response;
        }

        public ServiceResponse HC_GetAllBillingSetting(BillingSettingModel billingSettingModel)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData> {
                    new SearchValueData { Name = "PayorID", Value = Convert.ToString(billingSettingModel.PayorID)},
                    new SearchValueData { Name = "searchtext", Value =billingSettingModel.searchtext}
                };
                List<BillingSettingModel> model = GetEntityList<BillingSettingModel>(StoredProcedure.HC_GetAllEDI837Settings, searchParam);

                response.Data = model;
                response.IsSuccess = true;
                return response;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_SaveBillingSetting(BillingSettingModel billingSettingModel)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData> {
                    new SearchValueData { Name = "PayorID", Value = Convert.ToString(billingSettingModel.PayorID)},
                    new SearchValueData { Name = "Key", Value =billingSettingModel.Key},
                    new SearchValueData { Name = "Val", Value =billingSettingModel.Val}
                };
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.HC_SaveEDI837Setting, searchParam);

                if (result.TransactionResultId == 1)
                {
                    response.IsSuccess = true;
                    response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.BillingSettings);
                    return response;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = result.ErrorMessage;
                }
                return response;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }
        #endregion

    }
}
