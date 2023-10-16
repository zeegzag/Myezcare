using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using HomeCareApi.Resources;

namespace HomeCareApi.Models.ViewModel
{
    public class SendOtp
    {
        [Required(ErrorMessageResourceName = "ActionRequired", ErrorMessageResourceType = typeof(Resource))]
        public int Action { get; set; }

        [Required(ErrorMessageResourceName = "MobileRequired", ErrorMessageResourceType = typeof(Resource))]
        public string MobileNumber { get; set; }

        [RequiredIf("Action == 2",ErrorMessageResourceName = "PasswordRequired",ErrorMessageResourceType = typeof(Resource))]
        public string Password { get; set; }

        [RequiredIf("Action == 3", ErrorMessageResourceName = "OtpRequired", ErrorMessageResourceType = typeof(Resource))]
        public string OTP { get; set; }
    }
}