CREATE PROCEDURE [dbo].[CheckDTRDetails]
@VehicleNumber varchar(100),
@VehicleType  varchar(100),
@PickUpAddress  varchar(500),
@DropOffAddress  varchar(500)
AS
BEGIN

 IF (SELECT COUNT(*) FROM DTRDetails WHERE VehicleNumber=@VehicleNumber AND IsDeleted=0) = 0
 BEGIN
	INSERT INTO DTRDetails(VehicleNumber,DTRDetailType) Values(@VehicleNumber,1)
 END

 IF (SELECT COUNT(*) FROM DTRDetails WHERE VehicleType=@VehicleType AND IsDeleted=0) = 0
 BEGIN
	INSERT INTO DTRDetails(VehicleType,DTRDetailType) Values(@VehicleType,1)
 END

 IF (SELECT COUNT(*) FROM DTRDetails WHERE LocationAddress=@PickUpAddress AND IsDeleted=0) = 0
 BEGIN
	INSERT INTO DTRDetails(LocationAddress,DTRDetailType) Values(@PickUpAddress,1)
 END

 IF (SELECT COUNT(*) FROM DTRDetails WHERE LocationAddress=@DropOffAddress AND IsDeleted=0) = 0
 BEGIN
	INSERT INTO DTRDetails(LocationAddress,DTRDetailType) Values(@DropOffAddress,1)
 END

END