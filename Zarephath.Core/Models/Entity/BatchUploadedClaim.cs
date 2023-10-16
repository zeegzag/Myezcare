using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("BatchUploadedClaims")]
    [PrimaryKey("BatchUploadedClaimID")]
    [Sort("BatchUploadedClaimID", "DESC")]
    public class BatchUploadedClaim : BaseEntity
    {
        public long BatchUploadedClaimID { get; set; }
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
        public string PatientName { get; set; }
        public string Payer { get; set; }
        public string BillingProvider { get; set; }

        
        public string ResponseID { get; set; }
        public string MessageStr { get; set; }

        public string ClaimNotes { get; set; }
    }



    public class ClaimMessageModel
    {
        public string Sender_Name { get; set; }
        public string e_fields { get; set; }
        public string e_mesgid { get; set; }
        public string e_message { get; set; }
        public string e_status { get; set; }
    }






    public class BatchUploadedClaimMessage 
    {
        public long BatchUploadedClaimMessageID { get; set; }
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
        public string PatientName { get; set; }
        public string Payer { get; set; }
        public string BillingProvider { get; set; }


        public string LastResponseID { get; set; }
        public string Messages { get; set; }

    }

    [TableName("BatchUploadedClaimErrors")]
    [PrimaryKey("BatchUpClaimErrorID")]
    [Sort("BatchUpClaimErrorID", "DESC")]
    public class BatchUploadedClaimErrors
    {
        public long BatchUpClaimErrorID { get; set; }
        public long BatchUploadedClaimID { get; set; }
        public string Field { get; set; }
        public string MsgID { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }

    [TableName("BatchUploadedClaimFiles")]
    [PrimaryKey("BatchUpClaimFileID")]
    [Sort("BatchUpClaimFileID", "DESC")]
    public class BatchUploadedClaimFiles
    {
        public long BatchUpClaimFileID { get; set; }
        public string ClaimMD_FileID { get; set; }
        public string FileName { get; set; }
        public int Claims { get; set; }
        public string Amount { get; set; }
        public DateTime Date { get; set; }
        public long BatchUploadedClaimID { get; set; }
        public string ClaimMD_ID { get; set; }
    }
}
