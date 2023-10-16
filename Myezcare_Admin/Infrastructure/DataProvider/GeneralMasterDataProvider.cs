using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.Entity;
using Myezcare_Admin.Models.ViewModel;
using Myezcare_Admin.Resources;

namespace Myezcare_Admin.Infrastructure.DataProvider
{
    public class GeneralMasterDataProvider : BaseDataProvider, IGeneralMasterDataProvider
    {
        public ServiceResponse AddGeneralMaster(int ddTypeId)
        {
            var response = new ServiceResponse();

            List<DDMasterType> DDMasterTypeList = GetEntityList<DDMasterType>();

            var ChildRecord = DDMasterTypeList.Where(x => x.ParentID > 0).ToList();
            var ParentRecord = DDMasterTypeList.Where(w => ChildRecord.Select(s => s.ParentID).Contains(w.DDMasterTypeID)).ToList();

            DDMasterModel model = new DDMasterModel()
            {
                DDMaster = new DDMaster(),
                TypeList = DDMasterTypeList.OrderBy(x => x.Name).ToList(),
                DeleteFilter = Common.SetDeleteFilter(),
                SearchDDMasterListPage = new SearchDDMasterListPage() { IsDeleted = 0 },
                IsShowButtonForDisplay = ParentRecord.Count > 0,
                MappingDDMaster = new MappingDDMaster
                {
                    ParentTypeList = ParentRecord
                }
            };

            model.DDMaster.ItemType = ddTypeId>0? ddTypeId : model.DDMaster.ItemType;

            response.Data = model;
            return response;
        }


        public ServiceResponse SaveDDmaster(DDMaster ddMaster, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            try
            {
                bool isEditMode = ddMaster.DDMasterID > 0;

                searchList.Add(new SearchValueData("DDMasterID", Convert.ToString(ddMaster.DDMasterID)));
                searchList.Add(new SearchValueData("ItemType", Convert.ToString(ddMaster.ItemType)));
                searchList.Add(new SearchValueData("Title", ddMaster.Title));
                searchList.Add(new SearchValueData("Value", ddMaster.Value));
                searchList.Add(new SearchValueData("loggedInUserId", Convert.ToString(loggedInUserId)));
                searchList.Add(new SearchValueData("SystemID", Common.GetHostAddress()));

                int data = (int)GetScalar(StoredProcedure.SaveDDmaster, searchList);

                if (data == -1)
                {
                    response.Message = Resource.ItemAlreadyExists;
                    response.IsSuccess = false;
                    return response;
                }
                response.IsSuccess = true;
                response.Message = isEditMode ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.GeneralMaster) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.GeneralMaster);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }


        public ServiceResponse GetGeneralMasterList(SearchDDMasterListPage searchDDMasterListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchDDMasterListPage != null)
                SetSearchFilterForGeneralMasterList(searchDDMasterListPage, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<DDMasterListModel> totalData = GetEntityList<DDMasterListModel>(StoredProcedure.GetDDMasterList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<DDMasterListModel> ddMasterList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = ddMasterList;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForGeneralMasterList(SearchDDMasterListPage searchDDMasterListPage, List<SearchValueData> searchList)
        {
            if (!string.IsNullOrEmpty(searchDDMasterListPage.ItemType))
                searchList.Add(new SearchValueData { Name = "ItemType", Value = Convert.ToString(searchDDMasterListPage.ItemType) });

            if (!string.IsNullOrEmpty(searchDDMasterListPage.Title))
                searchList.Add(new SearchValueData { Name = "Title", Value = Convert.ToString(searchDDMasterListPage.Title) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchDDMasterListPage.IsDeleted) });

            if (!string.IsNullOrWhiteSpace(searchDDMasterListPage.Value))
                searchList.Add(new SearchValueData { Name = "Value", Value = Convert.ToString(searchDDMasterListPage.Value) });
        }

        public ServiceResponse DeleteDDMaster(SearchDDMasterListPage searchDDMasterListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
            SetSearchFilterForGeneralMasterList(searchDDMasterListPage, searchList);

            if (!string.IsNullOrEmpty(searchDDMasterListPage.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchDDMasterListPage.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });

            //List<DDMasterListModel> totalData = GetEntityList<DDMasterListModel>(StoredProcedure.DeleteDDMaster, searchList);
            DDMasterListTransactionModel ddMasterListTransactionModel = GetMultipleEntity<DDMasterListTransactionModel>(StoredProcedure.DeleteDDMaster, searchList);
            List<DDMasterListModel> totalData = ddMasterListTransactionModel.DDMasterListModel;

            if (ddMasterListTransactionModel.ResultId == -1)
            {
                response.IsSuccess = false;
                response.Message = Resource.ItemUsedInDifferentProcess;
            }
            else
            {
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.GeneralMaster);
            }

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;
            Page<DDMasterListModel> ddMasterList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = ddMasterList;
            return response;
        }

        public ServiceResponse GetParentChildMappingDDMaster(long DDMasterTypeID = 0, bool isFetchParentRecord = false, long parentID = 0)
        {
            var response = new ServiceResponse();
            ParentGeneralDetailForMapping parentGeneralDetailForMapping = GetMultipleEntity<ParentGeneralDetailForMapping>(
                StoredProcedure.GetParentGeneralDetailForMapping, new List<SearchValueData>
                {
                    new SearchValueData("DDMasterTypeID",Convert.ToString(DDMasterTypeID)),
                    new SearchValueData("IsFetchParentRecord",Convert.ToString(isFetchParentRecord)),
                    new SearchValueData("ParentID",Convert.ToString(parentID))
                });
            response.IsSuccess = true;
            response.Data = parentGeneralDetailForMapping;
            return response;
        }

        public ServiceResponse SaveParentChildMapping(long parentTaskId, List<long> childTaskIds)
        {
            var response = new ServiceResponse();
            TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.SaveParentChildMapping, new List<SearchValueData>
            {
                new SearchValueData("ParentTaskId",Convert.ToString(parentTaskId)),
                new SearchValueData("ChildTaskIds",Convert.ToString(string.Join(Constants.Comma,childTaskIds)))
            });
            if (result.TransactionResultId > 0)
            {
                response.IsSuccess = true;
                response.Message = Resource.ParentChildMappingSavedSuccessfully;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = Resource.ErrorOccured;
            }
            return response;
        }
    }
}
