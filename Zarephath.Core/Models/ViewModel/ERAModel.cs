using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class HC_ERAModel
    {
        public HC_ERAModel()
        {
            OrganizationSetting = new OrganizationSetting();
            PayorList = new List<PayorList>();
            SearchERAList = new SearchERAList();
        }
        public OrganizationSetting OrganizationSetting { get; set; }
        public List<PayorList> PayorList { get; set; }
        [Ignore]
        public SearchERAList SearchERAList { get; set; }
        [Ignore]
        public List<HC_AddLatestERAModel> AddLatestERAModelList { get; set; }
    }

    public class SearchERAList
    {
        public long? LatestEraID { get; set; }
        public long? PayorID { get; set; }
        public DateTime? PaidStartDate { get; set; }
        public DateTime? PaidEndDate { get; set; }
        public bool IsDeleted { get; set; }


        public string CheckNumber { get; set; }
        public string EraId { get; set; }


        public DateTime? ReceivedStartDate { get; set; }
        public DateTime? ReceivedEndDate { get; set; }
    }
        public class ListLatestERAModel
    {
        public int ROW { get; set; }
        public long LatestEraID { get; set; }
        public string CheckNumber { get; set; }
        public string CheckType { get; set; }
        public string ClaimProviderName { get; set; }
        public string DownTime { get; set; }
        public string EraID { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaidDate { get; set; }
        public string PayerName { get; set; }
        public string PayerID { get; set; }
        public string ProviderName { get; set; }
        public string ProviderNPI { get; set; }
        public string ProviderTaxID { get; set; }
        public DateTime RecievedTime { get; set; }
        public string Source { get; set; }
        public bool IsDeleted { get; set; }
        public int Count { get; set; }


        public int Upload835FileProcessStatus { get; set; }
        public string StrUpload835FileProcessStatus {
            get {

                if (Upload835FileProcessStatus == 1) return EnumUpload835FileProcessStatus.UnProcessed.ToString();
                if (Upload835FileProcessStatus == 2) return EnumUpload835FileProcessStatus.InProcess.ToString();
                if (Upload835FileProcessStatus == 3) return EnumUpload835FileProcessStatus.Processed.ToString();
                if (Upload835FileProcessStatus == 4) return EnumUpload835FileProcessStatus.ErrorInProcess.ToString();
                if (Upload835FileProcessStatus == 5) return EnumUpload835FileProcessStatus.Running.ToString();

                return null;
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



        public string ValidationMessage { get; set; }
        public string EraMappedBatches { get; set; }


    }
    public class BatchUploadedClaimMessageModel
    {
        public string LastResponseID { get; set; }
        public string BatchID { get; set; }
        public string Bill_NPI { get; set; }
        public string Bill_TaxID { get; set; }
        public string ClaimID { get; set; }
        public string ClaimMD_ID { get; set; }
        public string FDOS { get; set; }
        public string FileID { get; set; }
        public string FileName { get; set; }
        public string INS_Number { get; set; }
        public string PayerID { get; set; }
        public string PCN { get; set; }
        public string Remote_ClaimID { get; set; }
        public string Sender_ICN { get; set; }
        public string Sender_Name { get; set; }
        public string SenderID { get; set; }
        public string Status { get; set; }
        public string Total_Charge { get; set; }
        
        public string Messages { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class HC_AddLatestERAModel
    {
        public string CheckNumber { get; set; }
        public string CheckType { get; set; }
        public string ClaimProviderName { get; set; }
        public string DownTime { get; set; }
        public string EraID { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaidDate { get; set; }
        public string PayerName { get; set; }
        public string PayerID { get; set; }
        public string ProviderName { get; set; }
        public string ProviderNPI { get; set; }
        public string ProviderTaxID { get; set; }
        public DateTime RecievedTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
    }


    public class GetLatestERAModel
    {
        public GetLatestERAModel()
        {
            NPIDetails = new NPIDetails();
            RecievedTimeModel = new RecievedTimeModel();
        }
        public NPIDetails NPIDetails { get; set; }
        public RecievedTimeModel RecievedTimeModel { get; set; }
    }

    public class NPIDetails
    {
        public string Submitter_NM109_IdCode { get; set; }
        public string TaxID { get; set; }
    }
    public class RecievedTimeModel
    {
        public DateTime? RecievedTime { get; set; }
    }
}
