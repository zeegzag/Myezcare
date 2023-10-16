CREATE TABLE [dbo].[ServicePlanRates] (
    [ServicePlanRateID]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [ServicePlanID]        BIGINT         NOT NULL,
    [ModuleName]           NVARCHAR (100) NOT NULL,
    [MaximumAllowedNumber] INT            NULL,
    [CreatedBy]            BIGINT         NOT NULL,
    [CreatedDate]          DATETIME       NOT NULL,
    [UpdatedDate]          DATETIME       NOT NULL,
    [UpdatedBy]            BIGINT         NOT NULL,
    [SystemID]             VARCHAR (100)  NOT NULL,
    [IsDeleted]            BIT            NOT NULL,
    [ModuleID]             INT            NOT NULL,
    CONSTRAINT [PK_ServicePlanRates] PRIMARY KEY CLUSTERED ([ServicePlanRateID] ASC),
    CONSTRAINT [FK_ServicePlanRates_ServicePlans] FOREIGN KEY ([ServicePlanID]) REFERENCES [dbo].[ServicePlans] ([ServicePlanID])
);

