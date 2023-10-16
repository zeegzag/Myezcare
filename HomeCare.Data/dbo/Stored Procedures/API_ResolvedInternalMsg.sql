CREATE PROCEDURE [dbo].[API_ResolvedInternalMsg]        
 @ReferralInternalMessageID BIGINT,    
 @ResolvedComment NVARCHAR(MAX)           
AS            
BEGIN            
       
 UPDATE ReferralInternalMessages SET IsResolved=1,ResolvedComment=@ResolvedComment,ResolveDate=GETDATE() WHERE  ReferralInternalMessageID=@ReferralInternalMessageID    
              
END