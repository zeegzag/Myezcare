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


namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class ServiceCoceDataProvider : BaseDataProvider, IServiceCodeDataProvider
    {
        #region Add Service Code Detail

        public ServiceResponse SetServiceCode(long serviceCodeId)
        {
            var response = new ServiceResponse();
            try
            {
                bool isEditMode = serviceCodeId > 0;
                List<SearchValueData> searchParam = new List<SearchValueData> { new SearchValueData { Name = "ServiceCodeID", Value = serviceCodeId.ToString() } };

                AddServiceCodeModel model = GetMultipleEntity<AddServiceCodeModel>(StoredProcedure.SetAddServiceCodePage, searchParam);

                if (isEditMode && model.ServiceCodes == null)
                {
                    response.ErrorCode = Constants.ErrorCode_NotFound;
                    return response;
                }

                if (model.ServiceCodes == null)
                    model.ServiceCodes = new ServiceCodes();

                model.UnitTypeList = Common.SetUnitTypeFilter();


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

        public ServiceResponse AddServiceCode(AddServiceCodeModel addServiceCodeModel, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();

            if (addServiceCodeModel.ServiceCodes != null)
            {
                ServiceCodes serviceCode = addServiceCodeModel.ServiceCodes;
                var isEditMode = serviceCode.ServiceCodeID > 0;
                string customWhere = string.Format("ServiceCodeType='{0}' AND  ServiceCode='{1}' AND  ", Convert.ToString(serviceCode.ServiceCodeType).Replace("'", "''"), Convert.ToString(serviceCode.ServiceCode).Replace("'", "''"));

                if (serviceCode.ModifierID != null)
                    customWhere = customWhere + string.Format("ModifierID='{0}'", Convert.ToString(serviceCode.ModifierID));
                else customWhere = customWhere + string.Format("ModifierID IS NULL ");

                List<SearchValueData> searchModel = new List<SearchValueData> { new SearchValueData { Name = "ServiceCodeID", Value = addServiceCodeModel.ServiceCodes.ServiceCodeID.ToString(), IsNotEqual = true } };
                int serviceCodeCount = GetEntityCount<ServiceCodes>(isEditMode ? searchModel : null, customWhere);

                if (serviceCodeCount > 0)
                {
                    response.IsSuccess = false;
                    response.Message = Resource.ServiceCodeAlreadyExists;
                    return response;
                }
                SaveObject(serviceCode, loggedInUserId);
                response.IsSuccess = true;
                response.Message = !isEditMode ? string.Format(Resource.RecordCreatedSuccessfully, Resource.ServiceCode)
                                           : string.Format(Resource.RecordUpdatedSuccessfully, Resource.ServiceCode);
            }
            return response;
        }

        #endregion

        #region  Service Code Listing

        public ServiceResponse SetServiceCodeList()
        {
            var response = new ServiceResponse();
            try
            {
                SetServiceCodeListPage setPayorListPage = GetMultipleEntity<SetServiceCodeListPage>(StoredProcedure.SetServiceCodeListPage);
                setPayorListPage.DeleteFilter = Common.SetDeleteFilter();
                setPayorListPage.SearchServiceCodeListPage.IsDeleted = 0;
                setPayorListPage.SearchServiceCodeListPage.IsBillable = -1;
                setPayorListPage.SearchServiceCodeListPage.HasGroupOption = -1;
                response.Data = setPayorListPage;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse GetServiceCodeList(SearchServiceCodeListPage searchServiceCodeListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();

                if (searchServiceCodeListPage != null)
                    SetSearchFilterForPayorListPage(searchServiceCodeListPage, searchList);

                Page<ListServiceCodeModel> listModel = GetEntityPageList<ListServiceCodeModel>(StoredProcedure.GetServiceCodeList, searchList, pageSize,
                                                              pageIndex, sortIndex, sortDirection);
                response.Data = listModel;
                response.IsSuccess = true;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.Payor), Resource.ExceptionMessage);
            }
            return response;
        }

        private static void SetSearchFilterForPayorListPage(SearchServiceCodeListPage searchModel, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "ServiceCode", Value = Convert.ToString(searchModel.ServiceCode) });
            searchList.Add(new SearchValueData { Name = "ModifierID", Value = Convert.ToString(searchModel.ModifierID) });
            searchList.Add(new SearchValueData { Name = "ServiceName", Value = Convert.ToString(searchModel.ServiceName) });
            searchList.Add(new SearchValueData { Name = "ServiceCodeType", Value = Convert.ToString(searchModel.ServiceCodeType) });
            searchList.Add(new SearchValueData { Name = "UnitType", Value = Convert.ToString(searchModel.UnitType) });
            searchList.Add(new SearchValueData { Name = "IsBillable", Value = Convert.ToString(searchModel.IsBillable) });
            searchList.Add(new SearchValueData { Name = "HasGroupOption", Value = Convert.ToString(searchModel.HasGroupOption) });
            searchList.Add(new SearchValueData { Name = "ServiceCodeStartDate", Value = Convert.ToString(searchModel.ServiceCodeStartDate) });
            searchList.Add(new SearchValueData { Name = "ServiceCodeEndDate", Value = Convert.ToString(searchModel.ServiceCodeEndDate) });
            searchList.Add(new SearchValueData { Name = "AccountCode", Value = Convert.ToString(searchModel.AccountCode) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchModel.IsDeleted) });

        }

        #endregion

        #region Home Care Related Code

        public ServiceResponse HC_SetServiceCode(long serviceCodeId)
        {
            var response = new ServiceResponse();
            try
            {
                bool isEditMode = serviceCodeId > 0;
                List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData { Name = "ServiceCodeID", Value = serviceCodeId.ToString() },
                    new SearchValueData { Name = "DDType_CareType", Value =Convert.ToString((int)Common.DDType.CareType)},
                    new SearchValueData { Name = "DDType_RevenueCode", Value =Convert.ToString((int)Common.DDType.RevenueCode)}
                };

                HC_AddServiceCodeModel model = GetMultipleEntity<HC_AddServiceCodeModel>(StoredProcedure.HC_SetAddServiceCodePage, searchParam);

                if (isEditMode && model.ServiceCodes == null)
                {
                    response.ErrorCode = Constants.ErrorCode_NotFound;
                    return response;
                }

                if (model.ServiceCodes == null)
                    model.ServiceCodes = new ServiceCodes();

                model.DeleteFilter = Common.SetDeleteFilter();
                model.ModifierModel = new Modifier();
                model.UnitTypeList = Common.SetUnitTypeFilter();

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

        public ServiceResponse HC_AddServiceCode(AddServiceCodeModel addServiceCodeModel, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();

            if (addServiceCodeModel.ServiceCodes != null)
            {
                ServiceCodes serviceCode = addServiceCodeModel.ServiceCodes;
                var isEditMode = serviceCode.ServiceCodeID > 0;

                string strModifierIds = string.Empty;

                if (!string.IsNullOrWhiteSpace(addServiceCodeModel.ServiceCodes.ModifierID))
                {
                    string[] ListmodifierIds = addServiceCodeModel.ServiceCodes.ModifierID.Split(',').ToArray();
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

                List<SearchValueData> searchModel = new List<SearchValueData> {
                    new SearchValueData { Name = "ServiceCodeID", Value = Convert.ToString(addServiceCodeModel.ServiceCodes.ServiceCodeID)  },
                    new SearchValueData { Name = "ServiceCode", Value = addServiceCodeModel.ServiceCodes.ServiceCode },
                    new SearchValueData { Name = "ModifierID", Value = addServiceCodeModel.ServiceCodes.ModifierID },
                    new SearchValueData { Name = "ModifierIDCombinations", Value = strModifierIds },
                    new SearchValueData { Name = "ServiceName", Value =  addServiceCodeModel.ServiceCodes.ServiceName },
                    new SearchValueData { Name = "Description", Value =  addServiceCodeModel.ServiceCodes.Description },
                     new SearchValueData { Name = "AccountCode", Value =  addServiceCodeModel.ServiceCodes.AccountCode },
                    new SearchValueData { Name = "IsBillable", Value = Convert.ToString(addServiceCodeModel.ServiceCodes.IsBillable) },
                    new SearchValueData { Name = "VisitTypeId", Value = Convert.ToString(addServiceCodeModel.ServiceCodes.VisitTypeId) },

                    new SearchValueData { Name = "loggedInUserId", Value = Convert.ToString(loggedInUserId) },
                    new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() }
            };

                TransactionResult transactionResult = GetEntity<TransactionResult>(StoredProcedure.HC_SaveServiceCode, searchModel);

                if (transactionResult.TransactionResultId == -1)
                {
                    response.IsSuccess = false;
                    response.Message = Resource.ServiceCodeAlreadyExists;
                    return response;
                }
                response.IsSuccess = true;
                response.Message = !isEditMode ? string.Format(Resource.RecordCreatedSuccessfully, Resource.ServiceCode)
                                           : string.Format(Resource.RecordUpdatedSuccessfully, Resource.ServiceCode);
            }
            return response;
        }

        public ServiceResponse HC_GetServiceCodeList(SearchServiceCodeListPage searchServiceCodeListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();

                searchList.Add(new SearchValueData { Name = "ServiceCode", Value = Convert.ToString(searchServiceCodeListPage.ServiceCode) });
                searchList.Add(new SearchValueData { Name = "ModifierName", Value = Convert.ToString(searchServiceCodeListPage.ModifierName) });
                searchList.Add(new SearchValueData { Name = "ServiceName", Value = Convert.ToString(searchServiceCodeListPage.ServiceName) });
                searchList.Add(new SearchValueData { Name = "AccountCode", Value = Convert.ToString(searchServiceCodeListPage.AccountCode) });
                searchList.Add(new SearchValueData { Name = "IsBillable", Value = Convert.ToString(searchServiceCodeListPage.IsBillable) });

                Page<ListServiceCodeModel> listModel = GetEntityPageList<ListServiceCodeModel>(StoredProcedure.HC_GetServiceCodeList, searchList, pageSize,
                                                              pageIndex, sortIndex, sortDirection);
                response.Data = listModel;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format(Resource.ListFailed, Resource.ServiceCode), Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_SetServiceCodeList()
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData> {
                    new SearchValueData { Name = "DDType_CareType", Value =Convert.ToString((int)Common.DDType.CareType)}
                };
                HC_SetServiceCodeListPage setPayorListPage = GetMultipleEntity<HC_SetServiceCodeListPage>(StoredProcedure.HC_SetServiceCodeListPage, searchParam);
                setPayorListPage.SearchServiceCodeListPage.IsDeleted = 0;
                setPayorListPage.SearchServiceCodeListPage.ModifierID = null;
                setPayorListPage.SearchServiceCodeListPage.UnitType = 0;
                setPayorListPage.SearchServiceCodeListPage.IsBillable = -1;
                setPayorListPage.SearchServiceCodeListPage.CareType = -1;
                response.Data = setPayorListPage;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_GetModifierList(SearchModifierModel searchModifierModel)
        {
            var response = new ServiceResponse();
            try
            {
                List<Modifier> modifierlist = GetEntityList<Modifier>(StoredProcedure.HC_GetModifierList, new List<SearchValueData>
                {
                    new SearchValueData {Name = "ModifierCode",Value=searchModifierModel.ModifierCode },
                    new SearchValueData {Name = "ModifierName",Value=searchModifierModel.ModifierName },
                    new SearchValueData {Name = "IsDeleted",Value=Convert.ToString(searchModifierModel.IsDeleted) }
                });
                response.IsSuccess = true;
                response.Data = modifierlist;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse HC_GetModifierByServiceCode(long serviceCodeID)
        {
            var response = new ServiceResponse();
            try
            {
                List<Modifier> modifierlist = GetEntityList<Modifier>(StoredProcedure.HC_GetModifierByServiceCode, new List<SearchValueData>
                {
                    new SearchValueData {Name = "ServiceCodeID",Value= Convert.ToString(serviceCodeID)}
                });
                response.IsSuccess = true;
                response.Data = modifierlist;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse HC_SaveModifier(Modifier modifier, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                int data = (int)GetScalar(StoredProcedure.HC_SaveModifier, new List<SearchValueData>
                {
                    new SearchValueData {Name = "ModifierID",Value = Convert.ToString(modifier.ModifierID)},
                    new SearchValueData {Name = "ModifierCode",Value=modifier.ModifierCode },
                    new SearchValueData {Name = "ModifierName",Value=modifier.ModifierName },
                    new SearchValueData {Name = "CreatedBy", Value =Convert.ToString(loggedInUserId)},
                    new SearchValueData {Name = "UpdatedBy",Value = Convert.ToString(loggedInUserId)},
                    new SearchValueData {Name = "SystemID",Value = HttpContext.Current.Request.UserHostAddress}
                });

                if (data == -1)
                {
                    response.Message = Resource.ModifierAlreadyExists;
                    return response;
                }
                response.IsSuccess = true;
                response.Message = modifier.ModifierID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Modifier) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.Modifier);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse HC_DeleteModifier(SearchModifierModel searchModifierModel)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();

                if (!string.IsNullOrEmpty(searchModifierModel.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchModifierModel.ListOfIdsInCSV });

                int data = (int)GetScalar(StoredProcedure.HC_DeleteModifier, searchList);
                if (data > 0)
                {
                    response.IsSuccess = true;
                    response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Modifier);
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        #endregion

        #region Delete Service Code 
        public ServiceResponse DeleteServiceCode(SearchServiceCodeListPage searchServiceCodeListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForServiceCodeList(searchServiceCodeListPage, searchList);

            if (!string.IsNullOrEmpty(searchServiceCodeListPage.ListOfIdsInCSV))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchServiceCodeListPage.ListOfIdsInCSV });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });

            List<ListServiceCodeModel> totalData = GetEntityList<ListServiceCodeModel>(StoredProcedure.DeleteServiceCode, searchList);

            int count = 0;
            //if (totalData != null && totalData.Count > 0)
            //    count = totalData.First().Count;

            Page<ListServiceCodeModel> ServiceCodeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = ServiceCodeList;
            response.IsSuccess = true;
            //response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.ServiceCode);

            if (totalData.Count > 0)
            {
                response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.ServiceCode);
            }
            else
            {
                response.Message = string.Format("{0} is associate with some Payor. You can not delete this {0}.", Resource.ServiceCode);
            }
            return response;
        }

        private static void SetSearchFilterForServiceCodeList(SearchServiceCodeListPage searchServiceCodeListPage, List<SearchValueData> searchList)
        {
            if (!string.IsNullOrEmpty(searchServiceCodeListPage.ServiceCode))
                searchList.Add(new SearchValueData { Name = "ServiceCode", Value = Convert.ToString(searchServiceCodeListPage.ServiceCode) });
            if (!string.IsNullOrEmpty(searchServiceCodeListPage.ModifierName))
                searchList.Add(new SearchValueData { Name = "ModifierName", Value = Convert.ToString(searchServiceCodeListPage.ModifierName) });
            if (!string.IsNullOrEmpty(searchServiceCodeListPage.ServiceName))
                searchList.Add(new SearchValueData { Name = "ServiceName", Value = Convert.ToString(searchServiceCodeListPage.ServiceName) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchServiceCodeListPage.IsDeleted) });
        }
        #endregion
    }
}
