CREATE PROCEDURE [dbo].[GetAttendanceMasterByFacility]    
 @FacilityID bigint,    
 @ClientName varchar(50),    
 @StartDate Date,    
 @EndDate Date    
AS    
BEGIN     
    DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
    
 --select * from ScheduleMasters   
 --Schecudle List For Perticular Facility--    
 select am.AttendanceMasterID,am.ScheduleMasterID,am.ReferralID,am.StartDate,am.EndDate,am.Comment,am.AttendanceStatus,dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) as Name,    
 dbo.GetAge(r.Dob) as Age,am.UpdatedBy,am.UpdatedDate,dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) as UpdatedByName,   
 sm.PickUpLocation,sm.DropOffLocation, sm.ScheduleStatusID, sm.Comments, sm.IsAssignedToTransportationGroupDown,sm.IsAssignedToTransportationGroupUp,  
 sm.WhoCancelled,sm.WhenCancelled,sm.CancelReason,sm.IsReschedule  
  from AttendanceMaster am    
 inner join ScheduleMasters sm on sm.ScheduleID=am.ScheduleMasterID    
 inner join Referrals r on r.ReferralID=am.ReferralID    
 inner join Employees e on e.EmployeeID=am.UpdatedBy    
 where    
 --((@ClientName IS NULL OR LEN(@ClientName)=0) OR dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) LIKE '%' + @ClientName + '%')      
        ((@ClientName IS NULL OR LEN(r.LastName)=0)  
         OR  
     ((r.FirstName LIKE '%'+@ClientName+'%' )OR    
  (r.LastName  LIKE '%'+@ClientName+'%') OR    
  (r.FirstName +' '+r.LastName like '%'+@ClientName+'%') OR    
  (r.LastName +' '+r.FirstName like '%'+@ClientName+'%') OR    
  (r.FirstName +', '+r.LastName like '%'+@ClientName+'%') OR    
  (r.LastName +', '+r.FirstName like '%'+@ClientName+'%'))  
    )    
 and (sm.FacilityID=@FacilityID  )    
 and (@EndDate is not null or am.StartDate >= @StartDate or am.EndDate >= @StartDate)     
 and (@EndDate is null or ((am.StartDate between @StartDate and @EndDate)    
        or(am.EndDate between @StartDate and @EndDate )    
        or(@StartDate between am.StartDate and am.EndDate )) )           
            
 -- End Schecudle List For Perticular Facility--    
     
      
 --Get Facility Details--     
 select * from Facilities where FacilityID=@FacilityID order by FacilityName ASC    
 --End Get Facility Details--    
  
     
END    
    
    
    
    