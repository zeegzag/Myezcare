CREATE TABLE [dbo].[JO_AgencyLocations] (
    [JO_AgencyLocationID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [AgencyLocationID]    BIGINT        NOT NULL,
    [LocationName]        VARCHAR (50)  NOT NULL,
    [StreetAddress1]      VARCHAR (100) NULL,
    [StreetAddress2]      VARCHAR (100) NULL,
    [City]                VARCHAR (50)  NULL,
    [StateCode]           VARCHAR (10)  NULL,
    [ZipCode]             VARCHAR (15)  NULL,
    [AgencyID]            BIGINT        NOT NULL,
    [Action]              CHAR (1)      NOT NULL,
    [ActionDate]          DATETIME      NOT NULL,
    [IsDeleted]           BIT           CONSTRAINT [DF_JO_AgencyLocations_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_JO_AgencyLocations] PRIMARY KEY CLUSTERED ([JO_AgencyLocationID] ASC)
);

