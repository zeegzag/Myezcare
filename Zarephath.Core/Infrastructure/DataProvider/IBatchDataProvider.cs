using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IBatchDataProvider
    {
        #region Batch Creation And Listing

        #region Add Batch

        ServiceResponse SetAddBatchPage(long batchId);

        ServiceResponse AddBatchDetail(AddBatchModel addBatchModel, long loggedInUserId);

        ServiceResponse GetApprovedPayorsList(string payorid);

        #endregion

        #region Batch List

        ServiceResponse GetBatchList(SearchBatchList searchBatchList, int pageIndex, int pageSize, string sortIndex, string sortDirection);

        ServiceResponse DeleteBatch(SearchBatchList searchBatchList, int pageIndex, int pageSize, string sortIndex, string sortDirection);

        ServiceResponse MarkAsSentBatch(long loggedInUserId, SearchBatchList searchBatchList, int pageIndex, int pageSize, string sortIndex, string sortDirection);


        ServiceResponse GenrateOverViewFile(string csvBatchId, long loogedInID);
        ServiceResponse GenratePaperRemitsEOBTemplate(string csvBatchId, long loogedInID);

        #endregion

        #endregion

        #region Batch Validation And 837 Generation
        ServiceResponse ValidateBatchesAndGenerateEdi837Files(PostEdiValidateGenerateModel postEdiValidateGenerateModel, long loggedInUserId);
        #endregion

        #region Upload 835 And Processing
        ServiceResponse SetUpload835Page();
        ServiceResponse SaveUpload835File(AddUpload835Model model, HttpRequestBase httpRequestBase, long loggedInUserID);


        #region Upload 835 List

        ServiceResponse GetUpload835FileList(SearchUpload835ListPage searchUpload835Model, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteUpload835File(SearchUpload835ListPage searchUpload835Model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse ProcessUpload835File(SearchUpload835ListPage searchUpload835Model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse BackEndProcessUpload835File();
        ServiceResponse SaveUpload835Comment(Upload835CommentModel referralCommentModel, long loggedInId);
        #endregion

        #endregion

        #region Reconcile 835 / EOB
        ServiceResponse SetReconcile835Page();
        List<Upload835File> GetUpload835Files(long payorId, int pageSize, string searchText = null);
        ServiceResponse GetReconcile835List(SearchReconcile835ListPage searchReconcile835Model, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse ExportReconcile835List(SearchReconcile835ListPage searchReconcile835Model, string sortIndex = "", string sortDirection = "");
        ServiceResponse GetReconcileBatchNoteDetails(long BatchNoteID, long BatchID, long NoteID, long? Upload835FileID);
        ServiceResponse MarkNoteAsLatest(long BatchNoteID, long BatchID, long NoteID, long? Upload835FileID);
        
        ServiceResponse SetClaimAdjustmentFlag(long BatchNoteID, long BatchID, string ClaimAdjustmentType, string claimAdjustmentReason);
        ServiceResponse BulkSetClaimAdjustmentFlag(string ItemId, string ClaimAdjustmentType, string claimAdjustmentReason);

        ServiceResponse HC_UpdateBatchReconcile(long BatchNoteID, float PaidAmount, long ClaimStatusID, long ClaimStatusCodeID);


        #region New Change
        ServiceResponse GetParentReconcileList(SearchReconcile835ListPage searchReconcile835Model, int pageIndex,
            int pageSize, string sortIndex, string sortDirection);
        ServiceResponse GetChildReconcileList(long NoteID);
        ServiceResponse MarkNoteAsLatest01(long BatchNoteID, long BatchID, long NoteID, long? Upload835FileID);
        ServiceResponse ExportReconcileList(SearchReconcile835ListPage searchReconcile835Model, string sortIndex = "", string sortDirection = "");

        #endregion


        #endregion

        #region EDI File Logs List

        ServiceResponse SetEdiFileLogListPage();
        ServiceResponse GetEdiFileLogList(SearchEdiFileLogListPage searchEdiFilesLogListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteEdiFileLog(SearchEdiFileLogListPage searchEdiFilesLogListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);

        #endregion

      
        #region Process 270/271
        ServiceResponse Process270_271();
        ServiceResponse BackEndProcess277File();
        ServiceResponse Generate270File(AddProcess270Model model,SearchProcess270ListPage searchEdiFileLogListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId);
        ServiceResponse GetEdi270FileList(SearchProcess270ListPage searchProcess270Model, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteEdi270File(SearchProcess270ListPage searchProcess270Model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);



        ServiceResponse Upload271File(AddProcess271Model model, HttpRequestBase request, long loggedInUserId);
        ServiceResponse GetEdi271FileList(SearchProcess271ListPage searchProcess271Model, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteEdi271File(SearchProcess271ListPage searchProcess271Model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId);
        ServiceResponse Download277RedableFile(string id);
        #endregion




        #region Process 277
        ServiceResponse Process277();
        ServiceResponse Upload277File(AddProcess277Model model, HttpRequestBase request, long loggedInUserId);
        ServiceResponse GetEdi277FileList(SearchProcess277ListPage searchProcess277Model, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteEdi277File(SearchProcess277ListPage searchProcess277Model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId);
        #endregion

        #region HomeCare Related Code

        #region Batch 837
        ServiceResponse HC_SetAddBatchPage(long batchId);

        ServiceResponse HC_AddBatchDetail(HC_AddBatchModel addBatchModel, long loggedInUserId, bool isCaseManagement = false);
        ServiceResponse HC_RefreshAndGroupingNotes();

        ServiceResponse HC_GetPatientList(SearchPatientList SearchPatientList, bool isCaseManagement);
        ServiceResponse HC_SaveBillingNote(BillingNoteModel BillingNoteModel,long LoggedInID);
        ServiceResponse HC_GetBillingNote(long BatchID);
        ServiceResponse HC_UpdateBillingNote(BillingNoteModel updateNoteModel, long LoggedInID);
        ServiceResponse HC_DeleteBillingNote(string BillingNoteID, long BatchID);

        ServiceResponse HC_GetChildNoteDetails(SearchPatientList SearchPatientList, bool isCaseManagement);


        ServiceResponse HC_SaveMannualPaymentPostingDetails(MannualPaymentPostingModel model);

        ServiceResponse HC_GetBatchDetails(string BatchID);
        ServiceResponse HC_GetEdi837BatchDetails(string batchId, long loggedInUserId, bool isCaseManagement = false);
        

        ServiceResponse HC_GetBatchList(SearchBatchList searchBatchList, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeleteBatch(SearchBatchList searchBatchList, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_MarkAsSentBatch(long loggedInUserId, SearchBatchList searchBatchList, int pageIndex, int pageSize, string sortIndex, string sortDirection);

        ServiceResponse HC_GenrateOverViewFile(string csvBatchId, long loogedInID);
        ServiceResponse HC_GenratePaperRemitsEOBTemplate(string csvBatchId, long loogedInID);


        ServiceResponse HC_DeleteNote_Temporary(long noteID, bool permanentDelete, long loggedInID);

        

        #region Batch Validation And 837 Generation
        ServiceResponse HC_ValidateBatchesAndGenerateEdi837Files(PostEdiValidateGenerateModel postEdiValidateGenerateModel, long loggedInUserId, bool isCaseManagement = false);

        ServiceResponse GenerateCMS1500(EDIFileSearchModel EDIFileSearchModellong, bool isCaseManagement, long loggedInId);
        ServiceResponse GenerateUB04(EDIFileSearchModel EDIFileSearchModel);
        ServiceResponse UploadClaims(BatchValidationResponseModel claimModel, long loggedInUserId);
        
        ServiceResponse SyncClaimMessages(bool isSyncAllowed=false,string syncall="");
        ServiceResponse GetClaimMessageList(ClaimModel model);
        //ServiceResponse GetClaimMessages(string claimId, string claimMD_Id, string batchId);

        #endregion

        #region Upload 835 And Processing
        ServiceResponse HC_SetUpload835Page();
        ServiceResponse HC_SaveUpload835File(AddUpload835Model model, HttpRequestBase httpRequestBase, long loggedInUserID);
        ServiceResponse HC_GetUpload835FileList(SearchUpload835ListPage searchUpload835Model, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeleteUpload835File(SearchUpload835ListPage searchUpload835Model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse HC_ProcessUpload835File(SearchUpload835ListPage searchUpload835Model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse HC_SaveUpload835Comment(Upload835CommentModel upload835CommentModel, long loggedInId);
        ServiceResponse HC_BackEndProcessUpload835File();
        #endregion

        #endregion






        #region Reconcile 835 / EOB
        ServiceResponse HC_SetReconcile835Page();
        List<Upload835File> HC_GetUpload835Files(long payorId, int pageSize, string searchText = null);
        ServiceResponse HC_GetReconcile835List(SearchReconcile835ListPage searchReconcile835Model, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_GetReconcileBatchNoteDetails(long BatchNoteID, long BatchID, long NoteID, long? Upload835FileID);
        ServiceResponse HC_MarkNoteAsLatest(long BatchNoteID, long BatchID, long NoteID, long? Upload835FileID);
        ServiceResponse HC_ExportReconcile835List(SearchReconcile835ListPage searchReconcile835Model, string sortIndex = "", string sortDirection = "");
        ServiceResponse HC_SetClaimAdjustmentFlag(long batchId, long noteId, string claimAdjustmentType, string claimAdjustmentReason);
        ServiceResponse HC_BulkSetClaimAdjustmentFlag(BulkClaimAdjustmentFlagModel model);

        ServiceResponse HC_GetReconcileList(SearchReconcile835ListPage searchReconcile835Model, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_GetParentReconcileList(SearchReconcile835ListPage searchReconcile835Model, int fromIndex, int pageSize);
        ServiceResponse HC_GetChildReconcileList(long noteId, long batchId);
        ServiceResponse HC_MarkNoteAsLatest01(long BatchNoteID, long BatchID, long NoteID, long? Upload835FileID);
        ServiceResponse HC_ExportReconcileList(SearchReconcile835ListPage searchReconcile835Model, string sortIndex = "", string sortDirection = "");
        #endregion
        #region EDI File Logs List

        ServiceResponse HC_SetEdiFileLogListPage();
        ServiceResponse HC_GetEdiFileLogList(SearchEdiFileLogListPage searchEdiFilesLogListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeleteEdiFileLog(SearchEdiFileLogListPage searchEdiFilesLogListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);

        #endregion







        #region Process 270/271
        ServiceResponse HC_Process270_271();
        ServiceResponse HC_Generate270File(AddProcess270Model model, SearchProcess270ListPage searchEdiFileLogListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId);
        ServiceResponse HC_GetEdi270FileList(SearchProcess270ListPage searchProcess270Model, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeleteEdi270File(SearchProcess270ListPage searchProcess270Model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);



        ServiceResponse HC_Upload271File(AddProcess271Model model, HttpRequestBase request, long loggedInUserId);
        ServiceResponse HC_GetEdi271FileList(SearchProcess271ListPage searchProcess271Model, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeleteEdi271File(SearchProcess271ListPage searchProcess271Model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId);
        #endregion




        #endregion

        #region ManageClaims

        ServiceResponse GetUploadedClaims(ListManageClaimsModel searchClaimListModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId);
        ServiceResponse GetClaimErrorsList(long BatchUploadedClaimID);
        ServiceResponse GetClaimErrorsListAndCMS1500(long BatchUploadedClaimID, EDIFileSearchModel EDIFileSearchModellong,long loggedInId);
        ServiceResponse SaveCMS1500Data(SaveCMS1500Modal saveCMS1500Modal,GenerateEdiFileModel cmsModel);
        ServiceResponse GetLatestERA(long loggedInUserId);
        ServiceResponse GetLatestERAPDF(string eraId);
        ServiceResponse GetLatestERA835(string eraId);
        ServiceResponse ProcessERA835(string eraId, long loggedInUserID, bool forceProcess=false);
        
        ServiceResponse ArchieveClaim(string claimId);
        ServiceResponse HC_SetAddERAPage(long claimId);
        ServiceResponse HC_GetLatestERAList(SearchERAList SearchERAList, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_AddManageClaim();
        #endregion

        #region AR Aging Report
        ServiceResponse SetARAgingReportPage();
        ServiceResponse GetARAgingReport(SearchARAgingReportPage model);
        ServiceResponse ExportARAgingReportList(SearchARAgingReportPage model);

        #endregion

    }
}
