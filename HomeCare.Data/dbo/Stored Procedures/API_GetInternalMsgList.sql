CREATE PROCEDURE [dbo].[API_GetInternalMsgList]        
 @FromIndex INT,            
 @ToIndex INT,            
 @SortType NVARCHAR(10),  
 @SortExpression NVARCHAR(100),                    
 @EmployeeID BIGINT,    
 @MessageType NVARCHAR(100)           
AS            
BEGIN            
          
 IF(@SortExpression IS NULL OR @SortExpression ='')            
 BEGIN            
  SET @SortExpression = 'CreatedDate'            
  SET @SortType='DESC'            
 END            
             
  
DECLARE @UnreadMsgCount BIGINT=0;  
SELECT @UnreadMsgCount=COUNT(ReferralInternalMessageID) FROM ReferralInternalMessages WHERE Assignee=@EmployeeID AND IsDeleted=0 AND IsResolved=0   
  
  
 ;WITH CTEIMList AS                
 (                     
  SELECT ROW_NUMBER() OVER             
      (ORDER BY            
       CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN T.AssignedDate END            
       END ASC,            
       CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN T.AssignedDate END            
       END DESC          
                   
      ) AS Row,*,COUNT(T.ReferralInternalMessageID) OVER() AS Count            
  FROM            
  (            
      
  SELECT ReferralInternalMessageID,Message=Note,IsResolved,ResolveDate,ResolvedComment, PatientName= R.LastName+', '+ R.FirstName, AssignedBy=EC.FirstName+' '+EC.LastName, AssignedDate=RIM.CreatedDate, UnreadMsgCount=@UnreadMsgCount    
  FROM ReferralInternalMessages RIM    
  INNER JOIN Referrals R ON R.ReferralID=RIM.ReferralID    
  INNER JOIN Employees EC ON EC.EmployeeID=RIM.CreatedBy    
  WHERE RIM.Assignee=@EmployeeID AND RIM.IsDeleted=0 AND    
 ( (@MessageType='read' AND RIM.IsResolved=1) OR (@MessageType='unread' AND RIM.IsResolved=0)   
  OR (@MessageType='all')  
 )    
     
     
       
  ) AS T            
 )            
               
  SELECT * FROM CTEIMList WHERE ROW BETWEEN @FromIndex AND @ToIndex            
              
              
END