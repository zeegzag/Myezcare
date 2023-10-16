CREATE PROCEDURE [dbo].[HC_DeletePendingScheduleList]
 @PatientName VARCHAR(100),                
 @EmployeeID BIGINT=0,
 @StartDate DATE=null,                
 @EndDate DATE=null,          
 @IsDeleted BIGINT = -1,          
 @SortExpression VARCHAR(100),                  
 @SortType VARCHAR(10),                
 @FromIndex INT,                
 @PageSize INT,   
 @ListOfIdsInCsv varchar(300),                
 @IsShowList bit,                
 @loggedInID BIGINT                
AS                
BEGIN                
                
 IF(LEN(@ListOfIdsInCsv)>0)                
 BEGIN                  
   UPDATE PendingSchedules SET IsDeleted= CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as bigint) ,UpdatedDate=GETUTCDATE() WHERE PendingScheduleID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv))                     
  END                
                
 IF(@IsShowList=1)                
 BEGIN                
  EXEC HC_GetPendingScheduleList @PatientName, @EmployeeID, @StartDate, @EndDate,@IsDeleted, @SortExpression, @SortType , @FromIndex , @PageSize 
  
 END                
END
