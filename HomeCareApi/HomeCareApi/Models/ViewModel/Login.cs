using System.ComponentModel.DataAnnotations;
using HomeCareApi.Infrastructure;
using HomeCareApi.Models.Entity;
using HomeCareApi.Resources;

namespace HomeCareApi.Models.ViewModel
{
    public class Login
    {
        [Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "UsernameNotValid", ErrorMessageResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Password { get; set; }
        
        public bool? IsLoginViaPassword { get; set; }

        public string DeviceUDID { get; set; }

        public string DeviceOSVersion { get; set; }

        [Required(ErrorMessageResourceName = "DeviceTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string DeviceType { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public CachedData CachedData { get; set; }
        public string EncyptedUserId { get; set; }
    }

    public class LoginDetailResponse
    {
        public LoginDetailResponse()
        {
            LoginResponse = new LoginResponse();
            Employee = new Employee();
            Version = new VersionCode();
        }
        public LoginResponse LoginResponse { get; set; }
        public Employee Employee { get; set; }
        public VersionCode Version { get; set; }
        public bool ShowCaptcha { get; set; }
    }

    public class UserDeviceDetails
    {
        public string DeviceUDID { get; set; }
    }


    public class UserTokens
    {
        public long UserTokenId { get; set; }
        public long EmployeeID { get; set; }
        public string Token { get; set; }
    }

    public class LoginDetails
    {
        public LoginDetails()
        {
            Employee = new Employee();
            Version = new VersionCode();
        }
        public Employee Employee { get; set; }
        public VersionCode Version { get; set; }
    }
}