CREATE PROCEDURE [dbo].[GetVisitTaskDocumentList]  
  @EmployeeVisitID bigint,  
  @OrganizationID bigint  
AS  
BEGIN  
  
  DECLARE @EBForms [dbo].[EBFormsData]  
  INSERT INTO @EBForms  
  EXEC GetAdminDatabseTableData EBForms  
  
  DECLARE @OrganizationForms TABLE (  
    OrganizationFormID bigint,  
    EBFormID nvarchar(max),  
    OrganizationID bigint,  
    CreatedDate datetime,  
    UpdatedDate datetime,  
    CreatedBy bigint,  
    UpdatedBy bigint,  
    OrganizationFriendlyFormName nvarchar(max)  
  )  
  INSERT INTO @OrganizationForms  
  EXEC GetAdminDatabseTableData OrganizationForms  
  
  SELECT  
    EVN.EmployeeVisitNoteID,  
    RTM.ReferralTaskMappingID,  
    RTM.VisitTaskID,  
    VT.VisitTaskDetail,  
    TFM.TaskFormMappingID,  
    TFM.EBFormID,  
    RTFM.ReferralDocumentID,  
    RD.ComplianceID,  
    RTFM.ReferralTaskFormMappingID,  
    RD.GoogleFileId OrbeonFormID,  
    RD.[FileName] DocFileName,  
    RD.FilePath DocFilePath,  
    FormName = EBF.[Name],  
    FormLongName = ISNULL(OFRM.OrganizationFriendlyFormName, EBF.FormLongName),  
    EBF.NameForUrl,  
    EBF.[Version],  
    EBF.FormId,  
    EBF.IsInternalForm,  
    EBF.InternalFormPath,  
    EBF.IsOrbeonForm  
  FROM [dbo].[EmployeeVisitNotes] EVN  
  INNER JOIN [dbo].[ReferralTaskMappings] RTM  
    ON RTM.ReferralTaskMappingID = EVN.ReferralTaskMappingID  
  INNER JOIN [dbo].[VisitTasks] VT  
    ON VT.VisitTaskID = RTM.VisitTaskID  
  INNER JOIN [dbo].[TaskFormMappings] TFM  
    ON TFM.VisitTaskID = RTM.VisitTaskID  
    AND TFM.IsDeleted = 0  
  INNER JOIN @OrganizationForms OFRM  
    ON OFRM.EBFormID = TFM.EBFormID  
    AND OFRM.OrganizationID = @OrganizationID  
  INNER JOIN @EBForms EBF  
    ON EBF.EBFormID = OFRM.EBFormID  
  INNER JOIN [dbo].[ReferralTaskFormMappings] RTFM  
    ON RTFM.ReferralTaskMappingID = RTM.ReferralTaskMappingID  
    AND RTFM.TaskFormMappingID = TFM.TaskFormMappingID  
    AND RTFM.[EmployeeVisitID] = @EmployeeVisitID
  INNER JOIN [dbo].[ReferralDocuments] RD  
    ON RD.ReferralDocumentID = RTFM.ReferralDocumentID  
  WHERE  
    EVN.IsDeleted = 0  
    AND EVN.EmployeeVisitID = @EmployeeVisitID  
END