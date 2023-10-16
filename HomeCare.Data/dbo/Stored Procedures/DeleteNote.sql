CREATE PROCEDURE [dbo].[DeleteNote]
	@IsBillable int= -1,
	@AHCCCSID varchar(100) = NULL,	
	@CISNumber varchar(50) = NULL,
	--@ServiceCodeID int = 0,		
	@ServiceCodeIDs varchar(MAX),
	@ReferralID bigint =0,
	@ServiceDateStart date=null,
	@ServiceDateEnd date=null,	
	@ServiceCodeTypeID int=-1,
	@NoteKind varchar(30)=null,
	@IsCompleted int=-1,
	@SearchText varchar(50)=null,		
	@IsDeleted BIGINT =-1,
	@AllowEditStatuses varchar(200)=null,
	@BatchID bigint=0,
	@NoteID bigint=0,
	@BillingProviderID bigint=0,
	@RegionID bigint=0,
	@DepartmentID bigint=0,
	--@EmployeeID bigint=0,
	@CreatedByIDs varchar(MAX),	
	@AssigneeID bigint=0,	
	@SortExpression NVARCHAR(100),  
	@SortType NVARCHAR(10),
	@FromIndex INT,
	@PageSize INT,
	@ListOfIdsInCSV VARCHAR(300),
	@IsShowList bit,
	@loggedInID BIGINT
	--@StartDate DATE,
	--@EndDate DATE
	
AS
BEGIN    

	IF(LEN(@ListOfIdsInCSV)>0)
	BEGIN
			
		--IF (1!=1)
		--BEGIN 
		--	SELECT NULL;
		--	RETURN NULL;
		--END
		--ELSE
		--BEGIN
			UPDATE Notes SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as BIGINT) ,UpdatedDate=GETUTCDATE()
			WHERE NoteID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))	
			
			
			-- EXECUTE CURSOR TO RUN AND UPDATE LAST ATTANDACE DATE
				DECLARE @TempNoteID BIGINT
				DECLARE @RefID BIGINT
				DECLARE @CheckRespiteHours BIT=0
				DECLARE @IsAddHrsCall BIT=0
				
				DECLARE CUR CURSOR FOR SELECT CAST(Val AS BIGINT) AS NOTE FROM GETCSVTABLE(@ListOfIdsInCSV) 
				OPEN CUR
				FETCH NEXT FROM CUR INTO @TempNoteID
				WHILE @@FETCH_STATUS = 0 BEGIN

				 SELECT @RefID=ReferralID,@CheckRespiteHours=CheckRespiteHours,
				 @IsAddHrsCall=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END FROM Notes Where NoteID=@TempNoteID
					IF(@CheckRespiteHours=1)
					BEGIN
						EXEC UpdateRespiteHours @TempNoteID, @RefID
					END

                FETCH NEXT FROM CUR INTO @TempNoteID 

					
				END
				CLOSE CUR    
				DEALLOCATE CUR
			 -- CLOSE CURSOR TO RUN AND UPDATE LAST ATTANDACE DATE
			 
			  	
		--END
				
	END

	IF(@IsShowList=1)
	BEGIN
		EXEC GetNoteList @IsBillable, @AHCCCSID,@CISNumber,@ServiceCodeIDs,@ReferralID,@ServiceDateStart,@ServiceDateEnd,
		@ServiceCodeTypeID,@NoteKind,@IsCompleted,@SearchText,@BatchID,@NoteID,@BillingProviderID,@RegionID,@DepartmentID,@CreatedByIDs,@AssigneeID,
		@IsDeleted,@AllowEditStatuses, @SortExpression, @SortType, @FromIndex, @PageSize
	END
END