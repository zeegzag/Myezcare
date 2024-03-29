--CreatedBy:Akhilesh
--CreatedDate:22 march 2020
--Description:Create SaveDeviationNote table
CREATE TABLE [dbo].[SaveDeviationNote](
	[DeviationNoteID] [int] IDENTITY(1,1) NOT NULL,
	[DeviationID] [int] NULL,
	[EmployeeID] [int] NULL,
	[DeviationNotes] [nvarchar](500) NULL,
	[DeviationType] [nvarchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
	[UpdatedDate] [datetime] NULL,
	[EmployeeVisitID] [bigint] NULL,
 CONSTRAINT [PK_SaveDeviationNote] PRIMARY KEY CLUSTERED 
(
	[DeviationNoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
