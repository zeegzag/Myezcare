using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class NotificationConfigurationController : BaseController
    {
        private INotificationConfigurationDataProvider _dataProvider;

        public NotificationConfigurationController()
        {
            _dataProvider = new NotificationConfigurationDataProvider();
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Notification_Configuration)]
        public ActionResult Index()
        {
            // Additional data for select list
            ViewBag.Roles = _dataProvider.GetRoles();
            return View(new NotificationConfigurationModel());
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Notification_Configuration)]
        public JsonResult GetNotificationConfigurationDetails(NotificationConfigurationModel notificationConfiguration)
        {
            var response = _dataProvider.GetNotificationConfigurationDetails(notificationConfiguration);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Notification_Configuration_Update)]
        public JsonResult SaveNotificationConfigurationDetails(NotificationConfigurationModel notificationConfiguration)
        {
            var response = _dataProvider.SaveNotificationConfigurationDetails(notificationConfiguration, SessionHelper.LoggedInID);
            return Json(response);
        }
    }
}
