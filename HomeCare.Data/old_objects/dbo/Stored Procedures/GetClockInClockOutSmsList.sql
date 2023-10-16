-- =============================================    
-- Author:  Ankita Dobariya    
-- Create date: 31-March- 2018   
-- Description: this method will return the list of scheduler which has check in and check out not done yet 
-- =============================================    
--EXEC GetClockInClockOutSmsList     
CREATE PROCEDURE [dbo].[GetClockInClockOutSmsList] 
@Date DATETIME,
@ScheduleStatusID INT,
@TimeDifference TIME,
@ReferralStatusID INT,
@RoleId INT,
@MaxAttemptCount INT,
@StatusInProgress INT,
@Type_ClockIn NVARCHAR(10),
@Type_ClockOut NVARCHAR(10),
@ClockInMessageEmp NVARCHAR(Max),
@ClockOutMessageEmp NVARCHAR(Max),
@ClockInMessageAdmin NVARCHAR(Max),
@ClockOutMessageAdmin NVARCHAR(Max),
@StatusFail INT 
 -- Add the parameters for the stored procedure here    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
  
  -- GET LIST FOR CLOCK IN

    SELECT SM.ScheduleID, E.EmployeeID, REPLACE(@ClockInMessageEmp,'##PatientName##', LTRIM(COALESCE(' ' + R.FirstName, '') +COALESCE(' ' + R.MiddleName, '') +COALESCE(' ' + R.LastName,'')) ) AS MESSAGE, E.MobileNumber,@Type_ClockIn AS TYPE
	FROM ScheduleMasters SM
	INNER JOIN Employees E ON E.EmployeeID = SM.EmployeeID
	INNER JOIN Referrals  R ON R.ReferralID = SM.ReferralID
	INNER JOIN ReferralStatuses RS ON RS.ReferralStatusID=R.ReferralStatusID
	LEFT JOIN EmployeeVisits EV ON EV.ScheduleID = SM.ScheduleID
	WHERE CONVERT(DATE,SM.StartDate) = CONVERT(DATE,@Date) AND
	SM.IsDeleted = 0 AND SM.ScheduleStatusID = @ScheduleStatusID
	AND E.IsActive=1 AND E.IsDeleted=0 AND R.IsDeleted=0 AND R.ReferralStatusID=@ReferralStatusID AND RS.UsedInHomeCare=1
	AND (EV.EmployeeVisitID IS NULL OR EV.ClockInTime IS NULL ) 
	AND (DATEDIFF(MINUTE, SM.StartDate, @Date) >= DATEPART(MINUTE, @TimeDifference))
	AND NOT EXISTS (SELECT 1 FROM EVV_SmsLogs WHERE ScheduleID = SM.ScheduleID AND Type =@Type_ClockIn)

	UNION

	SELECT  L.ScheduleID, E.EmployeeID, 
	 REPLACE(
	 REPLACE(@ClockInMessageAdmin,'##PatientName##', LTRIM(COALESCE(' ' + L.FirstName, '') +COALESCE(' ' + L.MiddleName, '') +COALESCE(' ' + L.LastName,'')) ),'##EmployeeName##',
	 LTRIM(COALESCE(' ' + L.EFirstName, '') +COALESCE(' ' + L.EMiddleName, '') +COALESCE(' ' + L.ELastName,''))
	 ) AS MESSAGE,E.MobileNumber,@Type_ClockIn AS TYPE
	FROM Employees  E
	CROSS JOIN
	(
		SELECT SM.ScheduleID, R.FirstName, R.LastName, R.MiddleName , E.LastName AS ElastName, E.FirstName AS EFirstName,E.MiddleName AS EMiddleName
		FROM ScheduleMasters SM
		INNER JOIN Employees E ON E.EmployeeID = SM.EmployeeID
		INNER JOIN Referrals  R ON R.ReferralID = SM.ReferralID
		INNER JOIN ReferralStatuses RS ON RS.ReferralStatusID=R.ReferralStatusID
		LEFT JOIN EmployeeVisits EV ON EV.ScheduleID = SM.ScheduleID
		WHERE CONVERT(DATE,SM.StartDate) = CONVERT(DATE,@Date) AND
		SM.IsDeleted = 0 AND SM.ScheduleStatusID = @ScheduleStatusID
		AND E.IsActive=1 AND E.IsDeleted=0 AND R.IsDeleted=0 AND R.ReferralStatusID=@ReferralStatusID AND RS.UsedInHomeCare=1
		AND (EV.EmployeeVisitID IS NULL OR EV.ClockInTime IS NULL ) 
		AND (DATEDIFF(MINUTE, SM.StartDate, @Date) >= DATEPART(MINUTE, @TimeDifference))
		AND NOT EXISTS (SELECT 1 FROM EVV_SmsLogs WHERE ScheduleID = SM.ScheduleID AND Type =@Type_ClockIn)
	) AS L
	WHERE E.RoleID = @RoleId AND E.IsActive=1 AND E.IsDeleted=0

	UNION 


	SELECT SM.ScheduleID, E.EmployeeID, REPLACE(@ClockOutMessageEmp,'##PatientName##', LTRIM(COALESCE(' ' + R.FirstName, '') +COALESCE(' ' + R.MiddleName, '') +COALESCE(' ' + R.LastName,'')) ) AS MESSAGE, E.MobileNumber,@Type_ClockOut AS TYPE
	FROM ScheduleMasters SM
	INNER JOIN Employees E ON E.EmployeeID = SM.EmployeeID
	INNER JOIN Referrals  R ON R.ReferralID = SM.ReferralID
	INNER JOIN ReferralStatuses RS ON RS.ReferralStatusID=R.ReferralStatusID
	INNER JOIN EmployeeVisits EV ON EV.ScheduleID = SM.ScheduleID
	WHERE CONVERT(DATE,SM.EndDate) = CONVERT(DATE,@Date) AND
	SM.IsDeleted = 0 AND SM.ScheduleStatusID = @ScheduleStatusID
	AND E.IsActive=1 AND E.IsDeleted=0 AND R.IsDeleted=0 AND R.ReferralStatusID=@ReferralStatusID AND RS.UsedInHomeCare=1
	AND ( EV.ClockOutTime IS NULL ) 
	AND (DATEDIFF(MINUTE, SM.EndDate, @Date) >= DATEPART(MINUTE, @TimeDifference))
	AND NOT EXISTS (SELECT 1 FROM EVV_SmsLogs WHERE ScheduleID = SM.ScheduleID AND Type =@Type_ClockOut)

	SELECT  L.ScheduleID, E.EmployeeID,
	REPLACE(
	 REPLACE(@ClockOutMessageAdmin,'##PatientName##', LTRIM(COALESCE(' ' + L.FirstName, '') +COALESCE(' ' + L.MiddleName, '') +COALESCE(' ' + L.LastName,'')) ),'##EmployeeName##',
	 LTRIM(COALESCE(' ' + L.EFirstName, '') +COALESCE(' ' + L.EMiddleName, '') +COALESCE(' ' + L.ELastName,''))
	 ) AS MESSAGE,E.MobileNumber,@Type_ClockOut AS TYPE
	FROM Employees  E
	CROSS JOIN
	(
		SELECT SM.ScheduleID, R.FirstName, R.LastName, R.MiddleName , E.LastName AS ElastName, E.FirstName AS EFirstName,E.MiddleName AS EMiddleName
		FROM ScheduleMasters SM
		INNER JOIN Employees E ON E.EmployeeID = SM.EmployeeID
		INNER JOIN Referrals  R ON R.ReferralID = SM.ReferralID
		INNER JOIN ReferralStatuses RS ON RS.ReferralStatusID=R.ReferralStatusID
		INNER JOIN EmployeeVisits EV ON EV.ScheduleID = SM.ScheduleID
		WHERE CONVERT(DATE,SM.EndDate) = CONVERT(DATE,@Date) AND
		SM.IsDeleted = 0 AND SM.ScheduleStatusID = @ScheduleStatusID
		AND E.IsActive=1 AND E.IsDeleted=0 AND R.IsDeleted=0 AND R.ReferralStatusID=@ReferralStatusID AND RS.UsedInHomeCare=1
		AND ( EV.ClockOutTime IS NULL ) 
		AND (DATEDIFF(MINUTE, SM.EndDate, @Date) >= DATEPART(MINUTE, @TimeDifference))
		AND NOT EXISTS (SELECT 1 FROM EVV_SmsLogs WHERE ScheduleID = SM.ScheduleID AND Type =@Type_ClockOut)
	) AS L
	WHERE E.RoleID = @RoleId AND E.IsActive=1 AND E.IsDeleted=0

	UNION

	SELECT  L.ScheduleID, L.EmployeeID,
    CASE E.RoleID WHEN 1 THEN
				CASE l.Type WHEN @Type_ClockIn  THEN 
					REPLACE(
					REPLACE(@ClockInMessageAdmin,'##PatientName##', LTRIM(COALESCE(' ' + R.FirstName, '') +COALESCE(' ' + R.MiddleName, '') +COALESCE(' ' + R.LastName,'')) ),'##EmployeeName##',
					LTRIM(COALESCE(' ' + SME.FirstName, '') +COALESCE(' ' + SME.MiddleName, '') +COALESCE(' ' + SME.LastName,''))
					) 
				ELSE
				REPLACE(
					REPLACE(@ClockOutMessageAdmin,'##PatientName##', LTRIM(COALESCE(' ' + R.FirstName, '') +COALESCE(' ' + R.MiddleName, '') +COALESCE(' ' + R.LastName,'')) ),'##EmployeeName##',
					LTRIM(COALESCE(' ' + SME.FirstName, '') +COALESCE(' ' + SME.MiddleName, '') +COALESCE(' ' + SME.LastName,''))
					)
					END
	ELSE
		CASE L.Type WHEN @Type_ClockIn  THEN 
			REPLACE(@ClockInMessageEmp,'##PatientName##', LTRIM(COALESCE(' ' + R.FirstName, '') +COALESCE(' ' + R.MiddleName, '') +COALESCE(' ' + R.LastName,'')) )
		ELSE
    		REPLACE(@ClockOutMessageEmp,'##PatientName##', LTRIM(COALESCE(' ' + R.FirstName, '') +COALESCE(' ' + R.MiddleName, '') +COALESCE(' ' + R.LastName,'')) )
			END
	END AS Message, E.MobileNumber,L.Type
	FROM EVV_SmsLogs L
	INNER JOIN ScheduleMasters SM ON SM.ScheduleID = L.ScheduleID
	INNER JOIN Employees E ON E.EmployeeID = L.EmployeeID 
	INNER JOIN Referrals  R ON R.ReferralID = SM.ReferralID
	INNER JOIN Employees SME ON SME.EmployeeID = SM.EmployeeID
	WHERE L.SmsStatusId =@StatusFail AND L.AttemptCount < @MaxAttemptCount


	-- INSERT INTO EVV_SmsLogs Table FOR CLOCK IN

	INSERT INTO EVV_SmsLogs (SmsStatusId,Type,ScheduleID,EmployeeID,ErrorMessage,AttemptCount)
	SELECT @StatusInProgress, @Type_ClockIn,SM.ScheduleID, E.EmployeeID, NULL,0
	FROM ScheduleMasters SM
	INNER JOIN Employees E ON E.EmployeeID = SM.EmployeeID
	INNER JOIN Referrals  R ON R.ReferralID = SM.ReferralID
	INNER JOIN ReferralStatuses RS ON RS.ReferralStatusID=R.ReferralStatusID
	LEFT JOIN EmployeeVisits EV ON EV.ScheduleID = SM.ScheduleID
	WHERE CONVERT(DATE,SM.StartDate) = CONVERT(DATE,@Date) AND
	SM.IsDeleted = 0 AND SM.ScheduleStatusID = @ScheduleStatusID
	AND E.IsActive=1 AND E.IsDeleted=0 AND R.IsDeleted=0 AND R.ReferralStatusID=@ReferralStatusID AND RS.UsedInHomeCare=1
	AND (EV.EmployeeVisitID IS NULL OR EV.ClockInTime IS NULL ) 
	AND (DATEDIFF(MINUTE, SM.StartDate, @Date) >= DATEPART(MINUTE, @TimeDifference))
	AND NOT EXISTS (SELECT 1 FROM EVV_SmsLogs WHERE ScheduleID = SM.ScheduleID AND Type =@Type_ClockIn)


	INSERT INTO EVV_SmsLogs (SmsStatusId,Type,ScheduleID,EmployeeID,ErrorMessage,AttemptCount)
	SELECT @StatusInProgress, @Type_ClockIn,l.ScheduleID, E.EmployeeID, NULL,0
	FROM Employees  E
	CROSS JOIN
	(
		SELECT SM.ScheduleID, E.EmployeeID,@ClockInMessageAdmin AS MESSAGE, E.MobileNumber
		FROM ScheduleMasters SM
		INNER JOIN Employees E ON E.EmployeeID = SM.EmployeeID
		INNER JOIN Referrals  R ON R.ReferralID = SM.ReferralID
		INNER JOIN ReferralStatuses RS ON RS.ReferralStatusID=R.ReferralStatusID
		LEFT JOIN EmployeeVisits EV ON EV.ScheduleID = SM.ScheduleID
		WHERE CONVERT(DATE,SM.StartDate) = CONVERT(DATE,@Date) AND
		SM.IsDeleted = 0 AND SM.ScheduleStatusID = @ScheduleStatusID
		AND E.IsActive=1 AND E.IsDeleted=0 AND R.IsDeleted=0 AND R.ReferralStatusID=@ReferralStatusID AND RS.UsedInHomeCare=1
		AND (EV.EmployeeVisitID IS NULL OR EV.ClockInTime IS NULL ) 
		AND (DATEDIFF(MINUTE, SM.StartDate, @Date) >= DATEPART(MINUTE, @TimeDifference))
		AND NOT EXISTS (SELECT 1 FROM EVV_SmsLogs WHERE ScheduleID = SM.ScheduleID AND Type =@Type_ClockIn)
	) AS L
	WHERE E.RoleID = @RoleId AND E.IsActive=1 AND E.IsDeleted=0


	-- INSERT INTO EVV_SmsLogs Table For CLOCK OUT

	INSERT INTO EVV_SmsLogs (SmsStatusId,Type,ScheduleID,EmployeeID,ErrorMessage,AttemptCount)
	SELECT @StatusInProgress, @Type_ClockIn,SM.ScheduleID, E.EmployeeID, NULL,0
	FROM ScheduleMasters SM
	INNER JOIN Employees E ON E.EmployeeID = SM.EmployeeID
	INNER JOIN Referrals  R ON R.ReferralID = SM.ReferralID
	INNER JOIN ReferralStatuses RS ON RS.ReferralStatusID=R.ReferralStatusID
	LEFT JOIN EmployeeVisits EV ON EV.ScheduleID = SM.ScheduleID
	WHERE CONVERT(DATE,SM.EndDate) = CONVERT(DATE,@Date) AND
	SM.IsDeleted = 0 AND SM.ScheduleStatusID = @ScheduleStatusID
	AND E.IsActive=1 AND E.IsDeleted=0 AND R.IsDeleted=0 AND R.ReferralStatusID=@ReferralStatusID AND RS.UsedInHomeCare=1
	AND (EV.EmployeeVisitID IS NULL OR EV.ClockInTime IS NULL ) 
	AND (DATEDIFF(MINUTE, SM.EndDate, @Date) >= DATEPART(MINUTE, @TimeDifference))
	AND NOT EXISTS (SELECT 1 FROM EVV_SmsLogs WHERE ScheduleID = SM.ScheduleID AND Type =@Type_ClockOut)

	INSERT INTO EVV_SmsLogs (SmsStatusId,Type,ScheduleID,EmployeeID,ErrorMessage,AttemptCount)
	SELECT @StatusInProgress, @Type_ClockIn,l.ScheduleID, E.EmployeeID, NULL,0
	FROM Employees  E
	 CROSS JOIN
	(
		SELECT SM.ScheduleID, E.EmployeeID,@ClockInMessageAdmin AS MESSAGE, E.MobileNumber
		FROM ScheduleMasters SM
		INNER JOIN Employees E ON E.EmployeeID = SM.EmployeeID
		INNER JOIN Referrals  R ON R.ReferralID = SM.ReferralID
		INNER JOIN ReferralStatuses RS ON RS.ReferralStatusID=R.ReferralStatusID
		LEFT JOIN EmployeeVisits EV ON EV.ScheduleID = SM.ScheduleID
		WHERE CONVERT(DATE,SM.StartDate) = CONVERT(DATE,@Date) AND
		SM.IsDeleted = 0 AND SM.ScheduleStatusID = @ScheduleStatusID
		AND E.IsActive=1 AND E.IsDeleted=0 AND R.IsDeleted=0 AND R.ReferralStatusID=@ReferralStatusID AND RS.UsedInHomeCare=1
		AND (EV.EmployeeVisitID IS NULL OR EV.ClockInTime IS NULL ) 
		AND (DATEDIFF(MINUTE, SM.StartDate, @Date) >= DATEPART(MINUTE, @TimeDifference))
		AND NOT EXISTS (SELECT 1 FROM EVV_SmsLogs WHERE ScheduleID = SM.ScheduleID AND Type =@Type_ClockIn)
	) AS L
	WHERE E.RoleID = @RoleId AND E.IsActive=1 AND E.IsDeleted=0

END
