using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.General;
using HomeCareApi.Models.ViewModel;
using System.Web;
using System;

namespace HomeCareApi.Infrastructure.IDataProvider
{
    public interface IReferralDataProvider
    {
        ApiResponse GetReferralDetail(ApiRequest<ReferralTaskModel> request);
        ApiResponseList<PatientList> GetPatientList(ApiRequest<ListModel<SearchPatient>> request);
        ApiResponse GetPatientDetail(ApiRequest<ReferralTaskModel> request);
        ApiResponse GetIncompletedPastVisit(ApiRequest<SearchDetail> request);
        ApiResponseList<GetPastVisitModel> GetPastVisits(ApiRequest<ListModel<SearchDetail>> request);
        ApiResponse GetIncompletedEmpVisitHistory(ApiRequest<SearchVisitHistory> request);
        ApiResponseList<EmpVisitHistory> GetEmpVisitHistory(ApiRequest<ListModel<SearchVisitHistory>> request);
        ApiResponse CheckClockOut(ApiRequest<ClockInModel> request);
        ApiResponse CheckClockIn(ApiRequest<ClockInModel> request);
        ApiResponse SetEmployeeVisitTime(ApiRequest<ClockInModel> request);
        ApiResponse UpdateEmployeeVisitTime(ApiRequest<EmpVisitModel> request);
        ApiResponse PatientServiceDenied(ApiRequest<PatientModel> request);
        ApiResponse GetReferralTasks(ApiRequest<ReferralTaskModel> request);
        ApiResponse GetTaskFormList(ApiRequest<TaskFormModel> request);
        ApiResponse SaveTaskForm(ApiRequest<SaveFormModel> request);
        ApiResponse SaveTaskOrbeonForm(ApiRequest<SaveOrbeonFormModel> request);
        ApiResponse DeleteTaskOrbeonForm(ApiRequest<DeleteOrbeonFormModel> request);
        ApiResponse GetReferralConclusions(ApiRequest<ReferralTaskModel> request);
        ApiResponse AddTask(ApiRequest<AddTaskModel> request);
        ApiResponse DeleteTask(ApiRequest<DeleteTaskModel> request);
        ApiResponse GetTaskList(ApiRequest<TaskListModel> request);
        ApiResponse CheckRequiredTask(ApiRequest<TaskListModel> request);
        ApiResponse AddConclusion(ApiRequest<ConclusionDetailList> request);
        ApiResponse OnGoingVisit(ApiRequest<ReferralTaskModel> request);
        ApiResponse CheckPatientVisit(ApiRequest<ReferralTaskModel> request);
        ApiResponse GetPatientLocationDetail(ApiRequest<PatientModel> request);

        ApiResponseList<PatientList> RAL_GetPatientList(ApiRequest<ListModel<SearchPatient>> request);

        ApiResponseList<GetIMListModel> GetInternalMsgList(ApiRequest<ListModel<SearchIMModel>> request);
        ApiResponse ResolvedInternalMsg(ApiRequest<IMResolvedModel> request);
        ApiResponse OnHomePageLoad(ApiRequest<UnreadMsgCountModel> request);
        ApiResponse GetEmployeePatientList(ApiRequest<SearchIMModel> request);
        ApiResponse SendInternalMessage(ApiRequest<InternalMessage> request);
        ApiResponseList<GetIMListModel> SentInternalMsgList(ApiRequest<ListModel<SearchIMModel>> request);

        #region PCATimeSheet
        ApiResponse GetBeneficiaryDetail(ApiRequest<ReferralTaskModel> request);
        ApiResponse SaveBeneficiaryDetail(ApiRequest<BeneficiaryDetail> request);
        ApiResponse GetPCATask(ApiRequest<TaskListModel> request);
        ApiResponse SavePCATask(ApiRequest<OtherTask> request);
        ApiResponse GetPCAConclusion(ApiRequest<ReferralTaskModel> request);
        ApiResponse GetPCASignature(ApiRequest<GetPCASiganture> request);
        ApiResponse SavePCAWithSignature(HttpRequest currentHttpRequest, ApiRequest<PostFileModel> request);
        ApiResponse SavePCAWithoutSignature(ApiRequest<PCAModel> request);
        ApiResponse SavePCA(ApiRequest<PCAModel> request);

        ApiResponse DeletePCASignature(ApiRequest<Signature> request);
        #endregion

        #region Profile
        ApiResponse GetProfileDetail(ApiRequest<EmpProfile> request);
        ApiResponse SaveProfileWithoutSignature(ApiRequest<EmployeeProfileDetails> request);
        ApiResponse SaveProfileWithSignature(HttpRequest currentHttpRequest, ApiRequest<PostEmpSignatureModel> request);
        ApiResponse SavePatientProfileImage(HttpRequest currentHttpRequest, ApiRequest<PostRefSignatureModel> request);
        ApiResponse GetFacilityList(ApiRequest<EmpProfile> request);
        #endregion

        #region Employee Details for IDCard
        ApiResponse GetEmpDetailForIdCard(ApiRequest<EmpProfile> request);
        #endregion

        ApiResponse UpdatePatientLatLong(ApiRequest<PatientLatLongModel> request);
        ApiResponse AddClockInoutLog(ApiRequest<ClockInOutLogModel> request);

        #region Notes
        ApiResponse AddNote(ApiRequest<AddNoteModel> request);
        ApiResponseList<GetNoteListModel> GetNoteList(ApiRequest<ListModel<SearchNoteModel>> request);
        ApiResponse DeleteNote(ApiRequest<AddNoteModel> request);
        #endregion

        #region Employee PTO
        ApiResponseList<GetEmpPTOListModel> GetEmpPTOList(ApiRequest<ListModel<SearchEmpPTOModel>> request);
        ApiResponse SaveEmployeePTO(ApiRequest<EmployeeDayOffModel> request);
        ApiResponse DeleteEmployeePTO(ApiRequest<EmployeeDayOffModel> request);
        #endregion

        #region Case Management (RN)
        ApiResponse CaseLoadDashboard(ApiRequest<CaseLoadAppointment> request);
        ApiResponse GetCareTypes(ApiRequest request);
        ApiResponse CreateCareTypeSchedule(ApiRequest<CareTypeSchedule> request);
        #endregion

        ApiResponse GetNoteSentenceList(ApiRequest<EmployeeModel> request);
        ApiResponse RAL_SetEmployeeVisitTime(ApiRequest<ClockInModel> request);
        ApiResponse ChangeSchedule(ApiRequest<ChangeScheduleModel> request);

        ApiResponse GetSurveyQuestionsList(ApiRequest<CovidSurveyQuestionModel> request);

        ApiResponse SaveSurveyAnswers(ApiRequest<CovidSurveySaveModel> request);

        ApiResponse GetEmployeeSurveyList(ApiRequest<GetCovidSurveyModel> request);

        ApiResponse GetNotesCategory(ApiRequest<long> request);
        ApiResponse PrioAuthorization(ApiRequest<PriorAuthorizationRequestModel> request);

        #region "Referral Groups"
        ApiResponse SaveGroup(ApiRequest<ReferralGroupModel> request);
        ApiResponse DeleteGroup(ApiRequest<ReferralGroupModel> request);
        ApiResponse SaveGroupMapping(ApiRequest<ReferralGroupMappingModel> request);
        ApiResponse DeleteGroupMapping(ApiRequest<ReferralGroupMappingModel> request);

        ApiResponseList<ReferralGroupList> GetReferralGroupList(ApiRequest<ListModel<SearchReferralGroup>> request);
        #endregion
    }
}
