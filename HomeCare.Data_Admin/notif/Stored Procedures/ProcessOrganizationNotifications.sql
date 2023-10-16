CREATE PROCEDURE [notif].[ProcessOrganizationNotifications]
	@JobID UNIQUEIDENTIFIER
AS
BEGIN

	-- Declare required fields to fetch for creating SQL jobs.
	DECLARE @OrganizationID BIGINT, @DBName NVARCHAR(MAX), @BaseURL NVARCHAR(MAX), @OrgSPName NVARCHAR(MAX);

	-- Declare cursor to fetch fields to create job for processing notifications.
	DECLARE CursorJobConfig CURSOR
	FOR SELECT
			[OrganizationID],
			[DBName], 
			'https://' + [DomainName] + '.myezcare.com'
		FROM 
			[dbo].[Organizations]
		WHERE
			[IsActive] = 1
			--AND [OrganizationID] IN (30023)
            AND [DomainName] <> 'localhost'
			AND [IsDeleted] = 0
			AND [DBServer] = 'production';

	-- Open cursor to fetch data.
	OPEN CursorJobConfig;

	-- Fetch fields.
	FETCH NEXT FROM CursorJobConfig INTO 
		@OrganizationID,
		@DBName, 
		@BaseURL;

    DECLARE @FailedOrgs NVARCHAR(MAX) = NULL
	DECLARE @Message NVARCHAR(MAX)

	-- Loop the cursor.
	WHILE @@FETCH_STATUS = 0
		BEGIN

            BEGIN TRY  
                SELECT @OrgSPName = '[' + @DBName + '].[notif].[ProcessNotifications]';
			    IF (OBJECT_ID(@OrgSPName) IS NOT NULL)
				    BEGIN
					    PRINT '==== ORG ID: ' + CAST(@OrganizationID AS VARCHAR) + ', DB: ' + @DBName + ' ====';
					    DECLARE @cmd NVARCHAR(MAX) = N'EXEC ' + @OrgSPName + ' @JobID, @BaseURL;';
					    EXEC sp_executesql @cmd, 
										    N'@JobID UNIQUEIDENTIFIER, 
										        @BaseURL NVARCHAR(MAX)', 
										    @JobID = @JobID,
										    @BaseURL = @BaseURL;
				    END
            END TRY  
            BEGIN CATCH  
			     SELECT @Message =  ERROR_MESSAGE()
                 SELECT @FailedOrgs = ISNULL(@FailedOrgs + ', ', '') + 'ORG ID: ' + CAST(@OrganizationID AS VARCHAR) + ' & DB Name: ' + @DBName + ' & Message: ' + @Message
            END CATCH  

			-- Fetch fields.
			FETCH NEXT FROM CursorJobConfig INTO 
				@OrganizationID,
				@DBName, 
				@BaseURL;
		END;

    IF (@FailedOrgs IS NOT NULL)
        BEGIN
            RAISERROR (N'Failed for some Orgs! Details: %s.', 11, 1, @FailedOrgs);
        END
	
	-- Close cursor.
	CLOSE CursorJobConfig;

	-- Deallocate curser.
	DEALLOCATE CursorJobConfig;

END