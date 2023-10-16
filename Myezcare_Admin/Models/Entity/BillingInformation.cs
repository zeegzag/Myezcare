using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Myezcare_Admin.Models.Entity
{
    [TableName("BillingInformation")]
    [PrimaryKey("ProfileNumber")]
    public class BillingInformation
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
    }
}