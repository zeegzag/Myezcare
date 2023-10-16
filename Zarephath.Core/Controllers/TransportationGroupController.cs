using System.Web.Mvc;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Controllers
{
    public class TransportationGroupController : BaseController
    {
        #region Transportation Assignment

        ITransportationGroupDataProvider _iTransportationGroupDataProvider;

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Transportation_Groups)]
        public ActionResult TransportationAssignment()
        {
            _iTransportationGroupDataProvider = new TransportationGroupDataProvider();
            ServiceResponse response = _iTransportationGroupDataProvider.SetTransPortationGroup();
            return View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Transportation_Groups)]
        public ContentResult GetReferralListForTransportationAssignment(SearchReferralListForTransportationAssignment searchRefrrelForTransportatioGroupList, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iTransportationGroupDataProvider = new TransportationGroupDataProvider();
            return CustJson(_iTransportationGroupDataProvider.GetReferralListForTransportationAssignment(searchRefrrelForTransportatioGroupList, pageIndex, pageSize, sortIndex,
                                                                 sortDirection));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Transportation_Groups)]
        public ContentResult GetAssignedClientListForTransportationAssignment(SearchAssignedClientListForTransportationAssignment searchModel)
        {
            _iTransportationGroupDataProvider = new TransportationGroupDataProvider();
            return CustJson(_iTransportationGroupDataProvider.GetAssignedClientListForTransportationAssignment(searchModel));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Transportation_Groups)]
        public ContentResult SaveTransportationGroup(AddTransportationGroupModel transportationGroup, SearchAssignedClientListForTransportationAssignment searchTransportatioGroupList)
        {
            _iTransportationGroupDataProvider = new TransportationGroupDataProvider();
            return CustJson(_iTransportationGroupDataProvider.SaveTransportationGroup(transportationGroup, searchTransportatioGroupList, SessionHelper.LoggedInID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Transportation_Groups)]
        public ContentResult RemoveTransportationGroup(long transportationGroupID, SearchAssignedClientListForTransportationAssignment searchTransportatioGroupList)
        {
            _iTransportationGroupDataProvider = new TransportationGroupDataProvider();
            return CustJson(_iTransportationGroupDataProvider.RemoveTransportationGroup(transportationGroupID, searchTransportatioGroupList, SessionHelper.LoggedInID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Transportation_Groups)]
        public ContentResult SaveTransportationGroupClient(TransportationGroupClient transportationGroupClient, SearchAssignedClientListForTransportationAssignment searchTransportatioGroupList)
        {
            _iTransportationGroupDataProvider = new TransportationGroupDataProvider();
            return CustJson(_iTransportationGroupDataProvider.SaveTransportationGroupMultipleClient(transportationGroupClient, searchTransportatioGroupList, SessionHelper.LoggedInID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Transportation_Groups)]
        public ContentResult RemoveTransportationGroupClient(long transportationGroupClientID)
        {
            _iTransportationGroupDataProvider = new TransportationGroupDataProvider();
            return CustJson(_iTransportationGroupDataProvider.RemoveTransportationGroupClient(transportationGroupClientID, SessionHelper.LoggedInID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Transportation_Groups)]
        public JsonResult SaveTransportationGroupFilter(SaveTransportationGroupFilter model, SearchAssignedClientListForTransportationAssignment searchTransportatioGroupList)
        {
            _iTransportationGroupDataProvider = new TransportationGroupDataProvider();
            return JsonSerializer(_iTransportationGroupDataProvider.SaveTransportationGroupFilter(model, searchTransportatioGroupList, SessionHelper.LoggedInID));
        }
        #endregion
    }
}
