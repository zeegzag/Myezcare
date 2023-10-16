-- EXEC SetNoteListPage @ReferralID = '0'
CREATE PROCEDURE [dbo].[SetNoteListPage] 
@ReferralID bigint
AS
BEGIN
	select sc.*,psm.PosID,pos.PosName,psm.PayorServiceCodeMappingID from PayorServiceCodeMapping psm
	inner join ServiceCodes sc on sc.ServiceCodeID=psm.ServiceCodeID
	inner join PlaceOfServices pos on pos.PosID=psm.PosID
	Where  (@ReferralID=0 OR psm.PayorID in(select PayorID from ReferralPayorMappings where ReferralID=@ReferralID and IsActive=1 and IsDeleted=0)) and psm.IsDeleted=0;

	select * from ServiceCodeTypes where IsDeleted=0


	SELECT Value=FacilityID,Name=FacilityName FROM Facilities WHERE   ParentFacilityID=0   Order BY FacilityName ASC
	
	SELECT Value=RegionID,Name=RegionName FROM Regions  Order BY RegionName ASC

	SELECT Value=DepartmentID,Name=DepartmentName FROM Departments  Order by DepartmentName ASC

	SELECT Value=EmployeeID,Name=LastName+', '+FirstName  FROM Employees  Order by LastName ASC

	SELECT Value=PayorID,Name=ShortName  FROM Payors  Order by PayorName ASC
END
