USE [Live_AHSAPPO]
GO

/****** Object:  StoredProcedure [dbo].[DeleteScheduleFromReferralTimeSlotTables]    Script Date: 12/8/2020 11:04:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ali H
-- Create date: 12 Dec 2020
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteScheduleFromReferralTimeSlotTables]
	@ReferralID bigint,
	@StartDate date,
	@EndDate date	
AS
BEGIN
	DECLARE @ReferralTimeSlotMasterID BIGINT; 

	SELECT @ReferralTimeSlotMasterID = ReferralTimeSlotMasterID FROM ReferralTimeSlotMaster
	WHERE StartDate = @StartDate AND EndDate = @EndDate AND ReferralID = @ReferralID

	SELECT @ReferralTimeSlotMasterID

	DELETE FROM ReferralTimeSlotDates WHERE ReferralTimeSlotMasterID = @ReferralTimeSlotMasterID
	DELETE FROM ReferralTimeSlotDetails WHERE ReferralTimeSlotMasterID = @ReferralTimeSlotMasterID
	DELETE FROM ReferralTimeSlotMaster WHERE ReferralTimeSlotMasterID = @ReferralTimeSlotMasterID

	SELECT @ReferralID
END
GO

