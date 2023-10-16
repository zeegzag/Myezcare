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
using Zarephath.Core.Controllers;
using Zarephath.Core.Models.Entity;
using System.Net.Http;
using Newtonsoft.Json;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class AgencyController : BaseController
    {
        private IAgencyDataProvider _iagencyDataProvider;
        private IFacilityHouseDataProvider _iFacilityHouseDataProvider;

        #region Add Case Manager

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Agency_AddUpdate)]
        public ActionResult AddAgency(string id)
        {
            _iagencyDataProvider = new AgencyDataProvider();
            long agencyidnew = Convert.ToInt64(Crypto.Decrypt(id));
            _iFacilityHouseDataProvider = new FacilityHouseDataProvider();
            HC_SetFacilityHouseListModel facilityModel = (HC_SetFacilityHouseListModel) _iFacilityHouseDataProvider.HC_SetFacilityHouseListPage().Data;
            facilityModel.SearchFacilityHouseModel.AgencyID = agencyidnew;
            ViewBag.HC_SetFacilityHouseListModel = facilityModel;
            ServiceResponse response = _iagencyDataProvider.HC_SetAddAgencyPage(agencyidnew);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Agency_AddUpdate)]
        public ActionResult PartialAddAgency(string id)
        {
            _iagencyDataProvider = new AgencyDataProvider();
            long agencyidnew = Convert.ToInt64(Crypto.Decrypt(id));
            _iFacilityHouseDataProvider = new FacilityHouseDataProvider();
            HC_SetFacilityHouseListModel facilityModel = (HC_SetFacilityHouseListModel)_iFacilityHouseDataProvider.HC_SetFacilityHouseListPage().Data;
            facilityModel.SearchFacilityHouseModel.AgencyID = agencyidnew;
            ViewBag.HC_SetFacilityHouseListModel = facilityModel;
            ServiceResponse response = _iagencyDataProvider.HC_SetAddAgencyPage(agencyidnew);
            ViewBag.IsPartialView = true;
            return View("AddAgency", response.Data);
           // return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]

        [CustomAuthorize(Permissions = Constants.HC_Permission_Agency_AddUpdate)]
        public JsonResult AddAgency(AddAgencyModel addAgencyModel)
        {
            _iagencyDataProvider = new AgencyDataProvider();            
            return Json(_iagencyDataProvider.HC_AddAgency(addAgencyModel, SessionHelper.LoggedInID));
        }

        #endregion

        #region Agency List

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Agency_List)]
        public ActionResult AgencyList()
        {
            _iagencyDataProvider = new AgencyDataProvider();
            ServiceResponse response = _iagencyDataProvider.HC_SetAddAgencyListPage();
            return View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Agency_List)]
        public JsonResult GetAgencyList(SearchAgencyListPage searchAgencyListPage, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iagencyDataProvider = new AgencyDataProvider();
            return Json(_iagencyDataProvider.HC_GetAgencyList(searchAgencyListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Agency_Delete)]
        public JsonResult DeleteAgencyList(SearchAgencyListPage searchAgencyListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iagencyDataProvider = new AgencyDataProvider();
            var response = _iagencyDataProvider.HC_DeleteAgency(searchAgencyListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion


        [HttpPost]
        public JsonResult GetZipCodeList(string searchText, string state, int pageSize)
        {
            _iagencyDataProvider = new AgencyDataProvider();
            return Json(_iagencyDataProvider.GetZipCodeList(searchText, state, pageSize));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Agency_List)]
        public async Task<JsonResult> GetNPIData(string number)
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri("https://npiregistry.cms.hhs.gov/api/?version=2.1&number=" + number);
                var response = client.GetAsync(uri);
                string result = await response.Result.Content.ReadAsStringAsync();

                //We can Deserialize Object and bind with Agency model directly here
                //Agency resModel = JsonConvert.DeserializeObject<Agency>(result);

                return Json(new ServiceResponse()
                {
                    Data = result,
                    IsSuccess = true
                });

            }
        }
    }
}
