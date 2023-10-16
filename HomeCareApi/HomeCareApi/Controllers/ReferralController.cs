using System.Web.Http;
using HomeCareApi.Infrastructure.DataProvider;
using HomeCareApi.Infrastructure.IDataProvider;
using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.ViewModel;
using HomeCareApi.Models.General;
using HomeCareApi.Infrastructure.Attributes;
using System.Web;
using System.Linq;
using HomeCareApi.Infrastructure;
using HomeCareApi.Resources;
using System;
using System.Net;

namespace HomeCareApi.Controllers
{
    public class ReferralController : BaseController
    {
        private IReferralDataProvider _referralDataProvider;

        /// <summary>
        /// This method will fetch referral detail.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse GetReferralDetail(ApiRequest<ReferralTaskModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetReferralDetail(request);
        }

        [HttpPost]
        public ApiResponse GetPatientList(ApiRequest<ListModel<SearchPatient>> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetPatientList(request);
        }

        //It is called when user tap om patient from patient list.
        [HttpPost]
        public ApiResponse GetPatientDetail(ApiRequest<ReferralTaskModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetPatientDetail(request);
        }

        [HttpPost]
        public ApiResponse GetIncompletedPastVisit(ApiRequest<SearchDetail> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetIncompletedPastVisit(request);
        }

        [HttpPost]
        public ApiResponse GetPastVisits(ApiRequest<ListModel<SearchDetail>> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetPastVisits(request);
        }

        [HttpPost]
        public ApiResponse GetIncompletedEmpVisitHistory(ApiRequest<SearchVisitHistory> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetIncompletedEmpVisitHistory(request);
        }

        [HttpPost]
        public ApiResponse GetEmpVisitHistory(ApiRequest<ListModel<SearchVisitHistory>> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetEmpVisitHistory(request);
        }


        [HttpPost]
        public ApiResponse CheckClockOut(ApiRequest<ClockInModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.CheckClockOut(request);
        }
        [HttpPost]
        public ApiResponse CheckClockIn(ApiRequest<ClockInModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.CheckClockIn(request);
        }

        [HttpPost]
        public ApiResponse SetEmployeeVisitTime(ApiRequest<ClockInModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.SetEmployeeVisitTime(request);
        }

        [HttpPost]
        public ApiResponse UpdateEmployeeVisitTime(ApiRequest<EmpVisitModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.UpdateEmployeeVisitTime(request);
        }

        [HttpPost]
        public ApiResponse PatientServiceDenied(ApiRequest<PatientModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.PatientServiceDenied(request);
        }

        [HttpPost]
        public ApiResponse GetReferralTasks(ApiRequest<ReferralTaskModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetReferralTasks(request);
        }

        [HttpPost]
        public ApiResponse GetTaskFormList(ApiRequest<TaskFormModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetTaskFormList(request);
        }

        [HttpPost]
        public ApiResponse SaveTaskForm(ApiRequest<SaveFormModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.SaveTaskForm(request);
        }

        [HttpPost]
        public ApiResponse SaveTaskOrbeonForm(ApiRequest<SaveOrbeonFormModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.SaveTaskOrbeonForm(request);
        }

        [HttpPost]
        public ApiResponse DeleteTaskOrbeonForm(ApiRequest<DeleteOrbeonFormModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.DeleteTaskOrbeonForm(request);
        }

        [HttpPost]
        public ApiResponse GetReferralConclusions(ApiRequest<ReferralTaskModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetReferralConclusions(request);
        }

        [HttpPost]
        public ApiResponse AddTask(ApiRequest<AddTaskModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.AddTask(request);
        }

        [HttpPost]
        public ApiResponse DeleteTask(ApiRequest<DeleteTaskModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.DeleteTask(request);
        }

        [HttpPost]
        public ApiResponse GetTaskList(ApiRequest<TaskListModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetTaskList(request);
        }

        [HttpPost]
        public ApiResponse CheckRequiredTask(ApiRequest<TaskListModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.CheckRequiredTask(request);
        }

        [HttpPost]
        public ApiResponse AddConclusion(ApiRequest<ConclusionDetailList> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.AddConclusion(request);
        }

