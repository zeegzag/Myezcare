USE [Live_AHSAPPO]
GO

/****** Object:  StoredProcedure [dbo].[AddScheduleToReferralTimeSlotMaster]    Script Date: 12/8/2020 11:07:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ali H
-- Create date: 2 Dec 2020
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddScheduleToReferralTimeSlotMaster]
	@ReferralID bigint,
	@StartDate date,
	@EndDate date,
	@CreatedDate datetime,
	@UpdatedDate datetime,
	@UpdatedBy bigint,
	@CreatedBy bigint,
	@SystemID varchar(100),
	@IsEndDateAvailable bit,
	@IsAnyDay bit,
	@ReferralBillingAuthorizationID bigint
AS
BEGIN

	DECLARE @ReferralTimeSlotMasterID BIGINT;  

	INSERT INTO ReferralTimeSlotMaster (ReferralID,StartDate,EndDate,IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsEndDateAvailable,
										ReferralBillingAuthorizationID,IsAnyDay)
	VALUES (@ReferralID,@StartDate,@EndDate,0,@CreatedDate,@CreatedBy,@UpdatedDate,@UpdatedBy,@SystemID,@IsEndDateAvailable,@ReferralBillingAuthorizationID,@IsAnyDay);

	SET @ReferralTimeSlotMasterID=@@IDENTITY 

	SELECT @ReferralTimeSlotMasterID;  

END
GO

