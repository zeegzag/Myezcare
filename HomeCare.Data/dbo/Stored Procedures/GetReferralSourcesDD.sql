CREATE PROCEDURE  GetReferralSourcesDD    
@ItemType  NVARCHAR(MAX)=NULL,  
@Isdeleted BIGINT=-1  
AS        
BEGIN     
 IF(@ItemType='ReferralSources')     
 BEGIN    
SELECT ReferralSourceID AS Value ,ReferralSourceName AS Name,IsDeleted  FROM ReferralSources   WHERE((CAST(@IsDeleted AS BIGINT)=-1) OR IsDeleted=@IsDeleted)             
 END    
ELSE IF  (@ItemType='ReferralStatuses')     
    BEGIN    
 SELECT ReferralStatusID AS Value ,Status AS Name,IsDeleted FROM ReferralStatuses    WHERE((CAST(@IsDeleted AS BIGINT)=-1) OR IsDeleted=@IsDeleted)            
    
 END    
    
END