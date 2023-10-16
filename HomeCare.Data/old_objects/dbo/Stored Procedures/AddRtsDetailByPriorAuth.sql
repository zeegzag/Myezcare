CREATE PROCEDURE [dbo].[AddRtsDetailByPriorAuth]
	@ReferralTimeSlotDetailID BIGINT,     
	@ReferralTimeSlotMasterID BIGINT,
	@ReferralID BIGINT,
	@StartTime TIME(7),        
	@EndTime TIME(7),
	@UsedInScheduling BIT,
	@Notes NVARCHAR(1000),   
	@loggedInUserId BIGINT,       
	@SystemID VARCHAR(100),
	@SelectedDays VARCHAR(20),
	@TodayDate DATE,
	@SlotEndDate DATE,
	@CareTypeId BIGINT = NULL,
	@GeneratePatientSchedules BIT
AS
BEGIN
	IF (OBJECT_ID('tempdb..#DistinctDays') IS NOT NULL)
		DROP TABLE #DistinctDays

	CREATE TABLE #DistinctDays(Id INT IDENTITY(1,1), DayNumber INT)

	;WITH DateRange(DateData) AS 
	(
		SELECT @TodayDate as Date
		UNION ALL
		SELECT DATEADD(d,1,DateData)
		FROM DateRange 
		WHERE DateData < @SlotEndDate
	)

	INSERT INTO #DistinctDays
	SELECT 
		DISTINCT DATEPART(WEEKDAY, DateData) DayNumber-- , DateData
	FROM 
		DateRange
	-- WHERE DATEPART(WEEKDAY, DateData) NOT IN (7, 1)
	OPTION (MAXRECURSION 0)

	DECLARE @CSVDays VARCHAR(100)
	SET @CSVDays = (SELECT SUBSTRING((SELECT ',' + CONVERT(VARCHAR(10), s.DayNumber) FROM #DistinctDays s ORDER BY s.Id FOR XML PATH('')),2,200000) AS CSV)
	-- PRINT(@CSVDays)

	EXEC AddRtsDetail_CaseManagement @ReferralTimeSlotDetailID = @ReferralTimeSlotDetailID, @ReferralTimeSlotMasterID = @ReferralTimeSlotMasterID,
	@ReferralID = @ReferralID, @Day= 0, @StartTime = @StartTime, @EndTime = @EndTime, @UsedInScheduling =@UsedInScheduling,
	@Notes = @Notes, @loggedInUserId = @loggedInUserId, @SystemID = @SystemID, @SelectedDays = @CSVDays, @TodayDate = @TodayDate,
	@SlotEndDate = @SlotEndDate, @CareTypeId = @CareTypeId, @GeneratePatientSchedules = @GeneratePatientSchedules
END
