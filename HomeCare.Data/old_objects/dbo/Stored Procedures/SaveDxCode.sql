-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SaveDxCode]
	-- Add the parameters for the stored procedure here
	@DXCodeID BIGINT,
	@DXCodeName VARCHAR(100),
	@DXCodeWithoutDot VARCHAR(100),
	@DxCodeType varchar(50),
	@Description VARCHAR(500),
	@EffectiveFrom DATE,
	@EffectiveTo DATE,
	@loggedInUserId BIGINT,
	@IsEditMode BIT,
	@SystemID VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If edit mode
	IF(@IsEditMode=0)
	BEGIN
			INSERT INTO DXCodes
			(DXCodeName,Description,DXCodeWithoutDot,DxCodeType,EffectiveFrom,EffectiveTo,UpdatedBy,UpdatedDate,IsDeleted,CreatedBy,CreatedDate,SystemID)
			VALUES
			(@DXCodeName,@Description,@DXCodeWithoutDot,@DxCodeType,@EffectiveFrom,@EffectiveTo,@loggedInUserId,GETUTCDATE(),0,@loggedInUserId,GETUTCDATE(),@SystemID);	
			
	END

	ELSE
	BEGIN
			UPDATE DXCodes 
			SET			   
			   DXCodeName=@DXCodeName,
			   Description=@Description,
			   DXCodeWithoutDot=@DXCodeWithoutDot,
			   DxCodeType=@DxCodeType,
			   EffectiveFrom=@EffectiveFrom,
			   EffectiveTo=@EffectiveTo,
			   UpdatedBy=@loggedInUserId,
			   UpdatedDate=GETUTCDATE(),
			   SystemID=@SystemID
			WHERE DXCodeID=@DXCodeID;
	END
END

