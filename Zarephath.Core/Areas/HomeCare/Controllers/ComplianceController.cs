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
    public class ComplianceController : BaseController
    {
        private IComplianceDataProvider _complianceDataProvider;

        public ComplianceController()
        {
            _complianceDataProvider = new ComplianceDataProvider();
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Compliance_AddUpdate + "," + Constants.HC_Permission_Compliance_List)]
        public ActionResult ComplianceDetail()
        {
            ServiceResponse response = _complianceDataProvider.AddCompliance(0);
            return View(response.Data);
        }
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Compliance_AddUpdate)]
        public ContentResult GetAssigneeList(int userType = 0)
        {
            return CustJson(((ComplianceModel)_complianceDataProvider.GetAssigneeList(userType).Data).DirectoryList);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Compliance_AddUpdate)]
        public ContentResult GetDirectoryList(int userType=0)
        {
            return CustJson(((ComplianceModel)_complianceDataProvider.AddCompliance(userType).Data).DirectoryList);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Compliance_AddUpdate)]
        public ContentResult SaveCompliance(Compliance compliance)
        {
            return CustJson(_complianceDataProvider.SaveCompliance(compliance, SessionHelper.LoggedInID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Compliance_AddUpdate + "," + Constants.HC_Permission_Compliance_List)]
        public ContentResult GetComplianceList(SearchComplianceListPage searchComplianceListPage, int pageIndex = 1,int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            return CustJson(_complianceDataProvider.GetComplianceList(searchComplianceListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }
        
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Compliance_Delete)]
        public ContentResult DeleteCompliance(SearchComplianceListPage searchComplianceListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            return CustJson(_complianceDataProvider.DeleteCompliance(searchComplianceListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Compliance_AddUpdate)]
        public ContentResult GetOrganizationFormList()
        {
            _complianceDataProvider = new ComplianceDataProvider(Constants.MyezcareOrganizationConnectionString);
            var response = _complianceDataProvider.GetOrganizationFormList();
            return CustJson(response);
        }
        public JsonResult ChangeSortingOrder(ChangeSortingOrderModel model)
        {
            _complianceDataProvider = new ComplianceDataProvider();
            return JsonSerializer(_complianceDataProvider.ChangeSortingOrder(model));
        }
    }
}
