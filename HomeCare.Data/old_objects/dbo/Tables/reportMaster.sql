--CreatedBy: Vishwas kumar
--CreatedDate: 04/3/2020
--Description:For create ReportMaster table


CREATE TABLE [dbo].[ReportMaster](
	[ReportID] [int] IDENTITY(1,1) NOT NULL,
	[ReportName] [nvarchar](max) NULL,
	[SqlString] [nvarchar](max) NULL,
	[ReportDescription] [nvarchar](max) NULL,
	[DataSet] [nvarchar](max) NULL,
	[RDL_FileName] [nvarchar](max) NULL,
 CONSTRAINT [PK_ReportMaster] PRIMARY KEY CLUSTERED 
(
	[ReportID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ReportMaster] ON 

INSERT [dbo].[ReportMaster] ([ReportID], [ReportName], [SqlString], [ReportDescription], [DataSet], [RDL_FileName]) VALUES (1, N'ActivePatient', N'SELECT VW_ActivePatient.* FROM VW_ActivePatient', N'A report is a document that presents information in an organized format for a specific audience and purpose. Although summaries of reports may be delivered orally, complete reports are almost always in the form of written documents.', N'ActivePatientRpt', N'ActivePatientReport')
INSERT [dbo].[ReportMaster] ([ReportID], [ReportName], [SqlString], [ReportDescription], [DataSet], [RDL_FileName]) VALUES (2, N'EmployeeTimeSheetReoprt', N'SELECT VW_CompleteTimeSheet.*FROM VW_CompleteTimeSheet', N'A report is a document that presents information in an organized format for a specific audience and purpose. Although summaries of reports may be delivered orally, complete reports are almost always in the form of written documents.', N'EmpCompleteTimeSheetRpt', N'EmployeeTimeSheetReoprt')
INSERT [dbo].[ReportMaster] ([ReportID], [ReportName], [SqlString], [ReportDescription], [DataSet], [RDL_FileName]) VALUES (3, N'PatientVisitReport', N'SELECT VW_PatientVisit.* FROM VW_PatientVisit', N'A report is a document that presents information in an organized format for a specific audience and purpose. Although summaries of reports may be delivered orally, complete reports are almost always in the form of written documents.', N'PatientVisitRpt', N'PatientVisitReport')
INSERT [dbo].[ReportMaster] ([ReportID], [ReportName], [SqlString], [ReportDescription], [DataSet], [RDL_FileName]) VALUES (4, N'EmployeeVisitReport', N'SELECT VW_EmployeeVisit.* FROM VW_EmployeeVisit', N'A report is a document that presents information in an organized format for a specific audience and purpose. Although summaries of reports may be delivered orally, complete reports are almost always in the form of written documents.', N'EmployeeVisitRpt', N'EmployeeVisitReport')
INSERT [dbo].[ReportMaster] ([ReportID], [ReportName], [SqlString], [ReportDescription], [DataSet], [RDL_FileName]) VALUES (5, N'Patient HourSummaryReport', N'SELECT VW_PatientHourSummary.* FROM VW_PatientHourSummary', N'A report is a document that presents information in an organized format for a specific audience and purpose. Although summaries of reports may be delivered orally, complete reports are almost always in the form of written documents.', N'PatientHoursSummaryRpt', N'PatientHourSummaryReport')
SET IDENTITY_INSERT [dbo].[ReportMaster] OFF
