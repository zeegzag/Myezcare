CREATE PROCEDURE [dbo].[HC_MapForm]                
@ComplianceID BIGINT,          
@SectionID BIGINT,          
@MapPermanently BIT,                
@EBFormID NVARCHAR(MAX),            
@UserType INT,          
@CurrentDate DATETIME,                
@LoggedInID BIGINT,                
@SystemID NVARCHAR(MAX),  
@RoleID  BIGINT  
AS                
BEGIN          
          
IF(@MapPermanently=1)          
 UPDATE Compliances SET EBFormID=@EBFormID,UserType=@UserType,UpdatedBy=@LoggedInID,UpdatedDate=@CurrentDate,SystemID=@SystemID                
 WHERE ComplianceID=@ComplianceID                
    
 DECLARE @EBForms [dbo].[EBFormsData]
    
INSERT INTO @EBForms    
EXEC GetAdminDatabseTableData EBForms    
    
 SELECT Version,NameForUrl,IsInternalForm,FormId,EBFormID,InternalFormPath,FormLongName AS FormName, IsNull(IsOrbeonForm, 0) as IsOrbeonForm
  FROM @EBForms WHERE EBFormID=@EBFormID          
          
 EXEC HC_GetReferralSectionList @UserType , @RoleID         
          
 EXEC HC_GetReferralSubSectionList @SectionID,@UserType , @RoleID             
END