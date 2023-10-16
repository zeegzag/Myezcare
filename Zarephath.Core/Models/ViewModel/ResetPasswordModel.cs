using System.ComponentModel.DataAnnotations;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class ResetPasswordModel
    {
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(15, MinimumLength = 8, ErrorMessageResourceName = "PasswordMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPassword, ErrorMessageResourceName = "PasswordInvalid", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "ConfirmPasswordRequired", ErrorMessageResourceType = typeof(Resource))]
        //[StringLength(15, MinimumLength = 8, ErrorMessageResourceName = "PasswordMaxLength", ErrorMessageResourceType = typeof(Resource))]
        //[RegularExpression(Constants.RegxPassword, ErrorMessageResourceName = "PasswordInvalid", ErrorMessageResourceType = typeof(Resource))]
        [System.Web.Mvc.Compare("Password", ErrorMessageResourceName = "PasswordAndConfirmPasswordDoNotMatch", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}
