
namespace Zarephath.Core.Infrastructure
{
    public class StoredProcedure
    {
        #region Department

        /// <summary>
        /// For fill Dropdownlist on page load.
        /// </summary>
        public const string SetDepartmentListPage = "SetDepartmentListPage";

        /// <summary>
        /// For Get list of departments with searching, pagination and sorting functionality
        /// </summary>
        public const string GetDepartmentList = "GetDepartmentList";

        /// <summary>
        /// For delete single department or list of departments or get list of departments with passing parameter "IsShowList=true".
        /// "IsShowList" will return list of departments if its passed as "true".
        /// </summary>
        public const string DeleteDepartment = "DeleteDepartment";
        public const string DeleteNote = "DeleteNote";

        #endregion Department

        #region Employee

        /// <summary>
        /// For fill Dropdownlist on page load.
        /// </summary>
        public const string SetEmployeeListPage = "SetEmployeeListPage";

        /// <summary>
        /// For Get list of employees with searching, pagination and sorting  functionality
        /// </summary>
        public const string GetEmployeeList = "GetEmployeeList";


        ///To get limited records in employee records
        public const string GetEmployeeListForLimitedRecords = "GetEmployeeListForLimitedRecords";

        /// <summary>
        /// For delete single employee or list of employees or get list of employees with passing parameter "IsShowList=true".
        /// "IsShowList" will return list of employees if its passed as "true".
        /// </summary>
        public const string DeleteEmployee = "DeleteEmployee";
        public const string DeleteReferral = "DeleteReferral";

        public const string GetSearchSkill = "GetSearchSkill";
        public const string CheckForDuplicateAddPreference = "CheckForDuplicateAddPreference";
        public const string DeleteEmployeePreference = "DeleteEmployeePreference";
        public const string DeleteReferralPreference = "DeleteReferralPreference";

        public const string GetEmployeeDetails = "GetEmployeeDetails";

        public const string HC_GetEmployeeDetails = "HC_GetEmployeeDetails";
        public const string GetGeneralMaster = "GetGeneralMaster";
        public const string GetToken = "GetToken";

        public const string GetEmployeesForNurseSchedule = "GetEmployeesForNurseSchedule";
        public const string GetEmployeeLimitedRecordsForNurseSchedule = "GetEmployeeLimitedRecordsForNurseSchedule";
        public const string GetReferralsForNurseSchedule = "GetReferralsForNurseSchedule";
        public const string GetReferralLimitedRecordsForNurseSchedule = "GetReferralLimitedRecordsForNurseSchedule";
        public const string GetReferralsByCareTypeId = "GetReferralsByCareTypeId";
        public const string AddAppointmentToScheduleMaster = "AddAppointmentToScheduleMaster";
        public const string AddBulkNurseSchedule = "AddBulkNurseSchedule";
        public const string UpdateAppointmentToScheduleMaster = "UpdateAppointmentToScheduleMaster";
        public const string UpdateBulkNurseSchedule = "UpdateBulkNurseSchedule";
        public const string GetNurseScheduleMaster = "GetNurseScheduleMaster";
        public const string AddScheduleToReferralTimeSlotMaster = "AddScheduleToReferralTimeSlotMaster";
        public const string AddScheduleToReferralTimeSlotDetails = "AddScheduleToReferralTimeSlotDetails";
        public const string AddScheduleToReferralTimeSlotDates = "AddScheduleToReferralTimeSlotDates";
        public const string DeleteScheduleFromReferralTimeSlotTables = "DeleteScheduleFromReferralTimeSlotTables";
        public const string DeleteBulkNurseSchedule = "DeleteBulkNurseSchedule";
        public const string GetScheduleMasterById = "GetScheduleMasterById";
        public const string FindNurseScheduleConflict = "FindNurseScheduleConflict";

        #endregion Employee

        #region RolePermission

        public const string SetRolePermissionPage = "SetRolePermissionPage";
        public const string HC_SetRolePermissionPage = "HC_SetRolePermissionPage";
        public const string AddNewRole = "AddNewRole";
        public const string SaveRoleWisePermission = "SaveRoleWisePermission";
        public const string SavePermission = "SavePermission";
        public const string SaveMapReport = "SaveMapReport";
        public const string GetMapReport = "GetMapReport";

        #endregion RolePermission
        #region Case Manager

        public const string SetCaseManagerListPage = "SetCaseManagerListPage";
        public const string GetCaseManagerList = "GetCaseManagerList";
        public const string DeleteCaseManager = "DeleteCaseManager";
        #endregion


        #region Parent
        public const string SetAddParentPage = "SetAddParentPage";

        public const string SetParentListPage = "SetParentListPage";
        public const string GetParentList = "GetParentList";
        public const string DeleteParent = "DeleteParent";
        #endregion

        #region Referral



        public const string GetTemplateBody = "GetTemplateBody";
        public const string GetTokenList = "GetTokenList";
        public const string DeleteEmailTemplates = "DeleteTemplate";
        public const string UpdateEmailTemplate = "UpdateEmailTemplate";
        public const string SaveEmailTemplate = "SaveEmailTemplate";
        public const string GetReferralDocument = "GetReferralDocument";




        public const string SetAddReferralPage = "SetAddReferralPage";
        public const string HC_SetAddReferralPage = "HC_SetAddReferralPage";
        public const string SetReferralCheckList = "SetReferralCheckList";
        public const string SetReferralSparForm = "SetReferralSparForm";
        public const string SetReferralInternalMessage = "SetReferralInternalMessage";
        public const string SetReferralDocument = "SetReferralDocument";
        public const string GetReferralDocumentList = "GetReferralDocumentList";
        public const string SetReferralMissingDocument = "SetReferralMissingDocument";
        public const string SendReceiptNotificationEmail = "SendReceiptNotificationEmail";
        public const string NotificationToCMForInactiveStatus = "NotificationToCMForInactiveStatus";
        public const string NotificationToCMForReferralAcceptedStatus = "NotificationToCMForReferralAcceptedStatus";

        public const string SetReferralListPage = "SetReferralListPage";
        public const string HC_SetReferralListPage = "HC_SetReferralListPage";
        public const string GetReferralList = "GetReferralList";
        public const string GetReferralListForLimitedAccess = "GetReferralListForLimitedAccess";
        public const string GetReferralAuthorizationsDetails = "GetReferralAuthorizationsDetails";
        public const string GetReferralDetails = "GetReferralDetails";
        public const string GetScheduleEmail = "GetScheduleEmail";
        public const string GetScheduleEmailForReminder = "GetScheduleEmailForReminder";
        public const string DeleteDxCodeMapping = "DeleteDxCodeMapping";
        public const string HC_DeleteDxCodeMapping = "HC_DeleteDxCodeMapping";
        public const string GetDxCodeListForAutoCompleter = "GetDxCodeListForAutoCompleter";
        public const string GetCaseManagerListForAutoCompleter = "GetCaseManagerListForAutoCompleter";
        public const string Rpt_GetReferralList = "Rpt_GetReferralList";
        public const string GetSiblingPageReferrals = "GetSiblingPageReferrals";
        public const string DeleteReferralSibling = "DeleteReferralSibling";
        public const string GetAuditLogList = "GetAuditLogList";
        public const string UpdateAhcccsId = "UpdateAhcccsId";

        public const string MarkPayorAsActive = "MarkPayorAsActive";
        public const string GetReferralPayorDetails = "GetReferralPayorDetails";
        public const string GetReferralPayorByID = "GetReferralPayorByID";

        public const string GetPayorDetailsByReferralID = "GetPayorDetailsByReferralID";
        public const string GetAuthorizationDetailsByReferralId = "GetAuthorizationDetailsByReferralId";

        public const string GetTableDisplayValue = "GetTableDisplayValue";
        public const string GetBXContractList = "GetBXContractList";
        public const string GetSuspensionDetail = "GetSuspensionDetail";
        public const string DeleteReferralSuspension = "DeleteReferralSuspension";

        public const string SetReferralTrackingListPage = "SetReferralTrackingListPage";
        public const string GetReferralTrackingList = "GetReferralTrackingList";
        public const string SaveReferralDocument = "SaveReferralDocument";
        public const string SaveCommonNote = "SaveCommonNote";
        public const string BulkGroupUpdate = "BulkGroupUpdate";

        public const string SaveInvoiceAuthorizeNetDetail = "UpdateInvoicefromAuthorizeNetDetails";
        public const string SaveBillingAuthorizeNetDetail = "SaveBillingInformation";
        public const string GetBillingAuthorizeNetDetail = "GetBillingInformation";


        public const string GetCommonNotes = "GetCommonNotes";
        public const string GetReferralMappedDXcodeList = "GetReferralMappedDXcodeList";
        public const string GetReferralPayorDetail = "GetReferralPayorDetail";
        public const string ReferralDXCodeMappingChangeSortingOrder = "ReferralDXCodeMappingChangeSortingOrder";
        public const string GetReferralDocumentDetails = "GetReferralDocumentDetails";
        public const string GetEmailTemplates = "GetEmailTemplates";
        public const string GetOrganizationSetting = "GetOrganizationDetails";
        public const string GetEmployeeSignature = "GetEmployeeSignature";
        public const string GetEmailTemplateDetails = "GetEmailTemplateDetails";

        public const string GetCommonList = "GetCommonList";
        public const string EZC_GetEmpListByRoleId = "EZC_GetEmpListByRoleId";
        public const string GetNoteCategory = "GetNoteCategory";

        public const string GetEmailSignature = "GetEmailSignature";
        public const string GetPatientAttacmentTags = "GetPatientAttacmentTags";

        public const string DeleteCommonNote = "DeleteCommonNote";
        public const string SaveEmployeeEmailSignature = "SaveEmployeeEmailSignature";

        public const string FillGroupMonthlySummaryModel = "FillGroupMonthlySummaryModel";
        public const string SearchClientForGroupMonthlySummary = "SearchClientForGroupMonthlySummary";
        public const string SaveDxCodeAPI = "SaveDxCodeAPI";
        public const string GetDxCode = "GetDxCode";
        public const string SaveSpecialist = "SaveSpecialist";
        public const string GetSpecialist = "GetSpecialist";
        public const string GetReferralMedications = "GetReferralMedications";
        public const string SaveReferralallergyDetails = "SaveAllergy";
        public const string GetReferralallergyDetails = "GetAllergy";
        public const string GetAllergyDDL = "GetAllergyDDL";
        public const string GetAllergyTitle = "GetAllergyTitle";
        public const string GetFaxSetting = "GetFaxSetting";


