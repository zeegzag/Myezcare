--EXEC HC_DeleteReferralBillingAuthorization @SortExpression = 'ReferralBillingAuthorizationID', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50', @PayorID = '0', @AuthorizationCode = '', @ReferralID = '24345', @IsDeleted  = '0', @AuthType  = 'CMS1500', @ListOfIdsInCSV = '10061', @IsShowList = 'True'    
CREATE PROCEDURE [dbo].[HC_DeletePriorAuthorization]    
@ReferralID bigint=null,  
@PayorID BIGINT,    
@AuthorizationCode nvarchar(50)=null,    
@IsDeleted int = -1,    
@AuthType nvarchar(20)=null,    
@StartDate date=null,    
@EndDate date=null,    
@SORTEXPRESSION VARCHAR(100),                                            
@SORTTYPE VARCHAR(10),                                          
@FROMINDEX INT,                                          
@PAGESIZE INT  ,                 
@ListOfIdsInCSV varchar(300)=null,                                  
@IsShowList bit                          
AS                                  
BEGIN                                      
                                  
 IF(LEN(@ListOfIdsInCSV)>0)                                  
 BEGIN                                
  UPDATE ReferralBillingAuthorizations SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END                             
  WHERE ReferralBillingAuthorizationID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))                                       
 END                                  
 IF(@IsShowList=1)                                  
 BEGIN                                  
  EXEC HC_GetPriorAuthorizationList @ReferralID,@PayorID,@AuthorizationCode,@IsDeleted,@AuthType,@StartDate,@EndDate,    
  @SORTEXPRESSION,@SORTTYPE,@FROMINDEX,@PAGESIZE            
 END                                  
END