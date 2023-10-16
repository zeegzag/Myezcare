using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PetaPoco;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class BatchController : BaseController
    {
        IBatchDataProvider _iBatchDataProvider;

        #region Batch Creation And Listing

        #region Batch 837

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public ActionResult BatchMaster(string id)
        {
            _iBatchDataProvider = new BatchDataProvider();
            long batchId = Convert.ToInt64(id);// !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _iBatchDataProvider.HC_SetAddBatchPage(batchId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult AddBatchDetail(HC_AddBatchModel addBatchModel)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.HC_AddBatchDetail(addBatchModel, SessionHelper.LoggedInID, SessionHelper.IsCaseManagement);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult RefreshAndGroupingNotes()
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.HC_RefreshAndGroupingNotes();
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult GetPatientList(SearchPatientList SearchPatientList)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.HC_GetPatientList(SearchPatientList, SessionHelper.IsCaseManagement);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult GetChildNoteDetails(SearchPatientList SearchPatientList)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.HC_GetChildNoteDetails(SearchPatientList, SessionHelper.IsCaseManagement);
            return JsonSerializer(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult SaveMannualPaymentPostingDetails(MannualPaymentPostingModel model)
        {
            _iBatchDataProvider = new BatchDataProvider();
             return Json(_iBatchDataProvider.HC_SaveMannualPaymentPostingDetails(model));
            
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult GetBatchDetails(string BatchID)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.HC_GetEdi837BatchDetails(BatchID, SessionHelper.LoggedInID, SessionHelper.IsCaseManagement);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult GetBatchList(SearchBatchList searchBatchList, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_GetBatchList(searchBatchList, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult DeleteBatch(SearchBatchList searchBatchList, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_DeleteBatch(searchBatchList, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult MarkAsSentBatch(SearchBatchList searchBatchList, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_MarkAsSentBatch(SessionHelper.LoggedInID, searchBatchList, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }





        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult DeleteNote_Temporary(long NoteID, bool PermanentDelete)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_DeleteNote_Temporary(NoteID, PermanentDelete, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }



        #endregion

        #region OverView file

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult DownloadOverViewFile(string batchid)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_GenrateOverViewFile(batchid, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpGet]
        public ActionResult DownloadFiles(string vpath, string fname)
        {
            Common.SendZipFileBytesToResponse(vpath, fname);
            return null;
        }

        [HttpGet]
        public ActionResult Download(string vpath, string fname)
        {
            Common.SendFileBytesToResponse(Server.MapCustomPath(vpath), fname);
            return null;
        }

        #endregion

        #region Paper Remits/EOB Template

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult GenratePaperRemitsEOBTemplate(string batchid)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_GenratePaperRemitsEOBTemplate(batchid, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion

        #region Batch Validation And 837 Generation

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult ValidateBatches(PostEdiValidateGenerateModel postEdiValidateGenerateModel)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_ValidateBatchesAndGenerateEdi837Files(postEdiValidateGenerateModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult GenerateEdi837Files(PostEdiValidateGenerateModel postEdiValidateGenerateModel)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_ValidateBatchesAndGenerateEdi837Files(postEdiValidateGenerateModel, SessionHelper.LoggedInID, SessionHelper.IsCaseManagement);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult SubmitClaim(BatchValidationResponseModel claimModel)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.UploadClaims(claimModel,SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }




        

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult SyncClaimMessages()
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.SyncClaimMessages();
            return JsonSerializer(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult GetClaimMessageList(ClaimModel model )
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.GetClaimMessageList(model);
            return JsonSerializer(response);
        }

        



        #endregion

        #endregion

        #region Generate CMS-1500

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult GenerateCMS1500(EDIFileSearchModel EDIFileSearchModellong)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.GenerateCMS1500(EDIFileSearchModellong, SessionHelper.IsCaseManagement, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }


        #endregion

        #region Generate UB-04

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult GenerateUB04(EDIFileSearchModel EDIFileSearchModel)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.GenerateUB04(EDIFileSearchModel);
            return JsonSerializer(response);
        }


        #endregion

        #region Upload 835 And Processing
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Upload835)]
        public ActionResult Upload835()
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_SetUpload835Page();
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Upload835)]
        public JsonResult SaveUpload835File(AddUpload835Model model)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_SaveUpload835File(model, Request, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Upload835)]
        public JsonResult GetUpload835FileList(SearchUpload835ListPage searchUpload835Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_GetUpload835FileList(searchUpload835Model, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Upload835)]
        public JsonResult DeleteUpload835File(SearchUpload835ListPage searchUpload835Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_DeleteUpload835File(searchUpload835Model, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult ProcessUpload835File(SearchUpload835ListPage searchUpload835Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_ProcessUpload835File(searchUpload835Model, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }


        [HttpGet]
        public ActionResult BackEndProcessUpload835File()
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_BackEndProcessUpload835File();
            return null;
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Upload835)]
        public JsonResult SaveUpload835Comment(Upload835CommentModel upload835CommentModel)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_SaveUpload835Comment(upload835CommentModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion

        #region Reconcile 835 / EOB


        //[CustomAuthorize(Permissions = Constants.Permission_Billing_Reconcile835)]
        public ActionResult Reconcile835()
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_SetReconcile835Page();
            //return ShowUserFriendlyPages(response) ?? View(response.Data);
            return ShowUserFriendlyPages(response) ?? View("Reconcile", response.Data);
        }

        //getupload835files
        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_Billing_Reconcile835)]
        public JsonResult GetUpload835Files(string searchText, int pageSize, string searchParam)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var model = Common.DeserializeObject<Uploaded835FileSearchParam>(searchParam);
            return Json(_iBatchDataProvider.HC_GetUpload835Files(model.PayorID, pageSize, searchText));
        }


        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_Billing_Reconcile835)]
        public JsonResult GetReconcile835List(SearchReconcile835ListPage searchReconcile835Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_GetReconcile835List(searchReconcile835Model, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetReconcileBatchNoteDetails(long BatchNoteID, long BatchID, long NoteID, long? Upload835FileID)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.HC_GetReconcileBatchNoteDetails(BatchNoteID, BatchID, NoteID, Upload835FileID);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult UpdateBatchReconcile(long BatchNoteID, float PaidAmount, long ClaimStatusID, long ClaimStatusCodeID)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.HC_UpdateBatchReconcile(BatchNoteID, PaidAmount, ClaimStatusCodeID, ClaimStatusCodeID);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult MarkNoteAsLatest(long BatchNoteID, long BatchID, long NoteID, long? Upload835FileID)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.HC_MarkNoteAsLatest(BatchNoteID, BatchID, NoteID, Upload835FileID);
            return JsonSerializer(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_Billing_Reconcile835)]
        public JsonResult ExportReconcile835List(SearchReconcile835ListPage searchReconcile835Model, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            return JsonSerializer(_iBatchDataProvider.HC_ExportReconcile835List(searchReconcile835Model, sortIndex, sortDirection));
        }

        [HttpPost]
        public JsonResult SetClaimAdjustmentFlag(long batchId, long noteId, string claimAdjustmentType, string claimAdjustmentReason)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.HC_SetClaimAdjustmentFlag(batchId, noteId, claimAdjustmentType, claimAdjustmentReason);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult BulkSetClaimAdjustmentFlag(BulkClaimAdjustmentFlagModel model)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.HC_BulkSetClaimAdjustmentFlag(model);
            return JsonSerializer(response);
        }

        #region New Chnages

        //[CustomAuthorize(Permissions = Constants.Permission_Billing_Reconcile835)]
        public ActionResult Reconcile()
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_SetReconcile835Page();
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }


        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_Billing_Reconcile835)]
        public JsonResult GetReconcileList(SearchReconcile835ListPage searchReconcile835Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_GetReconcileList(searchReconcile835Model, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }


        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_Billing_Reconcile835)]
        public JsonResult GetParentReconcileList(SearchReconcile835ListPage searchReconcile835Model, int fromIndex = 1, int pageSize = 10)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_GetParentReconcileList(searchReconcile835Model, fromIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetChildReconcileList(long NoteID,long BatchID)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.HC_GetChildReconcileList(NoteID, BatchID);
            return JsonSerializer(response);
        }


        [HttpPost]
        public JsonResult MarkNoteAsLatest01(long BatchNoteID, long BatchID, long NoteID, long? Upload835FileID)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.HC_MarkNoteAsLatest01(BatchNoteID, BatchID, NoteID, Upload835FileID);
            return JsonSerializer(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_Billing_Reconcile835)]
        public JsonResult GetExportReconcileList(SearchReconcile835ListPage searchReconcile835Model, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            return JsonSerializer(_iBatchDataProvider.HC_ExportReconcileList(searchReconcile835Model, sortIndex, sortDirection));
        }



        #endregion


        #endregion

        #region List EDI FILE LOGS

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_EDIFileLogs)]
        public ActionResult EdiFileLogList()
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_SetEdiFileLogListPage();
            return View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_EDIFileLogs)]
        public JsonResult GetEdiFileLogList(SearchEdiFileLogListPage searchEdiFileLogModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_GetEdiFileLogList(searchEdiFileLogModel, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_EDIFileLogs)]
        public JsonResult DeleteEdiFileLog(SearchEdiFileLogListPage searchEdiFileLogModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_DeleteEdiFileLog(searchEdiFileLogModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }


        #endregion

        #region Process 270/271


        #region Process 270

        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_EDI270_271)]
        public ActionResult Process270_271()
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.HC_Process270_271();
            return View(response.Data);
        }

        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_EDI270_271)]
        public JsonResult Generate270File(AddProcess270Model model, SearchProcess270ListPage searchEdiFileLogListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.HC_Generate270File(model, searchEdiFileLogListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_EDI270_271)]
        public JsonResult GetEdi270FileList(SearchProcess270ListPage searchProcess270Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_GetEdi270FileList(searchProcess270Model, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_EDI270_271)]
        public JsonResult DeleteEdi270File(SearchProcess270ListPage searchProcess270Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_DeleteEdi270File(searchProcess270Model, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion

        #region Process 271

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_EDI270_271)]
        public JsonResult Upload271file(AddProcess271Model model)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_Upload271File(model, Request, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_EDI270_271)]
        public JsonResult GetEdi271FileList(SearchProcess271ListPage searchProcess271Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_GetEdi271FileList(searchProcess271Model, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_EDI270_271)]
        public JsonResult DeleteEdi271File(SearchProcess271ListPage searchProcess271Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_DeleteEdi271File(searchProcess271Model, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion

        #endregion

        #region ManageClaims

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public ActionResult ManageClaims()
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.HC_AddManageClaim();
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Upload835)]
        public ActionResult LatestERA(string id)
        {
            _iBatchDataProvider = new BatchDataProvider();
            long claimid = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _iBatchDataProvider.HC_SetAddERAPage(claimid);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_List)]
        public JsonResult GetClaimsList(ListManageClaimsModel SearchClaimListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.GetUploadedClaims(SearchClaimListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult GetERAsList()
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.GetLatestERA(SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult HCGetERAsList(SearchERAList searchERAListModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.HC_GetLatestERAList(searchERAListModel, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult GetERAPDFList(string eraId)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.GetLatestERAPDF(eraId);
            return JsonSerializer(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult GetLatestERA835(string eraId)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.GetLatestERA835(eraId);
            return JsonSerializer(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult ProcessERA835(string eraId, bool forceProcess)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.ProcessERA835(eraId, SessionHelper.LoggedInID, forceProcess);
            return JsonSerializer(response);
        }

        




        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_Batch837)]
        public JsonResult ArchieveClaim(string claimId)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.ArchieveClaim(claimId);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_List)]
        public JsonResult GetClaimErrorsList(ListManageClaimsModel item)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.GetClaimErrorsList(item.BatchUploadedClaimID);
            return JsonSerializer(response);
        }
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_List)]
        public JsonResult GetClaimErrorsListAndCMS1500(ListManageClaimsModel item, EDIFileSearchModel EDIFileSearchModellong)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.GetClaimErrorsListAndCMS1500(item.BatchUploadedClaimID, EDIFileSearchModellong, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_List)]
        public JsonResult SaveCMS1500Data(SaveCMS1500Modal saveCMS1500Modal, GenerateEdiFileModel cmsModel)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.SaveCMS1500Data(saveCMS1500Modal, cmsModel);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_List)]
        public JsonResult UpdateParentReconcileList(SaveCMS1500Modal saveCMS1500Modal, GenerateEdiFileModel cmsModel)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.SaveCMS1500Data(saveCMS1500Modal, cmsModel);
            return JsonSerializer(response);
        }

        #endregion
        #region Add Billing Note

        [HttpPost]
        public JsonResult SaveBillingNote(BillingNoteModel addBillingNoteModel)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.HC_SaveBillingNote(addBillingNoteModel, SessionHelper.LoggedInID);         
            return JsonSerializer(response);        
        }


        #endregion

        #region Get Billing Note

        [HttpPost]
        public JsonResult GetBillingNotes(long BatchID)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.HC_GetBillingNote(BatchID);        
            return JsonSerializer(response);          
        }


        #endregion
        #region Update Billing Note

        [HttpPost]
        public JsonResult UpdateBillingNotes(BillingNoteModel updateNoteModel)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.HC_UpdateBillingNote(updateNoteModel, SessionHelper.LoggedInID);            
            return JsonSerializer(response);         
        }


        #endregion
        #region Delete Billing Note
        [HttpPost]
        public JsonResult DeleteBillingNote(string BillingNoteID, long BatchID)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.HC_DeleteBillingNote(BillingNoteID, BatchID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AR Aging Report
        //aragingreport
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_ARAgingReport)]
        public ActionResult ARAgingReport()
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.SetARAgingReportPage();
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_ARAgingReport)]
        public JsonResult GetARAgingReport(SearchARAgingReportPage model)
        {
            _iBatchDataProvider = new BatchDataProvider();
            return Json(_iBatchDataProvider.GetARAgingReport(model));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Billing_ARAgingReport)]
        public JsonResult ExportARAgingReportList(SearchARAgingReportPage model)
        {
            _iBatchDataProvider = new BatchDataProvider();
            return JsonSerializer(_iBatchDataProvider.ExportARAgingReportList(model));
        }
        #endregion


       
    }
}
