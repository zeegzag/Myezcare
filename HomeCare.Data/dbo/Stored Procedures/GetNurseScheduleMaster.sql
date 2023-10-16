-- =============================================    
-- Author:  Ali H    
-- Create date: 21 Nov 2020    
-- Description: <Description,,>    
-- =============================================    
CREATE PROCEDURE [dbo].[GetNurseScheduleMaster]  
  @CareTypeIds VARCHAR(MAX),  
  @EmployeeIds VARCHAR(MAX),  
  @ReferralIds VARCHAR(MAX)  
AS  
BEGIN 
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
  SELECT ScheduleID,  
    SM.ReferralID,  
    SM.EmployeeID,  
    NS.FrequencyChoice,  
    NS.Frequency,  
    NS.DaysOfWeek,  
    NS.[DayOfMonth],  
    NS.IsMonthlyDaySelection,  
    NS.DailyInterval,  
    NS.WeeklyInterval,  
    NS.MonthlyInterval,  
    NS.IsSundaySelected,  
    NS.IsMondaySelected,  
    NS.IsTuesdaySelected,  
    NS.IsWednesdaySelected,  
    NS.IsThursdaySelected,  
    NS.IsFridaySelected,  
    NS.IsSaturdaySelected,  
    NS.IsFirstWeekOfMonthSelected,  
    NS.IsSecondWeekOfMonthSelected,  
    NS.IsThirdWeekOfMonthSelected,  
    NS.IsFourthWeekOfMonthSelected,  
    NS.IsLastWeekOfMonthSelected,  
    NS.FrequencyTypeOptions,  
    NS.ScheduleRecurrence,  
    NS.MonthlyIntervalOptions,  
    SM.CareTypeId,  
    dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS EmployeeFullName,  
    dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) AS PatientFullName,  
    SM.StartDate,  
    SM.EndDate,  
    NS.DaysOfWeekOptions,  
    SM.PayorID,  
    SM.ReferralBillingAuthorizationID,  
    SM.IsVirtualVisit,  
    P.PayorName,  
    CT.Title AS CareType,  
    NS.Notes,  
    NS.IsAnyDay,  
    SM.AnyTimeClockIn,  
    SM.StartTime AS ClockInStartTime,  
    SM.EndTime AS ClockInEndTime,  
    NS.AnniversaryDay,  
    NS.AnniversaryMonth,  
    SM.NurseScheduleID,  
    NS.StartDate AS ScheduleStartDate,  
    NS.EndDate AS ScheduleEndDate  
  FROM ScheduleMasters SM  
  INNER JOIN Employees E  
    ON SM.EmployeeId = E.EmployeeId  
  INNER JOIN Referrals R  
    ON SM.ReferralID = R.ReferralID  
  INNER JOIN NurseSchedules NS  
    ON SM.NurseScheduleID = NS.NurseScheduleID  
  LEFT JOIN Payors P  
    ON SM.PayorID = P.PayorID  
  INNER JOIN DDMaster CT  
    ON SM.CareTypeID = CT.DDMasterID  
  WHERE (  
      LEN(ISNULL(@CareTypeIds, '')) = 0  
      OR SM.CareTypeId IN (  
        SELECT Item  
        FROM dbo.SplitString(@CareTypeIds, ',')  
        )  
      )  
    AND (  
      LEN(ISNULL(@EmployeeIds, '')) = 0  
      OR SM.EmployeeId IN (  
        SELECT Item  
        FROM dbo.SplitString(@EmployeeIds, ',')  
        )  
      )  
    AND (  
      LEN(ISNULL(@ReferralIds, '')) = 0  
      OR SM.ReferralId IN (  
        SELECT Item  
        FROM dbo.SplitString(@ReferralIds, ',')  
        )  
      )  
    AND SM.IsDeleted = 0  
END  