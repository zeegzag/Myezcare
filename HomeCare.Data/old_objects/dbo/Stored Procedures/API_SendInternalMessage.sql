CREATE PROCEDURE [dbo].[API_SendInternalMessage]  
 @Assignee BIGINT,  
 @ReferralID BIGINT,  
 @EmployeeID BIGINT,  
 @Message NVARCHAR(MAX),  
 @SystemID VARCHAR(100)  
AS            
BEGIN            
 INSERT INTO ReferralInternalMessages VALUES (@Message,@ReferralID,@Assignee,0,GETDATE(),@EmployeeID,GETDATE(),@EmployeeID,@SystemID,0,NULL,0,NULL)  
END
