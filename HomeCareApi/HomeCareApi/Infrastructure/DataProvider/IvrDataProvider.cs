using System;
using System.Collections.Generic;
using HomeCareApi.Infrastructure.IDataProvider;
using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.Entity;
using HomeCareApi.Models.General;
using HomeCareApi.Models.ViewModel;

namespace HomeCareApi.Infrastructure.DataProvider
{
    public class IvrDataProvider : BaseDataProvider, IIvrDataProvider
    {
        static string FromMobile = string.Empty;

        public ApiResponse ValidateReferralContactNumber(string phoneNo)
        {
            ApiResponse response = new ApiResponse();
            FromMobile = phoneNo;
            try
            {
                phoneNo = phoneNo.Substring(phoneNo.Length - 10, 10);

                var referralId = GetScalar(StoredProcedure.ValidateReferralContactNumber, new List<SearchValueData>
                {
                    new SearchValueData(Properties.PhoneNo, phoneNo)
                });
                if (!string.IsNullOrWhiteSpace(Convert.ToString(referralId)))
                {
                    response.IsSuccess = true;
                    response.Data = Convert.ToString(referralId);
                }
                else
                {
                    response.IsSuccess = false;
                }
                
            }
            catch (Exception e)
            {
                InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse VerifyPatient(string accountNumber)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var referralIds = GetScalar(StoredProcedure.API_VerifyPatient, new List<SearchValueData>
                {
                    new SearchValueData(Properties.AccountNumber, accountNumber)
                });
                if (!string.IsNullOrWhiteSpace(Convert.ToString(referralIds)))
                {
                    response.IsSuccess = true;
                    response.Data = Convert.ToString(referralIds);
                }
                else
                {
                    response.IsSuccess = false;
                }

            }
            catch (Exception e)
            {
                InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse VerifyPatientByMobile(string mobileNumber)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var referralIds = GetScalar(StoredProcedure.API_VerifyPatientByMobile, new List<SearchValueData>
                {
                    new SearchValueData(Properties.MobileNumber, mobileNumber)
                });
                if (!string.IsNullOrWhiteSpace(Convert.ToString(referralIds)))
                {
                    response.IsSuccess = true;
                    response.Data = Convert.ToString(referralIds);
                }
                else
                {
                    response.IsSuccess = false;
                }

            }
            catch (Exception e)
            {
                InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse ValidateEmployeeMobile(string mobileNumber)
        {
            ApiResponse response;
            try
            {
                var details = (long)GetScalar(StoredProcedure.API_ValidateEmployeeMobile, new List<SearchValueData>
                {
                    new SearchValueData(Properties.MobileNumber, mobileNumber)
                });

                if (details >  0)//&& details.PermissionID > 0
                {
                    response = Common.ApiCommonResponse(true, "", StatusCode.Ok);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, "", StatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse ValidateIvrCode(string mobileNumber, string ivrPin,string PatientPhoneNo)
        {
            ApiResponse response;
            try
            {
                ValidateIVRCodeModel details = GetMultipleEntity<ValidateIVRCodeModel>(StoredProcedure.IVR_ValidateIvrCode, new List<SearchValueData>
                {
                    new SearchValueData(Properties.MobileNumber, mobileNumber),
                    new SearchValueData(Properties.IVRPin, ivrPin),
                    new SearchValueData(Properties.AutoApprovedIVRBypassPermission, Constants.AutoApprovedIVRBypassPermission),
                    new SearchValueData(Properties.ApprovalRequiredIVRBypassPermission, Constants.ApprovalRequiredIVRBypassPermission),
                    new SearchValueData(Properties.PatientPhoneNo, PatientPhoneNo)  //  Adding new parameter PatientPhoneNo for IVR 
                });

                if (details.Employee != null)//&& details.PermissionID > 0
                {
                    response = Common.ApiCommonResponse(true, "", StatusCode.Ok, (object)details);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, "", StatusCode.NotFound, (object)details);
                }

                //response = employee == null
                //    ? Common.ApiCommonResponse(false, "", StatusCode.NotFound)
                //    : Common.ApiCommonResponse(true, "", StatusCode.Ok, employee);
            }
            catch (Exception e)
            {
                InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse CheckForClockInClockOut(string employeeId, string referralIds)
        {
            ApiResponse response=new ApiResponse();
            try
            {
                var timeUtc = DateTime.UtcNow;
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
                var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
                var result = (int)GetScalar(StoredProcedure.API_CheckForClockInClockOut, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeID, employeeId),
                    new SearchValueData(Properties.ReferralIds, referralIds),
                    new SearchValueData(Properties.Today, today.ToString(Constants.DbDateFormat))
                });
                
                
                response.Data = result;
            }
            catch (Exception e)
            {
                InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse ClockIn(string employeeId, string referralIds)
        {
            ApiResponse response;
            try
            {
                var timeUtc = DateTime.UtcNow;
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
                var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.IVR_ClockIn, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeID, employeeId),
                    new SearchValueData(Properties.ReferralIds, referralIds),
                    new SearchValueData(Properties.ApprovalRequiredIVRBypassPermission, Constants.ApprovalRequiredIVRBypassPermission),
                    new SearchValueData(Properties.BypassAction, Convert.ToString((int)BypassActions.Pending)),
                    new SearchValueData(Properties.StartDateBefore,
                        today.AddMinutes(ConfigSettings.ClockTimeBefore).ToString(Constants.DbDateTimeFormat)),
                    new SearchValueData(Properties.StartDateAfter,
                        today.AddMinutes(ConfigSettings.ClockTimeAfter).ToString(Constants.DbDateTimeFormat)),
                    new SearchValueData(Properties.ClockInTime, today.ToString(Constants.DbDateTimeFormat))
                });
                response = result.TransactionResultId > 0
                    ? Common.ApiCommonResponse(true, "", StatusCode.Ok)
                    : Common.ApiCommonResponse(false, "", StatusCode.NotFound);


                #region CHECK FOR INSTANT NO SCHEDULE CLOCK IN _ CLOCK OUT PERMIISION CHECK
                int details = (int)GetScalar(StoredProcedure.API_CheckForPermissionExist, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeID, Convert.ToString(employeeId)),
                    new SearchValueData(Properties.IVRInstantNoSchClockInOut, Constants.IVRInstantNoSchClockInOut)
                });
                response.Data = details;
                #endregion

            }
            catch (Exception e)
            {
                InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

       

        public ApiResponse IVRBypassClockIn(string employeeId)
        {
            ApiResponse response;
            try
            {
                var timeUtc = DateTime.UtcNow;
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
                var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.IVR_Bypass_ClockIn, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeID, employeeId),
                    new SearchValueData(Properties.StartDateBefore,
                        today.AddMinutes(ConfigSettings.ClockTimeBefore).ToString(Constants.DbDateTimeFormat)),
                    new SearchValueData(Properties.StartDateAfter,
                        today.AddMinutes(ConfigSettings.ClockTimeAfter).ToString(Constants.DbDateTimeFormat)),
                    new SearchValueData(Properties.ClockInTime, today.ToString(Constants.DbDateTimeFormat))
                });
                response = result.TransactionResultId > 0
                    ? Common.ApiCommonResponse(true, "", StatusCode.Ok)
                    : Common.ApiCommonResponse(false, "", StatusCode.NotFound);


                #region CHECK FOR INSTANT NO SCHEDULE CLOCK IN _ CLOCK OUT PERMIISION CHECK
                int details = (int)GetScalar(StoredProcedure.API_CheckForPermissionExist, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeID, Convert.ToString(employeeId)),
                    new SearchValueData(Properties.IVRInstantNoSchClockInOut, Constants.IVRInstantNoSchClockInOut)
                });
                response.Data = details;
                #endregion


            }
            catch (Exception e)
            {
                InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }


