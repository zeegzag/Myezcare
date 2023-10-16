CREATE TABLE [dbo].[BillingGroups] (
    [BillingGroupID]   BIGINT       NOT NULL,
    [BillingGroupName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_BillingGroups] PRIMARY KEY CLUSTERED ([BillingGroupID] ASC)
);

