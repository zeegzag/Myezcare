-- EXEC ValidateServiceCodeForNotes @PayorCsv = '39', @ServiceCodeID = '8', @PosID = '53', @ServiceDate = '2016/12/01'
CREATE PROCEDURE [dbo].[ValidateServiceCodeForNotes]
@PayorCsv varchar(max),
@ServiceDate date=null,
@ServiceCodeID int=0,
@PosID bigint=0
AS 
BEGIN

		BEGIN
			-- EXECUTE CURSOR TO RUN AND UPDATE LAST ATTANDACE DATE
				DECLARE @TempPayorIDs varchar(max);
				DECLARE @TempPayorID BIGINT
				DECLARE CUR CURSOR FOR SELECT PayorID FROM Payors WHERE PayorID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@PayorCsv))
				OPEN CUR
				FETCH NEXT FROM CUR INTO @TempPayorID
				WHILE @@FETCH_STATUS = 0 BEGIN
				
					IF(	( SELECT COUNT(*) FROM PayorServiceCodeMapping PSM WHERE PSM.ServiceCodeID=@ServiceCodeID AND PSM.PosID=@PosID AND
					   (@ServiceDate >= PSM.POSStartDate AND @ServiceDate <= PSM.POSEndDate) AND PSM.PayorID=@TempPayorID  AND PSM.IsDeleted=0)  = 0 )
							BEGIN
							 
							 IF(@TempPayorIDs IS NULL OR LEN(@TempPayorIDs)=0)
							   SET @TempPayorIDs = (SELECT Convert(varchar(20),PayorID) FROM Payors WHERE PayorID=@TempPayorID)
							 ELSE
							   SET @TempPayorIDs=@TempPayorIDs+','+  (SELECT Convert(varchar(20),PayorID) FROM Payors WHERE PayorID=@TempPayorID)
							END
				
					FETCH NEXT FROM CUR INTO @TempPayorID
				END
				CLOSE CUR    
				DEALLOCATE CUR
			 -- CLOSE CURSOR TO RUN AND UPDATE LAST ATTANDACE DATE
		END	
  
  
	SELECT PayorID,PayorName,ShortName FROM Payors WHERE PayorID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@TempPayorIDs))

END