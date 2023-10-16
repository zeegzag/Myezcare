CREATE TABLE [dbo].[ReferralBillingAuthorizations] (
    [ReferralBillingAuthorizationID]  BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralID]                      BIGINT        NOT NULL,
    [Type]                            NVARCHAR (20) NULL,
    [AuthorizationCode]               NVARCHAR (20) NULL,
    [StartDate]                       DATE          NULL,
    [EndDate]                         DATE          NULL,
    [IsDeleted]                       BIT           CONSTRAINT [DF_ReferralBillingAuthrizations_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]                     DATETIME      NULL,
    [CreatedBy]                       BIGINT        NULL,
    [UpdatedDate]                     DATETIME      NULL,
    [UpdatedBy]                       BIGINT        NULL,
    [SystemID]                        VARCHAR (100) NULL,
    [PayorID]                         BIGINT        NULL,
    [AllowedTime]                     BIGINT        NULL,
    [PriorAuthorizationFrequencyType] BIGINT        NULL,
    [AllowedTimeType] VARCHAR(100) NULL, 
	[AttachmentFileName] NVARCHAR(MAX) NULL,
	[AttachmentFilePath] NVARCHAR(MAX) NULL,
    CONSTRAINT [PK_ReferralBillingAuthrizations] PRIMARY KEY CLUSTERED ([ReferralBillingAuthorizationID] ASC)
);