        [HttpPost]
        public ApiResponse OnGoingVisit(ApiRequest<ReferralTaskModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.OnGoingVisit(request);
        }

        [HttpPost]
        [IgnoreAuthentication(true)]
        public ApiResponse CheckPatientVisit(ApiRequest<ReferralTaskModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.CheckPatientVisit(request);
        }

        [HttpPost]
        public ApiResponse GetPatientLocationDetail(ApiRequest<PatientModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetPatientLocationDetail(request);
        }



        [HttpPost]
        public ApiResponse GetInternalMsgList(ApiRequest<ListModel<SearchIMModel>> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetInternalMsgList(request);
        }

        [HttpPost]
        public ApiResponse ResolvedInternalMsg(ApiRequest<IMResolvedModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.ResolvedInternalMsg(request);
        }

        [HttpPost]
        public ApiResponse OnHomePageLoad(ApiRequest<UnreadMsgCountModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.OnHomePageLoad(request);
        }

        [HttpPost]
        public ApiResponse GetEmployeePatientList(ApiRequest<SearchIMModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetEmployeePatientList(request);
        }

        [HttpPost]
        public ApiResponse SendInternalMessage(ApiRequest<InternalMessage> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.SendInternalMessage(request);
        }

        [HttpPost]
        public ApiResponse SentInternalMsgList(ApiRequest<ListModel<SearchIMModel>> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.SentInternalMsgList(request);
        }


        #region PCATimeSheet
        [HttpPost]
        public ApiResponse GetBeneficiaryDetail(ApiRequest<ReferralTaskModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetBeneficiaryDetail(request);
        }

        [HttpPost]
        public ApiResponse SaveBeneficiaryDetail(ApiRequest<BeneficiaryDetail> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.SaveBeneficiaryDetail(request);
        }


        [HttpPost]
        public ApiResponse GetPCATask(ApiRequest<TaskListModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetPCATask(request);
        }

        [HttpPost]
        public ApiResponse SavePCATask(ApiRequest<OtherTask> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.SavePCATask(request);
        }

        [HttpPost]
        public ApiResponse GetPCAConclusion(ApiRequest<ReferralTaskModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetPCAConclusion(request);
        }

        [HttpPost]
        public ApiResponse GetPCASignature(ApiRequest<GetPCASiganture> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetPCASignature(request);
        }


        [HttpPost]
        [IgnoreActionForIncomingRequest(true)]
        [ValidateMimeMultipartContentFilter]
        public ApiResponse SavePCAWithSignature()
        {
            _referralDataProvider = new ReferralDataProvider();
            ApiResponse response;

            if (HttpContext.Current.Request.Files.AllKeys.Any() && HttpContext.Current.Request.Files.Count > 0)
            {
                //// Get the current request contain all the information about posted files and input params
                HttpRequest currentHttpRequest = HttpContext.Current.Request; //.Files["files[]"];
                var token = HttpContext.Current.Request.Params[Constants.TokenParam];
                var key = HttpContext.Current.Request.Params[Constants.KeyParam];
                var companyName = HttpContext.Current.Request.Params[Constants.CompanyNameParam];

                var employeeVisitID = HttpContext.Current.Request.Params[Constants.EmployeeVisitID];
                var employeeSignatureID = HttpContext.Current.Request.Params[Constants.EmployeeSignatureID];
                var scheduleID = HttpContext.Current.Request.Params[Constants.ScheduleID];
                var Lat = HttpContext.Current.Request.Params[Constants.Lat];
                var Long = HttpContext.Current.Request.Params[Constants.Long];
                var IPAddress = HttpContext.Current.Request.Params[Constants.IPAddress];

                // check token value
                if (string.IsNullOrEmpty(token))
                {
                    response = Common.ApiCommonResponse(false, Resource.TokenNotFound,
                        Models.ApiModel.StatusCode.TokenExpired);
                    return response;
                }
                bool isValidToken = (CacheHelper.IsValidToken(token));

                if (isValidToken)
                {
                    PostFileModel model = new PostFileModel
                    {
                        EmployeeVisitID = Convert.ToInt64(employeeVisitID),
                        ScheduleID = Convert.ToInt64(scheduleID),
                        EmployeeSignatureID = Convert.ToInt64(employeeSignatureID),
                        Lat = Convert.ToDecimal(Lat),
                        Long = Convert.ToDecimal(Long),
                        IPAddress = IPAddress
                    };

                    ApiRequest<PostFileModel> request = new ApiRequest<PostFileModel>
                    {
                        Data = model,
                        Key = key,
                        Token = token,
                        CompanyName = companyName
                    };
                    response = _referralDataProvider.SavePCAWithSignature(currentHttpRequest, request);
                    //response = null;
                }
                else
                {
                    response = Common.ApiCommonResponse(false, Resource.TokenNotFound, Models.ApiModel.StatusCode.TokenExpired);
                }
            }
            else
            {
                response = Common.ApiCommonResponse<FileListModel>(false, Resource.FileNotSelected, Models.ApiModel.StatusCode.NotFound);
            }
            return response;
        }

