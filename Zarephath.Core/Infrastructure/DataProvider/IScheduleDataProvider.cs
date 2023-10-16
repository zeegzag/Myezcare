using System;
using System.Collections.Generic;
using System.Web;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.Scheduler;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IScheduleDataProvider
    {
        #region Schedule Assignment

        ServiceResponse SetScheduleAssignmentModel();

        ServiceResponse GetReferralListForSchedule(SearchReferralListForSchedule searchReferralModel, int pageIndex,
                                                   int pageSize, string sortIndex, string sortDirection, long loggedInId);

        ServiceResponse GetReferralDetailForPopup(long referralID);

        ServiceResponse GetScheduleListByFacility(SearchScheduleListByFacility searchPara);

        List<Facility> GetFacilutyListForAutoComplete(string searchText, int pageSize, long regionID);

        ServiceResponse LoadAllFacilityByRegion(long? regionID);
        ServiceResponse SaveScheduleMasterFromCalender(ScheduleAssignmentModel scheduleAssignment,
                                                       long loggedInUserID);

        ServiceResponse CreateWeek(WeekMaster model, long loggedInUserID);
        #endregion

        #region Schedule Master

        ServiceResponse SetScheduleMasterPage();
        ServiceResponse GetScheduleMasterList(SearchScheduleMasterModel searchScheduleMasterModel, int pageIndex,
                                                   int pageSize, string sortIndex, string sortDirection, long loggedInId, string sortIndexArray);
        ServiceResponse DeleteScheduleMaster(SearchScheduleMasterModel searchScheduleMasterModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId, string sortIndexArray);


        ServiceResponse UpdateScheduleFromScheduleList(SearchScheduleMasterModel searchScheduleMasterModel, ScheduleMaster scheduleMaster, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID, string sortIndexArray);
        ServiceResponse ReScheduleFromScheduleList(SearchScheduleMasterModel searchScheduleMasterModel, ScheduleMaster scheduleMaster, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID, string sortIndexArray);
        ServiceResponse GetEmailDetail(long scheduleId);
        ServiceResponse GetSMSDetail(long scheduleId);
        ServiceResponse RemoveSchedulesFromWeekFacility(long weekMasterID, long? facilityID, long loggedInUserID);
        ServiceResponse GetScheduleNotificationLogs(long scheduleId);
        ServiceResponse HC_SaveNewSchedule(ChangeScheduleModel model, long loggedInId);
        ServiceResponse GetCareType(long payorID);
        #endregion

        #region Schedule Aggregator Logs

        ServiceResponse SetScheduleAggregatorLogsPage();
        ServiceResponse GetScheduleAggregatorLogsList(SearchScheduleAggregatorLogsModel searchScheduleAggregatorLogsModel, int pageIndex,
                                                    int pageSize, string sortIndex, string sortDirection);
        ServiceResponse ResendAggregatorData(SearchScheduleAggregatorLogsModel searchScheduleAggregatorLogsModel);
        ServiceResponse GetScheduleAggregatorLogsDetails(SearchScheduleAggregatorLogsModel searchScheduleAggregatorLogsModel);

        #endregion

        #region ScheduleBatchService

        ServiceResponse SetScheduleBatchServiceListPage();
        ServiceResponse SaveScheduleBatchService(ScheduleBatchService batchService, long loggedInUserID);

        ServiceResponse GetScheduleBatchServiceList(SearchScheduleBatchServiceModel searchScheduleBatchServiceModel, int pageIndex, int pageSize,
                                                    string sortIndex, string sortDirection);

        ServiceResponse DeleteScheduleBatchService(SearchScheduleBatchServiceModel searchScheduleBatchServiceModel,
                                                   int pageIndex, int pageSize, string sortIndex, string sortDirection);

        #endregion

        #region Email Service Confirmation and Cancellation

        ServiceResponse ConfirmationStatus(string confirmid, long loggedinid);
        ServiceResponse GetScheduleDetailEmailHtml(long scheduleid);
        ServiceResponse UpdateScheduleCancelstatus(CancelEmailDetailModel cancelEmailDetailModel, long logedinid);

        ServiceResponse GetCancelEmailDetail(string id);
        ServiceResponse SendParentEmail(ScheduleEmailModel scheduleEmailModel, long loggedInUserID);
        ServiceResponse SendParentSMS(ScheduleSmsModel scheduleSmsModel, long loggedInUserID);
        ServiceResponse GetRegionWiseWeekFacility(long regionID, long weekMasterID);
        ServiceResponse SaveRegionWiseWeekFacility(long regionID, long weekMasterID, string facilites);

        #endregion


        ServiceResponse SendScheduleDetailEmailSMS(ScheduleDetailEmailSMSParam scheduleDetailEmailSMSParam, bool batchService = false);

        ServiceResponse PrintScheduleNotice(string scheduleId, bool printCompalsary, long? loggedInId = null);





        #region In Home Care Data Provider

        ServiceResponse HC_SetScheduleAssignmentModel();

        ServiceResponse HC_GetReferralListForSchedule(SearchReferralListForSchedule searchReferralModel, int pageIndex,
                                                  int pageSize, string sortIndex, string sortDirection, long loggedInId);

        ServiceResponse HC_GetReferralDetailForPopup(long referralID);

        ServiceResponse GetEmployeesForSchedulingURL(long referralID, string employeeName);

        ServiceResponse GetEmployeesForEmpCalender(string employeeIDs);


        ServiceResponse GetEmployeeMatchingPreferences(long employeeID, long referralID);
        ServiceResponse HC_PrivateDuty_GetEmployeeMatchingPreferences(long employeeID, long referralID);



        ServiceResponse GetScheduleListByEmployees(SearchScheduleListByFacility searchPara);

        ServiceResponse HC_SaveScheduleMasterFromCalender(ScheduleAssignmentModel scheduleAssignment,
                                                       long loggedInUserID);


        ServiceResponse HC_UpdateScheduleFromScheduleList(SearchScheduleMasterModel searchScheduleMasterModel, ScheduleMaster scheduleMaster, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID, string sortIndexArray);


        ServiceResponse HC_SetScheduleMasterPage();
        ServiceResponse HC_DayCare_SetScheduleMasterPage();
        ServiceResponse HC_GetScheduleMasterList(SearchScheduleMasterModel searchScheduleMasterModel, int pageIndex,
                                                   int pageSize, string sortIndex, string sortDirection, long loggedInId, string sortIndexArray);

        ServiceResponse HC_DayCare_GetScheduleMasterList(SearchScheduleMasterModel searchScheduleMasterModel, int pageIndex,
                                                   int pageSize, string sortIndex, string sortDirection, long loggedInId, string sortIndexArray);


        ServiceResponse ChecklistGetEmpSMSDetail(long scheduleId, int templateId);
        ServiceResponse ChecklistSendEmpSMS(ScheduleSmsModel scheduleSmsModel, long loggedInUserID);

        ServiceResponse GetEmpSMSDetail(long scheduleId, int templateId);
        ServiceResponse SendEmpSMS(ScheduleSmsModel scheduleSmsModel, long loggedInUserID);
        ServiceResponse HC_GetEmpEmailDetail(long scheduleId);
        ServiceResponse HC_SendEmpEmail(ScheduleEmailModel scheduleEmailModel, long loggedInUserID);
        ServiceResponse HC_GetEmailDetail(long scheduleId);
        ServiceResponse HC_SendParentEmail(ScheduleEmailModel scheduleEmailModel, long loggedInUserID);

        ServiceResponse HC_GetSMSDetail(long scheduleId);
        ServiceResponse HC_SendParentSMS(ScheduleSmsModel scheduleSmsModel, long loggedInUserID);




        #region In Home Care Assignment New Code


        ServiceResponse HC_SetScheduleAssignmentModel01();
        ServiceResponse HC_DayCare_SetScheduleAssignmentModel();
        ServiceResponse HC_DayCare_SetScheduleAttendenceModel();

        ServiceResponse HC_SetVirtualVisitsListPage();
        ServiceResponse HC_CaseManagement_SetScheduleAssignmentModel();
        ServiceResponse HC_PrivateDuty_SetScheduleAssignmentModel();


        ServiceResponse HC_GetSchEmployeeListForSchedule(SearchEmployeeListForSchedule searchEmployeeModel, int pageIndex,
                                                  int pageSize, string sortIndex, string sortDirection, long loggedInId);

        ServiceResponse HC_PrivateDuty_GetSchEmployeeListForSchedule(SearchEmployeeListForSchedule searchEmployeeModel, int pageIndex,
                                                  int pageSize, string sortIndex, string sortDirection, long loggedInId);


        ServiceResponse HC_GetSchEmployeeDetailForPopup(SearchEmployeeListForSchedule model);
        ServiceResponse HC_PrivateDuty_GetSchEmployeeDetailForPopup(SearchEmployeeListForSchedule model);

        ServiceResponse HC_GetReferralForScheduling(SearchScheduleListByFacility model);
        ServiceResponse HC_DayCare_GetReferralForScheduling(SearchScheduleListByFacility model);
        ServiceResponse HC_CaseManagement_GetReferralForScheduling(SearchScheduleListByFacility model);
        ServiceResponse HC_PrivateDuty_GetReferralForScheduling(SearchScheduleListByFacility model);


        ServiceResponse HC_GetScheduleListByReferrals(SearchScheduleListByFacility model);
        ServiceResponse HC_GetVirtualVisitsList(SearchVirtualVisitsListModel searchPara, int pageIndex,
                                                  int pageSize, string sortIndex, string sortDirection, long loggedInId);
        ServiceResponse HC_GetReferralEmployeeVisits(SearchReferralEmployeeModel searchPara, int pageIndex,
                                                  int pageSize, string sortIndex, string sortDirection, long loggedInId);
        ServiceResponse HC_DayCare_GetScheduleListByReferrals(SearchScheduleListByFacility model);
        ServiceResponse HC_Daycare_GetScheduledPatientList(SearchScheduledPatientModel model);
        ServiceResponse HC_Daycare_PatientClockInClockOut(Daycare_SavePatient_AttendecenModel model, long loggedInId);
        ServiceResponse HC_DayCare_GetSchedulePatientTasks(Daycare_SavePatient_AttendecenModel model);



        ServiceResponse HC_CaseManagement_GetScheduleListByReferrals(SearchScheduleListByFacility model);
        ServiceResponse HC_PrivateDuty_GetScheduleListByReferrals(SearchScheduleListByFacility model);


        ServiceResponse HC_SaveScheduleFromCalender(ScheduleAssignmentModel scheduleAssignment, long loggedInUserID);
        ServiceResponse HC_PrivateDuty_SaveScheduleFromCalender(ScheduleAssignmentModel scheduleAssignment, long loggedInUserID);
        ServiceResponse HC_DayCare_SaveScheduleFromCalender(ScheduleAssignmentModel scheduleAssignment, long loggedInUserID);

        ServiceResponse DeleteScheduleFromCalender(SearchScheduleMasterModel searchScheduleMasterModel, long loggedInUserID);
        ServiceResponse HC_PrivateDuty_DeleteScheduleFromCalender(SearchScheduleMasterModel searchScheduleMasterModel, long loggedInUserID);
        ServiceResponse HC_DayCare_DeleteScheduleFromCalender(SearchScheduleMasterModel searchScheduleMasterModel, long loggedInUserID);




        ServiceResponse HC_GetEmpRefSchPageModel();
        ServiceResponse HC_PrivateDuty_GetEmpRefSchPageModel();

        ServiceResponse HC_GetEmpRefSchOptions(SearchEmpRefSchOption model, int pageIndex,
                                                   int pageSize, string sortIndex, string sortDirection, string sortIndexArray, long loggedInId);
        /*Schedule master OPT*/
        ServiceResponse GetEmpRefSchOptions_PatientVisitFrequency_HC(SearchEmpRefSchOption model, int pageIndex,
            int pageSize, string sortIndex, string sortDirection, string sortIndexArray, long loggedInId);
        ServiceResponse GetEmpRefSchOptions_ClientOnHoldData_HC(SearchEmpRefSchOption model, int pageIndex,
                                                   int pageSize, string sortIndex, string sortDirection, string sortIndexArray, long loggedInId);
        ServiceResponse GetEmpRefSchOptions_ReferralInfo_HC(SearchEmpRefSchOption model, int pageIndex,
                                                   int pageSize, string sortIndex, string sortDirection, string sortIndexArray, long loggedInId);
        ServiceResponse GetEmpRefSchOptions_ScheduleInfo_HC(SearchEmpRefSchOption model, int pageIndex,
                                                   int pageSize, string sortIndex, string sortDirection, string sortIndexArray, long loggedInId);
        /*Schedule master OPT*/
        ServiceResponse HC_GetEmpCareTypeIds(SearchEmpRefSchOption model, long loggedInId);
        ServiceResponse HC_PrivateDuty_GetEmpRefSchOptions(SearchEmpRefSchOption model, int pageIndex,
                                                   int pageSize, string sortIndex, string sortDirection, string sortIndexArray, long loggedInId);


        ServiceResponse HC_DayCare_GetReferralBillingAuthorizationList(ReferralBillingAuthorizatioSearchModel model);
        ServiceResponse HC_DayCare_GetEmpRefSchOptions(SearchEmpRefSchOption model, long loggedInId);
        ServiceResponse HC_CaseManagement_GetEmpRefSchOptions(SearchEmpRefSchOption model, long loggedInId);


        ServiceResponse CreateBulkSchedule(SearchEmpRefSchOption model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId);
        ServiceResponse HC_PrivateDuty_CreateBulkSchedule(SearchEmpRefSchOption model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId);
        ServiceResponse HC_SaveReferralCSVFile(HttpRequestBase httpRequestBase, long loggedInUserID);


        ServiceResponse CreateBulkScheduleUsingCSV(ReferralCsvModel referralCsvModel, long loggedInUserID);
        ServiceResponse HC_DayCare_CreateBulkSchedule(SearchEmpRefSchOption model, long loggedInId);


        ServiceResponse GetSchEmpRefSkills(SearchEmpRefMatchModel model);
        ServiceResponse HC_PrivateDuty_GetSchEmpRefSkills(SearchEmpRefMatchModel model);


        ServiceResponse DeleteEmpRefSchedule(DeleteEmpRefScheduleModel model, long loggedInId);
        ServiceResponse HC_PrivateDuty_DeleteEmpRefSchedule(DeleteEmpRefScheduleModel model, long loggedInId);
        ServiceResponse HC_DayCare_DeleteEmpRefSchedule(DeleteEmpRefScheduleModel model, long loggedInId);

        ServiceResponse GetAssignedEmployees(ReferralTimeSlotModel model);
        ServiceResponse HC_PrivateDuty_GetAssignedEmployees(ReferralTimeSlotModel model);
        ServiceResponse HC_DayCare_GetAssignedFacilities(ReferralTimeSlotModel model);


        ServiceResponse OnHoldUnHoldAction(PatientHoldDetail model, long loggedInId);
        ServiceResponse HC_PrivateDuty_OnHoldUnHoldAction(PatientHoldDetail model, long loggedInId);

        ServiceResponse HC_DayCare_OnHoldUnHoldAction(PatientHoldDetail model, long loggedInId);
        ServiceResponse HC_CaseManagement_OnHoldUnHoldAction(PatientHoldDetail model, long loggedInId);


        ServiceResponse HC_DayCare_SavePatientAttendance(ScheduleAttendaceDetail model, long loggedInId);

        ServiceResponse OnRemoveScheduleAction(RemoveScheduleModel model, long loggedInId);
        ServiceResponse HC_PrivateDuty_OnRemoveScheduleAction(RemoveScheduleModel model, long loggedInId);

        ServiceResponse HC_DayCare_OnRemoveScheduleAction(RemoveScheduleModel model, long loggedInId);
        ServiceResponse HC_CaseManagement_OnRemoveScheduleAction(RemoveScheduleModel model, long loggedInId);


        ServiceResponse HC_DayCare_DeleteScheduleMaster(SearchScheduleMasterModel searchScheduleMasterModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId, string sortIndexArray);
        ServiceResponse HC_SetEmployeeVisitsPage();
        ServiceResponse SavePickUpDropCall(SaveEmployeeVisitsTransportLog model, long loggedInId);
        #endregion

        #region Referral Case Load

        ServiceResponse HC_GetRCLEmpRefSchOptions(SearchRCLEmpRefSchOption model, int pageIndex,
                                                   int pageSize, string sortIndex, string sortDirection, string sortIndexArray, long loggedInId);

        #endregion


        #region Myezcare - Pending Schedules

        ServiceResponse HC_PendingSchedules();
        ServiceResponse HC_GetPendingScheduleList(SearchPendingSchedules model, int pageIndex,
                                                  int pageSize, string sortIndex, string sortDirection, long loggedInId);
        ServiceResponse HC_DeletePendingSchedule(SearchPendingSchedules model, int pageIndex,
                                                  int pageSize, string sortIndex, string sortDirection, long loggedInId);

        ServiceResponse HC_ProcessPendingSchedule(PendingScheduleListModel model, long loggedInId);





        #endregion


        #endregion

        ServiceResponse SendVirtualVisitsReminderNotification(long scheduleID, long referralID, long employeeID, bool sendSMS, bool sendEmail);
        ServiceResponse Daycare_GetRelationTypeList(int type);
        ServiceResponse HC_SaveReferralProfileImg(HttpRequestBase currentHttpRequest, bool isEmployeeDocument = false, long referralID = 0);

        #region "Nurse Schedule"

        ServiceResponse GetCareTypes();
        ServiceResponse GetReferralsByCareTypeId(string careTypeId);
        //ServiceResponse AddAppointment(ScheduleDTO schedule, long loggedInUserID);
        //ServiceResponse UpdateAppointment(ScheduleDTO schedule, long loggedInUserID);

        ServiceResponse GetNurseSchedules(string careTypeIds, string employeeIds, string referralIds);

        long AddScheduleToReferralTimeSlotMaster(ReferralTimeSlotMaster schedule, long loggedInUserID);
        long AddScheduleToReferralTimeSlotDetails(ReferralTimeSlotDetail schedule, long loggedInUserID);
        long AddScheduleToReferralTimeSlotDates(ReferralTimeSlotDates schedule, long loggedInUserID);

        ServiceResponse CreateBulkNurseSchedule(ScheduleDTO schedule, long loggedInUserID);
        ServiceResponse UpdateBulkNurseSchedule(ScheduleDTO schedule, long loggedInUserID);

        bool DeleteScheduleFromReferralTimeSlotTables(long referralId, DateTime StartDate, DateTime EndDate);

        bool DeleteBulkNurseSchedule(long scheduleID);

        ScheduleMaster GetScheduleMasterById(long ScheduleID);

        #endregion

        #region Listen to events
        ServiceResponse ProcessEvent(EventData model);
        #endregion

        #region Visit Reason
        ServiceResponse GetVisitReasonList(GetVisitReasonModel model);
        ServiceResponse SaveVisitReason(VisitReasonModel model, long loggedInUserID);
        ServiceResponse GetVisitReasonModalDetail(long scheduleID);
        #endregion
    }
}
