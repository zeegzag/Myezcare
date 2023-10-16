CREATE PROCEDURE [dbo].[GetScheduleAggregatorLogsDetails] 
  @ScheduleID BIGINT
AS
BEGIN
  SELECT SDL.ScheduleDataEventProcessLogID,
    SDL.OrganizationID,
    SDL.TransactionID,
    SDL.ScheduleID,
    SDL.Aggregator,
    SDL.[FileName],
    [dbo].[GetOrgDateTime](SDL.CreatedDate) CreatedDate,
    [dbo].[GetOrgDateTime](SDL.UpdatedDate) UpdatedDate,
    SDL.IsSuccess,
    SDL.IsWaitingForResponse,
    SDL.[Messages]
  FROM
    [Admin_Myezcare_Live].[dbo].[ScheduleDataEventProcessLogs] SDL
  WHERE
	SDL.ScheduleID = @ScheduleID
	AND SDL.OrganizationID = [dbo].[GetOrgId]()
  ORDER BY
	SDL.CreatedDate DESC
END
