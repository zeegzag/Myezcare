CREATE TABLE [dbo].[ZipCodes] (
    [ZipCodeID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ZipCode]   VARCHAR (15)  NOT NULL,
    [City]      VARCHAR (50)  NOT NULL,
    [StateCode] VARCHAR (10)  NOT NULL,
    [County]    VARCHAR (30)  NULL,
    [Account]   VARCHAR (5)   NULL,
    [Type]      CHAR (1)      NULL,
    [StateName] VARCHAR (100) NULL,
    CONSTRAINT [PK_ZipCodes] PRIMARY KEY CLUSTERED ([ZipCodeID] ASC)
);

