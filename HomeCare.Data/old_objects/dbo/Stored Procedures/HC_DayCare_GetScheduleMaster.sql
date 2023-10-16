-- EXEC GetScheduleMaster @SortExpression = 'Name', @SortType = 'ASC', @FromIndex = '2', @PageSize = '50', @PreferredCommunicationMethodID=5 ,@StartDate= '2017/03/01'    
-- EXEC GetScheduleMaster @StartDate = '2017/03/01', @PreferredCommunicationMethodID = '5', @SortExpression = 'Name', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'    
CREATE PROCEDURE [dbo].[HC_DayCare_GetScheduleMaster]
 @ScheduleID bigint=0,                  
 @StartDate date=null,                  
 @EndDate date=null,                  
 @ScheduleStatusID int = 0,                  
 @Name varchar(50)=null,                  
 @ParentName varchar(50)=null,                  
 @PickUpLocation bigint =0,                  
 @DropOffLocation bigint=0,                    
 @FacilityID bigint=0,        
 @EmployeeID  bigint=0,                
 @RegionID bigint=0,                  
 @LanguageID bigint=0,                  
 @ReferralID bigint=0,                  
 @SortExpression VARCHAR(100),                    
 @SortType VARCHAR(10),                  
 @FromIndex INT,                  
 @PageSize INT,     
 @AttendanceStatus VARCHAR(10) = '',              
 @SortIndexArray VARCHAR(max)=null,    
 @PreferredCommunicationMethodID int =0
            
AS                  
BEGIN        
      
