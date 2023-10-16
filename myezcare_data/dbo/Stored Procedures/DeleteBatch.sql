--exec DeleteBatch01 @ListOfIdsInCSV='108,194,196' ,@IsShowList=1    
CREATE PROCEDURE [dbo].[DeleteBatch]    
@BatchID bigint=0,                                       
@BatchTypeID bigint=0,                                       
@PayorID bigint=0,                                       
@BillingProviderIDs varchar(4000)=0,                                                                    
@StartDate date=null,                                            
@EndDate date=null,                                  
@IsDeleted BIGINT = -1,
@IsSentStatus int=-1,             
@SORTEXPRESSION NVARCHAR(100)=null,                                  
@SORTTYPE NVARCHAR(10)=null,     
@FROMINDEX INT=1,                                
@PAGESIZE INT=100 ,                           
@ListOfIdsInCSV varchar(8000)=null,                                    
@IsShowList bit,  
@IND   INT=0,    
@EIND INT =0,    
@IsSent bit=0,    
@TempBatchID bigint=0    
as    
BEGIN    
IF(LEN(@ListOfIdsInCSV)>0)                                    
BEGIN     
  
SET @IND = CHARINDEX(',',@ListOfIdsInCSV)    
    
WHILE(@IND != LEN(@ListOfIdsInCSV))    
 BEGIN    
   SET @EIND = ISNULL(((CHARINDEX(',', @ListOfIdsInCSV, @IND + 1)) - @IND - 1), 0)      
   set @TempBatchID= (SUBSTRING(@ListOfIdsInCSV, (@IND  + 1), @EIND))    
   set @IND = ISNULL(CHARINDEX(',', @ListOfIdsInCSV, @IND + 1), 0)    
        
     select @IsSent=IsSent from Batches  Where BatchID =@TempBatchID    
  IF(@IsSent=0)    
   BEGIN           
      DELETE BatchApprovedServiceCodes where BatchID IN (@TempBatchID); -- SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))      
	  DELETE BatchApprovedFacility where BatchID IN (@TempBatchID); -- SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))      
      DELETE BatchNotes WHERE BatchID IN (@TempBatchID); --(SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))     
      DELETE Batches WHERE BatchID IN (@TempBatchID); --(SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))        
   END     
 END     
END     
  IF(@IsShowList=1)                                    
   BEGIN                                    
    EXEC GetBatchList01 @BatchID, @BatchTypeID,@PayorID,null,@StartDate,@EndDate,@IsDeleted,@IsSentStatus,@SORTEXPRESSION,@SORTTYPE,@FROMINDEX,@PAGESIZE                           
   END     
END
