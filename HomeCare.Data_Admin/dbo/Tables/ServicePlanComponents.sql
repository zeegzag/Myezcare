CREATE TABLE [dbo].[ServicePlanComponents] (
    [ServicePlanComponentID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ServicePlanID]          BIGINT        NOT NULL,
    [DDMasterID]             BIGINT        NOT NULL,
    [CreatedBy]              BIGINT        NOT NULL,
    [CreatedDate]            DATETIME      NOT NULL,
    [UpdatedDate]            DATETIME      NOT NULL,
    [UpdatedBy]              BIGINT        NOT NULL,
    [SystemID]               VARCHAR (100) NOT NULL,
    [IsDeleted]              BIT           NOT NULL,
    CONSTRAINT [PK_ServicePlanComponents] PRIMARY KEY CLUSTERED ([ServicePlanComponentID] ASC),
    CONSTRAINT [FK_ServicePlanComponents_DDMaster] FOREIGN KEY ([DDMasterID]) REFERENCES [dbo].[DDMaster] ([DDMasterID])
);

