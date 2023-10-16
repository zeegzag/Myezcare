--Exec DeleteRtsMaster @ListOfIdsInCsv =1, @SortExpression = 'Name', @SortType = 'DESC', @FromIndex = '1', @PageSize = '100', @IsShowList=1,@loggedInID=1      
CREATE PROCEDURE [dbo].[DeleteRtsMaster]    
 @ReferralID BIGINT = 0,         
 @Filter VARCHAR(10) = NULL,       
 @StartDate DATE = NULL,              
 @EndDate DATE = NULL,              
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
              
 IF(LEN(@ListOfIdsInCsv)>0)              
 BEGIN                
    
 DECLARE @TempTable AS TABLE(    
  ReferralTimeSlotMasterID BIGINT    
 )    
    
 INSERT INTO @TempTable SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv)    
     
 DECLARE @DateTempTable TABLE( ReferralTSDateID BIGINT )         
    
   INSERT INTO @DateTempTable    
   SELECT ReferralTSDateID FROM @TempTable T     
   INNER JOIN ReferralTimeSlotMaster EM ON EM.ReferralTimeSlotMasterID=T.ReferralTimeSlotMasterID    
 INNER JOIN ReferralTimeSlotDetails ED ON ED.ReferralTimeSlotMasterID=EM.ReferralTimeSlotMasterID               
    INNER JOIN ReferralTimeSlotDates ETD   ON ETD.ReferralTimeSlotDetailID = ED.ReferralTimeSlotDetailID    
    
 DECLARE @TableDeleteSch Table(ScheduleID BIGINT, StartDate DATE, EmployeeVisitID BIGINT);              
                 
   INSERT INTO @TableDeleteSch              
   SELECT S.ScheduleID, CONVERT(DATE, S.StartDate), ISNULL(EV.EmployeeVisitID,0) FROM ScheduleMasters S        
   LEFT JOIN EmployeeVisits EV ON EV.IsDeleted = @IsDeleted AND EV.ScheduleID = S.ScheduleID    
   INNER JOIN @DateTempTable T on T.ReferralTSDateID = S.ReferralTSDateID        
    
 IF EXISTS (SELECT 1 FROM @TableDeleteSch WHERE EmployeeVisitID > 0)    
 BEGIN    
  SELECT -3 AS TransactionResultId;    
  SELECT NULL;    
  RETURN;         
 END    
    
 UPDATE EM SET  EM.IsDeleted= @IsDeleted, EndDate = GETDATE(), IsEndDateAvailable = 1 FROM @TempTable T    
 INNER JOIN ReferralTimeSlotMaster EM ON EM.ReferralTimeSlotMasterID=T.ReferralTimeSlotMasterID    
     
    
     
 UPDATE ED SET  ED.IsDeleted= @IsDeleted FROM @TempTable T    
 INNER JOIN ReferralTimeSlotMaster EM ON EM.ReferralTimeSlotMasterID=T.ReferralTimeSlotMasterID    
 INNER JOIN ReferralTimeSlotDetails ED ON ED.ReferralTimeSlotMasterID=EM.ReferralTimeSlotMasterID    
    
                
       
    
 DECLARE @Output TABLE ( ScheduleID bigint  )      
    
 INSERT INTO @Output               
 SELECT ScheduleID FROM  ScheduleMasters (NOLOCK) WHERE ScheduleID IN (SELECT DISTINCT ScheduleID FROM @TableDeleteSch)          
        
    
 DECLARE @TodayDate date = (SELECT [dbo].[GetOrgCurrentDateTime]())       
    
 UPDATE ScheduleMasters SET IsDeleted = 0 WHERE ScheduleID IN (SELECT DISTINCT ScheduleID FROM @TableDeleteSch WHERE @TodayDate >= StartDate)              
    
 DELETE  FROM ScheduleMasters WHERE ScheduleID IN (SELECT DISTINCT ScheduleID FROM @TableDeleteSch WHERE @TodayDate < StartDate)      
                 
              
 DELETE FROM ReferralTimeSlotDates WHERE ReferralTSDateID IN (SELECT ReferralTSDateID FROM @DateTempTable)      
               
                
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
    
 SELECT 1 AS TransactionResultId;         
 IF(@IsShowList=1)              
 BEGIN              
  EXEC GetReferralTimeSlot @ReferralID,@Filter,@StartDate,@EndDate,@IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize              
 END         
 ELSE     
 BEGIN    
 SELECT NULL;    
 END    
END  