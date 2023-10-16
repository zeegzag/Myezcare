using System;
using System.Web.Mvc;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    [CustomAuthorize(Permissions = Constants.HC_Permission_AdministrativePermission)]
    public class VisitTaskController:BaseController
    {
        private IVisitTaskDataProvider _visitQuestionDataProvider;

        [CustomAuthorize(Permissions = Constants.HC_Permission_VisitTask_AddUpdate)]
        public ActionResult AddVisitTask(string id)
        {
            long visitTaskId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            ServiceResponse response = _visitQuestionDataProvider.AddVisitTask(visitTaskId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [CustomAuthorize(Permissions = Constants.HC_Permission_VisitTask_AddUpdate)]
        public ActionResult PartialAddVisitTask(string id)
        {
            long visitTaskId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            ServiceResponse response = _visitQuestionDataProvider.AddVisitTask(visitTaskId);
            ViewBag.IsPartialView = true;
            return View("AddVisitTask", response.Data);
            //return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_ServiceCode_AddUpdate)]
        public JsonResult GetAddVisitTask(string id)
        {
            long visitTaskId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            ServiceResponse response = _visitQuestionDataProvider.AddVisitTask(visitTaskId);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_VisitTask_AddUpdate)]
        public JsonResult GetVisitTaskCategory(string VisitTaskType)
        {
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            return Json(_visitQuestionDataProvider.GetVisitTaskCategory(VisitTaskType));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_VisitTask_AddUpdate)]
        public JsonResult GetVisitTaskSubCategory(string VisitTaskCategoryID)
        {
            long visitTaskCategoryID = Convert.ToInt64(VisitTaskCategoryID);
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            return Json(_visitQuestionDataProvider.GetVisitTaskSubCategory(visitTaskCategoryID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_VisitTask_AddUpdate)]
        public JsonResult GetModelCategoryList(SearchCategory model)
        {
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            return Json(_visitQuestionDataProvider.GetModelCategoryList(model));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_VisitTask_AddUpdate)]
        public JsonResult SaveCategory(Category Category)
        {
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            return Json(_visitQuestionDataProvider.SaveCategory(Category));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_VisitTask_AddUpdate)]
        public JsonResult SaveSubCategory(Category Category)
        {
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            return Json(_visitQuestionDataProvider.SaveSubCategory(Category));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_VisitTask_AddUpdate)]
        public JsonResult AddVisitTask(AddVisitTaskModel addVisitTaskModel)
        {
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            return Json(_visitQuestionDataProvider.AddVisitTask(addVisitTaskModel, SessionHelper.LoggedInID));
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_VisitTask_List)]
        public ActionResult VisitTaskList()
        {
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            return View(_visitQuestionDataProvider.SetVisitTaskListPage().Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_VisitTask_List)]
        public ContentResult GetVisitTaskList(SearchVisitTaskListPage searchVisitTaskListPage, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            return CustJson(_visitQuestionDataProvider.GetVisitTaskList(searchVisitTaskListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_VisitTask_Delete)]
        public ContentResult DeleteVisitTask(SearchVisitTaskListPage searchVisitTaskListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            return CustJson(_visitQuestionDataProvider.DeleteVisitTask(searchVisitTaskListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_VisitTask_AddUpdate)]
        public JsonResult GetServiceCodeList(string searchText, int pageSize)
        {
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            return Json(_visitQuestionDataProvider.GetServiceCodeList(searchText, pageSize));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_VisitTask_AddUpdate)]
        public JsonResult GetCareTypeListFromVisitType(SearchModelForCareTypeList searchModelForCareTypeList)
        {
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            return Json(_visitQuestionDataProvider.GetCareTypeListFromVisitType(searchModelForCareTypeList));
        }

        #region Form Mapping with Task
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Compliance_AddUpdate)]
        public ContentResult GetOrganizationFormList()
        {
            _visitQuestionDataProvider = new VisitTaskDataProvider(Constants.MyezcareOrganizationConnectionString);
            var response = _visitQuestionDataProvider.GetOrganizationFormList();
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_VisitTask_AddUpdate)]
        public JsonResult MapSelectedForms(MapFormModel mapFormModel)
        {
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            return Json(_visitQuestionDataProvider.MapSelectedForms(mapFormModel, SessionHelper.LoggedInID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Compliance_AddUpdate)]
        public ContentResult GetTaskFormList(string id)
        {
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            long VisitTaskID = Convert.ToInt64(id);
            var response = _visitQuestionDataProvider.GetTaskFormList(VisitTaskID);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Compliance_AddUpdate)]
        public ContentResult VisitTaskFormEditCompliance(TaskFormMappingModel model)
        {
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            return CustJson(_visitQuestionDataProvider.VisitTaskFormEditCompliance(model, SessionHelper.LoggedInID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Compliance_AddUpdate)]
        public ContentResult OnFormChecked(TaskFormMappingModel model)
        {
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            return CustJson(_visitQuestionDataProvider.OnFormChecked(model, SessionHelper.LoggedInID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Compliance_AddUpdate)]
        public ContentResult DeleteMappedForm(string id)
        {
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            long TaskFormMappingID = Convert.ToInt64(id);
            var response = _visitQuestionDataProvider.DeleteMappedForm(TaskFormMappingID);
            return CustJson(response);
        }
        #endregion


        #region Bulk update visit task
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_VisitTask_AddUpdate)]
        public JsonResult BulkUpdateVisitTaskDetail(string BulkType,string VisitTaskIDList,string Catrgory)
        {
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            return Json(_visitQuestionDataProvider.BulkUpdateVisitTaskDetail(BulkType, VisitTaskIDList, Catrgory, SessionHelper.LoggedInID));
        }
        #endregion

        //Vishwas changes for clon task start
        [HttpPost]
       // [CustomAuthorize(Permissions = Constants.HC_Permission_VisitTask_Delete)]
        public ContentResult SaveCloneTask(SearchVisitTaskListPage searchVisitTaskListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            return CustJson(_visitQuestionDataProvider.SaveCloneTask(searchVisitTaskListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }

        //Vishwas changes for clon task End


    }
}
