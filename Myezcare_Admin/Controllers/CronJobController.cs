using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Myezcare_Admin.Helpers;
using Myezcare_Admin.Infrastructure;
using Myezcare_Admin.Infrastructure.DataProvider;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.ViewModel;

namespace Myezcare_Admin.Controllers
{
    public class CronJobController : BaseController
    {
        private ICronJobDataProvider _cronJobDataProvider;

        public ActionResult UpdateEbriggsFormDetails()
        {
            _cronJobDataProvider=new CronJobDataProvider();
            ServiceResponse response = _cronJobDataProvider.UpdateEbriggsFormDetails();
            return View("CronJobMessage", response);
        }
    }
}
