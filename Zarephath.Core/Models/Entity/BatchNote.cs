using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("BatchNotes")]
    [PrimaryKey("BatchNoteID")]
    [Sort("BatchNoteID", "DESC")]
    public class BatchNote
    {
        public long BatchNoteID { get; set; }
        public long BatchID { get; set; }
        public long NoteID { get; set; }
        public string CLM_BilledAmount { get; set; }
        public string BatchNoteStatusID { get; set; }
        public long? ParentBatchNoteID { get; set; }


        //Payor Information
        public string N102_PayorName { get; set; }
        public string PER02_PayorBusinessContactName { get; set; }
        public string PER04_PayorBusinessContact { get; set; }
        public string PER02_PayorTechnicalContactName { get; set; }
        public string PER04_PayorTechnicalContact { get; set; }
        public string PER06_PayorTechnicalEmail { get; set; }
        public string PER04_PayorTechnicalContactUrl { get; set; }

        //Payee Information
        public string N102_PayeeName { get; set; }
        public string N103_PayeeIdentificationQualifier { get; set; }
        public string N104_PayeeIdentification { get; set; }

        //Claim Information
        public string LX01_ClaimSequenceNumber { get; set; }
        public string CLP02_ClaimStatusCode { get; set; } //Claim Status Code Information
        public string CLP01_ClaimSubmitterIdentifier { get; set; }
        public string CLP03_TotalClaimChargeAmount { get; set; }
        public string CLP04_TotalClaimPaymentAmount { get; set; }
        public string CLP05_PatientResponsibilityAmount { get; set; }
        public string CLP07_PayerClaimControlNumber { get; set; }
        public string CLP08_PlaceOfService { get; set; }

        //Patient Information
        public string NM103_PatientLastName { get; set; }
        public string NM104_PatientFirstName { get; set; }
        public string NM109_PatientIdentifier { get; set; }

        //Service Code Information
        public string NM103_ServiceProviderName { get; set; }
        public string NM109_ServiceProviderNpi { get; set; }
        public string SVC01_01_ServiceCodeQualifier { get; set; }
        public string SVC01_02_ServiceCode { get; set; }
        public string SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount { get; set; }
        public string SVC03_LineItemProviderPaymentAmoun_PaidAmount { get; set; }
        public string SVC05_ServiceCodeUnit { get; set; }
        public string DTM02_ServiceStartDate { get; set; }
        public string DTM02_ServiceEndDate { get; set; }

        //Claim Adjustment Code Information
        public string CAS01_ClaimAdjustmentGroupCode { get; set; }
        public string CAS02_ClaimAdjustmentReasonCode { get; set; }
        public string CAS03_ClaimAdjustmentAmount { get; set; }
        

        public string REF02_LineItem_ReferenceIdentification { get; set; }
        public string AMT01_ServiceLineAllowedAmount_AllowedAmount { get; set; }

        public string CheckDate { get; set; }
        public string CheckAmount { get; set; }
        public string CheckNumber { get; set; }
        public string PolicyNumber { get; set; }
        public string AccountNumber { get; set; }
        public string ICN { get; set; }
        //public string BilledAmount { get; set; }
        //public string AllowedAmount { get; set; }
        //public string PaidAmount { get; set; }
        public string Deductible { get; set; }
        public string Coins { get; set; }
        public long Upload835FileID { get; set; }

        public DateTime ReceivedDate { get; set; }
        public DateTime ProcessedDate { get; set; }
        public DateTime LoadDate { get; set; }
        public string DXCode { get; set; }

        public string ClaimAdjustmentTypeID { get; set; }
        public bool IsFirstTimeClaimInBatch { get; set; }


        public string Submitted_ClaimSubmitterIdentifier { get; set; }
        public string Submitted_ClaimAdjustmentTypeID { get; set; }
        public string Original_ClaimSubmitterIdentifier { get; set; }
        public string Original_PayerClaimControlNumber { get; set; }


        public string ClaimAdjustmentReason { get; set; }

        public string CLM_UNIT { get; set; }
        public float Original_Amount { get; set; }
        public float Original_Unit { get; set; }

        public string S277 { get; set; }
        public string S277CA { get; set; }

        public bool IsUseInBilling { get; set; }
        public bool IsNewProcess { get; set; }
        

        

    }
}
