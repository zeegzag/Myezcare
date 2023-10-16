
/*
Created by : Neeraj Sharma
Created Date: 31 July 2020
Updated by :
Updated Date :

Purpose: This table is used to keep the billing detail before creating profile on authorized.net

*/


CREATE TABLE [dbo].[BillingInformation](
	[ProfileNumber] [bigint] IDENTITY(1,1) NOT NULL,
	[OrganizationId] [bigint] NULL,
	[CardNumber] [varchar](100) NULL,
	[ExpirationDate] [datetime] NULL,
	[AccountNumber] [varchar](100) NULL,
	[RoutingNumber] [varchar](100) NULL,
	[NameOnAccount] [varchar](100) NULL,
	[BankName] [varchar](100) NULL,
	[customerProfileId] [bigint] NULL,
	[customerPaymentProfileId] [bigint] NULL,
	[customerShippingAddressId] [bigint] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[Statuscode] [nvarchar](MAX) NULL,
	[ErrorCode] [nvarchar](MAX) NULL,
	[ErrorText] [nvarchar](MAX) NULL,
 CONSTRAINT [PK_BillingInformation] PRIMARY KEY CLUSTERED 
(
	[ProfileNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


