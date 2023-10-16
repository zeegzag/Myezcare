USE [Live_AHSAPPO]
GO

/****** Object:  StoredProcedure [dbo].[AddScheduleToReferralTimeSlotDetails]    Script Date: 12/8/2020 11:07:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddScheduleToReferralTimeSlotDetails] 
	@ReferralTimeSlotMasterID bigint,
	@Day int,
	@StartTime time,
	@EndTime time,
	@CreatedDate datetime,
	@UpdatedDate datetime,
	@UpdatedBy bigint,
	@CreatedBy bigint,
	@SystemID varchar(100),
	@Notes varchar(100),
	@UsedInScheduling bit,
	@CareTypeId bigint,
	@AnyTimeClockIn bit
AS
BEGIN

	DECLARE @ReferralTimeSlotDetailID BIGINT;  

	INSERT INTO ReferralTimeSlotDetails (ReferralTimeSlotMasterID,ReferralTimeSlotMaster.Day,StartTime,EndTime,IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,
	                                    SystemID,Notes,UsedInScheduling,CareTypeId,AnyTimeClockIn)

	VALUES (@ReferralTimeSlotMasterID,@Day,@StartTime,@EndTime,0,@CreatedDate,@CreatedBy,@UpdatedDate,@UpdatedBy,@SystemID,@Notes,@UsedInScheduling,@CareTypeId,
	       @AnyTimeClockIn);

	SET @ReferralTimeSlotDetailID=@@IDENTITY 

	SELECT @ReferralTimeSlotDetailID;  

END
GO

