-- EXEC SaveProfileImage '','WWW',1,1,1
-- CreatedBy: Akhilesh kamal
--CreatedDate: 9/apr/2020 
--Description: For update employee profile image path in db.

CREATE PROCEDURE [dbo].[SaveProfileImage]	
@FileName varchar(MAX),
@FilePath varchar(MAX),
@EmployeeID varchar(MAX),
@LoggedInUserID varchar(MAX),
@SystemID varchar(MAX),
@ReferralID bigint = 0

AS
BEGIN
IF(@EmployeeID != 0)
BEGIN
	UPDATE EMPLOYEES SET ProfileImagePath= @FilePath WHERE EmployeeID=@EmployeeID
END
IF(@ReferralID != 0)
BEGIN
	UPDATE Referrals SET ProfileImagePath = @FilePath WHERE ReferralID=@ReferralID
END


END