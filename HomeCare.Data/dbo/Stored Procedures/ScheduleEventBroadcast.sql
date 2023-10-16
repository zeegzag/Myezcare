CREATE PROCEDURE [dbo].[ScheduleEventBroadcast]
  @EventName nvarchar(max),
  @ScheduleID bigint,
  @ReasonCode nvarchar(max),
  @ActionCode nvarchar(max),
  @ForceInsert bit = 0
AS
BEGIN

  BEGIN TRY
    -- statements that may cause exceptions
    DECLARE @AdminDatabaseName nvarchar(max);
    DECLARE @HasAggregator bit;
    SELECT
      @AdminDatabaseName = AdminDatabaseName,
      @HasAggregator = HasAggregator
    FROM OrganizationSettings

    IF (@HasAggregator = 1)
    BEGIN
      DECLARE @OrganizationID bigint;
      DECLARE @Query nvarchar(max)
      SET @Query = 'SELECT TOP 1 @OrganizationID = OrganizationID FROM ' + @AdminDatabaseName + '.dbo.Organizations WHERE IsDeleted = 0 AND IsActive = 1 AND DBName =''' + DB_NAME() + ''''
      EXEC sp_executesql @Query,
                         N'@OrganizationID BIGINT OUTPUT',
                         @OrganizationID = @OrganizationID OUTPUT;

      IF (@OrganizationID > 0)
      BEGIN

        SET @Query = 'EXEC ' + @AdminDatabaseName + '.dbo.MergeScheduleEventBroadcast @OrganizationID, @EventName, @ScheduleID, @ReasonCode, @ActionCode, @ForceInsert'
        EXEC sp_executesql @Query,
                           N'@OrganizationID bigint,
      				         @EventName nvarchar(max),
						     @ScheduleID bigint,
						     @ReasonCode nvarchar(max),
						     @ActionCode nvarchar(max),
                             @ForceInsert bit',
                           @OrganizationID = @OrganizationID,
                           @EventName = @EventName,
                           @ScheduleID = @ScheduleID,
                           @ReasonCode = @ReasonCode,
                           @ActionCode = @ActionCode,
                           @ForceInsert = @ForceInsert;
      END

    END
  END TRY
  BEGIN CATCH
  -- do nothing
  -- statements that handle exception
  --SELECT  
  --  ERROR_NUMBER() AS ErrorNumber  
  --  ,ERROR_SEVERITY() AS ErrorSeverity  
  --  ,ERROR_STATE() AS ErrorState  
  --  ,ERROR_PROCEDURE() AS ErrorProcedure  
  --  ,ERROR_LINE() AS ErrorLine  
  --  ,ERROR_MESSAGE() AS ErrorMessage; 
  END CATCH
END