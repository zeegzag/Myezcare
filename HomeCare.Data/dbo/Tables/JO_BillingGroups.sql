CREATE TABLE [dbo].[JO_BillingGroups] (
    [JO_BillingGroupID] BIGINT       NOT NULL,
    [BillingGroupID]    BIGINT       NOT NULL,
    [BillingGroup]      VARCHAR (50) NOT NULL,
    [Action]            CHAR (1)     NOT NULL,
    [ActionDate]        DATETIME     NOT NULL
);

