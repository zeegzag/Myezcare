/*
Created by : Neeraj Sharma
Created Date: 12 August 2020
Updated by :
Updated Date :

Purpose: This stored procedure is used to save the already created customer profile detail from authorised.net
into table BillingInformation.

*/

CREATE PROCEDURE [dbo].[SaveBillingInformation]    
 @OrganizationId BIGINT,         
 @CardNumber varchar(100),
 @ExpirationDate varchar(100),
 @AccountNumber varchar(100),
 @RoutingNumber varchar(100),
 @NameOnAccount varchar(100),
 @BankName BIGINT,
 @customerProfileId BIGINT,
 @customerPaymentProfileId BIGINT,
 @customerShippingAddressId BIGINT,
 @Statuscode nvarchar(MAX),
 @ErrorCode nvarchar(MAX),
 @ErrorText nvarchar(MAX)
 AS                                
BEGIN                                
                             
                       
IF NOT EXISTS (SELECT TOP 1 ProfileNumber FROM BillingInformation WHERE (OrganizationId=@OrganizationId))          
BEGIN        
    
   INSERT INTO [dbo].[BillingInformation]
           ([OrganizationId]
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
		   ,[UpdatedDate]
		   ,[Statuscode]
		   ,[ErrorCode]
		   ,[ErrorText]
		   )
     VALUES
           (@OrganizationId,
            @CardNumber,
            @ExpirationDate,
            @AccountNumber,
            @RoutingNumber,
            @NameOnAccount,
			@BankName,
			@customerProfileId,
            @customerPaymentProfileId,
            @customerShippingAddressId,
            getdate(),
			 getdate(),
			@Statuscode,
			@ErrorCode,
			@ErrorText
			)
      
 SELECT 1 As TransactionResultId;                            
END IF EXISTS (SELECT TOP 1 ProfileNumber FROM BillingInformation WHERE (OrganizationId=@OrganizationId))          
BEGIN 
      Update [BillingInformation]
	  Set 
	   [CardNumber]=@CardNumber
           ,[ExpirationDate]=@ExpirationDate
           ,[AccountNumber]=@AccountNumber
           ,[RoutingNumber]=@RoutingNumber
           ,[NameOnAccount]=@NameOnAccount
           ,[BankName]=@BankName
           ,[customerProfileId]=@customerProfileId
           ,[customerPaymentProfileId]=@customerPaymentProfileId
           ,[customerShippingAddressId]=@customerShippingAddressId
           ,[UpdatedDate]=getDate()
		   ,[Statuscode]=@Statuscode
		   ,[ErrorCode]=@ErrorCode
		   ,[ErrorText]=@ErrorText

		   where @OrganizationId=OrganizationId

 SELECT 1 As TransactionResultId;  
End        
ELSE        
BEGIN        
  SELECT -1 As TransactionResultId;        
  RETURN;        
END        
                    
                              
              
                      
END

