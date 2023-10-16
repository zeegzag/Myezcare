-- EXEC HC_DeleteEmployeeDayOff @SortExpression = 'EmployeeDayOffID', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50', @IsDeleted = '-1', @ListOfIdsInCsv = '8', @IsShowList = 'True', @loggedInID = '1'    
CREATE PROCEDURE [dbo].[HC_DeleteEmployeeDayOff]    
 @EmployeeID BIGINT = 0,       
 @StartTime DATE = NULL,              
 @EndTime DATE = NULL,      
 @SubmittedBy VARCHAR(100) = NULL,                
 @SubmittedDate DATE = NULL,       
 @IsDeleted int=-1,          
 @DayOffStatusInProgress VARCHAR(50),    
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
   UPDATE EmployeeDayOffs SET IsDeleted=Case WHEN IsDeleted=0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as BIGINT) ,UpdatedDate=GETUTCDATE() WHERE EmployeeDayOffID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv))  --AND     DayOffStatus =@DayOffStatusInProgress    
  --END      
 END      
      
 IF(@IsShowList=1)      
 BEGIN      
  EXEC HC_GetEmployeeDayOffList @EmployeeID, @StartTime, @EndTime,@SubmittedBy,@SubmittedDate,@IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize      
 END      
END 
