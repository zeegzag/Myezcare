﻿CREATE TABLE [dbo].[Notes] (
    [NoteID]                                                            BIGINT          IDENTITY (1, 1) NOT NULL,
    [ReferralID]                                                        BIGINT          NOT NULL,
    [AHCCCSID]                                                          VARCHAR (25)    NULL,
    [CISNumber]                                                         VARCHAR (15)    NULL,
    [ContinuedDX]                                                       VARCHAR (MAX)   NULL,
    [ServiceDate]                                                       DATE            NOT NULL,
    [ServiceCodeID]                                                     BIGINT          NULL,
    [ServiceCode]                                                       VARCHAR (50)    NULL,
    [ServiceName]                                                       VARCHAR (100)   NULL,
    [Description]                                                       VARCHAR (500)   NULL,
    [MaxUnit]                                                           INT             NULL,
    [DailyUnitLimit]                                                    INT             NULL,
    [UnitType]                                                          INT             NULL,
    [PerUnitQuantity]                                                   DECIMAL (18)    NULL,
    [ServiceCodeType]                                                   INT             NOT NULL,
    [ServiceCodeStartDate]                                              DATE            NULL,
    [ServiceCodeEndDate]                                                DATE            NULL,
    [CheckRespiteHours]                                                 BIT             NULL,
    [ModifierID]                                                        VARCHAR (500)   NULL,
    [PosID]                                                             BIGINT          NULL,
    [Rate]                                                              DECIMAL (18, 2) NULL,
    [POSStartDate]                                                      DATE            NULL,
    [POSEndDate]                                                        DATE            NULL,
    [ZarephathService]                                                  VARCHAR (50)    NULL,
    [StartMile]                                                         BIGINT          NULL,
    [EndMile]                                                           BIGINT          NULL,
    [StartTime]                                                         DATETIME        NULL,
    [EndTime]                                                           DATETIME        NULL,
    [CalculatedUnit]                                                    FLOAT (53)      NULL,
    [NoteDetails]                                                       VARCHAR (MAX)   NULL,
    [Assessment]                                                        VARCHAR (MAX)   NULL,
    [ActionPlan]                                                        VARCHAR (MAX)   NULL,
    [SpokeTo]                                                           VARCHAR (MAX)   NULL,
    [Relation]                                                          VARCHAR (50)    NULL,
    [OtherNoteType]                                                     VARCHAR (50)    NULL,
    [MarkAsComplete]                                                    BIT             CONSTRAINT [DF_Notes_MarkAsComplete] DEFAULT ((0)) NOT NULL,
    [SignatureDate]                                                     DATETIME        NULL,
    [CreatedBy]                                                         BIGINT          NULL,
    [CreatedDate]                                                       DATETIME        NOT NULL,
    [UpdatedDate]                                                       DATETIME        NULL,
    [UpdatedBy]                                                         BIGINT          NOT NULL,
    [SystemID]                                                          VARCHAR (100)   NULL,
    [IssueID]                                                           BIGINT          NULL,
    [IssueAssignID]                                                     BIGINT          NULL,
    [POSDetail]                                                         VARCHAR (100)   NULL,
    [IsBillable]                                                        BIT             CONSTRAINT [DF_Notes_IsBillable] DEFAULT ((0)) NOT NULL,
    [HasGroupOption]                                                    BIT             CONSTRAINT [DF_Notes_HasGroupOption] DEFAULT ((0)) NOT NULL,
    [PayorServiceCodeMappingID]                                         BIGINT          NULL,
    [PayorID]                                                           BIGINT          NULL,
    [IsDeleted]                                                         BIT             CONSTRAINT [DF_Notes_IsDeleted] DEFAULT ((0)) NOT NULL,
    [NoOfStops]                                                         INT             NULL,
    [Source]                                                            VARCHAR (MAX)   NULL,
    [RenderingProviderID]                                               BIGINT          NULL,
    [BillingProviderID]                                                 BIGINT          NULL,
    [BillingProviderName]                                               VARCHAR (100)   NULL,
    [BillingProviderAddress]                                            VARCHAR (200)   NULL,
    [BillingProviderCity]                                               VARCHAR (100)   NULL,
    [BillingProviderState]                                              VARCHAR (100)   NULL,
    [BillingProviderZipcode]                                            VARCHAR (50)    NULL,
    [BillingProviderEIN]                                                VARCHAR (10)    NULL,
    [BillingProviderNPI]                                                VARCHAR (20)    NULL,
    [BillingProviderGSA]                                                INT             NULL,
    [BillingProviderAHCCCSID]                                           VARCHAR (20)    NULL,
    [RenderingProviderName]                                             VARCHAR (100)   NULL,
    [RenderingProviderAddress]                                          VARCHAR (200)   NULL,
    [RenderingProviderCity]                                             VARCHAR (100)   NULL,
    [RenderingProviderState]                                            VARCHAR (100)   NULL,
    [RenderingProviderZipcode]                                          VARCHAR (50)    NULL,
    [RenderingProviderEIN]                                              VARCHAR (10)    NULL,
    [RenderingProviderNPI]                                              VARCHAR (20)    NULL,
    [RenderingProviderGSA]                                              INT             NULL,
    [RenderingProviderAHCCCSID]                                         VARCHAR (20)    NULL,
    [PayorName]                                                         VARCHAR (50)    NULL,
    [PayorShortName]                                                    VARCHAR (50)    NULL,
    [PayorAddress]                                                      VARCHAR (200)   NULL,
    [PayorIdentificationNumber]                                         VARCHAR (50)    NULL,
    [PayorCity]                                                         VARCHAR (50)    NULL,
    [PayorState]                                                        VARCHAR (50)    NULL,
    [PayorZipcode]                                                      VARCHAR (20)    NULL,
    [CalculatedAmount]                                                  FLOAT (53)      NULL,
    [AttachmentURL]                                                     VARCHAR (MAX)   NULL,
    [RandomGroupID]                                                     VARCHAR (100)   NULL,
    [DriverID]                                                          BIGINT          NULL,
    [VehicleNumber]                                                     VARCHAR (100)   NULL,
    [VehicleType]                                                       VARCHAR (100)   NULL,
    [PickUpAddress]                                                     VARCHAR (200)   NULL,
    [DropOffAddress]                                                    VARCHAR (200)   NULL,
    [RoundTrip]                                                         BIT             DEFAULT ((0)) NOT NULL,
    [OneWay]                                                            BIT             DEFAULT ((0)) NOT NULL,
    [MultiStops]                                                        BIT             DEFAULT ((0)) NOT NULL,
    [EscortName]                                                        VARCHAR (100)   NULL,
    [Relationship]                                                      VARCHAR (100)   NULL,
    [DTRIsOnline]                                                       BIT             DEFAULT ((0)) NOT NULL,
    [GroupIDForMileServices]                                            VARCHAR (100)   NULL,
    [NoteComments]                                                      VARCHAR (MAX)   NULL,
    [NoteAssignee]                                                      BIGINT          NULL,
    [NoteAssignedBy]                                                    BIGINT          NULL,
    [NoteAssignedDate]                                                  DATETIME        NULL,
    [MonthlySummaryIds]                                                 VARCHAR (500)   NULL,
    [GroupID]                                                           BIGINT          NULL,
    [ParentID]                                                          BIGINT          NULL,
    [CalculatedServiceTime]                                             BIGINT          NULL,
    [RenderingProviderFirstName]                                        VARCHAR (100)   NULL,
    [BillingProviderFirstName]                                          VARCHAR (100)   NULL,
    [EmployeeVisitID]                                                   BIGINT          NULL,
    [EmployeeVisitNoteIDs]                                              NVARCHAR (2000) NULL,
    [ScheduleID]                                                        BIGINT          NULL,
    [ReferralTSDateID]                                                  BIGINT          NULL,
    [ReferralBillingAuthorizationID]                                    BIGINT          NULL,
    [RenderingProvider_TaxonomyCode]                                    NVARCHAR (100)  NULL,
    [SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName] VARCHAR (100)   NULL,
    [SupervisingProvidername2420DLoop_NM104_NameFirst]                  VARCHAR (100)   NULL,
    [SupervisingProvidername2420DLoop_REF02_ReferenceId]                VARCHAR (100)   NULL,
    CONSTRAINT [PK_Notes] PRIMARY KEY CLUSTERED ([NoteID] ASC),
    CONSTRAINT [fk_Notes_Employess_ID] FOREIGN KEY ([DriverID]) REFERENCES [dbo].[Employees] ([EmployeeID])
);




