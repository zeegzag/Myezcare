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
    public class DocumentController : BaseController
    {
        private IDocumentDataProvider _documentDataProvider;

        [HttpPost]
        [IgnoreModelValidation(true)]
        public ApiResponse GetSectionList(ApiRequest<GetSectionModel> request)
        {
            _documentDataProvider = new DocumentDataProvider();
            return _documentDataProvider.GetSectionList(request);
        }

        [HttpPost]
        public ApiResponse SaveSectionSubSection(ApiRequest<AddDirSubDirModal> request)
        {
            _documentDataProvider = new DocumentDataProvider();
            return _documentDataProvider.SaveSectionSubSection(request);
        }

        [HttpPost]
        public ApiResponse DeleteSectionSubSection(ApiRequest<DeleteSecSubSecModel> request)
        {
            _documentDataProvider = new DocumentDataProvider();
            return _documentDataProvider.DeleteSectionSubSection(request);
        }

        [HttpPost]
        [IgnoreModelValidation(true)]
        public ApiResponse GetOrganizationFormList(ApiRequest request)
        {
            _documentDataProvider = new DocumentDataProvider(Constants.MyezcareOrganizationConnectionString);
            return _documentDataProvider.GetOrganizationFormList(request);
        }

        [HttpPost]
        public ApiResponse GetSubSectionList(ApiRequest<GetSubSecModel> request)
        {
            _documentDataProvider = new DocumentDataProvider();
            return _documentDataProvider.GetSubSectionList(request);
        }

        [HttpPost]
        public ApiResponse GetDocumentList(ApiRequest<ListModel<SearchDocumentModel>> request)
        {
            _documentDataProvider = new DocumentDataProvider();
            return _documentDataProvider.GetDocumentList(request);
        }

        [HttpPost]
        public ApiResponse GetDocumentInfo(ApiRequest<DocumentInfoRequestModal> request)
        {
            _documentDataProvider = new DocumentDataProvider();
            return _documentDataProvider.GetDocumentInfo(request);
        }

        [HttpPost]
        public ApiResponse UpdateDocument(ApiRequest<DocumentModel> request)
        {
            _documentDataProvider = new DocumentDataProvider();
            return _documentDataProvider.UpdateDocument(request);
        }

        [HttpPost]
        public ApiResponse SaveFormPreference(ApiRequest<FormPreferenceModel> request)
        {
            _documentDataProvider = new DocumentDataProvider();
            return _documentDataProvider.SaveFormPreference(request);
        }

        [HttpPost]
        public ApiResponse OpenSavedForm(ApiRequest<OpenFormModel> request)
        {
            _documentDataProvider = new DocumentDataProvider();
            return _documentDataProvider.OpenSavedForm(request);
        }

        [HttpPost]
        public ApiResponse OpenSavedOrbeonForm(ApiRequest<OpenOrbeonFormModel> request)
        {
            _documentDataProvider = new DocumentDataProvider();
            return _documentDataProvider.OpenSavedOrbeonForm(request);
        }

        [HttpPost]
        [IgnoreActionForIncomingRequest(true)]
        [ValidateMimeMultipartContentFilter]
        public ApiResponse UploadDocument()
        {
            _documentDataProvider = new DocumentDataProvider();
            ApiResponse response;

            if (HttpContext.Current.Request.Files.AllKeys.Any() && HttpContext.Current.Request.Files.Count > 0)
            {
                //// Get the current request contain all the information about posted files and input params
                HttpRequest currentHttpRequest = HttpContext.Current.Request; //.Files["files[]"];
                var token = HttpContext.Current.Request.Params[Constants.TokenParam];
                var key = HttpContext.Current.Request.Params[Constants.KeyParam];
                var companyName = HttpContext.Current.Request.Params[Constants.CompanyNameParam];

                var employeeID = HttpContext.Current.Request.Params[Constants.EmployeeID];
                var referralID = HttpContext.Current.Request.Params[Constants.ReferralID];
                var complianceID = HttpContext.Current.Request.Params["ComplianceID"];
                var kindOfDocument = HttpContext.Current.Request.Params["KindOfDocument"];
                var fileName = HttpContext.Current.Request.Params["FileName"];
                var fileType = HttpContext.Current.Request.Params["FileType"];

                // check token value
                if (string.IsNullOrEmpty(token))
                {
                    response = Common.ApiCommonResponse(false, Resource.TokenNotFound,
                        Models.ApiModel.StatusCode.TokenExpired);
                    return response;
                }
                bool isValidToken = (CacheHelper.IsValidToken(token));

                if (isValidToken)
                {
                    PostDocumentModel model = new PostDocumentModel
                    {
                        EmployeeID = Convert.ToInt64(employeeID),
                        ReferralID = Convert.ToInt64(referralID),
                        ComplianceID = Convert.ToInt64(complianceID),
                        KindOfDocument = kindOfDocument,
                        FileName = fileName,
                        FileType = fileType
                    };

                    ApiRequest<PostDocumentModel> request = new ApiRequest<PostDocumentModel>
                    {
                        Data = model,
                        Key = key,
                        Token = token,
                        CompanyName = companyName
                    };
                    response = _documentDataProvider.UploadDocument(currentHttpRequest, request);
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
        public ApiResponse SaveForm(ApiRequest<SaveFormModel> request)
        {
            _documentDataProvider = new DocumentDataProvider();
            return _documentDataProvider.SaveForm(request);
        }

        [HttpPost]
        public ApiResponse SaveOrbeonForm(ApiRequest<SaveOrbeonFormModel> request)
        {
            _documentDataProvider = new DocumentDataProvider();
            return _documentDataProvider.SaveOrbeonForm(request);
        }

        [HttpPost]
        public ApiResponse SaveFormName(ApiRequest<SaveFormNameRequestModal>request)
        {
            _documentDataProvider = new DocumentDataProvider();
            return _documentDataProvider.SaveFormName(request);
        }

        [HttpPost]
        public ApiResponse DeleteDocument(ApiRequest<DeleteDocumentModel> request)
        {
            _documentDataProvider = new DocumentDataProvider();
            return _documentDataProvider.DeleteDocument(request);
        }

        [HttpPost]
        [IgnoreActionForIncomingRequest(true)]
        [ValidateMimeMultipartContentFilter]
        public ApiResponse UploadImage()
        {
            _documentDataProvider = new DocumentDataProvider();
            ApiResponse response;

            if (HttpContext.Current.Request.Files.AllKeys.Any() && HttpContext.Current.Request.Files.Count > 0)
            {
                //// Get the current request contain all the information about posted files and input params
                HttpRequest currentHttpRequest = HttpContext.Current.Request; //.Files["files[]"];
                var token = HttpContext.Current.Request.Params[Constants.TokenParam];
                var key = HttpContext.Current.Request.Params[Constants.KeyParam];
                var companyName = HttpContext.Current.Request.Params[Constants.CompanyNameParam];

                var employeeID = HttpContext.Current.Request.Params[Constants.EmployeeID];
                var referralID = HttpContext.Current.Request.Params[Constants.ReferralID];
                var complianceID = HttpContext.Current.Request.Params["ComplianceID"];
                var kindOfDocument = HttpContext.Current.Request.Params["KindOfDocument"];
                var fileName = HttpContext.Current.Request.Params["FileName"];
                var fileType = HttpContext.Current.Request.Params["FileType"];

                // check token value
                if (string.IsNullOrEmpty(token))
                {
                    response = Common.ApiCommonResponse(false, Resource.TokenNotFound,
                        Models.ApiModel.StatusCode.TokenExpired);
                    return response;
                }
                bool isValidToken = (CacheHelper.IsValidToken(token));

                if (isValidToken)
                {
                    PostDocumentModel model = new PostDocumentModel
                    {
                        EmployeeID = Convert.ToInt64(employeeID),
                        ReferralID = Convert.ToInt64(referralID),
                        ComplianceID = Convert.ToInt64(complianceID),
                        KindOfDocument = kindOfDocument,
                        FileName = fileName,
                        FileType = fileType
                    };

                    ApiRequest<PostDocumentModel> request = new ApiRequest<PostDocumentModel>
                    {
                        Data = model,
                        Key = key,
                        Token = token,
                        CompanyName = companyName
                    };
                    response = _documentDataProvider.UploadDocument(currentHttpRequest, request);
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

        #region Organization Settings
        [HttpPost]
        [IgnoreAuthentication(true)]
        [IgnoreModelValidation(true)]
        public ApiResponse GetOrganizationSettingDetail(ApiRequest<OrganizationDataRequest> request)
        {
            _documentDataProvider = new DocumentDataProvider();
            return _documentDataProvider.GetOrganizationSettingDetail(request);
        }
        #endregion

    }
}