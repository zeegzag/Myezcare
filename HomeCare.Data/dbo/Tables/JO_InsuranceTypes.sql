CREATE TABLE [dbo].[JO_InsuranceTypes] (
    [JO_InsuranceTypeID] BIGINT       IDENTITY (1, 1) NOT NULL,
    [InsuranceTypeID]    BIGINT       NOT NULL,
    [InsuranceTypeName]  VARCHAR (50) NOT NULL,
    [Action]             CHAR (1)     NOT NULL,
    [ActionDate]         DATETIME     NOT NULL,
    CONSTRAINT [PK_JO_InsuranceTypes] PRIMARY KEY CLUSTERED ([JO_InsuranceTypeID] ASC)
);

