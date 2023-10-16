CREATE TABLE [dbo].[JO_DXCodes] (
    [JO_DXCodeID]      BIGINT        IDENTITY (1, 1) NOT NULL,
    [DXCodeID]         BIGINT        NOT NULL,
    [DXCodeName]       VARCHAR (100) NOT NULL,
    [DXCodeWithoutDot] VARCHAR (50)  NULL,
    [DxCodeType]       VARCHAR (50)  NULL,
    [Description]      VARCHAR (500) NULL,
    [IsDeleted]        BIT           NOT NULL,
    [EffectiveFrom]    DATE          NULL,
    [EffectiveTo]      DATE          NULL,
    [CreatedDate]      DATETIME      NOT NULL,
    [CreatedBy]        BIGINT        NOT NULL,
    [UpdatedDate]      DATE          NOT NULL,
    [UpdatedBy]        BIGINT        NOT NULL,
    [SystemID]         VARCHAR (100) NOT NULL,
    [Action]           CHAR (1)      NOT NULL,
    [ActionDate]       DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_DXCodes] PRIMARY KEY CLUSTERED ([JO_DXCodeID] ASC)
);

