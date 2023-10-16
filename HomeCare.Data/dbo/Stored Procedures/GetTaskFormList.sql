CREATE PROCEDURE [dbo].[GetTaskFormList]        
@VisitTaskID BIGINT        
AS        
BEGIN    
    
DECLARE @EBForms [dbo].[EBFormsData]  
    
INSERT INTO @EBForms    
EXEC GetAdminDatabseTableData EBForms    
    
DECLARE @OrganizationForms TABLE(    
OrganizationFormID bigint,    
EBFormID nvarchar(MAX),    
OrganizationID bigint,    
CreatedDate datetime,    
UpdatedDate datetime,    
CreatedBy bigint,    
UpdatedBy bigint,    
OrganizationFriendlyFormName nvarchar(MAX)    
)    
    
INSERT INTO @OrganizationForms    
EXEC GetAdminDatabseTableData OrganizationForms    
    
 SELECT DISTINCT TaskFormMappingID,VisitTaskID,TFM.EBFormID,TFM.ComplianceID,IsRequired,EBF.Name,FormLongName=ISNULL(OFRM.OrganizationFriendlyFormName,EBF.FormLongName),EBF.NameForUrl,EBF.Version,
 EBF.FormId, EBF.IsInternalForm, EBF.InternalFormPath, EBF.IsOrbeonForm          
 FROM TaskFormMappings TFM          
 INNER JOIN @OrganizationForms OFRM ON OFRM.EBFormID=TFM.EBFormID          
 INNER JOIN @EBForms EBF ON EBF.EBFormID=OFRM.EBFormID          
 WHERE TFM.VisitTaskID=@VisitTaskID AND TFM.IsDeleted=0        
END