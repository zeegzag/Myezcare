using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;


namespace Zarephath.Core.Controllers
{
    public class ReferralController : BaseController
    {
        private IReferralDataProvider _referralDataProvider;

        #region Add Referral

        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult AddReferral(string id)
        {
            _referralDataProvider = new ReferralDataProvider();
            long referralID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;

            ServiceResponse response = _referralDataProvider.SetAddReferralPage(referralID, SessionHelper.LoggedInID);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_View_AddUpdate)]
        public JsonResult SetAddReferralPage(string id)
        {

            _referralDataProvider = new ReferralDataProvider();
            long referralID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _referralDataProvider.SetAddReferralPage(referralID, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion

        #region Referral Details

        #region Client Information Tab

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_View_AddUpdate)]
        public JsonResult AddReferral(AddReferralModel addReferralModel)
        {
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.AddReferral(addReferralModel, SessionHelper.LoggedInID));
        }

        #endregion

        #region Contact Information Tab

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_View_AddUpdate)]
        public JsonResult AddContact(AddReferralModel addReferralModel)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.AddContact(addReferralModel, SessionHelper.LoggedInID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_View_AddUpdate)]
        public JsonResult GetContactList(string searchText, int pageSize)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.GetContactList(searchText, pageSize));
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_View_AddUpdate)]
        public JsonResult DeleteContact(long contactMappingID)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.DeteteContact(contactMappingID, SessionHelper.LoggedInID));
        }
        #endregion

        #region Referral History

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_View_AddUpdate)]
        public JsonResult DeleteReferralPayorMapping(long referralPayorMappingID)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.DeleteReferralPayorMapping(referralPayorMappingID, SessionHelper.LoggedInID));
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_View_AddUpdate)]
        public JsonResult MarkPayorAsActive(long referralID,long referralPayorMappingID)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.MarkPayorAsActive(referralID, referralPayorMappingID, SessionHelper.LoggedInID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_View_AddUpdate)]
        public JsonResult GetReferralPayorDetail( long referralPayorMappingID)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.GetReferralPayorDetail(referralPayorMappingID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_View_AddUpdate)]
        public JsonResult UpdateReferralPayorInformation(ReferralPayorMapping model)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.UpdateReferralPayorInformation(model,SessionHelper.LoggedInID));
        }

        
        


        #endregion

        #region Referral Audit Log
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_View_AddUpdate)]
        public JsonResult GetAuditLogList(SearchRefAuditLogListModel searchModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.GetAuditLogList(searchModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }
        #endregion

        #region Referral BX Contract
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_View_AddUpdate)]
        public JsonResult GetBXContractList(SearchRefBXContractListModel searchModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.GetBXContractList(searchModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_AddUpdate)]
        public JsonResult SaveBXContract(RefBXContractPageModel model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.SaveBXContract(model, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_AddUpdate)]
        public JsonResult UpdateBXContractStatus(ReferralBehaviorContract model)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.UpdateBXContractStatus(model, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_AddUpdate)]
        public JsonResult SaveSuspensionDetails(ReferralSuspention model, string EncryptedReferralID, bool ResetSuspension = false, bool ResetBXContract = false)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.SaveSuspensionDetails(model, EncryptedReferralID, SessionHelper.LoggedInID, ResetSuspension, ResetBXContract);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_View_AddUpdate)]
        public JsonResult GetSuspensionDetails(string EncryptedReferralID)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.GetSuspensionDetails(EncryptedReferralID);
            return JsonSerializer(response);
        }

        #endregion

        #region Referral Update AHCCCC ID
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_View_AddUpdate)]
        public JsonResult UpdateAhcccsid(ReferralAhcccsDetails model,Referral referral)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.UpdateAhcccsid(model, referral,SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }
        #endregion
      
        
        #endregion

        #region Referral Docuemnts, CheckList, SpaerForm, IM, Notes etc

        #region Referral Sibling  Mapping Delete

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_View_AddUpdate)]
        public JsonResult ReferralSiblingMappingDelete(long referralSiblingMappingId)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.ReferralSiblingMappingDelete(referralSiblingMappingId);
            return JsonSerializer(response);
        }

        #endregion

        #region ReferralCheckList Tab

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralChecklist_View_AddUpdate)]
        public JsonResult SetReferralCheckList(string EncryptedReferralID)
        {
            _referralDataProvider = new ReferralDataProvider();
            long referralId = Convert.ToInt64(Crypto.Decrypt(EncryptedReferralID));
            ServiceResponse response = _referralDataProvider.SetReferralCheckList(referralId);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralChecklist_AddUpdate)]
        public JsonResult SaveReferralCheckList(ReferralCheckList referralCheckList, List<DXCodeMappingList> dxCodeMappingList, string EncryptedReferralID)
        {
            _referralDataProvider = new ReferralDataProvider();
            referralCheckList.ReferralID = Convert.ToInt64(Crypto.Decrypt(EncryptedReferralID));
            ServiceResponse response = _referralDataProvider.SaveReferralCheckList(referralCheckList,dxCodeMappingList, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion ReferralCheckList Tab

        #region ReferralSparForm Tab

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralSparForm_View_AddUpdate)]
        public JsonResult SetReferralSparForm(string EncryptedReferralID)
        {
            _referralDataProvider = new ReferralDataProvider();
            long referralId = Convert.ToInt64(Crypto.Decrypt(EncryptedReferralID));
            ServiceResponse response = _referralDataProvider.SetReferralSparForm(referralId);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralSparForm_AddUpdate)]
        public JsonResult SaveReferralSparForm(ReferralSparForm referralSparForm, string EncryptedReferralID)
        {
            _referralDataProvider = new ReferralDataProvider();
            referralSparForm.ReferralID = Convert.ToInt64(Crypto.Decrypt(EncryptedReferralID));
            ServiceResponse response = _referralDataProvider.SaveReferralSparForm(referralSparForm, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion ReferralSparForm Tab

        #region ReferralInternalMessage Tab

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralInternalMessaging_View_AddUpdate)]
        public JsonResult SetReferralInternalMessage(string EncryptedReferralID, SearchReferralInternalMessage SearchReferralInternalMessage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            long referralId = Convert.ToInt64(Crypto.Decrypt(EncryptedReferralID));
            ServiceResponse response = _referralDataProvider.SetReferralInternalMessage(0, SearchReferralInternalMessage, referralId, pageIndex, pageSize, sortIndex, sortDirection, false);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralInternalMessaging_AddUpdate)]
        public JsonResult DeleteReferralInternalMessage(string EncryptedReferralInternalMessageID, long ReferralID, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            long referralInternalMessageId = Convert.ToInt64(Crypto.Decrypt(EncryptedReferralInternalMessageID));
            ServiceResponse response = _referralDataProvider.SetReferralInternalMessage(referralInternalMessageId, new SearchReferralInternalMessage(), ReferralID, pageIndex, pageSize, sortIndex, sortDirection, true);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralInternalMessaging_AddUpdate)]
        public JsonResult SaveReferralInternalMessage(ReferralInternalMessage referralInternalMessage)
        {
            _referralDataProvider = new ReferralDataProvider();
            referralInternalMessage.ReferralID = Convert.ToInt64(Crypto.Decrypt(referralInternalMessage.EncryptedReferralID));
            ServiceResponse response = _referralDataProvider.SaveReferralInternalMessage(referralInternalMessage, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralInternalMessaging_View_AddUpdate)]
        public JsonResult ResolveReferralInternalMessage(string EncryptedReferralInternalMessageID, long ReferralID)
        {
            _referralDataProvider = new ReferralDataProvider();

            long referralInternalMessageId = Convert.ToInt64(Crypto.Decrypt(EncryptedReferralInternalMessageID));

            ServiceResponse response = _referralDataProvider.ResolveReferralInternalMessage(referralInternalMessageId, ReferralID, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion ReferralInternalMessage Tab

        #region Referral Documents

        #region Documents

        [CustomAuthorize(Permissions = Constants.Permission_ReferralDocuments_View_AddUpdate)]
        public JsonResult SetReferralDocument(string EncryptedReferralID)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.SetReferralDocument(Convert.ToInt64(Crypto.Decrypt(EncryptedReferralID)));
            return JsonSerializer(response);
        }

        #endregion Documents

        #region Missing Documents

        [CustomAuthorize(Permissions = Constants.Permission_ReferralDocuments_View_AddUpdate)]
        public JsonResult SetReferralMissingDocument(string EncryptedReferralID)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.SetReferralMissingDocument(Convert.ToInt64(Crypto.Decrypt(EncryptedReferralID)));
            return JsonSerializer(response);
        }

        [CustomAuthorize(Permissions = Constants.Permission_ReferralDocuments_AddUpdate)]
        public JsonResult SendEmailForReferralMissingDocument(MissingDocumentModel missingDocumentModel, string EncryptedReferralID)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.SendEmailForReferralMissingDocument(missingDocumentModel, Convert.ToInt64(Crypto.Decrypt(EncryptedReferralID)), SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion Missing Documents

        #endregion Referral Documents

        #endregion Add Referral

        #region Referral List

        [CustomAuthorize(Permissions = Constants.Permission_View_All_Referral + "," + Constants.Permission_View_Assinged_Referral)]
        public ActionResult ReferralList()
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.SetReferralListPage(SessionHelper.LoggedInID).Data;
            return View("referrallist", response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_View_All_Referral + "," + Constants.Permission_View_Assinged_Referral)]
        public JsonResult GetReferralList(SearchReferralListModel searchReferralModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.GetReferralList(searchReferralModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_View_All_Referral + "," + Constants.Permission_View_Assinged_Referral)]
        public JsonResult SendReceiptNotificationEmail(string EncryptedReferralID)
        {
            _referralDataProvider = new ReferralDataProvider();

            var response = new ServiceResponse();
            long referralId = Convert.ToInt64(Crypto.Decrypt(EncryptedReferralID));
            response = _referralDataProvider.SendReceiptNotificationEmail(referralId);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_View_All_Referral + "," + Constants.Permission_View_Assinged_Referral)]
        public JsonResult DeleteReferral(SearchReferralListModel searchReferralModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.DeleteReferral(searchReferralModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_View_All_Referral + "," + Constants.Permission_View_Assinged_Referral)]
        public JsonResult SaveReferralStatus(ReferralStatusModel referralStatusModel)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.SaveReferralStatus(referralStatusModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_View_All_Referral + "," + Constants.Permission_View_Assinged_Referral)]
        public JsonResult UpdateAssignee(SearchReferralListModel SearchReferralModel, ReferralStatusModel referralStatusModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.UpdateAssignee(SearchReferralModel, referralStatusModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion

        #region Referral Document

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDocuments_View_AddUpdate)]
        public JsonResult UploadFile(string file, string fileName, string id)
        {
            long referralID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.SaveFile(file, fileName, referralID, SessionHelper.LoggedInID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDocuments_View_AddUpdate)]
        public JsonResult GetReferralDocumentList(string id, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            long referralID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.GetReferralDocumentList(referralID, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDocuments_AddUpdate)]
        public JsonResult DeleteDocument(long id, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.DeleteDocument(id, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDocuments_AddUpdate)]
        public JsonResult SaveDocument(ReferralDocument id, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.SaveDocument(id, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
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
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_View_AddUpdate)]
        public ContentResult GetDxcodeListForAutoComplete(string searchText, string ignoreIds, int pageSize)
        {
            _referralDataProvider = new ReferralDataProvider();
            return CustJson(_referralDataProvider.GetDxcodeListForAutoComplete(searchText, ignoreIds, pageSize));
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_View_AddUpdate)]
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
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDetails_View_AddUpdate)]
        public ContentResult DeleteReferralDxCodeMapping(ReferralDxCodeMappingDeleteModel referralDxCodeMappingDeleteModel)
        {
            _referralDataProvider = new ReferralDataProvider();
            long referralId = Convert.ToInt32(Crypto.Decrypt(referralDxCodeMappingDeleteModel.EncryptedReferralID));
            return CustJson(_referralDataProvider.DeleteReferralDxCodeMapping(referralDxCodeMappingDeleteModel, referralId, SessionHelper.LoggedInID));
        }
        #endregion Sagar

        #region Referral Tracking

        [CustomAuthorize(Permissions = Constants.Permission_Referral_Tracking)]
        public ActionResult ReferralTracking()
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.SetReferralTrackingListPage(SessionHelper.LoggedInID).Data;
            return View("ReferralTracking", response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Referral_Tracking)]
        public JsonResult GetReferralTrackingList(SearchReferralTrackingListModel searchReferralTrackingModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.GetReferralTrackingList(searchReferralTrackingModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Referral_Tracking)]
        public JsonResult DeleteReferralTracking(SearchReferralTrackingListModel searchReferralTrackingModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.DeleteReferralTracking(searchReferralTrackingModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Referral_Tracking)]
        public JsonResult SaveReferralTrackingStatus(ReferralStatusModel referralStatusModel)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.SaveReferralTrackingStatus(referralStatusModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Referral_Tracking)]
        public JsonResult SaveReferralTrackingComment(ReferralCommentModel referralCommentModel)
        {
            _referralDataProvider = new ReferralDataProvider();
            var response = _referralDataProvider.SaveReferralTrackingComment(referralCommentModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }


        #endregion

        #region Referral Outcome and Measurements
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReviewMeasurement_OutcomesMeasurement_View_AddUpdate)]
        public JsonResult SetReferralOutcomeMeasurement(long referralID, long referralOutcomeMeasurementID = 0)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.SetReferralOutcomeMeasurement(referralID, referralOutcomeMeasurementID, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReviewMeasurement_OutcomesMeasurement_AddUpdate)]
        public JsonResult SaveReferralOutcomeMeasurement(ReferralOutcomeMeasurement model)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.SaveReferralOutcomeMeasurement(model, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReviewMeasurement_OutcomesMeasurement_View_AddUpdate)]
        public JsonResult GetReferralOutcomeMeasurementList(SearchReferralOutcomeMeasurement searchModel)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.GetReferralOutcomeMeasurementList(searchModel);
            return JsonSerializer(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReviewMeasurement_OutcomesMeasurement_View_AddUpdate)]
        public JsonResult DeleteReferralOutcomeMeasurement(SearchReferralOutcomeMeasurement searchModel)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.DeleteReferralOutcomeMeasurement(searchModel);
            return JsonSerializer(response);
        }

        #endregion

        #region Add/List Referral Ansel Cassey Assessment



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReviewMeasurement_AnsellCaseyReview_View_AddUpdate)]
        public JsonResult SetReferralReviewAssessment(long referralID, long referralAssessmentID = 0)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.SetReferralReviewAssessment(referralID, referralAssessmentID, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReviewMeasurement_AnsellCaseyReview_AddUpdate)]
        public JsonResult SaveReferralReviewAssessment(ReferralAssessmentReview model)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.SaveReferralReviewAssessment(model, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReviewMeasurement_AnsellCaseyReview_View_AddUpdate)]
        public JsonResult GetReferralReviewAssessmentList(SearchReferralAssessmentReview searchModel)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.GetReferralReviewAssessmentList(searchModel);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReviewMeasurement_AnsellCaseyReview_View_AddUpdate)]
        public JsonResult DeleteReferralReviewAssessment(SearchReferralAssessmentReview searchModel)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.DeleteReferralReviewAssessment(searchModel);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReviewMeasurement_AnsellCaseyReview_View_AddUpdate)]
        public JsonResult UploadAssessmentResult(string file, string fileName, string id)
        {
            return null;
        }
        #endregion
        
        #region Referral Monthly Summary

        #region Add Referral Monthly Summary

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReviewMeasurement_MonthlySummary_View_AddUpdate)]
        public JsonResult SetReferralMonthlySummary(long referralID, long referralMonthlySummariesID = 0)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.SetReferralMonthlySummary(referralID, referralMonthlySummariesID, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReviewMeasurement_MonthlySummary_AddUpdate)]
        public JsonResult SaveReferralMonthlySummary(ReferralMonthlySummary model)
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.SaveReferralMonthlySummary(model, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReviewMeasurement_MonthlySummary_View_AddUpdate)]
        public JsonResult FindScheduleWithFaciltyAndServiceDate(FindScheduleWithFaciltyAndServiceDateModel model)
        {
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.FindScheduleWithFaciltyAndServiceDate(model));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReviewMeasurement_MonthlySummary_View_AddUpdate)]
        public JsonResult GetFacilityList()
        {
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.GetFacilityList());
        }

        #endregion

        #region List Referral Monthly Summary

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReviewMeasurement_MonthlySummary_View_AddUpdate)]
        public JsonResult GetReferralMonthlySummaryList(SearchReferralMonthlySummary searchModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.GetReferralMonthlySummaryList(searchModel, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }


        [CustomAuthorize(Permissions = Constants.Permission_Snapshot_Review)]
        public ActionResult MonthlySummaryList()
        {
            _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.MonthlySummaryList();
            return View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReviewMeasurement_MonthlySummary_AddUpdate)]
        public ContentResult DeleteReferralMonthlySummary(SearchReferralMonthlySummary searchModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _referralDataProvider = new ReferralDataProvider();
            return CustJson(_referralDataProvider.DeleteReferralMonthlySummary(searchModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }

        
        #endregion


        #endregion
        
        #region Referral Group MonthlySummary

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Snapshot_Add)]
        public ActionResult AddGroupMonthlySummary()
        {
            _referralDataProvider = new ReferralDataProvider();
            //var model = (GroupNoteModel)_noteDataProvider.SetAddGroupNote(SessionHelper.LoggedInID).Data;
            return View("AddGroupMonthlySummary",_referralDataProvider.FillGroupMonthlySummaryModel(SessionHelper.LoggedInID).Data);

        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Snapshot_Add)]
        public JsonResult SearchClientForMonthlySummary(SearchClientForMonthlySummary searchModel, List<long> ignoreClientID)
        {
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.SearchClientForMonthlySummary(searchModel, ignoreClientID));
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Snapshot_Add)]
        public JsonResult SaveMultipleMonthlySummary(List<ReferralMonthlySummary> monthlySummaries)
        {
            _referralDataProvider = new ReferralDataProvider();
            return JsonSerializer(_referralDataProvider.SaveMultipleMonthlySummary(monthlySummaries, SessionHelper.LoggedInID));
        }


        #endregion End Referral Group MonthlySummary
        
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public JsonResult GetReferralInfo(string searchText, string ignoreIds, int pageSize)
        {
            _referralDataProvider = new ReferralDataProvider();
            return Json(_referralDataProvider.GetReferralInfo(pageSize, ignoreIds, searchText));
        }





    }
}
