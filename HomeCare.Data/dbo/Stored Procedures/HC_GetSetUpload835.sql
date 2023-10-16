CREATE PROCEDURE [dbo].[HC_GetSetUpload835]    
AS                     
                   
BEGIN                              
 SELECT * FROM Payors where IsDeleted=0 AND IsBillingActive=1 AND PayorInvoiceType=1 Order BY PayorName;        
 SELECT 0;        
END