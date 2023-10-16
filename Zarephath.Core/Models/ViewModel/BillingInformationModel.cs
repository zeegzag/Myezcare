using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Models.ViewModel
{
    public class BillingInformationModel
    {
        public long ProfileNumber { get; set; }
        public long OrganizationId { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string AccountNumber { get; set; }
        public string RoutingNumber { get; set; }
        public string NameOnAccount { get; set; }
        public string BankName { get; set; }
        public long customerProfileId { get; set; }
        public long customerPaymentProfileId { get; set; }
        public long customerShippingAddressId { get; set; }
        public string Statuscode { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorText { get; set; }
    }
}
