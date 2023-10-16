using Ionic.Zlib;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using HomeCareApi.Infrastructure.DataProvider;
using HomeCareApi.Infrastructure.IDataProvider;
using HomeCareApi.Models.ApiModel;
using HomeCareApi.Resources;

namespace HomeCareApi.Infrastructure.Attributes
{
    public class ApiAuthorizationFilter : ActionFilterAttribute
    {
        /// <summary>
        /// This method is execute first on every api call. This will validate token and source (key).
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            if (ConfigSettings.AccessAction.Length > 0)
            {
                if (!ConfigSettings.AccessAction.Contains(actionContext.ActionDescriptor.ActionName))
                {
                    ApiResponse apiResponse = new ApiResponse
                    {
                        IsSuccess = false,
                        Code = StatusCode.InternalServerError,
                        Message = ConfigSettings.ActionRestrictMessage
                    };
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.InternalServerError,
                        apiResponse, actionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                    );
                    actionContext.Response.Headers.CacheControl = new CacheControlHeaderValue
                    {
                        MaxAge = TimeSpan.FromSeconds(86400),
                        MustRevalidate = true,
                        Public = true
                    };
                }
            }
            if (ConfigSettings.IsSaveRequestResponseLog)
            {
                // code for save request and response
                var str = actionContext.ActionArguments.Count == 0 ? "NODATA" : Common.SerializeObject(actionContext.ActionArguments[Constants.RequestModelName]);
                Common.CreateLogFile(
                Common.SerializeObject(
                        new
                        {
                            Request = "Request:" + str
                        }), "Request");
                //
            }
            //Filter Ping request
            //As it's get request we are just filtering it with get request.
            bool notOrbeonFormsAuthentication = (actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<OrbeonFormsAuthenticationAttribute>().Count == 0
                && actionContext.ActionDescriptor.GetCustomAttributes<OrbeonFormsAuthenticationAttribute>().Count == 0);
            if (actionContext.Request.Method.ToString().ToLower() != Constants.GetMethod && notOrbeonFormsAuthentication)
            {
                if (actionContext.ActionArguments.Count > 0)
                {
                    var data = actionContext.ActionArguments[Constants.RequestModelName];
                    if (data != null)
                    {
                        string key = (string)Common.GetPropValue(data, Constants.KeyParam);
                        if (!string.IsNullOrEmpty(key))
                        {
                            // Check key - Error 401
                            ISecurityDataProvider dataProvider = new SecurityDataProvider();
                            ApiResponse response = dataProvider.ValidateKey(key);
                            if (response.Code == StatusCode.Ok)
                            {
                                //Add action name in constants if ignore check for Token
                                // string requestUri = actionContext.Request.RequestUri.LocalPath.Split('/')[2].ToLower();

                                bool ignoreAuthentication = (actionContext.ActionDescriptor.GetCustomAttributes<IgnoreAuthenticationAttribute>().Count > 0)
                                    && actionContext.ActionDescriptor.GetCustomAttributes<IgnoreAuthenticationAttribute>().First().Value;

                                //if (ignoreActionForToken.Select(x => x.ToLower()).Contains(requestUri))
                                if (ignoreAuthentication)
                                    response.Code = StatusCode.Ok;
                                else
                                {
                                    // Check Token & Expires Update Time
                                    string token = (string)Common.GetPropValue(data, Constants.TokenParam);
                                    CacheHelper.IsValidToken(token);
                                }

                                if (response.Code == StatusCode.Ok)
                                {
                                    //Add action name in constants if ignore check for Data/Model

                                    bool ignoreDataValidation = (actionContext.ActionDescriptor.GetCustomAttributes<IgnoreModelValidationAttribute>().Count > 0)
                                    && actionContext.ActionDescriptor.GetCustomAttributes<IgnoreModelValidationAttribute>().First().Value;

                                    if (ignoreDataValidation)
                                    {
                                        base.OnActionExecuting(actionContext);
                                    }
                                    else
                                    {
                                        // Validate model for Data Annotation errors
                                        object requestData = Common.GetPropValue(data, Constants.DataParam);
                                        response = dataProvider.ValidateModel(requestData);
                                        if (response.Code == StatusCode.Ok)
                                        {
                                            base.OnActionExecuting(actionContext);
                                        }
                                        else
                                        {
                                            Common.BadRequest(response, actionContext);
                                        }
                                    }
                                }
                                else
                                {
                                    Common.UnauthorizedUser(response, actionContext);
                                }
                            }
                            else
                            {
                                Common.BadRequest(response, actionContext);
                            }
                        }
                        else
                        {
                            var response = Common.ApiCommonResponse(false, Resource.MSG_KeyMissingInvalid, StatusCode.BadRequest);
                            Common.UnauthorizedUser(response, actionContext);
                        }
                    }
                    else
                    {
                        ApiResponse response = Common.ApiCommonResponse(false, Resource.BadRequestMessage, StatusCode.BadRequest);
                        Common.BadRequest(response, actionContext);
                    }
                }
                else
                {
                    ApiResponse response;
                    bool ignoreActionForIncomingRequest = (actionContext.ActionDescriptor.GetCustomAttributes<IgnoreActionForIncomingRequestAttribute>().Count > 0)
                                    && actionContext.ActionDescriptor.GetCustomAttributes<IgnoreActionForIncomingRequestAttribute>().First().Value;

                    if (ignoreActionForIncomingRequest)
                    {
                        if (HttpContext.Current.Request.Files.AllKeys.Any())
                        {
                            var key = HttpContext.Current.Request.Params[Constants.KeyParam];

                            if (!string.IsNullOrEmpty(key))
                            {
                                // Check key - Error 401
                                ISecurityDataProvider dataProvider = new SecurityDataProvider();
                                response = dataProvider.ValidateKey(key);
                                if (response.Code == StatusCode.Ok)
                                {
                                    base.OnActionExecuting(actionContext);
                                }
                                else
                                {
                                    Common.BadRequest(response, actionContext);
                                }
                            }
                            else
                            {
                                response = Common.ApiCommonResponse(false, Resource.MSG_KeyMissingInvalid, StatusCode.BadRequest);
                                Common.UnauthorizedUser(response, actionContext);
                            }
                        }
                        else
                        {
                            response = Common.ApiCommonResponse(false, Resource.FileNotSelected,StatusCode.NotFound);
                            Common.BadRequest(response, actionContext);
                        }
                    }
                    else
                    {
                        response = Common.ApiCommonResponse(false, Resource.BadRequestMessage, StatusCode.BadRequest);
                        Common.BadRequest(response, actionContext);
                    }
                }
            }
            else
            {
                base.OnActionExecuting(actionContext);
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (ConfigSettings.IsSaveRequestResponseLog)
            {
                if (actionExecutedContext.Exception != null)
                {

                    Common.CreateLogFile(
                        Common.SerializeObject(
                                new
                                {
                                    Response = "Response" + '\n' + "URL" + actionExecutedContext.Request.RequestUri.AbsolutePath + '\n' +
                                    "ERRORRESPONSE:" + Common.SerializeObject(actionExecutedContext.Exception)
                                }), "Response");
                }
                else
                {
                    var objectContent = actionExecutedContext.Response.Content as ObjectContent;
                    if (objectContent != null)
                    {
                        var type = objectContent.ObjectType; //type of the returned object
                        var value = objectContent.Value; //holding the returned value
                        var str = Common.SerializeObject(value);

                        Common.CreateLogFile(
                        Common.SerializeObject(
                                new
                                {
                                    Response = "Response" + '\n' + "URL" + actionExecutedContext.Request.RequestUri.AbsolutePath + '\n' +
                                    "Str:" + str,
                                    Type = type
                                }), "Response");
                    }
                }
            }

            //ToDO:This is For What?
            //Set cache for response
            if (actionExecutedContext.Response != null)
            {
                #region Gzip Api Content Length reduce

                var content = actionExecutedContext.Response.Content;
                var bytes = content?.ReadAsByteArrayAsync().Result;
                var zlibbedContent = bytes == null ? new byte[0] : CompressionHelper.GzipByte(bytes);
                actionExecutedContext.Response.Content = new ByteArrayContent(zlibbedContent);
                actionExecutedContext.Response.Content.Headers.Remove("Content-Type");
                actionExecutedContext.Response.Content.Headers.Add("Content-encoding", "gzip");
                actionExecutedContext.Response.Content.Headers.Add("Content-Type", "application/json");

                #endregion

                actionExecutedContext.Response.Headers.CacheControl = new CacheControlHeaderValue
                {
                    MaxAge = TimeSpan.FromSeconds(86400),
                    MustRevalidate = true,
                    Public = true

                };
                actionExecutedContext.Response.Headers.Add("Accept", "application/json");
            }
        }

        public class CompressionHelper
        {
            public static byte[] GzipByte(byte[] str)
            {
                if (str == null)
                {
                    return null;
                }

                using (var output = new MemoryStream())
                {
                    using (var compressor = new GZipStream(output, CompressionMode.Compress, CompressionLevel.BestCompression))
                    {
                        compressor.Write(str, 0, str.Length);
                    }
                    return output.ToArray();
                }
            }
        }
    }
}