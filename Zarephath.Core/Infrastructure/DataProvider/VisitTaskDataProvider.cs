using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class VisitTaskDataProvider : BaseDataProvider, IVisitTaskDataProvider
    {

        public VisitTaskDataProvider()
        {
        }

        public VisitTaskDataProvider(string conString)
            : base(conString)
        {
        }

        public ServiceResponse AddVisitTask(long visitTaskId)
        {
            ServiceResponse response = new ServiceResponse();

            AddVisitTaskModel addVisitTaskModel = GetMultipleEntity<AddVisitTaskModel>(StoredProcedure.AddVisitTaskPageModel,
                new List<SearchValueData>
                {
                    new SearchValueData { Name = "VisitTaskID", Value = Convert.ToString(visitTaskId) },
                    //new SearchValueData { Name = "ServiceCodeTypeID", Value = Convert.ToString((int)ServiceCodeType.ServiceCodeTypes.HomeCare) },
                    new SearchValueData { Name = "DDType_VisitType", Value = Convert.ToString((int)Common.DDType.VisitType)},
                    new SearchValueData { Name = "DDType_TaskFrequencyCode", Value = Convert.ToString((int)Common.DDType.TaskFrequencyCode)}
                });
            if (addVisitTaskModel.VisitTask == null)
                addVisitTaskModel.VisitTask = new VisitTask();

            addVisitTaskModel.Category = new Category();
            addVisitTaskModel.VisitTaskTypes = Common.VisitTaskType();
            addVisitTaskModel.ConfigEBFormModel = new ConfigEBFormModel();
            response.IsSuccess = true;
            response.Data = addVisitTaskModel;
            return response;
        }

        public ServiceResponse GetVisitTaskCategory(string VisitTaskType)
        {
            var response = new ServiceResponse();
            try
            {
                AddVisitTaskModel model = new AddVisitTaskModel();
                List<VisitTaskCategory> categoryList = GetEntityList<VisitTaskCategory>(StoredProcedure.GetVisitTaskCategory, new List<SearchValueData>
                {
                    new SearchValueData {Name = "VisitTaskType",Value =VisitTaskType }
                });
                response.IsSuccess = true;
                response.Data = categoryList;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse GetVisitTaskSubCategory(long visitTaskCategoryID)
        {
            var response = new ServiceResponse();
            try
            {
                List<VisitTaskCategory> categoryList = GetEntityList<VisitTaskCategory>(StoredProcedure.GetVisitTaskSubCategory, new List<SearchValueData>
                {
                    new SearchValueData {Name = "VisitTaskCategoryID",Value=Convert.ToString(visitTaskCategoryID) }
                });
                response.IsSuccess = true;
                response.Data = categoryList;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse GetModelCategoryList(SearchCategory model)
        {
            var response = new ServiceResponse();
            try
            {
                List<VisitTaskCategory> categoryList = GetEntityList<VisitTaskCategory>(StoredProcedure.GetModelCategoryList, new List<SearchValueData>
                {
                    new SearchValueData {Name = "CategoryName",Value=model.CategoryName },
                    new SearchValueData {Name = "SubCategoryName",Value=model.SubCategoryName },
                    new SearchValueData {Name = "Type",Value=model.Type },
                    new SearchValueData {Name = "IsCategoryList",Value=Convert.ToString(model.IsCategoryList)}
                });
                response.IsSuccess = true;
                response.Data = categoryList;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse SaveCategory(Category Category)
        {
            var response = new ServiceResponse();
            try
            {
                int data = (int)GetScalar(StoredProcedure.SaveCategory, new List<SearchValueData>
                {
                    new SearchValueData {Name = "VisitTaskCategoryID",Value = Convert.ToString(Category.VisitTaskCategoryID)},
                    new SearchValueData {Name = "VisitTaskType",Value = Category.VisitTaskType},
                    new SearchValueData {Name = "CategoryName",Value =Category.CategoryName}
                });
                if (data == -1)
                {
                    response.Message = Resource.CategoryAlreadyExists;
                    return response;
                }
                response.IsSuccess = true;
                response.Message = Category.VisitTaskCategoryID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Category) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.Category);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse SaveSubCategory(Category Category)
        {
            var response = new ServiceResponse();
            try
            {
                int data = (int)GetScalar(StoredProcedure.SaveSubCategory, new List<SearchValueData>
                {
                    new SearchValueData {Name = "VisitTaskCategoryID",Value = Convert.ToString(Category.VisitTaskCategoryID)},
                    new SearchValueData {Name = "ParentCategoryLevel",Value = Convert.ToString(Category.ParentCategoryLevel)},
                    new SearchValueData {Name = "VisitTaskType",Value = Constants.Sub},
                    new SearchValueData {Name = "SubCategoryName",Value =Category.SubCategoryName}
                });
                if (data == -1)
                {
                    response.Message = Resource.SubCategoryAlreadyExists;
                    return response;
                }
                response.IsSuccess = true;
                response.Message = Category.VisitTaskCategoryID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.SubCategory) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.SubCategory);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse AddVisitTask(AddVisitTaskModel addVisitTaskModel, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                bool isEditMode = addVisitTaskModel.VisitTask.VisitTaskID > 0;
                VisitTask visitTask = addVisitTaskModel.VisitTask;
                if (visitTask.VisitTaskType == Convert.ToString(VisitTask.TaskType.Conclusion))
                {
                    //visitTask.IsRequired = false;
                    visitTask.MinimumTimeRequired = 0;
                }

                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "VisitTaskID", Value = Convert.ToString(visitTask.VisitTaskID) });
                dataList.Add(new SearchValueData { Name = "VisitTaskType", Value = visitTask.VisitTaskType });
                dataList.Add(new SearchValueData { Name = "VisitTaskDetail", Value = visitTask.VisitTaskDetail });

                if (visitTask.VisitTaskCategoryID.HasValue)
                    dataList.Add(new SearchValueData { Name = "VisitTaskCategoryID", Value = Convert.ToString(visitTask.VisitTaskCategoryID) });
                if (visitTask.VisitTaskSubCategoryID.HasValue)
                    dataList.Add(new SearchValueData { Name = "VisitTaskSubCategoryID", Value = Convert.ToString(visitTask.VisitTaskSubCategoryID) });

                dataList.Add(new SearchValueData { Name = "ServiceCodeID", Value = visitTask.ServiceCodeID.ToString() });
                dataList.Add(new SearchValueData { Name = "IsDefault", Value = Convert.ToString(visitTask.IsDefault) });
                dataList.Add(new SearchValueData { Name = "SendAlert", Value = Convert.ToString(visitTask.SendAlert) });
                dataList.Add(new SearchValueData { Name = "IsRequired", Value = Convert.ToString(visitTask.IsRequired) });
                dataList.Add(new SearchValueData { Name = "MinimumTimeRequired", Value = Convert.ToString(visitTask.MinimumTimeRequired) });
                dataList.Add(new SearchValueData { Name = "loggedInUserId", Value = Convert.ToString(loggedInUserId) });
                dataList.Add(new SearchValueData { Name = "IsEditMode", Value = Convert.ToString(isEditMode) });
                dataList.Add(new SearchValueData { Name = "SystemID", Value = HttpContext.Current.Request.UserHostAddress });
                dataList.Add(new SearchValueData { Name = "VisitType", Value = Convert.ToString(visitTask.VisitType) });
                dataList.Add(new SearchValueData { Name = "CareType", Value = Convert.ToString(visitTask.CareType) });
                dataList.Add(new SearchValueData { Name = "Frequency", Value = Convert.ToString(visitTask.Frequency) });
                dataList.Add(new SearchValueData { Name = "TaskCode", Value = visitTask.TaskCode });
                dataList.Add(new SearchValueData { Name = "TaskOption", Value = Convert.ToString(visitTask.TaskOption) });
                dataList.Add(new SearchValueData { Name = "DefaultTaskOption", Value = Convert.ToString(visitTask.DefaultTaskOption) });

                int data = (int)GetScalar(StoredProcedure.SaveVisitTask, dataList);


                if (data == -1)
                {
                    response.Message = Resource.TaskAlreadyExists;
                    return response;
                }
                response.IsSuccess = true;
                response.Message = isEditMode ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.VisitTask) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.VisitTask);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse SetVisitTaskListPage()
        {
            ServiceResponse response = new ServiceResponse();
            SetVisitTaskListPage model = GetMultipleEntity<SetVisitTaskListPage>(StoredProcedure.SetVisitTaskListPage,
                new List<SearchValueData>
                {
                    new SearchValueData { Name = "ServiceCodeTypeID", Value = Convert.ToString((int)ServiceCodeType.ServiceCodeTypes.HomeCare) },
                    new SearchValueData { Name = "DDType_VisitType", Value = Convert.ToString((int)Common.DDType.VisitType) },
                    new SearchValueData { Name = "DDType_CareType", Value = Convert.ToString((int)Common.DDType.CareType) }
                });

            model.VisitTaskTypes = Common.VisitTaskType();
            model.DeleteFilter = Common.SetDeleteFilter();
            model.SearchVisitTaskListPage = new SearchVisitTaskListPage() { IsDeleted = 0 };
            response.Data = model;
            return response;
        }

        public ServiceResponse GetVisitTaskList(SearchVisitTaskListPage searchVisitTaskListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchVisitTaskListPage != null)
                SetSearchFilterForVisitTaskList(searchVisitTaskListPage, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListVisitTaskModel> totalData = GetEntityList<ListVisitTaskModel>(StoredProcedure.GetVisitTaskList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListVisitTaskModel> visitTaskList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = visitTaskList;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForVisitTaskList(SearchVisitTaskListPage searchVisitTaskListPage, List<SearchValueData> searchList)
        {
            if (!string.IsNullOrEmpty(searchVisitTaskListPage.VisitTaskType))
                searchList.Add(new SearchValueData { Name = "VisitTaskType", Value = Convert.ToString(searchVisitTaskListPage.VisitTaskType) });

            if (!string.IsNullOrEmpty(searchVisitTaskListPage.VisitTaskDetail))
                searchList.Add(new SearchValueData { Name = "VisitTaskDetail", Value = Convert.ToString(searchVisitTaskListPage.VisitTaskDetail) });

            //searchList.Add(new SearchValueData { Name = "ServiceCodeID", Value = Convert.ToString(searchVisitTaskListPage.ServiceCodeID) });
            searchList.Add(new SearchValueData { Name = "ServiceCode", Value = Convert.ToString(searchVisitTaskListPage.ServiceCode) });

            searchList.Add(new SearchValueData { Name = "VisitTaskCategoryID", Value = Convert.ToString(searchVisitTaskListPage.VisitTaskCategoryID) });
            searchList.Add(new SearchValueData { Name = "VisitTaskVisitTypeID", Value = Convert.ToString(searchVisitTaskListPage.VisitTaskVisitTypeID) });
            searchList.Add(new SearchValueData { Name = "VisitTaskCareTypeID", Value = Convert.ToString(searchVisitTaskListPage.VisitTaskCareTypeID) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchVisitTaskListPage.IsDeleted) });
        }

        public ServiceResponse DeleteVisitTask(SearchVisitTaskListPage searchVisitTaskListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId)
        {
            var response = new ServiceResponse();

            //List<SearchValueData> searchList = new List<SearchValueData>();
            //SetSearchFilterForVisitTaskList(searchVisitTaskListPage, searchList);
            //searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));
            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForVisitTaskList(searchVisitTaskListPage, searchList);

            if (!string.IsNullOrEmpty(searchVisitTaskListPage.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchVisitTaskListPage.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });

            List<ListVisitTaskModel> totalData = GetEntityList<ListVisitTaskModel>(StoredProcedure.DeleteVisitTask, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListVisitTaskModel> visitTaskList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = visitTaskList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.VisitTask);
            return response;
        }

        public List<ServiceCodes> GetServiceCodeList(string searchText, int pageSize = 10)
        {
            List<ServiceCodes> contactlist = GetEntityList<ServiceCodes>(StoredProcedure.HC_GetVisitServiceCodeListForAutoCompleter,
                                            new List<SearchValueData>
                                                {
                                                    new SearchValueData {Name = "SearchText", Value = searchText},
                                                    new SearchValueData {Name = "PageSize", Value = pageSize.ToString()}
                                                });
            return contactlist;
        }


        public ServiceResponse GetCareTypeListFromVisitType(SearchModelForCareTypeList searchModelForCareTypeList)
        {
            var response = new ServiceResponse();
            try
            {
                List<NameValueData> caretypelist = GetEntityList<NameValueData>(StoredProcedure.HC_GetCareTypeListFromVisitType,
                                           new List<SearchValueData>
                                               {
                                                    new SearchValueData {Name = "VisitTaskID", Value = Convert.ToString(searchModelForCareTypeList.VisitTypeID)},
                                                    new SearchValueData {Name = "DDType_CareType", Value = Convert.ToString((int)Common.DDType.CareType)}
                                               });
                response.IsSuccess = true;
                response.Data = caretypelist;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
            }

            return response;
        }

        #region Bulk update Visit task
        public ServiceResponse BulkUpdateVisitTaskDetail(string BulkType, string VisitTaskIDList, string Catrgory, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                GetEntityList<NameValueData>(StoredProcedure.BulkUpdateVisitTaskDetail,
                                           new List<SearchValueData>
                                               {
                                                    new SearchValueData {Name = "BulkType", Value = Convert.ToString(BulkType)},
                                                   new SearchValueData {Name = "VisitTaskIDList", Value = Convert.ToString(VisitTaskIDList)},
                                                   new SearchValueData {Name = "loggedInID", Value = Convert.ToString(loggedInUserId)},
                                                   new SearchValueData {Name = "VisitTaskValue", Value = Convert.ToString(Catrgory)},
                                               });
                response.IsSuccess = true;

            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
            }

            return response;
        }
        #endregion

        #region Form Mapping with Task
        public ServiceResponse GetOrganizationFormList()
        {
            ServiceResponse response = new ServiceResponse();

            CacheHelper_MyezCare chMyezcareOrg = new CacheHelper_MyezCare();
            MyEzcareOrganization myOrg = chMyezcareOrg.GetCachedData<MyEzcareOrganization>();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(myOrg.OrganizationID) });
            List<FormListModel> data = GetEntityList<FormListModel>(StoredProcedure.GetOrgFormListForMappingWithTask, searchList);


            response.Data = data;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse MapSelectedForms(MapFormModel mapFormModel, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "VisitTaskID", Value = Convert.ToString(mapFormModel.VisitTaskID) });
                dataList.Add(new SearchValueData { Name = "EBFormIDs", Value = mapFormModel.EBFormIDs });
                dataList.Add(new SearchValueData { Name = "CurrentDateTime", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat) });
                dataList.Add(new SearchValueData { Name = "loggedInUserId", Value = Convert.ToString(loggedInUserId) });
                dataList.Add(new SearchValueData { Name = "SystemID", Value = HttpContext.Current.Request.UserHostAddress });

                int data = (int)GetScalar(StoredProcedure.MapSelectedForms, dataList);

                if (data > 0)
                {
                    response.IsSuccess = true;
                    response.Message = Resource.FormMappedSuccessfully;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse GetTaskFormList(long VisitTaskID)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "VisitTaskID", Value = Convert.ToString(VisitTaskID) });
            List<TaskFormMappingModel> data = GetEntityList<TaskFormMappingModel>(StoredProcedure.GetTaskFormList, searchList);
            response.Data = data;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse VisitTaskFormEditCompliance(TaskFormMappingModel model, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();
            var searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData() { Name = "TaskFormMappingID", Value = Convert.ToString(model.TaskFormMappingID) });
            searchList.Add(new SearchValueData() { Name = "ComplianceID", Value = Convert.ToString(model.ComplianceID) });
            searchList.Add(new SearchValueData() { Name = "CurrentDateTime", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat) });
            searchList.Add(new SearchValueData() { Name = "LoggedIn", Value = Convert.ToString(loggedInUserId) });
            GetScalar(StoredProcedure.VisitTaskFormEditCompliance, searchList);
            response.IsSuccess = true;
            return response;
        }


        public ServiceResponse OnFormChecked(TaskFormMappingModel model, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();
            var searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData() { Name = "TaskFormMappingID", Value = Convert.ToString(model.TaskFormMappingID) });
            searchList.Add(new SearchValueData() { Name = "IsRequired", Value = Convert.ToString(model.IsRequired) });
            searchList.Add(new SearchValueData() { Name = "CurrentDateTime", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat) });
            searchList.Add(new SearchValueData() { Name = "LoggedIn", Value = Convert.ToString(loggedInUserId) });
            GetScalar(StoredProcedure.OnFormChecked, searchList);
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse DeleteMappedForm(long TaskFormMappingID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "TaskFormMappingID", Value = Convert.ToString(TaskFormMappingID) });
                int data = (int)GetScalar(StoredProcedure.DeleteMappedForm, searchList);
                if (data > 0)
                {
                    response.IsSuccess = true;
                    response.Message = Resource.MappingDeletedSuccessfully;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
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

        public ServiceResponse SaveCloneTask(SearchVisitTaskListPage searchVisitTaskListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForVisitTaskList(searchVisitTaskListPage, searchList);

            if (!string.IsNullOrEmpty(searchVisitTaskListPage.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchVisitTaskListPage.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "TargetCareType", Value = Convert.ToString(searchVisitTaskListPage.TargetCareType) });
            searchList.Add(new SearchValueData { Name = "TargetServiceCode", Value = Convert.ToString(searchVisitTaskListPage.TargetServiceCode) });
            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });

            List<ListVisitTaskModel> totalData = GetEntityList<ListVisitTaskModel>(StoredProcedure.CloneTask, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListVisitTaskModel> visitTaskList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = visitTaskList;
            response.IsSuccess = true;
            response.Message = string.Format("VisitTask Cloning Successfully");
            return response;
        }
    }
}
