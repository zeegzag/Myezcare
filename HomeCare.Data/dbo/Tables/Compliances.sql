CREATE TABLE [dbo].[Compliances] (
    [ComplianceID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserType] [int] NULL,
	[DocumentationType] [int] NULL,
	[DocumentName] [nvarchar](max) NULL,
	[IsTimeBased] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
	[UpdatedDate] [datetime] NULL,
	[SystemID] [nvarchar](max) NULL,
	[EBFormID] [nvarchar](max) NULL,
	[ParentID] [bigint] NULL,
	[Type] [nvarchar](50) NULL,
	[Value] [nvarchar](50) NULL,
	[SortingID] [bigint] NULL,
	[ShowToAll] [bit] NULL,
	[EmployeeID] [int] NOT NULL,
	[ReferralID] [int] NULL,
    [HideIfEmpty]         BIT            CONSTRAINT [DF_Compliances_HideIfEmpty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Compliances] PRIMARY KEY CLUSTERED ([ComplianceID] ASC)
);

