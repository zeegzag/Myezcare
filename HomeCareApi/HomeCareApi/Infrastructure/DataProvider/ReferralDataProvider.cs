using System;
using System.Collections.Generic;
using HomeCareApi.Infrastructure.IDataProvider;
using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.General;
using HomeCareApi.Models.ViewModel;
using PetaPoco;
using HomeCareApi.Resources;
using System.Linq;
using System.Web;
using System.IO;
using Zarephath.Core.Models.Entity;
using HomeCareApi.Helpers;
using System.Net;
using Zarephath.Core.Infrastructure.Utility.Fcm;
using Zarephath.Core.Models.ViewModel;

namespace HomeCareApi.Infrastructure.DataProvider
{
    public class ReferralDataProvider : BaseDataProvider, IReferralDataProvider
    {
        public ApiResponse GetReferralDetail(ApiRequest<ReferralTaskModel> request)
        {
            ApiResponse response;
            try
            {
                GetReferralViewDetail getReferralViewDetail = GetEntity<GetReferralViewDetail>(StoredProcedure.GetReferralDetail, new List<SearchValueData>
                {
                    new SearchValueData(Properties.ReferralID, Convert.ToString(request.Data.ReferralID)),
                    new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID))
                });
                getReferralViewDetail.ImageUrl = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(getReferralViewDetail.ImageUrl);
                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, getReferralViewDetail);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponseList<PatientList> GetPatientList(ApiRequest<ListModel<SearchPatient>> request)
        {
            ApiResponseList<PatientList> response;
            var timeUtc = DateTime.UtcNow;
            var abc = GetTimeZone();
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.FromIndex, Convert.ToString(((request.Data.PageIndex - 1) * request.Data.PageSize) + 1)));
                srchParam.Add(new SearchValueData(Properties.ToIndex, Convert.ToString(request.Data.PageIndex * request.Data.PageSize)));
                srchParam.Add(new SearchValueData(Properties.SortType, request.Data.SortType));
                srchParam.Add(new SearchValueData(Properties.SortExpression, request.Data.SortExpression));
                srchParam.Add(new SearchValueData(Properties.ServerCurrentDate, today.ToString(Constants.DbDateFormat)));
                srchParam.Add(new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.SearchParams.EmployeeID)));
                if (request.Data.SearchParams.StartDate != null)
                    srchParam.Add(new SearchValueData { Name = Properties.StartDate, Value = request.Data.SearchParams.StartDate.Value.ToString(Constants.DbDateFormat) });
                if (request.Data.SearchParams.EndDate != null)
                    srchParam.Add(new SearchValueData { Name = Properties.EndDate, Value = request.Data.SearchParams.EndDate.Value.ToString(Constants.DbDateFormat) });
                List<PatientList> getPatientList = GetEntityList<PatientList>(StoredProcedure.API_GetPatientList, srchParam);
                if (getPatientList != null)
                {
                    foreach (var item in getPatientList)
                    {
                        item.ImageUrl = item.ImageUrl == null ? null : string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(item.ImageUrl);
                    }
                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, GetPageList(getPatientList, request.Data.PageIndex, request.Data.PageSize, null));
                }
                else
                {
                    var getPage = new Page<PatientList>();
                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, getPage);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<PatientList>(e.Message, null);
            }
            return response;
        }

        public ApiResponse GetPatientDetail(ApiRequest<ReferralTaskModel> request)
        {
            ApiResponse response;
            try
            {
                var timeUtc = DateTime.UtcNow;
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
                var localTimeZone = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
                GetPatientDetail getReferralViewDetail = GetEntity<GetPatientDetail>(StoredProcedure.API_GetPatientDetail, new List<SearchValueData>
                {
                    new SearchValueData(Properties.ReferralID, Convert.ToString(request.Data.ReferralID)),
                    new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID)),
                    new SearchValueData{Name = Properties.ServerCurrentDate,Value = localTimeZone.ToString(Constants.DbDateTimeFormat)}
                });
                if (getReferralViewDetail != null && getReferralViewDetail.ImageUrl != null)
                {
                    getReferralViewDetail.ImageUrl = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(getReferralViewDetail.ImageUrl);
                }
                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, getReferralViewDetail);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse GetIncompletedPastVisit(ApiRequest<SearchDetail> request)
        {
            ApiResponse response;
            var timeUtc = DateTime.UtcNow;
            var abc = GetTimeZone();
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            try
            {
                List<GetPastVisitModel> getIncompletedPastVisit = GetEntityList<GetPastVisitModel>(StoredProcedure.API_GetIncompletedPastVisit, new List<SearchValueData>
                {
                    new SearchValueData{Name = Properties.ReferralID,Value = request.Data.ReferralID.ToString()},
                    new SearchValueData{Name = Properties.EmployeeID,Value = request.Data.EmployeeID.ToString()},
                    new SearchValueData{Name = Properties.CurrentDateTime,Value = today.ToString(Constants.DbDateTimeFormat)}
                });
                if (getIncompletedPastVisit.Count > 0)
                {
                    foreach (var item in getIncompletedPastVisit)
                    {
                        item.ImageUrl = item.ImageUrl == null ? null : string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(item.ImageUrl);
                    }
                }
                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, getIncompletedPastVisit);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<GetPastVisitModel>(e.Message, null);
            }
            return response;
        }

        public ApiResponseList<GetPastVisitModel> GetPastVisits(ApiRequest<ListModel<SearchDetail>> request)
        {
            ApiResponseList<GetPastVisitModel> response;
            var timeUtc = DateTime.UtcNow;
            var abc = GetTimeZone();
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            try
            {
                List<GetPastVisitModel> getPastVisitDetails = GetEntityList<GetPastVisitModel>(StoredProcedure.GetPastVisitDetail, new List<SearchValueData>
                {
                    new SearchValueData{Name = Properties.FromIndex,Value = Convert.ToString(((request.Data.PageIndex - 1)*request.Data.PageSize) + 1)},
                        new SearchValueData{Name = Properties.ToIndex,Value = Convert.ToString(request.Data.PageIndex*request.Data.PageSize)},
                        new SearchValueData{Name = Properties.SortType,Value = request.Data.SortType},
                        new SearchValueData{Name = Properties.SortExpression,Value = request.Data.SortExpression},
                        new SearchValueData{Name = Properties.ReferralID,Value = request.Data.SearchParams.ReferralID.ToString()},
                        new SearchValueData{Name = Properties.EmployeeID,Value = request.Data.SearchParams.EmployeeID.ToString()},
                        new SearchValueData{Name = Properties.IsCompletedVisit,Value = Convert.ToString(request.Data.SearchParams.IsCompletedVisit)},
                        new SearchValueData{Name = Properties.CurrentDateTime,Value = today.ToString(Constants.DbDateTimeFormat)}
                });
                if (getPastVisitDetails != null)
                {
                    foreach (var item in getPastVisitDetails)
                    {
                        item.ImageUrl = item.ImageUrl == null ? null : string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(item.ImageUrl);
                    }
                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, GetPageList(getPastVisitDetails, request.Data.PageIndex, request.Data.PageSize, null));
                }
                else
                {
                    var getPage = new Page<GetPastVisitModel>();

                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, getPage);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<GetPastVisitModel>(e.Message, null);
            }
            return response;
        }

        public ApiResponse GetIncompletedEmpVisitHistory(ApiRequest<SearchVisitHistory> request)
        {
            ApiResponse response;
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ServerCurrentDate, today.ToString(Constants.DbDateFormat)));
                srchParam.Add(new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID)));
                srchParam.Add(new SearchValueData(Properties.Name, request.Data.Name));
                if (request.Data.StartDate.HasValue)
                    srchParam.Add(new SearchValueData(Properties.StartDate, Convert.ToDateTime(request.Data.StartDate).ToString(Constants.DbDateFormat)));
                if (request.Data.EndDate.HasValue)
                    srchParam.Add(new SearchValueData(Properties.EndDate, Convert.ToDateTime(request.Data.EndDate).ToString(Constants.DbDateFormat)));


                List<EmpVisitHistory> getIncompletedEmpVisitHistory = GetEntityList<EmpVisitHistory>(StoredProcedure.API_GetIncompletedEmpVisitHistory, srchParam);

                if (getIncompletedEmpVisitHistory.Count > 0)
                {
                    foreach (var item in getIncompletedEmpVisitHistory)
                    {
                        item.ImageUrl = item.ImageUrl == null ? null : string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(item.ImageUrl);
                    }
                }
                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, getIncompletedEmpVisitHistory);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponseList<EmpVisitHistory> GetEmpVisitHistory(ApiRequest<ListModel<SearchVisitHistory>> request)
        {
            ApiResponseList<EmpVisitHistory> response;
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.FromIndex, Convert.ToString(((request.Data.PageIndex - 1) * request.Data.PageSize) + 1)));
                srchParam.Add(new SearchValueData(Properties.ToIndex, Convert.ToString(request.Data.PageIndex * request.Data.PageSize)));
                srchParam.Add(new SearchValueData(Properties.SortType, request.Data.SortType));
                srchParam.Add(new SearchValueData(Properties.SortExpression, request.Data.SortExpression));
                srchParam.Add(new SearchValueData(Properties.ServerCurrentDate, today.ToString(Constants.DbDateFormat)));
                srchParam.Add(new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.SearchParams.EmployeeID)));
                srchParam.Add(new SearchValueData(Properties.Name, request.Data.SearchParams.Name));
                if (request.Data.SearchParams.StartDate.HasValue)
                    srchParam.Add(new SearchValueData(Properties.StartDate, Convert.ToDateTime(request.Data.SearchParams.StartDate).ToString(Constants.DbDateFormat)));
                if (request.Data.SearchParams.EndDate.HasValue)
                    srchParam.Add(new SearchValueData(Properties.EndDate, Convert.ToDateTime(request.Data.SearchParams.EndDate).ToString(Constants.DbDateFormat)));


                List<EmpVisitHistory> getEmpVisitHistoryList = GetEntityList<EmpVisitHistory>(StoredProcedure.API_GetEmpVisitHistory, srchParam);

                if (getEmpVisitHistoryList != null)
                {
                    foreach (var item in getEmpVisitHistoryList)
                    {
                        item.ImageUrl = item.ImageUrl == null ? null : string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(item.ImageUrl);
                    }
                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, GetPageList(getEmpVisitHistoryList, request.Data.PageIndex, request.Data.PageSize, null));
                }
                else
                {
                    var getPage = new Page<EmpVisitHistory>();

                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, getPage);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse CheckClockOut(ApiRequest<ClockInModel> request)
        {
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);

            ApiResponse response;
            try
            {
                var result = GetScalar(StoredProcedure.API_CheckClockOut, new List<SearchValueData>
                {
                    new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID)),
                    new SearchValueData(Properties.ClockOutTime, today.ToString(Constants.DbDateTimeFormat)),
                    new SearchValueData(Properties.ClockOutBeforeTime, ConfigSettings.ClockOutBeforeTime)
                });

                var Message = string.Empty;

                response = Common.ApiCommonResponse(true, Message, StatusCode.Ok, result);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }
        // Check the Early ClockIn permission based on ScheduleID and RoleID         
        public ApiResponse CheckClockIn(ApiRequest<ClockInModel> request)
        {
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);

            ApiResponse response;
            try
            {
                var result = GetScalar(StoredProcedure.API_CheckClockIN, new List<SearchValueData>
                {
                    new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID)),
                    new SearchValueData(Properties.ClockInTime, today.ToString(Constants.DbDateTimeFormat)),
                    new SearchValueData(Properties.ClockInBeforeTime,ConfigSettings.ClockInBeforeTime ),
                    new SearchValueData(Properties.RoleID, Convert.ToString(request.Data.RoleID))
                });
                var Message = string.Empty;
                if (result.ToString() == "0")
                {
                    Message = Resource.EarlyClockInMessage;
                    response = Common.ApiCommonResponse(true, Message, StatusCode.Ok, result);
                }
                // Start Added by Sagar,26 Dec 2019:cannot Clock In for a visit past Shift End Time
                else if ((int)result == -1)
                {
                    response = Common.ApiCommonResponse(true, "", StatusCode.Ok, "-1");
                }
                // End
                else
                {
                    response = Common.ApiCommonResponse(true, Message, StatusCode.Ok, result);
                }

            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse SetEmployeeVisitTime(ApiRequest<ClockInModel> request)
        {
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);

            ApiResponse response;
            try
            {
                var result = GetScalar(StoredProcedure.SetEmployeeVisitTime, new List<SearchValueData>
                {
                    new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID)),
                    new SearchValueData(Properties.ReferralID, Convert.ToString(request.Data.ReferralID)),
                    new SearchValueData(Properties.EmployeeId, Convert.ToString(request.Data.EmployeeID)),
                    new SearchValueData(Properties.Type, Convert.ToString(request.Data.Type)),
                    new SearchValueData(Properties.IsByPass, Convert.ToString(request.Data.IsByPass)),
                    new SearchValueData(Properties.IsCaseManagement, Convert.ToString(request.Data.IsCaseManagement)),
                    new SearchValueData(Properties.IsApprovalRequired, Convert.ToString(request.Data.IsApprovalRequired)),
                    new SearchValueData(Properties.ActionTaken, request.Data.IsApprovalRequired ? Convert.ToString((int)ClockInModel.BypassAction.Pending) : null),
                    new SearchValueData(Properties.EarlyClockOutComment, request.Data.EarlyClockOutComment),
                    new SearchValueData(Properties.ByPassReason, request.Data.ByPassReason),
                    new SearchValueData(Properties.Lat, Convert.ToString(request.Data.Lat)),
                    new SearchValueData(Properties.Long, Convert.ToString(request.Data.Long)),
                    new SearchValueData(Properties.BeforeClockIn, Constants.BeforeClockIn),
                    new SearchValueData(Properties.ClockInTime, today.ToString(Constants.DbDateTimeFormat)), //DateTime.UtcNow.ToString(Constants.DbDateTimeFormat)
                    new SearchValueData(Properties.ClockOutTime, (request.Data.Type==2)?today.ToString(Constants.DbDateTimeFormat):null),
                    new SearchValueData(Properties.Distance,ConfigSettings.Distance),
                    new SearchValueData(Properties.SystemID, Common.GetHostAddress()),
                    new SearchValueData(Properties.EarlyClockInComment, Convert.ToString(request.Data.EarlyClockInComment)),
                    new SearchValueData(Properties.IsEarlyClockIn, Convert.ToString(request.Data.IsEarlyClockIn)),
            });
                //var Message = (request.Data.Type == 1) ? Resource.ClockIn : Resource.ClockOut;
                var Message = string.Empty;

                if ((int)result > 0)
                {
                    Message = (request.Data.Type == 1) ? Resource.ClockInSuccessfully : Resource.ClockOutSuccessfully;
                    response = Common.ApiCommonResponse(true, Message, StatusCode.Ok, result);
                }
                else if ((int)result == -1)
                {
                    Message = (request.Data.Type == 1) ? Resource.ClockInFailed : Resource.ClockOutFailed;
                    response = Common.ApiCommonResponse(false, Message, StatusCode.Ok, result);
                }
                else if ((int)result == -3)
                {
                    Message = Resource.EarlyClockInMessage;
                    response = Common.ApiCommonResponse(false, Message, StatusCode.Ok, result);
                }
                else if ((int)result == -4)
                {
                    Message = Resource.CompleteRequiredTask;
                    response = Common.ApiCommonResponse(false, Message, StatusCode.Ok, result);
                }
                else if ((int)result == -5)
                {
                    Message = request.Data.Type == 1? string.Format("Cannot clock-in {0}", Resource.exceededClockouttimeLimit) : string.Format("Cannot clock-out {0}", Resource.exceededClockouttimeLimit);
                    response = Common.ApiCommonResponse( false, Message, StatusCode.Ok, result);
                }
                else
                {
                    Message = Resource.MissingPatientCoordinates;
                    response = Common.ApiCommonResponse(false, Message, StatusCode.Ok, result);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponseList<PatientList> RAL_GetPatientList(ApiRequest<ListModel<SearchPatient>> request)
        {
            ApiResponseList<PatientList> response;
            var timeUtc = DateTime.UtcNow;
            var abc = GetTimeZone();
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.FromIndex, Convert.ToString(((request.Data.PageIndex - 1) * request.Data.PageSize) + 1)));
                srchParam.Add(new SearchValueData(Properties.ToIndex, Convert.ToString(request.Data.PageIndex * request.Data.PageSize)));
                srchParam.Add(new SearchValueData(Properties.SortType, request.Data.SortType));
                srchParam.Add(new SearchValueData(Properties.SortExpression, request.Data.SortExpression));
                srchParam.Add(new SearchValueData(Properties.ServerCurrentDate, today.ToString(Constants.DbDateFormat)));
                srchParam.Add(new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.SearchParams.EmployeeID)));
                srchParam.Add(new SearchValueData(Properties.FacilityID, Convert.ToString(request.Data.SearchParams.FacilityID)));

                if (request.Data.SearchParams.StartDate != null)
                    srchParam.Add(new SearchValueData { Name = Properties.StartDate, Value = request.Data.SearchParams.StartDate.Value.ToString(Constants.DbDateFormat) });
                if (request.Data.SearchParams.EndDate != null)
                    srchParam.Add(new SearchValueData { Name = Properties.EndDate, Value = request.Data.SearchParams.EndDate.Value.ToString(Constants.DbDateFormat) });
                List<PatientList> getPatientList = GetEntityList<PatientList>(StoredProcedure.API_RAL_GetPatientList, srchParam);
                if (getPatientList != null)
                {
                    foreach (var item in getPatientList)
                    {
                        item.ImageUrl = item.ImageUrl == null ? null : string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(item.ImageUrl);
                    }
                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, GetPageList(getPatientList, request.Data.PageIndex, request.Data.PageSize, null));
                }
                else
                {
                    var getPage = new Page<PatientList>();
                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, getPage);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<PatientList>(e.Message, null);
            }
            return response;
        }


        //public void SendClockInClockOutNotification(string FcmTokenId, string DeviceType, string Body, string CompanyName, int NotificationType)
        //{
        //    FcmManager fcmManager = new FcmManager(new FcmManagerOptions
        //    {
        //        AuthenticationKey = ConfigSettings.FcmAuthenticationKey,
        //        SenderId = ConfigSettings.FcmSenderId
        //    });

        //    var fcmResponseIos = fcmManager.SendMessage(new FcmMessage
        //    {
        //        RegistrationIds = new List<string> { FcmTokenId },
        //        Notification = DeviceType.ToLower() == Constants.ios ? new FcmNotification
        //        {
        //            Body = Body,
        //            Title = CompanyName
        //        } : null,
        //        Data = new PatientResignatureNotificationModel
        //        {
        //            SiteName = CompanyName,
        //            Body = Body,
        //            NotificationType = NotificationType
        //        },
        //    });
        //}


        public ApiResponse UpdateEmployeeVisitTime(ApiRequest<EmpVisitModel> request)
        {
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);

            ApiResponse response;
            try
            {
                var result = GetScalar(StoredProcedure.API_UpdateEmployeeVisitTime, new List<SearchValueData>
                {
                    new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID)),
                    new SearchValueData(Properties.EmployeeVisitID, Convert.ToString(request.Data.EmployeeVisitID)),
                    new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID)),
                    new SearchValueData(Properties.ClockInTime, request.Data.ClockInTime),
                    new SearchValueData(Properties.ClockOutTime, request.Data.ClockOutTime)
                });
                var Message = string.Empty;

                if ((int)result == 1)
                {
                    Message = Resource.ClockInOutTimeUpdatedSuccessfully;
                    response = Common.ApiCommonResponse(true, Message, StatusCode.Ok, result);
                }
                else if ((int)result == -2)
                {
                    Message = Resource.ClockInLesserThanScheduleEnd;
                    response = Common.ApiCommonResponse(false, Message, StatusCode.Ok, result);
                }
                else if ((int)result == -3)
                {
                    Message = Resource.ClockOutGreaterThanClockIn;
                    response = Common.ApiCommonResponse(false, Message, StatusCode.Ok, result);
                }
                else
                {
                    Message = Resource.VisitTimeGreaterThanServiceTime;
                    response = Common.ApiCommonResponse(false, Message, StatusCode.Ok, result);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse PatientServiceDenied(ApiRequest<PatientModel> request)
        {
            ApiResponse response;
            try
            {
                var result = GetScalar(StoredProcedure.API_PatientServiceDenied, new List<SearchValueData>
                {
                    new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID)),
                    new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID)),
                    new SearchValueData(Properties.Note, request.Data.Note)
                });
                var Message = Resource.ServiceDeniedSuccessfully;

                response = Common.ApiCommonResponse(true, Message, StatusCode.Ok, result);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        //-------------------------------OLD CODE--------------------------------------------
        //public ApiResponse GetReferralTasks(ApiRequest<ReferralTaskModel> request)
        //{
        //    ApiResponse response;
        //    try
        //    {
        //        ReferralTaskListModel referralTasks = GetMultipleEntity<ReferralTaskListModel>(StoredProcedure.GetReferralTasks, new List<SearchValueData>
        //        {
        //            new SearchValueData(Properties.ReferralID, Convert.ToString(request.Data.ReferralID)),
        //            new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID)),
        //            new SearchValueData(Properties.VisitTaskType, Convert.ToString(ReferralTaskModel.TaskType.Task))
        //        });
        //        response = Common.ApiCommonResponse(true, "", StatusCode.Ok, referralTasks);
        //    }
        //    catch (Exception e)
        //    {
        //        response = Common.InternalServerError(e.Message);
        //    }
        //    return response;
        //}


        public ApiResponse GetReferralTasks(ApiRequest<ReferralTaskModel> request)
        {
            ApiResponse response;
            try
            {
                GetTaskListModel referralTasks = GetMultipleEntity<GetTaskListModel>(StoredProcedure.GetReferralTasks, new List<SearchValueData>
                {
                    new SearchValueData(Properties.ReferralID, Convert.ToString(request.Data.ReferralID)),
                    new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID)),
                    new SearchValueData(Properties.VisitTaskType, Convert.ToString(ReferralTaskModel.TaskType.Task))
                });

                //--------------------------------------Group By Code-----------------------------------------------------

                //List<Category> categoryList = referralTasks.TaskLists.ToList().GroupBy(c => new
                //{
                //    c.CategoryId,
                //    c.CategoryName,
                //}).Select(grp => new Category
                //{
                //    CategoryId=grp.Key.CategoryId,
                //    CategoryName=grp.Key.CategoryName,
                //    TaskLists=grp.ToList()
                //}).ToList();


                //foreach(Category category in categoryList)
                //{
                //    List<SubCategory> subCategoryList = category.TaskLists.GroupBy(sc => new
                //    {
                //        sc.SubCategoryId,
                //        sc.SubCategoryName,
                //    }).Select(grp => new SubCategory
                //    {
                //        SubCategoryId = grp.Key.SubCategoryId,
                //        SubCategoryName = grp.Key.SubCategoryName,
                //        TaskLists = grp.ToList()
                //    }).ToList();

                //    category.SubCategory.AddRange(subCategoryList.OrderBy(c => c.SubCategoryName).ToList());
                //}

                //CategoryResponse cat_response = new CategoryResponse();
                //cat_response.CategoryList = categoryList.OrderBy(c=>c.CategoryName).ToList();
                //cat_response.EmployeeVisitID = referralTasks.EmployeeVisitID;

                //response = Common.ApiCommonResponse(true, "", StatusCode.Ok, cat_response);
                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, referralTasks);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse GetTaskFormList(ApiRequest<TaskFormModel> request)
        {
            ApiResponse response;
            ConfigEBFormModel model = new ConfigEBFormModel();
            try
            {
                List<TaskFormMappingModel> taskForms = GetEntityList<TaskFormMappingModel>(StoredProcedure.API_GetTaskFormList, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeVisitID, Convert.ToString(request.Data.EmployeeVisitID)),
                    new SearchValueData(Properties.ReferralTaskMappingID, Convert.ToString(request.Data.ReferralTaskMappingID))
                });

                if (taskForms.Count > 0)
                {
                    foreach (TaskFormMappingModel item in taskForms)
                    {
                        if (item.IsOrbeonForm)
                        {
                            string mode = item.IsFilled ? "edit/"+ item.OrbeonFormID : "new";
                            string url = string.Format("{0}/fr/{1}&orbeon-embeddable=true&EmployeeID={2}&ReferralID={3}&OrganizationID={4}&DomainName={5}", 
                                ConfigSettings.OrbeonBaseUrl,
                                string.Format("{0}/{1}?form-version={2}", item.NameForUrl, mode ,item.Version),
                                request.Data.EmployeeID,
                                request.Data.ReferralID,
                                OrganizationData.OrganizationID,
                                OrganizationData.DomainName);
                            item.FormURL = string.Format("{0}/resources/forms/ezcare/embed.html?formURL={1}", ConfigSettings.OrbeonBaseUrl, HttpUtility.UrlEncode(url));
                        }
                        else if (item.IsInternalForm)
                        {
                            item.FormURL = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Constants.LoadHtmlFormURL
                            + "?FormURL=" + HttpUtility.UrlEncode(item.InternalFormPath)
                            + "&OrgPageID=" + "Mobile"
                            + "&IsEditMode=" + "true"
                            + "&EmployeeID=" + request.Data.EmployeeID
                            + "&ReferralID=" + request.Data.ReferralID
                            + "&EBriggsFormID=" + item.EBFormID
                            + "&OriginalEBFormID=" + item.EBFormID
                            + "&FormId=" + item.FormId
                            + "&EbriggsFormMppingID=" + "0";
                        }
                        else
                        {
                            var newFormURL = model.EBBaseSiteUrl + "/form/" + item.NameForUrl + "?version=" + item.Version + "&PageId=Mobile";
                            item.FormURL = model.MyezcareFormsUrl + "?formURL=" + HttpUtility.UrlEncode(newFormURL);
                        }
                    }
                }

                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, taskForms);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse SaveTaskForm(ApiRequest<SaveFormModel> request)
        {
            ApiResponse response;
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.Data.ReferralID)));
                srchParam.Add(new SearchValueData(Properties.TaskFormMappingID, Convert.ToString(request.Data.TaskFormMappingID)));
                srchParam.Add(new SearchValueData(Properties.ReferralTaskMappingID, Convert.ToString(request.Data.ReferralTaskMappingID)));
                srchParam.Add(new SearchValueData(Properties.EBriggsFormID, request.Data.EBriggsFormID));
                srchParam.Add(new SearchValueData(Properties.OriginalEBFormID, request.Data.OriginalEBFormID));
                srchParam.Add(new SearchValueData(Properties.FormId, request.Data.FormId));
                srchParam.Add(new SearchValueData(Properties.ServerCurrentDateTime, today.ToString(Constants.DbDateTimeFormat)));
                srchParam.Add(new SearchValueData(Properties.LoggedInID, request.Data.EmployeeID.ToString()));
                srchParam.Add(new SearchValueData(Properties.SystemID, Common.GetHostAddress()));

                GetScalar(StoredProcedure.API_SaveTaskForm, srchParam);

                response = Common.ApiCommonResponse(true, Resource.FormSavedSuccessfully, StatusCode.Ok, null);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse SaveTaskOrbeonForm(ApiRequest<SaveOrbeonFormModel> request)
        {
            ApiResponse response;
            try
            {
                var subSectionName = "Monthly Visit - " + request.Data.ClockInTime.ToString("MM/dd/yyyy");
                IDocumentDataProvider _documentDataProvider = new DocumentDataProvider();
                if (request.Data.ComplianceID == 0)
                {
                    List<SearchValueData> searchList = new List<SearchValueData>();
                    searchList.Add(new SearchValueData(Properties.DocumentName, subSectionName));
                    searchList.Add(new SearchValueData(Properties.ParentID, Convert.ToString(Constants.VisitsComplianceID)));
                    var compliance = GetEntity<Models.Entity.Compliance>(searchList);
                    if (compliance != null)
                    {
                        request.Data.ComplianceID = compliance.ComplianceID;
                    }
                    else
                    {
                        // Create Sub Section
                        ApiRequest<AddDirSubDirModal> subSectionReq = new ApiRequest<AddDirSubDirModal>()
                        {
                            Token = request.Token,
                            Key = request.Key,
                            CompanyName = request.CompanyName,
                            Data = new AddDirSubDirModal()
                            {
                                UserType = Common.UserType.Referral.ToString(),
                                Name = subSectionName,
                                Value = string.Format("#{0:X6}", new Random().Next(0x1000000)),
                                DocumentationType = 1, //Internal
                                ParentID = Constants.VisitsComplianceID
                            }
                        };
                        var secRes = _documentDataProvider.SaveSectionSubSection(subSectionReq);
                        if (!secRes.IsSuccess)
                        { return secRes; }

                        request.Data.ComplianceID = (long)secRes.Data;
                    }
                }

                // Save Referral Document Orbeon
                var res = _documentDataProvider.SaveOrbeonForm(request) as ApiResponse<Models.Entity.ReferralDocument>;
                if (!res.IsSuccess)
                { return res; }

                // Add Referral Task Form
                var doc = res.Data as Models.Entity.ReferralDocument;
                if (request.Data.ReferralTaskFormMappingID == 0)
                {
                    List<SearchValueData> searchList = new List<SearchValueData>();
                    searchList.Add(new SearchValueData(Properties.EmployeeVisitID, Convert.ToString(request.Data.EmployeeVisitID)));
                    searchList.Add(new SearchValueData(Properties.ReferralTaskMappingID, Convert.ToString(request.Data.ReferralTaskMappingID)));
                    searchList.Add(new SearchValueData(Properties.TaskFormMappingID, Convert.ToString(request.Data.TaskFormMappingID)));
                    searchList.Add(new SearchValueData(Properties.ReferralDocumentID, Convert.ToString(doc.ReferralDocumentID)));
                    GetScalar(StoredProcedure.API_AddReferralTaskForm, searchList);
                }
                response = Common.ApiCommonResponse(true, Resource.FormSavedSuccessfully, StatusCode.Ok, null);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse DeleteTaskOrbeonForm(ApiRequest<DeleteOrbeonFormModel> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var searchParameter = new List<SearchValueData>();
                searchParameter.Add(new SearchValueData(Properties.ReferralTaskFormMappingID, Convert.ToString(request.Data.ReferralTaskFormMappingID)));
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.API_DeleteTaskOrbeonForm, searchParameter);
                if (result.TransactionResultId > 0)
                {
                    response = Common.ApiCommonResponse(true, Resource.DeletedSuccessfully, StatusCode.Ok);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, result.ErrorMessage, StatusCode.BadRequest);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse GetReferralConclusions(ApiRequest<ReferralTaskModel> request)
        {
            ApiResponse response;
            try
            {
                GetConclusionListModel referralTasks = GetMultipleEntity<GetConclusionListModel>(StoredProcedure.GetReferralTasks, new List<SearchValueData>
                {
                    new SearchValueData(Properties.ReferralID, Convert.ToString(request.Data.ReferralID)),
                    new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID)),
                    new SearchValueData(Properties.VisitTaskType, Convert.ToString(ReferralTaskModel.TaskType.Conclusion))
                });

                List<Category> categoryList = referralTasks.TaskLists.ToList().GroupBy(c => new
                {
                    c.CategoryId,
                    c.CategoryName,
                }).Select(grp => new Category
                {
                    CategoryId = grp.Key.CategoryId,
                    CategoryName = grp.Key.CategoryName,
                    TaskLists = grp.ToList()
                }).ToList();


                foreach (Category category in categoryList)
                {
                    List<SubCategory> subCategoryList = category.TaskLists.GroupBy(sc => new
                    {
                        sc.SubCategoryId,
                        sc.SubCategoryName,
                    }).Select(grp => new SubCategory
                    {
                        SubCategoryId = grp.Key.SubCategoryId,
                        SubCategoryName = grp.Key.SubCategoryName,
                        TaskLists = grp.ToList()
                    }).ToList();

                    category.SubCategory.AddRange(subCategoryList);
                }

                CategoryResponse cat_response = new CategoryResponse();
                cat_response.CategoryList = categoryList;
                cat_response.EmployeeVisitID = referralTasks.FinalComment.EmployeeVisitID;
                cat_response.SurveyComment = referralTasks.FinalComment.SurveyComment;

                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, cat_response);

            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;

        }

        public ApiResponse AddTask(ApiRequest<AddTaskModel> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var searchParameter = new List<SearchValueData>();
                searchParameter.Add(new SearchValueData(Properties.EmployeeVisitNoteID, Convert.ToString(request.Data.EmployeeVisitNoteID)));
                searchParameter.Add(new SearchValueData(Properties.ReferralTaskMappingID, Convert.ToString(request.Data.ReferralTaskMappingID)));
                searchParameter.Add(new SearchValueData(Properties.EmployeeVisitID, Convert.ToString(request.Data.EmployeeVisitID)));
                searchParameter.Add(new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID)));
                searchParameter.Add(new SearchValueData(Properties.Description, request.Data.Description));
                searchParameter.Add(new SearchValueData(Properties.ServiceTime, Convert.ToString(request.Data.ServiceTime)));
                searchParameter.Add(new SearchValueData(Properties.SetAsIncomplete, Convert.ToString(request.Data.setAsIncomplete)));
                searchParameter.Add(new SearchValueData(Properties.PatientResignatureFlag, Convert.ToString(GetPatientResignatureFlag())));
                searchParameter.Add(new SearchValueData(Properties.SystemID, Common.GetHostAddress()));
                searchParameter.Add(new SearchValueData(Properties.SimpleTaskType, Convert.ToString(request.Data.SimpleTaskType)));
                var data = GetScalar(StoredProcedure.AddTask, searchParameter);

                int i = Convert.ToInt32(data);
                if (i == 1)
                {
                    //response = Common.ApiCommonResponse(true, Resource.TaskAddedSuccessfully, StatusCode.Ok, data);
                    var notesrchParam = new List<SearchValueData>();
                    notesrchParam.Add(new SearchValueData(Properties.EmployeeVisitID, Convert.ToString(request.Data.EmployeeVisitID)));
                    notesrchParam.Add(new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID)));
                    notesrchParam.Add(new SearchValueData(Properties.SystemID, Common.GetHostAddress()));
                    TransactionResult result = AddNote(notesrchParam);
                    if (result.TransactionResultId == 1)
                    {
                        //if (request.Data.setAsIncomplete)
                        if (request.Data.setAsIncomplete && GetPatientResignatureFlag())
                        {
                            //SendNotification(request);
                        }
                        response = Common.ApiCommonResponse(true, Resource.TaskAddedSuccessfully, StatusCode.Ok, data);
                    }
                    else
                    {
                        response = Common.ApiCommonResponse(false, result.ErrorMessage, StatusCode.Ok, data);
                    }
                }
                else if (i == -1)
                {
                    response = Common.ApiCommonResponse(false, Resource.TotalTimeExceeded, StatusCode.Ok, data);
                }
                else
                {
                    var Message = Resource.ServiceGreaterThan + " " + data + " " + Resource.Minutes;
                    response = Common.ApiCommonResponse(false, Message, StatusCode.Ok, data);
                }

            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse DeleteTask(ApiRequest<DeleteTaskModel> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var searchParameter = new List<SearchValueData>();
                searchParameter.Add(new SearchValueData(Properties.EmployeeVisitNoteID, Convert.ToString(request.Data.EmployeeVisitNoteID)));
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.DeleteTask, searchParameter);
                if (result.TransactionResultId > 0)
                {
                    response = Common.ApiCommonResponse(true, Resource.TaskDeletedSuccessfully, StatusCode.Ok);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, result.ErrorMessage, StatusCode.BadRequest);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public void SendNotification(ApiRequest<AddTaskModel> request)
        {
            var domainName = request.CompanyName;
            var notificationType = (int)Mobile_Notification.NotificationTypes.PatientResignature;

            PatientResignatureNotificationDetails detail = GetEntity<PatientResignatureNotificationDetails>(StoredProcedure.GetEmpDetailsForNotification, new List<SearchValueData>
                {
                    new SearchValueData("EmployeeVisitID", Convert.ToString(request.Data.EmployeeVisitID)),
                    new SearchValueData("NotificationType",Convert.ToString(notificationType)),
                    new SearchValueData("ServerDateTime",DateTime.UtcNow.ToString(Constants.DbDateTimeFormat)),
                    new SearchValueData("NotificationStatus",Convert.ToString((int)Mobile_Notification.NotificationStatuses.Sent)),
                    new SearchValueData("LoggedInId",Convert.ToString(request.Data.EmployeeID)),
                });

            FcmManager fcmManager = new FcmManager(new FcmManagerOptions
            {
                AuthenticationKey = ConfigSettings.FcmAuthenticationKey,
                SenderId = ConfigSettings.FcmSenderId
            });

            var fcmResponseIos = fcmManager.SendMessage(new FcmMessage
            {
                RegistrationIds = new List<string> { detail.FcmTokenId },
                Notification = detail.DeviceType.ToLower() == Constants.ios ? new FcmNotification
                {
                    Body = "Test",
                    Title = domainName
                } : null,
                Data = new PatientResignatureNotificationModel
                {
                    SiteName = domainName,
                    Body = "Test",
                    NotificationType = notificationType,
                    ReferralID = detail.ReferralID,
                    ScheduleID = detail.ScheduleID,
                    Editable = detail.Editable
                },
            });
        }

        public TransactionResult AddNote(List<SearchValueData> searchParams)
        {
            TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.API_AddNote, searchParams);
            return result;
        }

        public ApiResponse GetTaskList(ApiRequest<TaskListModel> request)
        {
            ApiResponse response;
            try
            {
                List<TaskList> referralTasks = GetEntityList<TaskList>(StoredProcedure.GetTaskList, new List<SearchValueData>
                {
                    new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID)),
                    new SearchValueData(Properties.ReferralID, Convert.ToString(request.Data.ReferralID)),
                    new SearchValueData(Properties.VisitTaskType, Convert.ToString(ReferralTaskModel.TaskType.Task))
                });
                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, referralTasks);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse CheckRequiredTask(ApiRequest<TaskListModel> request)
        {
            ApiResponse response;
            try
            {
                var result = (int)GetScalar(StoredProcedure.API_CheckRequiredTask, new List<SearchValueData>
                {
                    new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID)),
                    new SearchValueData(Properties.ReferralID, Convert.ToString(request.Data.ReferralID)),
                    new SearchValueData(Properties.VisitTaskType, Convert.ToString(ReferralTaskModel.TaskType.Task))
                });
                if (result > 0)
                {
                    response = Common.ApiCommonResponse(true, "", StatusCode.Ok);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, Resource.CompleteRequiredTask, StatusCode.Ok, null);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse AddConclusion(ApiRequest<ConclusionDetailList> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                if (request.Data.ConclusionDetail.Count > 0)
                {
                    foreach (ConclusionDetail cd in request.Data.ConclusionDetail)
                    {
                        searchList.Add(new SearchValueData { Name = Properties.ReferralTaskMappingID, Value = Convert.ToString(cd.ReferralTaskMappingID) });
                        searchList.Add(new SearchValueData { Name = Properties.Description, Value = cd.Answer });
                        searchList.Add(new SearchValueData { Name = Properties.AlertComment, Value = cd.AlertComment });
                        searchList.Add(new SearchValueData { Name = Properties.EmployeeVisitID, Value = Convert.ToString(request.Data.EmployeeVisitID) });
                        searchList.Add(new SearchValueData { Name = Properties.ScheduleID, Value = Convert.ToString(request.Data.ScheduleID) });
                        searchList.Add(new SearchValueData { Name = Properties.SurveyComment, Value = request.Data.Conclusion });
                        searchList.Add(new SearchValueData { Name = Properties.EmployeeID, Value = Convert.ToString(request.Data.EmployeeID) });
                        searchList.Add(new SearchValueData { Name = Properties.SystemID, Value = Common.GetHostAddress() });
                        GetScalar(StoredProcedure.AddConclusion, searchList);
                        searchList = new List<SearchValueData>();
                    }
                    response = Common.ApiCommonResponse(true, Resource.ConclusionAddedSuccesfully, StatusCode.Ok, null);
                }
                else
                {
                    searchList.Add(new SearchValueData { Name = Properties.EmployeeVisitID, Value = Convert.ToString(request.Data.EmployeeVisitID) });
                    searchList.Add(new SearchValueData { Name = Properties.ScheduleID, Value = Convert.ToString(request.Data.ScheduleID) });
                    searchList.Add(new SearchValueData { Name = Properties.SurveyComment, Value = request.Data.Conclusion });
                    searchList.Add(new SearchValueData { Name = Properties.EmployeeID, Value = Convert.ToString(request.Data.EmployeeID) });
                    GetScalar(StoredProcedure.UpdateSurveyDetail, searchList);
                    response = Common.ApiCommonResponse(true, Resource.ConclusionAddedSuccesfully, StatusCode.Ok, null);

                    //response = Common.ApiCommonResponse(false, Resource.SomethingWentWrong, StatusCode.Ok, null);
                }
                //GetScalar(StoredProcedure.AddConclusionFeedback, new List<SearchValueData>
                //{
                //    new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID)),
                //    new SearchValueData(Properties.Conclusion, request.Data.Conclusion)
                //});
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse OnGoingVisit(ApiRequest<ReferralTaskModel> request)
        {
            ApiResponse response;
            try
            {
                OnGoingVisitDetail onGoingVisit = GetEntity<OnGoingVisitDetail>(StoredProcedure.GetOnGoingVisitDetail, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID)),
                    new SearchValueData{Name = Properties.CurrentDateTime ,Value = request.Data.CurrentTime},
                    new SearchValueData{Name = Properties.ScheduleID ,Value = Convert.ToString(request.Data.ScheduleID)},
                });

                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, onGoingVisit);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse CheckPatientVisit(ApiRequest<ReferralTaskModel> request)
        {
            ApiResponse response;
            try
            {
                PatientVisitStatus result = new PatientVisitStatus();
                OnGoingVisitDetail onGoingVisit = GetEntity<OnGoingVisitDetail>(StoredProcedure.GetOnGoingVisitDetail, new List<SearchValueData>
                {
                    new SearchValueData{Name = Properties.CurrentDateTime ,Value = request.Data.CurrentTime},
                    new SearchValueData{Name = Properties.ScheduleID ,Value = Convert.ToString(request.Data.ScheduleID)},
                });
                if (onGoingVisit != null)
                {
                    result.IsValid = !onGoingVisit.IsPCACompleted;
                    result.SurveyCompleted = onGoingVisit.SurveyCompleted;
                    result.IsClockOut = onGoingVisit.ClockOutTime.HasValue;
                }
                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, result);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse GetPatientLocationDetail(ApiRequest<PatientModel> request)
        {
            ApiResponse response;
            try
            {

                SecurityDataProvider sc = new SecurityDataProvider();
                OrganizationSetting orgSettings = sc.GetOrganizationDetail();

                PatientDetails patientDetails = GetEntity<PatientDetails>(StoredProcedure.API_GetPatientLocationDetail, new List<SearchValueData>
                {
                    new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID))
                });
                patientDetails.Contact = orgSettings.TwilioFromNo;//string.IsNullOrEmpty(orgSettings.TwilioFromNo);
                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, patientDetails);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }


        public ApiResponseList<GetIMListModel> GetInternalMsgList(ApiRequest<ListModel<SearchIMModel>> request)
        {
            ApiResponseList<GetIMListModel> response;
            try
            {
                var list = new List<SearchValueData>
                {
                    new SearchValueData{Name = Properties.FromIndex,Value = Convert.ToString(((request.Data.PageIndex - 1)*request.Data.PageSize) + 1)},
                    new SearchValueData{Name = Properties.ToIndex,Value = Convert.ToString(request.Data.PageIndex*request.Data.PageSize)},
                    new SearchValueData{Name = Properties.SortType,Value = request.Data.SortType},
                    new SearchValueData{Name = Properties.SortExpression,Value = request.Data.SortExpression},
                    new SearchValueData{Name = Properties.EmployeeID,Value = request.Data.SearchParams.EmployeeID.ToString()},
                    new SearchValueData{Name = Properties.MessageType,Value = request.Data.SearchParams.MessageType                        }
                };
                List<GetIMListModel> listItem = GetEntityList<GetIMListModel>(StoredProcedure.GetInternalMsgList, list);
                if (listItem != null)
                {
                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, GetPageList(listItem, request.Data.PageIndex, request.Data.PageSize, null));
                }
                else
                {
                    var getPage = new Page<GetIMListModel>();

                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, getPage);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<GetIMListModel>(e.Message, null);
            }
            return response;
        }

        public ApiResponse ResolvedInternalMsg(ApiRequest<IMResolvedModel> request)
        {
            ApiResponse response;
            try
            {
                List<SearchValueData> data = new List<SearchValueData>();
                data.Add(new SearchValueData { Name = Properties.ReferralInternalMessageID, Value = Convert.ToString(request.Data.ReferralInternalMessageID), IsEqual = true });
                data.Add(new SearchValueData { Name = Properties.ResolvedComment, Value = request.Data.ResolvedComment, IsEqual = true });

                GetScalar(StoredProcedure.ResolvedInternalMsg, data);
                response = Common.ApiCommonResponse(true, Resource.MessageResolved, StatusCode.Ok);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse OnHomePageLoad(ApiRequest<UnreadMsgCountModel> request)
        {
            ApiResponse response;
            try
            {
                MenuPermissionModel model = GetMultipleEntity<MenuPermissionModel>(StoredProcedure.API_UnreadMsgCount, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID))
                });
                if(!model.Employee.IsActive || model.Employee.IsDeleted)
                {
                    response = Common.ApiCommonResponse<LoginDetailResponse>(false, Resource.InactiveAccount, StatusCode.Ok,null);
                    return response;
                }

                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, model);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse GetEmployeePatientList(ApiRequest<SearchIMModel> request)
        {
            ApiResponse response;
            try
            {
                EmpRefList data = GetMultipleEntity<EmpRefList>(StoredProcedure.API_GetEmpRefList, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID))
                });

                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, data);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse SendInternalMessage(ApiRequest<InternalMessage> request)
        {
            ApiResponse response;
            try
            {
                List<SearchValueData> data = new List<SearchValueData>();
                data.Add(new SearchValueData(Properties.Assignee, Convert.ToString(request.Data.Assignee)));
                data.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.Data.ReferralID)));
                data.Add(new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID)));
                data.Add(new SearchValueData(Properties.Message, request.Data.Message));
                data.Add(new SearchValueData(Properties.SystemID, Common.GetHostAddress()));

                GetScalar(StoredProcedure.API_SendInternalMessage, data);
                response = Common.ApiCommonResponse(true, Resource.InternalMessageSent, StatusCode.Ok);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponseList<GetIMListModel> SentInternalMsgList(ApiRequest<ListModel<SearchIMModel>> request)
        {
            ApiResponseList<GetIMListModel> response;
            try
            {
                var list = new List<SearchValueData>
                {
                    new SearchValueData{Name = Properties.FromIndex,Value = Convert.ToString(((request.Data.PageIndex - 1)*request.Data.PageSize) + 1)},
                    new SearchValueData{Name = Properties.ToIndex,Value = Convert.ToString(request.Data.PageIndex*request.Data.PageSize)},
                    new SearchValueData{Name = Properties.SortType,Value = request.Data.SortType},
                    new SearchValueData{Name = Properties.SortExpression,Value = request.Data.SortExpression},
                    new SearchValueData{Name = Properties.EmployeeID,Value = request.Data.SearchParams.EmployeeID.ToString()},
                    new SearchValueData{Name = Properties.MessageType,Value = request.Data.SearchParams.MessageType                        }
                };
                List<GetIMListModel> listItem = GetEntityList<GetIMListModel>(StoredProcedure.API_SentInternalMsgList, list);
                if (listItem != null)
                {
                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, GetPageList(listItem, request.Data.PageIndex, request.Data.PageSize, null));
                }
                else
                {
                    var getPage = new Page<GetIMListModel>();

                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, getPage);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<GetIMListModel>(e.Message, null);
            }
            return response;
        }

        #region PCATimeSheet
        public ApiResponse GetBeneficiaryDetail(ApiRequest<ReferralTaskModel> request)
        {
            ApiResponse response;
            try
            {
                BeneficiaryDetail model = GetEntity<BeneficiaryDetail>(StoredProcedure.API_GetBenficiaryDetail, new List<SearchValueData>
                {
                    new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID))
                });

                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, model);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse SaveBeneficiaryDetail(ApiRequest<BeneficiaryDetail> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                GetScalar(StoredProcedure.API_SaveBeneficiaryDetail, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeVisitID, Convert.ToString(request.Data.EmployeeVisitID)),
                    new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID)),
                    new SearchValueData(Properties.PlaceOfService,request.Data.PlaceOfService),
                    new SearchValueData(Properties.HHA_PCA_NP, request.Data.HHA_PCA_NP)
                });
                response = Common.ApiCommonResponse(true, Resource.DetailSaveSuccessfully, StatusCode.Ok);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }


        public ApiResponse GetPCATask(ApiRequest<TaskListModel> request)
        {
            ApiResponse response;
            try
            {
                PCATaskModel referralTasks = GetMultipleEntity<PCATaskModel>(StoredProcedure.API_GetPCATask, new List<SearchValueData>
                {
                    new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID)),
                    new SearchValueData(Properties.ReferralID, Convert.ToString(request.Data.ReferralID)),
                    new SearchValueData(Properties.VisitTaskType, Convert.ToString(ReferralTaskModel.TaskType.Task))
                });
                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, referralTasks);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse SavePCATask(ApiRequest<OtherTask> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                GetScalar(StoredProcedure.API_SavePCATask, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeVisitID, Convert.ToString(request.Data.EmployeeVisitID)),
                    new SearchValueData(Properties.OtherActivity, request.Data.OtherActivity),
                    new SearchValueData(Properties.OtherActivityTime, Convert.ToString(request.Data.OtherActivityTime)),
                    new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID)),
                    new SearchValueData(Properties.ServiceCodeID, Convert.ToString(request.Data.ServiceCodeID)),
                    new SearchValueData(Properties.CareTypeID, Convert.ToString(request.Data.CareTypeID)),
                });
                response = Common.ApiCommonResponse(true, Resource.DetailSaveSuccessfully, StatusCode.Ok);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse GetPCAConclusion(ApiRequest<ReferralTaskModel> request)
        {
            ApiResponse response;
            try
            {
                GetConclusionListModel referralTasks = GetMultipleEntity<GetConclusionListModel>(StoredProcedure.API_GetPCAConclusion, new List<SearchValueData>
                {
                    new SearchValueData(Properties.ReferralID, Convert.ToString(request.Data.ReferralID)),
                    new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID)),
                    new SearchValueData(Properties.VisitTaskType, Convert.ToString(ReferralTaskModel.TaskType.Conclusion))
                });

                foreach (var item in referralTasks.TaskLists)
                {
                    item.ConclusionAnswer = string.IsNullOrEmpty(item.Answer) ? false : (item.Answer.ToUpper() == Constants.Yes.ToUpper()) ? true : false;


                }

                List<Category> categoryList = referralTasks.TaskLists.ToList().GroupBy(c => new
                {
                    c.CategoryId,
                    c.CategoryName,
                }).Select(grp => new Category
                {
                    CategoryId = grp.Key.CategoryId,
                    CategoryName = grp.Key.CategoryName,
                    TaskLists = grp.ToList()
                }).ToList();


                foreach (Category category in categoryList)
                {
                    List<SubCategory> subCategoryList = category.TaskLists.GroupBy(sc => new
                    {
                        sc.SubCategoryId,
                        sc.SubCategoryName,
                    }).Select(grp => new SubCategory
                    {
                        SubCategoryId = grp.Key.SubCategoryId,
                        SubCategoryName = grp.Key.SubCategoryName,
                        TaskLists = grp.ToList()
                    }).ToList();

                    category.SubCategory.AddRange(subCategoryList);
                }

                CategoryResponse cat_response = new CategoryResponse();
                cat_response.CategoryList = categoryList;
                cat_response.EmployeeVisitID = referralTasks.FinalComment.EmployeeVisitID;
                cat_response.SurveyComment = referralTasks.FinalComment.SurveyComment;

                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, cat_response);

            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }


        public ApiResponse GetPCASignature(ApiRequest<GetPCASiganture> request)
        {
            ApiResponse response;
            try
            {
                GetPCASiganture referralTasks = GetEntity<GetPCASiganture>(StoredProcedure.API_GetPCASignature, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID)),
                    new SearchValueData(Properties.EmployeeVisitID, Convert.ToString(request.Data.EmployeeVisitID))
                });

                if (referralTasks.PatientSignature != null)
                {
                    Guid guid = Guid.NewGuid();
                    string filename = guid + Constants.ImageFormatJPG;
                    string path = string.Format(Common.UploadPath(), Common.GetDatabaseNameFromApi()) + ConfigSettings.ReferralPath + request.Data.EmployeeVisitID + "/";
                    string actualFilePath = HttpContext.Current.Server.MapCustomPath(path);
                    if (!Directory.Exists(actualFilePath))
                    {
                        Directory.CreateDirectory(actualFilePath);
                    }
                    string fullPath = actualFilePath + filename;
                    File.WriteAllBytes(fullPath, Convert.FromBase64String(referralTasks.PatientSignature));

                    referralTasks.PatientSignature = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(fullPath);
                }

                referralTasks.SignaturePath = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(referralTasks.SignaturePath);

                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, referralTasks);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }



        public ApiResponse SavePCAWithSignature(HttpRequest currentHttpRequest, ApiRequest<PostFileModel> request)
        {
            //ApiResponse<FileListModel> response = new ApiResponse<FileListModel>();

            ApiResponse response = new ApiResponse();
            try
            {
                string base64String = string.Empty;
                string fullPath = string.Empty;

                if (currentHttpRequest.Files.Count > 0)
                {
                    //string basePath = ConfigSettings.UploadBasePath;

                    List<FileModel> filelist = new List<FileModel>();
                    for (int i = 0; i < currentHttpRequest.Files.Count; i++)
                    {
                        HttpPostedFile file = currentHttpRequest.Files[i];

                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(file.ContentLength);
                        }
                        base64String = Convert.ToBase64String(fileData, 0, fileData.Length);
                    }

                    GetScalar(StoredProcedure.API_SavePCASignature, new List<SearchValueData>
                    {
                        new SearchValueData(Properties.EmployeeVisitID, Convert.ToString(request.Data.EmployeeVisitID)),
                        new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID)),
                        new SearchValueData(Properties.EmployeeSignatureID, Convert.ToString(request.Data.EmployeeSignatureID)),
                        new SearchValueData(Properties.PCACompletedLat, Convert.ToString(request.Data.Lat)),
                        new SearchValueData(Properties.PCACompletedLong, Convert.ToString(request.Data.Long)),
                        new SearchValueData(Properties.PCACompletedIPAddress, Convert.ToString(request.Data.IPAddress)),
                        new SearchValueData(Properties.PatientSignature, base64String)
                    });
                    response = Common.ApiCommonResponse(true, Resource.DetailSaveSuccessfully, StatusCode.Ok);

                }
                else
                {
                    response = Common.ApiCommonResponse(false, Resource.BadRequest, StatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                response = Common.ApiCommonResponse(false, Common.SetExceptionMessage(Resource.ServerError + e.Message),
                    StatusCode.InternalServerError);
            }
            return response;
        }

        public ApiResponse SavePCAWithoutSignature(ApiRequest<PCAModel> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                GetScalar(StoredProcedure.API_SavePCAWithoutSignature, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeVisitID, Convert.ToString(request.Data.EmployeeVisitID)),
                    new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID))
                });
                response = Common.ApiCommonResponse(true, Resource.DetailSaveSuccessfully, StatusCode.Ok);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse SavePCA(ApiRequest<PCAModel> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                //GetScalar(StoredProcedure.API_PCASigned, new List<SearchValueData>
                //{
                //    new SearchValueData(Properties.EmployeeVisitID, Convert.ToString(request.Data.EmployeeVisitID)),
                //    new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID))
                //});
                //response = Common.ApiCommonResponse(true, Resource.DetailSaveSuccessfully, StatusCode.Ok);


                List<SendAlertModel> sendAlertDetails = GetEntityList<SendAlertModel>(StoredProcedure.API_PCASigned, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeVisitID, Convert.ToString(request.Data.EmployeeVisitID)),
                    new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID)),
                    new SearchValueData(Properties.SignedLat, Convert.ToString(request.Data.Lat)),
                    new SearchValueData(Properties.SignedLong, Convert.ToString(request.Data.Long)),
                    new SearchValueData(Properties.SignedIPAddress, Convert.ToString(request.Data.IPAddress))
                });

                response = Common.ApiCommonResponse(true, Resource.DetailSaveSuccessfully, StatusCode.Ok);

            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse DeletePCASignature(ApiRequest<Signature> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                Uri uri = new Uri(request.Data.PatientSignature);
                string path = uri.LocalPath;
                string actualFilePath = HttpContext.Current.Server.MapPath(path);

                if (File.Exists(actualFilePath))
                {
                    File.Delete(actualFilePath);
                }

                response = Common.ApiCommonResponse(true, "", StatusCode.Ok);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        #endregion


        #region Profile
        public ApiResponse GetProfileDetail(ApiRequest<EmpProfile> request)
        {
            ApiResponse response;
            try
            {
                EmployeeProfileDetails data = GetEntity<EmployeeProfileDetails>(StoredProcedure.API_GetProfileDetail, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID))
                });
                data.EmployeeSignatureURL = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(data.EmployeeSignatureURL);
                data.EmployeeProfileImgURL = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(data.EmployeeProfileImgURL);
                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, data);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse SaveProfileWithoutSignature(ApiRequest<EmployeeProfileDetails> request)
        {
            ApiResponse response;
            try
            {
                List<SearchValueData> data = new List<SearchValueData>();
                data.Add(new SearchValueData(Properties.EmployeeID, Crypto.Decrypt(Convert.ToString(request.Data.EncryptedEmployeeID))));
                data.Add(new SearchValueData(Properties.IsFingerPrintAuth, Convert.ToString(request.Data.IsFingerPrintAuth)));
                data.Add(new SearchValueData(Properties.FirstName, request.Data.FirstName));
                data.Add(new SearchValueData(Properties.LastName, request.Data.LastName));
                data.Add(new SearchValueData(Properties.UserName, request.Data.UserName));
                data.Add(new SearchValueData(Properties.MobileNumber, request.Data.MobileNumber));
                data.Add(new SearchValueData(Properties.IVRPin, request.Data.IVRPin));
                if (!string.IsNullOrEmpty(request.Data.Password))
                {
                    PasswordDetail passwordDetail = Common.CreatePassword(request.Data.Password);
                    data.Add(new SearchValueData(Properties.Password, passwordDetail.Password));
                    data.Add(new SearchValueData(Properties.PasswordSalt, passwordDetail.PasswordSalt));
                }


                int result = (int)GetScalar(StoredProcedure.API_SaveProfileWithoutSignature, data);
                if (result == -1)
                {
                    response = Common.ApiCommonResponse(false, Resource.MobileNumberExist, StatusCode.Ok);
                }
                else if (result == -2)
                {
                    response = Common.ApiCommonResponse(false, Resource.UserNameExists, StatusCode.Ok);
                }
                else
                {
                    response = Common.ApiCommonResponse(true, Resource.DetailSaveSuccessfully, StatusCode.Ok);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse SaveProfileWithSignature(HttpRequest currentHttpRequest, ApiRequest<PostEmpSignatureModel> request)
        {
            ApiResponse response = new ApiResponse();
            WebClient client = new WebClient();
            string p = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Constants.UploadEmpSignatureURL;


            try
            {
                string base64String = string.Empty;
                string fullPath = string.Empty;
                List<SearchValueData> data = new List<SearchValueData>();
                if (currentHttpRequest.Files.Count > 0)
                {
                    //string basePath = ConfigSettings.UploadBasePath;
                    //List<FileModel> filelist = new List<FileModel>();
                    var AllKey = currentHttpRequest.Files.AllKeys;

                    for (int i = 0; i < currentHttpRequest.Files.Count; i++)
                    {
                        HttpPostedFile file = currentHttpRequest.Files[i];

                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(file.ContentLength);
                        }

                        //Remove Header to avoid duplication
                        client.Headers.Remove(Properties.EmployeeID);
                        client.Headers.Remove(Properties.EmployeeSignatureID);
                        client.Headers.Remove(Properties.ImageKey);

                        client.Headers.Add(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID));
                        client.Headers.Add(Properties.EmployeeSignatureID, Convert.ToString(request.Data.EmployeeSignatureID));
                        client.Headers.Add(Properties.ImageKey, AllKey[i]);
                        if (AllKey[i] == "Siganture")
                        {
                            client.UploadData(string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Constants.UploadEmpSignatureURL, Constants.Post, fileData);
                        }
                        if (AllKey[i] == "ProfilePic")
                        {
                            client.UploadData(string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Constants.UploadEmpSignatureURL, Constants.Post, fileData);
                        }
                    }


                    data.Add(new SearchValueData(Properties.EmployeeID, Crypto.Decrypt(Convert.ToString(request.Data.EncryptedEmployeeID))));
                    data.Add(new SearchValueData(Properties.IsFingerPrintAuth, Convert.ToString(request.Data.IsFingerPrintAuth)));
                    data.Add(new SearchValueData(Properties.FirstName, request.Data.FirstName));
                    data.Add(new SearchValueData(Properties.LastName, request.Data.LastName));
                    data.Add(new SearchValueData(Properties.UserName, request.Data.UserName));
                    data.Add(new SearchValueData(Properties.MobileNumber, request.Data.MobileNumber));
                    data.Add(new SearchValueData(Properties.IVRPin, request.Data.IVRPin));
                    if (!string.IsNullOrEmpty(request.Data.Password))
                    {
                        PasswordDetail passwordDetail = Common.CreatePassword(request.Data.Password);
                        data.Add(new SearchValueData(Properties.Password, passwordDetail.Password));
                        data.Add(new SearchValueData(Properties.PasswordSalt, passwordDetail.PasswordSalt));
                    }

                    GetScalar(StoredProcedure.API_SaveProfileWithoutSignature, data);
                    response = Common.ApiCommonResponse(true, Resource.DetailSaveSuccessfully, StatusCode.Ok);

                }
                else
                {
                    response = Common.ApiCommonResponse(false, Resource.BadRequest, StatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                response = Common.ApiCommonResponse(false, Common.SetExceptionMessage(Resource.ServerError + e.Message),
                    StatusCode.InternalServerError + " //--// " + e.InnerException + " //--// " + e.StackTrace);
            }
            return response;
        }

        public ApiResponse SavePatientProfileImage(HttpRequest currentHttpRequest, ApiRequest<PostRefSignatureModel> request)
        {
            ApiResponse response = new ApiResponse();
            WebClient client = new WebClient();
            try
            {
                string base64String = string.Empty;
                string fullPath = string.Empty;
                List<SearchValueData> data = new List<SearchValueData>();
                if (currentHttpRequest.Files.Count > 0)
                {

                    for (int i = 0; i < currentHttpRequest.Files.Count; i++)
                    {
                        HttpPostedFile file = currentHttpRequest.Files[i];

                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(file.ContentLength);
                        }

                        client.Headers.Add(Properties.ReferralID, Convert.ToString(request.Data.ReferralID));

                        client.UploadData(string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Constants.UploadRefProfileImageURL, Constants.Post, fileData);

                    }

                    response = Common.ApiCommonResponse(true, Resource.ImageUploadedSuccessfully, StatusCode.Ok);

                }
                else
                {
                    response = Common.ApiCommonResponse(false, Resource.BadRequest, StatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                response = Common.ApiCommonResponse(false, Common.SetExceptionMessage(Resource.ServerError + e.Message),
                    StatusCode.InternalServerError);
            }
            return response;
        }

        public ApiResponse GetFacilityList(ApiRequest<EmpProfile> request)
        {
            ApiResponse response;
            try
            {
                List<FacilityDetails> data = GetEntityList<FacilityDetails>(StoredProcedure.API_GetFacilityRAL, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID))
                });
                FacilityListModel facilityListModel = new FacilityListModel();
                facilityListModel.FacilityList = data;
                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, facilityListModel);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        
        #endregion

        #region Employee Details for ID Card
        public ApiResponse GetEmpDetailForIdCard(ApiRequest<EmpProfile> request)
        {
            ApiResponse response;
            try
            {
                EmployeeIdCardDetails data = GetEntity<EmployeeIdCardDetails>(StoredProcedure.API_GetEmpDetailForIdCard, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID))
                });
                data.EmployeeSignatureURL = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(data.EmployeeSignatureURL);
                data.EmployeeProfileImgURL = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(data.EmployeeProfileImgURL);
                data.SiteLogo = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(data.SiteLogo);
                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, data);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }
        #endregion

        public ApiResponse UpdatePatientLatLong(ApiRequest<PatientLatLongModel> request)
        {
            ApiResponse response = new ApiResponse();
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            try
            {
                var data = (int)GetScalar(StoredProcedure.API_UpdatePatientLatLong,
                    new List<SearchValueData>
                        {
                            new SearchValueData(Properties.ContactID,Convert.ToString(request.Data.ContactID)),
                            new SearchValueData(Properties.EmployeeID,Convert.ToString(request.Data.EmployeeID)),
                            new SearchValueData(Properties.Latitude,Convert.ToString(request.Data.Latitude)),
                            new SearchValueData(Properties.Longitude,Convert.ToString(request.Data.Longitude)),
                            new SearchValueData(Properties.UpdatedDate,today.ToString(Constants.DbDateTimeFormat))
                        });

                response.IsSuccess = data == 1 ? true : false;
                response.Message = Resource.GeocordinateUpdateSuccessfully;
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse AddClockInoutLog(ApiRequest<ClockInOutLogModel> request)
        {
            ApiResponse response = new ApiResponse();
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            try
            {
                var data = (int)GetScalar(StoredProcedure.API_AddClockInOutLog,
                    new List<SearchValueData>
                        {
                            new SearchValueData(Properties.PatientID,Convert.ToString(request.Data.PatientID)),
                            new SearchValueData(Properties.EmployeeID,Convert.ToString(request.Data.EmployeeID)),
                            new SearchValueData(Properties.ScheduleID,Convert.ToString(request.Data.ScheduleID)),
                            new SearchValueData(Properties.OrganizationID,Convert.ToString(request.Data.OrganizationID)),
                            new SearchValueData(Properties.ClockInOutType,Convert.ToString(request.Data.ClockInOutType)),
                            new SearchValueData(Properties.PatientLat,Convert.ToString(request.Data.PatientLat)),
                            new SearchValueData(Properties.PatientLong,Convert.ToString(request.Data.PatientLong)),
                            new SearchValueData(Properties.EmployeeLat,Convert.ToString(request.Data.EmployeeLat)),
                            new SearchValueData(Properties.EmployeeLong,Convert.ToString(request.Data.EmployeeLong)),
                            new SearchValueData(Properties.Time,today.ToString(Constants.DbDateTimeFormat))
                        });

                response.IsSuccess = data == 1 ? true : false;
                response.Message = Resource.GeocordinateUpdateSuccessfully;
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        #region Notes
        public ApiResponse AddNote(ApiRequest<AddNoteModel> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var searchParameter = new List<SearchValueData>();
                searchParameter.Add(new SearchValueData(Properties.CommonNoteID, Convert.ToString(request.Data.CommonNoteID)));
                searchParameter.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.Data.ReferralID)));
                searchParameter.Add(new SearchValueData(Properties.NoteDetail, Convert.ToString(request.Data.NoteDetail)));
                searchParameter.Add(new SearchValueData(Properties.LoggedInID, Convert.ToString(request.Data.LoggedInID)));
                searchParameter.Add(new SearchValueData(Properties.CategoryID, Convert.ToString(request.Data.CategoryID)));
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.API_SaveReferralNote, searchParameter);
                if (result.TransactionResultId > 0)
                {
                    response = Common.ApiCommonResponse(true, (request.Data.CommonNoteID > 0) ? Resource.NoteUpdatedSuccessfully : Resource.NoteAddedSuccessfully, StatusCode.Ok);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, result.ErrorMessage, StatusCode.Ok);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }
        public ApiResponseList<GetNoteListModel> GetNoteList(ApiRequest<ListModel<SearchNoteModel>> request)
        {
            ApiResponseList<GetNoteListModel> response;
            try
            {
                var list = new List<SearchValueData>
                {
                    new SearchValueData{Name = Properties.FromIndex,Value = Convert.ToString(((request.Data.PageIndex - 1)*request.Data.PageSize) + 1)},
                    new SearchValueData{Name = Properties.ToIndex,Value = Convert.ToString(request.Data.PageIndex*request.Data.PageSize)},
                    new SearchValueData{Name = Properties.SortType,Value = request.Data.SortType},
                    new SearchValueData{Name = Properties.SortExpression,Value = request.Data.SortExpression},
                    new SearchValueData{Name = Properties.ReferralID,Value = request.Data.SearchParams.ReferralID.ToString()},
                    new SearchValueData{Name = Properties.NoteDetail,Value = request.Data.SearchParams.NoteDetail}
                };

                List<GetNoteListModel> listItem = GetEntityList<GetNoteListModel>(StoredProcedure.API_GetReferralNoteList, list);
                if (listItem != null)
                {
                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, GetPageList(listItem, request.Data.PageIndex, request.Data.PageSize, null));
                }
                else
                {
                    var getPage = new Page<GetNoteListModel>();
                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, getPage);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<GetNoteListModel>(e.Message, null);
            }
            return response;
        }
        public ApiResponse DeleteNote(ApiRequest<AddNoteModel> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var searchParameter = new List<SearchValueData>();
                searchParameter.Add(new SearchValueData(Properties.CommonNoteID, Convert.ToString(request.Data.CommonNoteID)));
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.API_DeleteReferralNote, searchParameter);
                if (result.TransactionResultId > 0)
                {
                    response = Common.ApiCommonResponse(true, Resource.NoteDeletedSuccessfully, StatusCode.Ok);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, result.ErrorMessage, StatusCode.Ok);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }
        #endregion

        #region Employee PTO
        public ApiResponseList<GetEmpPTOListModel> GetEmpPTOList(ApiRequest<ListModel<SearchEmpPTOModel>> request)
        {
            ApiResponseList<GetEmpPTOListModel> response;
            try
            {
                List<SearchValueData> list = new List<SearchValueData>();

                list.Add(new SearchValueData { Name = Properties.FromIndex, Value = Convert.ToString(((request.Data.PageIndex - 1) * request.Data.PageSize) + 1) });
                list.Add(new SearchValueData { Name = Properties.ToIndex, Value = Convert.ToString(request.Data.PageIndex * request.Data.PageSize) });
                list.Add(new SearchValueData { Name = Properties.SortType, Value = request.Data.SortType });
                list.Add(new SearchValueData { Name = Properties.SortExpression, Value = request.Data.SortExpression });
                list.Add(new SearchValueData { Name = Properties.EmployeeID, Value = request.Data.SearchParams.EmployeeID.ToString() });
                list.Add(new SearchValueData { Name = Properties.DayOffTypeID, Value = Convert.ToString(request.Data.SearchParams.DayOffTypeID) });
                list.Add(new SearchValueData { Name = Properties.DayOffStatus, Value = request.Data.SearchParams.DayOffStatus });
                if (request.Data.SearchParams.StartDate != null)
                    list.Add(new SearchValueData { Name = Properties.StartDate, Value = request.Data.SearchParams.StartDate.Value.ToString(Constants.DbDateFormat) });
                if (request.Data.SearchParams.EndDate != null)
                    list.Add(new SearchValueData { Name = Properties.EndDate, Value = request.Data.SearchParams.EndDate.Value.ToString(Constants.DbDateFormat) });

                List<GetEmpPTOListModel> listItem = GetEntityList<GetEmpPTOListModel>(StoredProcedure.API_GetEmpPTOList, list);
                if (listItem != null)
                {
                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, GetPageList(listItem, request.Data.PageIndex, request.Data.PageSize, null));
                }
                else
                {
                    var getPage = new Page<GetEmpPTOListModel>();
                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, getPage);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<GetEmpPTOListModel>(e.Message, null);
            }
            return response;
        }

        public ApiResponse SaveEmployeePTO(ApiRequest<EmployeeDayOffModel> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var searchParameter = new List<SearchValueData>();
                searchParameter.Add(new SearchValueData(Properties.EmployeeDayOffID, Convert.ToString(request.Data.EmployeeDayOffID)));
                searchParameter.Add(new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID)));
                searchParameter.Add(new SearchValueData(Properties.StartTime, Convert.ToString(request.Data.StartTime)));
                searchParameter.Add(new SearchValueData(Properties.EndTime, Convert.ToString(request.Data.EndTime)));
                searchParameter.Add(new SearchValueData(Properties.DayOffStatus, Convert.ToString(EmployeeDayOffModel.EmployeeDayOffStatus.InProgress)));
                searchParameter.Add(new SearchValueData(Properties.DayOffTypeID, Convert.ToString(request.Data.DayOffTypeID)));
                searchParameter.Add(new SearchValueData(Properties.EmployeeComment, request.Data.EmployeeComment));
                searchParameter.Add(new SearchValueData(Properties.SystemID, Common.GetHostAddress()));
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.API_SaveEmployeePTO, searchParameter);
                if (result.TransactionResultId > 0)
                {
                    response = Common.ApiCommonResponse(true, (request.Data.EmployeeDayOffID > 0) ? Resource.DayOffUpdatedSuccessfully : Resource.DayOffCreatedSuccessfully, StatusCode.Ok);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, result.ErrorMessage, StatusCode.Ok);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse DeleteEmployeePTO(ApiRequest<EmployeeDayOffModel> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var searchParameter = new List<SearchValueData>();
                searchParameter.Add(new SearchValueData(Properties.EmployeeDayOffID, Convert.ToString(request.Data.EmployeeDayOffID)));
                searchParameter.Add(new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID)));
                searchParameter.Add(new SearchValueData(Properties.SystemID, Common.GetHostAddress()));
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.API_DeleteEmployeePTO, searchParameter);
                if (result.TransactionResultId > 0)
                {
                    response = Common.ApiCommonResponse(true, Resource.DayOffDeletedSuccessfully, StatusCode.Ok);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, result.ErrorMessage, StatusCode.Ok);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }
        #endregion


        #region Case Management (RN)

        public ApiResponse CaseLoadDashboard(ApiRequest<CaseLoadAppointment> request)
        {
            ApiResponse response;
            try
            {
                var timeUtc = DateTime.UtcNow;
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
                var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
                List<PatientAppointment> patientAppointments = GetEntityList<PatientAppointment>(StoredProcedure.API_GetAppointedPatients, new List<SearchValueData>
                {
                    new SearchValueData(Properties.ServerCurrentDate, today.ToString(Constants.DbDateFormat)),
                    new SearchValueData(Properties.EmployeeId, Convert.ToString(request.Data.EmployeeID))
                });

                if (patientAppointments.Count > 0)
                {
                    foreach (var item in patientAppointments)
                    {
                        item.ImageUrl = item.ImageUrl == null ? null : string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(item.ImageUrl);
                    }
                }

                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, patientAppointments);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse GetCareTypes(ApiRequest request)
        {
            ApiResponse response;
            try
            {
                List<CareType> careTypes = GetEntityList<CareType>(StoredProcedure.API_GetCareTypes, new List<SearchValueData>
                {
                    new SearchValueData(Properties.DDType_CareType, Convert.ToString((int)Common.DDType.CareType))
                });

                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, careTypes);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse CreateCareTypeSchedule(ApiRequest<CareTypeSchedule> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var timeUtc = DateTime.UtcNow;
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
                var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
                var searchParameter = new List<SearchValueData>();
                searchParameter.Add(new SearchValueData(Properties.CareTypeID, Convert.ToString(request.Data.CareTypeID)));
                searchParameter.Add(new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID)));
                searchParameter.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.Data.ReferralID)));
                searchParameter.Add(new SearchValueData(Properties.ServerCurrentDate, today.ToString(Constants.DbDateFormat)));
                searchParameter.Add(new SearchValueData(Properties.SystemID, Common.GetHostAddress()));
                searchParameter.Add(new SearchValueData(Properties.StartTime, Convert.ToString(request.Data.StartTime)));
                searchParameter.Add(new SearchValueData(Properties.EndTime, Convert.ToString(request.Data.EndTime)));
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.API_CreateCareTypeSchedule, searchParameter);
                if (result.TransactionResultId > 0)
                {
                    response = Common.ApiCommonResponse(true, Resource.ScheduleCreatedSuccessfully, StatusCode.Ok, result);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, result.ErrorMessage, StatusCode.Ok);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        #endregion

        #region NoteSentence
        public ApiResponse GetNoteSentenceList(ApiRequest<EmployeeModel> request)
        {
            ApiResponse response=new ApiResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                List<NoteSentenceModel> noteSentenceList = GetEntityList<NoteSentenceModel>(StoredProcedure.API_GetNoteSentenceList, searchParam);
                NoteSentenceList NoteSentenceListData = new NoteSentenceList();
                NoteSentenceListData.NoteSentences = noteSentenceList;
                response.Data = NoteSentenceListData;
                response.IsSuccess = true;
                return response;
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<NoteSentenceModel>(e.Message, null);
            }
            return response;
        }
        #endregion

        public ApiResponse RAL_SetEmployeeVisitTime(ApiRequest<ClockInModel> request)
        {
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);

            ApiResponse response;
            try
            {
                var result = GetScalar(StoredProcedure.RAL_SetEmployeeVisitTime, new List<SearchValueData>
                {
                    new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID)),
                    new SearchValueData(Properties.ReferralID, Convert.ToString(request.Data.ReferralID)),
                    new SearchValueData(Properties.EmployeeId, Convert.ToString(request.Data.EmployeeID)),
                    new SearchValueData(Properties.Type, Convert.ToString(request.Data.Type)),
                    new SearchValueData(Properties.IsByPass, Convert.ToString(request.Data.IsByPass)),
                    new SearchValueData(Properties.IsCaseManagement, Convert.ToString(request.Data.IsCaseManagement)),
                    new SearchValueData(Properties.IsApprovalRequired, Convert.ToString(request.Data.IsApprovalRequired)),
                    new SearchValueData(Properties.ActionTaken, request.Data.IsApprovalRequired ? Convert.ToString((int)ClockInModel.BypassAction.Pending) : null),
                    new SearchValueData(Properties.EarlyClockOutComment, request.Data.EarlyClockOutComment),
                    new SearchValueData(Properties.ByPassReason, request.Data.ByPassReason),
                    new SearchValueData(Properties.Lat, Convert.ToString(request.Data.Lat)),
                    new SearchValueData(Properties.Long, Convert.ToString(request.Data.Long)),
                    new SearchValueData(Properties.BeforeClockIn, Constants.BeforeClockIn),
                    new SearchValueData(Properties.ClockInTime, today.ToString(Constants.DbDateTimeFormat)), //DateTime.UtcNow.ToString(Constants.DbDateTimeFormat)
                    new SearchValueData(Properties.ClockOutTime, (request.Data.Type==2)?today.ToString(Constants.DbDateTimeFormat):null),
                    new SearchValueData(Properties.Distance,ConfigSettings.Distance),
                    new SearchValueData(Properties.SystemID, Common.GetHostAddress()),
                    new SearchValueData(Properties.EarlyClockInComment, Convert.ToString(request.Data.EarlyClockInComment)),
                    new SearchValueData(Properties.IsEarlyClockIn, Convert.ToString(request.Data.IsEarlyClockIn)),
            });
                //var Message = (request.Data.Type == 1) ? Resource.ClockIn : Resource.ClockOut;
                var Message = string.Empty;

                if ((int)result > 0)
                {
                    SendAlertToken token = new SendAlertToken();
                    //Get Template for Send Alert SMS
                    EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
                        {
                            new SearchValueData{Name = "EmailTemplateTypeID",Value =Convert.ToInt16(EnumEmailType.Alert_SMS_Notification).ToString(),IsEqual = true}
                        });

                    //Get Details for send ByPass Alert to all admins
                    List<SendAlertModel> sendAlertOnByPass = GetEntityList<SendAlertModel>(StoredProcedure.API_GetByPassDetail, new List<SearchValueData>
                        {
                            new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID))
                        });

                    var domainName = request.CompanyName;
                    var byPassClockInNotificationType = (int)Mobile_Notification.NotificationTypes.ByPassClockInNotification;
                    var byPassClockOutNotificationType = (int)Mobile_Notification.NotificationTypes.ByPassClockOutNotification;
                    var earlyClockOutNotificationType = (int)Mobile_Notification.NotificationTypes.EarlyClockOutNotification;

                    if (request.Data.Type == 1 && request.Data.IsByPass)
                    {
                        //Send SMS on ByPass ClockIn
                        foreach (SendAlertModel item in sendAlertOnByPass)
                        {
                            string emailTemplateTxt = emailTemplate.EmailTemplateBody;
                            token.EmployeeName = item.EmployeeName;
                            token.PatientName = item.PatientName;
                            token.Message = item.ByPassReasonClockIn;
                            token.AlertFor = Resource.ByPassClockIn;
                            emailTemplate.EmailTemplateBody = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, token);
                          //  Common.SendSms(item.MobileNumber, emailTemplate.EmailTemplateBody, EnumEmailType.Alert_SMS_Notification.ToString());

                            //Send FCM Notification on ByPass ClockIn
                            //SendClockInClockOutNotification(item.FcmTokenId, item.DeviceType, emailTemplate.EmailTemplateBody, domainName, byPassClockInNotificationType);

                            emailTemplate.EmailTemplateBody = emailTemplateTxt;
                        }
                    }

                    if (request.Data.Type == 2 && request.Data.IsByPass)
                    {
                        //Send SMS on ByPass ClockOut
                        foreach (SendAlertModel item in sendAlertOnByPass)
                        {
                            string emailTemplateTxt = emailTemplate.EmailTemplateBody;
                            token.EmployeeName = item.EmployeeName;
                            token.PatientName = item.PatientName;
                            token.Message = item.ByPassReasonClockOut;
                            token.AlertFor = Resource.ByPassClockOut;
                            emailTemplate.EmailTemplateBody = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, token);
                           // Common.SendSms(item.MobileNumber, emailTemplate.EmailTemplateBody, EnumEmailType.Alert_SMS_Notification.ToString());

                            //Send FCM Notification on ByPass ClockOut
                            //SendClockInClockOutNotification(item.FcmTokenId, item.DeviceType, emailTemplate.EmailTemplateBody, domainName, byPassClockOutNotificationType);

                            emailTemplate.EmailTemplateBody = emailTemplateTxt;
                        }

                        //Send SMS on Early ClockOut
                        List<SendAlertModel> sendAlertDetails = GetEntityList<SendAlertModel>(StoredProcedure.API_GetEarlyClockoutDetail, new List<SearchValueData>
                        {
                            new SearchValueData(Properties.ScheduleID, Convert.ToString(request.Data.ScheduleID))
                        });
                        foreach (SendAlertModel item in sendAlertDetails)
                        {
                            string emailTemplateTxt = emailTemplate.EmailTemplateBody;
                            token.EmployeeName = item.EmployeeName;
                            token.PatientName = item.PatientName;
                            token.Message = item.AlertComment;
                            token.AlertFor = Resource.EarlyClockOut;
                            emailTemplate.EmailTemplateBody = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, token);
                            //Common.SendSms(item.MobileNumber, emailTemplate.EmailTemplateBody, EnumEmailType.Alert_SMS_Notification.ToString());

                            //Send FCM Notification on Early ClockOut
                            //SendClockInClockOutNotification(item.FcmTokenId, item.DeviceType, emailTemplate.EmailTemplateBody, domainName, earlyClockOutNotificationType);

                            emailTemplate.EmailTemplateBody = emailTemplateTxt;
                        }
                    }
                    Message = (request.Data.Type == 1) ? Resource.ClockInSuccessfully : Resource.ClockOutSuccessfully;
                    response = Common.ApiCommonResponse(true, Message, StatusCode.Ok, result);
                }
                else if ((int)result == -1)
                {
                    Message = (request.Data.Type == 1) ? Resource.ClockInFailed : Resource.ClockOutFailed;
                    response = Common.ApiCommonResponse(false, Message, StatusCode.Ok, result);
                }
                else if ((int)result == -3)
                {
                    Message = Resource.EarlyClockInMessage;
                    response = Common.ApiCommonResponse(false, Message, StatusCode.Ok, result);
                }
                else if ((int)result == -4)
                {
                    Message = Resource.CompleteRequiredTask;
                    response = Common.ApiCommonResponse(false, Message, StatusCode.Ok, result);
                }
                else if ((int)result == -5)
                {
                    Message = request.Data.Type == 1 ? string.Format("Cannot clock-in {0}", Resource.exceededClockouttimeLimit) : string.Format("Cannot clock-out {0}", Resource.exceededClockouttimeLimit);
                    response = Common.ApiCommonResponse(false, Message, StatusCode.Ok, result);
                }
                else
                {
                    Message = Resource.MissingPatientCoordinates;
                    response = Common.ApiCommonResponse(false, Message, StatusCode.Ok, result);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        #region AnyTime ClockIn
        public ApiResponse ChangeSchedule(ApiRequest<ChangeScheduleModel> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                TimeSpan StartTime = new TimeSpan();
                TimeSpan EndTime = new TimeSpan();
                //if (!string.IsNullOrEmpty(request.Data.StartTime))
                //{
                //    string[] timeparts = request.Data.StartTime.Split(':');
                //    timeparts[0] = timeparts[0] != null && timeparts[0].Length == 1 ? timeparts[0].Insert(0, "0") : timeparts[0];
                //    request.Data.StartTime = timeparts[0] + ":" + timeparts[1];
                //    DateTime timeOnly = DateTime.ParseExact(request.Data.StartTime.ToLower(), "hh:mm tt",
                //                                            System.Globalization.CultureInfo.CurrentCulture);
                //    StartTime = timeOnly.TimeOfDay;
                //}

                //if (!string.IsNullOrEmpty(request.Data.EndTime))
                //{
                //    string[] timeparts = request.Data.EndTime.Split(':');
                //    timeparts[0] = timeparts[0] != null && timeparts[0].Length == 1 ? timeparts[0].Insert(0, "0") : timeparts[0];
                //    request.Data.EndTime = timeparts[0] + ":" + timeparts[1];
                //    DateTime timeOnly = DateTime.ParseExact(request.Data.EndTime.ToLower(), "hh:mm tt",
                //                                            System.Globalization.CultureInfo.CurrentCulture);
                //    EndTime = timeOnly.TimeOfDay;
                //}

                int data = (int)GetScalar(StoredProcedure.SaveNewSchedule, new List<SearchValueData>
                {
                    new SearchValueData {Name = "ScheduleID",Value = Convert.ToString(request.Data.ScheduleID)},
                    new SearchValueData {Name = "StartTime",Value = Convert.ToString(request.Data.StartTime)},
                    new SearchValueData {Name = "EndTime",Value = Convert.ToString(request.Data.EndTime)},

                    new SearchValueData {Name = "UpdatedBy",Value = Convert.ToString(request.Data.EmployeeID)}
                });
                response.Data = data;
                response.IsSuccess = true;
                return response;
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<NoteSentenceModel>(e.Message, null);
            }
            return response;
        }


        public ApiResponse GetSurveyQuestionsList(ApiRequest<CovidSurveyQuestionModel> request)
        {
            ApiResponse response;
            try
            {
                CovidSurveyQuestionModel data = GetMultipleEntity<CovidSurveyQuestionModel>(StoredProcedure.API_GetSurveyQuestionList);

                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, data);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse SaveSurveyAnswers(ApiRequest<CovidSurveySaveModel> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Data.EmployeeID != 0)
                {
                    foreach (var item in request.Data.Answer)
                    {
                        Models.Entity.EmpCovidSurvey obj = new Models.Entity.EmpCovidSurvey();
                        obj.CovidSurveyID = item.CovidSurveyID;
                        obj.EmployeeID = request.Data.EmployeeID;
                        obj.QuestionID = item.QuestionID;
                        obj.AnswersID = item.AnswersID;

                        var dataList = new List<SearchValueData>();
                        dataList.Add(new SearchValueData { Name = "CovidSurveyID", Value = Convert.ToString(item.CovidSurveyID) });
                        dataList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(request.Data.EmployeeID) });
                        dataList.Add(new SearchValueData { Name = "QuestionID", Value = Convert.ToString(item.QuestionID) });
                        dataList.Add(new SearchValueData { Name = "AnswersID", Value = Convert.ToString(item.AnswersID) });
                        dataList.Add(new SearchValueData { Name = "CreatedDate", Value = Convert.ToString(request.Data.CreatedDate) });
                        int data = (int)GetScalar(StoredProcedure.SaveSurveyAnswer, dataList);
                    }

                    //if (request.Data.EmployeeID == 0)
                    //{
                    //    response = Common.ApiCommonResponse(true, Resource.CreatedSuccessfully, StatusCode.Ok);
                    //}
                    //else
                    //{
                    //    response = Common.ApiCommonResponse(true, Resource.UpdatedSuccessfully, StatusCode.Ok);
                    //}
                    response = Common.ApiCommonResponse(true, Resource.CreatedSuccessfully, StatusCode.Ok);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, "ErrorMessage", StatusCode.Ok);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse GetEmployeeSurveyList(ApiRequest<GetCovidSurveyModel> request)
        {
            ApiResponse response;
            try
            {
                var SearchList = new List<SearchValueData>();
                SearchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(request.Data.EmployeeID) });
                SearchList.Add(new SearchValueData { Name = "CreatedDate", Value = Convert.ToString(request.Data.CreatedDate) });
                CovidSurveyListModel data = GetMultipleEntity<CovidSurveyListModel>(StoredProcedure.API_GetEmployeeSurveyList, SearchList);

                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, data);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse GetNotesCategory(ApiRequest<long> request)
        {
            ApiResponse response;
            try
            {
                var SearchList = new List<SearchValueData>();
                List<NotesCategoryModel> category = GetEntityList<NotesCategoryModel>(StoredProcedure.API_GetNotesCategory, SearchList);

                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, category);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }
        #endregion


        /// <summary>
        /// Return Available, Allocated, Used, Remaing and Unallocated Unites for Prior Authorization
        /// </summary>
        /// <param name="request">Expect BillingAuthorizationID as a parameter for Store Procedure</param>
        /// <returns><see cref="PriorAuthorizationModel"/></returns>
        public ApiResponse PrioAuthorization(ApiRequest<PriorAuthorizationRequestModel> request)
        {
            ApiResponse response;
            try
            {
                var SearchList = new List<SearchValueData>();
                SearchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(request.Data.ReferralID) });
                SearchList.Add(new SearchValueData { Name = "BillingAuthorizationID", Value = Convert.ToString(request.Data.BillingAuthorizationID) });

                var result = GetEntity<PriorAuthorizationModel>(StoredProcedure.API_UniversalPriorAuthorization, SearchList);

                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, result);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        #region "Referral Group"
        public ApiResponse SaveGroup(ApiRequest<ReferralGroupModel> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var searchParameter = new List<SearchValueData>();
                searchParameter.Add(new SearchValueData(Properties.ReferralGroupID, Convert.ToString(request.Data.ReferralGroupID)));
                searchParameter.Add(new SearchValueData(Properties.GroupName, Convert.ToString(request.Data.GroupName)));
                searchParameter.Add(new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID)));
                searchParameter.Add(new SearchValueData(Properties.CreatedBy, Convert.ToString(request.Data.CreatedBy)));
                searchParameter.Add(new SearchValueData(Properties.UpdatedBy, Convert.ToString(request.Data.UpdatedBy)));
                searchParameter.Add(new SearchValueData(Properties.IsDeleted, Convert.ToString(request.Data.IsDeleted)));
                searchParameter.Add(new SearchValueData(Properties.SystemID, Common.GetHostAddress()));
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.API_SaveReferralGroup, searchParameter);
                if (result.TransactionResultId > 0)
                {
                    response = Common.ApiCommonResponse(true, (request.Data.ReferralGroupID > 0) ? Resource.NoteUpdatedSuccessfully : Resource.NoteAddedSuccessfully, StatusCode.Ok);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, result.ErrorMessage, StatusCode.Ok);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse DeleteGroup(ApiRequest<ReferralGroupModel> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var searchParameter = new List<SearchValueData>();
                searchParameter.Add(new SearchValueData(Properties.ReferralGroupID, Convert.ToString(request.Data.ReferralGroupID)));
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.API_DeleteReferralGroup, searchParameter);
                if (result.TransactionResultId > 0)
                {
                    response = Common.ApiCommonResponse(true, Resource.DeletedSuccessfully, StatusCode.Ok);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, result.ErrorMessage, StatusCode.Ok);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse SaveGroupMapping(ApiRequest<ReferralGroupMappingModel> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var searchParameter = new List<SearchValueData>();
                searchParameter.Add(new SearchValueData(Properties.ReferralGroupMappingID, Convert.ToString(request.Data.ReferralGroupMappingID)));
                searchParameter.Add(new SearchValueData(Properties.ReferralGroupID, Convert.ToString(request.Data.ReferralGroupID)));
                searchParameter.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.Data.ReferralID)));
                searchParameter.Add(new SearchValueData(Properties.CreatedBy, Convert.ToString(request.Data.CreatedBy)));
                searchParameter.Add(new SearchValueData(Properties.UpdatedBy, Convert.ToString(request.Data.UpdatedBy)));
                searchParameter.Add(new SearchValueData(Properties.SystemID, Common.GetHostAddress()));
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.API_SaveReferralGroupMapping, searchParameter);
                if (result.TransactionResultId > 0)
                {
                    response = Common.ApiCommonResponse(true, (request.Data.ReferralGroupID > 0) ? Resource.NoteUpdatedSuccessfully : Resource.NoteAddedSuccessfully, StatusCode.Ok);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, result.ErrorMessage, StatusCode.Ok);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse DeleteGroupMapping(ApiRequest<ReferralGroupMappingModel> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var searchParameter = new List<SearchValueData>();
                searchParameter.Add(new SearchValueData(Properties.ReferralGroupMappingID, Convert.ToString(request.Data.ReferralGroupMappingID)));
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.API_DeleteReferralGroupMapping, searchParameter);
                if (result.TransactionResultId > 0)
                {
                    response = Common.ApiCommonResponse(true, Resource.DeletedSuccessfully, StatusCode.Ok);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, result.ErrorMessage, StatusCode.Ok);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponseList<ReferralGroupList> GetReferralGroupList(ApiRequest<ListModel<SearchReferralGroup>> request)
        {
            ApiResponseList<ReferralGroupList> response;
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.FromIndex, Convert.ToString(((request.Data.PageIndex - 1) * request.Data.PageSize) + 1)));
                srchParam.Add(new SearchValueData(Properties.ToIndex, Convert.ToString(request.Data.PageIndex * request.Data.PageSize)));
                srchParam.Add(new SearchValueData(Properties.SortType, request.Data.SortType));
                srchParam.Add(new SearchValueData(Properties.SortExpression, request.Data.SortExpression));
                srchParam.Add(new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.SearchParams.EmployeeID)));
                srchParam.Add(new SearchValueData(Properties.GroupName, Convert.ToString(request.Data.SearchParams.GroupName)));

                List<ReferralGroupList> getReferralGroupList = GetEntityList<ReferralGroupList>(StoredProcedure.API_GetReferralGroupList, srchParam);

                if (getReferralGroupList != null)
                {
                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, GetPageList(getReferralGroupList, request.Data.PageIndex, request.Data.PageSize, null));
                }
                else
                {
                    var getPage = new Page<ReferralGroupList>();
                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, getPage);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<ReferralGroupList>(e.Message, null);
            }
            return response;
        }

        #endregion
    }
}