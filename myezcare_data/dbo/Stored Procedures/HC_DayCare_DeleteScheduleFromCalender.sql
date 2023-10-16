CREATE PROCEDURE [dbo].[HC_DayCare_DeleteScheduleFromCalender]
  @ListOfIdsInCSV VARCHAR(300),
  @loggedInId BIGINT
AS
BEGIN    

	IF(LEN(@ListOfIdsInCSV)>0)
	BEGIN
			
		UPDATE ScheduleMasters SET IsDeleted=1, UpdatedBy=@loggedInId, UpdatedDate=GETUTCDATE()
		WHERE ScheduleID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))

		UPDATE Notes SET IsDeleted=1, UpdatedBy=@loggedInId, UpdatedDate=GETUTCDATE()
		WHERE ScheduleID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))
				
	END

	
END