        public const string Getallery = "SelectAllergy";
        public const string SearchMedication = " SearchMedication";
        public const string SaveMedication = "SaveMedication";
        public const string SaveReferralMedication = "SaveReferralMedication";
        public const string DeleteReferralMedication = "DeleteReferralMedication";
        public const string GetReferralMedicationsById = "GetReferralMedicationsById";

        public const string GetReferralHistory = "[dbo].[GetReferralHistory]";

        public const string GetAuthorizationLinkupList = "GetAuthorizationLinkupList";
        public const string GetAuthorizationScheduleLinkList = "GetAuthorizationScheduleLinkList";
        public const string UpdateAuthorizationLinkup = "UpdateAuthorizationLinkup";
        public const string GetAssigneeList = "GetAssigneeList";
        #endregion


        #region Schedule

        public const string SetScheduleAssignmentModel = "SetScheduleAssignmentModel";
        public const string HC_SetScheduleAssignmentModel = "HC_SetScheduleAssignmentModel";


        public const string GetReferralListForScheduling = "GetReferralListForScheduling";
        public const string HC_GetReferralListForScheduling = "HC_GetReferralListForScheduling";
        public const string GetReferralDetailForPopup = "GetReferralDetailForPopup";
        public const string HC_GetReferralDetailForPopup = "HC_GetReferralDetailForPopup";
        public const string HC_GetEmployeesForScheduling = "HC_GetEmployeesForScheduling";
        public const string HC_GetEmployeesForEmpCalender = "HC_GetEmployeesForEmpCalender";


        public const string HC_SetPendingSchedulesPage = "HC_SetPendingSchedulesPage";
        public const string HC_GetPendingScheduleList = "HC_GetPendingScheduleList";
        public const string HC_DeletePendingScheduleList = "HC_DeletePendingScheduleList";
        public const string HC_ProcessPendingSchedule = "HC_ProcessPendingSchedule";

        public const string SetScheduleAggregatorLogsListPage = "SetScheduleAggregatorLogsListPage";
        public const string GetScheduleAggregatorLogs = "GetScheduleAggregatorLogs";
        public const string ResendAggregatorData = "ResendAggregatorData";
        public const string GetScheduleAggregatorLogsDetails = "GetScheduleAggregatorLogsDetails";

        public const string SetScheduleMasterPage = "SetScheduleMasterPage";
        public const string HC_SetScheduleMasterPage = "HC_SetScheduleMasterPage";
        public const string HC_DayCare_SetScheduleMasterPage = "HC_DayCare_SetScheduleMasterPage";
        public const string GetScheduleMaster = "GetScheduleMaster";
        public const string HC_GetScheduleMaster = "HC_GetScheduleMaster";
        public const string HC_DayCare_GetScheduleMaster = "HC_DayCare_GetScheduleMaster";
        public const string GetCareTypeDropDownByPayorId = "GetCareTypeDropDownByPayorId";

        public const string InactiveSchedule = "InactiveSchedule";
        public const string UpdateReferralTimeSlotAssignee = "UpdateReferralTimeSlotAssignee";
        public const string DeleteSchedule = "DeleteSchedule";
        public const string HC_DayCare_DeleteSchedule = "HC_DayCare_DeleteSchedule";
        public const string GetFacilityScheduleDetails = "GetFacilityScheduleDetails";
        public const string HC_GetEmployeeScheduleDetails = "HC_GetEmployeeScheduleDetails";

        public const string HC_GetEmployeeMatchingPreferences = "HC_GetEmployeeMatchingPreferences";
        public const string HC_PrivateDuty_GetEmployeeMatchingPreferences = "HC_PrivateDuty_GetEmployeeMatchingPreferences";



        public const string SetAttendanceMasterPage = "SetAttendanceMasterPage";
        public const string GetAttendanceMasterByFacility = "GetAttendanceMasterByFacility";
        public const string UpdateReferralLastAttDate = "UpdateReferralLastAttDate";
        public const string CheckScheduleConflict = "CheckScheduleConflict";
        public const string HC_CheckScheduleConflict = "HC_CheckScheduleConflict";
        public const string GetScheduleCount = "GetScheduleCount";
        public const string GetClientCountAtTransportationLocations = "GetClientCountAtTransportationLocations";
        public const string GetScheduleNotificationLogs = "GetScheduleNotificationLogs";

        public const string GetScheduleEmailDetail = "GetScheduleEmailDetail";
        public const string HC_GetScheduleEmailDetail = "HC_GetScheduleEmailDetail";
        public const string HC_ChecklistGetScheduleEmailDetail = "HC_ChecklistGetScheduleEmailDetail";

        public const string GetScheduleBatchServiceForProcess = "GetScheduleBatchServiceForProcess";

        public const string SaveNewSchedule = "SaveNewSchedule";
        public const string ChangeSaveNewSchedule = "ChangeSaveNewSchedule";
        #endregion

        #region TransportLocation
        public const string GetTransportaLocationList = "GetTransportaLocationList";
        public const string DeleteTransportLocation = "DeleteTransportLocation";
        public const string GetSetAddTransportationLopcationPage = "GetSetAddTransportationLopcationPage";


        #endregion

        #region Facility House
        public const string GetFacilityHouseList = "GetFacilityHouseList";
        public const string SetAddFacilityHousePage = "SetAddFacilityHousePage";
        public const string SetFacilityHouseListPage = "SetFacilityHouseListPage";
        public const string DeleteFacilityHouse = "DeleteFacilityHouse";
        public const string GetParentFacilityHouse = "GetParentFacilityHouse";
        public const string GetAlreadyExistFacility = "GetAlreadyExistFacility";


        public const string HC_SetAddFacilityHousePage = "HC_SetAddFacilityHousePage";
        public const string HC_GetAlreadyExistFacility = "HC_GetAlreadyExistFacility";
        public const string GetEquipment = "GetEquipment";
        public const string SaveFacilityEquipment = "SaveFacilityEquipment";

        public const string HC_SetFacilityHouseListPage = "HC_SetFacilityHouseListPage";
        public const string HC_GetFacilityHouseList = "HC_GetFacilityHouseList";

        public const string HC_DeleteFacilityHouse = "HC_DeleteFacilityHouse";
        public const string HC_GetParentFacilityHouse = "HC_GetParentFacilityHouse";
        #endregion Facility House

        #region TransportaionGroup

        public const string SetTransportaionAssignmnetModel = "SetTransportaionAssignmnetModel";
        public const string GetReferralListForTransportationAssignment = "GetReferralListForTransportationAssignment";
        public const string GetAssignedClientListForTransportationAssignment = "GetAssignedClientListForTransportationAssignment";
        public const string ValidateTransportationGroup = "ValidateTransportationGroup";
        public const string SetTransportationGroupClientFilter = "SetTransportationGroupClientFilter";
        public const string DeleteTransportationGroupClientFilter = "DeleteTransportationGroupClientFilter";
        public const string DeleteTransportationGroupClientForDownDirection = "DeleteTransportationGroupClientForDownDirection";
        public const string GenerateDownTransportationGroupAndClient = "GenerateDownTransportationGroupAndClient";
        public const string GetTransportationGroupClientByScheduleID = "GetTransportationGroupClientByScheduleID";

        #endregion

        #region Payor

        public const string GetSetAddPayorPage = "GetSetAddPayorPage";
        public const string HC_GetSetAddPayorPage = "HC_GetSetAddPayorPage";

        public const string GetPayorServiceCodeMappingList = "GetPayorServiceCodeMappingList";
        public const string HC_GetPayorServiceCodeMappingList = "HC_GetPayorServiceCodeMappingList";

        public const string GetPayorList = "GetPayorList";
        public const string HC_GetPayorList = "HC_GetPayorList";

        public const string DeletePayor = " DeletePayor";
        public const string HC_DeletePayor = " HC_DeletePayor";

        public const string SetPayorListPage = " SetPayorListPage";
        public const string HC_SetPayorListPage = " HC_SetPayorListPage";

        public const string HC_GetPayorBillingSetting = " HC_GetPayorBillingSetting";
        public const string HC_SavePayorBillingSetting = " HC_SavePayorBillingSetting";

        public const string HC_GetAllEDI837Settings = "HC_GetAllEDI837Settings";
        public const string HC_SaveEDI837Setting = "HC_SaveEDI837Setting";


        public const string GetServiceCodeListForAutoCompleter = " GetServiceCodeListForAutoCompleter";
        public const string HC_GetServiceCodeListForAutoCompleter = "HC_GetServiceCodeListForAutoCompleter";

        public const string GetPayorServiceCodeListForAutoCompleter = "GetPayorServiceCodeListForAutoCompleter";
        public const string HC_GetPayorServiceCodeListForAutoCompleter = "HC_GetPayorServiceCodeListForAutoCompleter";


        public const string HC_PayorEnrollment_Update = "HC_PayorEnrollment_Update";

        public const string HC_GetPayorServicecodeList = "HC_GetPayorServicecodeList";


        public const string HC_GetVisitServiceCodeListForAutoCompleter = "HC_GetVisitServiceCodeListForAutoCompleter";
        public const string HC_GetCareTypeListFromVisitType = "HC_GetCareTypeListFromVisitType";

        public const string DeletePayorMappingCode = "DeletePayorMappingCode";
        public const string HC_DeletePayorMappingCode = "HC_DeletePayorMappingCode";
        public const string CheckForDuplicatePayorServiceCodeMapping = "CheckForDuplicatePayorServiceCodeMapping";
        public const string HC_CheckForDuplicatePayorServiceCodeMapping = "HC_CheckForDuplicatePayorServiceCodeMapping";

        public const string HC_CheckModifierAvailableOrNot = "HC_CheckModifierAvailableOrNot";


        public const string HC_SaveReferralPayorMapping = "HC_SaveReferralPayorMapping";
        public const string HC_CheckForDuplicateReferralPayorMapping = "HC_CheckForDuplicateReferralPayorMapping";
        public const string HC_GetReferralPayorMappingList = "HC_GetReferralPayorMappingList";
        public const string HC_DeleteReferralPayorMapping = "HC_DeleteReferralPayorMapping";

