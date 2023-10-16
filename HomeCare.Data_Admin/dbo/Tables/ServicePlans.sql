CREATE TABLE [dbo].[ServicePlans] (
    [ServicePlanID]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [ServicePlanName]        NVARCHAR (100) NOT NULL,
    [PerPatientPrice]        FLOAT (53)     NOT NULL,
    [NumberOfDaysForBilling] INT            NOT NULL,
    [CreatedBy]              BIGINT         NOT NULL,
    [CreatedDate]            DATETIME       NOT NULL,
    [UpdatedDate]            DATETIME       NOT NULL,
    [UpdatedBy]              BIGINT         NOT NULL,
    [SystemID]               VARCHAR (100)  NOT NULL,
    [IsDeleted]              BIT            NOT NULL,
    [SetupFees]              FLOAT (53)     NULL,
    CONSTRAINT [PK_ServicePlans] PRIMARY KEY CLUSTERED ([ServicePlanID] ASC)
);

