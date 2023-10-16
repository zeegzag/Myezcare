using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class UserPaymentDetailModel
    {
        [Required(ErrorMessage = "CardNumberRequired")]
        [RegularExpression(@"^([0-9]{16})$", ErrorMessage = "Invalid Card Number.")]
        public string CardNumber { get; set; }
         [Required(ErrorMessage = "CardExpirationDateRequired")]
        [RegularExpression(@"^([0-9]{4})$", ErrorMessage = "Invalid Expiration Number. Please enter MMYY format")]
        public string ExpirationDate { get; set; }
        [Required(ErrorMessage = "CardNumberRequired")]
        [RegularExpression(@"^([0-9]{9})$", ErrorMessage = "Invalid Account Number.")]
        public string AccountNumber { get; set; }
        [Required(ErrorMessage = "RoutingNumberRequired")]
        [RegularExpression(@"^([0-9]{9})$", ErrorMessage = "Invalid Routing Number.")]
        public string RoutingNumber { get; set; }
        [Required(ErrorMessage = "NameOnAccountRequired")]
        public string NameOnAccount { get; set; }
        [Required(ErrorMessage = "BankNameRequired")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "HomeAddressRequired")]
        public string HomeAddress { get; set; }
        [Required(ErrorMessage = "HomeAddressCityRequired")]
        public string HomeAddressCity { get; set; }
        [Required(ErrorMessage = "HomeAddressZipRequired")]
        public string HomeAddressZip { get; set; }

        [Required(ErrorMessage = "OfficeAddressRequired")]
        public string OfficeAddress { get; set; }
        [Required(ErrorMessage = "OfficeAddressCityRequired")]
        public string OfficeAddressCity { get; set; }
        [Required(ErrorMessage = "OfficeAddressZipRequired")]
        public string OfficeAddressZip { get; set; }
        
        public string MerchantCustomerId { get; set; }
        public bool TermsNConditions { get; set; }
       
    }



}
