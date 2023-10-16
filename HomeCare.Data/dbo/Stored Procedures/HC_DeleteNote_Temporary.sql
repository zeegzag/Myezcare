CREATE PROCEDURE HC_DeleteNote_Temporary        
@NoteID BIGINT ,        
@LoggedInID BIGINT,        
@PermanentDelete BIT = 0 ,      
@IsCaseManagement BIT      
AS        
BEGIN        
      
        
IF(@PermanentDelete=0)        
 BEGIN        
  UPDATE Notes_Temporary SET IsDeleted = 1, UpdatedBy = @LoggedInID, UpdatedDate = GETDATE() WHERE NoteID=@NoteID        
 END        
        
ELSE         
 BEGIN        
        
  IF(@IsCaseManagement=1)      
  BEGIN       
  
  DECLARE @ReferralID BIGINT=0;  
  SELECT @ReferralID=ReferralID FROM  Notes_Temporary WHERE NoteID=@NoteID      
           
  UPDATE RTD  SET RTD.UsedInScheduling = 0      
  FROM Notes_Temporary N      
  INNER JOIN ReferralTimeSlotDates RTD ON CONVERT(DATE,RTD.ReferralTSDate) = N.ServiceDate AND RTD.ReferralTSStartTime = N.StartTime      
  AND RTD.ReferralTSEndTime = N.EndTime   AND RTD.ReferralID = N.ReferralID    
  WHERE N.CreatedBy = @LoggedInID AND N.NoteID = @NoteID   AND RTD.ReferralID=@ReferralID  
    
      
  UPDATE Notes_Temporary SET IsDeleted = 1, UpdatedBy = @LoggedInID, UpdatedDate = GETDATE() WHERE NoteID=@NoteID       
      
  END      
  ELSE      
  BEGIN      
      
   DECLARE @ScheduleID BIGINT=0 ;        
   DECLARE  @EmployeeVisitID BIGINT=0 ;        
        
   SELECT @EmployeeVisitID= EmployeeVisitID FROM Notes_Temporary WHERE NoteID=@NoteID        
   SELECT @ScheduleID= ScheduleID FROM EmployeeVisits WHERE EmployeeVisitID= @EmployeeVisitID        
        
        
   UPDATE ScheduleMasters SET IsDeleted = 1, UpdatedBy = @LoggedInID, UpdatedDate = GETDATE() WHERE ScheduleID=@ScheduleID        
   UPDATE EmployeeVisits SET IsDeleted=1, UpdatedBy = @LoggedInID, UpdatedDate = GETDATE() WHERE  EmployeeVisitID=@EmployeeVisitID        
   UPDATE Notes_Temporary SET IsDeleted = 1, UpdatedBy = @LoggedInID, UpdatedDate = GETDATE() WHERE NoteID=@NoteID        
         
  END      
 END        
        
END