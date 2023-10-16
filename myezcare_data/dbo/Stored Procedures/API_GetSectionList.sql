CREATE PROCEDURE [dbo].[API_GetSectionList]                  
@UserType INT = 2                
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

SELECT DISTINCT ComplianceID,Name=DocumentName,Color=Value,EF.Version,EF.NameForUrl,C.IsTimeBased,EF.IsInternalForm,EF.InternalFormPath,      
 DocumentationType,C.EBFormID,FormName=ISNULL(ORGFRM.OrganizationFriendlyFormName,EF.FormLongName),EF.EBFormID    
 FROM Compliances C                      
 LEFT JOIN @EBForms EF ON EF.EBFormID=C.EBFormID    
 LEFT JOIN @OrganizationForms ORGFRM ON ORGFRM.EBFormID=EF.EBFormID    
 WHERE C.IsDeleted=0 AND ParentID=0 AND UserType=@UserType

 --SELECT DISTINCT ComplianceID,Name=DocumentName,Color=Value,EF.Version,EF.NameForUrl,C.IsTimeBased,EF.IsInternalForm,EF.InternalFormPath,      
 --DocumentationType,C.EBFormID,FormName=ISNULL(ORGFRM.OrganizationFriendlyFormName,EF.FormLongName),EF.EBFormID    
 --FROM Compliances C                      
 --LEFT JOIN MyezcareOrganization.dbo.EBForms EF ON EF.EBFormID=C.EBFormID    
 --LEFT JOIN MyezcareOrganization.dbo.OrganizationForms ORGFRM ON ORGFRM.EBFormID=EF.EBFormID    
 --WHERE C.IsDeleted=0 AND ParentID=0 AND UserType=@UserType
END
