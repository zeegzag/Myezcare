CREATE PROCEDURE [dbo].[MappReferralVisitTask]  --exec MappReferralVisitTask 78,0,40,20173,'123','1,2,3,4',32    
@VisitTaskID BIGINT,  --SELECT * FROM ReferralTaskMappings WHERE VisitTaskID=78 AND ReferralID=40 AND IsDeleted=0    
@IsRequired BIT,      
@ReferralID BIGINT,      
@LoggedIn BIGINT,      
@SystemID VARCHAR(100),    
@Days VARCHAR(100),    
@Frequency VARCHAR(100),    
@Comment VARCHAR(100)    
--@ListOfIdsInCsv varchar(max) = NULL    
AS      
BEGIN      
       
    
  IF EXISTS (SELECT 1 FROM ReferralTaskMappings WHERE VisitTaskID=@VisitTaskID AND ReferralID=@ReferralID AND IsDeleted=0)      
   BEGIN      
   UPDATE ReferralTaskMappings set Comment=@Comment, Frequency = @Frequency, days=@Days where  VisitTaskID=@VisitTaskID AND ReferralID=@ReferralID      
  SELECT 1;       
       
   END      
       
   ELSE    
   BEGIN    
  INSERT INTO ReferralTaskMappings (VisitTaskID,IsRequired,IsDeleted,ReferralID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,Days,Frequency,Comment)    
  VALUES(@VisitTaskID,@IsRequired,0,@ReferralID,GETDATE(),@LoggedIn,GETDATE(),@LoggedIn,@SystemID,@Days,@Frequency,@Comment)      
  SELECT 2;       
  END    
END    
    
    
--select * from ReferralTaskMappings 