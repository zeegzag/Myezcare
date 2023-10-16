using System;
using System.Collections.Generic;
using System.IO;
using HomeCareApi.Infrastructure.IDataProvider;
using HomeCareApi.Models.General;
using HomeCareApi.Models.ViewModel;

namespace HomeCareApi.Infrastructure.DataProvider
{
    public class SmsDataProvider : BaseDataProvider, ISmsDataProvider
    {
        public void SendClockInoutNotification()
        {
            string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.ClockInOutServiceLog);
            try
            {
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
                DateTime easternDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);

                List<ClockInOutSmsUser> list = GetEntityList<ClockInOutSmsUser>("GetClockInClockOutSmsList", new List<SearchValueData>
                {
                    new SearchValueData("Date",easternDateTime.ToString(Constants.DbDateTimeFormat)),
                    new SearchValueData("ScheduleStatusID",Convert.ToString((int)Common.ScheduleStatuses.Confirmed)),
                    new SearchValueData("TimeDifference",Convert.ToString(ConfigSettings.CheckClockInOutTime)),
                    new SearchValueData("ReferralStatusID",Convert.ToString((int)Common.ReferralStatuses.Active)),
                    new SearchValueData("RoleId",Convert.ToString((int)Common.Role.SuperAdmin)),
                    new SearchValueData("MaxAttemptCount",ConfigSettings.MaxSendClockInOutSms),
                    new SearchValueData("StatusInProgress",Convert.ToString((int)Common.SmsLogStatus.InProgress)),
                    new SearchValueData("Type_ClockIn",Convert.ToString((int)Common.SmsTypeEnum.ClockIn)),
                    new SearchValueData("Type_ClockOut",Convert.ToString((int)Common.SmsTypeEnum.ClockOut)),
                    new SearchValueData("ClockInMessageEmp",ConfigSettings.ClockInMessageEmp),
                    new SearchValueData("ClockOutMessageEmp",ConfigSettings.ClockOutMessageEmp),
                    new SearchValueData("ClockInMessageAdmin",ConfigSettings.ClockInMessageAdmin),
                    new SearchValueData("ClockOutMessageAdmin",ConfigSettings.ClockOutMessageAdmin),
                });

                string updatedlist = "";
                foreach (ClockInOutSmsUser clockInOutSmsUser in list)
                {
                    bool isSend = Common.SendSms(clockInOutSmsUser.MobileNumber, clockInOutSmsUser.MessageContent, null);
                    clockInOutSmsUser.IsSent = isSend;
                    if (updatedlist != "")
                        updatedlist = updatedlist + Constants.LeftArrowChar;
                    updatedlist = updatedlist + clockInOutSmsUser.ScheduleID + Constants.RightArrowChar +
                                  clockInOutSmsUser.EmployeeID + Constants.RightArrowChar +
                         ((isSend) ? (int)Common.SmsLogStatus.Sent : (int)Common.SmsLogStatus.Failed) + Constants.RightArrowChar +
                        clockInOutSmsUser.SmsType;

                }

                GetEntityList<TransactionResult>("UpdateClockInOutSmsUserLog", new List<SearchValueData>
                {
                    new SearchValueData("RecordList",updatedlist),
                    new SearchValueData("LeftArrowChar",Constants.LeftArrowChar),
                    new SearchValueData("RightArrowChar",Constants.RightArrowChar)
                });

            }
            catch (Exception e)
            {
                Common.CreateLogFile(Common.SerializeObject(e), ConfigSettings.ClockInOutServiceLogFileName, logpath);
            }
        }
    }
}