USE [Kundan_Homecare]
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (1, N'Masters', N'All master pages access', 0, 2, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2, N'Case Manager', N'Case manager access', 1, 22, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3, N'Department', N'Department access', 1, 23, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (4, N'Employee', N'Employee access', 1, 25, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (5, N'Facility House', N'Facility house access', 1, 26, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (6, N'Transportation', N'Transportation access', 1, 30, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (7, N'Add/Update', N'Case manager add/update access', 2, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (8, N'Add/Update', N'Department add/update access', 3, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (9, N'Add/Update', N'Employee add/update access', 4, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (10, N'Add/Update', N'Facility house add/update access', 5, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (11, N'Add/Update', N'Transportation add/update access', 6, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (12, N'List', N'Case manager listing access', 2, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (13, N'List', N'Department listing access', 3, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (14, N'List', N'Employee listing access', 4, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (15, N'List', N'Facility house listing access', 5, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (16, N'List', N'Transportation listing access', 6, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (17, N'Referral & Intake / Client Management', N'Referral and intake / client management pages access', 0, 3, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (18, N'Add / Update', N'Referral and intake/client management add/update access', 17, 41, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (19, N'List', N'Referral and intake/client management list access', 17, 42, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (20, N'Dashboard', N'Dashboard page access', 0, 1, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (21, N'Referral Details', N'Referral detail (clinet information / contact information / compliance information / referral history) access', 18, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (22, N'Referral Documents', N'Referral documents access', 18, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (23, N'Referral Checklist', N'Referral checklist access', 18, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (24, N'Referral Spar Form', N'Referral spar form access', 18, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (25, N'Internal Messaging', N'Internal messaging access', 18, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (26, N'Schedule Hisotry', N'Schedule hisotry access', 18, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (27, N'Scheduling', N'Scheduling pages access', 0, 4, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (28, N'Schedule Master', N'Schedule master page access', 27, 52, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (29, N'Assignment', N'Assignment page access', 27, 51, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (30, N'Transportation Groups', N'Transportation groups page access', 27, 53, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (31, N'List', N'Schedule master listing access', 28, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (32, N'Update', N'Schedule master update access', 28, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (33, N'View All', N'All referral access', 19, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (34, N'View Assinged', N'Assigned referral access', 19, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (35, N'Payor', N'Payor access', 1, 28, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (36, N'Add/Update', N'Payor add/update access', 35, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (37, N'List', N'Payor listing access', 35, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (38, N'Payor & Service Code', N'Payor and service code page access', 35, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (39, N'Attendance', N'Attendance master page access', 27, 54, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (40, N'DxCode', N'DxCode access', 1, 24, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (41, N'Add/Update', N'DxCode add/update access', 40, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (42, N'List', N'DxCode listing access', 40, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (43, N'Template Details', N'Email template access', 1, 29, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (44, N'Add/Update', N'Email template add/update access', 43, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (45, N'List', N'Email template listing access', 43, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (46, N'Billing & Claim Processing', N'Permission to create and update batch, generate EDI 837 file and process EDI 835 file', 0, 6, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (47, N'Note Generation', N'Permission to create billable/non-billable note', 0, 5, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (48, N'Reports', N'Permission to generate reports', 0, 7, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (49, N'Agency', N'Agency page access', 1, 21, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (50, N'Add/Update', N'Agency add/update access', 49, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (51, N'List', N'Agency lisitng access', 49, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (52, N'Referral Tracking', N'Referral tracking access', 17, 44, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (53, N'Client Status Report', N'Client status report access', 48, 84, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (54, N'Referral Detail Report', N'Referral detail report access', 48, 91, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (55, N'Client Information Report', N'Client information report access', 48, 83, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (56, N'Internal Service Plan Expiration Dates Report', N'Internal service plan expiration dates report access', 48, 89, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (57, N'Respite Usage Report', N'Respite usage report access', 48, 93, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (58, N'Attendance Report', N'Attendance report access', 48, 81, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (59, N'Behaviour Contract Report', N'Behaviour contract report access', 48, 82, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (60, N'Encounter Print Report', N'Encounter Print report access', 48, 87, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (61, N'DSP Roster Report', N'DSP roster report access', 48, 85, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (62, N'Schedule Attendance Report', N'Schedule attendance report access', 48, 94, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (63, N'LS Outcome Measurements Report', N'LS Outcome Measurements Report', 48, 90, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (64, N'LSTM Caseloads', N'LS Team Member Caseloads', 17, 43, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (65, N'General Notice', N'General Notice', 48, 88, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (66, N'Review & Measurement', N'Review & Measurement', 18, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (67, N'Snapshot Print Report', N'Snapshot Print Report', 48, 95, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (68, N'DTR Print Report', N'DTR Print Report', 48, 86, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (69, N'Required Documents For Attendance', N'Required Documents For Attendance', 48, 92, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (70, N'View All', N'All caseloads access', 64, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (71, N'View Assinged', N'Assinged caseloads access', 64, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (72, N'Note Sentence', N'Note sentence page access', 1, 27, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (73, N'Add/Update', N'Note sentence add/update access', 72, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (74, N'List', N'Note sentence listing access', 72, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (75, N'View Only', N'Only view only permission for Referral Details pages', 21, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (76, N'Add/Update', N'Add / update permission for Referral Details pages', 21, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (77, N'View Only', N'Only view only permission for Referral Documents pages', 22, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (78, N'Add/Update', N'Add / update permission for Referral Documents pages', 22, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (79, N'View Only', N'Only view only permission for Referral Checklist', 23, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (80, N'Add/Update', N'Add / update permission for Referral Checklist', 23, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (81, N'View Only', N'Only view only permission for Referral Spar Form', 24, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (82, N'Add/Update', N'Add / update permission for Referral Spar Form', 24, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (83, N'View Only', N'Only view only permission for Internal Messaging', 25, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (84, N'Add/Update', N'Add / update permission for Internal Messaging', 25, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (85, N'Outcomes Measurement', N'Outcomes Measurement', 66, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (86, N'View Only', N'Only view only permission for Outcomes Measurement page', 85, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (87, N'Add/Update', N'Add / update permission for Outcomes Measurement page', 85, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (88, N'Ansell Casey Review', N'AnsellCaseyReview', 66, 16, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (89, N'View All', N'View all permission for Ansell Casey Review page', 88, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (90, N'View Assigned', N'Only view assigned permission for Ansell Casey Review page', 88, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (91, N'Add/Update', N'Add / update permission for Ansell Casey Review page', 88, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (92, N'Monthly Summary', N'MonthlySummary', 66, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (93, N'View Only', N'Only view only permission for Monthly Summary page', 92, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (94, N'Add/Update', N'Add / update permission for Monthly Summary page', 92, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (96, N'Add Snapshot', N'Permissions to add Group Monthly Summary / Group Snapshot', 47, 65, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (97, N'Snapshot Review', N'Permissions to Review Snapshot details page', 47, 66, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (98, N'Assigned IM Notifications', N'Permissions to access/view Assigned IM Notifications', 20, 11, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (99, N'Resolved / Assigned to IM Notifications', N'Permissions to access/view Resolved / Assigned to IM Notifications', 20, 12, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (100, N'Missing / Incomplete CheckList and SPAR', N'Permissions to access/view Missing / Incomplete CheckList and SPAR', 20, 13, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (101, N'Missing / Expired Documents', N'Permissions to access/view Missing / Expired Documents', 20, 14, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (102, N'Missing Internal Document List', N'Permissions to access/view Missing Internal Document List', 20, 15, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (103, N'Batch 837', N'Permissions to access/view Batch 837 page', 46, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (104, N'Upload 835', N'Permissions to access/view Upload 835 page', 46, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (105, N'Reconcile 835 / EOB', N'Permissions to access/view Reconcile 835 / EOB page', 46, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (106, N'EDI File Logs', N'Permissions to access/view EDI File Logs page', 46, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (107, N'Note List', N'Permissions to access/view Note List page', 47, 61, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (108, N'Note Review', N'Permissions to access/view Note Review page', 47, 62, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (109, N'Group Note', N'Permissions to access/view Group Note page', 47, 63, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (110, N'Ansell Casey Review', N'Permissions to access/view Ansell Casey Review', 20, 16, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (112, N'View All', N'View all permission for Note List page', 107, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (113, N'View Assigned', N'Only view assigned permission for Note List page', 107, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (114, N'View All', N'View all permission for Note Review page', 108, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (115, N'View Assigned', N'Only view assigned permission for Note Review page', 108, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (116, N'Assigned Notes For Review', N'Permissions to access/view Assigned Notes For Review', 20, 17, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (117, N'Additional Permissions', N'Additional Permissions', 0, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (118, N'Role Permission', N'Access to Role Permission page', 117, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (119, N'Upload Employees Signature', N'Access to upload all Empoyees Signature', 117, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (120, N'View Notes Amount', N'Access to view Notes Amount', 117, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (121, N'Eligibility Check - 270 / 271', N'Permissions to access/view Eligibility Check - 270 / 271 page', 46, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (122, N'Claim Ack - 277CA', N'Permissions to access/view Claim Ack - 277CA page', 46, NULL, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (123, N'Prorate Service Code', N'Permissions to access/view Prorate Service Code page', 47, 64, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (124, N'Request - Client List Report', N'Request - Client List Report', 48, 91, 0, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (1001, N'Administrative Permission', N'Special permissions for admin/superadmin', 0, NULL, 1, 0, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2001, N'Dashboard', N'Dashboard access', 0, 2, 0, 1, N'Dashboard', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2002, N'Employees - Didn’t Clock In / Clock Out', N'Dashboard access', 2001, NULL, 0, 1, N'Dashboard_Emp_DidNot_ClockInOut', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2003, N'Employees - Over Time in Last 7 Days', N'Dashboard access', 2001, NULL, 0, 1, N'Dashboard_Emp_OverTime_In7Days', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2004, N'Patient - New', N'Dashboard access', 2001, NULL, 0, 1, N'Dashboard_Patient_New', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2005, N'Patient - Fully Not Scheduled for Next 7 Days', N'Dashboard access', 2001, NULL, 0, 1, N'Dashboard_Patient_Fully_NotSch', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2006, N'Received Internal Message', N'Dashboard access', 2001, NULL, 0, 1, N'Dashboard_Received_InternalMessage', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2007, N'Sent Internal Message', N'Dashboard access', 2001, NULL, 0, 1, N'Dashboard_Sent_InternalMessage', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2008, N'Masters', N'All master pages access', 0, 3, 0, 1, N'Masters', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2009, N'Employee', N'Employee access', 0, 4, 0, 1, N'Masters_Employee', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2010, N'Preference/Skill', N'Preference/Skill access', 2008, 27, 0, 1, N'Masters_PreferenceSkill', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2011, N'Service Code', N'Service Code access', 2008, 28, 0, 1, N'Masters_ServiceCode', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2012, N'Visit Task', N'Visit Task access', 2008, 29, 0, 1, N'Masters_VisitTask', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2013, N'Add/Update', N'Employee add/update access', 2009, 1, 0, 1, N'Masters_Employee_AddUpdate', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2014, N'List', N'Employee list access', 2009, 3, 0, 1, N'Masters_Employee_List', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2015, N'Calendar', N'Employee calendar access', 2009, 4, 0, 1, N'Masters_Employee_Calender', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2016, N'Schedule', N'Employee schedule access', 2009, 5, 0, 1, N'Masters_Employee_Schedule', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2017, N'PTO', N'Employee PTO access', 2009, 6, 0, 1, N'Masters_Employee_PTO', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2018, N'Add/Update', N'Preference/Skill add/update access', 2010, 1, 0, 1, N'Masters_PreferenceSkill_AddUpdate', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2019, N'List', N'Preference/Skill list access', 2010, 3, 0, 1, N'Masters_PreferenceSkill_List', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2020, N'Add/Update', N'Service Code add/update access', 2011, NULL, 0, 1, N'Masters_ServiceCode_AddUpdate', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2021, N'List', N'Service Code list access', 2011, NULL, 0, 1, N'Masters_ServiceCode_List', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2022, N'Add/Update', N'Visit Task add/update access', 2012, 1, 0, 1, N'Masters_VisitTask_AddUpdate', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2023, N'List', N'Visit Task list access', 2012, 3, 0, 1, N'Masters_VisitTask_List', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2024, N'Patient Intake', N'Patient pages access', 0, 5, 0, 1, N'PatientIntake', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2025, N'Add/Update', N'Patient add/update access', 2024, 31, 0, 1, N'Patient_AddUpdate', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2026, N'List', N'Patient list access', 2024, 33, 0, 1, N'Patient_List', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2027, N'Calendar', N'Patient calendar access', 2024, 34, 0, 1, N'Patient_Calendar', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2028, N'Schedule', N'Patient schedule access', 2024, 35, 0, 1, N'Patient_Schedule', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2029, N'Scheduling', N'Scheduling pages access', 0, 6, 0, 1, N'Scheduling', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2030, N'Schedule Master', N'Schedule Master access', 2029, 41, 0, 1, N'Scheduling_ScheduleMaster', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2031, N'Schedule Log', N'Schedule Log access', 2029, 42, 0, 1, N'Scheduling_ScheduleLog', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2032, N'Messages', N'Messages pages access', 0, 8, 0, 1, N'Messages', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2033, N'Received Message', N'Received Message access', 2032, 51, 0, 1, N'Message_Received_Msg', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2034, N'Sent Message', N'Sent Message access', 2032, 52, 0, 1, N'Message_Sent_Msg', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2035, N'Group SMS', N'Group SMS access', 2032, 53, 0, 1, N'GroupSMS', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2036, N'Reports', N'Reports pages access', 0, 9, 0, 1, N'Reports', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2037, N'Additional Permissions', N'Additional Permissions', 0, 1, 0, 1, N'AdditionalPermissions', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2038, N'Role Permission', N'Access to Role Permission page', 2037, NULL, 0, 1, N'AdditionalPermissions_RolePermission_Page', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2039, N'Employees Average Delays', N'Dashboard access', 2001, NULL, 0, 1, N'Dashboard_Emp_Average_Delays', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2040, N'Delete', N'Visit Task delete access', 2012, 2, 0, 1, N'Masters_VisitTask_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2041, N'Delete', N'Employee delete access', 2009, 2, 0, 1, N'Masters_Employee_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2042, N'Delete', N'Preference/Skill delete access', 2010, 2, 0, 1, N'Masters_PreferenceSkill_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2043, N'Delete', N'Patient delete access', 2024, 32, 0, 1, N'Patient_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2044, N'Payor', N'Payor Access', 2008, 25, 0, 1, N'Masters_Payor', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2045, N'Add/Update', N'Payor add/update access', 2044, NULL, 0, 1, N'Masters_Payor_AddUpdate', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2046, N'Delete', N'Payor delete access', 2044, NULL, 0, 1, N'Masters_Payor_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2047, N'List', N'Payor list access', 2044, NULL, 0, 1, N'Masters_Payor_List', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2048, N'Payor & Service Code Mapping', N'Payor and service code mapping page access', 2044, NULL, 0, 1, N'Masters_Payor_ServiceCode_Mapping', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2050, N'General Master', N'General Master page access', 2008, 24, 0, 1, N'Masters_GM', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2051, N'Add/Update', N'General Master add/update access', 2050, NULL, 0, 1, N'Masters_GM_AddUpdate', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2052, N'Delete', N'General Master delete access', 2050, NULL, 0, 1, N'Masters_GM_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2053, N'List', N'General Master list page access', 2050, NULL, 0, 1, N'Masters_GM_List', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2054, N'Organization Settings', N'Organization Settings page access', 2037, NULL, 0, 1, N'AdditionalPermissions_OrganizationSettings', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2055, N'DX Code', N'DX Code Access', 2008, 22, 0, 1, N'Masters_DXCode', 1, N'WEB')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2056, N'Add/Update', N'DX Code add/update access', 2055, NULL, 0, 1, N'Masters_DXCode_AddUpdate', 1, N'WEB')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2057, N'Delete', N'DX Code delete access', 2055, NULL, 0, 1, N'Masters_DXCode_Delete', 1, N'WEB')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2058, N'List', N'DX Code list access', 2055, NULL, 0, 1, N'Masters_DXCode_List', 1, N'WEB')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2059, N'Physician', N'Physician Access', 2008, 26, 0, 1, N'Masters_Physician', 1, N'WEB')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2060, N'Add/Update', N'Physician add/update access', 2059, NULL, 0, 1, N'Masters_Physician_AddUpdate', 1, N'WEB')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2061, N'Delete', N'Physician delete access', 2059, NULL, 0, 1, N'Masters_Physician_Delete', 1, N'WEB')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2062, N'List', N'Physician list access', 2059, NULL, 0, 1, N'Masters_Physician_List', 1, N'WEB')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2063, N'Billing & Claim Processing', N'Permission to create and update batch, generate EDI 837 file and process EDI 835 file', 0, 7, 0, 1, N'Billing', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2064, N'Batch 837', N'Permissions to access/view Batch 837 page', 2063, NULL, 0, 1, N'Billing_Batch837', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2065, N'Upload 835', N'Permissions to access/view Upload 835 page', 2063, NULL, 0, 1, N'Billing_Upload835', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2066, N'Reconcile 835 / EOB', N'Permissions to access/view Reconcile 835 / EOB page', 2063, NULL, 0, 1, N'Billing_Reconcile835', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2067, N'EDI File Logs', N'Permissions to access/view EDI File Logs page', 2063, NULL, 0, 1, N'Billing_EDIFileLogs', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2068, N'Broadcast Notifications', N'Permissions to access Broadcast notifications pages', 2032, 54, 0, 1, N'BroadcastNotifications', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2069, N'Facility House', N'Permisson to access Facility House page', 2008, 23, 0, 1, N'Masters_FacilityHouse', 1, N'WEB')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2070, N'Add/Update', N'Permisson to access Facility House add/update page', 2069, NULL, 0, 1, N'Masters_FacilityHouse_AddUpdate', 1, N'WEB')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2071, N'Delete', N'Permisson to access Facility House delete page', 2069, NULL, 0, 1, N'Masters_FacilityHouse_Delete', 1, N'WEB')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2072, N'List', N'Permisson to access Facility House list page', 2069, NULL, 0, 1, N'Masters_FacilityHousen_List', 1, N'WEB')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2073, N'Compliance', N'Compliance Detail page access', 2008, 21, 0, 1, N'Master_Compliance', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2074, N'Employee Billing Report', N'Employee Billing Report page access', 2036, 91, 0, 1, N'Report_EmployeeBillingReport', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2075, N'Employee Visit Report', N'Employee Visit Report page access', 2036, 92, 0, 1, N'Report_EmployeeVisitReport', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2076, N'Add/Update', N'Compliance Detail add/update access', 2073, NULL, 0, 1, N'Master_Compliance_AddUpdate', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2077, N'Delete', N'Compliance Detail delete access', 2073, NULL, 0, 1, N'Master_Compliance_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2078, N'List', N'Compliance Detail list page access', 2073, NULL, 0, 1, N'Master_Compliance_List', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2079, N'Pending Schedules', N'Pending Schedule List', 2029, 43, 0, 1, N'Scheduling_PendingSchedule', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2080, N'Can Approve Bypass Clock In & Clock Out', N'Permisson to do approve or reject bypass visit entries', 0, 100, 0, 1, N'Can_Approve_Bypass_ClockInOut', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2081, N'RAL', N'RAL', 0, 0, 0, 1, N'RAL', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2082, N'GeoFencing Required', N'RAL', 2081, 0, 0, 1, N'GeoFencing_Required', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2083, N'Default Schedule Time', N'RAL', 2081, 0, 0, 1, N'Default_Schedule', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2084, N'Eligibility - 270/271', N'Permissions to access/view Eligibility - 270/271 page', 2063, 0, 0, 1, N'Billing_EDI270_271', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2085, N'Agency', N'Agency page access', 2008, NULL, 0, 1, N'Masters_Agency', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2086, N'Add/Update', N'Agency add/update access', 2085, NULL, 0, 1, N'Masters_Agency_AddUpdate', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2087, N'Delete', N'Agency delete access', 2085, NULL, 0, 1, N'Masters_Agency_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2088, N'List', N'Agency lisitng access', 2085, NULL, 0, 1, N'Masters_Agency_List', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2089, N'Patient Details', N'Patient Details Access', 2025, 1, 0, 1, N'PatientIntake_PatientDetails', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2090, N'View Only', N'Patient Details View Only Access', 2089, 1, 0, 1, N'PatientIntake_PatientDetails_ViewOnly', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2091, N'Add/Update', N'Patient Details Add/Update Access', 2089, 1, 0, 1, N'PatientIntake_PatientDetails_AddUpdate', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2092, N'Documents', N'Patient Document Access', 2025, 1, 0, 1, N'PatientIntake_Documents', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2093, N'Add Section', N'Add Section Access', 2092, 1, 0, 1, N'PatientIntake_Documents_AddSection', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2094, N'Add Subsection', N'Add Subsection Access', 2092, 1, 0, 1, N'PatientIntake_Documents_AddSubSection', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2095, N'View Documents', N'View Documents Access', 2092, 1, 0, 1, N'PatientIntake_Documents_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2096, N'Add/Update Documents', N'Add/Update Documents Access', 2092, 1, 0, 1, N'PatientIntake_Documents_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2097, N'Delete Documents', N'Delete Documents Access', 2092, 1, 0, 1, N'PatientIntake_Documents_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2098, N'Billing', N'Billing Setting Access', 2025, 1, 0, 1, N'PatientIntake_Billing', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2099, N'Patient Payors', N'Patient Payors Access', 2098, 1, 0, 1, N'PatientIntake_Billing_PatientPayor', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2100, N'View Only', N'View Only Access', 2099, 1, 0, 1, N'PatientIntake_Billing_PatientPayor_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2101, N'Add/Update', N'Add/Update Access', 2099, 1, 0, 1, N'PatientIntake_Billing_PatientPayor_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2102, N'Delete', N'Delete Access', 2099, 1, 0, 1, N'PatientIntake_Billing_PatientPayor_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2103, N'Billing Details', N'Billing Details Access', 2098, 1, 0, 1, N'PatientIntake_Billing_Details', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2104, N'View Only', N'View Only Access', 2103, 1, 0, 1, N'PatientIntake_Billing_Details_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2105, N'Add/Update', N'Add/Update Access', 2103, 1, 0, 1, N'PatientIntake_Billing_Details_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2106, N'Delete', N'Delete Access', 2103, 1, 0, 1, N'PatientIntake_Billing_Details_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2107, N'Prior Authorization', N'Prior Authorization Access', 2098, 1, 0, 1, N'PatientIntake_Billing_PR', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2108, N'View Only', N'View Only Access', 2107, 1, 0, 1, N'PatientIntake_Billing_PR_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2109, N'Add/Update', N'Add/Update Access', 2107, 1, 0, 1, N'PatientIntake_Billing_PR_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2110, N'Delete', N'Delete Access', 2107, 1, 0, 1, N'PatientIntake_Billing_PR_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2111, N'Care Plan', N'Care Plan Access', 2025, 1, 0, 1, N'PatientIntake_CarePlan', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2112, N'Tasks Mapping', N'Patient Payors Access', 2111, 1, 0, 1, N'PatientIntake_CarePlan_TaskMapping', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2113, N'View Only', N'View Only Access', 2112, 1, 0, 1, N'PatientIntake_CarePlan_TaskMapping_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2114, N'Add/Update', N'Add/Update Access', 2112, 1, 0, 1, N'PatientIntake_CarePlan_TaskMapping_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2115, N'Delete', N'Delete Access', 2112, 1, 0, 1, N'PatientIntake_CarePlan_TaskMapping_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2116, N'Patient Schedule', N'Billing Details Access', 2111, 1, 0, 1, N'PatientIntake_CarePlan_PatientSchedule', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2117, N'View Only', N'View Only Access', 2116, 1, 0, 1, N'PatientIntake_CarePlan_PatientSchedule_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2118, N'Add/Update', N'Add/Update Access', 2116, 1, 0, 1, N'PatientIntake_CarePlan_PatientSchedule_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2119, N'Calendar', N'Calendar Access', 2025, 1, 0, 1, N'PatientIntake_PatientCalendar', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2120, N'Block Employees', N'Block Employees Access', 2025, 1, 0, 1, N'PatientIntake_BlockEmployees', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2121, N'View Only', N'View Only Access', 2120, 1, 0, 1, N'PatientIntake_BlockEmployees_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2122, N'Add/Update', N'Add/Update Access', 2120, 1, 0, 1, N'PatientIntake_BlockEmployees_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2123, N'Delete', N'Delete Access', 2120, 1, 0, 1, N'PatientIntake_BlockEmployees_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2124, N'Internal Messaging', N'Internal Messaging Access', 2025, 1, 0, 1, N'PatientIntake_IM', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2125, N'View Only', N'View Only Access', 2124, 1, 0, 1, N'PatientIntake_IM_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2126, N'Add/Update', N'Add/Update Access', 2124, 1, 0, 1, N'PatientIntake_IM_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2127, N'Delete', N'Delete Access', 2124, 1, 0, 1, N'PatientIntake_IM_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2128, N'TimeSheet', N'TimeSheet Access', 2025, 1, 0, 1, N'PatientIntake_TimeSheet', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2129, N'View Only', N'View Only Access', 2128, 1, 0, 1, N'PatientIntake_TimeSheet_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2130, N'Add/Update', N'Add/Update Access', 2128, 1, 0, 1, N'PatientIntake_TimeSheet_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2131, N'Delete', N'Delete Access', 2128, 1, 0, 1, N'PatientIntake_TimeSheet_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2132, N'Notes', N'Notes Access', 2025, 1, 0, 1, N'PatientIntake_Notes', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2133, N'View Only', N'View Only Access', 2132, 1, 0, 1, N'PatientIntake_Notes_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2134, N'Add/Update', N'Add/Update Access', 2132, 1, 0, 1, N'PatientIntake_Notes_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2135, N'Delete', N'Delete Access', 2132, 1, 0, 1, N'PatientIntake_Notes_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2136, N'Forms', N'Forms Access', 2025, 1, 0, 1, N'PatientIntake_Forms', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2137, N'View Only', N'View Only Access', 2136, 1, 0, 1, N'PatientIntake_Forms_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2138, N'Add/Update', N'Add/Update Access', 2136, 1, 0, 1, N'PatientIntake_Forms_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2139, N'Information', N'Employee Information Access', 2013, 1, 0, 1, N'Employee_EmployeeInfo', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2140, N'Employee Documents', N'Employee Document Access', 2013, 1, 0, 1, N'Employee_EmployeeDocuments', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2141, N'Add Section', N'Add Section Access', 2140, 1, 0, 1, N'Employee_EmployeeDocuments_AddSection', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2142, N'Add Subsection', N'Add Subsection Access', 2140, 1, 0, 1, N'Employee_EmployeeDocuments_AddSubSection', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2143, N'View Documents', N'View Documents Access', 2140, 1, 0, 1, N'Employee_EmployeeDocuments_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2144, N'Add/Update Documents', N'Add/Update Documents Access', 2140, 1, 0, 1, N'Employee_EmployeeDocuments_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2145, N'Delete Documents', N'Delete Documents Access', 2140, 1, 0, 1, N'Employee_EmployeeDocuments_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2146, N'Employee Schedule', N'Employee Schedule Access', 2013, 1, 0, 1, N'Employee_EmployeeSchedule', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2147, N'View Only', N'View Only Access', 2146, 1, 0, 1, N'Employee_EmployeeSchedule_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2148, N'Add/Update', N'Add/Update Access', 2146, 1, 0, 1, N'Employee_EmployeeSchedule_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2149, N'Personal Time Off', N'Personal Time Off Access', 2013, 1, 0, 1, N'Employee_EmployeePTO', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2150, N'View Only', N'View Only Access', 2149, 1, 0, 1, N'Employee_EmployeePTO_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2151, N'Add/Update', N'Add/Update Access', 2149, 1, 0, 1, N'Employee_EmployeePTO_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2152, N'Delete', N'Delete Access', 2149, 1, 0, 1, N'Employee_EmployeePTO_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2153, N'Calendar', N'Calendar Access', 2013, 1, 0, 1, N'Employee_EmployeeCalendar', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2154, N'TimeSheet', N'TimeSheet Access', 2013, 1, 0, 1, N'Empoyee_TimeSheet', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2155, N'View Only', N'View Only Access', 2154, 1, 0, 1, N'Empoyee_TimeSheet_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2156, N'Add/Update', N'Add/Update Access', 2154, 1, 0, 1, N'Empoyee_TimeSheett_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2157, N'Delete', N'Delete Access', 2154, 1, 0, 1, N'Empoyee_TimeSheet_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2158, N'Notes', N'Notes Access', 2013, 1, 0, 1, N'Empoyee_Notes', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2159, N'View Only', N'View Only Access', 2158, 1, 0, 1, N'Empoyee_Notes_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2160, N'Add/Update', N'Add/Update Access', 2158, 1, 0, 1, N'Empoyee_Notest_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2161, N'Delete', N'Delete Access', 2158, 1, 0, 1, N'Empoyee_Notes_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2162, N'Certificate', N'Certificate Access', 2013, 1, 0, 1, N'Employee_Certificate', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2163, N'Checklist', N'Checklist Access', 2013, 1, 0, 1, N'Employee_Checklist', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2164, N'View Only', N'View Only Access', 2163, 1, 0, 1, N'Employee_Checklist_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2165, N'Add/Update', N'Add/Update Access', 2163, 1, 0, 1, N'Employee_Checklist_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2166, N'View Only', N'View Only Access', 2139, 1, 0, 1, N'Employee_EmployeeInfo_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2167, N'Add/Update', N'Add/Update Access', 2139, 1, 0, 1, N'Employee_EmployeeInfo_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2168, N'Frequency', N'Frequency Access', 2111, 1, 0, 1, N'PatientIntake_CarePlan_Frequency', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2169, N'View Only', N'View Only Access', 2168, 1, 0, 1, N'PatientIntake_CarePlan_Frequency_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2170, N'Add/Update', N'Add/Update Access', 2168, 1, 0, 1, N'PatientIntake_CarePlan_Frequency_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2171, N'Delete', N'Delete Access', 2168, 1, 0, 1, N'PatientIntake_CarePlan_Frequency_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2172, N'Case Load', N'Case Load Access', 2111, 1, 0, 1, N'PatientIntake_CarePlan_CaseLoad', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2173, N'View Only', N'View Only Access', 2172, 1, 0, 1, N'PatientIntake_CarePlan_CaseLoad_View', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2174, N'Add/Update', N'Add/Update Access', 2172, 1, 0, 1, N'PatientIntake_CarePlan_CaseLoad_Add', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2175, N'Delete', N'Delete Access', 2172, 1, 0, 1, N'PatientIntake_CarePlan_CaseLoad_Delete', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2200, N'CaseManager', N'CaseManager access', 2008, 28, 0, 1, N'Masters_CaseManager', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2201, N'Add/Update', N'CaseManager add/update access', 2200, NULL, 0, 1, N'Masters_CaseManager_AddUpdate', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2202, N'List', N'CaseManager list access', 2200, NULL, 0, 1, N'Masters_CaseManager_List', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (2999, N'Administrative Permission', N'Special permissions for admin/superadmin', 0, NULL, 0, 1, N'AdministrativePermission', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3000, N'1 hr', N'Default Schedule Time', 2083, 0, 0, 1, N'1_hr', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3001, N'Home - Upcoming Visits', N'Home - Upcoming Visit page Access', 0, 1, 0, 1, N'Mobile_Home_UpcomingVisits', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3002, N'Messages', N'Message menu access', 0, 11, 0, 1, N'Mobile_Messages', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3003, N'Received Message', N'Received Message menu access', 3002, 12, 0, 1, N'Mobile_Message_Received_Msg', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3004, N'Sent Message', N'Sent Message menu access', 3002, 13, 0, 1, N'Mobile_Message_Sent_Msg', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3005, N'Create Message', N'Create Message menu access', 3002, 14, 0, 1, N'Mobile_CreateSMS', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3006, N'Clock In - Clock Out Time Update', N'Permisson to change the Clock In - Clock Out Time Update', 0, 101, 0, 1, N'Mobile_ClockInOutTimeUpdate', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3007, N'Patient Service Denied', N'Patient Service Denied permisson', 0, 102, 0, 1, N'Mobile_Patient_ServiceDenied', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3008, N'Auto Approved Bypass Clock In & Clock Out', N'Permisson to allow Bypass Clock In & Clock Out', 3016, 103, 0, 1, N'Mobile_Bypass_ClockInOut', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3009, N'Visit History', N'Permisson to access Visit History page', 0, 105, 0, 1, N'Mobile_VisitHistory', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3010, N'View Only', N'Permisson to View Visit History Page', 3009, 104, 0, 1, N'Mobile_VisitHistory_ViewOnly', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3011, N'Update Tasks', N'Permisson to Update Visit Task time', 3009, 104, 0, 1, N'Mobile_VisitHistory_UpdateTasks', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3012, N'Auto Approved IVR Bypass Clock In & Clock Out', N'Auto Approved Permisson to allow IVR Bypass Clock In & Clock Out', 3016, 104, 0, 1, N'Mobile_IVR_Bypass_ClockInOut', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3013, N'IVR Instant No Schedule Clock In / Clock Out', N'Permission for instant no schedule clock in / clock out', 0, 104, 0, 1, N'Mobile_IVR_InstantNoSchedule_ClockInOut', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3014, N'Can Update Location Coordinates', N'Permission for send geo coordinates', 0, 106, 0, 1, N'Mobile_Send_Location', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3015, N'Approval Required for Bypass Clock In & Clock Out', N'Permisson to allow IVR Bypass Clock In & Clock Out but approval required', 3017, NULL, 0, 1, N'Mobile_ApprovalRequired_Bypass_ClockInOut', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3016, N'Auto Approved Bypass Permissions', N'Permission to Auto Approved Bypass Clock In & Clock Out', 0, 103, 0, 1, N'Mobile_Auto_Approved_Bypass', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3017, N'Approval Required for Bypass Permissions', N'Permission to Approval Required for Bypass Clock In & Clock Out', 0, 103, 0, 1, N'Mobile_Approval_Required_Bypass', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3018, N'Approval Required for IVR Bypass Clock In & Clock Out', N'Permission to Approval Required for IVR Bypass Clock In & Clock Out', 3017, NULL, 0, 1, N'Mobile_ApprovalRequired_IVR_Bypass_ClockInOut', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3019, N'Department of Medical Assistance Services (DMAS)', N'DMAS forms(only for virginia)', 2036, 93, 0, 1, N'DMAS_From', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3020, N'Weekly Time Sheet', N'Permission to add weekly time sheet', 2036, 94, 0, 1, N'Weekly_TimeSheet', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3021, N'Record Access', N'Records can be seen by user', 0, 1, 0, 1, N'Record_Access', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3022, N'All Record', N'All Record', 3021, 1, 0, 1, N'Record_Access_All_Record', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3023, N'Limited Record', N'Limited Record', 3021, 1, 0, 1, N'Record_Access_Limited_Record', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3024, N'Early Clockin', N'Permission to Clock in early', 0, NULL, 0, 1, N'Mobile_Early_Clock_in', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3025, N'15 min', N'15 min early Clock-In', 3024, NULL, 0, 1, N'Mobile_Early_Clock_in_15', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3026, N'30 min', N'30 min early Clock-In', 3024, NULL, 0, 1, N'Mobile_Early_Clock_in_30', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3027, N'45 min', N'45 min early Clock-In', 3024, NULL, 0, 1, N'Mobile_Early_Clock_in_45', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3028, N'60 min', N'60 min early Clock-In', 3024, NULL, 0, 1, N'Mobile_Early_Clock_in_60', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3029, N'90 min', N'90 min early Clock-In', 3024, NULL, 0, 1, N'Mobile_Early_Clock_in_90', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3030, N'120 min', N'120 min early Clock-In', 3024, NULL, 0, 1, N'Mobile_Early_Clock_in_120', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3031, N'Task Entry Type', N'Simple or Detail Task Entry', 0, 1, 0, 1, NULL, 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3032, N'Simple', N'Simple Task Entry', 3031, 2, 0, 1, N'Empoyee_TimeSheet_SimpleTask', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3033, N'Detail', N'Detail Task Entry', 3031, 2, 0, 1, N'Empoyee_TimeSheet_DetailTask', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3034, N'Task Entry Type', N'Simple or Detail Task Entry', 0, 1, 0, 1, NULL, 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3035, N'Simple', N'Simple Task Entry', 3034, 2, 0, 1, N'Mobile_Simple_Entry', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3036, N'Detail', N'Detail Task Entry', 3034, 2, 0, 1, N'Mobile_Detail_Entry', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3100, N'2 hr', N'Default Schedule Time', 2083, 0, 0, 1, N'2_hr', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3101, N'3 hr', N'Default Schedule Time', 2083, 0, 0, 1, N'3_hr', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3102, N'4 hr', N'Default Schedule Time', 2083, 0, 0, 1, N'4_hr', 1, N'Mobile')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3103, N'ActivePatient', N'ActivePatient', 2036, 94, 0, 1, N'ActivePatient', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3104, N'EmployeeTimeSheetReoprt', N'EmployeeTimeSheetReoprt', 2036, 94, 0, 1, N'EmployeeTimeSheetReoprt', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3105, N'PatientHourSummaryReoprt', N'PatientHourSummaryReoprt', 2036, 94, 0, 1, N'PatientHourSummaryReoprt', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3106, N'EmployeeVisitReport', N'EmployeeVisitReport', 2036, 94, 0, 1, N'EmployeeVisitReport', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3107, N'PatientVisitReport', N'PatientVisitReport', 2036, 94, 0, 1, N'PatientVisitReport', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3108, N'SSN', N'Social Security Number', 2009, 1, 0, 1, N'Employee_SSN', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3109, N'Can See', N'Can See', 3108, 1, 0, 1, N'Can_See_SSN', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3110, N'Can''t See', N'Can''t See', 3108, 1, 0, 1, N'Can_Not_See_SSN', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3111, N'Can See Last Four Digit', N'Can See ', 3108, 1, 0, 1, N'Can_See_Last_Four_Digit', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3112, N'SSN', N'Social Security Number', 2024, 1, 0, 1, N'Patient_SSN', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3113, N'Can See', N'Can See', 3112, 1, 0, 1, N'Can_See_SSN_Patient', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3114, N'Can''t See', N'Can''t See', 3112, 1, 0, 1, N'Can_Not_See_SSN_Patient', 1, N'Web')
GO
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (3115, N'Can See Last Four Digit', N'Can See ', 3112, 1, 0, 1, N'Can_See_Last_Four_Digit_SSN_Patient', 1, N'Web')
GO
