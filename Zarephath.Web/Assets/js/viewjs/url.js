var SiteUrl =
{
    GetLayoutRelatedDetailsUrl: "/home/getlayoutrelateddetails/",
    GetDashboardUrl: "/home/dashboard",

    //Start: Master Pages
    AddEmployeeURL: "/employee/addemployee/",
    EmployeelistURL: "/employee/employeelist",
    GetEmployeeListURL: "/employee/getemployeelist",
    GetBulkSendRegistrationURL: "/employee/SendRegistration",
    DeleteEmployeeURL: "/employee/DeleteEmployee",
    //AddPreferenceURL: "/employee/AddPreference",
    DeletePreferenceURL: "/employee/DeletePreference",
    AddCaseManagerURL: "/casemanager/addcasemanager/",
    CaseManagerListURL: "/casemanager/casemanagerlist",
    GetAgencyLocationListURL: "/casemanager/getagencylocation",
    GetCaseManagerList: "/casemanager/getcasemanagerlist",
    DeleteCaseManager: "/casemanager/deletecasemanager",

    AddParentURL: "/parent/addparent/",
    ParentListURL: "/parent/parentlist",
    GetParentList: "/parent/getparentlist",
    DeleteParent: "/parent/deleteparent",



    AddDepartmentURL: "/department/adddepartment/",
    DepartmentListURL: "/department/departmentlist/",
    GetDepartmentListURL: "/department/getdepartmentlist/",
    DeleteDepartmentURL: "/department/deletedepartment/",
    GetZipCodeListURL: "/department/getzipcodelist/",

    AddFacilityHouseURL: "/facilityhouse/addfacilityhouse/",
    FacilityHouseListURL: "/facilityhouse/facilityhouselist",
    GetFacilityHouseListURL: "/facilityhouse/getfacilityhouselist",
    DeleteFacilityHouseURL: "/facilityhouse/deletefacilityhouse",
    GetParentFacilityHouse: "/facilityhouse/getparentfacilityhouse",
    GetFacilityTransportLocationMappingURL: "/facilityhouse/getfacilitytransportlocationmapping",
    SaveFacilityTransportLocationMappingURL: "/facilityhouse/savefacilitytransportlocationmapping",

    AddDxCodeURL: "/dxcode/adddxcode/",
    DxCodeListURL: "/dxcode/dxcodelist",
    GetDxCodeList: "/dxcode/getdxcodelist",
    DeleteDxCode: "/dxcode/deletedxcode",


    AddNoteSentenceURL: "/notesentence/addnotesentence/",
    PartialAddNoteSentenceURL: "/notesentence/PartialAddNoteSentence/",
    NoteSentenceListURL: "/notesentence/notesentencelist",
    GetNoteSentenceList: "/notesentence/getnotesentencelist",
    DeleteNoteSentence: "/notesentence/deletenotesentence",



    //End: Master Pages

    //Start: Role Permission
    GetRolePermissionURL: "/security/getrolepermission",
    GetReportsURL: "/security/getReports",
    SaveRoleWisePermissionURL: "/security/saverolewisepermission/",

    UpdateRoleNameURL: "/security/updaterolename/",
    AddNewRoleURL: "/security/addnewrole/",
    //End: Role Permission

    // Start Referral Page
    UpdateAHCCCSIDURL: "/referral/updateahcccsid/",
    GetAuditLogListURL: "/referral/getauditloglist/",
    GetTableDisplayValue: "/auditlog/gettabledisplayvalue/",
    GetBXContractListURL: "/referral/getbxcontractlist/",
    SaveBXContractURL: "/referral/savebxcontract/",
    UpdateBXContractStatusURL: "/referral/updatebxcontractstatus/",
    SaveSuspensionURL: "/referral/savesuspensiondetails/",
    GetSuspensionURL: "/referral/getsuspensiondetails/",


    SetReferralCheckListURL: "/referral/setreferralchecklist/",
    SaveReferralCheckListURL: "/referral/savereferralchecklist/",


    SetReferralSparFormURL: "/referral/setreferralsparform/",
    SaveReferralSparFormURL: "/referral/savereferralsparform/",

    SetReferralInternalMessageListURL: "/referral/setreferralinternalmessage/",
    SaveReferralInternalMessageURL: "/referral/saveReferralInternalmessage/",
    DeleteReferralInternalMessageURL: "/referral/deletereferralinternalmessage/",
    ResolveReferralInternalMessageURL: "/referral/resolvereferralinternalmessage/",
    MarkResolvedMessageAsReadURL: "/home/markresolvedmessageasread/",
    GetCaseManagersURL: "/hc/referral/getcasemanagerforautocomplete/",
    GetReferralDocumentList: "/referral/getreferraldocumentlist/",
    SaveReferralDocumentURL: "/referral/uploadfile/",
    DeleteDocumentURL: "/referral/DeleteDocument/",
    EditDocumentURL: "/referral/savedocument/",

    SetReferralMissingDocumentURL: "/referral/setreferralmissingdocument/",
    SendEmailForReferralMissingDocumentURL: "/referral/sendemailforreferralmissingdocument/",

    MarkPayorAsActive: "/referral/markpayorasactive/",
    GetReferralPayorDetail: "/referral/getreferralpayordetail/",
    UpdateReferralPayorInformation: "/referral/updatereferralpayorinformation/",



    // ReferraLlList Page
    SendReceiptNotificationEmailURL: "/referral/sendreceiptnotificationemail/",
    GetReferralListURL: "/referral/getreferrallist",
    GetNoteListURL: "/note/getnotelist",
    GetExportNoteListURL: "/note/exportnotelist",
    GetNoteClientListURL: "/note/getnoteclientlist",
    ValidateServiceCodeDetailsURL: "/note/validateservicecodedetails",
    ReferralListURL: "/referral/referrallist",
    DeleteReferralURL: "/referral/deletereferral",
    ReferralStatusUpdateURL: "/referral/savereferralstatus",
    UpdateAssigneeURL: "/referral/updateassignee",
    UploadFile: "/referral/uploadfile",
    GetDXCodeListForAutoCompleteURL: "/referral/getdxcodelistforautocomplete/",
    DeleteReferralDXCodeMappingURL: "/referral/deletereferraldxcodemapping/",
    DeleteReferralSiblingMappingURL: "/referral/ReferralSiblingMappingDelete/",
    // End Referral Page


    // ReferralTrackingList Page
    GetReferralTrackingListURL: "/referral/getreferraltrackinglist",
    ReferralTrackingListURL: "/referral/referraltrackinglist",
    DeleteReferralTrackingURL: "/referral/deletereferraltracking",
    ReferralTrackingStatusUpdateURL: "/referral/savereferraltrackingstatus",
    ReferralTrackingCommentUpdateURL: "/referral/savereferraltrackingcomment",
    // End Referral Page

    // Start: Error pages
    InternalErrorURL: "/security/internalerror",
    // End: Error pages

    //#region Schedule Assignment
    GetCreateWeekURL: "/schedule/createweek/",

    GetReferralListForScheduleURL: "/schedule/getreferrallistforschedule/",
    GetRegionWiseWeekFacilityURL: "/schedule/getregionwiseweekfacility/",
    SaveRegionWiseWeekFacilityURL: "/schedule/saveregionwiseweekfacility/",
    GetReferralDetailForPopupURL: "/schedule/getreferraldetailforpopup/",
    GetScheduleMasterListURL: "/schedule/getschedulemasterlist/",
    DeleteScheduleMasterURL: "/schedule/deleteschedulemaster/",
    UpdateScheduleMasterURL: "/schedule/saveschedule/",
    GetFacilutyListForAutoCompleteURL: "/schedule/getfacilutylistforautocomplete/",
    GetScheduleListByFacilityURL: "/schedule/getschedulelistbyfacility/",
    SaveScheduleMasterFromCalenderURL: "/schedule/saveschedulemasterfromcalender/",
    ReScheduleClientURL: "/schedule/reschedule/",
    RemoveScheduleFromCalenderURL: "/schedule/removeschedulefromcalender/",
    RemoveSchedulesFromWeekFacilityURL: "/schedule/removeschedulesfromWeekfacility/",
    LoadAllFacilityByRegion: "/schedule/loadallfacilitybyregion/",
    SendParentEmailURL: "/schedule/sendparentemail/",
    SendParentSMSURL: "/schedule/sendparentsms/",
    GetEmailDetailURL: "/schedule/getemaildetail/",
    GetScheduleNotificationLogsURL: "/schedule/getschedulenotificationlogs/",
    GetSMSDetailURL: "/schedule/getsmsdetail/",
    SaveScheduleBatchServiceURL: "/schedule/saveschedulebatchservice/",
    PrintNoticeScheduleNotificationURL: "/schedule/printnoticeschedulenotification",

    //#endregion Schedule Assignment

    // manage TransportationLocation
    TransPortationModelListURL: "/transportlocation/addtransportlocation/",
    GetTransPortationListURL: "/transportlocation/transportlocationlist/",
    GetTransportationList: "/transportlocation/gettransportatlocationlist",
    DeleteTransportation: "/transportlocation/deletetransportlocationlist",
    CommonUploadFileUrl: "/security/uploadfile",
    UploadProfileImageUrls: "/security/UploadProfileImage",

    //end

    //TransportationGrop
    GetReferralListForTransportationAssignmentURL: "/transportationgroup/getreferrallistfortransportationassignment",
    SaveTransportationGroupURL: "/transportationgroup/savetransportationgroup",
    RemoveTransportationGroupURL: "/transportationgroup/removetransportationgroup",
    RemoveTransportationGroupClientURL: "/transportationgroup/removetransportationgroupclient",
    GetAssignedClientListForTransportationAssignmentURL: "/transportationgroup/getassignedclientlistfortransportationassignment",
    SaveTransportationGroupClientURL: "/transportationgroup/savetransportationgroupclient",
    SaveTransportationGroupFilterURL: "/transportationgroup/savetransportationgroupfilter",
    //end

    //Email Service 
    UpdateScheduleCancelstatus: "/schedule/updateschedulecancelstatus",
    //End

    AddReferralPageUrl: "/referral/addreferral/",

    //Paryor
    AddPayorDetailURL: "/payor/addpayordetail",
    GetServiceCodeMappingList: "/payor/getservicecodemappinglist",
    AddServiceCodeDetail: "/payor/addservicecodedetail",
    GetPayorList: "/payor/getpayorlist",
    DeletePayorList: "/payor/deletepayorlist",
    SetAddPayorPage: "/payor/setaddpayorpage/",
    SetAddPayorPageURL: "/payor/addpayor/", //end

    EditServiceCodeURL: "/payor/editservicecodemapping/",
    DeleteServiceCodeURL: "/payor/deleteservicecodemapping",


    //Service Code 
    SetAddServiceCodePageURL: "/servicecode/addservicecode/",
    SetServiceCodeList: "/servicecode/servicecodelist",
    GetServiceCodeList: "/servicecode/getservicecodelist",


    GetAttendanceListByFacilityURL: "/attendance/getattendancelistbyfacility/",
    UpdateAttendanceURL: "/attendance/updateattendance/",
    UpdateCommentAttendanceURL: "/attendance/updatecommentforattendance/",
    SetAddNotePageURL: "/note/setaddnote",
    GetDTRDetailsURL: "/note/getdtrdetails/",
    SaveNoteURL: "/note/savenote",
    SaveMultiNoteURL: "/note/savemultinote",
    SaveGroupNoteURL: "/note/savegroupnote",
    GetServiceCodesURL: "/note/getservicecodes",
    GetCoreServiceCodesListURL: "/note/getservicecodelist",
    GetGroupPageServiceCodesURL: "/note/getgrouppageservicecodes",
    GetBatchURL: "/note/getbatches",
    GetReferralInfoURL: "/note/getreferralinfo",
    GetPosCodesURL: "/note/getposcodes",
    GetAutoCreateServiceInformationURL: "/note/getautocreateserviceinformation",
    DeleteNoteURL: "/note/deletenote",
    SearchClientForNoteURL: "/note/searchclientfornote",
    SearchNoteForChangeServiceCodeURL: "/note/searchnoteforchangeservicecode",
    ValidateChangeServiceCodeURL: "/note/validatechangeservicecode",
    ReplaceServiceCodeURL: "/note/replaceservicecode",
    GetReferralSiblingURL: "/referral/getreferralinfo/",

    //Dashbord
    ReferralInternalMessageList: "/home/referralinternalmessagelist",
    GetReferralSparFormandCheckList: "/home/getReferralsparformandchecklist",
    GetReferralMissingDocumentList: "/home/getreferralmissingdocumentlist",
    GetReferralInternalMissingDocumentList: "/home/getreferralinternalmissingdocumentlist",
    GetReferralMissingDocument: "/home/getreferralmissingdocument/",
    GetReferralInternalMissingDocument: "/home/getreferralinternalmissingdocument/",
    GetReferralResolvedInternalMessageURL: "/home/getreferralresolvedinternalmessagelist/",
    GetReferralAnsellCaseyReviewURL: "/home/getreferralansellcaseyreviewlist/",
    GetReferralAssignedNotesReviewURL: "/home/getreferralassignednotesreviewlist/",

    //EmailTemplate
    AddEmailTemplate: "/emailtemplate/addemailtemplate/",
    EmailTemplateList: "/emailtemplate/emailtemplatelist",
    GetEmailTemplateList: "/emailtemplate/getemailtemplatelist/",
    DeleteEmailTemplateList: "/emailtemplate/DeleteEmailTemplateList/",

    //Batch
    AddBatchDetailURL: "/batch/addbatchdetail",
    GetApprovedPayorsList: "/batch/getapprovedpayorslist/",
    GetBatchList: "/batch/getbatchlist/",
    DeleteBatch: "/batch/deletebatch/",
    MarkasSentBatch: "/batch/markassentbatch/",
    ValidateBatches: "/batch/validatebatches/",
    GenerateEdi837Files: "/batch/generateedi837files/",
    GenrateOverViewFile: "/batch/downloadoverviewFile/",
    GenratePaperRemitsEOBTemplate: "/batch/genratepaperremitseobtemplate/",






    //Batch Upload 835 Files
    Upload835FileListURL: "/batch/getupload835filelist/",
    DeleteUpload835FileUrl: "/batch/deleteupload835file/",
    ProcessUpload835FileUrl: "/batch/processupload835file/",
    SaveUpload835FileUrl: "/batch/saveupload835file/",
    SaveUpload835CommentUrl: "/batch/saveupload835comment/",


    GetUpload835FilesURL: "/batch/getupload835files/",
    Reconcile835ListURL: "/batch/getreconcile835list/",
    GetExportReconcile835ListListURL: "/batch/exportreconcile835list",
    GetReconcileBatchNoteDetailsUrl: "/batch/getreconcilebatchnotedetails/",
    MarkNoteAsLatestUrl: "/hc/batch/marknoteaslatest/",
    SetClaimAdjustmentFlagUrl: "/batch/setclaimadjustmentflag/",
    BulkSetClaimAdjustmentFlagUrl: "/batch/bulksetclaimadjustmentflag/",

    ReconcileListURL: "/batch/GetParentReconcileList/",
    GetChildReconcileListUrl: "/batch/GetChildReconcileList/",
    MarkNoteAsLatest01Url: "/batch/marknoteaslatest01/",
    GetExportReconcileListURL: "/batch/GetExportReconcileList",


    //EdiFilesLog
    GetEdiFileLogList: "/batch/getedifileloglist/",
    DeleteEdiFileLog: "/batch/deleteedifilelog/",


    //Reports

    GetAttendanceReportUrl: "/report/getattendancereport/",
    GetBehaviourContractReportUrl: "/report/getbehaviourcontractreport/",
    GetClientInformationReportUrl: "/report/getclientinformationreport/",
    GetClientStatusReportUrl: "/report/getclientstatusreport/",
    GetRequestClientListReportUrl: "/report/getrequestclientlistreport/",
    GetInternalServicePlanReportUrl: "/report/getinternalserviceplanreport/",
    GetReferralDetailsReportUrl: "/report/getreferraldetailsreport/",
    GetRespiteUsageReportUrl: "/report/getrespiteusagereport/",
    GetReferralInfoforReportURL: "/report/getreferralinfo",
    GetEncounterPrintReportUrl: "/report/getencounterprintreport",
    GetSnapshotPrintReportUrl: "/report/getsnapshotprintreport",
    GetEDTRPrintReportUrl: "/report/getdtrprintreport",
    GetGeneralNoticeReportUrl: "/report/getgeneralnoticereport",
    GetDspRosterReportUrl: "/report/getdsprosterreport",
    GetScheduleAttendanceScheduleReportUrl: "/report/scheduleattendanceschedulereport",
    GetRequiredDocsForAttendanceReportUrl: "/report/getrequireddocsforattendancereport",
    GetLifeSkillsOutcomeMeasurementsReportURL: "/report/getlifeskillsoutcomemeasurementsreport",
    GetLSTeamMemberCaseloadReportURL: "/report/getlsteammembercaseloadreport",
    GetBXContractStatusReportURL: "/report/getbxcontractstatusreport",
    GetLSTMCaseloadListURL: "/report/getlsteammembercaseloadlist",
    SaveReferralLSTMCaseloadsCommentURL: "/report/savereferrallstmcaseloadscomment",
    GetBillingSummaryReportUrl: "/report/getbillingsummarylist/",
    //Agency
    AddAgencyURL: "/agency/addagency/",
    GetAgencyList: "/agency/getAgencylist/",
    GetAgencyListPageURL: "/agency/agencylist",
    DeleteAgencyListUrl: "/agency/deleteagencylist",

    //GetScheduleBatchServiceList
    GetScheduleBatchServiceListURL: "/schedule/getschedulebatchservicelist",
    DeleteScheduleBatchServiceURL: "/schedule/deleteschedulebatchservice",

    GetReferralReviewAssessmentURL: "/referral/setreferralreviewassessment",
    SaveReferralReviewAssessmentURL: "/referral/savereferralreviewassessment",
    GetReferralReviewAssessmentList: "/referral/getreferralreviewassessmentlist",
    DeleteReferralReviewAssessmentUrl: "/referral/deletereferralreviewassessment",

    GetReferralOutcomeMeasurementURL: "/referral/setreferraloutcomemeasurement",
    SaveReferralOutcomeMeasurementURL: "/referral/savereferraloutcomemeasurement",
    GetReferralOutcomeMeasurementList: "/referral/getreferraloutcomemeasurementlist",
    DeleteReferralOutcomeMeasurementUrl: "/referral/deletereferraloutcomemeasurement",
    UploadAssessmentResult: "/referral/uploadassessmentresult",


    //Referral Monthly Summary

    //GetReferralMonthlySummaryURL: "/referral/getreferralmonthlysummarylist",
    GetSetReferralMonthlySummaryURL: "/referral/setreferralmonthlysummary",
    SearchClientForMonthlySummary: "/referral/searchclientformonthlysummary",
    SaveMultipleMonthlySummaryURL: "/referral/savemultiplemonthlysummary",



    SaveReferralMonthlySummaryURL: "/referral/savereferralmonthlysummary",
    GetReferralMonthlySummaryListURL: "/referral/getreferralmonthlysummarylist",
    FindScheduleWithFaciltyAndServiceDateURL: "/referral/findschedulewithfaciltyandserviceDate",
    GetFacilityListURL: "/referral/getfacilitylist",
    DeleteReferralMonthlySummaryURL: "/referral/deletereferralmonthlysummary",




    //270-271 Sections
    Generate270FileUrl: "/batch/generate270file",
    GetEdi270FileListUrl: "/batch/getedi270filelist",
    DeleteEdi270FileUrl: "/batch/deleteedi270file",
    Upload271FileUrl: "/batch/upload271file/",
    GetEdi271FileListUrl: "/batch/getedi271filelist",
    DeleteEdi271FileUrl: "/batch/deleteedi271file",


    Upload277FileUrl: "/batch/upload277file/",
    GetEdi277FileListUrl: "/batch/getedi277filelist",
    DeleteEdi277FileUrl: "/batch/deleteedi277file",
    Download277RedableFileUrl: "/batch/download277redablefile"


};




