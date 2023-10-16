--EXEC GetReferralListForTransportationAssignment @StartDate = '2016-08-03', @TripDirection = 'DOWN', @ContactTypeID = '1', @SortExpression = 'FirstName', @SortType = 'DESC', @FromIndex = '1', @PageSize = '10'  
--EXEC GetReferralListForTransportationAssignment @StartDate = '2016-08-02', @TripDirection = 'UP', @ContactTypeID = '1', @SortExpression = 'FirstName', @SortType = 'DESC', @FromIndex = '1', @PageSize = '10'  
CREATE PROCEDURE [dbo].[GetReferralListForTransportationAssignment]  
--= declare  
 @FacilityId bigint=0,    
 @TransportLocationID bigint=0,    
 @TripDirection varchar(10),    
 @StartDate date,  
 @ContactTypeID int,  
 @DisallowScheduleStatuses varchar(100),  
 @SortExpression VARCHAR(100),  
 @SortType VARCHAR(10),    
 @FromIndex INT,    
 @PageSize INT   
  AS    
BEGIN        
 ;WITH CTEReferralList AS    
 (     
 SELECT *,COUNT(t1.ReferralID) OVER() AS Count FROM     
  (    
 SELECT ROW_NUMBER() OVER (ORDER BY     
   CASE WHEN @SortType = 'ASC' THEN    
     CASE     
    WHEN @SortExpression = 'Phone' THEN C.Phone1  
     WHEN @SortExpression = 'Name' THEN R.LastName  
     WHEN @SortExpression = 'Gender' THEN CONVERT(varchar(5),R.Gender )  
     --WHEN @SortExpression = 'Age' THEN R.Dob  
     WHEN @SortExpression = 'ParentName' THEN C.LastName  
     WHEN @SortExpression = 'Location' THEN DrpTL.Location  
     WHEN @SortExpression = 'FacilityName' THEN FS.FacilityName            
     END     
   END ASC,    
   CASE WHEN @SortType = 'DESC' THEN    
     CASE     
    WHEN @SortExpression = 'Phone' THEN C.Phone1  
     WHEN @SortExpression = 'Name' THEN R.LastName  
     WHEN @SortExpression = 'Gender' THEN CONVERT(varchar(5),R.Gender )  
     --WHEN @SortExpression = 'Age' THEN  R.Dob
     WHEN @SortExpression = 'ParentName' THEN C.LastName  
     WHEN @SortExpression = 'Location' THEN DrpTL.LocationCode  
     WHEN @SortExpression = 'FacilityName' THEN FS.FacilityName  
     END    
   END DESC ,
   
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Age' THEN R.Dob END END ASC,        
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Age' THEN R.Dob END END DESC         
      
    ) AS Row,    
  R.ReferralID, R.LastName+', '+R.FirstName as Name,  
  R.Gender,dbo.GetAge(R.Dob) Age ,  
  C.LastName+', '+C.FirstName as ParentName,C.Phone1 Phone ,C.Phone2,  
  DrpTL.LocationCode as Location ,FS.FacilityName,  
  S.ScheduleID,S.IsDeleted,S.StartDate,S.EndDate,R.NeedPrivateRoom,p.PayorName,F.Description as FrequencyCode,  
  s.IsAssignedToTransportationGroupUp,s.IsAssignedToTransportationGroupDown,R.CISNumber,R.AHCCCSID,C.Email  
   from Referrals R    
    inner join ReferralPayorMappings RPM on R.ReferralID = RPM.ReferralID and RPM.IsActive=1 and RPM.IsDeleted=0    
    inner join Payors P on P.PayorID = RPM.PayorID     
    inner join ContactMappings CM on CM.ReferralID=R.ReferralID and CM.ContactTypeID =@ContactTypeID    
    inner join Contacts C on C.ContactID = CM.ContactID       
    inner join TransportLocations DrpTL on DrpTL.TransportLocationID = R.DropOffLocation    
    inner join TransportLocations PickUPL on PickUPL.TransportLocationID = R.PickUpLocation    
    inner join Regions Reg on Reg.RegionID = R.RegionID    
    inner join FrequencyCodes F on R.FrequencyCodeID = F.FrequencyCodeID    
    inner join ScheduleMasters S on s.ReferralID=r.ReferralID and S.ScheduleStatusID not in(SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@DisallowScheduleStatuses))  
    inner join Facilities FS on FS.FacilityID=S.FacilityID      
   where   
 S.IsDeleted=0 and R.IsDeleted=0 and  
    (@FacilityId=0 or S.FacilityID = @FacilityId)   
    and  (@TransportLocationID=0 or DrpTL.TransportLocationID = @TransportLocationID)   
    and (  
   (@TripDirection='UP' and S.StartDate=@StartDate) OR  
   (@TripDirection='DOWN' and S.EndDate=@StartDate)       
  )  
  ) AS t1      
 )     
 SELECT * FROM CTEReferralList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)     
    
END

