
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FindNurseScheduleConflict]

	@ScheduleDayList nvarchar(max),
	@EmployeeId bigint,
	@StartTime time,
	@EndTime time,
	@NurseScheduleId bigint = NULL
AS

	DECLARE @pos INT
	DECLARE @len INT
	DECLARE @scheduleDay NVARCHAR(MAX)
	DECLARE @scheduleStartDate datetime
	DECLARE @scheduleEndDate datetime
	DECLARE @retValue int;  

	set @pos = 0
	set @len = 0

	WHILE CHARINDEX(',', @ScheduleDayList, @pos+1)>0
	BEGIN
		set @len = CHARINDEX(',', @ScheduleDayList, @pos+1) - @pos
		set @scheduleDay = SUBSTRING(@ScheduleDayList, @pos, @len)
		set @scheduleStartDate = CONVERT(DATETIME, CONVERT(varchar(20), @scheduleDay,101)  + ' ' + CONVERT(varchar(8), @StartTime, 108))
		set @scheduleEndDate = CONVERT(DATETIME, CONVERT(varchar(20), @scheduleDay,101)  + ' ' + CONVERT(varchar(8), @EndTime, 108))

		 SELECT  @retValue = COUNT(DISTINCT CONVERT(date, SM.StartDate))  
		  FROM ScheduleMasters SM  
		  WHERE  
			((DATEADD(MINUTE, 1, @scheduleStartDate) BETWEEN SM.StartDate AND SM.EndDate) OR (DATEADD(MINUTE, -1, @scheduleEndDate) BETWEEN SM.StartDate AND SM.EndDate))     
			AND SM.EmployeeID = @EmployeeId 
			AND ISNULL(SM.AnyTimeClockIn, 0) = 0
			AND SM.IsDeleted = 0 AND (NurseScheduleID != @NurseScheduleId OR @NurseScheduleId IS NULL)

		IF (@retValue > 0)
			BEGIN
				SELECT -1
				RETURN;
			END
		ELSE
			BEGIN
				set @pos = CHARINDEX(',', @ScheduleDayList, @pos+@len) +1
			END		
	END

	SELECT 0
	RETURN;
GO

