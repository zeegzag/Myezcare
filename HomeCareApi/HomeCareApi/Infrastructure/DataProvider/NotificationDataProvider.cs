using HomeCareApi.Infrastructure.IDataProvider;
using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.General;
using HomeCareApi.Models.ViewModel;
using HomeCareApi.Resources;
using PetaPoco;
using System;
using System.Collections.Generic;

namespace HomeCareApi.Infrastructure.DataProvider
{
    public class NotificationDataProvider : BaseDataProvider, INotificationDataProvider
    {
        public ApiResponse GetUserNotifications(ApiRequest<ListModel<SearchNotificationModel>> request, long employeeId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                List<UserNotificationsDetails> userNotificationsDetails =
                    GetEntityList<UserNotificationsDetails>(StoredProcedure.API_GetUserNotifications,
                        new List<SearchValueData>
                        {
                            new SearchValueData(Properties.FromIndex,Convert.ToString(((request.Data.PageIndex - 1)*request.Data.PageSize) + 1)),
                            new SearchValueData(Properties.ToIndex,Convert.ToString(request.Data.PageIndex*request.Data.PageSize)),
                            new SearchValueData(Properties.SortType,request.Data.SortType),
                            new SearchValueData(Properties.SortExpression,request.Data.SortExpression),
                            new SearchValueData(Properties.NotificationType,Convert.ToString(request.Data.SearchParams.NotificationType)),
                            new SearchValueData(Properties.EmployeeId,Convert.ToString(employeeId))
                        });
                response = userNotificationsDetails != null && userNotificationsDetails.Count > 0
                    ? Common.ApiCommonResponse(true, Resource.NotificationListBindSuccessfully, StatusCode.Ok,
                        GetPageList(userNotificationsDetails, request.Data.PageIndex, request.Data.PageSize))
                    : Common.ApiCommonResponse(true, Resource.NoNotificationShow, StatusCode.Ok,
                        new Page<UserNotificationsDetails>());
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse ReadUserNotification(ApiRequest<MarkAsReadNotification> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = (int)GetScalar(StoredProcedure.API_ReadUserNotification,
                    new List<SearchValueData>
                        {
                            new SearchValueData(Properties.NotificationID,Convert.ToString(request.Data.NotificationID)),
                            new SearchValueData(Properties.EmployeeID,Convert.ToString(request.Data.EmployeeID))

                        });

                response.IsSuccess = data == 1 ? true : false;
                
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse AcceptScheduleNotification(ApiRequest<MarkAsReadNotification> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = (int)GetScalar(StoredProcedure.API_AcceptScheduleNotification,
                    new List<SearchValueData>
                        {
                            new SearchValueData(Properties.NotificationID,Convert.ToString(request.Data.NotificationID)),
                            new SearchValueData(Properties.EmployeeID,Convert.ToString(request.Data.EmployeeID))
                        });

                response.IsSuccess = data == 1 ? true : false;
                response.Message = Resource.ScheduleAccepted;

            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse ActionOnNotification(ApiRequest<MarkAsReadNotification> request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var data = (int)GetScalar(StoredProcedure.API_ActionOnNotification,
                    new List<SearchValueData>
                        {
                            new SearchValueData(Properties.NotificationID,Convert.ToString(request.Data.NotificationID)),
                            new SearchValueData(Properties.EmployeeID,Convert.ToString(request.Data.EmployeeID)),
                            new SearchValueData(Properties.Action,request.Data.Action)
                        });

                response.IsSuccess = data == 1 ? true : false;
                response.Message = (request.Data.Action.ToUpper() == Constants.Delete) ? Resource.NotificationDeleted : Resource.NotificationArchieved;

            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }
    }
}