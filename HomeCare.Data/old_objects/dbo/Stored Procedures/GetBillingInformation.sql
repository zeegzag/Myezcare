/*
Created by : Neeraj Sharma
Created Date: 14 august 2020
Updated by :
Updated Date :

Purpose: This stored procedure is used to get the already created customer profile detail from authorised.net

*/

Create PROCEDURE [dbo].[GetBillingInformation]    
 @OrganizationId BIGINT         
 AS                                
BEGIN                                
                             
      
    
 Select   [OrganizationId]
           ,[CardNumber]
           ,[ExpirationDate]
           ,[AccountNumber]
           ,[RoutingNumber]
           ,[NameOnAccount]
           ,[BankName]
           ,[customerProfileId]
           ,[customerPaymentProfileId]
           ,[customerShippingAddressId]
           ,[CreatedDate]
   
            from   BillingInformation where OrganizationId=@OrganizationId
                      
END

