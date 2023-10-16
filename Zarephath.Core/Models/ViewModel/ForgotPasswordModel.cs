using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class ForgotPasswordModel
    {
        public ForgotPasswordModel()
        {
            
            SecurityQuestionList = new List<SecurityQuestion>();
            ForgotPasswordDetailModel = new ForgotPasswordDetailModel();
            Announcement = new Announcement();

        }
        
        public List<SecurityQuestion> SecurityQuestionList { get; set; }
        public ForgotPasswordDetailModel ForgotPasswordDetailModel { get; set; }
        public bool IsUnlockAccountPage { get; set; }
        public Announcement Announcement { get; set; }

    }


    public class ForgotPasswordDetailModel
    {
        [Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "SecurityQuestionRequired", ErrorMessageResourceType = typeof(Resource))]
        public int SecurityQuestionID { get; set; }

        [Required(ErrorMessageResourceName = "SecurityAnswerRequired", ErrorMessageResourceType = typeof(Resource))]
        public string SecurityAnswer { get; set; }

        public long EmployeeID { get; set; }

        [Ignore]
        public string EncryptedValue { get; set; }
    }



}
