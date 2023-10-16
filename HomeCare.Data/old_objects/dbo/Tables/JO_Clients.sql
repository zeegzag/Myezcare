CREATE TABLE [dbo].[JO_Clients] (
    [_Log_ClientID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ClientID]      BIGINT        NOT NULL,
    [FirstName]     VARCHAR (50)  NOT NULL,
    [MiddleName]    VARCHAR (50)  NULL,
    [LastName]      VARCHAR (50)  NOT NULL,
    [Dob]           DATE          NOT NULL,
    [Gender]        CHAR (1)      NOT NULL,
    [ClientNumber]  VARCHAR (10)  NULL,
    [AHCCCSID]      VARCHAR (10)  NOT NULL,
    [CISNumber]     VARCHAR (10)  NOT NULL,
    [CreatedDate]   DATETIME      NOT NULL,
    [CreatedBy]     BIGINT        NOT NULL,
    [UpdatedDate]   DATETIME      NOT NULL,
    [UpdatedBy]     BIGINT        NOT NULL,
    [SystemID]      VARCHAR (100) NOT NULL,
    [IsDeleted]     BIT           NOT NULL,
    [Action]        CHAR (1)      NOT NULL,
    [ActionDate]    DATETIME      NOT NULL,
    CONSTRAINT [PK__LOG_Clients] PRIMARY KEY CLUSTERED ([_Log_ClientID] ASC)
);

