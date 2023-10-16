CREATE PROCEDURE [dbo].[AddReferralVisitTask]  
@VisitTaskID BIGINT,  
@IsRequired BIT,  
@ReferralID BIGINT,  
@LoggedIn BIGINT,  
@SystemID VARCHAR(100)  ,
@ListOfIdsInCsv varchar(max) = NULL
AS  
BEGIN  
   IF(LEN(@ListOfIdsInCsv)>0)                  
 BEGIN                    
   INSERT INTO ReferralTaskMappings (VisitTaskID,IsRequired,IsDeleted,ReferralID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID, Days)
  SELECT V.VisitTaskID,V.IsRequired,0,@ReferralID,GETDATE(),@LoggedIn,GETDATE(),@LoggedIn,@SystemID, '1,2,3,4,5,6,7'
  FROM VisitTasks v WHERE V.VisitTaskID IN (SELECT CAST([val] AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv))
   
  SELECT 1;    
  END 
   ELSE
   BEGIN

  IF EXISTS (SELECT 1 FROM ReferralTaskMappings WHERE VisitTaskID=@VisitTaskID AND ReferralID=@ReferralID AND IsDeleted=0)  
   BEGIN  
   SELECT -1;   
   RETURN;  
   END  
    
  INSERT INTO ReferralTaskMappings (VisitTaskID,IsRequired,IsDeleted,ReferralID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,Days)
  VALUES(@VisitTaskID,@IsRequired,0,@ReferralID,GETDATE(),@LoggedIn,GETDATE(),@LoggedIn,@SystemID,'1,2,3,4,5,6,7')  
  SELECT 1;   
  END
END
