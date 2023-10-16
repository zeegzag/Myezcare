using Myezcare_Admin.Helpers;
using Myezcare_Admin.Infrastructure;
using Myezcare_Admin.Infrastructure.DataProvider;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.Entity;
using Myezcare_Admin.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Myezcare_Admin.Controllers
{
    public class OrganizationController : BaseController
    {
        private IOrganizationDataProvider _organizationDataProvider;

        public OrganizationController()
        {
            _organizationDataProvider = new OrganizationDataProvider();
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult AddOrganization(string id)
        {
            long organizationId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = new ServiceResponse();
            if (organizationId > 0)
                response = _organizationDataProvider.SetAddOrganizationPage(organizationId);
            else
            {
                response.IsSuccess = false;
                response.ErrorCode = Constants.ErrorCode_NotFound;
            }
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public JsonResult AddOrganization(MyEzcareOrganization organization)
        {
            var response = _organizationDataProvider.AddOrganization(organization, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult OrganizationList()
        {
            var response = _organizationDataProvider.SetOrganizationListPage().Data;
            return View(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ContentResult GetOrganizationList(SearchOrganizationModel searchOrganizationModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            var response = _organizationDataProvider.GetOrganizationList(searchOrganizationModel, pageIndex, pageSize, sortIndex, sortDirection);
            return CustJson(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public JsonResult SaveOrganization(AddOrganizationModel model)
        {
            var response = _organizationDataProvider.SaveOrganization(model, SessionHelper.LoggedInID);
            return Json(response);
        }

        #region Upload File

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public JsonResult SaveFile()
        {
            var response = _organizationDataProvider.SaveFile(Request, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public JsonResult ImportDataInDatabase(ImportDataTypeModel model)
        {
            var response = _organizationDataProvider.ImportDataInDatabase(model, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult Esign(string id, string id1)
        {
            long organizationId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            long organizationEsignId = 0;
            if (!string.IsNullOrEmpty(id1))
                organizationEsignId = !string.IsNullOrEmpty(id1) ? Convert.ToInt64(Crypto.Decrypt(id1)) : 0;
            ServiceResponse response = new ServiceResponse();
            if (organizationId > 0)
                response = _organizationDataProvider.SetOrganizationEsignPage(organizationId, organizationEsignId);
            else
            {
                response.IsSuccess = false;
                response.ErrorCode = Constants.ErrorCode_NotFound;
            }
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ContentResult Esign(OrganizationEsignModel organizationEsign)
        {
            var response = _organizationDataProvider.OrganizationEsign(organizationEsign, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ContentResult SendEsignEmail(OrganizationEsignDetails organizationEsign)
        {
            var response = _organizationDataProvider.SendEsignEmail(organizationEsign, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousPermission)]
        public ActionResult CustomerEsign(string id)
        {
            ServiceResponse response = new ServiceResponse();
            response = _organizationDataProvider.SetCustomerEsignPage(id);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousPermission)]
        public ActionResult CustomerEsign(CustomerEsignModel customerEsign)
        {
            var response = _organizationDataProvider.CustomerEsign(customerEsign, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        public ActionResult ValidateDomain([Bind(Prefix = "CustomerEsignDetails.DomainName")]string DomainName)
        {
            try
            {
                ServiceResponse response = new ServiceResponse();
                response = _organizationDataProvider.CheckDomainNameExists(DomainName);
                return Json(!response.IsSuccess, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public JsonResult GetAllOrganizationList()
        {
            var response = _organizationDataProvider.GetAllOrganizationList().Data;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
