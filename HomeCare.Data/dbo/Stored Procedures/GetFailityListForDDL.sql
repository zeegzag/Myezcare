CREATE PROCEDURE [dbo].[GetFailityListForDDL]
AS
BEGIN
SELECT Value=FacilityID,Name=FacilityName from Facilities where (IsDeleted=0) order by FacilityName ASC;       
--(IsDeleted=0 AND ParentFacilityID=0) order by FacilityName ASC;       
END