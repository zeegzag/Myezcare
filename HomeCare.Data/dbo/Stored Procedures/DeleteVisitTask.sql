CREATE PROCEDURE [dbo].[DeleteVisitTask]          
 @VisitTaskType varchar(100)=NULL,                   
 @VisitTaskDetail varchar(100)=null,          
 --@ServiceCodeID BIGINT = 0,            
 @ServiceCode VARCHAR(100) = null,        
 @VisitTaskCategoryID BIGINT=0,    
 @VisitTaskVisitTypeID BIGINT=0,                     
 @VisitTaskCareTypeID BIGINT=0,                              
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
   UPDATE VisitTasks SET IsDeleted= CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as bigint) ,UpdatedDate=GETUTCDATE() WHERE VisitTaskID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv))                       
    EXEC UpdateReferralTaskMappings @ListOfIdsInCsv, @loggedInID, '::1' --TODO
  END                  
                  
 IF(@IsShowList=1)                  
 BEGIN                  
  EXEC GetVisitTaskList @VisitTaskType,@VisitTaskDetail,@ServiceCode,@VisitTaskCategoryID,@VisitTaskVisitTypeID,@VisitTaskCareTypeID,@IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize                  
 END                  
END