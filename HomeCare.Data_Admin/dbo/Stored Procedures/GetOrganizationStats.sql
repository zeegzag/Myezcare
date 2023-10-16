-- EXEC [GetOrganizationStats]
-- EXEC [GetOrganizationStats] '2019-03-01','2019-03-31'
CREATE PROCEDURE [dbo].[GetOrganizationStats]
	@StartDate DATETIME = NULL,
	@EndDate DATETIME = NULL
AS
BEGIN
	IF(@StartDate IS NULL)
	BEGIN
		SET @StartDate = DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-1, 0) --First day of previous month
	END

	IF(@EndDate IS NULL)
	BEGIN
		SET @EndDate = DATEADD(MONTH, DATEDIFF(MONTH, -1, GETDATE())-1, -1) --Last Day of previous month
	END

	DECLARE @ReportMonth INT = MONTH(@StartDate)
	DECLARE @ReportYear INT = YEAR(@StartDate)

	SET @EndDate = DATEADD(DAY, 1, @EndDate) -- Adding one day to get the last day records.

	IF OBJECT_ID('tempdb..#OrganizationList') IS NOT NULL 
		DROP TABLE #OrganizationList

	IF OBJECT_ID('tempdb..#OrganizationStats') IS NOT NULL 
		DROP TABLE #OrganizationStats

	IF OBJECT_ID('tempdb..#ActivePatients') IS NOT NULL 
		DROP TABLE #ActivePatients

	IF OBJECT_ID('tempdb..#DischargedPatients') IS NOT NULL 
		DROP TABLE #DischargedPatients 

	IF OBJECT_ID('tempdb..#EmployeeVisitStats') IS NOT NULL 
		DROP TABLE #EmployeeVisitStats 

	CREATE TABLE #OrganizationStats 
	(
		OrganizationID BIGINT, OrganizationName NVARCHAR(1000), ActivePatientCount BIGINT, DischargedPatientCount BIGINT, 
		EmployeeID BIGINT, ClockInTimeCount BIGINT, ClockOutTimeCount BIGINT, PCACompleteCount BIGINT, IVRClockInCount BIGINT, IVRClockOutCount BIGINT
	)
	CREATE TABLE #ActivePatients (ReferralID BIGINT)
	CREATE TABLE #DischargedPatients (DischargedPatientCount BIGINT)
	CREATE TABLE #EmployeeVisitStats (EmployeeID BIGINT, ClockInTimeCount BIGINT, ClockOutTimeCount BIGINT, PCACompleteCount BIGINT, IVRClockInCount BIGINT, IVRClockOutCount BIGINT)
	CREATE TABLE #EmployeeRecords (EmployeeID BIGINT)

	SELECT
		OrganizationID,
		DisplayName AS OrganizationName,
		DBName
	INTO
		#OrganizationList
	FROM
		Organizations

	--SELECT
	--	1 OrganizationID,
	--	'Local_Homecare' OrganizationName,
	--	'Local_Homecare' DBName
	--INTO
	--	#OrganizationList

	WHILE EXISTS (SELECT * FROM #OrganizationList)
	BEGIN
		DECLARE @CurrentOrganizationID BIGINT
		DECLARE @CurrentDBName VARCHAR(MAX) = ''
		DECLARE @CurrentOrganizationName VARCHAR(1000) = ''
		DECLARE @ActivePatientCount BIGINT
		DECLARE @DischargedPatientCount BIGINT
		DECLARE @ClockInTimeCount BIGINT
		DECLARE @ClockOutTimeCount BIGINT
		DECLARE @PCACompleteCount BIGINT
		DECLARE @IVRClockInCount BIGINT
		DECLARE @IVRClockOutCount BIGINT
		
		SELECT TOP 1
			@CurrentOrganizationID = OrganizationID,
			@CurrentDBName = DBName,
			@CurrentOrganizationName = OrganizationName
		FROM
			#OrganizationList
		ORDER BY
			OrganizationID ASC
		
		-- SET @CurrentDBName = 'Local_Homecare'
		DECLARE @DBPrefix VARCHAR(500) = @CurrentDBName + '.dbo.'

		DECLARE @Sql NVARCHAR(MAX) = ''
		SET @Sql = 
		'
			INSERT INTO
				#ActivePatients
			SELECT 
				DISTINCT ReferralID AS ReferralID			
			FROM
			(
				SELECT
					DISTINCT ReferralID 
				FROM
					' + @DBPrefix + 'Referrals
				WHERE
					CreatedDate <= ''' + CONVERT(VARCHAR(10), @EndDate, 101) + '''
					--AND (ClosureDate IS NULL OR ClosureDate > ''' + CONVERT(VARCHAR(10), @StartDate, 101) + ''') 
					AND ReferralStatusID  = 1
				UNION
				SELECT
					DISTINCT ReferralID
				FROM 
					' + @DBPrefix + 'JO_Referrals
				WHERE 
					ReferralStatusID = 1
					AND ActionDate BETWEEN ''' + CONVERT(VARCHAR(10), @StartDate, 101) + ''' AND ''' + CONVERT(VARCHAR(10), @EndDate, 101) + '''
			) AS T
			--WHERE
			--	ReferralID NOT IN
			--	(
			--		-- Get records which are activated after the date range
			--		SELECT DISTINCT ReferralID FROM ' + @DBPrefix + 'JO_Referrals WHERE ReferralStatusID = 1 AND ActionDate > ''' + CONVERT(VARCHAR(10), @EndDate, 101) + '''
			--		EXCEPT
			--		-- Get records which are still activated within the date range and skip those
			--		SELECT DISTINCT ReferralID FROM ' + @DBPrefix + 'JO_Referrals WHERE ReferralStatusID = 1 AND ActionDate < ''' + CONVERT(VARCHAR(10), @StartDate, 101) + '''					
			--	)
			--	AND ReferralID NOT IN
			--	(
			--		-- Get records which are deleted before the date range
			--		SELECT DISTINCT ReferralID FROM ' + @DBPrefix + 'JO_Referrals WHERE IsDeleted = 1 AND ActionDate < ''' + CONVERT(VARCHAR(10), @StartDate, 101) + '''
			--		EXCEPT
			--		-- Get records which are activated after the start date and skip those records
			--		SELECT DISTINCT ReferralID FROM ' + @DBPrefix + 'JO_Referrals WHERE ReferralStatusID = 1 AND ActionDate > ''' + CONVERT(VARCHAR(10), @StartDate, 101) + '''
			--	)

			INSERT INTO
				#DischargedPatients 
			SELECT
				COUNT(DISTINCT JR.ReferralID) DischargedPatients			
			FROM 
				' + @DBPrefix + 'JO_Referrals JR
				LEFT JOIN #ActivePatients AP ON JR.ReferralID = AP.ReferralID
			WHERE 
				ReferralStatusID = 4
				AND ActionDate BETWEEN ''' + CONVERT(VARCHAR(10), @StartDate, 101) + ''' AND ''' + CONVERT(VARCHAR(10), @EndDate, 101) + '''
				AND AP.ReferralID IS NULL
			
			SELECT @ActivePatientCount = COUNT(*) FROM #ActivePatients
			SELECT @DischargedPatientCount = DischargedPatientCount FROM #DischargedPatients 

			INSERT INTO
				#EmployeeRecords
			SELECT
				EmployeeID
			FROM
				' + @DBPrefix + 'Employees

			INSERT INTO
				#EmployeeVisitStats
			SELECT
				E.EmployeeID,
				ISNULL(ClockInTime,0) ClockInTime,
				ISNULL(ClockOutTime,0) ClockOutTime,
				ISNULL(IsPCACompleted,0) IsPCACompleted,
				ISNULL(IVRClockIn,0) IVRClockIn,
				ISNULL(IVRClockOut,0) IVRClockOut
			FROM
				#EmployeeRecords E
				LEFT JOIN
				(
					SELECT 
						SM.EmployeeID EmployeeID,
						COUNT(ClockInTime) ClockInTime,
						COUNT(ClockOutTime) ClockOutTime,
						COUNT(CASE WHEN IsPCACompleted = 0 THEN NULL ELSE 1 END) IsPCACompleted,
						COUNT(CASE WHEN IVRClockIn = 0 THEN NULL ELSE 1 END) IVRClockIn,
						COUNT(CASE WHEN IVRClockOut = 0 THEN NULL ELSE 1 END) IVRClockOut
					FROM 
						' + @DBPrefix + 'EmployeeVisits EV
						INNER JOIN ' + @DBPrefix + 'ScheduleMasters SM ON EV.ScheduleID = SM.ScheduleID
					WHERE
						ClockInTime BETWEEN ''' + CONVERT(VARCHAR(10), @StartDate, 101) + ''' AND ''' + CONVERT(VARCHAR(10), @EndDate, 101) + '''
					GROUP BY
						SM.EmployeeID
				) AS T ON E.EmployeeID = T.EmployeeID

			-- SELECT * FROM #EmployeeVisitStats

			DELETE FROM #ActivePatients
			DELETE FROM #DischargedPatients
		'

		PRINT(@Sql)
		exec sp_executesql @Sql, N'@ActivePatientCount BIGINT OUT, @DischargedPatientCount BIGINT OUT, @ClockInTimeCount BIGINT OUT, @ClockOutTimeCount BIGINT OUT,
		@PCACompleteCount BIGINT OUT, @IVRClockInCount BIGINT OUT, @IVRClockOutCount BIGINT OUT', 
		@ActivePatientCount OUT, @DischargedPatientCount OUT, @ClockInTimeCount OUT, @ClockOutTimeCount OUT,
		@PCACompleteCount OUT, @IVRClockInCount OUT, @IVRClockOutCount OUT

		INSERT INTO #OrganizationStats
		SELECT 
			@CurrentOrganizationID, @CurrentOrganizationName, @ActivePatientCount, @DischargedPatientCount, 
			EmployeeID, ClockInTimeCount, ClockOutTimeCount, PCACompleteCount, IVRClockInCount, IVRClockOutCount
		FROM
			#EmployeeVisitStats
		-- SELECT @CurrentOrganizationID, @CurrentOrganizationName, @ActivePatientCount, @DischargedPatientCount, @ClockInTimeCount, @ClockOutTimeCount, @PCACompleteCount, @IVRClockInCount, @IVRClockOutCount
		
		DELETE FROM #EmployeeRecords
		DELETE FROM #EmployeeVisitStats
		DELETE FROM #OrganizationList WHERE OrganizationID = @CurrentOrganizationID
	END

	MERGE OrganizationStats OS
	USING
		#OrganizationStats TOS
	ON 
		OS.OrganizationID = TOS.OrganizationID
		AND OS.[Month] = @ReportMonth
		AND OS.[Year] = @ReportYear
		AND OS.EmployeeID = TOS.EmployeeID
	WHEN NOT MATCHED BY TARGET THEN
		INSERT
		(
			OrganizationID,
			OrganizationName,
			[Month],
			[Year],
			ActivePatientCount,
			DischargedPatientCount,
			EmployeeID,
			ClockInTimeCount,
			ClockOutTimeCount,
			PCACompleteCount,
			IVRClockInCount,
			IVRClockOutCount
		)
		VALUES
		(
			OrganizationID,
			OrganizationName,
			@ReportMonth,
			@ReportYear,
			ActivePatientCount,
			DischargedPatientCount,
			EmployeeID,
			ClockInTimeCount,
			ClockOutTimeCount,
			PCACompleteCount,
			IVRClockInCount,
			IVRClockOutCount
		)
	WHEN MATCHED THEN  
	UPDATE SET   
		OS.OrganizationName = TOS.OrganizationName,
		OS.ActivePatientCount = TOS.ActivePatientCount,
		OS.DischargedPatientCount = TOS.DischargedPatientCount,
		OS.EmployeeID = TOS.EmployeeID,
		OS.ClockInTimeCount = TOS.ClockInTimeCount,
		OS.ClockOutTimeCount = TOS.ClockOutTimeCount,
		OS.PCACompleteCount = TOS.PCACompleteCount,
		OS.IVRClockInCount = TOS.IVRClockInCount,
		OS.IVRClockOutCount = TOS.IVRClockOutCount;  

	SELECT 
		*
	FROM 
		OrganizationStats
	WHERE
		[Month] = @ReportMonth
		AND [Year] = @ReportYear
END