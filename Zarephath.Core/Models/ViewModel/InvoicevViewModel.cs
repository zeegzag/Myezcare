using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Models.ViewModel
{
    public class InvoiceViewModel
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
        public int PaymentNotificationTime { get; set; }
        public bool IsFirst3DayNotification { get; set; }
        public string DomainName { get; set; }
        public string OrganizationType { get; set; }
        public string FilePath { get; set; }
        public string OrginalFileName { get; set; }
        public string EncryptedAmount { get; set; }
        public string EncryptedMonthDate { get; set; }
        public string EncryptedInvoiceNumber { get; set; }
        public string TransactionId { get; set; }


        public string TransactionIdAuthNet { get; set; }
        public string ResponseCodeAuthNet { get; set; }
        public string MessageCodeAuthNet { get; set; }
        public string DescriptionAuthNet { get; set; }
        public string AuthCodeAuthNet { get; set; }
        public string NinjaInvoiceNumber { get; set; }
        public string Statuscode { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorText { get; set; }

    }
    public class InvoiceMod
    {
        public string id { get; set; }
        public string amounts { get; set; }
        public string amount
        {
            get
            {
                if (!string.IsNullOrEmpty(amounts))
                {
                    double amt = Convert.ToDouble(amounts);
                    amt = Math.Floor(amt*100)/100;
                    return amt.ToString();

                }
                return string.Empty;
            }
        }
        public string balance { get; set; }
        public string client_id { get; set; }
        public string invoice_status_id { get; set; }
        public string updated_at { get; set; }
        public string invoice_number { get; set; }
        public string invoice_date { get; set; }
        public string DueDate { get; set; }
        public string is_deleted { get; set; }
        public string invoice_type_id { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public bool IsFirst3DayNotification { get; set; }
        public string InvoiceDate { get; set; }
        public string invoicepath { get; set; }
    }
    public class InvoiceModBilling
    {
        public string id { get; set; }
        public string amounts { get; set; }
        public string amount
        {
            get
            {
                if (!string.IsNullOrEmpty(amounts))
                {
                    double amt = Convert.ToDouble(amounts);
                    amt = Math.Floor(amt * 100) / 100;
                    return amt.ToString();

                }
                return string.Empty;
            }
        }
        public string balance { get; set; }
        public string client_id { get; set; }
        public string invoice_status_id { get; set; }
        public string updated_at { get; set; }
        public string invoice_number { get; set; }
        public string invoice_date { get; set; }
        public string DueDate { get; set; }
        public string is_deleted { get; set; }
        public string invoice_type_id { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public bool IsFirst3DayNotification { get; set; }
        public string InvoiceDate { get; set; }
        public string invoicepath { get; set; }
    }
}
