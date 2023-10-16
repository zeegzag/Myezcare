CREATE PROCEDURE [dbo].[MarkEmployeeVisitAsComplete]          
 @EmployeeVisitID BIGINT=0,            
 @EmployeeIDs NVARCHAR(500) = NULL,            
 @ReferralIDs NVARCHAR(500) = NULL,                  
 @Name NVARCHAR(100) = NULL,                  
 @PatientName NVARCHAR(100) = NULL,                  
 @StartDate DATE = NULL,                                      
 @EndDate DATE = NULL,                
 @StartTime VARCHAR(20)=NULL,                
 @EndTime VARCHAR(20)=NULL,                           
 @IsDeleted int=-1,          
 @ActionTaken int=0,                          
 @SortExpression NVARCHAR(100),                            
 @SortType NVARCHAR(10),                          
 @FromIndex INT,                          
 @PageSize INT,                          
 @ListOfIdsInCsv varchar(300),                          
 @IsShowList bit,                          
 @loggedInID BIGINT,      
 @PayorIDs NVARCHAR(500) = NULL,         
 @CareTypeIDs NVARCHAR(500) = NULL,        
 @ServiceTypeID int=0,  
 @IsPCACompleted int = 0  -- Added by Satyaprakash on 13 Dec 2019  
as          
          
begin          
          
 IF(LEN(@ListOfIdsInCsv)>0)                            
 BEGIN                              
   UPDATE EmployeeVisits SET IsPCACompleted = 1,UpdatedBy=CAST(@loggedInID as bigint) ,UpdatedDate=GETUTCDATE() WHERE EmployeeVisitID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv));                           
            
   UPDATE sm SET    
   sm.PayorID = p.PayorID,    
   sm.UpdatedBy=CAST(@loggedInID as bigint),    
   sm.UpdatedDate=GETUTCDATE()        
   from ScheduleMasters sm     
 inner join EmployeeVisits ev on sm.ScheduleID=ev.ScheduleID     
 inner join Referrals r on r.ReferralID=sm.ReferralID    
 inner join ReferralPayorMappings rpm on rpm.ReferralID=r.ReferralID    
 inner join Payors p on p.PayorID=rpm.PayorID     
 where ev.EmployeeVisitID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv))    
 and sm.PayorID is null;          
       
       
       DECLARE @CurScheduleID bigint;
 	DECLARE eventCursor CURSOR FORWARD_ONLY FOR
            SELECT ScheduleID FROM EmployeeVisits where EmployeeVisitID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv));
        OPEN eventCursor;
        FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        WHILE @@FETCH_STATUS = 0 BEGIN
            EXEC [dbo].[ScheduleEventBroadcast] 'CreateSchedule', @CurScheduleID,'',''
            FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        END;
        CLOSE eventCursor;
        DEALLOCATE eventCursor;         
  END               
          
        
   IF(@IsShowList=1)                          
 BEGIN                          
  EXEC GetEmployeeVisitList @EmployeeVisitID,@EmployeeIDs,@ReferralIDs,@StartDate,@EndDate,@StartTime,@EndTime,@IsDeleted,@ActionTaken,@SortExpression, @SortType, @FromIndex, @PageSize,@PayorIDs,@CareTypeIDs,@ServiceTypeID                         
 END            
          
end  