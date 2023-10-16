CREATE TABLE [dbo].[JO_AgencyTypes] (
    [JO_AgencyTypeID] BIGINT       IDENTITY (1, 1) NOT NULL,
    [AgencyTypeID]    BIGINT       NOT NULL,
    [AgencyTypeName]  VARCHAR (50) NOT NULL,
    [Action]          CHAR (1)     NOT NULL,
    [ActionDate]      DATETIME     NOT NULL,
    CONSTRAINT [PK_JO_AgencyTypes] PRIMARY KEY CLUSTERED ([JO_AgencyTypeID] ASC)
);

