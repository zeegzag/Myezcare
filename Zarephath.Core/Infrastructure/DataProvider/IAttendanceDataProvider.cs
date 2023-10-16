using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IAttendanceDataProvider
    {
        #region Attendance Master

        ServiceResponse SetAttendanceMasterModel();

        ServiceResponse GetAttendanceListByFacility(AttendanceMasterSearchModel searchParam);

        ServiceResponse UpdateAttendance(AttendanceDetail model, long loggedInUserID);

        ServiceResponse UpdateCommentForAttendance(AttendanceDetail model, long loggedInUserID);

        #endregion
    }
}
