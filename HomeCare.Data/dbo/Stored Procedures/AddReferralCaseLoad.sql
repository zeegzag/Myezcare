-- EXEC AddReferralCaseLoad 0,24254,2,'Permanent','Permanent',NULL,NULL,1,':::1'
-- EXEC AddReferralCaseLoad @ReferralCaseloadID = '0', @ReferralID = '24254', @EmployeeID = '4', @CaseLoadType = 'Permanent', @PermanentCaseLoadType = 'Permanent', @StartDate = '0001/01/01', @EndDate = '', @loggedInUserID = '1', @SystemID = '::1'
-- EXEC AddReferralCaseLoad @ReferralCaseloadID = '0', @ReferralID = '24254', @EmployeeID = '6', @CaseLoadType = 'Permanent', @PermanentCaseLoadType = 'Permanent', @StartDate = '0001/01/01', @EndDate = '', @loggedInUserID = '1', @SystemID = '::1'
CREATE PROCEDURE [dbo].[AddReferralCaseLoad]
	@ReferralCaseloadID BIGINT,
	@ReferralID BIGINT,
	@EmployeeID BIGINT,
	@CaseLoadType VARCHAR(100),
	@PermanentCaseLoadType VARCHAR(100) = 'Permanent',
	@StartDate DATE=NULL,
	@EndDate DATE=NULL,        
	@loggedInUserId BIGINT,              
	@SystemID VARCHAR(100)              
AS                        
BEGIN	                   
	BEGIN TRANSACTION trans
	BEGIN TRY
		
		DECLARE @IsEditMode INT = CASE WHEN @ReferralCaseloadID > 0 THEN 1 ELSE 0 END
		DECLARE @InsertedEmployeeID BIGINT = 0
		DECLARE @TransactionResultId INT
		DECLARE @SaveCaseLoad INT = 0
		
		IF(@CaseLoadType = @PermanentCaseLoadType)
		BEGIN		
			IF EXISTS(SELECT 1 FROM ReferralCaseLoads WHERE ReferralID = @ReferralID AND CaseLoadType = @PermanentCaseLoadType)
			BEGIN
				SELECT
					@ReferralCaseloadID = ReferralCaseloadID,
					@InsertedEmployeeID = EmployeeID
				FROM
					ReferralCaseLoads
				WHERE
					ReferralID = @ReferralID
					AND CaseLoadType = @PermanentCaseLoadType
					AND EndDate IS NULL
					
				IF(@InsertedEmployeeID = @EmployeeID)
				BEGIN
					SET @TransactionResultId = -2
				END
				ELSE
				BEGIN
					-- Mark the last case load as End
					UPDATE
						ReferralCaseLoads
					SET
						EndDate = GETDATE(),
						UpdatedBy = @loggedInUserId,
						UpdatedDate = GETDATE()
					WHERE
						ReferralCaseloadID = @ReferralCaseloadID
					
					SET @SaveCaseLoad = 1
				END
			END
			ELSE
			BEGIN
				SET @SaveCaseLoad = 1
			END
		END
		ELSE
		BEGIN
			IF EXISTS
			(
				-- Check temporary case load alreay exists between date range
				SELECT
					1
				FROM
					ReferralCaseLoads
				WHERE
					ReferralID = @ReferralID
					AND ReferralCaseLoadID != @ReferralCaseloadID
					AND CaseLoadType != @PermanentCaseLoadType
					AND (StartDate <= @EndDate AND EndDate >= @StartDate)
			)
			BEGIN
				SET @TransactionResultId = -3
			END
			ELSE
			BEGIN
				SET @SaveCaseLoad = 1
			END
		END
		
		IF(@SaveCaseLoad = 1)
		BEGIN
			IF(@IsEditMode = 1)
			BEGIN
				UPDATE
					ReferralCaseLoads
				SET
					StartDate = CASE WHEN @CaseLoadType = @PermanentCaseLoadType THEN GETDATE() ELSE @StartDate END,
					EndDate = CASE WHEN @CaseLoadType = @PermanentCaseLoadType THEN NULL ELSE @EndDate END
				WHERE
					ReferralCaseloadID = @ReferralCaseloadID
			END
			ELSE
			BEGIN
				INSERT INTO ReferralCaseLoads
				(
					ReferralID,
					EmployeeID,
					CaseLoadType,
					StartDate,
					EndDate,
					CreatedBy,
					CreatedDate,
					UpdatedDate,
					UpdatedBy,
					SystemID
				)
				SELECT
					@ReferralID,
					@EmployeeID,
					@CaseLoadType,
					CASE WHEN @CaseLoadType = @PermanentCaseLoadType THEN GETDATE() ELSE @StartDate END,
					CASE WHEN @CaseLoadType = @PermanentCaseLoadType THEN NULL ELSE @EndDate END,
					@loggedInUserId,
					GETDATE(),
					GETDATE(),
					@loggedInUserId,
					@SystemID
					
				SET @ReferralCaseloadID = @@IDENTITY;
			END				
			
			SET @TransactionResultId = 1;
		END
		
		SELECT
			@TransactionResultId AS TransactionResultId,
			@ReferralCaseloadID AS TablePrimaryId,
			'' AS ErrorMessage
	
		IF @@TRANCOUNT > 0                    
		BEGIN                     
			COMMIT TRANSACTION trans                     
		END
		
    END TRY
    BEGIN CATCH
    
		SELECT 
			-1 AS TransactionResultId,
			ERROR_MESSAGE() AS ErrorMessage;
			
		IF @@TRANCOUNT > 0                    
		BEGIN                     
			ROLLBACK TRANSACTION trans                     
		END
		
	END CATCH               
END