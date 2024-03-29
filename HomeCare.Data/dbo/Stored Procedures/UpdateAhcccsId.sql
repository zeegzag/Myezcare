﻿-- EXEC UpdateAhcccsId @ReferralID = '2892', @AHCCCSID = 'A42347867', @NewAHCCCSID = 'A42347867'
CREATE PROCEDURE [dbo].[UpdateAhcccsId]
@ReferralID BIGINT,
@AHCCCSID VARCHAR(20),
@NewAHCCCSID VARCHAR(20)
AS
BEGIN

  IF (LEN(@NewAHCCCSID)<9 OR @NewAHCCCSID=NULL) 
  SELECT 3;
  ELSE IF EXISTS(SELECT 1 FROM Referrals WHERE ReferralID!=@ReferralID AND AHCCCSID=@NewAHCCCSID)
  SELECT 1;
  ELSE IF EXISTS(SELECT 1 FROM Referrals WHERE ReferralID=@ReferralID AND AHCCCSID=@NewAHCCCSID)
  SELECT 2;
  ELSE 
   BEGIN
    UPDATE Referrals SET AHCCCSID=@NewAHCCCSID  WHERE AHCCCSID=@AHCCCSID AND  ReferralID=@ReferralID
    UPDATE ReferralDocumentUploadStatuses SET AHCCCSID=@NewAHCCCSID  WHERE AHCCCSID=@AHCCCSID AND  ReferralID=@ReferralID
    UPDATE Notes SET AHCCCSID=@NewAHCCCSID  WHERE AHCCCSID=@AHCCCSID AND  ReferralID=@ReferralID

	DECLARE @ClientID BIGINT=0;
	SELECT @ClientID=ClientID FROM Referrals WHERE AHCCCSID=@AHCCCSID AND  ReferralID=@ReferralID
	UPDATE Clients SET AHCCCSID=@NewAHCCCSID  WHERE ClientID=@ClientID

	SELECT 4;
   END

END


-- SELECT * FROM Referrals WHERE Firstname='Ashish'
--SELECT * FROM NOTES WHERE  ReferralID=2892