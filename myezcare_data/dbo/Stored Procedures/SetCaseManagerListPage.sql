-- EXEC [SetCaseManagerListPage]
CREATE PROCEDURE [dbo].[SetCaseManagerListPage] 
	
AS
BEGIN
	SELECT A.AgencyID AS AgencyID, A.NickName
	FROM Agencies A Order by NickName ASC

	SELECT AL.AgencyLocationID AS AgencyLocationID, AL.LocationName
	FROM AgencyLocations AL order by LocationName desc

	-- RETURN 0 FOR THE MODEL DUE TO THE MULTIPLE ENTITY
	SELECT 0;

	-- RETURN 0 FOR THE MODEL DUE TO THE MULTIPLE ENTITY
	SELECT 0;
END

