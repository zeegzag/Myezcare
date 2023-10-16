CREATE PROCEDURE [dbo].[API_SaveForm]                  
@ReferralID BIGINT,        
@ComplianceID BIGINT,                
@EBriggsFormID NVARCHAR(MAX)=NULL,                
@OriginalEBFormID NVARCHAR(MAX)=NULL,                
@FormId NVARCHAR(MAX)=NULL,                
@LoggedInID BIGINT,        
@ServerCurrentDateTime DATETIME,        
@SystemID NVARCHAR(100)        
AS                  
BEGIN                  
                
  IF(@ReferralID = 0) SET @ReferralID = NULL                
  IF(@ComplianceID = 0) SET @ComplianceID = NULL                
     
  DECLARE @EbriggsFormMppingID BIGINT    
  DECLARE @FormName NVARCHAR(MAX)  
  
  SELECT TOP 1 @EbriggsFormMppingID=EbriggsFormMppingID FROM EbriggsFormMppings WHERE OriginalEBFormID=@OriginalEBFormID AND FormId=@FormId AND              
  EBriggsFormID=@EBriggsFormID AND (ReferralID=@ReferralID OR SubSectionID=@ComplianceID)    
  


  DECLARE @EBForms TABLE(
EBFormID nvarchar(MAX),
FromUniqueID nvarchar(MAX),
Id nvarchar(MAX),
FormId nvarchar(MAX),
Name nvarchar(MAX),
FormLongName nvarchar(MAX),
NameForUrl nvarchar(MAX),
Version	nvarchar(MAX),
IsActive bit,
HasHtml	bit,
NewHtmlURI nvarchar(MAX),
HasPDF bit,
NewPdfURI nvarchar(MAX),
EBCategoryID nvarchar(MAX),
EbMarketIDs	nvarchar(MAX),
FormPrice decimal(10, 2),
CreatedDate	datetime,
UpdatedDate	datetime,
UpdatedBy bigint,
IsDeleted bit,
IsInternalForm bit,
InternalFormPath nvarchar(MAX)
)

INSERT INTO @EBForms
EXEC GetAdminDatabseTableData EBForms

DECLARE @OrganizationForms TABLE(
OrganizationFormID bigint,
EBFormID nvarchar(MAX),
OrganizationID bigint,
CreatedDate	datetime,
UpdatedDate	datetime,
CreatedBy bigint,
UpdatedBy bigint,
OrganizationFriendlyFormName nvarchar(MAX)
)

INSERT INTO @OrganizationForms
EXEC GetAdminDatabseTableData OrganizationForms

 SELECT @FormName=ISNULL(OrganizationFriendlyFormName,FormLongName)+'_'+ replace(convert(NVARCHAR, getdate(), 101),'/', '')   
 FROM @OrganizationForms ORGFRM      
  INNER JOIN @EBForms EF ON EF.EBFormID=ORGFRM.EBFormID      
  WHERE EF.EBFormID=@OriginalEBFormID

 -- SELECT @FormName=ISNULL(OrganizationFriendlyFormName,FormLongName)+'_'+ replace(convert(NVARCHAR, getdate(), 101),'/', '')   
 --FROM MyezcareOrganization.dbo.OrganizationForms ORGFRM      
 -- INNER JOIN MyezcareOrganization.dbo.EBForms EF ON EF.EBFormID=ORGFRM.EBFormID      
 -- WHERE EF.EBFormID=@OriginalEBFormID
    
  IF (@EbriggsFormMppingID>0)    
  BEGIN                
                
  UPDATE EbriggsFormMppings SET                 
  ReferralID=@ReferralID, SubSectionID=@ComplianceID, UpdatedDate=@ServerCurrentDateTime, UpdatedBy=@LoggedInID, SystemID=@SystemID                 
  WHERE EbriggsFormMppingID=@EbriggsFormMppingID    
  --OriginalEBFormID=@OriginalEBFormID AND FormId=@FormId AND  EBriggsFormID=@EBriggsFormID AND (ReferralID=@ReferralID OR SubSectionID=@ComplianceID)                
         
   --Return EbriggsFormMppingID             
 SElect EbriggsFormMppingID=@EbriggsFormMppingID,Action='Update'    
  END                
                
  ELSE                
  BEGIN                
                
  INSERT INTO EbriggsFormMppings(EBriggsFormID,OriginalEBFormID,FormId,ReferralID,EmployeeID,SubSectionID,      
  CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted,FormName)                 
  SELECT @EBriggsFormID,@OriginalEBFormID,@FormId,@ReferralID,NULL,@ComplianceID,@ServerCurrentDateTime, @LoggedInID, @ServerCurrentDateTime, @LoggedInID,@SystemID,0,  
  @FormName                
      
  --Return EbriggsFormMppingID      
 SELECT EbriggsFormMppingID=@@IDENTITY,Action='Insert',FormName=@FormName    
  END                
                
                
                  
END
