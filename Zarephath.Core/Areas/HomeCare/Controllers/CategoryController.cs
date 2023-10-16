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
    public class CategoryController : BaseController
    {
           private IEBCategoryDataProvider _ebCategoryDataProvider;
       // private IFormDataProvider _ebCategoryDataProvider;
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult CategoryList()
        {
            _ebCategoryDataProvider = new EBCategoryDataProvider(Constants.MyezcareOrganizationConnectionString);
            return View(_ebCategoryDataProvider.SetEBCategoryListPage().Data);
        }

        [HttpGet]
        // [CustomAuthorize(Permissions = Constants.HC_Permission_Physician_AddUpdate)]
        public ActionResult AddCategory(string id)
        {

            string CategoryId = !string.IsNullOrEmpty(id) ? Crypto.Decrypt(id) : "0";
            _ebCategoryDataProvider = new EBCategoryDataProvider(Constants.MyezcareOrganizationConnectionString);
            ServiceResponse response = _ebCategoryDataProvider.AddEBCategory(CategoryId, 0);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        [HttpPost]
        // [CustomAuthorize(Permissions = Constants.HC_Permission_EBCategory_AddUpdate)]
        public JsonResult AddCategory(EBCategory category)
        {
            _ebCategoryDataProvider = new EBCategoryDataProvider(Constants.MyezcareOrganizationConnectionString);
             int ISiNSuP = 0;
            if (Convert.ToInt32(category.ID) > 0)
                ISiNSuP = Convert.ToInt32(category.ID);
            return Json(_ebCategoryDataProvider.AddEBCategory(category, ISiNSuP, SessionHelper.LoggedInID));
        }

        //[HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Physician_List)]
        //public ActionResult PhysicianList()
        //{
        //    _physicianDataProvider = new PhysicianDataProvider();
        //    return View(_physicianDataProvider.SetPhysicianListPage().Data);
        //}

        [HttpPost]
        //  [CustomAuthorize(Permissions = Constants.HC_Permission_EBCategory_List)]
        public ContentResult CategoryList(SearchEBCategoryListPage searchEBCategoryListPage, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _ebCategoryDataProvider = new EBCategoryDataProvider(Constants.MyezcareOrganizationConnectionString);
            return CustJson(_ebCategoryDataProvider.GetCategoryList(searchEBCategoryListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpPost]
        // [CustomAuthorize(Permissions = Constants.HC_Permission_EBCategory_Delete)]
        public ContentResult DeleteCategory(SearchEBCategoryListPage searchEBCategoryListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _ebCategoryDataProvider = new EBCategoryDataProvider(Constants.MyezcareOrganizationConnectionString);
            return CustJson(_ebCategoryDataProvider.DeleteCategory(searchEBCategoryListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }

        //[HttpPost]
        //public JsonResult GetPhysicianListForAutoComplete(string searchText, int pageSize)
        //{
        //    _physicianDataProvider = new PhysicianDataProvider();
        //    return Json(_physicianDataProvider.HC_GetPhysicianListForAutoComplete(searchText, pageSize));
        //}
    }
}
