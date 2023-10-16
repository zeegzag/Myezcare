using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Zarephath.Core.Controllers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class MyezConnectController : BaseController
    {
        private IMyezConnectDataProvider _releaseNoteDataProvider;

        public MyezConnectController()
        {
            _releaseNoteDataProvider = new MyezConnectDataProvider();            
        }

        [HttpGet]
        public ActionResult ReleaseNote(string id)
        {
            long ReleaseNoteID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _releaseNoteDataProvider.ViewReleaseNote(ReleaseNoteID);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        public JsonResult UpdateSiteCache(string id)
        {
            ServiceResponse response = _releaseNoteDataProvider.UpdateSiteCache(id);
            return Json(response);
        }

    }
}
