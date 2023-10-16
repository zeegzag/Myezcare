
-- =============================================
-- Author:		Ashar A
-- Create date: 27 Oct 2021
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[API_GetGroupVisitTask]
	@VisitTaskType NVARCHAR(30)
	,@CareType BIGINT
AS
BEGIN
	IF (@VisitTaskType = 'Task')
	BEGIN
		SELECT v.VisitTaskID
			,v.VisitTaskDetail
		FROM VisitTasks v
		WHERE v.VisitTaskType = @VisitTaskType
			AND v.caretype = @CareType
			AND v.IsDeleted = 0
		ORDER BY v.VisitTaskDetail ASC
	END
	ELSE
	BEGIN
		SELECT v.VisitTaskID
			,v.VisitTaskDetail
		FROM VisitTasks v
		WHERE v.VisitTaskType = @VisitTaskType
		--and v.caretype=@CareType
			AND v.IsDeleted = 0
		ORDER BY v.VisitTaskDetail ASC
	END
END