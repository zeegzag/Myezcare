CREATE TABLE [dbo].[contacts_backup] (
    [ContactID]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [FirstName]     VARCHAR (50)  NULL,
    [LastName]      VARCHAR (50)  NULL,
    [Email]         VARCHAR (100) NULL,
    [Address]       VARCHAR (100) NULL,
    [City]          VARCHAR (50)  NULL,
    [State]         VARCHAR (10)  NULL,
    [ZipCode]       VARCHAR (15)  NULL,
    [Phone1]        VARCHAR (50)  NULL,
    [Phone2]        VARCHAR (50)  NULL,
    [OtherPhone]    VARCHAR (MAX) NULL,
    [LanguageID]    BIGINT        NOT NULL,
    [CreatedDate]   DATETIME      NOT NULL,
    [CreatedBy]     BIGINT        NOT NULL,
    [UpdatedDate]   DATETIME      NOT NULL,
    [UpdatedBy]     BIGINT        NOT NULL,
    [SystemID]      VARCHAR (100) NOT NULL,
    [IsDeleted]     BIT           NOT NULL,
    [Latitude]      FLOAT (53)    NULL,
    [Longitude]     FLOAT (53)    NULL,
    [TempPatientID] BIGINT        NULL
);

