using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;
using Microsoft.SqlServer.Types;
using ExpressiveAnnotations.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Employees")]
    [PrimaryKey("EmployeeID")]
    [Sort("EmployeeID", "DESC")]
    public class Employee : BaseEntity
    {
        public long EmployeeID { get; set; }

        [Required(ErrorMessageResourceName = "EmployeeIDRequired", ErrorMessageResourceType = typeof(Resource))]
        public string EmployeeUniqueID { get; set; }

        [Required(ErrorMessageResourceName = "FirstNameRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^(([A-za-a]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please Enter Alphabets Only")]
        public string FirstName { get; set; }
        [RegularExpression(@"^(([A-za-a]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please Enter Alphabets Only")]
        public string MiddleName { get; set; }
        [RegularExpression(@"^(([A-za-a]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please Enter Alphabets Only")]
        [Required(ErrorMessageResourceName = "LastNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string UserName { get; set; }

        //   [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(20, MinimumLength = 8, ErrorMessageResourceName = "PasswordMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPassword, ErrorMessageResourceName = "PasswordInvalid", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        public string PasswordSalt { get; set; }

        [Required(ErrorMessageResourceName = "PhoneWorkRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidWorkPhone", ErrorMessageResourceType = typeof(Resource))]
        public string PhoneWork { get; set; }

        [Required(ErrorMessageResourceName = "PhoneHomeRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidHomePhone", ErrorMessageResourceType = typeof(Resource))]
        public string PhoneHome { get; set; }

        [RequiredIf("RoleID==Role.RoleEnum.Nurse", ErrorMessageResourceName = "MobileNumberRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidMobileNumber", ErrorMessageResourceType = typeof(Resource))]
        public string MobileNumber { get; set; }

        //[Required(ErrorMessageResourceName = "DepartmentRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? DepartmentID { get; set; }

        //[RegularExpression(Constants.RegxSSN, ErrorMessage = "Invalid Social Security Number")]
        //public string SocialSecurityNumber { get; set; }

         // [CustomRegularExpression(@"^\d{3}-\d{2}-\d{4}$", ErrorMessage = "Invalid SSN format.")]
        [RegularExpression(Constants.RegxIDCard, ErrorMessage = "Invalid ID Card Number")]
        public string SocialSecurityNumber { get; set; }
        public bool IsDepartmentSupervisor { get; set; }

        [Required(ErrorMessageResourceName = "SecurityQuestionRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? SecurityQuestionID { get; set; }

        [Required(ErrorMessageResourceName = "SecurityAnswerRequired", ErrorMessageResourceType = typeof(Resource))]
        public string SecurityAnswer { get; set; }

        [Required(ErrorMessageResourceName = "EmpSignatureRequired", ErrorMessageResourceType = typeof(Resource))]
        public string EmpSignature { get; set; }

        public long EmployeeSignatureID { get; set; }

        public bool IsActive { get; set; }
        public bool IsVerify { get; set; }

        public string GroupIDs { get; set; }

        public int LoginFailedCount { get; set; }

        [Required(ErrorMessageResourceName = "RoleIDRequired", ErrorMessageResourceType = typeof(Resource))]
        public long RoleID { get; set; }

        public bool IsSecurityQuestionSubmitted { get; set; }

        [Ignore]
        [Required(ErrorMessageResourceName = "ConfirmPasswordRequired", ErrorMessageResourceType = typeof(Resource))]
        //[StringLength(20, MinimumLength = 8, ErrorMessageResourceName = "PasswordMaxLength", ErrorMessageResourceType = typeof(Resource))]
        //[RegularExpression(Constants.RegxPassword, ErrorMessageResourceName = "PasswordInvalid", ErrorMessageResourceType = typeof(Resource))]
        [System.Web.Mvc.Compare("Password", ErrorMessageResourceName = "PasswordAndConfirmPasswordDoNotMatch", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Ignore]
        public string EncryptedValue { get; set; }

        [Ignore]
        // [Required(ErrorMessageResourceName = "NewPasswordRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(20, MinimumLength = 8, ErrorMessageResourceName = "PasswordMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPassword, ErrorMessageResourceName = "PasswordInvalid", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Ignore]
        //[StringLength(20, MinimumLength = 8, ErrorMessageResourceName = "PasswordMaxLength", ErrorMessageResourceType = typeof(Resource))]
        //[RegularExpression(Constants.RegxPassword, ErrorMessageResourceName = "PasswordInvalid", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordAndConfirmPasswordDoNotMatch", ErrorMessageResourceType = typeof(Resource))]
        public string TempConfirmPassword { get; set; }

        public bool IsDeleted { get; set; }

        [Required(ErrorMessageResourceName = "CredentialIDRequired", ErrorMessageResourceType = typeof(Resource))]
        public string CredentialID { get; set; }

        public string Degree { get; set; }


        //[Required(ErrorMessageResourceName = "LatitudeRequired", ErrorMessageResourceType = typeof(Resource))]
        public double? Latitude { get; set; }
        //[Required(ErrorMessageResourceName = "LongitudeRequired", ErrorMessageResourceType = typeof(Resource))]
        public double? Longitude { get; set; }

        [RequiredIf("RoleID==18", ErrorMessageResourceName = "IVRPinRequired", ErrorMessageResourceType = typeof(Resource))]
        public string IVRPin { get; set; }

        [Required(ErrorMessageResourceName = "AssociateWithRequired", ErrorMessageResourceType = typeof(Resource))]
        public string AssociateWith { get; set; }

        [Ignore]
        [Required(ErrorMessageResourceName = "EmpSignatureRequired", ErrorMessageResourceType = typeof(Resource))]
        public string TempSignaturePath { get; set; }

        public string ApartmentNo { get; set; }
        [Required(ErrorMessageResourceName = "AddressRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Address { get; set; }
        [Required(ErrorMessageResourceName = "CityRequired", ErrorMessageResourceType = typeof(Resource))]
        public string City { get; set; }
        [Required(ErrorMessageResourceName = "StateRequired", ErrorMessageResourceType = typeof(Resource))]
        public string StateCode { get; set; }
        [Required(ErrorMessageResourceName = "ZipCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ZipCode { get; set; }

        public string HHA_NPI_ID { get; set; }

        public string CareTypeIds { get; set; }

        [Ignore]
        public string id
        {
            get { return Convert.ToString(EmployeeID); }
        }
        [Ignore]
        public string EmployeeName { get { return String.Format("{0} {1}", FirstName, LastName); } }

        [Ignore]
        public string EmpGeneralNameFormat
        {
            get { return Common.GetGenericNameFormat(FirstName, MiddleName, LastName); }
        }

        [Ignore]
        public string EmployeeHours { get { return "160"; } }

        [Ignore]
        public string CalculatedEmployeeHours { get { return "160"; } }

        [Ignore]
        public string eventColor
        {
            get
            {
                if (EmployeeID == 2)
                {
                    return "red";
                }

                if (EmployeeID % 4 == 0)
                {
                    return "green";
                }
                if (EmployeeID % 5 == 0)
                {
                    return "orange";
                }
                if (EmployeeID % 6 == 0)
                {
                    return "#f751d8";
                }
                return "";
            }
        }

        [ResultColumn]
        public string ProfileImagePath { get; set; }

        [ResultColumn]
        public string ProfileImageAccessPath { get => Common.GetAccessPath(ProfileImagePath); }

        [ResultColumn]
        public string str_Role { get; set; }

        [ResultColumn]
        public string str_Designation { get; set; }

        [ResultColumn]
        public string EmployeeFullName { get; set; }

        [Ignore]
        public string EncryptedEmployeeID
        {
            get { return EmployeeID != 0 ? Crypto.Encrypt(Convert.ToString(EmployeeID)) : string.Empty; }
        }

        public string Designation { get; set; } // NOTE: due to I have used this field in drop down I have declare as string.
        public string DesignationName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string FacilityID { get; set; }
        public decimal? RegularHours { get; set; }
        public int? RegularHourType { get; set; }
        public double? RegularPayHours { get; set; }
        public double? OvertimePayHours { get; set; }
        //[Ignore]
        //public double? OvertimeHours { get; set; }

        [Required(ErrorMessageResourceName = "HireDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? HireDate { get; set; }
        [Required(ErrorMessageResourceName = "GenderRequired", ErrorMessageResourceType = typeof(Resource))]
        public string EmpGender { get; set; }
        public string StateRegistrationID { get; set; }
        public string ProfessionalLicenseNumber { get; set; }
        public bool CanUpdateCoordinate { get; set; }
    }
    public class EmployeeVisitReport
    {
        public string ShortName { get; set; }
        public string EmployeeUniqueID { get; set; }
        public TimeSpan? ClockInTime { get; set; }
        public TimeSpan? ClockOutTime { get; set; }
        public string EmployeeName { get; set; }
        public string Value { get; set; }
        public string ReferralName { get; set; }
        public long ReferralID { get; set; }
        public string PayPerHour { get; set; }
        public string BillableHrs { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CareType { get; set; }
        public string PayorName { get; set; }
        public string RoleName { get; set; }
        public string ServiceCode { get; set; }
        public long Amount { get; set; }
    }
    //public class GetOrganizationRpt
    //{
    //    public string SiteLogo { get; set; }
    //    public string SiteName { get; set; }
    //    public string Submitter_NM109_IdCode { get; set; }
    //    public string Submitter_EDIContact1_PER04_CommunicationNumber { get; set; }
    //    public string SiteLSubmitter_EDIContact1_PER08_CommunicationNumberogo { get; set; }
    //    public string TemplateLogo { get; set; }
    //    public string Submitter_NM103_NameLastOrOrganizationName { get; set; }
    //    public string OrganizationAddress { get; set; }
    //}
    public class EmployeeActiveRpt
    {
        public long EmployeeID { get; set; }
        public string EmployeeUniqueID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public long? DepartmentID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class PriorAuthExpireRpt
    {
        public string PayorName { get; set; }
        public string PatientName { get; set; }
        public string AuthorizationCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? AllowedTime { get; set; }
    }
    public class EmployeeTimeSheetRpt
    {
        public string PayorName { get; set; }
        public string EmployeeName { get; set; }
        public string PatientName { get; set; }
        public TimeSpan? ClockInTime { get; set; }
        public TimeSpan? ClockOutTime { get; set; }
        public string CareType { get; set; }
        public bool IsPCACompleted { get; set; }
        public TimeSpan? TotalHours { get; set; }
    }
    public class PatientHourSummaryRpt
    {
        public long RefId { get; set; }
        public string PatientName { get; set; }
        public int PatientVisits { get; set; }
        public double VisitHours { get; set; }
        public double Amount { get; set; }
    }
    public class EmployeeVisitRpt
    {
        public string ShortName { get; set; }
        public string EmployeeUniqueID { get; set; }
        public TimeSpan? ClockInTime { get; set; }
        public TimeSpan? ClockOutTime { get; set; }
        public string EmployeeName { get; set; }
        public string Value { get; set; }
        public string ReferralName { get; set; }
        public long ReferralID { get; set; }
        public string PayPerHour { get; set; }
        public string BillableHrs { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
