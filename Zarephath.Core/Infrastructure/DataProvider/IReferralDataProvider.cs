using System;
using System.Collections.Generic;
using System.Web;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IReferralDataProvider
    {
        #region Add Referral

        #region Client Tab
        //  ServiceResponse DxCodes();
        ServiceResponse SetAddReferralPage(long referralID, long loggedInUserID);
        ServiceResponse AddReferral(AddReferralModel addReferralModel, long loggedInUserID);
        ServiceResponse AddContact(AddReferralModel addReferralModel, long loggedInUserID);
        List<Contact> GetContactList(string searchText, int pageSize);
        ServiceResponse DeteteContact(long contactMappingID, long loggedInUserID);
        ServiceResponse DeleteReferralPayorMapping(long referralPayorMappingID, long loggedInUserID);
        ServiceResponse MarkPayorAsActive(long referralID, long referralPayorMappingID, long loggedInUserID);
        ServiceResponse GetReferralPayorDetail(long referralPayorMappingID);
        ServiceResponse UpdateReferralPayorInformation(ReferralPayorMapping model, long loggedInUserID);




        ServiceResponse GetAuditLogList(SearchRefAuditLogListModel searchModel, int pageIndex, int pageSize,
                                        string sortIndex, string sortDirection, long loggedInId);

        ServiceResponse GetBXContractList(SearchRefBXContractListModel searchModel, int pageIndex, int pageSize,
                                        string sortIndex, string sortDirection, long loggedInId);

        ServiceResponse SaveBXContract(RefBXContractPageModel model, int pageIndex, int pageSize,
                                        string sortIndex, string sortDirection, long loggedInId);

        ServiceResponse UpdateBXContractStatus(ReferralBehaviorContract model, long loggedInId);
        ServiceResponse SaveSuspensionDetails(ReferralSuspention model, string EncryptedReferralID, long loggedInId, bool ResetSuspension = false, bool ResetBXContract = false);
        ServiceResponse GetSuspensionDetails(string encryptedReferralID);
        ServiceResponse HC_AddReferralSSNLog(HC_AddReferralModel addReferralModel, long loggedInUserID);

        #endregion

        #region ReferralCheckList Tab
        ServiceResponse SetReferralCheckList(long referralId);
        ServiceResponse SaveReferralCheckList(ReferralCheckList referralCheckList, List<DXCodeMappingList> dxCodeMappingList, long loggedInUserId);
        #endregion ReferralCheckList Tab

        #region ReferralSparForm Tab
        ServiceResponse SetReferralSparForm(long referralId);
        ServiceResponse SaveReferralSparForm(ReferralSparForm referralCheckList, long loggedInUserId);
        #endregion ReferralSparForm Tab

        #region ReferralInternalMessage Tab
        ServiceResponse SetReferralInternalMessage(long referralInternalMessageId, SearchReferralInternalMessage SearchReferralInternalMessage, long referralId, int pageIndex, int pageSize, string sortIndex, string sortDirection, bool isDelete);
        ServiceResponse SaveReferralInternalMessage(ReferralInternalMessage referralInternalMessage, long loggedInUserId);
        ServiceResponse ResolveReferralInternalMessage(long referralInternalMessageId, long referralId, long loggedInUserId);

        #endregion ReferralInternalMessage Tab
        #region ReferralNotes
        ServiceResponse HC_SaveReferralNotes(string RoleID, string EmployeesID, long referralNoteId, long employeeID, string noteDetail, string catId, long loggedInUserId, long CommonNoteID, bool isEdit = false, bool isPrivate = true);
        ServiceResponse GetReferralNotes(long referralId, long employeeId, long loggedInUserId);
        ServiceResponse DeleteReferralNote(long CommonNoteID, long loggedInUserId);
        ServiceResponse GetReferralEmployee(string RoleID);
        ServiceResponse DeleteReferralMedication(long ReferralMedicationID, long loggedInUserId);
        #endregion ReferralNot

        #region Referral Certificate
        ServiceResponse GetReferralCertifictaes(long id);
        ServiceResponse SaveCertificates(ReferralCertificate model);
        ServiceResponse DeleteCertificates(long certificateid);
        ServiceResponse CertificateAuthority();
        ServiceResponse UploadCertificate(HttpRequestBase currentHttpRequest, bool isEmployeeDocument = false);
        #endregion

        ServiceResponse GetCategory();


        #region ReferralDocuments
        ServiceResponse SendFax(FaxModel model);
        ServiceResponse GetReferralDetails(string id);
        ServiceResponse GetTemplateList();
        ServiceResponse GetOrganizationSettings();
        ServiceResponse GetSignature(long id);
        ServiceResponse GetTemplateDetails(string id, long ReferralID);
        ServiceResponse GetReferralEmail(string ReferralID);
        ServiceResponse GetEmployeeGroup(string name);
        ServiceResponse GetEmployeeEmail(long id);
        ServiceResponse SendReferralAttachment(MailModel model);
        ServiceResponse SendBulkEmail(MailModel model);
        string GetReferralDocumentPath(string id);
        ServiceResponse SetReferralDocument(long referralId);
        ServiceResponse SetReferralMissingDocument(long referralId);
        ServiceResponse SendEmailForReferralMissingDocument(MissingDocumentModel missingDocumentModel, long referralId, long loggedInUserID);
        ServiceResponse SaveFile(string file, string fileName, long referralID, long loggedInID);
        ServiceResponse GetReferralDocumentList(long referralID, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteDocument(long referralDocumentID, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse SaveDocument(ReferralDocument referralDoc, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        #endregion ReferralDocuments
        ServiceResponse SaveDxCode(string DXCodeName, string DXCodeWithoutDot, string DxCodeType, string Description, string DxCodeShortName);
        List<DXCode> GetDxcodeListForAutoComplete(string searchText, string ignoreIds, int pageSize);
        List<ReferralCaseManagerAutoComplete> GetCaseManagerForAutoComplete(string searchText, int pageSize);
        ServiceResponse DeleteReferralDxCodeMapping(ReferralDxCodeMappingDeleteModel referralDxCodeMappingDeleteModel, long referralId, long loggedInId);
        ServiceResponse HC_DeleteReferralDxCodeMapping(ReferralDxCodeMappingDeleteModel referralDxCodeMappingDeleteModel, long referralId, long loggedInId);
        ServiceResponse HC_DeleteReferralBeneficiaryType(ReferralBeneficiaryDetail referralBeneficiaryDetail, long loggedInId);
        ServiceResponse HC_DeleteReferralPhysician(ReferralPhysicianDetail referralPhysicianDetail, long loggedInId);
        ServiceResponse GetReferralDocumentDetails(string id);

        #region Referral Update AHCCCC ID
        ServiceResponse UpdateAhcccsid(ReferralAhcccsDetails model, Referral referral, long loggedInUserId);
        #endregion


        #endregion Add Referral

        #region Referral List        
        ServiceResponse SendReceiptNotificationEmail(long referralId);
        ServiceResponse SetReferralListPage(long loggedInID);

        ServiceResponse GetReferralList(SearchReferralListModel searchReferralModel, int pageIndex, int pageSize,
                                        string sortIndex, string sortDirection, long loggedInId);

        ServiceResponse GetReferralsForNurseSchedule(SearchReferralListModel searchReferralModel, string sortIndex, string sortDirection);
        ServiceResponse GetPayorDetailsByReferralID(long referralID);
        ServiceResponse GetAuthorizationCodesByReferralId(long referralID);
        ServiceResponse GetReferralAuthorizationsDetails(string referralIDs);
        ServiceResponse GetReferralDetails(Referral referral);

        ServiceResponse DeleteReferral(SearchReferralListModel searchReferralListModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId);
        ServiceResponse SaveReferralStatus(ReferralStatusModel referralStatusModel, long loggedInId);
        ServiceResponse UpdateAssigneeBulk(ReferralBulkUpdateModel referralBulkUpdateModel, long loggedInId);
        ServiceResponse UpdateAssignee(SearchReferralListModel searchReferralListModel, ReferralStatusModel referralStatusModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId);
        ServiceResponse UpdateStatus(SearchReferralListModel searchReferralListModel, ReferralStatusModel referralStatusModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId);
        ServiceResponse UpdateTimeSlotDetailEmployee(SearchReferralTimeSlotDetail searchReferralTimeSlotDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId);
        ServiceResponse SaveIncidentReportOrbeonForm(EmployeeVisitNoteForm form);
        ServiceResponse SaveReferralFaceSheetForm(EmployeeVisitNoteForm form);
        ServiceResponse SaveVitalForm(EmployeeVisitNoteForm form);
        #endregion Referral List

        #region Referral Medication
        ServiceResponse GetReferralMedications(int referralId, int pageSize, bool isActive);
        ServiceResponse Medication(ReferralMedication ReferralMedication);
        ServiceResponse SaveReferralMedication(ReferralMedication ReferralMedication);
        ServiceResponse EditReferralMedication(long ReferralMedicationID);
        ServiceResponse SearchReferralMedications(string search);
        ServiceResponse SaveReferralAllergy(ReferralAllergyModel model);
        ServiceResponse GetReferralAllergy(SearchAllergyModel model);
        ServiceResponse DeleteAllergy(string id);
        ServiceResponse GetAllergyDDL();
        ServiceResponse GetAllergyTitle();


        #endregion


        #region Referral Tracking List

        ServiceResponse SetReferralTrackingListPage(long loggedInID);
        ServiceResponse GetReferralTrackingList(SearchReferralTrackingListModel searchReferralTrackingModel, int pageIndex, int pageSize,
                                        string sortIndex, string sortDirection, long loggedInId);
        ServiceResponse DeleteReferralTracking(SearchReferralTrackingListModel searchReferralTrackingListModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId);
        ServiceResponse SaveReferralTrackingStatus(ReferralStatusModel referralStatusModel, long loggedInId);
        ServiceResponse SaveReferralTrackingComment(ReferralCommentModel referralCommentModel, long loggedInId);

        #endregion Referral Tracking List



        #region Autocompeleter list for Sibling

        List<GetReferralInfoList> GetReferralInfo(int pageSize, string IgnorIds, string searchText = null);
        ServiceResponse DeletePreference(ReferralPreferenceModel model);

        #endregion

        #region Delete for Referral Sibling

        ServiceResponse ReferralSiblingMappingDelete(long referralSiblingMappingId);

        #endregion

        #region Referral Review Assessment

        ServiceResponse SetReferralReviewAssessment(long referralID, long referralAssessmentID, long loggedInUserID);
        ServiceResponse SaveReferralReviewAssessment(ReferralAssessmentReview model, long loggedInUserID);
        ServiceResponse GetReferralReviewAssessmentList(SearchReferralAssessmentReview searchModel);
        ServiceResponse DeleteReferralReviewAssessment(SearchReferralAssessmentReview searchModel);

        //ServiceResponse SaveAssessmentResult(string file, string fileName, long referralID, long loggedInID);
        #endregion

        #region Referral Outcome and Measurements

        ServiceResponse SetReferralOutcomeMeasurement(long referralID, long referralOutcomeMeasurementID, long loggedInUserID);
        ServiceResponse SaveReferralOutcomeMeasurement(ReferralOutcomeMeasurement model, long loggedInUserID);
        ServiceResponse GetReferralOutcomeMeasurementList(SearchReferralOutcomeMeasurement searchModel);
        ServiceResponse DeleteReferralOutcomeMeasurement(SearchReferralOutcomeMeasurement searchModel);
        #endregion


        #region Referral Monthly Summery

        ServiceResponse SetReferralMonthlySummary(long referralID, long referralMonthlySummariesID, long loggedInUserID);
        ServiceResponse SaveReferralMonthlySummary(ReferralMonthlySummary model, long loggedInUserID);
        ServiceResponse GetReferralMonthlySummaryList(SearchReferralMonthlySummary searchModel, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse FindScheduleWithFaciltyAndServiceDate(FindScheduleWithFaciltyAndServiceDateModel model);
        ServiceResponse GetFacilityList();
        #endregion


        #region Referral Group MonthlySummary

        ServiceResponse FillGroupMonthlySummaryModel(long loggedInUserID);

        ServiceResponse SearchClientForMonthlySummary(SearchClientForMonthlySummary searchGroupNoteClient,
                                                      List<long> ignoreClientID);

        ServiceResponse SaveMultipleMonthlySummary(List<ReferralMonthlySummary> monthlySummaries,
                                                          long loggedInUserID);

        ServiceResponse MonthlySummaryList();


        ServiceResponse DeleteReferralMonthlySummary(SearchReferralMonthlySummary searchReferralMonthlySummary, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId);

        #endregion End Referral Group MonthlySummary





        #region In HOME CARE Data Provider Code


        ServiceResponse HC_ResolveReferralInternalMessage(long referralInternalMessageId, long referralId, string resolvedComment, long loggedInUserId);




        ServiceResponse HC_SetAddReferralPage(long referralID, long loggedInUserID);
        List<Region> GetSearchRegion(int pageSize, string searchText = null);
        ServiceResponse HC_AddReferral(HC_AddReferralModel addReferralModel, long loggedInUserID);
        ServiceResponse ReferralDxCodeMapping(HC_AddReferralModel addReferralModel, long loggedInUserID);
        ServiceResponse HC_UpdateAccount(ReferralAhcccsDetails model, Referral referral, long loggedInUserId);
        ServiceResponse SaveTaskOrder(List<ReferralDXCodeMapping> model, long RefID);

        #region Referral Compliance Details
        ServiceResponse HC_SaveReferralCompliance(ReferralComplianceModel referralComplianceModel, long loggedInUserID);
        #endregion

        #region Referral List

        ServiceResponse HC_SetReferralListPage(long loggedInID);
        ServiceResponse HC_SaveReferralStatus(ReferralStatusModel referralStatusModel, long loggedInId);

        #endregion Referral List


        #region Referral Documents

        #region Documents

        ServiceResponse HC_SetReferralDocument(long referralId);
        ServiceResponse HC_SaveFile(HttpRequestBase currentHttpRequest, bool isEmployeeDocument = false, bool isGoogleDriveDocument = false, string googleRefeshToken = "");
        ServiceResponse HC_GetReferralDocumentList(long referralID, long complianceID, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_GetReferralDocumentListNew(long roleId, SearchReferralDocumentListPage searchReferralDocumentListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeleteDocument(long referralDocumentID, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse HC_SaveDocument(ReferralDocument referralDoc, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse HC_SaveDocumentNew(ReferralDocument model, long loggedInUserID);
        ServiceResponse HC_GetDocumentListGoogleDrive(string refreshToken);
        ServiceResponse HC_LinkGoogleDocument(LinkDocModel model, string googleRefeshToken, bool isEmployeeDocument);

        #endregion

        #region Missing Documents
        ServiceResponse HC_SetReferralMissingDocument(long referralId);
        ServiceResponse HC_SendEmailForReferralMissingDocument(MissingDocumentModel missingDocumentModel, long referralId, long loggedInUserID);
        #endregion

        #endregion

        #region Referral History

        ServiceResponse GetReferralHistory(long referralID);
        ServiceResponse SaveReferralHistoryItem(ReferralHistory referralHistoryModel, long loggedInUserID);
        ServiceResponse DeleteReferralHistoryItem(long referralHistoryID);

        #endregion

        #region Employee Calender

        ServiceResponse HC_ReferralCalender();

        #endregion


        #region Referral Task Mapping Screen
        ServiceResponse GetVisitTaskList(SearchVisitTaskListPage searchVisitQuestionListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse SaveRefVisitTaskList(RefVisitTaskModel model, long loggedInUserId);
        ServiceResponse SaveTaskDetail(TaskModel model);
        ServiceResponse GetPatientTaskMappings(RefVisitTaskModel model);
        ServiceResponse GetReferralTaskMappingDetails(SearchVisitTaskListPage model);
        ServiceResponse GetReferralTaskMappingReports(SearchVisitTaskListPage model);
        ServiceResponse GetReferralGoal(SearchVisitTaskListPage model);
        ServiceResponse UpdateGoalIsActiveIsDeletedFlag(SearchVisitTaskListPage model);
        ServiceResponse SaveReferralGoal(SearchVisitTaskListPage model, long loggedInUserId);
        ServiceResponse DeleteRefTaskMapping(RefVisitTaskModel model, long loggedInUserId);
        ServiceResponse OnTaskChecked(RefVisitTaskModel model, long loggedInUserId);
        ServiceResponse SaveBulkRefVisitTaskList(List<RefVisitTaskModel> model, long loggedInUserId);
        ServiceResponse GetCaretype();
        ServiceResponse GetCarePlanCaretypes(long ReferralID);
        ServiceResponse GetVisitTaskCategory(string VisitTaskType, long CareType);
        ServiceResponse GetTaskByActivity(string VisitTaskType, long CareType, long VisitTaskCategoryId);

        #endregion

        ServiceResponse GetBlockEmpList(SearchRefBlockEmpListModel searchModel, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse SaveBlockEmp(ReferralBlockedEmployee model, SearchRefBlockEmpListModel searchModel, long loggedInId, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteBlockEmp(ReferralBlockedEmployee model, SearchRefBlockEmpListModel searchModel, long loggedInId, int pageIndex, int pageSize, string sortIndex, string sortDirection);






        #region Patient TimeSlots
        ServiceResponse HC_ReferralTimeSlots();
        ServiceResponse HC_ReferralTimeSlotss(string id);

        #region Employee Days with time slots


        #region Patient Msater

        ServiceResponse GetRtsMasterlist(SearchRTSMaster searchRTSMaster, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse GetReferralTimeSlotDetail(SearchReferralTimeSlotDetail searchReferralTimeSlotDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteRtsMaster(SearchRTSMaster searchRTSMaster, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse AddRtsMaster(ReferralTimeSlotMaster rtsMaster, long loggedInUserID);
        ServiceResponse AddRtsByPriorAuth(ReferralTimeSlotMaster rtsMaster, long loggedInUserID);
        ServiceResponse GetReferralAuthorizationsByReferralID(long referralID, long careTypeId);

        List<ReferralBillingAuthorizations> GetReferralAuthorizationsByReferralId(long referralID, long careTypeId);


        #endregion

        #region Patient Detail

        ServiceResponse GetRtsDetaillist(SearchRTSDetail searchRTSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteRtsDetail(SearchRTSDetail searchRTSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse AddRtsDetail(ReferralTimeSlotDetail rtsDetail, long loggedInUserID);
        ServiceResponse UpdateRtsDetail(ReferralTimeSlotDetail rtsDetail, SearchRTSDetail searchRTSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse ReferralTimeSlotForceUpdate(ReferralTimeSlotDetail model, SearchRTSDetail searchRTSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);

        #endregion


        #endregion



        #endregion

        #region Referral Payor Mapping
        ServiceResponse HC_AddReferralPayorMapping(ReferralPayorMapping referralPayorMapping, long loggedInUserId);
        ServiceResponse HC_GetReferralPayorMappingList(string encryptedReferralId, SearchReferralPayorMapping searchReferralPayorMapping, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeleteReferralPayorMapping(string encryptedReferralId, SearchReferralPayorMapping searchReferralPayorMapping, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        #endregion

        #region Referral Billing Setting
        ServiceResponse HC_AddReferralBillingSetting(ReferralBillingSetting referralBillingSetting, long loggedInUserId);
        ServiceResponse HC_GetReferralBillingSetting(ReferralBillingSetting referralBillingSetting);

        ServiceResponse HC_AddReferralBillingAuthrization(ReferralBillingAuthorization referralBillingAuthorization, long loggedInUserId);
        ServiceResponse GetAuthorizationLinkupList(long referralBillingAuthorizationID);
        ServiceResponse GetAuthorizationScheduleLinkList(SearchAuthorizationScheduleLinkList searchAuthorizationScheduleLinkList);
        ServiceResponse UpdateAuthorizationLinkup(UpdateAuthorizationLinkupModel model);
        ServiceResponse HC_SavePriorAuthorization(ReferralBillingAuthorization referralBillingAuthorization, long loggedInUserId);
        ServiceResponse HC_SavePriorAuthorizationServiceCodeDetails(ReferralBillingAuthorizationServiceCodeModel model, long loggedInUserId);
        List<ServiceCodes> HC_GetPayorServicecodeList(string searchText, string ReferralBillingAuthorizationID, int pageSize = 10);



        ServiceResponse HC_GetAuthorizationLoadModel(string encryptedReferralId);
        List<ServiceCodes> HC_GetPayorMappedServiceCodeList(string searchText, long PayorID, int pageSize = 10);
        ServiceResponse HC_GetPayorMappedServiceCodes(long PayorID);
        ServiceResponse HC_GetAuthServiceCodes(SearchAuthServiceCodesModel model);

        ServiceResponse HC_GetReferralBillingAuthorizationList(string encryptedReferralId, SearchReferralBillingAuthorization searchReferralBillingAuthorization, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_GetPriorAuthorizationList(string encryptedReferralId, SearchReferralBillingAuthorization searchReferralBillingAuthorization, int pageIndex, int pageSize, string sortIndex, string sortDirection);


        ServiceResponse HC_DeleteReferralBillingAuthorization(string encryptedReferralId, SearchReferralBillingAuthorization searchReferralBillingAuthorization, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeletePriorAuthorization(string encryptedReferralId, SearchReferralBillingAuthorization searchReferralBillingAuthorization, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse HC_DeletePAServiceCode(string referralBillingAuthorizationServiceCodeID, long loggedIdID);



        ServiceResponse HC_GetPAServiceCodeList(SearchReferralBillingAuthorizationCode searchReferralBillingAuthorizationCode);
        ServiceResponse GetPayorIdentificationNumber(string PayorID);
        #endregion

        #region Upload Referral Profile Image From API
        ServiceResponse UploadRefProfileImage(HttpRequestBase currentHttpRequest);
        #endregion

        #region CareForm
        ServiceResponse HC_GetCareFormDetails(SearchCareFormDetails searchCareFormDetails);
        ServiceResponse HC_SaveCareFormDetails(CareForm careForm, long loggedInUserId);
        ServiceResponse HC_SaveClientSignature(CareForm careForm, long loggedInUserId);
        ServiceResponse HC_SaveCareFormPdfFile(byte[] pdf, string filename, long careformID, long loggedInUserId);
        #endregion

        #region MIF Form
        ServiceResponse SetMIFForm(long referralID);
        ServiceResponse SaveMIFDetail(MIFDetail model);
        ServiceResponse HC_SaveMIFSignature(MIFDetail MIFDetail, long loggedInUserId);
        ServiceResponse GetMIFDetailForPDF(long MIFFormID);
        ServiceResponse GetReferralMIFForms(long referralId);
        #endregion

        #region New Document related changes
        ServiceResponse HC_GetReferralSectionList(string userType, long roleId, long referralId, string EmployeeID);
        ServiceResponse HC_GetReferralSubSectionList(long SectionID, string userType, long roleId, long referralId, string EmployeeID);
        ServiceResponse HC_SaveSection(SaveSection modal);
        ServiceResponse HC_SaveSectionNew(AddDirSubDirModal modal, long roleId);
        ServiceResponse HC_SaveSubSection(SaveSubSection modal);
        ServiceResponse HC_SaveSubSectionNew(AddDirSubDirModal modal);
        ServiceResponse HC_GetReferralFormList(FormModal modal);
        ServiceResponse HC_MapForm(MapFormDocModel modal, long roleID);
        ServiceResponse HC_SavedNewHtmlFormWithSubsection(SaveNewEBFormModel model, long loggedInId);
        ServiceResponse HC_DeleteReferralDocument(DeleteDocModel model, long loggedInId);
        ServiceResponse DeleteReferralDocumentViaAPI(DeleteDocModel model);
        ServiceResponse SaveDocumentFormName(DocFormNameModal model);
        ServiceResponse HC_DeleteReferralDocumentGoogle(DeleteDocModel model, long loggedInId, string googleRefeshToken);
        #endregion

        #region Patient Related eBriggs Form
        //_referralDataProvider
        #endregion

        ServiceResponse HC_GetFormListPage();
        ServiceResponse HC_SaveNewEBForm(SaveNewEBFormModel model, long loggedInId);

        //ServiceResponse HC_GetSavedFormListPage();

        #endregion

        #region Referral Case Load

        ServiceResponse GetReferralCaseLoadList(SearchRCLMaster searchRclMaster, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse RemoveReferralCaseLoad(SearchRCLMaster searchRclMaster, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse AddRclMaster(ReferralCaseload rtsMaster, long loggedInUserID);
        ServiceResponse MarkCaseLoadComplete(ReferralCaseload rtsMaster, long loggedInUserID);

        #endregion


        #region Referral Time Slots For Care Type
        ServiceResponse HC_ReferralCareTypeTimeSlots();
        ServiceResponse AddCareTypeSlot(CareTypeTimeSlot model, long loggedInUserID);
        ServiceResponse GetCareTypeScheduleList(SearchCTSchedule searchCTSchedule, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        #endregion

        #region Upload Referral Document From API
        ServiceResponse UploadDocumentViaAPI(HttpRequestBase currentHttpRequest);
        #endregion

        ServiceResponse HC_GetReferralPayorsMapping(long referralID, DateTime startDate);
        ServiceResponse HC_GetPriorAutherizationCodeByPayorAndRererrals(long payorID, long referralID);

        ServiceResponse HC_GetOrbeonFormDetailsByID(LinkDocModel model, bool isEmployeeDocument = false);

        ServiceResponse GetNoteSentenceList(string NoteSentenceTitle, string NoteSentenceDetails, int pageIndex, int pageSize, string sortIndex, string sortDirection);

        ServiceResponse DxCodeMappingList1(long RefID);

        ServiceResponse HC_SaveRefProfileImg(HttpRequestBase currentHttpRequest, bool isEmployeeDocument = false);
        ServiceResponse HC_GetReferralPayor(long loggedInUser);
        ServiceResponse HC_GetReferralStatus(long loggedInUser);
        ServiceResponse HC_GetReferralCareType(long loggedInUser);
        ServiceResponse GetDXcodeList(string ReferralID, long loggedInUserId);
        ServiceResponse GetPayorDetails(string PayorID, string ReferralID);
        ServiceResponse DxChangeSortingOrder(DxChangeSortingOrderModel model);
        ServiceResponse ExistanceOfReferralTimeslot(ReferralTimeSlotMaster rtsMaster);
        ServiceResponse PrioAuthorization(long ReferralID, long BillingAuthorizationID);

        ServiceResponse GetMasterJurisdictionList(string claimProcessor);
        ServiceResponse GetMasterTimezoneList(string claimProcessor);
        ServiceResponse GetReferralSourcesDD(string ItemType, int Isdeleted);
        ServiceResponse SaveReferralSourcesDD(ReferralSources model, long LoggedInID);
        ServiceResponse DeleteReferralSourcesDD(long id, long IsDeleted, string ItemType);

        bool IsDXCodeExist(string referralID, string PayorID);
    }
}
