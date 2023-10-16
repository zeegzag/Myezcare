CREATE PROCEDURE [dbo].[TransportGroupAssignmentGroupList]                                                    
(                      
@RegionID VARCHAR(100) = NULL,                                                     
@FacilityID VARCHAR(100) = NULL,                                                     
@StartDate VARCHAR(10) = NULL,                                
@EndDate VARCHAR(100) = NULL,                                                    
@ClientName varchar(100) = NULL,                       
@Address VARCHAR(100) = NULL,                                                                          
@IsDeleted BIGINT = -1,                                                      
@SortExpression VARCHAR(100) = '',                                                        
@SortType VARCHAR(10) = 'DESC',                                                      
@FromIndex INT=1,                                                      
@PageSize INT  =10  ,                      
@EmployeeId int = 0 ,    
@TransportGroupID VARCHAR(100) = NULL    
)                                                                
AS                                                                            
BEGIN                         
    
-----------------------------------------------------------------------    
DECLARE @TAP_Startdate DATETIME = @StartDate, @TAP_Enddate DATETIME =@EndDate    
 SELECT    
 @TAP_Startdate = IsNull(TG.Startdate, @StartDate),    
 @TAP_Enddate = IsNull(TG.EndDate, @EndDate)    
 FROM    
 [fleet].TransportGroup TG (NOLOCK)                 
 WHERE TransportGroupID = @TransportGroupID        
    
DECLARE @TransportAssignAvailablePatientIDs NVARCHAR(MAX) =                                  
(                  
 SELECT distinct STRING_AGG(CONVERT(NVARCHAR(max),RTSD.ReferralID), ', ')            
 FROM                   
 ReferralTimeSlotDates RTSD (NOLOCK)    
 WHERE        
 (    
 (ReferralTSStartTime between @TAP_Startdate and @TAP_Enddate)    
 or    
 (ReferralTSEndTime between @TAP_Startdate and @TAP_Enddate)    
 )    
    
)       
-----------------------------------------------------------------------    
DECLARE @LoginUserGroupIDs nvarchar(max) =                                                    
  (                                                    
    SELECT                                                    
      e.GroupIDs                                                    
    FROM dbo.Employees e   (NOLOCK)                                                  
    WHERE                                                    
      e.EmployeeID = @EmployeeId                                                    
  )                                                    
  ;                                                    
  WITH SCList AS                                                    
  (                                                    
    SELECT DISTINCT                                                    
      r.ReferralID                                                    
    FROM dbo.ScheduleMasters sm   (NOLOCK)                                                  
    INNER JOIN dbo.Referrals r    (NOLOCK)                                                 
      ON sm.ReferralID = r.ReferralID                                                    
      AND r.IsDeleted = 0                                                    
      AND r.ReferralStatusID = 1                                                    
    LEFT JOIN EmployeeVisits ev  (NOLOCK)                                                   
      ON ev.ScheduleID = sm.ScheduleID                                                    
    WHERE                                                    
      sm.EmployeeID = @EmployeeId                                                    
      AND (@StartDate is null or CONVERT(date, sm.StartDate) >= @StartDate)                      
   AND (@EndDate is null or CONVERT(date, sm.EndDate) <= @EndDate)                      
      AND sm.ScheduleStatusID = 2                                                    
      AND (ev.IsSigned = 0 OR ev.IsSigned IS NULL)              
   AND ((CAST(@RegionID AS bigint) = 0)                              OR r.RegionID = CAST(@RegionID AS bigint))                                            
),                       
 List AS                                                                            
 (                                                   
  SELECT *,                      
  COUNT(T1.ReferralID) OVER() AS Count FROM                                                          
  (                                                                            
   SELECT ROW_NUMBER() OVER (ORDER BY                                                                             
                                      
     CASE                                                    
          WHEN @SortType = 'ASC' THEN CASE                                     
    WHEN @SortExpression = 'ClientName' or @SortExpression =  '' THEN t.[Name]                          
    WHEN @SortExpression = 'Address' THEN t.Address                              
            END                             
        END ASC,                                          CASE                       
          WHEN @SortType = 'DESC' THEN CASE                                                    
              WHEN @SortExpression = 'ClientName' or @SortExpression =  '' THEN t.Name                                                        
              WHEN @SortExpression = 'Address' THEN t.Address                                  
            END                                                    
        END DESC                                                
                                                                      
                                                                    
   ) AS ROW,                                                                            
   t.*  FROM     (                                                                  
                                                        
select       Distinct                     
   r.ReferralID,                      
   r.LastName + ', ' + r.FirstName AS Name,                                     
   c.Address ,                      
   cast(1 as bit) as isAssigned  ,                  
   case when TG.TransportGroupID is null then null else TGP.TransportGroupAssignPatientID end TransportGroupAssignPatientID      ,            
   case when TG.TransportGroupID is null then null else TGP.TransportGroupID end TransportGroupID,            
   r.RegionID            
  from                      
   Referrals r                                                                                                   
        LEFT JOIN ContactMappings cmp (NOLOCK)                                                    
          ON cmp.ReferralID = r.ReferralID                                                    
          AND cmp.ContactTypeID = 1                                                    
        LEFT JOIN Contacts c (NOLOCK)                                                    
          ON c.ContactID = cmp.ContactID                                                    
        LEFT JOIN referralGroup rg  (NOLOCK)                                                   
          ON rg.ReferralID = r.ReferralID                                       
    LEFT JOIN ReferralDocuments rd  (NOLOCK)                                     
    ON rd.UserID = r.ReferralID AND rd.FileName ='Client-FaceSheet' AND rd.ComplianceID=-5                                      
  LEFT JOIN DDmaster dm  (NOLOCK)                                                   
          ON dm.DDMasterID IN (select val from GetCSVTable(R.CareTypeIds))                        
  LEFT JOIN [fleet].TransportGroupAssignPatient TGP (NOLOCK)                  
  ON TGP.ReferralID = r.ReferralID                  
  and IsNull(TGP.IsDeleted,0) = 0    
  and TGP.ReferralID in (select val from GetCSVTable(@TransportAssignAvailablePatientIDs))    
      
  LEFT JOIN [fleet].TransportGroup TG (NOLOCK)                  
  ON TGP.TransportGroupID = TG.TransportGroupID               
  and TG.EndDate > GETUTCDATE()    
    
where                             
 ((CAST(@IsDeleted AS bigint) = -1)                 
            OR r.IsDeleted = @IsDeleted)                                                    
          AND ((@ClientName IS NULL                                                    
              OR LEN(r.LastName) = 0)                                                    
          OR                                                    
          ((r.FirstName LIKE '%' + @ClientName + '%')                                                    
              OR (r.LastName LIKE '%' + @ClientName + '%')                                                    
              OR (r.FirstName + ' ' + r.LastName LIKE '%' + @ClientName + '%')                                                    
              OR (r.LastName + ' ' + r.FirstName LIKE '%' + @ClientName + '%')                                                    
              OR (r.FirstName + ', ' + r.LastName LIKE '%' + @ClientName + '%')                                                    
              OR (r.LastName + ', ' + r.FirstName LIKE '%' + @ClientName + '%'))                                                    
          )                     
     AND                  
          (c.Address LIKE '%' + @Address + '%' or @Address is null)          
     AND (@RegionID is null                              OR r.RegionID = CAST(@RegionID AS bigint))           
     AND r.ReferralID in (select val from GetCSVTable(@TransportAssignAvailablePatientIDs))              
  ) t                                                      
                                       
)  AS T1 )                                                               
                                                                
SELECT * FROM List WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                                                                             
END 