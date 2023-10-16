-- exec [HC_GetReferralPayorMappingList] 14232,null,0,-1,null,null,null,'ASC',1,10                        
CREATE PROCEDURE [dbo].[HC_GetReferralBillingAuthorizationList]  
@ReferralID bigint=null,  
@PayorID bigint=null,  
@AuthorizationCode nvarchar(50)=null,  
@IsDeleted int = -1,  
@AuthType nvarchar(20)=null,  
@StartDate date=null,  
@EndDate date=null,  
@SORTEXPRESSION VARCHAR(100),  
@SORTTYPE VARCHAR(10),  
@FROMINDEX INT,  
@PAGESIZE INT  
AS                                                          
BEGIN                                                            
;WITH CTEReferralBillingAuthorization AS                                                      
 (                                                           
 SELECT *,COUNT(ReferralBillingAuthorizationID) OVER() AS COUNT FROM                                                   
 (                                                          
  SELECT ROW_NUMBER() OVER (ORDER BY                                                      
                                                      
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralBillingAuthorizationID' THEN ReferralBillingAuthorizationID END END ASC,                                                      
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralBillingAuthorizationID' THEN ReferralBillingAuthorizationID END END DESC,                                                      
                                      
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AuthorizationCode' THEN AuthorizationCode END END ASC,                                        
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AuthorizationCode' THEN AuthorizationCode END END DESC,                                 

 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PayorName' THEN P.PayorName END END ASC,                                        
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PayorName' THEN P.PayorName END END DESC,                                 

 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AllowedTime' THEN AllowedTime END END ASC,                                        
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AllowedTime' THEN AllowedTime END END DESC,                                 
                                                              
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'StartDate' THEN  CONVERT(date, StartDate, 105) END END ASC,                                                      
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'StartDate' THEN  CONVERT(date, StartDate, 105) END END DESC,                                                      
                                  
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EndDate' THEN  CONVERT(date, EndDate, 105) END END ASC,    
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EndDate' THEN  CONVERT(date, EndDate, 105) END END DESC                                                     
                           
    ) AS ROW,    
 RB.ReferralBillingAuthorizationID, RB.ReferralID, RB.AuthorizationCode, RB.StartDate, RB.EndDate,RB.IsDeleted,RB.PayorID, P.PayorName, RB.AllowedTime,
 StrServiceCodeIDs=
	 STUFF( (SELECT ', ' + CONVERT(VARCHAR(100),RBS.ServiceCodeID, 120)            
	 FROM ReferralBillingAuthorizationServiceCodes RBS 
	 WHERE RBS.ReferralBillingAuthorizationID=RB.ReferralBillingAuthorizationID AND RBS.IsDeleted=0
	 FOR XML PATH ('')), 1, 1, '')    
 FROM ReferralBillingAuthorizations   RB
 LEFT JOIN Payors P ON P.PayorID=RB.PayorID
 --INNER JOIN ReferralBillingAuthorizationServiceCodes RBS ON RBS.ReferralBillingAuthorizationID=RB.ReferralBillingAuthorizationID AND RBS.IsDeleted=0
 --INNER JOIN ServiceCodes SC ON SC.ServiceCodeID=RBS.ServiceCodeID
 WHERE Type=@AuthType AND  

 (@PayorID=0 OR RB.PayorID=@PayorID) AND
 (@ReferralID =(case when @ReferralID=0 then @ReferralID else ReferralID  end )) AND                                   
 ((@IsDeleted=-1) OR (RB.IsDeleted=@IsDeleted)) AND  
 ((@AuthorizationCode IS NULL) OR (AuthorizationCode LIKE '%' + @AuthorizationCode+ '%')) AND  
 ((@StartDate is null OR StartDate >= @StartDate) and (@EndDate is null OR EndDate<= @EndDate))    
      
   ) AS P1                                  
 )                                    
 SELECT * FROM CTEReferralBillingAuthorization WHERE ROW BETWEEN ((@PAGESIZE*(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)                                        
END
