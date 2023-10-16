CREATE PROCEDURE [dbo].[API_OpenSavedForm]  
@EbriggsFormMppingID BIGINT,  
@UpdateForm BIT  
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

SELECT DISTINCT Version,NameForUrl,EBriggsFormID,IsInternalForm,InternalFormPath,EF.FormId,EF.EBFormID,EFM.ReferralID  
  FROM EbriggsFormMppings EFM  
 INNER JOIN @OrganizationForms ORGFRM ON ORGFRM.EBFormID=EFM.OriginalEBFormID  
 INNER JOIN @EBForms EF ON EF.EBFormID=ORGFRM.EBFormID  
 WHERE EFM.EbriggsFormMppingID=@EbriggsFormMppingID


 --SELECT DISTINCT Version,NameForUrl,EBriggsFormID,IsInternalForm,InternalFormPath,EF.FormId,EF.EBFormID,EFM.ReferralID  
 -- FROM EbriggsFormMppings EFM  
 --INNER JOIN MyezcareOrganization.dbo.OrganizationForms ORGFRM ON ORGFRM.EBFormID=EFM.OriginalEBFormID  
 --INNER JOIN MyezcareOrganization.dbo.EBForms EF ON EF.EBFormID=ORGFRM.EBFormID  
 --WHERE EFM.EbriggsFormMppingID=@EbriggsFormMppingID  
END