        public const string HC_GetPhysicianListForAutoComplete = "HC_GetPhysicianListForAutoComplete";


        public const string HC_AddReferralBillingSetting = "  HC_AddReferralBillingSetting";
        public const string HC_GetReferralBillingSetting = "  HC_GetReferralBillingSetting";

        public const string HC_AddReferralBillingAuthrization = "HC_AddReferralBillingAuthrization";
        public const string HC_SavePriorAuthorization = "  HC_SavePriorAuthorization";
        public const string HC_SavePriorAuthorizationServiceCodeDetails = "HC_SavePriorAuthorizationServiceCodeDetails";

        public const string HC_GetReferralBillingAuthorizationList = "HC_GetReferralBillingAuthorizationList";
        public const string HC_GetPriorAuthorizationList = "HC_GetPriorAuthorizationList";

        public const string HC_GetAuthorizationLoadModel = "HC_GetAuthorizationLoadModel";
        public const string GetPayorIdentificationNumber = "GetPayorIdentificationNumber";
        public const string HC_GetPayorMappedServiceCode = "HC_GetPayorMappedServiceCode";
        public const string HC_GetAuthServiceCodes = "HC_GetAuthServiceCodes";
        public const string HC_GetPAServiceCodeList = "HC_GetPAServiceCodeList";




        public const string HC_DeleteReferralBillingAuthorization = "HC_DeleteReferralBillingAuthorization";
        public const string HC_DeletePriorAuthorization = "HC_DeletePriorAuthorization";
        public const string HC_DeletePAServiceCode = "HC_DeletePAServiceCode";


        public const string HC_SaveServiceCode = "HC_SaveServiceCode";

        public const string HC_SaveCareFormDetails = "HC_SaveCareFormDetails";
        public const string HC_GetCareFormDetails = "HC_GetCareFormDetails";

        public const string HC_SaveCareFormDocument = "HC_SaveCareFormDocument";

        public const string HC_GetEmpDataForGenerateCertificate = "HC_GetEmpDataForGenerateCertificate";


        #endregion


        #region Service Code

        public const string SetAddServiceCodePage = "SetAddServiceCodePage";
        public const string HC_SetAddServiceCodePage = "HC_SetAddServiceCodePage";
        public const string SetServiceCodeListPage = "SetServiceCodeListPage";
        public const string HC_SetServiceCodeListPage = "HC_SetServiceCodeListPage";
        public const string GetServiceCodeList = "GetServiceCodeList";
        public const string HC_GetServiceCodeList = "HC_GetServiceCodeList";

        public const string HC_GetModifierList = "HC_GetModifierList";
        public const string HC_SaveModifier = "HC_SaveModifier";
        public const string HC_DeleteModifier = "HC_DeleteModifier";
        public const string DeleteServiceCode = "DeleteServiceCode";
        public const string HC_GetModifierByServiceCode = "HC_GetModifierByServiceCode";
        #endregion




        #region Notes

        public const string SetNoteListPage = "SetNoteListPage";
        public const string SetAddNotePage = "SetAddNotePage";
        public const string GetDTRDetailsList = "GetDTRDetailsList";
        public const string CheckDTRDetails = "CheckDTRDetails";
        public const string GetNoteList = "GetNoteList";
        public const string ExportNoteList = "ExportNoteList";
        public const string GetNoteClientList = "GetNoteClientList";
        public const string UpdateFirstDos = "UpdateFirstDos";
        public const string GetServiceCodes = "GetNotePageServiceCode";
        public const string GetGroupNotePageServiceCode = "GetGroupNotePageServiceCode";
        public const string GetServiceCodesForChangeServiceCode = "GetServiceCodesForChangeServiceCode";
        public const string ValidateChangeServiceCode = "ValidateChangeServiceCode";

        public const string GetPOS = "GetNotePagePlaceOfService";
        public const string GetAutoCreateServiceInformation = "GetAutoCreateServiceInformation";
        public const string SaveGeneralNote = "SaveNote";
        public const string GetNotePageReferrals = "GetNotePageReferrals";
        public const string ReSyncNoteDxCodeMappings = "ReSyncNoteDxCodeMappings";
        public const string GetClientsForGroupNote = "GetClientsForGroupNote";
        public const string GetNotesForChangeServiceCode = "GetNotesForChangeServiceCode";
        public const string GetPayorWiseFacilities = "GetPayorWiseFacilities";
        public const string SetAddGroupNotePage = "SetAddGroupNotePage";
        public const string SetChangeServiceCodePage = "SetChangeServiceCodePage";
        public const string ValidateServiceCodeForNotes = "ValidateServiceCodeForNotes";


        #endregion

        #region Home

        public const string GetDashboardInternalMessgaeList = "GetDashboardInternalMessgaeList";
        public const string GetDashboardPageList = "GetDashboardPageList";
        public const string GetDashboardInCompeleteSparFormandCheckList = "GetDashboardInCompeleteSparFormandCheckList";
        public const string GetDashboardMissingandExpireDocumentList = "GetDashboardMissingandExpireDocumentList";
        public const string GetDashboardMissingDocument = "GetDashboardMissingDocument";
        public const string GetDashboardInternalMissingandExpireDocumentList = "GetDashboardInternalMissingandExpireDocumentList";
        public const string GetDashboardInternalMissingDocument = "GetDashboardInternalMissingDocument";
        public const string GetDashboardResolvedInternalMessgaeList = "GetDashboardResolvedInternalMessgaeList";
        public const string GetDashboardAnsellCaseyReviewList = "GetDashboardAnsellCaseyReviewList";
        public const string GetDashboardAssignedNoteReviewList = "GetDashboardAssignedNoteReviewList";

        public const string GetLayoutRelatedDetails = "GetLayoutRelatedDetails";

        #endregion

        #region Dx Code

        public const string GetDxCodeList = "GetDxCodeList";
        public const string DeleteDxCode = "DeleteDxCode";
        public const string CheckForDuplicateDxCodeMapping = "CheckForDuplicateDxCodeMapping";

        public const string HC_GetDxCodeList = "HC_GetDxCodeList";
        public const string HC_SaveDxCode = "HC_SaveDxCode";
        public const string HC_DeleteDxCode = "HC_DeleteDxCode";
        public const string HC_CheckForDuplicateDxCodeMapping = "HC_CheckForDuplicateDxCodeMapping";
        public const string IsDXCodeExistByReferralID = "IsDXCodeExistReferralID";

        #endregion Dx Code

        #region VisitQuestion
        public const string AddVisitTaskPageModel = "AddVisitTaskPageModel";
        public const string SetVisitTaskListPage = "SetVisitTaskListPage";
        public const string SaveVisitTask = "SaveVisitTask";
        public const string GetVisitTaskList = "GetVisitTaskList";
        public const string GetVisitTaskListForReferral = "GetVisitTaskListForReferral";
        public const string DeleteVisitTask = "DeleteVisitTask";
        public const string CloneTask = "CloneTask";
        public const string MapReferralVisitTask = "MappReferralVisitTask"; //"AddReferralVisitTask";
        public const string AddReferralVisitTask = "AddReferralVisitTask"; //"AddReferralVisitTask";
        public const string AddReferralGoal = "AddOrUpdateReferralTaskMappingsGoal";
        public const string GetReferralGoal = "GetReferralTaskMappingsGoal";
        public const string UpdateGoalIsActiveIsDeletedFlag = "UpdateGoalIsActiveIsDeletedFlag";
        public const string GetPatientTaskMappings = "GetPatientTaskMappings";
        public const string GetReferralTaskMapping = "GetReferralTaskMapping";
        public const string GetReferralTaskMappingReport = "ReferralTaskMappingReport";
        public const string DeleteRefTaskMapping = "DeleteRefTaskMapping";
        public const string OnTaskChecked = "OnTaskChecked";
        public const string BulkUpdateVisitTaskDetail = "API_BulkUpdateVisitTasksDetail";
        public const string BulkUpdateVisitReport = "BulkUpdateVisitReport";





        public const string CheckForDuplicateVisitQuestionMapping = "CheckForDuplicateVisitQuestionMapping";
        #endregion VisitQuestion


        #region Note Sentence
        public const string GetNoteSentenceList = "GetNoteSentenceList";
        public const string DeleteNoteSentence = "DeleteNoteSentence";
        public const string CheckForDuplicateNoteSentence = "CheckForDuplicateNoteSentence";
        #endregion Note Sentence


        #region TransportLocation

        public const string GetSetAddEmailTemplate = "GetSetAddEmailTemplate";
        public const string GetEmailTemplateTypesList = "GetEmailTemplateTypesList";
        public const string GetEmailTemplateList = "GetEmailTemplateList";
        public const string DeleteEmailTemplate = "DeleteEmailTemplate";

        #endregion

        #region Batch

        public const string GetSetAddBatchPage = "GetSetAddBatchPage";
        public const string HC_GetSetAddBatchPage = "HC_GetSetAddBatchPage";
        public const string HC_GetSetAddERAPage = "HC_GetSetAddERAPage";


        public const string GetBatchList = "GetBatchList";
        public const string HC_GetBatchList = "HC_GetBatchList";

        public const string SaveNewBatch = "SaveNewBatch";
        public const string HC_SaveNewBatch = "HC_SaveNewBatch";
        public const string HC_SaveNewBatch_Resend_And_DataValidation = "HC_SaveNewBatch_Resend_And_DataValidation";

        public const string HC_CM_SaveNewBatch = "HC_CM_SaveNewBatch";

        public const string DeleteBatch = "DeleteBatch";
        public const string HC_DeleteBatch = "HC_DeleteBatch";

        public const string GetUnsentBatches = "GetUnsentBatches";
        public const string HC_GetUnsentBatches = "HC_GetUnsentBatches";

        public const string SetMarkasSentBatch = "SetMarkasSentBatch";
        public const string HC_SetMarkasSentBatch = "HC_SetMarkasSentBatch";

        public const string SaveNewBatch01 = "SaveNewBatch01";
        public const string GetBatchList01 = "GetBatchList01";

        public const string GenratePaperRemitsEOBTemplate = "GenratePaperRemitsEOBTemplate";
        public const string HC_GenratePaperRemitsEOBTemplate = "HC_GenratePaperRemitsEOBTemplate";

