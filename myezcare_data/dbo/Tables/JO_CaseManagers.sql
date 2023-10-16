CREATE TABLE [dbo].[JO_CaseManagers] (
    [JO_CaseManagerID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [CaseManagerID]    BIGINT        NOT NULL,
    [FirstName]        VARCHAR (50)  NOT NULL,
    [LastName]         VARCHAR (50)  NOT NULL,
    [Phone]            VARCHAR (15)  NULL,
    [Extension]        VARCHAR (10)  NULL,
    [Fax]              VARCHAR (15)  NULL,
    [Cell]             VARCHAR (15)  NULL,
    [Email]            VARCHAR (50)  NULL,
    [Notes]            VARCHAR (500) NULL,
    [AgencyID]         BIGINT        NOT NULL,
    [AgencyLocationID] BIGINT        NOT NULL,
    [IsDeleted]        BIT           NOT NULL,
    [CreatedDate]      DATETIME      NOT NULL,
    [CreatedBy]        BIGINT        NOT NULL,
    [UpdatedDate]      DATETIME      NOT NULL,
    [UpdatedBy]        BIGINT        NOT NULL,
    [SystemID]         VARCHAR (100) NOT NULL,
    [Action]           CHAR (1)      NOT NULL,
    [ActionDate]       DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_CaseManagers] PRIMARY KEY CLUSTERED ([JO_CaseManagerID] ASC)
);

