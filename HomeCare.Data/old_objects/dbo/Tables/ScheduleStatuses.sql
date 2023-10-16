CREATE TABLE [dbo].[ScheduleStatuses] (
    [ScheduleStatusID]   BIGINT        NOT NULL,
    [ScheduleStatusName] VARCHAR (100) NULL,
    CONSTRAINT [PK_ScheduleStatuses] PRIMARY KEY CLUSTERED ([ScheduleStatusID] ASC)
);


GO
CREATE TRIGGER [dbo].[tr_ScheduleStatuses_Deleted] ON [dbo].[ScheduleStatuses]
FOR DELETE AS 

INSERT INTO JO_ScheduleStatuses( 
ScheduleStatusID,
ScheduleStatusName,
Action,ActionDate
)

SELECT  
ScheduleStatusID,
ScheduleStatusName,
'D',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_ScheduleStatuses_Deleted]
    ON [dbo].[ScheduleStatuses];


GO
CREATE TRIGGER [dbo].[tr_ScheduleStatuses_Updated] ON [dbo].[ScheduleStatuses]
FOR UPDATE AS 

INSERT INTO JO_ScheduleStatuses( 
ScheduleStatusID,
ScheduleStatusName,
Action,ActionDate
)

SELECT  
ScheduleStatusID,
ScheduleStatusName,
'U',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_ScheduleStatuses_Updated]
    ON [dbo].[ScheduleStatuses];

