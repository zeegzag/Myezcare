CREATE PROCEDURE [dbo].[DeleteSchedule]    
@ScheduleID bigint=0,    
 @StartDate varchar(20)=null,    
 @EndDate varchar(20)=null,    
 @ScheduleStatusID int = 0,    
 @ConfirmationScheduleStatusID int = 2,    
 @Name varchar(50)=null,    
 @ParentName varchar(50)=null,    
 @PickUpLocation bigint =0,    
 @DropOffLocation bigint=0,      
 @FacilityID bigint=0,    
 @RegionID bigint=0,    
 @ReferralID bigint=0,    
 @SortExpression VARCHAR(100),      
 @SortType VARCHAR(10),    
 @FromIndex INT,    
 @PageSize INT,    
 @ListOfIdsInCSV VARCHAR(300),    
 @IsShowList bit    
AS    
BEGIN        
    
 IF(LEN(@ListOfIdsInCSV)>0)    
 BEGIN    
       
  IF (1!=1)    
  BEGIN     
   SELECT NULL;    
   RETURN NULL;    
  END    
  ELSE    
  BEGIN    
    UPDATE ScheduleMasters SET IsDeleted=1    
    WHERE ScheduleID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))     
    
    -- EXECUTE CURSOR TO RUN AND UPDATE LAST ATTANDACE DATE    
    DECLARE @TempReferralID BIGINT, @TempScheduleID BIGINT    
    DECLARE CUR CURSOR FOR SELECT ReferralID,ScheduleID FROM ScheduleMasters WHERE ScheduleID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))    
    OPEN CUR    
    FETCH NEXT FROM CUR INTO @TempReferralID , @TempScheduleID   
    WHILE @@FETCH_STATUS = 0 BEGIN    
     EXEC UpdateReferralLastAttDate @TempReferralID, @ConfirmationScheduleStatusID   
     EXEC [dbo].[ScheduleEventBroadcast] 'DeleteSchedule', @TempScheduleID,'','' 
     FETCH NEXT FROM CUR INTO @TempReferralID , @TempScheduleID  
    END    
    CLOSE CUR        
    DEALLOCATE CUR    
    -- CLOSE CURSOR TO RUN AND UPDATE LAST ATTANDACE DATE    
        
  END    
        
 END    
    
 IF(@IsShowList=1)    
 BEGIN    
  EXEC GetScheduleMaster @ScheduleID,@StartDate,@EndDate,@ScheduleStatusID,@Name,@ParentName,@PickUpLocation,@DropOffLocation,@FacilityID,@RegionID,@ReferralID,@SortExpression,@SortType,@FromIndex,@PageSize    
 END    
END