using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using Elmah;
using HomeCareApi.Models.ApiModel;
using HomeCareApi.Resources;

namespace HomeCareApi.Infrastructure.Attributes
{
    public class HandleApiExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception as HttpResponseException;
            if (exception != null)
            {
                Common.BadRequest(new ApiResponse
                {
                    Code = StatusCode.TokenExpired,
                    Message = exception.Response.ReasonPhrase,
                    IsSuccess = false
                }, context.ActionContext);
                return;
            }

            var request = context.ActionContext.Request;

            var response = new ApiResponse
            {
                Message = ConfigSettings.IsShowActualError ? context.Exception.Message : Resource.ExceptionMessage,
                Code = StatusCode.InternalServerError,
                IsSuccess = false
            };

            context.Response = request.CreateResponse(HttpStatusCode.BadRequest, response);


            //Common.CreateLogFile(
            //    Common.SerializeObject(
            //            new
            //            {
            //                RefRequestResponseLogID = Convert.ToInt64(HttpContext.Current.Cache["RequestResponseLogID"]),
            //                Error = context.Exception
            //            }), "ERROR" + DateTime.Now.Ticks);
            // Handle elmah error  
            var e = context.Exception;
            RaiseErrorSignal(e);
        }

        private static void RaiseErrorSignal(Exception e)
        {
            var context = HttpContext.Current;
            if (context == null)
                return;
            var signal = ErrorSignal.FromContext(context);
            if (signal == null)
                return;
            signal.Raise(e, context);
        }
    }
}