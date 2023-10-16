--EXEC HC_GetReferralListForScheduling @Name = 'braham, Brendon', @MaxAge = '100', @LastAttFromDate = '', @LastAttToDate = '', @MinAge = '0', @ContactTypeID = '1', @IgnoreFrequency = '10', @SortExpression = 'ClientName', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'  
CREATE PROCEDURE [dbo].[HC_GetReferralListForScheduling]        
 @RegioinID bigint=0,                      
 @FrequencyCodeID bigint=0,                      
 @ServiceID int = -1,                      
 @Gender int = 0,                      
 @MaxAge bigint,                      
 @MinAge bigint,                      
 @PayorID bigint =0,                      
 @ContactName VARCHAR(100)=null,                        
 @LastAttFromDate date=null,                    
 @LastAttToDate date=null,                    
 @ContactTypeID bigint,                        
 @Name VARCHAR(100)=null,                    
 @IgnoreFrequency int,                        
 @SortExpression VARCHAR(100),                        
 @SortType VARCHAR(10),                      
 @FromIndex INT,                      
 @PageSize INT             
AS                      
BEGIN                          
 ;WITH CTEReferralListForSchedulings AS                      
 (                       
 SELECT  *,COUNT(t1.ReferralID) OVER() AS Count FROM                       
  (                      
  SELECT ROW_NUMBER() OVER (ORDER BY                 
                
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ClientName' THEN Name  END END ASC,                    
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ClientName' THEN Name  END END DESC,                    
                    
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Gender' THEN Gender  END END ASC,                    
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Gender' THEN Gender  END END DESC,                    
                    
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Age' THEN Dob  END END ASC,                    
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Age' THEN Dob  END END DESC,                    
                    
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Location' THEN DropOffLocationName  END END ASC,                    
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Location' THEN DropOffLocationName  END END DESC,                    
          
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Payor' THEN ShortName  END END ASC,                    
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Payor' THEN ShortName  END END DESC,                    
          
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Region' THEN RegionName  END END ASC,                    
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Region' THEN RegionName END END DESC,                    
                    
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'LastAtt' THEN CONVERT(datetime, LastAttendedDate, 103)  END END ASC,                    
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'LastAtt' THEN CONVERT(datetime, LastAttendedDate, 103)  END END DESC ,  
    
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'NextAtt' THEN CONVERT(datetime, NextAttDate, 103)  END END ASC,                                     
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'NextAtt' THEN CONVERT(datetime, NextAttDate, 103)  END END DESC                                     
                           
   ) AS Row, ReferralID, Name ,Gender, Age,NeedPrivateRoom,PayorName ,DropOffLocationName , DropOffLocation, PickUpLocationName, PickUpLocation, RequestDate, RegionName  
   , RegionCode, LastAttendedDate, Code, DefaultScheduleDays, UsedRespiteHours, ShortName, PlacementRequirement, ReferralSiblingMappingVlaue,NextAttDate,NextAttDateCount  
  
   FROM  (  
   SELECT DISTINCT R.ReferralID,R.LastName+', '+R.FirstName as Name,R.Gender,R.Dob, dbo.GetAge(R.Dob) Age, R.NeedPrivateRoom                      
   , P.PayorName, DrpTL.Location DropOffLocationName,DrpTL.TransportLocationID DropOffLocation,                      
   PickUPL.Location PickUpLocationName,PickUPL.TransportLocationID PickUpLocation,     
   --RequestDate=NULL,       
   dbo.GetNextScheduleRequestDate(ISNULL(R.ScheduleRequestDates,'')) as RequestDate,          
   Reg.RegionName,Reg.RegionCode,R.LastAttendedDate,F.Code,F.DefaultScheduleDays,P.ShortName,R.PlacementRequirement,            
   -- rrp.UsedRespiteHours,  
   --(select min(StartDate) from ScheduleMasters where ReferralID=R.ReferralID and StartDate >= GETUTCDATE() and IsDeleted=0) as NextAttDate,       
     
    ROW_NUMBER() OVER ( PARTITION BY R.ReferralID ORDER BY SM.StartDate DESC) AS NextAttDateCount,   
 SUM(DATEDIFF(mi, SM.StartDate,SM.EndDate) / 60.0) OVER ( PARTITION BY R.ReferralID ORDER BY SM.StartDate ASC) AS UsedRespiteHours,   
 NextAttDate=SM.StartDate,        
                  
  --(SELECT STUFF((SELECT '~' + RN.LastName + ' '+RN.FirstName+ ' ; ' + CONVERT (varchar,RN.ReferralID)              
  -- FROM Referrals RN           
  -- where ReferralID in (select (case when RM.ReferralID1 = R.ReferralID then RM.ReferralID2 else RM.ReferralID1 end)  as ReferralID            
  --from ReferralSiblingMappings RM           
  --where RM.ReferralID1= R.ReferralID  or RM.ReferralID2=R.ReferralID)           
  -- FOR XML PATH('')),1,1,'') ) ReferralSiblingMappingVlaue   
      
   (SELECT STUFF((  
     
   SELECT '~' +  
    (CASE WHEN RM1.ReferralID1 = R.ReferralID   
     THEN RL2.LastName + ' '+RL2.FirstName+ ' ; ' + CONVERT (varchar,RL2.ReferralID)  
     ELSE RL1.LastName + ' '+RL1.FirstName+ ' ; ' + CONVERT (varchar,RL1.ReferralID) END  
 )  FROM ReferralSiblingMappings RM1  
   INNER JOIN Referrals RL1 ON RL1.ReferralID=RM1.ReferralID1  
   INNER JOIN Referrals RL2 ON RL2.ReferralID=RM1.ReferralID2  
   WHERE RM1.ReferralID1= R.ReferralID  or RM1.ReferralID2=R.ReferralID           
   FOR XML PATH('')),1,1,'') ) ReferralSiblingMappingVlaue          
         
                 
   from Referrals R                      
    LEFT join ReferralPayorMappings RPM on R.ReferralID = RPM.ReferralID and RPM.IsActive=1 and RPM.IsDeleted=0                      
    LEFT join Payors P on P.PayorID = RPM.PayorID                       
    inner join ContactMappings CM on CM.ReferralID=R.ReferralID and CM.ContactTypeID =@ContactTypeID                      
    inner join Contacts C on C.ContactID = CM.ContactID                         
    LEFT join TransportLocations DrpTL on DrpTL.TransportLocationID = R.DropOffLocation                      
    LEFT join TransportLocations PickUPL on PickUPL.TransportLocationID = R.PickUpLocation                      
    LEFT join Regions Reg on Reg.RegionID = R.RegionID                      
    LEFT join FrequencyCodes F on R.FrequencyCodeID = F.FrequencyCodeID                      
    LEFT join ReferralRespiteUsageLimit rrp on rrp.ReferralID=r.ReferralID and rrp.IsActive=1  and  rrp.StartDate <=GETUTCDATE()            
    LEFT join  ReferralSuspentions RS on RS.ReferralID=R.ReferralID    
    LEFT JOIN  ScheduleMasters SM ON  SM.ReferralID=R.ReferralID AND ISNULL(SM.OnHold, 0) = 0 AND SM.ISDeleted=0  
   where                      
    (R.ReferralStatusID = 1 and R.IsDeleted=0) -- AND (SM.ISDeleted IS NULL OR SM.ISDeleted=0)                     
    and (@RegioinID=0 or R.RegionID = @RegioinID)                        
    and (@FrequencyCodeID = 0 or R.FrequencyCodeID = @FrequencyCodeID)                      
    and ((CAST(@ServiceID AS bigint)=-1)                      
    OR  (CAST(@ServiceID AS bigint) = 0 and R.RespiteService = 1)                       
    OR  (CAST(@ServiceID AS bigint) = 1 and R.LifeSkillsService = 1)    
    OR  (CAST(@ServiceID AS bigint) = 2 and R.CounselingService = 1)  
 OR  (CAST(@ServiceID AS bigint) = 3 and r.ConnectingFamiliesService = 1))                      
    and (DATEDIFF(YEAR,R.Dob,GETDATE()) between @MinAge and @MaxAge )                      
    and (@PayorID = 0 or P.PayorID = @PayorID)                      
    and (@Gender = 0 or ( (@Gender = 1 and R.Gender='M') or (@Gender = 2 and R.Gender='F') ))                      
    and           
              
     ((@ContactName IS NULL OR LEN(c.LastName)=0)           
        OR(          
       (c.FirstName LIKE '%'+@ContactName+'%' )OR            
    (c.LastName  LIKE '%'+@ContactName+'%') OR            
    (C.FirstName +' '+C.LastName like '%'+@ContactName+'%') OR            
    (C.LastName +' '+C.FirstName like '%'+@ContactName+'%') OR            
    (C.FirstName +', '+C.LastName like '%'+@ContactName+'%') OR            
    (C.LastName +', '+C.FirstName like '%'+@ContactName+'%')))              
    --(@ContactName is null or(C.FirstName+' '+C.LastName) like '%'+@ContactName+'%' )              
 AND           
 ((@Name IS NULL OR LEN(r.LastName)=0)           
            
    OR          
   ((r.FirstName LIKE '%'+@Name+'%' )OR            
  (r.LastName  LIKE '%'+@Name+'%') OR            
  (r.FirstName +' '+r.LastName like '%'+@Name+'%') OR            
  (r.LastName +' '+r.FirstName like '%'+@Name+'%') OR            
  (r.FirstName +', '+r.LastName like '%'+@Name+'%') OR            
  (r.LastName +', '+r.FirstName like '%'+@Name+'%'))          
    )          
 AND R.FrequencyCodeID NOT IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@IgnoreFrequency))     
 AND (RS.ReturnEligibleDate is null OR CONVERT(VARCHAR(10),RS.ReturnEligibleDate,101)<=CONVERT(VARCHAR(10), GETDATE(),101))                   
      
  
  
  
     
   ) AS t   WHERE  NextAttDateCount=1        
                 
   ) AS t1 )                      
                       
 SELECT * FROM CTEReferralListForSchedulings WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                
                   
END