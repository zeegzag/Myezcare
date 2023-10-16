-- =============================================
-- Author:		Kundan Kumar Rai
-- Create date: 10 Jan 2020
-- Description:	To get number of days to generate DMAS90 seperate for each clock in/clock out
-- =============================================
CREATE PROCEDURE [dbo].[WeeklyReportDMAS90GetDays]
	@StartDate DATETIME=NULL,
	@ReferralID BIGINT=NULL,
	@EmployeeID BIGINT = NULL,
	@Caretype BIGINT=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- Kundan Rai: 14-05-2020
	-- Added care type in result to passed correctly to generate dmas 90 report.
    SELECT sm.StartDate as ScheduleDate, EmployeeVisits.ClockInTime, EmployeeVisits.ClockOutTime, sm.CareTypeId
	FROM
	VisitTasks AS vt  
		 INNER JOIN ReferralTaskMappings AS rtm ON vt.VisitTaskID = rtm.VisitTaskID AND rtm.ReferralID=@ReferralID   
		 LEFT OUTER JOIN VisitTaskCategories ON vt.VisitTaskCategoryID = VisitTaskCategories.VisitTaskCategoryID  
		 INNER JOIN dbo.Referrals r ON rtm.ReferralID = r.ReferralID
		 INNER JOIN ScheduleMasters AS sm ON  sm.referralID = r.ReferralID  
		 INNER JOIN EmployeeVisits ON  sm.ScheduleID = EmployeeVisits.ScheduleID 
	WHERE          
	(r.ReferralID = @ReferralID) and (@EmployeeID is null or sm.EmployeeID=@EmployeeID)
	AND vt.VisitTaskType='Task'  AND dbo.EmployeeVisits.IsPCACompleted=1 AND EmployeeVisits.IsDeleted=0 
	AND (@CareType IS NULL OR @CareType=0 OR (vt.CareType=@CareType AND sm.CareTypeId=@CareType))
	AND (cast(sm.StartDate as date) >= @StartDate AND cast(sm.StartDate as date) <=DATEADD(DAY, 8 - DATEPART(WEEKDAY, @StartDate), CAST(@StartDate AS DATE)))  
	group by 
		sm.StartDate, EmployeeVisits.ClockInTime, EmployeeVisits.ClockOutTime,sm.CareTypeId
	order by 
		sm.StartDate
END