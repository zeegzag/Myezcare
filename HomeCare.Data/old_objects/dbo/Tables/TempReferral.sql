CREATE TABLE [dbo].[TempReferral] (
    [Id]           BIGINT        NOT NULL,
    [FirstName]    VARCHAR (100) NOT NULL,
    [LastName]     VARCHAR (100) NOT NULL,
    [ErrorMessage] VARCHAR (MAX) NULL,
    [CreatedBy]    BIGINT        NOT NULL,
    [IsShow]       BIT           CONSTRAINT [DF__TempRefer__IsSho__4B380934] DEFAULT ((0)) NULL
);

