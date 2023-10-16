CREATE PROCEDURE [dbo].[HC_SetMarkasSentBatch]                                        
@BatchTypeID bigint=0,                                                       
@PayorID bigint=0,      
@StartDate date=null,                                                            
@EndDate date=null,     
@Comment nvarchar(MAX)=null,    
@IsSentStatus int=-1,       
      
@SORTEXPRESSION NVARCHAR(100),                                                  
@SORTTYPE NVARCHAR(10),                                                
@FROMINDEX INT,                                                
@PAGESIZE INT ,                                          
    
@ListOfIdsInCSV varchar(300)=null,                                                    
@IsShowList bit,                                  
    
@MarkAsSentStatus bigint ,                              
@MarkAsUnSentStatus bigint,     
@IsSentBy bigint,    
@IsSent bit                       
--@SentReason varchar(100) ,                          
--@UnSentReason varchar(100),    
                          
AS                                                    
BEGIN                                                                              
 IF(LEN(@ListOfIdsInCSV)>0)                                                    
 BEGIN                                                  
                               
           
   UPDATE BatchNotes SET                            
   BatchNoteStatusID = CASE BatchNoteStatusID WHEN @MarkAsSentStatus THEN @MarkAsUnSentStatus  ELSE @MarkAsSentStatus  END                          
   --Reason = CASE Reason WHEN @SentReason  THEN @UnSentReason  ELSE @SentReason END,                          
   --ReasonCode = CASE ReasonCode WHEN @MarkAsSentStatus THEN @MarkAsUnSentStatus  ELSE @MarkAsSentStatus  END                                
   WHERE BatchID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))                                       
        
   UPDATE Batches SET IsSent=@IsSent,IsSentBy=@IsSentBy,SentDate=GETUTCDATE()                             
   WHERE BatchID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV)) 

   EXEC HC_GenerateBatchNotesAfterSent @ListOfIdsInCSV        
        
 END                                                    
 IF(@IsShowList=1)                                                    
 BEGIN                                                    
  EXEC HC_GetBatchList @BatchTypeID, @PayorID, @StartDate,@EndDate,@Comment,@IsSentStatus,@SORTEXPRESSION,@SORTTYPE,@FROMINDEX,@PAGESIZE                                           
 END                                                    
END
