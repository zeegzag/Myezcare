using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Controllers
{
    //[CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
    
    public class HomeController : BaseController
    {
        private IHomeDataProvider _iHomeDataProvider;

        [CustomAuthorize(Permissions = Constants.Permission_Dashboard)]
        [HttpGet]
        public ActionResult Dashboard()
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.SetDashboardPage(SessionHelper.LoggedInID);
            return View(response.Data);
        }

        [HttpPost]
        public JsonResult ReferralInternalMessageList(int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.GetReferralInternalMessageList(SessionHelper.LoggedInID, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetReferralResolvedInternalMessageList(int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.GetReferralResolvedInternalMessageList(SessionHelper.LoggedInID, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult MarkResolvedMessageAsRead(string EncryptedReferralInternalMessageID, long ReferralID, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {

            long referralInternalMessageId = Convert.ToInt64(Crypto.Decrypt(EncryptedReferralInternalMessageID));

            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.MarkResolvedMessageAsRead(referralInternalMessageId, ReferralID, SessionHelper.LoggedInID,
                sortDirection, sortIndex, pageIndex, pageSize);
            return JsonSerializer(response);
        }


        [HttpPost]
        public JsonResult GetReferralSparFormandCheckList(int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.GetReferralSparFormList(SessionHelper.LoggedInID, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetReferralMissingDocumentList(int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.GetReferralMissingDocumentList(SessionHelper.LoggedInID, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetReferralMissingDocument(long ReferralID)
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.GetReferralMissingDocument(ReferralID);
            return JsonSerializer(response);
        }


        [HttpPost]
        public JsonResult GetReferralInternalMissingDocumentList(int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.GetReferralInternalMissingDocumentList(SessionHelper.LoggedInID, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetReferralInternalMissingDocument(long ReferralID)
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.GetReferralInternalMissingDocument(ReferralID);
            return JsonSerializer(response);
        }


        [HttpPost]
        public JsonResult GetLayoutRelatedDetails(SearchLayoutDetailModel searchModel)
        {
            _iHomeDataProvider = new HomeDataProvider();

            searchModel.AssineeID = SessionHelper.LoggedInID;
            searchModel.CreatedID = SessionHelper.LoggedInID;
            ServiceResponse response = _iHomeDataProvider.GetLayoutRelatedDetails(searchModel);
            return JsonSerializer(response);
        }


        [HttpPost]
        public JsonResult GetReferralAnsellCaseyReviewList(int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.GetReferralAnsellCaseyReviewList(SessionHelper.LoggedInID, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }


        [HttpPost]
        public JsonResult GetReferralAssignedNotesReviewList(int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.GetReferralAssignedNotesReviewList(SessionHelper.LoggedInID, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }


        public Action Edi837()
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.GenerateEdi837();
            return null;

        }

    }
}
