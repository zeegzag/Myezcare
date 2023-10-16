USE [Live_AHSAPPO]
GO

/****** Object:  StoredProcedure [dbo].[GetScheduleMasterById]    Script Date: 12/8/2020 11:03:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ali H
-- Create date: 12 Dec 2020
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetScheduleMasterById]
	@ScheduleID bigint
AS
BEGIN
	SELECT *FROM ScheduleMasters WHERE ScheduleID = @ScheduleID 
END
GO

