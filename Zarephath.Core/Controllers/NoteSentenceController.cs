using System;
using System.Web.Mvc;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Controllers
{
    public class NoteSentenceController : BaseController
    {
        private INoteSentenceDataProvider _noteSentenceDataProvider;

        #region Add Note Sentence

        [CustomAuthorize(Permissions = Constants.Permission_NoteSentence_AddUpdate)]
        public ActionResult AddNoteSentence(string id)
        {
            long noteSentenceId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _noteSentenceDataProvider = new NoteSentenceDataProvider();
            ServiceResponse response = _noteSentenceDataProvider.SetAddNoteSentencePage(noteSentenceId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        [CustomAuthorize(Permissions = Constants.Permission_NoteSentence_AddUpdate)]
        public ActionResult PartialAddNoteSentence(string id)
        {
            long noteSentenceId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _noteSentenceDataProvider = new NoteSentenceDataProvider();
            ServiceResponse response = _noteSentenceDataProvider.SetAddNoteSentencePage(noteSentenceId);
            ViewBag.IsPartialView = true;
            return View("AddNoteSentence", response.Data);
            // return ShowUserFriendlyPages(response) ?? View(response.Data);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_NoteSentence_AddUpdate)]
        public JsonResult AddNoteSentence(NoteSentence noteSentence)
        {
            _noteSentenceDataProvider = new NoteSentenceDataProvider();
            return Json(_noteSentenceDataProvider.AddNoteSentence(noteSentence, SessionHelper.LoggedInID));
        }

        #endregion Add Note Sentence

        #region Note Sentence List

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_NoteSentence_List)] 
        public ActionResult NoteSentenceList()
        {
            _noteSentenceDataProvider = new NoteSentenceDataProvider();
            return View(_noteSentenceDataProvider.SetAddNoteSentenceListPage().Data);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_NoteSentence_List + "," + Constants.HC_Permission_PatientIntake_Notes_View)]
        public ContentResult GetNoteSentenceList(SearchNoteSentenceListPage searchNoteSentenceListPage, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _noteSentenceDataProvider = new NoteSentenceDataProvider();
            return CustJson(_noteSentenceDataProvider.GetNoteSentenceList(searchNoteSentenceListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_NoteSentence_AddUpdate)]
        public ContentResult DeleteNoteSentence(SearchNoteSentenceListPage searchNoteSentenceListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _noteSentenceDataProvider = new NoteSentenceDataProvider();
            return CustJson(_noteSentenceDataProvider.DeleteNoteSentence(searchNoteSentenceListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }

        #endregion Note Sentence List
    }
}
