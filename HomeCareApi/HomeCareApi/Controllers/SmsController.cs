using System.Web.Mvc;
using HomeCareApi.Infrastructure.DataProvider;
using HomeCareApi.Infrastructure.IDataProvider;

namespace HomeCareApi.Controllers
{
    public class SmsController : BaseController
    {
        private ISmsDataProvider _smsDataProvider;

        /// <summary>
        /// This method will send message to the employee who don't submit their request for the clock in/out 
        /// </summary>
        [HttpGet]
        public void SendClockInoutNotification()
        {
            _smsDataProvider = new SmsDataProvider();
            _smsDataProvider.SendClockInoutNotification();
        }
    }
}