CREATE PROCEDURE [dbo].[HC_PrivateDuty_RemoveSchedule]
  @ListOfIdsInCSV VARCHAR(300),  
  @loggedInId BIGINT  
AS  
BEGIN      
  
 IF(LEN(@ListOfIdsInCSV)>0)  
 BEGIN  
     
  UPDATE ScheduleMasters SET IsDeleted=1, UpdatedBy=@loggedInId, UpdatedDate=GETUTCDATE()  
  WHERE ScheduleID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))  
      
 END  
  
   
END
