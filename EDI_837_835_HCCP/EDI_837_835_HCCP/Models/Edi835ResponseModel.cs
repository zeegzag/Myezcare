using System.Collections.Generic;

namespace OopFactory.Edi835Parser.Models
{

    public class Edi835ResponseModel
    {
        public Edi835ResponseModel()
        {
            Edi835ModelList = new List<Edi835Model>();
        }

        public List<Edi835Model> Edi835ModelList { get; set; }
        public string GeneratedFileRelativePath { get; set; }
        public string GeneratedFileAbsolutePath { get; set; }
    }



    public class Edi835Model
    {
        public string FileName { get; set; }
        public string CheckSequence { get; set; }


        //public string BPR02_TotalActualProviderPaymentAmount  { get; set; }
        //public string BPR04_PaymentMethod { get; set; }
        //public string BPR16_IssueDate { get; set; }
        //public string REF01_ReceiverIdentificationNumber { get; set; }


        public string N102_PayorName { get; set; }
        public string PER02_PayorBusinessContactName { get; set; }
        public string PER04_PayorBusinessContact { get; set; }
        public string PER02_PayorTechnicalContactName { get; set; }
        public string PER04_PayorTechnicalContact { get; set; }
        public string PER06_PayorTechnicalEmail { get; set; }
        public string PER04_PayorTechnicalContactUrl { get; set; }

        
        public string N102_PayeeName { get; set; }
        public string N103_PayeeIdentificationQualifier { get; set; }
        public string N104_PayeeIdentification { get; set; }


        public string LX01_ClaimSequenceNumber { get; set; }
        public string CLP02_ClaimStatusCode { get; set; }
        public string CLP01_ClaimSubmitterIdentifier { get; set; } //TODO: WE PASSED THROUGH 837
        public string CLP03_TotalClaimChargeAmount { get; set; }
        public string CLP04_TotalClaimPaymetAmount { get; set; }
        public string CLP05_PatientResponsibilityAmount { get; set; }
        public string CLP07_PayerClaimControlNumber  { get; set; } //TODO: WE PASSED THROUGH 837
        public string CLP08_PlaceOfService { get; set; }


        public string NM103_PatientLastName { get; set; }
        public string NM104_PatientFirstName { get; set; }
        public string NM109_PatientIdentifier { get; set; }

        public string NM103_ServiceProviderName { get; set; }
        public string NM109_ServiceProviderNpi { get; set; }



        public string SVC01_01_ServiceCodeQualifier { get; set; }
        public string SVC01_02_ServiceCode { get; set; }

        public string SVC01_02_ServiceCode_Mod_01 { get; set; }
        public string SVC01_02_ServiceCode_Mod_02 { get; set; }
        public string SVC01_02_ServiceCode_Mod_03 { get; set; }
        public string SVC01_02_ServiceCode_Mod_04 { get; set; }



        public string SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount { get; set; }
        public string SVC03_LineItemProviderPaymentAmoun_PaidAmount { get; set; }
        public string SVC05_ServiceCodeUnit { get; set; }
        public string DTM02_ServiceStartDate { get; set; }
        public string DTM02_ServiceEndDate { get; set; }
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
        public string Deductible { get; set; }
        public string Coins { get; set; }
        public string ProcessedDate { get; set; }
        //public string PaidAmount { get; set; }



        public string CLP04_TotalClaimPaymentAmount { get; set; }


        public string BatchID { get; set; }
        public string BatchNoteID { get; set; }
        public string NoteID { get; set; }

    }

    public class ClaimIdentifierModel
    {
        public long BatchId { get; set; }
        public long NoteId { get; set; }
        public long BatchNoteId { get; set; }
        public long BatchNoteId02 { get; set; }
    }


}
