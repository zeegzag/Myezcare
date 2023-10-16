using System.ComponentModel.DataAnnotations;
using Myezcare_Admin.Infrastructure;
using Myezcare_Admin.Models;
using Myezcare_Admin.Resources;

namespace Myezcare_Admin.Models.ViewModel
{
    public class LoginModel
    {
        [Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Password { get; set; }

        public bool IsRemember { get; set; }
    }

    public class LoginResponseModel
    {
        public LoginResponseModel()
        {
            SessionValueData=new SessionValueData();
        }
        public SessionValueData SessionValueData { get; set; }
        public bool IsNotVerifiedEmail { get; set; }
        public string Email { get; set; }
    }

    public class SetPasswordModel
    {
        public long EmployeeId { get; set; }

        public long EncryptedMailID { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(20, MinimumLength = 8, ErrorMessageResourceName = "PasswordMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPassword, ErrorMessageResourceName = "PasswordInvalid", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "ConfirmPasswordRequired", ErrorMessageResourceType = typeof(Resource))]
        [System.Web.Mvc.Compare("Password", ErrorMessageResourceName = "PasswordAndConfirmPasswordDoNotMatch", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public class SetPasswordResponseModel
    {
        public bool IsAdmin { get; set; }

        public string RedirectUrl{ get; set; }

    }
}
