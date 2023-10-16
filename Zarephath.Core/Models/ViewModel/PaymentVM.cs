using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Models.ViewModel
{
    public class PaymentVM
    {
        public string CardNumber { get; set; }
        public string CCV { get; set; }
        public string EXPIRYMM { get; set; }
        public string EXPIRYYY { get; set; }
        public string CompanyName { get; set; }
        public string BillingMonthDate { get; set; }
        public string Amount { get; set; }
        public string InvoiceNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
    }
    public class TranscationResponceVM : PaymentVM
    {
        public string TransactionId { get; set; }
        public string TrancationDate { get; set; }
        public string TrancationStatusCode { get; set; }
        public string TrancationMessage { get; set; }
        public string ResponseCode { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string AuthCode { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorText { get; set; }
        public string Text { get; set; }
    }
}
