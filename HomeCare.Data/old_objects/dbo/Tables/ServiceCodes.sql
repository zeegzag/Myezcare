CREATE TABLE [dbo].[ServiceCodes] (
    [ServiceCodeID]                BIGINT        IDENTITY (1, 1) NOT NULL,
    [ServiceCode]                  VARCHAR (50)  NOT NULL,
    [ModifierID]                   VARCHAR (200) NULL,
    [ServiceName]                  VARCHAR (100) NOT NULL,
    [Description]                  VARCHAR (500) NULL,
    [ServiceCodeType]              INT           NULL,
    [UnitType]                     INT           NULL,
    [MaxUnit]                      INT           NULL,
    [DailyUnitLimit]               INT           NULL,
    [PerUnitQuantity]              DECIMAL (18)  NULL,
    [IsBillable]                   BIT           NOT NULL,
    [HasGroupOption]               BIT           CONSTRAINT [DF_ServiceCodes_HasGroupOption] DEFAULT ((0)) NULL,
    [ServiceCodeStartDate]         DATE          NULL,
    [ServiceCodeEndDate]           DATE          NULL,
    [IsDeleted]                    BIT           CONSTRAINT [DF_ServiceCodes_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CheckRespiteHours]            BIT           CONSTRAINT [DF_ServiceCodes_CheckRespiteHours] DEFAULT ((0)) NULL,
    [RandomGroupID]                VARCHAR (100) NULL,
    [DefaultUnitIgnoreCalculation] INT           CONSTRAINT [DF__ServiceCo__Defau__2EC5E7B8] DEFAULT ((0)) NULL,
    [CareType]                     BIGINT        NULL,
    [RoundUpUnit]                  INT           NULL,
    CONSTRAINT [PK_ServiceCodes] PRIMARY KEY CLUSTERED ([ServiceCodeID] ASC)
);

