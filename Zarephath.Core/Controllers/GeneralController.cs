using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Controllers
{
    public class GeneralController:Controller
    {
        #region Errros

        public ActionResult DomainNotFound()
        {
            return View("DomainNotFound");
        }


        public ActionResult NotFound()
        {
            return View("NotFound");
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousPermission)]
        public ActionResult AccessDenied()
        {
            return View("accessdenied");
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousPermission)]
        public ActionResult InternalError()
        {
            return View("InternalError");
        }


        #endregion
    }
}