        //For Both Single Batch or Multiple
        public const string GetOverviewFileList = "GetOverviewFileList";
        public const string HC_GetOverviewFileList = "HC_GetOverviewFileList";

        //For Single Batch
        public const string GetOverviewFileList01 = "GetOverviewFileList01";
        public const string HC_GetOverviewFileList01 = "HC_GetOverviewFileList01";


        public const string GetApprovedPayorsList = "GetApprovedPayorsList";
        public const string GetAlreadyExistReferralRespiteUsageLimit = "GetAlreadyExistReferralRespiteUsageLimit";


        public const string HC_CM_GetPatientList = "HC_CM_GetPatientList";
        public const string HC_CM_GetPatientList_Temporary = "HC_CM_GetPatientList_Temporary";

        public const string HC_GetPatientList = "HC_GetPatientList";
        public const string HC_GetPatientList_Temporary = "HC_GetPatientList_Temporary";

        public const string HC_RefreshAndGroupingNotes = "HC_RefreshAndGroupingNotes";
        public const string HC_GetChildNoteList = "HC_GetChildNoteList";
        public const string HC_GetChildNoteList_Temporary = "HC_GetChildNoteList_Temporary";
        public const string HC_SaveMannualPaymentPostingDetails = "HC_SaveMannualPaymentPostingDetails";

        
        //public const string HC_CaseManagement_GetChildNoteList = "HC_CaseManagement_GetChildNoteList";

        public const string HC_ViewBatchDetails = "HC_ViewBatchDetails";

        public const string HC_SaveBatchUploadedClaim = "HC_SaveBatchUploadedClaim";
        public const string HC_SaveBatchUploadedClaimErrors = "HC_SaveBatchUploadedClaimErrors";
        public const string HC_SaveBatchUploadedClaimFile = "HC_SaveBatchUploadedClaimFile";
        public const string HC_GetBatchUploadedClaims = "HC_GetBatchUploadedClaims";
        public const string HC_GetBatchUploadedClaimErrors = "HC_GetBatchUploadedClaimErrors";
        public const string HC_ValdiateAndUpdateERA835ProcessStatus = "HC_ValdiateAndUpdateERA835ProcessStatus";

        public const string HC_GetPayorFromERA835Response = "HC_GetPayorFromERA835Response";




        public const string HC_SaveCMS1500Data = "HC_SaveCMS1500Data";
        public const string HC_GetBatchByBatchID = "HC_GetBatchByBatchID";

        public const string HC_AddManageClaim = "HC_AddManageClaim";

        #endregion

        #region Batch 837/835

        public const string GetBatchRelatedAllData = "GetBatchRelatedAllData";
        public const string GetBatchRelatedAllData01 = "GetBatchRelatedAllData01";

        public const string HC_GetBatchRelatedAllData = "HC_GetBatchRelatedAllData";

        public const string GetSetUpload835 = "GetSetUpload835";
        public const string GetUpload835FileList = "GetUpload835FileList";
        public const string DeleteUpload835File = "DeleteUpload835File";
        public const string GetUpload835FilePathList = "GetUpload835FilePathList";
        public const string ProcessUpload835File = "ProcessUpload835File";
        public const string GetDeleteUpload835FilePathList = "GetDeleteUpload835FilePathList";
        public const string UpdateBatchAfter835FileProcessed = "UpdateBatchAfter835FileProcessed";
        public const string GetBatchNoteDetailsBasedOnServiceDetails = "GetBatchNoteDetailsBasedOnServiceDetails";


        public const string GetUpload835FilesForProcess = "GetUpload835FilesForProcess";

        #endregion

        #region Reconcile 835 / EOB
        public const string GetSetReconcile835 = "GetSetReconcile835";
        public const string GetUpload835FilesForAutoComplete = "GetUpload835FilesForAutoComplete";
        public const string GetReconcile835List = "GetReconcile835List";
        public const string ExportReconcile835List = "ExportReconcile835List";
        public const string GetReconcileBatchNoteDetails = "GetReconcileBatchNoteDetails";
        public const string MarkNoteAsLatest = "MarkNoteAsLatest";
        public const string HC_UpdateBatchWithERAReference = "HC_UpdateBatchWithERAReference";


        public const string SetClaimAdjustmentFlag = "SetClaimAdjustmentFlag";
        public const string SetClaimAdjustmentFlag01 = "SetClaimAdjustmentFlag01";

        public const string BulkSetClaimAdjustmentFlag = "BulkSetClaimAdjustmentFlag";



        #region New Changes
        public const string GetParentReconcileList = "GetParentReconcileList";
        public const string GetChildReconcileList = "GetChildReconcileList";
        public const string MarkNoteAsLatest01 = "MarkNoteAsLatest01";
        public const string ExportReconcileList = "ExportReconcileList";
        #endregion

        #endregion

        #region 270 / 271

        public const string GetAddProcess270271Model = "GetAddProcess270271Model";
        public const string GetClientDetailsFor270Process = "GetClientDetailsFor270Process";

        public const string GetEdi270271FileList = "GetEdi270271FileList";
        public const string DeleteEdi270271Files = "DeleteEdi270271Files";


        public const string HC_GetAddProcess270271Model = "HC_GetAddProcess270271Model";
        public const string HC_GetClientDetailsFor270Process = "HC_GetClientDetailsFor270Process";
        public const string HC_GetEdi270271FileList = "HC_GetEdi270271FileList";
        public const string HC_DeleteEdi270271Files = "HC_DeleteEdi270271Files";

        
        public const string HC_GetResponseIDForBatchUploadedClaimMessages = "HC_GetResponseIDForBatchUploadedClaimMessages";
        public const string HC_SaveBatchUploadedClaimMessageTable = "HC_SaveBatchUploadedClaimMessageTable";
        public const string HC_SaveBatchUploadedClaimMessage = "HC_SaveBatchUploadedClaimMessage";
        public const string HC_GetBatchUploadedClaimMessages = "HC_GetBatchUploadedClaimMessages";
        #endregion


        #region 277

        public const string UpdateBNFromEdi277FileDetails = "UpdateBNFromEdi277FileDetails";
        public const string GetEdi277FileForProcess = "GetEdi277FileForProcess";
        public const string GetAddProcess277Model = "GetAddProcess277Model";
        public const string GetEdi277FileList = "GetEdi277FileList";
        public const string DeleteEdi277Files = "DeleteEdi277Files";
        public const string Download277RedableFile = "Download277RedableFile";

        #endregion


        #region ReferralRespiteUsageLimit

        public const string InsertUpdateReferralRespiteUsageLimit = "InsertUpdateReferralRespiteUsageLimit";
        public const string SaveRespiteHours = "SaveRespiteHours";
        public const string UpdateRespiteHours = "UpdateRespiteHours";


        #endregion

        #region EDI FileLog

        public const string GetEdiFileLogList = "GetEdiFileLogList";
        public const string SetEdiFilesLogListPage = "SetEdiFilesLogListPage";
        public const string DeleteEdiFileLog = "DeleteEdiFileLog";
        public const string GetEdiFileLogsFilePathList = "GetEdiFileLogsFilePathList";

        public const string HC_GetEdiFileLogList = "HC_GetEdiFileLogList";
        public const string HC_SetEdiFilesLogListPage = "HC_SetEdiFilesLogListPage";
        public const string HC_DeleteEdiFileLog = "HC_DeleteEdiFileLog";
        public const string HC_GetEdiFileLogsFilePathList = "HC_GetEdiFileLogsFilePathList";
        #endregion

        #region Report

        public const string Rpt_GetClientSatus = "Rpt_GetClientSatus";
        public const string Rpt_GetRequestClientList = "Rpt_GetRequestClientList";
        public const string Rpt_GetReferralDetail = "Rpt_GetReferralDetail";
        public const string Rpt_GetClientInformation = "Rpt_GetClientInformation";
        public const string Rpt_GetServicePlanExpiration = "Rpt_GetServicePlanExpiration";
        public const string Rpt_GetAttendance = "Rpt_GetAttendance";
        public const string SetReportPage = "SetReportPage";
        public const string Rpt_GetRespiteUsage = "Rpt_GetRespiteUsage";
        public const string Rpt_GetNotePageReferrals = "Rpt_GetNotePageReferrals";
        public const string Rpt_EncounterPrint = "Rpt_EncounterPrint";
        public const string Rpt_DTRPrint = "Rpt_DTRPrint";
        public const string Rpt_GeneralNotice = "Rpt_GeneralNotice";
        public const string Rpt_GetDspRoster = "Rpt_GetDspRoster";
        public const string Rpt_GetLSTeamMemberCaseload = "Rpt_GetLSTeamMemberCaseload";
        public const string Rpt_GetScheuldeAttendanceList = "Rpt_GetScheuldeAttendanceList";
        public const string Rpt_GetRequestedDocsForAttendanceList = "Rpt_GetRequestedDocsForAttendanceList";
        public const string Rpt_GetLifeSkillsOutcomeMeasurementsList = "Rpt_GetLifeSkillsOutcomeMeasurementsList";
        public const string Rpt_BehaviorContractTracking = "Rpt_BehaviorContractTracking";


        public const string SetLSTeamMemberCaseloadListPage = "SetLSTeamMemberCaseloadListPage";
        public const string GetLSTeamMemberCaseloadList = "GetLSTeamMemberCaseloadList";

        public const string GetBillingSummary = "GetBillingSummary";
        public const string SetGroupTimesheetPage = "SetGroupTimesheetPage";
        public const string SetNurseSignaturePage = "SetNurseSignaturePage";
        public const string SetReferralActivityPage = "SetReferralActivityPage";
        public const string GetReferralsByMonthYear = "GetReferralsByMonthYear";
        public const string GetReferralActivityById = "GetReferralActivityById";
        public const string GetReferralNotesById = "GetReferralNotesById";
        public const string SaveReferralActivityList = "SaveReferralActivityList";
        public const string SaveReferralActivityNotes = "SaveReferralActivityNotes";
        public const string EditDeleteReferralActivityNotes = "EditDeleteReferralActivityNotes";
        #endregion

        #region Agency

        public const string SetAddAgencyPage = "SetAddAgencyPage";
        public const string SetAgencyListPage = "SetAgencyListPage";
        public const string GetAgencyList = "GetAgencyList";
        public const string DeleteAgency = "DeleteAgency";

