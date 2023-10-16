using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using HomeCareApi.Controllers;
using HomeCareApi.Infrastructure.Attributes;

namespace HomeCareApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}/{id1}",
                defaults: new { controller = "base", action = "ping", id = RouteParameter.Optional, id1 = RouteParameter.Optional }
            );

            //  Setting response type JSON for each and every api calls.
            MediaTypeHeaderValue appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            // Error and Exception Handling.
            config.Filters.Add(new HandleApiExceptionAttribute());

            //Authentication, Authorization & Common Error Handing.
            config.Filters.Add(new ApiAuthorizationFilter());

            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new CustomDateConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new CustomDateTimeConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new CustomUTCDateTimeConverter());
        }
    }
}
