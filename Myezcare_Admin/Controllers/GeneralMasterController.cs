using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Myezcare_Admin.Helpers;
using Myezcare_Admin.Infrastructure;
using Myezcare_Admin.Infrastructure.DataProvider;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.Entity;
using Myezcare_Admin.Models.ViewModel;

namespace Myezcare_Admin.Controllers
{
    public class GeneralMasterController : BaseController
    {
        private IGeneralMasterDataProvider _ddMasterDataProvider;

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult GeneralMasterDetail(string id)
        {
            int ddTypeId = Convert.ToInt32(id);
            _ddMasterDataProvider = new GeneralMasterDataProvider();
            ServiceResponse response = _ddMasterDataProvider.AddGeneralMaster(ddTypeId);
            return View("AddGeneralMaster", response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ContentResult SaveDDmaster(DDMaster ddMaster)
        {
            _ddMasterDataProvider = new GeneralMasterDataProvider();
            return CustJson(_ddMasterDataProvider.SaveDDmaster(ddMaster, SessionHelper.LoggedInID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ContentResult GetGeneralMasterList(SearchDDMasterListPage searchDDMasterListPage, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _ddMasterDataProvider = new GeneralMasterDataProvider();
            return CustJson(_ddMasterDataProvider.GetGeneralMasterList(searchDDMasterListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ContentResult DeleteDDMaster(SearchDDMasterListPage searchDDMasterListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _ddMasterDataProvider = new GeneralMasterDataProvider();
            return CustJson(_ddMasterDataProvider.DeleteDDMaster(searchDDMasterListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public JsonResult GetParentChildMappingDDMaster(long DDMasterTypeID = 0, bool isFetchParentRecord = false,long parentID = 0)
        {
            _ddMasterDataProvider = new GeneralMasterDataProvider();
            return JsonSerializer(_ddMasterDataProvider.GetParentChildMappingDDMaster(DDMasterTypeID, isFetchParentRecord, parentID));
        }

        public JsonResult SaveParentChildMapping(long parentTaskId,List<long> childTaskIds)
        {
            _ddMasterDataProvider = new GeneralMasterDataProvider();
            return JsonSerializer(_ddMasterDataProvider.SaveParentChildMapping(parentTaskId, childTaskIds));
        }
    }
}