        #region Home Care

        public const string HC_SetAddAgencyPage = "HC_SetAddAgencyPage";
        public const string HC_SetAgencyListPage = "HC_SetAgencyListPage";
        public const string HC_GetAgencyList = "HC_GetAgencyList";
        public const string HC_DeleteAgency = "HC_DeleteAgency";

        #endregion

        #endregion

        #region Notification Configuration

        public const string GetNotificationConfigurationDetails = "[notif].[GetNotificationConfigurationDetails]";
        public const string SaveNotificationConfigurationDetails = "[notif].[SaveNotificationConfigurationDetails]";

        #endregion

        #region CronJOb

        public const string GetConfiguredNotifications = "[notif].[GetConfiguredNotifications]";
        public const string UpdateProcessedNotifications = "[notif].[UpdateProcessedNotifications]";
        public const string DMTGetReferralData = "DMTGetReferralData";
        public const string GetEDIFileDeleteList = "GetEDIFileDeleteList";
        public const string DeleteEDIFileDeleteList = "DeleteEDIFileDeleteList";
        public const string GetReferralsForMissingDocumentMail = "GetReferralsForMissingDocumentMail";
        public const string GetAttendanceNotificationList = "GetAttendanceNotificationList";
        public const string GetServicePlanList = "GetServicePlanList";

        #endregion

        #region Schedule Batch Service

        public const string GetScheduleBatchServiceList = "GetScheduleBatchServiceList";
        public const string GetScheduleBatchServices = "GetScheduleBatchServices";
        public const string DeleteScheduleBatchService = "DeleteScheduleBatchService";

        #endregion


        public const string ReferralReviewAssessment = "ReferralReviewAssessment";
        public const string GetReferralAssessmentReview = "GetReferralAssessmentReview";
        public const string DeleteReferralAssessmentReview = "DeleteReferralAssessmentReview";

        public const string ReferralOutcomeMeasurement = "ReferralOutcomeMeasurement";
        public const string GetReferralOutcomeMeasurement = "GetReferralOutcomeMeasurement";
        public const string DeleteReferralOutcomeMeasurement = "DeleteReferralOutcomeMeasurement";

        public const string ReferralMonthlySummary = "ReferralMonthlySummary";
        public const string GetReferralMonthlySummaryList = "GetReferralMonthlySummaryList";
        public const string DeleteReferralMonthlySummary = "DeleteReferralMonthlySummary";
        public const string GetAttandanceMonthlySummaryList = "GetAttandanceMonthlySummaryList";

        public const string GetFailityListForDDL = "GetFailityListForDDL";
        public const string GetEmployeeListForDDL = "GetEmployeeListForDDL";
        public const string SaveEmployeePreferences = "SaveEmployeePreferences";
        public const string SaveReferralPreferences = "SaveReferralPreferences";

        public const string SetDefaultReferralTaskMapping = "SetDefaultReferralTaskMapping";
        public const string HC_SaveReferralCareGiverDetails = "HC_SaveReferralCareGiverDetails";





        public const string GetEmployeeTimeSlotsPageModel = "GetEmployeeTimeSlotsPageModel";
        public const string GetEmployeeCalenderPageModel = "GetEmployeeCalenderPageModel";
        public const string GetReferralCalenderPageModel = "GetReferralCalenderPageModel";
        public const string EmpBulkSchedule = "EmpBulkSchedule";


        public const string GetEtsMasterList = "GetEtsMasterList";
        public const string DeleteEtsMaster = "DeleteEtsMaster";
        public const string AddEtsMaster = "AddEtsMaster";

        public const string GetEtsDetailList = "GetEtsDetailList";
        public const string DeleteEtsDetail = "DeleteEtsDetail";
        public const string AddEtsDetail = "AddEtsDetail";
        public const string EmployeeTimeSlotForceUpdate = "EmployeeTimeSlotForceUpdate";

        public const string GetEmployeeVisitList = "GetEmployeeVisitList";
        public const string GetGroupTimesheetList = "GetGroupTimesheetList";
        public const string SaveGroupTimesheetList = "SaveGroupTimesheetList";
        public const string DeleteEmployeeVisit = "DeleteEmployeeVisit";
        public const string MarkEmployeeVisitAsComplete = "MarkEmployeeVisitAsComplete";
        public const string GetMissingTimesheet = "GetMissingTimesheet";

        public const string GetEmployeeVisitNoteList = "GetEmployeeVisitNoteList";
        public const string GetVisitTaskDocumentList = "GetVisitTaskDocumentList";
        public const string GetVisitApprovalList = "GetVisitApprovalList";
        public const string ApproveVisitList = "ApproveVisitList";
        public const string GetNurseSignatureList = "GetNurseSignatureList";
        public const string NurseSignature = "NurseSignature";
        public const string API_SetEmployeeVisitTime = "API_SetEmployeeVisitTime";

        public const string GetEmployeeVisitConclusionList = "GetEmployeeVisitConclusionList";
        public const string DeleteEmployeeVisitNote = "DeleteEmployeeVisitNote";
        public const string DeleteDeviationNote = "DeleteDeviationNote";
        public const string CheckEmployeeUniqueID = "CheckEmployeeUniqueID";
        public const string CheckReferralUserName = "CheckReferralUserName";
        public const string GetNpiDetails = "GetNpiDetails";


        public const string GetBlockEmpList = "GetBlockEmpList";
        public const string SaveBlockEmp = "SaveBlockEmp";
        public const string DeleteBlockEmp = "DeleteBlockEmp";



        public const string AddPreferencePageModel = "AddPreferencePageModel";
        public const string SavePreference = "SavePreference";
        public const string SetPreferenceListPage = "SetPreferenceListPage";
        public const string GetPreferenceList = "GetPreferenceList";
        public const string GetPreferenceListForReferral = "GetPreferenceListForReferral";
        public const string DeletePreference = "DeletePreference";

        public const string HC_GetEmployeeBillingReportList = "HC_GetEmployeeBillingReportList";
        public const string HC_GetEmployeeOverPayReportList = "HC_GetEmployeeOverPayReportList";

        public const string HC_GetReferralPayorsMapping = "HC_GetReferralPayorsMapping";
        public const string HC_GetPriorAutherizationCodeByPayorAndRererrals = "HC_GetPriorAutherizationCodeByPayorAndRererrals";

        #region Employee Day off
        public const string HC_EmployeeDayOffPage = "HC_EmployeeDayOffPage";
        public const string HC_SaveEmployeeDayOff = "HC_SaveEmployeeDayOff";
        public const string HC_GetEmpSchedulesOnDayOff = "HC_GetEmpSchedulesOnDayOff";


        public const string HC_DayOffAction = "HC_DayOffAction";


        public const string HC_GetEmployeeDayOffList = "HC_GetEmployeeDayOffList";
        public const string HC_DeleteEmployeeDayOff = "HC_DeleteEmployeeDayOff";

        #endregion



        public const string GenerateEmployeeTimeSlotDates = "GenerateEmployeeTimeSlotDates";
        public const string GenerateReferralTimeSlotDates = "GenerateReferralTimeSlotDates";
        public const string GenerateReferralTimeSlotDates_ForDayCare = "GenerateReferralTimeSlotDates_ForDayCare";
        public const string WindowService_CreateBulkSchedules = "WindowService_CreateBulkSchedules";



        public const string GetReferralTimeSlotsPageModel = "GetReferralTimeSlotsPageModel";
        public const string GetCertificateAuthority = "GetCertificateAuthority";


        public const string GetRtsMasterList = "GetRtsMasterList";
        public const string GetReferralTimeSlot = "GetReferralTimeSlot";
        public const string GetReferralTimeSlotDetail = "GetReferralTimeSlotDetail";
        public const string DeleteRtsMaster = "DeleteRtsMaster";
        public const string AddRtsMaster = "AddRtsMaster";
        public const string AddRtsMaterCaseManagement = "AddRtsMater_CaseManagement";
        public const string GetReferralAuthorizations = "HC_GetReferralAuthorizationsByReferralID";

        public const string GetReferralCaseLoadList = "GetReferralCaseLoadList";
        public const string RemoveReferralCaseLoad = "RemoveReferralCaseLoad";
        public const string AddReferralCaseLoad = "AddReferralCaseLoad";

        public const string GetChecklistItems = "CHK_GetChecklistItems";
        public const string GetIsChecklistRemaining = "CHK_GetIsChecklistRemaining";
        public const string SaveChecklistItems = "CHK_SaveChecklistItems";
        public const string GetVisitChecklistItems = "CHK_GetVisitChecklistItems";
        public const string GetVisitChecklistItemDetail = "CHK_GetVisitChecklistItemDetail";

        public const string GetRtsDetailList = "GetRtsDetailList";
        public const string DeleteRtsDetail = "DeleteRtsDetail";
        public const string AddRtsDetail = "AddRtsDetail";
        public const string AddRtsDetailByPriorAuth = "AddRtsDetailByPriorAuth";
        public const string UpdateRtsDetail = "UpdateRtsDetail";
        public const string ReferralTimeSlotForceUpdate = "ReferralTimeSlotForceUpdate";
        public const string UpdateEtsDetail = "UpdateEtsDetail";


        #region  New Scheduling Changes

        public const string HC_SetScheduleAssignmentModel01 = "HC_SetScheduleAssignmentModel01";
        public const string HC_PrivateDuty_SetScheduleAssignmentModel = "HC_PrivateDuty_SetScheduleAssignmentModel";
        public const string HC_DayCare_SetScheduleAssignmentModel = "HC_DayCare_SetScheduleAssignmentModel";


        public const string HC_CaseManagement_SetScheduleAssignmentModel = "HC_CaseManagement_SetScheduleAssignmentModel";
        public const string HC_GetReferralEmployeeVisits = "GetReferralEmployeeVisits";
        public const string SaveEmployeeVisitsTransportLog = "SaveEmployeeVisitsTransportLog";
        public const string SaveEmployeeVisitsTransportLogDetail = "SaveEmployeeVisitsTransportLogDetail";

