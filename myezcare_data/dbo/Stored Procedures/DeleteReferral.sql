CREATE PROCEDURE [dbo].[DeleteReferral]  
 @AssigneeID bigint=0,  
 @ClientName varchar(100) = NULL,   
 @PayorID bigint = 0,  
 @NotifyCaseManagerID int = -1,  
 @ChecklistID INT = -1,  
 @ClinicalReviewID INT = -1,  
 @CaseManagerID int = 0,  
 @ServiceID int = -1,  
 @AgencyID bigint=0,  
 @AgencyLocationID bigint =0,  
 @ReferralStatusID bigint =0,  
 @IsSaveAsDraft int=-1,  
 @AHCCCSID varchar(20)=null,  
 @CISNumber varchar(20)=null,  
 @IsDeleted BIGINT=-1,  
 @SortExpression NVARCHAR(100),    
 @SortType NVARCHAR(10),  
 @FromIndex INT,  
 @PageSize INT,  
 @ListOfIdsInCSV VARCHAR(300),  
 @IsShowList bit,  
 @loggedInID BIGINT,
 @CurrentDateTime DATETIME,
   @DDType_PatientSystemStatus INT = 12     
AS  
BEGIN      
  
 IF(LEN(@ListOfIdsInCSV)>0)  
 BEGIN  
     
  --IF (1!=1)  
  --BEGIN   
  -- SELECT NULL;  
  -- RETURN NULL;  
  --END  
  --ELSE  
  --BEGIN  
   UPDATE Referrals SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as BIGINT) ,UpdatedDate=GETUTCDATE()  
   WHERE ReferralID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))       
  --END  
      

	--Delete Schedules from Today to future
	DECLARE @TempTable TABLE(      
     EmployeeTSDateID BIGINT      
   )      
      
   INSERT INTO @TempTable
   SELECT ReferralTSDateID FROM ReferralTimeSlotDates RTD       
   INNER JOIN (SELECT ReferralID=CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv)) AS T ON T.ReferralID=RTD.ReferralID      
   WHERE RTD.ReferralTSDate >= CONVERT(date, @CurrentDateTime)      

   DECLARE @TableDeleteSch Table(ScheduleID BIGINT, EmployeeVisitID BIGINT, EmployeeVisitNoteID BIGINT, NoteID BIGINT);

   INSERT INTO @TableDeleteSch  
    SELECT S.ScheduleID, EV.EmployeeVisitID, EVN.EmployeeVisitNoteID,EVN.NoteID FROM EmployeeVisitNotes EVN  
   INNER JOIN EmployeeVisits EV ON EV.EmployeeVisitID=EVN.EmployeeVisitID  
   INNER JOIN ScheduleMasters S ON S.ScheduleID=EV.ScheduleID  
   WHERE 1=1 AND  S.EmployeeTSDateID IN (SELECT EmployeeTSDateID FROM @TempTable)
  
   DELETE FROM EmployeeVisitNotes WHERE EmployeeVisitNoteID IN (SELECT EmployeeVisitNoteID FROM @TableDeleteSch)  
   DELETE FROM NoteDXCodeMappings WHERE NoteID IN (SELECT NoteID FROM @TableDeleteSch)  
   DELETE FROM SignatureLogs WHERE NoteID IN (SELECT NoteID FROM @TableDeleteSch)  
  
   DELETE FROM Notes WHERE NoteID IN (SELECT NoteID FROM @TableDeleteSch)  
   DELETE FROM EmployeeVisits WHERE EmployeeVisitID IN (SELECT EmployeeVisitID FROM @TableDeleteSch)

   DELETE SM FROM ScheduleMasters SM
   INNER JOIN Referrals r ON r.ReferralID=sm.ReferralID
	WHERE sm.StartDate >= @CurrentDateTime AND sm.ReferralID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv))

	--UPDATE sm SET sm.IsDeleted=1,sm.UpdatedDate=GETUTCDATE(),sm.UpdatedBy=@loggedInID
 --FROM ScheduleMasters sm      
 --INNER JOIN Referrals r ON r.ReferralID=sm.ReferralID
 --WHERE sm.StartDate >= @CurrentDateTime AND sm.ReferralID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv))  

 END  
  
 IF(@IsShowList=1)  
 BEGIN  
  EXEC GetReferralList @AssigneeID, @ClientName,@PayorID,@NotifyCaseManagerID,@ChecklistID,@ClinicalReviewID,@CaseManagerID,@ServiceID,@AgencyID,@AgencyLocationID,@ReferralStatusID,@IsSaveAsDraft,@AHCCCSID,@CISNumber,@IsDeleted, @SortExpression, @SortType, @FromIndex, @PageSize  ,@DDType_PatientSystemStatus
 END  
END
