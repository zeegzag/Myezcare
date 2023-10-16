using System;
using System.Web.Mvc;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;


namespace Zarephath.Core.Controllers
{

    public class TransportLocationController : BaseController
    {
        ITransportaionLocation _TransportaionLocationDataProvider;

        #region Add Transportation Location

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Transportation_AddUpdate)]
        public ActionResult AddTransportLocation(string id)
        {
            long locationId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _TransportaionLocationDataProvider = new TransportaionLocationDataProvider();
            ServiceResponse response = _TransportaionLocationDataProvider.SetAddTransportaionLocationPage(locationId,SessionHelper.LoggedInID);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Transportation_AddUpdate)]
        public JsonResult AddTransportLocation(AddTransportationLocationModel transportlocation)
        {
            _TransportaionLocationDataProvider = new TransportaionLocationDataProvider();
            var response = _TransportaionLocationDataProvider.AddTransportaionLocation(transportlocation, SessionHelper.LoggedInID);
            return Json(response);
        }

        //[HttpPost]
        //public JsonResult UploadFile(string description)
        //{
        //    string Message, fileName, actualFileName = string.Empty;

        //    bool flag = false;
        //    if (Request.Files != null)
        //    {
        //        var file = Request.Files[0];
        //        //  actualFileName = file.FileName;
        //        fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        //        int size = file.ContentLength;

        //        try
        //        {
        //            file.SaveAs(Path.Combine(Server.MapCustomPath("~/UploadedFiles"), fileName));
        //        }
        //        catch (Exception)
        //        {
        //            Message = "File upload failed! Please try again";
        //        }
        //    }
        //    return new JsonResult { Data = new { Status = flag } };
        //}

        #endregion

        #region List Transportation Location

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Transportation_List)]
        public ActionResult TransportLocationList()
        {
            _TransportaionLocationDataProvider = new TransportaionLocationDataProvider();
            var response = _TransportaionLocationDataProvider.SetTransportaionLocationListPage();
            return View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Transportation_List)]
        public JsonResult GetTransportatLocationList(SearchTransPortationListPage searchTransportLocationModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _TransportaionLocationDataProvider = new TransportaionLocationDataProvider();
            var response = _TransportaionLocationDataProvider.GetTransportaionLocationList(searchTransportLocationModel, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Transportation_Delete)]
        public JsonResult DeleteTransportatLocationList(SearchTransPortationListPage searchTransportLocationModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _TransportaionLocationDataProvider = new TransportaionLocationDataProvider();
            var response = _TransportaionLocationDataProvider.DeleteTransportaionLocation(searchTransportLocationModel, pageIndex, pageSize, sortIndex, sortDirection,SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Transportation_AddUpdate + "," + Constants.Permission_Transportation_List)]
        public JsonResult GetZipCodeList(string searchText, string state, int pageSize)
        {
            _TransportaionLocationDataProvider = new TransportaionLocationDataProvider();
            return Json(_TransportaionLocationDataProvider.GetZipCodeList(searchText, state, pageSize));
        }
    }
}
