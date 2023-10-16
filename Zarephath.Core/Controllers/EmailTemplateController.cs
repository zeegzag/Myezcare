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

namespace Zarephath.Core.Controllers
{
    public class EmailTemplateController : BaseController
    {
        IEmailTemplateDataProvider _IEmailTemplateDataProvider;

        #region Add Email Template

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_EmailTemplate_AddUpdate)]
        public ActionResult AddEmailTemplate(string id)
        {
            long emailtemplateId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _IEmailTemplateDataProvider = new EmailTemplateDataProvider();
            ServiceResponse response = _IEmailTemplateDataProvider.SetAddEmailTemplatePage(emailtemplateId);
            ViewBag.EmailTypeList = _IEmailTemplateDataProvider.GetEmailType();
            ViewBag.ModuleList = _IEmailTemplateDataProvider.GetModuleNames();
            if (emailtemplateId > 0)
            {
                TempData["IsEditMode"] = true;

            }
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_EmailTemplate_AddUpdate)]
        public JsonResult AddEmailTemplate(AddEmailTemplateModel emailTemplate)
        {
            if (TempData["IsEditMode"] != null)
            {
                emailTemplate.IsEditMode = Convert.ToBoolean(TempData["IsEditMode"]);
            }
            _IEmailTemplateDataProvider = new EmailTemplateDataProvider();
            var response = _IEmailTemplateDataProvider.AddEmailTemplate(emailTemplate, SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion


        #region List Email Template

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_EmailTemplate_List)]
        public ActionResult EmailTemplateList()
        {
            _IEmailTemplateDataProvider = new EmailTemplateDataProvider();
            var response = _IEmailTemplateDataProvider.SetEmailTemplateListPage();
            ViewBag.ModuleList = _IEmailTemplateDataProvider.GetModuleNames();
            ViewBag.EmailTypeList = _IEmailTemplateDataProvider.GetEmailType();
            return View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_EmailTemplate_List)]
        public JsonResult GetEmailTemplateList(SearchEmailTemplateListPage searchEmailTemplateListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _IEmailTemplateDataProvider = new EmailTemplateDataProvider();
            var response = _IEmailTemplateDataProvider.GetEmailTemplateList(searchEmailTemplateListPage, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        [HttpPost]
        public JsonResult DeleteEmailTemplateList(string tempid)
        {
            _IEmailTemplateDataProvider = new EmailTemplateDataProvider();
            var response = _IEmailTemplateDataProvider.DeleteTemplate(Convert.ToInt64(tempid));
            return Json(response);
        }

        [HttpPost]
        public JsonResult GetTokenList(string module)
        {
            _IEmailTemplateDataProvider = new EmailTemplateDataProvider();
            var response = _IEmailTemplateDataProvider.GetTokenList(module);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTemplateBody(string tempid)
        {
            _IEmailTemplateDataProvider = new EmailTemplateDataProvider();
            var response = _IEmailTemplateDataProvider.GetTemplateBody(Convert.ToInt64(tempid));
            return Json(response);
        }

        

        #endregion
    }
}
