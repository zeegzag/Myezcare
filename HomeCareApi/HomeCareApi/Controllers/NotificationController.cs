using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.General;
using System.Web.Http;
using HomeCareApi.Infrastructure.IDataProvider;
using HomeCareApi.Infrastructure.DataProvider;
using HomeCareApi.Infrastructure;
using HomeCareApi.Models.ViewModel;

namespace HomeCareApi.Controllers
{
    public class NotificationController : BaseController
    {
        INotificationDataProvider _notificationDataProvider;

        /// <summary>
        /// This Api is used to fetch user notification list
        /// </summary>
        /// <param name="request">request contain value of pagination</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse GetUserNotifications(ApiRequest<ListModel<SearchNotificationModel>> request)
        {
            _notificationDataProvider = new NotificationDataProvider();
            return _notificationDataProvider.GetUserNotifications(request, CacheHelper.EmployeeId);
        }

        [HttpPost]
        public ApiResponse ReadUserNotification(ApiRequest<MarkAsReadNotification> request)
        {
            _notificationDataProvider = new NotificationDataProvider();
            return _notificationDataProvider.ReadUserNotification(request);
        }

        [HttpPost]
        public ApiResponse AcceptScheduleNotification(ApiRequest<MarkAsReadNotification> request)
        {
            _notificationDataProvider = new NotificationDataProvider();
            return _notificationDataProvider.AcceptScheduleNotification(request);
        }

        [HttpPost]
        public ApiResponse ActionOnNotification(ApiRequest<MarkAsReadNotification> request)
        {
            _notificationDataProvider = new NotificationDataProvider();
            return _notificationDataProvider.ActionOnNotification(request);
        }
    }
}