        public const string HC_GetEmployeeListForScheduling = "HC_GetEmployeeListForScheduling";
        public const string HC_PrivateDuty_GetEmployeeListForScheduling = "HC_PrivateDuty_GetEmployeeListForScheduling";
        public const string GetSchEmployeeDetailForPopup = "GetSchEmployeeDetailForPopup";
        public const string HC_PrivateDuty_GetSchEmployeeDetailForPopup = "HC_PrivateDuty_GetSchEmployeeDetailForPopup";


        public const string HC_GetReferralForScheduling = "HC_GetReferralForScheduling";
        public const string HC_DayCare_GetReferralForScheduling = "HC_DayCare_GetReferralForScheduling";
        public const string HC_CaseManagement_GetReferralForScheduling = "HC_CaseManagement_GetReferralForScheduling";
        public const string HC_PrivateDuty_GetReferralForScheduling = "HC_PrivateDuty_GetReferralForScheduling";




        public const string HC_GetScheduleListByReferrals = "HC_GetScheduleListByReferrals";
        public const string HC_GetVirtualVisitsList = "HC_GetVirtualVisitsList";
        public const string HC_PrivateDuty_GetScheduleListByReferrals = "HC_PrivateDuty_GetScheduleListByReferrals";

        public const string HC_DayCare_GetScheduleListByReferrals = "HC_DayCare_GetScheduleListByReferrals";





        public const string HC_CaseManagement_GetScheduleListByReferrals = "HC_CaseManagement_GetScheduleListByReferrals";



        public const string SaveSchedule = "SaveSchedule";
        public const string HC_PrivateDuty_SaveSchedule = "HC_PrivateDuty_SaveSchedule";

        public const string HC_DayCare_SaveSchedule = "HC_DayCare_SaveSchedule";
        public const string RemoveSchedule = "RemoveSchedule";
        public const string HC_PrivateDuty_RemoveSchedule = "HC_PrivateDuty_RemoveSchedule";

        public const string HC_DayCare_DeleteScheduleFromCalender = "HC_DayCare_DeleteScheduleFromCalender";


        public const string GetEmpRefSchPageModel = "GetEmpRefSchPageModel";
        public const string HC_PrivateDuty_GetEmpRefSchPageModel = "HC_PrivateDuty_GetEmpRefSchPageModel";

        public const string GetEmpRefSchOptions = "GetEmpRefSchOptions";
        public const string GetEmpRefSchOptions_GetCareTypeIds = "GetEmpRefSchOptions_GetCareTypeIds";
        public const string GetRCLEmpRefSchOptions = "GetRCLEmpRefSchOptions";
        public const string HC_PrivateDuty_GetEmpRefSchOptions = "HC_PrivateDuty_GetEmpRefSchOptions";

        /*Schedule master OPT*/
        public const string GetEmpRefSchOptions_PatientVisitFrequency_HC = "GetEmpRefSchOptions_PatientVisitFrequency_HC";
        public const string GetEmpRefSchOptions_ClientOnHoldData_HC = "GetEmpRefSchOptions_ClientOnHoldData_HC";
        public const string GetEmpRefSchOptions_ReferralInfo_HC = "GetEmpRefSchOptions_ReferralInfo_HC";
        public const string GetEmpRefSchOptions_ScheduleInfo_HC = "GetEmpRefSchOptions_ScheduleInfo_HC";
        /*Schedule master OPT*/

        public const string HC_DayCare_GetEmpRefSchOptions = "HC_DayCare_GetEmpRefSchOptions";
        public const string HC_CaseManagement_GetEmpRefSchOptions = "HC_CaseManagement_GetEmpRefSchOptions";
        public const string CreateBulkSchedules = "CreateBulkSchedules";
        public const string HC_PrivateDuty_CreateBulkSchedules = "HC_PrivateDuty_CreateBulkSchedules";


        public const string HC_DayCare_CreateBulkSchedules = "HC_DayCare_CreateBulkSchedules";

        public const string CreateBulkSchedules01 = "CreateBulkSchedules01";




        #endregion


        public const string GetEmpClockInOutList = "GetEmpClockInOutList";
        public const string GetPatientAddress = "GetPatientAddress";
        public const string GetEmpOverTimeList = "GetEmpOverTimeList";
        public const string GetNewPatientList = "GetNewPatientList";
        public const string GetActivePatientCountList = "GetActivePatientCountList";
        public const string GetPatientNotScheduleList = "GetPatientNotScheduleList";
        public const string GetEmployeeTimeStatics = "GetEmployeeTimeStatics";
        public const string GetNotifications = "GetNotifications";

        public const string GetSchEmpRefSkills = "GetSchEmpRefSkills";
        public const string HC_PrivateDuty_GetSchEmpRefSkills = "HC_PrivateDuty_GetSchEmpRefSkills";

        public const string DeleteEmpRefSchedule = "DeleteEmpRefSchedule";
        public const string HC_PrivateDuty_DeleteEmpRefSchedule = "HC_PrivateDuty_DeleteEmpRefSchedule";

        public const string HC_DayCare_DeleteEmpRefSchedule = "HC_DayCare_DeleteEmpRefSchedule";

        public const string GetAssignedEmployees = "GetAssignedEmployees";
        public const string HC_PrivateDuty_GetAssignedEmployees = "HC_PrivateDuty_GetAssignedEmployees";
        public const string HC_DayCare_GetAssignedFacilities = "HC_DayCare_GetAssignedFacilities";

        public const string SetEmployeeVisitListPage = "SetEmployeeVisitListPage";
        public const string SetEmployeeVisitListPageDMAS = "SetEmployeeVisitListPageDMAS";
        public const string SetEmployeeBillingListPage = "SetEmployeeBillingListPage";



        public const string GetVisitTaskCategory = "GetVisitTaskCategory";
        public const string GetVisitTaskSubCategory = "GetVisitTaskSubCategory";
        public const string GetModelCategoryList = "GetModelCategoryList";
        public const string SaveCategory = "SaveCategory";
        public const string SaveSubCategory = "SaveSubCategory";

        public const string RefreshNotes01 = "RefreshNotes01";

        public const string GetSBSEmployeeList = "GetSBSEmployeeList";

        public const string GetTwilioNotifyModel = "GetTwilioNotifyModel";
        public const string SaveGroupMessageLogs = "SaveGroupMessageLogs";
        public const string GetSentSMSList = "GetSentSMSList";
        public const string GetEmployeesForSentSms = "GetEmployeesForSentSms";
        public const string GetPCATimesheetDetail = "GetPCATimesheetDetail";
        public const string GetPCATimesheetDetailDayCare = "adc.GetPCATimesheetDetail";
        public const string GetAddressOfNullLatLong = "GetAddressOfNullLatLong";
        public const string UpdateLatLong = "UpdateLatLong";
        public const string SaveEmployeeVisit = "SaveEmployeeVisit";
        public const string GetPCATimesheetDetailEdit = "GetPCATimesheetDetailEdit";
        public const string SavePCACompeleted = "SavePCACompeleted";
        public const string UpdateEmployeeVisitPayorAndAutherizationCode = "HC_UpdateEmployeeVisitPayorAndAutherizationCode";
        public const string UpdateEmployeeSignature = "UpdateEmployeeSignature";
        public const string UpdateReferralProfileImgPath = "UpdateReferralProfileImgPath";

        public const string GetGroupVisitTask = "GetGroupVisitTask";
        public const string GetGroupTaskOptionList = "GetGroupTaskOptionList";
        public const string GetMappedVisitTask = "GetMappedVisitTask";
        public const string GetReferralTaskForms = "GetReferralTaskForms";
        public const string AddReferralTaskForm = "AddReferralTaskForm";
        public const string BulkUpdateReferralList = "BulkUpdateReferralList";
        public const string DeleteReferralTaskForm = "DeleteReferralTaskForm";
        public const string SaveVisitNote = "SaveVisitNote";
        public const string ChangeConclusionAnswer = "ChangeConclusionAnswer";
        public const string SaveVisitConclusion = "SaveVisitConclusion";
        public const string SaveDeviationNotes = "SaveDeviationNotes";
        public const string GetDeviationNotes = "getDeviationNotes";
        public const string SaveRegularHours = "SaveRegularHours";
        public const string GetEmployeeEmailSignature = "GetEmployeeEmailSignature";
        public const string OnHoldUnHoldAction = "OnHoldUnHoldAction";
        public const string HC_PrivateDuty_OnHoldUnHoldAction = "HC_PrivateDuty_OnHoldUnHoldAction";
        public const string HC_DayCare_OnHoldUnHoldAction = "HC_DayCare_OnHoldUnHoldAction";
        public const string HC_CaseManagement_OnHoldUnHoldAction = "HC_CaseManagement_OnHoldUnHoldAction";

        public const string HC_DayCare_SavePatientAttendance = "HC_DayCare_SavePatientAttendance";



        public const string GetSearchRegion = "GetSearchRegion";
        public const string AddRegion = "AddRegion";
        public const string OnRemoveScheduleAction = "OnRemoveScheduleAction";
        public const string HC_PrivateDuty_OnRemoveScheduleAction = "HC_PrivateDuty_OnRemoveScheduleAction";

        public const string HC_DayCare_OnRemoveScheduleAction = "HC_DayCare_OnRemoveScheduleAction";
        public const string HC_ValidateReferralBillingAuthorization = "HC_ValidateReferralBillingAuthorization";

        public const string GetDDMasterList = "GetDDMasterList";
        public const string DeleteDDMaster = "DeleteDDMaster";
        public const string SaveDDmaster = "SaveDDmaster";

        public const string SetCompliancePage = "SetCompliancePage";
        public const string GetComplianceList = "GetComplianceList";
        public const string DeleteCompliance = "DeleteCompliance";
        public const string SaveCompliance = "SaveCompliance";
        public const string CheckForParentChildMapping = "CheckForParentChildMapping";
        public const string CompliancesListChangeSortingOrder = "CompliancesListChangeSortingOrder";



        #region Physician
        public const string GetPhysicianDetail = "GetPhysicianDetail";
        public const string SavePhysician = "SavePhysician";
        public const string GetPhysicianList = "GetPhysicianList";
        public const string DeletePhysician = "DeletePhysician";
        #endregion

        #region EBCategory
        public const string GetEBCategoryDetail = "GetEBCategoryDetail";
        public const string SaveEBCategory = "SaveEBCategory";
        public const string GetEBCategoryList = "GetEBCategoryList";
        public const string DeleteEBCategory = "DeleteEBCategory";
        #endregion

