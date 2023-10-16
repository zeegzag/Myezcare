using System.Web.Http;
using HomeCareApi.Infrastructure;
using HomeCareApi.Infrastructure.Attributes;
using HomeCareApi.Infrastructure.DataProvider;
using HomeCareApi.Infrastructure.IDataProvider;
using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.Entity;
using HomeCareApi.Models.ViewModel;

namespace HomeCareApi.Controllers
{
    public class SecurityController : BaseController
    {
        private ISecurityDataProvider _securityDataProvider;

        #region Login/Logout/Forgot Password

        /// <summary>
        ///  this method will used for login 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [IgnoreAuthentication(true)]
        public ApiResponse<LoginDetailResponse> Login(ApiRequest<Login> request)
        {
            _securityDataProvider = new SecurityDataProvider();
            ApiResponse<LoginDetailResponse> response = _securityDataProvider.Login(request);
            return response;
        }

        /// <summary>
        /// This will be called when user logs out
        /// </summary>
        /// <param name="request">Request object</param>
        /// <returns>Instance of ApiResponse</returns>
        [HttpPost]
        [IgnoreModelValidation(true)]
        public ApiResponse Logout(ApiRequest<UserDeviceDetails> request)
        {
            _securityDataProvider = new SecurityDataProvider();
            return _securityDataProvider.Logout(request);
        }

        /// <summary>
        /// This will be called when user forgot password
        /// </summary>
        /// <param name="request">Request object</param>
        /// <returns>Instance of ApiResponse</returns>
        [HttpPost]
        [AllowAnonymous]
        [IgnoreAuthentication(true)]
        public ApiResponse ForgotPassword(ApiRequest<SendOtp> request)
        {
            _securityDataProvider = new SecurityDataProvider();
            return _securityDataProvider.ForgotPassword(request);
        }

        #endregion Login/Logout

        #region Get Employee
        /// <summary>
        /// this method will used to get employee 
        /// </summary>
        /// <param name="request">Request object</param>
        /// <returns>Instance of ApiResponse</returns>
        [HttpPost]
        [IgnoreModelValidation(true)]
        public ApiResponse<Employee> GetEmployee(ApiRequest<string> request)
        {
            _securityDataProvider = new SecurityDataProvider();
            return _securityDataProvider.GetEmployee(request);
        }
        #endregion

        #region Dashboard

        /// <summary>
        /// This will be called when Home page init
        /// </summary>
        /// <param name="request">Request object</param>
        /// <returns></returns>
        [HttpPost]
        [IgnoreModelValidation(true)]
        public ApiResponse Dashboard(ApiRequest request)
        {
            _securityDataProvider = new SecurityDataProvider();
            return _securityDataProvider.Dashboard(request, CacheHelper.EmployeeId);
        }

        #endregion
    }
}
