--Exec DeleteRtsMaster @ListOfIdsInCsv =1, @SortExpression = 'Name', @SortType = 'DESC', @FromIndex = '1', @PageSize = '100', @IsShowList=1,@loggedInID=1              
CREATE PROCEDURE [dbo].[DeleteRtsDetail]            
 @ReferralTimeSlotMasterID BIGINT = 0,                      
 @StartTime VARCHAR(20) = NULL,                      
 @EndTime VARCHAR(20) = NULL,                     
 @IsDeleted int=-1,                      
 @SortExpression NVARCHAR(100),                        
 @SortType NVARCHAR(10),                      
 @FromIndex INT,                      
 @PageSize INT,                      
 @ListOfIdsInCsv varchar(300),                      
 @IsShowList bit,                      
 @loggedInID BIGINT,  
 @CareTypeID BIGINT = 0              
AS                      
BEGIN                      
        
        
    DECLARE @Output TABLE (      
    ScheduleID bigint     
  )      
                    
 IF(LEN(@ListOfIdsInCsv)>0)                      
 BEGIN                        
   UPDATE ReferralTimeSlotDetails SET IsDeleted= 1 ,UpdatedBy=CAST(@loggedInID as bigint) ,UpdatedDate=GETUTCDATE()     
   WHERE ReferralTimeSlotDetailID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv)            
)                  
            
            
            
  DECLARE @TempTable TABLE(            
     ReferralTSDateID BIGINT            
   )            
            
   DECLARE @TodayDate date = (SELECT [dbo].[GetOrgCurrentDateTime]())        
            
   INSERT INTO @TempTable            
   SELECT ReferralTSDateID FROM ReferralTimeSlotDates ETD   (NOLOCK)          
   --INNER JOIN EmployeeTimeSlotDetails ETSD ON ETSD.EmployeeTimeSlotDetailID=ETD.EmployeeTimeSlotDetailID            
   INNER JOIN (SELECT ReferralTimeSlotDetailID=CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv)) AS T ON T.ReferralTimeSlotDetailID=ETD.ReferralTimeSlotDetailID            
   WHERE  ETD.ReferralTSDate > @TodayDate          
            
            
   DECLARE @TableDeleteSch Table(ScheduleID BIGINT, EmployeeVisitID BIGINT, EmployeeVisitNoteID BIGINT, NoteID BIGINT);          
             
   INSERT INTO @TableDeleteSch          
   SELECT S.ScheduleID, EV.EmployeeVisitID, EVN.EmployeeVisitNoteID,EVN.NoteID FROM ScheduleMasters S        
   --INNER JOIN EmployeeVisitNotes EVN ON EVN.NoteID=N.NoteID          
   LEFT JOIN EmployeeVisits EV ON EV.ScheduleID=S.ScheduleID        
   LEFT JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID=EV.EmployeeVisitID        
       
   INNER JOIN @TempTable T on T.ReferralTSDateID = S.ReferralTSDateID    
    
   -- Replaced with the innerjoin above -- Pallav :)    
   --WHERE S.ReferralTSDateID IN (SELECT ReferralTSDateID FROM @TempTable)           
             
    /*    
 Commented by Pallav = We will not have clock-in clock-outs in future    
    
   DELETE FROM EmployeeVisitNotes WHERE EmployeeVisitNoteID IN (SELECT DISTINCT EmployeeVisitNoteID FROM @TableDeleteSch)          
   DELETE FROM NoteDXCodeMappings WHERE NoteID IN (SELECT NoteID FROM @TableDeleteSch)          
   DELETE FROM SignatureLogs WHERE NoteID IN (SELECT NoteID FROM @TableDeleteSch)          
   DELETE FROM Notes WHERE NoteID IN (SELECT DISTINCT NoteID FROM @TableDeleteSch)          
   DELETE FROM EmployeeVisits WHERE EmployeeVisitID IN (SELECT DISTINCT EmployeeVisitID FROM @TableDeleteSch)     
    
   */    
       
   INSERT INTO @Output           
   SELECT ScheduleID FROM  ScheduleMasters (NOLOCK) WHERE ScheduleID IN (SELECT DISTINCT ScheduleID FROM @TableDeleteSch)      
    
   UPDATE ScheduleMasters
      SET IsDeleted = 1 WHERE ScheduleID IN (SELECT DISTINCT ScheduleID FROM @TableDeleteSch)          
            
          
   --DELETE FROM ScheduleMasters WHERE ReferralTSDateID IN (SELECT ReferralTSDateID FROM @TempTable) --ORDER BY StartDate ASC            
   DELETE FROM ReferralTimeSlotDates WHERE ReferralTSDateID IN (SELECT ReferralTSDateID FROM @TempTable)            
            
            
            
            
            
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
  EXEC GetRtsDetailList @ReferralTimeSlotMasterID,@StartTime,@EndTime,@IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize, @CareTypeID                      
 END                      
END