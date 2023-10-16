CREATE PROCEDURE [dbo].[AddReferralVisitTask]  
@VisitTaskID BIGINT,  
@IsRequired BIT,  
@ReferralID BIGINT,  
@LoggedIn BIGINT,  
@SystemID VARCHAR(100)  
AS  
BEGIN  
   
  IF EXISTS (SELECT 1 FROM ReferralTaskMappings WHERE VisitTaskID=@VisitTaskID AND ReferralID=@ReferralID AND IsDeleted=0)  
   BEGIN  
   SELECT -1;   
   RETURN;  
   END  
    
  INSERT INTO ReferralTaskMappings (VisitTaskID,IsRequired,IsDeleted,ReferralID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)
  VALUES(@VisitTaskID,@IsRequired,0,@ReferralID,GETDATE(),@LoggedIn,GETDATE(),@LoggedIn,@SystemID)  
  SELECT 1;   
END