var HomeCareSiteUrl =
{
    CheckDxCode: "/hc/referral/CheckDxCode",
    CertificateDownloadURL: "/hc/referral/Download",
    GetReportMasterListUrl: "/hc/report/ReportMasterList/",
    AddReferralPageUrl: "/hc/referral/addreferral/",
    PartialAddReferralURL: "/hc/referral/PartialddReferral/",
    GetDXCodeListForAutoCompleteURLs: "/hc/referral/GetDxcodeListForAutoComplete/",
    SaveDxCodeURLs: "/hc/referral/SaveDxCode/",
    ReferralCertificateListURL: "/hc/referral/ReferralCertificateList",
    SaveRefrallCertificationURL: "/hc/referral/SaveRefrallCertification",
    DeleteEmployeeCertificationURL: "/hc/referral/DeleteEmployeeCertification",
    CertificateAuthorityURL: "/hc/referral/CertificateAuthority",
    UploadCertificateURL: "/hc/referral/UploadCertificate",
    GetPayorIdentificationNumberURL: "/hc/referral/GetPayorIdentificationNumber",
    GetPayorDetailsURL: "/hc/referral/GetPayorDetails",

    SetReferralInternalMessageListURL: "/hc/referral/setreferralinternalmessage/",
    SaveReferralInternalMessageURL: "/hc/referral/saveReferralInternalmessage/",
    DeleteReferralInternalMessageURL: "/hc/referral/deletereferralinternalmessage/",
    ResolveReferralInternalMessageURL: "/hc/referral/resolvereferralinternalmessage/",
    MarkResolvedMessageAsReadURL: "/hc/home/markresolvedmessageasread/",
    DeleteDeviationNoteURL: "/hc/report/DeleteDeviationNote/",
    SearhRegionURL: "/hc/referral/searchregion",
    UpdateAccountURL: "/hc/referral/updateaccount/",
    SaveReferralNoteURL: "/hc/referral/savereferralnote/",
    GetReferralNotesURL: "/hc/referral/GetReferralNotes",
    GetReferralNotesByModelURL: "/hc/referral/GetReferralNotesByModel",
    GetReferralEmployeeURL: "/hc/referral/GetReferralEmployee",
    DeleteReferralNoteURL: "/hc/referral/DeleteReferralNote",
    GetReferralRoleURL: "/hc/referral/GetReferralRole",
    SaveMedicationURL: "/hc/referral/Medication/",
    SaveReferralMedicationURL: "/hc/referral/SaveReferralMedication/",
    GetReferralMedicationURL: "/hc/referral/GetReferralMedications/",
    DeleteReferralMedicationURL: "/hc/referral/DeleteReferralMedication/",
    SearchReferralMedicationsURL: "/hc/referral/SearchReferralMedications/",
    PrioAuthorizationURL: "/hc/report/PrioAuthorization/",


    EditReferralMedicationURL: "/hc/referral/EditReferralMedication",

    ReferralListURL: "/hc/referral/referrallist",
    GetReferralListURL: "/hc/referral/getreferrallist",
    GetReferralAuthorizationsDetails: "/hc/referral/getreferralauthorizationsdetails",
    GetReferralDetailsURL: "/hc/referral/getreferraldetails",
    DeleteReferralURL: "/hc/referral/deletereferral",
    ReferralStatusUpdateURL: "/hc/referral/savereferralstatus",
    UpdateAssigneeURL: "/hc/referral/updateassignee",
    UpdateAssigneeBulkURL: "/hc/referral/updateassigneebulk",
    UpdateStatusURL: "/hc/referral/updatestatus",
    UpdateBulkStatusURL: "/hc/referral/updatebulkstatus",
    UpdateTimeSlotDetailEmployeeURL: "/hc/referral/UpdateTimeSlotDetailEmployee",
    DeleteReferralPreferenceURL: "/hc/referral/DeletePreference",
    ReferralAddSSNLog: "/hc/referral/AddSSNLog/",


    GetReferralHistoryURL: "/hc/referral/GetReferralHistory",
    SaveReferralHistoryItemURL: "/hc/referral/SaveReferralHistoryItem",
    DeleteReferralHistoryItemURL: "/hc/referral/DeleteReferralHistoryItem",

    AddVisitTaskURL: "/hc/visittask/addvisittask/",
    PartialAddVisitTaskURL: "/hc/visittask/PartialAddVisitTask/",
    GetAddVisitTaskURL: "/hc/visittask/getaddvisittask/",
    GetOrgFormListURL: "/hc/visittask/getorganizationformlist",
    MapSelectedFormURL: "/hc/visittask/mapselectedforms",
    GetTaskFormListURL: "/hc/visittask/gettaskformlist",
    VisitTaskFormEditComplianceURL: "/hc/visittask/visittaskformeditcompliance",
    OnFormCheckedURL: "/hc/visittask/onformchecked",
    DeleteMappedFormURL: "/hc/visittask/deletemappedform",
    VisitTaskListURL: "/hc/visittask/visittasklist",
    SaveCloneTaskURL: "/hc/visittask/SaveCloneTask",
    GetVisitTaskList: "/hc/visittask/getvisittasklist",
    DeleteVisitTask: "/hc/visittask/deletevisittask",
    GetVisitTaskCategoryURL: "/hc/visittask/GetVisitTaskCategory",
    GetVisitTaskSubCategoryURL: "/hc/visittask/GetVisitTaskSubCategory",
    GetModelCategoryListURL: "/hc/visittask/GetModelCategoryList",
    SaveCategoryURL: "/hc/visittask/SaveCategory",
    SaveSubCategoryURL: "/hc/visittask/SaveSubCategory",
    BulkUpdateVisitTaskDetail: "/hc/visittask/BulkUpdateVisitTaskDetail",

    AddEmployeeURL: "/hc/employee/addemployee/",
    AddEmployee11URL: "/hc/employee/addemployee11/",
    PartialAddEmployeeURL: "/hc/employee/PartialAddEmployee/",
    EmployeeAddSSNLog: "/hc/employee/AddSSNLog/",
    SaveRegularHours: "/hc/employee/SaveRegularHours/",


    ResendMail: "/hc/employee/ResendRegistrationMail/",
    ResendSMS: "/hc/employee/ResendRegistrationSMS/",
    SearhSkillURL: "/hc/employee/SearchSkill",
    EmployeelistURL: "/hc/employee/employeelist",
    //AddPreferenceURL: "/hc/employee/AddPreference",
    GetEmployeeListURL: "/hc/employee/getemployeelist",
    DeleteEmployeeURL: "/hc/employee/DeleteEmployee",
    DeletePreferenceURL: "/hc/employee/DeletePreference",
    PartialAddGeneralMasterURL: "/hc/generalmaster/PartialGeneralMasterDetail/",
    AddGeneralMasterURL: "/hc/generalmaster/GeneralMasterDetailAPI/",

    GetEmployeeChecklistURL: "/hc/employee/getemployeechecklist",
    SaveEmployeeChecklistURL: "/hc/employee/saveemployeechecklist",

    GetEmployeeNotificationPrefsURL: "/hc/employee/getemployeenotificationprefs",
    SaveEmployeeNotificationPrefsURL: "/hc/employee/saveemployeenotificationprefs",

    UploadEmployeeDocumentURL: "/hc/employee/uploademployeedocument/",
    GetEmployeeDocumentList: "/hc/employee/getemployeedocumentlist/",
    DeleteEmployeeDocumentURL: "/hc/employee/deleteemployeedocument/",
    SaveEmployeeDocumentURL: "/hc/employee/saveemployeedocument/",

    SaveEmployeeNoteURL: "/hc/Employee/saveEmployeenote/",
    GetEmployeeNotesURL: "/hc/Employee/GetEmployeeNotes",
    DeleteEmployeeNoteURL: "/hc/Employee/DeleteEmployeeNote",
    SaveEmployeeEmailSignature: "/hc/Employee/SaveEmployeeEmailSignature",
    GetEmployeeEmailSignature: "/hc/Employee/GetEmployeeEmailSignature",
    LogoutURL: "/security/logout",
    SaveSetPasswordUrl: "/security/savesetpassword",

    GetETSMasterListURL: "/hc/employee/getetsmasterlist",
    DeleteETSMasterURL: "/hc/employee/deleteetsmaster",
    AddEtsMasterURL: "/hc/employee/addetsmaster",

    GetETSDetailListURL: "/hc/employee/getetsdetaillist",
    DeleteETSDetailURL: "/hc/employee/deleteetsdetail",
    AddEtsDetailURL: "/hc/employee/addetsdetail",
    AddEtsDetailBulkURL: "/hc/employee/AddEtsDetailBulk",
    UploadFileURL: "/hc/employee/uploadfile",
    EmployeeTimeSlotForceUpdateURL: "/hc/employee/employeetimeslotforceupdate",

    AddMissingTimeASheet: "/hc/report/AddMissingTimeASheet",
    GetMissingTimeSheet: "/hc/report/getMissingTimeSheet",
    GetGroupTimesheetList: "/hc/report/GetGroupTimesheetList",
    SaveGroupTimesheetList: "/hc/report/SaveGroupTimesheetList",
    SaveReferralActivity: "/hc/report/SaveReferralActivity",
    AddReferralActivityNotes: "/hc/report/AddReferralActivityNotes",
    EditDeleteReferralActivityNotes: "/hc/report/EditDeleteReferralActivityNotes",
    GetReferralNotes: "/hc/report/GetReferralNotes",
    GetReferral: "/hc/report/GetReferral",
    GetReferrals: "/hc/report/GetReferrals",
    GetEmployeeVisitList: "/hc/report/getemployeevisitlist",
    GetVisitApprovalList: "/hc/report/getvisitapprovallist",
    ApproveVisitList: "/hc/report/approvevisitlist",
    GetNurseSignatureList: "/hc/report/getnursesignaturelist",
    NurseSignature: "/hc/report/nursesignature",
    DeleteEmployeeVisit: "/hc/report/deleteemployeevisit",
    MarkAsCompleteEmployeeVisit: "/hc/report/markascompleteemployeevisit",
    GetEmployeeVisitNoteList: "/hc/report/getemployeevisitnotelist",
    GetVisitTaskDocumentList: "/hc/report/getvisittaskdocumentlist",
    GetEmployeeVisitConclusionList: "/hc/report/getemployeevisitconclusionlist",
    ChangeConclusionAnswer: "/hc/report/changeconclusionanswer",
    DeleteEmployeeVisitNote: "/hc/report/deleteemployeevisitnote",
    SaveEmployeeVisit: "/hc/report/saveemployeevisit",
    SavePCAComplete: "/hc/report/savepcacomplete",
    SaveEmployeeVisitPayer: "/hc/report/SaveEmployeeVisitPayer",
    UpdateEmployeeVisitPayorAutherizationCode: "/hc/report/updateemployeevisitpayorautherizationcode",
    GetGroupVisitTask: "/hc/report/getgroupvisittask",
    GetGroupVisitTaskOptionList: "/hc/report/GetGroupVisitTaskOptionList",
    GetMappedVisitTask: "/hc/report/getmappedvisittask",
    GetMappedVisitConclusion: "/hc/report/getmappedvisitconclusion",
    GetMappedVisitTaskForms: "/hc/report/getmappedvisittaskforms",
    SaveVisitNoteOrbeonForm: "/hc/report/savevisitnoteorbeonform",
    DeleteVisitNoteForm: "/hc/report/deletevisitnoteform",
    CategoriesListURL: "/hc/report/CategoryList",
    BulkUpdateVisitReportURL: "/hc/report/BulkUpdateVisitReport",
    SaveVisitNote: "/hc/report/savevisitnote",
    SaveVisitNoteTimeSheet: "/hc/report/savevisitnotetimesheet",
    GenerateBillingNote: "/hc/report/GenerateBillingNote",
    SaveVisitConclusion: "/hc/report/savevisitconclusion",

    BypassActionTaken: "/hc/report/bypassactiontaken",
    SaveDeviationNotesURL: "/hc/report/SaveDeviationNotes",
    GetDeviationNotesURL: "/hc/report/GetDeviationNotes",
    SaveByPassReasonNoteURL: "/hc/report/SaveByPassReasonNote",
    EmployeeBillingReportListURL: "/hc/report/getemployeebillingreportlist",
    PatientTotalReportListURL: "/hc/report/GetPatientTotalReportList",
    ExportToCSV: "/hc/report/Export",
    //Service Code 
    SetAddServiceCodePageURL: "/hc/servicecode/addservicecode/",
    SetPartialAddServiceCodePageURL: "/hc/servicecode/PartialAddServiceCode/",
    SetGetServiceCodePageURL: "/hc/servicecode/getservicecode/",
    SetServiceCodeList: "/hc/servicecode/servicecodelist",
    GetServiceCodeList: "/hc/servicecode/getservicecodelist",
    GetModifierListURL: "/hc/servicecode/getmodifierlist",
    GetModifierByServiceCodeListURL: "/hc/servicecode/getmodifierbyservicecode",
    SaveModifierURL: "/hc/servicecode/savemodifier",
    DeleteModifierURL: "/hc/servicecode/deletemodifier",
    DeleteServiceCode: "/hc/servicecode/deleteservicecode",
    //#region Schedule Assignment

    GetReferralListForScheduleURL: "/hc/schedule/getreferrallistforschedule/",
    GetReferralDetailForPopupURL: "/hc/schedule/getreferraldetailforpopup/",
    GetEmployeesForSchedulingURL: "/hc/schedule/getemployeesforscheduling/",
    GetScheduleListByEmployeesURL: "/hc/schedule/getschedulelistbyemployees/",
    RemoveScheduleFromCalenderURL: "/hc/schedule/removeschedulefromcalender/",
    UpdateScheduleMasterURL: "/hc/schedule/saveschedule/",
    PrivateDuty_UpdateScheduleMasterURL: "/hc/schedule/privateduty_saveschedule/",
    ReScheduleClientURL: "/hc/schedule/reschedule/",
    GetEmployeeMatchingPreferencesURL: "/hc/schedule/getemployeematchingpreferences/",
    PrivateDuty_GetEmployeeMatchingPreferencesURL: "/hc/schedule/privateduty_getemployeematchingpreferences/",

    SaveNewScheduleURL: "/hc/schedule/SaveNewSchedule",
    GetVisitReasonListURL: "/hc/schedule/GetVisitReasonList",
    SaveVisitReasonURL: "/hc/schedule/SaveVisitReason",
    GetVisitReasonModalDetailURL: "/hc/schedule/GetVisitReasonModalDetail",

    SaveReferralComplianceURL: "/hc/referral/savereferralcompliance",
    ChangeSortingOrderURL: "/hc/compliance/ChangeSortingOrder",

    SaveScheduleMasterFromCalenderURL: "/hc/schedule/saveschedulemasterfromcalender/",

    GetSchEmployeeListForScheduleURL: "/hc/schedule/getschemployeelistforschedule/",
    PrivateDuty_GetSchEmployeeListForScheduleURL: "/hc/schedule/privateduty_getschemployeelistforschedule/",
    GetSchEmployeeDetailForPopupURL: "/hc/schedule/getschemployeedetailforpopup/",
    PrivateDuty_GetSchEmployeeDetailForPopupURL: "/hc/schedule/privateduty_getschemployeedetailforpopup/",
    GetReferralForSchedulingURL: "/hc/schedule/getreferralforscheduling/",
    PrivateDuty_GetReferralForSchedulingURL: "/hc/schedule/privateduty_getreferralforscheduling/",
    DayCare_GetReferralForSchedulingURL: "/hc/schedule/daycare_getreferralforscheduling/",
    DayCare_GetScheduledPatientListURL: "/hc/schedule/daycare_getscheduledpatientlist/",
    DayCare_GetRelationTypeListURL: "/hc/schedule/daycare_getrelationtypelist/",
    Daycare_PatientClockInClockOut: "/hc/schedule/daycare_patientclockinclockout/",
    DayCare_GetSchedulePatientTasks: "/hc/schedule/daycare_getschedulepatienttasks/",
    DayCare_GetSchedulePatientTaskOption: "/hc/schedule/DayCare_GetSchedulePatientTaskOption/",

    CaseManagement_GetReferralForSchedulingURL: "/hc/schedule/casemanagement_getreferralforscheduling/",
    GetScheduleListByReferralsURL: "/hc/schedule/getschedulelistbyreferrals/",
    PrivateDuty_GetScheduleListByReferralsURL: "/hc/schedule/privateduty_getschedulelistbyreferrals/",
    DayCare_GetScheduleListByReferralsURL: "/hc/schedule/daycare_getschedulelistbyreferrals/",
    CaseManagement_GetScheduleListByReferralsURL: "/hc/schedule/casemanagement_getschedulelistbyreferrals/",
    HC_GetVirtualVisitsListURL: "/hc/schedule/GetVirtualVisitsList/",
    UploadProfileImageUrls: "/hc/schedule/UploadProfileImage",
    UploadReferralProfileImageUrls: "/hc/referral/UploadReferralProfileImage",
    HC_GetReferralEmployeeVisitsURL:"/hc/schedule/GetReferralEmployeeVisits",
    SavePickUpDropCallURL: "/hc/schedule/SavePickUpDropCall",

    SaveScheduleFromCalenderURL: "/hc/schedule/saveschedulefromcalender/",
    PrivateDuty_SaveScheduleFromCalenderURL: "/hc/schedule/privateduty_saveschedulefromcalender/",


    DeleteScheduleFromCalenderURL: "/hc/schedule/deleteschedulefromcalender/",
    PrivateDuty_DeleteScheduleFromCalenderURL: "/hc/schedule/privateduty_deleteschedulefromcalender/",
    DayCare_DeleteScheduleFromCalenderURL: "/hc/schedule/daycare_deleteschedulefromcalender/",

    GetEmpRefSchPageModelUrl: "/hc/schedule/getemprefschpagemodel/",
    PrivateDuty_GetEmpRefSchPageModelUrl: "/hc/schedule/privateduty_getemprefschpagemodel/",

    GetRCLEmpRefSchOptionsURL: "/hc/schedule/getrclemprefschoptions/",
    GetCareTypesbyPayorID: "/hc/schedule/GetCareTypesbyPayorID/",
    GetEmpRefSchOptionsURL: "/hc/schedule/getemprefschoptions/",

    /*Schedule master OPT*/
    GetEmpRefSchOptions_PatientVisitFrequency_HC: "/hc/schedule/GetEmpRefSchOptions_PatientVisitFrequency_HC/",
    GetEmpRefSchOptions_ClientOnHoldData_HC: "/hc/schedule/GetEmpRefSchOptions_ClientOnHoldData_HC/",
    GetEmpRefSchOptions_ReferralInfo_HC: "/hc/schedule/GetEmpRefSchOptions_ReferralInfo_HC/",
    GetEmpRefSchOptions_ScheduleInfo_HC: "/hc/schedule/GetEmpRefSchOptions_ScheduleInfo_HC/",
    /*Schedule master OPT*/
    GetEmpCareTypeIDURL: "/hc/schedule/getempcaretype/",
    PrivateDuty_GetEmpRefSchOptionsURL: "/hc/schedule/privateduty_getemprefschoptions/",
    DayCare_GetEmpRefSchOptionsURL: "/hc/schedule/daycare_getemprefschoptions/",
    CaseManagement_GetEmpRefSchOptionsURL: "/hc/schedule/casemanagement_getemprefschoptions/",
    CreateBulkScheduleUrl: "/hc/schedule/createbulkschedule/",
    SendScheduleReminderUrl: "/hc/schedule/sendschedulereminder/",
    PrivateDuty_CreateBulkScheduleUrl: "/hc/schedule/privateduty_createbulkschedule/",
    DayCare_CreateBulkScheduleUrl: "/hc/schedule/daycare_createbulkschedule/",
    DeleteEmpRefScheduleURL: "/hc/schedule/deleteemprefschedule/",
    PrivateDuty_DeleteEmpRefScheduleURL: "/hc/schedule/privateduty_deleteemprefschedule/",
    DayCare_DeleteEmpRefScheduleURL: "/hc/schedule/daycare_deleteemprefschedule/",
    PrivateDuty_GetAssignedEmployeesURL: "/hc/schedule/privateduty_getassignedemployees/",
    GetAssignedFacilitiesURL: "/hc/schedule/daycare_getassignedfacilities/",
    OnHoldUnHoldActionURL: "/hc/schedule/onholdunholdaction/",
    PrivateDuty_OnHoldUnHoldActionURL: "/hc/schedule/privateduty_onholdunholdaction/",
    DayCare_OnHoldUnHoldActionURL: "/hc/schedule/daycare_onholdunholdaction/",
    CaseManagement_OnHoldUnHoldActionURL: "/hc/schedule/casemanagement_onholdunholdaction/",
    RemoveScheduleURL: "/hc/schedule/RemoveSchedule/",
    PrivateDuty_RemoveScheduleURL: "/hc/schedule/privateduty_RemoveSchedule/",
    DayCare_RemoveScheduleURL: "/hc/schedule/daycare_removeschedule/",
    DayCare_SavePatientAttendanceURL: "/hc/schedule/daycare_savepatientattendance/",

    GetDirectoryListURL: "/hc/compliance/getdirectorylist",
    SaveComplianceURL: "/hc/compliance/savecompliance/",
    GetAssigneeList: "/hc/compliance/getAssigneeList",
    GetComplianceListURL: "/hc/compliance/getcompliancelist/",
    DeleteComplianceURL: "/hc/compliance/deletecompliance/",
    GetOrganizationFormListURL: "/hc/compliance/getorganizationformlist",

    GetScheduleAggregatorLogsListURL: "/hc/schedule/GetScheduleAggregatorLogsList",
    ResendAggregatorDataURL: "/hc/schedule/ResendAggregatorData",
    GetScheduleAggregatorLogsDetailsURL: "/hc/schedule/GetScheduleAggregatorLogsDetails",

    GetScheduleMasterListURL: "/hc/schedule/getschedulemasterlist/",
    GetDayCare_GetScheduleMasterListURL: "/hc/schedule/daycare_getschedulemasterlist/",

    DeleteScheduleMasterURL: "/hc/schedule/deleteschedulemaster/",
    DayCare_DeleteScheduleMasterURL: "/hc/schedule/daycare_deleteschedulemaster/",
    GetScheduleNotificationLogsURL: "/hc/schedule/getschedulenotificationlogs/",

    GetCreateWeekURL: "/hc/schedule/createweek/",

    GetRegionWiseWeekFacilityURL: "/hc//schedule/getregionwiseweekfacility/",
    SaveRegionWiseWeekFacilityURL: "/hc//schedule/saveregionwiseweekfacility/",

    GetFacilutyListForAutoCompleteURL: "/hc//schedule/getfacilutylistforautocomplete/",
    GetScheduleListByFacilityURL: "/hc//schedule/getschedulelistbyfacility/",

    RemoveSchedulesFromWeekFacilityURL: "/hc//schedule/removeschedulesfromWeekfacility/",
    LoadAllFacilityByRegion: "/hc//schedule/loadallfacilitybyregion/",
    SendParentEmailURL: "/hc/schedule/sendparentemail/",
    SendEmpEmailURL: "/hc/schedule/sendempemail/",
    SendParentSMSURL: "/hc/schedule/sendparentsms/",
    GetEmailDetailURL: "/hc/schedule/getemaildetail/",
    GetEmpEmailDetailURL: "/hc/schedule/getempemaildetail/",

    GetSMSDetailURL: "/hc/schedule/getsmsdetail/",

    GetEmpSMSDetailURL: "/hc/schedule/getempsmsdetail/",
    SendEmpSMSURL: "/hc/schedule/sendempsms/",

    ChecklistGetEmpSMSDetailURL: "/hc/schedule/checklistgetempsmsdetail/",
    ChecklistSendEmpSMSURL: "/hc/schedule/checklistsendempsms/",

    SaveScheduleBatchServiceURL: "/hc/schedule/saveschedulebatchservice/",
    PrintNoticeScheduleNotificationURL: "/hc/schedule/printnoticeschedulenotification",

    GetEmployeesForEmpCalenderURL: "/hc/schedule/getemployeesforempcalender/",
    GetAssignedEmployeesURL: "/hc/schedule/getassignedemployees/",

    GetGeneralMasterList: "/hc/generalmaster/getgeneralmasterlist/",
    DeleteDDMaster: "/hc/generalmaster/deleteddMaster/",
    SaveDDmasterURL: "/hc/generalmaster/saveddmaster/",
    GetDMASForm_90FormListURL: "/hc/report/GetDMASForm_90FormList",
    GetENewDMAS90ListURL: "/hc/report/GetENewDMAS90List",
    GetENewDMAS90ListURLs: "/hc/report/GetENewDMAS90ListNew",
    GetEmployeeByReferralIDURL: "/hc/report/GetEmployeeByReferralID",

    GetVisitTaskListForRef: "/hc/referral/getvisittasklist",
    SaveReferralTaskMappingURL: "/hc/referral/saverefvisittasklist",
    ReferralTaskMappingURL: "/hc/referral/saverefvisittasklist",
    SaveBulkReferralTaskMappingURL: "/hc/referral/SaveBulkRefVisitTaskList",
    GetPatientTaskMappingsURL: "/hc/referral/getpatienttaskmappings",
    GetReferralTaskMappingsURL: "/hc/referral/GetReferralTaskMappingDetails",
    GetReferralTaskMappingsReportURL: "/hc/referral/GetReferralTaskMappingReport",
    GetReferralGoalURL: "/hc/referral/GetReferralGoal",
    UpdateIsActiveIsDeletedReferralGoalURL: "/hc/referral/UpdateIsActiveIsDeletedReferralGoal",
    SaveReferralGoalURL: "/hc/referral/SaveGoalForReferral",

    DeleteRefTaskMappingURL: "/hc/referral/deletereftaskmapping",
    OnTaskCheckedURL: "/hc/referral/ontaskchecked",
    GetBlockEmpListURL: "/hc/referral/getblockemplist",
    SaveBlockEmpURL: "/hc/referral/saveblockemp",
    DeleteBlockEmpURL: "/hc/referral/deleteblockemp",
    GetCaretypesURL: "/hc/referral/GetCaretype",
    GetCarePlanCaretypes: "/hc/referral/GetCarePlanCaretypes",
    ExistanceOfReferralTimeslot: "/hc/referral/ExistanceOfReferralTimeslot",
    GetCarePlanCaretype: "/hc/referral/ReferralTimeSlots",
    AddPreferenceURL: "/hc/preference/addpreference/",
    PartialAddPreferenceURL: "/hc/preference/PartialAddPreference/",
    PreferenceListURL: "/hc/preference/preferencelist",
    GetPreferenceList: "/hc/preference/getpreferencelist",
    DeletePreference: "/hc/preference/deletepreference",

    //#endregion Schedule Assignment

    GetNoteListURL: '/hc/referral/GetNoteCategory',
    GetEmployeeDayOffListURL: '/hc/employee/getemployeedayofflist',
    SaveEmployeeDayOffURL: '/hc/employee/saveemployeedayoff',
    DayOffActionURL: '/hc/employee/dayoffaction',
    CheckForEmpSchedulesURL: '/hc/employee/checkforempschedules',
    DeleteEmployeeDayOffURL: '/hc/employee/deleteemployeedayoff',

    GetRTSMasterListURL: "/hc/referral/getrtsmasterlist",
    GetReferralTimeSlotDetailURL: "/hc/referral/GetReferralTimeSlotDetaillist",
    DeleteRTSMasterURL: "/hc/referral/deletertsmaster",
    AddRTSMasterURL: "/hc/referral/addrtsmaster",
    AddRTSByPriorAuthURL: "/hc/referral/addrtsbypriorauth",
    GetDXcodeListDDURL: "/hc/referral/GetDXcodeListDD",
    DxChangeSortingOrderURL: "/hc/referral/DxChangeSortingOrder",
    SaveDXcodeListDDURL: "/hc/referral/SaveDXcodeListDD",

    GetReferralAuthorizationsByReferralIDUrl: "/hc/referral/getreferralauthorizationsbyreferralid",

    GetRTSDetailListURL: "/hc/referral/getrtsdetaillist",
    DeleteRTSDetailURL: "/hc/referral/deletertsdetail",
    AddRTSDetailURL: "/hc/referral/addrtsdetail",
    UpdateRTSDetailURL: "/hc/referral/updatertsdetail",
    ReferralTimeSlotForceUpdateURL: "/hc/referral/referraltimeslotforceupdate",
    UpdateETSDetailURL: "/hc/employee/updateetsdetail",

    GetChecklistItemTypesURL: "/hc/checklist/getchecklistitemtypes",
    GetChecklistItemsURL: "/hc/checklist/getchecklistitems",
    SaveChecklistItemsURL: "/hc/checklist/savechecklistitems",
    GetIsChecklistRemainingURL: "/hc/checklist/getischecklistremaining",
    GetVisitChecklistItemsURL: "/hc/checklist/getvisitchecklistitems",
    GetVisitChecklistItemDetailURL: "/hc/checklist/getvisitchecklistitemdetail",

    GetRCLMasterListURL: "/hc/referral/getrclmasterlist",
    AddRCLMasterURL: "/hc/referral/addrclmaster",
    MarkCaseLoadCompelteURL: "/hc/referral/markcaseloadcomplete",
    RemoveCaseLoadURL: "/hc/referral/deleterclmaster",

    AddPhysicianURL: "/hc/physician/addphysician/",
    PartialAddPhysicianURL: "/hc/physician/PartialAddPhysician/",
    PhysicianListURL: "/hc/physician/physicianlist",
    GetPhysicianList: "/hc/physician/getphysicianlist",
    DeletePhysician: "/hc/physician/deletephysician",
    GetSpecialistListForAutoCompleteURL: "/hc/physician/GetSpecialistListForAutoComplete/",
    SaveSpecialistURLs: "/hc/physician/SaveSpecialist/",
    AddCategoryURL: "/hc/category/addcategory/",
    CategoryListURL: "/hc/category/categorylist",
    GetCategoryList: "/hc/category/categorylist",
    DeleteCategory: "/hc/category/deletecategory",

    AddMarketURL: "/hc/ebmarkets/addebmarkets/",
    MarketListURL: "/hc/ebmarkets/ebmarketsList",
    GetMarketList: "/hc/ebmarkets/ebmarketsList",
    DeleteMarket: "/hc/ebmarkets/deleteebmarkets",

    AddFormURL: "/hc/ebforms/addebforms/",
    FormListURL: "/hc/ebforms/ebformsList",
    GetFormList: "/hc/ebforms/ebformsList",
    DeleteForm: "/hc/ebforms/deleteebforms",

    GetEmpClockInOutListURL: "/hc/home/getempclockinoutlist",
    GetPatientAddressListURL: "/hc/home/GetPatientAddressList",
    GetEmpOverTimeListURL: "/hc/home/getempovertimelist",
    GetPatientNewListURL: "/hc/home/getpatientnewlist",
    GetActivePatientCountListURL: "/hc/home/GetActivePatientCountList",
    GetPatientNotScheduleListURL: "/hc/home/getpatientnotschedulelist",
    GetEmployeeTimeStaticsUrl: "/hc/home/getemployeetimestatics",
    GetPendingBypassVisitUrl: "/hc/home/getpendingbypassvisit",
    GetWebNotificationListURL: "/hc/home/getwebnotifications",
    DeleteWebNotificationsURL: "/hc/home/deletewebnotifications/",
    DeleteWebNotificationURL: "/hc/home/deletewebnotification/",
    MarkAsReadWebNotificationsURL: "/hc/home/markasreadwebnotifications/",
    GetNotificationsListURL: "/hc/home/getnotificationslist",
    GetPriorAuthExpiringURL: "/hc/home/getpriorauthexpiring/",
    GetPriorAuthExpiredURL: "/hc/home/getpriorauthexpired/",
    GetPatientBirthdayURL: "/hc/home/getpatientbirthday/",
    GetEmployeeBirthdayURL: "/hc/home/getemployeebirthday/",
    GetPatientClockInOutListURL: "/hc/home/getpatientclockinoutlist",

    GetSchEmpRefSkillsUrl: "/hc/schedule/getschemprefskills",
    PrivateDuty_GetSchEmpRefSkillsUrl: "/hc/schedule/privateduty_getschemprefskills",

    ReferralInternalMessageList: "/hc/home/referralinternalmessagelist",
    GetReferralResolvedInternalMessageURL: "/hc/home/getreferralresolvedinternalmessagelist/",

    GetEmployeeListForSendSmsUrl: "/hc/employee/getemployeelistforsendsms/",
    SendBulkSMSUrl: "/hc/employee/sendbulksms/",
    GetSentSmsListUrl: "/hc/employee/getsentsmslist/",
    GetEmployeesForSentSMSUrl: "/hc/employee/getemployeesforsentsms/",

    GeneratePcaTimeSheetPdfURL: "/hc/report/generatepcatimesheetpdf/",

    GetDashboardUrl: "/hc/home/dashboard",
    GetLateClockInNotificationURL: "/hc/home/GetLateClockInNotification",

    FileUplaodURL: "/hc/setting/uploadfile",
    SaveSettingsURL: "/hc/setting/savesettings",
    TestEmailURL:"/hc/setting/TestEmail",

    CommonUploadFileUrl: "/hc/security/uploadfile",

    // PatientTimeSheet
    GetPatientTimeSheetList: "/hc/patienttimesheet/getpatienttimesheetlist",
    GeneratePatientTimeSheetPdfURL: "/hc/patienttimesheet/generatepatienttimesheetpdf/",

    //Start: Role Permission
    RolePermissionsURL: "/hc/security/RolePermissions",
    GetRolePermissionsURL: "/hc/security/getrolepermissions",
    GetRolePermissionURL: "/hc/security/getrolepermission",
    GetMapReportURL: "/hc/security/GetMapReport",
    SaveRoleWisePermissionURL: "/hc/security/saverolewisepermission/",
    SaveMapReportURL: "/hc/security/saveMapReport/",
    SavePermissionURL: "/hc/security/savepermission/",
    UpdateRoleNameURL: "/hc/security/updaterolename/",
    AddNewRoleURL: "/hc/security/addnewrole/",
    //End: Role Permission

    SaveBillingNoteUrl: "/hc/batch/SaveBillingNote/",
    GetBillingNotesUrl: "/hc/batch/GetBillingNotes/",
    DeleteBillingNoteUrl: "/hc/batch/DeleteBillingNote/", 
    UpdateBillingNoteUrl: "/hc/batch/UpdateBillingNotes/",
    
    //Batch Upload 835 Files
    Upload835FileListURL: "/hc/batch/getupload835filelist/",
    DeleteUpload835FileUrl: "/hc/batch/deleteupload835file/",
    ProcessUpload835FileUrl: "/hc/batch/processupload835file/",
    SaveUpload835FileUrl: "/hc/batch/saveupload835file/",
    SaveUpload835CommentUrl: "/hc/batch/saveupload835comment/",

    SaveReferralCSVFileUrl: "/hc/schedule/SaveReferralCSVFile/",

    GetLayoutRelatedDetailsUrl: "/home/getlayoutrelateddetails/",

    AddCaseManagerURL: "/casemanager/addcasemanager/",
    PartialAddCaseManagerURL: "/casemanager/PartialAddCaseManager/",
    CaseManagerListURL: "/casemanager/casemanagerlist",
    GetAgencyLocationListURL: "/casemanager/getagencylocation",
    GetCaseManagerList: "/casemanager/getcasemanagerlist",
    DeleteCaseManager: "/casemanager/deletecasemanager",

    AddParentURL: "/parent/addparent/",
    ParentListURL: "/parent/parentlist",
    GetParentList: "/parent/getparentlist",
    DeleteParent: "/parent/deleteparent",

    AddDepartmentURL: "/department/adddepartment/",
    DepartmentListURL: "/department/departmentlist/",
    GetDepartmentListURL: "/department/getdepartmentlist/",
    DeleteDepartmentURL: "/department/deletedepartment/",
    GetZipCodeListURL: "/department/getzipcodelist/",

    AddFacilityHouseURL: "/hc/facilityhouse/addfacilityhouse/",
    FacilityHouseListURL: "/hc/facilityhouse/facilityhouselist",
    GetFacilityHouseListURL: "/hc/facilityhouse/getfacilityhouselist",
    DeleteFacilityHouseURL: "/hc/facilityhouse/deletefacilityhouse",
    GetParentFacilityHouse: "/hc/facilityhouse/getparentfacilityhouse",
    GetFacilityTransportLocationMappingURL: "/hc/facilityhouse/getfacilitytransportlocationmapping",
    SaveFacilityTransportLocationMappingURL: "/hc/facilityhouse/savefacilitytransportlocationmapping",
    SearhEquipmentURL: "/hc/facilityhouse/searchequipment",
    PartialAddFacilityHouseURL: "/hc/facilityhouse/PartialAddFacilityHouse/",


    AddDxCodeURL: "/hc/dxcode/adddxcode/",
    PartialAddDxCodeURL: "/hc/dxcode/PartialAddDxCode/",
    DxCodeListURL: "/hc/dxcode/dxcodelist",
    GetDxCodeList: "/hc/dxcode/getdxcodelist",
    DeleteDxCode: "/hc/dxcode/deletedxcode",

    AddNoteSentenceURL: "/notesentence/addnotesentence/",
    PartialAddNoteSentenceURL: "/notesentence/PartialAddNoteSentence/",
    NoteSentenceListURL: "/notesentence/notesentencelist",
    GetNoteSentenceList: "/notesentence/getnotesentencelist",
    DeleteNoteSentence: "/notesentence/deletenotesentence",

    UpdateAHCCCSIDURL: "/referral/updateahcccsid/",
    GetAuditLogListURL: "/referral/getauditloglist/",
    GetTableDisplayValue: "/auditlog/gettabledisplayvalue/",
    GetBXContractListURL: "/referral/getbxcontractlist/",
    SaveBXContractURL: "/referral/savebxcontract/",
    UpdateBXContractStatusURL: "/referral/updatebxcontractstatus/",
    SaveSuspensionURL: "/referral/savesuspensiondetails/",
    GetSuspensionURL: "/referral/getsuspensiondetails/",

    SetReferralCheckListURL: "/referral/setreferralchecklist/",
    SaveReferralCheckListURL: "/referral/savereferralchecklist/",

    SetReferralSparFormURL: "/referral/setreferralsparform/",
    SaveReferralSparFormURL: "/referral/savereferralsparform/",

    GetCaseManagersURL: "/referral/getcasemanagerforautocomplete/",
    GetReferralDocumentList: "/hc/referral/getreferraldocumentlist/",
    SaveReferralDocumentURL: "/hc/referral/uploadfile/",
    DeleteDocumentURL: "/hc/referral/DeleteDocument/",
    EditDocumentURL: "/hc/referral/savedocument/",
    EditNewDocumentURL: "/hc/referral/savedocumentnew/",

    GetMasterJurisdictionListUrl: "/hc/referral/getmasterjurisdictionlist/",
    GetMasterTimezoneListUrl: "/hc/referral/getmastertimezonelist/",

    SetReferralMissingDocumentURL: "/hc/referral/setreferralmissingdocument/",
    SendEmailForReferralMissingDocumentURL: "/hc/referral/sendemailforreferralmissingdocument/",

    MarkPayorAsActive: "/referral/markpayorasactive/",
    GetReferralPayorDetail: "/referral/getreferralpayordetail/",
    UpdateReferralPayorInformation: "/referral/updatereferralpayorinformation/",

    // ReferraLlList Page
    SendReceiptNotificationEmailURL: "/referral/sendreceiptnotificationemail/",

    GetNoteListURL: "/note/getnotelist",
    GetExportNoteListURL: "/note/exportnotelist",
    GetNoteClientListURL: "/note/getnoteclientlist",
    ValidateServiceCodeDetailsURL: "/note/validateservicecodedetails",

    GetDXCodeListForAutoCompleteURL: "/hc/referral/getdxcodelistforautocomplete/",
    DeleteReferralDXCodeMappingURL: "/hc/referral/DeleteReferralDxCodeMapping/",
    DeleteReferralSiblingMappingURL: "/hc/referral/ReferralSiblingMappingDelete/",
    DeleteReferralBeneficiaryTypeURL: "/hc/referral/deletereferralbeneficiarytype",
    DeletePhysicianURL: "/hc/referral/deletereferralphysician",
    // End Referral Page

    // ReferralTrackingList Page
    GetReferralTrackingListURL: "/referral/getreferraltrackinglist",
    ReferralTrackingListURL: "/referral/referraltrackinglist",
    DeleteReferralTrackingURL: "/referral/deletereferraltracking",
    ReferralTrackingStatusUpdateURL: "/referral/savereferraltrackingstatus",
    ReferralTrackingCommentUpdateURL: "/referral/savereferraltrackingcomment",
    // End Referral Page

    // Start: Error pages
    InternalErrorURL: "/security/internalerror",
    // End: Error pages

    // manage TransportationLocation
    TransPortationModelListURL: "/transportlocation/addtransportlocation/",
    GetTransPortationListURL: "/transportlocation/transportlocationlist/",
    GetTransportationList: "/transportlocation/gettransportatlocationlist",
    DeleteTransportation: "/transportlocation/deletetransportatlocationlist",
    //end

    //TransportationGrop
    GetReferralListForTransportationAssignmentURL: "/transportationgroup/getreferrallistfortransportationassignment",
    SaveTransportationGroupURL: "/transportationgroup/savetransportationgroup",
    RemoveTransportationGroupURL: "/transportationgroup/removetransportationgroup",
    RemoveTransportationGroupClientURL: "/transportationgroup/removetransportationgroupclient",
    GetAssignedClientListForTransportationAssignmentURL: "/transportationgroup/getassignedclientlistfortransportationassignment",
    SaveTransportationGroupClientURL: "/transportationgroup/savetransportationgroupclient",
    SaveTransportationGroupFilterURL: "/transportationgroup/savetransportationgroupfilter",
    //end

    //Email Service 
    UpdateScheduleCancelstatus: "/schedule/updateschedulecancelstatus",
    //End

    //Paryor
    GetServiceCodeMappingList: "/hc/payor/getservicecodemappinglist",
    AddServiceCodeDetail: "/hc/payor/addservicecodedetail",
    DeletePayorList: "/hc/payor/deletepayorlist",
    GetServiceCodeMappingListNew: "/hc/payor/getservicecodemappinglistnew",
    AddServiceCodeDetailNew: "/hc/payor/addservicecodedetailnew",
    DeleteServiceCodeURLNew: "/hc/payor/deleteservicecodemappingnew",

    GetPayorList: "/hc/payor/getpayorlist",
    AddPayorDetailURL: "/hc/payor/addpayordetail",
    SavePayorBillingSettingURL: "/hc/payor/savepayorbillingsetting",
    GetPayorBillingSetting: "/hc/payor/getpayorbillingsetting",
    SetAddPayorPage: "/hc/payor/setaddpayorpage/",
    SetAddPayorPageURL: "/hc/payor/addpayor/",
    SearchPayorPageURL: "/hc/payor/searchpayor/",
    PayorEnrollmentURL: "/hc/payor/PayorEnrollment/",
    SetPartialAddPayorPageURL: "/hc/payor/PartialAddPayor/",

    GetAllBillingSettings: "/hc/payor/GetAllBillingSettings",
    SaveBillingSetting: "/hc/payor/SaveBillingSetting",

    GetServiceCodeListURL: "/hc/payor/getServicecodeList/",
    //end

    EditServiceCodeURL: "/hc/payor/editservicecodemapping/",
    DeleteServiceCodeURL: "/hc/payor/deleteservicecodemapping",

    GetAttendanceListByFacilityURL: "/attendance/getattendancelistbyfacility/",
    UpdateAttendanceURL: "/attendance/updateattendance/",
    UpdateCommentAttendanceURL: "/attendance/updatecommentforattendance/",
    SetAddNotePageURL: "/note/setaddnote",
    GetDTRDetailsURL: "/note/getdtrdetails/",
    SaveNoteURL: "/note/savenote",
    SaveMultiNoteURL: "/note/savemultinote",
    SaveGroupNoteURL: "/note/savegroupnote",
    GetServiceCodesURL: "/note/getservicecodes",
    GetCoreServiceCodesListURL: "/note/getservicecodelist",
    GetGroupPageServiceCodesURL: "/note/getgrouppageservicecodes",
    GetBatchURL: "/note/getbatches",
    GetReferralInfoURL: "/note/getreferralinfo",
    GetPosCodesURL: "/note/getposcodes",
    GetAutoCreateServiceInformationURL: "/note/getautocreateserviceinformation",
    DeleteNoteURL: "/note/deletenote",
    SearchClientForNoteURL: "/note/searchclientfornote",
    SearchNoteForChangeServiceCodeURL: "/note/searchnoteforchangeservicecode",
    ValidateChangeServiceCodeURL: "/note/validatechangeservicecode",
    ReplaceServiceCodeURL: "/note/replaceservicecode",
    GetReferralSiblingURL: "/referral/getreferralinfo/",

    //Dashbord

    GetReferralSparFormandCheckList: "/home/getReferralsparformandchecklist",
    GetReferralMissingDocumentList: "/home/getreferralmissingdocumentlist",
    GetReferralInternalMissingDocumentList: "/home/getreferralinternalmissingdocumentlist",
    GetReferralMissingDocument: "/home/getreferralmissingdocument/",
    GetReferralInternalMissingDocument: "/home/getreferralinternalmissingdocument/",

    GetReferralAnsellCaseyReviewURL: "/home/getreferralansellcaseyreviewlist/",
    GetReferralAssignedNotesReviewURL: "/home/getreferralassignednotesreviewlist/",

    //EmailTemplate
    AddEmailTemplate: "/emailtemplate/addemailtemplate/",
    EmailTemplateList: "/emailtemplate/emailtemplatelist",
    GetEmailTemplateList: "/emailtemplate/getemailtemplatelist/",
    DeleteEmailTemplateList: "/emailtemplate/deleteemailtemplatelist/",

    //Batch
    SyncClaimMessagesURL: "/hc/batch/SyncClaimMessages",
    GetClaimMessageListURL: "/hc/batch/GetClaimMessageList",
    GetPatientListURl: "/hc/batch/getpatientlist",
    GetChildNoteDetailsURL: "/hc/batch/getchildnotedetails",
    GetBatchDetailsURL: "/hc/batch/getbatchdetails",

    GetBatchListURL: "/hc/batch/getbatchlist",
    MarkasSentBatchURL: "/hc/batch/markassentbatch",
    DeleteBatchURL: "/hc/batch/deletebatch",

    AddBatchDetailURL: "/hc/batch/addbatchdetail",
    RefreshAndGroupingNotesServiceURL: "/hc/batch/refreshandgroupingnotes",

    ValidateBatchesURL: "/hc/batch/validatebatches/",
    GenerateEdi837FilesURL: "/hc/batch/generateedi837files/",

    GenrateOverViewFileURL: "/hc/batch/downloadoverviewFile/",
    GenratePaperRemitsEOBTemplateURL: "/hc/batch/genratepaperremitseobtemplate/",

    GetApprovedPayorsList: "/batch/getapprovedpayorslist/",

    GetAuthorizationLoadModelURL: "/hc/referral/getauthorizationloadmodel",
    GetPayorMappedServiceCodeListURL: "/hc/referral/getpayormappedservicecodelist",
    GetPayorMappedServiceCodesURL: "/hc/referral/getpayormappedservicecodes",
    GetAuthServiceCodesURL: "/hc/referral/getauthservicecodes",


    GetReferralBillingSettingURL: "/hc/referral/getreferralbillingsetting",
    AddReferralBillingSettingURL: "/hc/referral/addreferralbillingsetting",

    GetUpload835FilesURL: "/hc/batch/getupload835files/",
    Reconcile835ListURL: "/hc/batch/getreconcile835list/",
    GetExportReconcile835ListListURL: "/hc/batch/exportreconcile835list",
    GetReconcileBatchNoteDetailsUrl: "/hc/batch/getreconcilebatchnotedetails/",
    MarkNoteAsLatestUrl: "/hc/batch/marknoteaslatest/",
    SetClaimAdjustmentFlagUrl: "/hc/batch/setclaimadjustmentflag/",
    BulkSetClaimAdjustmentFlagUrl: "/hc/batch/bulksetclaimadjustmentflag/",

    //ReconcileListURL: "/hc/batch/GetParentReconcileList/",
    ReconcileListURL: "/hc/batch/GetReconcileList/",
    ParentReconcileListURL: "/hc/batch/GetParentReconcileList/",
    GetChildReconcileListUrl: "/hc/batch/GetChildReconcileList/",
    MarkNoteAsLatest01Url: "/hc/batch/marknoteaslatest01/",
    GetExportReconcileListURL: "/hc/batch/GetExportReconcileList",

    AddReferralBillingAuthorization: "/hc/referral/AddReferralBillingAuthorization",
    GetAuthorizationLinkupListURL: "/hc/referral/GetAuthorizationLinkupList",
    GetAuthorizationScheduleLinkListURL: "/hc/referral/GetAuthorizationScheduleLinkList",
    UpdateAuthorizationLinkupURL: "/hc/referral/UpdateAuthorizationLinkup",
    GetReferralBillingAuthorizationList: "/hc/referral/getreferralbillingauthorizationlist",
    DeleteReferralBillingAuthorization: "/hc/referral/deletereferralbillingauthorization",
    GetReferralPayorMappingListUrl: "/hc/referral/getreferralpayormappinglist",


    SavePriorAuthorizationUrl: "/hc/referral/SavePriorAuthorization",
    GetPriorAuthorizationListUrl: "/hc/referral/GetPriorAuthorizationList",
    DeletePriorAuthorizationUrl: "/hc/referral/DeletePriorAuthorization",

    GetPAServiceCodeListURL: "/hc/referral/GetPAServiceCodeList",
    SavePriorAuthorizationServiceCodeDetailsUrl: "/hc/referral/SavePriorAuthorizationServiceCodeDetails",

    GetPayorServiceCodeListURL: "/hc/referral/GetPayorServicecodeList/",
    DeletePAServiceCodeUrl: "/hc/referral/DeletePAServiceCode/",
    //EdiFilesLog
    GetEdiFileLogList: "/hc/batch/getedifileloglist/",
    DeleteEdiFileLog: "/hc/batch/deleteedifilelog/",




    //270-271 Sections
    Generate270FileUrl: "/hc/batch/generate270file",
    GetEdi270FileListUrl: "/hc/batch/getedi270filelist",
    DeleteEdi270FileUrl: "/hc/batch/deleteedi270file",
    Upload271FileUrl: "/hc/batch/upload271file/",
    GetEdi271FileListUrl: "/hc/batch/getedi271filelist",
    DeleteEdi271FileUrl: "/hc/batch/deleteedi271file",

















    //Reports

    GetAttendanceReportUrl: "/report/getattendancereport/",
    GetBehaviourContractReportUrl: "/report/getbehaviourcontractreport/",
    GetClientInformationReportUrl: "/report/getclientinformationreport/",
    GetClientStatusReportUrl: "/report/getclientstatusreport/",
    GetRequestClientListReportUrl: "/report/getrequestclientlistreport/",
    GetInternalServicePlanReportUrl: "/report/getinternalserviceplanreport/",
    GetReferralDetailsReportUrl: "/report/getreferraldetailsreport/",
    GetRespiteUsageReportUrl: "/report/getrespiteusagereport/",
    GetReferralInfoforReportURL: "/report/getreferralinfo",
    GetEncounterPrintReportUrl: "/report/getencounterprintreport",
    GetSnapshotPrintReportUrl: "/report/getsnapshotprintreport",
    GetEDTRPrintReportUrl: "/report/getdtrprintreport",
    GetGeneralNoticeReportUrl: "/report/getgeneralnoticereport",
    GetDspRosterReportUrl: "/report/getdsprosterreport",
    GetScheduleAttendanceScheduleReportUrl: "/report/scheduleattendanceschedulereport",
    GetRequiredDocsForAttendanceReportUrl: "/report/getrequireddocsforattendancereport",
    GetLifeSkillsOutcomeMeasurementsReportURL: "/report/getlifeskillsoutcomemeasurementsreport",
    GetLSTeamMemberCaseloadReportURL: "/report/getlsteammembercaseloadreport",
    GetBXContractStatusReportURL: "/report/getbxcontractstatusreport",
    GetLSTMCaseloadListURL: "/report/getlsteammembercaseloadlist",
    SaveReferralLSTMCaseloadsCommentURL: "/report/savereferrallstmcaseloadscomment",
    GetBillingSummaryReportUrl: "/report/getbillingsummarylist/",
    //Agency
    AddAgencyURL: "/hc/agency/addagency/",
    PartialAddAgencyURL: "/hc/agency/PartialAddAgency/",
    GetAgencyList: "/hc/agency/getAgencylist/",
    GetAgencyListPageURL: "/hc/agency/agencylist",
    DeleteAgencyListUrl: "/hc/agency/deleteagencylist",
    GetNPIAPI: "/hc/agency/getnpidata",


    //GetScheduleBatchServiceList
    GetScheduleBatchServiceListURL: "/schedule/getschedulebatchservicelist",
    DeleteScheduleBatchServiceURL: "/schedule/deleteschedulebatchservice",

    GetReferralReviewAssessmentURL: "/referral/setreferralreviewassessment",
    SaveReferralReviewAssessmentURL: "/referral/savereferralreviewassessment",
    GetReferralReviewAssessmentList: "/referral/getreferralreviewassessmentlist",
    DeleteReferralReviewAssessmentUrl: "/referral/deletereferralreviewassessment",

    GetReferralOutcomeMeasurementURL: "/referral/setreferraloutcomemeasurement",
    SaveReferralOutcomeMeasurementURL: "/referral/savereferraloutcomemeasurement",
    GetReferralOutcomeMeasurementList: "/referral/getreferraloutcomemeasurementlist",
    DeleteReferralOutcomeMeasurementUrl: "/referral/deletereferraloutcomemeasurement",
    UploadAssessmentResult: "/referral/uploadassessmentresult",


    //Referral Monthly Summary

    //GetReferralMonthlySummaryURL: "/referral/getreferralmonthlysummarylist",
    GetSetReferralMonthlySummaryURL: "/referral/setreferralmonthlysummary",
    SearchClientForMonthlySummary: "/referral/searchclientformonthlysummary",
    SaveMultipleMonthlySummaryURL: "/referral/savemultiplemonthlysummary",



    SaveReferralMonthlySummaryURL: "/referral/savereferralmonthlysummary",
    GetReferralMonthlySummaryListURL: "/referral/getreferralmonthlysummarylist",
    FindScheduleWithFaciltyAndServiceDateURL: "/referral/findschedulewithfaciltyandserviceDate",
    GetFacilityListURL: "/referral/getfacilitylist",
    DeleteReferralMonthlySummaryURL: "/referral/deletereferralmonthlysummary",
    SaveTaskDetailURL: "/hc/referral/savetaskdetail",

    HC_GetReferralPayorsMapping: "/hc/referral/getreferralpayorsmapping",
    HC_GetPriorAutherizationCodeByPayorAndRererrals: "/hc/referral/getpriorautherizationcodebypayorandrererrals",



    Upload277FileUrl: "/batch/upload277file/",
    GetEdi277FileListUrl: "/batch/getedi277filelist",
    DeleteEdi277FileUrl: "/batch/deleteedi277file",
    Download277RedableFileUrl: "/batch/download277redablefile",


    CheckForParentChildMappingUrl: "/generalmaster/checkforparentchildmapping",

    GetParentChildMappingDDMasterUrl: "/generalmaster/getparentchildmappingddmaster",
    SaveParentChildMappingUrl: "/generalmaster/saveparentchildmapping",

    GenerateCMS1500Url: "/hc/batch/generatecms1500",
    GenerateUB04Url: "/hc/batch/generateub04",

    BroadcastNotificationUrl: "/hc/employee/broadcastnotification",
    GetBroadcastNotificationUrl: "/hc/employee/getbroadcastnotification",
    GetEmployeesForBroadcastNotificationsUrl: "/hc/employee/getemployeesforbroadcastnotifications",

    CreateBulkScheduleUsingCSVURL: "/hc/schedule/createbulkscheduleusingcsv",


    UploadFile: "/hc/referral/uploadfile",
    UploadEmployeeDocument: "/hc/referral/uploademployeedocument",

    GetEmployeeAuditLogListURL: "/hc/employee/GetAuditLogList/",
    GetReferralAuditLogListURL: "/hc/referral/GetAuditLogList/",
    GeneratePatientTimeSchedule: "/hc/cronjob/generatepatienttimeschedule",
    PayInvoiceAmountUrl: "/hc/invoice/payinvoiceamount",

    InvoiceDetailURL: "/hc/invoice/invoicedetail/",
    GetInvoiceList: "/hc/invoice/getinvoicelist",
    GenerateInvoices: "/hc/invoice/GenerateInvoices",
    DeleteInvoicesURL: "/hc/invoice/DeleteInvoices",
    DeleteInvoice: "/hc/invoice/DeleteInvoice",
    CompanyClientInvoiceListURL: "/hc/invoice/CompanyClientInvoiceList",
    NinjaInvoiceList: "/hc/invoice/NinjaInvoiceListBilling",
    GetInvoiceDownload: "/hc/invoice/NinjaInvoiceDownload",
    InvoicePDFUrl: "/hc/invoice/InvoicePDF",
    ProcessPaymentURL: "/invoice/StartProcessingPayment",
    AddBillingPaymentDetailURL: "/UserPaymentDetail/AddPaymentDetail",
    UpdateInvoiceDueDateURL: "/hc/invoice/UpdateInvoiceDueDate",

    //Care Form
    SaveCareFormDetailsURL: "/hc/referral/savecareformdetails",
    GenerateCareFormPdfURL: "/hc/referral/generatecareformpdf/",
    GetCareFormDetailsURL: "/hc/referral/getcareformdetails",
    SaveClientSignURL: "/hc/referral/saveclientsign",

    GetPendingScheduleListURL: "/hc/schedule/getpendingschedulelist",
    DeletePendingScheduleURL: "/hc/schedule/deletependingschedule",
    ProcessPendingScheduleURL: "/hc/schedule/processpendingschedule",
    SetMIFFormURL: "/hc/referral/SetMIFForm",
    SaveMIFDetailURL: "/hc/referral/SaveMIFDetail",
    SaveMIFSignURL: "/hc/referral/savemifsign",
    GenerateMIFPdfURL: "/hc/referral/GenerateMIFPdf/",
    GetReferralMIFFormsURL: "/hc/referral/GetReferralMIFForms/",

    GetReferralSectionList: "/hc/referral/getreferralsectionlist",
    GetReferralSubSectionList: "/hc/referral/getreferralsubsectionlist",
    GetReferralFormList: "/hc/referral/getreferralformlist",
    SaveSubSectionURL: "/hc/referral/savesubsection",
    SaveSectionURL: "/hc/referral/savesection",
    MapFormURL: "/hc/referral/mapform",
    SavedNewHtmlFormWithSubsectionURL: "/hc/referral/SavedNewHtmlFormWithSubsection",
    DeleteReferralDocumentURL: "/hc/referral/deletereferraldocument",

    LoadHtmlFormURL: "/hc/form/loadhtmlform/",
    LoadPdfFormURL: "/hc/form/loadpdfform/",
    LoadPdfForm_URL: "/hc/form/loadpdf_form/",
    SaveHtmlFormURL: "/hc/form/savehtmlform",

    SaveNewEBFormURL: "/hc/referral/savenewebform",
    GetFormListURL: "/hc/form/getformlist/",
    GetSavedFormListURL: "/hc/form/getsavedformlist/",
    SaveOrganizationFormDetailsURL: "/hc/form/saveorganizationformdetails/",
    OrganizationFormsURL: "/hc/form/organizationforms",
    OrganizationSettingURL: "/hc/setting/organizationsetting",
    GetPendingEmployeeVisitListURL: "/hc/report/employeevisitlist/pending",
    GenerateMultiplePcaTimeSheetURL: "/hc/report/generatemultiplepcatimesheet/",
    SaveOrganizationFormNameURL: "/hc/form/saveorganizationformname/",
    SearchTagURL: "/hc/form/searchtag/",
    GetOrgFormTagListURL: "/hc/form/getorgformtaglist/",
    DeleteFormTagURL: "/hc/form/deleteformtag/",
    AddOrgFormTagURL: "/hc/form/addorgformtag/",
    GeneratePatientActivePdfURL: "/hc/report/GeneratePatientActivePdfurl/",
    OrganizationPreferenceURL: "/hc/orgpreference/preference/",
    OrganizationPreferenceSaveURL: "/hc/orgpreference/preference/",
    OrganizationPreferenceDateFormatURL: "/hc/orgpreference/PreferenceDateFormat/",
    OrganizationPreferenceCurrencyFormatURL: "/hc/orgpreference/PreferenceCurrencyFormat/",

    //GetReferralSectionList: "/hc/referral/getreferralsectionlist",
    //GetReferralSubSectionList: "/hc/referral/getreferralsubsectionlist",
    //GetReferralFormList: "/hc/referral/getreferralformlist",
    //SaveSubSectionURL: "/hc/referral/savesubsection",
    //SaveSectionURL: "/hc/referral/savesection",
    //MapFormURL: "/hc/referral/mapform",
    //SavedNewHtmlFormWithSubsectionURL: "/hc/referral/SavedNewHtmlFormWithSubsection",
    //DeleteReferralDocumentURL: "/hc/referral/deletereferraldocument",

    AddCareTypeSlotURL: "/hc/referral/addcaretypeslot",
    GetCTScheduleListURL: "/hc/referral/getcaretypeschedulelist",
    ChangeOrgTypeURL: "/hc/home/changeorgtype",
    SaveDocumentFormNameURL: "/hc/referral/savedocumentformname",
    GetUserCertificateURL: "/hc/employee/getusercertificates",

    GetEmployeesListURL: "/hc/employee/getemployeesList",
    GetGeneralMasterListURL: "/hc/employee/GetGeneralMasterList",
    // DMAF forms
    GenerateDMAS_90FormsPdfURL: "/hc/employee/GenerateDMAS_90FormsPdfURL",
    GetCaretypeURL: "/hc/Report/GetCaretype",

    // Manage Claims Page
    GetClaimsListURL: "/hc/batch/getclaimslist",
    GetERAsListURL: "/hc/batch/GetERAsList",
    GetLatestERAPDF: "/hc/batch/GetERAPDFList/",
    GetLatestERA835: "/hc/batch/GetLatestERA835/",
    ProcessERA835: "/hc/batch/ProcessERA835/",

    HC_GetERAsListURL: "/hc/batch/HCGetERAsList",
    GetClaimErrorsListURL: "/hc/batch/getclaimerrorslist",
    GetClaimErrorsListAndCMS1500: "/hc/batch/getclaimerrorslistandCMS1500",
    SaveCMS1500DataURL: "/hc/batch/SaveCMS1500Data",
    ArchieveClaim: "/hc/batch/ArchieveClaim",


    // Google Drive Things
    GoogleAuthUrl: "/hc/setting/GetGoogleUrl",
    GoogleAuthCallbackUrl: "/hc/setting/GoogleAuthCallback",
    UploadFileGoogleDrive: "/hc/referral/UploadFileGoogleDrive",
    DeleteReferralDocumentGoogleURL: "/hc/referral/deletereferraldocumentgoogle",
    ListFilesGoogleDriveUrl: "/hc/referral/GetDocumentListGoogleDrive",
    LinkGoogleDocument: "/hc/referral/LinkGoogleDocumentFromDrive",
    PaymentBillURL: "/hc/Payment/PaymentBill",
    UpdateBatchReconcile: "/hc/batch/updatebatchreconcile/",
    DeleteCaptureCall: "/hc/capturecall/DeleteCaptureCall/",
    GetCaptureCallList: "/hc/capturecall/GetCaptureCallList",
    CaptureCallList: "/hc/capturecall/CaptureCallList",
    PartialAddCaptureCallURL: "/hc/capturecall/PartialAddCaptureCall/",
    AddCaptureCall: "/hc/capturecall/AddCaptureCall/",

    // Orbeon Forms
    OrbeonLoadHtmlFormURL: "/hc/form/OrbeonLoadHtmlForm",
    OrbeonGetFormURL: "/hc/form/OrbeonGetFormUrl",
    DuplicateOrbeonFormURL: "/hc/form/DuplicateOrbeonForm",
    SaveOrbeonDocumentURL: "/hc/referral/OrbeonSaveFormByID",

    //#region NotificationConfiguration
    GetNotificationConfigurationDetailsURL: "/hc/NotificationConfiguration/GetNotificationConfigurationDetails/",
    SaveNotificationConfigurationDetailsURL: "/hc/NotificationConfiguration/SaveNotificationConfigurationDetails/",
    DayCare_GetReferralBillingAuthorizationListURL: "/hc/schedule/DayCare_GetReferralBillingAuthorizationList/",
    SaveNotificationConfigurationDetailsURL: "/hc/NotificationConfiguration/SaveNotificationConfigurationDetails/",
    //#endregion

    AddDmas97URL: "/hc/DMAS/AddDMAS_97_AB/",
    SaveDmas97URL: "/hc/DMAS/DMAS_97_AB/",
    Dmas97AbListURL: "/hc/DMAS/DMAS97ABList/",
    Dmas97AbList1URL: "/hc/DMAS/DMAS97ABList1/",
    Dmas97AbDeleteURL: "/hc/DMAS/DeleteDMAS97AB/",
    GetDmas97AbListURL: "/hc/DMAS/GetDmas97AbList/",
    CloneDataDMAS97ABURL: "/hc/DMAS/CloneDataDMAS97AB/",

    AddDmas99URL: "/hc/DMAS/AddDMAS_99/",
    SaveDmas99URL: "/hc/DMAS/DMAS_99/",
    Dmas99ListURL: "/hc/DMAS/DMAS99List/",
    Dmas99List1URL: "/hc/DMAS/DMAS99List1/",
    Dmas99DeleteURL: "/hc/DMAS/DeleteDmas99/",
    CloneDataDMAS99URL: "/hc/DMAS/CloneDataDMAS99/",

    Cms485AddURL: "/hc/DMAS/AddCms485Form/",
    Cms485SaveURL: "/hc/DMAS/SaveCms485Form/",
    Cms485ListURL: "/hc/DMAS/Cms485List/",
    Cms485DeleteURL: "/hc/DMAS/DeleteCms485Form/",
    Cms485CloneURL: "/hc/DMAS/CloneCms485Form/",

    GetVisitTaskCategoryURL1: "/hc/referral/GetVisitTaskByCategory",
    GetVisitTaskSubCategoryURL1: "/hc/referral/GetTaskByActivity",
    ConvertToReferralURL: "/hc/capturecall/ConvertToReferral",

    SaveIncidentReportOrbeonForm: "/hc/referral/SaveIncidentReportOrbeonForm",
    SaveOrbeonFormUrl: "/hc/capturecall/SaveOrbeonForm/",
    GetEmployeeList: "/hc/report/GetEmployeeList",
    DxCodeMappingList1URL: "/hc/referral/DxCodeMappingList1",
    GetReferralPayorUrl: "/hc/home/getreferralpayor",
    GetPatientDischargedListURL: "/hc/home/getpatientdischargedlist",
    GetPatientTransferListURL: "/hc/home/getpatienttransferlist",
    GetPatientPendingListURL: "/hc/home/getpatientpendinglist",
    GetPatientOnHoldListURL: "/hc/home/getpatientonholdlist",
    GetPatientMedicaidListURL: "/hc/home/getpatientmedicaidlist",
    GetReferralPayorsUrl: "/hc/referral/getreferralpayor",
    GetReferralStatusUrl: "/hc/referral/getreferralstatus",
    GetReferralCareTypeUrl: "/hc/referral/getreferralcaretype",
    GetPatientMedicaidListURL: "/hc/home/getpatientmedicaidlist",
    SaveReferralFaceSheetFormURL: "/hc/referral/SaveReferralFaceSheetForm",
    SaveVitalFormURL: "/hc/referral/SaveVitalForm",
    //GetPatientMedicaidListURL: "/hc/home/getpatientmedicaidlist",
    GetWebNotificationCountURL: "/hc/home/getwebnotificationscount",
    GetCareTypeListURL: "/hc/home/getcaretype",
    GetEmpClockInOutListWithOutStatusURL: "/hc/home/getempclockinoutlistwithoutstatus",
    GetEmployeeReportsListURL: "/hc/report/getemployeereportslist",
    GetPatientReportsListURL: "/hc/report/getpatientreportslist",
    GetOtherReportsListURL: "/hc/report/getotherreportslist",


    DeleteNote_Temporary: "/hc/batch/DeleteNote_Temporary",
    SetClaimAdjustmentFlagUrl: "/hc/batch/setclaimadjustmentflag/",
    BulkSetClaimAdjustmentFlagUrl: "/hc/batch/bulksetclaimadjustmentflag/",


    //#region TransportService
    GetTransportContactList: "/hc/transportservice/addtransportcontact/",
    GetTransportContactListURL: "/hc/transportservice/gettransportcontactlist/",
    DeleteTransportContactURL: "/hc/transportservice/deletetransportcontact/",
    PartialAddTransportContactURL: "/hc/transportservice/partialaddtransportcontact/",
    AddTransportContactURL: "/hc/transportservice/AddTransportContact",
    SearchOrganizationNameURL: "/hc/transportservice/SearchOrganizationName",
    //#endregion

    //#region Vehicle
    AddVehicleURL: "/hc/transportservice/addvehicle/",
    GetVehicleListURL: "/hc/transportservice/getvehiclelist/",
    DeleteVehicleURL: "/hc/transportservice/deletevehicle/",
    PartialAddVehicleURL: "/hc/transportservice/partialaddvehicle/",
    //#endregion
    SaveTransportAssignmentURL: "/hc/transportservice/SaveTransportAssignment/",
    GetTransportAssignmentListURL: "/hc/transportservice/GetTransportAssignmentList/",
    DeleteTransportAssignmentListURL: "/hc/transportservice/DeleteTransportAssignmentList/",
    GetReferralListURL2: "/hc/referral/getreferrallist2/",
    //ReferralListURL2: "/hc/referral/referrallist2/"
    TransportAssignmentListURL: "/hc/transportservice/TransportAssignmentList/",
    GetTransportAssignmentReferralListURL: "/hc/transportservice/GetTransportAssignmentReferralList/",
    SaveTransportAssignPatientURL: "/hc/transportservice/SaveTransportAssignPatient/",
    DeleteTransportAssignmentReferralListURL: "/hc/transportservice/DeleteTransportAssignmentReferralList/",
    //
    SearchTransportAssignmentGroupListURL: "/hc/transportservice/SearchTransportAssignmentGroupList/",
    GetTransportGroupURL: "/hc/transportservice/GetTransportGroup/",
    GetTransportGroupDetailURL: "/hc/transportservice/GetTransportGroupDetail/",
    SaveTransportGroupAssignPatientURL: "/hc/transportservice/SaveTransportGroupAssignPatient/",
    DeleteTransportGroupAssignPatientNoteURL: "/hc/transportservice/DeleteTransportGroupAssignPatientNote/",
    SaveTransportGroupURL: "/hc/transportservice/SaveTransportGroup/",
    SaveTransportGroupAssignPatientNoteURL: "/hc/transportservice/SaveTransportGroupAssignPatientNote/",

    SaveAndGetFacilityDetails: "/st/facility/saveandgetfacilitydetails",
    DeleteSTReferralURL: "/st/facility/deletereferral",
    GetReferralAuthorizationsByReferralIDsUrl: "/st/facility/getreferralauthorizationsbyreferralid",
    // already used above -> GetRTSMasterListURL: "/st/facility/getrtsmasterlist",

    // already used above -> GetRTSMasterListURL: "/st/facility/getrtsmasterlist",
    // already used above -> GeneratePatientTimeSchedule: "/hc/cronjob/generatepatienttimeschedule",
    // already used above -> DeleteRTSMasterURL: "/st/facility/deletertsmaster",
    // already used above -> AddRTSByPriorAuthURL: "/st/facility/addrtsbypriorauth",
    // already used above -> GetRTSDetailListURL: "/st/facility/getrtsdetaillist",
    // already used above -> AddRTSMasterURL: "/st/facility/addrtsmaster",
    // already used above -> DeleteRTSDetailURL: "/st/facility/deletertsdetail",
    // already used above -> AddRTSDetailURL: "/st/facility/addrtsdetail",
    // already used above -> ReferralTimeSlotForceUpdateURL: "/st/facility/referraltimeslotforceupdate",
    // already used above -> UpdateRTSDetailURL: "/st/facility/updatertsdetail",
    GetEmployeeGroupURL: "/st/facility/GetEmployeeGroup",
    GetEmployeeByGroupIdUrl: "/st/facility/GetEmployeeByGroupId",
    UpdateEmployeeGroupIdUrl: "/st/facility/updateemployeegroupid",
    GroupNameUrl: "/st/facility/SaveEmpGroup",
    RemoveAllAssignedGroupUrl: "/st/facility/RemoveAllAssignedGroup",
    PartialAddfacilityURL: "/st/facility/PartialddReferral",
    // already used above -> DeleteReferralURL: "/st/facility/deletereferral"

    SaveclockinoutURL: "/hc/attendance/saveclockinout",
    GetclockinoutURL: "/hc/attendance/getclockinout",
    EmployeeAttendanceCalendarURL: "/hc/attendance/employeeattendancecalendar",

    GetARAgingReportURL: "/hc/batch/getaragingreport",
    ExportARAgingReportListURL: "/hc/batch/exportaragingreportlist",
    BatchMasterURL: "/hc/batch/batchmaster",

    GetRegionURL: "/hc/home/GetRegion",
    SaveMannualPaymentPostingDetailsUrl: "/hc/batch//SaveMannualPaymentPostingDetails"
};


