CREATE TABLE [dbo].[EmployeeContactMappings] (
    [ContactMappingID]                BIGINT        IDENTITY (1, 1) NOT NULL,
    [ContactID]                       BIGINT        NOT NULL,
    [EmployeeID]                      BIGINT        NOT NULL,
    [ClientID]                        BIGINT        NOT NULL,
    [IsEmergencyContact]              BIT           CONSTRAINT [DF_EmployeeContactMappings_IsEmergencyContact] DEFAULT ((0)) NOT NULL,
    [ContactTypeID]                   BIGINT        NULL,
    [IsPrimaryPlacementLegalGuardian] BIT           CONSTRAINT [DF_EmployeeContactMappings_IsPrimaryPlacementLegalGuardian] DEFAULT ((0)) NOT NULL,
    [IsDCSLegalGuardian]              BIT           CONSTRAINT [DF_EmployeeContactMappings_IsDCSLegalGuardian] DEFAULT ((0)) NOT NULL,
    [ROIExpireDate]                   DATE          NULL,
    [ROIType]                         VARCHAR (40)  NULL,
    [Relation]                        VARCHAR (50)  NULL,
    [IsNoticeProviderOnFile]          BIT           CONSTRAINT [DF_EmployeeContactMappings_IsNoticeProviderOnFile] DEFAULT ((0)) NOT NULL,
    [CreatedDate]                     DATETIME      NOT NULL,
    [CreatedBy]                       BIGINT        NOT NULL,
    [UpdatedDate]                     DATETIME      NOT NULL,
    [UpdatedBy]                       BIGINT        NOT NULL,
    [SystemID]                        VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_EmployeeContactMappings] PRIMARY KEY CLUSTERED ([ContactMappingID] ASC),
    CONSTRAINT [FK_EmployeeContactMappings_Contacts] FOREIGN KEY ([ContactID]) REFERENCES [dbo].[Contacts] ([ContactID]),
    CONSTRAINT [FK_EmployeeContactMappings_Employees] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID])
);

