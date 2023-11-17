CREATE PROCEDURE  SaveReferralSourcesDD      
@ItemType  NVARCHAR(MAX)=NULL,      
@Value BIGINT =0,      
@Name NVARCHAR(MAX)=NULL,      
@UpdatedBy NVARCHAR(MAX)=NULL      
AS  
BEGIN      
  if(@ItemType='ReferralSources')      
   IF EXISTS (SELECT TOP 1 ReferralSourceID FROM ReferralSources WHERE  ReferralSourceName=@Name)                
  BEGIN           
  SELECT -1 as Result RETURN;                              
  END       
   IF(@Value>0)      
   BEGIN      
   UPDATE ReferralSources SET ReferralSourceName=@Name where ReferralSourceID=@Value   
   SELECT 1 as Result RETURN;  
   END      
   ELSE      
   BEGIN      
   INSERT INTO ReferralSources (ReferralSourceName) VALUES (@Name)    
   SELECT 1 as Result RETURN;  
   END    
 IF(@ItemType='ReferralStatuses')      
   IF EXISTS (SELECT TOP 1 ReferralStatusID FROM ReferralStatuses WHERE Status=@Name)                
  BEGIN           
  SELECT -1 RETURN;                              
  END       
   IF(@Value>0)      
   BEGIN      
   UPDATE ReferralStatuses SET Status=@Name where ReferralStatusID=@Value   
   SELECT 1 as Result RETURN;  
   END      
   ELSE      
   BEGIN      
   Declare @Maxid int    
   Select @Maxid = coalesce(MAX(ReferralStatusID),0) + 1 FROM ReferralStatuses;    
   INSERT INTO ReferralStatuses (ReferralStatusID,Status,UsedInRespiteCare,UsedInHomeCare,IsDeleted) VALUES (@Maxid,@Name,1,1,0)    
   SELECT 1 as Result RETURN;  
   END     
      
       
         
      
END