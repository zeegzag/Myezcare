CREATE PROCEDURE [dbo].[CloneTask]              
                           
 @VisitTaskType varchar(100)=NULL,                 
 @VisitTaskDetail varchar(100)=null,                  
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
 @loggedInID BIGINT,
 @TargetCareType nvarchar(max) = null,
 @TargetServiceCode nvarchar(max) = null
   
AS              
BEGIN                  
               
 SET @ServiceCode = CAST(@TargetServiceCode AS INT)
 SET @VisitTaskCareTypeID = CAST(@TargetCareType AS INT)

 IF(LEN(@ListOfIdsInCsv)>0)              
	BEGIN             
		INSERT INTO VisitTasks		
		SELECT VisitTaskType,VisitTaskDetail,IsDeleted,GETDATE(),CreatedBy,GETDATE(),UpdatedBy,SystemID,@ServiceCode,IsRequired,MinimumTimeRequired,IsDefault,
			VisitTaskCategoryID,VisitTaskSubCategoryID,SendAlert,VisitType,@VisitTaskCareTypeID,Frequency FROM VisitTasks WHERE VisitTaskID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv))  
	END

	SELECT * FROM VisitTasks WHERE VisitTaskID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv)) 

 --IF(@IsShowList=1)                
	--BEGIN                
	--	EXEC GetVisitTaskList @VisitTaskType,@VisitTaskDetail,@ServiceCode,@VisitTaskCategoryID,@VisitTaskVisitTypeID,@VisitTaskCareTypeID,@IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize                
	--END  
END
