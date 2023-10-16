CREATE TABLE [dbo].[EmployeeVisits] (
    [EmployeeVisitID]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [ScheduleID]                BIGINT          NOT NULL,
    [ClockInTime]               DATETIME        NULL,
    [ClockOutTime]              DATETIME        NULL,
    [IsDeleted]                 BIT             CONSTRAINT [DF_EmployeeVisits_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]               DATETIME        NULL,
    [CreatedBy]                 BIGINT          NULL,
    [UpdatedDate]               DATETIME        NULL,
    [UpdatedBy]                 BIGINT          NULL,
    [SystemID]                  VARCHAR (100)   NULL,
    [SurveyCompleted]           BIT             DEFAULT ((0)) NOT NULL,
    [SurveyComment]             NVARCHAR (1000) NULL,
    [IsByPassClockIn]           BIT             CONSTRAINT [DF__EmployeeV__IsByP__25276EE5] DEFAULT ((0)) NOT NULL,
    [IsByPassClockOut]          BIT             CONSTRAINT [DF__EmployeeV__IsByP__261B931E] DEFAULT ((0)) NOT NULL,
    [ByPassReasonClockIn]       NVARCHAR (MAX)  NULL,
    [ByPassReasonClockOut]      NVARCHAR (MAX)  NULL,
    [PlaceOfService]            VARCHAR (200)   NULL,
    [HHA_PCA_NP]                VARCHAR (200)   NULL,
    [OtherActivity]             VARCHAR (MAX)   NULL,
    [OtherActivityTime]         BIGINT          NULL,
    [IsSigned]                  BIT             CONSTRAINT [DF__EmployeeV__IsSig__781FBE44] DEFAULT ((0)) NOT NULL,
    [IsPCACompleted]            BIT             CONSTRAINT [DF__EmployeeV__IsPCA__7913E27D] DEFAULT ((0)) NOT NULL,
    [EmployeeSignatureID]       BIGINT          NULL,
    [PatientSignature]          NVARCHAR (MAX)  NULL,
    [IVRClockOut]               BIT             DEFAULT ((0)) NOT NULL,
    [BeneficiaryID]             VARCHAR (10)    NULL,
    [EarlyClockOutComment]      NVARCHAR (MAX)  NULL,
    [Latitude]                  FLOAT (53)      NULL,
    [Longitude]                 FLOAT (53)      NULL,
    [PCACompletedLat]           FLOAT (53)      NULL,
    [PCACompletedLong]          FLOAT (53)      NULL,
    [PCACompletedIPAddress]     VARCHAR (100)   NULL,
    [SignedLat]                 FLOAT (53)      NULL,
    [SignedLong]                FLOAT (53)      NULL,
    [SignedIPAddress]           VARCHAR (100)   NULL,
    [ActionTaken]               INT             NULL,
    [RejectReason]              NVARCHAR (1000) NULL,
    [IsApprovalRequired]        BIT             CONSTRAINT [DF__EmployeeV__IsApp__0623C4D8] DEFAULT ((0)) NULL,
    [IVRClockIn]                BIT             CONSTRAINT [DF__EmployeeV__IVRCl__137DBFF6] DEFAULT ((0)) NOT NULL,
    [isNotified]                BIT             CONSTRAINT [DF_EmployeeVisits_isNotified] DEFAULT ((0)) NULL,
    [LastCheckTime]             DATETIME        CONSTRAINT [DF_EmployeeVisits_LastCheckTime] DEFAULT (getdate()) NULL,
    [IsEarlyClockIn]            BIT             NULL,
    [EarlyClockInComment]       NVARCHAR (MAX)  NULL,
    [PatientSignature_ClockOut] NVARCHAR (MAX)  NULL,
    [IsSelf]                    BIT             NULL,
    [Name]                      NVARCHAR (100)  NULL,
    [Relation]                  NVARCHAR (100)  NULL,
    [IsInvoiceGenerated]        BIT             CONSTRAINT [DF__EmployeeV__IsInv__4ACDF4E0] DEFAULT ((0)) NULL,
    [Attendence]                NVARCHAR (MAX)  NULL,
    [IsApproved]                BIT             NULL,
    [ApproveNote]               NVARCHAR (MAX)  NULL,
    [Signature]                 NVARCHAR (MAX)  NULL,
    [SignedBy]                  BIGINT          NULL,
    [SignedDate]                DATETIME        NULL,
    [SignNote]                  NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_EmployeeVisits] PRIMARY KEY CLUSTERED ([EmployeeVisitID] ASC),
    CONSTRAINT [FK_EmployeeVisits_ScheduleMasters] FOREIGN KEY ([ScheduleID]) REFERENCES [dbo].[ScheduleMasters] ([ScheduleID]) ON DELETE CASCADE ON UPDATE CASCADE
);






GO
CREATE NONCLUSTERED INDEX [EmployeeVisits_ScheduleID]
    ON [dbo].[EmployeeVisits]([ScheduleID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisits001]
    ON [dbo].[EmployeeVisits]([ActionTaken] ASC, [IsApprovalRequired] ASC);

GO
CREATE TRIGGER [dbo].[tr_EmployeeVisits_Inserted] ON [dbo].[EmployeeVisits]
FOR INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE EV
    SET IsApproved = [dbo].[GetDefaultIsApprovedFlag](EV.CreatedBy)
    FROM [dbo].[EmployeeVisits] EV 
    INNER JOIN inserted I ON EV.[EmployeeVisitID] = I.[EmployeeVisitID]

END
GO
CREATE NONCLUSTERED INDEX [missing_index_210285_210284_EmployeeVisits]
    ON [dbo].[EmployeeVisits]([IsDeleted] ASC)
    INCLUDE([ScheduleID], [ClockInTime], [ClockOutTime], [IsPCACompleted]);


GO
CREATE NONCLUSTERED INDEX [missing_index_209844_209843_EmployeeVisits]
    ON [dbo].[EmployeeVisits]([IsDeleted] ASC, [IsPCACompleted] ASC)
    INCLUDE([ScheduleID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_209842_209841_EmployeeVisits]
    ON [dbo].[EmployeeVisits]([IsDeleted] ASC, [IsPCACompleted] ASC)
    INCLUDE([ScheduleID], [ClockInTime], [ClockOutTime]);

