CREATE PROCEDURE [dbo].[HC_GetEmployeeScheduleDetails]  
 @EmployeeIDs VARCHAR(MAX),  
 @ReferralIDs VARCHAR(MAX)=NULL,  
 @StartDate DATE,          
 @EndDate DATE  
AS          
BEGIN           
  --Declare @StartDate Date,@EndDate Date    
  --SELECT @StartDate = StartDate,@EndDate=EndDate from WeekMasters where WeekMasterID=@WeekMAsterID    
   
 --Schecudle List For Perticular Facility--          
 SELECT  
   SM.*, SS.ScheduleStatusName ,          
   R.FirstName,R.LastName, dbo.GetAge(R.Dob)as Age,R.Gender,R.NeedPrivateRoom , R.IsDeleted as IsRefrralDeleted,  R.PlacementRequirement, R.Dob,  
   C.LastName+ ', '+ C.FirstName PrimaryContactName,C.Phone1,C.Phone2, C.Email,    
     
   (SELECT STUFF((SELECT '~' + RN.LastName + ' '+RN.FirstName+ ' ; ' + CONVERT (varchar,RN.ReferralID)              
   FROM Referrals RN           
   where ReferralID in (select (case when RM.ReferralID1 = R.ReferralID then RM.ReferralID2 else RM.ReferralID1 end)  as ReferralID            
  from ReferralSiblingMappings RM           
  where RM.ReferralID1= R.ReferralID  or RM.ReferralID2=R.ReferralID)           
   FOR XML PATH('')),1,1,'') ) ReferralSiblingMappingVlaue,  
  
     
      
   F.DefaultScheduleDays,EmployeeName = E.FirstName+' '+ E.LastName      
 FROM ScheduleMasters SM          
  INNER JOIN Referrals R on R.ReferralID = SM.ReferralID      
  INNER JOIN ContactMappings CM on CM.ReferralID=R.ReferralID and CM.ContactTypeID=1-- (CM.IsPrimaryPlacementLegalGuardian =1 OR CM.IsDCSLegalGuardian=1)    
  INNER JOIN Contacts C on C.ContactID = CM.ContactID  
  INNER JOIN ScheduleStatuses SS on SS.ScheduleStatusID= SM.ScheduleStatusID          
  left JOIN FrequencyCodes F on R.FrequencyCodeID = F.FrequencyCodeID              
  LEFT JOIN Employees E ON E.EmployeeID = SM.EmployeeID  
 WHERE   
 (SM.IsDeleted = 0 )          
 AND (@EmployeeIDs IS NULL OR LEN(@EmployeeIDs)=0 OR SM.EmployeeID IN (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@EmployeeIDs)))          
 AND (@ReferralIDs IS NULL OR LEN(@ReferralIDs)=0 OR SM.ReferralID IN (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@ReferralIDs)))          
 --and (SM.WeekMasterID=@WeekMAsterID)   
   
 and (@EndDate is not null or SM.StartDate >= @StartDate or SM.EndDate >= @StartDate)           
 and (@EndDate is null or ((SM.StartDate between @StartDate and @EndDate)          
        or(SM.EndDate between @StartDate and @EndDate )          
        or(@StartDate between SM.StartDate and SM.EndDate )) )          
                  
                  
 -- End Schecudle List For Perticular Facility--          
           
           
                  
 -- Get Date wise Schedule and Confirmed Schedule List For Perticular Facility--          
 select           
  DT.Date, COUNT(SM.ScheduleID) TotalScheduleCount, SUM(Case when SM.ScheduleStatusID = 2 then 1 else 0 end) ConfirmedScheduleCount,          
  SUM(Case When R.NeedPrivateRoom=1 then 1 else 0 end) RequiredPrivateRoom,          
  SUM(Case When R.NeedPrivateRoom=1 and  SM.ScheduleStatusID = 2 then 1 else 0 end) ConfirmedPrivateRoom          
 from           
  (SELECT DATEADD(DAY,number+1,DATEADD(day,-1, @StartDate)) [Date]          
   FROM master..spt_values          
   WHERE type = 'P'          
   AND DATEADD(DAY,number+1,DATEADD(day,-1, @StartDate)) < DATEADD(day,1, @EndDate)) DT           
  left join ScheduleMasters SM on (DT.Date between SM.StartDate and SM.EndDate)    
    
  AND (@EmployeeIDs IS NULL OR LEN(@EmployeeIDs)=0 OR SM.EmployeeID IN (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@EmployeeIDs)))          
  --M.EmployeeID = @EmployeeID          
  inner join Referrals R On R.ReferralID = SM.ReferralID          
  where SM.IsDeleted=0           
 group by DT.Date          
 --End Get Date wise Schedule and Confirmed Schedule List For Perticular Facility--              
           
  
END
