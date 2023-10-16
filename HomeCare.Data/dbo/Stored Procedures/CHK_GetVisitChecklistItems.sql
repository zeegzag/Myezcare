-- CHK_GetVisitChecklistItems '2018-01-01','2018-12-31',30,24237  
-- CHK_GetVisitChecklistItems '2018-01-01','2019-12-31',30,24254  
CREATE PROCEDURE [dbo].[CHK_GetVisitChecklistItems]  
 @StartDate DATETIME,  
 @EndDate DATETIME,  
 @NoOfDays INT,  
 @ReferralID BIGINT  
AS  
BEGIN  
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
 DECLARE @IsCareTypeAssigned BIT  
 SELECT  
  @IsCareTypeAssigned = CASE WHEN CareTypeIds IS NOT NULL THEN 1 ELSE 0 END  
 FROM  
  Referrals  
 WHERE  
  ReferralID = @ReferralID  
  
 SELECT  
  EV.EmployeeVisitID,  
  SM.ReferralID,  
  SM.ScheduleID,  
  SM.EmployeeID,  
  dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS EmployeeName,  
  E.MobileNumber AS EmployeeMobileNumber,  
  @IsCareTypeAssigned AS IsCareTypeAssigned,  
  SM.StartDate,  
  SM.EndDate,  
  ClockInTime,  
  ClockOutTime,  
  IsPCACompleted,  
  IsSigned,  
  CASE WHEN  
   ClockInTime IS NOT NULL AND ClockOutTime IS NOT NULL AND IsPCACompleted = 1 AND IsSigned = 1  
  THEN  
   1  
  ELSE  
   0  
  END AS IsCompleted  
 FROM  
  ScheduleMasters SM  
  INNER JOIN CareTypeTimeSlots CTTS ON CTTS.CareTypeTimeSlotID = SM.CareTypeTimeSlotID  
  LEFT JOIN EmployeeVisits EV ON SM.ScheduleID = EV.ScheduleID  
  LEFT JOIN Employees E ON E.EmployeeID = SM.EmployeeID  
 WHERE  
  SM.ReferralID= @ReferralID  
  AND SM.StartDate < @EndDate AND SM.EndDate > @StartDate  
  AND CTTS.Frequency = @NoOfDays  
  AND SM.IsDeleted = 0 AND (EV.IsDeleted IS NULL OR EV.IsDeleted = 0)  
 ORDER BY  
  SM.EndDate DESC  
END  