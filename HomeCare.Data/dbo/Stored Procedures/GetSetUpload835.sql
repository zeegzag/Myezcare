CREATE PROCEDURE [dbo].[GetSetUpload835]    
          
AS               
             
BEGIN                        
 select * from Payors where IsDeleted=0 AND IsBillingActive=1  Order BY PayorName;  
 SELECT 0;  
END