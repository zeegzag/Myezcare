CREATE PROCEDURE [dbo].[OnTaskChecked]  
@ReferralTaskMappingID BIGINT,  
@ReferralID BIGINT,  
@IsRequired BIT,  
@TaskTypeTask VARCHAR(100),  
@TaskTypeConclusion VARCHAR(100),  
@LoggedIn BIGINT  
AS  
  
BEGIN  
   
 UPDATE  ReferralTaskMappings SET IsRequired=@IsRequired, UpdatedBy=@LoggedIn, UpdatedDate=GETDATE()  
 WHERE  ReferralTaskMappingID=@ReferralTaskMappingID AND ReferralID=@ReferralID  
  
 EXEC GetPatientTaskMappings  @ReferralID,@TaskTypeTask,@TaskTypeConclusion  
END  
  
  
