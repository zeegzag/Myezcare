--CreateBy                          CreatedDate                  UpdatedDate                 description        
--vikas srivastava                   27-05-2019                 ------------                 For current month Active Referral List        
        
-- EXEC GetActiveReferralList null,null,'ReferralID','desc','1','100'    
-- EXEC GetActiveReferralList '2019-03-1','2019-03-30','ReferralID','desc','1','100'  -- pass StartDate and EndDate                  
create PROCEDURE [dbo].[GetActiveReferralList]               
 @StartDate DATE = '',                                                                
 @EndDate DATE = '',                                      
 @SortExpression NVARCHAR(100),                                                                  
 @SortType NVARCHAR(10),                                                                
 @FromIndex INT,                                                                
 @PageSize INT                                                                 
AS                                                                
BEGIN                                                                            
 ;WITH CTEActiveReferralList AS                                                                
 (                                                                 
  SELECT *,COUNT(T1.ReferralID) OVER() AS Count FROM                                                                 
  (                                                                
   SELECT ROW_NUMBER() OVER (ORDER BY                    
                                                                  
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralID' THEN ReferralID END END ASC,                                     
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralID' THEN ReferralID END END DESC,                                                                       
                
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AHCCCSID' THEN CONVERT(varchar(50),T.AHCCCSID) END END ASC,                                
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AHCCCSID' THEN CONVERT(varchar(50),T.AHCCCSID) END END DESC                 
                         
  ) AS Row,                                   
  * FROM                  
   (                
   select DISTINCT r.ReferralID,r.FirstName +' '+r.LastName as Name,r.AHCCCSID,r.ReferralStatusID,        
   r.Gender, r.Dob, dbo.GetAge(r.Dob) as Age,                               
   r.CreatedDate,r.UpdatedDate              
  from JO_Referrals r where month(r.UpdatedDate)=(SELECT MONTH(GETDATE())) and YEAR(r.UpdatedDate)=(select YEAR(getdate()))    
  and r.IsDeleted=0    
  or ((@StartDate IS NULL) OR ( r.UpdatedDate>=@StartDate)) AND    
      ((@EndDate IS NULL) OR ( r.UpdatedDate<=@EndDate))    
 )AS T                    
 )AS T1                
 )                            
 SELECT * FROM CTEActiveReferralList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                 
 END
