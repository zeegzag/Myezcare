CREATE PROCEDURE [dbo].[HC_DayOffAction]    
 @EmployeeDayOffID BIGINT,    
 @DayOffStatus NVARCHAR(50),        
 @ApproverComment NVARCHAR(1000),    
 @loggedInID BIGINT,     
 @SystemID VARCHAR(100)    
AS      
BEGIN          
    
 IF(@EmployeeDayOffID>0)    
 BEGIN    
   UPDATE EmployeeDayOffs    
   SET DayOffStatus=@DayOffStatus,    
       ApproverComment=@ApproverComment,  
    ActionTakenBy=@loggedInID,  
    ActionTakenDate=GETDATE(),  
    UpdatedDate=GETDATE(),    
    UpdatedBy=@loggedInID    
   WHERE EmployeeDayOffID=@EmployeeDayOffID    
 END    
     
 SELECT 1;    
END