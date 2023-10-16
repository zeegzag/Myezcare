using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.Entity;
using HomeCareApi.Models.ViewModel;

namespace HomeCareApi.Infrastructure.IDataProvider
{
    public interface ISecurityDataProvider
    {
        #region CheckAppKey

        ApiResponse ValidateKey(string key);
        CachedDataForKey GetValidKeys();
        CachedData ValidateToken(string token);
        ApiResponse ValidateModel<T>(T requestData) where T : class, new();

        #endregion CheckAppKey

        #region User Token

        void DeleteUserToken(string token, string deviceUdid);

        #endregion User Token

        #region Login/Logout

        ApiResponse<LoginDetailResponse> Login(ApiRequest<Login> request);

        ApiResponse Logout(ApiRequest<UserDeviceDetails> request);
        ApiResponse ForgotPassword(ApiRequest<SendOtp> request);

        #endregion Login/Logout

        #region Dashboard

        ApiResponse Dashboard(ApiRequest request, long employeeId);

        #endregion

        #region Get Employee
        ApiResponse<Employee> GetEmployee(ApiRequest<string> request);
        #endregion
    }
}
