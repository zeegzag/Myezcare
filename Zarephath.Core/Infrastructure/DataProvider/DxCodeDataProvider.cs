using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class DxCodeDataProvider : BaseDataProvider, IDxCodeDataProvider
    {
        #region Add DX Code

        public ServiceResponse SetAddDxCodePage(long dxCodeId)
        {
            ServiceResponse response = new ServiceResponse();

            AddDxCodeModel addDxCodeModel = new AddDxCodeModel();
            addDxCodeModel.DxCodeTypes = GetEntityList<DxCodeType>(null, "", "DxCodeTypeOrder", "ASC");
            if (dxCodeId > 0)
            {
                List<SearchValueData> searchDxCodeParam = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "DXCodeID", Value = Convert.ToString(dxCodeId)},
                        //new SearchValueData {Name = "IsDeleted", Value = "0"},
                    };
                DXCode dxCode = GetEntity<DXCode>(searchDxCodeParam);

                if (dxCode != null)
                {
                    addDxCodeModel.DxCode = dxCode;
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
                response.IsSuccess = true;
            }
            response.Data = addDxCodeModel;
            return response;
        }

        public ServiceResponse AddDxCode(AddDxCodeModel addDxCodeModel, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                bool isEditMode = addDxCodeModel.DxCode.DXCodeID > 0;

                List<SearchValueData> searchModel = new List<SearchValueData>
                {
                    new SearchValueData { Name = "DXCodeID", Value =Convert.ToString(addDxCodeModel.DxCode.DXCodeID), IsNotEqual = true },
                    new SearchValueData { Name = "DXCodeName", Value = addDxCodeModel.DxCode.DXCodeName, IsNotEqual = true },
                    //new SearchValueData{Name = "EffectiveFrom",Value =Convert.ToDateTime(addDxCodeModel.DxCode.EffectiveFrom).ToString(Constants.DbDateFormat),IsEqual = true},
                    //new SearchValueData{Name = "EffectiveTo",Value =Convert.ToDateTime(addDxCodeModel.DxCode.EffectiveTo).ToString(Constants.DbDateFormat),IsEqual = true}
                };

                if ((int)GetScalar(StoredProcedure.CheckForDuplicateDxCodeMapping, searchModel) > 0)
                {
                    response.Message = Resource.DxCodeRecordAlreadyExists;
                    return response;
                }

                //DXCode dxCodeMapping = isEditMode ? GetEntity<DXCode>(addDxCodeModel.DxCode.DXCodeID) : new DXCode();

                //dxCodeMapping.DXCodeName = addDxCodeModel.DxCode.DXCodeName;
                //dxCodeMapping.DXCodeID = addDxCodeModel.DxCode.DXCodeID;
                //dxCodeMapping.Description = addDxCodeModel.DxCode.Description;
                //dxCodeMapping.EffectiveFrom = addDxCodeModel.DxCode.EffectiveFrom;
                //dxCodeMapping.EffectiveTo = addDxCodeModel.DxCode.EffectiveTo;
                //response = SaveObject(dxCodeMapping, loggedInUserId); 

                GetEntity<DXCode>("SaveDxCode", new List<SearchValueData>
                {
                    new SearchValueData {Name = "DXCodeID",Value = Convert.ToString(addDxCodeModel.DxCode.DXCodeID)},
                    new SearchValueData {Name = "DXCodeName",Value =addDxCodeModel.DxCode.DXCodeName },
                    new SearchValueData {Name = "DxCodeType",Value =addDxCodeModel.DxCode.DxCodeType },
                    new SearchValueData {Name = "DXCodeWithoutDot",Value =addDxCodeModel.DxCode.DXCodeWithoutDot },
                    new SearchValueData {Name = "Description",Value =addDxCodeModel.DxCode.Description },
                    new SearchValueData {Name = "EffectiveFrom",Value = addDxCodeModel.DxCode.EffectiveFrom.ToString(Constants.DbDateFormat) },
                    new SearchValueData {Name = "EffectiveTo",Value = addDxCodeModel.DxCode.EffectiveTo.ToString(Constants.DbDateFormat) },
                    new SearchValueData {Name = "loggedInUserId",Value = Convert.ToString(loggedInUserId)},
                    new SearchValueData {Name = "IsEditMode",Value = Convert.ToString(isEditMode)},
                    new SearchValueData {Name = "SystemID",Value = HttpContext.Current.Request.UserHostAddress}
                });

                response.IsSuccess = true;
                response.Message = isEditMode ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.DXCode) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.DXCode);

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        #endregion Add DX Code

        #region DX Code List

        public ServiceResponse SetAddDxCodeListPage()
        {
            ServiceResponse response = new ServiceResponse();
            var setDxCodeListPage = new SetDxCodeListPage
            {
                DeleteFilter = Common.SetDeleteFilter(),
                SearchDxCodeListPage = { IsDeleted = 0 }
            };
            response.Data = setDxCodeListPage;
            return response;
        }

        public ServiceResponse GetDxCodeList(SearchDxCodeListPage searchDxCodeListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchDxCodeListPage != null)
                SetSearchFilterForDxCodeList(searchDxCodeListPage, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListDxCodeModel> totalData = GetEntityList<ListDxCodeModel>(StoredProcedure.GetDxCodeList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListDxCodeModel> caseManagerList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = caseManagerList;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForDxCodeList(SearchDxCodeListPage searchDxCodeListPage, List<SearchValueData> searchList)
        {
            if (!string.IsNullOrEmpty(searchDxCodeListPage.DXCodeName))
                searchList.Add(new SearchValueData { Name = "DXCodeName", Value = Convert.ToString(searchDxCodeListPage.DXCodeName) });

            if (!string.IsNullOrEmpty(searchDxCodeListPage.DXCodeWithoutDot))
                searchList.Add(new SearchValueData { Name = "DXCodeWithoutDot", Value = Convert.ToString(searchDxCodeListPage.DXCodeWithoutDot) });

            if (!string.IsNullOrEmpty(searchDxCodeListPage.DXCodeShortName))
                searchList.Add(new SearchValueData { Name = "DXCodeShortName", Value = Convert.ToString(searchDxCodeListPage.DXCodeShortName) });

            if (!string.IsNullOrEmpty(searchDxCodeListPage.Description))
                searchList.Add(new SearchValueData { Name = "Description", Value = Convert.ToString(searchDxCodeListPage.Description) });

            if (!string.IsNullOrEmpty(Convert.ToString(searchDxCodeListPage.EffectiveFrom)))
                searchList.Add(new SearchValueData { Name = "EffectiveFrom", Value = (searchDxCodeListPage.EffectiveFrom != null) ? searchDxCodeListPage.EffectiveFrom.Value.ToString(Constants.DbDateFormat) : null });

            if (!string.IsNullOrEmpty(Convert.ToString(searchDxCodeListPage.EffectiveTo)))
                searchList.Add(new SearchValueData { Name = "EffectiveTo", Value = (searchDxCodeListPage.EffectiveTo != null) ? searchDxCodeListPage.EffectiveTo.Value.ToString(Constants.DbDateFormat) : null });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchDxCodeListPage.IsDeleted) });
        }

        public ServiceResponse DeleteDxCode(SearchDxCodeListPage searchDxCodeListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForDxCodeList(searchDxCodeListPage, searchList);

            if (!string.IsNullOrEmpty(searchDxCodeListPage.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchDxCodeListPage.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });

            List<ListDxCodeModel> totalData = GetEntityList<ListDxCodeModel>(StoredProcedure.DeleteDxCode, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListDxCodeModel> dxCodeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = dxCodeList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.DXCode);
            return response;
        }

        #endregion DX Code List

        #region HomeCare Related Code
        
            #region Add DX Code

            public ServiceResponse HC_SetAddDxCodePage(long dxCodeId)
            {
                ServiceResponse response = new ServiceResponse();

                AddDxCodeModel addDxCodeModel = new AddDxCodeModel();
                addDxCodeModel.DxCodeTypes = GetEntityList<DxCodeType>(null, "", "DxCodeTypeOrder", "ASC");
                if (dxCodeId > 0)
                {
                    List<SearchValueData> searchDxCodeParam = new List<SearchValueData>
                        {
                            new SearchValueData {Name = "DXCodeID", Value = Convert.ToString(dxCodeId)},
                            //new SearchValueData {Name = "IsDeleted", Value = "0"},
                        };
                    DXCode dxCode = GetEntity<DXCode>(searchDxCodeParam);

                    if (dxCode != null)
                    {
                        addDxCodeModel.DxCode = dxCode;
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
                    response.IsSuccess = true;
                }
                response.Data = addDxCodeModel;
                return response;
            }

            public ServiceResponse HC_AddDxCode(AddDxCodeModel addDxCodeModel, long loggedInUserId)
            {
                var response = new ServiceResponse();
                try
                {
                    bool isEditMode = addDxCodeModel.DxCode.DXCodeID > 0;

                    List<SearchValueData> searchModel = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "DXCodeID", Value =Convert.ToString(addDxCodeModel.DxCode.DXCodeID), IsNotEqual = true },
                        new SearchValueData { Name = "DXCodeName", Value = addDxCodeModel.DxCode.DXCodeName, IsNotEqual = true },
                        //new SearchValueData{Name = "EffectiveFrom",Value =Convert.ToDateTime(addDxCodeModel.DxCode.EffectiveFrom).ToString(Constants.DbDateFormat),IsEqual = true},
                        //new SearchValueData{Name = "EffectiveTo",Value =Convert.ToDateTime(addDxCodeModel.DxCode.EffectiveTo).ToString(Constants.DbDateFormat),IsEqual = true}
                    };

                    if ((int)GetScalar(StoredProcedure.HC_CheckForDuplicateDxCodeMapping, searchModel) > 0)
                    {
                        response.Message = Resource.DxCodeRecordAlreadyExists;
                        return response;
                    }

                    //DXCode dxCodeMapping = isEditMode ? GetEntity<DXCode>(addDxCodeModel.DxCode.DXCodeID) : new DXCode();

                    //dxCodeMapping.DXCodeName = addDxCodeModel.DxCode.DXCodeName;
                    //dxCodeMapping.DXCodeID = addDxCodeModel.DxCode.DXCodeID;
                    //dxCodeMapping.Description = addDxCodeModel.DxCode.Description;
                    //dxCodeMapping.EffectiveFrom = addDxCodeModel.DxCode.EffectiveFrom;
                    //dxCodeMapping.EffectiveTo = addDxCodeModel.DxCode.EffectiveTo;
                    //response = SaveObject(dxCodeMapping, loggedInUserId); 

                    GetEntity<DXCode>(StoredProcedure.HC_SaveDxCode, new List<SearchValueData>
                    {
                        new SearchValueData {Name = "DXCodeID",Value = Convert.ToString(addDxCodeModel.DxCode.DXCodeID)},
                        new SearchValueData {Name = "DXCodeName",Value =addDxCodeModel.DxCode.DXCodeName },
                        new SearchValueData {Name = "DxCodeType",Value =addDxCodeModel.DxCode.DxCodeType },
                        new SearchValueData {Name = "DXCodeWithoutDot",Value =addDxCodeModel.DxCode.DXCodeWithoutDot },
                        new SearchValueData {Name = "Description",Value =addDxCodeModel.DxCode.Description },
                        new SearchValueData {Name = "EffectiveFrom",Value = addDxCodeModel.DxCode.EffectiveFrom.ToString(Constants.DbDateFormat) },
                        new SearchValueData {Name = "EffectiveTo",Value = addDxCodeModel.DxCode.EffectiveTo.ToString(Constants.DbDateFormat) },
                        new SearchValueData {Name = "loggedInUserId",Value = Convert.ToString(loggedInUserId)},
                        new SearchValueData {Name = "IsEditMode",Value = Convert.ToString(isEditMode)},
                        new SearchValueData {Name = "SystemID",Value = HttpContext.Current.Request.UserHostAddress}
                    });

                    response.IsSuccess = true;
                    response.Message = isEditMode ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.DXCode) :
                        string.Format(Resource.RecordCreatedSuccessfully, Resource.DXCode);

                }
                catch (Exception)
                {
                    response.IsSuccess = false;
                    response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
                }
                return response;
            }

            #endregion Add DX Code

            #region DX Code List

            public ServiceResponse HC_SetAddDxCodeListPage()
            {
                ServiceResponse response = new ServiceResponse();
                var setDxCodeListPage = new SetDxCodeListPage
                {
                    DeleteFilter = Common.SetDeleteFilter(),
                    SearchDxCodeListPage = { IsDeleted = 0 }
                };
                response.Data = setDxCodeListPage;
                return response;
            }

            public ServiceResponse HC_GetDxCodeList(SearchDxCodeListPage searchDxCodeListPage, int pageIndex, int pageSize,
                string sortIndex, string sortDirection)
            {
                ServiceResponse response = new ServiceResponse();

                List<SearchValueData> searchList = new List<SearchValueData>();

                if (searchDxCodeListPage != null)
                    HC_SetSearchFilterForDxCodeList(searchDxCodeListPage, searchList);

                searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

                List<ListDxCodeModel> totalData = GetEntityList<ListDxCodeModel>(StoredProcedure.HC_GetDxCodeList, searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListDxCodeModel> caseManagerList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = caseManagerList;
                response.IsSuccess = true;
                return response;
            }

            private static void HC_SetSearchFilterForDxCodeList(SearchDxCodeListPage searchDxCodeListPage, List<SearchValueData> searchList)
            {
                if (!string.IsNullOrEmpty(searchDxCodeListPage.DXCodeName))
                    searchList.Add(new SearchValueData { Name = "DXCodeName", Value = Convert.ToString(searchDxCodeListPage.DXCodeName) });

                if (!string.IsNullOrEmpty(searchDxCodeListPage.DXCodeWithoutDot))
                    searchList.Add(new SearchValueData { Name = "DXCodeWithoutDot", Value = Convert.ToString(searchDxCodeListPage.DXCodeWithoutDot) });

                if (!string.IsNullOrEmpty(searchDxCodeListPage.DXCodeShortName))
                    searchList.Add(new SearchValueData { Name = "DXCodeShortName", Value = Convert.ToString(searchDxCodeListPage.DXCodeShortName) });

                if (!string.IsNullOrEmpty(searchDxCodeListPage.Description))
                    searchList.Add(new SearchValueData { Name = "Description", Value = Convert.ToString(searchDxCodeListPage.Description) });

                if (!string.IsNullOrEmpty(Convert.ToString(searchDxCodeListPage.EffectiveFrom)))
                    searchList.Add(new SearchValueData { Name = "EffectiveFrom", Value = (searchDxCodeListPage.EffectiveFrom != null) ? searchDxCodeListPage.EffectiveFrom.Value.ToString(Constants.DbDateFormat) : null });

                if (!string.IsNullOrEmpty(Convert.ToString(searchDxCodeListPage.EffectiveTo)))
                    searchList.Add(new SearchValueData { Name = "EffectiveTo", Value = (searchDxCodeListPage.EffectiveTo != null) ? searchDxCodeListPage.EffectiveTo.Value.ToString(Constants.DbDateFormat) : null });

                searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchDxCodeListPage.IsDeleted) });
            }

            public ServiceResponse HC_DeleteDxCode(SearchDxCodeListPage searchDxCodeListPage, int pageIndex, int pageSize,
                string sortIndex, string sortDirection, long loggedInUserId)
            {
                var response = new ServiceResponse();
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

                SetSearchFilterForDxCodeList(searchDxCodeListPage, searchList);

                if (!string.IsNullOrEmpty(searchDxCodeListPage.ListOfIdsInCsv))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchDxCodeListPage.ListOfIdsInCsv });

                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

                searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });

                List<ListDxCodeModel> totalData = GetEntityList<ListDxCodeModel>(StoredProcedure.HC_DeleteDxCode, searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListDxCodeModel> dxCodeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = dxCodeList;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.DXCode);
                return response;
            }

            #endregion DX Code List

        #endregion
    }
}
