--EXEC DeleteEmployee @SortExpression = 'Name', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50', @IsDeleted = '-1', @IsDepartmentSupervisor = '-1', @ListOfIdsInCsv = '108', @IsShowList = 'True', @loggedInID = '1'            
CREATE PROCEDURE [dbo].[DeleteEmployee]                
 @Name varchar(100)=NULL,                
 @Email varchar(10)=NULL,                
 @DepartmentID bigint=0,                 
 @RoleID BIGINT=0,               
 @IsDepartmentSupervisor INT = -1,                
 @Degree varchar(100)=null,            
 @Address varchar(50)=null,            
 @MobileNumber varchar(10)=null,            
 @CredentialID varchar(50)=null,                
 @IsDeleted int=-1,                
 @SortExpression NVARCHAR(100),                  
 @SortType NVARCHAR(10),                
 @FromIndex INT,                
 @PageSize INT,                
 @ListOfIdsInCsv varchar(300),                
 @IsShowList bit,                 
 @loggedInID BIGINT,      
 @CurrentDateTime DATETIME      
AS                
BEGIN                    
                 
DECLARE @Output TABLE (  
    ScheduleID bigint 
  )

 IF(LEN(@ListOfIdsInCsv)>0)                
 BEGIN                
  --IF EXISTS (SELECT * FROM Referrals WHERE Assignee IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV)))                
  --BEGIN                 
  -- SELECT NULL;                
  -- RETURN NULL;                
  --END                
  --ELSE                
  --BEGIN                
   UPDATE Employees SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as BIGINT) ,UpdatedDate=GETUTCDATE() WHERE EmployeeID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv))          
          
   --Delete Schedules from Today to future    
   DECLARE @TempTable TABLE(          
     EmployeeTSDateID BIGINT          
   )          
          
   INSERT INTO @TempTable    
   SELECT EmployeeTSDateID FROM EmployeeTimeSlotDates ETD           
   INNER JOIN (SELECT EmployeeID=CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv)) AS T ON T.EmployeeID=ETD.EmployeeID          
   WHERE ETD.EmployeeTSDate >= CONVERT(date, @CurrentDateTime)          
    
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
    
    INSERT INTO @Output       
   SELECT ScheduleID FROM ScheduleMasters SM    
   INNER JOIN Employees e ON e.EmployeeID=sm.EmployeeID          
    WHERE sm.StartDate >= @CurrentDateTime AND sm.EmployeeID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv))
   DELETE SM FROM ScheduleMasters SM    
   INNER JOIN Employees e ON e.EmployeeID=sm.EmployeeID          
 WHERE sm.StartDate >= @CurrentDateTime AND sm.EmployeeID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv))    
    
 --UPDATE sm SET sm.IsDeleted=1,sm.UpdatedDate=GETUTCDATE(),sm.UpdatedBy=@loggedInID    
 --FROM ScheduleMasters sm          
 --INNER JOIN Employees e ON e.EmployeeID=sm.EmployeeID          
 --WHERE sm.StartDate >= @CurrentDateTime AND sm.EmployeeID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv))    
  --END    
  
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
  EXEC GetEmployeeList @Name, @Email, @DepartmentID,@RoleID, @IsDepartmentSupervisor,@Degree,@CredentialID,@MobileNumber,@Address,@IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize                
 END   
END  