BEGIN TRANSACTION

-- Use DBName and corresponding website Web URL's base.
DECLARE @DBName NVARCHAR(50) = N'Kundan_Admin';

DECLARE @jobName NVARCHAR(100) = N'ProcessNotificationsJob-' + @DBName;
DECLARE @cmd NVARCHAR(1000) = N'DECLARE @JobID UNIQUEIDENTIFIER = CONVERT(UNIQUEIDENTIFIER, $(ESCAPE_NONE(JOBID)));
EXEC [notif].[ProcessOrganizationNotifications] @JobID;';
DECLARE @ReturnCode INT = 0
DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=@jobName, 
		@enabled=1, 
		@job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, 
		@step_name=N'ProcessNotifications', 
		@subsystem=N'TSQL', 
		@command=@cmd, 
		@database_name=@DBName
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, 
		@name=N'Every Day - Every 5 minutes', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=4, 
		@freq_subday_interval=5
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, 
		@server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO