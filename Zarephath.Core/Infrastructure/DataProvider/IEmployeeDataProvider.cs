using System.Collections.Generic;
using System.Web;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IEmployeeDataProvider
    {
        #region Add Employee

        ServiceResponse SetAddEmployeePage(long employeeID);
        ServiceResponse AddEmployee(AddEmployeeModel addEmployeeModel, long loggedInUserID);
        ServiceResponse HC_EmployeeLogin(HC_AddEmployeeModel addEmployeeModel);
        ServiceResponse HC_AddEmployeeSSNLog(HC_AddEmployeeModel addEmployeeModel, long loggedInUserID);
        #endregion

        #region Employee List

        ServiceResponse SetListEmployeePage();
        ServiceResponse GetEmployeeList(SearchEmployeeModel searchEmployeeModel, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        List<ListEmployeeModel> GetEmployeesForNurseSchedule(SearchEmployeeModel searchEmployeeModel, string sortIndex, string sortDirection);
        ServiceResponse DeleteEmployee(SearchEmployeeModel searchEmployeeModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse HC_BulkResendRegistrationMail(string empIds); 
        ServiceResponse GetEmployeeGroup(string name);
        ServiceResponse UpdateBulkGroup(string empids, string groupids);
        ServiceResponse GetEmployeeEmailSignature(string employeeid);
        ServiceResponse SaveEmployeeEmailSignature(string EmployeeID, string Name, string Description, string UpdatedBy);
        #endregion Employee List


        #region EmployeeNotes
        ServiceResponse HC_SaveEmployeeNotes(long EmployeeNoteId, string noteDetail, long loggedInUserId, long CommonNoteID, bool isEdit = false);
        ServiceResponse GetEmployeeNotes(long EmployeeId);
        ServiceResponse DeleteEmployeeNote(long CommonNoteID, long loggedInUserId);
        #endregion EmployeeNotes

        #region Employee Checklist

        ServiceResponse HC_GetEmployeeChecklist(string employeeId);
        ServiceResponse HC_SaveEmployeeChecklist(EmployeeChecklist model, long loggedInUserID);

        #endregion Employee Checklist

        #region Employee Notification Prefs

        ServiceResponse HC_GetEmployeeNotificationPrefs(string employeeId);
        ServiceResponse HC_SaveEmployeeNotificationPrefs(EmployeeNotificationPrefsModel model, long loggedInUserID);

        #endregion Employee Notification Prefs

        #region In HOME CARE Data Provider Code



        List<EmployeePreferenceModel> GetSearchSkill(int pageSize, string searchText = null);
        //ServiceResponse AddPreference(Preference preference);
        ServiceResponse DeletePreference(EmployeePreferenceModel model);

        #region HC_AddEmployee
        ServiceResponse HC_SetAddEmployeePage(long employeeID);
        ServiceResponse HC_AddEmployee(HC_AddEmployeeModel addEmployeeModel, long loggedInUserID);
        ServiceResponse HC_ResendRegistrationMail(long EmployeeID);
        ServiceResponse HC_ResendRegistrationSMS(long EmployeeID);
        ServiceResponse UploadFile(HttpRequestBase currentHttpRequest);
        ServiceResponse AddEmployeeContact(HC_AddEmployeeModel addEmployeeModel, long loggedInUserID);
        ServiceResponse DeteteEmployeeContact(long contactMappingID, long loggedInUserID);
        ServiceResponse GetEmployeeOverTimePayBillingReportList(SearchEmployeeBillingReportListPage searchEmployeeBillingReportListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse SaveRegularHours(RegularHoursModel model);
        #endregion



        #region Employee Days with time slots

        ServiceResponse HC_EmployeeTimeSlots();
       
        #region ETS Msater

        ServiceResponse GetEtsMasterlist(SearchETSMaster searchETSMaster, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteEtsMaster(SearchETSMaster searchETSMaster, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse AddEtsMaster(EmployeeTimeSlotMaster etsMaster, long loggedInUserID);
        #endregion

        #region ETS Detail

        ServiceResponse GetEtsDetaillist(SearchETSDetail searchETSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteEtsDetail(SearchETSDetail searchETSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse AddEtsDetail(EmployeeTimeSlotDetail etsDetail, long loggedInUserID);
        ServiceResponse UpdateEtsDetail(EmployeeTimeSlotDetail model, SearchETSDetail searchETSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse EmployeeTimeSlotForceUpdate(EmployeeTimeSlotDetail model, SearchETSDetail searchETSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse AddEtsDetailBulk(BulkEmployeeTimeSlotDetails etsDetail, long loggedInUserID);
        #endregion


        #endregion


        #region Employee Calender
        ServiceResponse HC_EmployeeCalender();
        #endregion


        #region Employee DayOff
        ServiceResponse HC_EmployeeDayOffPage();
        ServiceResponse HC_SaveEmployeeDayOff(EmployeeDayOff model, long loggedInUserId);
        ServiceResponse HC_CheckForEmpSchedules(EmployeeDayOff model);
        
        ServiceResponse HC_DayOffAction(EmployeeDayOff model, long loggedInUserId);
        

        ServiceResponse HC_GetEmployeeDayOffList(SearchEmpDayOffModel searchEmpDayOffModel, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeleteEmployeeDayOffList(SearchEmpDayOffModel searchEmpDayOffModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        #endregion



        #region Send Bulk SMS 
        ServiceResponse HC_SendBulkSms();
        ServiceResponse HC_GetEmployeeListForSendSms(SearchSBSEmployeeModel model);
        ServiceResponse HC_SendBulkSms(SendSMSModel model, long loggedinId);
        ServiceResponse GetSentSmsList(SearchSentSmsModel model, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse GetEmployeesForSentSms(long groupMessageLogId);

        #endregion

        #region UploadEmployeeSignatureFromAPI
        ServiceResponse UploadEmpSignature(HttpRequestBase currentHttpRequest);
        #endregion

        #region EmployeeAttendance
        ServiceResponse HC_ClockInOut(long employeeID);
        ServiceResponse SaveClockInOut(SaveClockInOutModel saveClockInOutModel);
        ServiceResponse EmployeeAttendanceCalendar(SearchRefCalender model);
        #endregion EmployeeAttendance
        #endregion

        #region Web Notification

        ServiceResponse HC_SaveWebNotification(SendSMSModel model, long loggedInUserID);

        #endregion

        #region Broadcast Notification

        ServiceResponse HC_BroadcastNotification(string type, long Id, string siteName);

        ServiceResponse HC_SaveBroadcastNotification(SendSMSModel model, long loggedinId, string domainName);

        ServiceResponse HC_GetBroadcastNotification(SearchSentSmsModel model, int pageIndex, int pageSize, string sortIndex, string sortDirection);

        ServiceResponse HC_GetEmployeesForBroadcastNotifications(SearchSBSEmployeeModel searchSBSEmployeeModel);

        #endregion

        #region AuditLog
        ServiceResponse GetAuditLogList(SearchRefAuditLogListModel searchModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId);
        #endregion

        ServiceResponse HC_SaveEmployeeDocument(HttpRequestBase currentHttpRequest);
        ServiceResponse HC_GetEmployeeDocumentList(long EmployeeID, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeleteEmployeeDocument(long EmployeeDocumentID, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);

        ServiceResponse HC_SaveEmployeeDocument(ReferralDocument referralDoc, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserID);

        ServiceResponse HC_GenerateCertificateForEmployee(long EmployeeID);


        ServiceResponse HC_GetEmployeeByUserName(string username);
    }
}
