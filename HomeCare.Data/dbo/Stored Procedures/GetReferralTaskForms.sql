CREATE PROCEDURE [dbo].[GetReferralTaskForms]  
  @EmployeeVisitID bigint, 
  @ReferralTaskMappingID bigint,  
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
    TFM.TaskFormMappingID,
    TFM.ComplianceID,
    TFM.IsRequired,  
    TFM.VisitTaskID,  
    TFM.EBFormID,  
    RTFM.ReferralDocumentID,  
    RD.ComplianceID,  
    RTFM.ReferralTaskFormMappingID,  
    RD.GoogleFileId OrbeonFormID,  
    EBF.Name,  
    FormLongName = ISNULL(OFRM.OrganizationFriendlyFormName, EBF.FormLongName),  
    EBF.NameForUrl,  
    EBF.Version,  
    EBF.FormId,  
    EBF.IsInternalForm,  
    EBF.InternalFormPath,  
    EBF.IsOrbeonForm  
  FROM [dbo].[ReferralTaskMappings] RTM  
  INNER JOIN [dbo].[TaskFormMappings] TFM  
    ON TFM.VisitTaskID = RTM.VisitTaskID  
    AND TFM.IsDeleted = 0  
  INNER JOIN @OrganizationForms OFRM  
    ON OFRM.EBFormID = TFM.EBFormID  
    AND OFRM.OrganizationID = @OrganizationID  
  INNER JOIN @EBForms EBF  
    ON EBF.EBFormID = OFRM.EBFormID  
  LEFT JOIN [dbo].[ReferralTaskFormMappings] RTFM  
    ON RTFM.ReferralTaskMappingID = RTM.ReferralTaskMappingID  
    AND RTFM.TaskFormMappingID = TFM.TaskFormMappingID  
    AND RTFM.[EmployeeVisitID] = @EmployeeVisitID
  LEFT JOIN [dbo].[ReferralDocuments] RD  
    ON RD.ReferralDocumentID = RTFM.ReferralDocumentID  
  WHERE  
    RTM.ReferralTaskMappingID = @ReferralTaskMappingID  
END