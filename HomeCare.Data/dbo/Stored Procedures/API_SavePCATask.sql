--EXEC API_SavePCATask @EmployeeVisitID = N'30152', @OtherActivity = N'Hey ', @OtherActivityTime = N'20', @EmployeeID = N'170', @ServiceCodeID = N'49', @CareTypeID = N'1'
CREATE PROCEDURE [dbo].[API_SavePCATask]      
 @EmployeeVisitID BIGINT,                
 @OtherActivity VARCHAR(MAX),      
 @OtherActivityTime BIGINT,    
 @EmployeeID BIGINT,    
 @ServiceCodeID BIGINT,  
 @CareTypeID BIGINT    
AS                          
BEGIN              
    DECLARE @EmployeeVisitNoteID BIGINT;    
 Select TOP 1 @EmployeeVisitNoteID=EmployeeVisitNoteID from EmployeeVisitNotes WHERE EmployeeVisitID=@EmployeeVisitID AND ServiceTime>0 AND ReferralTaskMappingID IS NULL    
    
 IF(@EmployeeVisitNoteID>0)    
 BEGIN    
  UPDATE EmployeeVisitNotes SET Description=@OtherActivity,ServiceTime=@OtherActivityTime,ServiceCodeID=@ServiceCodeID,CareTypeID=@CareTypeID  
  WHERE EmployeeVisitNoteID=@EmployeeVisitNoteID    
 END    
 ELSE    
 BEGIN    
  INSERT INTO EmployeeVisitNotes (EmployeeVisitID,Description,ServiceTime,ServiceCodeID,CareTypeID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy)    
  VALUES (@EmployeeVisitID,@OtherActivity,@OtherActivityTime,@ServiceCodeID,@CareTypeID,GETDATE(),@EmployeeID,GETDATE(),@EmployeeID)    
 END    
   
 --Generate Note for Other Task  
 --EXEC API_AddNote @EmployeeVisitID,@EmployeeID  
    
  --Update EmployeeVisits    
  --SET OtherActivity=@OtherActivity,OtherActivityTime=@OtherActivityTime,      
  --UpdatedDate=GETUTCDATE()      
  --WHERE EmployeeVisitID=@EmployeeVisitID    
END