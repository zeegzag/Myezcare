GO

CREATE TABLE [dbo].[Medication](
	[MedicationId] [bigint] IDENTITY(2000,1) NOT NULL,
	[MedicationName] [nvarchar](500) NULL,
	[Generic_Name] [nvarchar](500) NULL,
	[Brand_Name] [nvarchar](500) NULL,
	[Product_Type] [nvarchar](500) NULL,
	[Route] [nvarchar](500) NULL,
	[AddedDate] [datetime] NULL,
	[IsActive] [bit] NULL,
	[UpdatedDate] [datetime] NULL,
	[Dosage_Form] [nvarchar](100) NULL,
 CONSTRAINT [PK_Medication] PRIMARY KEY CLUSTERED 
(
	[MedicationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO