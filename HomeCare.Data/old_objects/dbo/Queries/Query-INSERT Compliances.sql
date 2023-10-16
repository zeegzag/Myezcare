SET IDENTITY_INSERT Compliances ON
INSERT INTO Compliances (ComplianceID, UserType, DocumentationType, DocumentName, IsTimeBased, IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate, SystemID, ParentID, Type, Value)
VALUES (-4, 2, 1, 'New Referral', 0, 0, 1, 2020-12-02, 1, 2020-12-02, 11, 0, 'Directory', '#0d1259');
SET IDENTITY_INSERT Compliances OFF

--For Mapping Role for Super Admin
--declare @SectionPermissionID bigint
--select @SectionPermissionID= coalesce(MAX(SectionPermissionID),0) + 1  from SectionPermissions
insert into SectionPermissions values(-4,1)