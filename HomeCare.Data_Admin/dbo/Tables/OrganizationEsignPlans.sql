CREATE TABLE [dbo].[OrganizationEsignPlans] (
    [OrganizationEsignPlanID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [OrganizationEsignID]     BIGINT        NOT NULL,
    [OrganizationID]          BIGINT        NOT NULL,
    [ServicePlanID]           BIGINT        NOT NULL,
    [IsSelectedByClient]      BIT           NOT NULL,
    [ClientSelecetdDate]      DATETIME      NULL,
    [PlanStartDate]           DATETIME      NULL,
    [PlanEndDate]             DATETIME      NULL,
    [PerPatientPrice]         FLOAT (53)    NULL,
    [NumberOfDaysForBilling]  INT           NULL,
    [NextDueDate]             DATETIME      NULL,
    [CreatedBy]               BIGINT        NOT NULL,
    [CreatedDate]             DATETIME      NOT NULL,
    [UpdatedDate]             DATETIME      NOT NULL,
    [UpdatedBy]               BIGINT        NOT NULL,
    [SystemID]                VARCHAR (100) NOT NULL,
    [IsDeleted]               BIT           NOT NULL,
    CONSTRAINT [PK_OrganizationEsignPlans] PRIMARY KEY CLUSTERED ([OrganizationEsignPlanID] ASC),
    CONSTRAINT [FK_OrganizationEsignPlans_OrganizationEsigns] FOREIGN KEY ([OrganizationEsignID]) REFERENCES [dbo].[OrganizationEsigns] ([OrganizationEsignID]),
    CONSTRAINT [FK_OrganizationEsignPlans_ServicePlans] FOREIGN KEY ([ServicePlanID]) REFERENCES [dbo].[ServicePlans] ([ServicePlanID])
);

