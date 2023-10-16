using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Edi277FileDetails")]
    [PrimaryKey("Edi277FileDetailID")]
    [Sort("Edi277FileDetailID", "DESC")]
    public class Edi277FileDetail
    {
        public long Edi277FileDetailID { get; set; }
        public long Edi277FileID { get; set; }
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
