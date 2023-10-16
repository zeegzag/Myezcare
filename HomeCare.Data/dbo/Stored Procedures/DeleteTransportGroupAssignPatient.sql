--TransportGroup    
--TransportGroupAssignPatient    
CREATE PROCEDURE [dbo].[DeleteTransportGroupAssignPatient]                
@TransportGroupID BIGINT = 0,    
@ReferralID    BIGINT = 0,    
@loggedInID    BIGINT = 0 ,    
@IsDeleted    BIT = 0    
AS                        
BEGIN                            
               
 UPDATE     
 [fleet].TransportGroupAssignPatient     
 SET     
 IsDeleted = @IsDeleted ,    
 UpdatedDate = GETDATE(),    
 UpdatedBy = @loggedInID    
 WHERE     
 TransportGroupID = @TransportGroupID AND    
 ReferralID  = @ReferralID  AND     
 IsNull(IsDeleted,0) = 0     
     
     SELECT 1; RETURN;                                                   
    
END 