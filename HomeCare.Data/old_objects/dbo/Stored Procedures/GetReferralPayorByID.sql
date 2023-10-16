-- EXEC GetReferralPayorByID @@ReferralPayorMappingID = '2892'
CREATE PROCEDURE [dbo].[GetReferralPayorByID]
@ReferralPayorMappingID BIGINT
AS
BEGIN
 
 SELECT RPM.*, RPM.PayorID AS TempPayorID          
 FROM ReferralPayorMappings RPM          
 WHERE  RPM.ReferralPayorMappingID =@ReferralPayorMappingID   
 
 
END
