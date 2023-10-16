using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.General;
using HomeCareApi.Models.ViewModel;

namespace HomeCareApi.Infrastructure.IDataProvider
{
    interface INotificationDataProvider
    {
        ApiResponse GetUserNotifications(ApiRequest<ListModel<SearchNotificationModel>> request,long employeeId);
        ApiResponse ReadUserNotification(ApiRequest<MarkAsReadNotification> request);
        ApiResponse AcceptScheduleNotification(ApiRequest<MarkAsReadNotification> request);
        ApiResponse ActionOnNotification(ApiRequest<MarkAsReadNotification> request);
    }
}
