CREATE PROCEDURE [dbo].[API_GetTaskFormList]  
  @EmployeeVisitID bigint, 
  @ReferralTaskMappingID bigint  
AS  
BEGIN  
  DECLARE @VisitTaskID bigint  
  SELECT  
    @VisitTaskID = VisitTaskID  
  FROM ReferralTaskMappings  
  WHERE  
    ReferralTaskMappingID = @ReferralTaskMappingID  
  
  
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
  
  
  
  SELECT DISTINCT  
    TFM.TaskFormMappingID,  
    VisitTaskID,  
    TFM.EBFormID,  
    IsRequired,  
    FormNumber = EBF.Name,  
    FormName = ISNULL(OFRM.OrganizationFriendlyFormName, EBF.FormLongName),  
    EBF.NameForUrl,  
    EBF.Version,  
    EBF.InternalFormPath,  
    EBF.IsInternalForm,  
    EBF.IsOrbeonForm,  
    EBF.EBFormID,  
    EBF.FormId,  
    IsFilled =  
              CASE  
                WHEN EBF.IsInternalForm = 1 AND  
                  (EBFM.EbriggsFormMppingID > 0 OR  
                  EBFM.EbriggsFormMppingID IS NOT NULL) THEN 1  
                WHEN EBF.IsOrbeonForm = 1 AND  
                  RD.GoogleFileId IS NOT NULL THEN 1  
                ELSE 0  
              END,  
    RD.GoogleFileId [OrbeonFormID],  
    RD.ComplianceID,  
    RTFM.ReferralTaskFormMappingID  
  FROM TaskFormMappings TFM  
  INNER JOIN @OrganizationForms OFRM  
    ON OFRM.EBFormID = TFM.EBFormID  
  INNER JOIN @EBForms EBF  
    ON EBF.EBFormID = OFRM.EBFormID  
  LEFT JOIN EbriggsFormMppings EBFM  
    ON EBFM.TaskFormMappingID = TFM.TaskFormMappingID  
    AND EBFM.ReferralTaskMappingID = @ReferralTaskMappingID  
    AND EBFM.EmployeeVisitNoteID IS NULL  
  LEFT JOIN ReferralTaskFormMappings RTFM  
    ON RTFM.TaskFormMappingID = TFM.TaskFormMappingID  
    AND RTFM.ReferralTaskMappingID = @ReferralTaskMappingID  
    AND RTFM.[EmployeeVisitID] = @EmployeeVisitID
  LEFT JOIN ReferralDocuments RD  
    ON RD.ReferralDocumentID = RTFM.ReferralDocumentID  
  
  WHERE  
    TFM.VisitTaskID = @VisitTaskID  
    AND TFM.IsDeleted = 0  
  
--SELECT DISTINCT TFM.TaskFormMappingID,VisitTaskID,TFM.EBFormID,IsRequired,FormNumber=EBF.Name,          
--FormName=ISNULL(OFRM.OrganizationFriendlyFormName,EBF.FormLongName),            
--EBF.NameForUrl,EBF.Version,EBF.InternalFormPath,EBF.IsInternalForm,EBF.EBFormID,EBF.FormId,        
--IsFilled=CASE WHEN (EBFM.EbriggsFormMppingID>0 OR EBFM.EbriggsFormMppingID IS NOT NULL) THEN 1 ELSE 0 END        
--FROM TaskFormMappings TFM                
--INNER JOIN MyezcareOrganization.dbo.OrganizationForms OFRM ON OFRM.EBFormID=TFM.EBFormID                
--INNER JOIN MyezcareOrganization.dbo.EBForms EBF ON EBF.EBFormID=OFRM.EBFormID        
--LEFT JOIN EbriggsFormMppings EBFM ON EBFM.TaskFormMappingID=TFM.TaskFormMappingID AND EBFM.ReferralTaskMappingID=@ReferralTaskMappingID AND EmployeeVisitNoteID IS NULL        
--WHERE TFM.VisitTaskID=@VisitTaskID AND TFM.IsDeleted=0      
END