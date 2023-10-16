using System;
using System.Collections.Generic;
using System.Web;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IFacilityDataProvider
    {
        #region
        ServiceResponse HC_SetAddReferralPage(long referralID, long loggedInUserID);
        ServiceResponse HC_AddReferral(HC_AddReferralModel addReferralModel, long loggedInUserID);
        ServiceResponse AddContact(AddReferralModel addReferralModel, long loggedInUserID);
        List<Contact> GetContactList(string searchText, int pageSize);
        ServiceResponse DeteteContact(long contactMappingID, long loggedInUserID);
        ServiceResponse HC_SetReferralListPage(long loggedInID);
        ServiceResponse GetReferralList(SearchReferralListModel searchReferralModel, int pageIndex, int pageSize,
                                        string sortIndex, string sortDirection, long loggedInId);
        ServiceResponse DeleteReferral(SearchReferralListModel searchReferralListModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId);
        ServiceResponse HC_ReferralTimeSlots();
        ServiceResponse HC_ReferralTimeSlotss(string id);
        ServiceResponse GetRtsMasterlist(SearchRTSMaster searchRTSMaster, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteRtsMaster(SearchRTSMaster searchRTSMaster, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse AddRtsMaster(ReferralTimeSlotMaster rtsMaster, long loggedInUserID);
        ServiceResponse GetRtsDetaillist(SearchRTSDetail searchRTSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteRtsDetail(SearchRTSDetail searchRTSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse AddRtsDetail(ReferralTimeSlotDetail rtsDetail, long loggedInUserID);
        ServiceResponse UpdateRtsDetail(ReferralTimeSlotDetail rtsDetail, SearchRTSDetail searchRTSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse ReferralTimeSlotForceUpdate(ReferralTimeSlotDetail model, SearchRTSDetail searchRTSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);

        ServiceResponse HC_ReferralCareTypeTimeSlots();
        ServiceResponse AddCareTypeSlot(CareTypeTimeSlot model, long loggedInUserID);
        ServiceResponse GetCareTypeScheduleList(SearchCTSchedule searchCTSchedule, int pageIndex, int pageSize, string sortIndex, string sortDirection);

        //ServiceResponse SaveAndGetFacilityDetails(FacilityDetailModel objFacility);

        ServiceResponse GetEmployeeGroup(string GroupID);

        ServiceResponse GetEmployeeByGroupId(string groupIds);
        ServiceResponse UpdateEmployeeGroupId(long GroupID, string EmployeeIDs,bool IsChecked);
        ServiceResponse SaveEmpGroup(string GroupName,long ReferralID,bool IsEditMode);
        ServiceResponse RemoveAllAssignedGroup(long GroupID, string EmployeeID);
        #endregion

        //  end Ak


    }
}
