using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Myezcare_Admin.Helpers;
using Myezcare_Admin.Infrastructure;
using Myezcare_Admin.Infrastructure.DataProvider;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.ViewModel;
using System.IO;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Myezcare_Admin.Controllers
{
    public class FormController : BaseController
    {
        private IFormDataProvider _formDataProvider;


        #region Form List

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult FormList()
        {
            _formDataProvider = new FormDataProvider();
            var response = _formDataProvider.SetFormListPage().Data;
            return View(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ContentResult GetFormList(SearchFormModel model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _formDataProvider = new FormDataProvider();
            var response = _formDataProvider.GetFormList(model, pageIndex, pageSize, sortIndex, sortDirection);
            return CustJson(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public JsonResult DeleteForm(SearchFormModel model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _formDataProvider = new FormDataProvider();
            var response = _formDataProvider.DeleteForm(model, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public JsonResult UpdateFormPrice(FormListModel model)
        {
            _formDataProvider = new FormDataProvider();
            var response = _formDataProvider.UpdateFormPrice(model, SessionHelper.LoggedInID);
            return Json(response);
        }


        #endregion

        [HttpPost]
        public JsonResult UpdateEbriggsFormDetails()
        {
            _formDataProvider = new FormDataProvider();
            var response = _formDataProvider.UpdateEbriggsFormDetails();
            return Json(response);
        }






        #region Load Internal HTML Forms

        [HttpGet]
        public ActionResult LoadHtmlForm()
        {
            string path = Request.QueryString["formURL"];

            if (!System.IO.File.Exists(Server.MapPath(path)))
            {
                return View("Error");
            }

            string data = System.IO.File.ReadAllText(Server.MapPath(path));
            //string data = System.IO.File.ReadAllText(Server.MapPath("~/Assets/include/InternalForms/COST_SHARE_AGREEMENT_FORM/index.html"));
            NameValueDataInString obj = new NameValueDataInString();
            obj.Value = data;
            return View(obj);
        }

        #endregion
    }
}
