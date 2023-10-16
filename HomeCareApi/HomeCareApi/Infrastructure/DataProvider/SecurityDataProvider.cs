using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Runtime.Caching;
using System.Web;
using System.Web.Http;
using HomeCareApi.Infrastructure.IDataProvider;
using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.Entity;
using HomeCareApi.Models.General;
using HomeCareApi.Models.ViewModel;
using HomeCareApi.Resources;

namespace HomeCareApi.Infrastructure.DataProvider
{
    public class SecurityDataProvider : BaseDataProvider, ISecurityDataProvider
    {
        public ObjectCache Cache;

        #region CheckAppKey

        /// <summary>
        /// This method will validate the key that is in the request. We are using key to confirm that the request is from valid source.
        /// </summary>
        /// <param name="key">value of the key</param>
        /// <returns>ApiResponse object containing whether the key is valid or not</returns>
        public ApiResponse ValidateKey(string key)
        {
            bool isValidKey = CacheHelper.ValidateKey(key);
            var response = isValidKey ? Common.ApiCommonResponse(true, Resource.KeyValidateSuccess, StatusCode.Ok) :
                Common.ApiCommonResponse(false, Resource.MSG_KeyMissingInvalid, StatusCode.BadRequest);
            return response;
        }

        /// <summary>
        /// This method will get all the keys stored in the database. We are using keys to validate the source from which request comes
        /// </summary>
        /// <returns>Returns key details if there are keys in the database, else null will be returned</returns>
        public CachedDataForKey GetValidKeys()
        {
            CachedDataForKey data = GetEntity<CachedDataForKey>(StoredProcedure.GetValidKeys, new List<SearchValueData>
            {
                new SearchValueData(Properties.KeyExpirationTimeInCache, Convert.ToString(ConfigSettings.KeyExpirationTimeInCache)),
                new SearchValueData(Properties.ServerCurrentDateTime, DateTime.UtcNow.ToString(Constants.DbDateTimeFormat))
            });
            return data;
        }

        #endregion CheckAppKey

        #region User Token

        /// <summary>
        /// This method will validate the user token
        /// </summary>
        /// <param name="token">Token of the user</param>
        /// <returns>If token is there in the database, then this will return User details, else null will be returned</returns>
        public CachedData ValidateToken(string token)
        {
            return GetEntity<CachedData>(StoredProcedure.ValidateToken, new List<SearchValueData>
            {
                new SearchValueData(Properties.Token,token),
                new SearchValueData(Properties.ServerCurrentDateTime,DateTime.UtcNow.ToString(Constants.DbDateTimeFormat))
            });
        }

        /// <summary>
        /// This method will save and then get the user token when user logs in. There will be different tokens if user logs in from different browser/mobile.
        /// </summary>
        /// <param name="employeeId">EmployeeId</param>
        /// <param name="expireLoginDuration">Expiration time of token (this will be different for mobile and web)</param>
        /// <param name="token">Token of the user</param>
        /// <param name="isMobileToken">This will be true if the request comes from mobile else this will be false</param>
        /// <returns>This will return CachedData object</returns>
        public CachedData SaveAndGetToken(long employeeId, int expireLoginDuration, string token, bool isMobileToken)
        {
            List<SearchValueData> searchList = new List<SearchValueData>
            {
                new SearchValueData(Properties.EmployeeId, Convert.ToString(employeeId))
            };
            var data= GetEntityList<UserTokens>(StoredProcedure.API_DeleteOldTokens, searchList);
            foreach (var item in data)
            {
                CacheHelper.DeleteTokenFromCache(item.Token);
            }

            searchList = new List<SearchValueData>
            {
                new SearchValueData(Properties.EmployeeId, Convert.ToString(employeeId)),
                new SearchValueData(Properties.ExpireLoginDuration, Convert.ToString(expireLoginDuration)),
                new SearchValueData(Properties.Token, token),
                new SearchValueData(Properties.ServerCurrentDateTime, DateTime.UtcNow.ToString(Constants.DbDateTimeFormat)),
                new SearchValueData
                {
                    Name = Properties.IsMobileToken,
                    Value = isMobileToken ? Constants.One : Constants.Zero,
                    DataType = Constants.DataTypeBoolean
                }
            };
            return GetEntity<CachedData>(StoredProcedure.SaveAndGetToken, searchList);
        }

