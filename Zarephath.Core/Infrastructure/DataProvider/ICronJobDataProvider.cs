using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface ICronJobDataProvider
    {
        #region Referral CronJob

        ServiceResponse SetReferralRespiteUsageLimit();

        #endregion

        #region Send Configured Notifications CronJob
        ServiceResponse SendConfiguredNotifications(string id);
        #endregion

        #region Send Email

        ServiceResponse SendEmail();
        ServiceResponse SendScheduleReminderToParent();
        DownloadFileModel GenratePdfMonthlySummary(List<AttandanceNotificationEmailListModel> model);
        #endregion

        //#region ProcessEdi835Files

        //ServiceResponse ProcessEdi835Files();

        //#endregion

        #region  Referral List for Using AHCCID ,FirstName ,LastName,DateofBirth

        ServiceResponse GetReferralList(DMTSearchReferralListModel dmtSearchReferralListModel);

        ServiceResponse SaveReferralDocumentFile(List<ReferralDocument> referralDocument, long loggedInUserID);

        ServiceResponse SaveDMTReferralDocumentUploadStatus(
            DMTReferralDocumentUploadStatus dmtReferralDocumentUploadStatus, long loggedInUserID);



        ServiceResponse GetAdminUser(string userName);

        #endregion

        #region Delete EDI File Log using Service

        ServiceResponse DeleteEdiFileLog();

        #endregion

        #region ScheduleBatchServiceCronJob
        ServiceResponse PerformScheduleBatchServices();
        ServiceResponse SendMissingDocumentEmail();

        #endregion ScheduleBatchServiceCronJob

        #region Send Attendance Notification Email

        ServiceResponse SendAttendanceNotificationEmail();

        #endregion

        #region Send Service Plan

        ServiceResponse SendServicePlanEmail();

        #endregion


        #region Take DataBase Back UP

        ServiceResponse TakeDbBackUp();

        #endregion

        //#region Genrate Monthy Review PDF

        //ServiceResponse GenrateMonthyReviewPDF(string referralIDs);

        //#endregion



        #region Home Care 

        ServiceResponse GenerateEmployeeTimeSchedule();
        ServiceResponse SyncClaimMessages(string syncall="");
        ServiceResponse GeneratePatientTimeSchedule(int scheduleDays);
        ServiceResponse GeneratePatientTimeSchedule_ForDayCarePatient(DayCareTimeSlotModal modal, int scheduleDays, long loggedInId);
        ServiceResponse GenerateBulkSchedules();
        ServiceResponse UpdateGeoCode();


        ServiceResponse Download_Process_AllERA(bool IsThreadCall=false);

        #endregion
    }
}
