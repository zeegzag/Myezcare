using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Amazon.S3;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Threading;
//using static Zarephath.Core.Infrastructure.Common;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class CronJobDataProvider : BaseDataProvider, ICronJobDataProvider
    {
        public CronJobDataProvider()
        {
            SessionHelper objsession = new SessionHelper();
        }

        CacheHelper _cacheHelper = new CacheHelper();

        #region Other Service

        #region Referral Respite UsageLimit CronJob

        public ServiceResponse SetReferralRespiteUsageLimit()
        {
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.RespiteHourseLog);

            if (!ConfigSettings.Service_RespiteHoursReset_ON)
            {
                Common.CreateLogFile("Referral RespiteUsage Limit CronJob can't run because it is mark as offline from web config.", ConfigSettings.RespiteHoursFileName, logpath);
                return null;
            }


            try
            {
                Common.CreateLogFile("Referral RespiteUsage Limit CronJob Started.", ConfigSettings.RespiteHoursFileName, logpath);
                var response = new ServiceResponse();
                if ((DateTime.Now.Month == ConfigSettings.ResetRespiteUsageMonth && DateTime.Now.Day == ConfigSettings.ResetRespiteUsageDay) || ConfigSettings.CheckRespiteFlag)
                {
                    DateTime getDate;
                    if (DateTime.Now.Month >= ConfigSettings.ResetRespiteUsageMonth)
                    {
                        getDate = new DateTime(DateTime.Now.Year, ConfigSettings.ResetRespiteUsageMonth, ConfigSettings.ResetRespiteUsageDay);
                    }
                    else
                    {
                        DateTime lastYear = DateTime.Today.AddYears(-(ConfigSettings.ResetRespiteUsageDay));
                        getDate = new DateTime(lastYear.Year, ConfigSettings.ResetRespiteUsageMonth, ConfigSettings.ResetRespiteUsageDay);
                    }

                    List<SearchValueData> searchParam = new List<SearchValueData>
                            {
                                new SearchValueData {Name = "StartDate",Value =Convert.ToDateTime(getDate).ToString(Constants.DbDateFormat)},
                                new SearchValueData {Name = "EndDate",Value =Convert.ToDateTime( getDate.AddYears(ConfigSettings.ResetRespiteUsageDay).AddDays(-(ConfigSettings.ResetRespiteUsageDay))).ToString(Constants.DbDateFormat)}
                            };
                    GetScalar(StoredProcedure.InsertUpdateReferralRespiteUsageLimit, searchParam);
                }
                Common.CreateLogFile("Referral RespiteUsage Limit CronJob Completed Succesfully.", ConfigSettings.RespiteHoursFileName, logpath);
                return response;
            }
            catch (Exception ex)
            {
                Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.RespiteHoursFileName, logpath);
                throw ex;
            }
        }

        #endregion

        #region Edi835FileProcess Service are run From Batch Data Provider
        #endregion

        #region Delete EDI FIle Log

        public ServiceResponse DeleteEdiFileLog()
        {
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.DeleteEDIFileLog);
            if (!ConfigSettings.Service_DeleteEDIFileLog_ON)
            {
                Common.CreateLogFile("Delete EDIFile Log CronJob can't run because it is mark as offline from web config.", ConfigSettings.DeleteEDIFileName, logpath);
                return null;
            }


            ServiceResponse response = new ServiceResponse();

            Common.CreateLogFile("Delete EDIFile Log CronJob Started.", ConfigSettings.DeleteEDIFileName, logpath);

            var searchList = new List<SearchValueData> { new SearchValueData { Name = "Days", Value = Convert.ToString(ConfigSettings.GetDaysForDeleteEDIFileList) } };

            List<EdiFileLog> ediFileLog = GetEntityList<EdiFileLog>(StoredProcedure.GetEDIFileDeleteList, searchList);

            if (ediFileLog != null)
            {
                foreach (var fileLog in ediFileLog)
                {
                    AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                    amazonFileUpload.DeleteFile(ConfigSettings.ZarephathBucket, fileLog.FilePath);
                }
                GetScalar(StoredProcedure.DeleteEDIFileDeleteList, searchList);
                return response;
            }
            Common.CreateLogFile("Delete EDIFile Log CronJob Completed Succesfully.", ConfigSettings.DeleteEDIFileName, logpath);
            return response;
        }

        #endregion

        #endregion

        #region Send Configured Notifications CronJob
        public ServiceResponse SendConfiguredNotifications(string jobID)
        {
            var response = new ServiceResponse();
            try
            {
                var paramList = new List<SearchValueData> {
                    new SearchValueData { Name = "JobID", Value = jobID, DataType = "UNIQUEIDENTIFIER" },
                    new SearchValueData { Name = "BaseURL", Value = _cacheHelper.SiteBaseURL }
                };
                List<ConfiguredNotifications> listConfiguredNotifications = GetEntityList<ConfiguredNotifications>(StoredProcedure.GetConfiguredNotifications, paramList);
                if (listConfiguredNotifications.Count > 0)
                {
                    List<long> emailSentList = new List<long>();
                    List<long> smsSentList = new List<long>();
                    List<long> webNotifSentList = new List<long>();
                    List<long> mobileAppNotifSentList = new List<long>();
                    List<long> processedList = new List<long>();
                    foreach (ConfiguredNotifications configuredNotification in listConfiguredNotifications)
                    {
                        bool isEmailSent = true;
                        bool isSMSSent = true;
                        bool isWebNotifSent = true;
                        bool isMobileAppNotifSent = true;

                        #region SendEmail Or/And SMS

                        if (configuredNotification.EmailRecipients != null)
                        {
                            isEmailSent = Common.SendEmail(configuredNotification.EmailSubject, _cacheHelper.SupportEmail, configuredNotification.EmailRecipients, configuredNotification.EmailBody, EnumEmailType.HomeCare_Configured_Notification_Email.ToString(), orgTemplateWrapper: true);
                        }

                        if (configuredNotification.SMSRecipients != null)
                        {
                            configuredNotification.SMSRecipients.Split(';').ToList().ForEach((r) =>
                            {
                                if (!Common.SendSms(r, configuredNotification.SMSText, EnumEmailType.HomeCare_Configured_Notification_SMS.ToString()))
                                { isSMSSent = false; }
                            });
                        }

                        if (configuredNotification.WebNotificationEmployeeIds != null)
                        {
                            SendSMSModel notification = new SendSMSModel();
                            notification.EmployeeIds = configuredNotification.WebNotificationEmployeeIds;
                            notification.Message = configuredNotification.SMSText;
                            EmployeeDataProvider _employeeDataProvider = new EmployeeDataProvider();
                            var result = _employeeDataProvider.HC_SaveWebNotification(notification, SessionHelper.LoggedInID);
                            isWebNotifSent = result.IsSuccess;
                        }

                        if (configuredNotification.MobileAppNotificationEmployeeIds != null)
                        {
                            SendSMSModel notification = new SendSMSModel();
                            notification.EmployeeIds = configuredNotification.MobileAppNotificationEmployeeIds;
                            notification.Message = configuredNotification.SMSText;
                            notification.NotificationType = 2;
                            EmployeeDataProvider _employeeDataProvider = new EmployeeDataProvider();
                            var result = _employeeDataProvider.HC_SaveBroadcastNotification(notification, SessionHelper.LoggedInID, SessionHelper.DomainName);
                            isMobileAppNotifSent = result.IsSuccess;
                        }

                        #endregion

                        if (isEmailSent) { emailSentList.Add(configuredNotification.NotificationID); }
                        if (isSMSSent) { smsSentList.Add(configuredNotification.NotificationID); }
                        if (isWebNotifSent) { webNotifSentList.Add(configuredNotification.NotificationID); }
                        if (isMobileAppNotifSent) { mobileAppNotifSentList.Add(configuredNotification.NotificationID); }
                        if (isEmailSent && isSMSSent && isWebNotifSent && isMobileAppNotifSent)
                        { processedList.Add(configuredNotification.NotificationID); }
                    }

                    paramList = new List<SearchValueData> {
                        new SearchValueData { Name = "EmailSentIDs", Value = string.Join(",", emailSentList) },
                        new SearchValueData { Name = "SMSSentIDs", Value = string.Join(",", smsSentList) },
                        new SearchValueData { Name = "WebNotificationSentIDs", Value = string.Join(",", webNotifSentList) },
                        new SearchValueData { Name = "MobileAppNotificationSentIDs", Value = string.Join(",", mobileAppNotifSentList) },
                        new SearchValueData { Name = "ProcessedIDs", Value = string.Join(",", processedList) }
                    };
                    GetScalar(StoredProcedure.UpdateProcessedNotifications, paramList);
                    response.Data += $"Processed Count: {processedList.Count}, ";
                }
                response.Data += $"Total Count: {listConfiguredNotifications.Count}.";
                response.Message = "Notifications processed successsfully!";
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ErrorCode = "500";
            }
            return response;
        }
        #endregion

        #region Clients/Parents Notifications Services

        #region EmailService CronJob

        public ServiceResponse SendEmail()
        {
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.ScheduleNotificationLog);

            if (!ConfigSettings.Service_Client_ScheduleNotification_ON)
            {
                Common.CreateLogFile("Client Schedule Notification Service CronJob can't run because it is mark as offline from web config.", ConfigSettings.ScheduleNotificationFileName, logpath);
                return null;
            }


            var response = new ServiceResponse();
            try
            {
                Common.CreateLogFile("EmailService CronJob Started.", ConfigSettings.ScheduleNotificationFileName, logpath);

                ScheduleDataProvider scheduleDataProvider = new ScheduleDataProvider();
                ScheduleDetailEmailSMSParam pram = new ScheduleDetailEmailSMSParam
                {
                    IsWeekMonthFromService = true,
                    IsSendEmail = true,
                    IsSendSMS = true
                };

                scheduleDataProvider.SendScheduleDetailEmailSMS(pram);
                Common.CreateLogFile("EmailService CronJob Completed Succesfully.", ConfigSettings.ScheduleNotificationFileName, logpath);
                return response;
            }
            catch (Exception ex)
            {
                Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.ScheduleNotificationFileName, logpath);
                throw ex;
            }
        }

        public ServiceResponse SendScheduleReminderToParent()
        {
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.ScheduleReminderLog);

            if (!ConfigSettings.Service_Client_ScheduleReminder_ON)
            {
                Common.CreateLogFile("Client Schedule Reminder Service CronJob can't run because it is mark as offline from web config.", ConfigSettings.ScheduleReminderFileName, logpath);
                return null;
            }


            var response = new ServiceResponse();
            try
            {
                Common.CreateLogFile("EmailService CronJob Started.", ConfigSettings.ScheduleReminderFileName, logpath);


                #region Send Email / Sms Reminder to Clients

                if (DayOfWeek.Thursday != DateTime.Now.DayOfWeek)
                {
                    Common.CreateLogFile("Today is not Thursday. Send Email / Sms Reminder CronJob Completed.", ConfigSettings.SendMissingDocumentEmailFileName, logpath);
                    return response;
                }


                DateTime fromDate = DateTime.Now.AddDays(1);
                DateTime toDate = DateTime.Now.AddDays(3);

                var searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "FromDate", Value = fromDate.ToString(Constants.DbDateFormat)},
                    new SearchValueData {Name = "ToDate", Value = toDate.ToString(Constants.DbDateFormat)},
                    new SearchValueData {Name = "ScheduleStatus", Value = Convert.ToString((int)ScheduleStatus.ScheduleStatuses.Confirmed)},
                };

                List<ListEmailService> listEmailService = GetEntityList<ListEmailService>(StoredProcedure.GetScheduleEmailForReminder, searchList);



                if (listEmailService.Count > 0)
                {
                    EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
                                            {
                                                new SearchValueData{Name = "EmailTemplateTypeID",Value =Convert.ToInt16(EnumEmailType.Schedule_Reminder_Email).ToString(),IsEqual = true}
                                            });

                    EmailTemplate smsTemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
                                            {
                                                new SearchValueData{Name = "EmailTemplateTypeID",Value =Convert.ToInt16(EnumEmailType.Schedule_Reminder_SMS).ToString(),IsEqual = true}
                                            });

                    ScheduleDataProvider scheduleDataProvider = new ScheduleDataProvider();

                    foreach (ListEmailService emailServiceModel in listEmailService)
                    {
                        bool isSentMail = false;
                        string emailbody = "";
                        bool isSentSms = false;
                        string smSbody = "";

                        //Set Model Values To Send Mail.
                        SetReminderEmailDetailTemplateModel(emailServiceModel);

                        #region SendEmail Or/And SMS

                        //If Referral have set Email Permission and Function passed To Send Mail then it Will send Mail.
                        //if (scheduleDetailEmailSMSParam.IsSendEmail && emailServiceModel.PermissionForEmail && emailServiceModel.PCMEmail && !emailServiceModel.EmailSent)
                        if (emailServiceModel.PermissionForEmail && emailServiceModel.PCMEmail)
                        {
                            if (emailServiceModel.Email != null)
                            {

                                emailbody = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, emailServiceModel);
                                isSentMail = Common.SendEmail(emailTemplate.EmailTemplateSubject, _cacheHelper.SupportEmail, emailServiceModel.Email, emailbody, EnumEmailType.Schedule_Reminder_Email.ToString());

                                #region SAVE Schedle Notification Log
                                scheduleDataProvider.SaveScheduleNotificationLogDetails(emailServiceModel.ReferralID, emailServiceModel.ScheduleID, Constants.Email,
                                    Resource.ScheduleNotificationReminderEmail, emailbody, isSentMail,
                                    toEmail: emailServiceModel.Email, subject: emailTemplate.EmailTemplateSubject);
                                #endregion

                            }
                        }

                        //If Referral have set SMS Permission and Function passed To Send SMS then it Will send Mail.
                        //if (scheduleDetailEmailSMSParam.IsSendSMS && emailServiceModel.PermissionForSMS && emailServiceModel.PCMSMS && !emailServiceModel.SMSSent)
                        if (emailServiceModel.PermissionForSMS && emailServiceModel.PCMSMS)
                        {

                            if (emailServiceModel.Phone1 != null)
                            {
                                smSbody = TokenReplace.ReplaceTokens(smsTemplate.EmailTemplateBody, emailServiceModel);
                                isSentSms = Common.SendSms(emailServiceModel.Phone1, smSbody, EnumEmailType.Schedule_Reminder_SMS.ToString());
                            }

                            #region SAVE Schedle Notification Log

                            scheduleDataProvider.SaveScheduleNotificationLogDetails(emailServiceModel.ReferralID, emailServiceModel.ScheduleID, Constants.SMS,
                                Resource.ScheduleNotificationReminderSMS, smSbody, isSentSms,
                                toPhone: emailServiceModel.Phone1);

                            #endregion



                        }

                        #endregion

                        //Make Entry in GeneralNote For Sending mail and Sms.

                        #region AddGeneralNote

                        if (isSentMail)
                        {
                            INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                            iNoteDataProvider.SaveGeneralNote(emailServiceModel.ReferralID, emailbody, Resource.ScheduleNotificationReminderEmail, 0, emailServiceModel.ParentFirstName + " (" +
                                                              emailServiceModel.Phone1 + ")", Resource.Parent, Resource.Email);
                        }

                        if (isSentSms)
                        {
                            INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                            iNoteDataProvider.SaveGeneralNote(emailServiceModel.ReferralID, smSbody, Resource.ScheduleNotificationReminderSMS, 0, emailServiceModel.ParentFirstName + " (" +
                                                              emailServiceModel.Phone1 + ")", Resource.Parent, Resource.SMS);
                        }

                        #endregion
                    }
                }






                #endregion




                Common.CreateLogFile("EmailService CronJob Completed Succesfully.", ConfigSettings.ScheduleReminderFileName, logpath);
                return response;
            }
            catch (Exception ex)
            {
                Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.ScheduleReminderFileName, logpath);
                throw ex;
            }
        }



        private void SetReminderEmailDetailTemplateModel(ListEmailService listEmailService)
        {

            listEmailService.ZerpathLogoImage = _cacheHelper.SiteBaseURL + Constants.ZerpathLogoImage;

            #region Get Day of DropOff for base on end date

            string dropOffDay = Convert.ToString(Convert.ToDateTime(listEmailService.StartDate).ToString("dddd"));
            string dropOfftime = null;

            switch (dropOffDay)
            {
                case Constants.Monday:
                    dropOfftime = listEmailService.MondayDropOff;
                    break;
                case Constants.Tuesday:
                    dropOfftime = listEmailService.TuesdayDropOff;
                    break;
                case Constants.Wednesday:
                    dropOfftime = listEmailService.WednesdayDropOff;
                    break;
                case Constants.Thursday:
                    dropOfftime = listEmailService.ThursdayDropOff;
                    break;
                case Constants.Friday:
                    dropOfftime = listEmailService.FridayDropOff;
                    break;
                case Constants.Saturday:
                    dropOfftime = listEmailService.SaturdayDropOff;
                    break;
                case Constants.Sunday:
                    dropOfftime = listEmailService.SundayDropOff;
                    break;
                default:
                    dropOfftime = "5pm";
                    break;
            }


            #endregion

            #region Set PickUP and Dropoff

            listEmailService.DropOffTime = dropOfftime;
            listEmailService.DropOffDay = dropOffDay + " " +
                Convert.ToDateTime(listEmailService.StartDate).ToString(Constants.GlobalDateFormat).Replace("-", "/");


            #endregion
        }

        #endregion

        #region Get Referral List for Using AHCCID, FirstName, LastName, DateofBirth

        public ServiceResponse GetReferralList(DMTSearchReferralListModel dmtSearchReferralListModel)
        {
            ServiceResponse response = new ServiceResponse();

            //Common.CreateLogFile("EmailService CronJob Started.", ConfigSettings.ScheduleNotificationFileName, logpath);
            var searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "AHCCCSID", Value = Convert.ToString(dmtSearchReferralListModel.AHCCCSID)},
                    //new SearchValueData {Name = "FirstName", Value = Convert.ToString(dmtSearchReferralListModel.FirstName)},
                    //new SearchValueData {Name = "LastName", Value = Convert.ToString(dmtSearchReferralListModel.LastName)},
                    //new SearchValueData {Name = "Dob", Value = dmtSearchReferralListModel.Dob.ToString(Constants.DbDateFormat)},
                };

            DMTReferralList referralList = GetEntity<DMTReferralList>(StoredProcedure.DMTGetReferralData, searchList);

            if (referralList != null)
            {
                response.IsSuccess = true;
                response.Data = referralList;
            }
            return response;
        }

        public ServiceResponse SaveReferralDocumentFile(List<ReferralDocument> referralDocument, long loggedInUserID)
        {
            //Convert.ToString(DocumentType.DocumentKind.Internal),
            //(int)DocumentType.DocumentTypes.Other,
            ServiceResponse response = new ServiceResponse();

            if (referralDocument != null)
            {
                foreach (var documentmodel in referralDocument)
                {
                    SaveObject(documentmodel, loggedInUserID);
                }
                response.IsSuccess = true;
            }
            return response;
        }

        public ServiceResponse GetAdminUser(string userName)
        {
            ServiceResponse response = new ServiceResponse();

            Employee employee = GetEntity<Employee>(new List<SearchValueData>
                {
                    new SearchValueData
                        {
                            Name = "UserName",
                            Value = userName
                        }
                });

            if (employee != null)
            {
                response.IsSuccess = true;
                response.Data = employee;
            }
            return response;
        }

        public ServiceResponse SaveDMTReferralDocumentUploadStatus(DMTReferralDocumentUploadStatus dmtReferralDocumentUploadStatus, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            if (dmtReferralDocumentUploadStatus != null)
            {
                ReferralDocumentUploadStatus referralDocumentUploadStatus = new ReferralDocumentUploadStatus();

                referralDocumentUploadStatus.ReferralID = dmtReferralDocumentUploadStatus.ReferralID;
                referralDocumentUploadStatus.AHCCCSID = dmtReferralDocumentUploadStatus.AHCCCSID;
                referralDocumentUploadStatus.UploadStatus = dmtReferralDocumentUploadStatus.UploadStatus;
                response = SaveObject(referralDocumentUploadStatus, loggedInUserID);
            }
            return response;
        }

        #endregion

        #region ScheduleBatchServiceCronJob

        public ServiceResponse PerformScheduleBatchServices()
        {
            ScheduleDataProvider scheduleDataProvider = new ScheduleDataProvider();

            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.ScheduleBatchServicesLog);
            if (!ConfigSettings.Service_Client_ScheduleBatchNotification_ON)
            {
                Common.CreateLogFile("Client Schedule Batch Notification Service CronJob can't run because it is mark as offline from web config.", ConfigSettings.ScheduleBatchServicesFileName, logpath);
                return null;
            }


            var response = new ServiceResponse();
            try
            {

                Common.CreateLogFile("ScheduleBatchServices CronJob Started.", ConfigSettings.ScheduleBatchServicesFileName, logpath);

                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "GETScheduleBatchServiceStatus", Value = ScheduleBatchService.ScheduleBatchServiceStatuses.Initiated.ToString() });
                searchParam.Add(new SearchValueData { Name = "SETScheduleBatchServiceStatus", Value = ScheduleBatchService.ScheduleBatchServiceStatuses.InProgress.ToString() });
                List<ScheduleBatchService> scheduleBatchService = GetEntityList<ScheduleBatchService>(StoredProcedure.GetScheduleBatchServiceForProcess, searchParam);
                foreach (ScheduleBatchService batchService in scheduleBatchService)
                {
                    ServiceResponse resp = null;

                    if (batchService.ScheduleBatchServiceType == (int)ScheduleBatchService.ScheduleBatchServiceTypes.GenerateMailNotice)
                    {
                        resp = scheduleDataProvider.PrintScheduleNotice(batchService.ScheduleIDs, true, batchService.CreatedBy);
                    }
                    else
                    {
                        ScheduleDetailEmailSMSParam pram = new ScheduleDetailEmailSMSParam
                        {
                            IsWeekMonthFromService = false,
                            ScheduleIds = batchService.ScheduleIDs,
                            CreatedBy = batchService.CreatedBy
                        };
                        if (batchService.ScheduleBatchServiceType == (int)ScheduleBatchService.ScheduleBatchServiceTypes.SendEmail)
                        {
                            pram.IsSendEmail = true;
                        }
                        if (batchService.ScheduleBatchServiceType == (int)ScheduleBatchService.ScheduleBatchServiceTypes.SendSMS)
                        {
                            pram.IsSendSMS = true;
                        }
                        resp = scheduleDataProvider.SendScheduleDetailEmailSMS(pram, true);
                    }

                    if (resp.IsSuccess)
                    {
                        if (batchService.ScheduleBatchServiceType ==
                            (int)ScheduleBatchService.ScheduleBatchServiceTypes.GenerateMailNotice)
                        {
                            DownloadFileModel file = (DownloadFileModel)resp.Data;

                            string fileKey = ConfigSettings.AmazoneUploadPath +
                                             ConfigSettings.RespiteNoticePrintFilePath + file.FileName;
                            AmazonFileUpload fileuploader = new AmazonFileUpload();
                            fileuploader.UploadFile(ConfigSettings.ZarephathBucket, fileKey, file.AbsolutePath, true);
                            batchService.FilePath = fileKey;
                        }
                        batchService.ScheduleBatchServiceStatus =
                            ScheduleBatchService.ScheduleBatchServiceStatuses.Completed.ToString();
                        SaveEntity(batchService);
                    }
                    else
                    {
                        batchService.ScheduleBatchServiceStatus =
                            ScheduleBatchService.ScheduleBatchServiceStatuses.Failed.ToString();
                        SaveEntity(batchService);
                    }
                }

                Common.CreateLogFile("ScheduleBatchServices Cronob Completed Succesfully.", ConfigSettings.ScheduleNotificationFileName, logpath);
                return response;
            }
            catch (Exception ex)
            {
                Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.ScheduleNotificationFileName, logpath);
                throw ex;
            }
        }

        #endregion ScheduleBatchServiceCronJob

        #endregion

        #region Case Managers Notifications Services

        #region SendMissingDocumentEmail

        public ServiceResponse SendMissingDocumentEmail12()
        {
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.SendMissingDocumentEmailLog);

            if (!ConfigSettings.Service_CM_Compliance_ON)
            {
                Common.CreateLogFile("CM Compliance Service CronJob can't run because it is mark as offline from web config.", ConfigSettings.SendMissingDocumentEmailFileName, logpath);
                return null;
            }


            var response = new ServiceResponse();
            try
            {
                Common.CreateLogFile("MissingDocumentEmail CronJob Started.", ConfigSettings.SendMissingDocumentEmailFileName, logpath);
                DateTime lastdateofcurrentmonth = Common.GetLastDayOfMonth(DateTime.Now);
                if (lastdateofcurrentmonth.Date != DateTime.Now.Date)
                {
                    Common.CreateLogFile("Today is not Last day of month. MissingDocumentEmail CronJob Completed.", ConfigSettings.SendMissingDocumentEmailFileName, logpath);
                    return response;
                }

                #region Get active referral list and group by requestmail

                var searchlist = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "AgencyROI", Value = Convert.ToString(Constants.AgencyROI)},
                        new SearchValueData
                            {
                                Name = "NetworkServicePlan",
                                Value = Convert.ToString(Constants.NetworkServicePlan)
                            },
                        new SearchValueData {Name = "BXAssessment", Value = Convert.ToString(Constants.BXAssessment)},
                        new SearchValueData {Name = "Demographic", Value = Convert.ToString(Constants.Demographic)},
                        new SearchValueData {Name = "SNCD", Value = Convert.ToString(Constants.SNCD)},
                        new SearchValueData{Name = "NetworkCrisisPlan",Value = Convert.ToString(Constants.NetworkCrisisPlan)},
                        new SearchValueData {Name = "CAZOnly", Value = Convert.ToString(Constants.CAZOnly)},
                        new SearchValueData {Name = "Missing", Value = Convert.ToString(Constants.Missing)},
                        new SearchValueData {Name = "Expired", Value = Convert.ToString(Constants.Expired)}
                    };
                List<ReferralListForMissingDocumentEmail> missingDocumentListModel =
                    GetEntityList<ReferralListForMissingDocumentEmail>(
                        StoredProcedure.GetReferralsForMissingDocumentMail, searchlist);

                #endregion Get active referral list and group by requestmail

                #region get email templates

                string htmlString = "";

                List<EmailWiseReferralList> groupedbyMail =
                    (from referralListForMissingDocumentEmail in missingDocumentListModel
                     group referralListForMissingDocumentEmail by
                         referralListForMissingDocumentEmail.RecordRequestEmail
                         into g
                     select new EmailWiseReferralList { RecordRequestEmail = g.Key, ReferralList = g.ToList() }).ToList();



                EmailTemplate missingDocTemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
                    {
                        new SearchValueData
                            {
                                Name = "EmailTemplateTypeID",
                                Value = Convert.ToInt16(EnumEmailType.Missing_Expire_Document_Template).ToString(),
                                IsEqual = true
                            }
                    });

                EmailTemplate nonMissingDocTemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
                    {
                        new SearchValueData
                            {
                                Name = "EmailTemplateTypeID",
                                Value = Convert.ToInt16(EnumEmailType.NonMissing_Expire_Document_Template).ToString(),
                                IsEqual = true
                            }
                    });

                #endregion #region get email templates

                #region Send Mail to all

                foreach (EmailWiseReferralList referralgroup in groupedbyMail)
                {
                    if (string.IsNullOrEmpty(referralgroup.RecordRequestEmail))
                    {
                        continue;
                    }

                    #region Send Missing Document detail mail

                    //Send Mail for client list with Missing document details
                    if (referralgroup.ReferralList.Any(m => !string.IsNullOrEmpty(m.MissingDocumentDetails)))
                    {
                        MissingDocumentEmailTokenModel emailTokenModel = new MissingDocumentEmailTokenModel();

                        emailTokenModel.ClientList = GetClientListForMissingDocumentMail(referralgroup.ReferralList.Where(m => !string.IsNullOrEmpty(m.MissingDocumentDetails)).ToList(), true);

                        emailTokenModel.CaseManager = referralgroup.ReferralList.First().CaseManager;
                        string emailbody = TokenReplace.ReplaceTokens(missingDocTemplate.EmailTemplateBody, emailTokenModel);
                        bool isSentMail = Common.SendEmail(missingDocTemplate.EmailTemplateSubject, _cacheHelper.SupportEmail, referralgroup.RecordRequestEmail,
                                                           emailbody, EnumEmailType.Missing_Expire_Document_Template.ToString(),
                                                           ConfigSettings.CCEmailAddress, (int)EmailHelper.SMTPSetting.EncryptedEmailSetting);
                        if (isSentMail)
                        {
                            //Enter in notes for All  referral
                            foreach (var referral in referralgroup.ReferralList.Where(m => !string.IsNullOrEmpty(m.MissingDocumentDetails)).ToList())
                            {
                                INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                                iNoteDataProvider.SaveGeneralNote(referral.ReferralID, emailbody, Resource.ServiceMissingDocumentEmail, 0,
                                                                  referralgroup.RecordRequestEmail, Resource.CaseManager, Resource.Email);
                            }
                        }

                    }

                    #endregion Send Missing Document detail mail

                    #region Send Active client list mail who have proper document

                    //Send Active ClientList  who have proper document
                    if (referralgroup.ReferralList.Any(m => string.IsNullOrEmpty(m.MissingDocumentDetails)))
                    {
                        MissingDocumentEmailTokenModel emailTokenModel = new MissingDocumentEmailTokenModel();

                        emailTokenModel.ClientList = GetClientListForMissingDocumentMail(referralgroup.ReferralList.Where(m => string.IsNullOrEmpty(m.MissingDocumentDetails)).ToList(), false);
                        emailTokenModel.CaseManager = referralgroup.ReferralList.First().CaseManager;

                        string emailbody = TokenReplace.ReplaceTokens(nonMissingDocTemplate.EmailTemplateBody, emailTokenModel);
                        bool isSentMail = Common.SendEmail(nonMissingDocTemplate.EmailTemplateSubject, _cacheHelper.SupportEmail,
                                                           referralgroup.RecordRequestEmail, emailbody, EnumEmailType.NonMissing_Expire_Document_Template.ToString(),
                                                           ConfigSettings.CCEmailAddress, (int)EmailHelper.SMTPSetting.EncryptedEmailSetting);
                        //Enter notes for All Referral
                        if (isSentMail)
                        {
                            foreach (var referral in referralgroup.ReferralList.Where(m => string.IsNullOrEmpty(m.MissingDocumentDetails)).ToList())
                            {
                                INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                                iNoteDataProvider.SaveGeneralNote(referral.ReferralID, emailbody, Resource.ServiceMissingDocumentEmail, 0, referralgroup.RecordRequestEmail,
                                                                  Resource.CaseManager, Resource.Email);
                            }
                        }
                    }

                    #endregion Send Active client list mail who have proper document
                }

                #endregion Send Mail to all

                Common.CreateLogFile("MissingDocumentEmail CronJob Completed.", ConfigSettings.SendMissingDocumentEmailFileName, logpath);
                return response;
            }
            catch (Exception ex)
            {
                Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.SendMissingDocumentEmailFileName, logpath);
                throw ex;
            }
        }



        public ServiceResponse SendMissingDocumentEmail()
        {
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.SendMissingDocumentEmailLog);

            if (!ConfigSettings.Service_CM_Compliance_ON)
            {
                Common.CreateLogFile("CM Compliance Service CronJob can't run because it is mark as offline from web config.", ConfigSettings.SendMissingDocumentEmailFileName, logpath);
                return null;
            }

            var response = new ServiceResponse();
            try
            {
                Common.CreateLogFile("MissingDocumentEmail CronJob Started.", ConfigSettings.SendMissingDocumentEmailFileName, logpath);
                //DateTime lastdateofcurrentmonth = Common.GetLastDayOfMonth(DateTime.Now); new DateTime(dateTime.Year, dateTime.Month, i);
                DateTime lastdateofcurrentmonth = Common.GetLastBusinessDayOfMonth(DateTime.Now);

                if (lastdateofcurrentmonth.Date != DateTime.Now.Date)
                {
                    Common.CreateLogFile("Today is not Last day of month. MissingDocumentEmail CronJob Completed.", ConfigSettings.SendMissingDocumentEmailFileName, logpath);
                    return response;
                }

                #region Get active referral list and group by requestmail

                var searchlist = new List<SearchValueData>
                    {
                        //new SearchValueData {Name = "AgencyROI", Value = Convert.ToString(Constants.AgencyROI)},
                        //new SearchValueData{Name = "NetworkServicePlan",Value = Convert.ToString(Constants.NetworkServicePlan)},
                        //new SearchValueData {Name = "BXAssessment", Value = Convert.ToString(Constants.BXAssessment)},
                        //new SearchValueData {Name = "Demographic", Value = Convert.ToString(Constants.Demographic)},
                        //new SearchValueData {Name = "SNCD", Value = Convert.ToString(Constants.SNCD)},
                        //new SearchValueData{Name = "NetworkCrisisPlan",Value = Convert.ToString(Constants.NetworkCrisisPlan)},
                        //new SearchValueData {Name = "CAZOnly", Value = Convert.ToString(Constants.CAZOnly)},
                        new SearchValueData {Name = "Yes", Value = Convert.ToString(Constants.Yes)},
                        new SearchValueData {Name = "NotApplicable", Value = Convert.ToString(Constants.NotApplicable)},
                        new SearchValueData {Name = "Missing", Value = Convert.ToString(Constants.Missing)},
                        new SearchValueData {Name = "Expired", Value = Convert.ToString(Constants.Expired)}
                    };
                List<ReferralListForMissingDocumentEmail> missingDocumentListModel = GetEntityList<ReferralListForMissingDocumentEmail>(StoredProcedure.GetReferralsForMissingDocumentMail, searchlist);

                #endregion Get active referral list and group by requestmail

                #region get email templates

                List<EmailWiseReferralList> groupedbyMail =
                    (from referralListForMissingDocumentEmail in missingDocumentListModel
                     group referralListForMissingDocumentEmail by referralListForMissingDocumentEmail.RecordRequestEmail into g
                     select new EmailWiseReferralList { RecordRequestEmail = g.Key, ReferralList = g.ToList() }).ToList();

                EmailTemplate missingDocTemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
                    {
                        new SearchValueData{Name = "EmailTemplateTypeID",Value = Convert.ToInt16(EnumEmailType.Missing_Expire_Document_Template).ToString(),IsEqual = true}
                    });

                EmailTemplate nonMissingDocTemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
                    {
                        new SearchValueData{Name = "EmailTemplateTypeID",Value = Convert.ToInt16(EnumEmailType.NonMissing_Expire_Document_Template).ToString(),IsEqual = true}
                    });

                #endregion #region get email templates

                #region Send Mail to all

                string failedMissingLogs = "", passedMissingLogs = "", failedActiveLogs = "", passedActiveLogs = "";

                foreach (EmailWiseReferralList referralgroup in groupedbyMail)
                {
                    if (string.IsNullOrEmpty(referralgroup.RecordRequestEmail))
                        continue;

                    #region Send Missing Document detail mail


                    List<ReferralListForMissingDocumentEmail> missingExpireDocRef = referralgroup.ReferralList.Where(c => !c.ServicePlanExpirationStatus || !c.SPGuardianSignatureStatus || !c.SPBHPSignatureStatus
                        || !c.BXAssessmentExpirationStatus || !c.BXAssessmentBHPSignedStatus || !c.SNCDStatus).ToList();  //|| !c.DemographicStatus || !c.ROIExpirationDateStatus

                    //Send Mail for client list with Missing document details
                    if (missingExpireDocRef.Any())
                    {
                        MissingDocumentEmailTokenModel emailTokenModel = new MissingDocumentEmailTokenModel();

                        //List<ReferralListForMissingDocumentEmail> referralList = referralgroup.ReferralList.Where(m => !m.SPGuardianSignatureStatus
                        //   || !m.SPBHPSignatureStatus || !m.BXAssessmentExpirationStatus || !m.DemographicStatus || !m.ROIExpirationDateStatus).ToList();

                        //StringBuilder str = new StringBuilder();
                        StringBuilder mainStringBuilder = new StringBuilder();
                        StringBuilder tempStringBuilder = new StringBuilder();
                        #region Set value and genrate design for client list with Missing document details

                        mainStringBuilder.Append("<table style='width: 100%; padding-top:30px; border: 1px solid ; border-collapse: collapse;' cellpadding='4' cellspacing='3'>");

                        mainStringBuilder.Append("<tbody>" +
                                   "<tr style='border: 1px solid;'>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1' >Last Name</th>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>First Name</th>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>Birth Date</th>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>AHCCCS ID</th>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>Case Manager</th>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>Service Plan Expiration Date</th>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>SP Guardian Signature</th>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>SP BHP Signature</th>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>SP Identify Respite</th>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>BX Assessment Expiration Date</th>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>BX Assessment BHP Signature</th>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>Strengths Needs And Cultural Discovery</th>" +
                                   //"<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>Demographic</th>" +
                                   //"<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>ROI Expiration Date</th>" +
                                   "</tr>");

                        tempStringBuilder = new StringBuilder(mainStringBuilder.ToString());
                        foreach (var item in missingExpireDocRef)
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append("<tr style='border: 1px solid #ddd'>");
                            str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'> " + item.LastName + "</td>");
                            str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.FirstName + "</td>");
                            str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.Dob.ToString(Constants.GlobalDateFormat, CultureInfo.InvariantCulture) + "</td>");
                            str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.AHCCCSID + "</td>");
                            str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.CaseManager + "</td>");

                            #region Set Color

                            if (!item.ServicePlanExpirationStatus)
                                str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center; color:red;' colspban='1'>" + item.ServicePlanExpirationDate + "</td>");
                            else
                                str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspban='1'>" + item.ServicePlanExpirationDate + "</td>");

                            if (!item.SPGuardianSignatureStatus)
                                str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;color:red;'' colspan='1'>" + item.SPGuardianSignature + " </td>");
                            else
                                str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.SPGuardianSignature + " </td>");

                            if (!item.SPBHPSignatureStatus)
                                str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center; color:red;'' colspan='1'>" + item.SPBHPSignature + " </td>");
                            else
                                str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.SPBHPSignature + " </td>");
                            if (!item.SPIdentifyStatus)
                                str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center; color:red;'' colspan='1'>" + item.SPIdentify + " </td>");
                            else
                                str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.SPIdentify + " </td>");

                            if (!item.BXAssessmentExpirationStatus)
                                str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;color:red;'' colspan='1'>" + item.BXAssessmentExpirationDate + " </td>");
                            else
                                str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.BXAssessmentExpirationDate + " </td>");
                            if (!item.BXAssessmentBHPSignedStatus)
                                str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;color:red;'' colspan='1'>" + item.BXAssessmentBHPSigned + " </td>");
                            else
                                str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.BXAssessmentBHPSigned + " </td>");

                            if (!item.SNCDCompletionDateStatus)
                                str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;color:red;'' colspan='1'>" + item.SNCDCompletionDate + " </td>");
                            else
                                str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.SNCDCompletionDate + " </td>");

                            //if (!item.DemographicStatus)
                            //    str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;color:red;'' colspan='1'>" + item.Demographic + " </td>");
                            //else
                            //    str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.Demographic + " </td>");

                            //if (!item.ROIExpirationDateStatus)
                            //    str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;color:red;'' colspan='1'>" + item.ROIExpirationDate + " </td>");
                            //else
                            //    str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.ROIExpirationDate + " </td>");

                            #endregion

                            str.Append("</tr>");

                            mainStringBuilder.Append(str);

                            #region Individual Email Template For Note Log

                            StringBuilder builder = new StringBuilder(tempStringBuilder.ToString());
                            builder.Append(str);
                            builder.Append("</tbody>");
                            builder.Append("</table>");
                            MissingDocumentEmailTokenModel tokens = new MissingDocumentEmailTokenModel();
                            tokens.ClientList = builder.ToString();
                            item.SentEmailTemplate = TokenReplace.ReplaceTokens(missingDocTemplate.EmailTemplateBody, tokens);

                            #endregion
                        }

                        mainStringBuilder.Append("</tbody>");
                        mainStringBuilder.Append("</table>");

                        #endregion

                        emailTokenModel.ClientList = mainStringBuilder.ToString();

                        //emailTokenModel.CaseManager = referralgroup.ReferralList.First().CaseManager;
                        string emailbody = TokenReplace.ReplaceTokens(missingDocTemplate.EmailTemplateBody, emailTokenModel);
                        bool isSentMail = Common.SendEmail(missingDocTemplate.EmailTemplateSubject, _cacheHelper.SupportEmail, referralgroup.RecordRequestEmail,
                                                           emailbody, EnumEmailType.Missing_Expire_Document_Template.ToString(),
                                                           ConfigSettings.RecordCCEmailAddress, (int)EmailHelper.SMTPSetting.EncryptedEmailSetting);
                        if (isSentMail)
                        {
                            //Enter in notes for All  referral
                            foreach (var referral in missingExpireDocRef)
                            {
                                INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                                iNoteDataProvider.SaveGeneralNote(referral.ReferralID, referral.SentEmailTemplate,
                                    Resource.ServiceMissingDocumentEmail, 0,
                                    referralgroup.RecordRequestEmail, Resource.CaseManager, Resource.Email);
                            }
                            passedMissingLogs = passedMissingLogs + string.Format("{0}{1}", referralgroup.RecordRequestEmail, Environment.NewLine);
                        }
                        else
                        {
                            failedMissingLogs = failedMissingLogs + string.Format("{0}{1}", referralgroup.RecordRequestEmail, Environment.NewLine);
                        }
                    }



                    #endregion Send Missing Document detail mail

                    #region Send Active client list mail who have proper document

                    List<ReferralListForMissingDocumentEmail> properdocumentreferralList = referralgroup.ReferralList.Where(c => c.ServicePlanExpirationStatus && c.SPGuardianSignatureStatus && c.SPBHPSignatureStatus
                       && c.BXAssessmentExpirationStatus && c.BXAssessmentBHPSignedStatus && c.SNCDStatus).ToList();//&& c.DemographicStatus && c.ROIExpirationDateStatus



                    //Send Active ClientList  who have proper document
                    if (properdocumentreferralList.Any())
                    {
                        MissingDocumentEmailTokenModel emailTokenModel = new MissingDocumentEmailTokenModel();

                        //List<ReferralListForMissingDocumentEmail> properdocumentreferralList = referralgroup.ReferralList.Where(m => m.SPGuardianSignatureStatus
                        //   && m.SPBHPSignatureStatus && m.BXAssessmentExpirationStatus && m.DemographicStatus && m.ROIExpirationDateStatus).ToList();


                        StringBuilder mainStringBuilder = new StringBuilder();
                        StringBuilder tempStringBuilder = new StringBuilder();

                        #region Set value and genrate design ClientList  who have proper document

                        mainStringBuilder.Append("<table style='width: 100%; padding-top:30px; border: 1px solid ; border-collapse: collapse;' cellpadding='4' cellspacing='3'>");

                        mainStringBuilder.Append("<tbody>" +
                                   "<tr style='border: 1px solid;'>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1' >Last Name</th>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>First Name</th>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>Birth Date</th>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>AHCCCS ID</th>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>Case Manager</th>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>Service Plan Expiration Date</th>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>BX Assessment Expiration Date</th>" +
                                   "<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>Strengths Needs And Cultural Discovery</th>" +
                                   //"<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>Demographic</th>" +
                                   //"<th style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>ROI Expiration Date</th>" +
                                   "</tr>");
                        tempStringBuilder = new StringBuilder(mainStringBuilder.ToString());
                        foreach (var item in properdocumentreferralList)
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append("<tr style='border: 1px solid #ddd'>");
                            str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'> " + item.LastName + "</td>");
                            str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.FirstName + "</td>");
                            str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.Dob.ToString(Constants.GlobalDateFormat, CultureInfo.InvariantCulture) + "</td>");
                            str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.AHCCCSID + "</td>");
                            str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.CaseManager + "</td>");
                            str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspban='1'>" + item.ServicePlanExpirationDate + "</td>");
                            str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.BXAssessmentExpirationDate + " </td>");
                            str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.SNCDCompletionDate + " </td>");
                            //str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.Demographic + " </td>");
                            //str.Append("<td style='font-size: 10px; border: 1px solid #000000;text-align: center;' colspan='1'>" + item.ROIExpirationDate + " </td>");
                            str.Append("</tr>");

                            mainStringBuilder.Append(str);

                            #region Individual Email Template For Note Log

                            StringBuilder builder = new StringBuilder(tempStringBuilder.ToString());
                            builder.Append(str);
                            builder.Append("</tbody>");
                            builder.Append("</table>");
                            MissingDocumentEmailTokenModel tokens = new MissingDocumentEmailTokenModel();
                            tokens.ClientList = builder.ToString();
                            item.SentEmailTemplate = TokenReplace.ReplaceTokens(nonMissingDocTemplate.EmailTemplateBody, tokens);

                            #endregion

                        }

                        mainStringBuilder.Append("</tbody>");
                        mainStringBuilder.Append("</table>");

                        #endregion

                        emailTokenModel.ClientList = mainStringBuilder.ToString();
                        //emailTokenModel.ClientList = GetClientListForMissingDocumentMail(referralgroup.ReferralList.Where(m => m.SPGuardianSignatureStatus
                        //   && m.SPBHPSignatureStatus && m.BXAssessmentExpirationStatus && m.DemographicStatus && m.ROIExpirationDateStatus).ToList(), false);

                        emailTokenModel.CaseManager = referralgroup.ReferralList.First().CaseManager;

                        string emailbody = TokenReplace.ReplaceTokens(nonMissingDocTemplate.EmailTemplateBody, emailTokenModel);
                        bool isSentMail = Common.SendEmail(nonMissingDocTemplate.EmailTemplateSubject, _cacheHelper.SupportEmail,
                                                           referralgroup.RecordRequestEmail, emailbody, EnumEmailType.NonMissing_Expire_Document_Template.ToString(),
                                                           ConfigSettings.RecordCCEmailAddress, (int)EmailHelper.SMTPSetting.EncryptedEmailSetting);
                        //Enter notes for All Referral
                        if (isSentMail)
                        {
                            foreach (var referral in properdocumentreferralList)
                            {
                                INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                                iNoteDataProvider.SaveGeneralNote(referral.ReferralID, referral.SentEmailTemplate,
                                    Resource.ServiceNonMissingDocumentEmail, 0, referralgroup.RecordRequestEmail,
                                    Resource.CaseManager, Resource.Email);
                            }
                            passedActiveLogs = passedActiveLogs +
                                                string.Format("{0}{1}", referralgroup.RecordRequestEmail,
                                                    Environment.NewLine);
                        }
                        else
                        {
                            failedActiveLogs = failedActiveLogs + string.Format("{0}{1}", referralgroup.RecordRequestEmail, Environment.NewLine);
                        }
                    }

                    #endregion Send Active client list mail who have proper document
                }

                #endregion Send Mail to all

                #region Send Logs Details To Alberto
                if (!string.IsNullOrEmpty(failedMissingLogs) || !string.IsNullOrEmpty(failedActiveLogs))
                {
                    string message =
                        string.Format(//"Record Request has been sent successfully on following Email address.{0}{1}{0}{0}" +
                                      "Record Request email sent failed on following Email address.{0}{2}{0}{0}" +
                                      //"Record Update has been sent successfully on following Email address.{0}{3}{0}{0}" +
                                      "Record Update email sent failed on following Email address.{0}{4}{0}{0}",
                                      Environment.NewLine,
                                      string.IsNullOrEmpty(passedMissingLogs) ? "No Email Address Found" : passedMissingLogs,
                                      string.IsNullOrEmpty(failedMissingLogs) ? "No Email Address Found" : failedMissingLogs,
                                      string.IsNullOrEmpty(passedActiveLogs) ? "No Email Address Found" : passedActiveLogs,
                                      string.IsNullOrEmpty(failedActiveLogs) ? "No Email Address Found" : failedActiveLogs);

                    string emailMsg = message.Replace(Environment.NewLine, "<br/>");
                    string virtualPath = Common.CreateLogFile(message, "log_details_" + DateTime.Now.Ticks + ".txt", logpath);
                    List<string> attachement = new List<string> { HttpContext.Current.Server.MapCustomPath(virtualPath) };
                    Common.SendEmail("Record Request Log Details", "", ConfigSettings.RecordLogEmailAddress, emailMsg, "", ConfigSettings.RecordCCEmailAddress, 1, attachement);
                }
                #endregion



                Common.CreateLogFile("MissingDocumentEmail CronJob Completed.", ConfigSettings.SendMissingDocumentEmailFileName, logpath);
                return response;
            }
            catch (Exception ex)
            {
                Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.SendMissingDocumentEmailFileName, logpath);
                throw ex;
            }
        }



        public string GetClientListForMissingDocumentMail(List<ReferralListForMissingDocumentEmail> referralList, bool isWithMissingDocument)
        {
            string html = "<ul>";


            #region Missing Document
            string clientli = "<li><b>{0} (#{2}, DOB : {1})</b></li>";
            string clientWithDocDtlli = "<li><b>{0} (#{2}, DOB : {1})</b><br/> {3}</li>";
            #endregion
            foreach (var item in referralList)
            {
                if (isWithMissingDocument)
                {
                    string documentstring = FormatMissingDocumentString(item.MissingDocumentDetails);

                    html += string.Format(clientWithDocDtlli, Common.GetGeneralNameFormat(item.FirstName, item.LastName),
                                          item.Dob.ToString(Constants.GlobalDateFormat, CultureInfo.InvariantCulture), item.AHCCCSID, documentstring);
                }
                else
                {
                    html += string.Format(clientli, Common.GetGeneralNameFormat(item.FirstName, item.LastName),
                                         item.Dob.ToString(Constants.GlobalDateFormat, CultureInfo.InvariantCulture), item.AHCCCSID);
                }
            }
            html += "</ul>";

            return html;
        }

        public string FormatMissingDocumentString(string missingDocumentString)
        {

            List<MissingDocumentListModel> list =
                missingDocumentString.Split(new string[] { "##" }, StringSplitOptions.RemoveEmptyEntries).Where(m => !string.IsNullOrEmpty(m)).Select(m =>
                                                                                              new MissingDocumentListModel
                                                                                              {
                                                                                                  MissingDocumentType =
                                                                                                       m.Split(':')[0],
                                                                                                  MissingDocumentName =
                                                                                                       m.Split(':')[1]
                                                                                              }
                    ).ToList();



            string formatstr = "";

            if (list.Any(m => m.MissingDocumentType == Constants.Missing))
            {
                formatstr += string.Format("<b>{0}</b> : {1}<br>", Constants.Missing, string.Join(", ", list.Where(m => m.MissingDocumentType == Constants.Missing).Select(m => m.MissingDocumentName)));
            }

            if (list.Any(m => m.MissingDocumentType == Constants.Expired))
            {
                formatstr += string.Format("<b>{0}</b> : {1}<br>", Constants.Expired, string.Join(", ", list.Where(m => m.MissingDocumentType == Constants.Expired).Select(m => m.MissingDocumentName)));
            }
            formatstr += "<br/>";
            return formatstr;
        }

        #endregion ScheduleBatchServiceCronJob

        #region Send Attendance Notification Email  / Monthly Service

        public ServiceResponse SendAttendanceNotificationEmail()
        {
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.AttendanceNotificationLog);
            if (!ConfigSettings.Service_CM_Attendance_ON)
            {
                Common.CreateLogFile("CM Attendance Service CronJob can't run because it is mark as offline from web config.", ConfigSettings.AttendanceNotificationFileName, logpath);
                return null;
            }

            var response = new ServiceResponse();

            try
            {
                Common.CreateLogFile("Attendance Notification Email CronJob Started.", ConfigSettings.AttendanceNotificationFileName, logpath);

                #region Check For month Wise

                DateTime currentMonth7Th = Common.GetDayOfMonth(DateTime.Now, 7);

                if (currentMonth7Th.Date != DateTime.Now.Date)
                {
                    Common.CreateLogFile("Today is not 7th of the current month. Attendance Notification Email CronJob Completed.", ConfigSettings.AttendanceNotificationFileName, logpath);
                    return response;
                }

                #endregion

                #region Check For Day For Half month or Full Month

                //DateTime baseDate = DateTime.Today;
                //int day = baseDate.Day;

                //var fromdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                //var todate = fromdate.AddMonths(1).AddDays(-1);

                //var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                //var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);

                //if (day >= 15)
                //{
                //    fromdate = thisMonthStart;
                //    todate = thisMonthEnd;
                //}
                //else if (day <= 15)
                //{
                //    fromdate = thisMonthStart.AddMonths(-1);
                //    todate = thisMonthStart.AddSeconds(-1);
                //}

                #endregion

                #region Get Email templates

                EmailTemplate attendanceNotification = GetEntity<EmailTemplate>(new List<SearchValueData>
                    {
                        new SearchValueData
                            {
                                Name = "EmailTemplateTypeID",
                                Value = Convert.ToInt16(EnumEmailType.Attendance_Notification).ToString(),
                                IsEqual = true
                            }
                    });

                EmailTemplate noneAttendanceNotification = GetEntity<EmailTemplate>(new List<SearchValueData>
                    {
                        new SearchValueData
                            {
                                Name = "EmailTemplateTypeID",
                                Value = Convert.ToInt16(EnumEmailType.None_Attendance_Notification).ToString(),
                                IsEqual = true
                            }
                    });

                #endregion get email templates

                #region Attand Scheudle list

                var month = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var fromdate = month.AddMonths(-1);
                var todate = month.AddDays(-1);

                var searchlist = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "FromDate", Value = Convert.ToDateTime(fromdate).ToString(Constants.DbDateFormat)},
                        new SearchValueData {Name = "ToDate", Value = Convert.ToDateTime(todate).ToString(Constants.DbDateFormat)},
                        new SearchValueData {
                            Name = "ReferralStatusIDs", Value =Convert.ToString((int)Common.ReferralStatusEnum.Active)
                        },
                        new SearchValueData {
                            Name = "PayorIDs", Value =
                            Convert.ToString( (int)Payor.PayorCode.CAZ + "," +(int)Payor.PayorCode.MMIC  + "," +(int) Payor.PayorCode.UHC+ "," +(int) Payor.PayorCode.HCIC)
                        },
                        new SearchValueData {
                            Name = "ScheduleStatusID", Value =
                            Convert.ToString((int)ScheduleStatus.ScheduleStatuses.Confirmed)
                        },
                    };

                AttandanceMonthlySummaryModel model = GetMultipleEntity<AttandanceMonthlySummaryModel>(StoredProcedure.GetAttendanceNotificationList, searchlist);

                #endregion

                string failedAttendanceLogs = "", passedAttendanceLogs = "", failedNonAttendanceLogs = "", passedNonAttendanceActiveLogs = "";

                #region Send Email To ALL

                #region Send Email For Attendance

                List<EmailWiseattandanceNotificationList> groupedbyMail =
                   (from referralListForAttandanceNotification in model.Attandance
                    group referralListForAttandanceNotification by
                      referralListForAttandanceNotification.RecordRequestEmail
                        into g
                    select new EmailWiseattandanceNotificationList
                    {
                        RecordRequestEmail = g.Key,
                        ClientList = g.ToList()
                    }).ToList();

                foreach (EmailWiseattandanceNotificationList attandancegroup in groupedbyMail)
                {
                    if (string.IsNullOrEmpty(attandancegroup.RecordRequestEmail))
                    {
                        continue;
                    }



                    if (attandancegroup.ClientList.Count > 0)
                    {
                        List<string> attachmentList = new List<string>();

                        MissingDocumentEmailTokenModel emailTokenModel = new MissingDocumentEmailTokenModel();

                        //Genrate Html for Client

                        #region Generate Client List Into Templates

                        List<AttandanceNotificationEmailListModel> referralList = attandancegroup.ClientList.ToList();
                        List<AhcccsidWiseGroupList> groupedbyClient = (from item in referralList group item by item.AHCCCSID into g select new AhcccsidWiseGroupList { AHCCCSID = g.Key, Client = g.ToList() }).ToList();
                        string html = "<ul>";
                        string clientDetail = "<li><b>{0} (#{2}, DOB : {1})</b></li>";
                        foreach (var item in groupedbyClient)
                        {
                            AttandanceNotificationEmailListModel modellist = item.Client.FirstOrDefault();
                            html += string.Format(clientDetail, Common.GetGeneralNameFormat(modellist.FirstName, modellist.LastName),
                                                  modellist.Dob.ToString(Constants.GlobalDateFormat, CultureInfo.InvariantCulture), item.AHCCCSID);

                            #region Individual Email Template For Note Log


                            string builder = "<ul>" + string.Format(clientDetail, Common.GetGeneralNameFormat(modellist.FirstName, modellist.LastName),
                                                  modellist.Dob.ToString(Constants.GlobalDateFormat, CultureInfo.InvariantCulture), item.AHCCCSID) + "</ul>";
                            MissingDocumentEmailTokenModel tokens = new MissingDocumentEmailTokenModel();
                            tokens.ClientList = builder;
                            item.SentEmailTemplate = TokenReplace.ReplaceTokens(attendanceNotification.EmailTemplateBody, tokens);

                            #endregion

                        }
                        html += "</ul>";
                        #endregion



                        //emailTokenModel.ClientList = GetClientListForAttendanceNotificationEmail(attandancegroup.ClientList.ToList());
                        emailTokenModel.ClientList = html;
                        emailTokenModel.CaseManager = Resource.Facilitator;

                        //Genrate Attachment for Client
                        DownloadFileModel downloadFileModel = GenratePdfMonthlySummary(attandancegroup.ClientList.ToList());
                        if (!string.IsNullOrEmpty(downloadFileModel.AbsolutePath))
                            attachmentList.Add(downloadFileModel.AbsolutePath);

                        string emailbody = TokenReplace.ReplaceTokens(attendanceNotification.EmailTemplateBody, emailTokenModel);

                        bool isSentMail = Common.SendEmail(attendanceNotification.EmailTemplateSubject, _cacheHelper.SupportEmail, attandancegroup.RecordRequestEmail,
                                                           emailbody, EnumEmailType.Attendance_Notification.ToString(),
                                                           ConfigSettings.RecordCCEmailAddress, (int)EmailHelper.SMTPSetting.EncryptedEmailSetting, attachmentList);
                        if (isSentMail)
                        {
                            AmazonFileUpload amazoneFileUpload = new AmazonFileUpload();
                            string destPath = ConfigSettings.AmazoneUploadPath + ConfigSettings.NoteAttachment +
                                              downloadFileModel.VirtualPath.Substring(downloadFileModel.VirtualPath.LastIndexOf('/'));
                            var file = amazoneFileUpload.UploadFile(ConfigSettings.ZarephathBucket, destPath, downloadFileModel.AbsolutePath.TrimStart('/'), true);

                            foreach (var item in groupedbyClient)
                            {
                                string monthlySummaryIds = string.Empty;
                                foreach (var data in attandancegroup.ClientList.ToList())
                                {
                                    if (data.ReferralID == item.Client.First().ReferralID)
                                    {
                                        if (string.IsNullOrEmpty(monthlySummaryIds))
                                            monthlySummaryIds = data.ReferralMonthlySummariesID.ToString();
                                        else
                                            monthlySummaryIds += "," + data.ReferralMonthlySummariesID.ToString();
                                    }

                                }


                                INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                                iNoteDataProvider.SaveGeneralNote(item.Client.First().ReferralID, item.SentEmailTemplate, Resource.AttendanceNotificationEmail, 0,
                                                                  attandancegroup.RecordRequestEmail, Resource.CaseManager, Resource.Email, file, monthlySummaryIds);
                            }
                            passedAttendanceLogs = passedAttendanceLogs + string.Format("{0}{1}", attandancegroup.RecordRequestEmail, Environment.NewLine);

                        }
                        else
                        {
                            failedAttendanceLogs = failedAttendanceLogs + string.Format("{0}{1}", attandancegroup.RecordRequestEmail, Environment.NewLine);
                        }


                    }


                }

                #endregion

                #region Send Mail to Non Attendance

                List<EmailWiseattandanceNotificationList> nonAttandgroupedbyMail =
                   (from referralListForAttandanceNotification in model.NonAttandance
                    group referralListForAttandanceNotification by
                           referralListForAttandanceNotification.RecordRequestEmail
                        into g
                    select new EmailWiseattandanceNotificationList
                    {
                        RecordRequestEmail = g.Key,
                        ClientList = g.ToList()
                    }).ToList();

                foreach (EmailWiseattandanceNotificationList attandancegroup in nonAttandgroupedbyMail)
                {
                    if (string.IsNullOrEmpty(attandancegroup.RecordRequestEmail))
                    {
                        continue;
                    }



                    if (attandancegroup.ClientList.Count > 0)
                    {
                        List<string> attachmentList = new List<string>();
                        MissingDocumentEmailTokenModel emailTokenModel = new MissingDocumentEmailTokenModel();
                        emailTokenModel.MonthName = fromdate.ToString("MMMM");

                        #region Generate Client List Into Templates

                        List<AttandanceNotificationEmailListModel> referralList = attandancegroup.ClientList.ToList();
                        List<AhcccsidWiseGroupList> groupedbyClient = (from item in referralList group item by item.AHCCCSID into g select new AhcccsidWiseGroupList { AHCCCSID = g.Key, Client = g.ToList() }).ToList();
                        string html = "<ul>";
                        string clientDetail = "<li><b>{0} (#{2}, DOB : {1})</b></li>";
                        foreach (var item in groupedbyClient)
                        {
                            AttandanceNotificationEmailListModel modellist = item.Client.FirstOrDefault();
                            html += string.Format(clientDetail, Common.GetGeneralNameFormat(modellist.FirstName, modellist.LastName),
                                                  modellist.Dob.ToString(Constants.GlobalDateFormat, CultureInfo.InvariantCulture), item.AHCCCSID);

                            #region Individual Email Template For Note Log


                            string builder = "<ul>" + string.Format(clientDetail, Common.GetGeneralNameFormat(modellist.FirstName, modellist.LastName),
                                                  modellist.Dob.ToString(Constants.GlobalDateFormat, CultureInfo.InvariantCulture), item.AHCCCSID) + "</ul>";
                            MissingDocumentEmailTokenModel tokens = new MissingDocumentEmailTokenModel();
                            tokens.MonthName = fromdate.ToString("MMMM");
                            tokens.ClientList = builder;
                            item.SentEmailTemplate = TokenReplace.ReplaceTokens(noneAttendanceNotification.EmailTemplateBody, tokens);

                            #endregion

                        }
                        html += "</ul>";
                        #endregion




                        emailTokenModel.ClientList = html;//GetClientListForAttendanceNotificationEmail(attandancegroup.ClientList.ToList());
                        emailTokenModel.CaseManager = Resource.Facilitator;
                        string emailbody = TokenReplace.ReplaceTokens(noneAttendanceNotification.EmailTemplateBody, emailTokenModel);

                        bool isSentMail = Common.SendEmail(noneAttendanceNotification.EmailTemplateSubject, _cacheHelper.SupportEmail, attandancegroup.RecordRequestEmail,
                                                           emailbody, EnumEmailType.Attendance_Notification.ToString(),
                                                           ConfigSettings.RecordCCEmailAddress, (int)EmailHelper.SMTPSetting.EncryptedEmailSetting, attachmentList);
                        if (isSentMail)
                        {
                            foreach (var item in groupedbyClient)
                            {
                                INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                                iNoteDataProvider.SaveGeneralNote(item.Client.First().ReferralID, item.SentEmailTemplate, Resource.AttendanceNotificationEmail, 0,
                                                                  attandancegroup.RecordRequestEmail, Resource.CaseManager, Resource.Email);
                            }
                            passedNonAttendanceActiveLogs = passedNonAttendanceActiveLogs + string.Format("{0}{1}", attandancegroup.RecordRequestEmail, Environment.NewLine);

                        }
                        else
                        {
                            failedNonAttendanceLogs = failedNonAttendanceLogs + string.Format("{0}{1}", attandancegroup.RecordRequestEmail, Environment.NewLine);
                        }
                    }



                }



                #endregion

                #region Send Logs Details To Alberto
                if (!string.IsNullOrEmpty(failedAttendanceLogs) || !string.IsNullOrEmpty(failedNonAttendanceLogs))
                {
                    string message =
                        string.Format(//"Record Request has been sent successfully on following Email address.{0}{1}{0}{0}" +
                                      "SECURE Zarephath Monthly Summary email sent failed on following Email address.{0}{2}{0}{0}" +
                                      //"Record Update has been sent successfully on following Email address.{0}{3}{0}{0}" +
                                      "SECURE Zarephath Monthly Summary - No Attendance email sent failed on following Email address.{0}{4}{0}{0}",
                                      Environment.NewLine,
                                      string.IsNullOrEmpty(passedAttendanceLogs) ? "No Email Address Found" : passedAttendanceLogs,
                                      string.IsNullOrEmpty(failedAttendanceLogs) ? "No Email Address Found" : failedAttendanceLogs,
                                      string.IsNullOrEmpty(passedNonAttendanceActiveLogs) ? "No Email Address Found" : passedNonAttendanceActiveLogs,
                                      string.IsNullOrEmpty(failedNonAttendanceLogs) ? "No Email Address Found" : failedNonAttendanceLogs);

                    string emailMsg = message.Replace(Environment.NewLine, "<br/>");
                    string virtualPath = Common.CreateLogFile(message, "log_details_" + DateTime.Now.Ticks + ".txt", logpath);
                    List<string> attachement = new List<string> { HttpContext.Current.Server.MapCustomPath(virtualPath) };
                    Common.SendEmail("Monthly Summary Automation Log Details", "", ConfigSettings.RecordLogEmailAddress, emailMsg, "", ConfigSettings.RecordCCEmailAddress, 1, attachement);
                }
                #endregion

                #endregion

                Common.CreateLogFile("Attendance Notification Email CronJob Completed.", ConfigSettings.AttendanceNotificationFileName, logpath);
                return response;
            }
            catch (Exception ex)
            {
                Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.AttendanceNotificationFileName, logpath);
                throw ex;
            }
        }

        public string GetClientListForAttendanceNotificationEmail(List<AttandanceNotificationEmailListModel> referralList)
        {
            List<AhcccsidWiseGroupList> groupedbyMail =
                (from model in referralList group model by model.AHCCCSID into g select new AhcccsidWiseGroupList { AHCCCSID = g.Key, Client = g.ToList() }).ToList();

            string html = "<ul>";
            string clientDetail = "<li><b>{0} (#{2}, DOB : {1})</b></li>";

            foreach (var item in groupedbyMail)
            {
                AttandanceNotificationEmailListModel modellist = item.Client.FirstOrDefault();
                html += string.Format(clientDetail, Common.GetGeneralNameFormat(modellist.FirstName, modellist.LastName),
                                      modellist.Dob.ToString(Constants.GlobalDateFormat, CultureInfo.InvariantCulture), item.AHCCCSID);

            }
            html += "</ul>";
            return html;
        }

        public DownloadFileModel GenratePdfMonthlySummary(List<AttandanceNotificationEmailListModel> model)
        {
            #region Get Monthly Summary List and Email Template

            EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
                                        {
                                            new SearchValueData{Name = "EmailTemplateTypeID",Value =Convert.ToInt16(EnumEmailType.Monthly_Summary_Email).ToString(),IsEqual = true}
                                        });

            //string PrintHtml = Common.ReadHtmlFile("/MonthlySummaryPrint.html");

            string PrintHtml = emailTemplate.EmailTemplateBody;
            PrintHtml = Regex.Replace(PrintHtml, "<hr(.*?)>", "<hr $1 />");
            PrintHtml = Regex.Replace(PrintHtml, "<br(.*?)>", "<br $1 />");

            #endregion

            string pdfHtmlString = "";

            #region Set File Path and File Name
            string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);
            string path = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string fileName = string.Format("{0}_{1}", Constants.MonthlySummary, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));

            var downloadFileModel = new DownloadFileModel();
            downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", path, fileName, Constants.Extention_pdf);
            downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_pdf);
            downloadFileModel.FileName = fileName + Constants.Extention_pdf;

            #endregion

            #region Genrate PDF

            if (model.Count > 0)
            {
                foreach (var item in model)
                {
                    if (item.ReferralMonthlySummariesID > 0)
                    {
                        #region Check Mood for Throughout Weekend

                        if (!item.MoodforThroughoutWeekend.Split(',').Contains(Constants.OtherDetails))
                        {
                            item.MoodforThroughoutWeekendTxt = null;
                            item.MoodforThroughoutWeekendExplanation = null;
                        }
                        else
                        {
                            item.MoodforThroughoutWeekendExplanation = Constants.OtherDetailsLable;
                            item.MoodforThroughoutWeekendTxt = string.IsNullOrEmpty(item.MoodforThroughoutWeekendTxt) ? Resource.NA : item.MoodforThroughoutWeekendTxt;
                        }

                        #endregion

                        item.MonthlySummaryDate = item.MonthlySummaryStartDate + " - " + item.MonthlySummaryEndDate;

                        #region Check Medications Dispensed

                        if (item.MedicationsDispensed == "Yes")
                        {
                            item.MedicationsDispensedExplanation = Constants.Explanation;
                            item.MedicationsDispensedTxt = string.IsNullOrEmpty(item.MedicationsDispensedTxt) ? Resource.NA : item.MedicationsDispensedTxt;
                        }
                        else
                        {
                            item.MedicationsDispensedTxt = null;
                        }

                        #endregion

                        #region Check Coordination of Create  Pickup

                        if (!string.IsNullOrEmpty(item.CoordinationofCareatPickupOption) && !item.CoordinationofCareatPickupOption.Split(',').Contains(Constants.Other))
                        {
                            item.CoordinationofCareatPickupTxt = null;
                            item.CoordinationofCareatPickupExplanation = null;
                        }
                        else
                        {
                            item.CoordinationofCareatPickupTxt = string.IsNullOrEmpty(item.CoordinationofCareatPickupTxt) ? Resource.NA : item.CoordinationofCareatPickupTxt;
                            item.CoordinationofCareatPickupExplanation = Constants.OtherLabel;
                        }

                        #endregion

                        #region Check Coordination of Create Drop Off

                        if (!string.IsNullOrEmpty(item.CoordinationofCareatDropOffOption) && !item.CoordinationofCareatDropOffOption.Split(',').Contains(Constants.Other))
                        {
                            item.CoordinationofCareatDropOffTxt = null;
                            item.CoordinationofCareatDropOffExplanation = null;
                        }
                        else
                        {
                            item.CoordinationofCareatDropOffExplanation = Constants.OtherLabel;
                            item.CoordinationofCareatDropOffTxt = string.IsNullOrEmpty(item.CoordinationofCareatDropOffTxt) ? Resource.NA : item.CoordinationofCareatDropOffTxt;
                        }

                        item.ImageText = "<img src='" + ConfigSettings.AmazonS3Url + ConfigSettings.ZarephathBucket + "/" + item.Signature + "' width='200px' style='float:right;'/>";

                        #endregion

                        #region Replace Text

                        item.Breakfast = string.IsNullOrEmpty(item.Breakfast) ? Resource.NA : item.Breakfast.Replace("_", " ");
                        item.Lunch = string.IsNullOrEmpty(item.Lunch) ? Resource.NA : item.Lunch.Replace("_", " ");
                        item.Dinner = string.IsNullOrEmpty(item.Dinner) ? Resource.NA : item.Dinner.Replace("_", " ");


                        item.Breakfast = string.IsNullOrEmpty(item.Breakfast) ? Resource.NA : item.Breakfast.Replace(",", " ");
                        item.Lunch = string.IsNullOrEmpty(item.Lunch) ? Resource.NA : item.Lunch.Replace(",", " ");
                        item.Dinner = string.IsNullOrEmpty(item.Dinner) ? Resource.NA : item.Dinner.Replace(",", " ");

                        item.MoodforThroughoutWeekend = string.IsNullOrEmpty(item.MoodforThroughoutWeekend) ? Resource.NA : item.MoodforThroughoutWeekend.Replace("_", " ");
                        item.CoordinationofCareatPickupOption = string.IsNullOrEmpty(item.CoordinationofCareatPickupOption) ? Resource.NA : item.CoordinationofCareatPickupOption.Replace("_", " ");
                        item.CoordinationofCareatDropOffOption = string.IsNullOrEmpty(item.CoordinationofCareatDropOffOption) ? Resource.NA : item.CoordinationofCareatDropOffOption.Replace("_", " ");


                        //item.Breakfast = Regex.Replace(item.Breakfast, "(?<=,)(?!\\s)", " ");
                        //item.Lunch = Regex.Replace(item.Lunch, "(?<=,)(?!\\s)", " ");
                        //item.Dinner = Regex.Replace(item.Dinner, "(?<=,)(?!\\s)", " ");

                        item.MoodforThroughoutWeekend = Regex.Replace(item.MoodforThroughoutWeekend, "(?<=,)(?!\\s)", " ");
                        item.CoordinationofCareatPickupOption = Regex.Replace(item.CoordinationofCareatPickupOption, "(?<=,)(?!\\s)", " ");
                        item.CoordinationofCareatDropOffOption = Regex.Replace(item.CoordinationofCareatDropOffOption, "(?<=,)(?!\\s)", " ");

                        #endregion

                        item.Medication = string.IsNullOrEmpty(item.Medication) ? Resource.NA : item.Medication;
                        item.BreakfastTxt = string.IsNullOrEmpty(item.BreakfastTxt) ? Resource.NA : item.BreakfastTxt;
                        item.LunchTxt = string.IsNullOrEmpty(item.LunchTxt) ? Resource.NA : item.LunchTxt;

                        item.DinnerTxt = string.IsNullOrEmpty(item.DinnerTxt) ? Resource.NA : item.DinnerTxt;

                        item.OutingDetails = string.IsNullOrEmpty(item.OutingDetails) ? Resource.NA : item.OutingDetails;
                        item.PCIInformation = string.IsNullOrEmpty(item.PCIInformation) ? Resource.NA : item.PCIInformation;
                        item.TreatmentPlan = string.IsNullOrEmpty(item.TreatmentPlan) ? Resource.NA : item.TreatmentPlan;
                        item.Nextvisit = string.IsNullOrEmpty(item.Nextvisit) ? Resource.NA : item.Nextvisit;
                        item.CurrentServicePlan = string.IsNullOrEmpty(item.CurrentServicePlan) ? Resource.NA : item.CurrentServicePlan;
                        item.BelongingsandMedicationsReturned = string.IsNullOrEmpty(item.BelongingsandMedicationsReturned) ? Resource.NA : item.BelongingsandMedicationsReturned;
                        pdfHtmlString = pdfHtmlString + TokenReplace.ReplaceTokens(PrintHtml, item);
                    }

                }
                if (!string.IsNullOrEmpty(pdfHtmlString))
                {
                    Byte[] bytes = Common.ReturnByteArrayFromStringForitextSharpPDF(pdfHtmlString);
                    File.WriteAllBytes(downloadFileModel.AbsolutePath, bytes);
                }
            }

            #endregion

            return downloadFileModel;
        }



        //#region Genrate Monthly Summary PDf

        //public ServiceResponse GenrateMonthyReviewPDF(string referralIDs)
        //{
        //    var response = new ServiceResponse();

        //    #region Set Search Values with Parameter

        //    var fromdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    var todate = fromdate.AddMonths(1).AddDays(-1);

        //    var searchlist = new List<SearchValueData>
        //            {
        //                 new SearchValueData {Name = "ReferralIDs", Value =referralIDs},
        //                 new SearchValueData {Name = "FromDate", Value =Convert.ToDateTime(fromdate).ToString(Constants.DbDateFormat)},
        //                 new SearchValueData {Name = "ToDate", Value =Convert.ToDateTime(todate).ToString(Constants.DbDateFormat)},
        //            };

        //    #endregion

        //    #region Get Monthly Summary List and Email Template

        //    List<string> model;
        //    // List<MothlySummaryListModel> model = GetEntityList<MothlySummaryListModel>(StoredProcedure.GetAttandanceMonthlySummaryList, searchlist);
        //    EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
        //                            {
        //                                new SearchValueData{Name = "EmailTemplateTypeID",Value =Convert.ToInt16(EnumEmailType.Monthly_Summary_Email).ToString(),IsEqual = true}
        //                            });

        //    //string PrintHtml = Common.ReadHtmlFile("/MonthlySummaryPrint.html");
        //    string PrintHtml = emailTemplate.EmailTemplateBody;
        //    PrintHtml = Regex.Replace(PrintHtml, "<hr(.*?)>", "<hr $1 />");
        //    PrintHtml = Regex.Replace(PrintHtml, "<br(.*?)>", "<br $1 />");

        //    #endregion

        //    string pdfHtmlString = "";

        //    #region Set File Path and File Name

        //    string path = HttpContext.Current.Server.MapCustomPath(_cacheHelper.ReportExcelFileUploadPath);
        //    string fileName = string.Format("{0}_{1}", Constants.ReferralMonthlySummary, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));

        //    var downloadFileModel = new DownloadFileModel();
        //    downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", path, fileName, Constants.Extention_pdf);
        //    downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", _cacheHelper.ReportExcelFileUploadPath, fileName, Constants.Extention_pdf);
        //    downloadFileModel.FileName = fileName + Constants.Extention_pdf;

        //    #endregion

        //    #region Genrate PDF

        //    if (model.Count > 0)
        //    {
        //        foreach (var item in model)
        //        {
        //            #region Check Mood for Throughout Weekend

        //            if (!item.MoodforThroughoutWeekend.Split(',').Contains(Constants.OtherDetails))
        //            {
        //                item.MoodforThroughoutWeekendTxt = null;
        //                item.MoodforThroughoutWeekendExplanation = null;
        //            }
        //            else
        //            {
        //                item.MoodforThroughoutWeekendExplanation = Constants.OtherDetailsLable;
        //            }

        //            #endregion

        //            #region Check Medications Dispensed

        //            if (item.MedicationsDispensed == "Yes")
        //            {
        //                item.MedicationsDispensedExplanation = Constants.Explanation;
        //            }
        //            else
        //            {
        //                item.MedicationsDispensedTxt = null;
        //            }

        //            #endregion

        //            #region Check Coordination of Careat Pickup

        //            if (!item.CoordinationofCareatPickup.Split(',').Contains(Constants.Other))
        //            {
        //                item.CoordinationofCareatPickupTxt = null;
        //                item.CoordinationofCareatPickupExplanation = null;
        //            }
        //            else
        //            {
        //                item.CoordinationofCareatPickupExplanation = Constants.OtherLabel;
        //            }

        //            #endregion

        //            #region Check Coordination of Careat Drop Off

        //            if (!item.CoordinationofCareatDropOff.Split(',').Contains(Constants.Other))
        //            {
        //                item.CoordinationofCareatDropOffTxt = null;
        //                item.CoordinationofCareatDropOffExplanation = null;
        //            }
        //            else
        //            {
        //                item.CoordinationofCareatDropOffExplanation = Constants.OtherLabel;
        //            }

        //            #endregion

        //            pdfHtmlString = pdfHtmlString + TokenReplace.ReplaceTokens(PrintHtml, item);
        //        }

        //        Byte[] bytes = Common.ReturnByteArrayFromStringForitextSharpPDF(pdfHtmlString);
        //        File.WriteAllBytes(downloadFileModel.AbsolutePath, bytes);
        //        response.IsSuccess = true;
        //        response.Data = downloadFileModel;
        //    }
        //    return response;
        //}

        //    #endregion

        //#endregion

        #endregion

        #region Send Service Plan Email

        public ServiceResponse SendServicePlanEmail()
        {
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.ServicePlanLog);
            if (!ConfigSettings.Service_CM_ServicePlan_ON)
            {
                Common.CreateLogFile("CM ServicePlan Service CronJob can't run because it is mark as offline from web config.", ConfigSettings.ServicePlanFileName, logpath);
                return null;
            }

            var response = new ServiceResponse();

            try
            {
                Common.CreateLogFile("Service Plan Email CronJob Started.", ConfigSettings.ServicePlanFileName, logpath);

                #region Check For Quarterly Basis
                DateTime date = DateTime.Now;
                int quarterNumber = (date.Month - 1) / 3 + 1;
                DateTime firstDayOfQuarter = new DateTime(date.Year, (quarterNumber - 1) * 3 + 1, 1);
                DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3).AddDays(-1);


                if (DateTime.Today != lastDayOfQuarter.Date)
                {
                    Common.CreateLogFile("Today is not Last day of Quarter. Service Plan Email CronJob Completed.", ConfigSettings.ServicePlanFileName, logpath);
                    return response;
                }

                //DateTime dt = DateTime.Today;
                //int month = dt.Month;
                //int quarter = (month + 2) / 3;

                //if (month != quarter)
                //{
                //    Common.CreateLogFile("Today is not Last day of Quarter. Attendance Notification Email CronJob Completed.", ConfigSettings.ServicePlanFileName, logpath);
                //    return response;
                //}

                #endregion

                #region Get Service Plan Email

                var searchlist = new List<SearchValueData>
                    {
                         new SearchValueData {
                            Name = "PayorIDs", Value =Convert.ToString( (int)Payor.PayorCode.MMIC)
                         },
                           new SearchValueData {
                            Name = "ReferralStatusIDs", Value =Convert.ToString((int)Common.ReferralStatusEnum.Active)
                        },
                    };

                List<ServicePlanListModel> servicePlanListModel = GetEntityList<ServicePlanListModel>(StoredProcedure.GetServicePlanList, searchlist);

                #endregion Get Service Plan Email

                #region Get email templates

                List<EmailWiseServicePlanList> servicePlan =
                    (from servicePlanList in servicePlanListModel
                     group servicePlanList by
                         servicePlanList.RecordRequestEmail
                         into g
                     select new EmailWiseServicePlanList { RecordRequestEmail = g.Key, ServicePlanList = g.ToList() }).ToList();

                EmailTemplate serviceplantemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
                    {
                        new SearchValueData
                            {
                                Name = "EmailTemplateTypeID",Value = Convert.ToInt16(EnumEmailType.Service_Plan).ToString(),IsEqual = true
                            }
                    });

                #endregion get email templates

                string failedServicePlanLogs = "", passedServicePlanLogs = "";

                #region Send Email To ALL

                #region Send Mail To CM

                foreach (EmailWiseServicePlanList servicePlanmodel in servicePlan)
                {
                    if (string.IsNullOrEmpty(servicePlanmodel.RecordRequestEmail))
                    {
                        continue;
                    }

                    MissingDocumentEmailTokenModel emailTokenModel = new MissingDocumentEmailTokenModel();

                    #region Service Plan Html String with Client Detail

                    string html = "<ul>";

                    string clientWithDocDtlli = "<li><b>{0} (#{2}, DOB : {1})</b></li>";

                    foreach (var item in servicePlanmodel.ServicePlanList)
                    {
                        html += string.Format(clientWithDocDtlli, Common.GetGeneralNameFormat(item.FirstName, item.LastName),
                                                  item.Dob.ToString(Constants.GlobalDateFormat, CultureInfo.InvariantCulture), item.AHCCCSID);


                        #region Individual Email Template For Note Log


                        string builder = "<ul>" + string.Format(clientWithDocDtlli, Common.GetGeneralNameFormat(item.FirstName, item.LastName),
                                              item.Dob.ToString(Constants.GlobalDateFormat, CultureInfo.InvariantCulture), item.AHCCCSID) + "</ul>";
                        MissingDocumentEmailTokenModel tokens = new MissingDocumentEmailTokenModel();
                        tokens.ClientList = builder;
                        item.SentEmailTemplate = TokenReplace.ReplaceTokens(serviceplantemplate.EmailTemplateBody, tokens);

                        #endregion


                    }
                    html += "</ul>";

                    #endregion

                    #region Set Value for Html Email Template

                    emailTokenModel.ClientList = html;
                    emailTokenModel.CaseManager = Resource.Facilitator;
                    string emailbody = TokenReplace.ReplaceTokens(serviceplantemplate.EmailTemplateBody, emailTokenModel);

                    #endregion



                    bool isSentMail = Common.SendEmail(serviceplantemplate.EmailTemplateSubject, _cacheHelper.SupportEmail, servicePlanmodel.RecordRequestEmail,
                                                       emailbody, EnumEmailType.Service_Plan.ToString(),
                                                       ConfigSettings.RecordCCEmailAddress, (int)EmailHelper.SMTPSetting.EncryptedEmailSetting);

                    #region Enter in notes for All  referral

                    if (isSentMail)
                    {
                        foreach (var item in servicePlanmodel.ServicePlanList)
                        {
                            INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                            iNoteDataProvider.SaveGeneralNote(item.ReferralID, item.SentEmailTemplate, Resource.ServicePlanEmail, 0,
                                                              servicePlanmodel.RecordRequestEmail, Resource.CaseManager, Resource.Email);
                        }
                    }

                    #endregion
                }

                #endregion Send Mail

                #region Send Logs Details To Alberto
                if (!string.IsNullOrEmpty(failedServicePlanLogs) || !string.IsNullOrEmpty(failedServicePlanLogs))
                {
                    string message =
                        string.Format( //"SECURE Zarephath Service Plan email has been sent successfully on following Email address.{0}{1}{0}{0}" +
                                        "SECURE Zarephath Service Plan email sent failed on following Email address.{0}{2}{0}{0}",
                                      Environment.NewLine,
                                      string.IsNullOrEmpty(passedServicePlanLogs) ? "No Email Address Found" : passedServicePlanLogs,
                                      string.IsNullOrEmpty(failedServicePlanLogs) ? "No Email Address Found" : failedServicePlanLogs);

                    string emailMsg = message.Replace(Environment.NewLine, "<br/>");
                    string virtualPath = Common.CreateLogFile(message, "log_details_" + DateTime.Now.Ticks + ".txt", logpath);
                    List<string> attachement = new List<string> { HttpContext.Current.Server.MapCustomPath(virtualPath) };
                    Common.SendEmail("Zarephath Service Plan Log Details", "", ConfigSettings.RecordLogEmailAddress, emailMsg, "", ConfigSettings.RecordCCEmailAddress, 1, attachement);
                }
                #endregion

                #endregion

                Common.CreateLogFile("Service Plan Email CronJob Completed.", ConfigSettings.ServicePlanFileName, logpath);
                return response;
            }
            catch (Exception ex)
            {
                Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.ServicePlanFileName, logpath);
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Take DataBase Back UP


        public ServiceResponse TakeDbBackUp()
        {
            ServiceResponse response = new ServiceResponse();
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.TakeDbBackUpLog);

            try
            {
                Common.CreateLogFile("Create Database BackUp Service Started.", ConfigSettings.TakeDbBackUpFileName, logpath);

                #region Create Database BackUP File

                // read connectionstring from config file
                var connectionString = ConfigurationManager.ConnectionStrings["ZarePhath"].ConnectionString;

                // read backup folder from config file ("C:/temp/")
                string backupFolder = ConfigSettings.DBBackupFolder;
                if (backupFolder == null) return response;

                if (!Directory.Exists(backupFolder))
                    Directory.CreateDirectory(backupFolder);


                var sqlConStrBuilder = new SqlConnectionStringBuilder(connectionString);

                // set backupfilename (you will get something like: "C:/temp/MyDatabase-2013-12-07.bak")
                var backupFileName = String.Format("{0}{1}_{2}.bak",
                    backupFolder, sqlConStrBuilder.InitialCatalog,
                    DateTime.Now.ToString("MM_dd_yyyy"));

                var backupZipFileName = String.Format("{0}{1}_{2}.zip",
                   backupFolder, sqlConStrBuilder.InitialCatalog,
                   DateTime.Now.ToString("MM_dd_yyyy"));

                if (File.Exists(backupFileName))
                    File.Delete(backupFileName);

                using (var connection = new SqlConnection(sqlConStrBuilder.ConnectionString))
                {


                    var query = String.Format("BACKUP DATABASE {0} TO DISK='{1}'",
                        sqlConStrBuilder.InitialCatalog, backupFileName);

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.CommandTimeout = 1200;
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                #endregion

                #region Create ZIP Of Taken Database BackUP
                if (File.Exists(backupZipFileName)) File.Delete(backupZipFileName);

                ZipArchive zip = ZipFile.Open(backupZipFileName, ZipArchiveMode.Create);
                zip.CreateEntryFromFile(backupFileName, Path.GetFileName(backupFileName), CompressionLevel.Optimal);
                zip.Dispose();

                if (File.Exists(backupFileName)) File.Delete(backupFileName);

                #endregion

                #region Delete 10 Days Old BackUp Files

                string[] files = Directory.GetFiles(backupFolder);
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.LastAccessTime < DateTime.Now.AddDays(-10))
                        fi.Delete();
                }


                #endregion


                Common.CreateLogFile("Create Database BackUp Service Completed.", ConfigSettings.TakeDbBackUpFileName, logpath);
                return response;
            }
            catch (Exception ex)
            {
                Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.TakeDbBackUpFileName, logpath);
                throw ex;
            }
        }




        #endregion

        #region SCRAP CODE: Cron JOB TO Process Queued 835 Files

        //public ServiceResponse ProcessEdi835Files()
        //{
        //    string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.Edi835FileLog);
        //    try
        //    {
        //        Common.CreateLogFile("Process Edi835Files CronJob Started.", ConfigSettings.Edi835FileName, logpath);
        //        ServiceResponse response = new ServiceResponse();

        //        #region Process 835 Files
        //        List<SearchValueData> searchList = new List<SearchValueData>();
        //        searchList.Add(new SearchValueData { Name = "Upload835FileProcessStatus", Value = ((int)EnumUpload835FileProcessStatus.InProcess).ToString() });
        //        List<Upload835File> upload835FileList = GetEntityList<Upload835File>(searchList);

        //        foreach (var upload835File in upload835FileList)
        //        {
        //            Edi835 edi835 = new Edi835();
        //            string ediFilePath = HttpContext.Current.Server.MapCustomPath(upload835File.FilePath); ;
        //            string newRedablefilePath = string.Format("{0}{1}{2}", _cacheHelper.EdiFile835CsvDownLoadPath, Guid.NewGuid().ToString(), Constants.FileExtension_Csv);
        //            Edi835ResponseModel edi835ResponseModel = edi835.GenerateEdi835Model(ediFilePath, newRedablefilePath, newRedablefilePath);

        //            foreach (var edi835Model in edi835ResponseModel.Edi835ModelList)
        //            {
        //                edi835Model.CLP01_ClaimSubmitterIdentifier = "112ZRPB208BN37";

        //                string[] strTemp = edi835Model.CLP01_ClaimSubmitterIdentifier.Split(new string[] { "ZRPB" }, StringSplitOptions.None);
        //                long noteId = Convert.ToInt64(strTemp[0]);
        //                long batchId = Convert.ToInt64(strTemp[1].Split(new string[] { "BN" }, StringSplitOptions.None)[0]);
        //                long batchNoteId = Convert.ToInt64(strTemp[1].Split(new string[] { "BN" }, StringSplitOptions.None)[1]);

        //                BatchNote subBatchNote = new BatchNote();
        //                subBatchNote.BatchID = batchId;
        //                subBatchNote.NoteID = noteId;

        //                subBatchNote.ParentBatchNoteID = batchNoteId;

        //                subBatchNote.N102_PayorName = edi835Model.N102_PayorName;
        //                subBatchNote.PER02_PayorBusinessContactName = edi835Model.PER02_PayorBusinessContactName;
        //                subBatchNote.PER04_PayorBusinessContact = edi835Model.PER04_PayorBusinessContact;
        //                subBatchNote.PER02_PayorTechnicalContactName = edi835Model.PER02_PayorTechnicalContactName;
        //                subBatchNote.PER04_PayorTechnicalContact = edi835Model.PER04_PayorTechnicalContact;
        //                subBatchNote.PER06_PayorTechnicalEmail = edi835Model.PER06_PayorTechnicalEmail;
        //                subBatchNote.PER04_PayorTechnicalContactUrl = edi835Model.PER04_PayorTechnicalContactUrl;
        //                subBatchNote.N102_PayeeName = edi835Model.N102_PayeeName;
        //                subBatchNote.N103_PayeeIdentificationQualifier = edi835Model.N103_PayeeIdentificationQualifier;
        //                subBatchNote.N104_PayeeIdentification = edi835Model.N104_PayeeIdentification;

        //                subBatchNote.LX01_ClaimSequenceNumber = edi835Model.LX01_ClaimSequenceNumber;
        //                subBatchNote.CLP02_ClaimStatusCode = edi835Model.CLP02_ClaimStatusCode;
        //                subBatchNote.CLP01_ClaimSubmitterIdentifier = edi835Model.CLP01_ClaimSubmitterIdentifier;
        //                subBatchNote.CLP03_TotalClaimChargeAmount = edi835Model.CLP03_TotalClaimChargeAmount;
        //                subBatchNote.CLP04_TotalClaimPaymentAmount = edi835Model.CLP04_TotalClaimPaymetAmount;
        //                subBatchNote.CLP05_PatientResponsibilityAmount = edi835Model.CLP05_PatientResponsibilityAmount;
        //                subBatchNote.CLP07_PayerClaimControlNumber = edi835Model.CLP07_PayerClaimControlNumber;
        //                subBatchNote.CLP08_PlaceOfService = edi835Model.CLP08_PlaceOfService;

        //                subBatchNote.NM103_PatientLastName = edi835Model.NM103_PatientLastName;
        //                subBatchNote.NM104_PatientFirstName = edi835Model.NM104_PatientFirstName;
        //                subBatchNote.NM109_PatientIdentifier = edi835Model.NM109_PatientIdentifier;
        //                subBatchNote.NM103_ServiceProviderName = edi835Model.NM103_ServiceProviderName;
        //                subBatchNote.NM109_ServiceProviderNpi = edi835Model.NM109_ServiceProviderNpi;

        //                subBatchNote.SVC01_01_ServiceCodeQualifier = edi835Model.SVC01_01_ServiceCodeQualifier;
        //                subBatchNote.SVC01_02_ServiceCode = edi835Model.SVC01_02_ServiceCode;
        //                subBatchNote.SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount = edi835Model.SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount;
        //                subBatchNote.SVC03_LineItemProviderPaymentAmoun_PaidAmount = edi835Model.SVC03_LineItemProviderPaymentAmoun_PaidAmount;
        //                subBatchNote.SVC05_ServiceCodeUnit = edi835Model.SVC05_ServiceCodeUnit;
        //                subBatchNote.DTM02_ServiceStartDate = edi835Model.DTM02_ServiceStartDate;
        //                subBatchNote.DTM02_ServiceEndDate = edi835Model.DTM02_ServiceEndDate;
        //                subBatchNote.CAS01_ClaimAdjustmentGroupCode = edi835Model.CAS01_ClaimAdjustmentGroupCode;
        //                subBatchNote.CAS02_ClaimAdjustmentReasonCode = edi835Model.CAS02_ClaimAdjustmentReasonCode;
        //                subBatchNote.REF02_LineItem_ReferenceIdentification = edi835Model.REF02_LineItem_ReferenceIdentification;
        //                subBatchNote.AMT01_ServiceLineAllowedAmount_AllowedAmount = edi835Model.AMT01_ServiceLineAllowedAmount_AllowedAmount;

        //                subBatchNote.CheckDate = edi835Model.CheckDate;
        //                subBatchNote.CheckAmount = edi835Model.CheckAmount;
        //                subBatchNote.CheckNumber = edi835Model.CheckNumber;
        //                subBatchNote.PolicyNumber = edi835Model.PolicyNumber;
        //                subBatchNote.AccountNumber = edi835Model.AccountNumber;
        //                subBatchNote.ICN = edi835Model.ICN;
        //                subBatchNote.Deductible = edi835Model.Deductible;
        //                subBatchNote.Coins = edi835Model.Coins;
        //                subBatchNote.ProcessedDate = Convert.ToDateTime(edi835Model.ProcessedDate, CultureInfo.InvariantCulture);
        //                subBatchNote.ReceivedDate = upload835File.CreatedDate;
        //                subBatchNote.LoadDate = DateTime.Now.Date;
        //                subBatchNote.Upload835FileID = upload835File.Upload835FileID;
        //                SaveEntity(subBatchNote);
        //            }

        //            upload835File.ReadableFilePath = edi835ResponseModel.GeneratedFileRelativePath;
        //            upload835File.Upload835FileProcessStatus = (int)EnumUpload835FileProcessStatus.Processed;
        //            SaveEntity(upload835File);

        //            GetScalar(StoredProcedure.UpdateBatchAfter835FileProcessed);
        //        }

        //        #endregion

        //        Common.CreateLogFile("Process Edi835Files CronJob Completed Succesfully.", ConfigSettings.Edi835FileName, logpath);

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.Edi835FileName, logpath);
        //        throw ex;
        //    }
        //}

        #endregion

        #region Home Care Code

        public ServiceResponse SyncClaimMessages(string syncall="")
        {
            ServiceResponse response = new ServiceResponse();
            BatchDataProvider batchDataProvider = new BatchDataProvider();
            batchDataProvider.SyncClaimMessages(true,syncall);
            response.IsSuccess = true;
            response.Data = "SyncClaimMessages service completed.";
            return response;
        }
            public ServiceResponse GenerateEmployeeTimeSchedule()
        {
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.GenerateEmpTimeScheduleDaysLog);

            if (!ConfigSettings.Service_GenerateEmpTimeScheduleDays_ON)
            {
                Common.CreateLogFile("Generate Employee TimeS chedule CronJob can't run because it is mark as offline from web config.", ConfigSettings.GenerateEmpTimeScheduleDaysFileName, logpath);
                return null;
            }


            try
            {
                Common.CreateLogFile("Generate Employee Time Schedule CronJob Started.", ConfigSettings.GenerateEmpTimeScheduleDaysFileName, logpath);
                var response = new ServiceResponse();

                DateTime getDate = DateTime.Now;
                List<SearchValueData> searchParam = new List<SearchValueData>
                            {
                                new SearchValueData {Name = "StartDate",Value =Convert.ToDateTime(getDate).ToString(Constants.DbDateFormat)},
                                new SearchValueData {Name = "EndDate",Value =Convert.ToDateTime( getDate.AddDays(ConfigSettings.GenerateEmpRefTimeScheduleDays)).ToString(Constants.DbDateFormat)}
                            };
                GetScalar(StoredProcedure.GenerateEmployeeTimeSlotDates, searchParam);

                Common.CreateLogFile("Generate Employee TimeSchedule CronJob Completed Succesfully.", ConfigSettings.GenerateEmpTimeScheduleDaysFileName, logpath);
                return response;
            }
            catch (Exception ex)
            {
                Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.GenerateEmpTimeScheduleDaysFileName, logpath);
                throw ex;
            }
        }


        public ServiceResponse GeneratePatientTimeSchedule_ForDayCarePatient(DayCareTimeSlotModal modal, int scheduleDays, long loggedInId)
        {
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.GenerateEmpTimeScheduleDaysLog);

            if (!ConfigSettings.Service_GenerateRefTimeScheduleDays_ON)
            {
                Common.CreateLogFile("Generate Patient Time Schedule CronJob can't run because it is mark as offline from web config.", ConfigSettings.GenerateRefTimeScheduleDaysFileName, logpath);
                return null;
            }

            try
            {
                Common.CreateLogFile("Generate Patient Time Schedule CronJob Started.", ConfigSettings.GenerateRefTimeScheduleDaysFileName, logpath);
                var response = new ServiceResponse();

                List<SearchValueData> searchParam = new List<SearchValueData>
                            {
                                new SearchValueData {Name = "StartDate",Value =Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat)},
                                new SearchValueData {Name = "EndDate",Value =Common.GetOrgCurrentDateTime().AddDays(scheduleDays).ToString(Constants.DbDateFormat)},
                                new SearchValueData {Name = "ReferralID",Value = Convert.ToString(modal.ReferralID)},
                                new SearchValueData {Name = "ReferralTimeSlotMasterID",Value = Convert.ToString(modal.ReferralTimeSlotMasterID)},
                                new SearchValueData {Name = "GeneratePatientSchedules",Value = modal.GeneratePatientSchedules ?"1":"0"},
                                new SearchValueData {Name = "LoggedInID",Value = Convert.ToString(loggedInId)}
                            };
                GetScalar(StoredProcedure.GenerateReferralTimeSlotDates_ForDayCare, searchParam);
                response.IsSuccess = true;
                response.Message = Resource.TimeSlotAddedSuccessfully;
                Common.CreateLogFile("Generate Patient TimeSchedule CronJob Completed Succesfully.", ConfigSettings.GenerateRefTimeScheduleDaysFileName, logpath);
                return response;
            }
            catch (Exception ex)
            {
                Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.GenerateRefTimeScheduleDaysFileName, logpath);
                throw ex;
            }
        }

        public ServiceResponse GeneratePatientTimeSchedule(int scheduleDays)
        {
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.GenerateEmpTimeScheduleDaysLog);

            if (!ConfigSettings.Service_GenerateRefTimeScheduleDays_ON)
            {
                Common.CreateLogFile("Generate Patient Time Schedule CronJob can't run because it is mark as offline from web config.", ConfigSettings.GenerateRefTimeScheduleDaysFileName, logpath);
                return null;
            }


            try
            {
                Common.CreateLogFile("Generate Patient Time Schedule CronJob Started.", ConfigSettings.GenerateRefTimeScheduleDaysFileName, logpath);
                var response = new ServiceResponse();

                DateTime getDate = DateTime.Now;


                List<SearchValueData> searchParam = new List<SearchValueData>
                            {
                                new SearchValueData {Name = "StartDate",Value =Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat)},
                                new SearchValueData {Name = "EndDate",Value =Common.GetOrgCurrentDateTime().AddDays(scheduleDays).ToString(Constants.DbDateFormat)}
                            };
                GetScalar(StoredProcedure.GenerateReferralTimeSlotDates, searchParam);
                response.IsSuccess = true;
                response.Message = Resource.TimeSlotAddedSuccessfully;
                Common.CreateLogFile("Generate Patient TimeSchedule CronJob Completed Succesfully.", ConfigSettings.GenerateRefTimeScheduleDaysFileName, logpath);
                return response;
            }
            catch (Exception ex)
            {
                Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.GenerateRefTimeScheduleDaysFileName, logpath);
                throw ex;
            }
        }

        public ServiceResponse GenerateBulkSchedules()
        {
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.GenerateBulkSchedulesLog);

            if (!ConfigSettings.Service_GenerateRefTimeScheduleDays_ON)
            {
                Common.CreateLogFile("Generate Bulk Schedule CronJob can't run because it is mark as offline from web config.", ConfigSettings.GenerateBulkSchedulesFileName, logpath);
                return null;
            }


            try
            {
                Common.CreateLogFile("Generate Bulk Schedule CronJob Started.", ConfigSettings.GenerateBulkSchedulesFileName, logpath);
                var response = new ServiceResponse();

                DateTime getDate = DateTime.Now;
                List<SearchValueData> searchParam = new List<SearchValueData>
                            {
                                new SearchValueData {Name = "StartDate",Value =Convert.ToDateTime(getDate).ToString(Constants.DbDateFormat)},
                                new SearchValueData {Name = "EndDate",Value =Convert.ToDateTime( getDate.AddDays(180)).ToString(Constants.DbDateFormat)}
                            };
                GetScalar(StoredProcedure.WindowService_CreateBulkSchedules, searchParam);

                Common.CreateLogFile("Generate Bulk Schedule CronJob Completed Succesfully.", ConfigSettings.GenerateBulkSchedulesFileName, logpath);
                return response;
            }
            catch (Exception ex)
            {
                Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.GenerateBulkSchedulesFileName, logpath);
                throw ex;
            }
        }

        public ServiceResponse UpdateGeoCode()
        {
            try
            {
                LatitudeLongitudeModel address = GetMultipleEntity<LatitudeLongitudeModel>(StoredProcedure.GetAddressOfNullLatLong);
                DateTime getDate = DateTime.Now;

                foreach (EmployeeAddress item in address.EmployeeAdressList)
                {
                    if (item.FullAddress != null)
                    {
                        LatLong latLong = Common.GetLatLongByAddress(item.FullAddress);
                        if (latLong != null)
                        {
                            List<SearchValueData> searchParam = new List<SearchValueData>
                            {
                                new SearchValueData {Name = "ID",Value =Convert.ToString(item.EmployeeID)},
                                new SearchValueData {Name = "Latitude",Value =Convert.ToString(latLong.Latitude)},
                                new SearchValueData {Name = "Longitude",Value =Convert.ToString(latLong.Longitude)},
                                new SearchValueData {Name = "Type",Value =Convert.ToString(Common.UserType.Employee)},
                                new SearchValueData {Name = "UpdatedDate",Value =Convert.ToDateTime(getDate).ToString(Constants.DbDateFormat)}
                            };
                            GetScalar(StoredProcedure.UpdateLatLong, searchParam);
                        }
                    }
                }

                foreach (ReferralAddress item in address.ReferralAddressList)
                {
                    if (item.FullAddress != null)
                    {
                        LatLong latLong = Common.GetLatLongByAddress(item.FullAddress);
                        if (latLong != null)
                        {
                            List<SearchValueData> searchParam = new List<SearchValueData>
                            {
                                new SearchValueData {Name = "ID",Value =Convert.ToString(item.ReferralID)},
                                new SearchValueData {Name = "Latitude",Value =Convert.ToString(latLong.Latitude)},
                                new SearchValueData {Name = "Longitude",Value =Convert.ToString(latLong.Longitude)},
                                new SearchValueData {Name = "Type",Value =Convert.ToString(Common.UserType.Referral)},
                                new SearchValueData {Name = "UpdatedDate",Value =Convert.ToDateTime(getDate).ToString(Constants.DbDateFormat)}
                            };
                            GetScalar(StoredProcedure.UpdateLatLong, searchParam);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.GenerateRefTimeScheduleDaysFileName);
                throw ex;
            }
            return null;
        }






        #region Automated Process for GET Latest ERA & ERA Processing


        public void ERA_ProcessLog(string message, string logFileName, double percentComplete)
        {

            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.RespiteHourseLog);

            string keyName = Constants.HC_CronJob_Download_Process_AllERA_ProcessName; ;
            Common.CreateLogFile_WithProgress(message, logFileName, logpath, keyName, percentComplete);



        }
        public ServiceResponse Download_Process_AllERA(bool IsThreadCall = false)
        {



            ServiceResponse response = new ServiceResponse();
            string prsMsg = "";
            string filePath = "";
            string logFileName = String.Format("ERA_Process_{0}.txt", DateTime.Now.ToString("MMddyyyy_mmhhss"));
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.RespiteHourseLog);

            ERA_ProcessLog("Download_Process_AllERA - Service is started. <br/>", logFileName, 1);

            #region Get Admin Employee & Get Latest ERA

            long loggedInUserID = 0;
            EmployeeDataProvider employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse empResponse = employeeDataProvider.HC_GetEmployeeByUserName(Constants.SuperAdmin);

            if (empResponse.Data != null)
                loggedInUserID = ((Employee)empResponse.Data).EmployeeID;


            if (loggedInUserID == 0)
            {
                prsMsg = string.Format("<img src='/Assets/images/error.png' alt='' height='20px' /> Download_Process_AllERA - Admin user is not found, Can't process furher.");
                ERA_ProcessLog(prsMsg, logFileName, 100);

                ERA_ProcessLog("Download_Process_AllERA - Service is completed.", logFileName, 100);
                filePath = string.Format("<a href='{0}{1}' download=''>Download Log File</a>", logpath, logFileName);
                response.Message = "Download_Process_AllERA - Admin user is not found, Can't process furher." + filePath;
                response.IsSuccess = true;
                return response;
            }



            ERA_ProcessLog("Download_Process_AllERA - Downloading latest ERA is started.", logFileName, 5);

            BatchDataProvider batchDataProvider = new BatchDataProvider();
            ServiceResponse response_ERA = batchDataProvider.GetLatestERA(loggedInUserID);

            prsMsg = string.Format("<img src='/Assets/images/{0}' alt='' height='20px' /> Download_Process_AllERA - {1}", response_ERA.IsSuccess ? "check.ico" : "error.png", response_ERA.Message);
            ERA_ProcessLog(prsMsg, logFileName, 10);
            ERA_ProcessLog("Download_Process_AllERA - Downloading latest ERA is completed.<br/>", logFileName, 10);



            #endregion



            #region Get All No ProcessedEra 
            ERA_ProcessLog("Download_Process_AllERA - Fetching List of Non Processed ERA from Database - Start", logFileName, 10);
            List<NonProcessedERA> nonProcessedERAList = new List<NonProcessedERA>();
            try
            {
                nonProcessedERAList = GetEntityList<NonProcessedERA>(StoredProcedure.HC_GetNonProcessedERA, null);
                if (nonProcessedERAList == null)
                    nonProcessedERAList = new List<NonProcessedERA>();
            }
            catch (Exception ex)
            {

                prsMsg = string.Format("<img src='/Assets/images/{0}' alt='' height='20px' /> Download_Process_AllERA - {1}", "error.png", ex.Message);
                ERA_ProcessLog(prsMsg, logFileName, 15);
            }
            ERA_ProcessLog("Download_Process_AllERA - <strong>Total ERA Found = " + nonProcessedERAList.Count + "</strong>", logFileName, 100);
            ERA_ProcessLog("Download_Process_AllERA - Fetching List of Non Processed ERA from Database - Completed<br/>", logFileName, 15);

            if (nonProcessedERAList.Count == 0)
            {
                ERA_ProcessLog("Download_Process_AllERA - No ERA file found. Service is completed.", logFileName, 100);
                filePath = string.Format("<a href='{0}{1}' download=''>Download Log File</a>", logpath, logFileName);
                response.Message = "Download_Process_AllERA - No ERA file found. Service is completed" + filePath;
                response.IsSuccess = true;
                return response;
            }
            #endregion


            #region Process The NotProcessed ERA


            if (nonProcessedERAList != null && nonProcessedERAList.Count > 0)
            {

                int TotalCount = nonProcessedERAList.Count;
                int remaingPercent = 80;





                double eachERACompletion = (double)remaingPercent / TotalCount;

                //double eachERACompletion =    Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(remaingPercent / TotalCount)));

                string ids = string.Join(",", nonProcessedERAList.Select(x => x.EraID).ToArray());


                ERA_ProcessLog("Download_Process_AllERA - Individual ERA Processing is started.<br/>", logFileName, 18);


                double currentProgressPercent = 18;
                int current = 1;
                foreach (var nonProcessedERA in nonProcessedERAList)
                {


                    ERA_ProcessLog("Service Download_Process_AllERA - ERA #" + nonProcessedERA.EraID + " , process start. #" + current, logFileName, Convert.ToInt32(currentProgressPercent));
                    ServiceResponse response_ERAProcess = batchDataProvider.ProcessERA835(nonProcessedERA.EraID, loggedInUserID);
                    currentProgressPercent = currentProgressPercent + eachERACompletion;


                    prsMsg = string.Format("<img src='/Assets/images/{0}' alt='' height='20px' /> Download_Process_AllERA - {1}", response_ERAProcess.IsSuccess ? "check.ico" : "error.png", response_ERAProcess.Message);
                    ERA_ProcessLog(prsMsg, logFileName, Convert.ToInt32(currentProgressPercent));
                    ERA_ProcessLog("Download_Process_AllERA - ERA #" + nonProcessedERA.EraID + " process complete. <br/>", logFileName,Convert.ToInt32(currentProgressPercent));

                    current = current + 1;

                    if (IsThreadCall)
                        Thread.Sleep(1);


                }
                //Common.CreateLogFile_WithProgress("Download_Process_AllERA - Individual ERA Processing is completed.", logFileName, logpath, keyName);
                ERA_ProcessLog("Download_Process_AllERA - Individual ERA Processing is completed.<br/>", logFileName, 99);
            }

            #endregion



            ERA_ProcessLog("Download_Process_AllERA - Service is completed.", logFileName, 100);
            filePath = string.Format("<a href='{0}{1}' download=''>Download Log File</a>", logpath, logFileName);
            response.Message = "Download_Process_AllERA service is completed successfully. Processed ERA count = " + nonProcessedERAList.Count + ". " + filePath;
            response.IsSuccess = true;
            return response;
        }

        #endregion


        #endregion
    }
}