using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class SecurityQuestionModel
    {
        public SecurityQuestionModel()
        {
            SecurityQuestionList = new List<SecurityQuestion>();
            SecurityQuestionDetailModel = new SecurityQuestionDetailModel();
        }

        public SecurityQuestionDetailModel SecurityQuestionDetailModel { get; set; }
        public List<SecurityQuestion> SecurityQuestionList { get; set; }
        public AmazonSettingModel AmazonSettingModel { get; set; }


    }

    public class SecurityQuestionDetailModel
    {
        [Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "SecurityQuestionRequired", ErrorMessageResourceType = typeof(Resource))]
        public int SecurityQuestionID { get; set; }

        [Required(ErrorMessageResourceName = "SecurityAnswerRequired", ErrorMessageResourceType = typeof(Resource))]
        public string SecurityAnswer { get; set; }

        //[Required(ErrorMessageResourceName = "EmpSignatureRequired", ErrorMessageResourceType = typeof(Resource))]
        public string EmpSignature { get; set; }

        [Required(ErrorMessageResourceName = "EmpSignatureRequired", ErrorMessageResourceType = typeof(Resource))]
        public string TempSignaturePath { get; set; }
    }
}
