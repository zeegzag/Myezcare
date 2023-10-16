using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class AssessmentModel
    {
        public AssessmentModel()
        {
            AssessmentQuestionCategoryList = new List<NameValueData>();
            AssessmentQuestionSubCategoryList = new List<NameValueData>();
            AssesmentQuestion = new AssessmentQuestion();
            AssessmentOption = new AssessmentOption();
        }
        public List<NameValueData> AssessmentQuestionCategoryList { get; set; }
        public List<NameValueData> AssessmentQuestionSubCategoryList { get; set; }
        public AssessmentQuestion AssesmentQuestion { get; set; }
        public AssessmentOption AssessmentOption { get; set; }
    }
}
