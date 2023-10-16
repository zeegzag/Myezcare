CREATE TABLE [dbo].[JO_Payors] (
    [JO_PayorID]                BIGINT        IDENTITY (1, 1) NOT NULL,
    [PayorID]                   BIGINT        NOT NULL,
    [PayorName]                 VARCHAR (100) NOT NULL,
    [ShortName]                 VARCHAR (50)  NOT NULL,
    [PayorSubmissionName]       VARCHAR (100) NULL,
    [PayorIdentificationNumber] VARCHAR (50)  NULL,
    [Address]                   VARCHAR (MAX) NOT NULL,
    [City]                      VARCHAR (50)  NOT NULL,
    [StateCode]                 VARCHAR (10)  NOT NULL,
    [ZipCode]                   VARCHAR (15)  NOT NULL,
    [PayorTypeID]               BIGINT        NOT NULL,
    [IsDeleted]                 BIT           CONSTRAINT [DF_JO_Payors_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]               DATETIME      NOT NULL,
    [CreatedBy]                 BIGINT        NOT NULL,
    [UpdatedDate]               DATETIME      NOT NULL,
    [UpdatedBy]                 BIGINT        NOT NULL,
    [SystemID]                  VARCHAR (100) NOT NULL,
    [Action]                    CHAR (1)      NOT NULL,
    [ActionDate]                DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_Payors] PRIMARY KEY CLUSTERED ([JO_PayorID] ASC)
);

