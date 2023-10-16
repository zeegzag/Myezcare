using System;
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
    public class EBMarketsController : BaseController
    {
           private IEBMarketsDataProvider _ebmarketsDataProvider;
       // private IFormDataProvider _ebCategoryDataProvider;
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult EBMarketslist()
        {
            _ebmarketsDataProvider = new EBMarketsDataProvider(Constants.MyezcareOrganizationConnectionString);
            return View(_ebmarketsDataProvider.SetEBMarketsListPage().Data);
        }

        [HttpGet]
        // [CustomAuthorize(Permissions = Constants.HC_Permission_Physician_AddUpdate)]
        public ActionResult AddEBMarkets(string id)
        {

            string CategoryId = !string.IsNullOrEmpty(id) ? Crypto.Decrypt(id) : "0";
            _ebmarketsDataProvider = new EBMarketsDataProvider(Constants.MyezcareOrganizationConnectionString);
            ServiceResponse response = _ebmarketsDataProvider.AddEBMarkets(CategoryId, 0);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        [HttpPost]
        // [CustomAuthorize(Permissions = Constants.HC_Permission_EBCategory_AddUpdate)]
        public JsonResult AddEBMarkets(EBMarkets markets)
        {
            _ebmarketsDataProvider = new EBMarketsDataProvider(Constants.MyezcareOrganizationConnectionString);
             int ISiNSuP = 0;
            if (Convert.ToInt32(markets.ID) > 0)
                ISiNSuP = Convert.ToInt32(markets.ID);
            return Json(_ebmarketsDataProvider.AddEBMarkets(markets, ISiNSuP, SessionHelper.LoggedInID));
        }

        //[HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Physician_List)]
        //public ActionResult PhysicianList()
        //{
        //    _physicianDataProvider = new PhysicianDataProvider();
        //    return View(_physicianDataProvider.SetPhysicianListPage().Data);
        //}

        [HttpPost]
        //  [CustomAuthorize(Permissions = Constants.HC_Permission_EBCategory_List)]SearchEBMarketListPage
        public ContentResult EBMarketsList(SearchEBMarketsListPage SearchEBMarketListPage, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _ebmarketsDataProvider = new EBMarketsDataProvider(Constants.MyezcareOrganizationConnectionString);
            return CustJson(_ebmarketsDataProvider.GetEBMarketsList(SearchEBMarketListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpPost]
        // [CustomAuthorize(Permissions = Constants.HC_Permission_EBCategory_Delete)]
        public ContentResult DeleteEBMarkets(SearchEBMarketsListPage SearchEBMarketListPage, int pageSize = 10, int  pageIndex= 1, string sortIndex = "", string sortDirection = "")
        {
            _ebmarketsDataProvider = new EBMarketsDataProvider(Constants.MyezcareOrganizationConnectionString);
            return CustJson(_ebmarketsDataProvider.DeleteEBMarkets(SearchEBMarketListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }

        //[HttpPost]
        //public JsonResult GetPhysicianListForAutoComplete(string searchText, int pageSize)
        //{
        //    _physicianDataProvider = new PhysicianDataProvider();
        //    return Json(_physicianDataProvider.HC_GetPhysicianListForAutoComplete(searchText, pageSize));
        //}
    }
}
