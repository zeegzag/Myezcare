--  EXEC [UpdateDxCode]          
CREATE PROCEDURE [dbo].[UpdateDxCode]                      
 @DXCodeID bigint = 0,   
 @ReferralID bigint = 0,                    
 @Precedence int = 0,
 @ReferralDXCodeMappingID bigint = 0             
          
           
AS                  
 BEGIN          
  UPDATE ReferralDXCodeMappings SET          
  DXCodeID=@DXCodeID,  
  ReferralID=@ReferralID,                    
  Precedence=@Precedence  
           
  where ReferralID = @ReferralID AND ReferralDXCodeMappingID=@ReferralDXCodeMappingID;          
                
END