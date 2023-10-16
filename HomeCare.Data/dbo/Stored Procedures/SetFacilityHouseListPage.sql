
CREATE PROCEDURE [dbo].[SetFacilityHouseListPage]	
AS
BEGIN   	

	SELECT DISTINCT RegionID, RegionName FROM Regions
	
	-- If we will not "return 0", error has been occurred in "GetMultipleEntity"
	SELECT 0;

	SELECT 0;

	SELECT 0;
END