        #region EBMarkets
        public const string GetEBMarketsDetail = "GetEBMarketsDetail";
        public const string SaveEBMarkets = "SaveEBMarkets";
        public const string Getebmarketslist = "Getebmarketslist";
        public const string Deleteebmarkets = "Deleteebmarkets";
        #endregion

        #region EBForms
        public const string GetEBFormsDetail = "GetEBFormsDetail";
        public const string SaveEBForms = "SaveEBForms";
        public const string Getebformslist = "Getebformslist";
        public const string Deleteebforms = "Deleteebforms";
        #endregion

        #region Home Care Note
        public const string HC_AddNote = "HC_AddNote";
        #endregion

        #region HomeCare Upload 835 File
        public const string HC_GetSetUpload835 = "HC_GetSetUpload835";
        public const string HC_GetUpload835FileList = "HC_GetUpload835FileList";
        public const string HC_GetDeleteUpload835FilePathList = "HC_GetDeleteUpload835FilePathList";
        public const string HC_DeleteUpload835File = "HC_DeleteUpload835File";
        public const string HC_ProcessUpload835File = "HC_ProcessUpload835File";
        public const string HC_GetUpload835FilesForProcess = "HC_GetUpload835FilesForProcess";
        public const string HC_GetBatchNoteDetailsBasedOnServiceDetails = "HC_GetBatchNoteDetailsBasedOnServiceDetails";
        public const string HC_UpdateBatchReconcile = "HC_UpdateBatchReconcile";
        #endregion

        #region Reconcile 835 / EOB
        public const string HC_GetSetReconcile835 = "HC_GetSetReconcile835";
        public const string HC_GetUpload835FilesForAutoComplete = "HC_GetUpload835FilesForAutoComplete";
        public const string HC_GetReconcile835List = "HC_GetReconcile835List";
        public const string HC_GetReconcileBatchNoteDetails = "HC_GetReconcileBatchNoteDetails";
        public const string HC_MarkNoteAsLatest = "HC_MarkNoteAsLatest";
        public const string HC_ExportReconcile835List = "HC_ExportReconcile835List";
        public const string HC_SetClaimAdjustmentFlag01 = "HC_SetClaimAdjustmentFlag01";
        public const string HC_BulkSetClaimAdjustmentFlag = "HC_BulkSetClaimAdjustmentFlag";

        public const string HC_GetReconcileList = "HC_GetReconcileList";
        public const string HC_GetParentReconcileList = "HC_GetParentReconcileList";
        public const string HC_GetChildReconcileList = "HC_GetChildReconcileList";
        public const string HC_MarkNoteAsLatest01 = "HC_MarkNoteAsLatest01";
        public const string HC_ExportReconcileList = "HC_ExportReconcileList";
        #endregion

        public const string GetParentGeneralDetailForMapping = "GetParentGeneralDetailForMapping";
        public const string SaveParentChildMapping = "SaveParentChildMapping";

        #region CMS - 1500

        public const string HC_GenerateEdiFileModel = "HC_GenerateEdiFileModel";
        public const string HC_CM_GenerateEdiFileModel = "HC_CM_GenerateEdiFileModel";

        #endregion

        public const string CreateWebNotifications = "CreateWebNotifications";
        public const string GetWebNotifications = "GetWebNotifications";
        public const string DeleteWebNotification = "DeleteWebNotification";
        public const string DeleteWebNotifications = "DeleteWebNotifications";
        public const string MarkAsReadWebNotifications = "MarkAsReadWebNotifications";
        public const string GetWebNotificationsCount = "GetWebNotificationsCount";

        public const string CreateBroadcastNotifications = "CreateBroadcastNotifications";
        public const string GetBroadcastNotificationList = "GetBroadcastNotificationList";
        public const string GetEmployeesForBroadcastNotifications = "GetEmployeesForBroadcastNotifications";
        public const string GetDataForBroadcastNotification = "GetDataForBroadcastNotification";

        public const string GetOrganizationDetails = "GetOrganizationDetails";
        public const string GetOrganizationPreference = "GetOrganizationPreference";
        public const string SaveOrganizationPreference = "SaveOrganizationPreference";


        public const string GetEmpDetailsForNotification = "GetEmpDetailsForNotification";
        public const string OnHoldNotificationLog = "OnHoldNotificationLog";

        public const string HC_SaveReferralDocument = "HC_SaveReferralDocument";
        public const string SaveProfileImage = "SaveProfileImage";
        public const string HC_SaveReferralCompliance = "HC_SaveReferralCompliance";
        public const string HC_GetReferralDocumentList = "HC_GetReferralDocumentList";
        public const string HC_SaveUserDocument = "HC_SaveUserDocument";
        public const string HC_SaveDocument = "HC_SaveDocument";
        public const string HC_SaveDocumentNew = "HC_SaveDocumentNew";
        public const string HC_DeleteDocument = "HC_DeleteDocument";
        public const string HC_SetReferralMissingDocument = "HC_SetReferralMissingDocument";
        public const string SaveTaskDetail = "SaveTaskDetail";
        public const string HC_SaveEmpDocument = "HC_SaveEmpDocument";
        public const string SetAddAssessmentQuestionPage = "SetAddAssessmentQuestionPage";

        public const string HC_GetCaseManagerListForAutoCompleter = "HC_GetCaseManagerListForAutoCompleter";
        public const string UploadCerificate = "UploadCerificate";
        public const string GetEmployeeEmail = "GetEmployeeEmail";
        public const string GetReferralEmail = "GetReferralEmail";

        #region Invoices

        public const string SetInvoiceListPage = "SetInvoiceListPage";
        public const string HC_GetInvoiceDetail = "HC_GetInvoiceDetail";
        public const string HC_PayInvoiceAmount = "HC_PayInvoiceAmount";
        public const string HC_GetInvoiceList = "HC_GetInvoiceList";
        public const string HC_GenerateInvoicesService = "HC_GenerateInvoicesService";
        public const string DeleteInvoices = "DeleteInvoices";
        public const string UpdateInvoice = "UpdateInvoice";

        public const string GetAllOrganizationInvoiceByOrgId = "GetAllOrganizationInvoiceByOrgId";
        public const string GetUnPaidInvoiceByOrganizationId = "GetUnPaidInvoiceByOrganizationId";

        #endregion
        public const string HC_GetFormListPage = "HC_GetFormListPage";
        public const string HC_SaveNewEBForm = "HC_SaveNewEBForm";
        public const string HC_SetAddFormPage = "HC_SetAddFormPage";

        public const string GetPatientEmpInfo = "GetPatientEmpInfo";
        public const string SetOrganizationFormListPage = "SetOrganizationFormListPage";
        public const string GetOrganizationFormList = "GetOrganizationFormList";


        public const string SetOrganizationFormPage = "SetOrganizationFormPage";
        public const string SaveOrganizationSelectedForms = "SaveOrganizationSelectedForms";
        public const string SaveOrganizationFormName = "SaveOrganizationFormName";


        public const string GetSavedFormMappings = "GetSavedFormMappings";
        public const string GetSavedFormList = "GetSavedFormList";





        public const string HC_SetMIFForm = "HC_SetMIFForm";
        public const string HC_SaveMIFDetail = "HC_SaveMIFDetail";
        public const string HC_GetMIFDetailForPDF = "HC_GetMIFDetailForPDF";
        public const string HC_GetReferralMIFForms = "HC_GetReferralMIFForms";
        public const string HC_BypassActionTaken = "HC_BypassActionTaken";
        public const string HC_GetPendingBypassVisit = "HC_GetPendingBypassVisit";
        public const string GetOrganizationFormListForMapping = "GetOrganizationFormListForMapping";
        public const string GetSearchTag = "GetSearchTag";

        public const string GetOrgFormTagList = "GetOrgFormTagList";
        public const string AddOrgFormTag = "AddOrgFormTag";
        public const string DeleteFormTag = "DeleteFormTag";


        public const string HC_GetReferralSectionList = "HC_GetReferralSectionList";
        public const string HC_GetReferralSubSectionList = "HC_GetReferralSubSectionList";
        public const string HC_SaveSubSection = "HC_SaveSubSection";
        public const string HC_SaveSubSectionNew = "HC_SaveSubSectionNew";
        public const string HC_SaveSection = "HC_SaveSection";
        public const string HC_SaveSectionNew = "HC_SaveSectionNew";
        public const string HC_GetReferralFormList = "HC_GetReferralFormList";

        public const string GetOrgFormListForMappingWithTask = "GetOrgFormListForMappingWithTask";
        public const string MapSelectedForms = "MapSelectedForms";
        public const string OnFormChecked = "OnFormChecked";
        public const string VisitTaskFormEditCompliance = "VisitTaskFormEditCompliance";
        public const string GetTaskFormList = "GetTaskFormList";
        public const string DeleteMappedForm = "DeleteMappedForm";
        public const string HC_MapForm = "HC_MapForm";
        public const string HC_SavedNewHtmlFormWithSubsection = "HC_SavedNewHtmlFormWithSubsection";
        public const string HC_DeleteReferralDocument = "HC_DeleteReferralDocument";
        public const string GetReferralDocumentListNew = "GetReferralDocumentListNew";
        public const string AddCareTypeSlot = "AddCareTypeSlot";
        public const string GetCareTypeScheduleList = "GetCareTypeScheduleList";
        public const string HC_GetSavedHtmlFormContent = "HC_GetSavedHtmlFormContent";
        public const string HC_GetHTMLFormTokenReplaceModel = "HC_GetHTMLFormTokenReplaceModel";
        public const string SaveDocumentFormName = "SaveDocumentFormName";
        public const string HC_GetPdfFieldsData = "HC_GetPdfFieldsData";

        public const string GetLoginPageDetail = "GetLoginPageDetail";


