using System.Collections.Generic;

namespace OopFactory.Edi835Parser.Models
{

    public class Edi277ResponseModel
    {
        public Edi277ResponseModel()
        {
            Edi277ModelList = new List<Edi277Model>();
            RecordSeparator = "<>";
            FieldSeparator = "|";
        }

        public List<Edi277Model> Edi277ModelList { get; set; }
        public string GeneratedFileRelativePath { get; set; }
        public string GeneratedFileAbsolutePath { get; set; }

        public string Response { get; set; }
        public string RecordSeparator { get; set; }
        public string FieldSeparator { get; set; }
    }



    public class Edi277Model
    {
        public string Source { get; set; }
        public string TraceNumber { get; set; }
        public string ReceiptDate { get; set; }
        public string ProcessDate { get; set; }
        public string Receiver { get; set; }
        public string TotalAcceptedClaims { get; set; }
        public string TotalAcceptedAmount { get; set; }
        public string TotalRejectedClaims { get; set; }
        public string TotalRejectedAmount { get; set; }
        public string Provider { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string AHCCCSID { get; set; }
        public string Status { get; set; }
        public string CSCC { get; set; }
        public string CSC { get; set; }
        public string EIC { get; set; }
        public string Action { get; set; }
        public string Amount { get; set; }
        public string Message { get; set; }
        public string BatchNoteID { get; set; }
        public string BatchID { get; set; }
        public string NoteID { get; set; }
        public string ClaimNumber { get; set; }
        public string PayorClaimNumber { get; set; }
        public string ServiceDate { get; set; }
    }

}