Declare @Item1 varchar(max), @Item2 varchar(max), @Item3 varchar(max), @Item4 varchar(max), @Item5 varchar(max),@Item6 varchar(max),@Item7 varchar(max),@Item8 varchar(max),@Item9 varchar(max);      
select @Item1=splitdata  from dbo.fnSplitString(@SortIndexArray,',')  WHERE ROWID=1      
select @Item2=splitdata  from dbo.fnSplitString(@SortIndexArray,',')  WHERE ROWID=2      
select @Item3=splitdata  from dbo.fnSplitString(@SortIndexArray,',')  WHERE ROWID=3      
select @Item4=splitdata  from dbo.fnSplitString(@SortIndexArray,',')  WHERE ROWID=4      
select @Item5=splitdata  from dbo.fnSplitString(@SortIndexArray,',')  WHERE ROWID=5      
select @Item6=splitdata  from dbo.fnSplitString(@SortIndexArray,',')  WHERE ROWID=6      
select @Item7=splitdata  from dbo.fnSplitString(@SortIndexArray,',')  WHERE ROWID=7      
select @Item8=splitdata  from dbo.fnSplitString(@SortIndexArray,',')  WHERE ROWID=8      
select @Item9=splitdata  from dbo.fnSplitString(@SortIndexArray,',')  WHERE ROWID=9      
      
      
                     
 ;WITH CTEScheduleMasterList AS                  
 (                   
 SELECT *,COUNT(t1.ScheduleID) OVER() AS Count FROM                   
  (                  
   SELECT ROW_NUMBER() OVER (ORDER BY                  
            
    CASE WHEN  'Status ASC'= @Item1  THEN  ss.ScheduleStatusName  END  ASC, CASE WHEN  @Item1='Status DESC'  THEN  ss.ScheduleStatusName  END DESC,                
    CASE WHEN  'ClientName ASC'=@Item1 THEN  r.LastName  END  ASC, CASE WHEN 'ClientName DESC'=@Item1  THEN  r.LastName  END DESC,                
    CASE WHEN  'ParentName ASC'=@Item1 THEN c.LastName+' '+c.FirstName END  ASC,CASE WHEN 'ParentName DESC'=@Item1  THEN c.LastName+' '+c.FirstName END DESC,                
    CASE WHEN  'StartDate ASC'=@Item1 THEN CONVERT(datetime, sm.StartDate, 103) END  ASC,  
 CASE WHEN 'StartDate DESC'=@Item1  THEN CONVERT(datetime, sm.StartDate, 103) END DESC,   
 CASE WHEN  'EndDate ASC'=@Item1 THEN CONVERT(datetime, sm.StartDate, 103) END  ASC,  
 CASE WHEN 'EndDate DESC'=@Item1  THEN CONVERT(datetime, sm.StartDate, 103) END DESC,  
    CASE WHEN  'Facility ASC'= @Item1 THEN  f.FacilityName   END  ASC, CASE WHEN 'Facility DESC'= @Item1  THEN  f.FacilityName   END DESC,      
 CASE WHEN  'Age ASC'= @Item1 THEN  R.Dob END  ASC, CASE WHEN 'Age DESC'= @Item1  THEN  R.Dob  END DESC,      
 
    
    CASE WHEN  'Status ASC'= @Item2  THEN  ss.ScheduleStatusName  END  ASC, CASE WHEN  'Status DESC'=@Item2  THEN  ss.ScheduleStatusName  END DESC,                
    CASE WHEN  'ClientName ASC'=@Item2 THEN  r.LastName  END  ASC, CASE WHEN 'ClientName DESC'=@Item2  THEN  r.LastName  END DESC,        
  CASE WHEN  'ParentName ASC'=@Item2 THEN c.LastName+' '+c.FirstName END  ASC,CASE WHEN 'ParentName DESC'=@Item2  THEN c.LastName+' '+c.FirstName END DESC,                
    CASE WHEN  'StartDate ASC'=@Item2 THEN CONVERT(datetime, sm.StartDate, 103) END  ASC,  
 CASE WHEN 'StartDate DESC'=@Item2  THEN CONVERT(datetime, sm.StartDate, 103) END DESC,  
 CASE WHEN  'EndDate ASC'=@Item2 THEN CONVERT(datetime, sm.StartDate, 103) END  ASC,  
 CASE WHEN 'EndDate DESC'=@Item2  THEN CONVERT(datetime, sm.StartDate, 103) END DESC,  
    CASE WHEN  'Facility ASC'= @Item2 THEN  f.FacilityName   END  ASC, CASE WHEN 'Facility DESC'= @Item2  THEN  f.FacilityName   END DESC,      
  CASE WHEN  'Age ASC'= @Item2 THEN  R.Dob END  ASC, CASE WHEN 'Age DESC'= @Item2  THEN   R.Dob   END DESC,      
    
    
    CASE WHEN  'Status ASC'= @Item3  THEN  ss.ScheduleStatusName  END  ASC, CASE WHEN  'Status DESC'=@Item3  THEN  ss.ScheduleStatusName  END DESC,                
    CASE WHEN  'ClientName ASC'=@Item3 THEN  r.LastName  END  ASC, CASE WHEN 'ClientName DESC'=@Item3  THEN  r.LastName  END DESC,                
    CASE WHEN  'ParentName ASC'=@Item3 THEN c.LastName+' '+c.FirstName END  ASC,CASE WHEN 'ParentName DESC'=@Item3  THEN c.LastName+' '+c.FirstName END DESC,                
    CASE WHEN  'StartDate ASC'=@Item3 THEN CONVERT(datetime, sm.StartDate, 103) END  ASC,  
 CASE WHEN 'StartDate DESC'=@Item3  THEN CONVERT(datetime, sm.StartDate, 103) END DESC,  
 CASE WHEN  'EndDate ASC'=@Item3 THEN CONVERT(datetime, sm.StartDate, 103) END  ASC,  
 CASE WHEN 'EndDate DESC'=@Item3  THEN CONVERT(datetime, sm.StartDate, 103) END DESC,  
    CASE WHEN  'Facility ASC'= @Item3 THEN  f.FacilityName   END  ASC, CASE WHEN 'Facility DESC'= @Item3  THEN  f.FacilityName   END DESC,      
 CASE WHEN  'Age ASC'= @Item3 THEN   R.Dob END  ASC, CASE WHEN 'Age DESC'= @Item3  THEN   R.Dob   END DESC,      
    
    
    CASE WHEN  'Status ASC'= @Item4  THEN  ss.ScheduleStatusName  END  ASC, CASE WHEN  'Status DESC'=@Item4  THEN  ss.ScheduleStatusName  END DESC,                
    CASE WHEN  'ClientName ASC'=@Item4 THEN  r.LastName  END  ASC, CASE WHEN 'ClientName DESC'=@Item4  THEN  r.LastName  END DESC,                
    CASE WHEN  'ParentName ASC'=@Item4 THEN c.LastName+' '+c.FirstName END  ASC,CASE WHEN 'ParentName DESC'=@Item4  THEN c.LastName+' '+c.FirstName END DESC,                
    CASE WHEN  'StartDate ASC'=@Item4 THEN CONVERT(datetime, sm.StartDate, 103) END  ASC,CASE WHEN 'StartDate DESC'=@Item4  THEN CONVERT(datetime, sm.StartDate, 103) END DESC,        CASE WHEN  'EndDate ASC'=@Item4 THEN CONVERT(datetime, sm.StartDate, 103
  
) END  ASC,CASE WHEN 'EndDate DESC'=@Item4  THEN CONVERT(datetime, sm.StartDate, 103) END DESC,  
  
    CASE WHEN  'Facility ASC'= @Item4 THEN  f.FacilityName   END  ASC, CASE WHEN 'Facility DESC'= @Item4  THEN  f.FacilityName   END DESC,      
 CASE WHEN  'Age ASC'= @Item4 THEN   R.Dob END  ASC, CASE WHEN 'Age DESC'= @Item4  THEN   R.Dob   END DESC,      
    
    CASE WHEN  'Status ASC'= @Item5  THEN  ss.ScheduleStatusName  END  ASC, CASE WHEN  'Status DESC'=@Item5  THEN  ss.ScheduleStatusName  END DESC,                
    CASE WHEN  'ClientName ASC'=@Item5 THEN  r.LastName  END  ASC, CASE WHEN 'ClientName DESC'=@Item5  THEN  r.LastName  END DESC,                
    CASE WHEN  'ParentName ASC'=@Item5 THEN c.LastName+' '+c.FirstName END  ASC,CASE WHEN 'ParentName DESC'=@Item5  THEN c.LastName+' '+c.FirstName END DESC,                
    CASE WHEN  'StartDate ASC'=@Item5 THEN CONVERT(datetime, sm.StartDate, 103) END  ASC,  
 CASE WHEN 'StartDate DESC'=@Item5 THEN CONVERT(datetime, sm.StartDate, 103) END DESC,  
 CASE WHEN  'EndDate ASC'=@Item5 THEN CONVERT(datetime, sm.StartDate, 103) END  ASC,  
 CASE WHEN 'EndDate DESC'=@Item5  THEN CONVERT(datetime, sm.StartDate, 103) END DESC,  
 CASE WHEN  'Facility ASC'= @Item5 THEN  f.FacilityName   END  ASC, CASE WHEN 'Facility DESC'= @Item5  THEN  f.FacilityName   END DESC,      
 CASE WHEN  'Age ASC'= @Item5 THEN  R.Dob END  ASC, CASE WHEN 'Age DESC'= @Item5  THEN   R.Dob  END DESC,      
    
       
    CASE WHEN  'Status ASC'= @Item6  THEN  ss.ScheduleStatusName  END  ASC, CASE WHEN  'Status DESC'=@Item6  THEN  ss.ScheduleStatusName  END DESC,                
    CASE WHEN  'ClientName ASC'=@Item6 THEN  r.LastName  END  ASC, CASE WHEN 'ClientName DESC'=@Item6  THEN  r.LastName  END DESC,                
    CASE WHEN  'ParentName ASC'=@Item6 THEN c.LastName+' '+c.FirstName END  ASC,CASE WHEN 'ParentName DESC'=@Item6  THEN c.LastName+' '+c.FirstName END DESC,                
    CASE WHEN  'StartDate ASC'=@Item6 THEN CONVERT(datetime, sm.StartDate, 103) END  ASC,CASE WHEN 'StartDate DESC'=@Item6  THEN CONVERT(datetime, sm.StartDate, 103) END DESC,        CASE WHEN  'EndDate ASC'=@Item6 THEN CONVERT(datetime, sm.StartDate, 103  ) END  ASC,  
 CASE WHEN 'EndDate DESC'=@Item6  THEN CONVERT(datetime, sm.StartDate, 103) END DESC,   
 CASE WHEN  'Facility ASC'= @Item6 THEN  f.FacilityName   END  ASC, CASE WHEN 'Facility DESC'= @Item6  THEN  f.FacilityName   END DESC,      
 CASE WHEN  'Age ASC'= @Item6 THEN  R.Dob END  ASC, CASE WHEN 'Age DESC'= @Item6  THEN   R.Dob  END DESC,      
 
     
 CASE WHEN  'Status ASC'= @Item7  THEN  ss.ScheduleStatusName  END  ASC, CASE WHEN  'Status DESC'=@Item7  THEN  ss.ScheduleStatusName  END DESC,                
    CASE WHEN  'ClientName ASC'=@Item7 THEN  r.LastName  END  ASC, CASE WHEN 'ClientName DESC'=@Item7  THEN  r.LastName  END DESC,                
    CASE WHEN  'ParentName ASC'=@Item7 THEN c.LastName+' '+c.FirstName END  ASC,CASE WHEN 'ParentName DESC'=@Item7  THEN c.LastName+' '+c.FirstName END DESC,                
    CASE WHEN  'StartDate ASC'=@Item7 THEN CONVERT(datetime, sm.StartDate, 103) END  ASC,CASE WHEN 'StartDate DESC'=@Item7  THEN CONVERT(datetime, sm.StartDate, 103) END DESC,        CASE WHEN  'EndDate ASC'=@Item7 THEN CONVERT(datetime, sm.StartDate, 103
  
) END  ASC,  
 CASE WHEN 'EndDate DESC'=@Item7  THEN CONVERT(datetime, sm.StartDate, 103) END DESC,    
 CASE WHEN  'Facility ASC'= @Item7 THEN  f.FacilityName   END  ASC, CASE WHEN 'Facility DESC'= @Item7  THEN  f.FacilityName   END DESC,      
 CASE WHEN  'Age ASC'= @Item7 THEN   R.Dob END  ASC, CASE WHEN 'Age DESC'= @Item7  THEN  R.Dob   END DESC,      
 
    
 CASE WHEN  'Status ASC'= @Item8  THEN  ss.ScheduleStatusName  END  ASC, CASE WHEN  'Status DESC'=@Item8  THEN  ss.ScheduleStatusName  END DESC,                
    CASE WHEN  'ClientName ASC'=@Item8 THEN  r.LastName  END  ASC, CASE WHEN 'ClientName DESC'=@Item8  THEN  r.LastName  END DESC,                
    CASE WHEN  'ParentName ASC'=@Item8 THEN c.LastName+' '+c.FirstName END  ASC,CASE WHEN 'ParentName DESC'=@Item8  THEN c.LastName+' '+c.FirstName END DESC,                
    CASE WHEN  'StartDate ASC'=@Item8 THEN CONVERT(datetime, sm.StartDate, 103) END  ASC,CASE WHEN 'StartDate DESC'=@Item8  THEN CONVERT(datetime, sm.StartDate, 103) END DESC,        CASE WHEN  'EndDate ASC'=@Item8 THEN CONVERT(datetime, sm.StartDate, 103
  
  
) END  ASC,CASE WHEN 'EndDate DESC'=@Item8  THEN CONVERT(datetime, sm.StartDate, 103) END DESC,         
                
    CASE WHEN  'Facility ASC'= @Item8 THEN  f.FacilityName   END  ASC, CASE WHEN 'Facility DESC'= @Item8  THEN  f.FacilityName   END DESC,      
 CASE WHEN  'Age ASC'= @Item8 THEN   R.Dob END  ASC, CASE WHEN 'Age DESC'= @Item8  THEN  R.Dob  END DESC,      
    
    
 CASE WHEN  'Status ASC'= @Item9  THEN  ss.ScheduleStatusName  END  ASC, CASE WHEN  'Status DESC'=@Item9  THEN  ss.ScheduleStatusName  END DESC,                
    CASE WHEN  'ClientName ASC'=@Item9 THEN  r.LastName  END  ASC, CASE WHEN 'ClientName DESC'=@Item9  THEN  r.LastName  END DESC,                
    CASE WHEN  'ParentName ASC'=@Item9 THEN c.LastName+' '+c.FirstName END  ASC,CASE WHEN 'ParentName DESC'=@Item9  THEN c.LastName+' '+c.FirstName END DESC,                
    CASE WHEN  'StartDate ASC'=@Item9 THEN CONVERT(datetime, sm.StartDate, 103) END  ASC,CASE WHEN 'StartDate DESC'=@Item9  THEN CONVERT(datetime, sm.StartDate, 103) END DESC,        CASE WHEN  'EndDate ASC'=@Item9 THEN CONVERT(datetime, sm.StartDate, 103
  
  
) END  ASC,CASE WHEN 'EndDate DESC'=@Item9  THEN CONVERT(datetime, sm.StartDate, 103) END DESC,                         
    CASE WHEN  'Facility ASC'= @Item9 THEN  f.FacilityName   END  ASC, CASE WHEN 'Facility DESC'= @Item9  THEN  f.FacilityName   END DESC,      
 CASE WHEN  'Age ASC'= @Item9 THEN   R.Dob END  ASC, CASE WHEN 'Age DESC'= @Item9  THEN  R.Dob  END DESC,      
    
                
                
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'StartDate' THEN CONVERT(datetime, sm.StartDate, 103) END END ASC,                  
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'StartDate' THEN CONVERT(datetime, sm.StartDate, 103) END END DESC,                  
                  
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EndDate' THEN CONVERT(datetime, sm.EndDate, 103) END END ASC,                  
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EndDate' THEN CONVERT(datetime, sm.EndDate, 103) END END DESC,                  
                  
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'UpdatedDate' THEN CONVERT(datetime, sm.UpdatedDate, 103) END END ASC,                  
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'UpdatedDate' THEN CONVERT(datetime, sm.UpdatedDate, 103) END END DESC,                  
                  
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN CONVERT(datetime, sm.CreatedDate, 103) END END ASC,                  
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN CONVERT(datetime, sm.CreatedDate, 103) END END DESC,                  
           
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Status' THEN ss.ScheduleStatusName END END ASC,                  
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Status' THEN ss.ScheduleStatusName END END DESC,                  
                  
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Name' THEN r.LastName END END ASC,                  
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Name' THEN r.LastName END END DESC,                  
                
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ParentName' THEN c.LastName+' '+c.FirstName END END ASC,                  
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ParentName' THEN c.LastName+' '+c.FirstName END END DESC,                  
                              
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Facility' THEN f.FacilityName END END ASC,                  
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Facility' THEN f.FacilityName END END DESC,     
      
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Placement' THEN r.PlacementRequirement END END ASC,                  
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Placement' THEN r.PlacementRequirement END END DESC,                  
                  
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Comment' THEN sm.Comments END END ASC,                  
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Comment' THEN sm.Comments END END DESC,              
      
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Age' THEN  R.Dob  END END ASC,              
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Age' THEN  R.Dob END END DESC,  
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Address' THEN c.Address  END END ASC,              
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Address' THEN c.Address END END DESC               
  ) AS Row,                  
    sm.ScheduleID,sm.ReferralID,sm.FacilityID,f.FacilityName,sm.EmployeeID, sm.IsPatientAttendedSchedule, sm.AbsentReason, 
    PatAddress=c.Address+', '+c.City+', '+c.State+' - '+c.ZipCode,    
    sm.StartDate,sm.EndDate,sm.ScheduleStatusID,ss.ScheduleStatusName,r.PlacementRequirement,sm.Comments,sm.PickUpLocation,  
 --tlp.Location as PickupLocationName,  
 sm.DropOffLocation,                  
    --tld.Location as DropOffLocationName,  
   
 dbo.GetGeneralNameFormat(r.FirstName,r.LastName) as Name,  
 dbo.GetGeneralNameFormat(c.FirstName,c.LastName) as ParentName,c.Phone1,c.Phone2,                  
    c.Email,c.Address,c.City,c.State,c.ZipCode,  
 --re.RegionName,  
 r.RegionID,                  
    r.PermissionForEmail,r.PermissionForSMS,r.PermissionForVoiceMail,sm.WhoCancelled,sm.WhenCancelled,sm.CancelReason,r.BehavioralIssue,                  
    sm.UpdatedDate,sm.IsReschedule,R.PermissionForMail,dbo.GetAge(R.Dob) Age,    
    SM.EmailSent,SM.SMSSent,SM.NoticeSent,r.PCMVoiceMail,r.PCMMail,R.PCMSMS,r.PCMEmail  
   from ScheduleMasters sm                  
    inner join ScheduleStatuses ss on ss.ScheduleStatusID=sm.ScheduleStatusID                     
    inner join Referrals r on r.ReferralID=sm.ReferralID                
    --inner join Regions re on re.RegionID=r.RegionID                
    --inner join TransportLocations tlp on tlp.TransportLocationID=sm.PickUpLocation                  
    --inner join TransportLocations tld on tld.TransportLocationID=sm.DropOffLocation                  
    inner join ContactMappings CM on CM.ReferralID=sm.ReferralID and CM.ContactTypeID=1      
    inner join Contacts c on c.ContactID=CM.ContactID                  
    inner join Facilities f on f.FacilityID=sm.FacilityID    
    --INNER join Employees E on E.EmployeeID=sm.EmployeeID    
                      
   where                  
   (sm.IsDeleted= 0)
   AND ( @AttendanceStatus IS NULL OR LEN(@AttendanceStatus)=0 OR sm.IsPatientAttendedSchedule = CONVERT(BIT,@AttendanceStatus))
   AND (@ScheduleStatusID=0 or sm.ScheduleStatusID = @ScheduleStatusID)                    
   AND (@FacilityID = 0 or sm.FacilityID = @FacilityID)                        
   AND (@EmployeeID = 0 or sm.EmployeeID = @EmployeeID)                        
   AND (@RegionID = 0 or r.RegionID = @RegionID)    
   AND (@LanguageID = 0 or r.LanguageID = @LanguageID)                          
   AND (@PickUpLocation = 0 or sm.PickUpLocation = @PickUpLocation)                  
   AND (@DropOffLocation = 0 or sm.DropOffLocation = @DropOffLocation)                  
   AND (@ReferralID = 0 or sm.ReferralID = @ReferralID)                      
   AND (@ScheduleID = 0 or sm.ScheduleID = @ScheduleID)      
   AND ((@Name IS NULL OR LEN(r.LastName)=0)     
   OR              
      (    
     (r.FirstName LIKE '%'+@Name+'%' )OR                
     (r.LastName  LIKE '%'+@Name+'%') OR                
     (r.FirstName +' '+r.LastName like '%'+@Name+'%') OR                
     (r.LastName +' '+r.FirstName like '%'+@Name+'%') OR                
     (r.FirstName +', '+r.LastName like '%'+@Name+'%') OR                
     (r.LastName +', '+r.FirstName like '%'+@Name+'%'))              
      )                  
      AND ((@ParentName IS NULL OR LEN(c.LastName)=0)               
   OR        
      (    
     (c.FirstName LIKE '%'+@ParentName+'%' )OR                
     (c.LastName  LIKE '%'+@ParentName+'%') OR                
     (C.FirstName +' '+C.LastName like '%'+@ParentName+'%') OR                
     (C.LastName +' '+C.FirstName like '%'+@ParentName+'%') OR                
     (C.FirstName +', '+C.LastName like '%'+@ParentName+'%') OR                
     (C.LastName +', '+C.FirstName like '%'+@ParentName+'%') OR              
     (c.Address like '%' + @ParentName+'%') OR              
     (c.ZipCode =  @ParentName) OR               
     (c.Phone1 like '%' + @ParentName+'%') OR               
     (c.Phone2 like '%' + @ParentName+'%'))                  
      )                   
   AND ((@StartDate is null OR SM.StartDate >= @StartDate) and (@EndDate is null OR SM.EndDate <= @EndDate))                  
   AND (    
     ((CAST(@PreferredCommunicationMethodID AS bigint)=0)   )    
       OR  (CAST(@PreferredCommunicationMethodID AS bigint) = 5 and     
       ( (r.PCMVoiceMail IS NULL OR r.PCMVoiceMail = 0)  AND  (r.PCMEmail IS NULL OR r.PCMEmail = 0) AND (r.PCMSMS IS NULL OR r.PCMSMS = 0) AND  (r.PCMMail IS NULL OR r.PCMMail = 0)  )  )                              
       OR  (CAST(@PreferredCommunicationMethodID AS bigint) = 1 and (r.PCMVoiceMail = 1 AND r.PermissionForVoiceMail = 1 ) )              
       OR  (CAST(@PreferredCommunicationMethodID AS bigint) = 2 and (r.PCMEmail = 1 AND r.PermissionForEmail = 1 ) )                                                                                        
       OR  (CAST(@PreferredCommunicationMethodID AS bigint) = 3 and (r.PCMSMS = 1 AND r.PermissionForSMS = 1 ) )             
       OR  (CAST(@PreferredCommunicationMethodID AS bigint) = 4 and (r.PCMMail = 1 AND r.PermissionForMail = 1 ) )             
       )        
  ) AS t1                    
 )                   
 SELECT * FROM CTEScheduleMasterList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                   
                  
END