        //refrrel stored Procedure name
        public const string GetActiveReferralList = "GetActiveReferralList";
        public const string DMASForm_90FormList = "DMASFormList";
        public const string GetReportMaster = "GetReportMaster";
        public const string API_UniversalPriorAuthorization = "API_UniversalPriorAuthorization";
        public const string EmployeeList = "EmployeeList";
        public const string CategoryList = "CategoryList";
        public const string DMAS_90Forms = "DMAS_90Forms";
        public const string DMASFormList = "DMAS_90Forms";
        public const string GetWeeklyReportDMAS90 = "WeeklyReportDMAS90";
        public const string WeeklyReportDMAS90GetDays = "WeeklyReportDMAS90GetDays";
        public const string GetPatientScheduledEmployees = "GetPatientScheduledEmployees";
        public const string GetCaretype = "GetCaretype";
        public const string GetRegion = "rpt.locationList";
        public const string NotificationForEmployeeLateClockIn = "NotificationForEmployeeLateClockIn";

        public const string GetCareTypes = "GetCareTypes";
        public const string GetCarePlanCareTypes = "GetCarePlanCareTypes";
        public const string ExistanceOfRefferralTimeslot = "ExistanceOfRefferralTimeslot";
        #region Oraganization Setting
        public const string GetOrganizationSettingsPage = "GetOrganizationSettingsPage";
        #endregion

        public const string SetWizardStatus = "SetWizardStatus";
        public const string GetWizardStatus = "GetWizardStatus";
        public const string SaveEmployeeVisitPayer = "SaveEmployeeVisitPayer";

        public const string HC_AddLatestERA = "HC_AddLatestERA";
        
            
        public const string HC_GetLatestERA = "HC_GetLatestERA";
        public const string GetAnnouncement = "GetAnnouncement";
        public const string SaveAccessDeniedErrorLogs = "SaveAccessDeniedErrorLogs";
        public const string ConvertToReferral = "ConvertToReferral";
        //Added By Sanjay Start
        #region Capture Call
        public const string CaptureCallDetails = "CaptureCallDetails";
        public const string SaveCaptureCall = "SaveCaptureCall";
        public const string GetCaptureCallList = "GetCaptureCallList";
        public const string DeleteCaptureCall = "DeleteCaptureCall";

        #endregion
        //Added By Sanjay End
        //20220711 RN
        public const string GetSecurityQuestionByUserID = "GetSecurityQuestionByUserID";


        public const string GetEZOrbeonData_ByFormID = "GetEZOrbeonData_ByFormID";


        public const string HC_DayCare_SetScheduleAttendenceModel = "ADC.HC_DayCare_SetScheduleAttendenceModel";
        public const string HC_Daycare_GetScheduledPatientList = "ADC.HC_Daycare_GetScheduledPatientList";
        public const string Daycare_GetRelationTypeList = "ADC.Daycare_GetRelationTypeList";
        public const string HC_Daycare_PatientClockInClockOut = "ADC.HC_Daycare_PatientClockInClockOut";
        public const string HC_DayCare_GetSchedulePatientTasks = "ADC.HC_DayCare_GetSchedulePatientTasks";
        public const string HC_DayCare_GetReferralBillingAuthorizationList = "ADC.HC_DayCare_GetReferralBillingAuthorizationList";
        public const string HC_DayCare_GetVisitTaskOptionList = "ADC.GetVisitTaskOptionList";


        public const string GetDMAS97ABdetail = "GetDMAS97ABdetail";
        public const string Dmas97AbAddUpdate = "Dmas97AbAddUpdate";
        public const string SetDmas97AbListPage = "SetDmas97AbListPage";
        public const string DeleteDMAS97AB = "DeleteDMAS97AB";

        public const string GetDMAS99detail = "GetDMAS99detail";
        public const string DMAS99AddUpdate = "DMAS99AddUpdate";
        public const string Dmas99List = "Dmas99List";
        public const string DeleteDMAS99 = "DeleteDMAS99";
        public const string GetCertificateList = "GetCertificateList";
        public const string SaveCertificates = "SaveCertificates";
        public const string DeleteCertificate = "DeleteCertificate";
        public const string DeleteAllergy = "DeleteAllergy";



        public const string GetCMS485detail = "GetCMS485detail";
        public const string CMS485AddUpdate = "CMS485AddUpdate";
        public const string CMS485List = "CMS485List";
        public const string DeleteCMS48Form = "DeleteCMS48Form";

        public const string GetTaskListByActivity = "GetTaskListByActivity";
        public const string GetVisitTaskByCaretype = "GetVisitTaskByCaretype";

        public const string GetVisitReasonList = "GetVisitReasonList";
        public const string SaveVisitReason = "SaveVisitReason";
        public const string GetVisitReasonModalDetail = "GetVisitReasonModalDetail";

        public const string HHAXAggregatorVisitData = "[dbo].[HHAXAggregatorVisitData]";
        public const string CareBridgeAggregatorVisitData = "[dbo].[CareBridgeAggregatorVisitData]";
        public const string TellusAggregatorVisitData = "[dbo].[TellusAggregatorVisitData]";
        public const string SandataAggregatorVisitData = "[dbo].[SandataAggregatorVisitData]";

        public const string GetReferralDXCodeMappingsList = "GetReferralDXCodeMappingsList";
        public const string UpdateDxCode = "UpdateDxCode";
        public const string GetPriorAuthExpiring = "GetPriorAuthExpiring";
        public const string UpdateOrganizationSettings = "UpdateOrganizationSettings";
        public const string GetPatientBirthday = "GetPatientBirthday";
        public const string GetEmployeeBirthday = "GetEmployeeBirthday";
        public const string GetPatientClockInOutList = "GetPatientClockInOutList";
        public const string GetReferralPayor = "GetReferralPayor";
        public const string GetDischargedPatientList = "GetDischargedPatientList";
        public const string GetTransferPatientList = "GetTransferPatientList";
        public const string GetPendingPatientList = "GetPendingPatientList";
        public const string GetOnHoldPatientList = "GetOnHoldPatientList";
        public const string GetPatientMedicaidList = "GetPatientMedicaidList";
        public const string GetReferralStatus = "GetReferralStatus";
        public const string GetReferralCareType = "GetReferralCareType";
        public const string GetEmpClockInOutListWithOutStatus = "GetEmpClockInOutListWithOutStatus";
        public const string GetEmployeeReports = "GetEmployeeReports";
        public const string GetPatientReports = "GetPatientReports";
        public const string GetOtherReports = "GetOtherReports";



        public const string HC_DeleteNote_Temporary = "HC_DeleteNote_Temporary";
        public const string HC_GetBatchRelatedAllData_Temporary = "HC_GetBatchRelatedAllData_Temporary";

        public const string HC_GetNonProcessedERA = "HC_GetNonProcessedERA";
        public const string HC_SetERAValidationMessage = "HC_SetERAValidationMessage";
        public const string HC_GetEmployeeByUserName = "HC_GetEmployeeByUserName";
        public const string HC_ClockInOut = "HC_ClockInOut";
        public const string SaveEmployeeAttendanceMaster = "SaveEmployeeAttendanceMaster";
        public const string SaveEmployeeAttendanceDetail = "SaveEmployeeAttendanceDetail";
        public const string HC_EmployeeAttendanceCalendar = "HC_EmployeeAttendanceCalendar";
        public const string Get_EmployeeAttendanceCalendar = "Get_EmployeeAttendanceCalendar";

        #region TransportService
        public const string HC_SetTransportContactListPage = "HC_SetTransportContact";
        public const string SetAddTransportContactPage = "HC_SetAddTransportContact";
        public const string TransportContactAddUpdate = "TransportContactAddUpdate";
        public const string HC_GetTransportContactList = "HC_GetTransportContactList";
        public const string HC_DeleteTransportContact = "HC_DeleteTransportContact";
        public const string HC_SetAddTransportAssignment = "HC_SetAddTransportAssignment";
        public const string GetSearchOrganizationName = "GetSearchOrganizationName";
        #endregion

        #region Vehicle
        public const string HC_SetVehicleListPage = "HC_SetVehicleListPage";
        public const string SetAddVehiclePage = "HC_SetAddVehiclePage";
        public const string VehicleAddUpdate = "VehicleAddUpdate";
        public const string HC_GetVehicleList = "HC_GetVehicleList";
        public const string HC_DeleteVehicle = "HC_DeleteVehicle";
        public const string HC_SetEmployeeVisitsPage = "HC_SetEmployeeVisitsPage";

        #endregion
        public const string SaveTransportAssignment = "SaveTransportAssignment";
        public const string TransportAssignmentList = "TransportAssignmentList";
        public const string DeleteTransportAssignment = "DeleteTransportAssignment";
        public const string HC_TransportAssignmentListPage = "HC_SetTransportAssignmentList";//"HC_SetReferralListPage";
        public const string GetTransportAssignPatient = "GetTransportAssignPatient";
        public const string SaveTransportAssignPatient = "SaveTransportAssignPatient";
        public const string DeleteTransportAssignmentPatient = "DeleteTransportAssignmentPatient";
        //
        public const string HC_SetAddTransportAssignmentGroup = "HC_SetAddTransportAssignmentGroup";
        public const string TransportAssignmentGroupList = "TransportAssignmentGroupList";

        public const string GetTransportGroup = "GetTransportGroup";
        public const string GetTransportGroupDetail = "GetTransportGroupDetail";
        public const string SaveTransportGroup = "SaveTransportGroup";
        public const string SaveTransportGroupAssignPatient = "SaveTransportGroupAssignPatient";
        public const string SaveTransportGroupAssignPatientNote = "SaveTransportGroupAssignPatientNote";
        public const string DeleteTransportGroupAssignPatient = "DeleteTransportGroupAssignPatient";
        public const string TransportGroupAssignmentGroupList = "TransportGroupAssignmentGroupList";
        
        public const string HC_SetARAgingReportPage = "HC_SetARAgingReportPage";
        public const string HC_GetARAgingReport = "HC_GetARAgingReport";


        #region  Staffing
        public const string SaveAndGetFacilityDetails = "st.SaveAndGetFacilityDetails";
        public const string HC_GetReferralBillingAuthorizationDatesByReferralID = "HC_GetReferralBillingAuthorizationDatesByReferralID";
        public const string GetEmployeeGroup = "st.GetEmployeeGroup";
        public const string GetEmployeeByGroupId = "st.GetEmployeeByGroupId";
        public const string UpdateEmployeeGroupId = "st.UpdateEmployeeGroupId";
        public const string CreateEmpGroup = "st.CreateEmpGroup";
        public const string RemoveAllAssignedGroup = "st.RemoveAllAssignedGroup";
        #endregion
    }
}