GO
CREATE NONCLUSTERED INDEX [missing_index_74_73_Notes]
    ON [dbo].[Notes]([ReferralID] ASC, [IsBillable] ASC, [IsDeleted] ASC)
    INCLUDE([ServiceDate], [StartTime]);


GO
CREATE NONCLUSTERED INDEX [missing_index_17_16_Notes]
    ON [dbo].[Notes]([MarkAsComplete] ASC, [IsDeleted] ASC, [NoteAssignee] ASC);


GO
CREATE NONCLUSTERED INDEX [missing_index_15_14_Notes]
    ON [dbo].[Notes]([MarkAsComplete] ASC, [IsDeleted] ASC, [NoteAssignee] ASC)
    INCLUDE([ReferralID], [NoteComments], [NoteAssignedBy], [NoteAssignedDate]);


GO
CREATE NONCLUSTERED INDEX [missing_index_225185_225184_Notes]
    ON [dbo].[Notes]([EmployeeVisitID] ASC)
    INCLUDE([ServiceDate]);


GO
CREATE NONCLUSTERED INDEX [missing_index_210846_210845_Notes]
    ON [dbo].[Notes]([EmployeeVisitID] ASC);


GO
CREATE NONCLUSTERED INDEX [missing_index_210805_210804_Notes]
    ON [dbo].[Notes]([ReferralID] ASC)
    INCLUDE([ServiceDate], [ServiceCodeID], [ServiceCode], [PosID], [BillingProviderName], [BillingProviderNPI], [PayorName]);

