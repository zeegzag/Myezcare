CREATE PROCEDURE [dbo].[DeleteReferralSourcesDD]      
    @id BIGINT   =0,    
 @IsDeleted BIGINT=0,    
 @ItemType  NVARCHAR(MAX)=NULL    
AS                      
BEGIN        
 IF(@ItemType='ReferralSources')      
  IF(@IsDeleted=0)    
  BEGIN    
    UPDATE [dbo].[ReferralSources]  SET IsDeleted=1 WHERE  ReferralSourceID = @id      
    END    
    ELSE    
    BEGIN    
    UPDATE [dbo].[ReferralSources]  SET IsDeleted=0 WHERE  ReferralSourceID = @id      
    END    
  ELSE IF(@ItemType='ReferralStatuses')      
  IF(@IsDeleted=0)    
  BEGIN    
    UPDATE [dbo].[ReferralStatuses]  SET IsDeleted=1 WHERE  ReferralStatusID = @id      
    END    
    ELSE    
    BEGIN    
    UPDATE [dbo].[ReferralStatuses]  SET IsDeleted=0 WHERE  ReferralStatusID = @id      
    END  
END