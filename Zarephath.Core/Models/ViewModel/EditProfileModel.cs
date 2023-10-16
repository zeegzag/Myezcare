using System.Collections.Generic;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class EditProfileModel
    {
        public EditProfileModel()
        {
            Employee = new Employee();
            SecurityQuestionList = new List<SecurityQuestion>();
        }

        public Employee Employee { get; set; }
        public List<SecurityQuestion> SecurityQuestionList { get; set; }
        public AmazonSettingModel AmazonSettingModel { get; set; }
    }
}
