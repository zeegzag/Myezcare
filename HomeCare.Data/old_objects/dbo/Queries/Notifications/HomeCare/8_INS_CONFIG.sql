-- Make sure you will execute just once, when you set up this task first time.

-- Notification Events

INSERT [notif].[NotificationEvents] ([NotificationEventID], [EventName], [Description], [EventDefinitionSP], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (1, N'Late Clock-in', N'Event defined for late clock in', N'[notif].[LateClockIn]', GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationEvents] ([NotificationEventID], [EventName], [Description], [EventDefinitionSP], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (2, N'Late Clock-out', N'Event defined for late clock out', N'[notif].[LateClockOut]', GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationEvents] ([NotificationEventID], [EventName], [Description], [EventDefinitionSP], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (3, N'Early Clock out', N'Event defined for early clock out', N'[notif].[EarlyClockOut]', GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationEvents] ([NotificationEventID], [EventName], [Description], [EventDefinitionSP], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (4, N'Conclusion Notes', N'Event defined for conclusion notes', N'[notif].[ConclusionNotes]', GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationEvents] ([NotificationEventID], [EventName], [Description], [EventDefinitionSP], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (5, N'Expired Documents', N'Event defined for expired documents', N'[notif].[ExpiredDocuments]', GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationEvents] ([NotificationEventID], [EventName], [Description], [EventDefinitionSP], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (6, N'Expiring Documents Before 15 Days', N'Event defined for near expiring documents (before 15 days)', N'[notif].[NearExpiringDocumentsBefore15Days]', GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationEvents] ([NotificationEventID], [EventName], [Description], [EventDefinitionSP], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (7, N'PTO created', N'Event defined for pto created by employee', N'[notif].[PTOCreatedByEmployee]', GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationEvents] ([NotificationEventID], [EventName], [Description], [EventDefinitionSP], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (8, N'Patient Denied Service', N'Event defined for patient denied service', N'[notif].[PatientDeniedService]', GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationEvents] ([NotificationEventID], [EventName], [Description], [EventDefinitionSP], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (9, N'Patient on Hold', N'Event defined for patient on hold', N'[notif].[PatientOnHold]', GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationEvents] ([NotificationEventID], [EventName], [Description], [EventDefinitionSP], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (10, N'Schedule Cancellation', N'Event defined for schedule cancellation', N'[notif].[ScheduleCancellation]', GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationEvents] ([NotificationEventID], [EventName], [Description], [EventDefinitionSP], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (11, N'Internal Message', N'Event defined for messaging', N'[notif].[Messaging]', GETDATE(), 1, GETDATE(), 1, N'::1', 0)

-- Notification Configurations

INSERT [notif].[NotificationConfigurations] ([NotificationConfigurationID], [ConfigurationName], [Description], [NotificationEventID], [EmailTemplateID], [SMSTemplateID], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (1, N'Late Clock-in configuration', N'Configuration defined for late clock in event', 1, NULL, NULL, GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationConfigurations] ([NotificationConfigurationID], [ConfigurationName], [Description], [NotificationEventID], [EmailTemplateID], [SMSTemplateID], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (2, N'Late Clock-out configuration', N'Configuration defined for late clock out event', 2, NULL, NULL, GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationConfigurations] ([NotificationConfigurationID], [ConfigurationName], [Description], [NotificationEventID], [EmailTemplateID], [SMSTemplateID], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (3, N'Early Clock out configuration', N'Configuration defined for early clock out event', 3, NULL, NULL, GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationConfigurations] ([NotificationConfigurationID], [ConfigurationName], [Description], [NotificationEventID], [EmailTemplateID], [SMSTemplateID], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (4, N'Conclusion Notes configuration', N'Configuration defined for conclusion notes event', 4, NULL, NULL, GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationConfigurations] ([NotificationConfigurationID], [ConfigurationName], [Description], [NotificationEventID], [EmailTemplateID], [SMSTemplateID], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (5, N'Expired Documents configuration', N'Configuration defined for expired documents event', 5, NULL, NULL, GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationConfigurations] ([NotificationConfigurationID], [ConfigurationName], [Description], [NotificationEventID], [EmailTemplateID], [SMSTemplateID], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (6, N'Expiring Documents Before 15 Days configuration', N'Configuration defined for expiring documents before 15 days event', 6, NULL, NULL, GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationConfigurations] ([NotificationConfigurationID], [ConfigurationName], [Description], [NotificationEventID], [EmailTemplateID], [SMSTemplateID], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (7, N'PTO created configuration', N'Configuration defined for pto created event', 7, NULL, NULL, GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationConfigurations] ([NotificationConfigurationID], [ConfigurationName], [Description], [NotificationEventID], [EmailTemplateID], [SMSTemplateID], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (8, N'Patient Denied Service configuration', N'Configuration defined for patient denied service event', 8, NULL, NULL, GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationConfigurations] ([NotificationConfigurationID], [ConfigurationName], [Description], [NotificationEventID], [EmailTemplateID], [SMSTemplateID], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (9, N'Patient On Hold configuration', N'Configuration defined for patient on hold event', 9, NULL, NULL, GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationConfigurations] ([NotificationConfigurationID], [ConfigurationName], [Description], [NotificationEventID], [EmailTemplateID], [SMSTemplateID], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (10, N'Schedule Cancellation configuration', N'Configuration defined for schedule cancellation event', 10, NULL, NULL, GETDATE(), 1, GETDATE(), 1, N'::1', 0)
INSERT [notif].[NotificationConfigurations] ([NotificationConfigurationID], [ConfigurationName], [Description], [NotificationEventID], [EmailTemplateID], [SMSTemplateID], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]) 
	VALUES (11, N'Internal Message configuration', N'Configuration defined for messaging event', 11, NULL, NULL, GETDATE(), 1, GETDATE(), 1, N'::1', 0)

