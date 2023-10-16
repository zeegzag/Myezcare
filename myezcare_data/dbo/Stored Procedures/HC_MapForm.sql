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
    
 DECLARE @EBForms TABLE(    
EBFormID nvarchar(MAX),    
FromUniqueID nvarchar(MAX),    
Id nvarchar(MAX),    
FormId nvarchar(MAX),    
Name nvarchar(MAX),    
FormLongName nvarchar(MAX),    
NameForUrl nvarchar(MAX),    
Version nvarchar(MAX),    
IsActive bit,    
HasHtml bit,    
NewHtmlURI nvarchar(MAX),    
HasPDF bit,    
NewPdfURI nvarchar(MAX),    
EBCategoryID nvarchar(MAX),    
EbMarketIDs nvarchar(MAX),    
FormPrice decimal(10, 2),    
CreatedDate datetime,    
UpdatedDate datetime,    
UpdatedBy bigint,    
IsDeleted bit,    
IsInternalForm bit,    
InternalFormPath nvarchar(MAX)    
)    
    
INSERT INTO @EBForms    
EXEC GetAdminDatabseTableData EBForms    
    
 SELECT Version,NameForUrl,IsInternalForm,FormId,EBFormID,InternalFormPath,FormLongName AS FormName      
  FROM @EBForms WHERE EBFormID=@EBFormID          
          
 EXEC HC_GetReferralSectionList @UserType , @RoleID         
          
 EXEC HC_GetReferralSubSectionList @SectionID,@UserType , @RoleID             
END
