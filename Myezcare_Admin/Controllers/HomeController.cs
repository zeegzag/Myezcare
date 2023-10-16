using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Myezcare_Admin.Helpers;
using Myezcare_Admin.Infrastructure;
using Myezcare_Admin.Infrastructure.DataProvider;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.ViewModel;

namespace Myezcare_Admin.Controllers
{
    [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
    public class HomeController : BaseController
    {
        private IHomeDataProvider _iHomeDataProvider;
        public HomeController()
        {
            _iHomeDataProvider = new HomeDataProvider();
        }
        [HttpGet]
        public ActionResult Dashboard()
        {
            //ServiceResponse response = _iHomeDataProvider.SetDashboardPage(SessionHelper.LoggedInID);
            //return View(response.Data);
            return View();
        }
    }
}
