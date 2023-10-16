using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Myezcare_Admin.Helpers;
using Myezcare_Admin.Infrastructure;
using Myezcare_Admin.Infrastructure.DataProvider;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.Entity;
using Myezcare_Admin.Models.ViewModel;

namespace Myezcare_Admin.Controllers
{
    public class ReleaseNoteController : BaseController
    {
        private IReleaseNoteDataProvider _releaseNoteDataProvider;

        public ReleaseNoteController()
        {
            _releaseNoteDataProvider = new ReleaseNoteDataProvider();            
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult AddReleaseNote(string id)
        {
            long ReleaseNoteID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _releaseNoteDataProvider.SetAddReleaseNotePage(ReleaseNoteID);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        public JsonResult SaveReleaseNote(ReleaseNote releaseNote)
        {
            return Json(_releaseNoteDataProvider.SaveReleaseNote(releaseNote,SessionHelper.LoggedInID));
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult ReleaseNoteList()
        {
            return View(_releaseNoteDataProvider.SetReleaseNoteListPage().Data);
        }

        [HttpPost]
        public ContentResult GetReleaseNoteList(SearchReleaseNoteListPage searchReleaseNoteListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            var response = _releaseNoteDataProvider.GetReleaseNoteList(searchReleaseNoteListPage, pageIndex, pageSize, sortIndex, sortDirection);
            return CustJson(response);
        }

        [HttpPost]
        public JsonResult DeleteReleaseNote(SearchReleaseNoteListPage searchReleaseNoteListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            var response = _releaseNoteDataProvider.DeleteReleaseNote(searchReleaseNoteListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult ViewReleaseNote(string id)
        {
            long ReleaseNoteID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _releaseNoteDataProvider.ViewReleaseNote(ReleaseNoteID);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
    }
}
