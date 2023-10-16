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
    public class ServiceCodeController : BaseController
    {
        private IServiceCodeDataProvider _iServiceCodeDataProvider;

        #region ADD ServiceCode

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Payor_AddUpdate)]
        public ActionResult AddServiceCode(string id)
        {
            _iServiceCodeDataProvider =new ServiceCoceDataProvider();
            long serviceCodeId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _iServiceCodeDataProvider.SetServiceCode(serviceCodeId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Payor_AddUpdate)]
        public JsonResult AddServiceCode(AddServiceCodeModel addServiceCodeModel)
        {
            _iServiceCodeDataProvider = new ServiceCoceDataProvider();
            ServiceResponse response = _iServiceCodeDataProvider.AddServiceCode(addServiceCodeModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion

        #region List Service Code Detail

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Payor_List)]
        public ActionResult ServiceCodeList()
        {
            _iServiceCodeDataProvider = new ServiceCoceDataProvider();
            var response = _iServiceCodeDataProvider.SetServiceCodeList();
            return View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Payor_List)]
        public JsonResult GetServiceCodeList(SearchServiceCodeListPage searchServiceCodeListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iServiceCodeDataProvider = new ServiceCoceDataProvider();
            var response = _iServiceCodeDataProvider.GetServiceCodeList(searchServiceCodeListPage, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

       
        #endregion
    }
}
