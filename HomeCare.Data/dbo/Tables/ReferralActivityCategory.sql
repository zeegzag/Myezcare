﻿CREATE TABLE [dbo].[ReferralActivityCategory](
	[ReferralActivityCategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Category] [varchar](200) NULL,
	[Name] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[ReferralActivityCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO