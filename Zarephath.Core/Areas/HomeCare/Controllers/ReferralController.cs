using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Infrastructure.Utility;
using Zarephath.Core.Infrastructure.Utility.CareGiverApi;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class ReferralController : BaseController
    {
        private IReferralDataProvider _referralDataProvider;
        private ISettingDataProvider _settingDataProvider;
        CacheHelper _cacheHelper = new CacheHelper();

        #region Add Referral

        //[CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public ActionResult AddReferral(string id)
        {
            _referralDataProvider = new ReferralDataProvider();
            long referralID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;

            ServiceResponse response = _referralDataProvider.HC_SetAddReferralPage(referralID, SessionHelper.LoggedInID);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public ActionResult PartialddReferral(string id)
        {
            _referralDataProvider = new ReferralDataProvider();
            long referralID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;

            ServiceResponse response = _referralDataProvider.HC_SetAddReferralPage(referralID, SessionHelper.LoggedInID);
            // return ShowUserFriendlyPages(response) ?? View(response.Data);
            ViewBag.IsPartialView = true;
            return View("AddReferral", response.Data);

        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult SetAddReferralPage(string id)
        {

            _referralDataProvider = new ReferralDataProvider();
            long referralID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _referralDataProvider.HC_SetAddReferralPage(referralID, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public ContentResult DeleteReferralBeneficiaryType(ReferralBeneficiaryDetail referralBeneficiaryDetail)
        {
            _referralDataProvider = new ReferralDataProvider();
            return CustJson(_referralDataProvider.HC_DeleteReferralBeneficiaryType(referralBeneficiaryDetail, SessionHelper.LoggedInID));
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public ContentResult DeleteReferralPhysician(ReferralPhysicianDetail referralPhysicianDetail)
        {
            _referralDataProvider = new ReferralDataProvider();
            return CustJson(_referralDataProvider.HC_DeleteReferralPhysician(referralPhysicianDetail, SessionHelper.LoggedInID));
        }


        [HttpPost]
        public JsonResult AddSSNLog(HC_AddReferralModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.HC_AddReferralSSNLog(model, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }


        #endregion

        #region Referral Details

        #region Referral Compliance Details
        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult SaveReferralCompliance(ReferralComplianceModel referralComplianceModel)
        {
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.HC_SaveReferralCompliance(referralComplianceModel, SessionHelper.LoggedInID));
        }
        #endregion

        #region Client Information Tab

        [HttpPost]
        public JsonResult SearchRegion(string searchText, int pageSize)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.GetSearchRegion(pageSize, searchText));
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult AddReferral(HC_AddReferralModel addReferralModel)
        {
            //if (!addReferralModel.IsEditMode)
            //{
            //    addReferralModel.Referral.ReferralStatusID = (int)ReferralStatus.ReferralStatuses.Inactive;
            //}
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.HC_AddReferral(addReferralModel, SessionHelper.LoggedInID));
        }
        public JsonResult ReferralDxCodeMapping(HC_AddReferralModel addReferralModel)
        {
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.ReferralDxCodeMapping(addReferralModel, SessionHelper.LoggedInID));
        }

        public JsonResult SaveTaskOrder(List<ReferralDXCodeMapping> model, long RefID)
        {
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.SaveTaskOrder(model, RefID));
        }

        #endregion

        #region Contact Information Tab

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult AddContact(AddReferralModel addReferralModel)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.AddContact(addReferralModel, SessionHelper.LoggedInID));
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult GetContactList(string searchText, int pageSize)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.GetContactList(searchText, pageSize));
        }


        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult DeleteContact(long contactMappingID)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.DeteteContact(contactMappingID, SessionHelper.LoggedInID));
        }
        #endregion

        #region Referral Update Account #
        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult UpdateAccount(ReferralAhcccsDetails model, Referral referral)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.HC_UpdateAccount(model, referral, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }
        #endregion


        #endregion

        #region Referral Docuemnts, CheckList, SpaerForm, IM, Notes etc

        #region ReferralInternalMessage Tab

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult SetReferralInternalMessage(string EncryptedReferralID, SearchReferralInternalMessage SearchReferralInternalMessage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            long referralId = Convert.ToInt64(Crypto.Decrypt(EncryptedReferralID));
            ServiceResponse response = _referralDataProvider.SetReferralInternalMessage(0, SearchReferralInternalMessage, referralId, pageIndex, pageSize, sortIndex, sortDirection, false);
            return JsonSerializer(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult DeleteReferralInternalMessage(string EncryptedReferralInternalMessageID, long ReferralID, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            long referralInternalMessageId = Convert.ToInt64(Crypto.Decrypt(EncryptedReferralInternalMessageID));
            ServiceResponse response = _referralDataProvider.SetReferralInternalMessage(referralInternalMessageId, new SearchReferralInternalMessage(), ReferralID, pageIndex, pageSize, sortIndex, sortDirection, true);
            return JsonSerializer(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult SaveReferralInternalMessage(ReferralInternalMessage referralInternalMessage)
        {
            _referralDataProvider = new ReferralDataProvider();
            referralInternalMessage.ReferralID = Convert.ToInt64(Crypto.Decrypt(referralInternalMessage.EncryptedReferralID));
            ServiceResponse response = _referralDataProvider.SaveReferralInternalMessage(referralInternalMessage, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult ResolveReferralInternalMessage(string EncryptedReferralInternalMessageID, long ReferralID, string ResolvedComment)
        {
            _referralDataProvider = new ReferralDataProvider();

            long referralInternalMessageId = Convert.ToInt64(Crypto.Decrypt(EncryptedReferralInternalMessageID));

            ServiceResponse response = _referralDataProvider.HC_ResolveReferralInternalMessage(referralInternalMessageId, ReferralID, ResolvedComment, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion ReferralInternalMessage Tab

        #region ReferralNotes Tab
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate + Constants.Comma + Constants.HC_Permission_Employee_AddUpdate)]
        public JsonResult SaveReferralNote(string RoleID, string EmployeesID, string EncryptedReferralId, string EncryptedEmployeeID, string NoteDetail, string catId, bool IsEdit = false, string CommonNoteID = "", bool isPrivate = true)
        {
            _referralDataProvider = new ReferralDataProvider();
            long referralId = !string.IsNullOrWhiteSpace(EncryptedReferralId) ? Convert.ToInt64(Crypto.Decrypt(EncryptedReferralId)) : 0;
            long employeeID = !string.IsNullOrWhiteSpace(EncryptedEmployeeID) ? Convert.ToInt64(Crypto.Decrypt(EncryptedEmployeeID)) : 0;
            long commonNoteID = string.IsNullOrEmpty(CommonNoteID) ? 0 : Convert.ToInt64(CommonNoteID);
            //ReferralNotesModel model = new ReferralNotesModel { ReferralID = referralId, EmployeeID = employeeID, Note = NoteDetail, CreatedBy = SessionHelper.LoggedInID, CommonNoteID = commonNoteID };
            ServiceResponse response = _referralDataProvider.HC_SaveReferralNotes(RoleID, EmployeesID, referralId, employeeID, NoteDetail, catId, SessionHelper.LoggedInID, commonNoteID, IsEdit, isPrivate);
            return JsonSerializer(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate + Constants.Comma + Constants.HC_Permission_Employee_AddUpdate)]
        public JsonResult GetReferralNotes(string EncryptedReferralId, string EncryptedEmployeeID)
        {
            _referralDataProvider = new ReferralDataProvider();
            long referralId = !string.IsNullOrWhiteSpace(EncryptedReferralId) ? Convert.ToInt64(Crypto.Decrypt(EncryptedReferralId)) : 0;
            long employeeID = !string.IsNullOrWhiteSpace(EncryptedEmployeeID) ? Convert.ToInt64(Crypto.Decrypt(EncryptedEmployeeID)) : 0;
            ServiceResponse response = _referralDataProvider.GetReferralNotes(referralId, employeeID, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetReferralNotesByModel(SearchReferralNotesModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.GetReferralNotes(model.ReferralID, 0, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        public JsonResult GetReferralEmployee(string RoleId)
        {
            _referralDataProvider = new ReferralDataProvider();
            // long referralId = !string.IsNullOrWhiteSpace(RoleID) ? Convert.ToInt64(Crypto.Decrypt(RoleID)) : 0;
            ServiceResponse response = _referralDataProvider.GetReferralEmployee(RoleId);
            return JsonSerializer(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate + Constants.Comma + Constants.HC_Permission_Employee_AddUpdate)]
        public JsonResult DeleteReferralNote(string CommonNoteID)
        {
            _referralDataProvider = new ReferralDataProvider();
            long commonNoteID = Convert.ToInt64(CommonNoteID);
            ServiceResponse response = _referralDataProvider.DeleteReferralNote(commonNoteID, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion ReferralNotes Tab

        #region Referral Documents

        #region Documents

        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult SetReferralDocument(string EncryptedReferralID)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.HC_SetReferralDocument(Convert.ToInt64(Crypto.Decrypt(EncryptedReferralID)));
            return JsonSerializer(response);
        }

        #endregion Documents

        #region Missing Documents


        public JsonResult SetReferralMissingDocument(string EncryptedReferralID)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.HC_SetReferralMissingDocument(Convert.ToInt64(Crypto.Decrypt(EncryptedReferralID)));
            return JsonSerializer(response);
        }

        [CustomAuthorize(Permissions = Constants.Permission_ReferralDocuments_AddUpdate)]
        public JsonResult SendEmailForReferralMissingDocument(MissingDocumentModel missingDocumentModel, string EncryptedReferralID)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.HC_SendEmailForReferralMissingDocument(missingDocumentModel, Convert.ToInt64(Crypto.Decrypt(EncryptedReferralID)), SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion Missing Documents

        #endregion Referral Documents

        #endregion Add Referral

        #region Referral List

        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_List)]
        public ActionResult ReferralList()
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.HC_SetReferralListPage(SessionHelper.LoggedInID).Data;
            return View("referrallist", response);
        }

        [HttpPost]

        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_List)]
        public JsonResult GetReferralList(SearchReferralListModel searchReferralModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            string ids = "";
            if (searchReferralModel.GroupIds != null)
            {
                foreach (var item in searchReferralModel.GroupIds)
                {
                    ids += item + ",";
                }
            }
            searchReferralModel.CommaSeparatedIds = ids.ToString();
            var response = _referralDataProvider.GetReferralList(searchReferralModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_List)]
        public JsonResult GetReferralAuthorizationsDetails(string referralIDs)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.GetReferralAuthorizationsDetails(referralIDs);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_List)]
        public JsonResult GetReferralDetails(Referral referral)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.GetReferralDetails(referral);
            return JsonSerializer(response);
        }

        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Delete)]
        [CustomAuthorize(Permissions = Constants.Permission_View_All_Referral + "," + Constants.Permission_View_Assinged_Referral)]
        public JsonResult DeleteReferral(SearchReferralListModel searchReferralModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.DeleteReferral(searchReferralModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_View_All_Referral + "," + Constants.Permission_View_Assinged_Referral)]
        public JsonResult SaveReferralStatus(ReferralStatusModel referralStatusModel)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.HC_SaveReferralStatus(referralStatusModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult UpdateAssigneeBulk(ReferralBulkUpdateModel referralBulkUpdateModel)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.UpdateAssigneeBulk(referralBulkUpdateModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_View_All_Referral + "," + Constants.Permission_View_Assinged_Referral)]
        public JsonResult UpdateAssignee(SearchReferralListModel SearchReferralModel, ReferralStatusModel referralStatusModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.UpdateAssignee(SearchReferralModel, referralStatusModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult UpdateStatus(SearchReferralListModel SearchReferralModel, ReferralStatusModel referralStatusModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.UpdateStatus(SearchReferralModel, referralStatusModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }


        [HttpPost]
        public JsonResult UpdateTimeSlotDetailEmployee(SearchReferralTimeSlotDetail searchReferralTimeSlotDetail, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.UpdateTimeSlotDetailEmployee(searchReferralTimeSlotDetail, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }
        [HttpPost]
        public JsonResult SaveIncidentReportOrbeonForm(EmployeeVisitNoteForm form)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.SaveIncidentReportOrbeonForm(form));
        }

        [HttpPost]
        public JsonResult SaveReferralFaceSheetForm(EmployeeVisitNoteForm form)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.SaveReferralFaceSheetForm(form));
        }
        [HttpPost]
        public JsonResult SaveVitalForm(EmployeeVisitNoteForm form)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.SaveVitalForm(form));
        }
        #endregion

        #region Referral Document

        [HttpPost]

        public JsonResult UploadFile()
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_SaveFile(HttpContext.Request);
            return Json(response);
        }

        [HttpPost]

        public JsonResult UploadEmployeeDocument()
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_SaveFile(HttpContext.Request, true);
            return Json(response);
        }

        //[HttpPost]

        //public JsonResult GetReferralDocumentList(string id,string id2, int pageIndex = 1, int pageSize = 30, string sortIndex = "", string sortDirection = "")
        //{
        //    long referralID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
        //    long complianceID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(id2) : 0;
        //    _referralDataProvider = new ReferralDataProvider();
        //    return JsonSerializer(_referralDataProvider.HC_GetReferralDocumentList(referralID,complianceID, pageIndex, pageSize, sortIndex, sortDirection));
        //}

        [HttpPost]
        public JsonResult GetReferralDocumentList(SearchReferralDocumentListPage searchReferralDocument, int pageIndex = 1, int pageSize = 30, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.HC_GetReferralDocumentListNew(SessionHelper.RoleID, searchReferralDocument, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDocuments_AddUpdate)]
        public JsonResult DeleteDocument(long id, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.HC_DeleteDocument(id, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDocuments_AddUpdate)]
        public JsonResult SaveDocument(ReferralDocument id, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.HC_SaveDocument(id, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }

        [HttpPost]
        // [CustomAuthorize(Permissions = Constants.Permission_ReferralDocuments_AddUpdate)]
        public JsonResult SaveDocumentNew(ReferralDocument model)
        {
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.HC_SaveDocumentNew(model, SessionHelper.LoggedInID));
        }

        #endregion

        #region Referral History

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public JsonResult GetReferralHistory(long referralID)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.GetReferralHistory(referralID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public JsonResult SaveReferralHistoryItem(ReferralHistory referralHistoryModel)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.SaveReferralHistoryItem(referralHistoryModel, SessionHelper.LoggedInID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public JsonResult DeleteReferralHistoryItem(long referralHistoryID)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.DeleteReferralHistoryItem(referralHistoryID));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used for dxcode autocompleter. it will return the list of dxcode records
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="ignoreIds"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        //[HttpPost]
        //public ContentResult GetDxcodeListForAutoComplete(string searchText, string ignoreIds, int pageSize)
        //{
        //    _referralDataProvider = new ReferralDataProvider();
        //    return CustJson(_referralDataProvider.GetDxcodeListForAutoComplete(searchText, ignoreIds, pageSize));
        //}
        [HttpPost]
        public JsonResult GetDxcodeListForAutoComplete(string searchText, string ignoreIds, int pageSize)
        {
            _referralDataProvider = new ReferralDataProvider();
            var aa = _referralDataProvider.GetDxcodeListForAutoComplete(searchText, ignoreIds, pageSize);
            return Json(aa, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveDxCode(string DXCodeName, string DXCodeWithoutDot, string DxCodeType, string Description, string DxCodeShortName)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.SaveDxCode(DXCodeName, DXCodeWithoutDot, DxCodeType, Description, DxCodeShortName);
            return JsonSerializer(response);
        }


        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public ContentResult GetCaseManagerForAutoComplete(string searchText, int pageSize)
        {
            _referralDataProvider = new ReferralDataProvider();
            return CustJson(_referralDataProvider.GetCaseManagerForAutoComplete(searchText, pageSize));
        }

        /// <summary>
        /// This method will delete the mapping of dxcode
        /// </summary>

        /// <param name="referralDxCodeMappingDeleteModel"></param>
        /// <returns></returns>
        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public ContentResult DeleteReferralDxCodeMapping(ReferralDxCodeMappingDeleteModel referralDxCodeMappingDeleteModel)
        {
            _referralDataProvider = new ReferralDataProvider();
            long referralId = Convert.ToInt32(Crypto.Decrypt(referralDxCodeMappingDeleteModel.EncryptedReferralID));
            return CustJson(_referralDataProvider.HC_DeleteReferralDxCodeMapping(referralDxCodeMappingDeleteModel, referralId, SessionHelper.LoggedInID));
        }
        #endregion Sagar

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public JsonResult GetReferralInfo(string searchText, string ignoreIds, int pageSize)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.GetReferralInfo(pageSize, ignoreIds, searchText));
        }

        [HttpPost]
        public JsonResult DeletePreference(ReferralPreferenceModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.DeletePreference(model));
        }


        #region Referral Task Mapping

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_DxCode_List)]
        public ContentResult GetVisitTaskList(SearchVisitTaskListPage searchVisitTaskListPage, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            searchVisitTaskListPage.ReferralID = Convert.ToInt64(Crypto.Decrypt(searchVisitTaskListPage.EncryptedReferralID));
            return CustJson(_referralDataProvider.GetVisitTaskList(searchVisitTaskListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }


        [HttpPost]
        public ContentResult SaveRefVisitTaskList(RefVisitTaskModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            model.ReferralID = Convert.ToInt64(Crypto.Decrypt(model.EncryptedReferralID));
            return CustJson(_referralDataProvider.SaveRefVisitTaskList(model, SessionHelper.LoggedInID));


        }
        [HttpPost]
        public ContentResult SaveBulkRefVisitTaskList(List<RefVisitTaskModel> model)
        {
            _referralDataProvider = new ReferralDataProvider();
            //  model.ReferralID = Convert.ToInt64(Crypto.Decrypt(model.EncryptedReferralID));

            return CustJson(_referralDataProvider.SaveBulkRefVisitTaskList(model, SessionHelper.LoggedInID));
        }
        [HttpPost]
        public JsonResult SaveTaskDetail(TaskModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.SaveTaskDetail(model));
        }
        [HttpPost]
        public JsonResult UpdateIsActiveIsDeletedReferralGoal(SearchVisitTaskListPage model)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.UpdateGoalIsActiveIsDeletedFlag(model));
        }
        [HttpPost]
        public JsonResult GetReferralGoal(SearchVisitTaskListPage model)
        {
            _referralDataProvider = new ReferralDataProvider();
            model.ReferralID = Convert.ToInt64(Crypto.Decrypt(model.EncryptedReferralID));
            return Json(_referralDataProvider.GetReferralGoal(model));
        }
        [HttpPost]
        public ContentResult SaveGoalForReferral(SearchVisitTaskListPage model)
        {
            _referralDataProvider = new ReferralDataProvider();
            model.ReferralID = Convert.ToInt64(Crypto.Decrypt(model.EncryptedReferralID));
            return CustJson(_referralDataProvider.SaveReferralGoal(model, SessionHelper.LoggedInID));
        }


        [HttpPost]
        public ContentResult GetPatientTaskMappings(RefVisitTaskModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            model.ReferralID = Convert.ToInt64(Crypto.Decrypt(model.EncryptedReferralID));
            return CustJson(_referralDataProvider.GetPatientTaskMappings(model));

        }

        [HttpPost]
        public ContentResult GetReferralTaskMappingDetails(SearchVisitTaskListPage model)
        {
            _referralDataProvider = new ReferralDataProvider();
            model.ReferralID = Convert.ToInt64(Crypto.Decrypt(model.EncryptedReferralID));
            model.VisitTaskType = model.VisitTaskType;
            model.CareTypeID = Convert.ToInt64(model.CareTypeID);
            return CustJson(_referralDataProvider.GetReferralTaskMappingDetails(model));

        }

        [HttpPost]
        public ContentResult GetReferralTaskMappingReport(SearchVisitTaskListPage model)
        {
            _referralDataProvider = new ReferralDataProvider();
            model.ReferralID = Convert.ToInt64(Crypto.Decrypt(model.EncryptedReferralID));
            return CustJson(_referralDataProvider.GetReferralTaskMappingReports(model));

        }

        [HttpPost]
        public ContentResult DeleteRefTaskMapping(RefVisitTaskModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            model.ReferralID = Convert.ToInt64(Crypto.Decrypt(model.EncryptedReferralID));
            return CustJson(_referralDataProvider.DeleteRefTaskMapping(model, SessionHelper.LoggedInID));

        }


        [HttpPost]
        public ContentResult OnTaskChecked(RefVisitTaskModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            model.ReferralID = Convert.ToInt64(Crypto.Decrypt(model.EncryptedReferralID));
            return CustJson(_referralDataProvider.OnTaskChecked(model, SessionHelper.LoggedInID));
        }
        [HttpGet]
        public JsonResult GetCaretype()
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.GetCaretype();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCarePlanCaretypes(RefVisitTaskModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            long ReferralID = Convert.ToInt64(Crypto.Decrypt(model.EncryptedReferralID));
            ServiceResponse response = _referralDataProvider.GetCarePlanCaretypes(ReferralID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetVisitTaskByCategory(string VisitTaskType, string CareTypeID)
        {
            _referralDataProvider = new ReferralDataProvider();
            long CareType = Convert.ToInt64(CareTypeID);
            return Json(_referralDataProvider.GetVisitTaskCategory(VisitTaskType, CareType));
        }

        [HttpPost]
        public JsonResult GetTaskByActivity(string VisitTaskType, string CareTypeID, string VisitTaskCategoryID)
        {
            _referralDataProvider = new ReferralDataProvider();
            long CareType = Convert.ToInt64(CareTypeID);
            long VisitTaskCategoryId = Convert.ToInt64(VisitTaskCategoryID);
            return Json(_referralDataProvider.GetTaskByActivity(VisitTaskType, CareType, VisitTaskCategoryId));
        }

        #endregion


        #region Patient Calenders

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Calendar)]
        public ActionResult ReferralCalender()
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_ReferralCalender();
            var model = (HC_RefCalenderModel)response.Data;
            model.IsPartial = false;
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public ActionResult PartialReferralCalender(string id)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_ReferralCalender();
            var model = (HC_RefCalenderModel)response.Data;
            model.SearchRefCalender.ReferralID = new List<string>() { id };
            model.IsPartial = true;
            return ShowUserFriendlyPages(response) ?? View("ReferralCalender", response.Data);

        }



        #endregion


        #region Block Employee List

        [HttpPost]
        public ContentResult GetBlockEmpList(SearchRefBlockEmpListModel searchModel, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            searchModel.ReferralID = Convert.ToInt64(Crypto.Decrypt(searchModel.EncryptedReferralID));
            return CustJson(_referralDataProvider.GetBlockEmpList(searchModel, pageIndex, pageSize, sortIndex, sortDirection));
        }



        [HttpPost]
        public ContentResult SaveBlockEmp(ReferralBlockedEmployee model, SearchRefBlockEmpListModel searchModel, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            searchModel.ReferralID = Convert.ToInt64(Crypto.Decrypt(searchModel.EncryptedReferralID));
            return CustJson(_referralDataProvider.SaveBlockEmp(model, searchModel, SessionHelper.LoggedInID, pageIndex, pageSize, sortIndex, sortDirection));
        }



        [HttpPost]
        public ContentResult DeleteBlockEmp(ReferralBlockedEmployee model, SearchRefBlockEmpListModel searchModel, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            searchModel.ReferralID = Convert.ToInt64(Crypto.Decrypt(searchModel.EncryptedReferralID));
            return CustJson(_referralDataProvider.DeleteBlockEmp(model, searchModel, SessionHelper.LoggedInID, pageIndex, pageSize, sortIndex, sortDirection));

        }

        #endregion


        #region Referrals Days with time slots

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule)]
        public ActionResult ReferralTimeSlots()
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_ReferralTimeSlots();
            return ShowUserFriendlyPages(response) ?? View(response.Data);

        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public ActionResult PartialReferralTimeSlots(string id)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_ReferralTimeSlotss(id);

            var model = (HC_RTSModel)response.Data;
            model.SearchRTSMaster.ReferralID = Convert.ToInt64(id);
            model.IsPartial = true;
            return ShowUserFriendlyPages(response) ?? View("ReferralTimeSlots", response.Data);

        }


        #region RTS Master

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule + "," + Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult GetRtsMasterlist(SearchRTSMaster searchRTSMaster, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.GetRtsMasterlist(searchRTSMaster, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }
        [HttpPost]
        public JsonResult GetReferralTimeSlotDetaillist(SearchReferralTimeSlotDetail searchReferralTimeSlotDetail, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.GetReferralTimeSlotDetail(searchReferralTimeSlotDetail, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule)]
        public JsonResult DeleteRtsMaster(SearchRTSMaster searchRTSMaster, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.DeleteRtsMaster(searchRTSMaster, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule)]
        public JsonResult AddRtsMaster(ReferralTimeSlotMaster rtsMaster)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.AddRtsMaster(rtsMaster, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule)]
        public JsonResult AddRtsByPriorAuth(ReferralTimeSlotMaster rtsMaster)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.AddRtsByPriorAuth(rtsMaster, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule)]
        public JsonResult GetReferralAuthorizationsByReferralID(SearchRTSMaster rtsMaster)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.GetReferralAuthorizationsByReferralID(rtsMaster.ReferralID, rtsMaster.CareTypeID);
            return Json(response);
        }


        #endregion


        #region RTS Detail

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule)]
        public JsonResult GetRtsDetaillist(SearchRTSDetail searchRTSDetail, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.GetRtsDetaillist(searchRTSDetail, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule)]
        public JsonResult DeleteRtsDetail(SearchRTSDetail searchRTSDetail, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.DeleteRtsDetail(searchRTSDetail, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule)]
        public JsonResult AddRtsDetail(ReferralTimeSlotDetail rtsDetail)
        {
            _referralDataProvider = new ReferralDataProvider();
            if (rtsDetail.ReferralTimeSlotMasterID == 0 && rtsDetail.ReferralBillingAuthorizationID > 0)
            {
                var referralBillingAuthorization = _referralDataProvider.GetReferralAuthorizationsByReferralId(rtsDetail.ReferralID, 0)
                    .Where(t => t.ReferralBillingAuthorizationID == rtsDetail.ReferralBillingAuthorizationID)
                    .FirstOrDefault();
                var rtsMaster = new ReferralTimeSlotMaster()
                {
                    ReferralID = rtsDetail.ReferralID,
                    ReferralBillingAuthorizationID = rtsDetail.ReferralBillingAuthorizationID,
                    StartDate = Convert.ToDateTime(referralBillingAuthorization.StartDate),
                    EndDate = string.IsNullOrWhiteSpace(referralBillingAuthorization.EndDate) ? (DateTime?)null : Convert.ToDateTime(referralBillingAuthorization.EndDate),
                    IsEndDateAvailable = !string.IsNullOrWhiteSpace(referralBillingAuthorization.EndDate)
                };
                _referralDataProvider.AddRtsByPriorAuth(rtsMaster, SessionHelper.LoggedInID);

            }
            var response = _referralDataProvider.AddRtsDetail(rtsDetail, SessionHelper.LoggedInID);
            return Json(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule)]
        public JsonResult UpdateRtsDetail(ReferralTimeSlotDetail rtsDetail, SearchRTSDetail searchRTSDetail, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.UpdateRtsDetail(rtsDetail, searchRTSDetail, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        public JsonResult ReferralTimeSlotForceUpdate(ReferralTimeSlotDetail rtsDetail, SearchRTSDetail searchRTSDetail, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.ReferralTimeSlotForceUpdate(rtsDetail, searchRTSDetail, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion





        #endregion

        #region Referral Payor Mapping
        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult AddReferralPayorMapping(ReferralPayorMapping referralPayorMapping)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_AddReferralPayorMapping(referralPayorMapping, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult GetReferralPayorMappingList(string encryptedReferralId, SearchReferralPayorMapping searchReferralPayorMapping, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_GetReferralPayorMappingList(encryptedReferralId, searchReferralPayorMapping, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult DeleteReferralPayorMapping(string encryptedReferralId, SearchReferralPayorMapping searchReferralPayorMapping, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_DeleteReferralPayorMapping(encryptedReferralId, searchReferralPayorMapping, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        #endregion

        #region Referral Billing Setting

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult AddReferralBillingSetting(ReferralBillingSetting referralBillingSetting)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_AddReferralBillingSetting(referralBillingSetting, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult GetReferralBillingSetting(ReferralBillingSetting referralBillingSetting)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_GetReferralBillingSetting(referralBillingSetting);
            return JsonSerializer(response);
        }


        [HttpPost]
        public JsonResult CheckDxCode(string refferalId, string PayorID)
        {
            _referralDataProvider = new ReferralDataProvider();
            if (_referralDataProvider.IsDXCodeExist(refferalId, PayorID))
            {
                return JsonSerializer(true);
            }
            return JsonSerializer(false);
        }


        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult AddReferralBillingAuthorization(ReferralBillingAuthorization referralBillingAuthorization)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_AddReferralBillingAuthrization(referralBillingAuthorization, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        public ActionResult GetAuthorizationLinkupList(long referralBillingAuthorizationID)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.GetAuthorizationLinkupList(referralBillingAuthorizationID);
            return CustJson(response);
        }

        [HttpPost]
        public ActionResult GetAuthorizationScheduleLinkList(SearchAuthorizationScheduleLinkList searchAuthorizationScheduleLinkList)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.GetAuthorizationScheduleLinkList(searchAuthorizationScheduleLinkList);
            return CustJson(response);
        }

        [HttpPost]
        public ActionResult UpdateAuthorizationLinkup(UpdateAuthorizationLinkupModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.UpdateAuthorizationLinkup(model);
            return CustJson(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult GetAuthorizationLoadModel(string encryptedReferralId)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_GetAuthorizationLoadModel(encryptedReferralId);
            return JsonSerializer(response);
        }


        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult GetPayorMappedServiceCodeList(string searchText, long PayorID, int pageSize = 10)
        {

            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.HC_GetPayorMappedServiceCodeList(searchText, PayorID, pageSize));
        }



        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult GetPayorMappedServiceCodes(long PayorID)
        {

            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.HC_GetPayorMappedServiceCodes(PayorID));
        }


        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult GetAuthServiceCodes(SearchAuthServiceCodesModel model)
        {

            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.HC_GetAuthServiceCodes(model));
        }



        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult GetReferralBillingAuthorizationList(string encryptedReferralId, SearchReferralBillingAuthorization searchReferralBillingAuthorization, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_GetReferralBillingAuthorizationList(encryptedReferralId, searchReferralBillingAuthorization, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }



        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult DeleteReferralBillingAuthorization(string encryptedReferralId, SearchReferralBillingAuthorization searchReferralBillingAuthorization, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_DeleteReferralBillingAuthorization(encryptedReferralId, searchReferralBillingAuthorization, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }





        #region New Prior Authorization Code


        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult SavePriorAuthorization(ReferralBillingAuthorization referralBillingAuthorization)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_SavePriorAuthorization(referralBillingAuthorization, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }


        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult GetPriorAuthorizationList(string encryptedReferralId, SearchReferralBillingAuthorization searchReferralBillingAuthorization, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_GetPriorAuthorizationList(encryptedReferralId, searchReferralBillingAuthorization, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }


        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult DeletePriorAuthorization(string encryptedReferralId, SearchReferralBillingAuthorization searchReferralBillingAuthorization, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_DeletePriorAuthorization(encryptedReferralId, searchReferralBillingAuthorization, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }




        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult GetPAServiceCodeList(SearchReferralBillingAuthorizationCode searchReferralBillingAuthorizationCode)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_GetPAServiceCodeList(searchReferralBillingAuthorizationCode);
            return JsonSerializer(response);
        }



        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult SavePriorAuthorizationServiceCodeDetails(ReferralBillingAuthorizationServiceCodeModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_SavePriorAuthorizationServiceCodeDetails(model, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_ServiceCode_Mapping)]
        public JsonResult GetPayorServicecodeList(string searchText, string ReferralBillingAuthorizationID, int pageSize)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.HC_GetPayorServicecodeList(searchText, ReferralBillingAuthorizationID, pageSize));
        }


        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult DeletePAServiceCode(string referralBillingAuthorizationServiceCodeID)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_DeletePAServiceCode(referralBillingAuthorizationServiceCodeID, SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion


        #region Upload Referral Profile Image From API
        [HttpPost]
        public JsonResult UploadRefProfileImage(byte[] bytes)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.UploadRefProfileImage(HttpContext.Request);
            return Json(response);
        }
        #endregion

        #region Referral Case Load

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult GetRclMasterList(SearchRCLMaster searchRCLMaster, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.GetReferralCaseLoadList(searchRCLMaster, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult DeleteRclMaster(SearchRCLMaster searchRCLMaster, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.RemoveReferralCaseLoad(searchRCLMaster, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult AddRclMaster(ReferralCaseload rclMaster)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.AddRclMaster(rclMaster, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult MarkCaseLoadComplete(ReferralCaseload rclMaster)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.MarkCaseLoadComplete(rclMaster, SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion

        #region AuditLog
        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult GetAuditLogList(SearchRefAuditLogListModel searchModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.GetAuditLogList(searchModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }
        #endregion

        #region CareForm
        [HttpPost]
        public JsonResult GetCareFormDetails(SearchCareFormDetails searchCareFormDetails)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_GetCareFormDetails(searchCareFormDetails);
            return Json(response);
        }
        [HttpPost]
        public JsonResult SaveCareFormDetails(CareForm careForm)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_SaveCareFormDetails(careForm, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        public JsonResult SaveClientSign(CareForm careForm)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_SaveClientSignature(careForm, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpGet]
        public ActionResult GenerateCareForm(string id)
        {
            long CareFormID = Convert.ToInt64(id);
            _referralDataProvider = new ReferralDataProvider();
            SearchCareFormDetails searchCareFormDetails = new SearchCareFormDetails();
            searchCareFormDetails.CareFormID = CareFormID;
            ServiceResponse response = _referralDataProvider.HC_GetCareFormDetails(searchCareFormDetails);
            return View(response.Data);
        }

        [HttpGet]
        public ActionResult CareFormHeader()
        {
            //long CareFormID = Convert.ToInt64(id);
            //_referralDataProvider = new ReferralDataProvider();
            //SearchCareFormDetails searchCareFormDetails = new SearchCareFormDetails();
            //searchCareFormDetails.CareFormID = CareFormID;
            //ServiceResponse response = _referralDataProvider.HC_GetCareFormDetails(searchCareFormDetails);
            //return View(response.Data);
            return View();
        }

        [HttpGet]
        public ActionResult GenerateCareFormPDF(string id)
        {
            CacheHelper _cacheHelper = new CacheHelper();
            long careformID = Convert.ToInt64(Crypto.Decrypt(id));
            if (careformID == 0)
                return null;

            string CareFormUrl = string.Format("{0}{1}{2}", _cacheHelper.SiteBaseURL, Constants.HC_GenerateCareForm, careformID);
            string HeaderUrl = string.Format("{0}{1}", _cacheHelper.SiteBaseURL, Constants.HC_GenerateCareFormHeader);

            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.DisplayHeader = true;
            //converter.Header.DisplayOnFirstPage = true;
            //converter.Header.DisplayOnOddPages = true;
            //converter.Header.DisplayOnEvenPages = true;
            converter.Header.Height = 50;

            PdfHtmlSection headerHtml = new PdfHtmlSection(HeaderUrl);
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Header.Add(headerHtml);

            PdfDocument doc = converter.ConvertUrl(CareFormUrl);
            byte[] pdf = doc.Save();

            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = String.Format("{0}_{1}.pdf", "POC_", DateTime.Now.ToString(Constants.ReadableFileNameDateTimeFormat));
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_SaveCareFormPdfFile(pdf, fileResult.FileDownloadName, careformID, SessionHelper.LoggedInID);

            //SelectHtmlToPdf data = new SelectHtmlToPdf();
            //byte[] pdf = data.GenerateHtmlUrlToPdf(url);
            //// return resulted pdf document
            //FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            //fileResult.FileDownloadName = String.Format("{0}_{1}.pdf", "POC_", DateTime.Now.ToString(Constants.ReadableFileNameDateTimeFormat));
            //_referralDataProvider = new ReferralDataProvider();
            //ServiceResponse response = _referralDataProvider.HC_SaveCareFormPdfFile(pdf, fileResult.FileDownloadName,careformID,SessionHelper.LoggedInID);

            return fileResult;
        }
        #endregion

        #region MIF Form
        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult SetMIFForm(long referralID)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.SetMIFForm(referralID);
            return JsonSerializer(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult SaveMIFDetail(MIFDetail model)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.SaveMIFDetail(model);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult SaveMIFSign(MIFDetail MIFDetail)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_SaveMIFSignature(MIFDetail, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpGet]
        public ActionResult GenerateMIFPdf(string id)
        {
            long MIFFormID = Convert.ToInt64(Crypto.Decrypt(id));
            //long MIFFormID = Convert.ToInt64(id);

            if (MIFFormID == 0)
                return null;

            string url = string.Format("{0}{1}{2}", _cacheHelper.SiteBaseURL, Constants.HC_GenerateMIF, MIFFormID);

            SelectHtmlToPdf data = new SelectHtmlToPdf();
            byte[] pdf = data.GenerateHtmlUrlToPdf(url);
            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = String.Format("{0}_{1}.pdf", "MIF", DateTime.Now.ToString(Constants.ReadableFileNameDateTimeFormat)); ;
            return fileResult;
        }

        [HttpGet]
        public ActionResult GenerateMIF(string id)
        {
            long MIFFormID = Convert.ToInt64(id);
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.GetMIFDetailForPDF(MIFFormID);
            return View(response.Data);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate + Constants.Comma + Constants.HC_Permission_Employee_AddUpdate)]
        public JsonResult GetReferralMIFForms(string EncryptedReferralId)
        {
            _referralDataProvider = new ReferralDataProvider();
            long referralId = !string.IsNullOrWhiteSpace(EncryptedReferralId) ? Convert.ToInt64(Crypto.Decrypt(EncryptedReferralId)) : 0;
            ServiceResponse response = _referralDataProvider.GetReferralMIFForms(referralId);
            return JsonSerializer(response);
        }
        #endregion

        #region New Document related changes
        [HttpPost]
        public JsonResult GetReferralSectionList(string EncryptedReferralID, string userType, string EmployeeID)
        {
            _referralDataProvider = new ReferralDataProvider();
            long referralId = Convert.ToInt64(Crypto.Decrypt(EncryptedReferralID));
            return JsonSerializer(_referralDataProvider.HC_GetReferralSectionList(userType, SessionHelper.RoleID, referralId, EmployeeID));
        }

        [HttpPost]
        public JsonResult GetReferralSubSectionList(string EncryptedReferralID, string id, string userType, string EmployeeID)
        {
            long SectionID = Convert.ToInt64(id);
            long referralId = Convert.ToInt64(Crypto.Decrypt(EncryptedReferralID));
            _referralDataProvider = new ReferralDataProvider();
            if (userType != "Employee")
            {
                EmployeeID = "0";
            }
            return JsonSerializer(_referralDataProvider.HC_GetReferralSubSectionList(SectionID, userType, SessionHelper.RoleID, referralId, EmployeeID));
        }

        [HttpPost]
        public JsonResult SaveSection(AddDirSubDirModal modal)
        {
            _referralDataProvider = new ReferralDataProvider();
            if (modal.UserType != "Employee")
            {
                modal.EmployeeID = Convert.ToString(Crypto.Decrypt(modal.EmployeeID));
            }
            return JsonSerializer(_referralDataProvider.HC_SaveSectionNew(modal, SessionHelper.RoleID));
        }

        [HttpPost]
        public JsonResult SaveSubSection(AddDirSubDirModal modal)
        {
            _referralDataProvider = new ReferralDataProvider();
            if (modal.UserType != "Employee")
            {
                modal.EmployeeID = Convert.ToString(Crypto.Decrypt(modal.EmployeeID));
            }
            return JsonSerializer(_referralDataProvider.HC_SaveSubSectionNew(modal));
        }

        [HttpPost]
        public JsonResult GetReferralFormList(FormModal modal)
        {
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.HC_GetReferralFormList(modal));
        }

        [HttpPost]
        public JsonResult MapForm(MapFormDocModel modal)
        {
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.HC_MapForm(modal, SessionHelper.RoleID));
        }

        [HttpPost]
        public JsonResult SavedNewHtmlFormWithSubsection(SaveNewEBFormModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_SavedNewHtmlFormWithSubsection(model, SessionHelper.LoggedInID);
            return Json(response);
        }

        public JsonResult DeleteReferralDocument(DeleteDocModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.HC_DeleteReferralDocument(model, SessionHelper.LoggedInID));
        }

        public JsonResult DeleteReferralDocumentViaAPI(DeleteDocModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.DeleteReferralDocumentViaAPI(model));
        }

        [HttpPost]
        public JsonResult SaveDocumentFormName(DocFormNameModal model)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.SaveDocumentFormName(model);
            return Json(response);
        }
        #endregion





        #region Patient Related eBriggs Form

        #region - SCRAP CODE NO LONGER NEEDED JUST PUT FOR REFERENCES

        //[HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_Schedule)]
        //public ActionResult FormList()
        //{
        //    _referralDataProvider = new ReferralDataProvider();
        //    ServiceResponse response = _referralDataProvider.HC_GetFormListPage();

        //    if (response.IsSuccess)
        //    {
        //        var model = (HC_EBFormModel)response.Data;
        //        model.ForPatient = true;
        //        model.ForEmployee = false;
        //    }

        //    return ShowUserFriendlyPages(response) ?? View(response.Data);

        //}

        //[HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_Schedule)]
        //public ActionResult PartialFormList(string id)
        //{
        //    _referralDataProvider = new ReferralDataProvider();
        //    ServiceResponse response = _referralDataProvider.HC_GetFormListPage();

        //    if (response.IsSuccess)
        //    {
        //        var model = (HC_EBFormModel)response.Data;
        //        model.SearchEbForm.ReferralID = Convert.ToInt64(id);
        //        model.ForPatient = false;
        //        model.ForEmployee = false;
        //        model.IsPartial = true;
        //    }
        //    return ShowUserFriendlyPages(response) ?? View("FormList", response.Data);

        //}

        #endregion

        [HttpPost]
        public JsonResult SaveNewEBForm(SaveNewEBFormModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_SaveNewEBForm(model, SessionHelper.LoggedInID);
            return Json(response);
        }



        #endregion


        #region Referral Time Slots For Care Type
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule)]
        public ActionResult ReferralCareTypeTimeSlots()
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_ReferralCareTypeTimeSlots();
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public ActionResult PartialReferralCareTypeTimeSlots(string id)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_ReferralCareTypeTimeSlots();

            var model = (RefCareTypeSlotsModel)response.Data;
            model.SearchCTSchedule.ReferralID = Convert.ToInt64(id);
            model.IsPartial = true;
            return ShowUserFriendlyPages(response) ?? View("ReferralCareTypeTimeSlots", response.Data);
        }

        [HttpPost]
        public JsonResult AddCareTypeSlot(CareTypeTimeSlot model)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.AddCareTypeSlot(model, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        public JsonResult GetCareTypeScheduleList(SearchCTSchedule searchCTSchedule, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.GetCareTypeScheduleList(searchCTSchedule, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }
        #endregion

        #region Upload Referral Document From API
        [HttpPost]
        public JsonResult UploadDocumentViaAPI(byte[] bytes)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.UploadDocumentViaAPI(HttpContext.Request);
            return Json(response);
        }
        #endregion

        #region
        public ActionResult Medication()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetProductType()
        {

            ServiceResponse response = new ServiceResponse();
            CareGiverApi careGiverApi = new CareGiverApi();
            response = careGiverApi.GetProductType();
            return Json(response, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        public JsonResult SearchReferralMedications(string Search)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.SearchReferralMedications(Search);
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Medication(Zarephath.Core.Models.ViewModel.MedicationModel Medication)
        {
            MedicationDataProvider medicationDataProvider = new MedicationDataProvider();
            ServiceResponse response = medicationDataProvider.AddMedication(Medication);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetReferralMedications(ReferralMedication ReferralMedication)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.GetReferralMedications(Convert.ToInt32(ReferralMedication.ReferralID), 5, Convert.ToBoolean(ReferralMedication.IsActive));
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveReferralMedication(ReferralMedication ReferralMedication)
        {
            _referralDataProvider = new ReferralDataProvider();
            if (ReferralMedication.AddMedicationModel != null && ReferralMedication.AddMedicationModel.MedicationName != null)
            {
                MedicationDataProvider medicationDataProvider = new MedicationDataProvider();
                Models.ViewModel.MedicationModel medication = new Models.ViewModel.MedicationModel();
                medication.MedicationName = ReferralMedication.AddMedicationModel.MedicationName;
                medication.Brand_Name = ReferralMedication.AddMedicationModel.Brand_Name;
                medication.Generic_Name = ReferralMedication.AddMedicationModel.Generic_Name;
                medication.Dosage_Form = ReferralMedication.AddMedicationModel.Dosage_Form;
                medication.DosageTime = ReferralMedication.AddMedicationModel.DosageTime;
                medication.Route = ReferralMedication.AddMedicationModel.Route;
                medication.Product_Type = ReferralMedication.AddMedicationModel.Product_Type;
                if (ReferralMedication.MedicationId == 0)
                {
                    ServiceResponse responseMedication = medicationDataProvider.AddMedication(medication);
                    ReferralMedication.MedicationId = Convert.ToInt64(responseMedication.Data);
                }

            }
            ServiceResponse response = _referralDataProvider.SaveReferralMedication(ReferralMedication);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteReferralMedication(long ReferralMedicationID)
        {
            _referralDataProvider = new ReferralDataProvider();
            // long ReferralMedicationID = Convert.ToInt64(ReferralMedicationID);
            ServiceResponse response = _referralDataProvider.DeleteReferralMedication(ReferralMedicationID, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }
        public JsonResult EditReferralMedication(long ReferralMedicationID)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.EditReferralMedication(ReferralMedicationID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ----- Google Drive Integration -----

        [HttpPost]
        public JsonResult UploadFileGoogleDrive()
        {
            _referralDataProvider = new ReferralDataProvider();
            _settingDataProvider = new SettingDataProvider();

            string googleRefeshToken = (_settingDataProvider.GetSettings().Data as AddOrganizationSettingModel).OrganizationSetting.GoogleDriveRefreshToken;

            ServiceResponse response = _referralDataProvider.HC_SaveFile(HttpContext.Request, false, true, googleRefeshToken);
            return Json(response);
        }

        public JsonResult DeleteReferralDocumentGoogle(DeleteDocModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            _settingDataProvider = new SettingDataProvider();

            string googleRefeshToken = (_settingDataProvider.GetSettings().Data as AddOrganizationSettingModel).OrganizationSetting.GoogleDriveRefreshToken;

            return JsonSerializer(_referralDataProvider.HC_DeleteReferralDocumentGoogle(model, SessionHelper.LoggedInID, googleRefeshToken));
        }


        [HttpPost]
        public JsonResult UploadFileEmployeeGoogleDrive()
        {
            _referralDataProvider = new ReferralDataProvider();
            _settingDataProvider = new SettingDataProvider();

            string googleRefeshToken = (_settingDataProvider.GetSettings().Data as AddOrganizationSettingModel).OrganizationSetting.GoogleDriveRefreshToken;

            ServiceResponse response = _referralDataProvider.HC_SaveFile(HttpContext.Request, true, true, googleRefeshToken);
            return Json(response);
        }

        public JsonResult GetDocumentListGoogleDrive()
        {
            _settingDataProvider = new SettingDataProvider();

            string googleRefeshToken = (_settingDataProvider.GetSettings().Data as AddOrganizationSettingModel).OrganizationSetting.GoogleDriveRefreshToken;

            //ConfigSettings.GoogleDriveFilesListUri;
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.HC_GetDocumentListGoogleDrive(googleRefeshToken);
            return Json(response);
        }

        public JsonResult LinkGoogleDocumentFromDrive(LinkDocModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            _settingDataProvider = new SettingDataProvider();

            string googleRefeshToken = (_settingDataProvider.GetSettings().Data as AddOrganizationSettingModel).OrganizationSetting.GoogleDriveRefreshToken;

            ServiceResponse response = _referralDataProvider.HC_LinkGoogleDocument(model, googleRefeshToken, model.EmployeeID > 0);
            return Json(response);
        }

        #endregion

        #region Edit Timesheet changes for payor and auth code
        [HttpPost]
        public JsonResult GetReferralPayorsMapping(long referralID, DateTime startDate)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_GetReferralPayorsMapping(referralID, startDate);
            return Json(response);
        }

        [HttpPost]
        public JsonResult GetPriorAutherizationCodeByPayorAndRererrals(long payorID, long referralID)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_GetPriorAutherizationCodeByPayorAndRererrals(payorID, referralID);
            return Json(response);
        }
        #endregion

        #region ----- POrbeon Forms -----

        [HttpPost]
        public ActionResult OrbeonSaveFormByID(LinkDocModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            bool isEmployeeForm = false;
            if (model.EmployeeID > 0) isEmployeeForm = true;

            var response = _referralDataProvider.HC_GetOrbeonFormDetailsByID(model, isEmployeeForm);

            return Json(response);
        }

        #endregion

        #region Referral Certificates
        [HttpPost]
        public JsonResult ReferralCertificateList(ReferralCertificate model)
        {
            _referralDataProvider = new ReferralDataProvider();
            var lst = _referralDataProvider.GetReferralCertifictaes(model.EmployeeID);
            return Json(lst);
        }

        [HttpPost]
        public JsonResult SaveRefrallCertification(ReferralCertificate model, HttpPostedFileBase[] files)
        {

            model.CreatedBy = SessionHelper.LoggedInID;
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.SaveCertificates(model);
            return JsonSerializer(response);
        }


        [HttpPost]
        public JsonResult DeleteEmployeeCertification(string CertificateID)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.DeleteCertificates(Convert.ToInt64(CertificateID));
            return JsonSerializer(response);

        }

        [HttpGet]
        public JsonResult CertificateAuthority(string id)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.CertificateAuthority();
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult UploadCertificate()
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.UploadCertificate(HttpContext.Request, true);
            return Json(response);
        }
        [HttpPost]
        public FileResult Download(string file)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(file);
            var response = new FileContentResult(fileBytes, "application/octet-stream");
            response.FileDownloadName = "loremIpsum.pdf";
            return response;
        }

        #endregion
        [HttpGet]
        public JsonResult GetNoteCategory()
        {
            _referralDataProvider = new ReferralDataProvider();
            var lst = _referralDataProvider.GetCategory();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        #region Referral Allergy
        [HttpPost]
        public JsonResult SaveReferralAllergy(ReferralAllergyModel model)
        {
            long referralID = !string.IsNullOrEmpty(model.Patient) ? Convert.ToInt64(Crypto.Decrypt(model.Patient)) : 0;
            model.Patient = referralID.ToString();
            model.CreatedBy = SessionHelper.LoggedInID.ToString();
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.SaveReferralAllergy(model);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetReferralAllergy(string referralId, string allergy, string status)
        {
            int statusVal = 0;
            SearchAllergyModel model = new SearchAllergyModel();
            long referralID = !string.IsNullOrEmpty(referralId) ? Convert.ToInt64(Crypto.Decrypt(referralId)) : 0;
            _referralDataProvider = new ReferralDataProvider();
            model.ReferralId = referralID.ToString();
            model.Allergy = allergy;
            //if(Convert.ToBoolean(status)==true)
            //{
            //    statusVal = 1;
            //}
            model.Status = statusVal.ToString();
            ServiceResponse response = _referralDataProvider.GetReferralAllergy(model);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteAllergy(string AllergyId)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.DeleteAllergy(AllergyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetReportedBy(string referralId)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.GetAllergyDDL();
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetAllergyTitle()
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.GetAllergyTitle();
            return Json(response, JsonRequestBehavior.AllowGet);
        }



        //[HttpPost]
        //public JsonResult GetAllergyList(string referralId)
        //{
        //    _referralDataProvider = new ReferralDataProvider();
        //    ServiceResponse response = _referralDataProvider.GetAllergy();
        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}



        #endregion
        [HttpPost]
        public JsonResult SendReferralAttachment(MailModel model)
        {
            _referralDataProvider = new ReferralDataProvider();

            //RefDocument refDoc = (RefDocument)_referralDataProvider.GetReferralDocumentDetails(SessionHelper.ReferralDocumentID.ToString()).Data;
            RefDocument refDoc = (RefDocument)_referralDataProvider.GetReferralDocumentDetails(model.ReferralDocumentID.ToString()).Data;
            var orbeonFormDownloadPath = "";

            if (refDoc.StoreType.ToLower() == "orbeon") //checking if document type is orbeon
            {
                orbeonFormDownloadPath = Server.MapCustomPath("~/assets/files/OrbeonDocument");
                if (!Directory.Exists(orbeonFormDownloadPath))
                    Directory.CreateDirectory(orbeonFormDownloadPath);

                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    var orbeonFormURL = ConfigSettings.OrbeonBaseUrl + "/fr" + refDoc.FilePath + "/pdf/" + refDoc.GoogleFileId; //building orbeon form url
                    orbeonFormDownloadPath = orbeonFormDownloadPath + @"\" + refDoc.GoogleFileId + ".pdf"; //temporary download location for orbeon form
                    client.Credentials = new NetworkCredential(ConfigSettings.OrbeonUserName, ConfigSettings.OrbeonPassword); //orbeon server credentials
                    //client.DownloadFile(new Uri(orbeonFormURL), orbeonFormDownloadPath); //downloading orbeon form
                    //model.Attachment = new List<string>() { orbeonFormDownloadPath }; //setting the form as attachment to the email
                }
            }
            else
            {
                var path = refDoc.FilePath;
                if (!string.IsNullOrEmpty(path))
                {
                    string completeFilePath = Server.MapCustomPath(path);
                    model.Attachment = new List<string>() { completeFilePath };
                }
            }

            var fileAdditionalAttachments = TempData["Attachments"] as List<string>;

            if ( fileAdditionalAttachments != null)
            {
                model.Attachment.AddRange(fileAdditionalAttachments);
            }

            var lst = _referralDataProvider.SendReferralAttachment(model);
            if (fileAdditionalAttachments != null)
            {
                foreach (var item in fileAdditionalAttachments)
                {
                    System.IO.File.Delete(item);
                }
            }

            if (refDoc.StoreType.ToLower() == "orbeon" && !string.IsNullOrEmpty(orbeonFormDownloadPath))
            {
                System.IO.File.Delete(orbeonFormDownloadPath); //deleting the downloaded orbeon file after email was sent
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UploadFiles()
        {
            string FileName = "";
            HttpFileCollectionBase files = Request.Files;
            string fname = "";
            List<string> fileAttachments = new List<string>();
            for (int i = 0; i < files.Count; i++)
            {
                //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";
                //string filename = Path.GetFileName(Request.Files[i].FileName);
                HttpPostedFileBase file = files[i];

                // Checking for Internet Explorer
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = file.FileName;
                    FileName = file.FileName;
                }
                // Get the complete folder path and store the file inside it.
                fname = Path.Combine(Server.MapCustomPath("~/uploads/"), fname);
                file.SaveAs(fname);
                fileAttachments.Add(fname);
            }

            if (fileAttachments.Count > 0)
            {
                TempData["Attachments"] = fileAttachments;
            }
            return Json(FileName, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetReferralAttachment(string refID, string ReferralDocumentID)
        {
            SessionHelper.ReferralDocumentID = Convert.ToInt64(ReferralDocumentID);
            long referralID = !string.IsNullOrEmpty(refID) ? Convert.ToInt64(Crypto.Decrypt(refID)) : 0;
            SessionHelper.ReferraID = referralID;
            _referralDataProvider = new ReferralDataProvider();
            var lst = _referralDataProvider.GetReferralDetails(referralID.ToString());
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetTemplateList()
        {
            _referralDataProvider = new ReferralDataProvider();
            var lst = _referralDataProvider.GetTemplateList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOrganizationSettings()
        {
            _referralDataProvider = new ReferralDataProvider();
            var lst = _referralDataProvider.GetOrganizationSettings();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTemplateDetails(string name, long ReferralID)
        {
            _referralDataProvider = new ReferralDataProvider();
            var lst = _referralDataProvider.GetTemplateDetails(name, ReferralID);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetReferralListGroupList()
        {
            _referralDataProvider = new ReferralDataProvider();
            var lst = _referralDataProvider.GetEmployeeGroup("Employee Group");
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmployeeEmail()
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.GetEmployeeEmail(SessionHelper.LoggedInID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTokenValues(string token)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.GetEmployeeEmail(SessionHelper.LoggedInID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult SendFax(FaxModel model)
        {
            _referralDataProvider = new ReferralDataProvider();

            RefDocument refDoc = (RefDocument)_referralDataProvider.GetReferralDocumentDetails(model.DocumentID).Data;
            var orbeonFormDownloadPath = "";

            if (refDoc.StoreType.ToLower() == "orbeon") //checking if document type is orbeon
            {
                orbeonFormDownloadPath = Server.MapCustomPath("~/assets/files/OrbeonDocument");
                if (!Directory.Exists(orbeonFormDownloadPath))
                    Directory.CreateDirectory(orbeonFormDownloadPath);

                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    var orbeonFormURL = ConfigSettings.OrbeonBaseUrl + "/fr" + refDoc.FilePath + "/pdf/" + refDoc.GoogleFileId; //building orbeon form url
                    orbeonFormDownloadPath = orbeonFormDownloadPath + @"\" + refDoc.GoogleFileId + ".pdf"; //temporary download location for orbeon form
                    client.Credentials = new NetworkCredential(ConfigSettings.OrbeonUserName, ConfigSettings.OrbeonPassword); //orbeon server credentials
                    client.DownloadFile(new Uri(orbeonFormURL), orbeonFormDownloadPath); //downloading orbeon form
                    model.Path = orbeonFormDownloadPath; //setting the form as attachment to the email
                }
            }
            else
            {
                var path = refDoc.FilePath;
                if (!string.IsNullOrEmpty(path))
                {
                    string completeFilePath = Server.MapCustomPath(path);
                    model.Path = completeFilePath;
                }
            }

            //string path = _referralDataProvider.GetReferralDocumentPath(model.DocumentID);
            //string completeFilePath = Server.MapCustomPath(path);
            //model.Path = completeFilePath;
            ServiceResponse response = _referralDataProvider.SendFax(model);

            if (refDoc.StoreType.ToLower() == "orbeon" && !string.IsNullOrEmpty(orbeonFormDownloadPath))
            {
                System.IO.File.Delete(orbeonFormDownloadPath); //deleting the downloaded orbeon file after email was sent
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetNoteSentenceList(string NoteSentenceTitle, string NoteSentenceDetails, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.GetNoteSentenceList(NoteSentenceTitle, NoteSentenceDetails, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DxCodeMappingList1(long RefID)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.DxCodeMappingList1(RefID));
        }
        [HttpPost]
        public JsonResult UploadReferralProfileImage()
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_SaveRefProfileImg(HttpContext.Request, true);
            return Json(response);
        }

        [HttpPost]
        public JsonResult SendBulkEmail(MailModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            var fileAdditionalAttachments = TempData["Attachments"] as List<string>;
            model.Attachment = TempData["Attachments"] as List<string>;
            //if (fileAdditionalAttachments != null)
            //{
            //    model.Attachment.AddRange(fileAdditionalAttachments);
            //}

            var lst = _referralDataProvider.SendBulkEmail(model);
            if (fileAdditionalAttachments != null)
            {
                foreach (var item in fileAdditionalAttachments)
                {
                    System.IO.File.Delete(item);
                }
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetReferralEmail(string ReferralID)
        {
            _referralDataProvider = new ReferralDataProvider();
            var Res = _referralDataProvider.GetReferralEmail(ReferralID);
            return Json(Res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddRecipient(string ReferralId)
        {
            _referralDataProvider = new ReferralDataProvider();
            var lst = _referralDataProvider.GetReferralDetails(ReferralId);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetReferralPayor()
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_GetReferralPayor(SessionHelper.LoggedInID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetReferralStatus()
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_GetReferralStatus(SessionHelper.LoggedInID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetReferralCareType()
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.HC_GetReferralCareType(SessionHelper.LoggedInID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPayorIdentificationNumber(string PayorID)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.GetPayorIdentificationNumber(PayorID);
            return JsonSerializer(response);
        }
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_List)]
        public JsonResult ReferralList2()
        {
            _referralDataProvider = new ReferralDataProvider();
            SetReferralListModel response = (SetReferralListModel)_referralDataProvider.HC_SetReferralListPage(SessionHelper.LoggedInID).Data;
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_List)]
        public JsonResult GetReferralList2(SearchReferralListModel searchReferralModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            string ids = "";
            if (searchReferralModel.GroupIds != null)
            {
                foreach (var item in searchReferralModel.GroupIds)
                {
                    ids += item + ",";
                }
            }
            searchReferralModel.CommaSeparatedIds = ids.ToString();
            var response = _referralDataProvider.GetReferralList(searchReferralModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }


        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate + Constants.Comma + Constants.HC_Permission_Employee_AddUpdate)]
        public JsonResult GetDXcodeListDD(string ReferralID)
        {
            _referralDataProvider = new ReferralDataProvider();
            // long referralId = !string.IsNullOrWhiteSpace(EncryptedReferralId) ? Convert.ToInt64(Crypto.Decrypt(EncryptedReferralId)) : 0;
            ServiceResponse response = _referralDataProvider.GetDXcodeList(ReferralID, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }
        public JsonResult DxChangeSortingOrder(DxChangeSortingOrderModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.DxChangeSortingOrder(model));
        }
        [HttpPost]
        public JsonResult GetPayorDetails(string PayorID, string ReferralID)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.GetPayorDetails(PayorID, ReferralID);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult ExistanceOfReferralTimeslot(ReferralTimeSlotMaster rtsMaster)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.ExistanceOfReferralTimeslot(rtsMaster);
            return Json(response);
        }
        [HttpPost]
        public JsonResult PrioAuthorization(long ReferralID, long BillingAuthorizationID)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.PrioAuthorization(ReferralID, BillingAuthorizationID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMasterJurisdictionList(string claimProcessor)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.GetMasterJurisdictionList(claimProcessor));
        }

        [HttpPost]
        public JsonResult GetMasterTimezoneList(string claimProcessor)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.GetMasterTimezoneList(claimProcessor));
        }
    }
}
