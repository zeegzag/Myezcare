using System.Web.Mvc;

namespace Zarephath.Web.Areas.HomeCare
{
    public class HomeCareAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "HomeCare";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "RegistrationEmail",
                url: "k/{id}",
                defaults: new { controller = "security", action = "setpassword", id = UrlParameter.Optional },
                namespaces: new[] { "Zarephath.Core.Controllers" }
            );

#if DEBUG
            context.MapRoute(
                "HomeCare_Dev_default",
                "HomeCare/{controller}/{action}/{id}/{id1}",
                new { action = "Index", id = UrlParameter.Optional, id1 = UrlParameter.Optional },
                new[] { "Zarephath.Core.Areas.HomeCare.Controllers" }
            );
#endif

            context.MapRoute(
                "HomeCare_default",
                "hc/{controller}/{action}/{id}/{id1}",
                new { action = "Index", id = UrlParameter.Optional,id1 = UrlParameter.Optional },
                new[] { "Zarephath.Core.Areas.HomeCare.Controllers" }
            );
        }
    }
}
