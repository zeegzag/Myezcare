using System;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IHomeDataProvider
    {
        //Internal Messgae List
        ServiceResponse GetNotification();
        ServiceResponse SetDashboardPage(long loggedInUser);
        ServiceResponse GetReferralInternalMessageList(long loggedInUser, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse GetReferralResolvedInternalMessageList(long loggedInUser, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse GetPendingBypassVisit();
        ServiceResponse MarkResolvedMessageAsRead(long messageId, long referralId, long loggedInUser, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10);
        ServiceResponse GetReferralSparFormList(long loggedInUser, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse GetReferralMissingDocumentList(long loggedInUser, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse GetReferralMissingDocument(long referralId);
        ServiceResponse GetReferralInternalMissingDocumentList(long loggedInUser, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse GetReferralInternalMissingDocument(long referralId);
        ServiceResponse GetLayoutRelatedDetails(SearchLayoutDetailModel searchModel);

        ServiceResponse GetReferralAnsellCaseyReviewList(long loggedInUser, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse GetReferralAssignedNotesReviewList(long loggedInUser, string sortDirection, string sortIndex, int pageIndex, int pageSize);


        ServiceResponse GenerateEdi837();





        #region Home Care Dashboards
        ServiceResponse HC_SetDashboardPage(long loggedInUser);
        ServiceResponse HC_GetEmpClockInOutList(long loggedInUser, DateTime? startDate, DateTime? endDate, string employeeName, string careTypeID, string status, string sortDirection, string sortIndex, int pageIndex, int pageSize, string TimeSlots, string RegionID = "");
        ServiceResponse GetPatientAddressList(long loggedInUser, DateTime? startDate, DateTime? endDate, string employeeName, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse HC_GetEmpOverTimeList(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse HC_GetPatientNewList(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse GetActivePatientCountList();
        ServiceResponse HC_GetPatientNotScheduleList(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse HC_GetEmployeeTimeStatics(DateTime? startDate, DateTime? endDate);
        ServiceResponse HC_GetNotificationsList(long loggedInUser, string IsDeleted, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse HC_GetPriorAuthExpiring(long loggedInUser, string sortDirection, string sortIndex, int pageIndex, int pageSize, bool isExpired);
        ServiceResponse HC_GetPatientBirthday(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse HC_GetEmployeeBirthday(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse HC_GetPatientClockInOutList(long loggedInUser, DateTime? startDate, DateTime? endDate, string patientName, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse HC_GetReferralPayor(long loggedInUser);
        ServiceResponse HC_GetPatientDischargedList(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse HC_GetPatientTransferList(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse HC_GetPatientPendingList(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse HC_GetPatientOnHoldList(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse HC_GetPatientMedicaidList(long loggedInUser, DateTime? startDate, DateTime? endDate, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse GetCaretype();
        ServiceResponse GetRegion();
        ServiceResponse HC_GetEmpClockInOutListWithOutStatus(long loggedInUser, DateTime? startDate, DateTime? endDate);

        #region Web Notification

        ServiceResponse HC_GetWebNotifications(long loggedInUserID);
        ServiceResponse HC_DeleteWebNotification(string webNotificationIDs, long loggedInUserID);
        ServiceResponse HC_MarkAsReadWebNotifications(string webNotificationIDs, long loggedInUserID, bool IsRead);
        ServiceResponse HC_GetWebNotificationsCount(long loggedInUserID);
        #endregion

        #endregion









    }
}
