-- EXEC SetMarkasSentBatch @SortExpression = 'BatchID', @SortType = 'DESC', @FromIndex = '1', @PageSize = '50', @IsSentStatus = '-1', @ListOfIdsInCSV = '47', @IsShowList = 'True', @MarkAsSentStatus = '4', @MarkAsUnSentStatus = '5', @SentReason = 'Mark As Sent', @UnSentReason = 'Mark As UnSent ', @IsSentBy = '1', @IsSent = 'True'
CREATE PROCEDURE [dbo].[SetMarkasSentBatch]                                
@BatchID bigint=0,                                               
@BatchTypeID bigint=0,                                               
@PayorID bigint=0,                                               
@BillingProviderIDs varchar(4000)=null,                                                                            
@StartDate date=null,                                                    
@EndDate date=null,                                          
@IsDeleted BIGINT = -1,  
@IsSentStatus int=-1,                                               
@SORTEXPRESSION NVARCHAR(100),                                          
@SORTTYPE NVARCHAR(10),                                        
@FROMINDEX INT,                                        
@PAGESIZE INT ,                                  
@ListOfIdsInCSV varchar(300)=null,                                            
@IsShowList bit,                          
@MarkAsSentStatus bigint ,                      
@MarkAsUnSentStatus bigint,                    
@SentReason varchar(100),                    
@UnSentReason varchar(100) ,                  
@IsSentBy bigint ,      
@IsSent bit                  
AS                                            
BEGIN                                                                      
 IF(LEN(@ListOfIdsInCSV)>0)                                            
 BEGIN                                          
   UPDATE Batches SET IsSent=@IsSent,IsSentBy=@IsSentBy,SentDate=GETUTCDATE()                     
   WHERE BatchID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))                      
   
   UPDATE BatchNotes SET                    
   BatchNoteStatusID = CASE BatchNoteStatusID WHEN @MarkAsSentStatus THEN @MarkAsUnSentStatus  ELSE @MarkAsSentStatus  END                  
   --Reason = CASE Reason WHEN @SentReason  THEN @UnSentReason  ELSE @SentReason END,                  
   --ReasonCode = CASE ReasonCode WHEN @MarkAsSentStatus THEN @MarkAsUnSentStatus  ELSE @MarkAsSentStatus  END                        
   WHERE BatchID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))                               


   EXEC GenerateBatchNotesAfterSent01 @ListOfIdsInCSV

 END                                            
 IF(@IsShowList=1)                                            
 BEGIN                                            
  EXEC GetBatchList01 @BatchID, @BatchTypeID, @PayorID,@BillingProviderIDs,@StartDate,@EndDate,@IsDeleted,@IsSentStatus,@SORTEXPRESSION,@SORTTYPE,@FROMINDEX,@PAGESIZE                                   
 END                                            
END 
