using System.Web.Mvc;
using System.Web.Routing;

namespace Zarephath.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //for Report WebForm Routing
            routes.MapPageRoute("Report", "Report/Template", "~/Areas/HomeCare/Views/Report/Partial/ReportTemplateWebForm.aspx");


            routes.MapRoute(
                name: "ScheduleEmail",
                url: "m/{id}",
                defaults: new { controller = "Schedule", action = "ScheduleEmailDetailHtml", id = UrlParameter.Optional },
                namespaces:new[] { "Zarephath.Core.Controllers" }
            );

            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Security", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Zarephath.Core.Controllers" }
            );

            routes.MapRoute(
                name: "DefaultWithParameters",
                url: "{controller}/{action}/{id}/{id1}/{id2}",
                defaults: new { controller = "Security", action = "Index", id = UrlParameter.Optional, id1 = UrlParameter.Optional, id2 = UrlParameter.Optional },
                namespaces: new[] { "Zarephath.Core.Controllers" }
            );

            routes.LowercaseUrls = true;
            
        }
    }
}