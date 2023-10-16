CREATE PROCEDURE [dbo].[HC_GetReferralFormList]    
@ComplianceID BIGINT,    
@ReferralID BIGINT,    
@UserType INT    
AS      
BEGIN

 DECLARE @TempTable TABLE (  
 ReferralDocumentID BIGINT,  
 FileName VARCHAR(MAX),  
 FilePath VARCHAR(MAX),  
 EbriggsFormMppingID BIGINT,   
 EBriggsFormID NVARCHAR(MAX),  
 OriginalEBFormID NVARCHAR(MAX),  
 FormId NVARCHAR(MAX),  
 NameForUrl NVARCHAR(MAX),  
 Version NVARCHAR(MAX),  
 FormName NVARCHAR(MAX),  
 UpdatedDate DATETIME,  
 Tags NVARCHAR(MAX)  
 )  
 
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

DECLARE @OrganizationFormTags TABLE(
OrganizationFormTagID bigint,
OrganizationFormID bigint,
FormTagID bigint
)

INSERT INTO @OrganizationFormTags
EXEC GetAdminDatabseTableData OrganizationFormTags

DECLARE @FormTags TABLE(	
FormTagID bigint,
FormTagName	nvarchar(200)
)
INSERT INTO @OrganizationFormTags
EXEC GetAdminDatabseTableData OrganizationFormTags	

  
 INSERT INTO @TempTable (EbriggsFormMppingID,EBriggsFormID,OriginalEBFormID,FormId,NameForUrl,Version,FormName,UpdatedDate,Tags)  
 SELECT DISTINCT EBFM.EbriggsFormMppingID, EBFM.EBriggsFormID,EBFM.OriginalEBFormID,EBFM.FormId ,EBF.NameForUrl,EBF.Version,  
 ISNULL(ORGFRM.OrganizationFriendlyFormName,EBF.FormLongName) AS FormName,EBFM.UpdatedDate,  
 Tags = STUFF((SELECT ', ' + FT.FormTagName    
           FROM @OrganizationFormTags OFT    
     INNER JOIN @FormTags FT ON FT.FormTagID=OFT.FormTagID    
           WHERE OFT.OrganizationFormID = ORGFRM.OrganizationFormID    
          FOR XML PATH('')), 1, 2, '')  
 FROM EbriggsFormMppings EBFM  
 INNER JOIN @EBForms EBF ON EBF.EBFormID=EBFM.OriginalEBFormID  
 LEFT JOIN @OrganizationForms ORGFRM ON ORGFRM.EBFormID=EBF.EBFormID  
 WHERE SubSectionID = @ComplianceID AND EBFM.IsDeleted=0  
    
  INSERT INTO @TempTable (ReferralDocumentID,FileName,FilePath)  
 SELECT ReferralDocumentID,FileName,FilePath FROM ReferralDocuments RD  
 WHERE UserID=@ReferralID AND ComplianceID=@ComplianceID AND UserType=@UserType  
  
 SELECT * FROM @TempTable  
END
