  
CREATE PROCEDURE [dbo].[BulkUpdateReferralList]          
@BulkUpdateType VARCHAR(MAX) = NULL,        
@ReferralIDs VARCHAR(MAX) = NULL,    
@AssignedValues NVARCHAR(MAX)= NULL   
          
AS            
BEGIN        
       
   IF  @BulkUpdateType = 'Group'      
   BEGIN    
       UPDATE [Referrals] SET      
       GroupIDs = @AssignedValues    
       WHERE ReferralID     
       IN (    
       SELECT val FROM dbo.GetCSVTable(@ReferralIDs))     
   END    
   ELSE  IF  @BulkUpdateType = 'Assignee'     
   BEGIN    
       UPDATE [Referrals] SET      
       Assignee = @AssignedValues    
       WHERE ReferralID     
       IN (    
       SELECT val FROM dbo.GetCSVTable(@ReferralIDs))     
   END    
   ELSE  IF  @BulkUpdateType = 'Status'      
   BEGIN    
       UPDATE [Referrals] SET      
       ReferralStatusID = @AssignedValues    
       WHERE ReferralID     
       IN (    
       SELECT val FROM dbo.GetCSVTable(@ReferralIDs))     
   END    
END    
    