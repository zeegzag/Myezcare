CREATE PROCEDURE [dbo].[SetAttendanceMasterPage]
AS
BEGIN
SELECT * from ScheduleStatuses Order by ScheduleStatusName
SELECT RegionID,RegionName from Regions
SELECT FacilityID,FacilityName,IsDeleted from Facilities WHERE IsDeleted=0 ORDER BY FacilityName ASC
END
