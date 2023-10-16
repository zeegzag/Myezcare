using System.Web.Mvc;

namespace Zarephath.Web.Areas.Staffing
{
    public class StaffingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Staffing";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Staffing_default",
                "st/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional, id1 = UrlParameter.Optional },
                new[] { "Zarephath.Core.Areas.Staffing.Controllers" }
            );


        }
    }
}
