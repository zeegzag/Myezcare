--EXEC SaveDocumentFormName @EbriggsFormMppingID = '10902', @FormName = '', @UpdateFormName = 'True'  
  
CREATE PROCEDURE [dbo].[SaveDocumentFormName]              
@EbriggsFormMppingID BIGINT,            
@FormName NVARCHAR(500),  
@UpdateFormName BIT  
AS              
BEGIN    
 DECLARE @OriginalEBFormID NVARCHAR(MAX)    
 SELECT @OriginalEBFormID=OriginalEBFormID FROM EbriggsFormMppings WHERE EbriggsFormMppingID=@EbriggsFormMppingID    
    
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
   
 IF(@UpdateFormName=1)   
 UPDATE EbriggsFormMppings SET FormName=@FormName WHERE EbriggsFormMppingID=@EbriggsFormMppingID          
END
