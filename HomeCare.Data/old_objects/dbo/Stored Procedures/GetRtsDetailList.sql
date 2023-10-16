-- EXEC GetRtsDetailList @EmployeeTimeSlotMasterID =8, @SortExpression = 'Name', @SortType = 'DESC', @FromIndex = '1', @PageSize = '100'                      
CREATE PROCEDURE [dbo].[GetRtsDetailList]          
 @ReferralTimeSlotMasterID BIGINT = 0,         
 @StartTime VARCHAR(20) = NULL,                      
 @EndTime VARCHAR(20) = NULL,                
 @IsDeleted int=-1,                      
 @SortExpression NVARCHAR(100),                        
 @SortType NVARCHAR(10),                      
 @FromIndex INT,                      
 @PageSize INT,  
 @CareTypeID BIGINT = 0  
AS                      
BEGIN                          
 ;WITH CTERtsDetailList AS                      
 (                       
  SELECT *,COUNT(t1.ReferralTimeSlotDetailID) OVER() AS Count FROM                       
  (                      
   SELECT ROW_NUMBER() OVER (ORDER BY                       
                  
               
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralTimeSlotDetailID' THEN etd.ReferralTimeSlotDetailID END END ASC,                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralTimeSlotDetailID' THEN etd.ReferralTimeSlotDetailID END END DESC,                      
                   
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Name' THEN e.FirstName END END ASC,                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Name' THEN e.FirstName END END DESC,                      
            
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Notes' THEN etd.Notes END END ASC,                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Notes' THEN etd.Notes END END DESC,      
       
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'UsedInScheduling' THEN etd.UsedInScheduling END END ASC,                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'UsedInScheduling' THEN etd.UsedInScheduling END END DESC,        
          
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Title' THEN d.Title END END ASC,                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Title' THEN d.Title END END DESC,        
                
    CASE WHEN @SortType = 'ASC' THEN                      
      CASE                       
      WHEN @SortExpression = 'StartTime' THEN StartTime              
      END                       
    END ASC,                      
    CASE WHEN @SortType = 'DESC' THEN                      
      CASE                       
      WHEN @SortExpression = 'EndTime' THEN EndTime                  
      END                      
    END DESC, Day, StartTime                      
  ) AS Row,                      
   etd.ReferralTimeSlotDetailID,etd.ReferralTimeSlotMasterID, Name=e.FirstName+ ' ' + e.LastName,      
   CreatedBy=ec.FirstName+ ' ' + ec.LastName,UpdatedBy=eu.FirstName+ ' ' + eu.LastName , etd.CreatedDate,etd.UpdatedDate,              
   etd.IsDeleted,etd.StartTime,etd.EndTime,etd.Day,etd.Notes,etd.UsedInScheduling,etd.CareTypeId,d.Title,etd.AnyTimeClockIn      
   FROM  ReferralTimeSlotDetails etd     (NOLOCK)         
   INNER JOIN ReferralTimeSlotMaster ets (NOLOCK)     on ets.ReferralTimeSlotMasterID=etd.ReferralTimeSlotMasterID              
   INNER JOIN Referrals e  (NOLOCK)     on e.ReferralID=ets.ReferralID              
   INNER JOIN Employees ec  (NOLOCK)     on ec.EmployeeID=etd.CreatedBy              
   INNER JOIN Employees eu  (NOLOCK)     on eu.EmployeeID=etd.UpdatedBy      
   LEFT JOIN DDMaster d     (NOLOCK)     ON d.DDMasterID=etd.CareTypeId      
              
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR etd.IsDeleted=@IsDeleted)                      
   AND ((@ReferralTimeSlotMasterID =0 OR LEN(@ReferralTimeSlotMasterID)=0) OR etd.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID)              
   AND ((@StartTime IS NULL OR LEN(@StartTime)=0) OR etd.StartTime LIKE '%' + @StartTime + '%')               
   AND ((@EndTime IS NULL OR LEN(@EndTime)=0) OR etd.EndTime LIKE '%' + @EndTime + '%')      
   AND (ISNULL(@CareTypeID, 0) = 0 OR etd.CareTypeId = @CareTypeID)  
   AND etd.IsDeleted=0          
  ) AS t1 )           
                 
 SELECT * FROM CTERtsDetailList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                       
    
END 