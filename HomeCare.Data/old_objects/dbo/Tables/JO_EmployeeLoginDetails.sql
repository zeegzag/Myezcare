CREATE TABLE [dbo].[JO_EmployeeLoginDetails] (
    [JO_EmployeeLoginDetailID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [EmployeeLoginDetailID]    BIGINT        NOT NULL,
    [EmployeeID]               BIGINT        NOT NULL,
    [LoginTime]                DATETIME      NOT NULL,
    [CreatedDate]              DATETIME      NOT NULL,
    [CreatedBy]                BIGINT        NOT NULL,
    [UpdatedDate]              DATETIME      NOT NULL,
    [UpdatedBy]                BIGINT        NOT NULL,
    [SystemID]                 VARCHAR (100) NULL,
    [Action]                   CHAR (1)      NOT NULL,
    [ActionDate]               DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_EmployeeLoginDetails] PRIMARY KEY CLUSTERED ([JO_EmployeeLoginDetailID] ASC)
);

