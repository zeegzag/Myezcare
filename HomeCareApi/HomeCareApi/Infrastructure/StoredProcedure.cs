namespace HomeCareApi.Infrastructure
{
    public class StoredProcedure
    {
        public const string GetEZOrbeonData_ByFormID = "GetEZOrbeonData_ByFormID";
        public const string API_SaveOrbeonForm = "API_SaveOrbeonForm";
        public const string API_DeleteTaskOrbeonForm = "API_DeleteTaskOrbeonForm";
        public const string API_AddReferralTaskForm = "API_AddReferralTaskForm";
        public const string API_OpenSavedOrbeonForm = "API_OpenSavedOrbeonForm";

        public const string API_GetCMS485Data = "[dbo].[API_GetCMS485Data]";

        public const string SaveMultipleEmailLogs = "API_SaveMultipleEmailLogs";
        public const string GetValidKeys = "API_GetValidKeys";
        public const string ValidateToken = "API_ValidateToken";
        public const string DeleteUserToken = "API_DeleteUserToken";
        public const string SaveAndGetToken = "API_SaveAndGetToken";
        public const string API_DeleteOldTokens = "API_DeleteOldTokens";


        public const string GetEmployeeDetail = "API_GetEmployeeDetail";
        public const string RegisterUserDevice = "API_RegisterUserDevice";
        public const string ForgotPassword = "API_ForgotPassword";
        public const string GetDashboardDetail = "API_GetDashboardDetail";
        public const string GetReferralDetail = "API_GetReferralDetail";

        public const string GetPastVisitDetail = "API_GetPastVisitDetail";
        public const string GetInternalMsgList = "API_GetInternalMsgList";
        public const string SetEmployeeVisitTime = "API_SetEmployeeVisitTime";
        public const string API_UpdateEmployeeVisitTime = "API_UpdateEmployeeVisitTime";
        public const string API_PatientServiceDenied = "API_PatientServiceDenied";
        public const string GetReferralTasks = "API_GetReferralTasks";
        public const string AddTask = "API_AddTask";
        public const string DeleteTask = "API_DeleteTask";
        public const string GetTaskList = "API_GetTaskList";
        public const string AddConclusion = "API_AddConclusion";
        public const string GetOnGoingVisitDetail = "API_GetOnGoingVisitDetail";
        public const string API_GetPatientLocationDetail = "API_GetPatientLocationDetail";

        public const string IVR_ValidateIvrCode = "API_IVR_ValidateIvrCode";
        public const string API_CheckForPermissionExist = "API_CheckForPermissionExist";
        public const string API_GetPatientID = "API_GetPatientID";
        public const string API_CreatePendingScheduleClockInOut = "API_CreatePendingScheduleClockInOut";
        
        public const string IVR_ClockIn = "API_IVR_ClockIn";
        public const string IVR_ClockOut = "API_IVR_ClockOut";

        public const string IVR_Bypass_ClockIn = "API_IVR_Bypass_ClockIn";
        public const string IVR_Bypass_ClockOut = "API_IVR_Bypass_ClockOut";

        public const string UpdateSurveyDetail = "API_UpdateSurveyDetail";

        public const string ValidateReferralContactNumber = "API_ValidateReferralContactNumber";

        public const string ResolvedInternalMsg = "API_ResolvedInternalMsg";
        public const string API_UnreadMsgCount = "API_UnreadMsgCount";

        public const string GetReferralTasks01 = "API_GetReferralTasks01";

        public const string API_GetBenficiaryDetail = "API_GetBenficiaryDetail";
        public const string API_SaveBeneficiaryDetail = "API_SaveBeneficiaryDetail";
        public const string API_GetPCATask = "API_GetPCATask";
        public const string API_SavePCATask = "API_SavePCATask";
        public const string API_GetPCAConclusion = "API_GetPCAConclusion";
        public const string API_GetPCASignature = "API_GetPCASignature";
        public const string API_PCASigned = "API_PCASigned";
        public const string API_SavePCASignature = "API_SavePCASignature";
        public const string API_SavePCAWithoutSignature = "API_SavePCAWithoutSignature";
        public const string API_UpdateLoginCount = "API_UpdateLoginCount";
        public const string API_GetEmpRefList = "API_GetEmpRefList";
        public const string API_SendInternalMessage = "API_SendInternalMessage";
        public const string API_SentInternalMsgList = "API_SentInternalMsgList";
        public const string API_GetProfileDetail = "API_GetProfileDetail"; 
        public const string API_SaveProfileWithoutSignature = "API_SaveProfileWithoutSignature";
        public const string API_CheckClockOut = "API_CheckClockOut";
        public const string API_CheckClockIN = "API_CheckClockIN";
        public const string API_SaveProfileWithSignature = "API_SaveProfileWithSignature";
        public const string API_GetEarlyClockoutDetail = "API_GetEarlyClockoutDetail";
        public const string API_CheckRequiredTask = "API_CheckRequiredTask";
        public const string GetTimeZone = "GetTimeZone";
        public const string API_AddNote = "API_AddNote";
        public const string API_GetByPassDetail = "API_GetByPassDetail";
        public const string API_GetEmpVisitHistory = "API_GetEmpVisitHistory";
        public const string API_GetUserNotifications = "API_GetUserNotifications";
        public const string API_GetIncompletedEmpVisitHistory = "API_GetIncompletedEmpVisitHistory";
        public const string GetOrganizationDetails = "GetOrganizationDetails";
        public const string GetOrganizationDetailsById = "[dbo].[GetOrganizationDetailsById]";
        public const string API_GetIncompletedPastVisit = "API_GetIncompletedPastVisit";
        public const string API_ReadUserNotification = "API_ReadUserNotification";
        public const string API_AcceptScheduleNotification = "API_AcceptScheduleNotification";
        public const string API_GetEmpDetailForIdCard = "API_GetEmpDetailForIdCard";
        public const string GetEmpDetailsForNotification = "GetEmpDetailsForNotification";
        public const string GetPatientResignatureFlag = "GetPatientResignatureFlag";
        public const string API_UpdatePatientLatLong = "API_UpdatePatientLatLong";
        public const string API_AddClockInOutLog = "API_AddClockInOutLog";
        public const string API_ActionOnNotification = "API_ActionOnNotification";

        public const string API_SaveReferralNote = "API_SaveReferralNote";
        public const string API_GetReferralNoteList = "API_GetReferralNoteList";
        public const string API_DeleteReferralNote = "API_DeleteReferralNote";
        public const string API_VerifyPatient = "API_VerifyPatient";
        public const string API_VerifyPatientByMobile = "API_VerifyPatientByMobile";
        public const string API_CheckForClockInClockOut = "API_CheckForClockInClockOut";
        public const string API_ValidateEmployeeMobile = "API_ValidateEmployeeMobile";
        public const string API_InsertIvrLogInDatabse = "API_InsertIvrLogInDatabse";

        public const string API_GetEmpPTOList = "API_GetEmpPTOList";
        public const string API_SaveEmployeePTO = "API_SaveEmployeePTO";
        public const string API_DeleteEmployeePTO = "API_DeleteEmployeePTO";

        public const string API_GetPatientList = "API_GetPatientList";
        public const string API_GetPatientDetail = "API_GetPatientDetail";
        public const string API_GetAppointedPatients = "API_GetAppointedPatients";
        public const string API_GetCareTypes = "API_GetCareTypes";
        public const string API_CreateCareTypeSchedule = "API_CreateCareTypeSchedule";

        public const string API_GetFacilityRAL = "API_GetFacilityRAL";
        public const string API_RAL_GetPatientList = "API_RAL_GetPatientList";
        public const string RAL_SetEmployeeVisitTime = "API_RAL_SetEmployeeVisitTime";


        public const string API_GetSectionList = "API_GetSectionList";
        public const string API_SaveSectionSubSection = "API_SaveSectionSubSection";
        public const string API_DeleteSectionSubSection = "API_DeleteSectionSubSection";
        public const string API_GetOrganizationFormListForMapping = "API_GetOrganizationFormListForMapping";
        public const string API_GetSubSectionList = "API_GetSubSectionList";
        public const string API_GetDocumentList = "API_GetDocumentList";
        public const string API_UpdateDocument = "API_UpdateDocument";
        public const string API_DeleteDocument = "API_DeleteDocument";
        public const string API_GetTaskFormList = "API_GetTaskFormList";
        public const string API_SaveForm = "API_SaveForm";
        public const string API_SaveTaskForm = "API_SaveTaskForm";
        public const string API_SaveFormPreference = "API_SaveFormPreference";
        public const string API_OpenSavedForm = "API_OpenSavedForm";
        public const string API_GetDocumentInfo = "API_GetDocumentInfo";
        public const string API_SaveFormName = "API_SaveFormName";

        public const string API_GetNoteSentenceList = "API_GetNoteSentenceList";
        public const string SaveNewSchedule = "SaveNewSchedule";

        public const string API_GetSurveyQuestionList = "API_GetSurveyQuestionList";
        public const string SaveSurveyAnswer = "SaveSurveyAnswer";
        public const string API_GetEmployeeSurveyList = "API_GetEmployeeSurveyList";

        public const string GetOrganizationData = "[API].[GetOrganizationData]";
        public const string GetPatientPersonalData = "[API].[GetPatientPersonalData]";
        public const string GetPatientContactData = "[API].[GetPatientContactData]";
        public const string GetPatientPhysicianData = "[API].[GetPatientPhysicianData]";
        public const string GetPatientDxCodeData = "[API].[GetPatientDxCodeData]";
        public const string GetPatientMedicationData = "[API].[GetPatientMedicationData]";
        public const string GetPatientNotesData = "[API].[GetPatientNotesData]";
        public const string GetPatientPreferencesData = "[API].[GetPatientPreferencesData]";
        public const string GetPatientTaskMappingsData = "[API].[GetPatientTaskMappingsData]";
        public const string GetPatientPayorData = "[API].[GetPatientPayorData]";
        public const string GetPatientAllergyData = "[API].[GetPatientAllergyData]";

        public const string GetEmployeesPersonalData = "[API].[GetEmployeesPersonalData]";

        
        public const string API_GetNotesCategory = "[API].[GetNotesCategory] ";
        public const string GetPatientAllergyDataAsCommaSeparted = "[API].[GetPatientAllergyDataAsCommaSeparted]";
        public const string GetPatientDxCodeDataAsCommaSeparted = "[API].[GetPatientDxCodeDataAsCommaSeparted]";
        public const string GetPatientBillingAuthorizationData = "[API].[GetPatientBillingAuthorizationData]";

        
        public const string API_Update_EmployeeIVR = "API_Update_EmployeeIVR";
        public const string API_UniversalPriorAuthorization = "API_UniversalPriorAuthorization";
        public const string API_AcceptAgreement = "API_AcceptAgreement";

        #region "Referral Groups"
        public const string API_SaveReferralGroup = "API_SaveReferralGroup";
        public const string API_DeleteReferralGroup = "API_DeleteReferralGroup";
        public const string API_SaveReferralGroupMapping = "API_SaveReferralGroupMapping";
        public const string API_DeleteReferralGroupMapping = "API_DeleteReferralGroupMapping";
        public const string API_GetReferralGroupList = "API_GetReferralGroupList";
        #endregion

    }
}