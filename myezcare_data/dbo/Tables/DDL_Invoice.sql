
--CreatedBy: BALWINDER SINGH
--CreatedDate: 09-04-2020
--Description: inserting invoice data for Admin UI
CREATE TABLE [dbo].[Invoice](
	[InvoiceNumber] [bigint] IDENTITY(1000,1) NOT NULL,
	[OrganizationId] [bigint] NULL,
	[InvoiceDate] [datetime] NOT NULL,
	[PlanName] [nvarchar](50) NULL,
	[DueDate] [datetime] NULL,
	[ActivePatientQuantity] [int] NOT NULL,
	[ActivePatientUnit] [decimal](18, 3) NULL,
	[ActivePatientAmount] [decimal](18, 3) NULL,
	[NumberOfTimeSheetQuantity] [int] NULL,
	[NumberOfTimeSheetUnit] [decimal](18, 3) NULL,
	[NumberOfTimeSheetAmount] [decimal](18, 3) NULL,
	[IVRQuantity] [int] NULL,
	[IVRUnit] [decimal](18, 3) NULL,
	[IVRAmount] [decimal](18, 3) NULL,
	[MessageQuantity] [int] NULL,
	[MessageUnit] [decimal](18, 3) NULL,
	[MessageAmount] [decimal](18, 3) NULL,
	[ClaimsQuantity] [int] NULL,
	[ClaimsUnit] [decimal](18, 3) NULL,
	[ClaimsAmount] [decimal](18, 3) NULL,
	[FormsQuantity] [int] NULL,
	[FormsUnit] [decimal](18, 3) NULL,
	[FormsAmount] [decimal](18, 3) NULL,
	[InvoiceStatus] [nvarchar](50) NULL,
	[Status] [bit] NULL,
	[IsPaid] [bit] NULL,
	[PaymentDate] [datetime] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[MonthId] [int] NULL,
	[PaidAmount] [decimal](18, 3) NULL,
	[InvoiceAmount] [decimal](18, 3) NULL,
	[FilePath] [nvarchar](200) NULL,
	[OrginalFileName] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[InvoiceNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]