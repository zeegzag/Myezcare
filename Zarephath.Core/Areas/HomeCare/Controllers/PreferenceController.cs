using System;
using System.Web.Mvc;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    //[CustomAuthorize(Permissions = Constants.Permission_Administrative_Permission)]
    public class PreferenceController : BaseController
    {
        private IPreferenceDataProvider _preferenceProvider;

        [CustomAuthorize(Permissions = Constants.HC_Permission_PreferenceSkill_AddUpdate)]
        public ActionResult AddPreference(string id)
        {
            long visitTaskId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _preferenceProvider = new PreferenceDataProvider();
            ServiceResponse response = _preferenceProvider.AddPreference(visitTaskId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        [CustomAuthorize(Permissions = Constants.HC_Permission_PreferenceSkill_AddUpdate)]
        public ActionResult PartialAddPreference(string id)
        {
            long visitTaskId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _preferenceProvider = new PreferenceDataProvider();
            ServiceResponse response = _preferenceProvider.AddPreference(visitTaskId);
            ViewBag.IsPartialView = true;
            return View("AddPreference", response.Data);
            // return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_PreferenceSkill_AddUpdate)]
        public JsonResult AddPreference(AddPreferenceModel addPreferenceModel)
        {
            _preferenceProvider = new PreferenceDataProvider();
            return Json(_preferenceProvider.AddPreference(addPreferenceModel, SessionHelper.LoggedInID));
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_PreferenceSkill_List)]
        public ActionResult PreferenceList()
        {
            _preferenceProvider = new PreferenceDataProvider();
            return View(_preferenceProvider.SetPreferenceListPage().Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_PreferenceSkill_List)]
        public ContentResult GetPreferenceList(SearchPreferenceListPage searchPreferenceListPage, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _preferenceProvider = new PreferenceDataProvider();
            return CustJson(_preferenceProvider.GetPreferenceList(searchPreferenceListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_PreferenceSkill_Delete)]
        public ContentResult DeletePreference(SearchPreferenceListPage searchPreferenceListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _preferenceProvider = new PreferenceDataProvider();
            return CustJson(_preferenceProvider.DeletePreference(searchPreferenceListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }
    }
}
