CREATE TABLE [dbo].[OrganizationPlanRates] (
    [OrganizationPlanRateID]  BIGINT         IDENTITY (1, 1) NOT NULL,
    [OrganizationEsignPlanID] BIGINT         NOT NULL,
    [ServicePlanRateID]       BIGINT         NOT NULL,
    [ServicePlanID]           BIGINT         NOT NULL,
    [ModuleName]              NVARCHAR (100) NULL,
    [MaximumAllowedNumber]    INT            NULL,
    [CreatedBy]               BIGINT         NOT NULL,
    [CreatedDate]             DATETIME       NOT NULL,
    [UpdatedDate]             DATETIME       NOT NULL,
    [UpdatedBy]               BIGINT         NOT NULL,
    [SystemID]                VARCHAR (100)  NOT NULL,
    [IsDeleted]               BIT            NOT NULL,
    CONSTRAINT [PK_OrganizationPlanRates] PRIMARY KEY CLUSTERED ([OrganizationPlanRateID] ASC),
    CONSTRAINT [FK_OrganizationPlanRates_OrganizationEsignPlans] FOREIGN KEY ([OrganizationEsignPlanID]) REFERENCES [dbo].[OrganizationEsignPlans] ([OrganizationEsignPlanID]),
    CONSTRAINT [FK_OrganizationPlanRates_ServicePlanRates] FOREIGN KEY ([ServicePlanRateID]) REFERENCES [dbo].[ServicePlanRates] ([ServicePlanRateID]),
    CONSTRAINT [FK_OrganizationPlanRates_ServicePlans] FOREIGN KEY ([ServicePlanID]) REFERENCES [dbo].[ServicePlans] ([ServicePlanID])
);

