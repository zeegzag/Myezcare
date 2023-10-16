CREATE TABLE [dbo].[UserOtps] (
    [UserOtpId]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [EmployeeID]  BIGINT         NOT NULL,
    [OTP]         NVARCHAR (50)  NOT NULL,
    [SentDate]    DATETIME       NOT NULL,
    [IsSMSSent]   BIT            CONSTRAINT [DF_UserOtps_IsUsed] DEFAULT ((0)) NOT NULL,
    [SMSResponse] NVARCHAR (MAX) NULL,
    [Type]        NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_UserOtps] PRIMARY KEY CLUSTERED ([UserOtpId] ASC)
);

