CREATE TABLE [dbo].[ZipCodes] (
    [ZipCodeID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [ZipCode]   VARCHAR (15)   NOT NULL,
    [City]      VARCHAR (50)   NOT NULL,
    [StateCode] NVARCHAR (MAX) NOT NULL,
    [County]    VARCHAR (30)   NULL,
    [Account]   VARCHAR (5)    NULL,
    [Type]      NVARCHAR (MAX) NULL,
    [StateName] VARCHAR (100)  NULL,
    CONSTRAINT [PK_ZipCodes] PRIMARY KEY CLUSTERED ([ZipCodeID] ASC)
);



