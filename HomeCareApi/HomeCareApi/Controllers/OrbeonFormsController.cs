using System.Web.Http;
using HomeCareApi.Infrastructure.DataProvider;
using HomeCareApi.Infrastructure.IDataProvider;
using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.ViewModel;
using HomeCareApi.Models.General;
using HomeCareApi.Infrastructure.Attributes;
using System.Web;
using System.Linq;
using HomeCareApi.Infrastructure;
using HomeCareApi.Resources;
using System;
using System.Net;

namespace HomeCareApi.Controllers
{
    [OrbeonFormsAuthentication]
    public class OrbeonFormsController : ApiController
    {
        private IOrbeonFormsDataProvider _orbeonFormsDataProvider;

        [HttpGet]
        public ServiceResponse Ping()
        {
            return new ServiceResponse
            {
                IsSuccess = true,
                Message = "Hey! It's working."
            };
        }

        [HttpPost]
        public ApiResponse GetCMS485Data(CMS485DataRequest request)
        {
            _orbeonFormsDataProvider = new OrbeonFormsDataProvider(request.OrganizationID);
            return _orbeonFormsDataProvider.GetCMS485Data(request);
        }

        #region Referral Details
        [HttpPost]
        public ApiResponse GetOrganizationDetail(ReferralChartDataRequest request)
        {
            _orbeonFormsDataProvider = new OrbeonFormsDataProvider(request.OrganizationID);
            return _orbeonFormsDataProvider.GetOrganizationDetail(request);
        }

        [HttpPost]
        public ApiResponse GetReferralPersonalDetail(ReferralChartDataRequest request)
        {
            _orbeonFormsDataProvider = new OrbeonFormsDataProvider(request.OrganizationID);
            return _orbeonFormsDataProvider.GetReferralPersonalDetail(request);
        }

        [HttpPost]
        public ApiResponse GetReferralContactDetail(ReferralChartDataRequest request)
        {
            _orbeonFormsDataProvider = new OrbeonFormsDataProvider(request.OrganizationID);
            return _orbeonFormsDataProvider.GetReferralContactDetail(request);
        }

        [HttpPost]
        public ApiResponse GetReferralPhysicianDetail(ReferralChartDataRequest request)
        {
            _orbeonFormsDataProvider = new OrbeonFormsDataProvider(request.OrganizationID);
            return _orbeonFormsDataProvider.GetReferralPhysicianDetail(request);
        }

        [HttpPost]
        public ApiResponse GetReferralDxCodeDetail(ReferralChartDataRequest request)
        {
            _orbeonFormsDataProvider = new OrbeonFormsDataProvider(request.OrganizationID);
            return _orbeonFormsDataProvider.GetReferralDxCodeDetail(request);
        }

        [HttpPost]
        public ApiResponse GetReferralMedicationDetail(ReferralChartDataRequest request)
        {
            _orbeonFormsDataProvider = new OrbeonFormsDataProvider(request.OrganizationID);
            return _orbeonFormsDataProvider.GetReferralMedicationDetail(request);
        }

        [HttpPost]
        public ApiResponse GetReferralNotesDetail(ReferralChartDataRequest request)
        {
            _orbeonFormsDataProvider = new OrbeonFormsDataProvider(request.OrganizationID);
            return _orbeonFormsDataProvider.GetReferralNotesDetail(request);
        }

        [HttpPost]
        public ApiResponse GetReferralPreferencesDetail(ReferralChartDataRequest request)
        {
            _orbeonFormsDataProvider = new OrbeonFormsDataProvider(request.OrganizationID);
            return _orbeonFormsDataProvider.GetReferralPreferencesDetail(request);
        }

        [HttpPost]
        public ApiResponse GetReferralTaskMappingsDetail(ReferralChartDataRequest request)
        {
            _orbeonFormsDataProvider = new OrbeonFormsDataProvider(request.OrganizationID);
            return _orbeonFormsDataProvider.GetReferralTaskMappingsDetail(request);
        }

        [HttpPost]
        public ApiResponse GetReferralPayorDetail(ReferralChartDataRequest request)
        {
            _orbeonFormsDataProvider = new OrbeonFormsDataProvider(request.OrganizationID);
            return _orbeonFormsDataProvider.GetReferralPayorDetail(request);
        }

        [HttpPost]
        public ApiResponse GetReferralAllergyDetail(ReferralChartDataRequest request)
        {
            _orbeonFormsDataProvider = new OrbeonFormsDataProvider(request.OrganizationID);
            return _orbeonFormsDataProvider.GetReferralAllergyDetail(request);
        }

        [HttpPost]
        public ApiResponse GetReferralAllergyDetailAsCommaSeparated(ReferralChartDataRequest request)
        {
            _orbeonFormsDataProvider = new OrbeonFormsDataProvider(request.OrganizationID);
            return _orbeonFormsDataProvider.GetReferralAllergyDetailAsCommaSeparated(request);
        }

        [HttpPost]
        public ApiResponse GetReferralDxCodeDetailAsCommaSeparated(ReferralChartDataRequest request)
        {
            _orbeonFormsDataProvider = new OrbeonFormsDataProvider(request.OrganizationID);
            return _orbeonFormsDataProvider.GetReferralDxCodeDetailAsCommaSeparated(request);
        }

        [HttpPost]
        public ApiResponse GetReferralBillingAuthorizationDetail(ReferralChartDataRequest request)
        {
            _orbeonFormsDataProvider = new OrbeonFormsDataProvider(request.OrganizationID);
            return _orbeonFormsDataProvider.GetReferralBillingAuthorizationDetail(request);
        }

        #endregion

        #region Employee Detail
        [HttpPost]
        public ApiResponse GetEmployeePersonalDetail(DataRequest request)
        {
            _orbeonFormsDataProvider = new OrbeonFormsDataProvider(request.OrganizationID);
            return _orbeonFormsDataProvider.GetEmployeePersonalDetail(request);
        }
        #endregion

    }
}
