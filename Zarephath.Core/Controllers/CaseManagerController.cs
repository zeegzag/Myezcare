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
    public class CaseManagerController : BaseController
    {
        private ICaseManagerDataProvider _caseManagerDataProvider;

        #region Add Case Manager

        [CustomAuthorize(Permissions = Constants.Permission_CaseManager_AddUpdate)]
        public ActionResult AddCaseManager(string id, string id1, string id2)
        {
            long caseManagerID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            long agencyID = !string.IsNullOrEmpty(id1) ? Convert.ToInt64(Crypto.Decrypt(id1)) : 0;
            long agencyLocationID = !string.IsNullOrEmpty(id2) ? Convert.ToInt64(Crypto.Decrypt(id2)) : 0;
            _caseManagerDataProvider = new CaseManagerDataProvider();
            ServiceResponse response = _caseManagerDataProvider.SetAddCaseManagerPage(caseManagerID, agencyID, agencyLocationID);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        [CustomAuthorize(Permissions = Constants.Permission_CaseManager_AddUpdate)]
        public ActionResult PartialAddCaseManager(string id, string id1, string id2)
        {
            long caseManagerID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            long agencyID = !string.IsNullOrEmpty(id1) ? Convert.ToInt64(Crypto.Decrypt(id1)) : 0;
            long agencyLocationID = !string.IsNullOrEmpty(id2) ? Convert.ToInt64(Crypto.Decrypt(id2)) : 0;
            _caseManagerDataProvider = new CaseManagerDataProvider();
            ServiceResponse response = _caseManagerDataProvider.SetAddCaseManagerPage(caseManagerID, agencyID, agencyLocationID);
            ViewBag.IsPartialView = true;
            return View("AddCaseManager", response.Data);
            // return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_CaseManager_AddUpdate)]
        public JsonResult GetAgencyLocation(int agencyID, long agencyLocationID = 0)
        {
            _caseManagerDataProvider = new CaseManagerDataProvider();
            return Json(_caseManagerDataProvider.GetAgencyLocation(agencyID, agencyLocationID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_CaseManager_AddUpdate)]
        public JsonResult GetCaseManagers(int agencyID, long caseManagerID = 0)
        {
            _caseManagerDataProvider = new CaseManagerDataProvider();
            return Json(_caseManagerDataProvider.GetCaseManagers(agencyID, caseManagerID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_CaseManager_AddUpdate)]
        public JsonResult GetCaseManagerDetail(int caseManagerID)
        {
            _caseManagerDataProvider = new CaseManagerDataProvider();
            return Json(_caseManagerDataProvider.GetCaseManagerDetail(caseManagerID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_CaseManager_AddUpdate)]
        public JsonResult AddCaseManager(AddCaseManagerModel addCaseManagerModel)
        {
            _caseManagerDataProvider = new CaseManagerDataProvider();
            return Json(_caseManagerDataProvider.AddCaseManager(addCaseManagerModel, SessionHelper.LoggedInID));
        }

        #endregion

        #region Case Manager List
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_CaseManager_List)]
        public ActionResult CaseManagerList()
        {
            _caseManagerDataProvider = new CaseManagerDataProvider();
            ServiceResponse response = _caseManagerDataProvider.SetAddCaseManagerListPage();
            return View(response.Data);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_CaseManager_List)]
        public JsonResult GetCaseManagerList(SearchCaseManagerListPage searchCaseManagerListPage, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _caseManagerDataProvider = new CaseManagerDataProvider();
            return
                Json(_caseManagerDataProvider.GetCaseManagerList(searchCaseManagerListPage, pageIndex, pageSize, sortIndex,
                                                                 sortDirection));
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_CaseManager_AddUpdate)]
        public JsonResult DeleteCaseManager(SearchCaseManagerListPage searchCaseManagerListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _caseManagerDataProvider = new CaseManagerDataProvider();
            var response = _caseManagerDataProvider.DeleteCaseManager(searchCaseManagerListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }
        #endregion
    }
}
