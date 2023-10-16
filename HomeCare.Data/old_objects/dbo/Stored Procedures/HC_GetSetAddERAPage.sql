-- =============================================
-- Author:		Kundan Kumar Rai
-- Create date: 2 March, 2020
-- Description:	This stored procedure set and get ERA page for filter
CREATE PROCEDURE [dbo].[HC_GetSetAddERAPage]              
@ClaimID BIGINT                        
AS                         
                       
BEGIN        
                                   
select * from Payors where IsDeleted=0 AND IsBillingActive=1  AND PayorInvoiceType=1 order by PayorName;       
 
END
