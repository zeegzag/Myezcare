using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;

namespace Zarephath.Core.Models.ViewModel
{
    public class MissingDocumentEmailTokenModel : EmailTokenBase
    {
        


        public string TagZerpathLogoImage
        {
            get
            {
                CacheHelper cacheHelper = new CacheHelper();
                return "<img src='" + cacheHelper.TemplateLogo + "' width='300'/>";
                //return "<img src='" + cacheHelper.SiteBaseURL + Constants.ZerpathLogoImage + "' width='300'/>";
            }
        }
        public string ClientList { get; set; }
        public string CaseManager { get; set; }
        public string MonthName { get; set; }

    }

    #region Missing Document Model

    public class EmailWiseReferralList
    {
        public string RecordRequestEmail { get; set; }
        public List<ReferralListForMissingDocumentEmail> ReferralList { get; set; }
    }

    public class ReferralListForMissingDocumentEmail
    {
        public long ReferralID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public string AHCCCSID { get; set; }
        public string CaseManager { get; set; }

        public string MissingDocumentDetails { get; set; }
        public string RecordRequestEmail { get; set; }


        public string ServicePlanExpirationDate { get; set; }
        public bool ServicePlanExpirationStatus { get; set; }
        public string SPGuardianSignature { get; set; }
        public bool SPGuardianSignatureStatus { get; set; }
        public string SPBHPSignature { get; set; }
        public bool SPBHPSignatureStatus { get; set; }
        public string SPIdentify { get; set; }
        public bool SPIdentifyStatus { get; set; }

        public string BXAssessmentExpirationDate { get; set; }
        public bool BXAssessmentExpirationStatus { get; set; }
        public string BXAssessmentBHPSigned { get; set; }
        public bool BXAssessmentBHPSignedStatus { get; set; }


        public string Demographic { get; set; }
        public bool DemographicStatus { get; set; }

        public string SNCD { get; set; }
        public bool SNCDStatus { get; set; }

        public string SNCDCompletionDate { get; set; }
        public bool SNCDCompletionDateStatus { get; set; }

        public string ROIExpirationDate { get; set; }
        public bool ROIExpirationDateStatus { get; set; }

        public string SentEmailTemplate { get; set; }
        //public string MissingDocumentDetails { get; set; }

    }

    #endregion

    #region  Service Plan Model

    public class EmailWiseServicePlanList
    {
        public string RecordRequestEmail { get; set; }
        public List<ServicePlanListModel> ServicePlanList { get; set; }
    }

    public class ServicePlanListModel
    {
        public long ReferralID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AHCCCSID { get; set; }
        public DateTime Dob { get; set; }
        public string RecordRequestEmail { get; set; }
        public string CaseManager { get; set; }
        public string SentEmailTemplate { get; set; }
    }

    #endregion

}
