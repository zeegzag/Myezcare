﻿CREATE TABLE [dbo].[JO_NoteDXCodeMappings] (
    [JO_NoteDXCodeMappingID]  BIGINT         IDENTITY (1, 1) NOT NULL,
    [NoteDXCodeMappingID]     BIGINT         NOT NULL,
    [ReferralDXCodeMappingID] BIGINT         NULL,
    [ReferralID]              BIGINT         NOT NULL,
    [NoteID]                  BIGINT         NOT NULL,
    [DXCodeID]                BIGINT         NOT NULL,
    [DXCodeName]              VARCHAR (50)   NULL,
    [DxCodeType]              VARCHAR (50)   NULL,
    [Precedence]              INT            NULL,
    [StartDate]               DATE           NULL,
    [EndDate]                 DATE           NULL,
    [Description]             NVARCHAR (MAX) NULL,
    [DXCodeWithoutDot]        VARCHAR (50)   NULL,
    [DxCodeShortName]         VARCHAR (50)   NULL,
    [Action]                  CHAR (1)       NULL,
    [ActionDate]              DATETIME       NULL,
    [BatchTypeID]             BIGINT         NULL,
    [BatchID]                 BIGINT         NULL
);





