﻿CREATE TABLE [dbo].[CaptureCall] (
    [Id]                 BIGINT         IDENTITY (1, 1) NOT NULL,
    [FirstName]          NVARCHAR (50)  NULL,
    [LastName]           NVARCHAR (50)  NULL,
    [Contact]            NVARCHAR (15)  NULL,
    [Email]              NVARCHAR (100) NULL,
    [Flag]               INT            NULL,
    [Address]            NVARCHAR (100) NULL,
    [City]               NVARCHAR (50)  NULL,
    [StateCode]          NVARCHAR (50)  NULL,
    [ZipCode]            NVARCHAR (15)  NULL,
    [RoleIds]            NVARCHAR (MAX) NULL,
    [IsDeleted]          BIT            NULL,
    [CreatedDate]        DATETIME       NULL,
    [CreatedBy]          BIGINT         NULL,
    [UpdatedDate]        DATETIME       NULL,
    [UpdatedBy]          BIGINT         NULL,
    [Notes]              NVARCHAR (MAX) NULL,
    [EmployeesIDs]       NVARCHAR (255) NULL,
    [CallType]           NVARCHAR (255) NULL,
    [RelatedWithPatient] BIGINT         NULL,
    [InquiryDate]        DATETIME       NULL,
    [FileName]           NVARCHAR (MAX) NULL,
    [FilePath]           NVARCHAR (MAX) NULL,
    [OrbeonID]           NVARCHAR (MAX) NULL,
    [Status]             NVARCHAR (MAX) NULL,
    [GroupIDs] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_CaptureCallDetails] PRIMARY KEY CLUSTERED ([Id] ASC)
);

