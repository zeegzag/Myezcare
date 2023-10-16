CREATE PROCEDURE [dbo].[HC_GetReferralSectionList]
	 @UserType INT = 2
	,@RoleID BIGINT
	,@ReferralID BIGINT = 0 
	,@EmployeeID nvarchar(50) 
AS
BEGIN
	DECLARE @EBForms [dbo].[EBFormsData]

	INSERT INTO @EBForms
	EXEC GetAdminDatabseTableData EBForms

	DECLARE @OrganizationForms TABLE (
		OrganizationFormID BIGINT
		,EBFormID NVARCHAR(MAX)
		,OrganizationID BIGINT
		,CreatedDate DATETIME
		,UpdatedDate DATETIME
		,CreatedBy BIGINT
		,UpdatedBy BIGINT
		,OrganizationFriendlyFormName NVARCHAR(MAX)
		)

	INSERT INTO @OrganizationForms
	EXEC GetAdminDatabseTableData OrganizationForms

	DECLARE @OrgId NVARCHAR(MAX) = dbo.GetOrgId();

	SELECT C.ComplianceID
		,SectionName = DocumentName
		,Color = Value
		,EF.Version
		,EF.NameForUrl
		,C.IsTimeBased
		,EF.IsInternalForm
		,EF.InternalFormPath
		,FormName = IsNULL(ORGFRM.OrganizationFriendlyFormName, EF.FormLongName)
		,EF.EBFormID
		,EF.FormId
		,IsNull(EF.IsOrbeonForm, 0) AS IsOrbeonForm
	FROM Compliances C
	CROSS APPLY (
	 SELECT (SELECT STRING_AGG(CC.ComplianceID, ',') FROM Compliances CC WHERE CC.UserType = @UserType AND CC.ParentID = C.ComplianceID AND IsDeleted = 0) ChildCompliances
	) CC
	CROSS APPLY (
	 SELECT CASE WHEN EXISTS(SELECT 1 FROM ReferralDocuments RD WHERE RD.UserID = @ReferralID AND RD.ComplianceID = C.ComplianceID) THEN 1 ELSE 0 END HasDocuments,
	  CASE WHEN EXISTS(SELECT 1 FROM ReferralDocuments RD WHERE RD.UserID = @ReferralID AND ',' + CC.ChildCompliances + ',' LIKE '%,' + CONVERT(VARCHAR(MAX), RD.ComplianceID) + ',%') THEN 1 ELSE 0 END AnyChildHasDocuments
	) D
	INNER JOIN SectionPermissions SP ON SP.ComplianceID = C.ComplianceID
		AND SP.RoleID = @RoleID
	LEFT JOIN @EBForms EF ON EF.EBFormID = C.EBFormID
		AND ',' + EF.OrganizationIDs + ',' LIKE '%,' + CONVERT(VARCHAR(MAX), @OrgId) + ',%'
	LEFT JOIN (SELECT DISTINCT EBFormID, OrganizationFriendlyFormName FROM @OrganizationForms WHERE OrganizationID = @OrgId) ORGFRM ON ORGFRM.EBFormID = EF.EBFormID
	WHERE C.IsDeleted = 0
		AND ParentID = 0
		AND UserType = @UserType
		AND (C.HideIfEmpty = 0 OR D.AnyChildHasDocuments = 1 OR D.HasDocuments = 1)
		AND ISNULL(C.EmployeeID,ReferralID) = ( CASE WHEN @EmployeeID = '0' then @ReferralID ELSE @EmployeeID END)OR ISNULL(ShowToAll,1)=1
	ORDER BY c.SortingID DESC
END
