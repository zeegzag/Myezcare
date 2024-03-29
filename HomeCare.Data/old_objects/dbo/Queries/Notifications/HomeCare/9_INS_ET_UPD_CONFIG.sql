-- Make sure you will execute just once, when you set up this task first time.

DECLARE @MaxID BIGINT = (SELECT MAX([EmailTemplateID]) FROM [dbo].[EmailTemplates])

-- Email Templates

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Late Clock-in Notification Email', N'Late Clock-in Notification', N'Hello,<br><br>##EmployeeName##&nbsp;has done late clock-in for the ##PatientName##.', N'28', N'##EmployeeName##,##PatientName##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 26)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Late Clock-in Notification SMS', N'Late Clock-in Notification', N'##EmployeeName## has done late clock-in for the ##PatientName##.', N'29', N'##EmployeeName##,##PatientName##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 27)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Late Clock-out Notification Email', N'Late Clock-out Notification', N'Hello,<br><br>##EmployeeName##&nbsp;has done late clock-out for the ##PatientName##.', N'28', N'##EmployeeName##,##PatientName##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 28)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Late Clock-out Notification SMS', N'Late Clock-out Notification', N'##EmployeeName## has done late clock-out for the ##PatientName##.', N'29', N'##EmployeeName##,##PatientName##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 29)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Early Clock-out Notification Email', N'Early Clock-out Notification', N'Hello,<br><br>##EmployeeName##&nbsp;has done early clock-out for the ##PatientName##.', N'28', N'##EmployeeName##,##PatientName##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 30)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Early Clock-out Notification SMS', N'Early Clock-out Notification', N'##EmployeeName## has done early clock-out for the ##PatientName##.', N'29', N'##EmployeeName##,##PatientName##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 31)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Conclusion Notes Notification Email', N'Conclusion Notes Notification', N'Hello,<br><br>##EmployeeName##&nbsp;has added conclusion note for the ##PatientName##.', N'28', N'##EmployeeName##,##PatientName##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 32)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Conclusion Notes Notification SMS', N'Conclusion Notes Notification', N'##EmployeeName## has added conclusion note for the ##PatientName##.', N'29', N'##EmployeeName##,##PatientName##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 33)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Expired Documents Notification Email', N'Expired Documents Notification', N'Hello,<br><br>The document ##FileName## have expired for the ##FullName##.', N'28', N'##FileName##,##FullName##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 34)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Expired Documents Notification SMS', N'Expired Documents Notification', N'The document ##FileName## have expired for the ##FullName##.', N'29', N'##FileName##,##FullName##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 35)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Near Expiring Documents Notification Email', N'Near Expiring Documents Notification', N'Hello,<br><br>The document ##FileName## soon to be expired for the ##FullName##.', N'28', N'##FileName##,##FullName##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 36)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Near Expiring Documents Notification SMS', N'Near Expiring Documents Notification', N'The document ##FileName## soon to be expired for the ##FullName##.', N'29', N'##FileName##,##FullName##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 37)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'PTO Created Notification Email', N'PTO Created Notification', N'<div>Hello,<br><br><span style="font-weight: bold;">##FirstName## ##LastName##</span> has created PTO.&nbsp;</div><div><br></div><div>Please refer to the below details:<br></div><div><br></div><div><span style="font-weight: bold;">Start Time:</span> ##DayOffStartTime##</div><div><span style="font-weight: bold;">End Time: </span>##DayOffEndTime##</div><div><span style="font-weight: bold;">Type:</span> ##DayOffType##<br></div><div><br></div><div><span style="font-weight: bold;">Comment:</span> ##Comment##<br></div>', N'28', N'##FirstName##,##LastName##,##DayOffStartTime##,##DayOffEndTime##,##DayOffType##,##Comment##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 38)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'PTO Created Notification SMS', N'PTO Created Notification', N'##FirstName## ##LastName## has created PTO with ##DayOffType## type from ##DayOffStartTime## to ##DayOffEndTime##.', N'29', N'##FirstName##,##LastName##,##DayOffStartTime##,##DayOffEndTime##,##DayOffType##,##Comment##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 39)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Patient Denied Service Notification Email', N'Patient Denied Service Notification', N'Hello,<br><br>The ##PatientName## denied service for ##DropOffDay## @ ##DropOffTime##.', N'28', N'##DropOffDay##,##DropOffTime##,##PatientName##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 40)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Patient Denied Service Notification SMS', N'Patient Denied Service Notification', N'The ##PatientName## denied service of the for ##DropOffDay## @ ##DropOffTime##.', N'29', N'##DropOffDay##,##DropOffTime##,##PatientName##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 41)
--
INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Patient on Hold Notification Email', N'Patient on Hold Notification', N'Hello,<br><br>##EmployeeName##&nbsp;has put the ##PatientName## on hold.', N'28', N'##EmployeeName##,##PatientName##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 42)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Patient on Hold Notification SMS', N'Patient on Hold Notification', N'##EmployeeName## has put the ##PatientName## on hold.', N'29', N'##EmployeeName##,##PatientName##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 43)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Schedule Cancellation Notification Email', N'Schedule Cancellation Notification', N'Hello,<br><br>The Schedule is cancelled for ##DropOffDay## @ ##DropOffTime## of the ##PatientName##.', N'28', N'##DropOffDay##,##DropOffTime##,##PatientName##,##EmployeeName##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 44)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Schedule Cancellation Notification SMS', N'Schedule Cancellation Notification', N'The Schedule is cancelled for ##DropOffDay## @ ##DropOffTime## of the ##PatientName##.', N'29', N'##DropOffDay##,##DropOffTime##,##PatientName##,##EmployeeName##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 45)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Internal Message Notification Email', N'Internal Message Notification', N'Hello,<br><br>The internal message received.<br>Message: ##Message##<br>Sent By: ##MessageBy##<br>Assignee: ##Assignee##', N'28', N'##Message##,##MessageBy##,##Assignee##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 46)

INSERT [dbo].[EmailTemplates] ([EmailTemplateName], [EmailTemplateSubject], [EmailTemplateBody], [EmailTemplateTypeID], [Token], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted], [OrderNumber]) 
	VALUES (N'Internal Message Notification SMS', N'Internal Message Notification', N'The internal message received. Message: ##Message##', N'29', N'##Message##,##MessageBy##,##Assignee##', GETDATE(), 1, GETDATE(), 1, N'::1', 0, 47)

-- Update Notification Configurations Templates

UPDATE NC
SET 
	[EmailTemplateID] = N.[STID] - 1,
	[SMSTemplateID] = N.[STID]
FROM 
	[notif].[NotificationConfigurations] NC
CROSS APPLY	(
	SELECT @MaxID + (NC.[NotificationConfigurationID] * 2) [STID]
) N
WHERE
	NC.[NotificationConfigurationID] <= 11