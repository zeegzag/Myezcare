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
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Controllers
{
    public class BatchController : BaseController
    {
        IBatchDataProvider _iBatchDataProvider;

        #region Batch Creation And Listing

        #region Add Batch Detail

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Batch837)]
        public ActionResult BatchMaster(string id)
        {
            _iBatchDataProvider = new BatchDataProvider();
            long batchId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _iBatchDataProvider.SetAddBatchPage(batchId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Batch837)]
        public JsonResult AddBatchDetail(AddBatchModel addBatchModel)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.AddBatchDetail(addBatchModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Batch837)]
        public JsonResult GetApprovedPayorsList(string payorId)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.GetApprovedPayorsList(payorId);
            return JsonSerializer(response);
        }

        #endregion

        #region Batch List

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Batch837)]
        public JsonResult GetBatchList(SearchBatchList searchBatchList, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.GetBatchList(searchBatchList, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Batch837)]
        public JsonResult DeleteBatch(SearchBatchList searchBatchList, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.DeleteBatch(searchBatchList, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Batch837)]
        public JsonResult MarkAsSentBatch(SearchBatchList searchBatchList, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.MarkAsSentBatch(SessionHelper.LoggedInID, searchBatchList, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        #endregion

        #region OverView file

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Batch837)]
        public JsonResult DownloadOverViewFile(string batchid)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.GenrateOverViewFile(batchid, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpGet]
        public ActionResult DownloadFiles(string vpath, string fname)
        {
            Common.SendZipFileBytesToResponse(vpath, fname);
            return null;
        }

        #endregion

        #region Paper Remits/EOB Template

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Batch837)]
        public JsonResult GenratePaperRemitsEOBTemplate(string batchid)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.GenratePaperRemitsEOBTemplate(batchid, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion

        #endregion

        #region 837 AND 835 Processing

        #region Batch Validation And 837 Generation

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Batch837)]
        public JsonResult ValidateBatches(PostEdiValidateGenerateModel postEdiValidateGenerateModel)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.ValidateBatchesAndGenerateEdi837Files(postEdiValidateGenerateModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Batch837)]
        public JsonResult GenerateEdi837Files(PostEdiValidateGenerateModel postEdiValidateGenerateModel)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.ValidateBatchesAndGenerateEdi837Files(postEdiValidateGenerateModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion

        #region Upload 835 And Processing
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Upload835)]
        public ActionResult Upload835()
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.SetUpload835Page();
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Upload835)]
        public JsonResult SaveUpload835File(AddUpload835Model model)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.SaveUpload835File(model, Request, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Upload835)]
        public JsonResult GetUpload835FileList(SearchUpload835ListPage searchUpload835Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.GetUpload835FileList(searchUpload835Model, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Upload835)]
        public JsonResult DeleteUpload835File(SearchUpload835ListPage searchUpload835Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.DeleteUpload835File(searchUpload835Model, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult ProcessUpload835File(SearchUpload835ListPage searchUpload835Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.ProcessUpload835File(searchUpload835Model, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }


        [HttpGet]
        public ActionResult BackEndProcessUpload835File()
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.BackEndProcessUpload835File();
            return null;
        }



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Upload835)]
        public JsonResult SaveUpload835Comment(Upload835CommentModel upload835CommentModel)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.SaveUpload835Comment(upload835CommentModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion

        #endregion

        #region Reconcile 835 / EOB


        [CustomAuthorize(Permissions = Constants.Permission_Billing_Reconcile835)]
        public ActionResult Reconcile835()
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.SetReconcile835Page();
            //return ShowUserFriendlyPages(response) ?? View(response.Data);
            return ShowUserFriendlyPages(response) ?? View("Reconcile",response.Data);
        }

        //getupload835files
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Reconcile835)]
        public JsonResult GetUpload835Files(string searchText, int pageSize, string searchParam)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var model = Common.DeserializeObject<Uploaded835FileSearchParam>(searchParam);
            return Json(_iBatchDataProvider.GetUpload835Files(model.PayorID, pageSize, searchText));
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Reconcile835)]
        public JsonResult GetReconcile835List(SearchReconcile835ListPage searchReconcile835Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.GetReconcile835List(searchReconcile835Model, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetReconcileBatchNoteDetails(long BatchNoteID, long BatchID, long NoteID, long? Upload835FileID)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.GetReconcileBatchNoteDetails(BatchNoteID, BatchID, NoteID, Upload835FileID);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult MarkNoteAsLatest(long BatchNoteID, long BatchID, long NoteID, long? Upload835FileID)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.MarkNoteAsLatest(BatchNoteID, BatchID, NoteID, Upload835FileID);
            return JsonSerializer(response);
        }
        

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Reconcile835)]
        public JsonResult ExportReconcile835List(SearchReconcile835ListPage searchReconcile835Model, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            return JsonSerializer(_iBatchDataProvider.ExportReconcile835List(searchReconcile835Model, sortIndex, sortDirection));
        }



        [HttpPost]
        public JsonResult SetClaimAdjustmentFlag(long batchId, long noteId, string claimAdjustmentType, string claimAdjustmentReason)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.SetClaimAdjustmentFlag(batchId, noteId, claimAdjustmentType, claimAdjustmentReason);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult BulkSetClaimAdjustmentFlag(string itemId, string claimAdjustmentType, string claimAdjustmentReason)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.BulkSetClaimAdjustmentFlag(itemId, claimAdjustmentType, claimAdjustmentReason);
            return JsonSerializer(response);
        }




        #region New Chnages

        [CustomAuthorize(Permissions = Constants.Permission_Billing_Reconcile835)]
        public ActionResult Reconcile()
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.SetReconcile835Page();
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Reconcile835)]
        public JsonResult GetParentReconcileList(SearchReconcile835ListPage searchReconcile835Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.GetParentReconcileList(searchReconcile835Model, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetChildReconcileList(long NoteID)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.GetChildReconcileList( NoteID);
            return JsonSerializer(response);
        }


        [HttpPost]
        public JsonResult MarkNoteAsLatest01(long BatchNoteID, long BatchID, long NoteID, long? Upload835FileID)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.MarkNoteAsLatest01(BatchNoteID, BatchID, NoteID, Upload835FileID);
            return JsonSerializer(response);
        }
        
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Reconcile835)]
        public JsonResult GetExportReconcileList(SearchReconcile835ListPage searchReconcile835Model, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            return JsonSerializer(_iBatchDataProvider.ExportReconcileList(searchReconcile835Model, sortIndex, sortDirection));
        }

        

        #endregion




        #endregion

        #region List EDI FILE LOGS

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_EDIFileLogs)]
        public ActionResult EdiFileLogList()
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.SetEdiFileLogListPage();
            return View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_EDIFileLogs)]
        public JsonResult GetEdiFileLogList(SearchEdiFileLogListPage searchEdiFileLogModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.GetEdiFileLogList(searchEdiFileLogModel, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_EDIFileLogs)]
        public JsonResult DeleteEdiFileLog(SearchEdiFileLogListPage searchEdiFileLogModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.DeleteEdiFileLog(searchEdiFileLogModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }


        #endregion

        #region Process 270/271


        //public ActionResult Process270_271(string fileid, string filename, string authcode, string userid, string fileextension, string boxurl,

        //    string username, string filedata)
        //{
        //    _iBatchDataProvider = new BatchDataProvider();
        //    ServiceResponse response = new ServiceResponse();

        //    response.Data = string.Format("<ul>" +
        //                                  "<li><b>File Id: </b>{0}</li>" +
        //                                  "<li><b>File Name: </b>{1}</li>" +
        //                                  "<li><b>Auth Code: </b>{2}</li>" +
        //                                  "<li><b>User Id: </b>{3}</li>" +
        //                                  "<li><b>User Name: </b>{4}</li>" +
        //                                  "<li><b>File Exte.: </b>{5}</li>" +
        //                                  "<li><b>Box Url: </b>{6}</li>" +
        //                                  "<li><b>File Data: </b>{7}</li>" +
        //                                  "</ul>", fileid, filename, authcode, userid, username, fileextension, boxurl, filedata);
        //    return View(response);
        //}


        #region Process 270

        [CustomAuthorize(Permissions = Constants.Permission_Billing_EDI270_271)]
        public ActionResult Process270_271()
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.Process270_271();
            return View(response.Data);
        }

        [CustomAuthorize(Permissions = Constants.Permission_Billing_EDI270_271)]
        public JsonResult Generate270File(AddProcess270Model model, SearchProcess270ListPage searchEdiFileLogListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.Generate270File(model, searchEdiFileLogListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_EDI270_271)]
        public JsonResult GetEdi270FileList(SearchProcess270ListPage searchProcess270Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.GetEdi270FileList(searchProcess270Model, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_EDI270_271)]
        public JsonResult DeleteEdi270File(SearchProcess270ListPage searchProcess270Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.DeleteEdi270File(searchProcess270Model, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion

        #region Process 271

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_EDI270_271)]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_Upload835)]
        public JsonResult Upload271file(AddProcess271Model model)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.Upload271File(model, Request, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_EDI270_271)]
        public JsonResult GetEdi271FileList(SearchProcess271ListPage searchProcess271Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.GetEdi271FileList(searchProcess271Model, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_EDI270_271)]
        public JsonResult DeleteEdi271File(SearchProcess271ListPage searchProcess271Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.DeleteEdi271File(searchProcess271Model, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion

        #endregion

        #region Process 277

        [CustomAuthorize(Permissions = Constants.Permission_Billing_EDI277CA)]
        public ActionResult Process277()
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.Process277();
            return View("Process277CA",response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_EDI277CA)]
        public JsonResult Upload277File(AddProcess277Model model)
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.Upload277File(model, Request, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }


        public ActionResult BackEndProcess277File()
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.BackEndProcess277File();
            return null;
        }
        

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_EDI277CA)]
        public JsonResult GetEdi277FileList(SearchProcess277ListPage searchProcess277Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.GetEdi277FileList(searchProcess277Model, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Billing_EDI277CA)]
        public JsonResult DeleteEdi277File(SearchProcess277ListPage searchProcess277Model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iBatchDataProvider = new BatchDataProvider();
            var response = _iBatchDataProvider.DeleteEdi277File(searchProcess277Model, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_Attendance)]
        public JsonResult Download277RedableFile(string id)
        {
            _iBatchDataProvider = new BatchDataProvider();
            ServiceResponse response = _iBatchDataProvider.Download277RedableFile(id);
            return JsonSerializer(response);
        }


        #endregion

    }
}
