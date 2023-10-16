using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDI_837_835_HCCP.Models;
using ExportToExcel;
using ExpressiveAnnotations.Attributes;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    #region Bacth  Model

    public class AddBatchModel
    {
        public AddBatchModel()
        {
            Batch = new Batch();
            BatchTypeList = new List<BatchTypeList>();
            PayorList = new List<PayorList>();
            ApprovedFacilityList = new List<ApprovedFacilityList>();
            ServiceCodeList=new List<ServiceCodeList>();
            BatchStatusFilter = new List<NameValueData>();
        }
        public Batch Batch { get; set; }
        public List<BatchTypeList> BatchTypeList { get; set; }
        public List<PayorList> PayorList { get; set; }
        public List<ApprovedFacilityList> ApprovedFacilityList { get; set; }
        public List<ServiceCodeList> ServiceCodeList { get; set; }
        [Ignore]
        public List<NameValueData> BatchStatusFilter { get; set; }
    }

    public class HC_AddBatchModel
    {
        public HC_AddBatchModel()
        {
            OrganizationSetting = new OrganizationSetting();
            Batch = new Batch();
            BatchTypeList = new List<BatchTypeList>();
            PayorList = new List<PayorList>();
            ServiceCodeList = new List<ServiceCodeList>();
            BatchStatusFilter = new List<NameValueData>();
            SearchPatientList = new SearchPatientList();
            SearchBatchList = new SearchBatchList();
            MPPAdjustmentTypes = new List<NameValueDataInString> ();
        }
        public OrganizationSetting OrganizationSetting { get; set; }
        public Batch Batch { get; set; }
        public List<BatchTypeList> BatchTypeList { get; set; }
        public List<PayorList> PayorList { get; set; }
        public List<ServiceCodeList> ServiceCodeList { get; set; }

        public List<NameValueDataInString> MPPAdjustmentTypes { get; set; }

        [Ignore]
        public List<NameValueData> BatchStatusFilter { get; set; }
        [Ignore]
        public SearchPatientList SearchPatientList { get; set; }
        [Ignore]
        public SearchBatchList SearchBatchList { get; set; }
    }

    public class BatchTypeList
    {
        public long BatchTypeID { get; set; }
        public string BatchTypeName { get; set; }
    }

    public class PayorList
    {
        public string DisplayPayorName { set; get; }
        public long PayorID { get; set; }
        public string PayorName { get; set; }
        public string ShortName { get; set; }
        public int Precedence { get; set; }
    }

    public class ReferralAuthorizationServiceCodeList
    {
        //public long ReferralBillingAuthorizationID { get; set; }
        //public string ReferralBillingAuthorizationName { get; set; }
        public long ReferralBillingAuthorizationID { get; set; }
        public string ReferralBillingAuthorizationName { get; set; }
        public string Available { get; set; }
        public string Allocated { get; set; }
        public string Used { get; set; }
        public string Remaining { get; set; }
        public string Unallocated { get; set; }


        public DateTime StartDate { get; set; }
        public string StartDates
        {
            get
            {
                if (StartDate != null)
                {
                    return StartDate.ToString("MM/dd/yyyy");

                }
                return string.Empty;
            }
        }
        public DateTime EndDate { get; set; }
        public string EndDates
        {
            get
            {
                if (EndDate != null)
                {
                    return EndDate.ToString("MM/dd/yyyy");

                }
                return string.Empty;
            }
        }
        public string ServiceCode { get; set; }
        public string CareType { get; set; }
    }

    public class ServiceCodeList
    {
        public long ServiceCodeID { get; set; }
        public string ServiceCode { get; set; }
    }


    public class ApprovedFacilityList
    {
        public long BillingProviderID { get; set; }
        public string FacilityName { get; set; }
    }

    public class BatchListPage
    {
        public BatchListPage()
        {
            SearchBatchList = new SearchBatchList();
            DeleteFilter = new List<NameValueData>();
        }
        public SearchBatchList SearchBatchList { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
    }

    public class SearchBatchList
    {
        public long BatchID { get; set; }

        public bool BatchView{ get; set; }
        public string StrBatchID { get; set; }
        public long? BatchTypeID { get; set; }
        public long PayorID { get; set; }
        public string BillingProviderIDS { get; set; }
        public string Comment { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int IsDeleted { get; set; }
        public bool IsSent { get; set; }
        public int IsSentStatus { get; set; }
        public DateTime SentDate { get; set; }
        public string ListOfIdsInCSV { get; set; }

        public string ClientName { get; set; }


        public string ClaimAdjustmentTypeID { get; set; }
        public string ReconcileStatus { get; set; }
        
    }

    public class SearchPatientList
    {
        public long BatchID { get; set; }

        [Required(ErrorMessageResourceName = "BatchTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long BatchTypeID { get; set; }

        [Required(ErrorMessageResourceName = "PayorRequired", ErrorMessageResourceType = typeof(Resource))]
        public long PayorID { get; set; }

        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime EndDate { get; set; }

        public string ServiceCodeIDs { get; set; }

        public string ClientName { get; set; }

        public long ReferralID { get; set; }

        public string Comment { get; set; }

        public bool CreatePatientWiseBatch{ get; set; }

        public string NoteIDs { get; set; }
    }

    public class ListPatientModelClaims
    {
        public ListPatientModelClaims()
        {
            ListPatientModelCount = new List<ListPatientModelCount>();
            ListPatientModel = new List<ListPatientModel>();
        }
        public List<ListPatientModelCount> ListPatientModelCount { get; set; }
        public List<ListPatientModel> ListPatientModel { get; set; }


    }
    public class ListPatientModelCount
    {
        public long Count { get; set; }
    }

    public class ListPatientModel
    {
        public long ReferralID { get; set; }

        public string PatientName { get; set; }
        public long TotalClaims { get; set; }

        public string AuthorizationCode { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalBilledAmount { get; set; }
        public decimal TotalAllowedAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }

        public decimal TotalMPP_AdjustmentAmount { get; set; }

        public long BatchID { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public DateTime Dob { get; set; }
        public long PayorID { get; set; }
        public string PayorBillingType { get; set; }
        public string strDOB
        {
            get
            {
                return Dob.ToString("MM/dd/yyyy");
            }
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string StrNoteIds { get; set; }
    }

    public class ChildNoteListMainModel { 

        public string ClaimSubmitterID { get; set; }
        public List<ChildNoteListModel> ChildNoteListModel { get; set; }
    }

    public class ChildNoteListModel
    {
        public long NoteID { get; set; }
        public string EmployeeName { get; set; }
        public string ServiceCode { get; set; }
        public string CareType { get; set; }
        public int CalculatedUnit { get; set; }
        public decimal CalculatedAmount { get; set; }


        public decimal ClaimBilledAmount { get; set; }
        public decimal ClaimAllowedAmount { get; set; }
        public decimal ClaimPaidAmount { get; set; }

        public string ClaimStatus { get; set; }
        
        public long BatchNoteID{ get; set; }


        public string CalculatedServiceTime { get; set; }
        public float Rate { get; set; }
        public DateTime ServiceDate { get; set; }
        public string strServiceDate
        {
            get
            {
                return ServiceDate.ToString("MM/dd/yyyy");
            }
        }


        public string PayorClaimNumber { get; set; }

        public long BatchID { get; set; }
        public string ClaimAdjustmentTypeID { get; set; }
        public string ClaimAdjustmentReason { get; set; }


        public long ClaimStatusCodeID { get; set; }
        public string ClaimStatusName { get; set; }
        public string ClaimStatusCodeDescription { get; set; }
        public string ClaimAdjustmentGroupCodeID { get; set; }
        public string ClaimAdjustmentGroupCodeName { get; set; }
        public string ClaimAdjustmentGroupCodeDescription { get; set; }
        public string ClaimAdjustmentReasonCodeID { get; set; }
        public string ClaimAdjustmentReasonDescription { get; set; }
        
        public string AdjustmentAmount { get; set; }

        //COmbination Of AdjustmentCode
        public string AdjustmentCode
        {
            get
            {
                if (!string.IsNullOrEmpty(ClaimAdjustmentGroupCodeID) &&
                    !string.IsNullOrEmpty(ClaimAdjustmentReasonCodeID))
                    return string.Format("{0}: {1}", ClaimAdjustmentGroupCodeID, ClaimAdjustmentReasonCodeID);

                if (!string.IsNullOrEmpty(ClaimAdjustmentGroupCodeID) &&
                    string.IsNullOrEmpty(ClaimAdjustmentReasonCodeID))
                    return ClaimAdjustmentGroupCodeID;

                if (string.IsNullOrEmpty(ClaimAdjustmentGroupCodeID) &&
                    !string.IsNullOrEmpty(ClaimAdjustmentReasonCodeID))
                    return ClaimAdjustmentReasonCodeID;

                return "";
            }
        }

        public string AdjustmentCodeDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(ClaimAdjustmentGroupCodeName) &&
                    !string.IsNullOrEmpty(ClaimAdjustmentReasonDescription))
                    return string.Format("{0}: {1}", ClaimAdjustmentGroupCodeName, ClaimAdjustmentReasonDescription);

                if (!string.IsNullOrEmpty(ClaimAdjustmentGroupCodeName) &&
                    string.IsNullOrEmpty(ClaimAdjustmentReasonDescription))
                    return ClaimAdjustmentGroupCodeName;

                if (string.IsNullOrEmpty(ClaimAdjustmentGroupCodeName) &&
                    !string.IsNullOrEmpty(ClaimAdjustmentReasonDescription))
                    return ClaimAdjustmentReasonDescription;

                return "";
            }
        }

        public string AdjustmentGroup
        {
            get
            {
                return string.Format("{0}: {1}",
                    string.IsNullOrEmpty(ClaimAdjustmentGroupCodeID) ? Resource.NA : ClaimAdjustmentGroupCodeID,
                    string.IsNullOrEmpty(ClaimAdjustmentGroupCodeName) ? Resource.NA : ClaimAdjustmentGroupCodeName);
            }
        }
        public string AdjustmentReason
        {
            get
            {
                return string.Format("{0}: {1}",
                    string.IsNullOrEmpty(ClaimAdjustmentReasonCodeID) ? Resource.NA : ClaimAdjustmentReasonCodeID,
                    string.IsNullOrEmpty(ClaimAdjustmentReasonDescription) ? Resource.NA : ClaimAdjustmentReasonDescription);
            }
        }

        public string AuthorizationCode { set; get; }



        public string MPP_AdjustmentGroupCodeID { set; get; }
        public string MPP_AdjustmentGroupCodeName { set; get; }
        public decimal MPP_AdjustmentAmount { set; get; }

        public string MPP_AdjustmentComment { set; get; }

    }



    public class MannualPaymentPostingModel
    {
        [Required(ErrorMessageResourceName = "AdjustmentTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        //[RequiredIf("MPP_AdjustmentGroupCodeID == NA", ErrorMessageResourceName = "AdjustmentTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string MPP_AdjustmentGroupCodeID { set; get; }
        public string MPP_AdjustmentGroupCodeName { set; get; }

        [Required(ErrorMessageResourceName = "AdjustmentAmountRequired", ErrorMessageResourceType = typeof(Resource))]
        public decimal MPP_AdjustmentAmount { set; get; }

        [Required(ErrorMessageResourceName = "AdjustmentAmountRequired", ErrorMessageResourceType = typeof(Resource))]
        public string MPP_AdjustmentComment { set; get; }

        public long NoteID { get; set; }
        public string ServiceCode { get; set; }
        public string CareType { get; set; }
        public int CalculatedUnit { get; set; }
        public decimal CalculatedAmount { get; set; }
        public decimal ClaimBilledAmount { get; set; }
        public decimal ClaimAllowedAmount { get; set; }
        public decimal ClaimPaidAmount { get; set; }
        public string ClaimStatus { get; set; }
        public long BatchNoteID { get; set; }
        public string CalculatedServiceTime { get; set; }
        public float Rate { get; set; }
        public DateTime ServiceDate { get; set; }
        public string strServiceDate
        {
            get
            {
                return ServiceDate.ToString("MM/dd/yyyy");
            }
        }

        public long BatchID { get; set; }
        public string ClaimAdjustmentTypeID { get; set; }
        public string ClaimAdjustmentReason { get; set; }


        public long ClaimStatusCodeID { get; set; }
        public string ClaimStatusName { get; set; }
        public string ClaimStatusCodeDescription { get; set; }
        public string ClaimAdjustmentGroupCodeID { get; set; }
        public string ClaimAdjustmentGroupCodeName { get; set; }
        public string ClaimAdjustmentGroupCodeDescription { get; set; }
        public string ClaimAdjustmentReasonCodeID { get; set; }
        public string ClaimAdjustmentReasonDescription { get; set; }

        public string AdjustmentAmount { get; set; }

        //COmbination Of AdjustmentCode
        public string AdjustmentCode
        {
            get
            {
                if (!string.IsNullOrEmpty(ClaimAdjustmentGroupCodeID) &&
                    !string.IsNullOrEmpty(ClaimAdjustmentReasonCodeID))
                    return string.Format("{0}: {1}", ClaimAdjustmentGroupCodeID, ClaimAdjustmentReasonCodeID);

                if (!string.IsNullOrEmpty(ClaimAdjustmentGroupCodeID) &&
                    string.IsNullOrEmpty(ClaimAdjustmentReasonCodeID))
                    return ClaimAdjustmentGroupCodeID;

                if (string.IsNullOrEmpty(ClaimAdjustmentGroupCodeID) &&
                    !string.IsNullOrEmpty(ClaimAdjustmentReasonCodeID))
                    return ClaimAdjustmentReasonCodeID;

                return "";
            }
        }

        public string AdjustmentCodeDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(ClaimAdjustmentGroupCodeName) &&
                    !string.IsNullOrEmpty(ClaimAdjustmentReasonDescription))
                    return string.Format("{0}: {1}", ClaimAdjustmentGroupCodeName, ClaimAdjustmentReasonDescription);

                if (!string.IsNullOrEmpty(ClaimAdjustmentGroupCodeName) &&
                    string.IsNullOrEmpty(ClaimAdjustmentReasonDescription))
                    return ClaimAdjustmentGroupCodeName;

                if (string.IsNullOrEmpty(ClaimAdjustmentGroupCodeName) &&
                    !string.IsNullOrEmpty(ClaimAdjustmentReasonDescription))
                    return ClaimAdjustmentReasonDescription;

                return "";
            }
        }

        public string AdjustmentGroup
        {
            get
            {
                return string.Format("{0}: {1}",
                    string.IsNullOrEmpty(ClaimAdjustmentGroupCodeID) ? Resource.NA : ClaimAdjustmentGroupCodeID,
                    string.IsNullOrEmpty(ClaimAdjustmentGroupCodeName) ? Resource.NA : ClaimAdjustmentGroupCodeName);
            }
        }
        public string AdjustmentReason
        {
            get
            {
                return string.Format("{0}: {1}",
                    string.IsNullOrEmpty(ClaimAdjustmentReasonCodeID) ? Resource.NA : ClaimAdjustmentReasonCodeID,
                    string.IsNullOrEmpty(ClaimAdjustmentReasonDescription) ? Resource.NA : ClaimAdjustmentReasonDescription);
            }
        }

        public string AuthorizationCode { set; get; }

    }


    public class ListBatchModel
    {
        public long BatchID { get; set; }
        public long BatchTypeID { get; set; }
        public long NoteID { get; set; }
        public long Gathered { get; set; }
        public long BillingGathered { get; set; }
        public decimal Amount { get; set; }
        public decimal BillingAmount { get; set; }

        public decimal AllowedAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal MPP_AdjustedAmount { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime GatherDate { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? SentDate { get; set; }

        public DateTime? ServiceStartDate { get; set; }
        public DateTime? ServiceEndDate { get; set; }

        public string BatchTypeName { get; set; }
        public string PayorName { get; set; }
        public string FacilityName { get; set; }
        public string GatheredBy { get; set; }
        public string Cleard { get; set; }
        public string IsSentBy { get; set; }
        public string EncryptedBatchID { get { return Crypto.Encrypt(Convert.ToString(BatchID)); } }

        public int Count { get; set; }
        public int EmpCount { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSent { get; set; }

        public string PayorBillingType { get; set; }

        public string Comment { get; set; }


        public string EraIDs { get; set; }

        [Ignore]
        public List<string> ListEraID { get { if (string.IsNullOrEmpty(EraIDs)) return null; else  return EraIDs.Split(',').ToList<string>(); } }

    }

    public class ListNoteDetail
    {
        public long NoteID { get; set; }
    }

    public class PostEdiValidateGenerateModel
    {

        public string ListOfBacthIdsInCsv { get; set; }
        public string FileExtension { get; set; }
        public bool GenerateEdiFile { get; set; }
        public EdiFileType EdiFileType { get; set; }
    }

    /// <summary>
    /// TODO: I have added same enum in EDI_837_835_HCCP > Models > EdiFileType.cs file 
    /// please maintain name and sequance of both
    /// </summary>
    //public enum EdiFileType : byte
    //{
    //    [Display(ResourceType = typeof(Resource), Name = "P")]
    //    Edi_837_P = 1,
    //    [Display(ResourceType = typeof(Resource), Name = "I")]
    //    Edi_837_I,
    //}

    public class BatchValidationResponseModel
    {
        public long BatchID { get; set; }
        public string FileName { get; set; }
        public long EdiFileTypeID { get; set; }
        public bool ValidationPassed { get; set; }
        public bool Edi837GenerationPassed { get; set; }
        public string ValidationErrorFilePath { get; set; }
        public string Edi837FilePath { get; set; }
    }

    #endregion


    #region Upload 835 Model

    public class AddUpload835Model
    {
        public AddUpload835Model()
        {
            PayorList = new List<PayorList>();
            FileProcessStatus = new List<NameValueData>();
            SearchUpload835ListPage = new SearchUpload835ListPage();
        }
        public List<PayorList> PayorList { get; set; }

        public List<NameValueData> FileProcessStatus { get; set; }

        [Ignore]
        [Required(ErrorMessageResourceName = "A835TemplateTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string A835TemplateType { get; set; }

        [Ignore]
        [Required(ErrorMessageResourceName = "PayorRequired", ErrorMessageResourceType = typeof(Resource))]
        public long PayorID { get; set; }

        [Ignore]
        public string Comment { get; set; }

        [Ignore]
        [Required(ErrorMessageResourceName = "Upload835FileRequired", ErrorMessageResourceType = typeof(Resource))]
        public string TempFilePath { get; set; }

        [Ignore]
        public SearchUpload835ListPage SearchUpload835ListPage { get; set; }
    }


    public class SearchUpload835ListPage
    {
        public long Upload835FileID { get; set; }
        public long PayorID { get; set; }
        public string A835TemplateType { get; set; }
        public int Upload835FileProcessStatus { get; set; }
        public string FileName { get; set; }
        public string Comment { get; set; }
        public string FilePath { get; set; }
        public string ListOfIdsInCSV { get; set; }
        public int IsDeleted { get; set; }
    }

    public class Upload835CommentModel
    {
        public long Upload835FileID { get; set; }
        public string Comment { get; set; }
    }




    public class ListUpload835Model
    {
        public long Upload835FileID { get; set; }
        public long PayorID { get; set; }
        public string PayorName { get; set; }
        public long? BatchID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileSize { get; set; }
        public string Comment { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsProcessed { get; set; }
        public string A835TemplateType { get; set; }
        public int Upload835FileProcessStatus { get; set; }
        [Ignore]
        public String StringUpload835FileProcessStatus
        {
            get { return Common.GetEnumDisplayValue((EnumUpload835FileProcessStatus)Upload835FileProcessStatus); }
        }
        public bool IsDeleted { get; set; }
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime AddedDate { get; set; }
        public int Count { get; set; }
      //  public string StrDisplayName { get { return Common.GetGeneralNameFormat(FirstName, LastName); } }
        public string StrDisplayName { get; set; }
        public string StrFileSize { get { return Common.GetFileSize(Convert.ToDecimal(FileSize), Constants.SizeIn_KB); } }
        [Ignore]
        public string AWSSignedFilePath
        {
            get
            {
                //AmazonFileUpload az = new AmazonFileUpload();
                //return az.GetPreSignedUrl(ConfigSettings.ZarephathBucket, FilePath);
                CacheHelper ch = new CacheHelper();
                return ch.SiteBaseURL + FilePath;
            }
        }
        public string ReadableFilePath { get; set; }
        public string AWSReadableFilePath
        {
            get
            {
                //AmazonFileUpload az = new AmazonFileUpload();
                //return az.GetPreSignedUrl(ConfigSettings.ZarephathBucket, ReadableFilePath);

                if (string.IsNullOrEmpty(ReadableFilePath))
                    return ReadableFilePath;


                CacheHelper ch = new CacheHelper();
                return ch.SiteBaseURL + ReadableFilePath;
            }
        }


        public string LogFilePath { get; set; }
        public string AWSLogFilePath
        {
            get
            {
                //AmazonFileUpload az = new AmazonFileUpload();
                //return az.GetPreSignedUrl(ConfigSettings.ZarephathBucket, ReadableFilePath);

                if (string.IsNullOrEmpty(LogFilePath))
                    return LogFilePath;

                CacheHelper ch = new CacheHelper();
                return ch.SiteBaseURL + LogFilePath;
            }
        }

        public string EraMappedBatches { get; set; }
    }

    #endregion

    #region Reconcile 835 Model

    public class AddReconcile835Model
    {
        public AddReconcile835Model()
        {
            PayorList = new List<PayorList>();
            ModifierList = new List<Modifier>();
            PlaceOfServiceList = new List<PlaceOfService>();
            ClaimStatusCodeList = new List<ClaimStatusCode>();
            ClaimAdjustmentGroupCodeList = new List<ClaimAdjustmentGroupCode>();
            ClaimAdjustmentReasonCodeList = new List<ClaimAdjustmentReasonCode>();
            SearchReconcile835ListPage = new SearchReconcile835ListPage();
            Get835ProcessedOnlyList = Common.Set835ProcessedOnlyList();
            ServiceCodeList=new List<ServiceCodeList>();
        }
        public List<PayorList> PayorList { get; set; }
        public List<Modifier> ModifierList { get; set; }
        public List<PlaceOfService> PlaceOfServiceList { get; set; }
        public List<ClaimStatusCode> ClaimStatusCodeList { get; set; }
        public List<ClaimAdjustmentGroupCode> ClaimAdjustmentGroupCodeList { get; set; }
        public List<ClaimAdjustmentReasonCode> ClaimAdjustmentReasonCodeList { get; set; }
        public List<ServiceCodeList> ServiceCodeList { get; set; }
        [Ignore]
        public List<NameValueData> Get835ProcessedOnlyList { get; set; }

        [Ignore]
        public long PayorID { get; set; }
        [Ignore]
        public long ModifierID { get; set; }
        [Ignore]
        public long PosID { get; set; }
        [Ignore]
        public long ClaimStatusCodeID { get; set; }
        [Ignore]
        public string ClaimAdjustmentGroupCodeID { get; set; }
        [Ignore]
        public string ClaimAdjustmentReasonCodeID { get; set; }


        [Ignore]
        public SearchReconcile835ListPage SearchReconcile835ListPage { get; set; }

        [Ignore]
        public List<NameValueDataInString> Services { get; set; }

        [Ignore]
        public List<NameValueDataInString> ClaimAdjustmentTypes { get; set; }


    }

    public class SearchGetParentReconcileList
    {
        public long ReferralID { get; set; }
        public int FromIndex { get; set; }
        public int PageSize { get; set; }
        public DateTime? ServiceStartDate { get; set; }
        public DateTime? ServiceEndDate { get; set; }
    }

    public class SearchReconcile835ListPage
    {
        //public SearchReconcile835ListPage()
        //{
        //    Str835ProcessedOnlyID = 2;
        //}
        public long ReferralID { get; set; }
        public long BatchNoteID { get; set; }
        public long PayorID { get; set; }
        public string Batch { get; set; }
        public string ClaimNumber { get; set; }
        public string Client { get; set; }
        public string ServiceCode { get; set; }
        public DateTime? ServiceStartDate { get; set; }
        public DateTime? ServiceEndDate { get; set; }
        public long ModifierID { get; set; }
        public string ServiceID { get; set; }
        public string StrServiceCodeID { get; set; }
        public string ClaimAdjustmentTypeID { get; set; }
        public long PosID { get; set; }
        public long ClaimStatusCodeID { get; set; }
        public string Status { get; set; }
        public string ClaimAdjustmentGroupCodeID { get; set; }
        public string ClaimAdjustmentReasonCodeID { get; set; }
        public long Str835ProcessedOnlyID { get; set; }
        public long Upload835FileID { get; set; }



        public string NoteID { get; set; }
        public string PayorClaimNumber { get; set; }
        

    }



    public class BulkClaimAdjustmentFlagModel
    {
        public string ReferralIDs { get; set; }
        public string NoteIDs { get; set; }
        public string BatchIDs{ get; set; }
        
        public string AdjustmentType { get; set; }
        public string AdjustmentReason { get; set; }


    }

    public class ListReconcileModel
    {
        public long ReferralID { get; set; }
        public long EmployeeVisitID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Employee { get; set; }
        public string Patient { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public DateTime Dob { get; set; }
        public int Count { get; set; }
        public float BilledAmount { get; set; }
        public int NoteCount { get; set; }
    }


    public class ListReconcile835Model
    {
        public long BatchID { get; set; }
        public long BatchNoteID { get; set; }
        public long NoteID { get; set; }

        public string Status { get; set; }
        public string ClaimType { get; set; }
        
        public string ClaimStatus { get; set; }

        public string ClaimNumber { get; set; }
        public string PayorClaimNumber { get; set; }
        public string BillingProviderNPI { get; set; }
        public string BillingProvider { get; set; }

        public string ClientName { get; set; }
        public DateTime? ClientDob { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public string S277 { get; set; }
        public string S277CA { get; set; }



        public string Population { get; set; }
        public string Title { get; set; }
        public DateTime? ServiceDate { get; set; }
        public string ServiceCode { get; set; }
        public string Modifier { get; set; }
        public string PosID { get; set; }
        public string CalculatedUnit { get; set; }
        public string BilledAmount { get; set; }
        public string AllowedAmount { get; set; }
        public string AdjustmentAmount { get; set; }

        public string PaidAmount { get; set; }

        public long ClaimStatusCodeID { get; set; }
        public string ClaimStatusName { get; set; }
        public string ClaimStatusCodeDescription { get; set; }
        public string ClaimAdjustmentGroupCodeID { get; set; }
        public string ClaimAdjustmentGroupCodeName { get; set; }
        public string ClaimAdjustmentGroupCodeDescription { get; set; }
        public string ClaimAdjustmentReasonCodeID { get; set; }
        public string ClaimAdjustmentReasonDescription { get; set; }
        public string ClaimAdjustmentTypeID { get; set; }

        public bool MarkAsLatest { get; set; }
        


        //COmbination Of AdjustmentCode
        public string AdjustmentCode
        {
            get
            {
                if (!string.IsNullOrEmpty(ClaimAdjustmentGroupCodeID) &&
                    !string.IsNullOrEmpty(ClaimAdjustmentReasonCodeID))
                    return string.Format("{0}: {1}", ClaimAdjustmentGroupCodeID, ClaimAdjustmentReasonCodeID);

                if (!string.IsNullOrEmpty(ClaimAdjustmentGroupCodeID) &&
                    string.IsNullOrEmpty(ClaimAdjustmentReasonCodeID))
                    return ClaimAdjustmentGroupCodeID;

                if (string.IsNullOrEmpty(ClaimAdjustmentGroupCodeID) &&
                    !string.IsNullOrEmpty(ClaimAdjustmentReasonCodeID))
                    return ClaimAdjustmentReasonCodeID;

                return "";
            }
        }

        public string AdjustmentCodeDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(ClaimAdjustmentGroupCodeName) &&
                    !string.IsNullOrEmpty(ClaimAdjustmentReasonDescription))
                    return string.Format("{0}: {1}", ClaimAdjustmentGroupCodeName, ClaimAdjustmentReasonDescription);

                if (!string.IsNullOrEmpty(ClaimAdjustmentGroupCodeName) &&
                    string.IsNullOrEmpty(ClaimAdjustmentReasonDescription))
                    return ClaimAdjustmentGroupCodeName;

                if (string.IsNullOrEmpty(ClaimAdjustmentGroupCodeName) &&
                    !string.IsNullOrEmpty(ClaimAdjustmentReasonDescription))
                    return ClaimAdjustmentReasonDescription;

                return "";
            }
        }

        public string AdjustmentGroup
        {
            get
            {
                return string.Format("{0}: {1}",
                    string.IsNullOrEmpty(ClaimAdjustmentGroupCodeID) ? Resource.NA : ClaimAdjustmentGroupCodeID,
                    string.IsNullOrEmpty(ClaimAdjustmentGroupCodeName) ? Resource.NA : ClaimAdjustmentGroupCodeName);
            }
        }
        public string AdjustmentReason
        {
            get
            {
                return string.Format("{0}: {1}",
                    string.IsNullOrEmpty(ClaimAdjustmentReasonCodeID) ? Resource.NA : ClaimAdjustmentReasonCodeID,
                    string.IsNullOrEmpty(ClaimAdjustmentReasonDescription) ? Resource.NA : ClaimAdjustmentReasonDescription);
            }
        }



        public DateTime? LoadDate { get; set; }

        public string Payor { get; set; }

        public DateTime? ExtractDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string DXCode { get; set; }

        public string CheckNumber { get; set; }
        public string CheckAmount { get; set; }
        public string CheckInfomration
        {
            get
            {
                return string.Format("{0}",
                    string.IsNullOrEmpty(CheckNumber) ? Resource.NA : CheckNumber);
            }
        }

        public string ClaimAdjustmentReason { get; set; }
        public int Count { get; set; }

    }

    public class ReconcileBatchNoteDetailsModel
    {
        public ReconcileBatchNoteDetailsModel()
        {
            AdjudicationDetailsModelList = new List<ListReconcile835Model>();
            ClaimSubmissionDetailsModel = new ClaimSubmissionDetailsModel();
        }

        public List<ListReconcile835Model> AdjudicationDetailsModelList { get; set; }
        public ClaimSubmissionDetailsModel ClaimSubmissionDetailsModel { get; set; }
    }



    public class ChildReconcileListModel
    {
        public ChildReconcileListModel()
        {
            AdjudicationDetailsModelList = new List<ListReconcile835Model>();
            BatchHistoryModelList = new List<BatchHistoryModel>();
            NoteHistoryModel = new NoteHistoryModel();
        }

        public List<ListReconcile835Model> AdjudicationDetailsModelList { get; set; }
        public List<BatchHistoryModel> BatchHistoryModelList { get; set; }
        public NoteHistoryModel NoteHistoryModel { get; set; }
    }


    public class BatchHistoryModel
    {
        public long BatchID { get; set; }
        public DateTime BatchStartDate { get; set; }
        public DateTime BatchEndDate { get; set; }
        public DateTime? SentDate { get; set; }
        public string SentBy { get; set; }
        public DateTime GatherDate { get; set; }
        public string GatheredBy { get; set; }
        public string BatchPayorName { get; set; }
        public string BatchPayorShortName { get; set; }
        public string BatchTypeName { get; set; }
    }

    public class NoteHistoryModel
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string SignedBy { get; set; }
        public DateTime SignedDate { get; set; }
    }




    public class ClaimSubmissionDetailsModel
    {
        public long ReferralID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Gender { get; set; }
        public string ClientDob { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }

        public string Population { get; set; }
        public string Title { get; set; }


        public long NoteID { get; set; }
        public string NoteDetails { get; set; }
        public string Assessment { get; set; }
        public string ActionPlan { get; set; }


        public string ServiceCode { get; set; }
        public string PosID { get; set; }
        public string POSDetail { get; set; }
        public string ServiceDate { get; set; }
        public string ContinuedDX { get; set; }

        public string BillingProviderName { get; set; }
        public string BillingProviderNPI { get; set; }
        public string BillingProviderEIN { get; set; }
        public string BillingProviderAHCCCSID { get; set; }
        public string BillingProviderAddress { get; set; }
        public string BillingProviderCity { get; set; }
        public string BillingProviderState { get; set; }
        public string BillingProviderZipcode { get; set; }

        public string RenderingProviderName { get; set; }
        public string RenderingProviderNPI { get; set; }
        public string RenderingProviderEIN { get; set; }
        public string RenderingProviderAHCCCSID { get; set; }
        public string RenderingProviderAddress { get; set; }
        public string RenderingProviderCity { get; set; }
        public string RenderingProviderState { get; set; }
        public string RenderingProviderZipcode { get; set; }


        public long BatchID { get; set; }
        public long BatchTypeID { get; set; }
        public DateTime BatchStartDate { get; set; }
        public DateTime BatchEndDate { get; set; }
        public DateTime SentDate { get; set; }
        public string SentBy { get; set; }
        public DateTime GatherDate { get; set; }
        public string GatheredBy { get; set; }
        public string BatchPayorName { get; set; }
        public string BatchPayorShortName { get; set; }
        public string BatchTypeName { get; set; }



    }

    #endregion

    #region EDI File Logs Model

    public class SetEdiFileLogModelListPage
    {
        public SetEdiFileLogModelListPage()
        {
            SearchEdiFileLogListPage = new SearchEdiFileLogListPage();
            EdiTypeList = new List<EdiTypeList>();
        }
        public SearchEdiFileLogListPage SearchEdiFileLogListPage { get; set; }
        public List<EdiTypeList> EdiTypeList { get; set; }
    }

    public class EdiTypeList
    {
        public long EdiFileTypeID { get; set; }
        public string EdiFileTypeName { get; set; }
    }

    public class SearchEdiFileLogListPage
    {
        public long EdiFileLogID { get; set; }
        public long EdiFileTypeID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ListOfIdsInCSV { get; set; }
        public int IsDeleted { get; set; }
    }

    public class ListEdiFileLogModel
    {
        public long EdiFileLogID { get; set; }
        public long BatchID { get; set; }
        public long EdiFileTypeID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileSize { get; set; }
        public string EdiFileTypeName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime AddedDate { get; set; }
        public int Count { get; set; }
        public string StrDisplayName { get; set; }
      //  public string StrDisplayName { get { return Common.GetGeneralNameFormat(FirstName, LastName); } }
        public string StrFileSize { get { return Common.GetFileSize(Convert.ToDecimal(FileSize), Constants.SizeIn_KB); } }
        [Ignore]
        public string AWSSignedFilePath
        {
            get
            {
                AmazonFileUpload az = new AmazonFileUpload();
                return az.GetPreSignedUrl(ConfigSettings.ZarephathBucket, FilePath);
            }
        }
    }

    #endregion

    #region Download Overview File

    [TableName("Overview File")]
    public class ListOverViewFileModel
    {
        [CreateExcelFile.ExcelHead("BatchNew", typeof(Resource))]
        public long BatchID { get; set; }

        [CreateExcelFile.ExcelHead("ParentNote", typeof(Resource))]
        public long NoteID { get; set; }

        //[CreateExcelFile.ExcelHead("ClaimId", typeof(Resource))]
        //public string ClaimId { get; set; } //Reaminig to add in Procedure

        [CreateExcelFile.ExcelHead("Payor", typeof(Resource))]
        public string BatchPayorName { get; set; }

        [CreateExcelFile.ExcelHead("ClientN", typeof(Resource))]
        public string ClientName { get; set; }

        [CreateExcelFile.ExcelHead("CISNumberLabel", typeof(Resource))]
        public string CISNumber { get; set; }

        [CreateExcelFile.ExcelHead("AHCCCSID", typeof(Resource))]
        public string AHCCCSID { get; set; }

        [CreateExcelFile.ExcelHead("SubscriberPrimaryId", typeof(Resource))]
        public string SubscriberPrimaryId { get { return AHCCCSID; } }

        [CreateExcelFile.ExcelHead("DOBShort", typeof(Resource))]
        public DateTime ClientDob { get; set; }

        [CreateExcelFile.ExcelHead("Address", typeof(Resource))]
        public string Address { get; set; }

        [CreateExcelFile.ExcelHead("City", typeof(Resource))]
        public string City { get; set; }

        [CreateExcelFile.ExcelHead("State", typeof(Resource))]
        public string State { get; set; }

        [CreateExcelFile.ExcelHead("ZipCode", typeof(Resource))]
        public string ZipCode { get; set; }

        [CreateExcelFile.ExcelHead("ClosureDate", typeof(Resource))]
        public DateTime ClosureDate { get; set; }

        [CreateExcelFile.ExcelHead("Gender", typeof(Resource))]
        public string Gender { get; set; }

        [CreateExcelFile.ExcelHead("DXCode", typeof(Resource))]
        public string ContinuedDX { get; set; }

        [CreateExcelFile.ExcelHead("Population", typeof(Resource))]
        public string Population { get; set; }

        [CreateExcelFile.ExcelHead("Title", typeof(Resource))]
        public string Title { get; set; }


        [CreateExcelFile.ExcelHead("Modifier", typeof(Resource))]
        public string ModifierName { get; set; }

        [CreateExcelFile.ExcelHead("ServiceCode", typeof(Resource))]
        public string ServiceCode { get; set; }

        [CreateExcelFile.ExcelHead("ServiceDate", typeof(Resource))]
        public DateTime ServiceDate { get; set; }

        [CreateExcelFile.ExcelHead("POS", typeof(Resource))]
        public int PosID { get; set; }

        [CreateExcelFile.ExcelHead("Unit", typeof(Resource))]
        public long CalculatedUnit { get; set; }

        [CreateExcelFile.ExcelHead("CalculatedAmount", typeof(Resource))]
        public decimal CalculatedAmount { get; set; }

        [CreateExcelFile.ExcelHead("BillingProviderName", typeof(Resource))]
        public string BillingProviderName { get; set; }
        [CreateExcelFile.ExcelHead("BillingProviderNPI", typeof(Resource))]
        public string BillingProviderNPI { get; set; }
        [CreateExcelFile.ExcelHead("BillingProviderEIN", typeof(Resource))]
        public string BillingProviderEIN { get; set; }
        [CreateExcelFile.ExcelHead("BillingProviderAddress", typeof(Resource))]
        public string BillingProviderAddress { get; set; }
        [CreateExcelFile.ExcelHead("BillingProviderCity", typeof(Resource))]
        public string BillingProviderCity { get; set; }
        [CreateExcelFile.ExcelHead("BillingProviderState", typeof(Resource))]
        public string BillingProviderState { get; set; }
        [CreateExcelFile.ExcelHead("BillingProviderZipcode", typeof(Resource))]
        public string BillingProviderZipcode { get; set; }

        [CreateExcelFile.ExcelHead("RenderingProviderName", typeof(Resource))]
        public string RenderingProviderName { get; set; }
        [CreateExcelFile.ExcelHead("RenderingProviderNPI", typeof(Resource))]
        public string RenderingProviderNPI { get; set; }
        [CreateExcelFile.ExcelHead("RenderingProviderEIN", typeof(Resource))]
        public string RenderingProviderEIN { get; set; }
        [CreateExcelFile.ExcelHead("RenderingProviderAddress", typeof(Resource))]
        public string RenderingProviderAddress { get; set; }
        [CreateExcelFile.ExcelHead("RenderingProviderCity", typeof(Resource))]
        public string RenderingProviderCity { get; set; }
        [CreateExcelFile.ExcelHead("RenderingProviderState", typeof(Resource))]
        public string RenderingProviderState { get; set; }
        [CreateExcelFile.ExcelHead("RenderingProviderZipcode", typeof(Resource))]
        public string RenderingProviderZipcode { get; set; }

    }

    [TableName("Overview File")]
    public class HC_ListOverViewFileModel
    {
        [CreateExcelFile.ExcelHead("BatchNew", typeof(Resource))]
        public long BatchID { get; set; }

        [CreateExcelFile.ExcelHead("ParentNote", typeof(Resource))]
        public long NoteID { get; set; }

        [CreateExcelFile.ExcelHead("Payor", typeof(Resource))]
        public string BatchPayorName { get; set; }

        [CreateExcelFile.ExcelHead("ClientN", typeof(Resource))]
        public string ClientName { get; set; }

        [CreateExcelFile.ExcelHead("CISNumberLabel", typeof(Resource))]
        public string CISNumber { get; set; }

        [CreateExcelFile.ExcelHead("AHCCCSID", typeof(Resource))]
        public string AHCCCSID { get; set; }

        [CreateExcelFile.ExcelHead("SubscriberPrimaryId", typeof(Resource))]
        public string SubscriberPrimaryId { get { return AHCCCSID; } }

        [CreateExcelFile.ExcelHead("DOBShort", typeof(Resource))]
        public DateTime ClientDob { get; set; }

        [CreateExcelFile.ExcelHead("Address", typeof(Resource))]
        public string Address { get; set; }

        [CreateExcelFile.ExcelHead("City", typeof(Resource))]
        public string City { get; set; }

        [CreateExcelFile.ExcelHead("State", typeof(Resource))]
        public string State { get; set; }

        [CreateExcelFile.ExcelHead("ZipCode", typeof(Resource))]
        public string ZipCode { get; set; }

        [CreateExcelFile.ExcelHead("ClosureDate", typeof(Resource))]
        public DateTime ClosureDate { get; set; }

        [CreateExcelFile.ExcelHead("Gender", typeof(Resource))]
        public string Gender { get; set; }

        //[CreateExcelFile.ExcelHead("DXCode", typeof(Resource))]
        //public string ContinuedDX { get; set; }

        //[CreateExcelFile.ExcelHead("Population", typeof(Resource))]
        //public string Population { get; set; }

        //[CreateExcelFile.ExcelHead("Title", typeof(Resource))]
        //public string Title { get; set; }

        [CreateExcelFile.ExcelHead("CareType", typeof(Resource))]
        public string CareType { get; set; }

        [CreateExcelFile.ExcelHead("ServiceCode", typeof(Resource))]
        public string ServiceCode { get; set; }

        [CreateExcelFile.ExcelHead("Modifier", typeof(Resource))]
        public string ModifierName { get; set; }

        [CreateExcelFile.ExcelHead("ServiceDate", typeof(Resource))]
        public DateTime ServiceDate { get; set; }

        //[CreateExcelFile.ExcelHead("POS", typeof(Resource))]
        //public int PosID { get; set; }

        [CreateExcelFile.ExcelHead("Unit", typeof(Resource))]
        public long CalculatedUnit { get; set; }

        [CreateExcelFile.ExcelHead("CalculatedAmount", typeof(Resource))]
        public decimal CalculatedAmount { get; set; }

        [CreateExcelFile.ExcelHead("BillingProviderName", typeof(Resource))]
        public string BillingProviderName { get; set; }

        [CreateExcelFile.ExcelHead("BillingProviderNPI", typeof(Resource))]
        public string BillingProviderNPI { get; set; }

        [CreateExcelFile.ExcelHead("BillingProviderEIN", typeof(Resource))]
        public string BillingProviderEIN { get; set; }

        //[CreateExcelFile.ExcelHead("BillingProviderAddress", typeof(Resource))]
        //public string BillingProviderAddress { get; set; }
        //[CreateExcelFile.ExcelHead("BillingProviderCity", typeof(Resource))]
        //public string BillingProviderCity { get; set; }
        //[CreateExcelFile.ExcelHead("BillingProviderState", typeof(Resource))]
        //public string BillingProviderState { get; set; }
        //[CreateExcelFile.ExcelHead("BillingProviderZipcode", typeof(Resource))]
        //public string BillingProviderZipcode { get; set; }

        [CreateExcelFile.ExcelHead("RenderingProviderName", typeof(Resource))]
        public string RenderingProviderName { get; set; }
        [CreateExcelFile.ExcelHead("RenderingProviderNPI", typeof(Resource))]
        public string RenderingProviderNPI { get; set; }
        [CreateExcelFile.ExcelHead("RenderingProviderEIN", typeof(Resource))]
        public string RenderingProviderEIN { get; set; }

        //[CreateExcelFile.ExcelHead("RenderingProviderAddress", typeof(Resource))]
        //public string RenderingProviderAddress { get; set; }
        //[CreateExcelFile.ExcelHead("RenderingProviderCity", typeof(Resource))]
        //public string RenderingProviderCity { get; set; }
        //[CreateExcelFile.ExcelHead("RenderingProviderState", typeof(Resource))]
        //public string RenderingProviderState { get; set; }
        //[CreateExcelFile.ExcelHead("RenderingProviderZipcode", typeof(Resource))]
        //public string RenderingProviderZipcode { get; set; }
        public long ReferralID { get; set; }
    }

    #endregion


    #region

    [TableName("Paper Remit")]
    public class ListPaperRemitModel
    {
        [TitleAttribute("PR_BatchID", typeof(Resource))]
        public long BatchID { get; set; }
        [TitleAttribute("PR_NoteID", typeof(Resource))]
        public long NoteID { get; set; }
        [TitleAttribute("PR_BatchNoteID", typeof(Resource))]
        public long BatchNoteID { get; set; }
        [TitleAttribute("PR_LastName", typeof(Resource))]
        public string LastName { get; set; }
        [TitleAttribute("PR_FirstName", typeof(Resource))]
        public string FirstName { get; set; }


        //public string Payor { get; set; }
        [TitleAttribute("PR_CheckDate", typeof(Resource))]
        public DateTime? CheckDate { get; set; }
        [TitleAttribute("PR_CheckNumber", typeof(Resource))]
        public long? CheckNumber { get; set; }
        [TitleAttribute("PR_CheckAmount", typeof(Resource))]
        public decimal? CheckAmount { get; set; }

        [TitleAttribute("PR_InvoiceNumber", typeof(Resource))]
        public string InvoiceNumber { get; set; }

        [TitleAttribute("PR_PolicyNumber", typeof(Resource))]
        public string PolicyNumber { get; set; }

        [TitleAttribute("PR_ProcessedDate", typeof(Resource))]
        public DateTime? ProcessedDate { get; set; }
        [TitleAttribute("PR_ClientIdNumber", typeof(Resource))]
        public string ClientIdNumber { get; set; }
        [TitleAttribute("PR_ClaimNumber", typeof(Resource))]
        public string ClaimNumber { get; set; }
        [TitleAttribute("PR_BillingProviderName", typeof(Resource))]
        public string BillingProviderName { get; set; }
        [TitleAttribute("PR_BillingProviderNPI", typeof(Resource))]
        public string BillingProviderNPI { get; set; }
        //public string RenderingProviderName { get; set; }
        //public string RenderingProviderNPI { get; set; }

        [TitleAttribute("PR_ServiceDate", typeof(Resource))]
        public string ServiceDate { get; set; }
        [TitleAttribute("PR_PosID", typeof(Resource))]
        public long PosID { get; set; }
        [TitleAttribute("PR_BilledUnit", typeof(Resource))]
        public long BilledUnit { get; set; }
        [TitleAttribute("PR_ServiceCode", typeof(Resource))]
        public string ServiceCode { get; set; }
        [TitleAttribute("PR_ModifierCode", typeof(Resource))]
        public string ModifierCode { get; set; }
        [TitleAttribute("PR_BilledAmount", typeof(Resource))]
        public decimal BilledAmount { get; set; }

        [TitleAttribute("PR_DenialCodes", typeof(Resource))]
        public string DenialCodes { get; set; }
        [TitleAttribute("PR_AllowedAmount", typeof(Resource))]
        public decimal? AllowedAmount { get; set; }
        [TitleAttribute("PR_Deductible", typeof(Resource))]
        public string Deductible { get; set; }
        [TitleAttribute("PR_Coinsurance", typeof(Resource))]
        public string Coinsurance { get; set; }
        [TitleAttribute("PR_PaidAmount", typeof(Resource))]
        public decimal? PaidAmount { get; set; }

        //[TitleAttribute("PR_App_Denied", typeof(Resource))]
        //public string App_Denied { get; set; }
        //[TitleAttribute("PR_BillToNextPayor", typeof(Resource))]
        //public string BillToNextPayor { get; set; }
        //[TitleAttribute("PR_ClaimForwardedTo", typeof(Resource))]
        //public string ClaimForwardedTo { get; set; }

        //Claim Information
        [TitleAttribute("PR_LX01_ClaimSequenceNumber", typeof(Resource))]
        public string LX01_ClaimSequenceNumber { get; set; }
        [TitleAttribute("PR_CLP02_ClaimStatusCode", typeof(Resource))]
        public string CLP02_ClaimStatusCode { get; set; } //Claim Status Code Information
        [TitleAttribute("PR_CLP01_ClaimSubmitterIdentifier", typeof(Resource))]
        public string CLP01_ClaimSubmitterIdentifier { get; set; }
        [TitleAttribute("PR_CLP03_TotalClaimChargeAmount", typeof(Resource))]
        public string CLP03_TotalClaimChargeAmount { get; set; }
        [TitleAttribute("PR_CLP04_TotalClaimPaymentAmount", typeof(Resource))]
        public string CLP04_TotalClaimPaymentAmount { get; set; }
        [TitleAttribute("PR_CLP05_PatientResponsibilityAmount", typeof(Resource))]
        public string CLP05_PatientResponsibilityAmount { get; set; }
        [TitleAttribute("PR_CLP07_PayerClaimControlNumber", typeof(Resource))]
        public string CLP07_PayerClaimControlNumber { get; set; }

        //Claim Adjustment Code Information
        [TitleAttribute("PR_CAS01_ClaimAdjustmentGroupCode", typeof(Resource))]
        public string CAS01_ClaimAdjustmentGroupCode { get; set; }
        [TitleAttribute("PR_CAS02_ClaimAdjustmentReasonCode", typeof(Resource))]
        public string CAS02_ClaimAdjustmentReasonCode { get; set; }
        [TitleAttribute("PR_CAS03_ClaimAdjustmentAmount", typeof(Resource))]
        public string CAS03_ClaimAdjustmentAmount { get; set; }







        #region Payor Details
        [TitleAttribute("PR_Payor", typeof(Resource))]
        public string Payor { get; set; }
        [TitleAttribute("PR_PayorBusinessContactName", typeof(Resource))]
        public string BusinessContactName { get; set; }
        [TitleAttribute("PR_PayorBusinessContact", typeof(Resource))]
        public string BusinessContact { get; set; }
        [TitleAttribute("PR_PayorTechnicalContactName", typeof(Resource))]
        public string PayorTechnicalContactName { get; set; }
        [TitleAttribute("PR_PayorTechnicalContact", typeof(Resource))]
        public string PayorTechnicalContact { get; set; }
        [TitleAttribute("PR_PayorTechnicalEmail", typeof(Resource))]
        public string PayorTechnicalEmail { get; set; }
        [TitleAttribute("PR_PayorTechnicalContactUrl", typeof(Resource))]
        public string PayorTechnicalContactUrl { get; set; }
        #endregion

        #region Payee Name

        [TitleAttribute("PR_Payee", typeof(Resource))]
        public string Payee { get; set; }

        [TitleAttribute("PR_PayeeIDQualifier", typeof(Resource))]
        public string PayeeIDQualifier { get; set; }

        [TitleAttribute("PR_PayeeID", typeof(Resource))]
        public string PayeeID { get; set; }


        #endregion

    }

    #endregion


    public class Uploaded835FileSearchParam
    {
        public long PayorID { get; set; }
    }



    public class ExportReconcile835ListModel
    {


        //public long BatchNoteID { get; set; }
        //public long BatchID { get; set; }
        //public long NoteID { get; set; }

        public string Payor { get; set; }
        public string ClientName { get; set; }
        public string AHCCCSID { get; set; }
        public DateTime ClientDob { get; set; }
        //public string Gender { get; set; }
        public long CISNumber { get; set; }
        public string DXCode { get; set; }

        [CreateExcelFile.ExcelHead("S277Status", typeof(Resource))]
        public string S277 { get; set; }
        [CreateExcelFile.ExcelHead("S277CAStatus", typeof(Resource))]
        public string S277CA { get; set; }



        public string NoteCreatedBy { get; set; }
        public string RenderingProvider { get; set; }
        public string BillingProvider { get; set; }
        public long BillingProviderNPI { get; set; }

        [CreateExcelFile.ExcelHead("Program", typeof(Resource))]
        public string ProgrameName { get; set; }


        public string ServiceCode { get; set; }
        public string Modifier { get; set; }
        public long POS { get; set; }
        public DateTime ServiceDate { get; set; }
        public long CalculatedUnit { get; set; }
        public decimal BilledAmount { get; set; }


        public string NoteSigned { get; set; }
        public DateTime NoteSignedDate { get; set; }
        public long BatchID { get; set; }
        public string BatchTypeName { get; set; }
        public DateTime BatchStartDate { get; set; }
        public DateTime BatchEndDate { get; set; }
        public string GatheredBy { get; set; }
        public DateTime GatheredDate { get; set; }
        public string SentBy { get; set; }
        public DateTime SendDate { get; set; }

        public DateTime ExtractDate { get; set; }
        public DateTime ProcessedDate { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime LoadDate { get; set; }
        public string Status { get; set; }
        public string ClaimStatus { get; set; }
        public string ClaimNumber { get; set; }
        public string PayorClaimNumber { get; set; }
        public string ClaimAdjustmentGroupCodeID { get; set; }
        public string ClaimAdjustmentGroupCodeName { get; set; }
        public string ClaimAdjustmentReasonCodeID { get; set; }
        public string ClaimAdjustmentReasonDescription { get; set; }
        public string ClaimAdjustmentType { get; set; }
        [CreateExcelFile.ExcelHead("BilledAmount", typeof(Resource))]
        public decimal BilledAmount01 { get { return BilledAmount; } }
        public decimal AllowedAmount { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime CheckDate { get; set; }
        public string CheckNumber { get; set; }
        public decimal CheckAmount { get; set; }


        //public long BatchNoteID { get; set; }
        //public long NoteID { get; set; }
        //public string RenderingProviderNPI { get; set; }
        //public string Description { get; set; }
        //public string POSDetail { get; set; }

    }

    public class ClaimStatus
    {
        public long ClaimStatusID { get; set; }
        public string StatusName { get; set; }
    }

    #region HomeCare Reconcile 835
    public class HC_AddReconcile835Model
    {
        public HC_AddReconcile835Model()
        {
            PayorList = new List<PayorList>();
            ModifierList = new List<Modifier>();
            PlaceOfServiceList = new List<PlaceOfService>();
            ClaimStatusCodeList = new List<ClaimStatusCode>();
            ClaimAdjustmentGroupCodeList = new List<ClaimAdjustmentGroupCode>();
            ClaimAdjustmentReasonCodeList = new List<ClaimAdjustmentReasonCode>();
            SearchReconcile835ListPage = new SearchReconcile835ListPage();
            Get835ProcessedOnlyList = Common.Set835ProcessedOnlyList();
            ServiceCodeList = new List<ServiceCodeList>();
            ClaimStatusList = new List<ClaimStatus>();
            ClaimStatusCode = new ClaimStatusCode();
            ClaimStatus = new ClaimStatus();
        }
        public List<PayorList> PayorList { get; set; }
        public List<Modifier> ModifierList { get; set; }
        public List<PlaceOfService> PlaceOfServiceList { get; set; }
        public List<ClaimStatusCode> ClaimStatusCodeList { get; set; }
        public List<ClaimAdjustmentGroupCode> ClaimAdjustmentGroupCodeList { get; set; }
        public List<ClaimAdjustmentReasonCode> ClaimAdjustmentReasonCodeList { get; set; }
        public List<ServiceCodeList> ServiceCodeList { get; set; }
        [Ignore]
        public List<NameValueData> Get835ProcessedOnlyList { get; set; }

        [Ignore]
        public long PayorID { get; set; }
        [Ignore]
        public string ModifierID { get; set; }
        [Ignore]
        public long PosID { get; set; }
        [Ignore]
        public long ClaimStatusCodeID { get; set; }
        [Ignore]
        public string ClaimAdjustmentGroupCodeID { get; set; }
        [Ignore]
        public string ClaimAdjustmentReasonCodeID { get; set; }


        [Ignore]
        public SearchReconcile835ListPage SearchReconcile835ListPage { get; set; }

        [Ignore]
        public List<NameValueDataInString> Services { get; set; }

        [Ignore]
        public List<NameValueDataInString> ClaimAdjustmentTypes { get; set; }

        [Ignore]
        public ClaimStatusCode ClaimStatusCode { get; set; }

        [Ignore]
        public List<ClaimStatus> ClaimStatusList { get; set; }

        [Ignore]
        public ClaimStatus ClaimStatus { get; set; }
    }
    #endregion

    public enum Enum_IndexForSelectedRange
    {
        Range_0_60 = 1,
        Range_61_90 = 2,
        Range_91_120 = 3,
        Range_121_180 = 4,
        Range_181_270 = 5,
        Range_271_365 = 6,
        Range_0_365 = 7
    }

    public class SearchARAgingReportPage
    {
        public string StrReconcileStatus { get; set; }
        public string ReconcileStatus { get; set; }


        public string StrPayorIDs { get; set; }
        public string PayorIDs { get; set; }

        public string ClientName { get; set; }
        public string StrClientIDs { get; set; }
        public string ClientIDs { get; set; }
    }

    public class SetARAgingReportPage
    {
        public SetARAgingReportPage()
        {
            PayorList = new List<NameValueData>();
            ClientList = new List<NameValueData>();
            SearchARAgingReportPage = new SearchARAgingReportPage();

        }


        public List<NameValueData> PayorList { get; set; }
        public List<NameValueData> ClientList { get; set; }
        [Ignore]
        public SearchARAgingReportPage SearchARAgingReportPage { get; set; }
    }

    public class ListARAgingReportModel
    {
        [CreateExcelFile.ReportIgnore()]
        public string StrReconcileStatus { get; set; }

        [CreateExcelFile.ReportIgnore()]
        public int IndexForSelectedRange { get; set; }

        [CreateExcelFile.ReportIgnore()]
        public string AllActivePayorIDs { get; set; }

        [CreateExcelFile.ReportIgnore()]
        public bool IsFooterRow { get; set; }

        [CreateExcelFile.ReportIgnore()]
        public long PayorID { get; set; }

        [CreateExcelFile.ReportIgnore()]
        public string PayorName { get; set; }

        [CreateExcelFile.ExcelHead("Payor", typeof(Resource))]
        public string PayorShortName { get; set; }

        [CreateExcelFile.ExcelHead("PayorType", typeof(Resource))]
        public string PayorTypeName { get; set; }


        [CreateExcelFile.ExcelHead("Days0_60_Excel", typeof(Resource))]
        public decimal PendingAmount0_60 { get; set; }
        [CreateExcelFile.ExcelHead("Days61_90_Excel", typeof(Resource))]
        public decimal PendingAmount61_90 { get; set; }
        [CreateExcelFile.ExcelHead("Days91_120_Excel", typeof(Resource))]
        public decimal PendingAmount91_120 { get; set; }
        [CreateExcelFile.ExcelHead("Days121_180_Excel", typeof(Resource))]
        public decimal PendingAmount121_180 { get; set; }
        [CreateExcelFile.ExcelHead("Days181_270_Excel", typeof(Resource))]
        public decimal PendingAmount181_270 { get; set; }
        [CreateExcelFile.ExcelHead("Days271_365_Excel", typeof(Resource))]
        public decimal PendingAmount271_365 { get; set; }

        //[CreateExcelFile.ExcelHead("Days365Onwards_Excel", typeof(Resource))]
        //public string PendingAmount365_Plus { get; set; }

        [CreateExcelFile.ExcelHead("Total", typeof(Resource))]
        public decimal TotalPendingAmount { get; set; }

    }



    public class ARAgingModel
    {
        public int IndexForSelectedRange { get; set; }
        public long PayorID { get; set; }

        public string ClientName { get; set; }

        public string StrReconcileStatus { get; set; }

        public string StrClaimAdjustmentTypeID { get; set; }

    }

}
