  
CREATE PROCEDURE [adc].[HC_Daycare_GetScheduledPatientList]        
@PatientName NVARCHAR(100)='',        
@FacilityID BIGINT=0,        
@StartDate DATE=NULL        
AS        
BEGIN        
    DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()       
  IF(@StartDate IS NULL)        
  SET @StartDate =CONVERT(DATE, GETDATE());        
        
  PRINT @StartDate        
        
  SELECT R.ReferralID, R.FirstName,R.LastName, dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) AS PatientName,
  R.Dob, SM.ScheduleID, SM.ScheduleStatusID, SM.IsPatientAttendedSchedule,SM.AbsentReason,                       
 ScheduleStartDate= SM.StartDate,ScheduleEndDate=  SM.StartDate, F.FacilityName, EV.ClockInTime, EV.ClockOutTime, EV.EmployeeVisitID,        
      PatientSignature_ClockIN = EV.PatientSignature,   
      PatientSignature_ClockOut = EV.PatientSignature_ClockOut,  
   R.ProfileImagePath,  
   ev.IsSelf,ev.Name,ev.Relation  
  FROM ScheduleMasters SM        
  INNER JOIN Referrals R ON R.ReferralID=SM.ReferralID        
  LEFT JOIN EmployeeVisits EV ON EV.ScheduleID=SM.ScheduleID AND EV.IsDeleted = 0  
  LEFT JOIN Facilities F ON F.FacilityID= SM.FacilityID                
        
  WHERE ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted=0 AND R.IsDeleted=0 AND SM.FacilityID > 0 --AND SM.PayorID > 0 --AND SM.ScheduleStatusID=2        
  AND CONVERT(DATE, SM.StartDate )= @StartDate        
  AND (( CAST(@FacilityID AS BIGINT)=0) OR SM.FacilityID = CAST(@FacilityID AS BIGINT))                                     
  AND (                      
   (@PatientName IS NULL OR LEN(@PatientName)=0)                       
   OR (                      
       (R.FirstName LIKE '%'+@PatientName+'%' )OR                        
    (R.LastName  LIKE '%'+@PatientName+'%') OR                        
    (R.FirstName +' '+R.LastName like '%'+@PatientName+'%') OR                        
    (R.LastName +' '+R.FirstName like '%'+@PatientName+'%') OR                        
    (R.FirstName +', '+R.LastName like '%'+@PatientName+'%') OR                        
    (R.LastName +', '+R.FirstName like '%'+@PatientName+'%')))              
        
END  