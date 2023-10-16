using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IAssessmentDataProvider
    {
        ServiceResponse AddAssessmentQuestion(long questionId);
    }
}
