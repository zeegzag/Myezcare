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
    public class AgencyController : BaseController
    {
        private IAgencyDataProvider _iagencyDataProvider;

        #region Add Case Manager

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Agency_AddUpdate)]
        public ActionResult AddAgency(string id)
        {
            _iagencyDataProvider = new AgencyDataProvider();
            long agencyidnew = Convert.ToInt64(Crypto.Decrypt(id));
            ServiceResponse response = _iagencyDataProvider.SetAddAgencyPage(agencyidnew);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]

        [CustomAuthorize(Permissions = Constants.Permission_Agency_AddUpdate)]
        public JsonResult AddAgency(AddAgencyModel addAgencyModel)
        {
            _iagencyDataProvider = new AgencyDataProvider();
            return Json(_iagencyDataProvider.AddAgency(addAgencyModel, SessionHelper.LoggedInID));
        }

        #endregion

        #region Agency List

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Agency_Listing)]
        public ActionResult AgencyList()
        {
            _iagencyDataProvider = new AgencyDataProvider();
            ServiceResponse response = _iagencyDataProvider.SetAddAgencyListPage();
            return View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Agency_Listing)]
        public JsonResult GetAgencyList(SearchAgencyListPage searchAgencyListPage, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iagencyDataProvider = new AgencyDataProvider();
            return Json(_iagencyDataProvider.GetAgencyList(searchAgencyListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Agency_AddUpdate)]
        public JsonResult DeleteAgencyList(SearchAgencyListPage searchAgencyListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iagencyDataProvider = new AgencyDataProvider();
            var response = _iagencyDataProvider.DeleteAgency(searchAgencyListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion


        [HttpPost]
        public JsonResult GetZipCodeList(string searchText, string state, int pageSize)
        {
            _iagencyDataProvider = new AgencyDataProvider();
            return Json(_iagencyDataProvider.GetZipCodeList(searchText, state, pageSize));
        }
    }
}
