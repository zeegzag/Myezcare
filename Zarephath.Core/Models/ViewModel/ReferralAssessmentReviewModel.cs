using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class ReferralAssessmentReviewModel
    {
        public ReferralAssessmentReviewModel()
        {
            PastReferralAssessmentReview = new ReferralAssessmentReview();
            ReferralAssessmentReview = new ReferralAssessmentReview();
        }
        public ReferralAssessmentReview PastReferralAssessmentReview { get; set; }
        public ReferralAssessmentReview ReferralAssessmentReview { get; set; }
        [Ignore]
        public AmazonSettingModel AmazonSettingModel { get; set; }
        [Ignore]
        public string AHCCCSID { get; set; }
    }
}