        [HttpPost]
        public ApiResponse SavePCAWithoutSignature(ApiRequest<PCAModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.SavePCAWithoutSignature(request);
        }

        [HttpPost]
        public ApiResponse SavePCA(ApiRequest<PCAModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.SavePCA(request);
        }

        [HttpPost]
        public ApiResponse DeletePCASignature(ApiRequest<Signature> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.DeletePCASignature(request);
        }

        #endregion

        #region Profile
        [HttpPost]
        public ApiResponse GetProfileDetail(ApiRequest<EmpProfile> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetProfileDetail(request);
        }


        [HttpPost]
        public ApiResponse SaveProfileWithoutSignature(ApiRequest<EmployeeProfileDetails> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.SaveProfileWithoutSignature(request);
        }

        [HttpPost]
        [IgnoreActionForIncomingRequest(true)]
        [ValidateMimeMultipartContentFilter]
        public ApiResponse SaveProfileWithSignature()
        {
            _referralDataProvider = new ReferralDataProvider();
            ApiResponse response;

            if (HttpContext.Current.Request.Files.AllKeys.Any() && HttpContext.Current.Request.Files.Count > 0)
            {
                //// Get the current request contain all the information about posted files and input params
                HttpRequest currentHttpRequest = HttpContext.Current.Request; //.Files["files[]"];
                var token = HttpContext.Current.Request.Params[Constants.TokenParam];
                var key = HttpContext.Current.Request.Params[Constants.KeyParam];
                var companyName = HttpContext.Current.Request.Params[Constants.CompanyNameParam];

                var employeeID = HttpContext.Current.Request.Params[Constants.EmployeeID];
                var employeeSignatureID = HttpContext.Current.Request.Params[Constants.EmployeeSignatureID];
                var firstName = HttpContext.Current.Request.Params[Constants.FirstName];
                var lastName = HttpContext.Current.Request.Params[Constants.LastName];
                var userName = HttpContext.Current.Request.Params[Constants.UserName];
                var mobileNumber = HttpContext.Current.Request.Params[Constants.MobileNumber];
                var ivrPin = HttpContext.Current.Request.Params[Constants.IVRPin];
                var password = HttpContext.Current.Request.Params[Constants.Password];
                var isFingerPrintAuth = HttpContext.Current.Request.Params[Constants.IsFingerPrintAuth];

                // check token value
                if (string.IsNullOrEmpty(token))
                {
                    response = Common.ApiCommonResponse(false, Resource.TokenNotFound,
                        Models.ApiModel.StatusCode.TokenExpired);
                    return response;
                }
                bool isValidToken = (CacheHelper.IsValidToken(token));

                if (isValidToken)
                {
                    PostEmpSignatureModel model = new PostEmpSignatureModel
                    {
                        EmployeeID = Convert.ToInt64(employeeID),
                        EmployeeSignatureID = Convert.ToInt64(employeeSignatureID),
                        FirstName = firstName,
                        LastName = lastName,
                        UserName = userName,
                        Password = password,
                        MobileNumber = mobileNumber,
                        IVRPin = ivrPin,
                        IsFingerPrintAuth = Convert.ToBoolean(isFingerPrintAuth)
                    };

                    ApiRequest<PostEmpSignatureModel> request = new ApiRequest<PostEmpSignatureModel>
                    {
                        Data = model,
                        Key = key,
                        Token = token,
                        CompanyName = companyName
                    };
                    response = _referralDataProvider.SaveProfileWithSignature(currentHttpRequest, request);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, Resource.TokenNotFound, Models.ApiModel.StatusCode.TokenExpired);
                }
            }
            else
            {
                response = Common.ApiCommonResponse<FileListModel>(false, Resource.FileNotSelected, Models.ApiModel.StatusCode.NotFound);
            }
            return response;
        }


        [HttpPost]
        [IgnoreActionForIncomingRequest(true)]
        [ValidateMimeMultipartContentFilter]
        public ApiResponse SavePatientProfileImage()
        {
            _referralDataProvider = new ReferralDataProvider();
            ApiResponse response;

            if (HttpContext.Current.Request.Files.AllKeys.Any() && HttpContext.Current.Request.Files.Count > 0)
            {
                //// Get the current request contain all the information about posted files and input params
                HttpRequest currentHttpRequest = HttpContext.Current.Request; //.Files["files[]"];
                var token = HttpContext.Current.Request.Params[Constants.TokenParam];
                var key = HttpContext.Current.Request.Params[Constants.KeyParam];
                var companyName = HttpContext.Current.Request.Params[Constants.CompanyNameParam];

                var ReferralID = HttpContext.Current.Request.Params[Constants.ReferralID];

                // check token value
                if (string.IsNullOrEmpty(token))
                {
                    response = Common.ApiCommonResponse(false, Resource.TokenNotFound,
                        Models.ApiModel.StatusCode.TokenExpired);
                    return response;
                }
                bool isValidToken = (CacheHelper.IsValidToken(token));

                if (isValidToken)
                {
                    PostRefSignatureModel model = new PostRefSignatureModel
                    {
                        ReferralID = Convert.ToInt64(ReferralID)
                    };

                    ApiRequest<PostRefSignatureModel> request = new ApiRequest<PostRefSignatureModel>
                    {
                        Data = model,
                        Key = key,
                        Token = token,
                        CompanyName = companyName
                    };
                    response = _referralDataProvider.SavePatientProfileImage(currentHttpRequest, request);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, Resource.TokenNotFound, Models.ApiModel.StatusCode.TokenExpired);
                }
            }
            else
            {
                response = Common.ApiCommonResponse<FileListModel>(false, Resource.FileNotSelected, Models.ApiModel.StatusCode.NotFound);
            }
            return response;
        }


        [HttpGet]
        public ServiceResponse DemoFileUpload()
        {
            ServiceResponse response = new ServiceResponse();
            WebClient client = new WebClient();
            string url = "http://localhost:5556";
            //Uri url = new Uri("localhost:5556\\uploads\\upload.txt");
            string filename = @"D:\Projects\DBChanges.txt";
            byte[] bytes = System.IO.File.ReadAllBytes(filename);
            try
            {
                client.Headers.Add("EmployeeID", "170");
                client.UploadData(string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Constants.UploadEmpSignatureURL, "POST", bytes);
                //client.UploadFile(ConfigSettings.UploadEmpSignatureURL, "POST",filename);
                response.IsSuccess = true;
                response.Message = "File Upload Test";
            }
            catch (Exception x1)
            {
                response.Message = x1.Message;
                response.IsSuccess = false;
            }
            return response;
        }
        #endregion

        #region Employee Details for IDCard
        [HttpPost]
        public ApiResponse GetEmpDetailForIdCard(ApiRequest<EmpProfile> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetEmpDetailForIdCard(request);
        }
        #endregion


        [HttpPost]
        public ApiResponse UpdatePatientLatLong(ApiRequest<PatientLatLongModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.UpdatePatientLatLong(request);
        }

        #region Notes
        [HttpPost]
        public ApiResponse AddNote(ApiRequest<AddNoteModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.AddNote(request);
        }

        [HttpPost]
        public ApiResponse DeleteNote(ApiRequest<AddNoteModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.DeleteNote(request);
        }

        [HttpPost]
        public ApiResponse GetNoteList(ApiRequest<ListModel<SearchNoteModel>> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetNoteList(request);
        }
        #endregion

        #region Employee PTO
        [HttpPost]
        public ApiResponse GetEmpPTOList(ApiRequest<ListModel<SearchEmpPTOModel>> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetEmpPTOList(request);
        }

        [HttpPost]
        public ApiResponse SaveEmployeePTO(ApiRequest<EmployeeDayOffModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.SaveEmployeePTO(request);
        }

        [HttpPost]
        public ApiResponse DeleteEmployeePTO(ApiRequest<EmployeeDayOffModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.DeleteEmployeePTO(request);
        }
        #endregion


        #region Case Management (RN)
        [HttpPost]
        public ApiResponse CaseLoadDashboard(ApiRequest<CaseLoadAppointment> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.CaseLoadDashboard(request);
        }

        [HttpPost]
        [IgnoreModelValidation(true)]
        public ApiResponse GetCareTypes(ApiRequest request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetCareTypes(request);
        }

        [HttpPost]
        public ApiResponse CreateCareTypeSchedule(ApiRequest<CareTypeSchedule> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.CreateCareTypeSchedule(request);
        }

        #endregion

        [HttpPost]
        public ApiResponse AddClockInOutLog(ApiRequest<ClockInOutLogModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.AddClockInoutLog(request);
        }

        #region RAL
        [HttpPost]
        public ApiResponse RALGetFacilityList(ApiRequest<EmpProfile> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetFacilityList(request);
        }
        

        [HttpPost]
        public ApiResponse RAL_GetPatientList(ApiRequest<ListModel<SearchPatient>> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.RAL_GetPatientList(request);
        }

        //It is called when user tap om patient from patient list.
        [HttpPost]
        public ApiResponse RAL_GetPatientDetail(ApiRequest<ReferralTaskModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetPatientDetail(request);
        }

        [HttpPost]
        public ApiResponse RAL_OnGoingVisit(ApiRequest<ReferralTaskModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.OnGoingVisit(request);
        }

        [HttpPost]
        public ApiResponse RAL_SetEmployeeVisitTime(ApiRequest<ClockInModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.RAL_SetEmployeeVisitTime(request);
        }
        #endregion

        #region NoteSentence
        [HttpPost]
        public ApiResponse GetNoteSentenceList(ApiRequest<EmployeeModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetNoteSentenceList(request);
        }
        #endregion
        #region AnyTime ClockIn
        [HttpPost]
        public ApiResponse ChangeSchedule(ApiRequest<ChangeScheduleModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.ChangeSchedule(request);
        }
        #endregion

        [HttpPost]
        public ApiResponse GetSurveyQuestionsList(ApiRequest<CovidSurveyQuestionModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetSurveyQuestionsList(request);
        }

        [HttpPost]
        public ApiResponse SaveSurveyAnswers(ApiRequest<CovidSurveySaveModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.SaveSurveyAnswers(request);
        }

        [HttpPost]
        public ApiResponse GetEmployeeSurveyList(ApiRequest<GetCovidSurveyModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetEmployeeSurveyList(request);
        }

        [HttpPost]
        public ApiResponse GetNotesCategory(ApiRequest<long> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetNotesCategory(request);
        }


        /// <summary>
        /// Return Available, Allocated, Used, Remaing and Unallocated Unites for Prior Authorization
        /// </summary>
        /// <param name="request">Expect BillingAuthorizationID as a parameter for Store Procedure</param>
        /// <returns><see cref="PriorAuthorizationModel"/></returns>
        [HttpPost]
        public ApiResponse PrioAuthorization(ApiRequest<PriorAuthorizationRequestModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.PrioAuthorization(request);
        }

        [HttpPost]
        public ApiResponse SaveGroup(ApiRequest<ReferralGroupModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.SaveGroup(request);
        }

        [HttpPost]
        public ApiResponse DeleteGroup(ApiRequest<ReferralGroupModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.DeleteGroup(request);
        }

        [HttpPost]
        public ApiResponse SaveGroupMapping(ApiRequest<ReferralGroupMappingModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.SaveGroupMapping(request);
        }

        [HttpPost]
        public ApiResponse DeleteGroupMapping(ApiRequest<ReferralGroupMappingModel> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.DeleteGroupMapping(request);
        }

        [HttpPost]
        public ApiResponse GetReferralGroupList(ApiRequest<ListModel<SearchReferralGroup>> request)
        {
            _referralDataProvider = new ReferralDataProvider();
            return _referralDataProvider.GetReferralGroupList(request);
        }
    }
}
