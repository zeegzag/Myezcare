using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.ViewModel;
using System.Web;

namespace HomeCareApi.Infrastructure.IDataProvider
{
    public interface IEmployeeDataProvider
    {
        ApiResponse GetIVRPin(ApiRequest<string> request);
        ApiResponse SaveIVRPin(ApiRequest<EmployeeIVRModel> request);

        ApiResponse SaveProfileImage(HttpRequest currentHttpRequest, ApiRequest<EmployeeProfileImageModel> request);
        
        ApiResponse SaveSignature(HttpRequest currentHttpRequest, ApiRequest<EmployeeSignatureModel> request);

        ApiResponse AcceptAgreement(HttpRequest currentHttpRequest, ApiRequest<EmployeeAgreementModel> request);

    }
}
