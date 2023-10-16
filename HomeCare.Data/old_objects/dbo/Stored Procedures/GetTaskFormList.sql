CREATE PROCEDURE [dbo].[GetTaskFormList]    
@VisitTaskID BIGINT    
AS    
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

 SELECT DISTINCT TaskFormMappingID,VisitTaskID,TFM.EBFormID,IsRequired,EBF.Name,FormLongName=ISNULL(OFRM.OrganizationFriendlyFormName,EBF.FormLongName),EBF.NameForUrl,EBF.Version      
 FROM TaskFormMappings TFM      
 INNER JOIN @OrganizationForms OFRM ON OFRM.EBFormID=TFM.EBFormID      
 INNER JOIN @EBForms EBF ON EBF.EBFormID=OFRM.EBFormID      
 WHERE TFM.VisitTaskID=@VisitTaskID AND TFM.IsDeleted=0    
END
