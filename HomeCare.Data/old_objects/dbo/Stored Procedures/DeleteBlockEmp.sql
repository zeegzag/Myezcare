CREATE PROCEDURE [dbo].[DeleteBlockEmp]
@ReferralBlockedEmployeeID BIGINT,
@LoggedInID BIGINT
AS
BEGIN

  UPDATE ReferralBlockedEmployees SET IsDeleted=1,UpdatedBy=@LoggedInID,UpdatedDate=GETDATE()
  WHERE ReferralBlockedEmployeeID=@ReferralBlockedEmployeeID

END


-- SELECT * FROM ReferralBlockedEmployees
