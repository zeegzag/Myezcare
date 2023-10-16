using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class AssessmentDataProvider: BaseDataProvider, IAssessmentDataProvider
    {
        public ServiceResponse AddAssessmentQuestion(long questionId)
        {
            ServiceResponse response = new ServiceResponse();

            AssessmentModel assessmentModel = GetMultipleEntity<AssessmentModel>(StoredProcedure.SetAddAssessmentQuestionPage,
                new List<SearchValueData>
                {
                    new SearchValueData { Name = "AssessmentQuestionID", Value = Convert.ToString(questionId) },
                    new SearchValueData { Name = "DDType_AssessmentQuestionCategory", Value = Convert.ToString((int)Common.DDType.AssessmentQuestionCategory) },
                    new SearchValueData { Name = "DDType_AssessmentQuestionSubCategory", Value = Convert.ToString((int)Common.DDType.AssessmentQuestionSubCategory) }
                });
            
            response.Data = assessmentModel;
            return response;
        }
    }
}
