using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Elmah;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models;
using Zarephath.Core.Resources;
using System.Web.Http;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Controllers
{
    //[PaymentFilter]
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = HttpContext.Request;
            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, HttpRuntime.AppDomainAppVirtualPath == "/" ? "" : HttpRuntime.AppDomainAppVirtualPath);

            //if (SessionHelper.CompanyInvoiceInfo != null && SessionHelper.CompanyInvoiceInfo.Count > 0)
            //{
            //    var isOverDue = checkOverDuePayment();
            //    if (isOverDue.Count > 0 && !baseUrl.Contains("PaymentPayNow"))
            //    {
            //        filterContext.Result = new ViewResult() { ViewName = "~/AccessDenied.cshtml" };
            //    }
            //}
            //else
            //{
            ViewBag.BasePath = baseUrl;
            //}
            base.OnActionExecuting(filterContext);

        }
        #region PaymentFilter
        private List<InvoiceMod> checkOverDuePayment()
        {
            List<InvoiceMod> checkPaymentDueDateOver4 = SessionHelper.CompanyInvoiceInfo != null && SessionHelper.CompanyInvoiceInfo.Count > 0 ?
        SessionHelper.CompanyInvoiceInfo.Where(x => DateTime.Now.AddDays(17).Date > Convert.ToDateTime(x.DueDate).AddDays(13).Date).ToList() :
        new List<InvoiceMod>();

            return checkPaymentDueDateOver4;
        }
        #endregion
        public object GetQueryStringValue(string encryptedQueryString, string queryname)
        {
            if (!string.IsNullOrEmpty(encryptedQueryString))
            {
                string invitation = Crypto.Decrypt(encryptedQueryString);
                if (!invitation.Contains("&"))
                {
                    if (invitation.StartsWith(queryname))
                    {
                        string[] values = invitation.Split('=');
                        return values[1];
                    }
                }
                else
                {
                    string[] invitations = invitation.Split('&');
                    return
                        (from inv in invitations where inv.StartsWith(queryname) select inv.Split('=')).Select(
                            values => values[1]).FirstOrDefault();
                }
            }
            return null;
        }

        public List<T> AddEncryptedURL<T>(List<T> items, string queryName) where T : class, new()
        {
            T itm = new T();
            PropertyInfo queryNameInfo = itm.GetType().GetProperty(queryName);
            PropertyInfo encryptedURLInfo = itm.GetType().GetProperty(Constants.EncryptedQueryString);
            if (queryNameInfo != null && encryptedURLInfo != null)
            {
                foreach (T item in items)
                {
                    string encryptedQueryString = Crypto.Encrypt(string.Format("{0}={1}", queryName, queryNameInfo.GetValue(item, null)));
                    encryptedURLInfo.SetValue(item, encryptedQueryString, null);
                }
            }
            return items;
        }

        public List<T> AddEncryptedURL<T>(List<T> items, string[] queryNames) where T : class, new()
        {
            T itm = new T();
            PropertyInfo encryptedURLInfo = itm.GetType().GetProperty(Constants.EncryptedQueryString);
            if (encryptedURLInfo != null)
            {
                foreach (T item in items)
                {
                    string encryptedQueryString = "";
                    foreach (string queryName in queryNames)
                    {
                        PropertyInfo queryNameInfo = itm.GetType().GetProperty(queryName);
                        if (queryNameInfo != null)
                            encryptedQueryString += string.Format("{0}={1}&", queryName, queryNameInfo.GetValue(item, null));
                    }
                    if (!string.IsNullOrEmpty(encryptedQueryString))
                        encryptedURLInfo.SetValue(item, Crypto.Encrypt(encryptedQueryString.TrimEnd('&')), null);
                }
            }

            return items;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                var httpContext = filterContext.HttpContext.ApplicationInstance.Context;
                var signal = ErrorSignal.FromContext(httpContext);
                signal.Raise(filterContext.Exception, httpContext);

                //Common.CreateLogFile(Common.SerializeObject(filterContext.Exception));
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    filterContext.Result = new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new
                        {
                            filterContext.Exception.Message
                        }
                    };
                    //filterContext.Result = RedirectToAction("InternalError", "Security");
                }
                else
                {
                    //filterContext.Result = RedirectToAction("InternalError", "Security");

                    filterContext.Result = new ViewResult
                    {
                        ViewName = "~/Views/Shared/Error.cshtml",
                    };


                    //filterContext.Result = RedirectToAction("InternalError", "Security");

                    string viewName = "~/Views/Shared/Error.cshtml";
                    if (filterContext.Exception.GetType() == typeof(HttpResponseException))
                    {
                        HttpResponseException exception = filterContext.Exception as HttpResponseException;
                        if (exception.Response.ReasonPhrase == Resource.DomainNotExist && exception.Response.StatusCode == HttpStatusCode.NotAcceptable)
                            viewName = "~/Views/Shared/DomainNotFound.cshtml";

                    }

                    filterContext.Result = new ViewResult
                    {
                        ViewName = viewName
                    };


                }


                if (filterContext.Exception.GetType() == typeof(HttpException))
                {
                    HttpException exception = filterContext.Exception as HttpException;
                    filterContext.HttpContext.Response.StatusCode = exception.GetHttpCode();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult RenderView(ServiceResponse response, string viewName, bool isSendDataToView = false)
        {
            if (!response.IsSuccess)
            {
                if (response.ErrorCode == Constants.ErrorCode_AccessDenied)
                    return RedirectToAction("accessdenied", "security");
                if (response.ErrorCode == Constants.ErrorCode_InternalError)
                    return RedirectToAction("internalerror", "security");
                if (response.ErrorCode == Constants.ErrorCode_NotFound)
                    return RedirectToAction("notFound", "security");
            }
            else
            {
                return isSendDataToView ? View(viewName, response.Data) : View(viewName);
            }

            return null;
        }

        public ActionResult ShowUserFriendlyPages(ServiceResponse response)
        {
            if (!response.IsSuccess)
            {
                if (response.ErrorCode == Constants.ErrorCode_AccessDenied)
                    return RedirectToAction("accessdenied", "security");
                if (response.ErrorCode == Constants.ErrorCode_InternalError)
                    return RedirectToAction("internalerror", "security");
                if (response.ErrorCode == Constants.ErrorCode_NotFound)
                    return RedirectToAction("notFound", "security");
                //return RedirectToAction("notfound", "security");
            }
            return null;
        }

        protected ContentResult CustJson<T>(T model)
        {
            return new JsonStringResult(Common.SerializeObject(model));
        }

        protected JsonResult JsonSerializer<T>(T model)
        {
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(model),
                MaxJsonLength = int.MaxValue
            };
        }

        public class CustomUTCDateTimeConverter : DateTimeConverterBase
        {
            public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
            {
                if (reader.Value == null)
                    return null;
                return DateTime.Parse(reader.Value.ToString());
            }

            public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
            {

                value = DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
                writer.WriteValue(((DateTime)value).ToLocalTime().ToString(ConfigSettings.AppDateTimeFormat));

            }

        }


    }



    #region Property Binder Attribute/ Json Model Binder

    public interface IPropertyBinder
    {
        object BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor);
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PropertyBinderAttribute : Attribute
    {
        public Type BinderType { get; private set; }

        public PropertyBinderAttribute(Type binderType)
        {
            BinderType = binderType;
        }

        public IPropertyBinder GetBinder()
        {
            return (IPropertyBinder)DependencyResolver.Current.GetService(BinderType);
        }
    }

    public class JsonModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor)
        {
            PropertyBinderAttribute attribute = propertyDescriptor.Attributes.OfType<PropertyBinderAttribute>().SingleOrDefault();
            if (attribute != null)
            {
                // var binder = CreateBinder(propertyBinderAttribute);
                //var value = binder.BindModel(controllerContext, bindingContext, propertyDescriptor);
                string fullPropertyKey = CreateSubPropertyName(bindingContext.ModelName, propertyDescriptor.Name);
                if (!bindingContext.ValueProvider.ContainsPrefix(fullPropertyKey))
                {
                    return;
                }
                object originalPropertyValue = propertyDescriptor.GetValue(bindingContext.Model);
                IModelBinder propertyBinder = Binders.GetBinder(propertyDescriptor.PropertyType);
                ModelMetadata propertyMetadata = bindingContext.PropertyMetadata[propertyDescriptor.Name];
                propertyMetadata.Model = originalPropertyValue;
                ModelBindingContext innerBindingContext = new ModelBindingContext()
                {
                    ModelMetadata = propertyMetadata,
                    ModelName = fullPropertyKey,
                    ModelState = bindingContext.ModelState,
                    ValueProvider = bindingContext.ValueProvider
                };


                object newPropertyValue = GetPropertyValue(controllerContext, innerBindingContext, propertyDescriptor, propertyBinder);

                #region Check and Update for DateTime Field. We can set any proprty type

                if (newPropertyValue != null)
                {
                    DateTime dateValue = (DateTime)newPropertyValue;
                    if (dateValue.Kind == DateTimeKind.Local)
                    {
                        // Change it
                        DateTime utcDate = dateValue.ToUniversalTime();
                        if (!propertyDescriptor.IsReadOnly && (propertyDescriptor.PropertyType == typeof(DateTime) || propertyDescriptor.PropertyType == typeof(DateTime?)))
                        {
                            //propertyDescriptor.SetValue(bindingContext.Model, utcDate);
                            SetProperty(controllerContext, bindingContext, propertyDescriptor, utcDate);
                        }
                        //   return;
                    }
                }

                #endregion
            }
            else // revert to the default behavior.
            {
                base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
            }
        }

        IPropertyBinder CreateBinder(PropertyBinderAttribute propertyBinderAttribute)
        {
            return (IPropertyBinder)DependencyResolver.Current.GetService(propertyBinderAttribute.BinderType);
        }

        //PropertyBinderAttribute TryFindPropertyBinderAttribute(PropertyDescriptor propertyDescriptor)
        //{
        //    return propertyDescriptor.Attributes
        //      .OfType<PropertyBinderAttribute>()
        //      .FirstOrDefault();
        //}
    }


    #endregion




}
