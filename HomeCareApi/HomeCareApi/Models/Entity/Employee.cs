using System.ComponentModel.DataAnnotations;
using HomeCareApi.Infrastructure.Attributes;
using HomeCareApi.Resources;
using PetaPoco;

namespace HomeCareApi.Models.Entity
{
    [TableName("Employees")]
    [PrimaryKey("EmployeeID")]
    [Sort("EmployeeID", "DESC")]
    public class Employee
    {
        public long EmployeeID { get; set; }
        
        public string FirstName { get; set; }

        public string MiddleName { get; set; }
        
        public string LastName { get; set; }
        
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Password { get; set; }
        
        public string PasswordSalt { get; set; }
        
        public string PhoneWork { get; set; }
        
        public string MobileNumber { get; set; }

        public string IVRPin { get; set; }

        public bool IsActive { get; set; }

        public bool IsVerify { get; set; }
        
        public long RoleID { get; set; }
        
        public bool IsDeleted { get; set; }

        public int LoginFailedCount { get; set; }
        
        public string ProfileUrl { get; set; }
        
        public string EmployeeName { get; set; }

        public bool IsFingerPrintAuth { get; set; }

        public string OrganizationTwilioNumber { get; set; }

        public string AssociateWith { get; set; }

        public bool IsTermsConditionMobileAccepted { get; set; }
        public bool IsFirstTimeLogin { get; set; }
        public bool IsTermsConditionMobileRequired { get; set; }
       
    }
}
