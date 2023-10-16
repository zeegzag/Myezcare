CREATE PROCEDURE [dbo].[HC_DayCare_DeleteScheduleFromCalender]  
  @ListOfIdsInCSV VARCHAR(300),  
  @loggedInId BIGINT  
AS  
BEGIN      
  
 IF(LEN(@ListOfIdsInCSV)>0)  
 BEGIN  
     
  UPDATE ScheduleMasters SET IsDeleted=1, UpdatedBy=@loggedInId, UpdatedDate=GETUTCDATE()  
  WHERE ScheduleID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))  

  DECLARE @CurScheduleID bigint;
 	DECLARE eventCursor CURSOR FORWARD_ONLY FOR
            SELECT ScheduleID FROM ScheduleMasters WHERE ScheduleID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV));
        OPEN eventCursor;
        FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        WHILE @@FETCH_STATUS = 0 BEGIN
            EXEC [dbo].[ScheduleEventBroadcast] 'DeleteSchedule', @CurScheduleID,'',''
            FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        END;
        CLOSE eventCursor;
        DEALLOCATE eventCursor;
  
  UPDATE Notes SET IsDeleted=1, UpdatedBy=@loggedInId, UpdatedDate=GETUTCDATE()  
  WHERE ScheduleID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))  
      
 END  
  
   
END  