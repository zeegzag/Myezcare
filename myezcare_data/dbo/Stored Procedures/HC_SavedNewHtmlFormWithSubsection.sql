--EXEC HC_SavedNewHtmlFormWithSubsection @ReferralID = '24247', @SubSectionID = '10194', @EBriggsFormID = '5c98964bc5e2c71b7cb608a4', @OriginalEBFormID = '5bee5aa78a063bc765ef1f24', @FormName = '', @UpdateFormName = 'True', @FormId = '543', @LoggedInID = '1', @SystemID = '74D435D13E71'  
CREATE PROCEDURE [dbo].[HC_SavedNewHtmlFormWithSubsection]
@ReferralID BIGINT=NULL,          
@EmployeeID BIGINT=NULL,          
@SubSectionID BIGINT=NULL,                
@EBriggsFormID NVARCHAR(MAX)=NULL,                
@OriginalEBFormID NVARCHAR(MAX)=NULL,                
@FormName NVARCHAR(MAX),        
@UpdateFormName BIT,      
@FormId NVARCHAR(MAX)=NULL,      
@HTMLFormContent NVARCHAR(MAX)=null,              
@EbriggsFormMppingID BIGINT=0,    
@LoggedInID BIGINT=NULL,                
@SystemID NVARCHAR(100)=NULL            
AS                  
BEGIN                  
                
  IF(@ReferralID = 0) SET @ReferralID = NULL                
  IF(@SubSectionID = 0) SET @SubSectionID = NULL                
          
  IF(@FormName IS NULL OR @FormName='')        
  BEGIN

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
  END        
          
    
  IF(@EbriggsFormMppingID>0)          
    BEGIN     -- INTERNAL FORMS UPDATE          
      UPDATE EbriggsFormMppings SET                 
      ReferralID=@ReferralID, EmployeeID=@EmployeeID, SubSectionID=@SubSectionID, FormName=@FormName, HTMLFormContent=@HTMLFormContent,       
      UpdatedDate=GETUTCDATE(), UpdatedBy=@LoggedInID, SystemID=@SystemID                 
      WHERE EbriggsFormMppingID=@EbriggsFormMppingID     
         
             
    END      
                  
  ELSE IF EXISTS (SELECT TOP 1 EbriggsFormMppingID FROM EbriggsFormMppings WHERE OriginalEBFormID=@OriginalEBFormID AND FormId=@FormId AND              
  EBriggsFormID=@EBriggsFormID AND (ReferralID=@ReferralID OR SubSectionID=@SubSectionID))            
  BEGIN                
      
  --Set Current FormName      
  IF(@UpdateFormName=0)      
    SELECT @FormName=FormName FROM EbriggsFormMppings WHERE       
    OriginalEBFormID=@OriginalEBFormID AND FormId=@FormId AND  EBriggsFormID=@EBriggsFormID AND (ReferralID=@ReferralID OR SubSectionID=@SubSectionID)      
                
  UPDATE EbriggsFormMppings SET                 
  ReferralID=@ReferralID, EmployeeID=@EmployeeID, SubSectionID=@SubSectionID, FormName=@FormName, HTMLFormContent=@HTMLFormContent,       
  UpdatedDate=GETUTCDATE(), UpdatedBy=@LoggedInID, SystemID=@SystemID                 
  WHERE OriginalEBFormID=@OriginalEBFormID AND FormId=@FormId AND  EBriggsFormID=@EBriggsFormID AND (ReferralID=@ReferralID OR SubSectionID=@SubSectionID)                
                
  END                
                
  ELSE                
  BEGIN                
                
  INSERT INTO EbriggsFormMppings(EBriggsFormID,OriginalEBFormID,FormId,ReferralID,EmployeeID,SubSectionID,FormName,HTMLFormContent,        
  CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted)                 
  SELECT @EBriggsFormID,@OriginalEBFormID,@FormId,@ReferralID,@EmployeeID,@SubSectionID,@FormName,@HTMLFormContent,        
   GETUTCDATE(), @LoggedInID, GETUTCDATE(), @LoggedInID,@SystemID,0                
                
  END                
                
                
                  
END
