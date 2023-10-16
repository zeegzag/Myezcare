-- EXEC GetDTRDetailsList 'Ca',3,10
CREATE PROCEDURE [dbo].[GetDTRDetailsList]
@Term varchar(100),
@Type int,
@PageSize int
AS
BEGIN

	IF(@Type=1)
	BEGIN
		SELECT TOP(@PageSize) VehicleNumber AS Name FROM DTRDetails WHERE VehicleNumber LIKE '%'+@Term+'%' AND IsDeleted=0
	END
	ELSE IF(@Type=2)
	BEGIN
		SELECT TOP(@PageSize) VehicleType AS Name FROM DTRDetails WHERE VehicleType LIKE '%'+@Term+'%' AND IsDeleted=0
	END
	ELSE IF(@Type=3)
	BEGIN
	   SELECT TOP(@PageSize) LocationAddress AS Name FROM DTRDetails WHERE LocationAddress LIKE '%'+@Term+'%' AND IsDeleted=0
	END

END