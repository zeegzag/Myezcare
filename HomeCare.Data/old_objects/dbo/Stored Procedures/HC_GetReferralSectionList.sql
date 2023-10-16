﻿CREATE PROCEDURE [dbo].[HC_GetReferralSectionList]              
@UserType INT = 2,
@RoleID  BIGINT
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
  
 SELECT DISTINCT C.ComplianceID,SectionName=DocumentName,Color=Value,EF.Version,EF.NameForUrl,C.IsTimeBased,EF.IsInternalForm,EF.InternalFormPath,      
 FormName=IsNULL(ORGFRM.OrganizationFriendlyFormName,EF.FormLongName),EF.EBFormID,EF.FormId    
 FROM Compliances C              
 INNER JOIN SectionPermissions SP ON SP.ComplianceID= C.ComplianceID AND SP.RoleID=@RoleID
 LEFT JOIN @EBForms EF ON EF.EBFormID=C.EBFormID      
 LEFT JOIN @OrganizationForms ORGFRM ON ORGFRM.EBFormID=EF.EBFormID      
 WHERE C.IsDeleted=0 AND ParentID=0 AND UserType=@UserType              
END
