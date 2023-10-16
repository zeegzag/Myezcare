CREATE TABLE [dbo].[JO_PayorTypes] (
    [JO_PayorTypeID] BIGINT       IDENTITY (1, 1) NOT NULL,
    [PayorTypeID]    BIGINT       NOT NULL,
    [PayorTypeName]  VARCHAR (50) NOT NULL,
    [Action]         CHAR (1)     NOT NULL,
    [ActionDate]     DATETIME     NOT NULL,
    CONSTRAINT [PK_JO_PayorTypes] PRIMARY KEY CLUSTERED ([JO_PayorTypeID] ASC)
);

