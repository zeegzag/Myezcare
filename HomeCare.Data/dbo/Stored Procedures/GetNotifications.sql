--   EXEC [dbo].[GetNotifications] 'Message', 'ASC', 1, 10, 1, 1
      
CREATE PROCEDURE [dbo].[GetNotifications]          
@SortExpression NVARCHAR(100),                            
@SortType NVARCHAR(10),                          
@FromIndex INT,                          
@PageSize INT,      
@EmployeeID BIGINT,
@IsDeleted NVARCHAR(10) = -1   
              
AS                          
BEGIN                          
                          
;WITH List AS                          
 (                           
  SELECT *,COUNT(T1.WebNotificationID) OVER() AS MsgCount FROM                           
  (                          
   SELECT ROW_NUMBER() OVER (ORDER BY                           
                      
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Message' THEN t.Message END END ASC,                    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Message' THEN t.Message END END DESC        
                  
   ) AS ROW,                          
   t.*  FROM     (                
          
     SELECT          
  WN.[WebNotificationID] AS WebNotificationID,          
  WN.[Message],          
  WN.[CreatedDate],          
  WUN.[IsRead],    
  WUN.[IsDeleted]       
 FROM          
  [dbo].[WebNotifications] WN          
 INNER JOIN [dbo].[WebUserNotifications] WUN         
  ON WN.[WebNotificationID] = WUN.[WebNotificationID]          
   AND WUN.[EmployeeID] = @EmployeeID           
   AND --WUN.[IsDeleted] = @IsDeleted  
   ((CAST(@IsDeleted AS bigint) = -1)  OR WUN.IsDeleted = @IsDeleted) 
  --  ORDER BY          
  --CreatedDate DESC      
        
       
      
) AS T      
    GROUP BY      
  WebNotificationID, Message, CreatedDate, IsRead, IsDeleted       
)  AS T1 )              
              
SELECT * FROM List WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                           
END 