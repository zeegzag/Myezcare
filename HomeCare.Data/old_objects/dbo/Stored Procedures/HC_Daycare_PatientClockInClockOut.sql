CREATE PROCEDURE [adc].[HC_Daycare_PatientClockInClockOut]        
@ScheduleID  BIGINT=0,        
@EmployeeVisitID BIGINT=0,        
@ClockInTime DATETIME=NULL,        
@ClockOutTime DATETIME=NULL,        
@PatientSignature_ClockIN NVARCHAR(MAX),        
@PatientSignature_ClockOut NVARCHAR(MAX),        
@LoggedInID BIGINT,        
@PatientName NVARCHAR(100)='',        
@FacilityID BIGINT=0,        
@ReferralTaskMappingIDs  NVARCHAR(MAX)='',  
@IsSelf BIT=NULL,  
@Name NVARCHAR(100)='',  
@Relation NVARCHAR(100)=''  
AS        
BEGIN        
          
        
  IF(@ClockInTime='1900-01-01 00:00:00.000') SET  @ClockInTime=NULL;        
  IF(@ClockOutTime='1900-01-01 00:00:00.000') SET  @ClockOutTime=NULL;        
    
    
  UPDATE ScheduleMasters SET EmployeeID=FacilityID WHERE ScheduleID=@ScheduleID    
          
        
  IF(@EmployeeVisitID=0)        
  BEGIN        
 INSERT INTO EmployeeVisits(ScheduleID,ClockInTime,ClockOutTime,IsDeleted,IsSigned,PatientSignature,IsSelf,Name,Relation,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy)        
 VALUES(@ScheduleID,@ClockInTime,@ClockOutTime,0,1,@PatientSignature_ClockIN,@IsSelf,@Name,@Relation,GETDATE(),@LoggedInID,GETDATE(),@LoggedInID)        
  END        
  ELSE         
  BEGIN        
        
  UPDATE EmployeeVisits SET ClockInTime=@ClockInTime, ClockOutTime=@ClockOutTime, PatientSignature_ClockOut=@PatientSignature_ClockOut,  
  IsSelf=@IsSelf, Name = @Name,Relation=@Relation,UpdatedDate=GETDATE(),UpdatedBy=@LoggedInID,IsPCACompleted=1        
  WHERE EmployeeVisitID=@EmployeeVisitID        
        
        
  IF(LEN(@ReferralTaskMappingIDs)>0)        
  BEGIN        
        
        
  INSERT INTO EmployeeVisitNotes (EmployeeVisitID,ReferralTaskMappingID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy)                        
  SELECT @EmployeeVisitID, RTM.ReferralTaskMappingID,GETUTCDATE(),@LoggedInID,GETUTCDATE(),@LoggedInID FROM ReferralTaskMappings RTM        
  INNER JOIN VisitTasks VT ON VT.VisitTaskID=RTM.VisitTaskID        
  WHERE VT.IsDeleted=0 AND RTM.IsDeleted=0 AND RTM.ReferralTaskMappingID IN (SELECT VAL FROM GetCSVTable(@ReferralTaskMappingIDs))                     
            
        
        
  END        
        
        
  END        
        
        
  UPdate ScheduleMasters SET IsPatientAttendedSchedule=1 WHERE ScheduleID=@ScheduleID        
        
   EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @ScheduleID,'222','25'     
  EXEC ADC.HC_Daycare_GetScheduledPatientList @PatientName, @FacilityID        
        
END 