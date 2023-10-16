using System;
using System.Web.Mvc;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;


namespace Zarephath.Core.Controllers
{
    public class CronJobController : BaseController
    {
        #region Other Service
        #region Referral Respite Usage Limit CronJob

        ICronJobDataProvider _icronJobDataProvider;
        [HttpGet]
        public ActionResult SetReferralRespiteUsageLimit()
        {
            _icronJobDataProvider = new CronJobDataProvider();
            _icronJobDataProvider.SetReferralRespiteUsageLimit();
            return null;
        }

        #endregion

        #region Process Queued 835 Files CronJob

        [HttpGet]
        public ActionResult ProcessEdi835Files()
        {
            IBatchDataProvider iBatchDataProvider = new BatchDataProvider();
            iBatchDataProvider.BackEndProcessUpload835File();
            return null;
        }

        #endregion

        #region Delete EDIFileLog File using Service

        [HttpGet]
        public ActionResult DeleteEdiFileLog()
        {
            _icronJobDataProvider = new CronJobDataProvider();
            _icronJobDataProvider.DeleteEdiFileLog();
            return null;
        }

        #endregion
        #endregion

        #region Send Configured Notifications CronJob
        [HttpGet]
        public ActionResult SendConfiguredNotifications(string id)
        {
            _icronJobDataProvider = new CronJobDataProvider();
            ServiceResponse response = _icronJobDataProvider.SendConfiguredNotifications(id);
            return CustJson(response);
        }
        #endregion

        #region Clients/Parents Notifications Services

        #region Send Email CronJob
        [HttpGet]
        public ActionResult SendScheduleNotificationToParent()
        {
            _icronJobDataProvider = new CronJobDataProvider();
            _icronJobDataProvider.SendEmail();
            return null;
        }

        #endregion

        #region Send Schedule Reminder To Parent CronJob
        [HttpGet]
        public ActionResult SendScheduleReminderToParent()
        {
            _icronJobDataProvider = new CronJobDataProvider();
            _icronJobDataProvider.SendScheduleReminderToParent();
            return null;
        }

        #endregion

        #region Schedule Batch Service CronJob

        [HttpGet]
        public ActionResult PerformScheduleBatchServices()
        {
            _icronJobDataProvider = new CronJobDataProvider();
            _icronJobDataProvider.PerformScheduleBatchServices();
            return null;
        }

        #endregion

        #endregion

        #region Case Managers Notifications Services

        #region Send Missing Document Email - Record Request

        public ActionResult SendMissingDocumentEmail()
        {
            _icronJobDataProvider = new CronJobDataProvider();
            _icronJobDataProvider.SendMissingDocumentEmail();
            return null;
        }

        #endregion

        #region Send Attendance or non Attendance Notification Email / Monthly Service

        public ActionResult SendAttendanceNotificationEmail()
        {
            _icronJobDataProvider = new CronJobDataProvider();
            _icronJobDataProvider.SendAttendanceNotificationEmail();
            return null;
        }

        #endregion

        #region Service Plan Send Email

        public ActionResult SendServicePlanEmail()
        {
            //COMMENTING THIS. BECAUSE AS PER THE CLIENT UPDATE THIS WILL BE NO LONGER NEEDED.

            //_icronJobDataProvider = new CronJobDataProvider();
            //_icronJobDataProvider.SendServicePlanEmail();
            return null;
        }

        #endregion

        #endregion

        //#region Monthly Summary PDf Genrate

        //public ActionResult GenrateMonthyReviewPDF()
        //{
        //    _icronJobDataProvider = new CronJobDataProvider();
        //    _icronJobDataProvider.GenrateMonthyReviewPDF("0");
        //    return null;
        //}

        //#endregion


        #region Take DataBase Back UP


        public ActionResult TakeDbBackUp()
        {
            _icronJobDataProvider = new CronJobDataProvider();
            _icronJobDataProvider.TakeDbBackUp();
            return null;
        }




        #endregion


        public ActionResult Test01()
        {

            DateTime upcomingDate = new DateTime(2017, 10, 30);

            for (DateTime i = DateTime.Today; i <= upcomingDate; i = i.AddDays(1))
            {
                string dropOffDay = Convert.ToString(Convert.ToDateTime(i).ToString("dddd"));
                string dropOfftime = null;

                switch (dropOffDay)
                {
                    case Constants.Monday:
                        dropOfftime = DayOfWeek.Monday.ToString();
                        break;
                    case Constants.Tuesday:
                        dropOfftime = DayOfWeek.Tuesday.ToString();
                        break;
                    case Constants.Wednesday:
                        dropOfftime = DayOfWeek.Wednesday.ToString();
                        break;
                    case Constants.Thursday:
                        dropOfftime = DayOfWeek.Thursday.ToString();
                        break;
                    case Constants.Friday:
                        dropOfftime = DayOfWeek.Friday.ToString();
                        break;
                    case Constants.Saturday:
                        dropOfftime = DayOfWeek.Saturday.ToString();
                        break;
                    case Constants.Sunday:
                        dropOfftime = DayOfWeek.Sunday.ToString();
                        break;
                    default:
                        dropOfftime = "5pm";
                        break;
                }

            }

            return null;



        }


    }
}
