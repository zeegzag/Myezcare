CREATE TABLE [dbo].[States] (
    [StateCode] VARCHAR (10) NOT NULL,
    [StateName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_States] PRIMARY KEY CLUSTERED ([StateCode] ASC)
);

