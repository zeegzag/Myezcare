-- EXEC DeleteEtsDetail @SortExpression = 'EmployeeTimeSlotMasterID', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50', @EmployeeTimeSlotMasterID = '23', @ListOfIdsInCsv = '18', @IsShowList = 'FALSE', @loggedInID = '1'          
CREATE PROCEDURE [dbo].[DeleteEtsDetail]    
 @EmployeeTimeSlotMasterID BIGINT = 0,                    
 @StartTime VARCHAR(20) = NULL,                    
 @EndTime VARCHAR(20) = NULL,                   
 @IsDeleted int=-1,                    
 @SortExpression NVARCHAR(100),                      
 @SortType NVARCHAR(10),                    
 @FromIndex INT,                    
 @PageSize INT,                    
 @ListOfIdsInCsv varchar(300),                    
 @IsShowList bit,                    
 @loggedInID BIGINT                    
AS                    
BEGIN                    
       
 DECLARE @Output TABLE (  
    ScheduleID bigint
  )  
               
 IF(LEN(@ListOfIdsInCsv)>0)                    
 BEGIN                      
   UPDATE EmployeeTimeSlotDetails SET IsDeleted= 1 ,UpdatedBy=CAST(@loggedInID as bigint) ,UpdatedDate=GETUTCDATE() WHERE EmployeeTimeSlotDetailID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv))          
             
             
          
   DECLARE @TempTable TABLE(          
     EmployeeTSDateID BIGINT          
   )          
          
          
          
   INSERT INTO @TempTable          
   SELECT EmployeeTSDateID FROM EmployeeTimeSlotDates ETD           
   --INNER JOIN EmployeeTimeSlotDetails ETSD ON ETSD.EmployeeTimeSlotDetailID=ETD.EmployeeTimeSlotDetailID          
   INNER JOIN (SELECT EmployeeTimeSlotDetailID=CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv)) AS T ON T.EmployeeTimeSlotDetailID=ETD.EmployeeTimeSlotDetailID          
   WHERE ETD.EmployeeTSDate >= CONVERT(date, GETDATE())          
          
   -- DELETE FROM ScheduleMasters WHERE EmployeeTSDateID IN (SELECT EmployeeTSDateID FROM @TempTable) --ORDER BY StartDate ASC          
          
      
 DECLARE @TableDeleteSch Table(ScheduleID BIGINT, EmployeeVisitID BIGINT, EmployeeVisitNoteID BIGINT, NoteID BIGINT);      
         
   INSERT INTO @TableDeleteSch      
    SELECT S.ScheduleID, EV.EmployeeVisitID, EVN.EmployeeVisitNoteID,EVN.NoteID FROM ScheduleMasters S    
   LEFT JOIN EmployeeVisits EV ON EV.ScheduleID=S.ScheduleID    
   LEFT JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID=EV.EmployeeVisitID    
   WHERE 1=1 AND  S.EmployeeTSDateID IN (SELECT EmployeeTSDateID FROM @TempTable)          
      
   --SELECT * FROM @TableDeleteSch      
      
   DELETE FROM EmployeeVisitNotes WHERE EmployeeVisitNoteID IN (SELECT EmployeeVisitNoteID FROM @TableDeleteSch)      
   DELETE FROM NoteDXCodeMappings WHERE NoteID IN (SELECT NoteID FROM @TableDeleteSch)      
   DELETE FROM SignatureLogs WHERE NoteID IN (SELECT NoteID FROM @TableDeleteSch)      
      
   DELETE FROM Notes WHERE NoteID IN (SELECT NoteID FROM @TableDeleteSch)      
   DELETE FROM EmployeeVisits WHERE EmployeeVisitID IN (SELECT EmployeeVisitID FROM @TableDeleteSch)  
   
   INSERT INTO @Output       
   SELECT ScheduleID FROM ScheduleMasters WHERE EmployeeTSDateID IN (SELECT EmployeeTSDateID FROM @TempTable)     
   DELETE FROM ScheduleMasters WHERE ScheduleID IN (SELECT ScheduleID FROM @TableDeleteSch)      
        
      
   -- UPDATE ScheduleMasters SET IsDeleted=1,UpdatedBy=@loggedInID,UpdatedDate=GETUTCDATE() WHERE EmployeeTSDateID IN (SELECT EmployeeTSDateID FROM @TempTable)          
          
   DELETE FROM EmployeeTimeSlotDates WHERE EmployeeTSDateID IN (SELECT EmployeeTSDateID FROM @TempTable)          
          
        DECLARE @CurScheduleID bigint;                         
      DECLARE eventCursor CURSOR FORWARD_ONLY FOR
            SELECT ScheduleID FROM @Output;
        OPEN eventCursor;
        FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        WHILE @@FETCH_STATUS = 0 BEGIN
            EXEC [dbo].[ScheduleEventBroadcast] 'DeleteSchedule', @CurScheduleID,'',''
            FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        END;
        CLOSE eventCursor;
        DEALLOCATE eventCursor;   
             
                            
  END                    
                    
 IF(@IsShowList=1)                    
 BEGIN                    
  EXEC GetEtsDetailList @EmployeeTimeSlotMasterID,@StartTime,@EndTime,@IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize                    
 END                    
END  