        public ApiResponse ClockOut(string employeeId, string referralIds)
        {
            ApiResponse response;
            try
            {
                var timeUtc = DateTime.UtcNow;
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
                var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.IVR_ClockOut, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeID, employeeId),
                    new SearchValueData(Properties.ReferralIds, referralIds),
                    new SearchValueData(Properties.ApprovalRequiredIVRBypassPermission, Constants.ApprovalRequiredIVRBypassPermission),
                    new SearchValueData(Properties.BypassAction,Convert.ToString((int)BypassActions.Pending)),
                    new SearchValueData(Properties.StartDateBefore,
                        today.AddMinutes(ConfigSettings.ClockTimeBefore).ToString(Constants.DbDateTimeFormat)),
                    new SearchValueData(Properties.StartDateAfter,
                        today.AddMinutes(ConfigSettings.ClockTimeAfter).ToString(Constants.DbDateTimeFormat)),
                    new SearchValueData(Properties.ClockOutTime, today.ToString(Constants.DbDateTimeFormat))
                });
                response = result.TransactionResultId > 0
                    ? Common.ApiCommonResponse(true, "", StatusCode.Ok,(object)result.TransactionResultId)
                    : Common.ApiCommonResponse(false, "", StatusCode.NotFound);
                


                if (!response.IsSuccess)
                {
                    #region CHECK FOR INSTANT NO SCHEDULE CLOCK IN _ CLOCK OUT PERMIISION CHECK
                    int details = (int)GetScalar(StoredProcedure.API_CheckForPermissionExist, new List<SearchValueData>
                    {
                        new SearchValueData(Properties.EmployeeID, Convert.ToString(employeeId)),
                        new SearchValueData(Properties.IVRInstantNoSchClockInOut, Constants.IVRInstantNoSchClockInOut)
                    });
                    response.Data = details;
                    #endregion
                }

            }
            catch (Exception e)
            {
                InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }
       
        public ApiResponse IVRBypassClockOut(string employeeId)
        {
            ApiResponse response;
            try
            {
                var timeUtc = DateTime.UtcNow;
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
                var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.IVR_Bypass_ClockOut, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeID, employeeId),
                    new SearchValueData(Properties.StartDateBefore,
                        today.AddMinutes(ConfigSettings.ClockTimeBefore).ToString(Constants.DbDateTimeFormat)),
                    new SearchValueData(Properties.StartDateAfter,
                        today.AddMinutes(ConfigSettings.ClockTimeAfter).ToString(Constants.DbDateTimeFormat)),
                    new SearchValueData(Properties.ClockOutTime, today.ToString(Constants.DbDateTimeFormat))
                });
                response = result.TransactionResultId > 0
                    ? Common.ApiCommonResponse(true, "", StatusCode.Ok)
                    : Common.ApiCommonResponse(false, "", StatusCode.NotFound);


                #region CHECK FOR INSTANT NO SCHEDULE CLOCK IN _ CLOCK OUT PERMIISION CHECK
                int details = (int)GetScalar(StoredProcedure.API_CheckForPermissionExist, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeID, Convert.ToString(employeeId)),
                    new SearchValueData(Properties.IVRInstantNoSchClockInOut, Constants.IVRInstantNoSchClockInOut)
                });
                response.Data = details;
                #endregion

            }
            catch (Exception e)
            {
                InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }



        public ApiResponse GetPatientID(string accountId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                int details = (int)GetScalar(StoredProcedure.API_GetPatientID, new List<SearchValueData>
                {
                    new SearchValueData(Properties.AccountNumber, Convert.ToString(accountId))
                });
                response.Data = details;
                response.IsSuccess = true;
            }
            catch (Exception e)
            {
                InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
                response = Common.InternalServerError(e.Message);
            }
            return response;

        }
        public ApiResponse CreatePendingScheduleClockInOut(string empId, string referralId, bool isClockIn)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var timeUtc = DateTime.UtcNow;
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
                var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
                
                GetScalar(StoredProcedure.API_CreatePendingScheduleClockInOut, new List<SearchValueData>
                {
                    new SearchValueData(Properties.EmployeeID, Convert.ToString(empId)),
                    new SearchValueData(Properties.ReferralID, Convert.ToString(referralId)),
                    //new SearchValueData(Properties.Time, Common.ConvertDateToOrgTimeZone(DateTime.Now).ToString(Constants.DbDateTimeFormat)),
                    new SearchValueData(Properties.Time, today.ToString(Constants.DbDateTimeFormat)),
                    new SearchValueData(Properties.IsClockIN, Convert.ToString(isClockIn)),
                });


                response.IsSuccess = true;
            }
            catch (Exception e)
            {
                InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }
        
    }
}