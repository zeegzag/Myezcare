using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Myezcare_Admin.Models.ViewModel
{
    public class InvoiceModel
    {
        public string MonthName { get; set; }
        public string MonthId { get; set; }
        public string InvoiceNumber { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationId { get; set; }
        public string InvoiceDate { get; set; }
        public string PlanName { get; set; }
        public string PlanId { get; set; }
        public string DueDate { get; set; }
        public string ActivePatient { get; set; }
        public string NumberofTimeSheet { get; set; }
        public string IVR { get; set; }
        public string Messages { get; set; }
        public string Claims { get; set; }
        public string Forms { get; set; }
        public string InvoiceStatus { get; set; }
        public string InvoiceStatusName { get; set; }
        public string InvoiceAmount { get; set; }
        public string PaidAmount { get; set; }
        public bool IsAll { get; set; }
        public bool IsPaid { get; set; }
        public string ActivePatientQuantity { get; set; }
        public string ActivePatientUnit { get; set; }
        public string ActivePatientAmount { get; set; }

        public string NumberOfTimeSheetQuantity { get; set; }
        public string NumberOfTimeSheetUnit { get; set; }
        public string NumberOfTimeSheetAmount { get; set; }

        public string IVRQuantity { get; set; }
        public string IVRUnit { get; set; }
        public string IVRAmount { get; set; }

        public string MessageQuantity { get; set; }
        public string MessageUnit { get; set; }
        public string MessageAmount { get; set; }


        public string ClaimsQuantity { get; set; }
        public string ClaimsUnit { get; set; }
        public string ClaimsAmount { get; set; }

        public string FormsQuantity { get; set; }
        public string FormsUnit { get; set; }
        public string FormsAmount { get; set; }

        public bool Status { get; set; }

        public string DomainName { get; set; }
        public string OrganizationType { get; set; }
        public string FilePath { get; set; }
        public string OrginalFileName { get; set; }



    }
}