Create PROCEDURE [dbo].[API_SentInternalMsgList]      
 @FromIndex INT,                    
 @ToIndex INT,                    
 @SortType NVARCHAR(10),          
 @SortExpression NVARCHAR(100),                            
 @EmployeeID BIGINT,            
 @MessageType NVARCHAR(100)                   
AS                    
BEGIN                    
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
 IF(@SortExpression IS NULL OR @SortExpression ='')                    
 BEGIN                    
  SET @SortExpression = 'CreatedDate'                    
  SET @SortType='DESC'                    
 END                    
                     
          
DECLARE @UnreadMsgCount BIGINT=0;          
SELECT @UnreadMsgCount=COUNT(ReferralInternalMessageID) FROM ReferralInternalMessages WHERE Assignee=@EmployeeID AND IsDeleted=0 AND IsResolved=0           
          
          
 ;WITH CTESentIMList AS                        
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
              
  SELECT ReferralInternalMessageID,Message=Note,IsResolved,ResolveDate,ResolvedComment, PatientName= dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat), AssignedBy=dbo.GetGenericNameFormat(ec.FirstName,ec.MiddleName, ec.LastName,@NameFormat), AssignedDate=RIM.CreatedDate, UnreadMsgCount=@UnreadMsgCount   
  
   
  FROM ReferralInternalMessages RIM            
  INNER JOIN Referrals R ON R.ReferralID=RIM.ReferralID            
  INNER JOIN Employees EC ON EC.EmployeeID=RIM.Assignee            
  WHERE RIM.CreatedBy=@EmployeeID AND RIM.IsDeleted=0 AND            
 ( (@MessageType='read' AND RIM.IsResolved=1) OR (@MessageType='unread' AND RIM.IsResolved=0)           
  OR (@MessageType='all')          
 )            
             
             
               
  ) AS T                    
 )                    
                       
  SELECT * FROM CTESentIMList WHERE ROW BETWEEN @FromIndex AND @ToIndex                    
                      
                      
END  