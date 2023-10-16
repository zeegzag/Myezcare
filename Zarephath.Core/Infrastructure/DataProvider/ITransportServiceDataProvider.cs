using System.Collections.Generic;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface ITransportServiceDataProvider
    {
        #region Transport Service
        ServiceResponse HC_SetAddTransportContactPage(long contactId);
        ServiceResponse HC_AddTransportContact(SetTransportContactModel transportContactModel);
        ServiceResponse HC_SetTransportContactListPage();
        ServiceResponse HC_GetTransportContactList(SearchTransportContactModel searchTransportContactModel, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeleteTransportContact(SearchTransportContactModel searchTransportContactModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        List<GetSearchOrganizationNameModel> GetSearchOrganizationName(int pageSize, string searchText = null);
        #endregion

        #region Vehicle
        ServiceResponse HC_SetAddVehiclePage(long vehicleId);
        ServiceResponse HC_AddVehicle(SetVehicleModel vehicleModel);
        ServiceResponse HC_SetVehicleListPage();
        ServiceResponse HC_GetVehicleList(SearchVehicleModel searchVehicleModel, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeleteVehicle(SearchVehicleModel searchVehicleModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse HC_SetTransportAssignmentPage(long transportId);
        #endregion
        ServiceResponse SaveTransportAssignment(TransportAssignmentModel transportAssignmentModel);
        ServiceResponse GetTransportAssignmentList(SearchTransportAssignmentModel searchTransportAssignmentModel, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteTransportAssignment(SearchTransportAssignmentModel searchTransportAssignmentModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse SaveTransportAssignPatient(TransportAssignPatientModel transportAssignPatientModel);
        ServiceResponse HC_SetTransportAssignmentList(long loggedInID);
        ServiceResponse SearchTransportAssignPatientListModel(SearchTransportAssignPatientListModel searchTransportAssignPatientListModel, int pageIndex, int pageSize,
                                        string sortIndex, string sortDirection, long loggedInId);
        ServiceResponse DeleteTransportAssignmentReferral(SearchTransportAssignPatientListModel searchTransportAssignPatientListModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        //Transport Assignment Group
        ServiceResponse HC_SetTransportAssignmentGroupPage(long transportId);
        ServiceResponse SearchTransportAssignmentGroupList(SearchTransportAssignmentGroupModel searchTransportAssignmentGroupModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse GetTransportGroup(long FacilityID, string StartDate, string EndDate);
        ServiceResponse GetTransportGroupDetail(long TransportGroupID); 
        ServiceResponse SaveTransportGroup(TransportGroupModel transportGroupModel);
        ServiceResponse SaveTransportGroupAssignPatient(SearchTransportAssignmentGroupModel searchTransportAssignmentGroupModel);
        ServiceResponse SaveTransportGroupAssignPatientNote(TransportGroupAssignPatientModel transportGroupAssignPatientModel);
        ServiceResponse DeleteTransportGroupAssignPatientNote(TransportGroupAssignPatientModel transportGroupAssignPatientModel);
    }
}
