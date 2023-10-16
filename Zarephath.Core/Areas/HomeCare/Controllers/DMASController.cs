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
using Zarephath.Core.Infrastructure.Utility;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class DMASController : BaseController
    {
        private IDMASDataProvider _iDMASDataProvider;
        CacheHelper _cacheHelper = new CacheHelper();

        #region DMAS 97

        [HttpPost]
        public JsonResult AddDMAS_97_AB(Dmas97Model dmas)
        {
            long Dmas97ID = Convert.ToInt64(dmas.Dmas97ID);
            _iDMASDataProvider = new DMASDataProvider();
            ServiceResponse response = _iDMASDataProvider.GetDmas97Ab(dmas);
            return Json(response,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DMAS_97_AB(DmasModel dmas)
        {
            _iDMASDataProvider = new DMASDataProvider();
            var response = _iDMASDataProvider.AddUpdateDmas97Ab(dmas, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        public JsonResult DMAS97ABList1(string id)
        {
            long ReferralID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _iDMASDataProvider = new DMASDataProvider();
            var response = _iDMASDataProvider.Dmas97AbList(ReferralID).Data;
            return Json(response,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDMAS97AB(Dmas97AbModel dmas)
        {
            _iDMASDataProvider = new DMASDataProvider();
            ServiceResponse response = _iDMASDataProvider.DeleteDmas97Ab(dmas);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerateDmas97AB(string id)
        {
            long Dmas97ID = Convert.ToInt64(id);
            _iDMASDataProvider = new DMASDataProvider();
            ServiceResponse response = _iDMASDataProvider.GenerateDmas97AB(Dmas97ID);
            return ShowUserFriendlyPages(response) ?? View((DmasModel)response.Data);
        }

        [HttpGet]
        public ActionResult GenerateDmas97Pdf(string id)
        {
            string url = string.Format("{0}{1}{2}", _cacheHelper.SiteBaseURL, Constants.GenerateDMAS_97_AB, id);
            SelectHtmlToPdf data = new SelectHtmlToPdf();
            byte[] pdf = data.GenerateHtmlUrlToPdf(url);
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = String.Format("{0}_{1}.pdf", "Dmas97AB Form", DateTime.Now.ToString(Constants.ReadableFileNameDateTimeFormat));
            return fileResult;

        }

        [HttpPost]
        public ActionResult CloneDataDMAS97AB(Dmas97CloneModel dmas)
        {
            _iDMASDataProvider = new DMASDataProvider();
            var response = _iDMASDataProvider.CloneDataDMAS97AB(dmas, SessionHelper.LoggedInID);
            return Json(response);
        }
        #endregion


        #region DMAS 99

        [HttpPost]
        public JsonResult AddDMAS_99(Dmas99Models dmas)
        {
            long Dmas99ID = Convert.ToInt64(dmas.Dmas99ID);
            _iDMASDataProvider = new DMASDataProvider();
            ServiceResponse response = _iDMASDataProvider.GetDmas99Form(dmas);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DMAS_99(GetDmas99Model dmas)
        {
            _iDMASDataProvider = new DMASDataProvider();
            var response = _iDMASDataProvider.AddUpdateDmas99(dmas, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        public JsonResult DMAS99List1(string id)
        {
            long ReferralID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _iDMASDataProvider = new DMASDataProvider();
            var response = _iDMASDataProvider.Dmas99ListPage(ReferralID).Data;
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDmas99(Dmas99Model dmas)
        {
            _iDMASDataProvider = new DMASDataProvider();
            ServiceResponse response = _iDMASDataProvider.DeleteDmas99Form(dmas);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerateDMAS99Form(string id)
        {
            long Dmas99ID = Convert.ToInt64(id);
            _iDMASDataProvider = new DMASDataProvider();
            ServiceResponse response = _iDMASDataProvider.GenerateDMAS99Form(Dmas99ID);
            return ShowUserFriendlyPages(response) ?? View((GetDmas99Model)response.Data);
        }

        [HttpGet]
        public ActionResult GenerateDmas99Pdf(string id)
        {
            string url = string.Format("{0}{1}{2}", _cacheHelper.SiteBaseURL, Constants.GenerateDMAS99Form, id);
            SelectHtmlToPdf data = new SelectHtmlToPdf();
            byte[] pdf = data.GenerateHtmlUrlToPdf(url);
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = String.Format("{0}_{1}.pdf", "Dmas99 Form", DateTime.Now.ToString(Constants.ReadableFileNameDateTimeFormat));
            return fileResult;

        }

        [HttpPost]
        public ActionResult CloneDataDMAS99(Dmas99CloneModel dmas)
        {
            _iDMASDataProvider = new DMASDataProvider();
            var response = _iDMASDataProvider.CloneDataDMAS99(dmas, SessionHelper.LoggedInID);
            return Json(response);
        }
        #endregion


        #region CMS 485
        [HttpPost]
        public JsonResult AddCms485Form(Cms485AddModel cms)
        {
            long Cms485ID = Convert.ToInt64(cms.Cms485ID);
            _iDMASDataProvider = new DMASDataProvider();
            ServiceResponse response = _iDMASDataProvider.GetCms485Form(cms);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveCms485Form(GetCms485Model cms)
        {
            _iDMASDataProvider = new DMASDataProvider();
            var response = _iDMASDataProvider.AddUpdateCms485Form(cms, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        public JsonResult Cms485List(string id)
        {
            long ReferralID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _iDMASDataProvider = new DMASDataProvider();
            var response = _iDMASDataProvider.Cms485FormList(ReferralID).Data;
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteCms485Form(Cms485Model cms)
        {
            _iDMASDataProvider = new DMASDataProvider();
            ServiceResponse response = _iDMASDataProvider.DeleteCms485Form(cms);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CloneCms485Form(Cms485CloneModel cms)
        {
            _iDMASDataProvider = new DMASDataProvider();
            var response = _iDMASDataProvider.CloneCms485Form(cms, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpGet]
        public ActionResult GenerateCms485Pdf(string id)
        {
            string url = string.Format("{0}{1}{2}", _cacheHelper.SiteBaseURL, Constants.GenerateCms485Form, id);
            SelectHtmlToPdf data = new SelectHtmlToPdf();
            byte[] pdf = data.GenerateHtmlUrlToPdf(url);
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = String.Format("{0}_{1}.pdf", "cms485Form", DateTime.Now.ToString(Constants.ReadableFileNameDateTimeFormat));
            return fileResult;

        }

        public ActionResult GenerateCms485Form(string id)
        {
            long Cms485ID = Convert.ToInt64(id);
            _iDMASDataProvider = new DMASDataProvider();
            ServiceResponse response = _iDMASDataProvider.GenerateCms485Form(Cms485ID);
            return ShowUserFriendlyPages(response) ?? View((GetCms485Model)response.Data);
        }
        #endregion
    }
}
