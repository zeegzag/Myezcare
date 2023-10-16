using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Invoice")]
    [PrimaryKey("InvoiceNumber")]
    public class Invoice
    {
        public long InvoiceNumber { get; set; }
        public int MonthId { get; set; }
        public long OrganizationId { get; set; }

        public DateTime InvoiceDate { get; set; }

        public string PlanName { get; set; }

        public DateTime DueDate { get; set; }

        public int ActivePatientQuantity { get; set; }
        public decimal ActivePatientUnit { get; set; }
        public decimal ActivePatientAmount { get; set; }
        public int NumberOfTimeSheetQuantity { get; set; }

        public decimal NumberOfTimeSheetUnit { get; set; }
        public decimal NumberOfTimeSheetAmount { get; set; }
        public int IVRQuantity { get; set; }
        public decimal IVRUnit { get; set; }
        public decimal IVRAmount { get; set; }
        public int MessageQuantity { get; set; }
        public decimal MessageUnit { get; set; }
        public decimal MessageAmount { get; set; }


        public int ClaimsQuantity { get; set; }
        public decimal ClaimsUnit { get; set; }
        public decimal ClaimsAmount { get; set; }
        public int FormsQuantity { get; set; }
        public decimal FormsUnit { get; set; }
        public decimal FormsAmount { get; set; }
        public string InvoiceStatus { get; set; }
        public bool Status { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string FilePath { get; set; }
        public string OrginalFileName { get; set; }
        public bool IsPaid { get; set; }
        public string PaymentDate { get; set; }
        public string UpdatedDate { get; set; }
        public string PaidAmount { get; set; }
        public string TransactionId { get; set; }
    }
}
