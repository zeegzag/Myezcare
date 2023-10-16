CREATE PROCEDURE [dbo].[SaveVisitConclusion]    
@EmployeeVisitID bigint,        
@EmployeeVisitNoteID bigint,        
@ReferralTaskMappingID bigint,            
@Description nvarchar(1000),   
@AlertComment nvarchar(1000)=null, 
@SystemID VARCHAR(100),                        
@LoggedInID bigint            
AS                          
BEGIN                          
     --UpdatedBy:Akhilesh kamal
--UpdateDate:17/01/2020
--Description:for check duplicate entry uncommente    
 IF EXISTS (SELECT TOP 1 EmployeeVisitNoteID FROM EmployeeVisitNotes WHERE EmployeeVisitID=@EmployeeVisitID AND ReferralTaskMappingID=@ReferralTaskMappingID AND EmployeeVisitNoteID!=@EmployeeVisitNoteID)        
 BEGIN                    
 SELECT -1 RETURN;                      
 END    
        
IF(@EmployeeVisitNoteID = 0)        
 BEGIN        
  INSERT INTO EmployeeVisitNotes (EmployeeVisitID,Description,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,ReferralTaskMappingID,AlertComment)            
  VALUES (@EmployeeVisitID,@Description,GETDATE(),@LoggedInID,GETDATE(),@LoggedInID,@SystemID,@ReferralTaskMappingID,@AlertComment)            
 END        
ELSE        
 BEGIN        
  UPDATE EmployeeVisitNotes SET ReferralTaskMappingID=@ReferralTaskMappingID,Description=@Description,UpdatedBy=@LoggedInID,UpdatedDate=GETDATE(),SystemID=@SystemID,AlertComment=@AlertComment        
  WHERE EmployeeVisitNoteID=@EmployeeVisitNoteID        
 END              
 SELECT 1; RETURN;                      
                      
                      
END