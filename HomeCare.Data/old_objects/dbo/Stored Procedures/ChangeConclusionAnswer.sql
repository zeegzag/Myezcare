CREATE PROCEDURE [dbo].[ChangeConclusionAnswer]  
@EmployeeVisitNoteID bigint,  
@Description nvarchar(1000),  
@SystemID VARCHAR(100),  
@LoggedInID bigint  
AS                      
BEGIN                      
 UPDATE EmployeeVisitNotes SET Description=@Description,SystemID=@SystemID,UpdatedBy=@LoggedInID,UpdatedDate=GETDATE()  
 Where EmployeeVisitNoteID=@EmployeeVisitNoteID  
END
