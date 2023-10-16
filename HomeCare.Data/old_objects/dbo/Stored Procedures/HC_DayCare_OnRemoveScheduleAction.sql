CREATE PROCEDURE [dbo].[HC_DayCare_OnRemoveScheduleAction]  
@ScheduleID BIGINT,   
@ReferralTSDateID BIGINT,  
@RemoveScheduleNotes NVARCHAR(100),        
@loggedInId BIGINT,        
@SystemID NVARCHAR(MAX),    
@isSaveNoteOnly BIT=0        
  
AS        
BEGIN        
         
   
 IF(@ScheduleID>0)  
  BEGIN  
   IF(@isSaveNoteOnly=1)    
    BEGIN  
  UPDATE ReferralTimeSlotDates  SET Notes=@RemoveScheduleNotes   
  WHERE  ReferralTSDateID IN (Select ReferralTSDateID from ScheduleMasters where ScheduleID=@ScheduleID)            
    END  
   ELSE    
    BEGIN    
     UPDATE ReferralTimeSlotDates SET UsedInScheduling=0,Notes=@RemoveScheduleNotes   
     WHERE  ReferralTSDateID IN (SELECT ReferralTSDateID FROM ScheduleMasters WHERE ScheduleID=@ScheduleID)            
                    
     UPDATE ScheduleMasters SET IsDeleted=1, UpdatedBy=@loggedInId,UpdatedDate=GETUTCDATE()   
     WHERE ScheduleID=@ScheduleID    
     
      EXEC [dbo].[ScheduleEventBroadcast] 'DeleteSchedule', @ScheduleID,'',''
    
  UPDATE Notes SET IsDeleted=1, UpdatedBy=@loggedInId,UpdatedDate=GETUTCDATE()   
     WHERE ScheduleID=@ScheduleID    
       
    END  
  END  
 ELSE  
  BEGIN  
    IF(@isSaveNoteOnly=1)    
    BEGIN  
  UPDATE ReferralTimeSlotDates  SET Notes=@RemoveScheduleNotes WHERE  ReferralTSDateID=ISNULL(@ReferralTSDateID,0)  
    END  
   ELSE    
    BEGIN    
      UPDATE ReferralTimeSlotDates SET UsedInScheduling=0,Notes=@RemoveScheduleNotes   
      WHERE  ReferralTSDateID=ISNULL(@ReferralTSDateID,0)  
                    
    END  
  END  
  
  
  
  
  SELECT 1 RETURN;        
        
          
END  