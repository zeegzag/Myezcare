﻿--EXEC GetRtsMasterList @ReferralID = '37', @SortExpression = 'ReferralTimeSlotMasterID', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'
CREATE PROCEDURE [dbo].[GetRtsMasterList]          
 @ReferralID BIGINT = 0,                            
 @StartDate DATE = NULL,                            
 @EndDate DATE = NULL,                      
 @IsDeleted int=-1,                            
 @SortExpression NVARCHAR(100),                              
 @SortType NVARCHAR(10),                            
 @FromIndex INT,                            
 @PageSize INT                             
AS
BEGIN      
      
DECLARE @ActiveReferralTimeSlotMasterID BIGINT      
      
SELECT TOP 1 @ActiveReferralTimeSlotMasterID=etsdate.ReferralTimeSlotMasterID FROM ReferralTimeSlotDetails etsd      
 INNER JOIN ReferralTimeSlotDates etsdate ON etsdate.ReferralTimeSlotDetailID=etsd.ReferralTimeSlotDetailID      
 WHERE etsd.ReferralTimeSlotMasterID IN (SELECT ReferralTimeSlotMasterID FROM ReferralTimeSlotMaster WHERE ReferralID=@ReferralID) AND IsDeleted=0      
 AND ReferralTSDate >= CONVERT(DATE,GETDATE())      
 ORDER BY ReferralTSDate      
      
 ;WITH CTERtsMasterList AS                            
 (                             
  SELECT *,COUNT(t1.ReferralTimeSlotMasterID) OVER() AS Count FROM                             
  (                            
   SELECT ROW_NUMBER() OVER (ORDER BY                             
                        
                     
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralTimeSlotMasterID' THEN TBL1.ReferralTimeSlotMasterID END END ASC,                            
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralTimeSlotMasterID' THEN TBL1.ReferralTimeSlotMasterID END END DESC,                            
                         
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Name' THEN TBL1.Name END END ASC,                            
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Name' THEN TBL1.Name END END DESC,             
             
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'TotalRTSDetailCount' THEN TBL1.TotalRTSDetailCount END END ASC,                            
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'TotalRTSDetailCount' THEN TBL1.TotalRTSDetailCount END END DESC,                             
                            
    CASE WHEN @SortType = 'ASC' THEN                            
      CASE                             
      WHEN @SortExpression = 'StartDate' THEN TBL1.StartDate                            
      END                             
    END ASC,                            
    CASE WHEN @SortType = 'DESC' THEN                            
      CASE                             
      WHEN @SortExpression = 'EndDate' THEN TBL1.EndDate                            
      END                            
    END DESC          
  ) AS Row,  * FROM (                  
                    
   SELECT DISTINCT ets.ReferralTimeSlotMasterID,ets.ReferralID,Name=dbo.GetGeneralNameFormat(e.FirstName,e.LastName), ED.CareTypeId , CT.Title AS CareType,  
  -- CreatedBy=dbo.GetGeneralNameFormat(ec.FirstName,ec.LastName),UpdatedBy=dbo.GetGeneralNameFormat(eu.FirstName,eu.LastName),  
  -- ets.CreatedDate,ets.UpdatedDate,      
   ets.IsDeleted,ets.StartDate,ets.EndDate,ets.IsEndDateAvailable,  
   TotalRTSDetailCount= COUNT(ed.ReferralTimeSlotMasterID) OVER(PARTITION BY ed.ReferralTimeSlotMasterID),      
  ActiveStat=CASE WHEN ets.ReferralTimeSlotMasterID=@ActiveReferralTimeSlotMasterID THEN 1 ELSE 0 END,      
 -- ets.ReferralBillingAuthorizationID,  
  ISNULL((rba.AllowedTime /(case when rba.AllowedTimeType = 'Minutes' then 60 else 1 end)),0) as AllowedTime ,    
  ISNULL(((CASE     
  when ddm.Title = 'Daily' Then (rba.AllowedTime)    
  when ddm.Title = 'Weekly' Then (rba.AllowedTime/(7))     
  when ddm.Title = 'Monthly' Then (rba.AllowedTime/(30))     
  when ddm.Title = 'Yearly' Then (rba.AllowedTime/(365))     
  Else    
  (rba.AllowedTime)     
  end)      
     * (CASE WHEN ISNULL(DATEDIFF(DAY,ets.StartDate,ets.EndDate),0) = 0 THEN 1 else ISNULL(DATEDIFF(DAY,ets.StartDate,ets.EndDate),0) end))/(case when rba.AllowedTimeType = 'Minutes' then 60 else 1 end),0) AS ScheduledHours,      
  SUM(ISNULL(DATEDIFF(HOUR,ev.ClockInTime, ev.ClockOutTime),0)) AS UsedHours,      
  ISNULL((((CASE     
  when ddm.Title = 'Daily' Then (rba.AllowedTime)    
  when ddm.Title = 'Weekly' Then (rba.AllowedTime/(7))     
  when ddm.Title = 'Monthly' Then (rba.AllowedTime/(30))     
  when ddm.Title = 'Yearly' Then (rba.AllowedTime/(365))     
  Else    
  (rba.AllowedTime)     
  end)      
     * (CASE WHEN ISNULL(DATEDIFF(DAY,ets.StartDate,ets.EndDate),0) = 0 THEN 1 else ISNULL(DATEDIFF(DAY,ets.StartDate,ets.EndDate),0) end))/(case when rba.AllowedTimeType = 'Minutes' then 60 else 1 end)      
    - (SUM(ISNULL(DATEDIFF(HOUR,ev.ClockInTime, ev.ClockOutTime),0)))),0) AS PendingHours       
   FROM  ReferralTimeSlotMaster ets                           
   INNER JOIN Referrals e on e.ReferralID=ets.ReferralID                    
   INNER JOIN Employees eu on eu.EmployeeID=ets.UpdatedBy                    
-- Changed by Pallav to include the records for the timeslots that has no Prior Authorization- Changed Inner Join to Left Join  
   left JOIN [dbo].[ReferralBillingAuthorizations] AS rba     ON rba.ReferralID = e.ReferralID  AND (ets.StartDate  BETWEEN rba.StartDate AND rba.EndDate)   
   AND (ets.EndDate  BETWEEN rba.StartDate AND rba.EndDate) and rba.isdeleted=0 and rba.enddate>=getdate()  
-- Changed by Kundan on 18, Jan 2020: to make ReferralBillingAuthorizationServiceCodes optional, changed inner joing to left joing  
   left join  [ReferralBillingAuthorizationServiceCodes] as rbas on rbas.[ReferralBillingAuthorizationID]=rba.[ReferralBillingAuthorizationID]  
   LEFT JOIN ReferralTimeSlotDetails ED ON ets.ReferralTimeSlotMasterID=ED.ReferralTimeSlotMasterID AND ED.IsDeleted=0   
   LEFT join   [DDMaster] CT on CT.DDMasterID=   ED.CareTypeId   
   --Changed by Pallav- Added the condition to filter the records for service code based on the caretype mapped to the service code   
   --Changed by Kundan on 18, Jan 2020: to make visitTasks optional, changed inner joing to left joing  
   left join visitTasks as vt on vt.caretype=ct.DDMasterID and vt.ServiceCodeID=rbas.servicecodeid  
   left JOIN [dbo].[ScheduleMasters] AS sm ON sm.ReferralID = e.ReferralID AND (sm.StartDate BETWEEN ets.StartDate AND ets.EndDate) AND (sm.EndDate  BETWEEN ets.StartDate AND ets.EndDate)  
   LEFT JOIN [dbo].[EmployeeVisits] AS ev       ON ev.ScheduleID = sm.ScheduleID AND ev.IsDeleted = 0     
-- Changed by Pallav to include the records for the timeslots that has no Prior Authorization -- Changed Inner Join to Left Join  
   left JOIN [dbo].[DDMaster] ddm ON ddm.DDMasterID = rba.PriorAuthorizationFrequencyType --OR ddm.DDMasterID = ED.CareTypeId      
     
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR ets.IsDeleted=@IsDeleted)                            
   AND ((@ReferralID =0 OR LEN(@ReferralID)=0) OR ets.ReferralID=@ReferralID)             
   AND ((@StartDate IS NULL OR LEN(@StartDate)=0) OR ets.StartDate LIKE '%' + CONVERT(VARCHAR(20),@StartDate) + '%')                            
   AND ((@EndDate IS NULL OR LEN(@EndDate)=0) OR ets.EndDate LIKE '%' + CONVERT(VARCHAR(20),@EndDate) + '%')       
   --AND (ets.IsEndDateAvailable = 0 OR (ets.EndDate >= GETDATE()))  
   GROUP BY ets.ReferralTimeSlotMasterID,ets.ReferralID,e.FirstName,e.LastName,Ed.CareTypeId,ets.CreatedDate,ets.UpdatedDate,ets.IsDeleted,ets.StartDate,ets.EndDate,ets.IsEndDateAvailable   
   ,ed.ReferralTimeSlotMasterID,ets.ReferralTimeSlotMasterID,ets.ReferralBillingAuthorizationID,  
   rba.AllowedTime,rba.AllowedTimeType,CT.Title, ddm.Title    
                    
  )   AS TBL1                                
  ) AS t1  )                            
                       
 SELECT distinct ReferralTimeSlotMasterID, ReferralID, Name, CareTypeId, CareType, IsDeleted, StartDate, EndDate, IsEndDateAvailable, TotalRTSDetailCount,   
 ActiveStat, 0 AllowedTime, 0 ScheduledHours, 0 UsedHours, 0 PendingHours, Count  
 FROM CTERtsMasterList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                             
                            
END  
  
--select * from ReferralTimeSlotMaster where referralID=19  
--Select * from referralTimeSlotDetails RD inner join ReferralTimeSlotMaster RT on rd.ReferralTimeSlotMasterID=RT.ReferralTimeSlotMasterID where RT.ReferralID=19 and caretypeID=0  