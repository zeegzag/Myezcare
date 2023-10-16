CREATE PROCEDURE [dbo].[DeleteEmployeeVisit]  
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
 @loggedInID BIGINT                
AS                
BEGIN                
                
 IF(LEN(@ListOfIdsInCsv)>0)                
 BEGIN                  
   UPDATE EmployeeVisits SET IsDeleted= CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as bigint) ,UpdatedDate=GETUTCDATE() WHERE EmployeeVisitID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv))                 
  
    
  END                
                
 IF(@IsShowList=1)                
 BEGIN                
  EXEC GetEmployeeVisitList @EmployeeVisitID,@EmployeeIDs,@ReferralIDs,@StartDate,@EndDate,@StartTime,@EndTime,@IsDeleted,@ActionTaken,@SortExpression, @SortType, @FromIndex, @PageSize                
 END                
END
