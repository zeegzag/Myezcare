CREATE PROCEDURE [dbo].[HC_GetReferralSubSectionList]                              
@SectionID BIGINT,                      
@UserType INT,      
@RoleID  BIGINT,
@ReferralID BIGINT = 0  ,
@EmployeeID nvarchar(50) 
AS                              
BEGIN        
        
DECLARE @EBForms [dbo].[EBFormsData]      
        
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
        
 SELECT DISTINCT C.ComplianceID,SubSectionName=DocumentName,EF.Version,EF.NameForUrl,IsTimeBased,EF.IsInternalForm,EF.InternalFormPath,            
 FormName=IsNULL(ORGFRM.OrganizationFriendlyFormName,EF.FormLongName),EF.FormId,EF.EBFormID,  IsNull(EF.IsOrbeonForm, 0) as IsOrbeonForm,
 SortingID
 FROM Compliances C
 CROSS APPLY (
	 SELECT CASE WHEN EXISTS(SELECT 1 FROM ReferralDocuments RD WHERE RD.UserID = @ReferralID AND RD.ComplianceID = C.ComplianceID) THEN 1 ELSE 0 END HasDocuments
 ) D
 INNER JOIN SectionPermissions SP ON SP.ComplianceID= C.ComplianceID AND SP.RoleID=@RoleID              
 LEFT JOIN @EBForms EF ON EF.EBFormID=C.EBFormID            
 LEFT JOIN @OrganizationForms ORGFRM ON ORGFRM.EBFormID=EF.EBFormID            
 WHERE C.ParentID=@SectionID AND C.IsDeleted=0 AND C.UserType=@UserType    
 AND (C.HideIfEmpty = 0 OR D.HasDocuments = 1)
 AND ISNULL(C.EmployeeID,ReferralID) = ( CASE WHEN @EmployeeID = '0' then @ReferralID ELSE @EmployeeID END) OR ISNULL(ShowToAll,1)=1
 ORDER BY C.SortingID DESC

END