-- GetSBSEmployeeList NULL,NULL,'1305229'
CREATE PROCEDURE [dbo].[GetSBSEmployeeList]
	@EmployeeName NVARCHAR(100),
	@MobileNumber NVARCHAR(10),
	@ReferralTsDateID BIGINT
AS
BEGIN
	DECLARE @TmpTable TABLE (EmployeeId BIGINT)

	IF(@ReferralTsDateID != 0)
		BEGIN
			DECLARE @ReferralTSStartTime DATETIME
			DECLARE @ReferralTSEndTime DATETIME

			SELECT @ReferralTSStartTime=rtsd.ReferralTSStartTime, @ReferralTSEndTime=rtsd.ReferralTSEndTime
			FROM ReferralTimeSlotDates rtsd
			WHERE ReferralTsDateID=@ReferralTsDateID

			INSERT INTO @TmpTable (EmployeeId)
			SELECT DISTINCT SM.EmployeeID 
			FROM EmployeeTimeSlotDates ETSD
			INNER JOIN ScheduleMasters SM ON SM.EmployeeTSDateID=ETSD.EmployeeTSDateID AND SM.IsDeleted=0 
			WHERE 
			@ReferralTSStartTime BETWEEN ETSD.EmployeeTSStartTime AND ETSD.EmployeeTSEndTime OR 
			@ReferralTSEndTime BETWEEN ETSD.EmployeeTSStartTime AND ETSD.EmployeeTSEndTime OR
			EmployeeTSStartTime BETWEEN @ReferralTSStartTime AND @ReferralTSEndTime  OR 
			EmployeeTSEndTime BETWEEN @ReferralTSStartTime AND @ReferralTSEndTime

			--EmployeeTSStartTime BETWEEN @ReferralTSStartTime AND @ReferralTSEndTime 
			--	OR 
			--EmployeeTSEndTime BETWEEN @ReferralTSStartTime AND @ReferralTSEndTime;
		END

	SELECT E.EmployeeID, E.MobileNumber, E.FirstName, E.LastName,CASE WHEN LEN(E.FcmTokenId) > 0 THEN 1 ELSE 0 END IsAbleToReceiveNotification
	FROM Employees E 
	WHERE E.IsDeleted=0 AND E.IsActive=1
	AND (@ReferralTsDateID = 0 OR E.EmployeeID NOT IN (SELECT EmployeeId FROM @TmpTable))
	AND (
			(@EmployeeName IS NULL OR LEN(@EmployeeName)=0)
				OR 
			(  
				(E.FirstName LIKE '%'+@EmployeeName+'%' ) OR
				(E.LastName  LIKE '%'+@EmployeeName+'%') OR
				(E.FirstName +' '+E.LastName like '%'+@EmployeeName+'%') OR
				(E.LastName +' '+E.FirstName like '%'+@EmployeeName+'%') OR
				(E.FirstName +', '+E.LastName like '%'+@EmployeeName+'%') OR
				(E.LastName +', '+E.FirstName like '%'+@EmployeeName+'%')
			)
	)
	AND ((@MobileNumber IS NULL OR LEN(@MobileNumber)=0) OR (E.MobileNumber LIKE '%' + @MobileNumber + '%'))
	ORDER BY E.LastName ASC
END
