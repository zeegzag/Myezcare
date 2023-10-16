CREATE TABLE [dbo].[ScheduleStatuses] (
    [ScheduleStatusID]   BIGINT        NOT NULL,
    [ScheduleStatusName] VARCHAR (100) NULL,
    CONSTRAINT [PK_ScheduleStatuses] PRIMARY KEY CLUSTERED ([ScheduleStatusID] ASC)
);


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