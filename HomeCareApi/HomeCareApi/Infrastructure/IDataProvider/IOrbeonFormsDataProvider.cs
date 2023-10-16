using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.General;
using HomeCareApi.Models.ViewModel;
using System.Web;

namespace HomeCareApi.Infrastructure.IDataProvider
{
    public interface IOrbeonFormsDataProvider
    {
        ApiResponse GetCMS485Data(CMS485DataRequest request);

        #region Referral Detail
        ApiResponse GetOrganizationDetail(ReferralChartDataRequest request);
        ApiResponse GetReferralPersonalDetail(ReferralChartDataRequest request);
        ApiResponse GetReferralContactDetail(ReferralChartDataRequest request);
        ApiResponse GetReferralPhysicianDetail(ReferralChartDataRequest request);
        ApiResponse GetReferralDxCodeDetail(ReferralChartDataRequest request);
        ApiResponse GetReferralMedicationDetail(ReferralChartDataRequest request);
        ApiResponse GetReferralNotesDetail(ReferralChartDataRequest request);
        ApiResponse GetReferralPreferencesDetail(ReferralChartDataRequest request);
        ApiResponse GetReferralTaskMappingsDetail(ReferralChartDataRequest request);
        ApiResponse GetReferralPayorDetail(ReferralChartDataRequest request);
        ApiResponse GetReferralAllergyDetail(ReferralChartDataRequest request);
        ApiResponse GetReferralAllergyDetailAsCommaSeparated(ReferralChartDataRequest request);
        ApiResponse GetReferralDxCodeDetailAsCommaSeparated(ReferralChartDataRequest request);
        ApiResponse GetReferralBillingAuthorizationDetail(ReferralChartDataRequest request);
        #endregion

        #region Employee Detail
        ApiResponse GetEmployeePersonalDetail(DataRequest request);
        #endregion

    }
}