CREATE PROCEDURE [dbo].[UpdateReferralLastAttDate]  
 @ReferralID bigint,
 @ScheduleStatusID bigint
  --@PresentStatus int
 AS  
BEGIN  
		--UPDATE Referrals set LastAttendedDate=(select max(StartDate) from AttendanceMaster where AttendanceStatus=@PresentStatus AND ReferralID=@ReferralID) where ReferralID=@ReferralID
		UPDATE Referrals set LastAttendedDate=(SELECT ISNULL(MAX(SM.StartDate),NULL) From  ScheduleMasters SM 
		WHERE SM.ReferralID=@ReferralID AND SM.ScheduleStatusID=@ScheduleStatusID  AND IsDeleted=0 )
		WHERE ReferralID=@ReferralID
END