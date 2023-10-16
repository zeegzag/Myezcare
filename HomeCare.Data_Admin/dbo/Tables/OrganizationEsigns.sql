CREATE TABLE [dbo].[OrganizationEsigns] (
    [OrganizationEsignID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [OrganizationID]      BIGINT         NOT NULL,
    [EsignTerms]          NVARCHAR (MAX) NOT NULL,
    [EsignSentDate]       DATETIME       NULL,
    [EsignName]           NVARCHAR (100) NULL,
    [EsignAcceptedDate]   DATETIME       NULL,
    [CreatedBy]           BIGINT         NOT NULL,
    [CreatedDate]         DATETIME       NOT NULL,
    [UpdatedDate]         DATETIME       NOT NULL,
    [UpdatedBy]           BIGINT         NOT NULL,
    [SystemID]            VARCHAR (100)  NOT NULL,
    [IsDeleted]           BIT            NOT NULL,
    [IsCompleted]         BIT            NOT NULL,
    [IsInProcess]         BIT            NOT NULL,
    [OrganizationTypeID]  BIGINT         NULL,
    CONSTRAINT [PK_OrganizationEsigns] PRIMARY KEY CLUSTERED ([OrganizationEsignID] ASC),
    CONSTRAINT [FK_OrganizationEsigns_OrganizationTypes] FOREIGN KEY ([OrganizationTypeID]) REFERENCES [dbo].[OrganizationTypes] ([OrganizationTypeID])
);

