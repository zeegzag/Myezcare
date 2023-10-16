--TransportGroup    
--TransportGroupAssignPatient    
CREATE PROCEDURE [dbo].[SaveTransportGroupAssignPatientNote]                
@TransportGroupID BIGINT = 0,    
@ReferralID    BIGINT = 0,    
@loggedInID    BIGINT = 0 ,    
@Note     NVARCHAR(100) = ''    
AS                        
BEGIN                            
               
 UPDATE     
 [fleet].TransportGroupAssignPatient     
 SET     
 Note = @Note ,    
 UpdatedDate = GETDATE(),    
 UpdatedBy = @loggedInID    
 WHERE     
 TransportGroupID = @TransportGroupID AND    
 ReferralID  = @ReferralID  AND     
 IsNull(IsDeleted,0) = 0     
     
     SELECT 1; RETURN;                                                   
    
END 