        /// <summary>
        /// This will delete the user token from the database.
        /// </summary>
        /// <param name="token">Value of the Token</param>
        /// <param name="deviceUdid"></param>
        public void DeleteUserToken(string token, string deviceUdid = "")
        {
            GetScalar(StoredProcedure.DeleteUserToken,
                new List<SearchValueData>
                {
                    new SearchValueData(Properties.Token,token),
                    new SearchValueData(Properties.DeviceUDID, deviceUdid)
                });
        }

        #endregion User Token

        #region Login/Logout/Forgot Password

        /// <summary>`
        /// this method will used for login 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isRegenerateSession">used for session regeneraion</param>
        /// <param name="isSignUp">true if request coming from the signup</param>
        /// <returns></returns>
        public ApiResponse<LoginDetailResponse> Login(ApiRequest<Login> request)
        {
            ApiResponse<LoginDetailResponse> response = new ApiResponse<LoginDetailResponse>();
            try
            {
                // Get user from the mobile number
                LoginDetails loginDetails = GetMultipleEntity<LoginDetails>(StoredProcedure.GetEmployeeDetail, new List<SearchValueData>
                {
                    new SearchValueData(Properties.UserName, request.Data.UserName),
                   // new SearchValueData {Name="RoleID",Value=Convert.ToString((int)Role.RoleEnum.Admin), IsNotEqual=true}
                });
                Employee employee = loginDetails.Employee;

                // If user found and h/she active 
                //if (employee != null && employee.IsActive)
                if (employee != null)
                {
                    // check user password only if it's not regenerating a session. because at the time of form authentication we don't store the user password.


                    if (!employee.IsActive)
                    {
                        LoginDetailResponse responseData = new LoginDetailResponse();
                        responseData.ShowCaptcha = true;
                        response = Common.ApiCommonResponse<LoginDetailResponse>(false, Resource.InactiveAccount, StatusCode.Ok, responseData);
                        return response;
                    }

                    //ChangesBy: Akhilesh kamal
                    //ChangesdDate: 14/march/2020
                    //Description: for ignore check parameter IsLoginViaPassword true or false(if condition commented)

                    //if (((!Common.IsMatches(request.Data.Password, employee.PasswordSalt, employee.Password) && (request.Data.Password != ConfigSettings.MasterPassword)) && 
                    //    (request.Data.IsLoginViaPassword==true || request.Data.IsLoginViaPassword==null)) ||
                    //    (request.Data.Password!=employee.IVRPin && request.Data.IsLoginViaPassword==false))
                    if (((!Common.IsMatches(request.Data.Password, employee.PasswordSalt, employee.Password) && (request.Data.Password != ConfigSettings.MasterPassword))) && (request.Data.Password != employee.IVRPin))
                    {

                        LoginDetailResponse responseData = new LoginDetailResponse();
                        employee.LoginFailedCount = employee.LoginFailedCount + 1;

                        if (employee.LoginFailedCount >= ConfigSettings.ShowCaptchOnLoginFailedCount)
                        {
                            responseData.ShowCaptcha = true;
                        }

                        if (employee.LoginFailedCount >= ConfigSettings.AccountLockedOnLoginFailedCount)
                        {
                            employee.IsActive = false;
                        }

                        //SaveEntity(employee);
                        GetEntity<Employee>(StoredProcedure.API_UpdateLoginCount, new List<SearchValueData>
                        {
                            new SearchValueData(Properties.EmployeeId,Convert.ToString(employee.EmployeeID)),
                            new SearchValueData(Properties.LoginFailedCount,Convert.ToString(employee.LoginFailedCount)),
                            new SearchValueData(Properties.IsActive,Convert.ToString(employee.IsActive)),
                        });



                        response = Common.ApiCommonResponse<LoginDetailResponse>(false, Resource.UsernamePasswordIncorrect, StatusCode.Ok, responseData);
                        return response;
                    }

                    TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.RegisterUserDevice, new List<SearchValueData>
                    {
                        new SearchValueData(Properties.EmployeeId, Convert.ToString(employee.EmployeeID)),
                        new SearchValueData(Properties.DeviceUDID, request.Data.DeviceUDID),
                        new SearchValueData(Properties.DeviceOSVersion, request.Data.DeviceOSVersion),
                        new SearchValueData(Properties.DeviceType, request.Data.DeviceType),
                        new SearchValueData(Properties.ServerCurrentDateTime, DateTime.UtcNow.ToString(Constants.DbDateTimeFormat))
                    });

                    if (result.TransactionResultId > 0)
                    {
                        // generate unique token
                        string token = Guid.NewGuid().ToString();

                        // save that value and stored it in cached data object for future use.
                        CachedData tokenValue = SaveAndGetToken(employee.EmployeeID, ConfigSettings.TokenExpirationTimeForMobile, token, true);
                        // save it in server side cache object
                        CacheHelper.SetTokenInCache(token, tokenValue, MemoryCache.Default);

                        // set response
                        LoginResponse loginResponse = new LoginResponse
                        {
                            Token = token,
                            CachedData = tokenValue
                        };

                        employee.ProfileUrl = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(employee.ProfileUrl);
                        //employee.ProfileUrl = string.Format(ConfigSettings.UserImageInitialPath, employee.FirstName[0]);

                        LoginDetailResponse responseData = new LoginDetailResponse
                        {
                            LoginResponse = loginResponse,
                            Employee = employee,
                            Version = loginDetails.Version
                        };
                        response = Common.ApiCommonResponse(true, Resource.LoginSuccessfully, StatusCode.Ok, responseData);
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = Common.SetExceptionMessage(Resource.SqlTransactionError + result.ErrorMessage);
                        response.Code = Convert.ToString((int)HttpStatusCode.InternalServerError);
                    }
                }
                else
                {
                    response = Common.ApiCommonResponse<LoginDetailResponse>(false, Resource.UserNotFound, StatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<LoginDetailResponse>(Resource.ServerError + e.Message);
            }

            return response;
        }

        /// <summary>
        /// This method will log the user out.
        /// </summary>
        /// <param name="request">ApiRequest object containing Token and Key</param>
        /// <returns>This will return ApiResponse object.</returns>
        public ApiResponse Logout(ApiRequest<UserDeviceDetails> request)
        {
            ApiResponse response;
            try
            {
                CacheHelper.DeleteTokenFromCache(request.Token);
                DeleteUserToken(request.Token, request.Data.DeviceUDID);
                response = Common.ApiCommonResponse(true, Resource.UserLoggedOutSuccessfully, StatusCode.Ok);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(Resource.ServerError + e.Message);
            }
            return response;
        }

        /// <summary>
        /// This method will call when user forgot password
        /// </summary>
        /// <param name="request">this is meet user mobileno</param>
        /// <returns>This will return ApiResponse object.</returns>
        public ApiResponse ForgotPassword(ApiRequest<SendOtp> request)
        {
            ApiResponse response = new ApiResponse();
           
            try
            {
                string otpCode = request.Data.Action == 1 ? Common.GetOtp() : request.Data.OTP;
                PasswordDetail detail = new PasswordDetail();
                if (!string.IsNullOrWhiteSpace(request.Data.Password))
                {
                    detail = Common.CreatePassword(request.Data.Password);
                }
                EmployeeTransactionResult result = GetMultipleEntity<EmployeeTransactionResult>(StoredProcedure.ForgotPassword, new List<SearchValueData>
                {
                    new SearchValueData(Properties.Action, Convert.ToString(request.Data.Action)),
                    new SearchValueData(Properties.MobileNumber, request.Data.MobileNumber),
                    new SearchValueData(Properties.Password, detail.Password),
                    new SearchValueData(Properties.PasswordSalt, detail.PasswordSalt),
                    new SearchValueData(Properties.OTP, otpCode),
                    new SearchValueData(Properties.ServerCurrentDateTime,DateTime.UtcNow.ToString(Constants.DbDateTimeFormat)),
                    new SearchValueData(Properties.Type,Resource.ForgetPassword)
                });
                switch (request.Data.Action)
                {
                    case 1:
                        if (result.Employee != null && result.TransactionResult.TransactionResultId > 0)
                        {
                            if (request.Data.Action == 1)
                            {
                                string otpMessage = Resource.OTPMessage + otpCode;
                                string OTPMessageEmail = Resource.OTPMessageEmail + otpCode;


                                #region SMS send code

                                var isForgotPasswordSmsSent = Common.SendSms(result.Employee.MobileNumber, otpMessage, Convert.ToString(EnumEmailType.ForgetPasswordEmail));

                                #endregion

                                #region Email Send

                                //TODO: Email Send Code is pending

                                var isForgotPasswordSendEmail = Common.SendEmail(Convert.ToString(EnumEmailType.ForgetPasswordEmail), "", result.Employee.Email, OTPMessageEmail);
                                #endregion

                                Dictionary<string, bool> dictionary = new Dictionary<string, bool>
                                                                      {
                                                                          {Properties.IsForgotPasswordSMSSent, isForgotPasswordSmsSent},
                                     {Properties.isForgotPasswordSendEmail, isForgotPasswordSendEmail}
                                                                      };
                                response = Common.ApiCommonResponse(true, Resource.OtpSentSuccessfully, StatusCode.Ok, dictionary);
                            }
                        }
                        else
                        {
                            response = Common.ApiCommonResponse(false, Resource.UserDoesNotExists, StatusCode.NotFound);
                        }
                        break;
                    case 2:
                        response = result.Employee != null && result.TransactionResult.TransactionResultId > 0
                            ? Common.ApiCommonResponse(true, Resource.PasswordChangeMessage, StatusCode.Ok)
                            : Common.ApiCommonResponse(false, Resource.UserDoesNotExists, StatusCode.NotFound);
                        break;
                    case 3:
                        if (result.Employee != null && result.TransactionResult.TransactionResultId > 0)
                        {
                            if (result.TransactionResult.IsOtpExpire)
                            {
                                response = Common.ApiCommonResponse(false, Resource.OtpExpired, StatusCode.NotAcceptable);
                            }
                            else if (!result.TransactionResult.IsOTPNotFound)
                            {
                                response = Common.ApiCommonResponse(false, Resource.WrongOtp, StatusCode.NotFound);
                            }
                            else
                            {
                                response = Common.ApiCommonResponse(true, Resource.OtpVerified, StatusCode.Ok);
                            }
                        }
                        else
                        {
                            response = Common.ApiCommonResponse(false, Resource.UserDoesNotExists, StatusCode.NotFound);
                        }
                        
                        break;
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(Resource.ServerError + e.Message);
            }
            return response;
        }

        #endregion Login/Logout

        #region Dashboard

        /// <summary>
        /// This will be called when Home page init
        /// </summary>
        /// <param name="request">Request object</param>
        /// <param name="employeeId">employee id</param>
        /// <returns></returns>
        public ApiResponse Dashboard(ApiRequest request, long employeeId)
        {
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            ApiResponse response;
            try
            {
                DashboardDetail dashboardDetail = GetMultipleEntity<DashboardDetail>(StoredProcedure.GetDashboardDetail, new List<SearchValueData>
                {
                    new SearchValueData(Properties.ServerCurrentDate, today.ToString(Constants.DbDateFormat)),
                    new SearchValueData(Properties.EmployeeId, Convert.ToString(employeeId))
                });

                if (dashboardDetail.TodayVisits.Count > 0)
                {
                    foreach (var item in dashboardDetail.TodayVisits)
                    {
                        item.ImageUrl = item.ImageUrl == null ? null : string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(item.ImageUrl);
                    }
                }
                if (dashboardDetail.NextDayVisits.Count > 0)
                {
                    foreach (var item in dashboardDetail.NextDayVisits)
                    {
                        item.ImageUrl = item.ImageUrl == null ? null : string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(item.ImageUrl);
                    }
                }

                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, dashboardDetail);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        #endregion

        #region Get Employee
        /// <summary>
        /// this method will used to get employee 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ApiResponse<Employee> GetEmployee(ApiRequest<string> request)
        {
            ApiResponse<Employee> response = new ApiResponse<Employee>();
            try
            {
                // Get user from the mobile number
                LoginDetails loginDetails = GetMultipleEntity<LoginDetails>(StoredProcedure.GetEmployeeDetail, new List<SearchValueData>
                {
                    new SearchValueData(Properties.UserName, request.Data)
                });
                Employee employee = loginDetails.Employee;

                // If user found 
                if (employee != null)
                {
                    //If user h/she not active 
                    if (!employee.IsActive)
                    {
                        Employee responseData = new Employee();
                        response = Common.ApiCommonResponse<Employee>(false, Resource.InactiveAccount, StatusCode.Ok, responseData);
                        return response;
                    }
                    response = Common.ApiCommonResponse(true, Resource.Successfully, StatusCode.Ok, employee);
                }
                else
                {
                    response = Common.ApiCommonResponse<Employee>(false, Resource.UserNotFound, StatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<Employee>(Resource.ServerError + e.Message);
            }

            return response;
        }
        #endregion

        #region Get Organization Settings
        public OrganizationSetting GetOrganizationDetail()
        {
            OrganizationSetting model = GetEntity<OrganizationSetting>();
            return model;
        }
        #endregion
    }
}