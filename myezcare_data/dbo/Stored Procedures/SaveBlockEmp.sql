CREATE PROCEDURE [dbo].[SaveBlockEmp]
@ReferralBlockedEmployeeID BIGINT,
@EmployeeID BIGINT,
@ReferralID BIGINT,
@BlockingReason VARCHAR(MAX),
@BlockingRequestedBy VARCHAR(100),
@LoggedInID BIGINT,
@SystemID VARCHAR(100)
AS
BEGIN


 IF EXISTS( SELECT 1 FROM ReferralBlockedEmployees WHERE ReferralID=@ReferralID AND IsDeleted=0 AND EmployeeID=@EmployeeID AND ReferralBlockedEmployeeID!=@ReferralBlockedEmployeeID)
  BEGIN
    SELECT -1 ; RETURN;
  END

  IF(@ReferralBlockedEmployeeID>0)
  BEGIN
    UPDATE ReferralBlockedEmployees SET
	  EmployeeID=@EmployeeID,
	  ReferralID=@ReferralID,
	  BlockingReason=@BlockingReason,
	  BlockingRequestedBy=@BlockingRequestedBy,
	  UpdatedBy=@LoggedInID,
	  UpdatedDate=GETDATE()
	  WHERE ReferralBlockedEmployeeID=@ReferralBlockedEmployeeID
  END
  ELSE 
  BEGIN 
    
	INSERT INTO  ReferralBlockedEmployees VALUES
	(@EmployeeID,@BlockingReason,@BlockingRequestedBy,@ReferralID,0,GETDATE(),@LoggedInID,GETDATE(),@LoggedInID,@SystemID)
    
  END




  SELECT 1 ; RETURN;

END
