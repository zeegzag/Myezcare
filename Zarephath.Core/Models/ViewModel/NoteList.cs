using System;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;
using ExportToExcel;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class NoteList : Note
    {
        public string PosName { get; set; }
        public string ModifierName { get; set; }
        public string Name { get; set; }
        public string ReferralName { get; set; }
        public string ServiceCodeTypeName { get; set; }
        public string CreatedByUserName { get; set; }
        public string UpdatedByUserName { get; set; }
        public string SignatureBy { get; set; }
        public string StrStartDate { get { return StartTime.HasValue ? StartTime.Value.ToString("MM/dd/yyyy h:mm tt") : null; } }
        public string StrEndDate { get { return EndTime.HasValue ? EndTime.Value.ToString("MM/dd/yyyy h:mm tt") : null; } }
        public bool AllowEdit { get; set; }
        public long? BatchID { get; set; }
        public string BatchDetails { get; set; }
        public bool IsSent { get; set; }
        public string EncryptedReferralID { get { return ReferralID > 0 ? Crypto.Encrypt(ReferralID.ToString()) : null; } }
        public string AttachmentURL { get; set; }
        [Ignore]
        public string AWSSignedAttachmentUrl
        {
            get
            {
                AmazonFileUpload az = new AmazonFileUpload();
                return az.GetPreSignedUrl(ConfigSettings.ZarephathBucket, AttachmentURL);
            }
        }
        public string MonthlySummaryIds { get; set; }
        public long Row { get; set; }
        public int Count { get; set; }
    }

    public class NoteClientList : Note
    {
        public long ReferralID { get; set; }
        public long FacilityID { get; set; }
        public string FacilityName { get; set; }
        public string ReferralName { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public string Payor { get; set; }
        public string Population { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public string StrStartDate { get { return StartDate.HasValue ? StartDate.Value.ToString("MM/dd/yyyy") : null; } }

        public long NoteID { get; set; }

        public string EncryptedReferralID { get { return ReferralID > 0 ? Crypto.Encrypt(ReferralID.ToString()) : null; } }

        public long Row { get; set; }
        public int Count { get; set; }
    }



    public class ExportNoteList
    {

        [CreateExcelFile.ExcelHead("NoteNew", typeof(Resource))]
        public long NoteID { get; set; }

        public string Name { get; set; }

        [CreateExcelFile.ExcelHead("AHCCCSNumber", typeof(Resource))]
        public string AHCCCSID { get; set; }
        [CreateExcelFile.ExcelHead("CISLabel", typeof(Resource))]
        public string CISNumber { get; set; }
        [CreateExcelFile.ExcelHead("Payor", typeof(Resource))]
        public string PayorShortName { get; set; }


        [CreateExcelFile.ExcelHead("ZarephathService", typeof(Resource))]
        public string ZarephathService { get; set; }
        [CreateExcelFile.ExcelHead("ServiceCodeType", typeof(Resource))]
        public string ServiceCodeTypeName { get; set; }
        [CreateExcelFile.ExcelHead("ServiceDate", typeof(Resource))]
        public DateTime ServiceDate { get; set; }
        [CreateExcelFile.ExcelHead("ServiceCode", typeof(Resource))]
        public string ServiceCode { get; set; }
        [CreateExcelFile.ExcelHead("Description", typeof(Resource))]
        public string Description { get; set; }
        [CreateExcelFile.ExcelHead("POS", typeof(Resource))]
        public string PosID { get; set; }
        [CreateExcelFile.ExcelHead("Place", typeof(Resource))]
        public string POSDetail { get; set; }

        //[CreateExcelFile.ExcelHead("ServiceStartDate", typeof(Resource))]
        //public string ServiceCodeStartDate { get; set; }
        //[CreateExcelFile.ExcelHead("ServiceEndDate", typeof(Resource))]
        //public string ServiceCodeEndDate { get; set; }

        //[CreateExcelFile.ExcelHead("POS", typeof(Resource))]
        //public string PosName { get; set; }

        //[CreateExcelFile.ExcelHead("Modifier", typeof(Resource))]
        //public string ModifierName { get; set; }


        [CreateExcelFile.ExcelHead("StartTime", typeof(Resource))]
        public string StartTime { get; set; }

        [CreateExcelFile.ExcelHead("EndTime", typeof(Resource))]
        public string EndTime { get; set; }
        

        [CreateExcelFile.ExcelHead("StartMile", typeof(Resource))]
        public long? StartMile { get; set; }
        [CreateExcelFile.ExcelHead("EndMile", typeof(Resource))]
        public long? EndMile { get; set; }

        [CreateExcelFile.ExcelHead("CalculatedUnit", typeof(Resource))]
        public long CalculatedUnit { get; set; }

        [CreateExcelFile.ExcelHead("CalculatedAmount", typeof(Resource))]
        public Decimal CalculatedAmount { get; set; }

        [CreateExcelFile.ExcelHead("SpokeTo", typeof(Resource))]
        public string SpokeTo { get; set; }
        [CreateExcelFile.ExcelHead("Relation", typeof(Resource))]
        public string Relation { get; set; }
        [CreateExcelFile.ExcelHead("NoteType", typeof(Resource))]
        public string OtherNoteType { get; set; }


        [CreateExcelFile.ExcelHead("NoteDetails", typeof(Resource))]
        public string NoteDetails { get; set; }
        [CreateExcelFile.ExcelHead("Assessment", typeof(Resource))]
        public string Assessment { get; set; }
        [CreateExcelFile.ExcelHead("ActionPlan", typeof(Resource))]
        public string ActionPlan { get; set; }

       

        
        [CreateExcelFile.ExcelHead("BillingProviderName", typeof(Resource))]
        public string BillingProviderName { get; set; }
        [CreateExcelFile.ExcelHead("BillingProviderAddress", typeof(Resource))]
        public string BillingProviderAddress { get; set; }
        [CreateExcelFile.ExcelHead("BillingProviderCity", typeof(Resource))]
        public string BillingProviderCity { get; set; }
        [CreateExcelFile.ExcelHead("BillingProviderState", typeof(Resource))]
        public string BillingProviderState { get; set; }
        [CreateExcelFile.ExcelHead("BillingProviderZipcode", typeof(Resource))]
        public string BillingProviderZipcode { get; set; }
        [CreateExcelFile.ExcelHead("BillingProviderEIN", typeof(Resource))]
        public string BillingProviderEIN { get; set; }
        [CreateExcelFile.ExcelHead("BillingProviderNPI", typeof(Resource))]
        public string BillingProviderNPI { get; set; }

        [CreateExcelFile.ExcelHead("RenderingProviderName", typeof(Resource))]
        public string RenderingProviderName { get; set; }
        [CreateExcelFile.ExcelHead("RenderingProviderAddress", typeof(Resource))]
        public string RenderingProviderAddress { get; set; }
        [CreateExcelFile.ExcelHead("RenderingProviderCity", typeof(Resource))]
        public string RenderingProviderCity { get; set; }
        [CreateExcelFile.ExcelHead("RenderingProviderState", typeof(Resource))]
        public string RenderingProviderState { get; set; }
        [CreateExcelFile.ExcelHead("RenderingProviderZipcode", typeof(Resource))]
        public string RenderingProviderZipcode { get; set; }
        [CreateExcelFile.ExcelHead("RenderingProviderEIN", typeof(Resource))]
        public string RenderingProviderEIN { get; set; }

        public string Issue { get; set; }
        public string Assignee { get; set; }
        [CreateExcelFile.ExcelHead("IssueResolved", typeof(Resource))]
        public string IssueResolved { get; set; }
        
        




        [CreateExcelFile.ExcelHead("CreatedBy", typeof(Resource))]
        public string CreatedByUserName { get; set; }
        [CreateExcelFile.ExcelHead("CreatedDate", typeof(Resource))]
        public DateTime CreatedDate { get; set; }
        [CreateExcelFile.ExcelHead("UpdatedBy", typeof(Resource))]
        public string UpdatedByUserName { get; set; }
        [CreateExcelFile.ExcelHead("UpdatedDate", typeof(Resource))]
        public DateTime UpdatedDate { get; set; }
        [CreateExcelFile.ExcelHead("SignatureBy", typeof(Resource))]
        public string SignatureBy { get; set; }
        [CreateExcelFile.ExcelHead("SignatureDate", typeof(Resource))]
        public DateTime SignatureDate { get; set; }



        [CreateExcelFile.ExcelHead("GroupID", typeof(Resource))]
        public string RandomGroupID { get; set; }
        
        //public string StrStartDate { get { return StartTime.HasValue ? StartTime.Value.ToString("MM/dd/yyyy h:mm tt") : null; } }
        //public string StrEndDate { get { return EndTime.HasValue ? EndTime.Value.ToString("MM/dd/yyyy h:mm tt") : null; } }
        
        
        //public long Row { get; set; }
        
    }

}
