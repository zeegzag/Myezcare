/*
Created by : Pallav Saxena
Created Date : 06/06/2019
Purpose: Create the summary and get the summary for the billing purposes. This procedure summarizes the data that is going to be populated in the organizationStats
table using scheduled job running. The Stored procedure that is executed to populate organizationstats is GetOrganizationStats.
Exec GetOrganizationUsageSummary 'Asap Care Billing'

*/

Create Procedure GetOrganizationUsageSummary(@OrganizationName varchar(255)=null, @StartDate Date=null,@EndDate Date=null)
	as
	
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

	SET @EndDate = DATEADD(DAY, 1, @EndDate)


	SELECT 
		OrganizationName, ActivePatientCount,DischargedPatientCount,sum(ClockInTimeCount) TotalClockIn,sum(ClockOutTimeCount) TotalClockOut,sum(PCACompleteCount) TotalIsPCACompleted,sum(IVRClockInCount) TotalIVRClockIn,sum(IVRClockOutCount) IVRClockOut
	FROM 
		OrganizationStats
Group by OrganizationName, ActivePatientCount,DischargedPatientCount,[Month],[YEAR]
	Having
		[Month] = @ReportMonth
		AND [Year] = @ReportYear
		and (@OrganizationName is null or OrganizationName=@OrganizationName)