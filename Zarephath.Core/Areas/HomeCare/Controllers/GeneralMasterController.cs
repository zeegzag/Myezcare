using System;
using System.Collections.Generic;
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
    public class GeneralMasterController : BaseController
    {
        private IGeneralMasterDataProvider _ddMasterDataProvider;

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_GeneralMaster_AddUpdate + "," + Constants.HC_Permission_GeneralMaster_List)]
        public ActionResult GeneralMasterDetail(string id)
        {
            int ddTypeId = Convert.ToInt32(id);
            _ddMasterDataProvider = new GeneralMasterDataProvider();
            ServiceResponse response = _ddMasterDataProvider.AddGeneralMaster(ddTypeId);
            return View("AddGeneralMaster", response.Data);
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_GeneralMaster_AddUpdate + "," + Constants.HC_Permission_GeneralMaster_List)]
        public ActionResult PartialGeneralMasterDetail(string id)
        {
            int ddTypeId = Convert.ToInt32(id);
            _ddMasterDataProvider = new GeneralMasterDataProvider();
            ServiceResponse response = _ddMasterDataProvider.AddGeneralMaster(ddTypeId);
            ViewBag.IsPartialView = true;
            return View("AddGeneralMaster", response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_GeneralMaster_AddUpdate)]
        public ContentResult SaveDDmaster(DDMaster ddMaster, List<long> childTaskIds)
        {
            _ddMasterDataProvider = new GeneralMasterDataProvider();
            return CustJson(_ddMasterDataProvider.SaveDDmaster(ddMaster, childTaskIds,SessionHelper.LoggedInID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_GeneralMaster_AddUpdate + "," + Constants.HC_Permission_GeneralMaster_List)]
        public ContentResult GetGeneralMasterList(SearchDDMasterListPage searchDDMasterListPage, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _ddMasterDataProvider = new GeneralMasterDataProvider();
            return CustJson(_ddMasterDataProvider.GetGeneralMasterList(searchDDMasterListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_GeneralMaster_Delete)]
        public ContentResult DeleteDDMaster(SearchDDMasterListPage searchDDMasterListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _ddMasterDataProvider = new GeneralMasterDataProvider();
            return CustJson(_ddMasterDataProvider.DeleteDDMaster(searchDDMasterListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_GeneralMaster_AddUpdate)]
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

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_GeneralMaster_AddUpdate)]
        public JsonResult CheckForParentChildMapping(DDMasterTypeModel model)
        {
            _ddMasterDataProvider = new GeneralMasterDataProvider();
            return JsonSerializer(_ddMasterDataProvider.CheckForParentChildMapping(model));
        }

        //[HttpGet]
        //public JsonResult GeneralMasterDetailAPI(string id)
        //{
        //    int ddTypeId = Convert.ToInt32(id);
        //    _ddMasterDataProvider  = new GeneralMasterDataProvider();
        //    GeneralMasterDataProvider GeneralMasterDataProvider = new GeneralMasterDataProvider();
        //    var result = GeneralMasterDataProvider.GetMasterDetailsApi(ddTypeId);
        //    return Json(result, JsonRequestBehavior.AllowGet);
         
           
        }

    }

