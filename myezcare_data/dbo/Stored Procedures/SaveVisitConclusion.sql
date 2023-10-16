CREATE PROCEDURE [dbo].[SaveVisitConclusion]  
@EmployeeVisitID bigint,      
@EmployeeVisitNoteID bigint,      
@ReferralTaskMappingID bigint,          
@Description nvarchar(1000),  
@SystemID VARCHAR(100),                      
@LoggedInID bigint          
AS                        
BEGIN                        
      
 IF EXISTS (SELECT TOP 1 EmployeeVisitNoteID FROM EmployeeVisitNotes WHERE EmployeeVisitID=@EmployeeVisitID AND ReferralTaskMappingID=@ReferralTaskMappingID AND EmployeeVisitNoteID!=@EmployeeVisitNoteID)      
 BEGIN                  
 SELECT -1 RETURN;                    
 END  
      
IF(@EmployeeVisitNoteID = 0)      
 BEGIN      
  INSERT INTO EmployeeVisitNotes (EmployeeVisitID,Description,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,ReferralTaskMappingID)          
  VALUES (@EmployeeVisitID,@Description,GETDATE(),@LoggedInID,GETDATE(),@LoggedInID,@SystemID,@ReferralTaskMappingID)          
 END      
ELSE      
 BEGIN      
  UPDATE EmployeeVisitNotes SET ReferralTaskMappingID=@ReferralTaskMappingID,Description=@Description,UpdatedBy=@LoggedInID,UpdatedDate=GETDATE(),SystemID=@SystemID      
  WHERE EmployeeVisitNoteID=@EmployeeVisitNoteID      
 END            
 SELECT 1; RETURN;                    
                    
                    
END 
