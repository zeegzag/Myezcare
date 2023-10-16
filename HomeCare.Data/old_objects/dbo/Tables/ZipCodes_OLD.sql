CREATE TABLE [dbo].[ZipCodes_OLD] (
    [ZipCodeID] BIGINT       IDENTITY (1, 1) NOT NULL,
    [ZipCode]   VARCHAR (15) NOT NULL,
    [City]      VARCHAR (50) NOT NULL,
    [StateCode] VARCHAR (10) NOT NULL,
    [County]    VARCHAR (30) NULL,
    [Account]   VARCHAR (5)  NULL,
    [Type]      CHAR (1)     NULL,
    CONSTRAINT [PK_ZipCodes_1] PRIMARY KEY CLUSTERED ([ZipCodeID] ASC)
);

