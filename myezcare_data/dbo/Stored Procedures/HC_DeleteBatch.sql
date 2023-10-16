--exec DeleteBatch01 @ListOfIdsInCSV='108,194,196' ,@IsShowList=1          
CREATE PROCEDURE [dbo].[HC_DeleteBatch]          
@BatchTypeID bigint=0,                                                       
@PayorID bigint=0,      
@StartDate date=null,                                                            
@EndDate date=null,     
@Comment nvarchar(MAX)=null,    
@IsSentStatus int=-1,     
        
@SORTEXPRESSION NVARCHAR(100),                                                  
@SORTTYPE NVARCHAR(10),                                                
@FROMINDEX INT,                                                
@PAGESIZE INT,       
                              
@ListOfIdsInCSV varchar(MAX)=null,                                          
@IsShowList bit  
        
--@IND   INT=0,          
--@EIND INT =0,  
--@IsSent bit=0,         
--@TempBatchID bigint=0         
as          
BEGIN      
      
IF(LEN(@ListOfIdsInCSV)>0)                                          
BEGIN        
   
 --For Deleting Multiple Bacth   
     
 --SET @IND = CHARINDEX(',',@ListOfIdsInCSV)          
          
 --WHILE(@IND != LEN(@ListOfIdsInCSV))          
 --BEGIN          
    
 -- SET @EIND = ISNULL(((CHARINDEX(',', @ListOfIdsInCSV, @IND + 1)) - @IND - 1), 0)            
 -- set @TempBatchID= (SUBSTRING(@ListOfIdsInCSV, (@IND  + 1), @EIND))          
 -- set @IND = ISNULL(CHARINDEX(',', @ListOfIdsInCSV, @IND + 1), 0)          
              
 -- select @IsSent=IsSent from Batches  Where BatchID =@TempBatchID   
           
 -- IF(@IsSent=0)          
 -- BEGIN                 
     
 --  DELETE BatchApprovedServiceCodes where BatchID IN (@TempBatchID); -- SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))            
 --  DELETE BatchApprovedFacility where BatchID IN (@TempBatchID); -- SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))            
 --  DELETE BatchNotes WHERE BatchID IN (@TempBatchID); --(SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))           
 --  DELETE Batches WHERE BatchID IN (@TempBatchID); --(SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))              
 -- END           
 --END -- while end  
    
 Declare @IsSent BIT = CONVERT(BIGINT,@ListOfIdsInCSV) ;  
 Declare @TempBatchID BIGINT = CONVERT(BIGINT,@ListOfIdsInCSV) ;  
  
 select @IsSent=IsSent from Batches  Where BatchID =@TempBatchID  
   
 IF(@IsSent=0)          
 BEGIN                 
  DELETE BatchApprovedServiceCodes where BatchID = @TempBatchID;    
  DELETE BatchApprovedFacility where BatchID = @TempBatchID;           
  DELETE BatchNotes WHERE BatchID = @TempBatchID;        
  DELETE Batches WHERE BatchID = @TempBatchID;              
 END           
END    
         
IF(@IsShowList=1)                                          
BEGIN                                          
 EXEC HC_GetBatchList @BatchTypeID, @PayorID, @StartDate,@EndDate,@Comment,@IsSentStatus,@SORTEXPRESSION,@SORTTYPE,@FROMINDEX,@PAGESIZE                                  
END           
END
