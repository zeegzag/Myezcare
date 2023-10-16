using System.ComponentModel.DataAnnotations;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Resources;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Zarephath.Core.Models.Entity;


namespace Zarephath.Core.Models.ViewModel
{
    public class LoginModel
    {
        public LoginModel()
        {
            Announcement = new Announcement();
            ResourceLanguageModel = new ResourceLanguageModel();
        }
        public Announcement Announcement { get; set; }
        public ResourceLanguageModel ResourceLanguageModel { get; set; }

        [Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Password { get; set; }

        public bool IsRemember { get; set; }

        public string iOSAppDownloadURL { get; set; }
        public string AndroidAppDownloadURL { get; set; }
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

    //public class SetPasswordModel

    //{
    public class SetPasswordModel
    {
        public SetPasswordModel()
        {
            Announcement = new Announcement();
            SecurityQuestionList = new List<SecurityQuestion>();
        }
        public Announcement Announcement { get; set; }
        public List<SecurityQuestion> SecurityQuestionList { get; set; }
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
        [Required(ErrorMessageResourceName = "SecurityQuestionRequired", ErrorMessageResourceType = typeof(Resource))]
        public int SecurityQuestionID { get; set; }

        [Required(ErrorMessageResourceName = "SecurityAnswerRequired", ErrorMessageResourceType = typeof(Resource))]
        public string SecurityAnswer { get; set; }
    }

    public class SetPasswordResponseModel
    {
        public bool IsAdmin { get; set; }

        public string RedirectUrl{ get; set; }

    }
    public class ResourceLanguageModel
    {
        public string Language { get; set; }
    }

    public class Announcement
    {
        public long ReleaseNoteID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public string Date
        {
            get
            {
                return Convert.ToDateTime(StartDate).ToShortDateString();
            }
        }
        public DateTime? EndDate { get; set; }
        public bool IsDeleted { get; set; }
        public string DescriptionWithOutCode { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }

        public string EncryptedReleaseNoteID { get { return Crypto.Encrypt(Convert.ToString(ReleaseNoteID)); } }
        public bool IsActive { get; set; }
    }
    public class AnnouncementList
    {
        public long ReleaseNoteID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public string Date
        {
            get
            {
                return Convert.ToDateTime(StartDate).ToShortDateString();
            }
        }
        public DateTime? EndDate { get; set; }
        public bool IsDeleted { get; set; }

        public int Row { get; set; }
        public int Count { get; set; }

        public string EncryptedReleaseNoteID { get { return Crypto.Encrypt(Convert.ToString(ReleaseNoteID)); } }
        public bool IsActive { get; set; }
    }
  
}
