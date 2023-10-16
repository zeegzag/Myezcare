USE [Live_AHSAPPO]
GO

/****** Object:  StoredProcedure [dbo].[AddScheduleToReferralTimeSlotDates]    Script Date: 12/8/2020 11:06:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ali H
-- Create date: 3 Dec 2020
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddScheduleToReferralTimeSlotDates]
	@ReferralID bigint,
	@ReferralTimeSlotMasterID bigint,
	@ReferralTSDate date,
	@ReferralTSStartTime datetime,
	@ReferralTSEndTime datetime,
	@UsedInScheduling bit,
	@Notes varchar(100),
	@DayNumber int,
	@ReferralTimeSlotDetailID bigint,
	@OnHold bit,
	@IsDenied bit
AS
BEGIN

		DECLARE @ReferralTSDateID BIGINT;  

		INSERT INTO ReferralTimeSlotDates (ReferralID,ReferralTimeSlotMasterID,ReferralTSDate,ReferralTSStartTime,ReferralTSEndTime,UsedInScheduling,Notes,DayNumber,
											ReferralTimeSlotDetailID,OnHold,IsDenied)
		VALUES (@ReferralID,@ReferralTimeSlotMasterID,@ReferralTSDate,@ReferralTSStartTime,@ReferralTSEndTime,@UsedInScheduling,@Notes,@DayNumber,
					@ReferralTimeSlotDetailID,@OnHold,@IsDenied);

		SET @ReferralTSDateID=@@IDENTITY 

		SELECT @ReferralTSDateID;  

END
GO

