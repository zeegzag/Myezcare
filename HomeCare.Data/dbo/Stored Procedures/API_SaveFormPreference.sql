CREATE PROCEDURE [dbo].[API_SaveFormPreference]        
@ComplianceID BIGINT,        
@EBFormID NVARCHAR(MAX),        
@SavePreference BIT,        
@ServerCurrentDateTime DATETIME,        
@LoggedInID BIGINT,        
@SystemID NVARCHAR(MAX)        
AS        
BEGIN        
 IF(@SavePreference=1)        
  Update Compliances SET EBFormID=@EBFormID WHERE ComplianceID=@ComplianceID        
    
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
    
 SELECT DISTINCT Version,NameForUrl,InternalFormPath,IsInternalForm,IsOrbeonForm,FormId,EF.EBFormID         
 FROM @OrganizationForms ORGFRM        
 INNER JOIN @EBForms EF ON EF.EBFormID=ORGFRM.EBFormID        
 WHERE ORGFRM.EBFormID=@EBFormID    
    
    
 --SELECT DISTINCT Version,NameForUrl,InternalFormPath,IsInternalForm,FormId,EF.EBFormID         
 --FROM MyezcareOrganization.dbo.OrganizationForms ORGFRM        
 --INNER JOIN MyezcareOrganization.dbo.EBForms EF ON EF.EBFormID=ORGFRM.EBFormID        
 --WHERE ORGFRM.EBFormID=@EBFormID      
        
END