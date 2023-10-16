    
CREATE PROCEDURE [dbo].[GetGroupTimesheetList]                    
  @EmployeeIDs NVARCHAR(500) = NULL              
 ,@FacilityIDs NVARCHAR(500) = NULL            
 ,@ReferralIDs NVARCHAR(500) = NULL                    
 ,@StartDate DATE = NULL                    
 ,@EndDate DATE = NULL                    
 ,@PayorIDs NVARCHAR(500) = NULL                    
 ,@CareTypeIDs NVARCHAR(500) = NULL                    
 ,@SortExpression NVARCHAR(100)                    
 ,@SortType NVARCHAR(10)                    
 ,@FromIndex INT                    
 ,@PageSize INT        
 ,@TypesOfTimeSheet NVARCHAR(MAX)                
  AS

             
BEGIN                    
  ;                    
          if(@TypesOfTimeSheet='missing')          
    Begin        
 WITH CTEScheduleMasterList                    
 AS (           
         
  SELECT *                    
   ,COUNT(t1.ScheduleID) OVER () AS Count                    
  FROM (                    
   SELECT ROW_NUMBER() OVER (                    
     ORDER BY CASE                     
       WHEN @SortType = 'ASC'                    
        THEN CASE                     
          WHEN @SortExpression = 'StartDate' THEN CONVERT(NVARCHAR(MAX), sm.StartDate, 103)                
    WHEN @SortExpression = 'EndDate' THEN CONVERT(NVARCHAR(MAX), sm.EndDate, 103)            
    WHEN @SortExpression = 'CareType' THEN d.Title            
    WHEN @SortExpression = 'Name' THEN dbo.GetGeneralNameFormat(E.FirstName, E.LastName)            
    WHEN @SortExpression = 'PatientName' THEN dbo.GetGeneralNameFormat(r.FirstName, r.LastName)            
    WHEN @SortExpression = 'ScheduleID' THEN dbo.GetGeneralNameFormat(r.FirstName, r.LastName)            
  END                  
       END ASC            
      ,CASE                     
       WHEN @SortType = 'DESC'                    
        THEN CASE                     
          WHEN @SortExpression = 'StartDate' THEN CONVERT(NVARCHAR(MAX), sm.StartDate, 103)             
    WHEN @SortExpression = 'EndDate' THEN CONVERT(NVARCHAR(MAX), sm.EndDate, 103)            
    WHEN @SortExpression = 'CareType' THEN d.Title            
    WHEN @SortExpression = 'Name' THEN dbo.GetGeneralNameFormat(E.FirstName, E.LastName)            
    WHEN @SortExpression = 'PatientName' THEN dbo.GetGeneralNameFormat(r.FirstName, r.LastName)            
    WHEN @SortExpression = 'ScheduleID' THEN dbo.GetGeneralNameFormat(r.FirstName, r.LastName)            
  END               
       END DESC             
    , CONVERT(NVARCHAR(MAX), sm.StartDate, 103) -- Addtional sort on start date            
     ) AS Row                    
    ,sm.ScheduleID                    
    ,sm.ReferralID                    
    ,sm.FacilityID                    
    ,f.FacilityName                    
    ,sm.EmployeeID                    
    ,EmployeeName = dbo.GetGeneralNameFormat(E.FirstName, E.LastName)                    
    ,EmpEmail = E.Email                    
    ,EmpMobile = E.MobileNumber                    
    ,PatAddress = c.Address + ', ' + c.City + ', ' + c.STATE + ' - ' + c.ZipCode                    
    ,EmpAddress = E.Address + ', ' + E.City + ', ' + E.StateCode + ' - ' + E.ZipCode                    
    ,sm.StartDate                    
    ,sm.EndDate                    
    ,sm.ScheduleStatusID                    
    ,ss.ScheduleStatusName                    
    ,r.PlacementRequirement                    
    ,sm.Comments                    
    ,sm.PickUpLocation                    
    ,sm.DropOffLocation                    
   ,dbo.GetGeneralNameFormat(r.FirstName, r.LastName) AS PatientName                    
    ,dbo.GetGeneralNameFormat(c.FirstName, c.LastName) AS ParentName                    
    ,c.Phone1      
    ,c.Phone2                    
    ,c.Email                    
    ,c.Address                    
    ,c.City                    
    ,c.STATE                    
    ,c.ZipCode                    
    ,r.RegionID                    
 ,r.PermissionForEmail                    
    ,r.PermissionForSMS                    
    ,r.PermissionForVoiceMail                    
    ,sm.WhoCancelled                    
    ,sm.WhenCancelled             
    ,sm.CancelReason                    
    ,r.BehavioralIssue                    
    ,sm.UpdatedDate                    
    ,sm.IsReschedule                    
    ,R.PermissionForMail                    
    ,dbo.GetAge(R.Dob) Age                    
    ,SM.EmailSent                    
    ,SM.SMSSent                    
    ,SM.NoticeSent                    
    ,r.PCMVoiceMail                    
    ,r.PCMMail                    
    ,R.PCMSMS                    
    ,r.PCMEmail                    
    ,p.PayorID                    
    ,p.PayorName                    
    ,d.DDMasterID CareTypeID               
    ,d.Title CareType                    
    ,sm.ReferralBillingAuthorizationID                    
    ,rba.AuthorizationCode         
   FROM ScheduleMasters sm                    
   left JOIN ScheduleStatuses ss ON ss.ScheduleStatusID = sm.ScheduleStatusID                    
   INNER JOIN Referrals r ON r.ReferralID = sm.ReferralID                    
   INNER JOIN ContactMappings CM ON CM.ReferralID = sm.ReferralID  AND CM.ContactTypeID = 1                    
   INNER JOIN Contacts c ON c.ContactID = CM.ContactID                    
   LEFT JOIN Facilities f ON f.FacilityID = sm.FacilityID and f.IsDeleted=0                    
   LEFT JOIN Employees E ON E.EmployeeID = sm.EmployeeID                    
   LEFT JOIN DDMaster d on d.DDMasterID=sm.CareTypeId                         
   LEFT JOIN ReferralBillingAuthorizations rba ON rba.ReferralBillingAuthorizationID = sm.ReferralBillingAuthorizationID AND rba.ReferralID = sm.ReferralID  and rba.IsDeleted=0                    
   LEFT JOIN EmployeeVisits EV ON EV.ScheduleID = sm.ScheduleID AND EV.IsDeleted = 0                
 LEFT JOIN Payors p on p.PayorID=rba.PayorID                                     
            
  INNER JOIN ReferralTimeSlotDates RTSD ON RTSD.ReferralTSDateID=SM.ReferralTSDateID              
  INNER JOIN ReferralTimeSlotDetails RTS ON RTS.ReferralTimeSlotDetailID=RTSD.ReferralTimeSlotDetailID AND RTS.IsDeleted = 0            
  INNER JOIN ReferralTimeSlotMaster RTSM ON RTSM.ReferralTimeSlotMasterID=RTSD.ReferralTimeSlotMasterID AND RTSM.IsDeleted = 0            
   WHERE (sm.IsDeleted = 0) --AND EV.IsDeleted=0                   
       AND EV.EmployeeVisitID IS NULL         
    --AND ev.ClockInTime is not null and ev.ClockOutTime is null        
    AND R.ReferralStatusID = 1            
    AND (LEN(@EmployeeIDs) = 0 OR SM.EmployeeID IN ( SELECT EL.[val] FROM dbo.GetCSVTable(@EmployeeIDs) EL) )                  
 AND (LEN(@FacilityIDs) = 0 OR SM.FacilityID IN ( SELECT FL.[val] FROM dbo.GetCSVTable(@FacilityIDs) FL) )              
    AND (LEN(@ReferralIDs) = 0 OR SM.ReferralID IN ( SELECT RL.[val] FROM dbo.GetCSVTable(@ReferralIDs) RL ))                    
    AND ( LEN(@PayorIDs) = 0 OR SM.PayorID IN (SELECT PL.[val] FROM dbo.GetCSVTable(@PayorIDs) PL) )                    
    AND ( LEN(@CareTypeIDs) = 0 OR SM.CareTypeID IN (SELECT CL.[val]  FROM dbo.GetCSVTable(@CareTypeIDs) CL))                    
    AND (( @StartDate IS NULL OR (CONVERT(DATE, SM.StartDate) >= @StartDate) )                    
     AND ( @EndDate IS NULL OR (CONVERT(DATE, SM.EndDate) <= @EndDate) ) )         
   ) AS t1                    
  )            
         
 SELECT *                    
 FROM CTEScheduleMasterList                    
 --WHERE ROW BETWEEN ((@PageSize * (@FromIndex - 1)) + 1)                    
 --  AND (@PageSize * @FromIndex)         
  End        
  else        
     Begin        
 WITH CTEScheduleMasterList                    
 AS (           
       
  SELECT *                    
   ,COUNT(t1.ScheduleID) OVER () AS Count                    
  FROM (                    
   SELECT ROW_NUMBER() OVER (                    
     ORDER BY CASE                     
       WHEN @SortType = 'ASC'                    
        THEN CASE                     
          WHEN @SortExpression = 'StartDate' THEN CONVERT(NVARCHAR(MAX), sm.StartDate, 103)                
    WHEN @SortExpression = 'EndDate' THEN CONVERT(NVARCHAR(MAX), sm.EndDate, 103)            
    WHEN @SortExpression = 'CareType' THEN d.Title            
    WHEN @SortExpression = 'Name' THEN dbo.GetGeneralNameFormat(E.FirstName, E.LastName)            
    WHEN @SortExpression = 'PatientName' THEN dbo.GetGeneralNameFormat(r.FirstName, r.LastName)            
    WHEN @SortExpression = 'ScheduleID' THEN dbo.GetGeneralNameFormat(r.FirstName, r.LastName)            
  END                  
       END ASC            
      ,CASE                     
       WHEN @SortType = 'DESC'                    
        THEN CASE                     
          WHEN @SortExpression = 'StartDate' THEN CONVERT(NVARCHAR(MAX), sm.StartDate, 103)             
    WHEN @SortExpression = 'EndDate' THEN CONVERT(NVARCHAR(MAX), sm.EndDate, 103)            
    WHEN @SortExpression = 'CareType' THEN d.Title            
    WHEN @SortExpression = 'Name' THEN dbo.GetGeneralNameFormat(E.FirstName, E.LastName)            
    WHEN @SortExpression = 'PatientName' THEN dbo.GetGeneralNameFormat(r.FirstName, r.LastName)            
    WHEN @SortExpression = 'ScheduleID' THEN dbo.GetGeneralNameFormat(r.FirstName, r.LastName)            
  END               
       END DESC             
    , CONVERT(NVARCHAR(MAX), sm.StartDate, 103) -- Addtional sort on start date            
     ) AS Row                    
    ,sm.ScheduleID                    
    ,sm.ReferralID                    
    ,sm.FacilityID                    
    ,f.FacilityName                    
    ,sm.EmployeeID                    
    ,EmployeeName = dbo.GetGeneralNameFormat(E.FirstName, E.LastName)                    
    ,EmpEmail = E.Email                    
    ,EmpMobile = E.MobileNumber                    
    ,PatAddress = c.Address + ', ' + c.City + ', ' + c.STATE + ' - ' + c.ZipCode                    
    ,EmpAddress = E.Address + ', ' + E.City + ', ' + E.StateCode + ' - ' + E.ZipCode                    
    ,sm.StartDate                    
    ,sm.EndDate                    
    ,sm.ScheduleStatusID                    
    ,ss.ScheduleStatusName                    
    ,r.PlacementRequirement                    
    ,sm.Comments                    
    ,sm.PickUpLocation                    
    ,sm.DropOffLocation                    
   ,dbo.GetGeneralNameFormat(r.FirstName, r.LastName) AS PatientName                    
    ,dbo.GetGeneralNameFormat(c.FirstName, c.LastName) AS ParentName                    
    ,c.Phone1                    
    ,c.Phone2                    
    ,c.Email                    
    ,c.Address                    
    ,c.City                    
    ,c.STATE                    
    ,c.ZipCode                    
    ,r.RegionID                    
    ,r.PermissionForEmail                    
    ,r.PermissionForSMS                    
    ,r.PermissionForVoiceMail                    
    ,sm.WhoCancelled                    
    ,sm.WhenCancelled                    
    ,sm.CancelReason                    
    ,r.BehavioralIssue                    
    ,sm.UpdatedDate                    
    ,sm.IsReschedule                    
    ,R.PermissionForMail                    
    ,dbo.GetAge(R.Dob) Age                    
    ,SM.EmailSent                    
    ,SM.SMSSent                    
    ,SM.NoticeSent                    
    ,r.PCMVoiceMail                    
    ,r.PCMMail                    
    ,R.PCMSMS                    
    ,r.PCMEmail                    
    ,p.PayorID                    
    ,p.PayorName                    
    ,d.DDMasterID CareTypeID               
    ,d.Title CareType            
    ,sm.ReferralBillingAuthorizationID                    
    ,rba.AuthorizationCode         
   FROM ScheduleMasters sm                    
   left JOIN ScheduleStatuses ss ON ss.ScheduleStatusID = sm.ScheduleStatusID                    
   INNER JOIN Referrals r ON r.ReferralID = sm.ReferralID                    
   INNER JOIN ContactMappings CM ON CM.ReferralID = sm.ReferralID  AND CM.ContactTypeID = 1                    
   INNER JOIN Contacts c ON c.ContactID = CM.ContactID                    
   LEFT JOIN Facilities f ON f.FacilityID = sm.FacilityID and f.IsDeleted=0                    
   LEFT JOIN Employees E ON E.EmployeeID = sm.EmployeeID                    
   LEFT JOIN DDMaster d on d.DDMasterID=sm.CareTypeId                         
   LEFT JOIN ReferralBillingAuthorizations rba ON rba.ReferralBillingAuthorizationID = sm.ReferralBillingAuthorizationID AND rba.ReferralID = sm.ReferralID  and rba.IsDeleted=0                    
  -- INNER JOIN ReferralBillingAuthorizations rba ON  rba.ReferralID = sm.ReferralID                    
  LEFT JOIN EmployeeVisits EV ON EV.ScheduleID = sm.ScheduleID              
 LEFT JOIN Payors p on p.PayorID=rba.PayorID                                     
            
  INNER JOIN ReferralTimeSlotDates RTSD ON RTSD.ReferralTSDateID=SM.ReferralTSDateID              
  INNER JOIN ReferralTimeSlotDetails RTS ON RTS.ReferralTimeSlotDetailID=RTSD.ReferralTimeSlotDetailID AND RTS.IsDeleted = 0            
  INNER JOIN ReferralTimeSlotMaster RTSM ON RTSM.ReferralTimeSlotMasterID=RTSD.ReferralTimeSlotMasterID AND RTSM.IsDeleted = 0            
   WHERE         
   (sm.IsDeleted = 0)    --AND EV.IsDeleted=0                 
   --    --AND EV.EmployeeVisitID IS NULL         
    AND        
    ev.ClockInTime is not null and ev.ClockOutTime is null        
    --AND R.ReferralStatusID = 1            
    AND (LEN(@EmployeeIDs) = 0 OR SM.EmployeeID IN ( SELECT EL.[val] FROM dbo.GetCSVTable(@EmployeeIDs) EL) )                  
 AND (LEN(@FacilityIDs) = 0 OR SM.FacilityID IN ( SELECT FL.[val] FROM dbo.GetCSVTable(@FacilityIDs) FL) )              
    AND (LEN(@ReferralIDs) = 0 OR SM.ReferralID IN ( SELECT RL.[val] FROM dbo.GetCSVTable(@ReferralIDs) RL ))                    
    AND ( LEN(@PayorIDs) = 0 OR SM.PayorID IN (SELECT PL.[val] FROM dbo.GetCSVTable(@PayorIDs) PL) )                    
    AND ( LEN(@CareTypeIDs) = 0 OR SM.CareTypeID IN (SELECT CL.[val]  FROM dbo.GetCSVTable(@CareTypeIDs) CL))                    
    AND (( @StartDate IS NULL OR (CONVERT(DATE, SM.StartDate) >= @StartDate) )                    
     AND ( @EndDate IS NULL OR (CONVERT(DATE, SM.EndDate) <= @EndDate) ) )         
   ) AS t1                    
  )            
         
 SELECT *                    
 FROM CTEScheduleMasterList                    
 --WHERE ROW BETWEEN ((@PageSize * (@FromIndex - 1)) + 1)                    
 --  AND (@PageSize * @FromIndex)         
  End        
END      
    