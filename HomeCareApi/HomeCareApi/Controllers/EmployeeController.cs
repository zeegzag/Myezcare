using HomeCareApi.Infrastructure;
using HomeCareApi.Infrastructure.Attributes;
using HomeCareApi.Infrastructure.DataProvider;
using HomeCareApi.Infrastructure.IDataProvider;
using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.General;
using HomeCareApi.Models.ViewModel;
using HomeCareApi.Resources;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace HomeCareApi.Controllers
{
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeDataProvider _employeeDataProvider;

        public EmployeeController()
        {
            _employeeDataProvider = new EmployeeDataProvider();
        }

        [HttpPost]
        public ApiResponse GetIVRPin(ApiRequest<string> request)
        {
            return _employeeDataProvider.GetIVRPin(request);
        }

        [HttpPost]
        [IgnoreModelValidation(true)]
        public ApiResponse SaveIVRPin(ApiRequest<EmployeeIVRModel> request)
        {
            return _employeeDataProvider.SaveIVRPin(request);
        }

        [HttpPost]
        [IgnoreActionForIncomingRequest(true)]
        [ValidateMimeMultipartContentFilter]
        //[SwaggerConsumes("multipart/form-data")]
        //[SwaggerProduces("multipart/form-data")]
        public ApiResponse SaveProfileImage()
        {
            ApiResponse response;

            if (HttpContext.Current.Request.Files.AllKeys.Any() && HttpContext.Current.Request.Files.Count > 0)
            {
                //// Get the current request contain all the information about posted files and input params
                HttpRequest currentHttpRequest = HttpContext.Current.Request; //.Files["files[]"];
                var token = HttpContext.Current.Request.Params[Constants.TokenParam];
                var key = HttpContext.Current.Request.Params[Constants.KeyParam];
                var companyName = HttpContext.Current.Request.Params[Constants.CompanyNameParam];

                var employeeID = HttpContext.Current.Request.Params[Constants.EmployeeID];

                // check token value
                if (string.IsNullOrEmpty(token))
                {
                    response = Common.ApiCommonResponse(false, Resource.TokenNotFound, Models.ApiModel.StatusCode.TokenExpired);
                    return response;
                }
                bool isValidToken = (CacheHelper.IsValidToken(token));

                if (isValidToken)
                {
                    EmployeeProfileImageModel model = new EmployeeProfileImageModel
                    {
                        EmployeeID = Convert.ToInt64(employeeID),
                    };

                    ApiRequest<EmployeeProfileImageModel> request = new ApiRequest<EmployeeProfileImageModel>
                    {
                        Data = model,
                        Key = key,
                        Token = token,
                        CompanyName = companyName
                    };
                    response = _employeeDataProvider.SaveProfileImage(currentHttpRequest, request);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, Resource.TokenNotFound, Models.ApiModel.StatusCode.TokenExpired);
                }
            }
            else
            {
                response = Common.ApiCommonResponse<FileListModel>(false, Resource.FileNotSelected, Models.ApiModel.StatusCode.NotFound);
            }
            return response;
        }

        [HttpPost]
        [IgnoreActionForIncomingRequest(true)]
        [ValidateMimeMultipartContentFilter]
        //[SwaggerConsumes("multipart/form-data")]
        //[SwaggerProduces("multipart/form-data")]
        public ApiResponse SaveSignature()
        {
            ApiResponse response;

            if (HttpContext.Current.Request.Files.AllKeys.Any() && HttpContext.Current.Request.Files.Count > 0)
            {
                //// Get the current request contain all the information about posted files and input params
                HttpRequest currentHttpRequest = HttpContext.Current.Request; //.Files["files[]"];
                var token = HttpContext.Current.Request.Params[Constants.TokenParam];
                var key = HttpContext.Current.Request.Params[Constants.KeyParam];
                var companyName = HttpContext.Current.Request.Params[Constants.CompanyNameParam];

                var employeeID = HttpContext.Current.Request.Params[Constants.EmployeeID];
                var employeeSignatureID = HttpContext.Current.Request.Params[Constants.EmployeeSignatureID];

                // check token value
                if (string.IsNullOrEmpty(token))
                {
                    response = Common.ApiCommonResponse(false, Resource.TokenNotFound, Models.ApiModel.StatusCode.TokenExpired);
                    return response;
                }
                bool isValidToken = (CacheHelper.IsValidToken(token));

                if (isValidToken)
                {
                    EmployeeSignatureModel model = new EmployeeSignatureModel
                    {
                        EmployeeID = Convert.ToInt64(employeeID),
                        EmployeeSignatureID = Convert.ToInt64(employeeSignatureID),
                    };

                    ApiRequest<EmployeeSignatureModel> request = new ApiRequest<EmployeeSignatureModel>
                    {
                        Data = model,
                        Key = key,
                        Token = token,
                        CompanyName = companyName
                    };
                    response = _employeeDataProvider.SaveSignature(currentHttpRequest, request);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, Resource.TokenNotFound, Models.ApiModel.StatusCode.TokenExpired);
                }
            }
            else
            {
                response = Common.ApiCommonResponse<FileListModel>(false, Resource.FileNotSelected, Models.ApiModel.StatusCode.NotFound);
            }
            return response;
        }

        [HttpPost]
        [IgnoreActionForIncomingRequest(true)]
        [ValidateMimeMultipartContentFilter]
        public ApiResponse AcceptAgreement()
        {
            ApiResponse response;

            if (HttpContext.Current.Request.Files.AllKeys.Any() && HttpContext.Current.Request.Files.Count > 0)
            {
                //// Get the current request contain all the information about posted files and input params
                HttpRequest currentHttpRequest = HttpContext.Current.Request; //.Files["files[]"];
                var token = HttpContext.Current.Request.Params[Constants.TokenParam];
                var key = HttpContext.Current.Request.Params[Constants.KeyParam];
                var companyName = HttpContext.Current.Request.Params[Constants.CompanyNameParam];

                var employeeID = HttpContext.Current.Request.Params[Constants.EmployeeID];
                var employeeSignatureID = HttpContext.Current.Request.Params[Constants.EmployeeSignatureID];
                var employeeLatitude = HttpContext.Current.Request.Params[Constants.Lat];
                var employeeLongitude = HttpContext.Current.Request.Params[Constants.Long];

                float lat = 0, longi = 0;
                float.TryParse(employeeLatitude, out lat);
                float.TryParse(employeeLongitude, out longi);

                // check token value
                if (string.IsNullOrEmpty(token))
                {
                    response = Common.ApiCommonResponse(false, Resource.TokenNotFound, Models.ApiModel.StatusCode.TokenExpired);
                    return response;
                }
                bool isValidToken = (CacheHelper.IsValidToken(token));

                if (isValidToken)
                {
                    EmployeeAgreementModel model = new EmployeeAgreementModel
                    {
                        EmployeeID = Convert.ToInt64(employeeID),
                        EmployeeSignatureID = Convert.ToInt64(employeeSignatureID),
                        Latitude = lat,
                        Longitude = longi    
                    };

                    ApiRequest<EmployeeAgreementModel> request = new ApiRequest<EmployeeAgreementModel>
                    {
                        Data = model,
                        Key = key,
                        Token = token,
                        CompanyName = companyName
                    };
                    response = _employeeDataProvider.AcceptAgreement(currentHttpRequest, request);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, Resource.TokenNotFound, Models.ApiModel.StatusCode.TokenExpired);
                }
            }
            else
            {
                response = Common.ApiCommonResponse<FileListModel>(false, Resource.FileNotSelected, Models.ApiModel.StatusCode.NotFound);
            }
            return response;
        }

    }
}