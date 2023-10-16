using System;
using System.Web.Mvc;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Controllers
{
    public class DxCodeController : BaseController
    {
        private IDxCodeDataProvider _dxCodeDataProvider;

        #region Add DX Code

        [CustomAuthorize(Permissions = Constants.Permission_DxCode_AddUpdate)]
        public ActionResult AddDxCode(string id)
        {
            long dxCodeId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _dxCodeDataProvider = new DxCodeDataProvider();
            ServiceResponse response = _dxCodeDataProvider.SetAddDxCodePage(dxCodeId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_DxCode_AddUpdate)]
        public JsonResult AddDxCode(AddDxCodeModel addDxCodeModel)
        {
            _dxCodeDataProvider = new DxCodeDataProvider();
            return Json(_dxCodeDataProvider.AddDxCode(addDxCodeModel, SessionHelper.LoggedInID));
        }

        #endregion Add DX Code

        #region DX Code List

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_DxCode_List)] // TODO :SET Permission
        public ActionResult DxCodeList()
        {
            _dxCodeDataProvider = new DxCodeDataProvider();
            return View(_dxCodeDataProvider.SetAddDxCodeListPage().Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_DxCode_List)]
        public ContentResult GetDxCodeList(SearchDxCodeListPage searchDxCodeListPage, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _dxCodeDataProvider = new DxCodeDataProvider();
            return CustJson(_dxCodeDataProvider.GetDxCodeList(searchDxCodeListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_DxCode_AddUpdate)]
        public ContentResult DeleteDxCode(SearchDxCodeListPage searchDxCodeListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _dxCodeDataProvider = new DxCodeDataProvider();
            return CustJson(_dxCodeDataProvider.DeleteDxCode(searchDxCodeListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }

        #endregion DX Code List
    }
}
