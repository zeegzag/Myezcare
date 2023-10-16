
CREATE PROCEDURE [dbo].[HC_DeletePayor]                
@PayorName VARCHAR(255)=null,                        
@ShortName VARCHAR(255)=null,        
@Address NVARCHAR(100)=null,                
@AgencyNPID NVARCHAR(20)=null,  
@PayorBillingType NVARCHAR(20)=null,                      
@PayerGroup bigint=-1,         
@IsDeleted BIGINT = -1,                       
@SORTEXPRESSION NVARCHAR(100),                
@SORTTYPE NVARCHAR(10),              
@FROMINDEX INT,              
@PAGESIZE INT,        
@ListOfIdsInCSV  varchar(300)=null,                  
@IsShowList bit,            
@loggedInID BIGINT                 
AS                  
BEGIN                      
                  
 IF(LEN(@ListOfIdsInCSV)>0)                  
 BEGIN                
   UPDATE Payors SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as BIGINT) ,UpdatedDate=GETUTCDATE()            
   WHERE PayorID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))                       
 END                  
 IF(@IsShowList=1)                  
 BEGIN                  
  EXEC HC_GetPayorList @PayorName,@ShortName,@Address,@AgencyNPID,@PayorBillingType,@PayerGroup,@IsDeleted,        
  @SortExpression,@SortType ,@FromIndex,@PageSize                
   END                  
END