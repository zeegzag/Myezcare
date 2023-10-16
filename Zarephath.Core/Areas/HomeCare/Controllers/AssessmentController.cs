using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Zarephath.Core.Controllers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class AssessmentController:BaseController
    {
        private IAssessmentDataProvider _assessmentDataProvider;

        [CustomAuthorize(Permissions = Constants.HC_Permission_VisitTask_AddUpdate)]
        public ActionResult AddAssessmentQuestion(string id)
        {
            long questionId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _assessmentDataProvider = new AssessmentDataProvider();
            ServiceResponse response = _assessmentDataProvider.AddAssessmentQuestion(questionId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
    }
}
