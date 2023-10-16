using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeCareApi.Models.ApiModel;

namespace HomeCareApi.Infrastructure.IDataProvider
{
    public interface IIvrDataProvider
    {
        ApiResponse ValidateReferralContactNumber(string phoneNo);

        ApiResponse VerifyPatient(string accountNumber);

        ApiResponse VerifyPatientByMobile(string mobileNumber);

        ApiResponse ValidateEmployeeMobile(string mobileNumber);

        ApiResponse ValidateIvrCode(string mobileNumber, string ivrPin, string patientPhoneNo);

        ApiResponse CheckForClockInClockOut(string employeeId, string referralIds);

        ApiResponse ClockIn(string employeeId, string referralIds);

        ApiResponse ClockOut(string employeeId, string referralIds);

        ApiResponse IVRBypassClockIn(string employeeId);

        ApiResponse IVRBypassClockOut(string employeeId);



        ApiResponse GetPatientID(string accountId);
        ApiResponse CreatePendingScheduleClockInOut(string empId, string referralId, bool isClockIn);
        //ApiResponse GetPendingScheduleClockInDetails(string empId, string referralId);
        
    }
}
