CREATE TABLE [dbo].[UserTokens] (
    [UserTokenId]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [EmployeeID]    BIGINT         NOT NULL,
    [ExpireLogin]   DATETIME       NOT NULL,
    [Token]         NVARCHAR (100) NOT NULL,
    [IsMobileToken] BIT            CONSTRAINT [DF_UserTokens_IsMobileToken] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_UserTokens] PRIMARY KEY CLUSTERED ([UserTokenId] ASC)
);

